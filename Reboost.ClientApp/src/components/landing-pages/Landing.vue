<template>
  <div>
    <Banner />
    <InitialTest />
    <Pricing />
    <!-- <HowItWorks2 />
    <Funfacts />
    <FeaturesArea />
    <Features />
    <UnlimitedTopics />
    <Matching />
    <ProRater /> -->
  </div>
</template>

<script>
import Banner from './landing/Banner'
// import BoxesArea from './landing/BoxesArea'
// import FeaturesArea from './landing/FeaturesArea'
// import Features from './landing/Features'
// import UnlimitedTopics from './landing/UnlimitedTopics'
// import Matching from './landing/Matching'
// import HowItWorks2 from './landing/HowItWorks2'
// import Funfacts from './landing/Funfacts'
// import ProRater from './landing/ProRater'
import InitialTest from './landing/InitialTest'
import Pricing from './landing/Pricing'
import $ from 'jquery'
export default {
  name: 'Developer',
  components: {
    Banner,
    // BoxesArea,
    // FeaturesArea,
    // Features,
    // UnlimitedTopics,
    // Matching,
    // HowItWorks2,
    // Funfacts,
    // ProRater,
    InitialTest,
    Pricing
  },
  mounted() {
    $('.nav-item').find('a').click(function(e) {
      e.preventDefault()
      var $href = $(this).attr('href')
      var $anchor = $('#' + $href).offset()
      if ($anchor) {
        window.scrollTo($anchor.left, $anchor.top - 20)
      }
      // return false
    })

    // Cache selectors
    var topMenu = $('#top-menu')
    var topMenuHeight = topMenu.outerHeight() + 15
    // All list items
    var menuItems = topMenu.find('a')
    // Anchors corresponding to menu items
    var scrollItems = menuItems.map(function() {
      if ($(this).attr('href') != '/rater') {
        var item = $('#' + $(this).attr('href'))
      if (item.length) { return item }
      }
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

    window.scrollTo(0, 1000)
    console.log('test')
  },
  updated: function () {
    this.$nextTick(function () {
      const section = this.getUrlParameter('section')
      if (section) {
        var href = ''
        var anchor = ''
        if (section == 'experience') {
          href = 'initialTest'
          anchor = $('#' + href).offset()
          if (anchor) {
            window.scrollTo(0, anchor.top - 20)
          }
          document.title = 'Reboost - Thi thử IELTS Writing'
        } else if (section == 'new-method') {
          href = 'howItWorks'
          anchor = $('#' + href).offset()
          if (anchor) {
            window.scrollTo(anchor.left, anchor.top - 20)
          }
          document.title = 'Reboost - Phương pháp học mới'
        } else if (section == 'benefit') {
          href = 'benefit'
          anchor = $('#' + href).offset()
          if (anchor) {
            window.scrollTo(0, anchor.top - 20)
          }
          document.title = 'Reboost - Lợi ích của thành viên'
        } else if (section == 'features') {
          href = 'features'
          anchor = $('#' + href).offset()
          if (anchor) {
            window.scrollTo(0, anchor.top - 20)
          }
          document.title = 'Reboost - Chức năng độc đáo'
        }
      }
    })
  },
  methods: {
    getUrlParameter(sParam) {
      const sPageURL = decodeURIComponent(window.location.search.substring(1))
      const sURLVariables = sPageURL.split('&')
      let sParameterName
      let i
      for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=')

        if (sParameterName[0] === sParam) {
          return sParameterName[1] === undefined ? true : sParameterName[1]
        }
      }
    }
  }
}
</script>
