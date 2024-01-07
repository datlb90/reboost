<template>
  <div>
    <Banner />
    <BoxesArea />
    <HowItWorks2 />
    <WhyChooseUs />
    <Funfacts />
    <FeaturesArea />
    <Features />
    <RevisionCenter />
    <Matching />
    <!-- <Feedback />
    <Blog />
    <FAQ /> -->
  </div>
</template>

<script>
import Banner from './landing/Banner'
import BoxesArea from './landing/BoxesArea'
import FeaturesArea from './landing/FeaturesArea'
import Features from './landing/Features'
import RevisionCenter from './landing/RevisionCenter'
import Matching from './landing/Matching'
import HowItWorks2 from './landing/HowItWorks2'
import Funfacts from './landing/Funfacts'
// import WhyChooseUs from './landing/WhyChooseUs'

// import Blog from './landing/Blog'
// import Feedback from './landing/Feedback'
// import FAQ from './landing/FAQ'
import $ from 'jquery'
// import Blog from './developer/Blog'
export default {
  name: 'Developer',
  components: {
    Banner,
    BoxesArea,
    FeaturesArea,
    Features,
    RevisionCenter,
    Matching,
    HowItWorks2,
    Funfacts
    // WhyChooseUs,
    // Blog,
    // Feedback,
    // FAQ
  },
  mounted() {
    $('.navbar').find('a').click(function() {
      var $href = $(this).attr('href')
      var $anchor = $('#' + $href).offset()
      window.scrollTo($anchor.left, $anchor.top)
      return false
    })

    $('ul.nav').find('a').click(function() {
      var $href = $(this).attr('href')
      var $anchor = $('#' + $href).offset()
      window.scrollTo($anchor.left, $anchor.top)
      return false
    })

    // Cache selectors
    var topMenu = $('#top-menu')
    var topMenuHeight = topMenu.outerHeight() + 15
    // All list items
    var menuItems = topMenu.find('a')
    // Anchors corresponding to menu items
    var scrollItems = menuItems.map(function() {
      var item = $('#' + $(this).attr('href'))
      if (item.length) { return item }
    })

    // Bind to scroll
    $(window).scroll(function() {
      // Get container scroll position
      var fromTop = $(this).scrollTop() + topMenuHeight

      // Get id of current scroll item
      var cur = scrollItems.map(function() {
        if ($(this).offset().top < fromTop) { return this }
      })
      // Get the id of the current element
      cur = cur[cur.length - 1]
      var id = cur && cur.length ? cur[0].id : ''
      // Set/remove active class
      menuItems.removeClass('active').filter("[href='" + id + "']").addClass('active')
    })
  }
}
</script>
