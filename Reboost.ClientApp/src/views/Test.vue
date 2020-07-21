<template>
	<div class="test">
		<div>
			<p>Hello {{ username }}</p>
		</div>

		<el-form ref="form" :model="form" label-width="120px" style="width: 30%; margin: auto;">
			<el-form-item label="Username">
				<el-input v-model="form.username" />
			</el-form-item>
			<el-form-item label="Password">
				<el-input v-model="form.password" type="password" autocomplete="off" />
			</el-form-item>
			<el-form-item>
				<el-button type="primary" @click="login()">
					Sign In
				</el-button>
				<el-button type="primary" plain @click="apiCall()">
					Api Call
				</el-button>
				<el-button type="primary" plain @click="logout()">
					Logout
				</el-button>
			</el-form-item>
		</el-form>

		
		<el-table
			v-if="weathers && weathers.length > 0"
			:data="weathers"
			border
			style="width: 50%; margin: auto; margin-top: 20px;"
		>
			<el-table-column
				prop="date"
				label="Date"
				width="180"
			/>
			<el-table-column
				prop="temperatureC"
				label="Temperature C"
				width="180"
			/>
			<el-table-column
				prop="temperatureF"
				label="Temperature F"
			/>
			<el-table-column
				prop="summary"
				label="Summary"
			/>
		</el-table>
	</div>
</template>
<script>
// @ is an alias to /src
import http from '@/utils/axios';
export default {
	name: 'Test',
	data() {
		return {
			weathers: [],
			user: null,
			mgr: null,
			username: 'guest',
			form: {
				username: '',
				password: ''
			},
			access_token: null,
		};
	},
	async created(){
	},
	methods:{
		login() {
			let that = this;
	  		http.post('http://localhost:5000/api/auth/login', {
				Email: this.form.username,
				Password: this.form.password
			})
				.then(function (response) {
					console.log(response);
					that.username = response.data.email;
					that.access_token = response.data.message;
				})
				.catch(function (error) {
					console.log(error);
				});
		},
		logout() {
			this.mgr.signoutRedirect();
		},
		apiCall(){

			const config = {
				headers: { Authorization: `Bearer ${this.access_token}` }
			};
			let url = 'http://localhost:5000/api/weatherforecast';
			//let url = 'https://edvision.ai/api/weatherforecast';
			http.get( 
				url,
				config
			).then(response => {
				if (response && response.data.length > 0){
					this.weathers = response.data;
						
				}
			}).catch(error => {
				this.$notify.error({
					title: 'Error',
					message: error
				});
					
			});
		}
	}
};
</script>
<style>
.text {
  font-size: 14px;
}

.item {
  margin-bottom: 18px;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}
.clearfix:after {
  clear: both;
}

.box-card {
  width: 480px;
}
</style>