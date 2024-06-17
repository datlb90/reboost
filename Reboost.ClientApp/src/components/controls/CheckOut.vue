<template>
  <el-dialog
    v-if="screenWidth > 780"
    id="practiceWritingCheckoutContainer"
    :visible.sync="dlVisible"
    width="600px"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 5px 20px; font-size: 18px; text-align:center;">Tuỳ Chọn Cho Phản Hồi</div>
    </div>
    <div class="dialog-body">
      <div id="feedbackConfigDialog" class="dialog-content" style="margin-top: 45px; margin-bottom: 50px;">
        <el-form
          ref="personalQuestionForm"
          label-width="160px"
        >
          <el-form-item v-if="currentUser && currentUser.id" size="medium" style="margin-bottom: 10px;">
            <label slot="label" style="font-size: 16px;">Phản hồi</label>
            <div v-if="!userSubscription">
              <el-badge :value="freeToken + ' free'" class="item" type="primary">
                <el-radio v-model="selectedType" style="margin-right: 5px;" label="detail" border :disabled="freeToken == 0">Chi tiết</el-radio>
              </el-badge>
              <el-badge :value="premiumToken + ' free'" class="item" type="primary">
                <el-radio v-model="selectedType" style="margin-right: 5px; margin-left: 30px;" label="deep" border :disabled="premiumToken == 0">Chuyên sâu</el-radio>
              </el-badge>
            </div>
            <div v-else>
              <el-radio v-model="selectedType" style="margin-right: 5px;" label="detail" border :disabled="userSubscription.planId >= 4">Chi tiết</el-radio>
              <el-radio v-model="selectedType" style="margin-right: 5px; margin-left: 10px;" label="deep" border :disabled="userSubscription.planId <= 3">Chuyên sâu</el-radio>
            </div>
          </el-form-item>

          <el-form-item size="medium" style="margin-bottom: 15px;">
            <label slot="label" style="font-size: 16px;">Ngôn ngữ</label>
            <el-radio v-model="selectedLanguage" style="margin-right: 5px;" label="vn" border>Phản hồi tiếng Việt</el-radio>
            <el-radio v-model="selectedLanguage" style="margin-right: 5px;" label="en" border>Phản hồi tiếng Anh</el-radio>
          </el-form-item>

          <el-form-item>
            <el-button size="medium" type="primary" :loading="loading" @click="requestAIReview()">Xác nhận</el-button>
            <el-button size="medium" @click="cancelRequest()">Huỷ</el-button>
          </el-form-item>
        </el-form>
      </div>
    </div>
  </el-dialog>

  <el-dialog
    v-else
    :visible.sync="dlVisible"
    :fullscreen="true"
    :style="'margin-top: 50px;'"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 0px 20px; font-size: 18px; text-align:center;">Tuỳ Chọn Cho Phản Hồi</div>
    </div>
    <div class="dialog-body">
      <div>

        <div id="feedbackConfigDialog" class="dialog-content" style="margin-top: 15px;">
          <el-form
            ref="personalQuestionForm"
          >
            <el-form-item v-if="currentUser && currentUser.id" size="medium" style="margin-bottom: 20px;">
              <label style="font-size: 16px;">Phản hồi</label>
              <div>
                <div v-if="!userSubscription">
                  <el-badge :value="freeToken + ' free'" class="item" type="primary">
                    <el-radio v-model="selectedType" style="margin-right: 5px;" label="detail" border :disabled="freeToken == 0">Chi tiết</el-radio>
                  </el-badge>
                  <el-badge :value="premiumToken + ' free'" class="item" type="primary">
                    <el-radio v-model="selectedType" style="margin-right: 5px; margin-left: 30px;" label="deep" border :disabled="premiumToken == 0">Chuyên sâu</el-radio>
                  </el-badge>
                </div>
                <div v-else>
                  <el-radio v-model="selectedType" style="margin-right: 5px;" label="detail" border :disabled="userSubscription.planId >= 4">Chi tiết</el-radio>
                  <el-radio v-model="selectedType" style="margin-right: 5px; margin-left: 10px;" label="deep" border :disabled="userSubscription.planId <= 3">Chuyên sâu</el-radio>
                </div>
              </div>
            </el-form-item>

            <el-form-item size="medium" style="margin-bottom: 30px;">
              <label style="font-size: 16px;">Ngôn ngữ</label>
              <div>
                <el-radio v-model="selectedLanguage" style="margin-right: 5px;" label="vn" border>Phản hồi tiếng Việt</el-radio>
                <el-radio v-model="selectedLanguage" style="margin-right: 5px;" label="en" border>Phản hồi tiếng Anh</el-radio>
              </div>
            </el-form-item>

            <el-form-item>
              <el-button size="medium" type="primary" :loading="loading" @click="requestAIReview()">Xác nhận</el-button>
              <el-button size="medium" @click="cancelRequest()">Huỷ</el-button>
            </el-form-item>
          </el-form>
        </div>
      </div>
    </div>
  </el-dialog>
</template>

<script>
import reviewService from '../../services/review.service'

export default {
  name: 'Checkout',
  props: {
    submissionId: { type: Number, default: null },
    questionId: { type: Number, default: null }
  },
  data() {
    return {
      dlVisible: false,
      card: null,
      stripe: null,
      clientSecret: null,
      checkoutDisable: true,
      newMethod: false,
      amount: 100000,
      paymentIntent: null,
      epaySelected: false,
      paid: false,
      loadingAutomatedReview: false,
      feedbackLanguage: 'Phản hồi bằng tiếng Việt',
      selectedLanguage: 'vn',
      specialRequest: null,
      screenWidth: window.innerWidth,
      loading: false,
      selectedType: 'detail',
      freeToken: this.$store.state.auth.user.freeToken,
      premiumToken: this.$store.state.auth.user.premiumToken,
      userSubscription: this.$store.state.auth.user.subscription
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
    openDialog() {
      this.freeToken = this.$store.state.auth.user.freeToken
      this.premiumToken = this.$store.state.auth.user.premiumToken
      this.userSubscription = this.$store.state.auth.user.subscription
      if (this.userSubscription && this.userSubscription.planId >= 4) {
        this.selectedType = 'deep'
      }
      this.dlVisible = true
      console.log(this.submissionId)
    },
    dialogClosed() {
      this.$emit('closed')
    },
    goBack() {
      this.selectedReview = ''
    },
    requestAIReview() {
      this.loading = true
      reviewService.createAutomatedReview({
        UserId: this.currentUser.id,
        SubmissionId: this.submissionId,
        FeedbackType: 'AI',
        FeedbackLanguage: this.selectedLanguage,
        ReviewType: this.selectedType
      }).then(rs => {
        this.loading = false
        this.dlVisible = false
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
  font-size: 35px;
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
