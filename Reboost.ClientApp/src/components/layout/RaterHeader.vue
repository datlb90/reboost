<template>
  <header v-if="screenWidth > 1200" id="header" :class="['headroom', {'is-sticky': isSticky}]">
    <div id="navigation-bar" class="startp-nav">
      <div class="container">
        <div>
          <div style="width: 140px; float:left; ">
            <router-link to="/" href="banner" style="padding-top: 0px;">
              <img src="../../assets/logo/logo.png" alt="logo" style="width: 140px;">
            </router-link>
          </div>
          <div id="top-menu" style="display: flex; width: 470px; float: left; margin-left: 60px;">
            <div class="nav-item">
              <a href="benefit" class="nav-link" style="cursor: pointer;">Lợi ích cho giáo viên</a>
            </div>
            <div class="nav-item">
              <a href="process" class="nav-link" style="cursor: pointer;">Quy Trình</a>
            </div>
            <div class="nav-item">
              <a class="nav-link" style="cursor: pointer;" @click.prevent="openContactDialog()">Liên hệ</a>
            </div>
          </div>
          <div class="menu-btns" style="float: right;">
            <div class="others-option">
              <a href="/rater/login" class="btn btn-gradient" style="padding: 10px 30px;">Đăng Nhập Tài Khoản Giáo Viên</a>
            </div>
          </div>
        </div>
      </div>
    </div>
    <contact-dialog ref="contactDialog" />
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
          <div id="top-menu" style="display: flex; width: 470px; float: left; margin-left: 20px;">
            <div class="nav-item">
              <a href="benefit" class="nav-link" style="cursor: pointer;">Lợi ích</a>
            </div>
            <div class="nav-item">
              <a href="process" class="nav-link" style="cursor: pointer;">Quy Trình</a>
            </div>
            <div class="nav-item">
              <a class="nav-link" style="cursor: pointer;" @click.prevent="openContactDialog()">Liên hệ</a>
            </div>
          </div>
          <div class="menu-btns" style="float: right;">
            <div class="others-option">
              <a href="/rater/login" class="btn btn-gradient" style="padding: 10px 30px;">Đăng Nhập</a>
            </div>
          </div>
        </div>
      </div>
    </div>
    <contact-dialog ref="contactDialog" />
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
              <el-dropdown-item command="o">
                <div class="nav-item">
                  <a href="/rater/login" class="nav-link">Đăng Nhập</a>
                </div>
              </el-dropdown-item>
              <el-dropdown-item command="a">
                <div class="nav-item">
                  <a href="benefit" class="nav-link">Lợi ích</a>
                </div>
              </el-dropdown-item>
              <el-dropdown-item command="b">
                <div class="nav-item">
                  <a href="process" class="nav-link">Quy Trình</a>
                </div>
              </el-dropdown-item>
              <el-dropdown-item command="d">
                <div class="nav-item">
                  <a class="nav-link" style="cursor: pointer;" @click.prevent="openContactDialog()">{{ messageTranslates('landingHeader', 'contact') }}</a>
                </div>
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <div class="menu-btns" style="float: right; margin-right: 10px;">
            <div class="others-option">
              <a href="/rater/register" class="btn btn-gradient" style="padding: 10px 30px;">Đăng Ký</a>
            </div>
          </div>
        </div>
      </div>
    </div>
    <contact-dialog ref="contactDialog" />
  </header>

</template>

<script>
import ContactDialog from '../../components/controls/ContactDialog.vue'
export default {
  name: 'RaterHeader',
  components: {
    'contact-dialog': ContactDialog
  },
  data() {
    return {
      isSticky: false,
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
    openContactDialog(e) {
      this.$refs.contactDialog?.openDialog()
    }
  }
}
</script>

<style scoped>
.nav-item:active{
  color: #44ce6f;
}
</style>
