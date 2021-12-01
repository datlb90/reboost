<template>
  <div>
    <div>
      Redirecting...
    </div>
  </div>
</template>
<script>
import authService from '@/services/auth.service'
export default {
  name: 'Redirect',
  data() {
    return {
    }
  },
  async created() {
    const user = await authService.getExternalUser()
    console.log(user)
  }
}
</script>

<style>
hr
{
	margin-bottom: 20px;
	border-bottom: 1px;
	border-top: 1px solid rgba(0, 0, 0, 0.1);
}

.el-form-item__content{
	line-height: 20px !important;
}
</style>
