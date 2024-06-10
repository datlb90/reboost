<template>
  <header
    v-if="screenWidth > 850"
    id="header"
    :class="['headroom navbar-style-two', {'is-sticky': isSticky}]"
    style="position: static;"
  >
    <div class="startp-nav">
      <div class="nav-container" :class="{ 'full-size': fullSizeHeader }">
        <nav class="navbar navbar-expand-md navbar-light">
          <router-link v-if="selectedTest && selectedTest.length > 0" to="/questions" style="margin-top: 0px; padding-top: 0px;">
            <img src="../../assets/logo/logo.png" alt="logo" class="main-header-logo">
          </router-link>
          <router-link v-else to="/select/test" style="margin-top: 0px; padding-top: 0px;">
            <img src="../../assets/logo/logo.png" alt="logo" class="main-header-logo">
          </router-link>
          <b-navbar-toggle target="navbarSupportedContent" />
          <b-collapse id="navbarSupportedContent" class="collapse navbar-collapse mean-menu" is-nav>
            <ul v-if="role == userRole.LEARNER" class="navbar-nav nav mr-auto nav-wrapper">
              <li v-if="selectedTest && selectedTest.length > 0" class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/questions" class="nav-link">Chủ đề viết</router-link>
              </li>
              <li v-if="selectedTest && selectedTest.length > 0" class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/submissions" class="nav-link">Bài viết của tôi</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <a href="#" class="nav-link" @click.prevent="gotoPricing()">Bảng giá</a>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <a href="/" class="nav-link" @click.prevent="openContactDialog()">Liên hệ</a>
              </li>
            </ul>
            <ul v-if="role == userRole.ADMIN" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/admin/raters" class="nav-link">Raters</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/admin/requests" class="nav-link">Requests</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/admin/questions" class="nav-link">Questions</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/admin/samples" class="nav-link">Samples</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/admin/articles" class="nav-link">Articles</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/admin/disputes" class="nav-link">Disputes</router-link>
              </li>
            </ul>
          </b-collapse>

          <div class="user-option">
            <el-button
              v-if="role == userRole.LEARNER"
              icon="el-icon-edit"
              class="btn btn-gradient"
              style="margin-right: 20px; padding: 6px 20px; font-size: 12px;"
              @click="openRequestReviewDialog"
            >Nhận phản hồi cho bài viết
            </el-button>

            <el-dropdown style="margin-top: 5px; margin-right: 2px;" trigger="click">
              <span class="el-dropdown-link" @click="getSubscription()">
                <el-link :underline="false" type="info">
                  <i class="far fa-user-circle" style="font-size: 24px;" />
                </el-link>
              </span>
              <el-dropdown-menu slot="dropdown">
                <div style="padding:0px 20px; margin-bottom: 0px; display:inline-grid">
                  <span style="padding:5px 0px; font-weight:500; font-size:15px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap; max-width: 200px;">
                    {{ displayName }}
                  </span>
                  <div style="font-size:14px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;">
                    {{ currentUser.email }}
                  </div>
                </div>
                <el-dropdown-item divided>
                  <div>
                    <div v-if="subscriptionName">
                      {{ subscriptionName }}
                      <i class="fas fa-star" :style="subscriptionName == 'Gói luyện tập cơ bản' ? 'color: #a5a5a5; vertical-align: -1px;' : 'color: gold; vertical-align: -1px;'" />
                    </div>
                    <div v-if="subscriptionName == 'Gói luyện tập cơ bản'">
                      Bài chấm miễn phí: {{ parseInt(freeToken) + parseInt(premiumToken) }}
                    </div>
                    <div v-else-if="!isExpired">
                      Ngày hết hạn: {{ expiredDate }}
                    </div>
                    <div v-else>Đã hết hạn</div>
                  </div>
                </el-dropdown-item>
                <el-dropdown-item divided>
                  <div @click="logout()"> Đăng xuất</div>
                </el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
          </div>
        </nav>

      </div>
    </div>

    <contact-dialog ref="contactDialog" />
    <review-request-dialog ref="reviewRequestDialog" @openCheckoutDialog="openCheckoutDialog" />
    <checkout-dialog
      ref="checkoutDialog"
      :question-id="questionId"
      :submission-id="submissionId"
      :requested-language="feedbackLanguage"
    />
  </header>

  <header
    v-else
    id="header"
    :class="['headroom navbar-style-two', {'is-sticky': isSticky}]"
    style="position: static;"
  >
    <div class="startp-nav">
      <div class="nav-container" :class="{ 'full-size': fullSizeHeader }" style="height: 50px;">
        <router-link v-if="selectedTest && selectedTest.length > 0" to="/questions" style="margin-top: 4px; margin-left: 2px;">
          <img src="../../assets/logo/logo.png" alt="logo" class="main-header-logo">
        </router-link>
        <router-link v-else to="/select/test" style="margin-top: 0px; padding-top: 0px;">
          <img src="../../assets/logo/logo.png" alt="logo" class="main-header-logo">
        </router-link>

        <el-dropdown style="float: right; margin-top: 5px; margin-right: 5px;" trigger="click">
          <span class="el-dropdown-link" style="cursor: pointer;" @click="getSubscription()">
            <i class="el-icon-menu" style="font-size: 37px; margin-top: 2px;" />
          </span>

          <el-dropdown-menu id="top-menu" slot="dropdown">
            <el-dropdown-item command="a">
              <a style="font-size: 16px; font-weight: 500; cursor: pointer; margin-left: 15px; margin-right: 15px;" href="/questions">
                Chủ đề viết
              </a>
            </el-dropdown-item>
            <el-dropdown-item command="b">
              <a style="font-size: 16px;  font-weight: 500; cursor: pointer; margin-left: 15px; margin-right: 15px;" href="/submissions">
                Bài viết của tôi
              </a>
            </el-dropdown-item>
            <el-dropdown-item command="c">
              <a style="font-size: 16px;  font-weight: 500; cursor: pointer; margin-left: 15px; margin-right: 15px;" href="/pricing">
                Bảng giá
              </a>
            </el-dropdown-item>
            <el-dropdown-item command="d">
              <a style="font-size: 16px;  font-weight: 500; cursor: pointer; margin-left: 15px; margin-right: 15px; color: #4a6f8a;" @click.prevent="openContactDialog()">
                Liên hệ
              </a>
            </el-dropdown-item>
            <el-dropdown-item divided>
              <a style="font-size: 16px;  font-weight: 500; cursor: pointer; margin-left: 15px; margin-right: 15px;">
                {{ displayName }}
              </a>
            </el-dropdown-item>
            <el-dropdown-item>
              <a style="font-size: 15px;  font-weight: 400; cursor: pointer; margin-left: 15px; margin-right: 15px;">
                {{ currentUser.email }}
              </a>
            </el-dropdown-item>
            <el-dropdown-item divided>
              <div>
                <div v-if="subscriptionName" style="font-size: 15px;  font-weight: 400; cursor: pointer; margin-left: 15px; margin-right: 15px;">
                  {{ subscriptionName }}
                  <i class="fas fa-star" :style="subscriptionName == 'Gói luyện tập cơ bản' ? 'color: #a5a5a5; vertical-align: -1px;' : 'color: gold; vertical-align: -1px;'" />
                </div>
                <div v-if="subscriptionName == 'Gói luyện tập cơ bản'" style="font-size: 15px; font-weight: 400; cursor: pointer; margin-left: 15px; margin-right: 15px;">
                  Bài chấm miễn phí: {{ parseInt(freeToken) + parseInt(premiumToken) }}
                </div>
                <div v-else-if="!isExpired" style="font-size: 15px;  font-weight: 400; cursor: pointer; margin-left: 15px; margin-right: 15px;">
                  Ngày hết hạn: {{ expiredDate }}
                </div>
                <div v-else style="font-size: 15px;  font-weight: 400; cursor: pointer; margin-left: 15px; margin-right: 15px;">Đã hết hạn</div>
              </div>
            </el-dropdown-item>
            <el-dropdown-item divided>
              <div style="font-size: 16px;  font-weight: 500; cursor: pointer; margin-left: 15px; margin-right: 15px;" @click="logout()"> Đăng xuất</div>
            </el-dropdown-item>
          </el-dropdown-menu>

        </el-dropdown>

        <div class="menu-btns" style="float: right; margin-right: 5px; margin-top: 10px;">
          <el-button
            v-if="role == userRole.LEARNER"
            class="btn btn-gradient"
            style="margin-right: 5px; padding: 6px 20px; font-size: 12px;"
            @click="openRequestReviewDialog"
          >Nhận phản hồi cho bài viết
          </el-button>
        </div>
      </div>
    </div>

    <contact-dialog ref="contactDialog" />
    <review-request-dialog ref="reviewRequestDialog" @openCheckoutDialog="openCheckoutDialog" />
    <checkout-dialog
      ref="checkoutDialog"
      :question-id="questionId"
      :submission-id="submissionId"
      :requested-language="feedbackLanguage"
    />
  </header>

  <!-- Start Navbar Area -->
  <!-- <div class="header-container" /> -->
  <!-- End Navbar Area -->
