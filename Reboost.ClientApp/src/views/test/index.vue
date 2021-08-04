<template>
  <div class="test">
    <div>
      <p>Hello {{ username }}</p>
    </div>
    <el-alert
      title="warning alert"
      type="warning"
    />
    <div>
      <div
        style="width: 420px;
					margin: auto;
					padding: 30px;
					border: #d4d3d3 solid 1px;
					border-radius: 8px;
					background-color: white;"
      >
        <div>
          <el-form :model="form">
            <div>
              <el-button plain>
                Edvision Logo
              </el-button>
              <p class="welcome">
                Welcome back, please login to your account.
              </p>
            </div>
            <el-form-item style="text-align: left;">
              <span style="font-size: 15px; font-weight: bold; color: #409eff;">
                Institution Email
              </span>
              <el-input id="email" v-model="form.username" type="text" />
              <br>
            </el-form-item>
            <el-form-item style="text-align: left;">
              <span style="font-size: 15px; font-weight: bold; color: #409eff;">
                Password
              </span>
              <el-input id="password" v-model="form.password" type="password" autocomplete="off" />
            </el-form-item>
            <el-form-item>
              <el-checkbox style="float: left;">
                Remember Me
              </el-checkbox>
              <a href="https://netid.usf.edu/una/?display=reset" style="float: right; color: #409eff; text-decoration: none;">
                Forgot Password?
              </a>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" style="width: 100%;" @click="login()">
                Sign in to EdVision
              </el-button>
            </el-form-item>
            <div style="font-size: 14px; text-align: center; padding-bottom: 5px;">
              By logging in, I agree to the
              <a href="#" style="color: #409eff; text-decoration: none;">
                terms
              </a> and
              <a href="#" style="color: #409eff; text-decoration: none;">
                policies
              </a>
            </div>
            <hr>
            <el-form-item style="margin-bottom: 5px;">
              <el-button type="primary" style="width: 100%;" plain>
                Contact Support
              </el-button>
            </el-form-item>
          </el-form>
        </div>
      </div>
    </div>

    <div style="width: 30%; margin: auto; margin-top: 20px;">
      <el-button type="primary" plain @click="apiCall()">
        Api Call
      </el-button>
      <el-button type="primary" plain @click="logout()">
        Logout
      </el-button>
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
      access_token: null
    }
  },
  async created() {
  },
  methods: {
    login() {
      const that = this
      http.post('http://localhost:5000/api/auth/login', {
        Email: this.form.username,
        Password: this.form.password
      })
        .then(function(response) {
          console.log(response)
          that.username = response.data.email
          that.access_token = response.data.message
        })
        .catch(function(error) {
          console.log(error)
        })
    },
    logout() {
      this.mgr.signoutRedirect()
    },
    apiCall() {
      const config = {
        headers: { Authorization: `Bearer ${this.access_token}` }
      }
      const url = 'http://localhost:5000/api/weatherforecast'
      // let url = 'https://edvision.ai/api/weatherforecast';
      http.get(
        url,
        config
      ).then(response => {
        if (response && response.data.length > 0) {
          this.weathers = response.data
        }
      }).catch(error => {
        this.$notify.error({
          title: 'Error',
          type: 'error',
          message: error
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
