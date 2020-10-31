<template>
  <div class="about">
    <h1>Welcome, {{ email }}</h1>
    <p>Token: {{ token }}</p>
  </div>
</template>

<script>
// @ is an alias to /src
// import http from '@/utils/axios'
export default {
  name: 'About',
  data() {
    return {
      email: null,
      token: null
    }
  },
  async created() {
    this.email = this.$cookies.get('email')
    this.token = this.$cookies.get('token')
  },
  methods: {
  }
}
</script>
