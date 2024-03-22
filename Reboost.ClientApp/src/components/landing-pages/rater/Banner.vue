<template>
  <div v-if="screenWidth > 1200" class="iot-main-banner" style="height: 850px;padding-top: 120px; padding-bottom: 50px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-7 col-md-12">
          <div class="ml-banner-content" style="margin-top: 10px;">
            <h1 style="font-size: 40px; color: #4a6f8a; font-weight: 400;">Trở thành giáo viên của Reboost</h1>
            <p style="max-width: 100%;">Reboost là nền tảng giúp giáo viên dạy IELTS kết nối với học viên, hỗ trợ học viên chấm bài và cải thiện kỹ năng viết.</p>
            <p style="max-width: 100%;">Chúng tôi tin rằng sự tiến bộ của học viên là niềm tự hào và thành tựu của các thầy cô giáo. Vì vậy, Reboost không chỉ là một công cụ giáo dục mà còn là cầu nối giúp họ chia sẻ kiến thức và kinh nghiệm của mình để tạo ra những sự khác biệt.</p>
            <p style="max-width: 100%;">Với Reboost, giáo viên sẽ được trải nghiệm một hệ thống đánh giá trực tuyến tiên tiến, linh hoạt về thời gian và địa điểm, giúp tối đa hóa cơ hội kiếm thêm thu nhập mà không làm ảnh hưởng đến công việc chính. Chúng tôi cam kết cung cấp một môi trường làm việc chuyên nghiệp, nơi giáo viên có thể phát triển bản thân và góp phần vào thành công của học viên.</p>
            <p style="max-width: 100%;">Hãy gia nhập Reboost ngay hôm nay để cùng chúng tôi tạo nên một cộng đồng học tập tích cực, nơi mỗi giáo viên đều có cơ hội phát huy tối đa tiềm năng của mình và giúp đỡ học viên đạt được mục tiêu học tập của họ.</p>
          </div>
        </div>

        <div class="col-lg-5 col-md-12">
          <div class="banner-form ml-3" style="padding-top: 30px;">
            <el-form ref="formSignUp" :model="form" :rules="rules">
              <el-form-item style="margin-bottom: 16px;" prop="fullName">
                <label class="m-0">Họ và tên</label>
                <el-input id="fullName" v-model="form.fullName" type="text" placeholder="Họ và tên của bạn. Ví dụ: Nguyễn Văn A" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="phoneNumber">
                <label class="m-0">Số điện thoại</label>
                <el-input id="phoneNumber" v-model="form.phoneNumber" type="text" placeholder="Số điện thoại của bạn. Ví dụ: 0981234567" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="email">
                <label class="m-0">Địa chỉ email</label>
                <el-input id="email" v-model="form.email" type="text" placeholder="Địa chỉ email của bạn. Ví dụ rater@reboost.vn" />
              </el-form-item>
              <el-form-item prop="password">
                <label class="m-0">Mật khẩu</label>
                <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Điền mật khẩu của bạn" />
              </el-form-item>

              <el-button
                class="btn btn-gradient"
                style="width: 100%; margin-right: 20px; padding: 12px 20px;"
                :loading="loading"
                @click="signUp()"
              > Tạo Tài Khoản Giáo Viên
              </el-button>

              <div class="separator" style="font-size: 14px; text-align: center; padding-bottom: 20px; padding-top: 20px;">
                Hoặc bạn có thể đăng ký với
              </div>

              <div style="padding-bottom: 30px;">
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
            </el-form>
          </div>
        </div>

      </div>
    </div>

    <!-- <div class="shape1"><img src="../../../assets/img/shape1.png" alt="shape"></div> -->
    <div class="shape3"><img src="../../../assets/img/shape3.svg" alt="shape"></div>
    <div class="shape4"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape6 rotateme"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape7"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <!-- <div class="shape8 rotateme"><img src="../../../assets/img/shape2.svg" alt="shape"></div> -->
  </div>
  <div v-else-if="screenWidth > 992" class="ml-main-section" style="height: 960px; padding-top: 120px; padding-bottom: 50px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-7 col-md-12">
          <div class="ml-banner-content" style="margin-top: 10px;">
            <h1 style="font-size: 40px; color: #4a6f8a; font-weight: 400;">Trở thành giáo viên của Reboost</h1>
            <p style="max-width: 100%;">Reboost là nền tảng được thiết kế để kết nối giáo viên dạy IELTS và TOEFL với học viên, giúp giáo viên có cơ hội chấm bài và hỗ trợ học viên cải thiện kỹ năng viết.</p>
            <p style="max-width: 100%;">Chúng tôi tin rằng sự tiến bộ của học viên là niềm tự hào và thành tựu của các thầy cô giáo. Vì vậy, Reboost không chỉ là một công cụ giáo dục mà còn là cầu nối giúp họ chia sẻ kiến thức và kinh nghiệm của mình để tạo ra những sự khác biệt.</p>
            <p style="max-width: 100%;">Với Reboost, giáo viên sẽ được trải nghiệm một hệ thống đánh giá trực tuyến tiên tiến, linh hoạt về thời gian và địa điểm, giúp tối đa hóa cơ hội kiếm thêm thu nhập mà không làm ảnh hưởng đến công việc chính. Chúng tôi cam kết cung cấp một môi trường làm việc chuyên nghiệp, nơi giáo viên có thể phát triển bản thân và góp phần vào thành công của học viên.</p>
            <p style="max-width: 100%;">Hãy gia nhập Reboost ngay hôm nay để cùng chúng tôi tạo nên một cộng đồng học tập tích cực, nơi mỗi giáo viên đều có cơ hội phát huy tối đa tiềm năng của mình và giúp đỡ học viên đạt được mục tiêu học tập của họ.</p>
          </div>
        </div>

        <div class="col-lg-5 col-md-12">
          <div class="banner-form ml-3" style="padding-top: 30px;">
            <el-form ref="formSignUp" :model="form" :rules="rules">
              <el-form-item style="margin-bottom: 16px;" prop="fullName">
                <label class="m-0">Họ và tên</label>
                <el-input id="fullName" v-model="form.fullName" type="text" placeholder="Họ và tên của bạn. Ví dụ: Nguyễn Văn A" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="phoneNumber">
                <label class="m-0">Số điện thoại</label>
                <el-input id="phoneNumber" v-model="form.phoneNumber" type="text" placeholder="Số điện thoại của bạn. Ví dụ: 0981234567" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="email">
                <label class="m-0">Địa chỉ email</label>
                <el-input id="email" v-model="form.email" type="text" placeholder="Địa chỉ email của bạn. Ví dụ rater@reboost.vn" />
              </el-form-item>
              <el-form-item prop="password">
                <label class="m-0">Mật khẩu</label>
                <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Điền mật khẩu của bạn" />
              </el-form-item>
              <el-button
                type="primary"
                class="login-btn"
                style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                :loading="loading"
                @click="signUp()"
              >
                Tạo Tài Khoản Giáo Viên
              </el-button>
              <div class="separator" style="font-size: 14px; text-align: center; padding-bottom: 20px; padding-top: 20px;">
                Hoặc bạn có thể đăng ký với
              </div>

              <div style="padding-bottom: 30px;">
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

              <!-- <div style="font-size: 14px; text-align: center; padding-top: 20px; height: 10px;">
                By logging in, I agree to the
                <a href="/terms" style="color: rgb(101 139 179); text-decoration: none;">
                  terms
                </a> and
                <a href="/privacy" style="color: rgb(101 139 179); text-decoration: none;">
                  policies
                </a>
              </div> -->

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
  <div v-else class="ml-main-section" style="height: auto; padding-top: auto; padding-bottom: 50px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-7 col-md-12">
          <div class="ml-banner-content" style="margin-top: 10px; margin-bottom: 20px;">
            <h1 style="font-size: 40px; color: #4a6f8a; font-weight: 400;">Trở thành giáo viên của Reboost</h1>
            <p style="max-width: 100%;">Reboost là nền tảng được thiết kế để kết nối giáo viên dạy IELTS và TOEFL với học viên, giúp giáo viên có cơ hội chấm bài và hỗ trợ học viên cải thiện kỹ năng viết.</p>
            <p style="max-width: 100%;">Chúng tôi tin rằng sự tiến bộ của học viên là niềm tự hào và thành tựu của các thầy cô giáo. Vì vậy, Reboost không chỉ là một công cụ giáo dục mà còn là cầu nối giúp họ chia sẻ kiến thức và kinh nghiệm của mình để tạo ra những sự khác biệt.</p>
            <p style="max-width: 100%;">Với Reboost, giáo viên sẽ được trải nghiệm một hệ thống đánh giá trực tuyến tiên tiến, linh hoạt về thời gian và địa điểm, giúp tối đa hóa cơ hội kiếm thêm thu nhập mà không làm ảnh hưởng đến công việc chính. Chúng tôi cam kết cung cấp một môi trường làm việc chuyên nghiệp, nơi giáo viên có thể phát triển bản thân và góp phần vào thành công của học viên.</p>
            <p style="max-width: 100%;">Hãy gia nhập Reboost ngay hôm nay để cùng chúng tôi tạo nên một cộng đồng học tập tích cực, nơi mỗi giáo viên đều có cơ hội phát huy tối đa tiềm năng của mình và giúp đỡ học viên đạt được mục tiêu học tập của họ.</p>
          </div>
        </div>

        <div class="col-lg-5 col-md-12">
          <div class="banner-form ml-3" style="padding-top: 30px;">
            <el-form ref="formSignUp" :model="form" :rules="rules">
              <el-form-item style="margin-bottom: 16px;" prop="fullName">
                <label class="m-0">Họ và tên</label>
                <el-input id="fullName" v-model="form.fullName" type="text" placeholder="Họ và tên của bạn. Ví dụ: Nguyễn Văn A" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="phoneNumber">
                <label class="m-0">Số điện thoại</label>
                <el-input id="phoneNumber" v-model="form.phoneNumber" type="text" placeholder="Số điện thoại của bạn. Ví dụ: 0981234567" />
              </el-form-item>
              <el-form-item style="margin-bottom: 16px;" prop="email">
                <label class="m-0">Địa chỉ email</label>
                <el-input id="email" v-model="form.email" type="text" placeholder="Địa chỉ email của bạn. Ví dụ rater@reboost.vn" />
              </el-form-item>
              <el-form-item prop="password">
                <label class="m-0">Mật khẩu</label>
                <el-input id="password" v-model="form.password" type="password" autocomplete="off" placeholder="Điền mật khẩu của bạn" />
              </el-form-item>

              <el-form-item>
                <el-button
                  type="primary"
                  class="login-btn"
                  style="width: 100%; background: rgb(73 124 153); border-color: transparent;"
                  :loading="loading"
                  @click="signUp()"
                >
                  Tạo Tài Khoản Giáo Viên
                </el-button>
                <div class="separator" style="font-size: 14px; text-align: center; padding-bottom: 20px; padding-top: 20px;">
                  Hoặc bạn có thể đăng ký với
                </div>

                <div style="padding-bottom: 30px;">
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

              </el-form-item>
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
</template>

