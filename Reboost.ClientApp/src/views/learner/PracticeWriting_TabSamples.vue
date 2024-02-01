<template>
  <div>
    <div v-if="samples && samples.length > 0">
      <div v-for="(item, index) in samples" :key="item.id">
        <el-card class="box-card sample-box">
          <div style="height: 50px; border-bottom: 1px solid rgb(220 223 229); margin-bottom: 20px; ">
            <div style="float: left; margin-top: 5px;">
              <span style="font-size: 16px; font-weight: 600;">Sample Response #{{ index+1 }}</span>
            </div>
            <div style="float: right;">
              <el-tag>Band Score: {{ item.bandScore ? item.bandScore.toFixed(1) : "Not Available" }}</el-tag>
            </div>
          </div>
          <div class="box-card__content">
            <div>
              <div class="sample-container" v-html="item.sampleText" />
            </div>
            <div v-if="item.comment" style="border-top: 1px solid rgb(220 223 229); margin-top: 20px; padding-top: 20px;">
              <div class="sample-container" v-html="item.comment" />
            </div>

          </div>
        </el-card>
      </div>
    </div>
    <div v-else>
      <el-card class="box-card" style="font-size: 14px; padding: 20px;">
        <div> We are working on collecting samples for this writing topic, please check back soon.</div>
      </el-card>
    </div>
    <add-sample ref="sampleDialog" />
  </div>
</template>
<script>
import AddQuestionSample from '../../components/controls/AddQuestionSampleDialog.vue'
export default {
  components: {
    'add-sample': AddQuestionSample
  },
  data() {
    return {
      sampleDialogVisible: false,
      questionId: null
    }
  },
  computed: {
    samples() {
      return this.$store.getters['question/getSampleByQuestion']
    }
  },
  mounted() {
    this.questionId = this.$route.params.id
    this.loadSamples()
  },
  methods: {
    loadSamples() {
      this.$store.dispatch('question/loadSampleByQuestion', +this.questionId)
    },
    openAddNewSampleDialog() {
      this.$refs.sampleDialog?.openDialog(this.questionId)
    }
  }
}
</script>

<style>
.sample-container p {
  color: black;
  line-height: 1.5 !important;
}
</style>
<style scoped>
.sample-box {
  padding: 20px;
  padding-top: 10px;
  margin-bottom: 20px;
}
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

  #addEditSampleDialog .el-tiptap-editor__menu-bar{
  background: #f0f8ff;
  }

  #addEditSampleDialog .el-tiptap-editor>.el-tiptap-editor__content {
    border-bottom: 1px solid #ebeef5;
  }

  #sampleContent>.el-form-item__content{
    margin-left:0!important;
  }

  .sample-container{
  word-break: break-word;
  }

</style>
