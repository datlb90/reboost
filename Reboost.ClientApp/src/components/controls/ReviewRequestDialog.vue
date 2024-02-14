<template>
  <el-dialog
    id="addEditQuestionDialog"
    title="Gửi Yêu Cầu Chấm Bài Viết"
    :visible.sync="dialogVisible"
    :before-close="handleClose"
    width="780px"
    top="15vh"
    :close-on-click-modal="false"
  >
    <div id="addQuestionDialog" class="dialog-content">
      <el-form
        ref="personalQuestionForm"
        :model="form"
        label-width="120px"
      >
        <el-form-item size="mini" label="Task">
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 1" border>IELTS Task 1</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 2" border>IELTS Task 2</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="TOEFL Independent" border>TOEFL Independent</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="TOEFL Integrated" border>TOEFL Integrated</el-radio>
        </el-form-item>

        <el-form-item prop="topic" :rules="[{ required: true, message: 'The writing topic is required' }]" size="mini" label="Topic">
          <el-input
            v-model="form.topic"
            type="textarea"
            :rows="4"
            :placeholder="'Enter the ' + selectedTask + ' topic'"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'" prop="reading" :rules="[{ required: true, message: 'The reading passage is required' }]" size="mini" label="Reading">
          <el-input
            v-model="toeflReading"
            type="textarea"
            :rows="4"
            placeholder="Enter the reading passage for the TOEFL Integrated topic"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'" size="mini" label="Listening">
          <el-upload
            action=""
            :on-preview="handlePreview"
            :on-change="handleToeflListeningChange"
            :on-remove="handleRemove"
            :multiple="false"
            :limit="1"
            :auto-upload="false"
            :on-exceed="handleExceed"
            :file-list="fileList"
          >
            <el-button size="small" type="primary">{{ messageTranslates('addEditQuestion', 'uploadButton') }}</el-button>
            <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateUpload') }}</div>
          </el-upload>
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'" size="mini" label="Transcript">
          <el-input
            v-model="toeflTranscript"
            type="textarea"
            :rows="4"
            placeholder="Enter the transcript of the listening if available"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'IELTS Task 1'" size="mini" label="Chart">
          <el-upload
            action=""
            :on-preview="handlePreview"
            :on-change="handleIeltsChartChange"
            :on-remove="handleRemove"
            :multiple="false"
            :limit="1"
            :auto-upload="false"
            :on-exceed="handleExceed"
            :file-list="chartFileList"
          >
            <el-button size="small" type="primary" plain>Upload chart for IELTS Task 1 (Optional)</el-button>
            <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateImgUpload') }}</div>
          </el-upload>
        </el-form-item>

        <el-form-item prop="response" :rules="[{ required: true, message: 'Your response is required' }]" size="mini" label="Response">
          <el-input
            v-model="form.response"
            type="textarea"
            :rows="8"
            :placeholder="'Enter your response for the ' + selectedTask + ' topic'"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item>
          <el-button size="mini" type="primary" @click="submit">{{ messageTranslates('addEditQuestion', 'submit') }}</el-button>
          <el-button size="mini" @click="dialogVisible = false">{{ messageTranslates('addEditQuestion', 'cancel') }}</el-button>
        </el-form-item>
      </el-form>
    </div>

  </el-dialog>
</template>

