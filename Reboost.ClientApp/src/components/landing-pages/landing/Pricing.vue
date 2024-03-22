<template>
  <!-- Start Features Area -->
  <section id="pricing" class="features-area-two ptb-80">
    <div class="container">
      <div class="row h-100 justify-content-center align-items-center">
        <div class="col-lg-7 col-md-12">
          <img src="../../../assets/img/feedback.png" class="wow fadeInDown" data-wow-delay="0.6s" alt="big-monitor">
        </div>
        <div class="col-lg-4 col-md-12 services-content">
          <div class="section-title">
            <h5 style="font-size: 30px;">{{ messageTranslates('pricing', 'title') }}</h5>
            <div class="bar" />
            <p>{{ messageTranslates('pricing', 'description') }}</p>
          </div>
        </div>
      </div>
    </div>
  </section>
  <!-- End Start Features Area -->
</template>

<script>
export default {
  name: 'Pricing'
}
</script>
