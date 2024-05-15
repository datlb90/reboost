<template>
  <!-- Start Boxes Area -->
  <section v-if="screenWidth > 992" id="benefit" class="boxes-area" style="margin-top: 60px; padding-bottom: 0px;">
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
  <section v-else id="benefit" class="boxes-area" style="margin-bottom: 50px; padding-top: 40px;">
    <div class="container">
      <div class="section-title">
        <div class="bar" />
        <h3 style="font-size: 25px;">Lợi ích của thành viên</h3>
      </div>

      <div class="row">
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-f78acb" style="text-align: center;">
            <div class="icon">
              <i class="flaticon-support" />
            </div>
            <h3 style="font-size: 18px;">Phản hồi miễn phí, chuyên sâu, và chất lượng</h3>
            <p style="text-align: justify;">
              Nhận phản hồi nhanh và chất lượng giúp bạn sớm cải thiện kỹ năng viết với các hình thức chấm bài được cá nhân hoá, bao gồm đánh giá chi tiết được cung cấp miễn phí bởi ChatGPT-4 và đánh giá chuyên sâu từ các giáo viên giàu kinh nghiệm.
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box" style="text-align: center;">
            <div class="icon">
              <i class="flaticon-money" />
            </div>
            <h3>Đa dạng chủ đề viết đã được phân loại</h3>
            <p style="text-align: justify;">
              Reboost khuyến khích bạn luyện tập mỗi ngày bằng cách cung cấp miễn phí bộ chủ đề viết đa dạng đã được phân loại theo kiểu đề và mức độ khó, giúp các bạn dễ dàng lựa chọn chủ để viết phù hợp với năng lực của mình.
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-eb6b3d" style="text-align: center;">
            <div class="icon">
              <i class="flaticon-data-1" />
            </div>
            <h3 style="font-size: 18px;">Tài nguyên hữu dụng cho từng chủ đề</h3>
            <p style="text-align: justify;">
              Với từng chủ đề viết, Reboost cung cấp cho bạn ý tưởng phát triển bài, gợi ý xây dựng bố cục, các từ vựng hữu dụng, và bài viết mẫu cho mỗi band score. Với Reboost, bạn sẽ luôn có đầy đủ tài nguyên cần thiết để phát triển bài viết của mình.
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-c679e3" style="text-align: center;">
            <div class="icon">
              <i class="flaticon-dashboard" />
            </div>
            <h3 style="font-size: 18px;">Phương pháp luyện tập mới hiệu quả</h3>
            <p style="text-align: justify;">
              Reboost mang tới cho bạn cơ hội cải thiện kỹ năng bằng cách cung cấp đánh giá. Quá trình này tạo điều kiện giúp bạn học hỏi các kỹ thuật, cách diễn đạt mới, và nhận ra điểm mạnh, điểm yếu trong bài viết của mình để từ đó có hướng cải thiện.
            </p>
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
