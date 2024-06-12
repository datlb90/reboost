<template>
  <div>
    <div>
      <div class="wrapper">
        <div>
          <div>
            <div style="margin: auto; width: 140px; padding-left: 10px; padding-bottom: 20px;">
              <router-link class="navbar-brand" to="/" style="padding-top: 0px;">
                <img src="@/assets/logo/logo.png" alt="logo" style="width: 140px;">
              </router-link>
            </div>
            <div>
              <h2>Kiểm tra email của bạn</h2>
              <p style="margin-bottom: 15px;">Chúng tôi đã gửi một email tới địa chỉ:</p>
              <el-tag type="info" style="font-size: 15px; margin-bottom: 15px; text-align: center; width: 100%;">
                {{ email }}
              </el-tag>
              <p>Hãy kiểm tra hòm thư và làm theo hướng dẫn để xác nhận tài khoản của bạn.</p>
              <hr>
              <p style="margin-bottom: 10px;">Nếu bạn không nhận được email xác nhận:</p>
              <ul style="padding-left: 18px;">
                <li style="color: #4a6f8a;">Xin vui lòng chờ trong giây lát</li>
                <li style="color: #4a6f8a;">Kiểm tra thư mục spam và hòm thư rác</li>
                <li style="color: #4a6f8a;">Chắc chắn rằng bạn đang sử dụng email ở trên</li>
                <li style="color: #4a6f8a;">Thử gửi lại email hoặc liên hệ với chúng tôi</li>
              </ul>
              <div style="height: 40px; margin-top: 30px; margin-bottom: 10px;">
                <el-button type="primary" plain style="width: 60%; float: left;" :loading="loading" @click="resendEmail()">
                  Gửi Lại Email Xác Nhận
                </el-button>
                <el-button plain style="width: 36%; float: right;" @click="contactUs()">
                  Liên Hệ
                </el-button>
              </div>
            </div>

          </div>
        </div>
      </div>
    </div>
    <contact-dialog ref="contactDialog" />
  </div>
</template>
  <script>
  import autService from '@/services/auth.service'
  import ContactDialog from '../../components/controls/ContactDialog.vue'
  export default {
    name: 'ConfirmEmail',
    components: {
    'contact-dialog': ContactDialog
    },
    data() {
      return {
        user: this.$store.state.auth.unconfirmedUser,
        email: null,
        loading: false
      }
    },
    async created() {
      document.title = 'Xác nhận tài khoản'
      console.log(this.user)
      if (this.user && this.user.email) {
        this.email = this.user.email
      } else {
        this.$router.push('/notfound')
      }
    },
    methods: {
      async resendEmail() {
        this.loading = true
        console.log(this.user.id)
        autService.resendConfirmationEmail(this.user.id).then(rs => {
          console.log(rs)
          this.loading = false
          if (rs.isSuccess) {
            this.$notify.success({
              title: 'Email xác nhận đã được gửi lại',
              message: 'Bạn vui lòng kiểm tra hòm thư và làm theo hướng dẫn để xác nhận tài khoản.',
              type: 'success',
              duration: 5000
            })
          } else {
            this.$notify.error({
              title: 'Đã có lỗi xảy ra',
              message: 'Đã có lỗi xảy ra trong quá trình gửi lại email. Bạn vui lòng thử lại hoặc liên hệ với chúng tôi.',
              type: 'error',
              duration: 5000
            })
          }
        }).catch(rs => {
            this.loading = false
        })
      },
      contactUs() {
        this.$refs.contactDialog?.openDialog()
      }
    }
  }
  </script>

  <style scoped>
  .login-btn:hover{
    background: rgb(95 147 177) !important;
  }

  .wrapper {
    width: 412px;
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
