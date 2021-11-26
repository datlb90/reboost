<template>
  <!-- Start Features Area -->
  <section class="features-area-two ptb-80 bg-f7fafd" style="background-color: #f9f6f6;">
    <div class="container">
      <div class="row h-100 justify-content-center align-items-center">
        <div class="col-lg-4 col-md-12 services-content">
          <div class="section-title">
            <h2>{{ messageTranslates('revisionCenter', 'title') }}</h2>
            <div class="bar" />
            <p>{{ messageTranslates('revisionCenter', 'description') }}</p>
          </div>
        </div>

        <div class="col-lg-8 col-md-12">
          <img src="../../../assets/img/features/Revision Center.png" class="wow fadeInDown" data-wow-delay="0.6s" alt="book-self">
        </div>
      </div>
    </div>
  </section>
  <!-- End Features Area -->
</template>

<script>
export default {
  name: 'RevisionCenter'
}
</script>
