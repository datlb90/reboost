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

import { BootstrapVue } from 'bootstrap-vue'
import VueFeather from 'vue-feather'
import vWow from 'v-wow'
import VueCarousel from 'vue-carousel'
import Toasted from 'vue-toasted'

import './assets/style/custom.scss'

Vue.use(BootstrapVue)
Vue.use(VueFeather)
Vue.use(vWow)
Vue.use(VueCarousel)
Vue.use(Toasted)

Vue.use(VueCookies)
Vue.use(FBSignInButton)
Vue.use(GSignInButton)

Vue.use(Element, { locale })
Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
