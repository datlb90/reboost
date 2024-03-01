<template>
  <div>
    <Banner />
    <BoxesArea />
    <FAQ />
  </div>
</template>

<script>
import Banner from './rater/Banner'
import BoxesArea from './rater/BoxesArea'
import FAQ from './rater/FAQ'
import $ from 'jquery'
// import Blog from './developer/Blog'
export default {
  name: 'Developer',
  components: {
    Banner,
    BoxesArea,
    FAQ
  },
  mounted() {
    $('.nav-item').find('a').click(function(e) {
      e.preventDefault()
      var $href = $(this).attr('href')
      var $anchor = $('#' + $href).offset()
      window.scrollTo($anchor.left, $anchor.top)
      // return false
    })

    // $('ul.nav').find('a').click(function() {
    //   var $href = $(this).attr('href')
    //   var $anchor = $('#' + $href).offset()
    //   window.scrollTo($anchor.left, $anchor.top)
    //   // return false
    // })

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
