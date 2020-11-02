<template id="home-wrapper">
  <div class="home">
    <div class="background">
      <!-- <img alt="Vue logo" src="../assets/logo.png">
		<HelloWorld msg="Welcome to Your Vue.js App" /> -->
      <!-- <el-button type="primary" style="width: 100%;" @click="login()">
				Sign in to EdVision
			</el-button> -->
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
// import HelloWorld from '@/components/HelloWorld.vue';

export default {
  name: 'Home',
  mounted() {
    // // console.log(document.body
    // document.documentElement.style.height = '100%'
    // // document.html.style.height = '100%';
    // document.body.style.height = '100%'
    // document.getElementById('app').style.height = '100%'
    // document.getElementById('content-wrapper').style.height = '100%'
  },
  methods: {
    login() {
      this.$router.push('/test')
    }
  }
}
</script>

<style scoped>
.home {
	height: 100%;
    width: 100%;
    display: flex;
    flex-direction: column;
}

.background{
	width: 100%;
  display: flex;
  flex-direction: column;
  flex: 1 1 0%;
	background: url("./../assets/background/Background 4.png") center center / auto 100% rgb(245, 245, 245);
	background-position: center center;
}
</style>
