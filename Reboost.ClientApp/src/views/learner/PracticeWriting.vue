<template>
  <el-row>
    <el-col :span="24" class="padding-10">
      <el-row>
        <el-col v-if="col != 24" :span="13" class="padding-10">
          <el-tabs type="border-card">
            <el-tab-pane label="Topic" style="padding: 0 5px;">
              <el-row>
                <el-col style="padding: 0 10px; ">
                  <div style="display: flex; justify-content: space-between;">
                    <div>
                      <div v-if="!isShowListeningTab && getReading != ''" class="title-tab">1. Reading Passage</div>
                      <div v-if="isShowListeningTab" class="title-tab">2. Reading Passage</div>
                      <div v-if="getReading == '' && getChart != ''" class="title-tab">1. Chart Passage</div>
                      <div v-if="getReading != '' && getChart != '' && isShowChart" class="title-tab">2. Chart Passage</div>
                      <div style="margin-top: 5px; display: flex;" />
                    </div>

                  </div>

                  <hr style="margin: 0;">

                </el-col>
              </el-row>
              <el-row>
                <div v-if="!isShowListeningTab && getReading != ''">
                  <div class="body-practice">
                    <div v-if="!closeTimer" class="tip" style="display: flex; align-items: center; justify-content: space-between;">
                      <div style="flex-grow: 1;">
                        <p style="line-height: 21px;"> <span style="font-weight: 600; font-size: 13px;">Direction: </span> Give yourseft 3 minutes to read the passage.</p>
                      </div>
                      <div v-if="!isShowTimer">
                        <el-button size="medium" @click="toggleBtnShowTab()">START</el-button>
                      </div>
                      <div v-if="getListenting != ''" style="margin-left: 14px;">
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
                <el-col v-if="col == 12 && getReading =='' && getListenting != '' || isShowListeningTab || getReading == ''" :span="24" class="padding-10">
                  <div v-if="isShowListeningTab" style="width: 100%;">
                    <div class="header-passage" style="display:flex; justify-content: space-between;">
                      LISTEN TO PART OF LECTURE ON THE SAME TOPIC
                      <div v-if="getReading != ''">
                        <el-button size="medium" @click="BackClick()">Back</el-button>
                      </div>
                    </div>
                    <div class="body-passage">
                      <div v-if="getListenting != ''" class="body-practice">
                        <div>
                          <audio controls style="width: 100%;">
                            <source :src="'/assets/' + getListenting.content" type="audio/mpeg">
                            Your browser does not support the audio element.
                          </audio>
                        </div>
                      </div>
                      <div style="padding: 0 20px;">
                        <div class="script-select" style="padding: 5px 20px; border: 2px solid #eff0f2;" @click="toggleBtnShowScript">
                          <i class="el-icon-document-copy" />
                          Audio Script
                        </div>
                      </div>
                      <div v-if="!isShowScript" style="margin-bottom: 20px;" />
                      <div v-if="isShowScript" class="body-practice">
                        <div class="body-transcript">
                          <p style="line-height: 21px; text-align: justify;"> {{ getTranscript.content }}</p>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div v-if="isShowChart || (getReading == '' && getChart != '')" style="width: 100%;">
                    <div class="header-passage" style="display:flex; justify-content: space-between;">
                      CHART
                      <div v-if="getReading != ''">
                        <el-button @click="BackClick()">Back</el-button>
                      </div>
                    </div>
                    <div class="body-passage" />
                    <div class="body-practice" style="padding: 30px;" />
                    <div style="display: flex; justify-content: center;">
                      <img src="../../assets/chart/1.png" :alt="getChart.content" style="max-height: 300px; max-width: 500px;">
                    </div>
                  </div>
                </el-col>
              </el-row>
            </el-tab-pane>
            <el-tab-pane label="Samples" />
            <el-tab-pane label="Discussions">Discussions</el-tab-pane>
            <el-tab-pane label="Similiar">Similiar</el-tab-pane>
          </el-tabs>
        </el-col>
        <el-col :span="col" class="padding-10">
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
          <el-col :span="24" class="padding-10" style="height: 100%;">
            <el-row style="margin-bottom: 15px;">
              <el-col :span="24" class="question-con">
                <span style="font-weight: bold;">Question:</span>
                {{ getQuestion.content }}
              </el-col>
            </el-row>
            <el-row>
              <el-col>
                <div style="height: 100%">
                  <div class="header-passage" style="display:flex; justify-content: space-between; border: 1px solid #ccc; border-bottom: none;">
                    <div v-if="getQuestion != ''">
                      <el-button @click="ToggleShowCount()">Hide Word Count</el-button>
                    </div>
                    <div v-if="getQuestion != ''">
                      <el-button @click="Submit()">Submit & Request Review</el-button>
                    </div>
                  </div>
                  <editor-text api-key="biyg6d0lkbqha5om2iu2q5i4qq0ecq58nlrn5n2wd9b5vtuh" :init="initTextArea" @PreInit="ToggleShowCount()" />
                </div>
              </el-col>
            </el-row>
          </el-col>
        </el-col>
      </el-row>
    </el-col>
  </el-row>
</template>

<script>
// @ is an alias to /src
// import http from '@/utils/axios'
import Editor from '@tinymce/tinymce-vue'
import questionService from '../../services/question.service'
export default {
  name: 'PracticeWriting',
  components: {
    'editor-text': Editor
  },
  data() {
    return {
      textarea: '',
      initTextArea: {
        height: 430,
        selector: 'textarea', // change this value according to your HTML
        plugins: 'wordcount',
        toolbar: '',
        content_style: 'body { line-height: 5px; font-size: 15px; }',
        menubar: false,
        branding: false,
        placeholder: 'Write your essay here...'
      },
      isShowTimer: false,
      isShowListeningTab: false,
      isShowChart: false,
      isShowScript: false,
      minute: 0,
      second: 1,
      closeTimer: false
    }
  },
  computed: {
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
    getListenting() {
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
    col() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.length == 1) {
          return 24
        }
      }
      return 11
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
      // return this.getDataQuestionParts.find(u => u.name == "Chart")
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
    this.$store.dispatch('question/loadQuestion', +this.$route.params.id)
  },
  methods: {
    Submit() {
      var data = {
        filename: new Date().getFullYear().toString() + (new Date().getMonth() + 1).toString() + new Date().getDate().toString() + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString() + '.pdf',
        text: '123'
      }
      // console.log(data);
      questionService.createDocument(data)
    },
    toggleBtnShowTab() {
      // this.isShowListeningTab = !this.isShowListeningTab;
      this.isShowTimer = true
      var abc = setInterval(() => {
        this.second--
        if (this.second < 0) {
          this.second = 59
          this.minute--
          if (this.minute < 0) {
            clearInterval(abc)
            if (this.getListenting != '') { this.isShowListeningTab = true }
            if (this.getChart != '') { this.isShowChart = true }
          }
        }
      }, 1000)
    },
    BackClick() {
      this.isShowListeningTab = false
      this.closeTimer = true
    },
    toggleBtnShowScript() {
      this.isShowScript = !this.isShowScript
    },
    ToggleShowCount() {
      this.initTextArea.height = 3000
    }
  }
}
</script>

<style scoped>
.padding-10 {
  padding: 10px;
}

.question-con {
  padding: 10px 30px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
  border: 1px solid #ebeef5;
  background-color: #fff;
  border-radius: 4px;
  font-size: 12px;
  font-style: italic;
}

.header-passage {
  padding: 5px 10px;
  min-height: 40px;
  line-height: 37px;
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
  padding: 15px 20px;
}

.body-transcript {
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
</style>
