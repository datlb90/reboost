<template>
  <!-- Start Boxes Area -->
  <section v-if="screenWidth > 1200" id="benefit" class="boxes-area" style="margin-top: 60px; padding-bottom: 0px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-f78acb" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 10px;">Chấm Điểm 4 Tiêu Chí</h3>
            <img src="../../../assets/img/landing/4-criteria.png" style="height: 300px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Sửa lỗi trực tiếp trên bài</h3>
            <img src="../../../assets/img/landing/intext-comment.png" style="height: 295px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-eb6b3d" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Nâng cấp vốn từ vựng</h3>
            <img src="../../../assets/img/landing/vocabulary.png" style="height: 300px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-c679e3" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Bài chữa tham khảo</h3>
            <img src="../../../assets/img/landing/improved-version.png" style="height: 300px; width: 100%;">
          </div>
        </div>
      </div>
    </div>
  </section>
  <section v-else-if="screenWidth > 992" id="benefit" class="boxes-area" style="margin-top: 60px; padding-bottom: 0px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-f78acb" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 10px;">Chấm Điểm 4 Tiêu Chí</h3>
            <img src="../../../assets/img/landing/4-criteria.png" style="height: 300px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Sửa lỗi trực tiếp</h3>
            <img src="../../../assets/img/landing/intext-comment.png" style="height: 295px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-eb6b3d" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Nâng cấp vốn từ vựng</h3>
            <img src="../../../assets/img/landing/vocabulary.png" style="height: 300px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-c679e3" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Bài chữa tham khảo</h3>
            <img src="../../../assets/img/landing/improved-version.png" style="height: 300px; width: 100%;">
          </div>
        </div>
      </div>
    </div>
  </section>
  <section v-else-if="screenWidth > 576" id="benefit" class="boxes-area" style="margin-top: 60px; padding-bottom: 0px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-f78acb" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 10px;">Chấm Điểm 4 Tiêu Chí</h3>
            <img src="../../../assets/img/landing/4-criteria.png" style="height: 400px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Sửa lỗi trực tiếp</h3>
            <img src="../../../assets/img/landing/intext-comment.png" style="height: 395px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-eb6b3d" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Nâng cấp vốn từ vựng</h3>
            <img src="../../../assets/img/landing/vocabulary.png" style="height: 400px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-c679e3" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Bài chữa tham khảo</h3>
            <img src="../../../assets/img/landing/improved-version.png" style="height: 400px; width: 100%;">
          </div>
        </div>
      </div>
    </div>
  </section>
  <section v-else id="benefit" class="boxes-area" style="margin-top: 60px; padding-bottom: 0px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-f78acb" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 10px;">Chấm Điểm 4 Tiêu Chí</h3>
            <img src="../../../assets/img/landing/4-criteria.png" style="height: 500px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box" style="padding: 20px 10px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Sửa lỗi trực tiếp</h3>
            <img src="../../../assets/img/landing/intext-comment.png" style="height: 495px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-eb6b3d" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Nâng cấp vốn từ vựng</h3>
            <img src="../../../assets/img/landing/vocabulary.png" style="height: 500px; width: 100%;">
          </div>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-c679e3" style="padding: 20px 10px; padding-bottom: 15px;">
            <h3 style="margin-top: 0px; font-size: 18px; font-weight: 500; text-align: center; margin-bottom: 15px;">Bài chữa tham khảo</h3>
            <img src="../../../assets/img/landing/improved-version.png" style="height: 500px; width: 100%;">
          </div>
        </div>
      </div>
    </div>
  </section>
  <!-- End Boxes Area -->
</template>

<script>
export default {
  name: 'BoxesArea',
  data() {
    return {
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
  }
}

</script>
