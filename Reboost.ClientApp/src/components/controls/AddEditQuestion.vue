<template>
  <el-dialog
    id="addEditQuestionDialog"
    :title="messageTranslates('addEditQuestion', 'title')"
    :visible.sync="dialogVisible"
    :before-close="handleClose"
    width="800px"
  >
    <div id="addQuestionDialog" class="dialog-content">
      <el-form
        ref="questionForm"
        :model="form"
        label-width="120px"
      >
        <el-form-item prop="name" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addEditQuestion', 'name')">
          <el-input
            v-model="form.name"
            type="textinput"
            :placeholder="messageTranslates('addEditQuestion', 'placeholderName')"
          />
        </el-form-item>
        <div style="display:flex">
          <el-form-item prop="test" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addEditQuestion', 'test')">
            <el-select v-model="form.test" :placeholder="messageTranslates('addEditQuestion', 'test')" @change="testChange()">
              <el-option v-for="item in tests" :key="item.id" :label="item.name" :value="item.id" />
            </el-select>
          </el-form-item>

          <el-form-item prop="task" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addEditQuestion', 'task')">
            <el-select v-model="form.task" :placeholder="messageTranslates('addEditQuestion', 'task')">
              <el-option v-for="item in tasksList" :key="item.id" :label="item.name" :value="item.id" />
            </el-select>
          </el-form-item>
          <el-form-item prop="type" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addEditQuestion', 'type')">
            <el-select v-model="form.type" :placeholder="messageTranslates('addEditQuestion', 'type')">
              <el-option v-for="item in types" :key="item" :label="item" :value="item" />
            </el-select>
          </el-form-item>
        </div>

        <!-- <div class="el-dialog__header" style="padding:0 0 20px 0;">
          <span class="el-dialog__title">
            Question
          </span>
        </div> -->
        <el-form-item prop="content" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addEditQuestion', 'question')">
          <el-tiptap
            v-model="form.content"
            lang="en"
            placeholder="Write something …"
            :extensions="extensions"
            :char-counter-count="false"
          />
        </el-form-item>

        <el-form-item id="questionPart" prop="part" size="mini" label="">
          <div v-if="form.test && (form.task!==2 && form.task!==3)" style="margin-left:120px">{{ messageTranslates('addEditQuestion', 'noPart') }}
          </div>
          <div v-if="form.test && form.task===2">
            <el-form-item prop="" size="mini" :label="messageTranslates('addEditQuestion', 'reading')">
              <el-tiptap
                v-model="toeflReading"
                lang="en"
                placeholder="Write something …"
                :extensions="extensions"
                :char-counter-count="false"
              />
            </el-form-item>

            <el-form-item prop="" size="mini" :label="messageTranslates('addEditQuestion', 'listening')">
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

            <el-form-item prop="" size="mini" :label="messageTranslates('addEditQuestion', 'transcript')">
              <el-tiptap
                v-model="toeflTranscript"
                lang="en"
                placeholder="Write something …"
                :extensions="extensions"
                :char-counter-count="false"
              />
            </el-form-item>
          </div>
          <div v-if="form.test && form.task===3">
            <el-form-item prop="" size="mini" :label="messageTranslates('addEditQuestion', 'chart')" :rules="[{ required: true }]">
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
                <el-button size="small" type="primary">{{ messageTranslates('addEditQuestion', 'uploadButton') }}</el-button>
                <div slot="tip" class="el-upload__tip">{{ messageTranslates('addEditQuestion', 'validateImgUpload') }}</div>
              </el-upload>
            </el-form-item>
          </div>
        </el-form-item>
      </el-form>
    </div>
    <span slot="footer" class="dialog-footer">
      <el-button size="mini" @click="dialogVisible = false">{{ messageTranslates('addEditQuestion', 'cancel') }}</el-button>
      <el-button v-if="isLearnerContributed" type="success" size="mini" @click="approveQuestion">{{ messageTranslates('addEditQuestion', 'approve') }}</el-button>
      <el-button size="mini" type="primary" @click="submit">{{ messageTranslates('addEditQuestion', 'submit') }}</el-button>
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
  OrderedList
} from 'element-tiptap'
import questionService from '../../services/question.service'
import * as stringUtil from '@/utils/string'
import { UserRole, QUESTION_STATUS } from '../../app.constant'
export default {
  name: 'AddEditQuestion',
  components: {
  },
  props: {
  },
  data() {
    return {
      dialogVisible: false,
      isEdit: false,
      tests: null,
      tasks: null,
      types: null,
      form: {
        id: null,
        name: null,
        test: null,
        task: null,
        type: null,
        content: `<p>Content here...</p>
        <p></p>`,
        part: null
      },
      toeflReading: `<p>Reading content here...</p>
        <p></p>
        <p></p>
        <p></p>
        <p></p>`,
      toeflListening: null,
      toeflTranscript: `<p>Transcipt content here...</p>
        <p></p>
        <p></p>
        <p></p>
        <p></p>`,
      ieltsChart: null,
      LISTENING_FILE_MAX_SIZE: 10000000,
      CHART_FILE_MAX_SIZE: 3000000,
      LISTENING_TYPE_FILE: ['video/mp4', 'audio/mpeg'],
      CHART_TYPE_FILE: ['image/jpeg', 'image/png'],
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
      ],
      fileList: [],
      isLearnerContributed: false,
      chartFileList: [],
      questionExist: null
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    tasksList() {
      return this.form.test ? this.tasks.filter(r => r.section.testId == this.form.test) : null
    }
  },
  mounted() {
    questionService.getAddEditQuestionData().then(rs => {
      this.tasks = rs.tasks
      this.tests = rs.tests
      this.types = rs.types
    })
  },
  methods: {
    openDialog() {
      this.resetData()
      this.isEdit = false
      this.dialogVisible = true
      this.$refs['questionForm'].resetFields()
    },
    openEditDialog(question) {
      this.fileList = []
      this.chartFileList = []

      questionService.getById(question.id).then(rs => {
        this.form.id = rs.id
        this.form.test = rs.testId
        this.form.name = rs.title
        this.form.type = rs.type
        this.form.task = rs.taskId
        this.form.content = rs.questionsPart.filter(r => r.name == 'Question')[0]?.content
        this.toeflReading = rs.questionsPart.filter(r => r.name == 'Reading')[0]?.content
        this.toeflListening = rs.questionsPart.filter(r => r.name == 'Listening')[0]?.content
        this.toeflTranscript = rs.questionsPart.filter(r => r.name == 'Transcript')[0]?.content
        this.ieltsChart = rs.questionsPart.filter(r => r.name == 'Chart')[0]?.content
        if (rs.status === QUESTION_STATUS.CONTRIBUTED) {
          this.isLearnerContributed = true
        }

        this.questionExist = rs
        this.isEdit = true
        this.dialogVisible = true
        console.log('----', this.questionExist)

        let file
        var fileListenName = this.questionExist.questionsPart.filter(r => r.name === 'Listening')[0]
        if (fileListenName) {
          file = {
            name: fileListenName.content,
            url: 'http://localhost:6990/audio/' + this.questionExist.id + '.' + fileListenName.content
          }
          this.fileList = [file]
        }

        var fileChartName = this.questionExist.questionsPart.filter(r => r.name === 'Chart')[0]
        if (fileChartName) {
          file = {
            name: fileChartName.content,
            url: 'http://localhost:6990/audio/' + this.questionExist.id + '.' + fileChartName.content
          }
          this.chartFileList = [file]
        }
      })
    },
    testChange() {
      this.form.task = this.tasksList[0].id
    },
    submit() {
      this.$refs['questionForm'].validate(async valid => {
        if (valid) {
          if (this.form.task === 2) {
            if (!this.toeflReading || !this.fileList[0] || !this.toeflTranscript) {
              this.$message.warning(`Please fill all fields.`)
              return null
            }
            this.form.part = {
              reading: this.toeflReading,
              listening: this.fileList[0],
              transcript: this.toeflTranscript
            }
          } else if (this.form.task === 3) {
            if (!this.chartFileList[0]) {
              this.$message.warning(`Please fill all fields.`)
              return null
            }
            this.form.part = { chart: this.chartFileList[0] }
          } else {
            this.form.part = null
          }

          const formData = new FormData()

          formData.set('Title', this.form.name)
          formData.set('Test', this.form.test)
          formData.set('TaskId', this.form.task)
          formData.set('Type', this.form.type)
          formData.set('Status', this.currentUser.role == UserRole.ADMIN ? QUESTION_STATUS.APPROVED : QUESTION_STATUS.CONTRIBUTED)

          formData.set('QuestionParts[0][Name]', 'Question')
          formData.set('QuestionParts[0][Content]', this.form.content)
          formData.set('QuestionParts[0][Order]', 1)
          formData.set(`QuestionParts[0][QuestionId]`, this.questionExist ? this.questionExist?.id : 0)

          if (this.form.part) {
            let order = 2
            let count = 1
            for (var p in this.form.part) {
              if (this.form.part[p]?.raw) {
                formData.set(`QuestionParts[${count}][Content]`, this.form.part[p].raw.name)
                formData.set(`QuestionParts[${count}][FileName]`, this.form.part[p].raw.name)
                formData.set(`QuestionParts[${count}][FileExtension]`, stringUtil.getFileExtension(this.form.part[p]?.name))
                formData.set(`UploadedFile`, this.form.part[p]?.raw)
              } else {
                formData.set(`QuestionParts[${count}][Content]`, this.form.part[p])
              }

              if (this.isEdit) {
                formData.set(`QuestionParts[${count}][QuestionId]`, this.questionExist.id)
              }

              formData.set(`QuestionParts[${count}][Order]`, order)
              formData.set(`QuestionParts[${count}][Name]`, this.partNameFormat(p))
              order += 1
              count += 1
            }
          }

          if (this.isEdit) {
            formData.set(`UserId`, this.questionExist.userId)
            formData.set(`Id`, this.questionExist.id)

            questionService.updateQuestion(formData).then(rs => {
              if (rs) {
                this.$notify.success({
                  title: 'Question updated.',
                  message: 'Question updated successfully.',
                  type: 'success',
                  duration: 2000
                })
                this.$emit('refreshQuestion')
                this.resetData()
                this.dialogVisible = false
                this.$refs['questionForm'].resetFields()
              }
            })
          } else {
            questionService.createQuestion(formData).then(rs => {
              if (rs) {
                this.$notify.success({
                  title: 'Question added.',
                  message: 'Question added successfully.',
                  type: 'success',
                  duration: 2000
                })
                this.resetData()
                this.dialogVisible = false
                this.$emit('refreshQuestion')
                this.$refs['questionForm'].resetFields()
              }
            })
          }
        }
      })
    },
    approveQuestion() {
      questionService.approveQuestion(this.questionExist.id).then(rs => {
        if (rs) {
          this.$notify.success({
            title: 'Question approved.',
            message: 'Question approved successfully.',
            type: 'success',
            duration: 2000
          })
          this.dialogVisible = false
          this.$emit('refreshQuestion')
          this.resetData()
          this.isLearnerContributed = false
        }
      })
    },

    // -------------------
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
    publishQuestion(e) {
      if (this.currentUser.role === UserRole.ADMIN) {
        questionService.publishQuestion(e.id).then(r => {
          if (r) {
            this.$notify.success({
              title: 'Question published.',
              message: 'Question published successfully.',
              type: 'success',
              duration: 2000
            })
            this.dialogVisible = false
            this.$emit('refreshQuestion')
            this.resetData()
            this.isLearnerContributed = false
          }
        })
      }
    },
    handleClose() {
      this.$store.dispatch('question/clearSelectedQuestion')
      this.dialogVisible = false
    },
    resetData() {
      this.fileList = []
      this.chartFileList = []
      this.form.name = null
      this.form.type = null
      this.form.test = null
      this.form.task = null
      this.form.content = `<p>Content here...</p>
        <p></p>`
      this.form.part = null

      this.toeflReading = `<p>Reading content here...</p>
        <p></p>
        <p></p>
        <p></p>
        <p></p>`
      this.toeflListening = null
      this.toeflTranscript = `<p>Transcipt content here...</p>
        <p></p>
        <p></p>
        <p></p>
        <p></p>`
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

