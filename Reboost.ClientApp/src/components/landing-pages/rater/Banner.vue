<template>
  <!-- Start Main Banner -->
  <div id="banner" class="main-banner" style="height: 860px; padding-top: 180px; padding-bottom: 80px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-7 col-md-12">
          <div class="ml-banner-content" style="margin-top: 10px;">
            <h1>Become a Rater with Reboost</h1>
            <el-steps :active="4" style="margin-top: 60px; margin-bottom: 60px; color: #6084a4 !important;">
              <el-step title="Step 1" description="Create an account" icon="far fa-user-circle" />
              <el-step title="Step 2" description="Upload credentials" icon="fas fa-file-upload" />
              <el-step title="Step 3" description="Complete tranning" icon="far fa-check-circle" />
              <el-step title="Step 4" description="Start rating" icon="fas fa-edit" />
            </el-steps>
            <p style="max-width: 100%;">Our raters are teachers, instructors, test makers, certified examiners, and industry professionals, who are all passionate about their areas of expertise and eager to help test takers improve their writing skill and test score.</p>
            <p style="max-width: 100%;">Our online evaluation platform makes connecting with test takers and earning extra income simple, convenient, and flexible.</p>
          </div>
        </div>

        <div class="col-lg-5 col-md-12">
          <div class="banner-form ml-3">
            <form>
              <div class="form-group">
                <label>Email address</label>
                <input v-model="email" type="text" class="form-control" placeholder="Enter username">
              </div>

              <div class="form-group">
                <label>Password</label>
                <input v-model="password" type="password" class="form-control" placeholder="Create a password">
              </div>

              <!-- <button type="submit" class="btn btn-primary" style="width: 100%; margin-top: 10px;" @click="register()">Sign Up</button> -->
              <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                @click="signUp()"
              >
                Create Rater Account
              </el-button>
              <div class="separator" style="font-size: 14px; text-align: center; padding-bottom: 20px; padding-top: 20px;">
                Or you can sign in with
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
                By logging in, I agree to the
                <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                  terms
                </a> and
                <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                  policies
                </a>
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
      returnUrl: '/',
      googleFormAction: null,
      facebookFormAction: null
    }
  },
  async created() {
    this.googleFormAction = 'api/auth/external/google/Rater/' + encodeURIComponent(this.returnUrl)
    this.facebookFormAction = 'api/auth/external/facebook/Rater/' + encodeURIComponent(this.returnUrl)
  },
  methods: {
    ...mapActions('auth', ['register']),
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    submitGoohlrLoginForm() {
      this.$refs.googleLoginForm.submit()
    },
    async signUp() {
      const user = await this.register({
        Email: this.email,
        Password: this.password,
        Role: 'Rater'
      })
      if (user) {
        console.log(user)
      }
    }
  }
}
</script>

<style>
.el-step__icon.is-icon{
  font-size: 30px;
}

.el-step__head.is-finish {
    color: #6e8fac !important;
    border-color: #6e8fac !important;
}

.el-step__title.is-finish{
   color: #6e8fac !important;
}

.el-step__description.is-finish {
    color: #6e8fac !important;
}

</style>

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
