<template>
  <div class="app-wrapper">
    <div class="main-container">
      <div>
        <navbar />
      </div>
      <app-main />
    </div>
  </div>
</template>

<script>
import { AppMain, Navbar } from './components'

export default {
  name: 'Layout',
  components: {
    AppMain,
    Navbar
  }
}
</script>

<style lang="css" scoped>
  .app-wrapper {
    position: relative;
    height: 100%;
    width: 100%;
  }
</style>

