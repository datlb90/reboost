<template>
  <div class="navBar" style="background: white; border-bottom: 1px solid #dcdfe6; height: 46px;">
    <el-button class="logo-btn expand" type="primary" plain style="float: left; margin-top: 3px; margin-left: 20px;">
      Reboost Logo
    </el-button>

    <el-menu
      :default-active="activeIndex"
      class="el-menu-demo"
      mode="horizontal"
      style="margin-left: 25px; float: left;"
      @select="handleSelect"
    >
      <el-menu-item v-for="(route, idx) in permission_routes" :key="route.path" :index="(idx + 1).toString()" :base-path="route.path">
        {{ route.name }}
      </el-menu-item>
    </el-menu>

    <div class="right-menu" style="float: right; margin-top: 12px;" />
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'NavBar',
  data() {
    return {
      activeIndex: '1'
    }
  },
  computed: {
    ...mapGetters([
      'permission_routes'
    ])
  },
  methods: {
    handleSelect(key) {
      this.$router.push(this.permission_routes[key - 1].path).catch(() => {})
    }
  }
}
</script>

<style lang="css" scoped>
.el-menu.el-menu--horizontal{
	border-bottom: none;
}
.el-menu--horizontal>.el-menu-item{
	height: 46px;
    line-height: 46px;
}
</style>
<style>
.el-menu--horizontal>.el-submenu .el-submenu__title{
	height: 46px !important;
    line-height: 46px !important;
}
</style>
