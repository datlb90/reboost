<template>
  <div id="practiceWritingContainer" :style="{ height: containerHeight, visibility: loadCompleted?'visible': 'hidden' }">
    <splitpanes class="default-theme" vertical style="height: 100%; width: 100%;">
      <pane>
        <el-tabs type="border-card" style="height: 100%;" @tab-click="showDiscussion">
          <el-tab-pane label="Topic">
            <div style="height: 100%;display: flex; flex-direction: column">
              <div style="margin-bottom: 8px;">
                <el-row>
                  <el-col>
                    <div style="display: flex; justify-content: space-between;">
                      <div>
                        <div class="title-tab">1. {{ getDataQuestion.title }}</div>
                        <div style="display: flex;">
                          <div style="font-size: 13px; color: #44ce6f; margin-right: 25px;">{{ getDataQuestion.test }} {{ getDataQuestion.section }}</div>
                          <div style="display: flex;  margin-right: 20px;">
                            <img class="img-icon" src="../../assets/img/like.png" alt="">
                            <label for="" style="font-weight: 200; font-size: 13px;">{{ getDataQuestion.like }}</label>
                          </div>
                          <div style="display: flex;  margin-right: 20px;">
                            <img class="img-icon" src="../../assets/img/dislike.png" alt="">
                            <label for="" style="font-weight: 200; font-size: 13px;">{{ getDataQuestion.dislike }}</label>
                          </div>
                        </div>
                      </div>
                      <div>
                        <!-- <el-switch v-model="isTest" style="display: block" active-color="#13ce66" inactive-color="#ff4949" active-text="Test" inactive-text="Practice" @change="changedOption()" /> -->
                        <el-checkbox v-model="isTest" size="mini" border @change="changedOption()">Test Mode</el-checkbox>
                      </div>
                    </div>
                    <hr style="margin: 0; margin-bottom: 8px;">
                    <div class="tip" style=" margin-bottom: 8px;">
                      <pre style="font-size: 13px; color: #6084a4;"> <span style="font-weight: 600;">Direction: </span> <span v-if="hideDirection == 'Hide'" v-html="getDataQuestion.direction" /> <span class="hide-direction" @click="toggleDirection()">{{ hideDirection }}</span></pre>
                    </div>
                  </el-col>
                </el-row>

              </div>
              <div id="parent-scroll" style="flex-grow: 1;position: relative;">
                <div id="child-scroll" class="par-content default">
                  <el-row style="margin-bottom: 8px;">
                    <el-col :span="24" class="question-con">
                      <div>
                        <pre style="font-size: 13px;"><span style="font-weight: 600;">Question: </span>{{ getQuestion.content }}</pre>
                      </div>
                    </el-col>
                  </el-row>
                  <el-row>
                    <div v-if="!isShowListeningTab && getReading != ''">
                      <div class="header-practice" style="margin-bottom: 8px; display: flex;">
                        <div style="flex-grow: 1; display: flex; align-items: center;">
                          Reading Passage
                        </div>
                        <div v-if="!isShowTimer">
                          <el-button size="mini" @click="toggleBtnShowTab()">Go to listening</el-button>
                        </div>
                      </div>
                      <div v-if="!closeTimer" class="body-practice" style="margin-bottom: 8px;">
                        <div class="tip" style="display: flex; align-items: center; justify-content: space-between;">
                          <div style="flex-grow: 1;">
                            <pre style="font-size: 13px; color: #6084a4;"> <span style="font-weight: 600;">Direction: </span>Give yourseft 3 minutes to read the passage.</pre>
                          </div>
                          <div v-if="getListening != ''" style="margin-left: 14px;">
                            <div v-if="isShowTimer && !closeTimer">
                              {{ minute }} : {{ second }}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <el-col v-if="getReading =='' && getListening != '' || isShowListeningTab || getReading == ''" :span="24">
                      <div v-if="isShowListeningTab" style="width: 100%;">
                        <div class="header-practice" style="display:flex; justify-content: space-between;margin-bottom: 8px; ">
                          <div style="display: flex; align-items: center;">
                            LISTEN TO PART OF LECTURE ON THE SAME TOPIC
                          </div>
                          <div v-if="getReading != ''">
                            <el-button size="mini" @click="backClick()">Back</el-button>
                          </div>
                        </div>
                        <hr style="margin:0; margin-bottom: 8px; ">
                        <div>
                          <audio v-if="getListening != ''" controls style="width: 100%; height: 35px; margin-bottom: 3px;">
                            <source :src="'/assets/' + getListening.content" type="audio/mpeg">
                          </audio>
                          <div class="script-select" style="border: 2px solid #eff0f2; display: flex; padding: 5px 10px;" @click="toggleBtnShowScript">
                            <div style="flex-grow: 1;">
                              <i class="el-icon-document-copy" />
                              Audio Script
                            </div>
                            <div :class="{'rotate-icon' : isShowScript}">
                              <i class="fas fa-caret-down" />
                            </div>
                          </div>

                        </div>
                      </div>
                    </el-col>
                  </el-row>
                  <div v-if="getReading != '' && !isShowListeningTab && isShowReading" style="margin: 0;">
                    <pre> {{ getReading.content }}</pre>
                  </div>
                  <div v-if="isShowScript && isShowListeningTab" class="body-transcript" style="margin: 0;">
                    <pre> {{ getTranscript.content }}</pre>
                  </div>
                </div>
                <div v-if="isShowChart || (getReading == '' && getChart != '')" style="position: absolute; top: 0; left: 0; height: 100%; width: 100%;">
                  <div style="height: 100%; width: 100%; display: flex; justify-content: center; align-items: center;">
                    <img src="../../assets/chart/1.png" :alt="getChart.content" style="max-height: 100%; max-width: 100%;">
                  </div>
                </div>
              </div>
            </div>

          </el-tab-pane>
          <el-tab-pane label="Samples" style="height: 100%; position: relative;">
            <div class="par-content">
              <tab-samples />
            </div>
          </el-tab-pane>
          <el-tab-pane label="Rubric" style="height: 100%; position: relative;">
            <div class="par-content">
              <tab-rubric />
            </div>
          </el-tab-pane>
          <el-tab-pane label="Discussions" style="height: 100%; position: relative;">
            <div class="par-content" style="padding-right: 10px;">
              <tab-discussion />
            </div>
          </el-tab-pane>
          <el-tab-pane label="Similiar">Similiar</el-tab-pane>
        </el-tabs>
      </pane>
      <pane v-if="!tabDisCussionShowed">
        <div style="height: 100%; display: flex; flex-direction: column;">
          <div class="header-passage" style="display:flex; justify-content: space-between; border: 1px solid #e2e2e2; padding: 5px;">
            <div v-if="getQuestion != '' && !writtingSubmitted">
              <el-button size="mini" @click="toggleShowCount()">Hide Word Count</el-button>
              <span v-if="isShowCountWord && countWord != 0" style="margin-left: 15px;">Words: {{ countWord }}</span>
            </div>
            <div v-if="writtingSubmitted">
              <el-tag type="success">Your writting has been successfully submitted. You can request a review now.</el-tag>
            </div>
            <div v-if="getQuestion != ''">
              <el-button v-if="!writtingSubmitted && !hasSubmitionForThisQuestion" size="mini" :disabled="!(writingContent && writingContent.length > 0)" @click="submit()">Submit & Request Review</el-button>
              <el-dropdown v-if="writtingSubmitted || hasSubmitionForThisQuestion" size="mini" @command="checkoutVisibles">
                <el-button size="mini">
                  Get Writting Preview
                </el-button>
                <el-dropdown-menu slot="dropdown" size="mini">
                  <el-dropdown-item command="free">Free Peer Review</el-dropdown-item>
                  <el-dropdown-item command="checkout">Pro Rater Review</el-dropdown-item>
                  <el-dropdown-item divided>View Review Sample</el-dropdown-item>
                </el-dropdown-menu>
              </el-dropdown>
              <el-button v-if="hasSubmitionForThisQuestion" style="margin-left:5px" size="mini" :disabled="!(writingContent && writingContent.length > 0)" @click="submit()">Save</el-button>
            </div>
          </div>
          <div style="flex-grow: 1;">
            <textarea v-model="writingContent" placeholder="Please input..." spellcheck="false" class="textarea-style" @keyup="countWords()" />
          </div>
        </div>
      </pane>
    </splitpanes>
    <div>
      <checkout :visible="checkoutVisible" @closed="checkoutVisible=false" />
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
// import http from '@/utils/axios'
import documentService from '../../services/document.service'
import userService from '../../services/user.service'
import reviewService from '../../services/review.service'
import TabDisCussion from '../learner/PracticeWriting_TabDiscussion.vue'
import TabRubric from '../learner/PracticeWriting_TabRubric.vue'
import TabSamples from '../learner/PracticeWriting_TabSamples.vue'
import CheckOut from '../../components/controls/CheckOut.vue'
import {
  Splitpanes,
  Pane
} from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
export default {
  name: 'PracticeWriting',
  components: {
    'splitpanes': Splitpanes,
    'pane': Pane,
    'tab-discussion': TabDisCussion,
    'tab-rubric': TabRubric,
    'tab-samples': TabSamples,
    'checkout': CheckOut
  },
  data() {
    return {
      loadCompleted: false,
      containerHeight: 0,
      textarea: '',
      isShowTimer: false,
      isShowListeningTab: false,
      isShowChart: false,
      isShowScript: false,
      isShowReading: true,
      minute: 0,
      second: 3,
      closeTimer: false,
      writingContent: '',
      questionId: undefined,
      isTest: false,
      countWord: 0,
      isShowCountWord: true,
      hideDirection: 'Hide',
      tabDisCussionShowed: false,
      writtingSubmitted: false,
      checkoutVisible: false,
      hasSubmitionForThisQuestion: false,
      timeSpent: 0,
      timeSpentInterval: null,
      idLocalStorage: '',
      timeout: null,
      submissionId: null
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    getDataQuestion() {
      var data = this.$store.getters['question/getSelected']
      if (data.direction) {
        data.direction = data.direction.trim()

        if (data.direction.substr(data.direction.length - 1) == '\n') {
          data.direction = data.direction.substring(0, data.direction.length - 1)
        }
      }
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
  async mounted() {
    this.questionId = this.$route.params.id
    this.$store.dispatch('question/loadQuestion', +this.questionId).then(rs => {
      this.calculateContainerHeight()
      this.loadCompleted = true
    })
    userService.hasSubmissionOnTaskOf(this.currentUser.id, this.questionId).then(rs => {
      if (rs) {
        this.email = this.currentUser.email
        this.hideDirection = 'Show'
      }
    })
    window.addEventListener('resize', this.calculateContainerHeight.bind(this))
    this.setIntervalForScroll = setInterval(() => {
      this.calculateStylePaddingScroll()
    }, 80)
    this.idLocalStorage = this.currentUser.username + '_QuestionId' + this.questionId
    this.loadData()
  },
  destroyed() {
    clearInterval(this.setIntervalForScroll)
  },
  methods: {
    loadData() {
      documentService.search(this.currentUser.id, this.questionId).then(rs => {
        console.log('Current user submition for this question', rs)
        if (rs && rs.length > 0) {
          const latestSubmition = rs[0]
          this.submissionId = rs[0]['id']
          this.writingContent = latestSubmition.text
          this.hasSubmitionForThisQuestion = true
          this.countWords()
        } else {
          if (localStorage.getItem(this.idLocalStorage) && localStorage.getItem(this.idLocalStorage) != '') {
            this.writingContent = localStorage.getItem(this.idLocalStorage)
            this.countWords()
          }
        }
      })
    },
    calculateContainerHeight() {
      const headerHeight = document.getElementById('header').clientHeight
      const containerHeight = window.innerHeight - headerHeight
      const elContainer = document.getElementById('practiceWritingContainer')
      elContainer.style.height = containerHeight + 'px'
    },
    calculateStylePaddingScroll() {
      const parentHeight = document.getElementById('parent-scroll').offsetHeight
      const childHeight = document.getElementById('child-scroll').scrollHeight
      if (parentHeight >= childHeight) {
        document.getElementById('child-scroll').style.paddingRight = '0'
      } else {
        document.getElementById('child-scroll').style.paddingRight = '10px'
      }
    },
    submit() {
      localStorage.removeItem(this.idLocalStorage)
      if (!this.writingContent) {
        this.$notify({
          title: 'Error',
          message: 'No thing to submit',
          type: 'error',
          duration: 1000
        })
        return
      }

      clearInterval(this.timeSpentInterval)
      var data = {
        filename: new Date().getFullYear().toString() + (new Date().getMonth() + 1).toString() + new Date().getDate().toString() + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString() + '.pdf',
        text: this.writingContent,
        userId: this.currentUser.id,
        questionId: +this.questionId,
        timeSpentInSeconds: this.timeSpent
      }
      // console.log('SUBMIT DATA', data)
      documentService.submitDocument(data).then(rs => {
        this.$notify({
          title: 'Success',
          message: 'Submit success',
          type: 'success',
          duration: 1000
        })
        this.writtingSubmitted = true
        this.hasSubmitionForThisQuestion = true
        this.timeSpent = 0
      })
    },
    toggleBtnShowTab() {
      this.isShowTimer = true
      this.isShowReading = false
      this.timeSpent = 0
      clearInterval(this.timeSpentInterval)
      if (this.isTest) {
        this.isShowReading = true
        this.timeSpentInterval = setInterval(() => {
          this.timeSpent++
          // console.log('TICK')
        }, 1000)
        var hld = setInterval(() => {
          this.second--
          if (this.second < 0) {
            this.second = 59
            this.minute--
            if (this.minute < 0) {
              this.isShowReading = false
              clearInterval(hld)
              if (this.getListening != '') {
                this.isShowListeningTab = true
              }
              if (this.getChart != '') {
                this.isShowChart = true
              }
            }
          }
        }, 1000)
      } else {
        this.isShowReading = false
        this.isShowTimer = false

        if (this.getListening != '') {
          this.isShowListeningTab = true
        }
        if (this.getChart != '') {
          this.isShowChart = true
        }
      }
    },
    toggleDirection() {
      if (this.hideDirection == 'Hide') { this.hideDirection = 'Show' } else {
        this.hideDirection = 'Hide'
      }
    },
    backClick() {
      this.isShowListeningTab = false
      if (this.minute <= 0 && this.second == 59) {
        this.closeTimer = true
      }
      this.isShowReading = true
    },
    toggleBtnShowScript() {
      this.isShowScript = !this.isShowScript
    },
    toggleShowCount() {
      this.isShowCountWord = !this.isShowCountWord
    },
    countWords() {
      this.countWord = this.writingContent.trim().split(/\b\S+\b/).length - 1
      if (!this.hasSubmitionForThisQuestion) {
        clearTimeout(this.timeout)
        this.timeout = setTimeout(() => {
          localStorage.setItem(this.idLocalStorage, this.writingContent)
        }, 500)
      }
    },
    changedOption() {
      this.timeSpent = 0
      clearInterval(this.timeSpentInterval)
      if (!this.isTest) {
        this.isShowReading = true
        this.minute = 0
        this.second = 3
        this.closeTimer = false
        this.isShowScript = false
        this.isShowTimer = false
      } else {
        if (!this.closeTimer) {
          this.isShowReading = false
        }
      }
    },
    showDiscussion(e) {
      if (e.label == 'Discussions') {
        this.tabDisCussionShowed = true
        this.$router.push('/PracticeWriting/' + this.getDataQuestion.id + '/discuss').catch(() => {})
      } else {
        this.$router.push('/PracticeWriting/' + this.getDataQuestion.id).catch(() => {})
        this.tabDisCussionShowed = false
      }
    },
    checkoutVisibles(e) {
      if (e == 'checkout') {
        this.checkoutVisible = true
      } else if (e == 'free') {
        this.createReview()
      }
    },
    logSmt(e) {
      console.log(e)
    },
    createReview() {
      reviewService.createReviewRequest({
        UserId: '123',
        SubmissionId: this.submissionId,
        FeedbackType: 'Free',
        Status: 'Waiting'
      })
    }
  }
}
</script>

<style>

.hide-direction {
  font-weight: bold;
  color: #409EFF;
}
.hide-direction:hover{
  cursor: pointer;
}
.el-tabs--border-card {
  display: flex !important;
  flex-direction: column !important;
  height: 100% !important;
}

.el-tabs__content {
  flex-grow: 1 !important;
}

.el-tab-pane {
  height: 100%;
  min-height: 0 !important;
}
#practiceWritingCheckoutContainer .el-dialog .el-dialog__header{
  padding: 20px 0 0 0 !important;
  height: 60px;
}
#practiceWritingCheckoutContainer .el-dialog .el-dialog__body{
  padding: 0 !important;
}

</style>
<style scoped>

.payment-method-title{
  display: flex;
  align-items: center;
}
.dialog-body{
  padding: 10px 0 10px 0;
  display: flex;
  align-items: center;
  border-bottom: solid 1px rgb(187, 187, 187);
}

.par-content {
  position: absolute;
  padding-right: 10px;
  top: 0;
  left: 0;
  overflow-y: scroll;
  height: 100%;
  width: 100%;
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
  padding: 0px;
}
.default{
  padding: 0;
  margin: 0;
}

.question-con {
  padding: 5px 10px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
  border: 1px solid #ebeef5;
  background-color: #fff;
  border-radius: 4px;
}

.header-passage {
  font-size: 13px;
  background-color: #f5f7fa;
  border: 2px solid #eff0f2;
}

.body-passage {
  border-bottom: 2px solid #eff0f2;
  border-left: 2px solid #eff0f2;
  border-right: 2px solid #eff0f2;
}

.header-practice {
  font-size: 14px;
  font-weight: 600;
  color: rgb(23, 56, 82);
}

.body-practice {
  padding: 0;
}

.body-transcript {
  margin-top: 0;
  padding: 10px;
  border: 1px solid #e2e2e2;
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
  font-size: 14px;
  font-weight: 600;
  color: rgb(23, 56, 82);
}

.img-icon {
  height: 17px;
  width: 20px;
  padding-right: 5px;
}

.tip {
  padding: 5px 10px;
  font-size: 12px;
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

pre {
  font-size: 14px;
  text-align: justify;
  white-space: break-spaces;
  font-family: inherit !important;
  margin-bottom: 0 !important;
}

.rotate-icon {
  transform: rotateZ(180deg);
}

.textarea-style {
  width: 100%;
  padding: 15px;
  border: 1px solid #e2e2e2;
  border-top: none;
  resize: none;
  outline: none;
  height: 100%;
}

/* .splitpanes--vertical>.splitpanes__splitter:before {
  display: none;
  left: -30px;
  right: -30px;
  height: 100%;
} */
</style>
