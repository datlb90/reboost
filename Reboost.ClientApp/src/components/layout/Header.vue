<template>
  <header
    id="header"
    :class="['headroom navbar-style-two', {'is-sticky': isSticky}]"
    style="position: static;"
  >
    <div class="startp-nav">
      <div class="nav-container" :class="{ 'full-size': fullSizeHeader }">
        <nav class="navbar navbar-expand-md navbar-light">
          <router-link to="/questions" style="margin-top: 0px; padding-top: 0px;">
            <img src="../../assets/logo/logo.png" alt="logo" class="main-header-logo">
          </router-link>
          <b-navbar-toggle target="navbarSupportedContent" />
          <b-collapse id="navbarSupportedContent" class="collapse navbar-collapse mean-menu" is-nav>
            <ul v-if="role == userRole.LEARNER" class="navbar-nav nav mr-auto nav-wrapper">
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/questions" class="nav-link">Questions</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/submissions" class="nav-link">Submissions</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/reviews" class="nav-link">Reviews</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <a href="/" class="nav-link" @click.prevent="openContactDialog()">Contact</a>
              </li>

              <li class="nav-item other-items" style="padding-bottom: 12px;">
                <a href="/" style="color: #4a6f8a;">
                  <el-button
                    icon="el-icon-edit"
                    class="btn btn-gradient"
                    style="margin-right: 20px; padding: 6px 20px; font-size: 12px;"
                    @click="openRequestReviewDialog"
                  >Request Review
                  </el-button>
                  <div style=" display: inline-grid; padding-top: 10px; width: 420px;">
                    <div style="padding:12px 0px;  text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;">Hi, {{ displayName }}</div>
                    <div style="padding:12px 0px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;">Review Rating: {{ toFix(raterRating) }} <i class="fas fa-star" style="color: gold; vertical-align: -1px;" /></div>
                    <div style="padding:12px 0px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;" @click.prevent="selectTest()">
                      Selected Test: {{ testsToText() }}
                    </div>
                    <div style="padding:12px 0px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;" @click.prevent="openAddQuestionDialog()">Contribute Question</div>
                    <div style="padding:12px 0px;text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;" @click.prevent="logout()">Logout</div>
                  </div>
                </a>
              </li>
            </ul>
            <!-- <ul v-if="role == UserRole.LEARNER && selectedTest.length==0" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
                <li class="nav-item">
                  <router-link to="/SelectYourTest" class="nav-link">Select Your Test</router-link>
                </li>
              </ul> -->
            <ul v-if="role == userRole.RATER" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/reviews" class="nav-link">Reviews</router-link>
              </li>
              <li class="nav-item" style="padding-bottom: 12px;">
                <router-link to="/rater/apply" class="nav-link">Application</router-link>
              </li>
              <li v-if="isApprovedRater" class="nav-item" style="padding-bottom: 12px;">
                <a :href="$router.resolve({name: reviewsPage}).href">Reviews</a>
              </li>
              <li class="nav-item">
                <a href="/" class="nav-link" @click.prevent="openContactDialog()">Contact</a>
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

            <!-- <el-button type="primary" icon="el-icon-edit" size="small" style="margin-right: 20px;" @click="openRequestReviewDialog">Request Review</el-button> -->

            <el-button
              icon="el-icon-edit"
              class="btn btn-gradient"
              style="margin-right: 20px; padding: 6px 20px; font-size: 12px;"
              @click="openRequestReviewDialog"
            >Request Review
            </el-button>

            <!-- <el-button
              class="header-notification"
              icon="el-icon-message-solid"
              circle
            /> -->
            <el-dropdown style="margin-top: 5px; margin-right: 2px;" trigger="click" @command="handleCommand">
              <span class="el-dropdown-link" @click="getRaterRating">
                <el-link :underline="false" type="info">
                  <i class="far fa-user-circle" style="font-size: 24px;" />
                </el-link>
              </span>
              <el-dropdown-menu slot="dropdown">
                <div style="padding:0px 20px; margin-bottom: 10px; display:inline-grid">
                  <span style="padding:5px 0px;font-weight:700;font-size:15px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap; max-width: 200px;">Hi, {{ displayName }}</span>
                  <span>Band score:  Undefined</span>
                  <span>Review Rating: {{ toFix(raterRating) }} <i class="fas fa-star" style="color: gold; vertical-align: -1px;" /></span>
                </div>

                <div style="padding:0px 20px; border-top: 1px solid #EBEEF5; padding-top: 12px; padding-bottom: 6px;">
                  <el-dropdown class="lang-dropdown" trigger="click" placement="left" @command="onChangeLanguage">
                    <span v-if="lang == 'English'">Language: English <flag iso="gb" style="border-radius: 3px; margin-left: 2px;" /></span>
                    <span v-else>Ngôn Ngữ: Tiếng Việt <flag iso="vn" style="border-radius: 3px; margin-left: 2px;" /></span>
                    <el-dropdown-menu slot="dropdown" class="lang-dropdown-menu">
                      <el-dropdown-item command="vietnamese"><flag iso="vn" style="border-radius: 3px; margin-right: 6px;" />Tiếng Việt</el-dropdown-item>
                      <el-dropdown-item command="english"><flag iso="gb" style="border-radius: 3px; margin-right: 6px;" />English</el-dropdown-item>
                    </el-dropdown-menu>
                  </el-dropdown>
                </div>
                <el-dropdown-item command="selectTest" divided>Selected Test: {{ testsToText() }}</el-dropdown-item>
                <el-dropdown-item command="addQuestion">Contribute Question</el-dropdown-item>
                <el-dropdown-item command="logout" divided>Logout</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
          </div>
        </nav>

      </div>
    </div>

    <add-edit-question ref="questionDialog" />
    <contact-dialog ref="contactDialog" />
    <review-request-dialog ref="reviewRequestDialog" @openCheckoutDialog="openCheckoutDialog" />
    <checkout-dialog
      ref="checkoutDialog"
      :question-id="questionId"
      :submission-id="submissionId"
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
import AddEditQuestion from '../../components/controls/AddEditQuestion.vue'
import ContactDialog from '../../components/controls/ContactDialog.vue'
import ReviewRequestDialog from '../../components/controls/ReviewRequestDialog.vue'
import CheckoutDialog from '../../components/controls/CheckOut.vue'
import { PageName, UserRole } from '@/app.constant'
export default {
  name: 'HeaderTwo',
  components: {
    'add-edit-question': AddEditQuestion,
    'contact-dialog': ContactDialog,
    'review-request-dialog': ReviewRequestDialog,
    'checkout-dialog': CheckoutDialog
  },
  data() {
    return {
      role: this.$store.state.auth.user.role,
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
      submissionId: null
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
    $route(to, from) {
      if (to.name == 'PracticeWriting' || to.name == 'Review') {
        this.fullSizeHeader = true
      } else {
        this.fullSizeHeader = false
      }
    }
  },
  mounted() {
    var _lang = localStorage.getItem('language')
    if (_lang) {
      this.lang = _lang.charAt(0).toUpperCase() + _lang.slice(1)
    } else {
      this.lang = 'English'
    }
    // Check if user is requesting a review
    // Open the request review dialog
    this.personalQuestion = this.$store.getters['question/getPersonalQuestion']
    if (this.personalQuestion) {
      this.$refs.reviewRequestDialog?.openDialog()
    }
  },
  created() {
    if (this.$router.currentRoute.name == 'PracticeWriting' || this.$router.currentRoute.name == 'Review') {
      this.fullSizeHeader = true
    } else {
      this.fullSizeHeader = false
    }
  },
  methods: {
    getRaterRating() {
      if (this.currentUser.id) {
        raterService.getRaterRating().then(rs => {
          console.log('User Rating: ', rs)
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
        this.$router.push('/')
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
    openCheckoutDialog({questionId, submissionId}) {
      this.questionId = questionId
      this.submissionId = submissionId
      this.$refs.checkoutDialog?.openDialog()
    },
    openRequestReviewDialog() {
      this.$refs.reviewRequestDialog?.openDialog()
    },
    onChangeLanguage(e) {
      this.lang = e.charAt(0).toUpperCase() + e.slice(1)
      localStorage.setItem('language', e)
      this.$ml.change(e)
    }
  }
}
</script>

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
