<template>
  <div id="app">
    <div v-if="$route.meta.isPricing">
      <div v-if="user.id">
        <Header />
        <router-view />
      </div>
      <div v-else>
        <LandingHeader />
        <router-view />
        <Footer />
      </div>
    </div>
    <div v-else>
      <div v-if="!$route.meta.plainLayout && $route.meta.landingPage">
        <LandingHeader />
        <router-view />
        <Footer />
      </div>
      <div v-if="!$route.meta.plainLayout && $route.meta.raterLanding">
        <RaterHeader />
        <router-view />
        <Footer />
      </div>
      <div v-if="!$route.meta.plainLayout && !$route.meta.landingPage && !$route.meta.raterLanding">
        <Header />
        <router-view />
      </div>
      <div v-if="$route.meta.plainLayout">
        <router-view />
      </div>
    </div>

  </div>
</template>

<script>
import Header from './components/layout/Header'
import LandingHeader from './components/layout/LandingHeader'
import RaterHeader from './components/layout/RaterHeader'
import Footer from './components/layout/Footer'
// import PreLoader from './components/layout/PreLoader'

export default {
  name: 'App',
  components: {
    Header, LandingHeader, RaterHeader, Footer
    // PreLoader
  },

  data() {
    return {
      user: this.$store.state.auth.user,
      // isLoading: true,
      currentUrl: ''
    }
  },

  watch: {
    '$route'(pathUrl) {
      this.currentUrl = pathUrl.path
      // this.isLoading = true
      // setTimeout(() => { this.isLoading = false }, 1500)
    }
  },
  mounted() {
    console.log(this.user)
    this.currentUrl = window.location.pathname
    var lang = localStorage.getItem('language')
    if (lang) {
      this.$ml.change(lang)
    } else {
      this.$ml.change('vietnamese')
    }
    // setTimeout(() => {
    //   this.isLoading = false
    // }, 2000)
  }
}
</script>

<style>
.el-notification {
  z-index: 999999 !important;
}
.el-dialog__header  {
  background: #ecf5ff;
  padding-bottom: 15px !important;
}
@media only screen and (max-width:  780px) {
  #header {
    position: fixed !important;
  }
  .list-container {
    padding-top: 50px;
  }
}

@media only screen and (min-width:  480px) {
  .nav-container {
    padding-left: 10px;
  }
  .list-container {
    padding-left: 5px;
  }
}

@media only screen and (min-width: 780px) {
  .list-container, .nav-container {
    max-width: 100%;
    margin: auto;
    padding: 0 10px;
  }
}

@media only screen and (min-width: 920px) {
  .list-container, .nav-container {
    max-width: 1200px;
    margin: auto;
  }
}

.list-container {
  margin-top: 5px;
}

.other-items {
  display: none;
}

@media only screen and (max-width: 768px) {
  .other-items {
    display: block;
  }
}

</style>
