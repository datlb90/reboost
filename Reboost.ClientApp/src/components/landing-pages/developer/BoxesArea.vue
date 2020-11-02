<template>
  <!-- Start Boxes Area -->
  <section class="boxes-area">
    <div class="container">
      <div class="boxes-slides">
        <carousel
          :autoplay="true"
          :loop="true"
          :autoplay-speed="true"
          :pagination-enabled="false"
          :autoplay-timeout="5000"
          :per-page-custom="[[0, 1],[576, 2],[768, 2],[1200, 4], [1500, 5]]"
        >
          <slide>
            <div class="single-box">
              <div class="icon">
                <feather type="airplay" />
              </div>
              <h3>Fully Customizable</h3>
              <p>Lorem ipsum dolor sit amet elit, adipiscing, sed do eiusmod tempor incididunt ut labore dolore magna.</p>
            </div>
          </slide>

          <slide>
            <div class="single-box bg-f78acb">
              <div class="icon">
                <feather type="pen-tool" />
              </div>
              <h3>Professional Tools</h3>
              <p>Lorem ipsum dolor sit amet elit, adipiscing, sed do eiusmod tempor incididunt ut labore dolore magna.</p>
            </div>
          </slide>

          <slide>
            <div class="single-box bg-c679e3">
              <div class="icon">
                <feather type="pie-chart" />
              </div>
              <h3>Marketing</h3>
              <p>Lorem ipsum dolor sit amet elit, adipiscing, sed do eiusmod tempor incididunt ut labore dolore magna.</p>
            </div>
          </slide>

          <slide>
            <div class="single-box bg-eb6b3d">
              <div class="icon">
                <feather type="server" />
              </div>
              <h3>Easy To Use</h3>
              <p>Lorem ipsum dolor sit amet elit, adipiscing, sed do eiusmod tempor incididunt ut labore dolore magna.</p>
            </div>
          </slide>

          <slide>
            <div class="single-box bg-eb6b3d">
              <div class="icon">
                <feather type="target" />
              </div>
              <h3>App Integration</h3>
              <p>Lorem ipsum dolor sit amet elit, adipiscing, sed do eiusmod tempor incididunt ut labore dolore magna.</p>
            </div>
          </slide>
        </carousel>
      </div>
    </div>
  </section>
  <!-- End Boxes Area -->
</template>

<script>
import { Carousel, Slide } from 'vue-carousel'
export default {
  name: 'BoxesArea',
  components: { Carousel, Slide }
}
</script>
