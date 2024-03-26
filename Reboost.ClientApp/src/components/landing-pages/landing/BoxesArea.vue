<template>
  <!-- Start Boxes Area -->
  <section v-if="screenWidth > 780" class="boxes-area" style=" margin-bottom: 120px;">
    <div class="container">
      <div class="section-title">
        <div class="bar" />
        <h3>Lợi ích khi luyện viết IELTS cùng Reboost</h3>
      </div>

      <div class="row">

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-f78acb">
            <div class="icon">
              <i class="flaticon-support" />
            </div>
            <h3>Phản hồi miễn phí, chuyên sâu, và chất lượng</h3>
            <p>
              Nhận phản hồi chất lượng giúp bạn sớm cải thiện với các hình thức chấm bài được cá nhân hoá bao gồm đánh giá chi tiết được cung cấp miễn phí bởi ChatGPT-4 và đánh giá chuyên sâu từ các giáo viên giàu kinh nghiệm.
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box">
            <div class="icon">
              <i class="flaticon-money" />
            </div>
            <div />
            <h3>Đa dạng chủ đề viết đã được phân loại</h3>
            <p>
              Reboost khuyến khích học viên luyện tập mỗi ngày bằng cách cung cấp miễn phí bộ chủ đề viết đa dạng đã được phân loại theo kiểu đề và mức độ khó, giúp học viên dễ dàng lựa chọn chủ để viết phù hợp với năng lực của mình.
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-eb6b3d">
            <div class="icon">
              <i class="flaticon-data-1" />
            </div>
            <h3>Tài nguyên hữu dụng cho từng chủ đề</h3>
            <p>
              Cho từng chủ đề viết, Reboost cung cấp ý tưởng phát triển bài, gợi ý xây dựng bố cục, các từ vựng hữu dụng, và rất nhiều bài viết mẫu. Với Reboost, bạn sẽ luôn có đầy đủ tài nguyên cần thiết để phát triển bài viết của mình.
            </p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-c679e3">
            <div class="icon">
              <i class="flaticon-dashboard" />
            </div>
            <h3>Phương pháp luyện tập mới hiệu quả</h3>
            <p>
              Reboost mang tới cơ hội cải thiện kỹ năng bằng cách cung cấp đánh giá. Quá trình này giúp bạn học hỏi được các kỹ thuật, cách diễn đạt mới, và nhận ra điểm mạnh, điểm yếu trong bài viết của mình để từ đó có hướng cải thiện.
            </p>
          </div>
        </div>

      </div>
    </div>
  </section>
  <section v-else class="boxes-area" style="margin-bottom: 50px;">
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
