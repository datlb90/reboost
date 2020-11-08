<template>
  <div id="app">
    <div v-if="!$route.meta.plainLayout && $route.meta.landingPage">
      <LandingHeader />
      <PreLoader v-if="isLoading" />
      <router-view />
      <Footer />
    </div>
    <div v-if="!$route.meta.plainLayout && $route.meta.raterLanding">
      <RaterHeader />
      <PreLoader v-if="isLoading" />
      <router-view />
      <Footer />
    </div>
    <div v-if="!$route.meta.plainLayout && !$route.meta.landingPage && !$route.meta.raterLanding">
      <Header />
      <router-view />
    </div>

  </div>
</template>

<script>
import Header from './components/layout/Header'
import LandingHeader from './components/layout/LandingHeader'
import RaterHeader from './components/layout/RaterHeader'
import Footer from './components/layout/Footer'
import PreLoader from './components/layout/PreLoader'

export default {
  name: 'App',
  components: {
    Header, LandingHeader, RaterHeader, Footer, PreLoader
  },

  data() {
    return {
      isLoading: true,
      currentUrl: ''
    }
  },

  watch: {
    '$route'(pathUrl) {
      this.currentUrl = pathUrl.path
      this.isLoading = true
      setTimeout(() => { this.isLoading = false }, 1500)
    }
  },

  mounted() {
    this.currentUrl = window.location.pathname
    console.log(this.currentUrl)
    setTimeout(() => {
      this.isLoading = false
    }, 2000)
  }
}
</script>
