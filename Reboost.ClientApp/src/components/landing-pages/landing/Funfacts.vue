<template>
  <!-- Start Fun Facts Area -->
  <section id="features" class="funfacts-area ptb-80" style="padding-bottom: 20px;">
    <div class="container">
      <div class="section-title">
        <h2>{{ messageTranslates('funfacts', 'title') }}</h2>
        <div class="bar" />
        <p>{{ messageTranslates('funfacts', 'description') }}</p>
      </div>
    </div>
  </section>
  <!-- End Fun Facts Area -->
</template>

<script>

export default {
  name: 'Funfacts'
}
</script>
