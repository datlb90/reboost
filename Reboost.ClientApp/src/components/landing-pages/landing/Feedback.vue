<template>
  <!-- Start ML Feedback Area -->
  <section id="testimony" class="ml-feedback-area ptb-80">
    <div class="container">
      <div class="section-title">
        <h2>{{ messageTranslates('feedback', 'title') }}</h2>
        <div class="bar" />
        <p>{{ messageTranslates('feedback', 'description') }}</p>
      </div>

      <div class="ml-feedback-slides">
        <carousel
          :autoplay="true"
          :loop="true"
          :smart-speed="1000"
          :autoplay-speed="true"
          :autoplay-timeout="5000"
          :per-page-custom="[[0, 1],[576, 1],[768, 1],[1024, 1], [1200, 1]]"
        >
          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/client-image/1.jpg" alt="image">
                <h3>Sarah Taylor</h3>
                <span>CEO at Envato</span>
              </div>
              <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Aliquid ullam harum sit. Accusantium veritatis dolore ducimus illum, cumque excepturi, autem rerum illo amet placeat odit corporis!</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/client-image/3.jpg" alt="image">
                <h3>Steven Smith</h3>
                <span>CEO at Envato</span>
              </div>
              <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Aliquid ullam harum sit. Accusantium veritatis dolore ducimus illum, cumque excepturi, autem rerum illo amet placeat odit corporis!</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/client-image/2.jpg" alt="image">
                <h3>James Eva</h3>
                <span>CEO at Envato</span>
              </div>
              <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Aliquid ullam harum sit. Accusantium veritatis dolore ducimus illum, cumque excepturi, autem rerum illo amet placeat odit corporis!</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>
        </carousel>
      </div>
    </div>

    <div class="shape1"><img src="../../../assets/img/shape1.png" alt="shape"></div>
    <div class="shape2 rotateme"><img src="../../../assets/img/shape2.svg" alt="shape"></div>
    <div class="shape3"><img src="../../../assets/img/shape3.svg" alt="shape"></div>
    <div class="shape4"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape6 rotateme"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape7"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape8 rotateme"><img src="../../../assets/img/shape2.svg" alt="shape"></div>
  </section>
  <!-- End ML Feedback Area -->
</template>

<script>
import { Carousel, Slide } from 'vue-carousel'
export default {
  name: 'Feedback',
  components: { Carousel, Slide }
}
</script>
