<template>
  <div>
    <div class="d-flex justify-content-end mb-2">
      <el-button size="mini" @click="openAddNewSampleDialog()">Add sample</el-button>
    </div>

    <ul>
      <li v-for="(item, index) in samples" :key="item.id">
        <el-card class="box-card">
          <div slot="header" class="box-card__title">
            <div>
              <span>Sample Response #{{ index+1 }}</span>
            </div>
            <div class="box-card__title__right">
              <span>Band Score: {{ item.bandScore ? item.bandScore : "Not Available" }}</span>
            </div>
          </div>
          <div class="box-card__content">
            <div class="sample-container" v-html="item.sampleText" />
            <!-- <pre>{{ item.sampleText }}</pre> -->
          </div>
        </el-card>
      </li>
    </ul>
    <div v-if="samples.length == 0" style="width: 100%; text-align: center;">
      <span class="no-content">No sample</span>
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
