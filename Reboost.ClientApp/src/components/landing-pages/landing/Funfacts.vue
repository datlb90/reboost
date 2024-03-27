<template>
  <!-- Start Fun Facts Area -->
  <section id="features" class="funfacts-area ptb-80" style="padding-bottom: 10px;">
    <div v-if="screenWidth > 780" class="container">
      <div class="section-title">
        <h3>{{ messageTranslates('funfacts', 'title') }}</h3>
        <div class="bar" />
      </div>
    </div>
  </section>
  <!-- End Fun Facts Area -->
</template>

<script>

export default {
  name: 'Funfacts',
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
