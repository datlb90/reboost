<template>
  <div>
    <div>
      <el-alert v-if="personalQuestion" title="Sign up free now to request review for your writing" type="success" center show-icon />
      <div class="wrapper">
        <div>
          <el-form ref="formSignUp" :model="form">
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 30px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/green_logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <el-form-item style="text-align: left;" prop="firstName" :rules="[{ required: true, message: messageTranslates('register', 'firstNameRequired')}]">
              <el-input id="firstName" v-model="form.firstName" type="text" :placeholder="messageTranslates('register', 'firstName')" />
            </el-form-item>
            <el-form-item style="text-align: left;" prop="lastName" :rules="[{ required: true, message: messageTranslates('register', 'lastNameRequired')}]">
              <el-input id="lastName" v-model="form.lastName" type="text" :placeholder="messageTranslates('register', 'lastName')" />
            </el-form-item>
            <el-form-item style="text-align: left;" prop="username" :rules="[{ required: true, message: messageTranslates('register', 'usernameRequired')}]">
              <el-input id="email" v-model="form.username" type="text" :placeholder="messageTranslates('register', 'usernameEmail')" />
            </el-form-item>
            <el-form-item style="text-align: left;" prop="password" :rules="[{ required: true, message: messageTranslates('register', 'passwordRequired')}]">
              <el-input id="password" v-model="form.password" type="password" autocomplete="off" :placeholder="messageTranslates('register', 'password')" />
            </el-form-item>
            <el-form-item>
              <!-- <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                @click="signUp()"
              >
                {{ messageTranslates('register', 'createAccount') }}
              </el-button> -->
              <el-button
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signUp()"
              >
                {{ messageTranslates('register', 'createAccount') }}
              </el-button>
            </el-form-item>

            <el-form-item style="text-align: center;">
              <p href="/forgot/password" style="color: black; text-decoration: none;">
                {{ messageTranslates('register', 'alreadyHave') }}  <a href="/login" style="color: rgb(101 139 179); text-decoration: none;">
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
            <div style="font-size: 14px; text-align: center; padding-bottom: 5px;">
              {{ messageTranslates('register', 'byLoggingIn') }}
              <a href="/terms" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('register', 'terms') }}
              </a> {{ messageTranslates('register', 'and') }}
              <a href="/privacy" style="color: rgb(101 139 179); text-decoration: none;">
                {{ messageTranslates('register', 'policies') }}
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
import { SCORES } from '../../app.constant'
import userService from '@/services/user.service'
import moment from 'moment'
export default {
  name: 'Login',
  data() {
    return {
      user: null,
      mgr: null,
      form: {
        username: null,
        password: null,
        firstName: null,
        lastName: null
      },
      googleExternalLogin: null,
      returnUrl: '/',
      googleFormAction: null,
      facebookFormAction: null,
      loading: false,
      personalQuestion: null
    }
  },
  async created() {
    this.personalQuestion = this.$store.getters['question/getPersonalQuestion']
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
            Email: this.form.username,
            Password: this.form.password,
            FirstName: this.form.firstName,
            LastName: this.form.lastName,
            Role: 'learner'
          })
          this.loading = false
          if (user) {
            // Add user score to by bass select test requirement if review is requested
            if (this.personalQuestion) {
              var scores = []
              const formData = {
                ieltsTestScore: {
                  WRITING: 0,
                  READING: 0,
                  LISTENING: 0,
                  SPEAKING: 0
                },
                toeflTestScore: {
                  WRITING: 0,
                  READING: 0,
                  LISTENING: 0,
                  SPEAKING: 0
                }
              }
              if (this.personalQuestion.Test == 'IELTS') {
                for (const key in formData.ieltsTestScore) {
                  scores.push({
                    sectionId: SCORES.IELTS[key].sectionId,
                    score: 0,
                    updatedDate: moment().format('yyyy-MM-DD')
                  })
                }
              }
              if (this.personalQuestion.Test == 'TOEFL') {
                for (const key in formData.toeflTestScore) {
                  scores.push({
                    sectionId: SCORES.TOEFL[key].sectionId,
                    score: 0,
                    updatedDate: moment().format('yyyy-MM-DD')
                  })
                }
              }
              userService.addScore(user.id, scores).then(rs => {
                this.$router.push({ name: PageName.AFTER_LOGIN })
              })
            }
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
