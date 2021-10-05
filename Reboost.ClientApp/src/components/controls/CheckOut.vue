<template>
  <el-dialog
    id="practiceWritingCheckoutContainer"
    :visible.sync="dlVisible"
    width="30%"
    :before-close="handleClose"
    @opened="dialogOpened"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 5px 20px"><i class="el-icon-shopping-cart-1" />Checkout</div>
      <div class="divider--horizontal divider" />
    </div>
    <div class="dialog-body">
      <div class="co-header">
        <img style="width:50px; height:50px; margin: 0 10px;" src="@/assets/img/checkout-icon-premium.png" alt="">
        <div class="title-container">
          <span>Writting preview</span>
          <span class="title-price">${{ total }}</span>
        </div>
      </div>
      <div class="co-form" :class="{ active: activeStep == 1 }">
        <div class="co-form__section" :class="{ inactive: activeStep == 2 }">
          <div class="co-form__title" style="padding-top: 10px">
            <!-- <span class="step-title" :class="{ active: activeStep == 1 }">1</span> -->
            <span style="padding: 0 10px; font-weight: 600;">Payment Method</span>
            <el-button v-if="activeStep == 2" style="position: absolute;right: 1rem;" size="mini" type="primary" @click="editCheckOutInfo">Edit</el-button>
          </div>

          <div class="saperator" style="padding-top: 10px" />

          <div v-if="!subcribe" id="paypal-button" class="paypal-icon__btn" />
          <div style="display:flex; justify-content:center">
            <div v-if="subcribe" id="paypal-subscribe-button-container" style="width:80%; margin:5px" />
          </div>
          <!-- <div class="co-form__content" :class="{ hidden: activeStep == 2 }" style="padding-top: 10px">
            <div class="section-container">
              <label size="mini"> Choose a Payment Method: </label>
              <div class="input-section">
                <el-select v-model="value" size="mini" placeholder="New Payment Method" @change="changeMethod">
                  <div v-if="currentMethods || currentMethods==[]">
                    <el-option
                      v-for="item in currentMethods"
                      :key="item.id"
                      :label="'**** **** **** '+item.card.last4"
                      :value="item.id"
                    />
                  </div>
                  <el-option key="0" label="New Payment Method" value="0">New Payment Method</el-option>
                </el-select>
              </div>
            </div>
            <div :style="{ display: newMethod ? 'block' : 'none' }">
              <div class="section-container">
                <label size="mini"> Please fill in your bill information: </label>
                <div style="display:flex; padding:0 0 10px 0; max-width:60%">
                  <el-input v-model="firstName" style="padding-right:10px;" size="mini" placeholder="First Name" />
                  <el-input v-model="lastName" size="mini" placeholder="Last Name" />
                </div>
                <el-input v-model="email" style="max-width:60%" :disabled="true" size="mini" placeholder="Please input" />
              </div>
              <div v-if="!dlVisible" class="section-container">
                <label size="mini"> Please fill in your credit information: </label>
                <div class="checkout">
                  <form id="payment-form">
                    <div id="card-element" />
                    <p id="card-error" role="alert" />
                  </form>
                </div>
              </div>
            </div>
          </div> -->
          <!-- <div v-if="activeStep == 2" class="co-form__content" style="padding-top: 10px; padding-bottom: 10px">
            <div>{{ email }}</div>
            <div>{{ firstName }} {{ lastName }}</div>
            <div>{{ credit }} </div>
          </div> -->
        </div>
        <div v-if="activeStep == 1" class="saperator" style="padding-bottom: 10px" />
        <!-- <div class="co-form__section" :class="{ active: activeStep == 2, inactive: activeStep ==1 }">
          <div class="co-form__title" style="padding-bottom: 10px; padding-top: 10px">
            <span class="step-title" :class="{ active: activeStep == 2 }">2</span>
            <span style="padding: 0 10px; font-weight: 600;">Review and comfirm</span>
          </div>
          <div class="saperator" />
          <div v-if="activeStep == 2" class="co-form__content" style="padding-top: 10px; padding-bottom: 10px">
            <div style="margin-bottom: 10px">Add a promotion code</div>
            <div><el-input v-model="promotionCode" style="padding-right:10px; width: 200px" size="mini" placeholder="Enter Code" /></div>
          </div>
        </div> -->
      </div>

    </div>
    <div slot="footer" class="dialog-footer">
      <div class="co-form-footer__title">
        <div>
          <span>Item:</span>
          <span class="title-price title-price__footer">${{ total }}</span>
        </div>
        <div>
          <span>Estimate Tax:</span>
          <span class="title-price title-price__footer">$0.00</span>
        </div>
        <div class="divider--horizontal divider" />
        <div class="title-footer__total">
          <span>Order total: </span>
          <span class="title-price title-price__footer">${{ total }}</span>
        </div>
      </div>
      <div class="footer--btn_container">
        <el-button v-if="activeStep == 1" :disabled="checkoutDisable" size="mini" type="primary" class="co-button" @click="dialogClosed">Cancel</el-button>
        <!-- <el-button v-if="activeStep == 1" :disabled="checkoutDisable" size="mini" type="primary" class="co-button" @click="nextStep()">Continue</el-button> -->
        <!-- <el-button v-if="activeStep == 2" size="mini" type="warning" class="co-button checkout" @click="completeCheckout()">Checkout</el-button> -->
      </div>
    </div>
    <!-- <div class="payment__divider">OR</div> -->

  </el-dialog>

