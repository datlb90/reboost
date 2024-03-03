<template>
  <div>
    <div>
      <el-alert v-if="!completed" title="Hãy cung cấp một mật khẩu mới và xác nhận để hoàn tất quá trình đặt lại mật khẩu" center show-icon />
      <el-alert v-else title="Mật khẩu của bạn đã được đặt lại thành công" type="success" center show-icon />
      <div class="wrapper">
        <div>
          <div>
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 20px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <el-form v-if="!completed" ref="formReset" :model="form" :rules="rules">
              <el-form-item prop="password">
                <label>Mật khẩu mới</label>
                <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Điền mật khẩu của bạn" />
              </el-form-item>

              <el-form-item prop="confirmPassword" style="margin-bottom: 25px;">
                <label>Xác nhận mật khẩu</label>
                <el-input id="confirmPassword" v-model="form.confirmPassword" type="password" autocomplete="off" placeholder="Xác nhận mật khẩu" />
              </el-form-item>

              <el-form-item>
                <el-button
                  class="btn btn-gradient"
                  style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                  :loading="loading"
                  @click="resetPassword()"
                >
                  Đặt Lại Mật Khẩu
                </el-button>
              </el-form-item>
            </el-form>

            <div v-if="completed">
              <el-button
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="gotoLoginPage()"
              >
                Đăng Nhập Ngay
              </el-button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
    <script>
    import autService from '@/services/auth.service'
    export default {
      name: 'ResetPassword',
      data() {
        var validatePassword = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('Vui lòng nhập mật khẩu'))
            } else if (value.length < 6) {
                callback(new Error('Mật khẩu cần có ít nhất 6 ký tự'))
            } else {
                callback()
            }
        }
        var validateConfirmPassword = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('Vui lòng xác nhận mật khẩu'))
            } else if (value.length < 6) {
                callback(new Error('Mật khẩu cần có ít nhất 6 ký tự'))
            } else if (value !== this.form.password) {
                callback(new Error('Mật khẩu và xác nhận mật khẩu không giống nhau'))
            } else {
                callback()
            }
        }
        return {
          form: {
            password: '',
            confirmPassword: ''
          },
          rules: {
            password: [
              {
                validator: validatePassword, trigger: 'blur'
              }
            ],
            confirmPassword: [
              {
                validator: validateConfirmPassword, trigger: 'blur'
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
          completed: false,
          email: null,
          token: null
        }
      },
      async created() {
        this.email = this.getUrlParameter('email')
        this.token = this.getUrlParameter('token')
      },
      methods: {
        gotoLoginPage() {
            this.$router.push('/login')
        },
        async resetPassword() {
          this.$refs['formReset'].validate(async valid => {
            if (valid && this.email && this.token) {
              this.loading = true
              const model = {
                Email: this.email,
                Token: this.token,
                NewPassword: this.form.password,
                ConfirmPassword: this.form.confirmPassword
              }

              console.log(model)

              autService.resetPassword(model).then(rs => {
                  this.loading = false
                  this.completed = true
              })
              .catch(rs => {
                this.loading = false
              })
            } else return false
          })
        },
        getUrlParameter(sParam) {
            const sPageURL = decodeURIComponent(window.location.search.substring(1))
            const sURLVariables = sPageURL.split('&')
            let sParameterName
            let i
            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=')

                if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : sParameterName[1]
                }
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