<script>
import authService from '@/services/auth.service'
import questionService from '../../services/question.service'
import * as stringUtil from '@/utils/string'
export default {
  name: 'AddEditQuestion',
  components: {
  },
  props: {
  },
  data() {
    return {
      dialogVisible: false,
      selectedTask: 'IELTS Task 2',
      form: {
        topic: null,
        response: null,
        reading: null,
        part: null
      },
      toeflReading: null,
      toeflListening: null,
      toeflTranscript: null,
      ieltsChart: null,
      LISTENING_FILE_MAX_SIZE: 10000000,
      CHART_FILE_MAX_SIZE: 3000000,
      LISTENING_TYPE_FILE: ['video/mp4', 'audio/mpeg'],
      CHART_TYPE_FILE: ['image/jpeg', 'image/png'],
      fileList: [],
      chartFileList: []
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  mounted() {
    console.log(this.currentUser)
  },
  methods: {
    openDialog() {
      this.resetData()
      this.dialogVisible = true
    },
    async submit() {
      console.log('User:', this.currentUser)

      this.$refs['personalQuestionForm'].validate(async valid => {
        if (valid) {
          if (this.selectedTasks == 'TOEFL Integrated') {
            this.form.part = {
              reading: this.toeflReading,
              listening: this.fileList && this.fileList.length > 0 ? this.fileList[0] : null,
              transcript: this.toeflTranscript
            }
          } else if (this.selectedTasks == 'IELTS Task 1') {
            this.form.part = {
                chart: this.chartFileList && this.chartFileList.length > 0 ? this.chartFileList[0] : null
            }
          } else {
            this.form.part = null
          }
          let taskId = 4
          if (this.selectedTask == 'IELTS Task 1') { taskId = 3 } else if (this.selectedTask == 'TOEFL Independent') { taskId = 1 } else if (this.selectedTask == 'TOEFL Integrated') { taskId = 2 }
          const formData = new FormData()

          formData.set('UserId', this.currentUser.id)
          formData.set('TaskName', this.selectedTask)
          formData.set('TaskId', taskId)
          formData.set('Text', this.form.response)

          formData.set('Question.QuestionParts[0][Name]', 'Question')
          formData.set('Question.QuestionParts[0][Content]', this.form.topic)
          formData.set('Question.QuestionParts[0][Order]', 1)
          formData.set(`Question.QuestionParts[0][QuestionId]`, 0)

          let order = 2
          let count = 1
          if (this.form.part) {
            for (var p in this.form.part) {
              if (this.form.part[p]?.raw) {
                formData.set(`Question.QuestionParts[${count}][Content]`, this.form.part[p].raw.name)
                formData.set(`Question.QuestionParts[${count}][FileName]`, this.form.part[p].raw.name)
                formData.set(`Question.QuestionParts[${count}][FileExtension]`, stringUtil.getFileExtension(this.form.part[p]?.name))
                formData.set(`Question.UploadedFile`, this.form.part[p]?.raw)
              } else {
                formData.set(`Question.QuestionParts[${count}][Content]`, this.form.part[p])
              }
              formData.set(`Question.QuestionParts[${count}][Order]`, order)
              formData.set(`Question.QuestionParts[${count}][Name]`, this.partNameFormat(p))
              order += 1
              count += 1
            }
          }

          if (!authService.isAuthenticated() || !this.currentUser?.id) {
            // save to review request to store
            return this.$router.push({ path: '/register?review=requested' })
          } else {
            var submission = await questionService.createPersonalSubmission(formData)
            console.log(submission)
            if (submission) {
              this.$notify.success({
                    title: 'Submission Created.',
                    message: 'Your submission has been created. You can view it in the submissions page.',
                    type: 'success',
                    duration: 5000
                  })

              this.resetData()
              this.dialogVisible = false
              this.$emit('openCheckoutDialog', {
                questionId: submission.questionId,
                submissionId: submission.id
              })
            }
          }
        }
      })
    },
    handleRemove(file, fileList) {
      this.fileList = fileList
    },
    handlePreview(file) {
      console.log('file', typeof (file), file)
    },
    handleExceed() {
      this.$message.warning(`The limit is 1 file.`)
    },
    handleToeflListeningChange(file, fileList) {
      if (file.size > this.LISTENING_FILE_MAX_SIZE) {
        this.$message.warning(`The limit size is 10mb.`)
        this.fileList = []
      } else {
        if (this.LISTENING_TYPE_FILE.includes(file.raw.type)) {
          this.fileList = fileList
        } else {
          this.$message.warning(`Please upload mp3/mp4 file.`)
          this.fileList = []
        }
      }
    },
    handleIeltsChartChange(file, fileList) {
      if (file.size > this.CHART_FILE_MAX_SIZE) {
        this.$message.warning(`The limit size is 3mb.`)
        this.chartFileList = []
      } else {
        if (this.CHART_TYPE_FILE.includes(file.raw.type)) {
          this.chartFileList = fileList
        } else {
          this.$message.warning(`Please upload png/jpeg file.`)
          this.chartFileList = []
        }
      }
    },
    partNameFormat(str) {
      return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function(word, index) {
        return index === 0 ? word.toUpperCase() : word.toLowerCase()
      }).replace(/\s+/g, '')
    },
    handleClose() {
      // Do not clear the data
      this.dialogVisible = false
    },
    resetData() {
      this.fileList = []
      this.chartFileList = []
      if (this.form) {
        this.form.topic = null
        this.form.response = null
      }
      this.ieltsChart = null
      this.toeflReading = null
      this.toeflListening = null
      this.toeflTranscript = null
    }
  }
}
</script>
<style>
#addEditQuestionDialog .el-dialog__header {
  text-align: center;
}

#addEditQuestionDialog .el-tiptap-editor__menu-bar{
  background: #f0f8ff;
}

#addEditQuestionDialog .el-tiptap-editor>.el-tiptap-editor__content {
  border-bottom: 1px solid #ebeef5;
}
#questionPart>.el-form-item__content{
  margin-left:0!important;
}
</style>

