<template>
  <div style="height: 100%;display: flex; flex-direction: column">
    <div id="parent-scroll" style="flex-grow: 1;position: relative;">
      <div id="child-scroll">
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
export default ({
  name: 'TabQuestion',
  components: {

  },
  props: {
    questionid: { type: Number, default: null }
  },
  data() {
    return {
      showDirection: true,
      isShowScript: false,
      userInfo: null,
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
      const header = document.getElementById('header')
      if (header) {
        const headerHeight = header.clientHeight
        const containerHeight = window.innerHeight - headerHeight
        const elContainer = document.getElementById('reviewContainer')
        elContainer.style.height = containerHeight + 'px'
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
