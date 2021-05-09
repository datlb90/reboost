<template>
  <div id="tool-bar" class="toolbar">
    <div id="chevron-scroll" :style="{ 'visibility': showChevronScroll ? 'visible' : 'hidden' }">
      <div id="chevron-scroll__right" class="right">
        <button class="toolbar-btn" @click="chevronClick('right')">
          <div class="icon">
            <i class="toolbar-icon el-icon-arrow-right" />
          </div>
        </button>
      </div>
      <div id="chevron-scroll__left" class="left">
        <button class="toolbar-btn" @click="chevronClick('left')">
          <div class="icon">
            <i class="toolbar-icon el-icon-arrow-left" />
          </div>
        </button>
      </div>
    </div>

    <div id="tool-bar__buttons__Con" class="tool-group-buttons-container">

      <!-- Colapse left panel -->
      <div class="toolbar-btn-wrapper">
        <el-tooltip class="item" effect="dark" content="Hide Question & Rubric (H)" placement="bottom">
          <button class="toolbar-btn" @click="hideQuestion">
            <div :class="{'hideQuestion':!showQuestion}" class="icon">
              <i class="toolbar-icon fas fa-angle-double-left" />
            </div>
          </button>
        </el-tooltip>
      </div>

      <!-- Note -->
      <div v-if="!disableAnnotation" class="toolbar-btn-wrapper">
        <el-tooltip class="item" effect="dark" content="Note (N)" placement="bottom">
          <button class="toolbar-btn" :class="[activeButton=='point'?'active':'']" data-tooltype="point" type="button" @click="toolBarButtonClick('point')">
            <div class="icon">
              <i class="toolbar-icon far fa-sticky-note" />
            </div>
          </button>
        </el-tooltip>
      </div>

      <!-- Free text -->
      <div v-if="!disableAnnotation" class="toolbar-btn-wrapper">
        <el-tooltip class="item" effect="dark" content="Free Text (T)" placement="bottom">
          <button class="toolbar-btn" :class="[activeButton=='text'?'active':'']" data-tooltype="text" type="button" @click="toolBarButtonClick('text')">
            <div class="icon">
              <i class="toolbar-icon fas fa-pen-alt" />
            </div>
          </button>
        </el-tooltip>
      </div>

      <!-- Rectangle -->
      <div v-if="!disableAnnotation" class="toolbar-btn-wrapper">
        <el-tooltip class="item" effect="dark" content="Rectangle (R)" placement="bottom">
          <button class="toolbar-btn" :class="[activeButton=='rectangle'?'active':'']" data-tooltype="rectangle" type="button" @click="toolBarButtonClick('rectangle')">
            <div class="icon">
              <i class="far fa-square" />
            </div>
          </button>
        </el-tooltip>
      </div>

      <!-- Color picker control -->
      <div :style="{display:(activeButton || clickedAnnotation)&&!disableAnnotation?'block':'none'}">
        <colorPicker ref="colorPickerCom" :initcolor="colorChosen" :documentid="documentid" @colorChange="colorPickerChange($event)" />
      </div>
      <!-- :expandcolorpicker.sync="expandColorPicker" -->
      <!-- Text selection tools -->
      <div v-if="!disableAnnotation" id="textTool" data-element="textToolGroupButton" class="tool-group-button text-tool-group-button">
        <div class="devider" />
        <div class="toolbar-btn-wrapper">
          <el-tooltip class="item" effect="dark" content="Highlight" placement="bottom">
            <button class="toolbar-btn" @click="HighlightText()">
              <div class="icon">
                <i class="fas fa-highlighter" />
              </div>
            </button>
          </el-tooltip>
        </div>

        <div class="toolbar-btn-wrapper">
          <el-tooltip class="item" effect="dark" content="Strikethrough" placement="bottom">
            <button class="toolbar-btn" @click="StrikethroughText()">
              <div class="icon">
                <i class="fas fa-strikethrough" />
              </div>
            </button>
          </el-tooltip>
        </div>

        <div class="toolbar-btn-wrapper">
          <el-tooltip class="item" effect="dark" content="Comment" placement="bottom">
            <button class="toolbar-btn" @click="CommentText()">
              <div class="icon">
                <i class="fas fa-comment-alt" />
              </div>
            </button>
          </el-tooltip>
        </div>
        <div class="devider" />
      </div>

      <!-- Text size -->
      <select v-if="!disableAnnotation" :style="{ 'display': activeButton == 'text' ? 'block' : 'none' }" class="text-size" />

      <!-- No presets -->
      <!-- <div v-if="(activeButton || clickedAnnotation) && !disableAnnotation" class="preset-none">
        <div style="color: gray;">No Presets</div>
      </div> -->

      <!-- Zoom controls -->
      <el-dropdown size="mini" split-button style="margin-left: 10px;min-width:95px" @command="handleScale">
        {{ scaleText }}
        <el-dropdown-menu slot="dropdown">
          <el-dropdown-item command="fitWidth" icon="fas fa-expand-arrows-alt"> Fit to width</el-dropdown-item>
          <el-dropdown-item command="fitPage" icon="fas fa-expand-alt"> Fit to page</el-dropdown-item>
          <el-dropdown-item command="0.5" divided> 50% </el-dropdown-item>
          <el-dropdown-item command="1">100%</el-dropdown-item>
          <el-dropdown-item command="1.25">125%</el-dropdown-item>
          <el-dropdown-item command="1.5">150%</el-dropdown-item>
          <el-dropdown-item command="2">200%</el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>

      <div data-element="highlightToolGroupButton" class="tool-group-button">
        <button class="toolbar-btn" style="margin-left: 5px;" @click="increaseScale">
          <div class="icon">
            <i class="toolbar-icon fas fa-plus" />
          </div>
        </button>

        <button class="toolbar-btn" @click="decreaseScale">
          <div class="icon">
            <i class="toolbar-icon fas fa-minus" />
          </div>
        </button>
      </div>

      <div v-if="!disableAnnotation" class="devider" />

      <button v-if="!disableAnnotation" :disabled="undoHistory.length == 0" class="toolbar-btn" @click="undoAnnotation()">
        <div class="icon">
          <i class="toolbar-icon fas fa-undo" />
        </div>
      </button>
      <button v-if="!disableAnnotation" :disabled="redoHistory.length == 0" class="toolbar-btn" @click="redo()">
        <div class="icon">
          <i class="toolbar-icon fas fa-redo" />
        </div>
      </button>
    </div>
    <!-- Submit button -->
    <div id="submit-container" class="submit-button">
      <div v-if="statusText!=''" class="submit-button__text" style="">{{ statusText }}</div>
      <el-button :disabled="readOnly" type="primary" size="mini" @click="submitReview()">Submit</el-button>
    </div>
  </div>
