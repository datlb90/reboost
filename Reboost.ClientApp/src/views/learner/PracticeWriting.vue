<template>
  <div style="height: 100%">
    <splitpanes class="default-theme" vertical style="height: 100%; position: fixed; width: 100%;">
      <pane>
        <el-tabs type="border-card" style="height: 100%">
          <el-tab-pane label="Topic" style="padding: 0;height: 100%; position: relative;">
            <div class="par-content">
              <el-row>
                <el-col style="padding: 0 10px; ">
                  <div style="display: flex; justify-content: space-between;">
                    <div>
                      <div class="title-tab">1. {{ getDataQuestion.title }}</div>
                      <div style="margin-top: 5px; display: flex;">
                        <div style="font-size: 15px; color: #44ce6f; margin-right: 25px;">{{ getDataQuestion.test }} {{ getDataQuestion.section }}</div>
                        <div style="display: flex;  margin-right: 20px;">
                          <img class="img-icon" src="../../assets/img/like.png" alt="">
                          <label for="" style="font-weight: 200; font-size: 13px;">{{ getDataQuestion.like }}</label>
                        </div>
                        <div style="display: flex;  margin-right: 20px;">
                          <img class="img-icon" src="../../assets/img/dislike.png" alt="">
                          <label for="" style="font-weight: 200; font-size: 13px;">{{ getDataQuestion.dislike }}</label>
                        </div>
                        <div style="display: flex;  margin-right: 20px;">
                          <img class="img-icon" src="../../assets/img/heart.png" alt="">
                          <label for="" style="font-weight: 200; font-size: 13px;">Add to List</label>
                        </div>
                        <div style="display: flex;  margin-right: 20px;">
                          <img class="img-icon" src="../../assets/img/share.png" alt="">
                          <label for="" style="font-weight: 200; font-size: 13px;">Share</label>
                        </div>
                      </div>
                    </div>
                  </div>
                  <hr style="margin: 20px 0;">
                  <div class="tip">
                    <p style="line-height: 21px;"> <span style="font-weight: 600; font-size: 13px;">Direction: </span>{{ getDataQuestion.direction }}</p>
                  </div>
                </el-col>
              </el-row>
              <el-row class="padding-10">
                <el-col :span="24" class="question-con">
                  <span style="font-weight: bold;">Question:</span>
                  {{ getQuestion.content }}
                </el-col>
              </el-row>
              <el-row>
                <div v-if="!isShowListeningTab && getReading != ''">
                  <div class="body-practice">
                    <div class="title-tab">
                      Reading Passage
                    </div>
                  </div>
                  <div class="body-practice">
                    <div v-if="!closeTimer" class="tip" style="display: flex; align-items: center; justify-content: space-between;">
                      <div style="flex-grow: 1;">
                        <p style="line-height: 21px;"> <span style="font-weight: 600; font-size: 13px;">Direction: </span> Give yourseft 3 minutes to read the passage.</p>
                      </div>
                      <div v-if="!isShowTimer">
                        <el-button size="medium" @click="toggleBtnShowTab()">START</el-button>
                      </div>
                      <div v-if="getListening != ''" style="margin-left: 14px;">
                        <div v-if="isShowTimer && !closeTimer">
                          {{ minute }} : {{ second }}
                        </div>
                      </div>
                    </div>
                  </div>
                  <div v-if="getReading != ''" class="body-practice">
                    <p v-if="isShowTimer && !closeTimer" style="line-height: 21px; text-align: justify;"> {{ getReading.content }}</p>
                  </div>
                </div>
                <el-col v-if="getReading =='' && getListening != '' || isShowListeningTab || getReading == ''" :span="24" class="padding-10">
                  <div v-if="isShowListeningTab" style="width: 100%;">
                    <div class="header-listening" style="display:flex; justify-content: space-between;">
                      <div class="title-tab">
                        LISTEN TO PART OF LECTURE ON THE SAME TOPIC
                      </div>
                      <div v-if="getReading != ''">
                        <el-button size="medium" @click="backClick()">Back</el-button>
                      </div>
                    </div>
                    <hr style="margin: 10px 0;">
                    <div>
                      <div v-if="getListening != ''" style="padding: 10px 0;">
                        <div>
                          <audio controls style="width: 100%;">
                            <source :src="'/assets/' + getListening.content" type="audio/mpeg">
                          </audio>
                        </div>
                      </div>
                      <div>
                        <div class="script-select" style="padding: 15px 20px; border: 2px solid #eff0f2;" @click="toggleBtnShowScript">
                          <i class="el-icon-document-copy" />
                          Audio Script
                        </div>
                      </div>
                      <div v-if="isShowScript">
                        <div class="body-transcript">
                          <p style="line-height: 21px; text-align: justify;"> {{ getTranscript.content }}</p>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div v-if="isShowChart || (getReading == '' && getChart != '')" style="width: 100%;">
                    <div class="header-passage" style="display:none; justify-content: space-between;">
                      CHART
                      <div v-if="getReading != ''">
                        <el-button @click="backClick()">Back</el-button>
                      </div>
                    </div>
                    <div style="display: flex; justify-content: center;">
                      <img src="../../assets/chart/1.png" :alt="getChart.content" style="height: 400px; width: 100%;">
                    </div>
                  </div>
                </el-col>
              </el-row>
            </div>

          </el-tab-pane>
          <el-tab-pane label="Samples" />
          <el-tab-pane label="Discussions">
            <tabDisCussion />
          </el-tab-pane>
          <el-tab-pane label="Similiar">Similiar</el-tab-pane>
        </el-tabs>
      </pane>
      <pane>
        <div style="height: 100%; display: flex; flex-direction: column">
          <div class="header-passage" style="display:flex; justify-content: space-between;">
            <div v-if="getQuestion != ''">
              <el-button size="mini" @click="toggleShowCount()">Hide Word Count</el-button>
            </div>
            <div v-if="getQuestion != ''">
              <el-button size="mini" @click="submit()">Submit & Request Review</el-button>
            </div>
          </div>
          <div style="flex-grow: 1">
            <editor-text v-model="writingContent" output-format="text" api-key="biyg6d0lkbqha5om2iu2q5i4qq0ecq58nlrn5n2wd9b5vtuh" :init="initTextArea" @PreInit="toggleShowCount()" />
          </div>
        </div>
      </pane>
    </splitpanes>
  </div>