</template>

<script>
// @ is an alias to /src
// import http from '@/utils/axios'
// import { loadStripe } from '@stripe/stripe-js'
import paymentService from '../../services/payment.service'
import { REVIEW_REQUEST_STATUS } from '../../app.constant'
import reviewService from '../../services/review.service'
import * as moment from 'moment'
// import { configs } from '../../app.constant'
export default {
  name: 'Checkout',
  props: { visible: { type: Boolean, default: true },
    subcribe: { type: Object, default: null },
    submissionId: { type: Number, default: null },
    questionId: { type: Number, default: null }
  },
  data() {
    return {
      dlVisible: false,
      value: '0',
      firstName: '',
      lastName: '',
      email: null,
      credit: '**** **** **** 1234',
      promotionCode: '',
      activeStep: 1,
      card: null,
      stripe: null,
      clientSecret: null,
      checkoutDisable: true,
      newMethod: false,
      selectedMethod: null,
      amount: 3500,
      paymentIntent: null,
      total: null
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    currentMethods() {
      return this.$store.getters['payment/getAllPaymentMethods']
    },
    currentPrices() {
      return this.$store.getters['payment/getAllPrices']
    }
  },
  watch: {
    visible: function(newVal, oldVal) {
      this.dlVisible = newVal
    },
    currentMethods: function(val) {
      if (!val || val.length == 0) {
        this.changeMethod('0')
      } else {
        console.log('CURRENT MENOTH CHANGED', val[0].id)
        this.changeMethod(val[0].id)
        this.value = val[0].id
      }
    }
  },
  async created() {

  },
  async mounted() {
    this.email = this.currentUser.email
    if (this.currentUser.stripeCustomerId) {
      this.$store.dispatch('payment/loadPaymentMethods', this.currentUser.stripeCustomerId)
    } else {
      this.$store.dispatch('payment/loadPaymentMethods', null)
    }

    const script = document.createElement('script')
    script.src = 'https://www.paypalobjects.com/api/checkout.js'
    document.body.appendChild(script)

    const sub_script = document.createElement('script')
    sub_script.src = 'https://www.paypal.com/sdk/js?client-id=AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F&vault=true&intent=subscription'
    document.body.appendChild(sub_script)
  },
  methods: {
    async dialogOpened() {
      if (this.subcribe) {
        this.amount = this.subcribe.price
      }
      this.total = this.amount

      // paymentService.getIntent(this.amount).then(rs => {
      //   this.paymentIntent = rs
      //   this.clientSecret = rs.client_secret
      //   console.log('this.paymentIntent', this.paymentIntent)
      // })

      // this.stripe = await loadStripe(configs.stripeApiKey)
      // var elements = this.stripe.elements()
      // var style = {
      //   base: {
      //     color: '#32325d',
      //     fontFamily: 'Arial, sans-serif',
      //     fontSmoothing: 'antialiased',
      //     fontSize: '16px',
      //     '::placeholder': {
      //       color: '#32325d'
      //     }
      //   },
      //   invalid: {
      //     fontFamily: 'Arial, sans-serif',
      //     color: '#fa755a',
      //     iconColor: '#fa755a'
      //   }
      // }
      // this.card = elements.create('card', { style: style })
      // this.card.mount('#card-element')
      // this.card.on('change', (event) => {
      // // Disable the Pay button if there are no card details in the Element
      //   this.checkoutDisable = !!event.error
      //   document.querySelector('#card-error').textContent = event.error ? event.error.message : ''
      // })

      this.loadPaypalBtn()
    },
    handleClose(done) {
      this.$confirm('Are you sure to exit?')
        .then(_ => {
          done()
        })
        .catch(_ => {})
    },
    checkoutVisibles(e) {
      if (e == 'checkout') {
        this.checkoutVisible = true
      }
    },
    nextStep() {
      if (this.newMethod) {
        this.stripe.createPaymentMethod({
          type: 'card',
          card: this.card,
          billing_details: {
            name: this.firstName + ' ' + this.lastName
          }
        })
          .then(rs => {
            console.log('create payment method result', rs)
            this.selectedMethod = rs.paymentMethod
            this.lastName = this.selectedMethod.billing_details.name
            this.credit = '**** **** **** ' + this.selectedMethod.card.last4
            this.activeStep = this.activeStep == 1 ? 2 : 1
          })
      } else {
        this.activeStep = this.activeStep == 1 ? 2 : 1
      }
    },
    completeCheckout() {
      // var paymentMethod = null
      if (this.newMethod) {
        // paymentMethod = {
        //   card: this.card,
        //   billing_details: {
        //     name: this.firstName + ' ' + this.lastName
        //   }
        // }
        // this.stripe.createPaymentMethod({
        //   type: 'card',
        //   card: this.card,
        //   billing_details: {
        //     name: this.firstName + ' ' + this.lastName
        //   }
        // })
        //   .then(result => {

        //   })

        // console.log('create payment method result', result)
        paymentService.attachMethod(this.selectedMethod.id).then(rs => {
          // paymentMethod = rs.id
          console.log('attach pm result', rs)
          this.selectedMethod = rs

          if (this.subcribe == null) {
            this.confirmPayment(this.selectedMethod.id)
          } else {
            this.$emit('subscribed', this.selectedMethod)
          }
        })
      } else {
        // paymentMethod = this.selectedMethod.id
        if (this.subcribe == null) {
          this.confirmPayment(this.selectedMethod.id)
          this.clearSection()
        } else {
          this.$emit('subscribed', this.selectedMethod)
          this.clearSection()
        }
      }
    },
    confirmPayment(paymentMethod) {
      this.stripe.confirmCardPayment(this.clientSecret, {
        payment_method: paymentMethod
      })
        .then((result) => {
          if (result.error) {
            console.error(result)
            this.$notify.error({
              title: 'Error',
              message: result.error.message,
              type: 'error',
              duration: 5000
            })
          } else {
            // The payment succeeded!
            paymentService.insertPayment({
              UserId: this.currentUser.id,
              PaymentId: result.paymentIntent.id,
              Description: result.paymentIntent.description ? result.paymentIntent.description : 'No Description',
              Amount: 35,
              Currency: result.paymentIntent.currency,
              Status: result.paymentIntent.status,
              PaymentDate: moment().format('YYYY-MM-DD') })
            this.clearSection()
          }
        })
    },
    clearSection() {
      this.firstName = ''
      this.lastName = ''
      this.credit = '**** **** **** 1234'
      this.promotionCode = ''
      this.activeStep = 1
      this.card = null
      this.stripe = null
      this.clientSecret = null
      this.newMethod = false
      this.selectedMethod = null
    },
    dialogClosed() {
      this.$emit('closed')
    },
    editCheckOutInfo() {
      this.activeStep = this.activeStep == 2 ? 1 : 2
    },
    changeMethod(e) {
      if (+e == 0) {
        this.newMethod = true
        this.checkoutDisable = true
        this.selectedMethod = null
      } else {
        this.newMethod = false
        this.checkoutDisable = false
        this.selectedMethod = this.currentMethods.find(m => m.id == e)
        this.lastName = this.selectedMethod.billing_details.name
        this.credit = '**** **** **** ' + this.selectedMethod.card.last4
      }
    },
    loadPaypalBtn() {
      if (document.getElementById('paypal-button') && !document.getElementById('paypal-button').childNodes.length > 0) {
        if (!this.subcribe) {
          window.paypal.Button.render({
            // Configure environment
            env: 'sandbox',
            client: {
              sandbox: 'AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F',
              production: 'demo_production_client_id'
            },
            // Customize button (optional)
            locale: 'en_US',
            style: {
              size: 'large',
              color: 'gold',
              shape: 'pill',
              label: 'checkout',
              tagline: 'true'
            },

            // Enable Pay Now checkout flow (optional)
            commit: true,

            // Set up a payment
            payment: function(data, actions) {
              return actions.payment.create({
                transactions: [{
                  amount: {
                    total: '0.5',
                    currency: 'USD'
                  }
                }]
              })
            },
            // Execute the payment
            onAuthorize: e => {
              this.createPaymentHistory(e.paymentID)
              this.paymentSuccessed()
            }
          }, '#paypal-button')
        }
      }
      if (this.subcribe?.name === 'month' || this.subcribe?.name === 'year') {
        const btnExisted = document.getElementById('paypal-subscribe-button-container')

        if (btnExisted.childNodes.length > 0) {
          btnExisted.childNodes.forEach(e => {
            e.remove()
          })
        }

        setTimeout(() => {
          this.createSubcribePaypalBtn()
        }, 50)
      }
    },
    createSubcribePaypalBtn() {
      console.log('plan id: ', this.subcribe?.pId)
      window.paypal.Buttons({
        createSubscription: function(data, actions) {
          return actions.subscription.create({
            'plan_id': this.subcribe?.pId
          })
        },

        style: {
          shape: 'pill',
          color: 'gold',
          layout: 'horizontal',
          label: 'subscribe'
        },

        onApprove: function(data, actions) {
          if (this.subcribe.name === 'month') {
            const data = {
              MonthSubs: data.subscriptionID
            }
            paymentService.createUpdateLearnerSubscriptions(data).then(rs => {
              console.log('sub to month: ', rs)
              this.$notify.success({
                title: 'Subcribe Successed!',
                message: 'Successed!',
                type: 'success',
                duration: 1500
              })
            })
          }
          if (this.subcribe.name === 'year') {
            const data = {
              YearSubs: data.subscriptionID
            }
            paymentService.createUpdateLearnerSubscriptions(data).then(rs => {
              console.log('sub to month: ', rs)
              this.$notify.success({
                title: 'Subcribe Successed!',
                message: 'Successed!',
                type: 'success',
                duration: 1500
              })
            })
          }
        }

      }).render('#paypal-subscribe-button-container')
    },
    paymentSuccessed() {
      this.$notify.success({
        title: 'Payment',
        message: 'Payment Successed!',
        type: 'success',
        duration: 1500
      })
      this.requestProReview()
    },
    requestProReview() {
      reviewService.createProReviewRequest({
        UserId: this.currentUser.id,
        SubmissionId: +this.submissionId,
        FeedbackType: 'Pro',
        Status: REVIEW_REQUEST_STATUS.WAITING
      }).then(rs => {
        this.dialogClosed()

        this.$notify.success({
          title: 'Submission Requested',
          message: 'Requested!',
          type: 'success',
          duration: 1500
        })
        this.$emit('proReviewRequested')
      })
    },
    createPaymentHistory(paymentId) {
      const data = {
        UserId: this.currentUser.id,
        QuestionId: this.questionId,
        PaypalInvoiceId: paymentId,
        Amount: '0.5'
      }

      paymentService.createPaymentHistory(data).then(rs => {
        console.log('payment history created: ', rs)
      })
    }
  }
}
</script>
<style scoped>
.hidden{
    display: none;
}
.dialog-body{
  overflow: hidden;
}
.co-header{
  width: 100%;
  padding: 10px 0 10px 0;
  display: flex;
  align-items: center;
  border-bottom: solid 1px rgb(187, 187, 187);
}
.co-form{
    background-color: #f1f1f1;
    display: block;
    margin-left: -5px;
    margin-right: -5px;
}
.co-form.active{
  box-shadow: inset 0px 0px 5px 0px #9a9a9a;
}
.co-form__title{
  display: flex;
  align-items: center;
}
.co-form__content{
  padding: 0px 25px;
}
.co-form__section.active{
  box-shadow: inset 0px 0px 5px 0px #9a9a9a;
}
.co-form__section.inactive{
  background-color: white;
}
.step-title{
    margin: 0 0 0 10px;
    padding: 5px 12px;
    border-radius: 50%;
    font-weight: 600;
    color: #969292;
    border: 1px solid rgb(210 210 210);
}
.step-title.active{
  color: white;
  background-color: rgb(53, 198, 255);
}
.divider--horizontal {
    display: block;
    height: 1px;
    width: 100%;
    background-color: rgb(202 203 205);
}
.saperator{
  height: 1px;
  border-bottom: 1px solid rgb(202 203 205);
}
.divider {
  margin: 10px 0 0 0;
  position: relative;
}
.title-price{
  color: #999;
  font-size: 12px;
}
.title-container{
padding: 0 10px;
width:100%;
display: flex;
justify-content: space-between;
}
.input-section{
  max-width: 65%;
}
input {
  border-radius: 6px;
  margin-bottom: 6px;
  padding: 12px;
  border: 1px solid rgba(50, 50, 93, 0.1);
  height: 44px;
  font-size: 16px;
  width: 100%;
  background: white;
}
#card-error {
  color: rgb(105, 115, 134);
  text-align: left;
  font-size: 13px;
  line-height: 17px;
  margin-top: 12px;
}
#card-element {
    border-radius: 4px;
    padding: 5px;
    border: 1px solid #DCDFE6;
    /* height: 44px; */
    width: 100%;
    background: white;
}
.dialog-footer{
  align-items: center;
}
.co-form-footer__title{
  position: relative;
  text-align: left;
  width: 100%;
}
.title-price__footer{
    position: absolute;
    right: 1px;
}
.title-footer__total{
  color: red ;
  font-size: 16px;
}
.title-footer__total .title-price{
  color: red;
  font-size: 13px;
}
.paypal-icon__btn{
  margin: 15px 0 0 0;
  text-align: center;
}
.payment__divider{
  margin-top: 10px;
  text-align: center;
}
.footer--btn_container{
  display: flex;
  justify-content: flex-end;
  margin-top: 15px;
}
</style>
