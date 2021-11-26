<template>
  <!-- Start Navbar Area -->
  <div>
    <header
      id="header"
      :class="['headroom navbar-style-two', {'is-sticky': isSticky}]"
      style=" padding-top: 0px; padding-bottom: 0px; -webkit-box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06); box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06);
    position: relative; padding-right: 20px;z-index:1032"
    >
      <div class="startp-nav">
        <div class="container" style=" max-width: 100%;">
          <nav class="navbar navbar-expand-md navbar-light">
            <router-link class="navbar-brand" to="/" style="    margin-top: 0px; padding-top: 0px;">
              <img src="../../assets/logo/green_logo.png" alt="logo" style="position: absolute; height: 22px; top: 13px;left: 20px; ">
            </router-link>

            <b-navbar-toggle target="navbarSupportedContent" />

            <b-collapse id="navbarSupportedContent" class="collapse navbar-collapse mean-menu" is-nav>
              <ul v-if="role == 'Learner' && selectedTest.length>0" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
                <li class="nav-item">
                  <router-link to="/questions" class="nav-link">Questions</router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/submissions" class="nav-link">Submissions</router-link>
                </li>
                <!-- <li class="nav-item">
                <router-link to="/practice" class="nav-link">Practice</router-link>
              </li> -->

                <li class="nav-item">
                  <router-link to="/reviews" class="nav-link">Reviews</router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/disputes" class="nav-link">Disputes</router-link>
                </li>
                <li class="nav-item">
                  <a href="/questions" class="nav-link" @click.prevent="openContactDialog()">Contact</a>
                </li>
                <li class="nav-item">
                  <a href="/articles" class="nav-link">Articles</a>
                </li>
              <!-- <li class="nav-item">
                <router-link to="/revision" class="nav-link">Revision</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/dicuss" class="nav-link">Discuss</router-link>
              </li> -->
              </ul>
              <ul v-if="role == 'Learner' && selectedTest.length==0" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
                <li class="nav-item">
                  <router-link to="/SelectYourTest" class="nav-link">Select Your Test</router-link>
                </li>
              </ul>
              <ul v-if="role == 'Rater'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">

                <li class="nav-item">
                  <router-link to="/rater/apply" class="nav-link">Application</router-link>
                </li>
                <li v-if="isApprovedRater" class="nav-item">
                  <router-link to="/reviews" class="nav-link">Review</router-link>
                </li>
                <li v-if="isApprovedRater" class="nav-item">
                  <router-link to="/rater/payout" class="nav-link">Balance</router-link>
                </li>
                <li class="nav-item">
                  <a href="/" class="nav-link" @click.prevent="openContactDialog()">Contact</a>
                </li>
              <!-- <li class="nav-item">
                <router-link to="/rater/evaluate" class="nav-link" style="pointer-events: none; color: #b7b7b7;">Evaluate</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/dicuss" class="nav-link">Discuss</router-link>
              </li> -->
              </ul>
              <ul v-if="role == 'Admin'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">

                <li class="nav-item">
                  <router-link to="/admin/raters" class="nav-link">Raters</router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/admin/disputes" class="nav-link">Disputes</router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/admin/questions" class="nav-link">Questions</router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/admin/samples" class="nav-link">Samples</router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/admin/articles" class="nav-link">Articles</router-link>
                </li>
              <!-- <li class="nav-item">
                <router-link to="/admin/rubric" class="nav-link">Rubric</router-link>
              </li>
              <li class="nav-item">
                <router-link to="/admin/topics" class="nav-link">Topics</router-link>
              </li>
              <li class="nav-item">
                <router-link to="/admin/tips" class="nav-link">Tips & Guides</router-link>
              </li>
              <li class="nav-item">
                <router-link to="/admin/payments" class="nav-link">Payments</router-link>
              </li>
              <li class="nav-item">
                <router-link to="/admin/dispute" class="nav-link">Disputes</router-link>
              </li> -->
              </ul>
            </b-collapse>

            <div class="others-option">
              <el-dropdown style="margin-right: 20px" @command="onChangeLanguage">
                <el-button type="primary">
                  {{ lang }}
                  <i class="el-icon-arrow-down el-icon--right" />
                </el-button>
                <el-dropdown-menu slot="dropdown">
                  <el-dropdown-item command="english">English</el-dropdown-item>
                  <el-dropdown-item command="vietnamese">Vietnamese</el-dropdown-item>
                </el-dropdown-menu>
              </el-dropdown>
              <el-dropdown @command="handleCommand">
                <span class="el-dropdown-link">
                  <i class="far fa-user-circle" style="font-size: 24px;" />
                </span>
                <el-dropdown-menu slot="dropdown">
                  <div style="padding:0px 20px; display:inline-grid">
                    <span style="padding:5px 0px;font-weight:700;font-size:20px;">Hi, {{ displayName }}</span>
                    <span>Email: {{ currentUser.email }}</span>
                    <span>Role: {{ currentUser.role }}</span>
                    <span>Rating: {{ toFix(raterRating) }} <i class="fas fa-star" /></span>
                  </div>
                  <!-- <el-dropdown-item>Action 1</el-dropdown-item>
                <el-dropdown-item>Action 2</el-dropdown-item>
                <el-dropdown-item>Action 3</el-dropdown-item>
                <el-dropdown-item disabled>Action 4</el-dropdown-item> -->
                  <el-dropdown-item command="selectTest" divided>Selected test {{ testsToText() }}</el-dropdown-item>
                  <el-dropdown-item v-if="role == 'Learner'" command="addQuestion">Contribute a question</el-dropdown-item>
                  <el-dropdown-item command="logout" divided>Logout</el-dropdown-item>
                </el-dropdown-menu>
              </el-dropdown>
            <!-- <a href="#" class="btn btn-primary">Login</a> -->
            </div>
          </nav>

        </div>
      </div>
    </header>
    <add-edit-question ref="questionDialog" />
    <contact-dialog ref="contactDialog" />
  </div>
  <!-- End Navbar Area -->
