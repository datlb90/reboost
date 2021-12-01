<template>
  <!-- Start Main Banner -->
  <div id="banner" class="ml-main-section" style=" padding-top: 150px; padding-bottom: 80px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-6 col-md-12">
          <div class="ml-banner-content" style="margin-top: 50px;">
            <h1>{{ messageTranslates('banner', 'bannerContentTitle') }}</h1>
            <p>{{ messageTranslates('banner', 'bannerContentDescription') }}</p>
            <ul>
              <li>{{ messageTranslates('banner', 'content1') }}</li>
              <li>{{ messageTranslates('banner', 'content2') }}</li>
              <li>{{ messageTranslates('banner', 'content3') }}</li>
              <li>{{ messageTranslates('banner', 'content4') }}</li>
            </ul>
          </div>
        </div>

        <div class="col-lg-5 offset-lg-1">
          <div class="banner-form ml-3">
            <form>
              <div class="form-group">
                <label>{{ messageTranslates('banner', 'emailAddress') }}</label>
                <input v-model="email" type="text" class="form-control" :placeholder="messageTranslates('banner', 'placeholderEmail')">
              </div>

              <div class="form-group">
                <label>{{ messageTranslates('banner', 'password') }}</label>
                <input v-model="password" type="password" class="form-control" :placeholder="messageTranslates('banner', 'placeholderPassword')">
              </div>

              <!-- <button class="btn btn-primary" style="width: 100%; margin-top: 10px;" @click="signUp()">Sign Up</button> -->
              <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                @click="signIn()"
              >
                {{ messageTranslates('banner', 'signIn') }}
              </el-button>
              <div class="separator" style="font-size: 14px; text-align: center; padding-bottom: 20px; padding-top: 20px;">
                {{ messageTranslates('banner', 'signInOther') }}
              </div>

              <div style="padding-bottom: 40px;">
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
              </div>

              <div style="font-size: 14px; text-align: center; padding-top: 20px; height: 10px;">
                {{ messageTranslates('banner', 'byLogging') }}
                <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                  {{ messageTranslates('banner', 'terms') }}
                </a> {{ messageTranslates('banner', 'and') }}
                <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                  {{ messageTranslates('banner', 'policies') }}
                </a>
              </div>
            </form>
          </div>
          <div class="banner-form ml-3 mt-3" style="padding: 30px">
            {{ messageTranslates('banner', 'dontAccount') }} <a style="color: rgb(101 139 179); text-decoration: none;" href="/register">{{ messageTranslates('banner', 'signUp') }}</a>
          </div>
        </div>

      </div>
    </div>

    <div class="shape1"><img src="../../../assets/img/shape1.png" alt="shape"></div>
    <div class="shape3"><img src="../../../assets/img/shape3.svg" alt="shape"></div>
    <div class="shape4"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape6 rotateme"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape7"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape8 rotateme"><img src="../../../assets/img/shape2.svg" alt="shape"></div>
  </div>
  <!-- End ML Main Banner -->
</template>

<script>
import { mapActions } from 'vuex'
import { PageName } from '@/app.constant'
// import authService from '@/services/auth.service'
export default {
  name: 'Banner',
  data() {
    return {
      user: null,
      mgr: null,
      form: {
        username: '',
        password: ''
      },
      email: null,
      password: null,
      googleExternalLogin: null,
      returnUrl: 'rater/application/status/88',
      googleFormAction: null,
      facebookFormAction: null
    }
  },
  async created() {
    // console.log(encodeURIComponent(this.returnUrl))
    // this.googleFormAction = 'api/auth/external/google/Learner/' // + encodeURIComponent(this.returnUrl)
    // this.facebookFormAction = 'api/auth/external/facebook/Learner/' // + encodeURIComponent(this.returnUrl)

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
    submitGoogleLoginForm() {
      this.$refs.googleLoginForm.submit()
      // authService.loginGoogle(encodeURIComponent(this.returnUrl))
    },
    async signIn() {
      this.$store.dispatch('auth/logout')
      const user = await this.login({
        Email: this.email,
        Password: this.password
      })
      if (user) {
        // this.$router.push({ name: PageName.AFTER_LOGIN })
        this.$store.dispatch('auth/setSelectedTest').then(rs => {
          this.$router.push({ name: PageName.AFTER_LOGIN })
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

.banner-form{
  background: #ffffff;
    -webkit-box-shadow: 0 2px 48px 0 rgba(0, 0, 0, 0.08);
    box-shadow: 0 2px 48px 0 rgba(0, 0, 0, 0.08);
    padding: 50px 30px;
    border-radius: 5px;
}

.ml-main-section .ml-banner-content ul {
    padding: 0;
    margin: 0;
    list-style-type: none;
}

.ml-main-section .ml-banner-content ul li{
     margin-bottom: 12px;
     position: relative;
     color: #6084a4;
     font-size: 16px;
     padding-left: 30px;
}

.ml-main-section .ml-banner-content ul li::before {
    position: absolute;
    left: 0;
    top: 50%;
    -webkit-transform: translateY(-50%);
    transform: translateY(-50%);
    width: 20px;
    height: 2px;
    background: #44ce6f;
    content: '';
}

.ml-banner-content p {
    font-size: 16px;
    max-width: 490px;
}

</style>
