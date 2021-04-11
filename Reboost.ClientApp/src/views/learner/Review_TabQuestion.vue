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

        <div class="content-con">
          <p>
            <b><a href="#">{{ getQuestionSection }}</a></b>
          </p>
          <p> {{ getQuestion.content }}</p></div>
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
      isShowScript: false
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
    }
  },
  async mounted() {
    console.log('this.getDataQuestionParts', this.getDataQuestionParts)
    if (localStorage.getItem('showQuestionDirection')) {
      this.showDirection = false
    }
    this.$store.dispatch('question/loadQuestion', this.questionid).then(rs => {
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
    }
  }
})
</script>

<style scoped>
@import '../../styles/review.css';
</style>