<script>
import { mapActions } from 'vuex'
import { PageName } from '@/app.constant'
export default {
  name: 'Banner',
  data() {
    var validatePhone = (rule, value, callback) => {
      var phoneno = /^\+?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$/
      if (!value.match(phoneno)) {
        callback(new Error('Vui lòng nhập một số điện thoại hợp lệ'))
      } else {
        callback()
      }
    }
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
    var validatePassword = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('Vui lòng nhập mật khẩu'))
      } else if (value.length < 6) {
        callback(new Error('Mật khẩu cần có ít nhất 6 ký tự'))
      } else {
        callback()
      }
    }
    return {
      form: {
        fullName: '',
        email: '',
        password: '',
        phoneNumber: ''
      },
      rules: {
        fullName: [
          {
            required: true, message: 'Vui lòng điền họ và tên của bạn'
          }
        ],
        email: [
          {
            validator: validateEmail, trigger: 'blur'
          }
        ],
        phoneNumber: [
          {
            validator: validatePhone, trigger: 'blur'
          }
        ],
        password: [
          {
            validator: validatePassword, trigger: 'blur'
          }
        ]
      },
      user: null,
      mgr: null,
      googleExternalLogin: null,
      returnUrl: '/',
      googleFormAction: null,
      facebookFormAction: null,
      screenWidth: window.innerWidth,
      loading: false
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  mounted() {
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
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
          this.loading = true
          const user = await this.register({
            Email: this.form.email,
            Password: this.form.password,
            FullName: this.form.fullName,
            PhoneNumber: this.form.phoneNumber,
            Role: 'Rater'
          })
          this.loading = false
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
