<template>
  <!-- Start Features Area -->
  <section v-if="screenWidth > 780" class="features-area-two ptb-80 bg-f7fafd" style="background-color: rgb(237 251 252);">
    <div class="container">
      <div class="row h-100 justify-content-center align-items-center">
        <div class="col-lg-4 col-md-12 services-content">
          <div class="section-title">
            <h5 style="font-size: 30px;">{{ messageTranslates('unlimittedTopics', 'title') }}</h5>
            <div class="bar" />
            <p>{{ messageTranslates('unlimittedTopics', 'description') }}</p>
          </div>
        </div>

        <div class="col-lg-8 col-md-12">
          <img src="../../../assets/img/topics.png" class="wow fadeInDown" data-wow-delay="0.6s" alt="book-self">
        </div>
      </div>
    </div>
  </section>
  <section v-else class="features-area-two ptb-80 bg-f7fafd" style="background-color: rgb(237 251 252);">
    <div class="container" style="text-align: center;">
      <div class="row h-100 justify-content-center align-items-center" style="text-align: center;">
        <div class="container">
          <div class="section-title">
            <h5 style="font-size: 30px;">{{ messageTranslates('unlimittedTopics', 'title') }}</h5>
            <div class="bar" />
            <p style="text-align: justify;">{{ messageTranslates('unlimittedTopics', 'description') }}</p>
          </div>
        </div>

        <div class="col-lg-8 col-md-12">
          <img src="../../../assets/img/topics.png" class="wow fadeInDown" data-wow-delay="0.6s" alt="book-self">
        </div>
      </div>
    </div>
  </section>
  <!-- End Features Area -->
</template>

<script>
export default {
  name: 'UnlimitedTopic',
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
