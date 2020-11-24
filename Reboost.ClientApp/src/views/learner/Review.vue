<template>
  <div>
    <!-- <div class="toolbar">
      <button class="cursor" type="button" title="Cursor" data-tooltype="cursor">
        ➚
      </button>

      <div class="spacer" />

      <button class="rectangle" type="button" title="Rectangle" data-tooltype="area">
&nbsp;
      </button>
      <button class="highlight" type="button" title="Highlight" data-tooltype="highlight">
&nbsp;
      </button>
      <button class="strikeout" type="button" title="Strikeout" data-tooltype="strikeout">
&nbsp;
      </button>

      <div class="spacer" />

      <button class="text" type="button" title="Text Tool" data-tooltype="text" />
      <select class="text-size" />
      <div class="text-color" />

      <div class="spacer" />

      <div class="spacer" />

      <button class="comment" type="button" title="Comment" data-tooltype="point">
        🗨
      </button>

      <div class="spacer" />

      <select class="scale">
        <option value=".5">
          50%
        </option>
        <option value="1">
          100%
        </option>
        <option value="1.33">
          133%
        </option>
        <option value="1.5">
          150%
        </option>
        <option value="2">
          200%
        </option>
      </select>

      <a href="javascript://" class="rotate-ccw" title="Rotate Counter Clockwise">⟲</a>
      <a href="javascript://" class="rotate-cw" title="Rotate Clockwise">⟳</a>

      <div class="spacer" />

      <a href="javascript://" class="clear" title="Clear">×</a>
      <button id="restore" title="Restore">
        Restore
      </button>
    </div> -->

    <!-- <div id="rubric-wrapper">
      <h4>Rubrics</h4>
    </div> -->

    <div id="content-wrapper">
      <div
        id="viewer"
        class="pdfViewer"
        :document-id="documentId"
      />
    </div>

    <div id="comment-wrapper">
      <el-card id="add-new-comment" class="box-card add-new-comment" style="width: 100%; display: none;">
        <div slot="header" class="clearfix">
          <div>
            <div style="font-size: 15px; font-weight: bold; text-align: left;">
              Dat Le
            </div>
          </div>
        </div>
        <div>
          <el-input
            id="comment-text-area"
            v-model="newComment"
            type="textarea"
            autosize
            placeholder="Add comment"
          />
          <div style="height: 20px; margin-top: 10px;">
            <el-button type="primary" :disabled="newComment.replace(/\s/g, '').length == 0" style="float: left; padding: 5px;" @click="addCommentText()">
              Comment
            </el-button>
            <el-button style="float: right; padding: 5px;" @click="cancelCommentText()">
              Cancel
            </el-button>
          </div>
        </div>
      </el-card>

      <el-card
        v-for="(comment, idx) in comments"
        :key="comment.uuid"
        class="box-card comment-card"
        :highlight-id="comment.uuid"
        :top-position="comment.topPosition"
        :style="{top: comment.topPosition + 'px', width: '100%'}"
      >
        <div slot="header" class="clearfix">
          <div>
            <div style="font-size: 15px; font-weight: bold; text-align: left;">
              Dat Le
            </div>
            <div style="font-size: 12px; text-align: left;">
              3:16 PM Jul 7
            </div>
          </div>
          <el-button style="right: -10px;" class="action-card-btn"	title="Delete Comment"	@click="deleteCommentCard(comment)">
            <i class="far fa-trash-alt" />
          </el-button>
          <el-button style="right: 10px;" class="action-card-btn"	title="Edit Comment"	@click="displayEditComment(comment, idx)">
            <i class="far fa-edit" />
          </el-button>
        </div>
        <div>
          <div
            v-show="!comment.isSelected"
            :id="'comment-text-' + comment.uuid"
            style="text-align: left; overflow-wrap: break-word; white-space: pre-wrap;"
          >
            {{ comment.content }}
          </div>
          <el-input
            v-show="comment.isSelected == true"
            :id="'comment-input-' + comment.uuid"
            :ref="'comment' + comment.uuid"
            v-model="comment.content"
            type="textarea"
            autosize
          />
          <div v-show="comment.isSelected == true" style="height: 20px; margin-top: 10px;">
            <el-button type="primary" :disabled="comment.content.replace(/\s/g, '').length == 0" style="float: left; padding: 5px;" @click="editCommentCard()">
              Comment
            </el-button>
            <el-button style="float: right; padding: 5px;" @click="cancelCommentText()">
              Cancel
            </el-button>
          </div>
        </div>
      </el-card>
    </div>

    <el-button-group id="textTool" style="display: none;">
      <el-button class="textToolBtn" @click="HighlightText()">
        <i class="fas fa-highlighter" />
      </el-button>
      <el-button class="textToolBtn" @click="StrikethroughText()">
        <i class="fas fa-strikethrough" />
      </el-button>
      <el-button id="comment-text" class="textToolBtn" @click="CommentText()">
        <i class="fas fa-comment-alt" />
      </el-button>
    </el-button-group>
  </div>
