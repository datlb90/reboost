<template>
  <el-dialog
    v-if="screenWidth > 780"
    id="practiceWritingCheckoutContainer"
    :visible.sync="dlVisible"
    width="760px"
    height="820px"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 5px 20px; font-size: 18px; text-align:center;">Nhận Phản Hồi Cho Bài Viết</div>
    </div>
    <div class="dialog-body">
      <div style="padding: 20px; padding-top: 20px;">
        <div v-if="selectedReview == ''" class="tip">
          <span style="font-size: 15px; color: #6084a4;"> Nhận phản hồi là cách tốt nhất giúp bạn tiến bộ, hãy lựa chọn 1 phản hồi thích hợp dưới đây:</span>
        </div>
        <el-page-header v-if="selectedReview == 'Pro'" class="tip" content="Cung cấp yêu cầu cho phản hồi và lựa chọn phương thức thanh toán" @back="goBack" />
        <el-page-header v-if="selectedReview == 'Free' || selectedReview == 'AI'" class="tip" content="Xác nhận lựa chọn của bạn và gửi yêu cầu chấm bài" @back="goBack" />

        <div v-if="selectedReview == '' || selectedReview == 'Pro'" id="pro-review-card" class="box-card review-card" style="height: 140px;" @click="onReviewSelect('Pro')">
          <div>
            <div class="review-icon-wrapper" style="width: 62px;"> <i class="fas fa-user-graduate review-icon" /></div>
            <div class="review-description-wrapper">
              <div class="review-title">Phản Hồi Chuyên Sâu Từ Giáo Viên</div>
              <div class="review-description" style="margin-bottom: 20px;">
                Nhận phản hồi có chiều sâu cùng những hướng dẫn bổ ích từ giáo viên của Reboost.
                Giáo viên có thể đánh giá sự tiến bộ của bạn dựa theo những bài đã nộp trước đó và phản hồi theo yêu cầu cụ thể.
                <!-- trong vòng 24 giờ (<a href="#">đánh giá mẫu</a>) -->
                <!-- Our certified rater will score your essay and provide detailed feedback within 24 hours (<a href="#">sample feedback</a>) -->
              </div>

            </div>
            <div class="review-price">50.000 VNĐ</div>
          </div>
        </div>

        <div v-if="selectedReview == '' || selectedReview == 'AI'" id="ai-review-card" class="box-card review-card" @click="onReviewSelect('AI')">
          <div>
            <div class="review-icon-wrapper"><i class="fas fa-robot review-icon" /></div>
            <div class="review-description-wrapper">
              <!-- <div class="review-title">Instant Feedback from AI Rater</div> -->
              <div class="review-title">Phản Hồi Nhanh và Chi Tiết Từ ChatGPT-4</div>
              <div class="review-description">
                Nhận phản hồi nhanh chóng, chi tiết, và chính xác cho bài viết của bạn từ hệ thống chấm bài tự động được vận hành bởi ChatGPT-4.
                <!-- (<a href="#">đánh giá mẫu</a>) -->
                <!-- Get insighful feedback and score from our powerful writing review AI within seconds (<a href="#">sample feedback</a>) -->
              </div>
            </div>
            <div class="review-price">Miễn phí</div>
          </div>
        </div>

        <div v-if="selectedReview == 'Free'">
          <div style="padding-top: 20px; text-align: center;">
            <el-alert
              title="Để tạo môi trường học tập lành mạnh và khuyến khích các bạn thực hiện đánh giá cho các học viên khác. Chúng tôi sẽ yêu cầu các bạn cung cấp ít nhất 1 đánh giá trước khi có thể nhận phản hồi miễn phí từ học viên khác. Quy định này sẽ không áp dụng cho lần yêu cầu nhận phản hồi đầu tiên."
              type="info"
              style="word-break: break-word; text-align: left;"
              :closable="false"
            />
          </div>
        </div>

        <div v-if="selectedReview == '' || selectedReview == 'Free'" id="peer-review-card" class="box-card review-card" @click="onReviewSelect('Free')">
          <div>
            <div class="review-icon-wrapper"> <i class="fas fa-user-friends review-icon" /></div>
            <div class="review-description-wrapper">
              <!-- <div class="review-title">Free Peer Review</div> -->
              <div class="review-title">Góp Ý Từ Học Viên Khác</div>
              <!-- <div class="review-title">Đánh Giá Miễn Phí Từ Học Viên Khác</div> -->
              <div class="review-description">
                Nhận phản hồi mang tính xây dựng từ học viên khác giúp bạn có cái nhìn đa chiều về bài viết, mở ra cơ hội học hỏi và cải thiện kỹ năng 1 cách hiệu quả.
                <!-- Get constructive feedback from another learner with similar or higher writing level (<a href="#">sample feedback</a>) -->
              </div>
            </div>
            <div class="review-price">Miễn Phí</div>
          </div>
        </div>
      </div>

      <div v-show="selectedReview == 'Pro' || (selectedReview == 'AI' && !requestedLanguage)" style="padding: 20px; padding-top: 0px;">
        <div>
          <div>
            <el-divider>
              <div class="review-title">
                Yêu cầu cho phản hồi
              </div>
            </el-divider>
            <div>
              <div style="text-align: center;">
                <el-radio-group v-if="!requestedLanguage" v-model="feedbackLanguage" style="margin-top: 10px; margin-bottom: 10px;">
                  <el-radio-button label="Phản hồi bằng tiếng Việt" />
                  <el-radio-button label="Phản hồi bằng tiếng Anh" />
                </el-radio-group>

                <el-input
                  v-if="selectedReview == 'Pro'"
                  v-model="specialRequest"
                  type="textarea"
                  :rows="3"
                  style="margin-bottom: 10px;"
                  placeholder="Cung cấp yêu cầu cụ thể của bạn cho giáo viên. Ví dụ: chú trọng phần từ vựng, ý tưởng, ngữ pháp, và bố cục bài. Nếu không có yêu cầu gì bạn có thể để trống phần này."
                />
              </div>

            </div>
          </div>

          <el-divider v-if="selectedReview == 'Pro'">
            <div class="review-title">
              Phương thức thanh toán
            </div>
          </el-divider>
          <div v-if="selectedReview == 'Pro'" style="text-align: center; padding-top: 10px;">
            <el-button plain style="margin-right: 10px;" @click="submitZaloPayRequest()">
              <div style="font-size: 20px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
              <img style="height: 26px;" src="../../assets/logo/zalopay.png" alt="logo" class="main-header-logo">
            </el-button>

            <el-button plain style="margin-left: 10px;" @click="submitVNPayRequest()">
              <div style="font-size: 20px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
              <img style="height: 26px;" src="../../assets/logo/vnpay.png" alt="logo" class="main-header-logo">
            </el-button>
          </div>
          <!-- <div v-if="!paid" id="paypal-button-container" class="pay-button-container" /> -->
        </div>
      </div>

      <div v-if="selectedReview == 'Free'|| selectedReview == 'AI'">
        <div style="padding-top: 20px; text-align: center;">
          <el-button v-if="selectedReview == 'Free'" type="primary" :loading="loading" @click="requestReview()">Xác nhận</el-button>
          <el-button v-else :loading="loading" @click="requestReview()">Xác nhận</el-button>
        </div>
      </div>
    </div>
    <div slot="footer" class="dialog-footer" />
  </el-dialog>

  <el-dialog
    v-else
    :visible.sync="dlVisible"
    :fullscreen="true"
    :style="'margin-top: 50px;'"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 0px 20px; font-size: 18px; text-align:center;">Nhận Phản Hồi Cho Bài Viết</div>
    </div>
    <div class="dialog-body">
      <div>
        <div v-if="selectedReview == ''" class="tip">
          <span style="font-size: 15px; color: #6084a4; word-break: break-word;">Cảm ơn bạn đã gửi bài viết cho chúng tôi! Để nhận phản hồi, hãy chọn 1 trong các dịch vụ dưới đây:</span>
        </div>
        <el-page-header v-if="selectedReview == 'Pro' " class="tip" content="Cung cấp yêu cầu cho phản hồi và lựa chọn phương thức thanh toán" @back="goBack" />
        <el-page-header v-if="selectedReview == 'Free' || selectedReview == 'AI'" class="tip" content="Hãy xác nhận lựa chọn của bạn và gửi yêu cầu chấm bài" @back="goBack" />

        <div v-if="selectedReview == '' || selectedReview == 'Pro'" id="pro-review-card" class="box-card review-card-mobile" @click="onReviewSelect('Pro')">
          <div>
            <div class="pricing-title-wrapper">
              <div class="pricing-option-icon"><i class="fas fa-user-graduate review-icon" /> </div>
              <div class="pricing-option-title">Phản Hồi Chuyên Sâu Từ Giáo Viên</div>
            </div>
            <div>
              <div class="pricing-option-description">
                <!-- Đội ngũ giáo viên giàu kinh nghiệm của Reboost sẽ chấm bài viết của bạn và cung cấp phản hồi chi tiết trong vòng 24 giờ. <a href="#">Xem phản hồi mẫu</a> -->
                Nhận phản hồi có chiều sâu cùng những hướng dẫn bổ ích từ giáo viên của Reboost.
                Giáo viên có thể đánh giá sự tiến bộ của bạn dựa theo những bài đã nộp trước đó và phản hồi theo yêu cầu cụ thể.
              </div>
            </div>
            <div class="pricing-option-price" style="width: 120px;">50.000 VNĐ</div>
          </div>
        </div>

        <div v-if="selectedReview == '' || selectedReview == 'AI'" id="ai-review-card" class="box-card review-card-mobile" @click="onReviewSelect('AI')">
          <div>
            <div class="pricing-title-wrapper">
              <div class="pricing-option-icon"><i class="fas fa-robot review-icon" /></div>
              <div class="pricing-option-title">Phản Hồi Nhanh Từ ChatGPT-4</div>
            </div>
            <div>
              <div class="pricing-option-description">
                <!-- Nhận phản hồi ngay lập tức và chính xác về bài viết của mình từ hệ thống chấm bài tự động được vận hành bởi ChatGPT-4. <a href="#">Xem phản hồi mẫu</a> -->
                Nhận phản hồi nhanh chóng, chi tiết, và chính xác cho bài viết của bạn từ hệ thống chấm bài tự động được vận hành bởi ChatGPT-4.
              </div>
            </div>
            <div class="pricing-option-price" style="width: 90px;">Miễn Phí</div>
          </div>
        </div>
        <div v-if="selectedReview == 'Free'">
          <div style="padding-top: 20px; text-align: center;">
            <el-alert
              title="Để tạo môi trường học tập lành mạnh và khuyến khích các bạn thực hiện đánh giá cho các học viên khác. Chúng tôi sẽ yêu cầu các bạn cung cấp ít nhất 1 đánh giá trước khi có thể nhận phản hồi miễn phí từ học viên khác. Quy định này sẽ không áp dụng cho lần yêu cầu nhận phản hồi đầu tiên."
              type="info"
              style="word-break: break-word; text-align: left;"
              :closable="false"
            />
          </div>
        </div>
        <div v-if="selectedReview == '' || selectedReview == 'Free'" id="peer-review-card" class="box-card review-card-mobile" @click="onReviewSelect('Free')">
          <div>
            <div class="pricing-title-wrapper">
              <div class="pricing-option-icon"> <i class="fas fa-user-friends review-icon" /></div>
              <div class="pricing-option-title">Góp Ý Từ Học Viên Khác</div>
            </div>
            <div>
              <div class="pricing-option-description">
                <!-- Nhận phản hồi mang tính xây dựng từ một học viên khác có band score tương đương hoặc cao hơn bạn. <a href="#">Xem phản hồi mẫu</a> -->
                Nhận phản hồi mang tính xây dựng từ học viên khác giúp bạn có cái nhìn đa chiều về bài viết, mở ra cơ hội học hỏi và cải thiện kỹ năng 1 cách hiệu quả.
              </div>
            </div>
            <div class="pricing-option-price" style="width: 90px;">Miễn Phí</div>
          </div>
        </div>
      </div>
      <div v-show="selectedReview == 'Pro' || (selectedReview == 'AI' && !requestedLanguage)" style="padding-top: 30px;">
        <div>
          <div>
            <el-divider content-position="center">
              <div class="review-title" style="width: 180px;">
                Yêu cầu cho phản hồi
              </div>
            </el-divider>
            <div>
              <div style="text-align: center;">
                <el-radio-group v-if="!requestedLanguage" v-model="feedbackLanguage" style="margin-top: 10px; margin-bottom: 10px;" size="small">
                  <el-radio-button label="Phản hồi bằng tiếng Việt" />
                  <el-radio-button label="Phản hồi bằng tiếng Anh" />
                </el-radio-group>

                <el-input
                  v-if="selectedReview == 'Pro'"
                  v-model="specialRequest"
                  type="textarea"
                  :rows="3"
                  style="margin-bottom: 20px;"
                  placeholder="Cung cấp yêu cầu cụ thể của bạn cho giáo viên. Ví dụ: chú trọng phần từ vựng, ý tưởng, ngữ pháp, và bố cục bài. Nếu không có yêu cầu gì bạn có thể để trống phần này."
                />
              </div>

            </div>
          </div>

          <el-divider v-if="selectedReview == 'Pro'" content-position="center">
            <div class="review-title" style="width: 200px;">
              Phương thức thanh toán
            </div>
          </el-divider>
          <div v-if="selectedReview == 'Pro'" style="text-align: center; padding-top: 20px;">
            <el-button plain style="width: 100%;" @click="submitZaloPayRequest()">
              <div style="width: 240px; margin: auto;">
                <div style="font-size: 20px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                <img style="height: 26px;" src="../../assets/logo/zalopay.png" alt="logo" class="main-header-logo">
              </div>

            </el-button>

            <el-button plain style="width: 100%; margin-left: 0px; margin-top: 10px;" @click="submitVNPayRequest()">
              <div style="width: 240px; margin: auto;">
                <div style="font-size: 20px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                <img style="height: 26px;" src="../../assets/logo/vnpay.png" alt="logo" class="main-header-logo">
              </div>

            </el-button>
          </div>
        </div>
      </div>

      <div v-if="selectedReview == 'Free' || selectedReview == 'AI'">
        <div style="padding-top: 20px; text-align: center;">
          <el-button v-if="selectedReview == 'Free'" type="primary" :loading="loading" @click="requestReview()">Xác nhận</el-button>
          <el-button v-else :loading="loading" @click="requestReview()">Xác nhận</el-button>
        </div>
      </div>
    </div>
    <div slot="footer" class="dialog-footer" />
  </el-dialog>