</template>

<script>
// @ is an alias to /src
// import http from '@/utils/axios'
import Editor from '@tinymce/tinymce-vue'
import documentService from '../../services/document.service'
import TabDisCussion from '../learner/PracticeWriting_TabDiscussion.vue'
import {
  Splitpanes,
  Pane
} from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'

export default {
  name: 'PracticeWriting',
  components: {
    'editor-text': Editor,
    'splitpanes': Splitpanes,
    'pane': Pane,
    'tabDisCussion': TabDisCussion
  },
  data() {
    return {
      textarea: '',
      initTextArea: {
        height: '100%',
        selector: 'textarea',
        plugins: 'wordcount',
        toolbar: '',

        menubar: false,
        branding: false,
        placeholder: 'Write your essay there...',
        min_width: 400,
        forced_root_block: false
      },
      isShowTimer: false,
      isShowListeningTab: false,
      isShowChart: false,
      isShowScript: false,
      minute: 0,
      second: 3,
      closeTimer: false,
      writingContent: '',
      questionId: undefined
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    getDataQuestion() {
      var data = this.$store.getters['question/getSelected']
      return data
    },
    getDataQuestionParts() {
      return this.$store.getters['question/getSelected']['questionsPart']
    },
    getReading() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Reading')) {
          return this.getDataQuestionParts.find(u => u.name == 'Reading')
        }
      }
      return ''
    },
    getListening() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Listening')) {
          return this.getDataQuestionParts.find(u => u.name == 'Listening')
        }
      }
      return ''
    },
    getQuestion() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Question')) {
          return this.getDataQuestionParts.find(u => u.name == 'Question')
        }
      }
      return ''
    },
    getChart() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Chart')) {
          var chart = this.getDataQuestionParts.find(u => u.name == 'Chart')
          chart.content = '../../assets/' + chart.content.trim()
          return chart
        }
      }
      return ''
    },
    getTranscript() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Transcript')) {
          return this.getDataQuestionParts.find(u => u.name == 'Transcript')
        }
      }
      return ''
    }
  },
  mounted() {
    this.questionId = this.$route.params.id
    this.$store.dispatch('question/loadQuestion', +this.questionId)
  },
  methods: {
    submit() {
      var data = {
        filename: new Date().getFullYear().toString() + (new Date().getMonth() + 1).toString() + new Date().getDate().toString() + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString() + '.pdf',
        text: this.writingContent,
        userId: this.currentUser.id,
        questionId: +this.questionId
      }
      console.log('SUBMIT_DATA', data)

      documentService.submitDocument(data).then(rs => {
        this.$notify({
          title: 'Success',
          message: 'Submit success',
          type: 'success',
          duration: 2000
        })
      })
    },
    toggleBtnShowTab() {
      this.isShowTimer = true
      var abc = setInterval(() => {
        this.second--
        if (this.second < 0) {
          this.second = 59
          this.minute--
          if (this.minute < 0) {
            clearInterval(abc)
            if (this.getListening != '') {
              this.isShowListeningTab = true
            }
            if (this.getChart != '') {
              this.isShowChart = true
            }
          }
        }
      }, 1000)
    },
    backClick() {
      this.isShowListeningTab = false
      this.closeTimer = true
    },
    toggleBtnShowScript() {
      this.isShowScript = !this.isShowScript
    },
    toggleShowCount() {
      this.initTextArea.height = 3000
    }
  }
}
</script>

