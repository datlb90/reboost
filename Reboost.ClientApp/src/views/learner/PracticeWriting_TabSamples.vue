<template>
  <div>
    <ul>
      <li v-for="(item, index) in samples" :key="item.id">
        <el-card class="box-card">
          <div slot="header" class="box-card__title">
            <div>
              <span>Sample Response #{{ index+1 }}</span>
            </div>
            <div class="box-card__title__right">
              <span>Band Score: {{ item.bandScore }}</span>
            </div>
          </div>
          <div class="box-card__content">
            <pre>{{ item.sampleText }}</pre>
          </div>
        </el-card>
      </li>
    </ul>
    <div v-if="samples.length == 0" style="width: 100%; text-align: center;">
      <span class="no-content">No sample</span>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      samples: []
    }
  },
  mounted() {
    var questionId = this.$route.params.id
    this.$store.dispatch('question/loadSampleByQuestion', +questionId).then(() => {
      this.samples = this.$store.getters['question/getSampleByQuestion']
    })
  }
}
</script>
<style scoped>
  pre{
    white-space: break-spaces;
    font-family: inherit !important;
    text-align: justify;
  }

  .no-content{
    color: rgba(176, 185, 196, 0.7);
  }

  ul {
      margin: 0;
      padding: 0;
      list-style-type: none;
  }
  li{
    margin-bottom: 10px;
  }

  .box-card {
    width: 100%;
  }
  .box-card__title{
    display: flex;
    justify-content: space-between;
    font-size: 16px;
    font-weight: 600;
  }
  .box-card__title__right{
    font-weight: normal;
  }
</style>
