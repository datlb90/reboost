<template>
  <div class="list-container">
    <el-card v-if="paymentStatus == 1" style="margin-top: 50px; padding: 30px; width: 100%; text-align: center;" class="box-card">
      <div style="">
        <i v-if="orderStatus == 1 || orderProcessing" class="el-icon-success" style="font-size: 50px; color: #338abb;" />
        <i v-else class="el-icon-error" style="font-size: 50px; color: #F56C6C;" />
      </div>
      <div style="margin-top: 20px;">
        <h1 v-if="orderStatus == 1 || orderProcessing" style="font-size: 40px; color: #4a6f8a; font-weight: 400;">Thanh Toán Thành Công!</h1>
        <h1 v-else style="font-size: 40px; color: #F56C6C; font-weight: 400;">Đã Có Lỗi Xảy Ra!</h1>
      </div>
      <div v-if="orderProcessing" style="margin-top: 20px;">
        <p>Cảm ơn bạn đã tin tưởng sử dụng dịch vụ của Reboost!</p>
        <div v-loading="true" style="width: 100%; height: 300px" element-loading-text="Vui lòng chờ trong giây lát trong khi chúng tôi xử lý đơn hàng của bạn" />
      </div>
      <div v-else style="margin-top: 20px;">
        <div v-if="orderStatus == 1">
          <div v-if="order">
            <p>Cảm ơn bạn đã
              <span v-if="order.subscriptionType == 'new'">mua</span>
              <span v-else-if="order.subscriptionType == 'renew'">gia hạn</span>
              <span v-else>nâng cấp lên</span>
              gói
              <span v-if="order.planId <=3 ">phản hồi chi tiết</span>
              <span v-else>phản hồi chuyên sâu</span>
              với thời hạn
              <span v-if="order.planId == 1 || order.planId == 4">6 tháng</span>
              <span v-else-if="order.planId == 2 || order.planId == 5">3 tháng</span>
              <span v-else>1 tháng</span>!
            </p>

            <p>Mã số cho đơn hàng của bạn là #{{ order.id }}. Biên lai cho thanh toán đã được gửi cho bạn qua email.</p>

            <p>Nếu bạn có thắc mắc gì xin vui lòng liên hệ với chúng tôi qua <a href="mailto:support@reboost.vn">support@reboost.vn</a> để được hỗ trợ trực tiếp.</p>
          </div>

          <div style="margin-top: 30px;">
            <el-button class="btn btn-gradient" style="padding: 10px 20px; font-size: 13px;" @click="getFeedback()">Nhận Phản Hồi Ngay</el-button>
            <el-button plain style="margin-left: 10px;" @click="viewQuestions()">Khám Phá Các Chủ Đề Viết</el-button>
          </div>

        </div>
        <div v-else>
          <div>
            <p style="color: #F56C6C;">Xin lỗi bạn vì sự bất tiện này! Bạn đã thanh toán thành công nhưng có sự cố xảy ra trong quá trình xử lý đơn hàng #{{ order.id }}.</p>
            <p style="color: #F56C6C;">Bạn vui lòng liên hệ với chúng tôi qua <a style="color: #F56C6C;" href="mailto:support@reboost.vn">support@reboost.vn</a> để được hỗ trợ trực tiếp.</p>
          </div>
          <div style="margin-top: 20px;">
            <el-button type="danger" plain @click="contactUs()">Liên Hệ Với Chúng tôi</el-button>
          </div>
        </div>
      </div>
    </el-card>
    <el-card v-else-if="paymentStatus == 0" style="margin-top: 100px; padding: 30px; width: 100%; text-align: center;" class="box-card">
      <div>
        <i class="el-icon-error" style="font-size: 50px; color: #F56C6C;" />
      </div>
      <div style="margin-top: 20px;">
        <h1 style="font-size: 40px; color: #F56C6C; font-weight: 400;">Thanh Toán Không Thành Công</h1>
      </div>
      <div style="margin-top: 20px;">
        <p style="color: #F56C6C;">Xin lỗi bạn vì sự bất tiện này! Đã có lỗi xảy ra trong quá trình thanh toán<span v-if="order"> cho mã đơn hàng: #{{ order.id }}</span>.</p>
        <p style="color: #F56C6C;">Bạn vui lòng liên hệ với chúng tôi qua <a href="mailto:support@reboost.vn" style="color: #F56C6C;">support@reboost.vn</a> để được hỗ trợ trực tiếp.</p>
      </div>
      <div style="margin-top: 20px;">
        <el-button type="danger" plain @click="contactUs()">Liên Hệ Với Chúng tôi</el-button>
      </div>
    </el-card>
    <contact-dialog ref="contactDialog" />
    <review-request-dialog ref="reviewRequestDialog" />
  </div>