<style scoped>
.par-content {
  position: absolute;
  /* padding: 10px; */
  top: 0;
  left: 0;
  overflow-y: scroll;
  height: 100%;
  -ms-overflow-style: none;
  /* IE and Edge */
  scrollbar-width: none;
}

.par-content::-webkit-scrollbar {
  width: 7px;
}

/* Handle */
.par-content::-webkit-scrollbar-thumb {
  background: #999;
  border-radius: 4px;
}

/* Handle on hover */
.par-content::-webkit-scrollbar-thumb:hover {
  background: #777;
}

.padding-10 {
  padding: 10px;
}

.question-con {
  padding: 20px 35px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
  border: 1px solid #ebeef5;
  background-color: #fff;
  border-radius: 4px;
  font-size: 12px;
  font-style: italic;
}

.header-passage {
  padding: 4px 10px;
  font-size: 13px;
  background-color: #f5f7fa;
  border: 2px solid #eff0f2;
}

.body-passage {
  border-bottom: 2px solid #eff0f2;
  border-left: 2px solid #eff0f2;
  border-right: 2px solid #eff0f2;
}

.body-practice {
  padding: 10px;
}

.body-transcript {
  margin-top: 20px;
  padding: 20px;
  border: 2px solid #eff0f2;
}

.btn-start {
  background-color: #e8e8e8;
  width: 90px;
  height: 30px;
  padding: 0;
  border-radius: 0;
}

.footer-end {
  display: flex;
  justify-content: flex-end;
  padding: 0 20px;
  margin-bottom: 40px;
}

.title-tab {
  font-size: 21px;
  font-weight: 600;
  color: rgb(23, 56, 82);
}

.img-icon {
  height: 17px;
  width: 20px;
  padding-right: 5px;
}

.tip {
  padding: 10px 15px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
}

.script-select:hover {
  cursor: pointer;
  background-color: #f5f7fa;
}

.el-tab-pane {
  min-height: 650px;
}
.splitpanes--vertical>.splitpanes__splitter {
  width: 55px;
  background: linear-gradient(90deg, #ccc, #111);
}

/* .splitpanes--vertical>.splitpanes__splitter:before {
  display: none;
  left: -30px;
  right: -30px;
  height: 100%;
} */
</style>
