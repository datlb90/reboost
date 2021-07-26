<template>
  <div>
    <div class="d-flex justify-content-end mb-2">
      <el-button size="mini" @click="sampleDialogVisible = true">Add sample</el-button>
    </div>

    <el-dialog
      title="Note Revision"
      :visible.sync="sampleDialogVisible"
      width="30%"
    >
      <el-form ref="form" :model="form">
        <el-form-item prop="sample" :rules="[{ required: true, message: 'Sample is required' }]">
          <label class="mb-0">Sample</label>
          <el-input
            v-model="form.sample"
            type="textarea"
            :rows="5"
            placeholder="Please input sample"
          />
        </el-form-item>
        <el-form-item prop="bandscore" :rules="[{ required: true, message: 'Band score is required' }]">
          <div><label class="mb-0">Band Score</label></div>
          <el-radio-group v-model="form.bandScore">
            <el-radio-button v-for="n in evenNumbers" :key="n" :label="n" />
          </el-radio-group>
        </el-form-item>
        <el-form-item prop="source" :rules="[{ required: true, message: 'Source is required' }]">
          <div><label class="mb-0">Source</label></div>
          <el-select v-model="form.source" clearable placeholder="Select source" style="width: 100%">
            <el-option
              v-for="item in optionsSource"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="sampleDialogVisible = false">Cancel</el-button>
        <el-button type="primary" @click="onAddSample">Submit</el-button>
      </span>
    </el-dialog>

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
      samples: [],
      evenNumbers: [1, 2, 3, 4, 5, 6, 7, 8],
      sampleDialogVisible: false,
      form: {
        sample: null,
        bandScore: 1,
        source: null
      },
      optionsSource: [
        { value: 'self', label: 'Self' },
        { value: 'other', label: 'Other' }
      ]
    }
  },
  mounted() {
    var questionId = this.$route.params.id
    this.$store.dispatch('question/loadSampleByQuestion', +questionId).then(() => {
      this.samples = this.$store.getters['question/getSampleByQuestion']
    })
  },
  methods: {
    onAddSample() {
      this.$refs['form'].validate((valid) => {
        if (valid) {
          console.log('ahihi')
        }
      })
    }
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