</template>

<script>
import raterService from '../../services/rater.service'
import reviewService from '../../services/review.service'
import { RATER_STATUS } from '../../app.constant'
import ContactDialog from '../../components/controls/ContactDialog.vue'
import ReviewRequestDialog from '../../components/controls/ReviewRequestDialog.vue'
import CheckoutDialog from '../../components/controls/CheckOut.vue'
import { PageName, UserRole } from '@/app.constant'
export default {
  name: 'HeaderTwo',
  components: {
    'contact-dialog': ContactDialog,
    'review-request-dialog': ReviewRequestDialog,
    'checkout-dialog': CheckoutDialog
  },
  data() {
    return {
      role: this.$store.state.auth.user.role,
      freeToken: this.$store.state.auth.user.freeToken,
      premiumToken: this.$store.state.auth.user.premiumToken,
      userSubscription: null,
      isSticky: false,
      appInProgress: true,
      raterRating: 0,
      isApprovedRater: false,
      lang: '',
      questionsPage: PageName.QUESTIONS,
      submissionsPage: PageName.SUBMISSIONS,
      reviewsPage: PageName.REVIEWS,
      fullSizeHeader: false,
      userRole: UserRole,
      questionId: null,
      submissionId: null,
      initialSubmission: null,
      screenWidth: window.innerWidth,
      feedbackLanguage: 'Phản hồi bằng tiếng Việt',
      subscriptionName: null,
      isExpired: false,
      expiredDate: null
    }
  },
  computed: {
    shoppingCart() {
      return this.$store.state.cart
    },
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    selectedTest() {
      return this.$store.getters['auth/getSelectedTest']
    },
    displayName() {
      return this.currentUser.firstName ?? this.currentUser.username
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    },
    $route(to, from) {
      if (to.name == 'PracticeWriting' || to.name == 'Review') {
        this.fullSizeHeader = true
      } else {
        this.fullSizeHeader = false
      }
    }
  },
  mounted() {
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    var _lang = localStorage.getItem('language')
    if (_lang) {
      this.lang = _lang.charAt(0).toUpperCase() + _lang.slice(1)
    } else {
      this.lang = 'English'
    }
    this.checkApprovedRater()
    // this.waitForFeedbackDialogVisible = true
  },
  created() {
    if (this.$router.currentRoute.name == 'PracticeWriting' || this.$router.currentRoute.name == 'Review') {
      this.fullSizeHeader = true
    } else {
      this.fullSizeHeader = false
    }
  },
  methods: {
   async getSubscription() {
      this.freeToken = this.$store.state.auth.user.freeToken
      this.premiumToken = this.$store.state.auth.user.premiumToken
      const userSubscription = this.$store.state.auth.user.subscription
      if (userSubscription) {
        if (userSubscription.planId <= 3) { this.subscriptionName = 'Gói phản hồi chi tiết' } else { this.subscriptionName = 'Gói phản hồi chuyên sâu' }
        this.isExpired = new Date(userSubscription.endDate) < new Date()
        this.expiredDate = new Date(userSubscription.endDate).toLocaleDateString('vi-VN')
      } else {
        this.subscriptionName = 'Gói luyện tập cơ bản'
      }
      console.log(this.subscriptionName)
    },
    gotoPricing() {
      window.location.href = '/pricing'
    },
    getRaterRating() {
      if (this.currentUser.id) {
        console.log('get user rating')
        raterService.getRaterRating().then(rs => {
          this.raterRating = rs
        })
      }
    },
    handleCommand(action) {
      if (action === 'logout') {
        this.logout()
      } else if (action === 'selectTest') {
        this.selectTest()
      } else if (action === 'addQuestion') {
        this.openAddQuestionDialog()
      }
    },
    logout() {
      var menu = document.getElementById('navbarSupportedContent')
      if (menu) {
        menu.style.display = ' none'
      }
      this.$store.dispatch('auth/logout').then(rs => {
        window.location.href = '/'
      })
    },
    selectTest() {
      var menu = document.getElementById('navbarSupportedContent')
      if (menu) {
        menu.style.display = ' none'
      }
      this.$router.push('/select/test')
    },
    toFix(number) {
      return Number((number).toFixed(1))
    },
    testsToText() {
      var rs = ''
      if (this.selectedTest.length > 0) {
        if (this.selectedTest.length == 2) {
          rs = 'TOEFL & IELTS'
        } else {
          rs = this.selectedTest[0].toUpperCase()
        }
      }
      return rs
    },
    checkApprovedRater() {
      if (this.currentUser.role === UserRole.RATER) {
        reviewService.raterApprovedCheck().then(r => {
          if (r && r.status === RATER_STATUS.APPROVED) {
            this.isApprovedRater = true
          }
        })
      }
    },
    openAddQuestionDialog() {
      var menu = document.getElementById('navbarSupportedContent')
      if (menu) {
        menu.style.display = ' none'
      }
      this.$refs.questionDialog?.openDialog(true)
    },
    openContactDialog(e) {
      this.$refs.contactDialog?.openDialog()
    },
    openCheckoutDialog({questionId, submissionId, feedbackLanguage}) {
      this.questionId = questionId
      this.submissionId = submissionId
      this.feedbackLanguage = feedbackLanguage == 'vn' ? 'Phản hồi bằng tiếng Việt' : 'Phản hồi bằng tiếng Anh'
      this.$refs.checkoutDialog?.openDialog()
    },
    openRequestReviewDialog() {
      this.freeToken = this.$store.state.auth.user.freeToken
      this.premiumToken = this.$store.state.auth.user.premiumToken
      // Check user token & subscription
      if (this.freeToken > 0 || this.premiumToken > 0 || (this.userSubscription && new Date(this.userSubscription.endDate) > new Date())) {
        this.$refs.reviewRequestDialog?.openDialog()
      } else {
        window.location.href = '/pricing'
      }
    },
    onChangeLanguage(e) {
      this.lang = e.charAt(0).toUpperCase() + e.slice(1)
      localStorage.setItem('language', e)
      this.$ml.change(e)
    }
  }
}
</script>

<style>
.el-message-box__wrapper{
  z-index: 9999 !important;
}
</style>
<style scoped>
.lang-dropdown-menu{
  z-index: 10000 !important;
}

.header-container, .navbar{
  height: 50px;
}

.main-header-logo{
  height: 35px;
  margin-top: 5px;
}

.nav-wrapper {
  margin-left: 20px;
  margin-top: 2px;
}

.header-notification {
    border: none;
    font-size: 19px;
    padding: 5px !important;
    position: absolute;
    right: 45px;
    top: 12px;
    color: #909399;
}
.full-size {
  max-width: 100%;
  padding: 0 10px;
}
</style>
