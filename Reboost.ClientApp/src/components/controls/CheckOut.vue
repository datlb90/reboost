<template>
  <el-dialog
    id="practiceWritingCheckoutContainer"
    :visible.sync="dlVisible"
    width="600px"
    height="820px"
    :before-close="handleClose"
    @opened="dialogOpened"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 5px 20px; font-size: 18px; text-align:center;">Get Writing Feedback & Score</div>
      <!-- <div class="divider--horizontal divider" /> -->
    </div>
    <div class="dialog-body">
      <div style="padding: 20px; padding-top: 5px;">
        <div v-if="selectedReview == ''" class="tip">
          <span style="font-size: 15px; color: #6084a4;">Select the right review service for you:</span>
        </div>
        <el-page-header v-if="selectedReview == 'pro' || selectedReview == 'ai'" class="tip" content="Select a payment method for your service" @back="goBack" />
        <el-page-header v-if="selectedReview == 'peer'" class="tip" content="Confirm your selection" @back="goBack" />
        <div v-if="selectedReview == '' || selectedReview == 'pro'" id="pro-review-card" class="box-card review-card" @click="onReviewSelect('pro')">
          <div>
            <div class="review-icon-wrapper" style="width: 62px; margin-left: 13px;"> <i class="fas fa-user-graduate review-icon" /></div>
            <div class="review-description-wrapper">
              <div class="review-title">Pro Review Service</div>
              <div class="review-description">
                Our certified rater will score your essay and provide detailed feedback within 24 hours (<a href="#">sample feedback</a>)
              </div>
            </div>
            <div class="review-price">$15</div>
          </div>
        </div>

        <div v-if="selectedReview == '' || selectedReview == 'ai'" id="ai-review-card" class="box-card review-card" @click="onReviewSelect('ai')">
          <div>
            <div class="review-icon-wrapper"><i class="fas fa-robot review-icon" /></div>
            <div class="review-description-wrapper">
              <div class="review-title">Instant Feedback from AI Rater</div>
              <div class="review-description">
                Get insighful feedback and score from our powerful writing review AI within seconds (<a href="#">sample feedback</a>)
              </div>
            </div>
            <div class="review-price">$2</div>
          </div>
        </div>

        <div v-if="selectedReview == '' || selectedReview == 'peer'" id="peer-review-card" class="box-card review-card" @click="onReviewSelect('peer')">
          <div>
            <div class="review-icon-wrapper"> <i class="fas fa-user-friends review-icon" /></div>
            <div class="review-description-wrapper">
              <div class="review-title">Free Peer Review</div>
              <div class="review-description">
                Get constructive feedback from another learner with similar or higher writing level (<a href="#">sample feedback</a>)
              </div>
            </div>
            <div class="review-price">Free</div>
          </div>
        </div>
      </div>

      <div v-if="selectedReview == 'peer'">
        <!-- <div class="tip" style="margin: 20px;">
          <span style="font-size: 15px; color: #6084a4;">We will notify you when the feedback is available</span>
        </div> -->
        <div style="padding-top: 10px; text-align: center;">
          <!-- <span style="font-weight: 600;"> Select Payment Method</span> -->
          <el-button type="primary" @click="requestPeerReview()">Confirm Selection</el-button>
        </div>
      </div>

      <!-- <div class="co-header">
        <img style="width: 50px; height: 40px; margin-bottom: 2px; margin-left: 18px;" src="@/assets/img/checkout-icon-premium.png" alt="">
        <div class="title-container">
          <span>Writing Review Service</span>
          <span class="title-price">${{ total }}.00</span>
        </div>
      </div>

      <div style="padding: 2px 20px;">
        <span style="font-size: 12px; color: grey;">A certified rater will review your writing and provide feedback within 24 hours</span>
      </div> -->
      <div v-show="selectedReview == 'pro' || selectedReview == 'ai'" class="co-form" :class="{ active: activeStep == 1 }">
        <div class="co-form__section" :class="{ inactive: activeStep == 2 }">
          <div class="co-form__title" style="padding-top: 10px">
            <!-- <span style="font-weight: 600;"> Select Payment Method</span> -->
            <el-button v-if="activeStep == 2" style="position: absolute;right: 1rem;" size="mini" type="primary" @click="editCheckOutInfo">Edit</el-button>
          </div>
          <div v-if="!paid" id="paypal-button-container" />
        </div>
        <div v-if="activeStep == 1" class="saperator" />
      </div>

    </div>
    <div slot="footer" class="dialog-footer" />
  </el-dialog>

</template>

<!-- <script src="https://www.paypal.com/sdk/js?client-id=Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I"></script> -->

