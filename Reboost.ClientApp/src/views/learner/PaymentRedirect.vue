<template>
  <div class="list-container">
    <el-card v-if="orderStatus == 1" style="margin-top: 50px; padding: 30px; width: 100%; text-align: center;" class="box-card">
      <div style="">
        <i class="el-icon-success" style="font-size: 50px; color: #338abb;" />
      </div>
      <div style="margin-top: 20px;">
        <h1 style="font-size: 40px; color: #4a6f8a; font-weight: 400;">Thanh Toán Thành Công!</h1>
      </div>
      <div v-if="orderProcessing" style="margin-top: 20px;">
        <p>Cảm ơn bạn đã tin tưởng sử dụng dịch vụ chấm bài của Reboost.</p>
        <div v-loading="true" style="width: 100%; height: 300px" element-loading-text="Vui lòng chờ trong giây lát trong khi chúng tôi xử lý yêu cầu của bạn" />
      </div>
      <div v-else style="margin-top: 20px;">
        <div v-if="order">
          <div v-if="order.status == 1">
            <div>
              <p>Cảm ơn bạn đã thanh toán cho đơn hàng với mã số: #{{ order.id }}. Biên lai cho thanh toán đã được gửi cho bạn qua email.</p>
              <p v-if="feedbackAvailable">Bài luận của bạn đã được chấm bởi hệ thống chấm bài tự động của Reboost. Bạn có thể xem phản hồi ngay bây giờ.</p>
              <p v-else>Giáo viên của Reboost sẽ chấm bài luận của bạn và cung cấp phản hồi trong vòng 24h. Chúng tôi sẽ thông báo cho bạn qua email khi giáo viên chấm xong.</p>
              <p>Nếu bạn có thắc mắc gì xin vui lòng liên hệ với chúng tôi qua <a href="mailto:support@reboost.vn">support@reboost.vn</a> để được hỗ trợ trực tiếp.</p>
            </div>
            <div v-if="feedbackAvailable" style="margin-top: 20px;">
              <el-button class="btn btn-gradient" style="padding: 10px 20px; font-size: 13px;" @click="viewFeedback()">Xem Phản Hồi</el-button>
            </div>
            <div v-else style="margin-top: 20px;">
              <el-button class="btn btn-gradient" style="padding: 10px 20px; font-size: 13px;" @click="viewQuestions()">Tiếp Tục Luyện Tập</el-button>
              <el-button plain style="margin-left: 10px;" @click="viewSubmission()">XEM LẠI BÀI ĐÃ NỘP</el-button>
            </div>
          </div>
          <div v-else-if="order.status == 2">
            <div>
              <p>Yêu cầu chấm bài của bạn đã được xử lý. Mã đơn hàng của bạn là: #{{ order.id }}.</p>
              <p v-if="feedbackAvailable">Bài luận của bạn đã được chấm bởi hệ thống chấm bài tự động của Reboost. Bạn có thể xem phản hồi ngay bây giờ.</p>
              <p v-else>Giáo viên của Reboost sẽ chấm bài luận của bạn và cung cấp phản hồi trong vòng 24h. Chúng tôi sẽ thông báo cho bạn qua email khi giáo viên chấm xong.</p>
              <p>Nếu bạn có thắc mắc gì xin vui lòng liên hệ với chúng tôi qua <a href="mailto:support@reboost.vn">support@reboost.vn</a> để được hỗ trợ trực tiếp.</p>
            </div>
            <div v-if="feedbackAvailable" style="margin-top: 20px;">
              <el-button class="btn btn-gradient" style="padding: 10px 20px; font-size: 13px;" @click="viewFeedback()">Xem Phản Hồi</el-button>
            </div>
            <div v-else style="margin-top: 20px;">
              <el-button class="btn btn-gradient" style="padding: 10px 20px; font-size: 13px;" @click="viewQuestions()">Tiếp Tục Luyện Tập</el-button>
              <el-button plain style="margin-left: 10px;" @click="viewSubmission()">XEM LẠI BÀI ĐÃ NỘP</el-button>
            </div>
          </div>
          <div v-else>
            <div>
              <p style="color: #F56C6C;">Xin lỗi bạn vì sự bất tiện này! Đã có sự cố xảy ra trong quá trình xử lý yêu cầu chấm bài của bạn.</p>
              <p style="color: #F56C6C;">Bạn vui lòng liên hệ với chúng tôi qua <a href="mailto:support@reboost.vn" style="color: #F56C6C;">support@reboost.vn</a> để được hỗ trợ trực tiếp.</p>
            </div>
            <div style="margin-top: 20px;">
              <el-button type="danger" plain @click="contactUs()">Liên Hệ Với Chúng tôi</el-button>
            </div>
          </div>
        </div>
      </div>
    </el-card>
    <el-card v-else-if="orderStatus == 0" style="margin-top: 100px; padding: 30px; width: 100%; text-align: center;" class="box-card">
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
  </div>

</template>
<script>
import paymentService from '../../services/payment.service'
import ContactDialog from '../../components/controls/ContactDialog.vue'
export default {
  name: 'PaymentRedirect',
  components: {
    'contact-dialog': ContactDialog
  },
  data() {
    return {
      orderStatus: null,
      paymentSuccess: false,
      paymentError: false,
      feedbackAvailable: false,
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
        if (paymentResult) {
          if (paymentResult.status == 1) {
            this.orderStatus = 1
            this.orderProcessing = true
            const rs = await paymentService.processOrder(orderId)
            if (rs) {
              if (rs.status == 1 || rs.status == 2) {
                this.review = rs.review
                this.order = rs.order
                if (rs.order.reviewType == 'AI') {
                  this.feedbackAvailable = true
                }
              } else {
                this.order = rs.order
              }
            }
            this.orderProcessing = false
          } else {
            this.orderStatus = 0
          }
        } else {
          this.orderStatus = 0
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
        if (paymentResult) {
          if (paymentResult.status == 1) {
            this.orderStatus = 1
            this.orderProcessing = true
            const rs = await paymentService.processOrder(orderId)
            if (rs) {
              if (rs.status == 1 || rs.status == 2) {
                this.review = rs.review
                this.order = rs.order
                if (rs.order.reviewType == 'AI') {
                  this.feedbackAvailable = true
                }
              } else {
                this.order = rs.order
              }
            }
            this.orderProcessing = false
          } else {
            this.orderStatus = 0
          }
        } else {
          this.orderStatus = 0
        }
      }
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
