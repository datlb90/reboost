<template>
  <!-- Start Footer Area -->
  <footer
    class="bg-f7fafd"
    style="border-top: 1px solid #d8ebfd; text-align: center; padding-top: 14px;
    padding-bottom: 1px;"
  >
    <p>Công Ty TNHH Reboost. Địa Chỉ: NO05-LK128, Khu đất dịch vụ Cửa Phủ, Phường La Khê, Quận Hà Đông, Thành Phố Hà Nội, Việt Nam.</p>
    <p>Chịu Trách Nhiệm Quản Lý Nội Dung: Nguyễn Thị Huyền Trang. Email: support@reboost.vn. Điện Thoại Liên Lạc: 0393535216</p>
    <p>Mã số doanh nghiệp: 0110542943 do Sở Kế hoạch & Đầu tư TP Hà Nội cấp lần đầu ngày 15/11/2023.</p>
    <p>© 2023 - Bản quyền thuộc về Công ty TNHH Reboost | <a href="/terms">Điều Khoản</a> | <a href="/privacy">Chính Sách Bảo Mật</a></p>
    <!-- <div class="container">
      <div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-footer-widget">
            <div class="logo">
              <a href="/">
                <img src="../../assets/logo/green_logo.png" alt="logo" style="width: 140px;">
              </a>
            </div>
            <p>Quis ipsum suspendisse ultrices gravida. Risus commodo viverra maecenas accumsan lacus vel facilisis.</p>
            <el-dropdown @command="onChangeLanguage">
              <el-button type="primary">
                {{ lang }}
                <i class="el-icon-arrow-down el-icon--right" />
              </el-button>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item command="english">English</el-dropdown-item>
                <el-dropdown-item command="vietnamese">Vietnamese</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-footer-widget pl-5">
            <h3>Company</h3>
            <ul class="list">
              <li><a href="#">About Us</a></li>
              <li><a href="#">Services</a></li>
              <li><a href="#">Features</a></li>
              <li><a href="#">Our Pricing</a></li>
              <li><a href="#">Latest News</a></li>
            </ul>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-footer-widget">
            <h3>Support</h3>
            <ul class="list">
              <li><a href="#">FAQ's</a></li>
              <li><a href="#">Privacy Policy</a></li>
              <li><a href="#">Terms & Condition</a></li>
              <li><a href="#">Community</a></li>
              <li><a href="#">Contact Us</a></li>
            </ul>
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-footer-widget">
            <h3>Address</h3>

            <ul class="footer-contact-info">
              <li><feather type="map-pin" /> Sweetwater Lake Dr, Tampa, FL, 33613, USA</li>
              <li><feather type="mail" /> Email: <a href="#">reboost.ad@gmail.com</a></li>
              <li><feather type="phone-call" /> Phone: <a href="#">+ (321) 984 754</a></li>
            </ul>
            <ul class="social-links">
              <li><a href="#" class="facebook"><feather type="facebook" /></a></li>
              <li><a href="#" class="twitter"><feather type="twitter" /></a></li>
              <li><a href="#" class="instagram"><feather type="instagram" /></a></li>
              <li><a href="#" class="linkedin"><feather type="linkedin" /></a></li>
            </ul>
          </div>
        </div>
        <div>
          <div class="copyright-area">
            <p>Copyright &copy; 2024 Reboost. All Rights Reserved</p>
          </div>
        </div>
      </div>
    </div> -->

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
      lang: 'English'
    }
  },
  mounted() {
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
