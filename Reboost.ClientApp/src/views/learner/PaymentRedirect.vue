<template>
  <div class="list-container">
    <el-card v-if="orderStatus == 1" style="margin-top: 100px; padding: 30px; width: 100%;" class="box-card">
      <div>
        <i class="el-icon-success" style="font-size: 50px; color: #338abb;" />
      </div>
      <div style="margin-top: 20px;">
        <h1 style="font-size: 40px; color: #4a6f8a; font-weight: 400;">Thanh Toán Thành Công!</h1>
      </div>
      <div style="margin-top: 20px;">
        <p>Cảm ơn bạn đã thanh toán cho đơn hàng với mã số: #{{ order.id }}. Biên lai cho thanh toán đã được gửi cho bạn qua email.</p>
        <p v-if="feedbackAvailable">Bài luận của bạn đã dược chấm bởi hệ thống chấm bài tự động của Reboost. Bạn có thể xem phản hồi ngay bây giờ.</p>
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
    </el-card>
    <el-card v-else-if="orderStatus == 2" style="margin-top: 100px; padding: 30px; width: 100%;" class="box-card">
      <div>
        <i class="el-icon-info" style="font-size: 50px; color: #909399;" />
      </div>
      <div style="margin-top: 20px;">
        <h1 style="font-size: 40px; color: #909399; font-weight: 400;">Yêu Cầu Đã Được Xử Lý</h1>
      </div>
      <div style="margin-top: 20px;">
        <p style="color: #909399;">Yêu cầu chấm bài của bạn đã được xử lý. Mã đơn hàng của bạn là: #{{ order.id }}.</p>
        <p v-if="feedbackAvailable" style="color: #909399;">Bài luận của bạn đã dược chấm bởi hệ thống chấm bài tự động của Reboost. Bạn có thể xem phản hồi ngay bây giờ..</p>
        <p v-else style="color: #909399;">Giáo viên của Reboost sẽ chấm bài luận của bạn và cung cấp phản hồi trong vòng 24h. Chúng tôi sẽ thông báo cho bạn qua email khi giáo viên chấm xong.</p>
        <p style="color: #909399;">Nếu bạn có thắc mắc gì xin vui lòng liên hệ với chúng tôi qua <a href="mailto:support@reboost.vn">support@reboost.vn</a> để được hỗ trợ trực tiếp.</p>
      </div>
      <div v-if="feedbackAvailable" style="margin-top: 20px;">
        <el-button type="into" @click="viewFeedback()">Xem Phản Hồi</el-button>
      </div>
      <div v-else style="margin-top: 20px;">
        <el-button type="info" @click="viewQuestions()">Tiếp Tục Luyện Tập</el-button>
        <el-button type="info" plain style="margin-left: 10px;" @click="viewSubmission()">XEM LẠI BÀI ĐÃ NỘP</el-button>
      </div>
    </el-card>
    <el-card v-else-if="orderStatus == 0" style="margin-top: 100px; padding: 30px; width: 100%;" class="box-card">
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
      order: null
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
        const loading = this.$loading({
          lock: true,
          text: 'Vui lòng đợi trong giây lát',
          spinner: 'el-icon-loading',
          background: 'rgba(0, 0, 0, 0.7)'
        })
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

        const rs = await paymentService.verifyZaloPayStatus(model)
        if (rs) {
          if (rs.status == 1) {
            // Payment is completed, and review has been processed
            this.review = rs.review
            this.order = rs.order
            if (rs.order.reviewType == 'AI') {
              this.feedbackAvailable = true
            }
            this.orderStatus = 1
          } else if (rs.status == 2) {
            // Order has already been processed
            this.review = rs.review
            this.order = rs.order
            if (rs.order.reviewType == 'AI') {
              this.feedbackAvailable = true
            }
            this.orderStatus = 2
          } else {
            // There was an error with the request
            this.orderStatus = 0
          }
        } else {
          // There was an error with the request
          this.orderStatus = 0
        }
        loading.close()
      }
    },
    async verifyVnPayStatus() {
      const orderId = this.getUrlParameter('vnp_TxnRef')
      if (orderId) {
        const loading = this.$loading({
          lock: true,
          text: 'Vui lòng đợi trong giây lát',
          spinner: 'el-icon-loading',
          background: 'rgba(0, 0, 0, 0.7)'
        })
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
            // Payment is completed, get review data
            const rs = await paymentService.processVnPayOrder(orderId)
            if (rs) {
              if (rs.status == 1) {
                // Payment is completed, and review has been processed
                this.review = rs.review
                this.order = rs.order
                if (rs.order.reviewType == 'AI') {
                  this.feedbackAvailable = true
                }
                this.orderStatus = 1
              } else {
                // There was an error with the request
                this.orderStatus = 0
              }
            } else {
              this.orderStatus = 0
            }
          } else {
            this.orderStatus = 0
          }
        } else {
          // There was an error with the request
          this.orderStatus = 0
        }
        loading.close()
      }
    },
    // async verifyVnPayStatus() {
    //   const orderId = this.getUrlParameter('vnp_TxnRef')
    //   if (orderId) {
    //     const loading = this.$loading({
    //       lock: true,
    //       text: 'Vui lòng đợi trong giây lát',
    //       spinner: 'el-icon-loading',
    //       background: 'rgba(0, 0, 0, 0.7)'
    //     })
    //     const model = {
    //       userId: this.currentUser.id,
    //       orderId: orderId,
    //       vnpAmount: this.getUrlParameter('vnp_Amount'),
    //       vnpayTranId: this.getUrlParameter('vnp_TransactionNo'),
    //       vnpResponseCode: this.getUrlParameter('vnp_ResponseCode'),
    //       vnpTransactionStatus: this.getUrlParameter('vnp_TransactionStatus'),
    //       vnpSecureHash: this.getUrlParameter('vnp_SecureHash'),
    //       queryString: decodeURIComponent(window.location.search.substring(1))
    //     }

    //     const rs = await paymentService.verifyVnPayStatus(model)
    //     if (rs) {
    //       if (rs.status == 1) {
    //         // Payment is completed, and review has been processed
    //         this.review = rs.review
    //         this.order = rs.order
    //         if (rs.order.reviewType == 'AI') {
    //           this.feedbackAvailable = true
    //         }
    //         this.orderStatus = 1
    //       } else if (rs.status == 2) {
    //         // Order has already been processed
    //         this.review = rs.review
    //         this.order = rs.order
    //         if (rs.order.reviewType == 'AI') {
    //           this.feedbackAvailable = true
    //         }
    //         this.orderStatus = 2
    //       } else {
    //         // There was an error with the request
    //         this.orderStatus = 0
    //       }
    //     } else {
    //       // There was an error with the request
    //       this.orderStatus = 0
    //     }
    //     loading.close()
    //   }
    // },
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
