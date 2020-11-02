<template>
  <div id="app">
    <div v-if="!$route.meta.plainLayout">
      <HeaderTwo v-if="currentUrl == '/web-hosting'" />
      <HeaderThree v-else-if="currentUrl == '/machine-learning'" />
      <HeaderFour v-else-if="currentUrl == '/digital-agency'" />
      <div v-else-if="currentUrl == '/not-found' || currentUrl == '/coming-soon'" />
      <Header v-else />
      <PreLoader v-if="isLoading" />
      <router-view />
      <div v-if="currentUrl == '/not-found' || currentUrl == '/coming-soon'" />
      <Footer v-else />
    </div>
    <div v-else>
      <router-view />
    </div>

  </div>
</template>

<script>
import Header from './components/layout/Header'
import HeaderTwo from './components/layout/HeaderTwo'
import HeaderThree from './components/layout/HeaderThree'
import HeaderFour from './components/layout/HeaderFour'
import Footer from './components/layout/Footer'
import PreLoader from './components/layout/PreLoader'

export default {
  name: 'App',
  components: {
    Header, HeaderTwo, HeaderThree, HeaderFour, Footer, PreLoader
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
