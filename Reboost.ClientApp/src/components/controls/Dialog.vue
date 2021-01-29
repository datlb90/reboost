<template>
  <div class="c-dialog" />
</template>
<script>
export default {
  props: { visible: { type: Boolean, default: false }},
  data() {
    return {
      clickHandler: (e) => {
        let exist = false
        for (const el of e.path) {
          if (el.classList && el.className == 'c-dropdown-menu') {
            exist = true
            break
          }
        }
        if (!exist) {
          this.toggleMenu('hide')
        }
      }
    }
  },
  mounted() {
    document.addEventListener('click', this.clickHandler)
  },
  destroyed() {
    document.removeEventListener('click', this.clickHandler)
  },
  methods: {

  }
}
</script>
<style scoped>

</style>
