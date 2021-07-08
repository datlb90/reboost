/* eslint-disable vue/attribute-hyphenation */
<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15">
        <el-main>
          <el-dialog v-if="defaultPaymentMethod" title="Outer Dialog" :visible.sync="outerVisible" width="40%">
            <div slot="title">
              <div class="container-title">
                Card on File
              </div>
            </div>
            <div>
              <div class="dialog-body__child">
                <span style="font-weight: bold;">Type: </span> {{ defaultPaymentMethod.card.brand }}
              </div>
              <div class="dialog-body__child">
                <span style="font-weight: bold;">Last 4 Digits: </span>  {{ defaultPaymentMethod.card.last4 }}
              </div>
              <div class="dialog-body__child">
                <span style="font-size: small;">For security, your card information is stored on <a href="#" style="color: blue">stripe.com</a></span>
              </div>
            </div>
            <div slot="footer" class="dialog-footer">
              <el-button @click="outerVisible = false">Close</el-button>
            </div>
          </el-dialog>
          <el-dialog
            title="Outer Dialog"
            :visible.sync="updateVisible"
            width="40%"
            @closed="dialogClosed"
          >
            <div slot="title">
              <div class="container-title">
                Current Card
              </div>
            </div>
            <div>

              <div v-if="defaultPaymentMethod" class="dialog-body__child">
                Current card on file is a <span style="font-weight: bold;">{{ defaultPaymentMethod.card.brand }}</span> ending is the digits <span style="font-weight: bold;">{{ defaultPaymentMethod.card.last4 }}</span>.
              </div>
              <div class="checkout">
                <form id="payment-form">
                  <div id="card-element" />
                  <p id="card-error" role="alert" style="color: #fa755a" />
                </form>
              </div>
            </div>
            <div slot="footer">
              <el-button
                v-loading="loading"
                element-loading-spinner="el-icon-loading"
                element-loading-custom-class="spiner-updating"
                type="success"
                :disabled="checkoutDisable"
                @click="updateCreditCard"
              >Update Credit Card</el-button>
            </div>
          </el-dialog>
          <div style="margin-bottom: 15px; display: none">
            <div class="alert-title alert-title__error">
              Your subcription has been
              <span style="font-weight: bold; padding-left: 5px">
                Canceled
              </span>
            </div>
          </div>
          <div class="container">
            <div class="container-title">
              Account Information
            </div>
            <div class="container-body__flex">
              <div class="child">Credit Card on file. </div>
              <div v-if="defaultPaymentMethod" class="child"><input type="button" class="btn__show-card" value="Show Card" @click="outerVisible = true"></div>
              <div class="child"><input type="button" class="btn__update-card" value="Update Credit Card" @click="dialogOpened()"></div>
            </div>
          </div>
          <div class="container">
            <div class="container-title">
              Subscrition
            </div>
            <div class="container-body__flex">
              <div class="child">You can <a href="../subscribe" style="color: #0d91f7; padding-left: 5px">subscribe</a> </div>
            </div>
            <el-table
              :data="listSubscription"
              style="width: 100%"
            >
              <el-table-column
                prop="plan"
                label="Plan"
                width="100"
              />
              <el-table-column
                prop="interval"
                label="Interval "
                width="100"
              />
              <el-table-column
                prop="description"
                label="Description"
              />
              <el-table-column
                prop="status"
                label=" "
                width="180"
              />
            </el-table>
          </div>
          <div class="container">
            <div class="container-title">
              Payment History
            </div>
            <div class="container-body__flex">
              <el-table
                :data="listTransfer"
                style="width: 100%"
              >
                <el-table-column
                  prop="description"
                  label="Description"
                  width="300"
                />
                <el-table-column
                  prop="amount"
                  label="Amount"
                  width="180"
                />
                <el-table-column
                  prop="paymentDate"
                  label="Payment Date"
                />
                <el-table-column
                  prop="status"
                  label="Status"
                  width="120"
                />
              </el-table>
            </div>
            <div style="width: 100%; margin-top: 15px; display: flex; justify-content: center;">
              <el-pagination
                background
                layout="total, sizes, prev, pager, next, jumper"
                :page-size="pageSize"
                :total="totalRow"
                @size-change="handleSizeChange"
                @current-change="handleCurrentChange"
              />
            </div>

            <!-- <div style="width: 100%; margin-top: 15px;">
              <ul class="page-list">
                <li class="page-list__number">1</li>
                <li class="page-list__number">2</li>
              </ul>
            </div> -->
          </div>
        </el-main>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import paymentService from '../../services/payment.service'
