<template>
  <el-dialog
    v-if="screenWidth > 780"
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
        <el-form-item size="mini" label="Chọn task">
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 1" border>IELTS Task 1</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 2" border>IELTS Task 2</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="TOEFL Independent" border>TOEFL Independent</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="TOEFL Integrated" border>TOEFL Integrated</el-radio>
        </el-form-item>

        <el-form-item prop="topic" :rules="[{ required: true, message: 'Hãy thêm một chủ đề viết' }]" size="mini" label="Chủ đề">
          <el-input
            v-model="form.topic"
            type="textarea"
            :rows="4"
            :placeholder="'Điền chủ đề cho bài ' + selectedTask + ''"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'" prop="reading" :rules="[{ required: true, message: 'Hãy thêm thông tin bài đọc' }]" size="mini" label="Bài đọc">
          <el-input
            v-model="form.reading"
            type="textarea"
            :rows="4"
            placeholder="Điền thông tin bài đọc"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'" size="mini" label="Bài nghe">
          <el-upload
            action=""
            :on-preview="handlePreview"
            :on-change="handleToeflListeningChange"
            :on-remove="handleRemove"
            :multiple="false"
            :limit="1"
            :auto-upload="false"
            :on-exceed="handleExceed"
            :file-list="listeningList"
            style="width: 96%;"
          >
            <el-button size="small" type="primary">{{ messageTranslates('addEditQuestion', 'uploadButton') }}</el-button>
            <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateUpload') }}</div>
          </el-upload>
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'" size="mini" label="Bản ghi">
          <el-input
            v-model="form.transcript"
            type="textarea"
            :rows="4"
            placeholder="Điền bản ghi của bài nghe nếu bạn có"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'IELTS Task 1'" size="mini" label="Biểu đồ">
          <el-upload
            action=""
            :on-preview="handlePreview"
            :on-change="handleIeltsChartChange"
            :on-remove="handleChartRemove"
            :multiple="false"
            :limit="1"
            :auto-upload="false"
            :on-exceed="handleExceed"
            :file-list="chartList"
            style="width: 96%;"
          >
            <el-button size="small" type="primary" plain>Nhấn để tải lên</el-button>
            <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateImgUpload') }}</div>
          </el-upload>
        </el-form-item>

        <el-form-item prop="response" :rules="[{ required: true, message: 'Hãy thêm bài viết của bạn cho chủ đề này' }]" size="mini" label="Bài viết">
          <el-input
            v-model="form.response"
            type="textarea"
            :rows="8"
            :placeholder="'Bài viết của bạn cho chủ đề này'"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item>
          <el-button size="mini" type="primary" @click="submit">{{ messageTranslates('addEditQuestion', 'submit') }}</el-button>
          <el-button size="mini" @click="cancelRequest()">{{ messageTranslates('addEditQuestion', 'cancel') }}</el-button>
        </el-form-item>
      </el-form>
    </div>

  </el-dialog>
  <el-dialog
    v-else
    id="addEditQuestionDialog"
    title="Gửi Yêu Cầu Chấm Bài Viết"
    :visible.sync="dialogVisible"
    :before-close="handleClose"
    :fullscreen="true"
    :close-on-click-modal="false"
  >
    <div id="addQuestionDialog" class="dialog-content">
      <el-form
        ref="personalQuestionForm"
        :model="form"
      >
        <el-form-item size="small">
          <label>Chọn task</label>
          <div>
            <div>
              <el-radio v-model="selectedTask" style="margin-right: 10px; margin-left: 0px;" label="IELTS Task 1" border>IELTS Academic Task 1</el-radio>
              <el-radio v-model="selectedTask" style="margin-left: 0px;" label="IELTS Task 2" border>IELTS Academic Task 2</el-radio>
            </div>
            <div>
              <el-radio v-model="selectedTask" style="margin-right: 10px;  margin-left: 0px; padding-left: 10px; padding-right: 34px;" label="TOEFL Independent" border>TOEFL Independent</el-radio>
              <el-radio v-model="selectedTask" style=" margin-left: 0px; padding-left: 10px; padding-right: 46px;" label="TOEFL Integrated" border>TOEFL Integrated</el-radio>
            </div>
          </div>

        </el-form-item>

        <el-form-item prop="topic" :rules="[{ required: true, message: 'Hãy thêm một chủ đề viết' }]">
          <label>Chủ đề</label>
          <el-input
            v-model="form.topic"
            type="textarea"
            :rows="4"
            :placeholder="'Điền chủ đề viết cho ' + selectedTask + ''"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'" prop="reading" :rules="[{ required: true, message: 'Hãy thêm thông tin bài đọc' }]">
          <label>Bài đọc</label>
          <el-input
            v-model="form.reading"
            type="textarea"
            :rows="4"
            placeholder="Điền thông tin bài đọc"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'">
          <label>Bài nghe</label>
          <el-upload
            action=""
            :on-preview="handlePreview"
            :on-change="handleToeflListeningChange"
            :on-remove="handleRemove"
            :multiple="false"
            :limit="1"
            :auto-upload="false"
            :on-exceed="handleExceed"
            :file-list="listeningList"
          >
            <el-button size="small" type="primary">{{ messageTranslates('addEditQuestion', 'uploadButton') }}</el-button>
            <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateUpload') }}</div>
          </el-upload>
        </el-form-item>

        <el-form-item v-if="selectedTask == 'TOEFL Integrated'">
          <label>Bản ghi của bài nghe</label>
          <el-input
            v-model="form.transcript"
            type="textarea"
            :rows="4"
            placeholder="Điền bản ghi của bài nghe nếu bạn có"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'IELTS Task 1'">
          <label>Biểu đồ</label>
          <el-upload
            action=""
            :on-preview="handlePreview"
            :on-change="handleIeltsChartChange"
            :on-remove="handleChartRemove"
            :multiple="false"
            :limit="1"
            :auto-upload="false"
            :on-exceed="handleExceed"
            :file-list="chartList"
          >
            <el-button size="small" type="primary" plain>Nhấn để tải lên</el-button>
            <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateImgUpload') }}</div>
          </el-upload>
        </el-form-item>

        <el-form-item prop="response" :rules="[{ required: true, message: 'Hãy thêm bài viết của bạn cho chủ đề này' }]">
          <label>Bài viết của bạn</label>
          <el-input
            v-model="form.response"
            type="textarea"
            :rows="8"
            :placeholder="'Bài viết của bạn cho chủ đề này'"
          />
        </el-form-item>

        <el-form-item>
          <el-button size="mini" type="primary" @click="submit">{{ messageTranslates('addEditQuestion', 'submit') }}</el-button>
          <el-button size="mini" @click="cancelRequest()">{{ messageTranslates('addEditQuestion', 'cancel') }}</el-button>
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
      form: {
        topic: null,
        response: null,
        reading: null,
        transcript: null,
        part: null
      },
      personalQuestion: null,
      dialogVisible: false,
      selectedTask: 'IELTS Task 2',
      fileUrl: null,
      LISTENING_FILE_MAX_SIZE: 10000000,
      CHART_FILE_MAX_SIZE: 3000000,
      LISTENING_TYPE_FILE: ['video/mp4', 'audio/mpeg'],
      CHART_TYPE_FILE: ['image/jpeg', 'image/png'],
      chartList: [],
      listeningList: [],
      screenWidth: window.innerWidth
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  mounted() {
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
  },
  methods: {
    openDialog() {
      this.personalQuestion = this.$store.getters['question/getPersonalQuestion']
      if (this.personalQuestion) {
        this.selectedTask = this.personalQuestion.TaskName
        const topic = this.personalQuestion.Parts.find(p => p.Name == 'Question')
        if (topic) {
          this.form.topic = topic.Content
        }
        this.form.response = this.personalQuestion.Text
        if (this.personalQuestion.TaskName == 'IELTS Task 1') {
          const chart = this.personalQuestion.Parts.find(p => p.Name == 'Chart')
          if (chart) {
            this.chartList.push({ name: chart.FileName, url: chart.Url})
          }
        }
        if (this.personalQuestion.TaskName == 'TOEFL Integrated') {
          const reading = this.personalQuestion.Parts.find(p => p.Name == 'Reading')
          if (reading) {
            this.form.reading = reading.Content
          }
          const listening = this.personalQuestion.Parts.find(p => p.Name == 'Listening')
          if (listening) {
            this.listeningList.push({ name: listening.FileName, url: listening.Url})
          }
          const transcript = this.personalQuestion.Parts.find(p => p.Name == 'Transcript')
          if (transcript) {
            this.form.transcript = transcript.Content
          }
        }
      }
      this.dialogVisible = true
    },
    async submit() {
      this.$refs['personalQuestionForm'].validate(async valid => {
        if (valid) {
          console.log('form validated')
          if (this.selectedTask == 'TOEFL Integrated') {
            this.form.part = {
              reading: this.form.reading,
              listening: this.listeningList && this.listeningList.length > 0 ? this.listeningList[0] : null,
              transcript: this.form.transcript
            }
          } else if (this.selectedTask == 'IELTS Task 1') {
            this.form.part = {
                chart: this.chartList && this.chartList.length > 0 ? this.chartList[0] : null
            }
          } else {
            this.form.part = null
          }
          let taskId = 4
          let test = 'IELTS'
          if (this.selectedTask == 'IELTS Task 1') {
            taskId = 3
          } else if (this.selectedTask == 'TOEFL Independent') {
            taskId = 1
            test = 'TOEFL'
          } else if (this.selectedTask == 'TOEFL Integrated') {
            taskId = 2
            test = 'TOEFL'
          }

          const formData = new FormData()
          formData.set('UserId', this.currentUser.id)
          formData.set('TaskName', this.selectedTask)
          formData.set('TaskId', taskId)
          formData.set('Text', this.form.response)
          formData.set('Test', test)

          var personalQuestion = {
            UserId: this.currentUser.id,
            TaskName: this.selectedTask,
            TaskId: taskId,
            Text: this.form.response,
            Test: test,
            Parts: []
          }
          const questionTopic = {
            Name: 'Question',
            Content: this.form.topic
          }
          personalQuestion.Parts.push(questionTopic)

          formData.set('Question.QuestionParts[0][Name]', 'Question')
          formData.set('Question.QuestionParts[0][Content]', this.form.topic)
          formData.set('Question.QuestionParts[0][Order]', 1)
          formData.set(`Question.QuestionParts[0][QuestionId]`, 0)
          let order = 2
          let count = 1
          if (this.form.part) {
            for (var p in this.form.part) {
              const part = {}
              part.Name = this.partNameFormat(p)

              console.log(this.form.part[p])
              if (this.form.part[p]?.raw) {
                // This is for Task 1's chart and Integrated's listenting
                // Setting form data to send to the api
                formData.set(`Question.QuestionParts[${count}][Content]`, this.form.part[p].raw.name)
                formData.set(`Question.QuestionParts[${count}][FileName]`, this.form.part[p].raw.name)
                formData.set(`Question.QuestionParts[${count}][FileExtension]`, stringUtil.getFileExtension(this.form.part[p]?.name))
                formData.set(`Question.UploadedFile`, this.form.part[p]?.raw)
                // Setting object to keep in the store
                part.Content = this.form.part[p].raw.name
                part.FileName = this.form.part[p].raw.name
                part.FileExtension = stringUtil.getFileExtension(this.form.part[p]?.name)
                part.UploadedFile = this.form.part[p]?.raw
                part.Url = this.fileUrl
              } else {
                // For integrated's reading and transcript only
                formData.set(`Question.QuestionParts[${count}][Content]`, this.form.part[p])
                part.Content = this.form.part[p]
              }
              formData.set(`Question.QuestionParts[${count}][Order]`, order)
              formData.set(`Question.QuestionParts[${count}][Name]`, this.partNameFormat(p))
              order += 1
              count += 1
              personalQuestion.Parts.push(part)
            }
          }
          // clear initial test on review request submission
          this.$store.dispatch('question/clearInitialSubmission')
          // Save to review request to store
          this.$store.dispatch('question/savePersonalQuestion', personalQuestion).then(rs => {
            console.log('question saved')
            if (!authService.isAuthenticated() || !this.currentUser?.id) {
              console.log('redirecting')
              // Redirect to the register page for authentication
              return this.$router.push({ path: '/register' })
            } else {
              console.log('create submission')
              questionService.createPersonalSubmission(formData).then(submission => {
                if (submission) {
                  this.resetData()
                  this.dialogVisible = false
                  this.$emit('openCheckoutDialog', {
                    questionId: submission.questionId,
                    submissionId: submission.id
                  })
                }
              })
            }
          })
        }
      })
    },
    handleRemove(file, fileList) {
      this.listeningList = fileList
    },
    handleChartRemove(file, fileList) {
      this.chartList = fileList
    },
    handlePreview(file) {
      console.log('file', typeof (file), file)
    },
    handleExceed() {
      this.$message.warning(`The limit is 1 file.`)
    },
    async handleToeflListeningChange(file, fileList) {
      if (file.size > this.LISTENING_FILE_MAX_SIZE) {
        this.$message.warning(`The limit size is 10mb.`)
        this.listeningList = []
      } else {
        if (this.LISTENING_TYPE_FILE.includes(file.raw.type)) {
          this.fileUrl = await this.fileListToBase64(file.raw)
          this.listeningList = fileList
          console.log(this.listeningList)
        } else {
          this.$message.warning(`Please upload mp3/mp4 file.`)
          this.listeningList = []
        }
      }
    },
    async handleIeltsChartChange(file, fileList) {
      if (file.size > this.CHART_FILE_MAX_SIZE) {
        this.$message.warning(`The limit size is 3mb.`)
        this.chartList = []
      } else {
        if (this.CHART_TYPE_FILE.includes(file.raw.type)) {
          this.fileUrl = await this.fileListToBase64(file.raw)
          this.chartList = fileList
        } else {
          this.$message.warning(`Please upload png/jpeg file.`)
          this.chartList = []
        }
      }
    },
     async fileListToBase64(file) {
      return await Promise.resolve(this.getBase64(file))
    },
    getBase64(file) {
      const reader = new FileReader()
      return new Promise(resolve => {
        reader.onload = ev => {
          resolve(ev.target.result)
        }
        reader.readAsDataURL(file)
      })
    },
    partNameFormat(str) {
      return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function(word, index) {
        return index === 0 ? word.toUpperCase() : word.toLowerCase()
      }).replace(/\s+/g, '')
    },
    handleClose() {
      // Do not clear the data
      // this.$store.dispatch('question/clearPersonalQuestion')
      this.dialogVisible = false
    },
    cancelRequest() {
      if (this.form.topic || this.form.response || this.form.reading || this.form.transcript ||
      this.chartList.length > 0 || this.listeningList.length > 0) {
        this.$confirm('Dữ liệu đã nhập sẽ bị mất nếu bạn huỷ yêu cầu chấm bài. Bạn vẫn muốn huỷ?')
        .then(_ => {
          this.resetData()
          this.dialogVisible = false
        })
        .catch(_ => {})
      } else {
        this.resetData()
        this.dialogVisible = false
      }
    },
    resetData() {
      this.$refs['personalQuestionForm'].resetFields()
      this.$store.dispatch('question/clearPersonalQuestion')
      if (this.form) {
        this.form.topic = null
        this.form.response = null
        this.form.reading = null
        this.form.transcript = null
      }
      this.chartList = []
      this.listeningList = []
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

