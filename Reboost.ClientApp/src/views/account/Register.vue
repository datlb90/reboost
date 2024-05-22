<template>
  <div>
    <div>
      <el-alert v-if="initialSubmission" title="Đăng ký tài khoản để nhận phản hồi miễn phí cho bài viết" type="success" center show-icon />
      <el-alert v-else-if="personalQuestion" title="Đăng ký tài khoản để nhận phản hồi miễn phí cho bài viết" type="success" center show-icon />
      <div class="wrapper">
        <div>
          <div>
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 10px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <el-form ref="formSignUp" :model="form" :rules="rules">
              <el-form-item style="margin-bottom: 16px;" prop="fullName">
                <label class="m-0">Họ và tên</label>
                <el-input id="fullName" v-model="form.fullName" type="text" placeholder="Họ và tên của bạn. Ví dụ: Nguyễn Văn A" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="email">
                <label class="m-0">Địa chỉ email</label>
                <el-input id="email" v-model="form.email" type="text" placeholder="Địa chỉ email của bạn. Ví dụ: learner@reboost.vn" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="phoneNumber">
                <label class="m-0">Số điện thoại</label>
                <el-input id="phoneNumber" v-model="form.phoneNumber" type="text" placeholder="Số điện thoại của bạn. Ví dụ: 0981234567" />
              </el-form-item>
              <el-form-item prop="password">
                <label class="m-0">Mật khẩu</label>
                <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Điền mật khẩu của bạn" />
              </el-form-item>

              <el-form-item>
                <el-button
                  v-if="!user"
                  class="btn btn-gradient"
                  style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                  :loading="loading"
                  @click="signUp()"
                >
                  Đăng Ký Tài Khoản
                </el-button>

                <el-button
                  v-if="user && loading && (personalQuestion || initialSubmission)"
                  class="btn btn-gradient"
                  style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                  :loading="loading"
                  :disabled="true"
                >
                  Đánh giá bài luận
                </el-button>
              </el-form-item>
              <el-form-item style="text-align: center;">
                <p href="/forgot/password" style="color: black; text-decoration: none;">
                  Đã có tài khoản? <a href="/login" style="color: rgb(101 139 179); text-decoration: none;">
                    {{ messageTranslates('register', 'signInNow') }}
                  </a>
                </p>
              </el-form-item>

              <hr>
              <div style="font-size: 14px; text-align: center; padding-bottom: 10px;">
                {{ messageTranslates('register', 'orSignInWith') }}
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

              <!-- <div style="font-size: 14px; text-align: center; padding-bottom: 5px; margin-top: 30px;">
                Khi tạo tài khoản, bạn đồng ý với
                <a href="/terms" style="color: rgb(101 139 179); text-decoration: none;">
                  {{ messageTranslates('login', 'terms') }}
                </a> {{ messageTranslates('login', 'and') }}
                <a href="/privacy" style="color: rgb(101 139 179); text-decoration: none;">
                  chính sách bảo mật
                </a>
                của chúng tôi.
              </div> -->

              <!-- <hr>
              <div style="font-size: 14px; text-align: center; padding-bottom: 10px;">
                {{ messageTranslates('register', 'orSignInWith') }}
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
              <div style="font-size: 14px; text-align: center; padding-bottom: 5px;">
                Bằng việc đăng ký tài khoản, bạn đồng ý với
                <a href="/terms" style="color: rgb(101 139 179); text-decoration: none;">
                  điều khoản
                </a> và
                <a href="/privacy" style="color: rgb(101 139 179); text-decoration: none;">
                  chính sách bảo mật
                </a>
                của chúng tôi
              </div> -->
            </el-form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapActions } from 'vuex'
import { PageName } from '@/app.constant'
import { SCORES } from '../../app.constant'
import userService from '@/services/user.service'
import moment from 'moment'
export default {
  name: 'Login',
  data() {
    var validatePhone = (rule, value, callback) => {
      var phoneno = /^\+?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$/
      if (!value.match(phoneno)) {
        callback(new Error('Vui lòng nhập một số điện thoại hợp lệ'))
      } else {
        callback()
      }
    }
    var validateEmail = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('Vui lòng điền địa chỉ email của bạn'))
      } else if (!value.toLowerCase().match(
          /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      )) {
        callback(new Error('Vui lòng nhập một email hợp lệ'))
      } else {
        callback()
      }
    }
    var validatePassword = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('Vui lòng nhập mật khẩu'))
      } else if (value.length < 6) {
        callback(new Error('Mật khẩu cần có ít nhất 6 ký tự'))
      } else {
        callback()
      }
    }
    return {
      form: {
        fullName: '',
        email: '',
        password: '',
        phoneNumber: ''
      },
      rules: {
        fullName: [
          {
            required: true, message: 'Vui lòng điền họ và tên của bạn'
          }
        ],
        email: [
          {
            validator: validateEmail, trigger: 'blur'
          }
        ],
        phoneNumber: [
          {
            validator: validatePhone, trigger: 'blur'
          }
        ],
        password: [
          {
            validator: validatePassword, trigger: 'blur'
          }
        ]
      },
      user: null,
      mgr: null,
      googleExternalLogin: null,
      returnUrl: '/',
      googleFormAction: null,
      facebookFormAction: null,
      loading: false,
      personalQuestion: null,
      initialSubmission: null
    }
  },
  async created() {
    document.title = 'Đăng ký tài khoản'
    this.personalQuestion = this.$store.getters['question/getPersonalQuestion']
    this.initialSubmission = this.$store.getters['question/getInitialSubmission']

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
    ...mapActions('auth', ['register']),
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
        this.signUp()
      }
    },
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    submitGoogleLoginForm() {
      this.$refs.googleLoginForm.submit()
    },
    async signUp() {
      document.activeElement.blur()
      this.$refs['formSignUp'].validate(async valid => {
        if (valid) {
          this.loading = true
          this.user = await this.register({
            Email: this.form.email,
            Password: this.form.password,
            FullName: this.form.fullName,
            PhoneNumber: this.form.phoneNumber,
            Role: 'learner'
          })
          if (this.user) {
            document.removeEventListener('keydown', this.pressEnterToLogin)
            // Add IELTS test score for all user
            var scores = []
            const formData = {
              ieltsTestScore: {
                WRITING: 0,
                READING: 0,
                LISTENING: 0,
                SPEAKING: 0
              }
            }
            for (const key in formData.ieltsTestScore) {
              scores.push({
                sectionId: SCORES.IELTS[key].sectionId,
                score: 0,
                updatedDate: moment().format('yyyy-MM-DD')
              })
            }
            userService.addScore(this.user.id, scores).then(rs => {
              const planId = this.getUrlParameter('planId')
              console.log(this.$store.getters['auth/getUser'])
              if (planId && planId != '0') {
                this.$store.dispatch('auth/setSelectedTest').then(rs => {
                  window.location.href = '/pricing?planId=' + planId
                })
              } else { this.$router.push({ name: PageName.AFTER_LOGIN }) }
            })
          } else {
            this.loading = false
          }
        } else return false
      })
    }
  }
}
</script>

<style scoped>
.login-btn:hover{
  background: rgb(95 147 177) !important;
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
