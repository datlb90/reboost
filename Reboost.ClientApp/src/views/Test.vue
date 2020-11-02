<template>
  <div class="test">
    <!-- <div>
			<p>Hello {{ username }}</p>
		</div>
		<el-alert
			title="warning alert"
			type="warning"
		/> -->
    <div>
      <div
        style="width: 400px;
					margin: auto;
					padding: 30px;
					border: #d4d3d3 solid 1px;
					border-radius: 8px;
					background-color: white;     margin-top: 10%;"
      >
        <div>
          <el-form :model="form">
            <div style="    margin: auto;    width: 140px; padding-left: 10px; padding-bottom: 20px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/green_logo.png" alt="logo" style="width: 140px;">
              </router-link>
              <!-- <el-button plain>
                Reboost Logo
              </el-button> -->
              <!-- <p class="welcome">
                Welcome back, please login to your account.
              </p> -->
            </div>
            <el-form-item style="text-align: left;">
              <span style="font-size: 15px; font-weight: bold; color: rgb(78 126 154);">
                Username or email
              </span>
              <el-input id="email" v-model="form.username" type="text" />
              <br>
            </el-form-item>
            <el-form-item style="text-align: left;">
              <span style="font-size: 15px; font-weight: bold; color:rgb(78 126 154);">
                Password
              </span>

              <a href="https://netid.usf.edu/una/?display=reset" style="float: right; color: rgb(101 139 179); text-decoration: none;">
                Forgot Password?
              </a>

              <el-input id="password" v-model="form.password" type="password" autocomplete="off" />
            </el-form-item>
            <!-- <el-form-item>
              <el-checkbox style="float: left;">
                Remember Me
              </el-checkbox>
              <a href="https://netid.usf.edu/una/?display=reset" style="float: right; color: #409eff; text-decoration: none;">
                Forgot Password?
              </a>
            </el-form-item> -->
            <el-form-item>
              <!-- <el-button type="primary" style="width: 100%;" @click="login()">
                Sign in to Reboost
              </el-button> -->
              <a href="#" class="btn btn-primary" style="width: 100%;" @click="login()">Sign In</a>
            </el-form-item>

            <!-- <el-form-item>
              <form ref="googleLoginForm" method="post" :action="googleFormAction">
                <el-button type="primary" style="width: 100%;" @click="submitGoogleLoginForm()">
                  Sign in with Google
                </el-button>
              </form>
            </el-form-item>

            <el-form-item>
              <form ref="facebookLoginForm" method="post" :action="facebookFormAction">
                <el-button type="primary" style="width: 100%;" @click="submitFacebookLoginForm()">
                  Sign in with Facebook
                </el-button>
              </form>
            </el-form-item> -->

            <div style="font-size: 14px; text-align: center; padding-bottom: 5px;">
              By logging in, I agree to the
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                terms
              </a> and
              <a href="#" style="color: rgb(101 139 179); text-decoration: none;">
                policies
              </a>
            </div>
            <hr>
            <el-form-item style="margin-bottom: 5px;">
              <a href="#" class="btn btn-light" style="width: 100%;text-transform: none; color: #246185;">New to Reboost? Register Now!</a>

            </el-form-item>
          </el-form>
        </div>
      </div>
    </div>

    <div v-if="access_token" style="width: 30%; margin: auto; margin-top: 20px; text-align: center;">
      <el-button type="primary" plain @click="apiCall()">
        Api Call
      </el-button>
      <!-- <el-button type="primary" plain @click="logout()">
        Logout
      </el-button> -->
    </div>

    <el-table
      v-if="weathers && weathers.length > 0"
      :data="weathers"
      border
      style="width: 50%; margin: auto; margin-top: 20px;"
    >
      <el-table-column
        prop="date"
        label="Date"
        width="180"
      />
      <el-table-column
        prop="temperatureC"
        label="Temperature C"
        width="180"
      />
      <el-table-column
        prop="temperatureF"
        label="Temperature F"
      />
      <el-table-column
        prop="summary"
        label="Summary"
      />
    </el-table>
  </div>
</template>
<script>
// @ is an alias to /src
import http from '@/utils/axios'
export default {
  name: 'Test',
  data() {
    return {
      weathers: [],
      user: null,
      mgr: null,
      username: 'guest',
      form: {
        username: '',
        password: ''
      },
      access_token: null,
      fbSignInParams: {
        scope: 'email',
        return_scopes: true
      },
      googleExternalLogin: null,
      returnUrl: '/about',
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
    oauthSignIn() {
      // Google's OAuth 2.0 endpoint for requesting an access token
      var oauth2Endpoint = 'https://accounts.google.com/o/oauth2/v2/auth'

      // Create <form> element to submit parameters to OAuth 2.0 endpoint.
      var form = document.createElement('form')
      form.setAttribute('method', 'GET') // Send as a GET request.
      form.setAttribute('action', oauth2Endpoint)

      // Parameters to pass to OAuth 2.0 endpoint.
      var params = { 'client_id': '296436268455-jec5622h7o285l5thb6es3cs2dkv4m45.apps.googleusercontent.com',
        'redirect_uri': 'http://localhost:3011/about',
        'response_type': 'token',
        'scope': 'https://www.googleapis.com/auth/drive.metadata.readonly',
        'include_granted_scopes': 'true',
        'state': 'pass-through value' }

      // Add form parameters as hidden input values.
      for (var p in params) {
        var input = document.createElement('input')
        input.setAttribute('type', 'hidden')
        input.setAttribute('name', p)
        input.setAttribute('value', params[p])
        form.appendChild(input)
      }

      // Add form to page and submit it to open the OAuth 2.0 endpoint.
      document.body.appendChild(form)
      form.submit()
    },
    submitGoogleLoginForm() {
      this.$refs.googleLoginForm.submit()
    },
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    login() {
      const that = this
      http.post('http://localhost:6990/api/auth/login', {
        Email: this.form.username,
        Password: this.form.password
      })
        .then(function(response) {
          console.log(response)
          that.username = response.data.email
          that.access_token = response.data.message
          that.$notify.success({
            title: 'Success',
            message: 'You have successfully logged in.'
          })
        })
        .catch(function(error) {
          // console.log(error)
          that.$notify.error({
            title: 'Error',
            message: error
          })
        })
    },
    apiCall() {
      const config = {
        headers: { Authorization: `Bearer ${this.access_token}` }
      }
      const url = 'http://localhost:6990/api/weatherforecast'
      // let url = 'https://edvision.ai/api/weatherforecast';
      http.get(
        url,
        config
      ).then(response => {
        if (response && response.data.length > 0) {
          this.weathers = response.data
        }
      }).catch(error => {
        console.log(error)
        this.$notify.error({
          title: 'Error',
          message: 'Username or password is incorrect.'
        })
      })
    }
  }
}
</script>

<style>
hr
{
	margin-bottom: 20px;
	border-bottom: 1px;
	border-top: 1px solid rgba(0, 0, 0, 0.1);
}

.el-form-item__content{
	line-height: 20px !important;
}
</style>

<style scoped>
.g-signin-button {
  /* This is where you control how the button looks. Be creative! */
  display: inline-block;
  padding: 4px 8px;
  border-radius: 3px;
  background-color: #3c82f7;
  color: #fff;
  box-shadow: 0 3px 0 #0f69ff;
}
.fb-signin-button {
  /* This is where you control how the button looks. Be creative! */
  display: inline-block;
  padding: 4px 8px;
  border-radius: 3px;
  background-color: #4267b2;
  color: #fff;
}
</style>
