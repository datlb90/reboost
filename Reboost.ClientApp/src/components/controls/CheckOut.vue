<template>
  <el-dialog
    id="practiceWritingCheckoutContainer"
    :visible.sync="dlVisible"
    width="800px"
    height="820px"
    :before-close="handleClose"
    @opened="dialogOpened"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 5px 20px; font-size: 18px; text-align:center;">Nhận Phản Hồi Cho Bài Viết Của Bạn</div>
    </div>
    <div class="dialog-body">
      <div style="padding: 20px; padding-top: 20px;">
        <div v-if="selectedReview == ''" class="tip">
          <span style="font-size: 15px; color: #6084a4;">Để nhận phản hồi cho bài viết của mình, hãy chọn 1 trong các dịch vụ dưới đây:</span>
        </div>
        <el-page-header v-if="selectedReview == 'Pro' || selectedReview == 'AI'" class="tip" content="Select a payment method for your service" @back="goBack" />
        <el-page-header v-if="selectedReview == 'Free'" class="tip" content="Confirm your selection" @back="goBack" />
        <div v-if="selectedReview == '' || selectedReview == 'Pro'" id="pro-review-card" class="box-card review-card" @click="onReviewSelect('Pro')">
          <div>
            <div class="review-icon-wrapper" style="width: 62px; margin-left: 13px;"> <i class="fas fa-user-graduate review-icon" /></div>
            <div class="review-description-wrapper">
              <div class="review-title">{{ messageTranslates('checkout', 'proTitle') }}</div>
              <div class="review-description">
                Đội ngũ giáo viên trình độ cao và giàu kinh nghiệm của Reboost sẽ chấm bài viết của bạn và cung cấp phản hồi chi tiết trong vòng 24 giờ. (<a href="#">sample feedback</a>)
                <!-- Our certified rater will score your essay and provide detailed feedback within 24 hours (<a href="#">sample feedback</a>) -->
              </div>
            </div>
            <div class="review-price">200.000 VNĐ</div>
          </div>
        </div>
        <div v-if="selectedReview == '' || selectedReview == 'AI'" id="ai-review-card" class="box-card review-card" @click="onReviewSelect('AI')">
          <div>
            <div class="review-icon-wrapper"><i class="fas fa-robot review-icon" /></div>
            <div class="review-description-wrapper">
              <!-- <div class="review-title">Instant Feedback from AI Rater</div> -->
              <div class="review-title">Phản Hồi Nhanh Từ AI </div>
              <div class="review-description">
                Nhận phản hồi ngay lập tức và chính xác về bài viết của mình từ hệ thống chấm bài tự động được vận hành bởi ChatGPT-4 (<a href="#">sample feedback</a>)
                <!-- Get insighful feedback and score from our powerful writing review AI within seconds (<a href="#">sample feedback</a>) -->
              </div>
            </div>
            <div class="review-price">15.000 VNĐ</div>
          </div>
        </div>

        <div v-if="selectedReview == 'Free'">
          <div style="padding-top: 20px; text-align: center;">
            <el-alert
              v-if="unratedCount > 0"
              title="Please review all feedback before requesting for a free peer review"
              type="error"
              :closable="false"
            />
          </div>
        </div>

        <div v-if="selectedReview == '' || selectedReview == 'Free'" id="peer-review-card" class="box-card review-card" @click="onReviewSelect('Free')">
          <div>
            <div class="review-icon-wrapper"> <i class="fas fa-user-friends review-icon" /></div>
            <div class="review-description-wrapper">
              <!-- <div class="review-title">Free Peer Review</div> -->
              <div class="review-title">Đánh Giá Miễn Phí Từ Học Viên Khác</div>
              <!-- <div class="review-title">Đánh Giá Miễn Phí Từ Học Viên Khác</div> -->
              <div class="review-description">
                Nhận phản hồi mang tính xây dựng từ một học viên khác có trình độ viết tương đương hoặc cao hơn bạn (<a href="#">sample feedback</a>)
                <!-- Get constructive feedback from another learner with similar or higher writing level (<a href="#">sample feedback</a>) -->
              </div>
            </div>
            <div class="review-price">Miễn Phí</div>
          </div>
        </div>
      </div>
      <div v-if="selectedReview == 'Free'">
        <div style="padding-top: 10px; text-align: center;">
          <el-button type="primary" :disabled="unratedCount > 0" @click="requestPeerReview()">Confirm Selection</el-button>
        </div>
      </div>
      <div v-show="selectedReview == 'Pro' || selectedReview == 'AI'" class="co-form" :class="{ active: activeStep == 1 }">
        <div class="co-form__section" :class="{ inactive: activeStep == 2 }">
          <div class="co-form__title" style="padding-top: 10px">
            <el-button v-if="activeStep == 2" style="position: absolute;right: 1rem;" size="mini" type="primary" @click="editCheckOutInfo">Edit</el-button>
          </div>
          <div class="pay-button-container">
            <el-button type="danger" style="width: 100%; color: #006BC2; font-size: 18px; padding: 15px;" @click="submitZaloPayRequest()">
              Pay with ZaloPay
            </el-button>
          </div>

          <div class="pay-button-container">
            <el-button type="danger" style="width: 100%; color: #006BC2; font-size: 18px; padding: 15px;" @click="submitVNPayRequest()">
              Pay with VNPay
            </el-button>
          </div>
          <div v-if="!paid" id="paypal-button-container" class="pay-button-container" />
        </div>
        <div v-if="activeStep == 1" class="saperator" />
      </div>

    </div>
    <div slot="footer" class="dialog-footer" />
  </el-dialog>
