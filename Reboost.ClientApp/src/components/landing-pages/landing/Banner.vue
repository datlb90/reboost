<template>
  <!-- Start Main Banner -->
  <div>
    <div v-if="screenWidth > 1200" id="banner" class="ml-main-section" style="padding-top: 200px; padding-bottom: 60px;">
      <div class="container">
        <div style="text-align: center;">
          <div>
            <div>
              <div>
                <h1 style="font-size: 42px; color: #4a6f8a; font-weight: 500;">Cách nâng band IELTS Writing hiệu quả nhất!</h1>
              </div>
              <p style="width: 62%; margin: auto; margin-top: 30px; font-size: 18px; color: #4a6f8a; ">
                <!-- Nâng band IELTS Writing hiệu quả với phương pháp học mới, đa dạng tài nguyên hữu dụng cùng phản hồi chuyên sâu, và chất lượng.\ -->
                Reboost cung cấp phản hồi chuyên sâu cùng đa dạng tài nguyên hữu dụng giúp bạn nâng band IELST Writing nhanh chóng và hiệu quả nhất.
              </p>
              <el-button
                class="btn btn-dark"
                style="font-size: 18px; padding: 12px 50px; margin-top: 50px; background-color: #4b6f8a;"
                @click="openRequestReviewDialog()"
              > Chấm IELTS Writing Miễn Phí
              <!-- Chấm Bài Miễn Phí Với ChatGPT-4 -->
              </el-button>
              <div style="margin-top: 20px;">
                <a style="text-decoration: underline; cursor: pointer;  font-weight: 500;" href="sample/feedback/basic" target="_blank">
                  Tham khảo bài chấm mẫu
                </a>
              </div>
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

      <BoxesArea />
    </div>

    <div v-else-if="screenWidth > 670" id="banner" class="ml-main-section" style=" padding-top: 180px; padding-bottom: 60px;">
      <div class="container">
        <div style="text-align: center;">

          <div>
            <div>
              <div>
                <h1 style="font-size: 32px; color: #4a6f8a; font-weight: 500;">Luyện IELTS Writing và nhận phản hồi miễn phí</h1>
              </div>
              <p style="width: 75%; margin: auto; margin-top: 30px; font-size: 17px; color: #4a6f8a; ">
                Nâng band IELTS Writing hiệu quả ứng dụng phương pháp học mới, với đa dạng tài nguyên hữu dụng cùng phản hồi miễn phí, chuyên sâu, và chất lượng.
              </p>
              <el-button
                class="btn btn-dark"
                style="font-size: 18px; padding: 12px 50px; margin-top: 50px; background-color: #4b6f8a;"
                @click="openRequestReviewDialog()"
              > Chấm Bài Miễn Phí Với ChatGPT-4
              </el-button>
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

    <div v-else id="banner" class="ml-main-section" style="padding-top: 160px; padding-bottom: 60px">
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-12">
            <div class="ml-banner-content">
              <div>
                <h1 style="font-size: 35px; color: #4a6f8a; font-weight: 500;">Luyện IELTS Writing và nhận phản hồi miễn phí</h1>
              </div>
              <p style="margin-top: 30px; font-size: 16px; color: #4a6f8a;">
                Nâng band IELTS Writing hiệu quả ứng dụng phương pháp học mới, với đa dạng tài nguyên hữu dụng cùng phản hồi miễn phí, chuyên sâu, và chất lượng.
              </p>

              <el-button
                class="btn btn-dark"
                style="font-size: 18px; padding: 12px 30px; margin-top: 30px; background-color: #4b6f8a;"
                @click="openRequestReviewDialog()"
              > Chấm Bài Miễn Phí Với ChatGPT-4
              </el-button>
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

    <review-request-dialog
      ref="reviewRequestDialog"
    />
  </div>

  <!-- End ML Main Banner -->
</template>

<script>
import { mapActions } from 'vuex'
import { PageName } from '@/app.constant'
import ReviewRequestDialog from '../../../components/controls/ReviewRequestDialog.vue'

import BoxesArea from '../landing/BoxesArea'
export default {
  name: 'Banner',
  components: {
    'review-request-dialog': ReviewRequestDialog,

    BoxesArea
  },
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
      returnUrl: 'rater/application/status/88',
      googleFormAction: null,
      facebookFormAction: null,
      loading: false,
      screenWidth: window.innerWidth
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
    if (this.$router.currentRoute.query?.returnUrl) {
      this.googleFormAction = 'api/auth/external/google/learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
      this.facebookFormAction = 'api/auth/external/facebook/learner?returnUrl=' + this.$router.currentRoute.query?.returnUrl
    } else {
      this.googleFormAction = 'api/auth/external/google/learner?returnUrl=/'
      this.facebookFormAction = 'api/auth/external/facebook/learner?returnUrl=/'
    }
  },
  methods: {
    ...mapActions('auth', ['login']),
    openRequestReviewDialog() {
      const header = document.querySelector('#header.headroom')
      // header.style['z-index'] = 2000
      header.style.display = 'none'
      this.$refs.reviewRequestDialog?.openDialog()
    },
    gotoTest() {
      var anchor = document.getElementById('initialTest')
      if (anchor) {
        console.lo9g
        window.scrollTo(anchor.offsetLeft, anchor.offsetTop - 50)
      }
    },
    submitFacebookLoginForm() {
      this.$refs.facebookLoginForm.submit()
    },
    submitGoogleLoginForm() {
      this.$refs.googleLoginForm.submit()
    },
    async signIn() {
      this.loading = true
      this.$store.dispatch('auth/logout')
      const user = await this.login({
        Email: this.email,
        Password: this.password
      })
      this.loading = false
      if (user) {
        this.$router.push({ name: PageName.AFTER_LOGIN }).catch(() => {})
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
    padding: 30px 30px;
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
