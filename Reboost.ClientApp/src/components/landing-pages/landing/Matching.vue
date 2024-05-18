<template>
  <!-- Start Features Area -->
  <section class="features-area-two ptb-80">
    <div class="container">

      <div class="section-title">
        <h2 style="font-size: 30px;">Phản hồi chất lượng từ AI</h2>
        <div class="bar" />
        <p style="max-width: 68%;">Được vận hành bởi model mới nhất của ChatGPT và cấu hình bởi các chuyên gia giàu kinh nghiệm, hệ thống chấm bài tự động của Reboost cung cấp phản hồi cụ thể về điểm mạnh và điểm yếu trong bài viết, giúp bạn cải thiện kỹ năng và tư duy phản biện.</p>
        <img src="../../../assets/img/features/AI Rater 3.jpeg" class="wow fadeInDown" data-wow-delay="0.6s" alt="big-monitor">

      </div>

      <div>
        <div />
        <div class="services-content" />
      </div>
    </div>
  </section>
  <!-- End Start Features Area -->
</template>

<script>
export default {
  name: 'Features',
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
