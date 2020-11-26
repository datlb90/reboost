<template>
  <!-- Start Main Banner -->
  <div id="banner" class="ml-main-section" style=" padding-top: 150px; padding-bottom: 80px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-6 col-md-12">
          <div class="ml-banner-content" style="margin-top: 50px;">
            <h1>A New Way to Learn</h1>
            <p>Reboost is the best platform to help you boost your language writing test scores.
              No matter which test you take, we will get you fully prepared and guarantee your score improvement by offering:</p>
            <ul>
              <li>Unlimited practice tests</li>
              <li>Active community that encourages you to practice more</li>
              <li>Opportunity to learn by providing feedback</li>
              <li>Useful and constructive feedback to help you improve</li>
            </ul>
          </div>
        </div>

        <div class="col-lg-5 offset-lg-1">
          <div class="banner-form ml-3">
            <form>
              <div class="form-group">
                <label>Email address</label>
                <input v-model="email" type="text" class="form-control" placeholder="Enter email address">
              </div>

              <div class="form-group">
                <label>Password</label>
                <input v-model="password" type="password" class="form-control" placeholder="Create a password">
              </div>

              <!-- <button class="btn btn-primary" style="width: 100%; margin-top: 10px;" @click="signUp()">Sign Up</button> -->
              <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                @click="signUp()"
              >
                Sign Up
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
    this.googleFormAction = 'api/auth/external/google/Learner/' + encodeURIComponent(this.returnUrl)
    this.facebookFormAction = 'api/auth/external/facebook/Learner/' + encodeURIComponent(this.returnUrl)
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
        Role: 'Learner'
      })
      if (user) {
        console.log(user)
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