</template>

<script>
import raterService from '../../services/rater.service'
import reviewService from '../../services/review.service'
import { RATER_STATUS } from '../../app.constant'
import AddEditQuestion from '../../components/controls/AddEditQuestion.vue'
import ContactDialog from '../../components/controls/ContactDialog.vue'

export default {
  name: 'HeaderTwo',
  components: {
    'add-edit-question': AddEditQuestion,
    'contact-dialog': ContactDialog
  },
  data() {
    return {
      role: this.$store.state.auth.user.role,
      isSticky: false,
      appInProgress: true,
      raterRating: 0,
      isApprovedRater: false,
      lang: ''
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
  mounted() {
    const that = this
    if (this.currentUser?.id) {
      this.$store.dispatch('auth/setSelectedTest')
    }
    console.log('current user', this.currentUser)
    this.checkApprovedRater()
    window.addEventListener('scroll', () => {
      const scrollPos = window.scrollY
      // eslint-disable-next-line no-console
      if (scrollPos >= 300) {
        that.isSticky = true
      } else {
        that.isSticky = false
      }
    })
    if (this.currentUser.id) {
      raterService.getRaterRating().then(rs => {
        this.raterRating = rs
      })
    }
    if (!this.currentUser.stripeCustomerId) {
      this.$store.dispatch('auth/loadCustomerId')
    }
    var _lang = localStorage.getItem('language')
    if (_lang) {
      this.lang = _lang.charAt(0).toUpperCase() + _lang.slice(1)
    } else {
      this.lang = 'English'
    }
  },
  created() {
    console.log(this.role)
    const t = process.env.VUE_APP_BASE_URL
    console.log('vue_app_const: ', t)
  },
  methods: {
    handleCommand(action) {
      if (action === 'logout') {
        this.$store.dispatch('auth/logout').then(rs => {
          this.$router.push('/')
        })
      } else if (action === 'selectTest') {
        this.$router.push('/SelectYourTest')
      } else if (action === 'addQuestion') {
        this.openAddQuestionDialog()
      }
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
      if (this.currentUser.role === 'Rater') {
        reviewService.raterApprovedCheck().then(r => {
          if (r && r.status === RATER_STATUS.APPROVED) {
            this.isApprovedRater = true
          }
        })
      }
    },
    openAddQuestionDialog() {
      this.$refs.questionDialog?.openDialog(true)
    },
    openContactDialog(e) {
      this.$refs.contactDialog?.openDialog()
    },
    onChangeLanguage(e) {
      this.lang = e.charAt(0).toUpperCase() + e.slice(1)
      localStorage.setItem('language', e)
      this.$ml.change(e)
    }
  }
}
</script>
