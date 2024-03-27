<template>
  <!-- Start Navbar Area -->
  <header v-if="screenWidth > 1200" id="header" :class="['headroom', {'is-sticky': isSticky}]">
    <div id="navigation-bar" class="startp-nav">
      <div class="container">
        <div>
          <div style="width: 140px; float:left; ">
            <router-link to="/" href="banner" style="padding-top: 0px;">
              <img src="../../assets/logo/logo.png" alt="logo" style="width: 140px;">
            </router-link>
          </div>

          <div id="top-menu" style="display: flex; width: 560px; float: left; margin-left: 60px;">
            <div class="nav-item">
              <a href="benefit" class="nav-link">Lợi ích</a>
            </div>
            <div class="nav-item">
              <a href="howItWorks" class="nav-link">Phương pháp mới</a>
            </div>
            <div class="nav-item">
              <a href="features" class="nav-link">Chức năng</a>
            </div>
            <div>
              <a href="/rater" class="nav-link"><span style="font-weight: 500; font-size: 15px;">Cho giáo viên</span></a>
            </div>
            <div class="nav-item">
              <a class="nav-link" style="cursor: pointer;" @click.prevent="openContactDialog()">Liên hệ</a>
            </div>
            <!-- <div>
              <el-dropdown class="lang-dropdown" style="margin-left: 15px; margin-top: 8px;" placement="bottom" @command="onChangeLanguage">
                <flag v-if="lang == 'English'" iso="vn" style="border-radius: 3px; height: 15px; width: 15px;" />
                <flag v-else iso="gb" style="border-radius: 3px;  height: 15px; width: 15px;" />
                <el-dropdown-menu slot="dropdown" class="lang-dropdown-menu">
                  <el-dropdown-item v-if="lang == 'English'" command="vietnamese"><flag iso="vn" style="border-radius: 2px; margin-right: 6px;" />Tiếng Việt</el-dropdown-item>
                  <el-dropdown-item v-else command="english"><flag iso="gb" style="border-radius: 2px; margin-right: 6px;" />English</el-dropdown-item>
                </el-dropdown-menu>
              </el-dropdown>
            </div> -->
          </div>

          <div class="menu-btns" style="float: right;">
            <a style="margin-right: 10px; padding: 9px 15px;" class="btn btn-light" href="/login">
              Đăng nhập
            </a>
            <el-button
              class="btn btn-gradient"
              style="padding: 10px 18px;"
              @click.prevent="gotoRegister()"
            >
              Đăng ký
            </el-button>
          </div>
        </div>
      </div>
    </div>

    <contact-dialog ref="contactDialog" />
    <review-request-dialog
      ref="reviewRequestDialog"
    />
  </header>
  <header v-else-if="screenWidth > 992" id="header" :class="['headroom', {'is-sticky': isSticky}]">
    <div id="navigation-bar" class="startp-nav">
      <div class="container">
        <div>
          <div style="width: 140px; float:left; ">
            <router-link to="/" href="banner" style="padding-top: 0px;">
              <img src="../../assets/logo/logo.png" alt="logo" style="width: 140px;">
            </router-link>
          </div>
          <div id="top-menu" style="display: flex; width: 550px; float: left; margin-left: 10px;">

            <div class="nav-item">
              <a href="benefit" class="nav-link">Lợi ích</a>
            </div>
            <div class="nav-item">
              <a href="howItWorks" class="nav-link">Phương pháp mới</a>
            </div>
            <div class="nav-item">
              <a href="features" class="nav-link">Chức năng</a>
            </div>
            <div>
              <a href="/rater" class="nav-link"><span style="font-weight: 500; font-size: 15px;">Cho giáo viên</span></a>
            </div>
            <div class="nav-item">
              <a class="nav-link" style="cursor: pointer;" @click.prevent="openContactDialog()">Liên hệ</a>
            </div>
          </div>

          <div class="menu-btns" style="float: right;">
            <a style="margin-right: 10px; padding: 9px 15px;" class="btn btn-light" href="/login">
              Đăng nhập
            </a>
            <el-button
              class="btn btn-gradient"
              style="padding: 10px 15px;"
              @click.prevent="gotoRegister()"
            >
              Đăng ký
            </el-button>
          </div>
        </div>
      </div>
    </div>

    <contact-dialog ref="contactDialog" />
    <review-request-dialog
      ref="reviewRequestDialog"
    />
  </header>
  <header v-else id="header" :class="['headroom', {'is-sticky': isSticky}]">
    <div id="navigation-bar" class="startp-nav">

      <div class="container">
        <div>
          <div style="width: 140px; float:left; ">
            <router-link to="/" href="banner" style="padding-top: 0px;">
              <img src="../../assets/logo/logo.png" alt="logo" style="width: 140px;">
            </router-link>
          </div>

          <el-dropdown style="float: right;" :hide-on-click="true">
            <span class="el-dropdown-link" style="cursor: pointer;">
              <i class="el-icon-menu" style="font-size: 40px;" />
            </span>

            <el-dropdown-menu id="top-menu" slot="dropdown">
              <el-dropdown-item command="a">
                <div class="nav-item">
                  <a href="benefit" class="nav-link">Lợi ích</a>
                </div>
              </el-dropdown-item>

              <el-dropdown-item command="a">
                <div class="nav-item">
                  <a href="howItWorks" class="nav-link">Phương pháp mới</a>
                </div>
              </el-dropdown-item>
              <el-dropdown-item command="b">
                <div class="nav-item">
                  <a href="features" class="nav-link">Chức năng</a>
                </div>
              </el-dropdown-item>
              <el-dropdown-item command="c">
                <div>
                  <a href="/rater" class="nav-link"><span style="font-weight: 500; font-size: 15px;">Trở thành giáo viên</span></a>
                </div>
              </el-dropdown-item>
              <el-dropdown-item command="d">
                <div class="nav-item">
                  <a class="nav-link" style="cursor: pointer;" @click.prevent="openContactDialog()">Liên hệ</a>
                </div>
              </el-dropdown-item>
            </el-dropdown-menu>

          </el-dropdown>

          <div class="menu-btns" style="float: right; margin-right: 5px; margin-top: 2px;">
            <a style="margin-right: 10px; padding: 7px 15px;" class="btn btn-light" href="/login">
              Đăng nhập
            </a>
            <el-button
              class="btn btn-gradient"
              style="padding: 8px 10px;"
              @click.prevent="gotoRegister()"
            >
              Đăng ký
            </el-button>
          </div>

        </div>
      </div>
    </div>

    <contact-dialog ref="contactDialog" />
    <review-request-dialog
      ref="reviewRequestDialog"
    />
  </header>
  <!-- End Navbar Area -->