</template>

<script>
// @ is an alias to /src

// import { PDFJS } from "@/shared/pdf.js";

// PDFJS.workerSrc = "/src/shared/pdf.worker.js";
// import {
//   PDFLinkService,
//   PDFPageView,
//   PDFFindController,
//   DefaultAnnotationLayerFactory,
//   DefaultTextLayerFactory
// } from "pdfjs-dist/web/pdf_viewer.js";

import PDFJS from 'pdf-dist/webpack.js'

// PDFJSAnnotate
import PDFJSAnnotate from '@/pdfjs/PDFJSAnnotate'
const { UI } = PDFJSAnnotate
PDFJSAnnotate.setStoreAdapter(new PDFJSAnnotate.LocalStoreAdapter())

// initColorPicker
import initColorPicker from '@/pdfjs/shared/initColorPicker'
import {

  findSVGAtPoint,
  getMetadata,
  // getOffset,
  scaleDown
  // scaleUp,
  // getAnnotationRect
} from '@/pdfjs/UI/utils'
import { addEventListener } from '@/pdfjs/UI/event'
import appendChild from '@/pdfjs/render/appendChild'

export default {
  name: 'Document',
  data() {
    return {
      viewer: null,
      PAGE_HEIGHT: 1,
      NUM_PAGES: 0,
      documentId: './static/test1.pdf',
      RENDER_OPTIONS: {
        documentId: './static/test1.pdf',
        pdfDocument: null,
        scale:
          parseFloat(localStorage.getItem(`${this.documentId}/scale`), 10) || 1.3,
        rotate: parseInt(localStorage.getItem(`${this.documentId}/rotate`), 10) || 0
      },
      newComment: '',
      comments: [],
      viewerWidth: 900,
      type: null,
      rects: null,
      svg: null,
      target: null,
      annotation: null,
      selectedText: null,
      inputHeight: null,
      order: null,
      hasSpace: true,
      editClicked: false
    }
  },
  mounted() {
    // Render stuff
    // #57 fix
    const renderedPages = {}
    const self = this
    document
      .getElementById('content-wrapper')
      .addEventListener('scroll', function(e) {
        const textTool = document.getElementById('textTool')
        textTool.style.visibility = 'hidden'
        const visiblePageNum =
          Math.round(e.target.scrollTop / self.PAGE_HEIGHT) + 1
        const visiblePage = document.querySelector(
          `.page[data-page-number="${visiblePageNum}"][data-loaded="false"]`
        )
        if (visiblePage) {
          // Prevent invoking UI.renderPage on the same page more than once.
          if (!renderedPages[visiblePageNum]) {
            renderedPages[visiblePageNum] = true
            setTimeout(function() {
              UI.renderPage(visiblePageNum, self.RENDER_OPTIONS)
            })
          }
        }
      })

    this.render()
    this.TextStuff()
    this.ToolbarButtons()
    this.ClearToolbarButton()
    // this.ScaleAndRotate();
    // this.ClearToolbarButton();
  },
  methods: {
    async render() {
      const self = this
      const pdf = await PDFJS.getDocument(this.RENDER_OPTIONS.documentId)
        .promise
      self.RENDER_OPTIONS.pdfDocument = pdf
      self.viewer = document.getElementById('viewer')
      self.viewer.innerHTML = ''
      self.NUM_PAGES = pdf.numPages

      for (let i = 0; i < self.NUM_PAGES; i++) {
        const page = UI.createPage(i + 1)
        self.viewer.appendChild(page)
      }

      // eslint-disable-next-line no-unused-vars
      const renderer = await UI.renderPage(1, self.RENDER_OPTIONS)
      const obj = {
        scale: self.RENDER_OPTIONS.scale,
        rotation: self.RENDER_OPTIONS.rotate
      }
      const viewport = renderer[0].getViewport(obj)
      self.PAGE_HEIGHT = viewport.height

      const pageWidth = document.getElementById('pageContainer1').offsetWidth
      this.viewerWidth = pageWidth + 250
      this.commentleftPos = pageWidth + 15

      const viewer = document.getElementById('viewer')
      const commentWrapper = document.getElementById('comment-wrapper')
      commentWrapper.style.left = pageWidth + 15 + 'px'
      viewer.appendChild(commentWrapper)

      this.comments = await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId)
      await this.comments.forEach(function(element) {
        element.isSelected = false
      })
      await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      this.TextSelection()
    },
    async TextStuff() {
      const self = this
      let textSize
      let textColor

      function initText() {
        const size = document.querySelector('.toolbar .text-size');
        [8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96].forEach(s => {
          size.appendChild(new Option(s, s))
        })

        setText(
          localStorage.getItem(`${self.RENDER_OPTIONS.documentId}/text/size`) ||
            10,
          localStorage.getItem(
            `${self.RENDER_OPTIONS.documentId}/text/color`
          ) || '#000000'
        )

        initColorPicker(
          document.querySelector('.text-color'),
          textColor,
          function(value) {
            setText(textSize, value)
          }
        )
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
    async ToolbarButtons() {
      const self = this
      let tooltype =
        localStorage.getItem(`${self.RENDER_OPTIONS.documentId}/tooltype`) ||
        'cursor'
      if (tooltype) {
        setActiveToolbarItem(
          tooltype,
          document.querySelector(`.toolbar button[data-tooltype=${tooltype}]`)
        )
      }

      function setActiveToolbarItem(type, button) {
        const active = document.querySelector('.toolbar button.active')
        if (active) {
          active.classList.remove('active')

          switch (tooltype) {
            case 'text':
              UI.disableText()
              break
            case 'point':
              UI.disablePoint()
              break
            case 'area':
            case 'highlight':
            case 'strikeout':
              UI.disableRect()
              break
          }
        }

        if (button) {
          button.classList.add('active')
        }
        if (tooltype !== type) {
          localStorage.setItem(
            `${self.RENDER_OPTIONS.documentId}/tooltype`,
            type
          )
        }
        tooltype = type

        switch (type) {
          case 'text':
            UI.enableText()
            break
          case 'point':
            UI.enablePoint()
            break
          case 'area':
          case 'highlight':
          case 'strikeout':
            UI.enableRect(type)
            break
        }
      }

      function handleToolbarClick(e) {
        if (e.target.nodeName === 'BUTTON') {
          setActiveToolbarItem(
            e.target.getAttribute('data-tooltype'),
            e.target
          )
        }
      }

      document
        .querySelector('.toolbar')
        .addEventListener('click', handleToolbarClick)
    },
    // Clear toolbar button
    ClearToolbarButton() {
      const self = this
      function handleClearClick() {
        if (confirm('Are you sure you want to clear annotations?')) {
          for (let i = 0; i < self.NUM_PAGES; i++) {
            document.querySelector(`div#pageContainer${i + 1} svg.annotationLayer`).innerHTML = ''
          }

          localStorage.removeItem(`${self.RENDER_OPTIONS.documentId}/annotations`)
        }
      }
      document.querySelector('a.clear').addEventListener('click', handleClearClick)
    },
    TextSelection() {
      const that = this
      // Register necessary events
      document.addEventListener('mouseup', handleDocumentMouseup)
      document.addEventListener('mousedown', handleDocumentMousedown)
      window.addEventListener('beforeunload', restoreCommentPositions)
      addEventListener('annotation:click', annotationClick)
      const commentCards = document.querySelectorAll('.comment-card')
      commentCards.forEach(item => {
        item.addEventListener('click', commentCardClick)
      })
      function handleDocumentMouseup(e) {
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
              } else {
                const textTool = document.getElementById('textTool')
                const posX = rects[rects.length - 1].left + (rects[rects.length - 1].width / 2)
                const posY = rects[rects.length - 1].top + 20
                textTool.style = 'position: absolute; top: ' + posY + 'px; left: ' + posX + 'px;'
                that.rects = rects
                that.svg = findSVGAtPoint(rects[0].left, rects[0].top)
                that.target = e.target
                that.selectedText = text
              }
            } else {
              // Display different tool bar
            }
          }
        }
      }
      function handleDocumentMousedown(e) {
        const svg = findSVGAtPoint(e.clientX, e.clientY)
        if (svg) {
          // hide text bool buttons
          if (!e.target.classList.contains('textToolBtn') &&
              !e.target.classList.contains('svg-inline--fa') &&
              !e.target.parentElement.classList.contains('svg-inline--fa')) {
            const textTool = document.getElementById('textTool')
            textTool.style.visibility = 'hidden'
          }
          // Hide add new comment form
          const newCommentWrapper = document.getElementById('add-new-comment')
          if (newCommentWrapper && newCommentWrapper.style.display != 'none') {
            const validTarget = that.hasAParentWithClass(e.target, 'add-new-comment')
            const comment = document.getElementById('comment-text-area').value
            if (!validTarget && comment.replace(/\s/g, '').length == 0) { that.cancelCommentText() }
          }
          // Unselect highlight comment
          if (!e.target.classList.contains('comment-highlight-selected')) {
            const validTarget = that.hasAParentWithClass(e.target, 'comment-card-selected')
            if (!validTarget) { that.unselectHighlightComment() }
          }
          return
        }
      }
      function annotationClick(target) {
        that.handleCommentAnnotationClick(target)
      }
      function commentCardClick() {
        that.handleCommentCardClick(this)
      }
      async function restoreCommentPositions() {
        await that.handleCommentPositionsRestore()
      }
    },
    handleCommentAnnotationClick(target) {
      const text = window.getSelection().toString().trim()
      // Display text tool bar if text is selected.
      if (!text) {
        const type = target.getAttribute('data-pdf-annotate-type')
        if (type == 'comment-highlight') {
          const selectedHighlight = document.getElementsByClassName('comment-highlight-selected')
          if (selectedHighlight.length > 0) { selectedHighlight[0].classList.remove('comment-highlight-selected') }
          target.classList.add('comment-highlight-selected')

          const commentCards = document.querySelectorAll('.comment-card')
          const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
          const highlightArr = Array.prototype.slice.call(commentHighlights)
          // Sort the highlights
          highlightArr.sort(this.compareTopAttributes)
          // Get order of the current highlight
          this.order = highlightArr.findIndex(item => {
            return item.dataset.pdfAnnotateId == target.getAttribute('data-pdf-annotate-id')
          })

          let gTop = parseInt(target.getAttribute('top')) - 35
          const svgHeight = parseInt(target.getAttribute('page-height'))
          const svgPageNum = parseInt(target.getAttribute('page-num'))
          if (svgPageNum > 1) { gTop += ((svgPageNum - 1) * svgHeight) }

          const cTop = this.comments[this.order].topPosition
          // parseInt(commentCards[this.order].style.top.substring(0, commentCards[this.order].style.top.length - 2));

          // remove current selected comment card class if any
          const selected = document.getElementsByClassName('comment-card-selected')
          if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
          // Set this card as selected
          commentCards[this.order].classList.add('comment-card-selected')

          if (cTop != gTop) {
            // Move the comment up to gTop
            this.comments[this.order].topPosition = gTop

            const selected = document.getElementsByClassName('comment-card-selected')
            if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
            commentCards[this.order].classList.add('comment-card-selected')

            const endPos = gTop + commentCards[this.order].offsetHeight
            if (cTop > gTop) {
              // Move up other uppen comments
              if (this.order > 0) { this.moveUpFromTopPos(commentCards, this.order - 1, gTop) }
              // Move up other lower comments
              if (this.order < commentCards.length - 1) { this.moveUpToEndPos(commentCards, this.order + 1, endPos) }
            } else if (cTop < gTop) {
              // Move down other uppen comments
              if (this.order > 0) { this.moveDownToTopPos(commentCards, this.order - 1, gTop) }
              // Move down other lower comments
              if (this.order < commentCards.length - 1) { this.moveDownFromEndPos(commentCards, this.order + 1, endPos) }
            }
          }
        }
      }
    },
    handleCommentCardClick(el) {
      const highlightId = el.getAttribute('highlight-id')
      const highlight = document.querySelector("g[data-pdf-annotate-id='" + highlightId + "']")
      this.handleCommentAnnotationClick(highlight)
    },
    async handleCommentPositionsRestore() {
      const documentId = document.getElementById('viewer').getAttribute('document-id')
      const highlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const highlightArr = Array.prototype.slice.call(highlights)
      // Sort the highlights
      highlightArr.sort(this.compareTopAttributes)
      this.handleCommentAnnotationClick(highlightArr[0])
      const comments = await this.updateCommentAnnotations(documentId)
      await PDFJSAnnotate.getStoreAdapter().updateComments(documentId, comments)
    },
    async updateCommentAnnotations(documentId) {
      const comments = await PDFJSAnnotate.getStoreAdapter().getComments(documentId)
      comments.forEach(comment => {
        const thisCard = document.querySelector(".comment-card[highlight-id='" + comment.uuid + "']")
        comment.topPosition = parseInt(thisCard.getAttribute('top-position'))
      })
      return comments
    },
    async HighlightText() {
      await this.highlightText('highlight')
    },
    async StrikethroughText() {
      await this.highlightText('strikeout')
      const textTool = document.getElementById('textTool')
      textTool.style.visibility = 'hidden'
    },
    // Migration code starts here
    async CommentText() {
      if (this.newComment.replace(/\s/g, '').length != 0) {
        // Save in-progress comment
        await this.addCommentText()
      }
      await this.commentText()
    },
    async addCommentText() {
      // let newComment = await UI.addCommentText(this.newComment, this.RENDER_OPTIONS.documentId);
      const commentWrapper = document.getElementById('add-new-comment')
      const topPos = parseInt(commentWrapper.style.top.substring(0, commentWrapper.style.top.length - 2))
      const newComment = await PDFJSAnnotate.getStoreAdapter().addComment(this.documentId,
        this.annotation,
        this.newComment,
        this.selectedText,
        topPos)
      await this.comments.push(newComment)
      await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      document.getElementById('add-new-comment').style.display = 'none'

      await this.updatePositionsAfterCommentAdded()
      this.newComment = ''
    },
    // Begin migrating code from select.js to document.vue
    async commentText() {
      // Highlight the text first
      await this.highlightText('comment-highlight')
      // Update positions of comment boxes
      this.updateCommentPositions()
      // Display add new form
      this.displayAddNewCommentForm()
      event.stopPropagation()
    },
    async highlightText(type) {
      if (!this.svg) {
        return
      }
      const boundingRect = this.svg.getBoundingClientRect()

      let color = 'FFFF00'
      if (type == 'strikeout') { color = 'FF0000' }

      // Initialize the annotation
      const annotation = {
        type,
        color,
        rectangles: [...this.rects].map((r) => {
          let offset = 0
          if (type == 'strikeout') { offset = r.height / 2 }
          return scaleDown(this.svg, {
            y: (r.top + offset) - boundingRect.top,
            x: r.left - boundingRect.left,
            width: r.width,
            height: r.height
          })
        }).filter((r) => r.width > 0 && r.height > 0 && r.x > -1 && r.y > -1)
      }

      // Short circuit if no rectangles exist
      if (annotation.rectangles.length === 0) {
        return
      }

      if (annotation.rectangles.length > 2) {
        annotation.rectangles = this.removeMinHeigthRects(annotation.rectangles)
      }

      const { documentId, pageNumber } = getMetadata(this.svg)

      annotation.top = parseInt(this.target.style.top.substring(0, this.target.style.top.length - 2))
      annotation.pageNum = pageNumber
      annotation.pageHeight = this.svg.getAttribute('height')
      this.annotation = annotation
      // Add the annotation
      await PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
        .then((annotation) => {
          appendChild(this.svg, annotation)
          this.hideTextToolBar()
        })
    },
    updateCommentPositions() {
      const commentCards = document.querySelectorAll('.comment-card')
      const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const highlightArr = Array.prototype.slice.call(commentHighlights)
      // Sort the highlights
      highlightArr.sort(this.compareTopAttributes)
      // Get order of the current highlight
      this.order = highlightArr.findIndex(item => {
        return item.dataset.pdfAnnotateId == this.annotation.uuid
      })
      this.hasSpace = this.hasEnoughSpace(commentCards)
      if (!this.hasSpace) {
        // Update comment positions

        // get add new form's top position
        const svg = this.svg
        const rectTop = parseInt(this.target.style.top.substring(0, this.target.style.top.length - 2))
        const svgHeight = svg.getAttribute('height')
        const svgPageNum = svg.getAttribute('data-pdf-annotate-page')
        let svgTop = 0
        if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
        const topPos = svgTop + rectTop - 35
        const endPos = topPos + 126

        if (this.order == 0) {
          // Move down all comment cards
          this.moveDownFromEndPos(commentCards, 0, endPos)
        } else if (this.order == highlightArr.length - 1) {
          // Move up all comment cards
          this.moveUpFromTopPos(commentCards, commentCards.length - 1, topPos)
        } else {
          // Move up commentCards[0] -> commentCards[i - 1]
          const upperTop = parseInt(commentCards[this.order - 1].style.top.substring(0, commentCards[this.order - 1].style.top.length - 2))
          const upperEnd = upperTop + commentCards[this.order - 1].offsetHeight
          // Check if move up is necessary
          if (upperEnd > (topPos - 10)) {
            this.moveUpFromTopPos(commentCards, this.order - 1, topPos)
          } else {
            // Check if move down is necessary
            const commentId = commentCards[this.order - 1].getAttribute('highlight-id')
            const g = this.getHighlightByCommentId(commentId)
            let gTop = parseInt(g.getAttribute('top')) - 35
            if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }
            if (upperTop < gTop) {
              this.moveDownToTopPos(commentCards, this.order - 1, topPos)
            }
          }
          // Move down commentCards[i] -> commentCards[commentCards.length - 1]
          const lowerTop = parseInt(commentCards[this.order].style.top.substring(0, commentCards[this.order].style.top.length - 2))
          if (lowerTop < (endPos + 10)) {
            this.moveDownFromEndPos(commentCards, this.order, endPos)
          } else {
            // Check if move up is necessary
            const commentId = commentCards[this.order].getAttribute('highlight-id')
            const g = this.getHighlightByCommentId(commentId)
            let gTop = parseInt(g.getAttribute('top')) - 35
            if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }
            if (lowerTop > gTop) {
              this.moveUpToEndPos(commentCards, this.order, endPos)
            }
          }
        }
      }
    },
    async updatePositionsAfterCommentAdded() {
      const newComment = document.querySelector(".comment-card[highlight-id='" + this.annotation.uuid + "']")

      if (!this.hasSpace) {
        const commentCards = document.querySelectorAll('.comment-card')
        const endPos = parseInt(newComment.getAttribute('top-position')) + newComment.offsetHeight
        if (this.order < (commentCards.length - 1)) { this.moveUpToEndPos(commentCards, this.order + 1, endPos) }
      }
      const that = this
      newComment.addEventListener('click', function commentCardClick() {
        that.handleCommentCardClick(this)
      })
    },
    displayAddNewCommentForm() {
      const newCommentWrapper = document.getElementById('add-new-comment')
      newCommentWrapper.style.visibility = 'visible'
      const svg = this.svg
      const rectTop = parseInt(this.target.style.top.substring(0, this.target.style.top.length - 2))
      const svgHeight = svg.getAttribute('height')
      const svgPageNum = svg.getAttribute('data-pdf-annotate-page')

      let svgTop = 0
      if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
      const topPos = svgTop + rectTop - 35
      newCommentWrapper.style = 'width: 100%; position: absolute; left: 5px; top: ' + topPos + 'px;'
      this.hideTextToolBar()
      const textArea = document.getElementById('comment-text-area')
      textArea.focus()
      this.inputHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2))
      const that = this
      textArea.addEventListener('keydown', function() {
        const commentCards = document.querySelectorAll('.comment-card')
        if (commentCards.length > 0) {
          setTimeout(function() {
            const newHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2))
            const newCommentWrapper = document.getElementById('add-new-comment')
            const topPos = parseInt(newCommentWrapper.style.top.substring(0, newCommentWrapper.style.top.length - 2))
            const endPos = topPos + newCommentWrapper.offsetHeight
            if (that.inputHeight < newHeight) {
              if (that.order <= commentCards.length - 1) { that.moveDownFromEndPos(commentCards, that.order, endPos) }
            } else if (that.inputHeight > newHeight) {
              if (that.order < commentCards.length) { that.moveUpToEndPos(commentCards, that.order, endPos) }
            }
            that.inputHeight = newHeight
          }, 100)
        }
      })
    },
    cancelCommentText() {
      this.newComment = ''
      const documentId = getMetadata(this.svg).documentId
      const newCommentWrapper = document.getElementById('add-new-comment')
      const topPos = parseInt(newCommentWrapper.style.top.substring(0, newCommentWrapper.style.top.length - 2))
      const endPos = topPos + newCommentWrapper.offsetHeight
      newCommentWrapper.style.display = 'none'
      const commentCards = document.querySelectorAll('.comment-card')
      if (commentCards.length > 0) {
        if (this.order <= (commentCards.length - 1)) {
          this.moveUpToEndPos(commentCards, this.order, topPos)
          if (this.order > 0) {
            const orderTopPos = this.comments[this.order].topPosition
            // parseInt(commentCards[this.order].style.top.substring(0, commentCards[this.order].style.top.length - 2));
            this.moveDownToTopPos(commentCards, this.order - 1, orderTopPos)
          }
        }
        if (this.order == commentCards.length) {
          this.moveDownToTopPos(commentCards, this.order - 1, endPos)
        }
      }
      // Remove the annotation
      PDFJSAnnotate.getStoreAdapter().deleteAnnotation(documentId, this.annotation.uuid)
        .then((isDeleted) => {
          if (isDeleted) {
            // remove highlight in UI
            document.querySelector("g[data-pdf-annotate-id='" + this.annotation.uuid + "']").remove()
            this.annotation = null
          }
        })
    },
    unselectHighlightComment() {
      const selectedHighlight = document.getElementsByClassName('comment-highlight-selected')
      if (selectedHighlight.length > 0) { selectedHighlight[0].classList.remove('comment-highlight-selected') }
      const selectedComment = document.getElementsByClassName('comment-card-selected')
      if (selectedComment.length > 0) { selectedComment[0].classList.remove('comment-card-selected') }
    },
    moveUpFromTopPos(commentCards, startIndex, topPos) {
      // Get start index card's position
      const adjTop = this.comments[startIndex].topPosition
      const adjEnd = adjTop + commentCards[startIndex].offsetHeight
      // Move the start index card up first

      this.comments[startIndex].topPosition = adjTop - (adjEnd - topPos) - 10
      if (startIndex > 0) {
        for (let m = startIndex - 1; m >= 0; m--) {
          const previousTop = this.comments[m + 1].topPosition
          const thisTop = this.comments[m].topPosition
          const thisEnd = thisTop + commentCards[m].offsetHeight
          if (thisEnd > (previousTop - 10)) {
            this.comments[m].topPosition = thisTop - (thisEnd - previousTop) - 10
          } else {
            break
          }
        }
      }
    },
    moveUpToEndPos(commentCards, startIndex, endPos) {
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      let gTop = parseInt(g.getAttribute('top')) - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }
      if (this.comments[startIndex].topPosition > gTop) {
        const desiredTop = gTop >= (endPos + 10) ? gTop : (endPos + 10)
        // Move start index card up
        this.comments[startIndex].topPosition = desiredTop
        if (commentCards.length > (startIndex + 1)) {
          for (let n = startIndex + 1; n < this.comments.length; n++) {
            const previousTop = this.comments[n - 1].topPosition
            const previousEnd = previousTop + commentCards[n - 1].offsetHeight
            const thisTop = this.comments[n].topPosition

            // Check if move down is necessary
            if (thisTop > (previousEnd + 10)) {
              const thisG = this.getHighlightByCommentId(this.comments[n].uuid)
              let thisGTop = parseInt(thisG.getAttribute('top')) - 35
              if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * thisG.getAttribute('page-height')) }
              if (thisTop > thisGTop) {
                const thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10)
                this.comments[n].topPosition = thisDesiredTop
              }
            } else {
              break
            }
          }
        }
      }
    },
    moveDownToTopPos(commentCards, startIndex, topPos) {
      // Get start index card's position
      const adjTop = this.comments[startIndex].topPosition
      const adjEnd = adjTop + commentCards[startIndex].offsetHeight
      // Check if move down is necessary
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      let gTop = parseInt(g.getAttribute('top')) - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height')) }
      // Only move cards down if the upper card is higher that its highlight
      if (adjTop < gTop) {
        const desiredTop = gTop <= (adjTop + (topPos - adjEnd) - 10) ? gTop : (adjTop + (topPos - adjEnd) - 10)
        // Move upper adjacent card down
        this.comments[startIndex].topPosition = desiredTop
        // Move other cards down
        if (startIndex > 0) {
          for (let n = startIndex - 1; n >= 0; n--) {
            const previousTop = this.comments[n + 1].topPosition
            // let previousEnd = previousTop + commentCards[n + 1].offsetHeight;
            const thisTop = this.comments[n].topPosition
            const thisEnd = thisTop + commentCards[n].offsetHeight
            // Check if move down is necessary
            if (thisEnd < (previousTop - 10)) {
              const thisG = this.getHighlightByCommentId(this.comments[n].uuid)
              let thisGTop = parseInt(thisG.getAttribute('top')) - 35
              if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * thisG.getAttribute('page-height')) }
              if (thisTop < thisGTop) {
                const thisDesiredTop = thisGTop <= (thisTop + (previousTop - thisEnd) - 10) ? thisGTop : (thisTop + (previousTop - thisEnd) - 10)
                this.comments[n].topPosition = thisDesiredTop
              }
            } else {
              break
            }
          }
        }
      }
    },
    moveDownFromEndPos(commentCards, startIndex, endPos) {
      // Move the start index card down first
      this.comments[startIndex].topPosition = endPos + 10
      // Move other cards down ward
      if (this.comments.length > (startIndex + 1)) {
        for (let j = startIndex + 1; j < this.comments.length; j++) {
          const previousTop = this.comments[j - 1].topPosition
          const previousEnd = previousTop + commentCards[j - 1].offsetHeight
          const thisTop = this.comments[j].topPosition
          if (thisTop < (previousEnd + 10)) {
            this.comments[j].topPosition = thisTop + (previousEnd - thisTop) + 10
          } else {
            break
          }
        }
      }
    },
    hasEnoughSpace(commentCards) {
      if (this.comments.length > 0) {
        const svg = this.svg
        const rectTop = parseInt(this.target.style.top.substring(0, this.target.style.top.length - 2))
        const svgHeight = svg.getAttribute('height')
        const svgPageNum = svg.getAttribute('data-pdf-annotate-page')

        let svgTop = 0
        if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
        const topPos = svgTop + rectTop - 35
        const endPos = topPos + 105

        // Check first and last position
        const firstTopPos = this.comments[0].topPosition
        const lastEndPos = this.comments[this.comments.length - 1].topPosition + commentCards[commentCards.length - 1].offsetHeight

        if (endPos < firstTopPos || topPos > lastEndPos) { return true }
        if (this.comments.length > 1) {
          for (let i = 0; i < this.comments.length - 1; i++) {
            const thisTopPos = this.comments[i].topPosition
            const thisEndPos = thisTopPos + commentCards[i].offsetHeight
            const nextTopPos = this.comments[i + 1].topPosition
            if (topPos > thisEndPos && endPos < nextTopPos) {
              return true
            }
          }
        }

        return false
      }
      return true
    },
    compareTopAttributes(a, b) {
      let top1 = parseInt(a.getAttribute('top'))
      if (a.getAttribute('page-num') > 1) { top1 += ((a.getAttribute('page-num') - 1) * a.getAttribute('page-height')) }

      let top2 = parseInt(b.getAttribute('top'))
      if (b.getAttribute('page-num') > 1) { top2 += ((b.getAttribute('page-num') - 1) * b.getAttribute('page-height')) }
      if (top1 > top2) return 1
      if (top2 > top1) return -1
      if (top2 == top1) {
        // compare order attributes
        const order1 = parseInt(a.getAttribute('order'))
        const order2 = parseInt(b.getAttribute('order'))
        if (order1 > order2) return 1
        if (order2 > order1) return -1
      }
      return 0
    },
    removeMinHeigthRects(arr) {
      const min = Math.min(...arr.map(a => a.height))
      return arr.filter(a => a.height != min)
    },
    displayEditComment(comment) {
      this.comments.forEach(function(element) {
        element.isSelected = false
      })
      comment.isSelected = true
      this.$forceUpdate()
      const refName = 'comment' + comment.uuid
      this.$refs[refName][0].$el.childNodes[0].focus()
    },
    editCommentCard() {

    },
    deleteCommentCard(commentId) {
      console.log(commentId)
    },
    hideTextToolBar() {
      const textTool = document.getElementById('textTool')
      textTool.style.visibility = 'hidden'
    },
    getHighlightByCommentId(commentId) {
      return document.querySelector("g[data-pdf-annotate-id='" + commentId + "']")
    },
    getSelectionRects() {
      const selection = window.getSelection()
      const range = selection.getRangeAt(0)
      const rects = range.getClientRects()

      if (rects.length > 0 &&
          rects[0].width > 0 &&
          rects[0].height > 0) {
        return rects
      }
      return null
    },
    hasAParentWithClass(element, className) {
      for (let i = 0; i <= 5; i++) {
        if (element.classList.contains(className)) { return true } else { element = element.parentNode }
      }
      return false
    }
    // End migration
  }
}
</script>

<style scoped>
@import '../../pdfjs/shared/document.css';
@import '../../pdfjs/shared/toolbar.css';
@import '../../pdfjs/shared/pdf_viewer.css';

</style>
