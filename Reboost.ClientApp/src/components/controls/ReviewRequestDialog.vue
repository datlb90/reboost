<template>
  <el-dialog
    v-if="screenWidth > 780"
    id="addEditQuestionDialog"
    title="Gửi Yêu Cầu Chấm Bài Miễn Phí"
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
        <el-form-item size="medium">
          <label slot="label" style="font-size: 16px;">Loại đề</label>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 2" border>Task 2</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 1" border>Task 1</el-radio>
          <el-radio v-model="selectedTask" style="margin-right: 5px;" label="Other" border>Đề khác</el-radio>
        </el-form-item>

        <el-form-item size="medium">
          <label slot="label" style="font-size: 16px;">Kiểu viết</label>
          <el-radio v-model="selectedWriteType" style="margin-right: 5px;" label="Đánh máy" border>Đánh máy</el-radio>
          <el-radio v-model="selectedWriteType" style="margin-right: 5px;" label="Viết tay" border>Tải lên bản viết tay</el-radio>
        </el-form-item>

        <el-form-item v-if="selectedWriteType != 'Viết tay'" prop="topic" size="medium">
          <label slot="label" style="font-size: 16px;">Đề bài</label>
          <el-input
            v-model="form.topic"
            type="textarea"
            :rows="3"
            :placeholder="selectedTask != 'Other' ? 'Đề bài viết cho ' + selectedTask + '' : 'Đê bài viết bất kỳ'"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'IELTS Task 1' && selectedWriteType != 'Viết tay'" size="medium">
          <label slot="label" style="font-size: 16px;">Biểu đồ</label>
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
            <el-button size="medium" type="primary" plain>Nhấn để tải lên</el-button>
            <div slot="tip" class="el-upload__tip">
              <div style="font-size: 15px;">Tệp png / jpeg có kích thước nhỏ hơn 10mb.</div>
            </div>
          </el-upload>
        </el-form-item>

        <el-form-item v-if="selectedWriteType == 'Viết tay'" size="medium" label="Ảnh viết tay">
          <label slot="label" style="font-size: 16px;">Ảnh viết tay</label>
          <el-upload
            v-if="!gettingTextFromImage"
            drag
            action=""
            :on-change="handleWritingImageChange"
            :limit="1"
            :multiple="false"
            :auto-upload="false"
          >
            <i class="el-icon-upload" />
            <div class="el-upload__text">Kéo thả ảnh hoặc <em>nhấn để tải lên</em></div>
            <div slot="tip" class="el-upload__tip">
              <div style="font-size: 15px;">Tệp png / jpeg có kích thước nhỏ hơn 10mb.</div>
              <div style="font-size: 15px;">Tải ảnh chụp bài viết của bạn kèm đề bài.</div>
              <div style="font-size: 15px;">Để tối ưu kết quả, hãy crop ảnh gọn gàng nhất có thể.</div>
            </div>

          </el-upload>

          <div v-if="gettingTextFromImage" style="width: 360px; height: 180px; border: #DCDFE6 solid 1px; padding-top: 60px; border-radius: 5px;">
            <div class="el-loading-spinner" style="position: relative; top: 10%;">
              <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
              <p class="el-loading-text" style="word-break: break-word;">Đang trích xuất đề và bài viết từ ảnh</p>
            </div>
          </div>

        </el-form-item>

        <el-form-item v-if="selectedWriteType != 'Viết tay'" prop="response" :rules="[{ required: true, message: 'Hãy thêm bài viết của bạn cho chủ đề này' }]" size="medium">
          <label slot="label" style="font-size: 16px;">Bài viết</label>
          <el-input
            v-model="form.response"
            type="textarea"
            :rows="5"
            :placeholder="'Bài viết của bạn cho chủ đề này'"
            style="width: 96%;"
          />
        </el-form-item>

        <el-form-item size="medium">
          <label slot="label" style="font-size: 16px;">Phản hồi</label>
          <el-radio v-model="selectedLanguage" style="margin-right: 5px;" label="vn" border>Phản hồi tiếng Việt</el-radio>
          <el-radio v-model="selectedLanguage" style="margin-right: 5px;" label="en" border>Phản hồi tiếng Anh</el-radio>
        </el-form-item>

        <el-form-item>
          <el-button size="medium" type="primary" :loading="isLoading" @click="submit">Xác nhận</el-button>
          <el-button size="medium" @click="cancelRequest()">{{ messageTranslates('addEditQuestion', 'cancel') }}</el-button>
        </el-form-item>
      </el-form>
    </div>

  </el-dialog>
  <el-dialog
    v-else
    id="addEditQuestionDialog"
    title="Gửi Yêu Cầu Chấm Bài Miễn Phí"
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
        <el-form-item size="medium" style="margin-bottom: 10px;">
          <label>Loại đề</label>
          <div>
            <div>
              <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 2" border>Task 2</el-radio>
              <el-radio v-model="selectedTask" style="margin-right: 5px;" label="IELTS Task 1" border>Task 1</el-radio>
              <el-radio v-model="selectedTask" style="margin-right: 5px;" label="Other" border>Đề khác</el-radio>
            </div>
          </div>

        </el-form-item>

        <el-form-item size="medium" style="margin-bottom: 10px;">
          <label>Kiểu viết</label>
          <div>
            <div>
              <el-radio v-model="selectedWriteType" style="margin-right: 5px;" label="Đánh máy" border>Đánh máy</el-radio>
              <el-radio v-model="selectedWriteType" style="margin-right: 5px;" label="Viết tay" border>Tải lên bản viết tay</el-radio>
            </div>
          </div>

        </el-form-item>

        <el-form-item v-if="selectedWriteType != 'Viết tay'" size="medium" style="margin-bottom: 15px;" prop="topic">
          <label>Đề bài</label>
          <el-input
            v-model="form.topic"
            type="textarea"
            :rows="3"
            :placeholder="selectedTask != 'Other' ? 'Đề bài viết cho ' + selectedTask + '' : 'Đê bài viết bất kỳ'"
          />
        </el-form-item>

        <el-form-item v-if="selectedTask == 'IELTS Task 1'" size="medium" style="margin-bottom: 10px;">
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
            <el-button size="medium" type="primary" plain>Nhấn để tải lên</el-button>
            <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateImgUpload') }}</div>
          </el-upload>
        </el-form-item>

        <el-form-item v-if="selectedWriteType == 'Viết tay'" size="medium" style="margin-bottom: 15px;">
          <label>Ảnh chụp bài viết kèm đề bài</label>
          <el-upload
            v-if="!gettingTextFromImage"
            action=""
            :on-change="handleWritingImageChange"
            :multiple="false"
            :limit="1"
            :auto-upload="false"
          >
            <el-button size="medium" type="primary" plain>Nhấn để tải lên</el-button>
            <div slot="tip" class="el-upload__tip">
              <div style="font-size: 15px;">Tệp png / jpeg có kích thước nhỏ hơn 10mb.</div>
              <div style="font-size: 15px;">Tải ảnh chụp bài viết của bạn kèm đề bài.</div>
              <div style="font-size: 15px;">Để tối ưu kết quả, hãy crop ảnh gọn gàng nhất có thể.</div>
            </div>
          </el-upload>

          <div v-if="gettingTextFromImage" style="width: 100%; height: 180px; border: #DCDFE6 solid 1px; padding-top: 60px; border-radius: 5px;">
            <div class="el-loading-spinner" style="position: relative; top: 10%;">
              <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
              <p class="el-loading-text" style="word-break: break-word;">Đang trích xuất đề và bài viết từ ảnh</p>
            </div>
          </div>

        </el-form-item>

        <el-form-item
          v-if="selectedWriteType != 'Viết tay'"
          size="medium"
          style="margin-bottom: 15px;"
          prop="response"
          :rules="[{ required: true, message: 'Hãy thêm bài viết của bạn cho chủ đề này' }]"
        >
          <label>Bài viết của bạn</label>
          <el-input
            v-model="form.response"
            type="textarea"
            :rows="5"
            :placeholder="'Bài viết của bạn'"
          />
        </el-form-item>

        <el-form-item size="medium" style="margin-bottom: 15px;">
          <label>Ngôn ngữ phản hồi</label>
          <div>
            <div>
              <el-radio v-model="selectedLanguage" style="margin-right: 10px; margin-left: 0px;" label="vn" border>Phản hồi tiếng Việt</el-radio>
              <el-radio v-model="selectedLanguage" style="margin-left: 0px;" label="en" border>Phản hồi tiếng Anh</el-radio>
            </div>
          </div>
        </el-form-item>

        <el-form-item>
          <el-button size="medium" type="primary" :loading="isLoading" @click="submit">Xác nhận</el-button>
          <el-button size="medium" @click="cancelRequest()">{{ messageTranslates('addEditQuestion', 'cancel') }}</el-button>
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
        feedbackLanguage: 'en',
        topic: null,
        response: null,
        reading: null,
        transcript: null,
        part: null
      },
      personalQuestion: null,
      dialogVisible: false,
      selectedTask: 'IELTS Task 2',
      selectedLanguage: 'vn',
      selectedWriteType: 'Đánh máy',
      fileUrl: null,
      LISTENING_FILE_MAX_SIZE: 10000000,
      CHART_FILE_MAX_SIZE: 10000000,
      LISTENING_TYPE_FILE: ['video/mp4', 'audio/mpeg'],
      CHART_TYPE_FILE: ['image/jpeg', 'image/png'],
      chartList: [],
      listeningList: [],
      screenWidth: window.innerWidth,
      isLoading: false,
      gettingTextFromImage: false
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
            this.chartList.push({ name: chart.FileName, url: chart.Url, raw: chart.UploadedFile})
          }
        }
      }
      this.dialogVisible = true
    },
    async submit() {
      this.$refs['personalQuestionForm'].validate(async valid => {
        if (valid) {
          this.isLoading = true
          if (this.selectedTask == 'IELTS Task 1') {
            if (this.chartList && this.chartList.length > 0) {
              this.form.part = {
                chart: this.chartList[0]
              }
            }
          } else {
            this.form.part = null
          }
          let taskId = 4
          const test = 'IELTS'
          if (this.selectedTask == 'IELTS Task 1') {
            taskId = 3
          }

          let topic = 'The writing topic is not provided'
          if (this.form.topic && this.form.topic != '' && this.form.topic != 'Không thể trích xuất từ ảnh') { topic = this.form.topic }
          // Setup question content to send to the backend
          const formData = new FormData()
          formData.set('UserId', this.currentUser.id)
          formData.set('TaskName', this.selectedTask)
          formData.set('TaskId', taskId)
          formData.set('Text', this.form.response)
          formData.set('Test', test)

          // Setup question content to send to the backend
          formData.set('Question.QuestionParts[0][Name]', 'Question')
          formData.set('Question.QuestionParts[0][Content]', topic)
          formData.set('Question.QuestionParts[0][Order]', 1)
          formData.set(`Question.QuestionParts[0][QuestionId]`, 0)

           // Setting object to keep in the store
          var personalQuestion = {
            UserId: this.currentUser.id,
            FeedbackLanguage: this.selectedLanguage,
            TaskName: this.selectedTask,
            TaskId: taskId,
            Text: this.form.response,
            Test: test,
            Parts: []
          }
          const questionTopic = {
            Name: 'Question',
            Content: topic
          }
          personalQuestion.Parts.push(questionTopic)

          // If there is a chart
          if (this.form.part && this.form.part.chart && this.form.part.chart.raw) {
            const chart = this.form.part.chart.raw
            // Setup chart to send to the backend
            formData.set(`Question.QuestionParts[1][Name]`, 'Chart')
            formData.set(`Question.QuestionParts[1][Order]`, 2)
            formData.set(`Question.QuestionParts[1][Content]`, chart.name)
            formData.set(`Question.QuestionParts[1][FileName]`, chart.name)
            formData.set(`Question.QuestionParts[1][FileExtension]`, stringUtil.getFileExtension(this.form.part.chart.name))
            formData.set(`Question.UploadedFile`, chart)
            // Setting object to keep in the store
            const part = {}
            part.Name = 'Chart'
            part.Content = chart.name
            part.FileName = chart.name
            part.FileExtension = stringUtil.getFileExtension(this.form.part.chart.name)
            part.UploadedFile = chart
            part.Url = this.fileUrl
            personalQuestion.Parts.push(part)
          }
          // clear initial test on review request submission
          this.$store.dispatch('question/clearInitialSubmission')
          // Save to review request to store
          this.$store.dispatch('question/savePersonalQuestion', personalQuestion).then(rs => {
            console.log('question saved')
            if (!authService.isAuthenticated() || !this.currentUser?.id) {
              console.log('redirecting')
              this.isLoading = false
              // Redirect to the register page for authentication
              return this.$router.push({ path: '/register' })
            } else {
              console.log('create submission')
              questionService.createPersonalSubmission(formData).then(submission => {
                if (submission) {
                  this.resetData()
                  this.isLoading = false
                  this.dialogVisible = false
                  this.$emit('openCheckoutDialog', {
                    questionId: submission.questionId,
                    submissionId: submission.id,
                    feedbackLanguage: this.selectedLanguage
                  })
                }
              })
            }
          })
        }
      })
    },
    async handleWritingImageChange(file, fileList) {
      if (file.size > this.CHART_FILE_MAX_SIZE) {
        this.$message.warning(`File tải lên cần nhỏ hơn 10mb`)
        this.chartList = []
      } else {
        if (this.CHART_TYPE_FILE.includes(file.raw.type)) {
          this.getWritingTextFromImage(file)
        } else {
          this.$message.warning(`Hãy tải lên file có định dạng png hoặc jpeg`)
          this.chartList = []
        }
      }
    },
    async getWritingTextFromImage(file) {
      this.gettingTextFromImage = true
      const formData = new FormData()
      formData.append(`file`, file.raw)
      const result = await questionService.getWritingTextFromImage(formData)
      this.form.response = result.essay
      this.form.topic = result.topic
      this.selectedWriteType = 'Đánh máy'
      this.gettingTextFromImage = false
    },
     async fileListToBase64(file) {
      return await Promise.resolve(this.getBase64(file))
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
        } else {
          this.$message.warning(`Please upload mp3/mp4 file.`)
          this.listeningList = []
        }
      }
    },
    async handleIeltsChartChange(file, fileList) {
      if (file.size > this.CHART_FILE_MAX_SIZE) {
        this.$message.warning(`File tải lên cần nhỏ hơn 10mb`)
        this.chartList = []
      } else {
        if (this.CHART_TYPE_FILE.includes(file.raw.type)) {
          this.fileUrl = await this.fileListToBase64(file.raw)
          this.chartList = fileList
          console.log(this.fileUrl)
        } else {
          this.$message.warning(`Hãy tải lên file có định dạng png hoặc jpeg`)
          this.chartList = []
        }
      }
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
      const header = document.querySelector('#header.headroom')
      if (header) {
        header.style.display = 'block'
      }

      this.dialogVisible = false
    },
    cancelRequest() {
      const header = document.querySelector('#header.headroom')
      if (header) {
        header.style.display = 'block'
      }
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