</template>

<script>

import { REVIEW_REQUEST_STATUS } from '../../app.constant'
import reviewService from '../../services/review.service'
import paymentService from '../../services/payment.service'

export default {
  name: 'Checkout',
  props: {
    submissionId: { type: Number, default: null },
    questionId: { type: Number, default: null },
    requestedLanguage: {type: String, default: null}
  },
  data() {
    return {
      dlVisible: false,
      card: null,
      stripe: null,
      clientSecret: null,
      checkoutDisable: true,
      newMethod: false,
      amount: 200000,
      paymentIntent: null,
      epaySelected: false,
      paid: false,
      selectedReview: '',
      loadingAutomatedReview: false,
      feedbackLanguage: 'Phản hồi bằng tiếng Việt',
      specialRequest: null,
      screenWidth: window.innerWidth,
      loading: false
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
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  async mounted() {
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    this.email = this.currentUser.email
  },
  async created() {
    // console.log(this.requestedLanguage)
  },
  methods: {
    async submitZaloPayRequest() {
      const model = {
        userId: this.currentUser.id,
        submissionId: this.submissionId,
        reviewType: this.selectedReview,
        amount: this.amount,
        status: 0,
        feedbackLanguage: this.requestedLanguage ? this.requestedLanguage : this.feedbackLanguage,
        specialRequest: this.specialRequest
      }
      var zaloPayUrl = await paymentService.submitZaloPayRequest(model)
      window.location.href = zaloPayUrl
    },
    async submitVNPayRequest() {
      // this.requestAIReview()
      const model = {
        userId: this.currentUser.id,
        submissionId: this.submissionId,
        reviewType: this.selectedReview,
        amount: this.amount,
        status: 0,
        feedbackLanguage: this.requestedLanguage ? this.requestedLanguage : this.feedbackLanguage,
        specialRequest: this.specialRequest
      }
      var vnPayUrl = await paymentService.submitVNPayRequest(model)
      window.location.href = vnPayUrl
    },
    openDialog() {
      this.dlVisible = true
      console.log(this.submissionId)
    },
    dialogClosed() {
      this.$emit('closed')
    },
    goBack() {
      this.selectedReview = ''
    },
    onReviewSelect(reviewType) {
      if (reviewType === 'Pro') {
        this.amount = 200000
        this.selectedReview = reviewType
      } else if (reviewType === 'AI') {
        this.amount = 15000
        this.selectedReview = reviewType
      } else {
        this.amount = 0
        this.selectedReview = reviewType
      }
    },
    requestReview() {
      if (this.selectedReview == 'Free') {
        this.requestPeerReview()
      } else {
        this.requestAIReview()
      }
    },
    requestPeerReview() {
      this.loading = true
      reviewService.createReviewRequest({
        UserId: this.currentUser.id,
        SubmissionId: this.submissionId,
        FeedbackType: 'Free',
        FeedbackLanguage: 'vn',
        Status: REVIEW_REQUEST_STATUS.REQUESTED
      }).then(rs => {
        this.dlVisible = false
        this.$notify.success({
          title: 'Yêu cầu đã được gửi đi',
          message: 'Yêu cầu nhận đánh giá từ học viên khác đã được gửi đi thành công. Chúng tôi sẽ thông báo cho bạn khi có phản hồi.',
          type: 'success',
          duration: 5000
        })
        this.loading = false
        this.selectedReview = ''
        this.$emit('reviewRequested')
      }).catch(rs => {
        // this.dlVisible = false
        this.selectedReview = ''
        this.loading = false
        this.$router.push('/reviews')
      })
    },
    requestAIReview() {
      this.loading = true
      reviewService.createAutomatedReview({
        UserId: this.currentUser.id,
        SubmissionId: this.submissionId,
        FeedbackType: 'AI',
        FeedbackLanguage: this.requestedLanguage ? this.requestedLanguage == 'Phản hồi bằng tiếng Việt' ? 'vn' : 'en' : this.feedbackLanguage == 'Phản hồi bằng tiếng Việt' ? 'vn' : 'en'
      }).then(rs => {
        this.loading = false
        this.dlVisible = false
        this.selectedReview = ''
        this.$emit('reviewRequested')
        const url = `/review/${rs.questionId}/${rs.docId}/${rs.reviewId}`
        window.location.href = url
      }).catch(rs => {
        // this.dlVisible = false
        this.loading = false
        this.$router.push('/reviews')
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

@media only screen and (max-width:  780px) {
  .el-page-header__left{
    margin-right: 25px !important;
  }
  .el-page-header__title{
    width: 40px;
    margin-top: 12px;
  }
  .el-page-header__left::after{
    right: -5px !important;
  }
  .el-page-header__content{
    word-break: break-word;
  }
}

</style>
<style scoped>

.review-card-mobile {
  display: inline-block;
  border: 1px solid #4a6f8a;
  border-radius: 4px;
  margin-top: 20px;
  padding: 20px;
  cursor: pointer
}
.review-card-mobile:hover {
  cursor: pointer;
  opacity: 0.9;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
}

.pricing-title-wrapper{
  height: 40px;
}

.pricing-option-icon{
  float: left;
  width: 50px;
}

.pricing-option-title{
  float: left;
  font-size: 18px;
  font-weight: 600;
  color: #4a6f8a;
  margin-top: 2px;
}

.pricing-option-description {
  word-break: break-word;
}

.pricing-option-price {
  font-size: 17px;
  font-weight: 500;
  color: #4a6f8a;
  margin-top: 10px;
  border: #4a6f8a dotted 1px;
  padding: 4px;
  padding-left: 9px;
  border-radius: 5px;
}

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
