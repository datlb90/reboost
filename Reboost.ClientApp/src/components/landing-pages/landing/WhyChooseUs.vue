<template>
  <!-- Start Why Choose US Area -->
  <section id="whyUs" class="why-choose-us ptb-80 pt-0" style="background: #f7fafd; margin-top: 50px;">
    <div class="container">
      <div class="row align-items-center">
        <div class="col-lg-6 col-md-12">
          <div class="section-title">
            <h2>{{ messageTranslates('whychooseUs', 'title') }}</h2>
            <div class="bar" />
            <p>{{ messageTranslates('whychooseUs', 'description') }}</p>
          </div>

          <div class="why-choose-us-image" style="margin-top: 60px;">
            <img src="../../../assets/img/why-choose-us-image/man-stand.png" class="wow fadeInLeft" alt="image">
            <img src="../../../assets/img/why-choose-us-image/database.png" class="wow fadeInRight" alt="image">
            <img src="../../../assets/img/services-left-image/cercle-shape.png" class="rotateme" alt="image">
            <img src="../../../assets/img/why-choose-us-image/main-static.png" class="main-pic wow fadeInUp" alt="image">
          </div>
        </div>

        <div class="col-lg-6 col-md-12" style="margin-top: 60px;">
          <div class="row">
            <div class="col-lg-6 col-md-6 col-sm-6">
              <div class="single-why-choose-us">
                <div class="icon">
                  <i class="flaticon-team" />
                </div>
                <h3>{{ messageTranslates('whychooseUs', 'titleBox1') }}</h3>
                <p>{{ messageTranslates('whychooseUs', 'contentBox1') }}</p>
              </div>
            </div>

            <div class="col-lg-6 col-md-6 col-sm-6">
              <div class="single-why-choose-us">
                <div class="icon">
                  <i class="flaticon-rocket" />
                </div>
                <h3>{{ messageTranslates('whychooseUs', 'titleBox2') }}</h3>
                <p>{{ messageTranslates('whychooseUs', 'contentBox2') }}</p>
              </div>
            </div>

            <div class="col-lg-6 col-md-6 col-sm-6">
              <div class="single-why-choose-us">
                <div class="icon">
                  <i class="flaticon-shield" />
                </div>
                <h3>{{ messageTranslates('whychooseUs', 'titleBox3') }}</h3>
                <p>{{ messageTranslates('whychooseUs', 'contentBox3') }}</p>
              </div>
            </div>

            <div class="col-lg-6 col-md-6 col-sm-6">
              <div class="single-why-choose-us">
                <div class="icon">
                  <i class="flaticon-diamond" />
                </div>
                <h3>{{ messageTranslates('whychooseUs', 'titleBox4') }}</h3>
                <p>{{ messageTranslates('whychooseUs', 'contentBox4') }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
  <!-- End Why Choose US Area -->
</template>

<script>
export default {
  name: 'WhyChooseUs'
}
</script>
