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
              <el-input id="email" v-model="form.username" type="text" placeholder="Username or email" />
            </el-form-item>
            <el-form-item style="text-align: left;">

              <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Password" />
            </el-form-item>
            <el-form-item>
              <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                @click="login()"
              >
                Sign In
              </el-button>
            </el-form-item>

            <el-form-item style="text-align: left;">
              <a href="/forgot/password" style="float: left; color: rgb(101 139 179); text-decoration: none;">
                Forgot Password?
              </a>
              <a href="/register" style="float: right; color: rgb(101 139 179); text-decoration: none;">
                Sign Up
              </a>
            </el-form-item>

            <hr>
            <div style="font-size: 14px; text-align: center; padding-bottom: 10px;">
              Or you can sign in with
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
              By logging in, I agree to the
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                terms
              </a> and
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                policies
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
      returnUrl: '/',
      googleFormAction: null,
      facebookFormAction: null
    }
  },
  async created() {
    this.googleFormAction = 'api/auth/external/google/' + encodeURIComponent(this.returnUrl)
    this.facebookFormAction = 'api/auth/external/facebook/' + encodeURIComponent(this.returnUrl)
  },
  methods: {
    ...mapActions('auth', ['authLogin']),
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    submitGoohlrLoginForm() {
      this.$refs.googleLoginForm.submit()
    },
    async login() {
      const user = await this.authLogin({
        Email: this.form.username,
        Password: this.form.password
      })
      if (user) {
        if (user.role == 'Admin') { this.$router.push('/admin') }
        if (user.role == 'Rater') { this.$router.push('/rater/home') }
        if (user.role == 'Learner') { this.$router.push('/topics/all') }
      }
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