</template>
<script>
import paymentService from '../../services/payment.service'
import ContactDialog from '../../components/controls/ContactDialog.vue'
import ReviewRequestDialog from '../../components/controls/ReviewRequestDialog.vue'
export default {
  name: 'PaymentRedirect',
  components: {
    'contact-dialog': ContactDialog,
    'review-request-dialog': ReviewRequestDialog
  },
  data() {
    return {
      paymentStatus: null,
      orderStatus: null,
      review: null,
      order: null,
      orderProcessing: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  async mounted() {
    this.verifyVnPayStatus()
    this.verifyZaloPayStatus()
  },
  async created() {

  },
  methods: {
    async verifyZaloPayStatus() {
      const apptransid = this.getUrlParameter('apptransid')
      if (apptransid) {
        const transPlited = apptransid.split('_')
        const orderId = parseInt(transPlited[1])
        const model = {
          userId: this.currentUser.id,
          appid: this.getUrlParameter('appid'),
          apptransid: this.getUrlParameter('apptransid'),
          pmcid: this.getUrlParameter('pmcid'),
          bankcode: this.getUrlParameter('bankcode'),
          amount: this.getUrlParameter('amount'),
          discountamount: this.getUrlParameter('discountamount'),
          status: this.getUrlParameter('status'),
          checksum: this.getUrlParameter('checksum')
        }
        const paymentResult = await paymentService.verifyZaloPayStatus(model)
        console.log(paymentResult)

        if (paymentResult && paymentResult.status == 1) {
          this.paymentStatus = 1 // Payment success
          // Process the order now
          this.orderProcessing = true
          const rs = await paymentService.processOrder('zalopay', orderId)
          console.log(rs)
          if (rs && rs.status == 2) {
            this.order = rs.order
            // Order processed successfully
            this.orderStatus = 1
            // Update user subscription
            if (rs.subscription) {
              let duration = 6
              if (rs.subscription.planId == 1) {
                duration = 0
              } else if (rs.subscription.planId == 2) {
                duration = 3
              } else if (rs.subscription.planId == 3) {
                duration = 1
              } else if (rs.subscription.planId == 4) {
                duration = 6
              } else if (rs.subscription.planId == 5) {
                duration = 3
              } else {
                duration = 1
              }
              const userSubscription = {
                userId: rs.subscription.userId,
                planId: rs.subscription.planId,
                duration: duration,
                startDate: rs.subscription.startDate,
                endDate: rs.subscription.endDate
              }
              this.$store.dispatch('auth/setSubscription', userSubscription)
            }
          } else {
            if (rs) this.order = rs.order
            // Order was not processed successfully
            this.orderStatus = 0
          }
          this.orderProcessing = false
        } else {
          this.paymentStatus = 0 // Payment error
        }
      }
    },
    async verifyVnPayStatus() {
      const orderId = this.getUrlParameter('vnp_TxnRef')
      if (orderId) {
        const model = {
          userId: this.currentUser.id,
          orderId: orderId,
          vnpAmount: this.getUrlParameter('vnp_Amount'),
          vnpayTranId: this.getUrlParameter('vnp_TransactionNo'),
          vnpResponseCode: this.getUrlParameter('vnp_ResponseCode'),
          vnpTransactionStatus: this.getUrlParameter('vnp_TransactionStatus'),
          vnpSecureHash: this.getUrlParameter('vnp_SecureHash'),
          queryString: decodeURIComponent(window.location.search.substring(1))
        }

        const paymentResult = await paymentService.verifyVnPayStatus(model)
        if (paymentResult && paymentResult.status == 1) {
          this.paymentStatus = 1 // Payment success
          // Process the order now
          this.orderProcessing = true
          const rs = await paymentService.processOrder('vnpay', orderId)
          if (rs && rs.status == 2) {
            this.order = rs.order
            // Order processed successfully
            this.orderStatus = 1
            // Update user subscription
            if (rs.subscription) {
              console.log(rs.subscription)
              let duration = 6
              if (rs.subscription.planId == 1) {
                duration = 6
              } else if (rs.subscription.planId == 2) {
                duration = 3
              } else if (rs.subscription.planId == 3) {
                duration = 1
              } else if (rs.subscription.planId == 4) {
                duration = 6
              } else if (rs.subscription.planId == 5) {
                duration = 3
              } else {
                duration = 1
              }
              const userSubscription = {
                userId: rs.subscription.userId,
                planId: rs.subscription.planId,
                duration: duration,
                startDate: rs.subscription.startDate,
                endDate: rs.subscription.endDate
              }
              console.log(userSubscription)
              this.$store.dispatch('auth/setSubscription', userSubscription)
            }
          } else {
            if (rs) this.order = rs.order
            // Order was not processed successfully
            this.orderStatus = 0
          }
          this.orderProcessing = false
        } else {
          this.paymentStatus = 0 // Payment error
        }
      }
    },
    getFeedback() {
      this.$refs.reviewRequestDialog?.openDialog()
    },
    viewFeedback() {
      const url = `/review/${this.review.questionId}/${this.review.docId}/${this.review.reviewId}`
      this.$router.push(url)
    },
    viewQuestions() {
      this.$router.push('/questions')
    },
    viewSubmission() {
      const url = `/practice/${this.review.questionId}/${this.review.submissionId}`
      this.$router.push(url)
    },
    contactUs() {
      this.$refs.contactDialog?.openDialog()
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
    }
  }
}
</script>
