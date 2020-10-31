import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import 'element-ui/lib/theme-chalk/index.css'
import Element from 'element-ui'
import locale from 'element-ui/lib/locale/lang/en'
import '@fortawesome/fontawesome-free/css/all.css'
import '@fortawesome/fontawesome-free/js/all.js'
import 'nprogress/nprogress.css'
import FBSignInButton from 'vue-facebook-signin-button'
import GSignInButton from 'vue-google-signin-button'
import VueCookies from 'vue-cookies'

Vue.use(VueCookies)
Vue.use(FBSignInButton)
Vue.use(GSignInButton)

// import './permission';

Vue.use(Element, { locale })
Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