// import accountService from '../../services/account.service'
import moment from 'moment'
import { configs } from '../../app.constant'
import { loadStripe } from '@stripe/stripe-js'
export default {
  name: 'PaymentInfo',
  data() {
    return {
      disable: false,
      text: '',
      accountId: null,
      destination: null,
      loginUrl: '#',
      listSubscription: [],
      subscribedPlans: null,
      updateVisible: false,
      outerVisible: false,
      checkoutDisable: false,
      questions: [],
      totalRow: 0,
      page: 1,
      pageSize: 10,
      selectedMethod: null,
      loading: false,
      isCardValid: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    currentProducts() {
      return this.$store.getters['payment/getAllProducts']
    },
    currentPrices() {
      return this.$store.getters['payment/getAllPrices']
    },
    listTransfer() {
      var listTransfer = this.$store.getters['payment/getAllTransferHistories']
      for (const i in listTransfer) {
        listTransfer[i].amount = '$' + listTransfer[i].amount
        listTransfer[i].name = 'Review ' + (+i + 1).toString()
        listTransfer[i].paymentDate = moment(listTransfer[i].paymentDate).format('MMM Do YYYY   h:mm a')
      }
      return listTransfer
    },
    defaultPaymentMethod() {
      return this.$store.getters['payment/getDefaultPaymentMethod']
    }
  },
  async mounted() {
    this.$store.dispatch('payment/loadProducts')
    this.$store.dispatch('payment/loadPrices')
    this.$store.dispatch('payment/loadPaymentHistories', this.currentUser.id)
    this.$store.dispatch('payment/loadDefaultPaymentMethod', this.currentUser.stripeCustomerId)
    this.loadTable()
    paymentService.getCustomerSubscriptions(this.currentUser.stripeCustomerId).then(rs => {
      this.subscribedPlans = rs.data.map(s => s.items.data[0].plan)
      for (const i of this.currentPrices) {
        this.listSubscription.push({
          plan: this.currentProducts.find(r => r.id == i.product)?.name,
          interval: i.recurring.interval,
          description: this.currentProducts.find(r => r.id == i.product)?.description,
          status: this.subscribedPlans.find(r => r.id == i.id)?.active ? 'Subscribed' : 'Not your plan'
        })
      }
    })
  },
  methods: {
    loadTable() {
      this.questions = this.listTransfer.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.totalRow = this.listTransfer.length
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadTable()
    },
    handleSizeChange(val) {
      this.pageSize = val
      this.loadTable()
    },
    async dialogOpened() {
      this.updateVisible = true
      this.stripe = await loadStripe(configs.stripeApiKey)
      var elements = this.stripe.elements()
      var style = {
        base: {
          color: '#32325d',
          fontFamily: 'Arial, sans-serif',
          fontSmoothing: 'antialiased',
          fontSize: '16px',
          '::placeholder': {
            color: '#32325d'
          }
        },
        invalid: {
          fontFamily: 'Arial, sans-serif',
          color: '#fa755a',
          iconColor: '#fa755a'
        }
      }
      this.card = elements.create('card', { style: style })
      this.card.mount('#card-element')
      this.card.on('change', (event) => {
        this.isCardValid = !event.error
        document.querySelector('#card-error').textContent = event.error ? event.error.message : ''
      })
    },
    formatPrice(id) {
      var value = this.currentPrices.filter(r => r.product == id)[0].unit_amount / 100
      const val = (value / 1).toFixed(2).replace('.', ',')
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.')
    },
    formatInterval(id) {
      var value = this.currentPrices.filter(r => r.product == id)[0].recurring.interval.toLowerCase()
      return value == 'year' ? 'yr' : 'mo'
    },
    planSelected(id) {
      this.priceId = this.currentPrices.filter(r => r.product == id)[0].id
      this.checkoutVisible = true
    },
    dialogClosed() {
      this.checkoutDisable = false
    },
    updateCreditCard() {
      if (!this.isCardValid) {
        return
      }

      this.loading = true

      setTimeout(() => {
        document.getElementsByClassName('el-loading-spinner')[0].setAttribute('style', 'margin-top: -10px;')
      }, 20)
      this.stripe.createPaymentMethod({
        type: 'card',
        card: this.card,
        billing_details: {
          name: this.currentUser.username
        }
      })
        .then(rs => {
          if (rs) {
            paymentService.updateDefaultPaymentMethod(this.currentUser.stripeCustomerId, rs.paymentMethod.id).then(result => {
              if (result) {
                this.$store.dispatch('payment/loadDefaultPaymentMethod', this.currentUser.stripeCustomerId)
                this.$notify({
                  title: 'Update Sucess',
                  message: 'Updated successfully',
                  type: 'success',
                  duration: 3000
                })
                this.updateVisible = false
                this.loading = false
              } else {
                this.$notify({
                  title: 'Error',
                  message: 'Error occured!',
                  type: 'error',
                  duration: 3000
                })
              }
              this.loading = false
            })
          }
          this.selectedMethod = rs.paymentMethod
          this.lastName = this.selectedMethod.billing_details.name
          this.credit = '**** **** **** ' + this.selectedMethod.card.last4
          this.activeStep = this.activeStep == 1 ? 2 : 1
        })
    }
  }

}
</script>
<style scoped>

.row-flex {
  display: flex;
  justify-content: center;
  margin: 25px 0;
}

.alert-title{
    width: 100%;
    padding: 15px 16px;
    margin: 0;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    border-radius: 4px;
    position: relative;
    background-color: #FFF;
    overflow: hidden;
    opacity: 1;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-align: center;
    -ms-flex-align: center;
    align-items: center;
    -webkit-transition: opacity .2s;
    transition: opacity .2s;
}

.alert-title__error{
  background-color: #fef0f0;
  color: #F56C6C;
}

.container{
  margin-bottom: 50px;
}

.container-title{
  font-size: 25px;
  font-weight: 500;
  margin-bottom: 10px;
}
.container-body__flex{
  display: flex;
}
.child{
  display: flex;
  justify-content: center;
  align-items: center;
  margin-right: 5px;
}
.child input{
  border-radius: 3px;
  outline: none;
  border: none;
  padding: 7px 15px;
}

.spiner-updating{
  margin-top: 10px;
}
.el-loading-mask{
  margin-top: 10px !important;
}
.btn__show-card{
  color: white;
  background-color: rgb(65, 190, 221);
}

.btn__show-card:hover{
  background-color: rgb(57, 179, 206);
}

.btn__show-card:active{
  background-color: rgb(35, 117, 136);
}

.btn__update-card{
  color: white;
  background-color: rgb(37, 123, 221);
}
.btn__update-card:hover{
  background-color: rgb(75, 147, 230);
}
.btn__update-card:active{
  background-color: rgb(47, 112, 187);
}
.el-dialog{
  border-radius: 12px !important
}

.dialog-body__child{
  padding: 8px 0;
  font-size: 15px;
}

#card-element {
    border-radius: 4px;
    padding: 5px;
    border: 1px solid #DCDFE6;
    /* height: 44px; */
    width: 100%;
    background: white;
}

.page-list{
  display: flex; justify-content: center; list-style-type: none;
}

.page-list__number{
  margin: 0 5px;
  border: 1px solid black;
  border-radius: 3px;
  background-color: blue;
  color: white; width: 20px;
  height: 25px;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