</template>

<script>
import ContactDialog from '../../components/controls/ContactDialog.vue'
import ReviewRequestDialog from '../../components/controls/ReviewRequestDialog.vue'
export default {
  name: 'LandingHeader',
  components: {
    'contact-dialog': ContactDialog,
    'review-request-dialog': ReviewRequestDialog
  },
  data() {
    return {
      isSticky: false,
      lang: '',
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
    this.$store.dispatch('auth/clearUser')
    const that = this
    window.addEventListener('scroll', () => {
      const scrollPos = window.scrollY
      // eslint-disable-next-line no-console
      if (scrollPos >= 100) {
        that.isSticky = true
      } else {
        that.isSticky = false
      }
    })
  },
  methods: {
    gotoRegister() {
      return this.$router.push({ path: '/register' })
    },
    openRequestReviewDialog() {
      this.$refs.reviewRequestDialog?.openDialog()
    },
    openContactDialog(e) {
      this.$refs.contactDialog?.openDialog()
    },
    onChangeLanguage(e) {
      this.lang = e.charAt(0).toUpperCase() + e.slice(1)
      console.log(this.lang)
      localStorage.setItem('language', e)
      this.$ml.change(e)
    }
  }
}
</script>

<style scoped>
.nav-item:active{
  color: #44ce6f;
}

.lang-dropdown-menu{
  z-index: 10000 !important;
}

.nav-item a {
  font-weight: 500;
  font-size: 15px;

}

.menu-btns .btn.btn-light {
    background: transparent;
    border: 2px dashed #cdf1d8;
}

.menu-btns .btn.btn-light::before {
  background: #27bebe;
}

.menu-btns .btn.btn-light::after {
  background: #27bebe;
}

.menu-btns .btn.btn-light:hover{
  color: white;
}

.menu-btns .btn.btn-light:hover,
.menu-btns .btn.btn-light:focus {
  border-color: #27bebe;
}
</style>
