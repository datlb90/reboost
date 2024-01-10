<template>
  <!-- Start Navbar Area -->
  <div class="header-container">
    <header
      id="header"
      :class="['headroom navbar-style-two', {'is-sticky': isSticky}]"
    >
      <div class="startp-nav">
        <div class="nav-container" :class="{ 'full-size': fullSizeHeader }">
          <nav class="navbar navbar-expand-md navbar-light">
            <router-link class="navbar-brand" to="/questions" style="margin-top: 0px; padding-top: 0px;">
              <img src="../../assets/logo/green_logo.png" alt="logo" class="main-header-logo">
            </router-link>
            <b-navbar-toggle target="navbarSupportedContent" />
            <b-collapse id="navbarSupportedContent" class="collapse navbar-collapse mean-menu" is-nav>
              <ul v-if="role == 'Learner' && selectedTest.length>0" class="navbar-nav nav mr-auto nav-wrapper">
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
                    <div style=" display: inline-grid; padding-top: 10px; border-top: #d2d2d2 solid 1px; border-top: 1px solid rgb(210, 210, 210); width: 420px;">
                      <div style="padding:5px 0px;font-weight:700;font-size:15px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;">Hi, {{ displayName }}</div>

                      <div>Review Rating: {{ toFix(raterRating) }} <i class="fas fa-star" style="color: gold; vertical-align: -1px;" /></div>
                      <div style="text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap;" @click.prevent="selectTest()">
                        Selected Test: {{ testsToText() }}
                      </div>
                      <div @click.prevent="openAddQuestionDialog()">Contribute Question</div>
                      <div @click.prevent="logout()">Logout</div>
                    </div>
                  </a>
                </li>

              </ul>
              <ul v-if="role == 'Learner' && selectedTest.length==0" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
                <li class="nav-item">
                  <router-link to="/SelectYourTest" class="nav-link">Select Your Test</router-link>
                </li>
              </ul>
              <ul v-if="role == 'Rater'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
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
              <ul v-if="role == 'Admin'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
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

            <!-- <div>
              <ul v-if="role == 'Learner' && selectedTest.length>0" class="navbar-nav nav mr-auto nav-wrapper">
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
              </ul>
              <ul v-if="role == 'Learner' && selectedTest.length==0" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
                <li class="nav-item">
                  <router-link to="/SelectYourTest" class="nav-link">Select Your Test</router-link>
                </li>
              </ul>
              <ul v-if="role == 'Rater'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
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
              <ul v-if="role == 'Admin'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">
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
            </div> -->

            <div class="user-option">

              <el-button
                class="header-notification"
                icon="el-icon-message-solid"
                circle
              />

              <el-dropdown style="margin-top: 4px; margin-right: 2px;" trigger="click" @command="handleCommand">
                <span class="el-dropdown-link" @click="getRaterRating">
                  <el-link :underline="false" type="info">
                    <i class="far fa-user-circle" style="font-size: 24px;" />
                  </el-link>
                </span>
                <el-dropdown-menu slot="dropdown">
                  <div style="padding:0px 20px; margin-bottom: 10px; display:inline-grid">
                    <span style="padding:5px 0px;font-weight:700;font-size:15px; text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap; max-width: 200px;">Hi, {{ displayName }}</span>
                    <span style="text-overflow: ellipsis; word-break: break-word; overflow: hidden; white-space: nowrap; max-width: 200px;">
                      Email: {{ currentUser.email }}
                    </span>
                    <!-- <span>Role: {{ currentUser.role }}</span> -->
                    <!-- <span>TOEFL Score: 20/30</span>
                    <span>IELTS Score: 7.0/9.0</span> -->
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
import { PageName } from '@/app.constant'
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
      lang: '',
      questionsPage: PageName.QUESTIONS,
      submissionsPage: PageName.SUBMISSIONS,
      reviewsPage: PageName.REVIEWS,
      fullSizeHeader: false
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
    const that = this
    // if (this.currentUser?.id) {
    //   this.$store.dispatch('auth/setSelectedTest')
    // }
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

    var _lang = localStorage.getItem('language')
    if (_lang) {
      this.lang = _lang.charAt(0).toUpperCase() + _lang.slice(1)
    } else {
      this.lang = 'English'
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
      this.$router.push('/SelectYourTest')
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
      var menu = document.getElementById('navbarSupportedContent')
      if (menu) {
        menu.style.display = ' none'
      }
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

<style scoped>
.lang-dropdown-menu{
  z-index: 10000 !important;
}

.header-container, .navbar{
  height: 50px;
}

.main-header-logo{
  height: 22px;
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
