<template>
  <div>
    <Banner />
    <RepairPartner />
    <About />
    <Services />
    <Cta />
    <Funfacts />
    <Team />
    <Feedback />
    <WhyChooseUs />
    <Blog />
  </div>
</template>

<script>
import Banner from './repair-center/Banner'
import RepairPartner from './repair-center/RepairPartner'
import About from './repair-center/About'
import Services from './repair-center/Services'
import Cta from './repair-center/Cta'
import Funfacts from './repair-center/Funfacts'
import Team from './repair-center/Team'
import Feedback from './repair-center/Feedback'
import WhyChooseUs from './repair-center/WhyChooseUs'
import Blog from './repair-center/Blog'

export default {
  name: 'RepairCenter',
  components: {
    Banner,
    RepairPartner,
    About,
    Services,
    Cta,
    Funfacts,
    Team,
    Feedback,
    WhyChooseUs,
    Blog
  }
}
</script>
