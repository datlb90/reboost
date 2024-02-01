<template>
  <el-dialog
    id="addEditSampleDialog"
    :title="messageTranslates('addQuestionSampleDialog', 'title')"
    :visible.sync="sampleDialogVisible"
    width="800px"
  >
    <el-form
      ref="form"
      :model="form"
      label-position="right"
      label-width="120px"
      style="width:100%;"
    >
      <el-form-item
        id="sampleContent"
        :label="messageTranslates('addQuestionSampleDialog', 'sample')"
        size="mini"
        prop="sample"
        :rules="[{ required: true, message: 'Sample is required' }]"
      >
        <el-tiptap
          v-model="form.sample"
          lang="en"
          placeholder="Add the sample"
          :extensions="extensions"
          :char-counter-count="false"
          style="width: 95%;"
        />
      </el-form-item>
      <el-form-item
        prop="bandScore"
        size="mini"
        :label="messageTranslates('addQuestionSampleDialog', 'bandScore')"
        :rules="[{ required: true, message: 'Band score is required' }]"
      >
        <el-select v-model="form.bandScore" :placeholder="messageTranslates('addQuestionSampleDialog', 'placeholderBandScore')">
          <el-option
            v-for="score in bandScore"
            :key="score"
            :label="score"
            :value="score"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        label="Comment"
        size="mini"
        prop="comment"
      >
        <el-tiptap
          v-model="form.comment"
          lang="en"
          placeholder="Add comment for the sample"
          :extensions="extensions"
          :char-counter-count="false"
          style="width: 95%;"
        />
      </el-form-item>

      <!-- <el-form-item
        prop="source"
        :label="messageTranslates('addQuestionSampleDialog', 'source')"
        size="mini"
        :rules="[{ required: true, message: 'Source is required' }]"
      >
        <el-select v-model="form.source" clearable placeholder="Select source" style="width: 100%">
          <el-option
            v-for="item in optionsSource"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item> -->
    </el-form>
    <span slot="footer" class="dialog-footer">
      <el-button size="mini" @click="sampleDialogVisible = false">{{ messageTranslates('addEditQuestion', 'cancel') }}</el-button>
      <el-button size="mini" type="primary" @click="onAddSample">{{ messageTranslates('addEditQuestion', 'submit') }}</el-button>
    </span>
  </el-dialog>
</template>
<script>
import {
  Doc,
  Text,
  Paragraph,
  Heading,
  Bold,
  Underline,
  Italic,
  Strike,
  ListItem,
  BulletList,
  OrderedList,
  FontSize,
  Indent,
  LineHeight,
  TextColor
} from 'element-tiptap'

// import { TEST_TYPES } from '../../app.constant'
import sampleService from '../../services/sample.service'
import { UserRole, SAMPLE_STATUS } from '../../app.constant'

export default {
  name: 'AddQuestionSample',
  props: {
  },
  data() {
    return {
      sampleDialogVisible: false,
      questionId: null,
      form: {
        sample: null,
        bandScore: 0,
        source: null,
        comment: null
      },
      optionsSource: [
        { value: 'google', label: 'Google' },
        { value: 'chatGPT', label: 'Chat GPT' },
        { value: 'other', label: 'Other' }
      ],
      extensions: [
        new Doc(),
        new Text(),
        new Paragraph(),
        new Heading({ level: 5 }),
        new Bold(),
        new Underline(),
        new Italic(),
        new Strike(),
        new ListItem(),
        new BulletList(),
        new OrderedList(),
        new FontSize(),
        new TextColor(),
        new Indent(),
        new LineHeight()
      ]
    }
  },
  computed: {
    bandScore() {
      // var data = this.$store.getters['question/getSelected']
      // return data.test === TEST_TYPES.IELTS ? [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] : [0, 1, 2, 3, 4, 5]
      return [0.0, 1.0, 2.0, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0]
    },
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  methods: {
    openDialog(id) {
      this.questionId = id
      this.sampleDialogVisible = true
    },
    loadSamples() {
      this.$store.dispatch('question/loadSampleByQuestion', +this.questionId)
    },
    resetFields() {
      console.log('reset')
      this.form.sample = null
      this.form.bandScore = 0
      this.form.source = null
    },
    onAddSample() {
      this.$refs['form'].validate((valid) => {
        if (valid) {
          const postData = {
            QuestionId: this.questionId,
            SampleText: this.form.sample,
            BandScore: this.form.bandScore,
            Comment: this.form.comment,
            Status: this.currentUser.role === UserRole.ADMIN ? SAMPLE_STATUS.APPROVED : SAMPLE_STATUS.CONTRIBUTED
          }
          sampleService.createQuestionSample(postData).then(rs => {
            if (rs) {
              this.$notify.success({
                title: 'Sample created',
                message: 'Sample submit successfully',
                type: 'success',
                duration: 2000
              })
              this.sampleDialogVisible = false
              this.resetFields()
              this.loadSamples()
              this.$emit('refreshQuestion')
            }
          })
        }
      })
    }
  }
}

</script>
