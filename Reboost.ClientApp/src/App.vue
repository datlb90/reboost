<template>
  <div id="app">
    <div v-if="!$route.meta.plainLayout">
      <layout />
    </div>
    <div v-if="$route.meta.plainLayout" id="content-wrapper">
      <router-view />
    </div>

    <!-- <div id="nav">
			<router-link to="/">
				Home
			</router-link> |
			<router-link to="/about">
				About
			</router-link> |
			<router-link to="/test">
				Test
			</router-link> |
			<router-link to="/document">
				Document
			</router-link>
		</div>
		<router-view /> -->
  </div>
</template>

<script>
import layout from '@/layout/Index.vue'
// import store from './store'
// import { mapState, mapGetters, mapActions } from 'vuex'
export default {
  name: 'App',
  components: {
    layout
  }
}
</script>

<style>
body{
	margin: 0px;
}
@font-face {
  font-family: "GoogleRoboto";
  src: url("assets/Roboto/Roboto-Regular.ttf") format("truetype");
}

#app {
  font-family: 'GoogleRoboto';
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
