<template>
  <div style="height: 100%;display: flex; flex-direction: column">
    <div id="parent-scroll" style="flex-grow: 1;position: relative;">
      <div id="child-scroll" class="par-content default">
        <div v-if="showDirection" class="tip" transition="fade" style="margin-bottom: 10px;">
          <p style="width: 98%;">
            <b>Direction:</b> Read the question description and provide feedback for the writing response on the right. A quality review consists of both in-text comments and rubric assesments. Please complete these before submiting your review.
          </p>
          <el-button size="mini" @click="showDirection = !showDirection">Got it</el-button>
          <el-button size="mini" @click="notShowDirection">Never show this again</el-button>
        </div>
        <div v-if="dispute && currentUser.role === UserRole.ADMIN" class="content-con" style="margin-bottom: 10px;">
          <p style="width: 98%;">
            Dispute
            <el-input
              v-model="dispute.reasons"
              :disabled="true"
            />
          </p>

          <div v-if="dispute.status === DISPUTE_STATUS.OPEN">
            <el-button size="mini" type="primary" @click="showNoteDialog(DISPUTE_STATUS.ACCEPTED)">Accepted</el-button>
            <el-button size="mini" type="danger" @click="showNoteDialog(DISPUTE_STATUS.DENIED)">Denied</el-button>
            <el-button size="mini" type="success" @click="showNoteDialog(DISPUTE_STATUS.REFUNDED)">Refunded</el-button>
          </div>
          <div v-if="dispute.status !== DISPUTE_STATUS.OPEN">
            <el-tag
              :type="dispute.status === DISPUTE_STATUS.ACCEPTED ? 'success'
                : dispute.status === DISPUTE_STATUS.DENIED ? 'danger' :'info'"
            >
              {{ dispute.status }}
            </el-tag>
          </div>
        </div>

        <div v-if="userInfo && trainingNote" class="tip note" transition="fade" style="margin-bottom: 10px;">
          <p style="width: 98%;">
            Note: {{ trainingNote }}
          </p>
        </div>

        <div v-if="loadingComplete" id="questionContent" class="content-con">
          <div style="padding-bottom: 10px; border-bottom: 1px solid #dcddde; margin-bottom: 10px;">
            <b><a href="#">{{ getQuestionSection }}</a></b>
          </div>
          <div v-html="getQuestion.content" />
        </div>
        <div v-if="getReading != ''" class="content-con">
          <p>
            <b>Reading</b>
          </p>
          <div style="margin: 0;">
            <pre> {{ getReading.content }}</pre>
          </div>
        </div>
        <div v-if="getListening != ''" class="content-con">
          <p>
            <b>Listening</b>
          </p>
          <audio controls style="width: 100%; height: 35px; margin-bottom: 3px;">
            <!-- <source :src="'/assets/' + getListening.content" type="audio/mpeg"> -->
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
          <div v-if="isShowScript" class="body-transcript" style="margin: 0;">
            <pre> {{ getTranscript.content }}</pre>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import raterService from '@/services/rater.service'
import reviewService from '@/services/review.service.js'
import { RATER_STATUS, RATER_TRAINING_STATUS, UserRole, DISPUTE_STATUS } from '../../app.constant'
export default ({
  name: 'TabQuestion',
  components: {

  },
  props: {
    questionid: { type: Number, default: null },
    reviewid: { type: Number, default: null }
  },
  data() {
    return {
      showDirection: true,
      isShowScript: false,
      userInfo: null,
      RATER_STATUS: RATER_STATUS,
      trainingNote: null,
      dispute: null,
      UserRole: UserRole,
      DISPUTE_STATUS: DISPUTE_STATUS,
      updateStatus: null,
      loadingComplete: false
    }
  },
  computed: {
    getDataQuestionParts() {
      return this.$store.getters['question/getSelected']['questionsPart']
    },
    getQuestionSection() {
      if (this.$store.getters['question/getSelected']) {
        return this.$store.getters['question/getSelected']['test'] + ' ' + this.$store.getters['question/getSelected']['section'] + ' - ' + this.$store.getters['question/getSelected']['title']
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
    },
    getQuestion() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Question')) {
          return this.getDataQuestionParts.find(u => u.name == 'Question')
        }
      }
      return ''
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
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  async mounted() {
    await raterService.getByCurrentUser().then(rs => {
      this.userInfo = rs
      if (rs.status === RATER_STATUS.REVISION_REQUESTED) {
        // Load rater's training
        reviewService.getRaterTrainings(rs.id).then(r => {
          var currentTraining = r.filter(r => r.reviewId === this.reviewid)[0]
          if (currentTraining.status == RATER_TRAINING_STATUS.REVISION_REQUEST) {
            this.trainingNote = currentTraining.note
          }
        })
      }
    })

    if (localStorage.getItem('showQuestionDirection')) {
      this.showDirection = false
    }
    this.$store.dispatch('question/loadQuestion', this.questionid).then(rs => {
      this.loadingComplete = true
      this.calculateContainerHeight()
    })
  },
  methods: {
    notShowDirection() {
      localStorage.setItem('showQuestionDirection', true)
      this.showDirection = false
    },
    toggleBtnShowScript() {
      this.isShowScript = !this.isShowScript
    },
    calculateContainerHeight() {
      const headerHeight = document.getElementById('header').clientHeight
      const containerHeight = window.innerHeight - headerHeight
      const elContainer = document.getElementById('reviewContainer')
      elContainer.style.height = containerHeight + 'px'
    },
    calculateStylePaddingScroll() {
      const parentHeight = document.getElementById('parent-scroll').offsetHeight
      const childHeight = document.getElementById('child-scroll').scrollHeight
      if (parentHeight >= childHeight) {
        // document.getElementById('child-scroll').style.paddingRight = '0'
      } else {
        // document.getElementById('child-scroll').style.paddingRight = '10px'
      }
    },
    getDisputeData(data) {
      this.dispute = data
    },
    showNoteDialog(e) {
      this.updateStatus = e
      this.$emit('openDisputeNote')
    },
    updateDispute(data) {
      if (this.dispute) {
        const postData = {
          Id: this.dispute.id,
          Status: this.updateStatus,
          AdminNote: data.note
        }

        reviewService.updateDispute(postData).then(rs => {
          if (rs) {
            this.dispute = rs
            if (rs.status === DISPUTE_STATUS.ACCEPTED) {
              this.$notify.success({
                title: 'Dispute accepted.',
                message: 'Dispute accepted.',
                type: 'success',
                duration: 2000
              })
            } else if (rs.status === DISPUTE_STATUS.DENIED) {
              this.$notify.error({
                title: 'Dispute denied.',
                message: 'Dispute denied',
                type: 'error',
                duration: 2000
              })
            } else {
              this.$notify.success({
                title: 'Dispute refunded.',
                message: 'Dispute refunded.',
                type: 'success',
                duration: 2000
              })
            }
          }
        })

        this.$emit('closeDisputeNote')
      }
    }
  }
})
</script>

<style scoped>
@import '../../styles/review.css';
</style>
