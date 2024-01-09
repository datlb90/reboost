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

          <b-collapse id="navbarSupportedContent" class="collapse navbar-collapse mean-menu justify-content-center" is-nav>
            <ul id="top-menu" class="navbar-nav nav">

              <li class="nav-item">
                <a href="howItWorks" class="nav-link">How It Works</a>
              </li>

              <li class="nav-item">
                <a href="features" class="nav-link">Features </a>
              </li>

              <!-- <li class="nav-item">
                <a href="whyUs" class="nav-link">Why Us</a>
              </li> -->

              <!-- <li class="nav-item">
                <a href="testimony" class="nav-link">Testimony</a>
              </li>
              <li class="nav-item">
                <a href="blogs" class="nav-link">Blogs</a>
              </li>
              <li class="nav-item">
                <a href="faq" class="nav-link">FAQs</a>
              </li> -->
              <li class="nav-item">
                <a class="nav-link" @click.prevent="openContactDialog()">Contact Us</a>
              </li>
            </ul>
          </b-collapse>

          <div class="others-option">
            <el-dropdown class="lang-dropdown" style="margin-right: 20px" placement="bottom" @command="onChangeLanguage">
              <flag v-if="lang == 'English'" iso="gb" style="border-radius: 2px;" />
              <flag v-else iso="vn" style="border-radius: 2px;" />
              <el-dropdown-menu slot="dropdown" class="lang-dropdown-menu">
                <el-dropdown-item command="vietnamese"><flag iso="vn" style="border-radius: 2px; margin-right: 6px;" />Tiếng Việt</el-dropdown-item>
                <el-dropdown-item command="english"><flag iso="gb" style="border-radius: 2px; margin-right: 6px;" />English</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>

            <a href="/rater" class="btn btn-light">Become A Rater</a>
            <!-- <a href="/login" class="btn btn-primary">Sign In</a> -->
          </div>
        </nav>
      </div>
    </div>

    <contact-dialog ref="contactDialog" />
  </header>
  <!-- End Navbar Area -->
</template>

<script>
import ContactDialog from '../../components/controls/ContactDialog.vue'

export default {
  name: 'LandingHeader',
  components: {
    'contact-dialog': ContactDialog
  },
  data() {
    return {
      isSticky: false,
      lang: ''
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
  },
  methods: {
    openContactDialog(e) {
      this.$refs.contactDialog?.openDialog()
    },
    onChangeLanguage(e) {
      this.lang = e.charAt(0).toUpperCase() + e.slice(1)
      console.log(this.lang)
      localStorage.setItem('language', e)
      this.$ml.change(e)
    }
  }
}
</script>

<style scoped>
.nav-item:active{
  color: #44ce6f;
}

.lang-dropdown-menu{
  z-index: 10000 !important;
}
</style>
