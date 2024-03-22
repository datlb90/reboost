<template>
  <!-- Start IoT Why Choose Us Area -->
  <section id="howItWorks" class="iot-why-choose-us ptb-80 pb-0">
    <div class="container">
      <div class="section-title">
        <h3 style="color: white;">{{ messageTranslates('howItWork2', 'title') }}</h3>
        <div class="bar" />
        <p style="max-width: 950px;">{{ messageTranslates('howItWork2', 'description') }}</p>
      </div>

      <div class="row">
        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="single-iot-box">
            <div class="icon">
              <img src="../../../assets/img/icon1.png" alt="image">
            </div>
            <h3>{{ messageTranslates('howItWork2', 'titleBox1') }}</h3>
            <p>{{ messageTranslates('howItWork2', 'contentBox1') }}</p>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="single-iot-box">
            <div class="icon">
              <img src="../../../assets/img/icon2.png" alt="image">
            </div>
            <h3>{{ messageTranslates('howItWork2', 'titleBox2') }}</h3>
            <p>{{ messageTranslates('howItWork2', 'contentBox2') }}</p>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6 offset-lg-0 offset-md-3 offset-sm-3">
          <div class="single-iot-box">
            <div class="icon">
              <img src="../../../assets/img/icon3.png" alt="image">
            </div>
            <h3>{{ messageTranslates('howItWork2', 'titleBox3') }}</h3>
            <p>{{ messageTranslates('howItWork2', 'contentBox3') }}</p>
          </div>
        </div>
      </div>
    </div>
  </section>
  <!-- End IoT Why Choose Us Area -->
</template>

<script>
export default {
  name: 'HowItWorks2'
}
</script>
