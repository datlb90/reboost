<template>
  <div />
</template>

<script>
export default {
  mounted() {
    this.$store.dispatch('auth/logout')
    this.$router.push('/')
  }
}
</script>

<style>

</style>
