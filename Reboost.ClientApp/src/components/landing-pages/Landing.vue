<template>
  <div>
    <Banner />
    <BoxesArea />
    <HowItWorks2 />
    <WhyChooseUs />
    <HowItWorks />
    <FeaturesArea />
    <Features />
    <Team />
    <Funfacts />
    <Pricing />
    <Feedback />

    <!-- Start Ready To Talk Area -->
    <section class="ready-to-talk">
      <div class="container">
        <h3>Ready to talk?</h3>
        <p>Our team is here to answer your question about StartP</p>
        <a href="#" class="btn btn-primary">Contact Us</a>
        <span><a href="#">Or, get started now with a free trial</a></span>
      </div>
    </section>
    <!-- End Ready To Talk Area -->

    <Partner />
    <Blog />
  </div>
</template>

<script>
import Banner from './landing/Banner'
import BoxesArea from './landing/BoxesArea'
import FeaturesArea from './developer/FeaturesArea'
import Features from './developer/Features'
import HowItWorks from './landing/HowItWorks'
import HowItWorks2 from './landing/HowItWorks2'
import WhyChooseUs from './landing/WhyChooseUs'
import Team from './developer/Team'
import Funfacts from './developer/Funfacts'
import Pricing from './developer/Pricing'
import Feedback from './developer/Feedback'
import Partner from './developer/Partner'
import Blog from './developer/Blog'
export default {
  name: 'Developer',
  components: {
    Banner,
    BoxesArea,
    FeaturesArea,
    Features,
    HowItWorks,
    HowItWorks2,
    WhyChooseUs,
    Team,
    Funfacts,
    Pricing,
    Feedback,
    Partner,
    Blog
  }
}
</script>
