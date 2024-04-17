<template>
  <div>
    <div>
      <el-alert v-if="!emailSent" title="Hãy cung cấp địa chỉ email của bạn. Chúng tôi sẽ gửi đường dẫn để bạn đặt lại mật khẩu." center show-icon />
      <el-alert v-else title="Đường dẫn để đặt lại mật khẩu đã được gửi cho bạn qua địa chỉ email." type="success" center show-icon />
      <div class="wrapper">
        <div>
          <div>
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 20px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <el-form ref="formForgot" :model="form" :rules="rules">
              <el-form-item style="margin-bottom: 25px;" prop="email">
                <label>Địa chỉ email của bạn</label>
                <el-input id="email" v-model="form.email" type="text" placeholder="Ví dụ: learner@reboost.vn" />
              </el-form-item>

              <el-form-item>
                <el-button
                  class="btn btn-gradient"
                  style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                  :loading="loading"
                  @click="forgotPassword()"
                >
                  Đặt Lại Mật Khẩu
                </el-button>
              </el-form-item>
              <el-form-item style="text-align: left;">
                <a href="/login" style="float: left; color: rgb(101 139 179); text-decoration: none;">
                  Đăng nhập tài khoản
                </a>
                <a href="/register" style="float: right; color: rgb(101 139 179); text-decoration: none;">
                  Tạo tài khoản mới
                </a>
              </el-form-item>
            </el-form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  <script>
  import autService from '@/services/auth.service'
  export default {
    name: 'ForgotPassword',
    data() {
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
      return {
        form: {
          email: ''
        },
        rules: {
          email: [
            {
              validator: validateEmail, trigger: 'blur'
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
        emailSent: false
      }
    },
    async created() {

    },
    methods: {
      async forgotPassword() {
        this.$refs['formForgot'].validate(async valid => {
          if (valid) {
            this.loading = true
            this.emailSent = false
            const model = {
              Email: this.form.email
            }

            autService.forgotPassword(model).then(rs => {
                this.loading = false
                this.emailSent = true
            }).catch(rs => {
                this.loading = false
            })
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
