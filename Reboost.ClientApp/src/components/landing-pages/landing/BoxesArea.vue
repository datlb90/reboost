<template>
  <!-- Start Boxes Area -->
  <section class="boxes-area" style="margin-top: 10px;">
    <div class="container">
      <div class="section-title">
        <h3>{{ messageTranslates('boxesArea', 'benefit') }}</h3>
        <div class="bar" />
      </div>

      <div class="row">
        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box">
            <div class="icon">
              <i class="flaticon-money" />
            </div>
            <div />
            <h3>{{ messageTranslates('boxesArea', 'title1') }}</h3>
            <p>{{ messageTranslates('boxesArea', 'content1') }}</p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-eb6b3d">
            <div class="icon">
              <i class="flaticon-data-1" />
            </div>
            <h3>{{ messageTranslates('boxesArea', 'title2') }}</h3>
            <p>{{ messageTranslates('boxesArea', 'content2') }}</p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-c679e3">
            <div class="icon">
              <i class="flaticon-dashboard" />
            </div>
            <h3>{{ messageTranslates('boxesArea', 'title3') }}</h3>
            <p>{{ messageTranslates('boxesArea', 'content3') }}</p>
          </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-6">
          <div class="single-box bg-f78acb">
            <div class="icon">
              <i class="flaticon-support" />
            </div>
            <h3>{{ messageTranslates('boxesArea', 'title4') }}</h3>
            <p>{{ messageTranslates('boxesArea', 'content4') }}</p>
          </div>
        </div>

      </div>
    </div>
  </section>
  <!-- End Boxes Area -->
</template>

<script>
export default {
  name: 'BoxesArea'
}
</script>
