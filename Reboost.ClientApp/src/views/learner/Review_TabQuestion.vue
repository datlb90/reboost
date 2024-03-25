<template>
  <div style="height: 100%;display: flex; flex-direction: column">
    <div id="parent-scroll" style="flex-grow: 1;position: relative;">
      <div id="child-scroll">
        <div v-if="reviewRequest && reviewRequest.feedbackType == 'Free' && showDirection" class="tip" transition="fade" style="margin-bottom: 10px;">
          <p style="width: 98%;">
            <!-- Read the question description and provide feedback for the writing response on the right. A quality review consists of both in-text comments and rubric assesments. Please complete these before submiting your review. -->
            Bạn hãy đọc hiểu yêu cầu của đề bài và tham khảo hướng dẫn ở tab kế bên trước khi cung cấp phản hồi cho bài viết. Một phản hồi chất lượng bao gồm đánh giá cho những tiêu chí chuẩn (rubric assessment) và phản hồi trực tiếp trong bài viết (in-text comments). Hãy hoàn thành những mục này trước khi gửi bài đánh giá của bạn.
          </p>
          <el-button size="mini" @click="showDirection = !showDirection">Ok</el-button>
          <el-button size="mini" @click="notShowDirection">Không hiện nữa</el-button>
        </div>

        <div v-if="reviewRequest && reviewRequest.feedbackType == 'Pro'" class="requirement" transition="fade" style="margin-bottom: 10px;">
          <h5 style="font-size: 18px;">Yêu cầu từ học viên</h5>
          <div style="width: 98%; color: #4a6f8a; font-size: 15px;">
            <b>- Ngôn ngữ phản hồi:</b> {{ reviewRequest.feedbackLanguage == 'vn' ? 'Tiếng Việt' : 'Tiếng Anh' }}
          </div>
          <div style="width: 98%; color: #4a6f8a;  font-size: 15px;">
            <b>- Yêu cầu khác:</b> {{ reviewRequest.specialRequest }}
          </div>
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

        <div v-if="getChart != ''">
          <img :src="'/photo/' + getChart.content" :alt="getChart.content" style="max-height: 100%; max-width: 100%; margin-top: 10px;">
        </div>

      </div>
    </div>
  </div>
</template>
<script>
// import raterService from '@/services/rater.service'
import reviewService from '@/services/review.service.js'
import { RATER_STATUS, UserRole, DISPUTE_STATUS } from '../../app.constant'
export default ({
  name: 'TabQuestion',
  components: {

  },
  props: {
    questionid: { type: Number, default: null },
    reviewid: { type: Number, default: null },
    reviewRequest: { type: Object, default: null }
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
    getQuestionSection() {
      if (this.$store.getters['question/getSelected']) {
        return this.$store.getters['question/getSelected']['test'] + ' ' + this.$store.getters['question/getSelected']['section'] + ' - ' + this.$store.getters['question/getSelected']['title']
      }
      return ''
    },
    getChart() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Chart')) {
          var chart = this.getDataQuestionParts.find(u => u.name == 'Chart')
          // chart.content = '../../assets/' + chart.content.trim()
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
    if (localStorage.getItem('showQuestionDirection')) {
      this.showDirection = false
    }
    // get question that has already been loaded from store
    if (this.getQuestion) {
      this.loadingComplete = true
      this.calculateContainerHeight()
    }
  },
  methods: {
    async getQuestionById(id) {
      return await this.$store.dispatch('question/loadQuestion', this.questionid)
    },
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
.requirement {
  padding: 10px 10px;
  font-size: 12px;
  background-color: #fdf6ec;
  border-radius: 4px;
  border-left: 5px solid #e6a23c;
}
</style>
