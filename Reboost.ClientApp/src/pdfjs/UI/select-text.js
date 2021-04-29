import {
  findSVGAtPoint
} from '@/pdfjs/UI/utils'
import {
  isEnabling
} from './text.js'

export function enableTextSelection(parent) {
  const that = parent
  // Register necessary events
  document.addEventListener('mouseup', handleDocumentMouseup)
  document.addEventListener('mousedown', handleDocumentMousedown)

  function handleDocumentMouseup(e) {
    // that.svg = findSVGAtPoint(e.x, e.y)
    if (e.target.classList.contains('textLayer') || e.target.parentElement.classList.contains('textLayer')) {
      const rects = that.getSelectionRects()
      if (rects) {
        const text = window
          .getSelection()
          .toString()
          .trim()
          // Display text tool bar if text is selected.
        if (text) {
          if (e.target.style.top == '') {
            window.getSelection().removeAllRanges()
            that.hideTextToolBar()
            that.hideTextToolGroup()
            that.hideRectToolBar()
            that.hideColorPickerTool()
          } else {
            if (!isEnabling()) {
              const textTool = document.getElementById('textTool')
              textTool.style.display = 'flex'
              const textToolGroup = document.getElementById('textToolGroup')
              var posX = parseInt(rects[rects.length - 1].left)
              // if (rects.length > 1) {
              //   posX = (parseInt(rects[rects.length - 1].left) + parseInt(rects[0].left)) / 2
              // }
              const posY = parseInt(rects[rects.length - 1].top) + e.target.offsetHeight + 10
              textToolGroup.style = 'position: absolute; top: ' + posY + 'px; left: ' + posX + 'px;'
              that.rects = rects
              that.svg = findSVGAtPoint(rects[0].left, rects[0].top)
              that.target = e.target
              that.selectedText = text
              that.boundingRect = that.svg.getBoundingClientRect()
              // that.updateActiveButton('select-text')
            }
          }
        } else {
          // Display different tool bar
        }
      } else {
        that.hideTextToolBar()
        that.hideTextToolGroup()
        that.$refs.toolBar.handleAnnotationClicked(null)
      }
    }
  }

  function handleDocumentMousedown(e) {
    const svg = findSVGAtPoint(e.clientX, e.clientY)
    that.svg = svg
    if (svg) {
      if (!e.target.classList.contains('colorPicker')) {
        if (e.target.parentElement.classList.contains('textLayer') || e.target.classList.contains('textLayer')) {
          that.hideColorPickerTool()
        }
      }
      // hide text tool buttons
      if (!e.target.classList.contains('textToolBtn') &&
            !e.target.classList.contains('svg-inline--fa') &&
            !e.target.parentElement.classList.contains('svg-inline--fa')) {
        const rectTool = document.getElementById('rectTool')
        rectTool.style.visibility = 'hidden'
        const textToolGroup = document.getElementById('textToolGroup')
        textToolGroup.style.visibility = 'hidden'
        const deleteTool = document.getElementById('deleteTool')
        deleteTool.style.visibility = 'hidden'
      }
      // Hide add new comment form
      const newCommentWrapper = document.getElementById('add-new-comment')
      if (newCommentWrapper && newCommentWrapper.style.display != 'none') {
        const validTarget = that.hasAParentWithClass(e.target, 'add-new-comment')
        const comment = document.getElementById('comment-text-area').value
        if (this.annotationClicked.getAttribute('data-pdf-annotate-type') == 'comment-area') {
          this.annotationClicked.setAttribute('data-pdf-annotate-type', 'area')
        }
        if (!validTarget && comment.replace(/\s/g, '').length == 0) { that.cancelCommentText() }
      }
      // Unselect highlight comment
      if (!e.target.classList.contains('comment-highlight-selected') && !e.target.classList.contains('toolbar-btn')) {
        const validTarget = that.hasAParentWithClass(e.target, 'comment-card-selected')
        if (!validTarget) { that.unselectHighlightComment() }
      }
      return
    }
  }
}
