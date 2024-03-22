<template>
  <!-- Start Features Area -->
  <section class="features-area-two ptb-80 bg-f7fafd" style="padding-top: 120px; padding-bottom: 120px;">
    <div class="container">
      <div class="row h-100 justify-content-center align-items-center">
        <div class="col-lg-4 col-md-12 services-content">
          <div class="section-title">
            <h5 style="font-size: 30px;">{{ messageTranslates('proRater', 'title') }}</h5>
            <div class="bar" />
            <p>{{ messageTranslates('proRater', 'description') }}</p>
          </div>
        </div>

        <div class="col-lg-8 col-md-12">
          <img src="../../../assets/img/features/ProRater.png" class="wow fadeInDown" data-wow-delay="0.6s" alt="book-self">
        </div>
      </div>
    </div>
  </section>
  <!-- End Features Area -->
</template>

<script>
export default {
  name: 'ProRater'
}
</script>
