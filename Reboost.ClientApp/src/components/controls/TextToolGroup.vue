<template>
  <div>
    <el-button-group id="textToolGroup" style="display: none;">
      <el-button class="textToolBtn" @click="HighlightText()">
        <i class="fas fa-highlighter" />
      </el-button>
      <el-button class="textToolBtn" @click="StrikethroughText()">
        <i class="fas fa-strikethrough" />
      </el-button>
      <el-button class="textToolBtn" @click="CommentText()">
        <i class="fas fa-comment-alt" />
      </el-button>
    </el-button-group>
  </div>
</template>
<script>

export default ({
  name: 'TextTool',
  props: {},
  data() {
    return {}
  },
  computed: {

  },
  watch: {
  },
  async mounted() {
  },
  beforeCreate: function() {
  },
  created() {
    window.addEventListener('resize', this.resizeEventHandle)
  },
  destroyed() {
    window.removeEventListener('resize', this.resizeEventHandle)
  },
  methods: {
    HideTextToolGroup() {
      const textTool = document.getElementById('textToolGroup')
      textTool.style.visibility = 'hidden'
    },
    HighlightText() {
      this.$emit('highLightText', 'highlight')
    },
    StrikethroughText() {
      this.$emit('highLightText', 'strikeout')
    },
    CommentText() {
      this.$emit('highLightText', 'comment-highlight')
    },
    showTextToolGroup(rects, e) {
      const textToolGroup = document.getElementById('textToolGroup')
      let posX, posY
      if (rects.length == 1) {
        console.log('e.target', e.target, textToolGroup.offsetWidth)
        // 167 is the width of textToolGroup
        posX = parseInt(rects[0].left) + parseInt(rects[0].width / 2) - 167 / 2
        posY = parseInt(rects[0].top) + e.target.offsetHeight + 10
      } else {
        console.log('rects', rects)
        const listRect = Array.prototype.slice.call(rects)

        const max = Math.max(...listRect.map(a => parseInt(a.width)))
        console.log(max)
        posX = parseInt(rects[rects.length - 1].left) + max / 2 - 167 / 2
        posY = parseInt(rects[rects.length - 1].top) + e.target.offsetHeight + 10
      }

      textToolGroup.style = 'position: absolute; top: ' + posY + 'px; left: ' + posX + 'px;'
    }
  }
})
</script>
<style scoped>
@import '../../pdfjs/shared/document.css';
@import '../../pdfjs/shared/toolbar.css';
@import '../../pdfjs/shared/pdf_viewer.css';
@import '../../styles/review.css';
</style>
