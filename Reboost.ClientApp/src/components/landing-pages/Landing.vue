<template>
  <div>
    <Banner />
    <InitialTest />
    <Feedback />
  </div>
</template>

<script>
import Banner from './landing/Banner'
import InitialTest from './landing/InitialTest'
// import Pricing from './landing/Pricing'
import Feedback from './landing/Feedback'
import $ from 'jquery'
export default {
  name: 'Developer',
  components: {
    Banner,
    InitialTest,
    // Pricing,
    Feedback
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
      if ($(this).attr('href') != '/rater' && $(this).attr('href') != '/sample/feedback/basic' && $(this).attr('href') != '/pricing') {
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
  },
  updated: function () {
    this.$nextTick(function () {
      const section = this.getUrlParameter('section')
      if (section) {
        var href = ''
        var anchor = ''
        if (section == 'experience') {
          href = 'experience'
          anchor = $('#' + href).offset()
          if (anchor) {
            window.scrollTo(0, anchor.top - 20)
          }
          document.title = 'Reboost - Trải nghiệm'
        }
        // else if (section == 'new-method') {
        //   href = 'howItWorks'
        //   anchor = $('#' + href).offset()
        //   if (anchor) {
        //     window.scrollTo(anchor.left, anchor.top - 20)
        //   }
        //   document.title = 'Reboost - Phương pháp học mới'
        // } else if (section == 'benefit') {
        //   href = 'benefit'
        //   anchor = $('#' + href).offset()
        //   if (anchor) {
        //     window.scrollTo(0, anchor.top - 20)
        //   }
        //   document.title = 'Reboost - Lợi ích của thành viên'
        // } else if (section == 'features') {
        //   href = 'features'
        //   anchor = $('#' + href).offset()
        //   if (anchor) {
        //     window.scrollTo(0, anchor.top - 20)
        //   }
        //   document.title = 'Reboost - Chức năng độc đáo'
        // }
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
