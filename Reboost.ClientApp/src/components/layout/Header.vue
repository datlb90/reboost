<template>
  <!-- Start Navbar Area -->
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
              <li class="nav-item">
                <router-link to="/reviews" class="nav-link">Review</router-link>
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
                <router-link to="/admin/raters" class="nav-link active">Raters</router-link>
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
            <el-dropdown @command="handleCommand">
              <span class="el-dropdown-link">
                <i class="far fa-user-circle" style="font-size: 24px;" />
              </span>
              <el-dropdown-menu slot="dropdown">
                <div style="padding:0 10px; display:inline-grid">
                  <span style="padding:5px 0px;font-weight:700;font-size:20px;">{{ currentUser.username }}</span>
                  <span>Email: {{ currentUser.email }}</span>
                  <span>Role: {{ currentUser.role }}</span>
                  <span>Rating: {{ toFix(raterRating) }} <i class="fas fa-star" /></span>
                </div>
                <!-- <el-dropdown-item>Action 1</el-dropdown-item>
                <el-dropdown-item>Action 2</el-dropdown-item>
                <el-dropdown-item>Action 3</el-dropdown-item>
                <el-dropdown-item disabled>Action 4</el-dropdown-item> -->
                <el-dropdown-item command="selectTest" divided>Select your test: {{ testsToText() }}</el-dropdown-item>
                <el-dropdown-item command="logout" divided>Logout</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
            <!-- <a href="#" class="btn btn-primary">Login</a> -->
          </div>
        </nav>
      </div>
    </div>
  </header>
  <!-- End Navbar Area -->
</template>

<script>
import raterService from '../../services/rater.service'
export default {
  name: 'HeaderTwo',
  data() {
    return {
      role: this.$store.state.auth.user.role,
      isSticky: false,
      appInProgress: true,
      raterRating: 0
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
    }
  },
  mounted() {
    const that = this
    if (this.currentUser?.id) {
      this.$store.dispatch('auth/setSelectedTest')
    }
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
  },
  created() {
    console.log(this.role)
  },
  methods: {
    handleCommand(action) {
      if (action === 'logout') {
        this.$store.dispatch('auth/logout').then(rs => {
          this.$router.push('/login')
        })
      } else if (action === 'selectTest') {
        this.$router.push('/SelectYourTest')
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
    }
  }
}
</script>
