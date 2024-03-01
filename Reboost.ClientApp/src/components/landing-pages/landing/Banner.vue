<template>
  <!-- Start Main Banner -->
  <div v-if="screenWidth > 1200" id="banner" class="ml-main-section" style="padding-top: 150px; padding-bottom: 80px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-6 col-md-12">
          <div class="ml-banner-content">
            <h1 style="font-size: 40px; color: #4a6f8a; font-weight: 400;">{{ messageTranslates('banner', 'slogan') }}</h1>
            <p style="margin-top: 30px;">{{ messageTranslates('banner', 'bannerContentDescription') }}</p>
            <ul>
              <li>{{ messageTranslates('banner', 'content1') }}</li>
              <li>{{ messageTranslates('banner', 'content2') }}</li>
              <li>{{ messageTranslates('banner', 'content3') }}</li>
              <li>{{ messageTranslates('banner', 'content4') }}</li>
            </ul>

            <el-button
              class="btn btn-dark"
              style="padding: 15px 80px; margin-top: 20px; background-color: #4b6f8a; margin-bottom: 40px;"
              @click="gotoTest()"
            > Thực Hiện Bài Kiểm Tra Đầu Vào Miễn Phí
            </el-button>
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
              <el-button
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signIn()"
              >{{ messageTranslates('banner', 'signIn') }}
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

              <div style="font-size: 14px; padding-top: 20px; height: 10px; padding-bottom: 20px;">

                <div style="width: 50%; float: left;">
                  <a style="text-decoration: none;" href="/register">{{ messageTranslates('login', 'signUp') }}</a>
                </div>

                <div style="width: 50%; float: right; text-align: right;">
                  <a style="text-decoration: none;" href="/forgot">{{ messageTranslates('banner', 'forgotPassword') }}</a>
                </div>

              </div>
            </form>
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

  <div v-else-if="screenWidth > 992" id="banner" class="ml-main-section" style=" padding-top: 150px; padding-bottom: 80px;">
    <div class="container">
      <div class="row">

        <div class="col-lg-6 col-md-12">
          <div class="ml-banner-content">
            <h1 style="font-size: 40px; color: #4a6f8a; font-weight: 400;">{{ messageTranslates('banner', 'slogan') }}</h1>
            <p style="margin-top: 30px;">{{ messageTranslates('banner', 'bannerContentDescription') }}</p>
            <ul>
              <li>{{ messageTranslates('banner', 'content1') }}</li>
              <li>{{ messageTranslates('banner', 'content2') }}</li>
              <li>{{ messageTranslates('banner', 'content3') }}</li>
              <li>{{ messageTranslates('banner', 'content4') }}</li>
            </ul>

            <el-button
              class="btn btn-dark"
              style="padding: 15px 60px; margin-top: 20px; background-color: #4b6f8a; margin-bottom: 40px;"
              @click="gotoTest()"
            > Thực Hiện Bài Kiểm Tra Đầu Vào Miễn Phí
            </el-button>
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
              <el-button
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signIn()"
              >{{ messageTranslates('banner', 'signIn') }}
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

              <div style="font-size: 14px; padding-top: 20px; height: 10px; padding-bottom: 20px;">

                <div style="width: 50%; float: left;">
                  <a style="text-decoration: none;" href="/register">{{ messageTranslates('banner', 'signUp') }}</a>
                </div>

                <div style="width: 50%; float: right; text-align: right;">
                  <a style="text-decoration: none;" href="/forgot">{{ messageTranslates('banner', 'forgotPassword') }}</a>
                </div>

              </div>
            </form>
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

  <div v-else id="banner" class="ml-main-section" style=" padding-top: 100px; padding-bottom: 80px;">
    <div class="container">
      <div class="row">

        <div class="col-lg-6 col-md-12">
          <div class="ml-banner-content">
            <h1 style="font-size: 32px; text-align: left; margin-left: 10px; color: #4a6f8a; font-weight: 400;">{{ messageTranslates('banner', 'slogan') }}</h1>
            <p style="margin-top: 30px; text-align: left; font-size: 14.5px; margin-left: 10px;">{{ messageTranslates('banner', 'bannerContentDescription') }}</p>
            <div>
              <div style="font-size: 14.5px; color: #4a6f8a; text-align: left; margin-left: 10px; margin-bottom: 5px;">- {{ messageTranslates('banner', 'content1') }}</div>
              <div style="font-size: 14.5px; color: #4a6f8a; text-align: left; margin-left: 10px; margin-bottom: 5px;">- {{ messageTranslates('banner', 'content2') }}</div>
              <div style="font-size: 14.5px; color: #4a6f8a; text-align: left; margin-left: 10px; margin-bottom: 5px;">- {{ messageTranslates('banner', 'content3') }}</div>
              <div style="font-size: 14.5px; color: #4a6f8a; text-align: left; margin-left: 10px; margin-bottom: 5px;">- {{ messageTranslates('banner', 'content4') }}</div>
            </div>

            <el-button
              class="btn btn-dark"
              style="width: 100%; padding: 15px 60px; margin-top: 20px; background-color: #4b6f8a; margin-bottom: 40px;"
              @click="gotoTest()"
            > Thực Hiện Bài Kiểm Tra Đầu Vào Miễn Phí
            </el-button>
          </div>
        </div>

        <div class="col-lg-5 offset-lg-1">
          <div class="banner-form">
            <form>
              <div class="form-group">
                <label>{{ messageTranslates('banner', 'emailAddress') }}</label>
                <input v-model="email" type="text" class="form-control" :placeholder="messageTranslates('banner', 'placeholderEmail')">
              </div>

              <div class="form-group">
                <label>{{ messageTranslates('banner', 'password') }}</label>
                <input v-model="password" type="password" class="form-control" :placeholder="messageTranslates('banner', 'placeholderPassword')">
              </div>
              <el-button
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signIn()"
              >{{ messageTranslates('banner', 'signIn') }}
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

              <div style="font-size: 14px; padding-top: 20px; height: 10px; padding-bottom: 20px;">

                <div style="width: 50%; float: left;">
                  <a style="text-decoration: none;" href="/register">{{ messageTranslates('banner', 'signUp') }}</a>
                </div>

                <div style="width: 50%; float: right; text-align: right;">
                  <a style="text-decoration: none;" href="/forgot">{{ messageTranslates('banner', 'forgotPassword') }}</a>
                </div>

              </div>
            </form>
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
      facebookFormAction: null,
      loading: false,
      screenWidth: window.innerWidth
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  mounted() {
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
  },
  async created() {
    if (this.$router.currentRoute.query?.returnUrl) {
      this.googleFormAction = 'api/auth/external/google/learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
      this.facebookFormAction = 'api/auth/external/facebook/learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
    } else {
      this.googleFormAction = 'api/auth/external/google/learner?returnUrl=/'
      this.facebookFormAction = 'api/auth/external/facebook/learner?returnUrl=/'
    }
  },
  methods: {
    ...mapActions('auth', ['login']),
    gotoTest() {
      var anchor = document.getElementById('initialTest')
      if (anchor) {
        console.lo9g
        window.scrollTo(anchor.offsetLeft, anchor.offsetTop - 50)
      }
    },
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    submitGoogleLoginForm() {
      this.$refs.googleLoginForm.submit()
    },
    async signIn() {
      this.loading = true
      this.$store.dispatch('auth/logout')
      const user = await this.login({
        Email: this.email,
        Password: this.password
      })
      this.loading = false
      if (user) {
        this.$router.push({ name: PageName.AFTER_LOGIN }).catch(() => {})
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
    padding: 30px 30px;
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