<script>
// const PayPalButton = window.paypal.Buttons.driver('vue', window.Vue)
// @ is an alias to /src
// import http from '@/utils/axios'
// import { loadStripe } from '@stripe/stripe-js'
import { loadScript } from '@paypal/paypal-js'
import paymentService from '../../services/payment.service'
import { REVIEW_REQUEST_STATUS, SUBSCRIPTION_PLANS } from '../../app.constant'
import reviewService from '../../services/review.service'
import moment from 'moment-timezone'
// import { openPayment } from '../../assets/epay/js/paymentClient'
// import { configs } from '../../app.constant'
export default {
  name: 'Checkout',
  // components: {
  //   'paypal-buttons': PayPalButton
  // },
  props: {
    visible: { type: Boolean, default: true },
    subcribe: { type: Object, default: null },
    submissionId: { type: Number, default: null },
    questionId: { type: Number, default: null },
    unratedCount: { type: Number, default: 0 }
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
      amount: 15.00,
      paymentIntent: null,
      total: null,
      epaySelected: false,
      paid: false,
      selectedReview: '',
      loadingAutomatedReview: false
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
  async mounted() {
    this.email = this.currentUser.email
  },
  async created() {
  },
  methods: {
    goBack() {
      this.selectedReview = ''
    },
    onReviewSelect(reviewType) {
      if (reviewType === 'pro') {
        this.amount = 15.00
        this.selectedReview = reviewType
      } else if (reviewType === 'ai') {
        this.amount = 2.00
        this.selectedReview = reviewType
      } else {
        if (this.unratedCount > 0) {
          this.$notify.error({
            title: 'Unrated Reviews',
            message: 'Please rate all of your unrated reviews before requesting a free peer review',
            type: 'error',
            duration: 2000
          })
        } else {
          this.amount = 0.00
          this.selectedReview = reviewType
        }
      }
    },
    createOrder: function(data, actions) {
      console.log('Creating order...')
      return actions.order.create({
        purchase_units: [
          {
            amount: {
              value: this.amount
            }
          }
        ]
      })
    },
    onApprove: function(data, actions) {
      console.log('Order approved...')
      return actions.order.capture().then(order => {
        console.log(order)
        const purchaseUnits = order.purchase_units
        if (purchaseUnits.length > 0) {
          const captures = purchaseUnits[0].payments.captures
          if (captures.length > 0) {
            this.paid = true
            this.createPaymentHistory(captures[0].id)
            this.paymentSuccessed()
          }
        }
      })
    },
    async dialogOpened() {
      if (this.subcribe) {
        this.amount = this.subcribe.price
      }
      this.total = this.amount

      loadScript({ 'client-id': 'Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I' }).then((paypal) => {
        paypal
          .Buttons({
            createOrder: this.createOrder,
            onApprove: this.onApprove
          })
          .render('#paypal-button-container')
      })
    },
    handleClose(done) {
      this.$confirm('Are you sure to exit?')
        .then(_ => {
          done()
          const paypalContaner = document.getElementById('paypal-button-container')
          paypalContaner.innerHTML = ''
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
      console.log(window.paypal)
      if (document.getElementById('paypal-button') && !document.getElementById('paypal-button').childNodes.length > 0) {
        if (!this.subcribe) {
          window.paypal.Button.render({
            // Configure environment
            env: 'sandbox',
            client: {
              sandbox: 'Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I',
              production: 'demo_production_client_id'
            },
            // Customize button (optional)
            locale: 'en_US',
            style: {
              shape: 'pill',
              color: 'gold',
              layout: 'horizontal',
              label: 'paypal',
              tagline: false
            },

            // Enable Pay Now checkout flow (optional)
            commit: true,

            // Set up a payment
            payment: function(data, actions) {
              return actions.payment.create({
                transactions: [{
                  amount: {
                    total: '90.50',
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
      if (SUBSCRIPTION_PLANS.filter(r => r.name === this.subcribe?.name).length > 0) {
        const btnExisted = document.getElementById('paypal-subscribe-button-container')

        if (btnExisted.childNodes.length > 0) {
          btnExisted.childNodes.forEach(e => {
            e.remove()
          })
        }

        setTimeout(() => {
          if (this.subcribe.id === 1) {
            this.createYearSubcribePaypalBtn()
          } else {
            this.createMonthSubcribePaypalBtn()
          }
        }, 50)
      }
    },
    createYearSubcribePaypalBtn() {
      window.paypal_sdk.Buttons({
        style: {
          shape: 'pill',
          color: 'gold',
          layout: 'horizontal',
          label: 'subscribe'
        },

        createSubscription: function(data, actions) {
          return actions.subscription.create({
            // Pass a const to plan_id, can not use variable
            'plan_id': 'P-6N9586386D9350704MFFTCIQ'
          })
        },

        onApprove: (data, actions) => {
          this.subscribed(data)
        }
      }).render('#paypal-subscribe-button-container')
    },
    createMonthSubcribePaypalBtn() {
      window.paypal_sdk.Buttons({
        style: {
          shape: 'pill',
          color: 'gold',
          layout: 'horizontal',
          label: 'subscribe'
        },

        createSubscription: function(data, actions) {
          return actions.subscription.create({
            // Pass a const to plan_id, can not use variable
            'plan_id': 'P-2XC42867D76575918MFFP53Q'
          })
        },

        onApprove: (data, actions) => {
          this.subscribed(data)
        }
      }).render('#paypal-subscribe-button-container')
    },
    subscribed(data) {
      if (this.subcribe.name === 'month') {
        const postData = {
          MonthSubs: data.subscriptionID
        }
        paymentService.createUpdateLearnerSubscriptions(postData).then(rs => {
          this.$notify.success({
            title: 'Subcribe Successed!',
            message: 'Successed!',
            type: 'success',
            duration: 1500
          })
        })
      }
      if (this.subcribe.name === 'year') {
        const postData = {
          YearSubs: data.subscriptionID
        }
        paymentService.createUpdateLearnerSubscriptions(postData).then(rs => {
          this.$notify.success({
            title: 'Subcribe Successed!',
            message: 'Successed!',
            type: 'success',
            duration: 1500
          })
        })
      }
    },
    paymentSuccessed() {
      if (this.selectedReview == 'pro') {
        this.requestProReview()
      } else if (this.selectedReview == 'ai') {
        this.requestAutomatedReview()
      }
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
          title: 'Pro Review Request',
          message: 'The pro review request has been successfully submitted!',
          type: 'success',
          duration: 1500
        })
        this.$emit('reviewRequested')
      })
    },
    requestPeerReview() {
      reviewService.createReviewRequest({
        UserId: this.currentUser.id,
        SubmissionId: +this.submissionId,
        FeedbackType: 'Free',
        Status: REVIEW_REQUEST_STATUS.IN_PROGRESS
      }).then(rs => {
        this.dialogClosed()
        this.$notify.success({
          title: 'Peer Review Request',
          message: 'The peer review request has been successfully submitted!',
          type: 'success',
          duration: 1500
        })
        this.$emit('reviewRequested')
      })
    },
    requestAutomatedReview() {
      const loading = this.$loading({
        lock: true,
        text: 'Please wait while our AI rater scores your essay',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.7)'
      })
      reviewService.createAutomatedReview({
        UserId: this.currentUser.id,
        SubmissionId: +this.submissionId
      }).then(rs => {
        this.$notify.success({
          title: 'Automated AI Review',
          message: 'Essay scoring has been completed. Please review the feedback.',
          type: 'success',
          duration: 2000
        })
        loading.close()
        const url = `/review/${this.questionId}/${rs.docId}/${rs.reviewId}`
        this.$router.push(url)
      })
    },
    createPaymentHistory(transactionId) {
      const data = {
        UserId: this.currentUser.id,
        QuestionId: this.questionId,
        SubmissionId: this.submissionId,
        PaypalTransactionId: transactionId,
        Amount: this.amount
      }
      paymentService.createPaymentHistory(data).then(rs => {
        console.log('payment history created: ', rs)
      })
    }
  }
}
</script>

<style scoped src="@/assets/epay/css/paymentClient.css">
</style>

<style>
.el-page-header__left {
  font-size: 15px;
  color: #6084a4;
}
.el-page-header__content {
  font-size: 15px !important;
  color: #6084a4 !important;
}
</style>
<style scoped>

.review-card {
  height: 120px;
  border: 1px solid #ebeef5;
  border-radius: 4px;
  margin-top: 20px;
  padding: 20px;
  cursor: pointer
}
.review-card:hover {
  cursor: pointer;
  opacity: 0.9;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
}
.review-icon-wrapper {
  float: left;
  width: 70px;
  margin-top: 22px;
  margin-left: 5px;
}
.review-description-wrapper {
  float: left;
  width: calc(100% - 115px);
}
.review-price {
  float: left;
  width: 40px;
  font-size: 16px;
  color: #4a6f8a;
  text-align: center;
  margin-top: 30px;
}
.review-icon {
  font-size: 30px;
  color: #4a6f8a;
}
.review-title {
  font-size: 18px;
    font-weight: 500;
    color: #4a6f8a;
}
.review-description {
  word-break: break-word;
  color: rgb(116, 116, 116);
  margin-top: 5px;
}

#paypal-button-container{
  width: 90%;
  margin: auto;
  overflow: auto;
  max-height: 400px;
  margin-top: 10px;
}
.hidden{
    display: none;
}
.dialog-body{
  overflow: hidden;
}
.co-header{
  width: 100%;
  padding: 5px 0 5px 0;
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
  display: block;
  text-align: center;
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
  font-size: 14px;
}
.title-container{
  margin-right: 10px;
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
  margin-bottom: 20px;
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
