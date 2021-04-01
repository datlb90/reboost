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
    <div @click="expandColorPickerToggle">
      <i id="colorPickerExpandIcon" style="margin-right:10px" :class="[expandColorPicker ? 'el-icon-caret-bottom' :'el-icon-caret-top']" />
    </div>
    <div id="colorPickerCollapse" class="color-picker-collapse">
      <el-tooltip v-for="item in listColor" :key="item.name" class="item" placement="none">
        <button class="toolbar-btn" data-tooltype="rectangle" type="button" @click="changeToolbarButtonColor(item.name)">
          <div :style="{'color': item.name}" :class="[colorChosen == item.name ? 'button-color-chosen':'']">
            <button :style="{'background-color': item.name, height:18+'px',width:18+'px', margin:'5px 5px 5px 5px', 'border-radius':'50%','outline': 'none'}" />
          </div>
        </button>
      </el-tooltip>
    </div>
  </div>

</template>
<script>
export default ({
  name: 'ColorPicker',
  props: {
  },
  data() {
    return {
      listColor: require('../../assets/data.json').listColor,
      colorChosen: null,
      expandColorPicker: false
    }
  },
  watch: {
  },
  methods: {
    changeToolbarButtonColor(e) {
      this.colorChosen = e
      localStorage.setItem('colorChosen', e)

      this.expandColorPicker = true
      this.expandColorPickerToggle()
    },
    expandColorPickerToggle(e) {
      if (typeof (e) == 'boolean') {
        this.expandColorPicker = e
      } else {
        this.expandColorPicker = !this.expandColorPicker
      }
      if (this.expandColorPicker) {
        const target = document.getElementsByClassName('toolbar-button-color__container')[0].getBoundingClientRect()
        var posX = 0
        posX = target.x
        const posY = target.y
        document.getElementById('colorPickerCollapse').style = 'width: 170px;overflow: hidden; display: grid;grid-template-columns: repeat(4,40px);z-index: 2; position:fixed;background:#ffffff;' +
        'top: ' + posY + 'px;left: ' + posX + 'px;'
        // this.$emit('expandColorPickerToggle')
      } else {
        document.getElementById('colorPickerCollapse').style = 'display = none'
      }
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
