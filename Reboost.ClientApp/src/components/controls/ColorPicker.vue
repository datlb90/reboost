<template>
  <div class="toolbar-button-color__container">
    <div style="display:flex;" class="toolbar-btn-wrapper">
      <el-tooltip v-for="item in listColor" :key="item.name" class="item " placement="none">
        <button class="toolbar-btn" data-tooltype="rectangle" type="button" @click="changeToolbarButtonColor(item.name)">
          <div :style="{'color': item.name}" :class="[colorChosen == item.name ? 'button-color-chosen':'']">
            <button :style="{'background-color': item.name, height:18+'px',width:18+'px', margin:'5px 5px 5px 5px', 'border-radius':'50%','outline': 'none'}" />
          </div>
        </button>
      </el-tooltip>
    </div>
    <el-popover popper-class="popover-color-picker" placement="bottom" @show="updatePositionColorPicker">
      <!-- <template> -->
      <div id="colorPickerCollapse" class="color-picker-collapse">
        <el-tooltip v-for="item in listColor" :key="item.name" class="item" placement="none">
          <button class="toolbar-btn" data-tooltype="rectangle" type="button" @click="changeToolbarButtonColor(item.name)">
            <div :style="{'color': item.name}" :class="[colorChosen == item.name ? 'button-color-chosen':'']">
              <button :style="{'background-color': item.name, height:18+'px',width:18+'px', margin:'5px 5px 5px 5px', 'border-radius':'50%','outline': 'none'}" />
            </div>
          </button>
        </el-tooltip>
      </div>
      <!-- </template> -->
      <div slot="reference" @click="expandColorPicker = expandColorPicker">
        <i id="colorPickerExpandIcon" style="margin-right:10px" :class="[expandColorPicker ? 'el-icon-caret-bottom' :'el-icon-caret-top']" />
      </div>
    </el-popover>
    <!-- <div @click="expandColorPickerToggle">
      <i id="colorPickerExpandIcon" style="margin-right:10px" :class="[expandColorPicker ? 'el-icon-caret-bottom' :'el-icon-caret-top']" />
    </div>
    <div id="colorPickerCollapse" class="color-picker-collapse">
      <el-tooltip v-for="item in listColor" :key="item.name" class="item" placement="none">
        <button class="toolbar-btn" data-tooltype="rectangle" type="button" @click="changeToolbarButtonColor(item.name)">
          <div :style="{'color': item.name}" :class="[colorChosen == item.name ? 'button-color-chosen':'']">
            <button :style="{'background-color': item.name, height:18+'px',width:18+'px', margin:'5px 5px 5px 5px', 'border-radius':'50%','outline': 'none'}" />
          </div>
        </button>
      </el-tooltip> -->
    <!-- </div> -->
  </div>

</template>
<script>
import PDFJSAnnotate from '@/pdfjs/PDFJSAnnotate'
export default ({
  name: 'ColorPicker',
  props: {
    documentid: { type: Number, default: null },
    initcolor: { type: String, default: '#ff0000' }
  },
  data() {
    return {
      listColor: require('../../assets/data.json').listColor,
      colorChosen: '#ff0000',
      expandColorPicker: false,
      annotation: null
    }
  },
  watch: {
  },
  created() {
    this.colorChosen = this.initcolor
  },
  async mounted() {
  },
  methods: {
    async changeToolbarButtonColor(e) {
      this.colorChosen = e.toLowerCase()
      localStorage.setItem(`${this.documentid}/color`, e.toLowerCase())
      if (this.annotation) {
        this.updateRectangleAnotation(e)
      }

      this.$emit('colorChange', this.colorChosen)
      // this.expandColorPickerToggle()
    },
    changeColorWhenClickToolBarButton(e) {
      if (typeof (e) != 'undefined') {
        this.colorChosen = e.toLowerCase()
        localStorage.setItem(`${this.documentid}/color`, e.toLowerCase())
      }
    },
    // expandColorPickerToggle(e) {
    //   if (typeof (e) == 'boolean') {
    //     this.expandColorPicker = e
    //   } else {
    //     this.expandColorPicker = !this.expandColorPicker
    //   }
    //   if (this.expandColorPicker) {
    //     const target = document.getElementsByClassName('toolbar-button-color__container')[0].getBoundingClientRect()
    //     var posX = 0
    //     posX = target.x + 6
    //     const posY = target.y
    //     document.getElementById('colorPickerCollapse').style = 'width: 160px;overflow: hidden; display: grid;grid-template-columns: repeat(4,40px);z-index: 2; position:fixed;background:#ffffff;' +
    //     'top: ' + posY + 'px;left: ' + posX + 'px;'
    //     // this.$emit('expandColorPickerToggle')
    //   } else {
    //     document.getElementById('colorPickerCollapse').style = 'display = none'
    //   }
    // },
    changeColorByClickedAnno(e, annotation) {
      if (typeof (e) != 'undefined') {
        this.colorChosen = e.toLowerCase()
      }
      if (typeof (annotation) != 'undefined' && annotation) {
        this.annotation = annotation
        localStorage.setItem(`${this.documentid}/color`, e.toLowerCase())
      } else {
        this.annotation = null
      }
    },
    async updateRectangleAnotation(e) {
      const previousAnno = Object.assign({}, this.annotation)
      var type = this.annotation.type
      if (type == 'comment-highlight') {
        return
      }
      const annotationClicked = document.querySelector(`[data-pdf-annotate-id="${this.annotation.uuid}"]`)
      if (type != 'highlight') {
        annotationClicked.setAttribute('stroke', e)
      }
      if (type == 'textbox') {
        annotationClicked.childNodes[0].style.color = e
      } else if (type == 'highlight') {
        if (annotationClicked.hasChildNodes()) {
          const child = annotationClicked.childNodes
          for (let i = 0; i < child.length; i++) {
            annotationClicked.childNodes[i].setAttribute('fill', e)
          }
        }
      }
      this.annotation.color = e
      await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentid, this.annotation.uuid, this.annotation, undefined, previousAnno)
    },
    updatePositionColorPicker() {
      setTimeout(function() {
        const popoverColorPicker = document.getElementsByClassName('popover-color-picker')[0]
        if (popoverColorPicker) {
          const initialLeft = parseInt(popoverColorPicker.style.left.substring(0, popoverColorPicker.style.left.length - 2))
          const initialTop = parseInt(popoverColorPicker.style.top.substring(0, popoverColorPicker.style.top.length - 2))
          const groupOffsetWidth = parseInt(document.getElementsByClassName('toolbar-button-color__container')[0].offsetWidth)

          popoverColorPicker.style.left = `${initialLeft - groupOffsetWidth / 2}px`
          popoverColorPicker.style.top = `${initialTop - 5}px`
        }
      }, 10)
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
