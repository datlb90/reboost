<template>
  <div>
    <div>
      <div class="wrapper">
        <div>
          <el-form ref="formSignUp" :model="form" :rules="rules">
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 10px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>

            <el-form-item style="margin-bottom: 16px;" prop="fullName">
              <label class="m-0">Họ và tên</label>
              <el-input id="fullName" v-model="form.fullName" type="text" placeholder="Họ và tên của bạn. Ví dụ: Nguyễn Văn A" />
            </el-form-item>
            <el-form-item style="margin-bottom: 16px;" prop="phoneNumber">
              <label class="m-0">Số điện thoại</label>
              <el-input id="phoneNumber" v-model="form.phoneNumber" type="text" placeholder="Số điện thoại của bạn. Ví dụ: 0981234567" />
            </el-form-item>
            <el-form-item style="margin-bottom: 16px;" prop="email">
              <label class="m-0">Địa chỉ email</label>
              <el-input id="email" v-model="form.email" type="text" placeholder="Địa chỉ email của bạn. Ví dụ learner@reboost.vn" />
            </el-form-item>
            <el-form-item prop="password">
              <label class="m-0">Mật khẩu</label>
              <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Điền mật khẩu của bạn" />
            </el-form-item>

            <el-form-item>
              <el-button
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signUp()"
              >
                Tạo Tài Khoản Giáo Viên
              </el-button>
            </el-form-item>

            <el-form-item style="text-align: center;">
              <p href="/forgot/password" style="color: black; text-decoration: none;">
                {{ messageTranslates('raterRegister', 'alreadyHave') }}&nbsp;<a href="/rater/login" style="color: rgb(101 139 179); text-decoration: none;">
                  {{ messageTranslates('raterRegister', 'signInNow') }}
                </a>
              </p>
            </el-form-item>

            <hr>
            <div style="font-size: 14px; text-align: center; padding-bottom: 10px;">
              {{ messageTranslates('raterRegister', 'orSignInWith') }}
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
              {{ messageTranslates('raterRegister', 'byLoggingIn') }}
              <a href="/terms" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('raterRegister', 'terms') }}
              </a> {{ messageTranslates('raterRegister', 'and') }}
              <a href="/privacy" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('raterRegister', 'policies') }}
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
      loading: false
    }
  },
  async created() {
    this.googleFormAction = 'api/auth/external/google/' + encodeURIComponent(this.returnUrl)
    this.facebookFormAction = 'api/auth/external/facebook/' + encodeURIComponent(this.returnUrl)
    // this.oauthSignIn()
  },
  methods: {
    ...mapActions('auth', ['register']),
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    async signUp() {
      this.$refs['formSignUp'].validate(async valid => {
        if (valid) {
          this.loading = true
          const user = await this.register({
            Email: this.form.email,
            Password: this.form.password,
            FullName: this.form.fullName,
            PhoneNumber: this.form.phoneNumber,
            Role: 'Rater'
          })
          this.loading = false
          if (user) {
            this.$router.push({ name: PageName.AFTER_LOGIN })
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
  margin-top: 8%;
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
