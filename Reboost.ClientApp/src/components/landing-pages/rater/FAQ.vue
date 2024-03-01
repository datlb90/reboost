<template>
  <div id="process" class="boxes-area" style="padding-top: 50px;">
    <div class="container">
      <div style="margin-top: 100px;">
        <div class="section-title">
          <h3> Quy trình đăng ký đơn giản</h3>
          <div class="bar" />
        </div>
        <el-steps :active="4" style="margin-top: 60px; margin-bottom: 60px; color: #6084a4 !important;">
          <el-step icon="far fa-user-circle">
            <div slot="icon">
              <i class="far fa-user-circle" style="font-size: 40px;" />
            </div>
            <div slot="title" style="font-size: 16px; font-weight: bold;">Bước 1</div>
            <div slot="description" style="font-size: 15px;">Tạo tài khoản giảo viên</div>
          </el-step>
          <el-step icon="fas fa-file-upload">
            <div slot="icon">
              <i class="fas fa-file-upload" style="font-size: 40px;" />
            </div>
            <div slot="title" style="font-size: 16px; font-weight: bold;">Bước 2</div>
            <div slot="description" style="font-size: 15px;">Nộp hồ sơ và chứng chỉ</div>
          </el-step>

          <el-step icon="far fa-check-circle">
            <div slot="icon">
              <i class="far fa-check-circle" style="font-size: 40px;" />
            </div>
            <div slot="title" style="font-size: 16px; font-weight: bold;">Bước 3</div>
            <div slot="description" style="font-size: 15px;">Hoàn thành khoá đào tạo</div>
          </el-step>

          <el-step icon="fas fa-edit">
            <div slot="icon">
              <i class="fas fa-edit" style="font-size: 40px;" />
            </div>
            <div slot="title" style="font-size: 16px; font-weight: bold;">Bước 4</div>
            <div slot="description" style="font-size: 15px;">Bắt đầu chấm bài</div>
          </el-step>

        </el-steps>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Faq',
  components: {
  },
  data() {
    return {
    }
  }
}
</script>