</template>

<script>
import ColorPicker from './ColorPicker'
import UI from '@/pdfjs/UI'
import ReviewVue from '@/views/learner/Review.vue'
export default ({
  name: 'ToolBar',
  components: {
    'colorPicker': ColorPicker
  },
  props: { documentid: { type: Number, default: null },
    reviewid: { type: Number, default: null },
    expandcolorpicker: { type: Boolean, default: false },
    renderoptions: { type: Object, default: null },
    reviewPage: ReviewVue
  },
  data() {
    return {
      colorChosen: '#ff0000',
      readOnly: false,
      showQuestion: true,
      scaleText: '100%',
      statusText: '',
      expandColorPicker: false,
      activeButton: null,
      undoHistory: [],
      redoHistory: [],
      scaleRatio: 1,
      defaultScale: 1,
      zoomStep: 0.2,
      minScale: 0.5,
      maxScale: 2,
      disableAnnotation: false,
      showChevronScroll: false,
      clickedAnnotation: null
    }
  },
  computed: {

  },
  watch: {
  },
  async mounted() {
    this.initTextSizeTool()
    localStorage.removeItem(`${this.documentid}/tooltype`)
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
    async hideQuestion() {
      this.showQuestion = !this.showQuestion
      this.$emit('hideQuestion', this.showQuestion)
      if (!this.showQuestion) {
        document.getElementById('right-panel').style.width = 100 + '%'
      } else {
        document.getElementById('right-panel').style.width = 69.2 + '%'
      }
      await this.handleScale(this.scaleRatio)
      if (this.activeButton != null && this.activeButton != '') {
        this.expandColor(false)
      }
      this.insertExpandMenu()
    },
    async toolBarButtonClick(e) {
      localStorage.setItem(`${this.documentid}/tooltype`, e)

      const tooltype = localStorage.getItem(`${this.documentid}/tooltype`) || 'cursor'

      // var activateButton = null

      if (tooltype) {
        const clickAgain = tooltype == this.activeButton

        UI.disableText()
        UI.disablePoint()
        UI.disableRect()
        this.activeButton = ''

        if (!clickAgain) {
          this.activeButton = tooltype
          switch (this.activeButton) {
            case 'text':
              UI.enableText()
              break
            case 'point':
              UI.enablePoint()
              break
            case 'area':
            case 'highlight':
            case 'rectangle':
              UI.enableRect('area')
              break
            case 'strikeout':
              UI.enableRect()
              break
          }
          this.colorChosen = localStorage.getItem(`${this.documentid}/color`)
        }
      }
      this.insertExpandMenu()
      this.$emit('toolBarButtonChange', this.activeButton)
    },
    async disableToolBarButton(e) {
      console.log('disableToolBarButton', e)
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
    async handleScale(e) {
      this.renderoptions.scale = this.defaultScale
      if (e != 'fitWidth' && e != 'fitPage') {
        this.renderoptions.scale = (+e)
        this.scaleRatio = this.renderoptions.scale
      } else {
        var docWidth = document.getElementById('viewerContainer').offsetWidth - 45
        var docHeight = document.getElementById('viewerContainer').offsetHeight - 30

        if (e == 'fitWidth') {
          this.renderoptions.scale = (docWidth / 612)
          this.scaleRatio = 'fitWidth'
        } else {
          if ((docWidth / 612) < (docHeight / 792)) {
            this.renderoptions.scale = (docWidth / 612)
          } else {
            this.renderoptions.scale = (docHeight / 792)
          }
          this.scaleRatio = 'fitPage'
        }
      }

      this.scaleText = parseInt(this.renderoptions.scale * 100) + '%'
      this.$emit('scaleChange', this.renderoptions.scale)
    },
    increaseScale() {
      if (typeof (this.scaleRatio) != 'number') { this.scaleRatio = this.renderoptions.scale }
      if (this.renderoptions.scale < 2) {
        this.scaleRatio += this.zoomStep
        if (this.scaleRatio > this.maxScale) { this.scaleRatio = this.maxScale }
        this.scaleText = parseInt(this.scaleRatio * 100) + '%'
        if (this.zoomDelayHandle) {
          clearTimeout(this.zoomDelayHandle)
        }
        this.zoomDelayHandle = setTimeout(() => {
          // this.renderoptions.scale = this.defaultScale
          this.renderoptions.scale = this.scaleRatio
          this.$emit('scaleChange', this.renderoptions.scale)
        }, 200)
      }
    },
    decreaseScale() {
      if (typeof (this.scaleRatio) != 'number') { this.scaleRatio = this.renderoptions.scale }
      if (this.renderoptions.scale > 0.5) {
        this.scaleRatio -= this.zoomStep
        if (this.scaleRatio < this.minScale) { this.scaleRatio = this.minScale }
        this.scaleText = parseInt(this.scaleRatio * 100) + '%'
        if (this.zoomDelayHandle) {
          clearTimeout(this.zoomDelayHandle)
        }
        this.zoomDelayHandle = setTimeout(() => {
          // this.renderoptions.scale = this.defaultScale
          this.renderoptions.scale = this.scaleRatio
          this.$emit('scaleChange', this.renderoptions.scale)
        }, 200)
      }
    },
    undoAnnotation() {
      this.$emit('undoAnnotation')
    },
    redo() {
      this.$emit('redoAnnotation', '123')
    },
    submitReview() {
      this.$emit('submit')
    },
    // expandColor(e) {
    //   this.expandColorPicker = e
    //   if (!e) {
    //     this.$refs.colorPickerCom.expandColorPickerToggle(e)
    //   }
    // },
    setStatusText(text = 'Saved') {
      this.statusText = text
      setTimeout(() => { this.statusText = '' }, 2000)
    },
    updateRedoList(e) {
      this.redoHistory = e
    },
    updateToolbarUndoList(e) {
      this.undoHistory = e
    },
    updateToolbarActiveButton(e) {
      this.activeButton = e
    },
    async resizeEventHandle() {
      document.getElementById('right-panel').offsetWidth

      if (this.scaleRatio != 'fitWidth' && this.scaleRatio != 'fitPage') {
        this.renderoptions.scale = this.defaultScale
        this.renderoptions.scale *= this.scaleRatio
        if (this.zoomDelayHandle) {
          clearTimeout(this.zoomDelayHandle)
        }
        this.zoomDelayHandle = setTimeout(() => {
          this.$emit('scaleChange', this.renderoptions.scale)
        }, 200)
      } else {
        (this.scaleRatio == 'fitWidth') ? this.handleScale('fitWidth') : this.handleScale('fitPage')
      }

      // if (this.activeButton != null && this.activeButton != '') {
      //   this.expandColor(false)
      // }
      this.insertExpandMenu()
    },
    insertExpandMenu() {
      const toolBarButtons = document.getElementById('tool-bar__buttons__Con')
      toolBarButtons.style.width = document.getElementById('tool-bar').offsetWidth - document.getElementById('submit-container').offsetWidth - 20 + 'px'
      if (toolBarButtons.offsetWidth < this.getInnerChildTotalLength(toolBarButtons)) {
        this.showChevronScroll = true
        toolBarButtons.style.left = document.getElementById('chevron-scroll__left').offsetWidth + 'px'
        toolBarButtons.style.width = document.getElementById('tool-bar').offsetWidth - document.getElementById('chevron-scroll__right').offsetWidth - document.getElementById('submit-container').offsetWidth - 40 + 'px'
        document.getElementById('chevron-scroll__right').style.right = document.getElementById('submit-container').offsetWidth + 10 + 'px'
      } else {
        this.showChevronScroll = false
        toolBarButtons.style.left = 0 + 'px'
      }
    },
    getInnerChildTotalLength(parent) {
      var totalWidth = 0
      parent.childNodes.forEach(r => {
        if (typeof (r.offsetWidth) != 'undefined') {
          const style = getComputedStyle(r)
          totalWidth += r.offsetWidth + parseInt(style.marginLeft) + parseInt(style.marginRight)
        }
      })
      return totalWidth
    },
    chevronClick(e) {
      const toolBarButtons = document.getElementById('tool-bar__buttons__Con')
      if (e === 'left') {
        toolBarButtons.scrollLeft -= 50
      } else if (e === 'right') {
        toolBarButtons.scrollLeft += 50
      }
    },
    async initTextSizeTool() {
      const self = this.reviewPage
      let textSize
      let textColor = 'red'

      function initText() {
        const size = document.querySelector('.toolbar .text-size');
        [8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96].forEach(s => {
          size.appendChild(new Option(s, s))
        })

        setText(
          // localStorage.getItem(`${self.RENDER_OPTIONS.documentId}/text/size`) ||
          12,
          localStorage.getItem(
            `${self.RENDER_OPTIONS.documentId}/text/color`
          ) || '#000000'
        )

        // initColorPicker(
        //   document.querySelector('.text-color'),
        //   textColor,
        //   function(value) {
        //     setText(textSize, value)
        //   }
        // )
      }

      function setText(size, color) {
        let modified = false

        if (textSize !== size) {
          modified = true
          textSize = size
          localStorage.setItem(
            `${self.RENDER_OPTIONS.documentId}/text/size`,
            textSize
          )
          document.querySelector('.toolbar .text-size').value = textSize
        }

        if (textColor !== color) {
          modified = true
          textColor = color
          localStorage.setItem(
            `${self.RENDER_OPTIONS.documentId}/text/color`,
            textColor
          )

          let selected = document.querySelector(
            '.toolbar .text-color.color-selected'
          )
          if (selected) {
            selected.classList.remove('color-selected')
            selected.removeAttribute('aria-selected')
          }

          selected = document.querySelector(
            `.toolbar .text-color[data-color="${color}"]`
          )
          if (selected) {
            selected.classList.add('color-selected')
            selected.setAttribute('aria-selected', true)
          }
        }

        if (modified) {
          UI.setText(textSize, textColor)
        }
      }

      function handleTextSizeChange(e) {
        setText(e.target.value, textColor)
      }

      document
        .querySelector('.toolbar .text-size')
        .addEventListener('change', handleTextSizeChange)

      initText()
    },
    disableSubmit() {
      this.readOnly = true
    },
    disableAnnotationCreate() {
      this.disableAnnotation = true
    },
    disableButtons() {
      this.activeButton = 'cursor'
      UI.disableText()
      UI.disablePoint()
      UI.disableRect()
      this.toolBarButtonClick('cursor')
    },
    handleAnnotationClicked(e) {
      if (e) {
        if (e.color) { this.colorChosen = e.color.includes('#') ? e.color : '#' + e.color }
        console.log('gan r ne', e)
        this.$refs.colorPickerCom.changeColorByClickedAnno(this.colorChosen, e)
        this.clickedAnnotation = e
      } else {
        this.clickedAnnotation = e
        this.$refs.colorPickerCom.changeColorByClickedAnno(this.colorChosen, null)
      }
    },
    colorPickerChange(e) {
      this.colorChosen = e
    },
    showTextTool() {
      const textTool = document.getElementById('textTool')
      textTool.style.display = 'flex'
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
