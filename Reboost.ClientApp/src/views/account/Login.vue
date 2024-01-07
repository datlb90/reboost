<template>
  <div>
    <div>
      <div class="wrapper">
        <div>
          <el-form :model="form">
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 30px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/green_logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <el-form-item style="text-align: left;">
              <el-input id="email" v-model="form.username" type="text" :placeholder="messageTranslates('login', 'usernameEmail')" />
            </el-form-item>
            <el-form-item style="text-align: left;">

              <el-input id="password" v-model="form.password" type="password" autocomplete="off" :placeholder="messageTranslates('login', 'password')" />
            </el-form-item>
            <el-form-item>
              <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                @click="signIn()"
              >
                {{ messageTranslates('login', 'signIn') }}
              </el-button>
            </el-form-item>

            <el-form-item style="text-align: left;">
              <a href="/forgot/password" style="float: left; color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('login', 'forgotPassword') }}
              </a>
              <a :href="linkToRegister" style="float: right; color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('login', 'signUp') }}
              </a>
            </el-form-item>

            <div class="separator" style="font-size: 14px; text-align: center; padding-bottom: 20px;">
              {{ messageTranslates('login', 'orSignIn') }}
            </div>

            <el-form-item>
              <form ref="facebookLoginForm" method="post" :action="facebookFormAction">
                <el-button type="primary" :disabled="true" plain style="width: 48%; float: left;" @click="submitFacebookLoginForm()">
                  Facebook
                </el-button>
              </form>
              <form ref="googleLoginForm" method="post" :action="googleFormAction">
                <el-button type="danger" plain style="width: 48%; float: right;" @click="submitGoogleLoginForm()">
                  Google
                </el-button>
              </form>
            </el-form-item>

            <div style="font-size: 14px; text-align: center; padding-bottom: 5px;">
              {{ messageTranslates('login', 'byLoggingIn') }}
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('login', 'terms') }}
              </a> {{ messageTranslates('login', 'and') }}
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('login', 'policies') }}
              </a>
            </div>

          </el-form>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapActions } from 'vuex'
import { PageName } from '@/app.constant'
import authService from '@/services/auth.service'

export default {
  name: 'Login',
  data() {
    return {
      user: null,
      mgr: null,
      form: {
        username: '',
        password: ''
      },
      googleExternalLogin: null,
      returnUrl: '',
      googleFormAction: null,
      facebookFormAction: null
    }
  },
  computed: {
    linkToRegister() {
      if (this.$router.currentRoute.path.includes('/rater')) {
        return '/rater/register'
      }
      return '/register'
    }
  },
  mounted() {
  },
  beforeRouteEnter(to, from, next) {
    if (authService.isAuthenticated()) {
      return next({ name: PageName.AFTER_LOGIN })
    }
    next()
  },
  async created() {
    if (this.$router.currentRoute.query?.returnUrl) {
      this.googleFormAction = 'api/auth/external/google/Learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
      this.facebookFormAction = 'api/auth/external/facebook/Learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
    } else {
      this.googleFormAction = 'api/auth/external/google/Learner?returnUrl=/'
      this.facebookFormAction = 'api/auth/external/facebook/Learner?returnUrl=/'
    }
  },
  methods: {
    ...mapActions('auth', ['login']),
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
      // authService.loginFacebook(encodeURIComponent(this.returnUrl)).then(rs => { console.log('login facebook', rs) })
    },
    submitGoohlrLoginForm() {
      this.$refs.googleLoginForm.submit()
      // authService.loginGoogle(encodeURIComponent(this.returnUrl))
    },
    async signIn() {
      this.$store.dispatch('auth/logout')
      const user = await this.login({
        Email: this.form.username,
        Password: this.form.password
      })
      if (user) {
        this.$store.dispatch('auth/setSelectedTest').then(rs => {
          if (this.$router.currentRoute.query?.returnUrl) {
            this.$router.push({ path: this.$router.currentRoute.query?.returnUrl })
          } else {
            this.$router.push({ name: PageName.AFTER_LOGIN }).catch(() => {})
          }
        })
      }
    }
  }
}
</script>

<style scoped>
.login-btn:hover{
  background: rgb(95 147 177) !important;
}

.separator {
    display: flex;
    align-items: center;
    text-align: center;
}
.separator::before, .separator::after {
    content: '';
    flex: 1;
    border-bottom: 1px solid #dcdcdc;
}
.separator::before {
    margin-right: 1em;
}
.separator::after {
    margin-left: 1em;
}

.wrapper {
  width: 400px;
  margin: auto;
  padding: 30px;
  border: 1px solid rgb(234 234 234);
  border-radius: 8px;
  background-color: white;
  margin-top: 10%;
  -webkit-box-shadow: 0 4px 20px rgba(0, 0, 0, 0.04);
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.04);
}

hr
{
	margin-bottom: 20px;
	border-bottom: 1px;
	border-top: 1px solid rgba(0, 0, 0, 0.1);
}

.el-form-item__content{
	line-height: 20px !important;
}

.g-signin-button {
  /* This is where you control how the button looks. Be creative! */
  display: inline-block;
  padding: 4px 8px;
  border-radius: 3px;
  color: #fff;
}
.fb-signin-button {
  /* This is where you control how the button looks. Be creative! */
  display: inline-block;
  padding: 4px 8px;
  border-radius: 3px;
  color: #fff;
}
</style>