</template>

<script>

import { loadScript } from '@paypal/paypal-js'
import paymentService from '../../services/payment.service'
import { REVIEW_REQUEST_STATUS } from '../../app.constant'
import reviewService from '../../services/review.service'

export default {
  name: 'Checkout',
  props: {
    submissionId: { type: Number, default: null },
    questionId: { type: Number, default: null },
    unratedCount: { type: Number, default: 0 }
  },
  data() {
    return {
      dlVisible: false,
      activeStep: 1,
      card: null,
      stripe: null,
      clientSecret: null,
      checkoutDisable: true,
      newMethod: false,
      selectedMethod: null,
      amount: 200000,
      paymentIntent: null,
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
    currentPrices() {
      return this.$store.getters['payment/getAllPrices']
    }
  },
  async mounted() {
    this.email = this.currentUser.email
  },
  async created() {
    this.getVnPayResultParams()
    this.getZaloPayResultParams()
  },
  methods: {
    openDialog() {
      this.dlVisible = true
    },
    async getZaloPayResultParams() {
      const apptransid = this.getUrlParameter('apptransid')
      if (apptransid) {
        const model = {
          appid: this.getUrlParameter('appid'),
          apptransid: this.getUrlParameter('apptransid'),
          pmcid: this.getUrlParameter('pmcid'),
          bankcode: this.getUrlParameter('bankcode'),
          amount: this.getUrlParameter('amount'),
          discountamount: this.getUrlParameter('discountamount'),
          status: this.getUrlParameter('status'),
          checksum: this.getUrlParameter('checksum')
        }
        console.log('Request model', model)
        const order = await paymentService.verifyZaloPayStatus(model)
        console.log('Updated order:', order)
        if (order) {
          if (order.status == 1) {
            if (order.reviewType == 'Pro') {
              this.requestProReview(order.userId, order.submissionId)
            } else if (order.reviewType == 'AI') {
              this.requestAutomatedReview(order.userId, order.submissionId)
            }
          }
        } else {
          // Return error message
        }
      }
    },
    async submitZaloPayRequest() {
      // Create a new order
      const order = {
        UserId: this.currentUser.id,
        SubmissionId: this.submissionId,
        ReviewType: this.selectedReview,
        Amount: this.amount,
        Status: 0 // Payment pending
      }
      const newOrder = await paymentService.createNewOrder(order)
      if (newOrder) {
        // Submit zalopay request
        const model = {
          userId: this.currentUser.id,
          orderId: newOrder.id,
          amount: this.amount
        }
        const orderUrl = await paymentService.submitZaloPayRequest(model)

        window.location.href = orderUrl
      }
    },
    async getVnPayResultParams() {
      const orderId = this.getUrlParameter('vnp_TxnRef')
      if (orderId) {
        const model = {
          orderId: orderId,
          vnpAmount: this.getUrlParameter('vnp_Amount'),
          vnpayTranId: this.getUrlParameter('vnp_TransactionNo'),
          vnpResponseCode: this.getUrlParameter('vnp_ResponseCode'),
          vnpTransactionStatus: this.getUrlParameter('vnp_TransactionStatus'),
          vnpSecureHash: this.getUrlParameter('vnp_SecureHash'),
          queryString: decodeURIComponent(window.location.search.substring(1))
        }
        console.log('Request model', model)
        const order = await paymentService.verifyVnPayStatus(model)
        console.log('Updated order:', order)
        if (order) {
          if (order.status == 1) {
            if (order.reviewType == 'Pro') {
              this.requestProReview(order.userId, order.submissionId)
            } else if (order.reviewType == 'AI') {
              this.requestAutomatedReview(order.userId, order.submissionId)
            }
          }
        } else {
          // Return error message
        }
      }
    },
    getUrlParameter(sParam) {
      const sPageURL = decodeURIComponent(window.location.search.substring(1))
      const sURLVariables = sPageURL.split('&')
      let sParameterName
      let i
      for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=')

        if (sParameterName[0] === sParam) {
          return sParameterName[1] === undefined ? true : sParameterName[1]
        }
      }
    },
    async submitVNPayRequest() {
      // Create a new order
      const order = {
        UserId: this.currentUser.id,
        SubmissionId: this.submissionId,
        ReviewType: this.selectedReview,
        Amount: this.amount,
        Status: 0 // Payment pending
      }
      const newOrder = await paymentService.createNewOrder(order)
      if (newOrder) {
        // Submit vnpay request
        const model = {
          orderId: newOrder.id,
          amount: this.amount
        }
        paymentService.submitVNPayRequest(model)
      }
    },
    async dialogOpened() {
      // Load paypal button
      loadScript({ 'client-id': 'Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I' }).then((paypal) => {
        paypal
          .Buttons({
            createOrder: this.createOrder,
            onApprove: this.onApprove
          })
          .render('#paypal-button-container')
      })
    },
    createOrder: function(data, actions) {
      return actions.order.create({
        purchase_units: [
          {
            amount: {
              value: Math.round(this.amount * 0.000041)
            }
          }
        ]
      })
    },
    onApprove: function(data, actions) {
      return actions.order.capture().then(order => {
        console.log(order)
        const purchaseUnits = order.purchase_units
        if (purchaseUnits.length > 0) {
          const captures = purchaseUnits[0].payments.captures
          if (captures.length > 0) {
            this.paid = true
            this.completeOrder(captures[0].id)
            this.paymentSuccessed()
          }
        }
      })
    },
    goBack() {
      this.selectedReview = ''
    },
    onReviewSelect(reviewType) {
      if (reviewType === 'Pro') {
        this.amount = 200000
        this.selectedReview = reviewType
      } else if (reviewType === 'AI') {
        this.amount = 30000
        this.selectedReview = reviewType
      } else {
        this.amount = 0
        this.selectedReview = reviewType
      }
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
    dialogClosed() {
      this.$emit('closed')
    },
    editCheckOutInfo() {
      this.activeStep = this.activeStep == 2 ? 1 : 2
    },
    paymentSuccessed() {
      if (this.selectedReview == 'Pro') {
        this.requestProReview(this.currentUser.id, this.submissionId)
      } else if (this.selectedReview == 'AI') {
        this.requestAutomatedReview(this.currentUser.id, this.submissionId)
      }
    },
    requestPeerReview() {
      if (this.unratedCount == 0) {
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
      }
    },
    requestProReview(userId, submissionId) {
      reviewService.createProReviewRequest({
        UserId: userId,
        SubmissionId: submissionId,
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
    requestAutomatedReview(userId, submissionId) {
      this.dialogClosed()
      const loading = this.$loading({
        lock: true,
        text: 'Please wait while our AI rater scores your essay',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.7)'
      })
      reviewService.createAutomatedReview({
        UserId: userId,
        SubmissionId: submissionId
      }).then(rs => {
        this.$notify.success({
          title: 'Automated AI Review',
          message: 'Essay scoring has been completed. Please review the feedback.',
          type: 'success',
          duration: 2000
        })
        loading.close()
        const url = `/review/${rs.questionId}/${rs.docId}/${rs.reviewId}`
        this.$router.push(url)
      })
    },
    async completeOrder(transactionId) {
      const order = {
        UserId: this.currentUser.id,
        SubmissionId: this.submissionId,
        ReviewType: this.selectedReview,
        Amount: this.amount,
        Status: 2, // Payment completed
        TransactionCode: transactionId
      }
      const orderCompleted = await paymentService.createNewOrder(order)
      console.log('Order Completed: ', orderCompleted)
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
  width: calc(100% - 205px);
}
.review-price {
  float: right;
  width: 120px;
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

.pay-button-container{
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
