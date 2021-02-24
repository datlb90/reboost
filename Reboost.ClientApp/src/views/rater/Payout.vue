<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15">
        <el-main>
          <div class="con">
            <div class="con-user">
              <div class="con-user-profile">
                <div class="con-user-profile__photo" />
                <p>Rater until {{ dateFormat(currentUser.expireDate) }}</p>
                <h1>{{ currentUser.username }}</h1>
                <h2>
                  <a :href="loginUrl">View Stripe account</a>
                </h2>
              </div>
              <div class="con-user-available block">
                <p>Your balance</p>
                <h1>${{ parseFloat(balance.available[0].amount/10).toFixed(2) }}</h1>
                <h2> ${{ parseFloat(balance.available[0].amount/10).toFixed(2) }} available</h2>
              </div>
              <div class="con-user-payout block">
                <button class="btn-payout" @click="payout">Pay out now</button>
                <button class="con-history__reviews__button btn-payout" @click="getPayment">Add Test Review</button>
                <!-- <p>
                  <a href="">View payouts on Stripe</a>
                </p> -->
              </div>
            </div>
            <div class="con-history">
              <div class="con-history__reviews">
                <div class="con-history__reviews__label">
                  Recent reviews
                </div>

              </div>
              <el-table
                :data="listTransfer"
                style="width: 100%"
              >
                <el-table-column
                  prop="name"
                  label="Name"
                  width="300"
                />
                <el-table-column
                  prop="paymentDate"
                  label="Created Date "
                  width="300"
                />
                <el-table-column
                  prop="amount"
                  label="Amount"
                />
              </el-table>
            </div>
          </div>
        </el-main>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import paymentService from '../../services/payment.service'
import accountService from '../../services/account.service'
import moment from 'moment'
export default {
  name: 'StartRating',
  data() {
    return {
      disable: false,
      text: '',
      accountId: null,
      destination: null,
      loginUrl: '#'
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    listTransfer() {
      var listTransfer = this.$store.getters['payment/getAllTransferHistories']
      for (const i in listTransfer) {
        listTransfer[i].name = 'Review ' + (+i + 1).toString()
        listTransfer[i].paymentDate = moment(listTransfer[i].paymentDate).format('yyyy-MM-DD')
      }
      return listTransfer
    },
    balance() {
      return this.$store.getters['payment/getBalance']
    }
  },
  mounted() {
    // console.log('Test', this.currentUser)
    this.onLoad()
    this.$store.dispatch('payment/loadPaymentHistories')
  },
  methods: {
    onLoad() {
      if (this.$route.params.accountId) {
        this.accountId = this.$route.params.accountId
        this.$store.dispatch('payment/loadBalance', this.accountId)
        this.$store.dispatch('payment/loadPaymentHistories', this.currentUser.id)
        paymentService.loginLink(this.accountId).then(rs => {
          this.loginUrl = rs.data.url
        })
      }
      paymentService.getStripeAccount(this.currentUser.id).then(rs => {
        if (!rs) {
          console.log('KHONG TON TAI ACCOUNT')
          this.$router.push({ path: '/rater/startRating' })
          return
        }
        accountService.isCompleteOnboard(this.currentUser.id).then(result => {
          console.log('IS USER COMLETED ONBOARD...', result)
          if (!result) {
            this.$router.push({ path: '/rater/startRating' })
            return
          }
        })
        this.accountId = rs.id
        this.$store.dispatch('payment/loadBalance', this.accountId)
        this.$store.dispatch('payment/loadPaymentHistories', this.currentUser.id)
        paymentService.loginLink(this.accountId).then(rs => {
          this.loginUrl = rs.data.url
        })
      })
    },
    dateFormat(time) {
      return moment(new Date(time)).format('yyyy-MM-DD')
    },
    getPayment() {
      paymentService.transferMoney({
        userId: this.currentUser.id,
        paymentId: 'this_paymentId',
        type: 'OUT',
        destination: this.accountId,
        description: 'No Description',
        amount: 50,
        currency: 'usd',
        status: 'success',
        paymentDate: new Date(),
        itemId: ''
      }).then(rs => {
        // console.log('transfer', rs)
        this.$store.dispatch('payment/loadPaymentHistories', this.currentUser.id)
        this.$store.dispatch('payment/loadBalance', this.accountId)
      })
    },
    payout() {
      paymentService.payout({ amount: this.balance.available[0].amount, destination: this.accountId }).then(rs => {
        // console.log('payout', rs)
        this.$store.dispatch('payment/loadBalance', this.accountId)
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

.el-main{

}

.con-user {
    display: flex;
    width: 100%;
    padding: 40px 0;
}

.con-user .con-user-profile {
    flex: 1;
    padding-left: 65px;
    position: relative;
    color: #292f45;
}

.con-user .con-user-profile .con-user-profile__photo {
    position: absolute;
    width: 50px;
    height: 50px;
    top: 36%;
    left: 0;
    margin-top: -25px;
    border-radius: 50%;
    background-image: url(../../assets/img/avatar-large.svg);
    box-shadow: 0 2px 5px -1px rgb(50 50 93 / 25%), 0 1px 3px -1px rgb(0 0 0 / 30%);
}
.con-user h1 {
    font-size: 24px;
    font-weight: 400;
}
.con-user p {
    font-size: 13px;
    font-weight: 500;
    margin-bottom: 7px;
    color: #292f45;
}
.con-user h2 {
    color: #8898aa;
    font-size: 13px;
    font-weight: 500;
    margin-top: 8px;
}
.con-user a:hover {
    text-decoration: underline;
}
.block{
    margin-left: 40px;
    position: relative;
    color: #292f45;
}
.btn-payout{
    font-size: 16px;
    font-weight: 500;
    text-transform: unset;
    letter-spacing: 0;
    height: 43px;
    margin: 30px 0 0 0;
    margin: 2px 0 14px;
    min-width: 150px;
    text-align: center;
    background: #15b67c;
    color: white;
    padding: 0 22px;
    line-height: 44px;
    display: block;
    width: 100%;
    border-style: none;
    border-radius: 4px;
    box-shadow: 0 2px 5px -1px rgb(50 50 93 / 25%), 0 1px 3px -1px rgb(0 0 0 / 30%);
    cursor: pointer;
    outline: none;
}
.con-history__reviews{
    display: flex;
    margin-bottom: 14px;
    color: #292f45;
    font-weight: 500;
}
.con-history__reviews__label{
    flex:1
}
.con-history__reviews__button{
    flex:0.2
}
</style>
