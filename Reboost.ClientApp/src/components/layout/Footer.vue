<template>
  <!-- Start Footer Area -->
  <footer
    v-if="screenWidth > 780"
    class="bg-f7fafd"
    style="border-top: 1px solid #d8ebfd; text-align: center; padding-top: 14px;
    padding-bottom: 1px; background: #4b6f8a;"
  >
    <p style="color: white;">Công Ty TNHH Reboost | Mã số doanh nghiệp: 0110542943 | Email: <a style="color: white; text-decoration: underline;" href="mailto:support@reboost.vn">support@reboost.vn</a> | Zalo: <a target="_blank" style="color: white; text-decoration: underline;" href="https://zalo.me/+18135700346">+18135700346</a></p>
    <p style="color: white;">© 2024 - Bản quyền thuộc về Công ty TNHH Reboost | <a style="color: white; text-decoration: underline;" href="/terms">Điều Khoản Dịch Vụ</a> | <a style="color: white; text-decoration: underline;" href="/privacy">Chính Sách Bảo Mật</a></p>
    <div class="shape1"><img src="../../assets/img/shape1.png" alt="shape"></div>
    <div class="shape8 rotateme"><img src="../../assets/img/shape2.svg" alt="shape"></div>
    <back-to-top bottom="50px" right="50px">
      <div class="go-top"><feather type="arrow-up" /></div>
    </back-to-top>
  </footer>
  <footer
    v-else
    class="bg-f7fafd"
    style="border-top: 1px solid #d8ebfd; text-align: center; padding-top: 14px;
    padding-bottom: 1px; background: #4b6f8a;"
  >
    <p style="color: white;">Công Ty TNHH Reboost. Mã số thuế: 0110542943</p>
    <p style="color: white;">Email: <a style="color: white; text-decoration: underline;" href="mailto:support@reboost.vn">support@reboost.vn</a> | Zalo: <a target="_blank" style="color: white; text-decoration: underline;" href="https://zalo.me/+18135700346">+18135700346</a></p>
    <!-- <p style="color: white;">Mã số doanh nghiệp: 0110542943 do Sở Kế hoạch & Đầu tư TP Hà Nội cấp lần đầu ngày 15/11/2023.</p> -->
    <p style="color: white;">© 2024 - Bản quyền thuộc về Công ty TNHH Reboost | <a style="color: white; text-decoration: underline;" href="/terms">Điều Khoản</a> | <a style="color: white; text-decoration: underline;" href="/privacy">Chính Sách Bảo Mật</a></p>
    <!-- <img src="../../assets/img/map.png" class="map" alt="map"> -->
    <div class="shape1"><img src="../../assets/img/shape1.png" alt="shape"></div>
    <div class="shape8 rotateme"><img src="../../assets/img/shape2.svg" alt="shape"></div>
    <!-- <div class="go-top"><feather type="arrow-up"></feather></div> -->
    <back-to-top bottom="50px" right="50px">
      <div class="go-top"><feather type="arrow-up" /></div>
    </back-to-top>
  </footer>
  <!-- End Footer Area -->
</template>

<script>
import BackToTop from 'vue-backtotop'
export default {
  name: 'Footer',
  components: { BackToTop },
  data() {
    return {
      lang: 'English',
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
    var _lang = localStorage.getItem('language')
    if (_lang) {
      this.lang = _lang.charAt(0).toUpperCase() + _lang.slice(1)
    } else {
      this.lang = 'English'
    }
  },
  methods: {
    onChangeLanguage(e) {
      this.lang = e.charAt(0).toUpperCase() + e.slice(1)
      localStorage.setItem('language', e)
      this.$ml.change(e)
    }
  }
}
</script>
