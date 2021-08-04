<template>
  <!-- Start Main Banner -->
  <div id="banner" class="main-banner" style="height: 860px; padding-top: 100px; padding-bottom: 80px;">
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
            <el-form ref="formSignUp" :model="form">
              <el-form-item class="m-1" prop="firstName" :rules="[{ required: true, message: 'First name is required'}]">
                <label class="m-0">First name</label>
                <el-input id="firstName" v-model="form.firstName" type="text" placeholder="First Name" />
              </el-form-item>

              <el-form-item class="m-1" prop="lastName" :rules="[{ required: true, message: 'Last name is required'}]">
                <label class="m-0">Last name</label>
                <el-input id="lastName" v-model="form.lastName" type="text" placeholder="Last Name" />
              </el-form-item>

              <el-form-item class="m-1" prop="email" :rules="[{ required: true, message: 'Email address is required'}]">
                <label class="m-0">Email address</label>
                <el-input id="email" v-model="form.email" type="text" placeholder="Username or email" />
              </el-form-item>
              <el-form-item prop="password" :rules="[{ required: true, message: 'Password is required'}]">
                <label class="m-0">Password</label>
                <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Password" />
              </el-form-item>
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

            </el-form>
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
export default {
  name: 'Banner',
  data() {
    return {
      user: null,
      mgr: null,
      form: {
        email: '',
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
      this.$refs['formSignUp'].validate(async valid => {
        if (valid) {
          console.log(this.form)
          const user = await this.register({
            Email: this.form.email,
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
