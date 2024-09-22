<!-- CoursePlayLayout.vue -->
<template>
  <div>
    <!-- Master layout content, like navigation bars, sidebars, etc. -->
    <!-- @open="handleOpen"
        @close="handleClose" -->
    <div>
      <el-menu
        default-active="1-1"
        class="el-menu-vertical-demo"
        style="float: left; margin-top: 2px; width: 350px; background: #ffffff; background-color: #ffffff; border-right: none;"
      >
        <div style="padding: 20px;">
          <el-button> Back to course home</el-button>
          <div>Course Progress 0%</div>
        </div>
        <el-submenu index="1">
          <template slot="title">
            <i class="el-icon-document" />
            <span>Chapter One</span>
          </template>
          <el-menu-item index="1-1" @click="viewLesson('test-doc-1')">
            <div>
              <i class="el-icon-circle-check" /> Lession 1
            </div>
          </el-menu-item>
          <el-menu-item index="1-2" @click="viewLesson('test-doc-2')">
            <div>
              <i class="el-icon-success" /> Lession 2
            </div>
          </el-menu-item>
          <el-menu-item index="1-3">
            <div>
              <i class="el-icon-lock" /> Lession 2
            </div>
          </el-menu-item>
        </el-submenu>
        <el-submenu index="2">
          <template slot="title">
            <i class="el-icon-document" />
            <span>Chapter Two</span>
          </template>
          <el-menu-item index="2-1">
            <div>
              <i class="el-icon-circle-check" /> Lession 1
            </div>
          </el-menu-item>
          <el-menu-item index="2-2">
            <div>
              <i class="el-icon-success" /> Lession 2
            </div>
          </el-menu-item>
          <el-menu-item index="2-3">
            <div>
              <i class="el-icon-lock" /> Lession 2
            </div>
          </el-menu-item>
        </el-submenu>
      </el-menu>
    </div>
    <router-view />
  </div>
</template>

  <script>
  export default {
    data() {
      return {
        courseName: this.$route.params.courseName,
        course: null
      }
    },
    created() {
      // Ensure the docTitle is correctly set when the component is created
      this.courseName = this.$route.params.courseName
      console.log(this.courseName)
    },
    methods: {
      viewLesson(docName) {
        const targetPath = `/course-play/test/doc/${docName}`
        if (this.$route.path !== targetPath) {
          this.$router.push(targetPath)
        }
      }
    }
  }
  </script>

<style>
.el-submenu__title{
    font-size: 16px !important;
    font-weight: bold;
}
.el-submenu .el-menu-item{
    font-size: 16px;
}
</style>
