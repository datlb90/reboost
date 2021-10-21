<template>
  <el-dialog
    id="addEditSampleDialog"
    :title="messageTranslates('addQuestionSampleDialog', 'title')"
    :visible.sync="sampleDialogVisible"
    width="40%"
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
          :placeholder="messageTranslates('addQuestionSampleDialog', 'placeholderSample')"
          height="230px"
          :extensions="extensions"
          :char-counter-count="false"
        />
      </el-form-item>
      <el-form-item
        prop="bandScore"
        size="mini"
        :label="messageTranslates('addQuestionSampleDialog', 'bandScore')"
        :rules="[{ required: true, message: 'Band score is required' }]"
      >
        <!-- <el-radio-group v-model="form.bandScore">
          <el-radio-button v-for="score in bandScore" :key="score" :label="score" />
        </el-radio-group> -->
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
      </el-form-item>
    </el-form>
    <span slot="footer" class="dialog-footer">
      <el-button size="mini" @click="sampleDialogVisible = false">{{ constantTranslate('DISPUTE_STATUS', 'cancel') }}</el-button>
      <el-button size="mini" type="primary" @click="onAddSample">{{ constantTranslate('DISPUTE_STATUS', 'submit') }}</el-button>
    </span>
  </el-dialog>
</template>
<script>
import {
  // necessary extensions
  Doc,
  Text,
  Paragraph,
  Heading,
  Bold,
  Underline,
  Italic,
  Strike,
  ListItem,
  BulletList, // use with ListItem
  OrderedList // use with ListItem
} from 'element-tiptap'

import { TEST_TYPES } from '../../app.constant'
import questionService from '../../services/question.service'
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
        source: null
      },
      optionsSource: [
        { value: 'self', label: 'Self' },
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
        new OrderedList()
      ]
    }
  },
  computed: {
    bandScore() {
      var data = this.$store.getters['question/getSelected']
      return data.test === TEST_TYPES.IELTS ? [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] : [0, 1, 2, 3, 4, 5]
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
            Status: this.currentUser.role === UserRole.ADMIN ? SAMPLE_STATUS.APPROVED : SAMPLE_STATUS.CONTRIBUTED
          }
          questionService.createQuestionSample(postData).then(rs => {
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
            }
          })
        }
      })
    }
  }
}

</script>
