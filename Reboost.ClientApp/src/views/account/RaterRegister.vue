<template>
  <div>
    <div>
      <div class="wrapper">
        <div>
          <el-form ref="formSignUp" :model="form">
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 30px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/green_logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <el-form-item style="text-align: left;" prop="firstName" :rules="[{ required: true, message: messageTranslates('raterRegister', 'firstNameRequired')}]">
              <el-input id="firstName" v-model="form.firstName" type="text" :placeholder="messageTranslates('raterRegister', 'firstName')" />
            </el-form-item>

            <el-form-item style="text-align: left;" prop="lastName" :rules="[{ required: true, message: messageTranslates('raterRegister', 'lastNameRequired')}]">
              <el-input id="lastName" v-model="form.lastName" type="text" :placeholder="messageTranslates('raterRegister', 'lastName')" />
            </el-form-item>

            <el-form-item style="text-align: left;" prop="username" :rules="[{ required: true, message: messageTranslates('raterRegister', 'usernameRequired')}]">
              <el-input id="username" v-model="form.username" type="text" :placeholder="messageTranslates('raterRegister', 'usernameEmail')" />
            </el-form-item>
            <el-form-item style="text-align: left;" prop="password" :rules="[{ required: true, message: messageTranslates('raterRegister', 'passwordRequired')}]">
              <el-input id="password" v-model="form.password" type="password" autocomplete="off" :placeholder="messageTranslates('raterRegister', 'password')" />
            </el-form-item>
            <el-form-item>
              <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                @click="signUp()"
              >
                {{ messageTranslates('raterRegister', 'createAccount') }}
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
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('raterRegister', 'terms') }}
              </a> {{ messageTranslates('raterRegister', 'and') }}
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
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
    return {
      user: null,
      mgr: null,
      form: {
        username: '',
        password: '',
        firstName: null,
        lastName: null
      },
      googleExternalLogin: null,
      returnUrl: '/',
      googleFormAction: null,
      facebookFormAction: null
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
          const user = await this.register({
            Email: this.form.username,
            Password: this.form.password,
            FirstName: this.form.firstName,
            LastName: this.form.lastName,
            Role: 'Rater'
          })
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
