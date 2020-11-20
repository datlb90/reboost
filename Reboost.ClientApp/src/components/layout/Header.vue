<template>
  <!-- Start Navbar Area -->
  <header
    id="header"
    :class="['headroom navbar-style-two', {'is-sticky': isSticky}]"
    style=" padding-top: 0px; padding-bottom: 0px; -webkit-box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06); box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06);
    position: relative; padding-right: 20px;"
  >
    <div class="startp-nav">
      <div class="container" style=" max-width: 100%;">
        <nav class="navbar navbar-expand-md navbar-light">
          <router-link class="navbar-brand" to="/" style="    margin-top: 0px; padding-top: 0px;">
            <img src="../../assets/logo/green_logo.png" alt="logo" style="position: absolute; height: 22px; top: 13px;left: 20px; ">
          </router-link>

          <b-navbar-toggle target="navbarSupportedContent" />

          <b-collapse id="navbarSupportedContent" class="collapse navbar-collapse mean-menu" is-nav>
            <ul v-if="role == 'Learner'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">

              <li class="nav-item">
                <router-link to="/practice" class="nav-link active">Practice</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/review" class="nav-link">Review</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/revision" class="nav-link">Revision</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/dicuss" class="nav-link">Discuss</router-link>
              </li>
            </ul>
            <ul v-if="role == 'Rater'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">

              <li class="nav-item">
                <router-link to="/rater/apply" class="nav-link active">Application</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/rater/evaluate" class="nav-link" style="pointer-events: none; color: #b7b7b7;">Evaluate</router-link>
              </li>

              <li class="nav-item">
                <router-link to="/dicuss" class="nav-link">Discuss</router-link>
              </li>
            </ul>
            <ul v-if="role == 'Admin'" class="navbar-nav nav ml-auto" style="margin-left: 150px !important;">

              <li class="nav-item">
                <router-link to="/admin/raters" class="nav-link active">Raters</router-link>
              </li>
              <li class="nav-item">
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
              </li>
            </ul>
          </b-collapse>

          <div class="others-option">
            <el-dropdown>
              <span class="el-dropdown-link">
                <i class="far fa-user-circle" style="font-size: 24px;" />
              </span>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item>Action 1</el-dropdown-item>
                <el-dropdown-item>Action 2</el-dropdown-item>
                <el-dropdown-item>Action 3</el-dropdown-item>
                <el-dropdown-item disabled>Action 4</el-dropdown-item>
                <el-dropdown-item divided>Action 5</el-dropdown-item>
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
export default {
  name: 'HeaderTwo',
  data() {
    return {
      role: this.$store.state.auth.user.role,
      isSticky: false,
      appInProgress: true
    }
  },

  computed: {
    shoppingCart() {
      return this.$store.state.cart
    }
  },
  mounted() {
    const that = this
    window.addEventListener('scroll', () => {
      const scrollPos = window.scrollY
      // eslint-disable-next-line no-console
      if (scrollPos >= 300) {
        that.isSticky = true
      } else {
        that.isSticky = false
      }
    })
  },
  created() {
    console.log(this.$store.state.auth.user.role)
    console.log(this.role)
  }
}
</script>
