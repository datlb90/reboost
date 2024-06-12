<template>
  <div>
    <div>
      <el-alert v-if="emailConfirmed" title="Địa chỉ email đã được xác thực thành công! Bạn có thể đăng nhập ngay bây giờ." type="success" center show-icon />

      <div class="wrapper">
        <div>
          <el-form :model="form">
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 30px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <el-form-item style="text-align: left;">
              <el-input id="email" v-model="form.username" type="text" placeholder="Email hoặc số điện thoại" />
            </el-form-item>
            <el-form-item style="text-align: left;">

              <el-input id="password" v-model="form.password" type="password" autocomplete="off" :placeholder="messageTranslates('login', 'password')" />
            </el-form-item>
            <el-form-item>
              <!-- <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                :loading="loading"
                @click="signIn()"
              >
                {{ messageTranslates('login', 'signIn') }}
              </el-button> -->

              <el-button
                v-if="!user"
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signIn()"
              >{{ messageTranslates('login', 'signIn') }}
              </el-button>

              <el-button
                v-if="user && loading && (personalQuestion || initialSubmission)"
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signIn()"
              >
                Đánh giá bài luận
              </el-button>

            </el-form-item>

            <el-form-item style="text-align: left; margin-bottom: 30px;">
              <a :href="linkToRegister" style="float: left; color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('login', 'signUp') }}
              </a>
              <a href="/forgot/password" style="float: right; color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('login', 'forgotPassword') }}
              </a>
            </el-form-item>

            <div class="separator" style="font-size: 14px; text-align: center; padding-bottom: 20px;">
              {{ messageTranslates('login', 'orSignIn') }}
            </div>

            <el-form-item>
              <form ref="facebookLoginForm" method="post" :action="facebookFormAction">
                <el-button type="primary" plain style="width: 48%; float: left;" @click="submitFacebookLoginForm()">
                  Facebook
                </el-button>
              </form>
              <form ref="googleLoginForm" method="post" :action="googleFormAction">
                <el-button type="danger" plain style="width: 48%; float: right;" @click="submitGoogleLoginForm()">
                  Google
                </el-button>
              </form>
            </el-form-item>

            <!-- <hr> -->

            <div style="font-size: 14px; text-align: center; padding-bottom: 5px; margin-top: 30px;">
              Khi đăng nhập, bạn đồng ý với
              <a href="/terms" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('login', 'terms') }}
              </a> {{ messageTranslates('login', 'and') }}
              <a href="/privacy" style="color: rgb(101 139 179); text-decoration: none;">
                chính sách bảo mật
              </a>
              của chúng tôi.
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
      facebookFormAction: null,
      emailConfirmed: false,
      loading: false,
      reviewRequested: false,
      personalQuestion: null,
      initialSubmission: null
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
    document.title = 'Đăng nhập - Reboost'
    this.personalQuestion = this.$store.getters['question/getPersonalQuestion']
    this.initialSubmission = this.$store.getters['question/getInitialSubmission']

    if (this.$router.currentRoute.query?.email && this.$router.currentRoute.query?.email == 'confirmed') {
      this.emailConfirmed = true
    }
    if (this.$router.currentRoute.query?.returnUrl) {
      this.googleFormAction = 'api/auth/external/google/learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
      this.facebookFormAction = 'api/auth/external/facebook/learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
    } else {
      this.googleFormAction = 'api/auth/external/google/learner?returnUrl=/'
      this.facebookFormAction = 'api/auth/external/facebook/learner?returnUrl=/'
    }
    document.addEventListener('keydown', this.pressEnterToLogin)
  },
  methods: {
    ...mapActions('auth', ['login']),
    getUrlParameter(sParam) {
      const sPageURL = decodeURIComponent(window.location.search.substring(1))
      const sURLVariables = sPageURL.split('&')
      let sParameterName
      let i
      for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=')

        if (sParameterName[0] === sParam) {
          return sParameterName[1] === undefined ? true : sParameterName[1]
        }
      }
    },
    pressEnterToLogin(e) {
      if (e.code === 'Enter') {
        console.log(e.code)
        this.signIn()
      }
    },
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    submitGoogleLoginForm() {
      this.$refs.googleLoginForm.submit()
    },
    async signIn() {
      document.activeElement.blur()
      this.$store.dispatch('auth/logout')
      this.loading = true
      const user = await this.login({
        Username: this.form.username,
        Password: this.form.password
      })
      if (user) {
        document.removeEventListener('keydown', this.pressEnterToLogin)
        const planId = this.getUrlParameter('planId')
        if (this.$router.currentRoute.query?.returnUrl) {
          this.$router.push({ path: this.$router.currentRoute.query?.returnUrl })
        } else if (planId && planId != '0') {
          this.$store.dispatch('auth/setSelectedTest').then(rs => {
            window.location.href = '/pricing?planId=' + planId
          })
        } else {
          this.$router.push({ name: PageName.AFTER_LOGIN }).catch(() => {})
        }
      } else {
        this.loading = false
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
