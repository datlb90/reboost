<template>
  <!-- Start Navbar Area -->
  <header id="header" :class="['headroom', {'is-sticky': isSticky}]">
    <div class="startp-nav">
      <div class="container">
        <nav class="navbar navbar-expand-md navbar-light">
          <router-link class="navbar-brand" to="/" href="banner" style="padding-top: 0px;">
            <img src="../../assets/logo/green_logo.png" alt="logo" style="width: 140px;">
          </router-link>

          <b-navbar-toggle target="navbarSupportedContent" />

          <b-collapse id="navbarSupportedContent" class="collapse navbar-collapse mean-menu" is-nav>
            <ul id="top-menu" class="navbar-nav nav ml-auto">

              <li class="nav-item">
                <a href="howItWorks" class="nav-link">How It Works</a>
              </li>

              <li class="nav-item">
                <a href="whyUs" class="nav-link">Why Us</a>
              </li>

              <li class="nav-item">
                <a href="features" class="nav-link">Features </a>
              </li>

              <li class="nav-item">
                <a href="testimony" class="nav-link">Testimony</a>
              </li>

              <li class="nav-item">
                <a href="blogs" class="nav-link">Blogs</a>
              </li>

              <li class="nav-item">
                <a href="faq" class="nav-link">FAQs</a>
              </li>
            </ul>
          </b-collapse>

          <div class="others-option">
            <a href="/rater" class="btn btn-light">Become A Rater</a>
            <a href="/test" class="btn btn-primary">Sign In</a>
          </div>
        </nav>
      </div>
    </div>
  </header>
  <!-- End Navbar Area -->
</template>

<script>
export default {
  name: 'LandingHeader',
  data() {
    return {
      isSticky: false
    }
  },

  mounted() {
    const that = this
    window.addEventListener('scroll', () => {
      const scrollPos = window.scrollY
      // eslint-disable-next-line no-console
      if (scrollPos >= 100) {
        that.isSticky = true
      } else {
        that.isSticky = false
      }
    })
  }
}
</script>

<style scoped>
.nav-item:active{
  color: #44ce6f;
}
</style>
