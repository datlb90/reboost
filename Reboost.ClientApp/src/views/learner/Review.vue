<template>
  <div id="reviewContainer" style="border-top: 1px solid rgb(223 224 238); padding: 5px; background: rgb(248, 249, 250);">

    <div id="content-wrapper" style="background: rgb(248, 249, 250); height: inherit; width: 100%;position: absolute; overflow: unset;">
      <div id="left-panel" :class="{'hideQuestion': !showQuestion}" style="float: left; width: 30%; position: sticky; top: 0px; min-width: 320px; height: 100%;">
        <el-tabs type="border-card">
          <el-tab-pane label="Question">
            <tabQuestion ref="tabQuestion" :questionid="questionId" :reviewid="reviewId" />
          </el-tab-pane>
          <el-tab-pane label="Rubric">
            <tabRubric ref="tabRubric" :questionid="questionId" :reviewid="reviewId" @submitted="disableToolbarSubmit" @setStatusText="setStatusText" />
          </el-tab-pane>
          <el-tab-pane label="Help">Questions and Answers</el-tab-pane>
        </el-tabs>
      </div>

      <div id="right-panel" :class="{'hideQuestion':!showQuestion}" style="float: left; position: absolute; margin-left: max(325px, 30.5%); width: 68.5%;height:100%; background: rgb(248, 249, 250);">
        <toolbar
          ref="toolBar"
          :review-page="this"
          :expandcolorpicker.sync="expandColorPicker"
          :documentid="documentId"
          :reviewid="reviewId"
          :renderoptions="RENDER_OPTIONS"
          @expandColorPickerToggle="expandColorPicker=$event"
          @hideQuestion="hideQuestion($event)"
          @submit="submitReview"
          @redoAnnotation="redo($event)"
          @undoAnnotation="undoAnnotation"
          @scaleChange="handleScale($event)"
          @highLightText="highlightEvent($event)"
        />
        <div id="viewerContainer">
          <div
            id="viewer"
            class="pdfViewer"
            :document-id="documentId"
          />
        </div>
      </div>

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
          <el-button style="right: 10px;padding:10px 0!important;" button-id="delete" class="action-card-btn"	title="Delete Comment" @click="isUndo = false; deleteButtonClicked(comment)">
            <i class="far fa-trash-alt" />
          </el-button>
          <el-button style="right: 30px;padding:10px 0!important;" button-id="edit" class="action-card-btn"	title="Edit Comment"	@click="displayEditComment(comment, idx)">
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
            @input="updateCommentPositionWhileEditing"
            @focus="updateCommentPositionWhileEditing"
            @change="updateCommentPositionWhileEditing"
          />
          <div v-show="comment.isSelected == true" style="height: 20px; margin-top: 10px;">
            <el-button type="primary" :disabled="comment.content.replace(/\s/g, '').length == 0" style="float: left; padding: 5px;" @click="editCommentCard(comment)">
              Comment
            </el-button>
            <el-button style="float: right; padding: 5px;" @click="cancelCommentText('edit')">
              Cancel
            </el-button>
          </div>
        </div>
      </el-card>
    </div>

    <!-- Inline text tools -->
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
    <!-- /Inline text tools -->
    <!-- Inline color picker -->
    <el-button-group id="rectTool" style="display: none;">
      <el-button class="textToolBtn" @click="showColorPickerTool('area')">
        <i class="fas fa-palette" />
      </el-button>
      <el-button class="textToolBtn" @click="deleteAnnotation()">
        <i class="fas fa-trash-alt" />
      </el-button>
    </el-button-group>
    <el-button-group id="deleteTool" style="display: none;">
      <el-button class="textToolBtn" @click="deleteAnnotation()">
        <i class="fas fa-trash-alt" />
      </el-button>
    </el-button-group>
    <div id="colorPickerTool" class="colorPicker">
      <span>
        Stroke
      </span>
      <ul class="group-color">
        <li v-for="item in listColor" :key="item.name" @click="changeColor(item.name)"><button :style="{'background-color': item.name, height:18+'px',width:18+'px', margin:'5px 5px 5px 5px', 'border-radius':'50%','outline': 'none','border': 'none'}" /></li>
      </ul>
    </div>
    <!-- /Inline color picker -->
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
import ToolBar from '../../components/controls/Viewer_ToolBar'
import TabQuestion from './Review_TabQuestion'
import TabRubric from './Review_TabRubric'
// PDFJSAnnotate
import PDFJSAnnotate from '@/pdfjs/PDFJSAnnotate'
const { UI } = PDFJSAnnotate
PDFJSAnnotate.setStoreAdapter(new PDFJSAnnotate.LocalStoreAdapter())

import {
  getMetadata,
  // getOffset,
  scaleDown
  // scaleUp,
  // getAnnotationRect
} from '@/pdfjs/UI/utils'
import { addEventListener } from '@/pdfjs/UI/event'
import appendChild from '@/pdfjs/render/appendChild'
import http from '@/utils/axios'
import reviewService from '@/services/review.service.js'
import { enableEdit } from '@/pdfjs/UI/edit'
import { enableTextSelection } from '@/pdfjs/UI/select-text.js'
import initColorPicker from '../../pdfjs/shared/initColorPicker'
import { deleteAnnotations } from '@/pdfjs/UI/edit.js'
// import { highlightText } from '../../pdfjs/UI/highlight-text.js'

export default {
  name: 'Document',
  components: {
    'toolbar': ToolBar,
    'tabQuestion': TabQuestion,
    'tabRubric': TabRubric
  },
  data() {
    return {
      rubricCriteria: [],
      criteriaFeedback: {},
      showDirection: true,
      viewer: null,
      PAGE_HEIGHT: 1,
      NUM_PAGES: 0,
      questionId: 1,
      documentId: 67,
      reviewId: 2,
      RENDER_OPTIONS: {
        documentId: null,
        pdfDocument: null,
        scale: 1, // parseFloat(localStorage.getItem(`${this.documentId}/scale`), 10) || 1.3,
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
      editClicked: false,
      loadCompleted: false,
      showQuestion: true,
      scaleRatio: 1,
      defaultScale: 1,
      scaleText: '100%',
      currentTooltype: null,
      annotationClicked: null,
      previousToolType: true,
      statusText: '',
      listColor: require('../../assets/data.json').listColor,
      colorChosen: null,
      expandColorPicker: false,
      activeButton: null,
      rubicCommentDelay: null,
      undoAnnotationList: [],
      boundingRect: null,
      zoomDelayHandle: null,
      fitDocumentWidth: false,
      note: null,
      undoHistory: [],
      redoHistory: [],
      annotationDelete: null,
      isUndo: false,
      preEditedAnno: null
    }
  },
  computed: {
    loadedAnnotation() {
      const data = this.$store.getters['review/getAnnotation']
      if (!data) {
        return null
      }

      return {
        annotations: data.annotations.map(a => ({ ...JSON.parse(a.data), id: a.id, top: a.top, pageNum: a.pageNum, color: a.color })),
        comments: data.comments.map(c => ({ ...JSON.parse(c.data), documentId: this.documentId, id: c.id }))
      }
    }
  },
  beforeMount() {
    if (this.$route.params.docId && this.$route.params.reviewId && this.$route.params.questionId) {
      this.questionId = +this.$route.params.questionId
      this.reviewId = +this.$route.params.reviewId
      this.documentId = +this.$route.params.docId
    }
  },
  async mounted() {
    window['PDFJSAnnotate'] = PDFJSAnnotate
    window['APP'] = this
    localStorage.setItem(`${this.RENDER_OPTIONS.documentId}/tooltype`, 'cursor')
    localStorage.removeItem('colorChosen')
    PDFJSAnnotate.getStoreAdapter().clearAnnotations(this.documentId)

    // Render stuff
    this.$store.dispatch('review/loadReviewAnnotation', { docId: this.documentId, reviewId: this.reviewId }).then(async() => {
      PDFJSAnnotate.getStoreAdapter().loadAnnotations(this.documentId, this.loadedAnnotation)

      await this.render()
      this.$refs.toolBar.handleScale('fitPage')
      // this.ToolbarButtons()
      // this.ScaleAndRotate();
    })

    enableEdit()
  },

  beforeCreate: function() {
    document.body.style = 'overflow: hidden'
  },
  created() {

  },
  destroyed() {
    clearInterval(this.setIntervalForScroll)
    document.removeEventListener('keyup', this.keyupHandler)
  },
  methods: {
    annotationAdded(documentId, pageNumber, annotation) {
      if (annotation.type == 'textbox' || annotation.type == 'area') {
        annotation.documentId = this.documentId
        var obj = {
          DocumentId: this.documentId,
          ReviewId: this.reviewId,
          Type: annotation.type,
          PageNum: annotation.page,
          Top: annotation.x,
          Color: annotation.color,
          Uuid: annotation.uuid,
          Data: JSON.stringify(annotation)
        }
        this.$store.dispatch('review/addReviewAnnotation', obj).then(async rs => {
          var temp = this.$store.getters['review/getAddedAnnotation']
          annotation.id = temp.id
          await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, annotation.uuid, annotation)
          this.undoHistory.push({ action: 'added', annotation: annotation })
          this.updateUndoList()
          this.setStatusText()
        })
      } else if (annotation.type == 'point') {
        // 123
      }
    },
    handleAnnotationDelete(target) {
      if (target.type == 'comment-highlight' || target.type == 'point') {
        PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(r => {
          var cmt = r.filter(t => { return t.uuid == target.uuid })[0]
          if (cmt) {
            this.deleteButtonClicked(cmt)
          }
        })
      } else {
        reviewService.deleteAnnotation(target.id).then(rs => {
          this.undoHistory.push({ action: 'deleted', annotation: target })
          this.updateUndoList()
          this.removeElementById(target.uuid)
          this.setStatusText()
        })
      }
    },

    base64ToArrayBuffer(base64) {
      const binaryString = window.atob(base64)
      const binaryLen = binaryString.length
      const bytes = new Uint8Array(binaryLen)
      for (let i = 0; i < binaryLen; i++) {
        const ascii = binaryString.charCodeAt(i)
        bytes[i] = ascii
      }
      return bytes
    },
    async render() {
      const self = this
      const response = await http.get(`http://localhost:6990/api/document/${this.documentId}`)
      const arrayBuffer = self.base64ToArrayBuffer(response.data.data)

      const pdf = await PDFJS.getDocument(arrayBuffer).promise
      self.RENDER_OPTIONS.documentId = this.documentId
      self.RENDER_OPTIONS.pdfDocument = pdf
      self.viewer = document.getElementById('viewer')
      self.viewer.innerHTML = ''
      self.NUM_PAGES = pdf.numPages

      var docWidth = document.getElementById('right-panel').offsetWidth - document.getElementById('comment-wrapper').offsetWidth - 15

      for (let i = 0; i < self.NUM_PAGES; i++) {
        const page = UI.createPage(i + 1)
        self.viewer.appendChild(page)
      }

      // eslint-disable-next-line no-unused-vars
      const renderer = await UI.renderPage(1, self.RENDER_OPTIONS)

      const obj = {
        scale: self.RENDER_OPTIONS.scale,
        rotation: self.RENDER_OPTIONS.rotatex
      }
      const viewport = renderer[0].getViewport(obj)
      self.PAGE_HEIGHT = viewport.height

      const pageWidth = document.getElementById('page1').offsetWidth
      document.getElementById('tool-bar').style.width = 100 + '%'

      this.viewerWidth = pageWidth + 250
      this.commentleftPos = docWidth

      const viewerContainer = document.getElementById('viewerContainer')
      const commentWrapper = document.getElementById('comment-wrapper')
      // commentWrapper.style.left = this.commentleftPos + 'px'
      viewerContainer.appendChild(commentWrapper)

      this.comments = await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId)
      await this.comments.forEach(function(element) {
        element.isSelected = false
      })
      await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      this.handleCommentPositionsRestore()
      this.hideDeleteToolBar()

      this.registerEvents()
      enableTextSelection(this)
      await this.reRenderPages()
    },
    registerEvents() {
      const that = this
      const renderedPages = {}

      const commentCards = document.querySelectorAll('.comment-card')
      commentCards.forEach(item => {
        item.addEventListener('click', commentCardClick)
      })

      window.addEventListener('beforeunload', restoreCommentPositions)
      window.addEventListener('resize', this.calculateContainerHeight.bind(this))
      document.addEventListener('keyup', this.keyupHandler)
      document.getElementById('viewerContainer').addEventListener('scroll', handlePageScroll)

      addEventListener('annotation:click', annotationClick)
      addEventListener('annotation:delete', annotationDelete)
      addEventListener('comment:delete', deleteComment)
      addEventListener('annotation:add', this.annotationAdded)
      addEventListener('annotation:insertNoteComment', this.insertNoteComment)

      function handlePageScroll(e) {
        // document.getElementById('left-panel').style.top = e.target.scrollTop + 'px'
        // eslint-disable-next-line no-console
        const rectTool = document.getElementById('rectTool')
        rectTool.style.visibility = 'hidden'
        const colorPickerTool = document.getElementById('colorPickerTool')
        colorPickerTool.style.visibility = 'hidden'
        const textToolGroup = document.getElementById('textToolGroup')
        textToolGroup.style.visibility = 'hidden'
        const deleteGroup = document.getElementById('deleteTool')
        deleteGroup.style.visibility = 'hidden'
        const visiblePageNum = Math.round(e.target.scrollTop / that.PAGE_HEIGHT) + 1

        const visiblePage = document.querySelector(
          `.page[data-page-number="${visiblePageNum}"][data-loaded="false"]`
        )
        if (visiblePage) {
          // Prevent invoking UI.renderPage on the same page more than once.
          if (!renderedPages[visiblePageNum]) {
            renderedPages[visiblePageNum] = true
            setTimeout(function() {
              UI.renderPage(visiblePageNum, that.RENDER_OPTIONS)
            })
          }
        }
      }
      function annotationClick(target) {
        if (!target) {
          return
        }
        PDFJSAnnotate.getStoreAdapter().getAnnotation(that.documentId, target.getAttribute('data-pdf-annotate-id')).then(r => {
          that.annotation = r
        })
        that.handleCommentAnnotationClick(target)
      }
      function annotationDelete(target) {
        that.handleAnnotationDelete(target)
      }
      function deleteComment(docId, comment) {
        that.handleCommentDelete(comment)
      }
      function commentCardClick(e) {
        if (e.target.getAttribute('button-id') != 'delete') {
          that.handleCommentCardClick(this)
        }
      }
      async function restoreCommentPositions() {
        await that.handleCommentPositionsRestore()
      }
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
    async toolBarButtonClick(e) {
      UI.disableText()
      UI.disablePoint()
      UI.disableRect()
      this.activeButton = e
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
      console.log('Activate Button', e)
    },

    clearAllAnnotations() {
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
    handleCommentDelete(comment) {
      reviewService.deleteInTextComment(comment.id).then(rs => {
        var obj = {
          action: 'deleted',
          annotation: JSON.parse(rs.data)
        }
        if (this.isUndo) {
          this.redoHistory.push(obj)
          this.updateRedoList()
        } else {
          this.undoHistory.push(obj)
          this.updateUndoList()
        }
        this.setStatusText()
      })
    },
    async handleCommentAnnotationClick(target) {
      const text = window.getSelection().toString().trim()
      const newCommentWrapper = document.getElementById('add-new-comment')
      if (newCommentWrapper && newCommentWrapper.style.display != 'none') {
        return
      }
      // Display text tool bar if text is selected.
      if (!text && (typeof (target) != 'undefined')) {
        const type = target.getAttribute('data-pdf-annotate-type')
        this.annotationClicked = target
        if (type == 'comment-highlight' || type == 'highlight' || type == 'point' || type == 'strikeout') {
          // var rect = target.getBoundingClientRect()
          // const deleteTool = document.getElementById('deleteTool')
          // posX = (parseInt(target.getAttribute('x')) + parseInt(target.getAttribute('width')) / 4) * this.RENDER_OPTIONS.scale
          // console.log('posX', posX)
          // if (this.showQuestion) {
          //   posX += parseInt(document.getElementById('left-panel').offsetWidth)
          // }
          // const posY = parseInt(rect.top) + parseInt(target.getAttribute('height')) * this.RENDER_OPTIONS.scale + 10
          // deleteTool.style = 'position: absolute; top: ' + posY + 'px; left: ' + posX + 'px;'

          this.colorChosen = target.getAttribute('stroke')
          uuid = target.getAttribute('data-pdf-annotate-id')
          this.annotation = this.loadedAnnotation.annotations.filter(r => { return r.uuid === uuid })[0]
          var rect = target.getBoundingClientRect()
          // Display rect tool bar if text is selected.
          if (target.getAttribute('top') == '') {
            // window.getSelection().removeAllRanges()
            this.hideDeleteToolBar()
          } else {
            const deleteTool = document.getElementById('deleteTool')
            posX = rect.left
            const posY = parseInt(rect.top) + rect.height + 10
            deleteTool.style = 'position: absolute; top: ' + posY + 'px; left: ' + posX + 'px;'
          }
        }
        if (type == 'comment-highlight' || type == 'point') {
          const selectedHighlight = document.getElementsByClassName('comment-highlight-selected')
          if (selectedHighlight.length > 0) { selectedHighlight[0].classList.remove('comment-highlight-selected') }
          target.classList.add('comment-highlight-selected')

          const commentCards = document.querySelectorAll('.comment-card')
          const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
          const commentPoints = document.querySelectorAll("[data-pdf-annotate-type='point']")
          const highlightArr = Array.prototype.slice.call(commentHighlights)
          var pointsArr = Array.prototype.slice.call(commentPoints)

          // Sort the highlightsvar pointsArr = Array.prototype.slice.call(commentPoints)
          // pointsArr.forEach(r => {
          //   r.setAttribute('top', parseInt(r.getAttribute('y')))
          // })
          // highlightArr.sort(this.compareTopAttributes)
          const combineArr = highlightArr.concat(pointsArr)
          combineArr.sort(this.compareTopAttributes)

          // Get order of the current highlight
          this.order = combineArr.findIndex(item => {
            return item.dataset.pdfAnnotateId == target.getAttribute('data-pdf-annotate-id')
          })
          var gTop = parseInt(parseInt(target.getAttribute('top')) * this.RENDER_OPTIONS.scale) - 34
          const svgHeight = parseInt(target.getAttribute('page-height') * this.RENDER_OPTIONS.scale)
          const svgPageNum = parseInt(target.getAttribute('page-num'))
          if (svgPageNum > 1) { gTop += ((svgPageNum - 1) * svgHeight) }
          const cTop = this.comments[this.order].topPosition
          // parseInt(commentCards[this.order].style.top.substring(0, commentCards[this.order].style.top.length - 2));
          // const cmtSelected = document.querySelector(`div[highlight-id='${target.getAttribute('data-pdf-annotate-id')}']`)
          // const cTop = cmtSelected.getAttribute('top-position')

          // remove current selected comment card class if any
          const selected = document.getElementsByClassName('comment-card-selected')
          if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
          // Set this card as selected
          // console.log('this.order', this.order)
          commentCards[this.order].classList.add('comment-card-selected')

          // cmtSelected.classList.add('comment-card-selected')
          // this.updateCommentCardPosition(this.annotationClicked.getAttribute('data-pdf-annotate-id'))
          if (cTop != gTop) {
            // Move the comment up to gTop
            this.comments[this.order].topPosition = gTop
            // cmtSelected.setAttribute('top-position', gTop)
            // cmtSelected.style.top = gTop + 'px'

            const selected = document.getElementsByClassName('comment-card-selected')
            if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
            commentCards[this.order].classList.add('comment-card-selected')

            // cmtSelected.classList.add('comment-card-selected')
            const endPos = gTop + commentCards[this.order].offsetHeight

            // this.updateCommentCardPosition(this.annotationClicked.getAttribute('data-pdf-annotate-id'))
            // const endPos = gTop + cmtSelected.offsetHeight
            if (cTop > gTop) {
              // Move up other uppen comments
              if (this.order > 0) { this.moveUpFromTopPos(commentCards, this.order - 1, gTop) }
              // Move up other lower comments
              if (this.order < commentCards.length - 1) { this.moveUpToEndPos(commentCards, this.order + 1, endPos) }
            } else if (cTop <= gTop) {
              // Move down other uppen comments
              if (this.order > 0) { this.moveDownToTopPos(commentCards, this.order - 1, gTop) }
              // Move down other lower comments
              if (this.order < commentCards.length - 1) { this.moveDownFromEndPos(commentCards, this.order + 1, endPos) }
            }
          }
        } else if (type == 'area' || type == 'text') {
          this.colorChosen = target.getAttribute('stroke')
          var uuid = target.getAttribute('data-pdf-annotate-id')
          this.annotation = this.loadedAnnotation.annotations.filter(r => { return r.uuid === uuid })[0]
          rect = target.getBoundingClientRect()
          // Display rect tool bar if text is selected.
          if (target.getAttribute('x') == '') {
            // window.getSelection().removeAllRanges()
            this.hideRectToolBar()
            this.hideDeleteToolBar()
          } else {
            const rectTool = document.getElementById('rectTool')
            var posX = (parseInt(target.getAttribute('x')) + parseInt(target.getAttribute('width')) / 4) * this.RENDER_OPTIONS.scale
            if (this.showQuestion) {
              posX += parseInt(document.getElementById('left-panel').offsetWidth)
            }
            const posY = parseInt(rect.top) + parseInt(target.getAttribute('height')) * this.RENDER_OPTIONS.scale + 10

            rectTool.style = 'position: absolute; top: ' + posY + 'px; left: ' + posX + 'px;'
          }
        } else if (type == 'textbox') {
          this.colorChosen = target.getAttribute('stroke')
          uuid = target.getAttribute('data-pdf-annotate-id')
          this.annotation = this.loadedAnnotation.annotations.filter(r => { return r.uuid === uuid })[0]
          rect = target.getBoundingClientRect()
          // Display rect tool bar if text is selected.
          if (target.getAttribute('x') == '') {
            // window.getSelection().removeAllRanges()
            this.hideRectToolBar()
            this.hideDeleteToolBar()
          } else {
            const rectTool = document.getElementById('rectTool')
            posX = (parseInt(target.getAttribute('x')) + parseInt(target.getAttribute('width')) / 4) * this.RENDER_OPTIONS.scale
            if (this.showQuestion) {
              posX += parseInt(document.getElementById('left-panel').offsetWidth)
            }
            const posY = parseInt(rect.top) + parseInt(target.getAttribute('height')) * this.RENDER_OPTIONS.scale + 10
            rectTool.style = 'position: absolute; top: ' + posY + 'px; left: ' + posX + 'px;'
          }
        }
      }
    },
    changeColor(e) {
      this.colorChosen = e
      this.updateRectangleAnotation(e)
      this.hideColorPickerTool()
    },
    showColorPickerTool(type) {
      this.hideRectToolBar()
      this.hideDeleteToolBar()
      if (type == 'area') {
        const boundingRect = this.annotationClicked.getBoundingClientRect()
        // Display rect tool bar if text is selected.
        const colorPicker = document.getElementById('colorPickerTool')
        const posX = parseInt(boundingRect.left)
        const posY = parseInt(boundingRect.top) + parseInt(boundingRect.height) + 10
        colorPicker.style.top = posY + 'px'
        colorPicker.style.left = posX + 'px'
        colorPicker.style.visibility = 'unset'
      }
    },
    hideColorPickerTool() {
      const colorPicker = document.getElementById('colorPickerTool')
      colorPicker.style.visibility = 'hidden'
    },
    async updateRectangleAnotation(e) {
      var anno
      this.annotationClicked.setAttribute('stroke', e)
      var uuid = this.annotationClicked.getAttribute('data-pdf-annotate-id')
      var type = this.annotationClicked.getAttribute('data-pdf-annotate-type')
      if (type == 'textbox') {
        this.annotationClicked.childNodes[0].style.color = e
      }
      this.annotation.color = e
      await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, uuid, this.annotation)
      anno = {
        Id: this.annotation.id,
        DocumentId: this.documentId,
        ReviewId: this.reviewId,
        Type: this.annotation.type,
        Color: this.annotation.color,
        Uuid: this.annotation.uuid,
        PageNum: this.annotation.pageNum,
        Top: this.annotation.top,
        Data: JSON.stringify(this.annotation)
      }

      reviewService.editAnnotation(anno).then(rs => {
      })
    },
    hideRectToolBar() {
      const rectTool = document.getElementById('rectTool')
      rectTool.style.visibility = 'hidden'
    },
    hideDeleteToolBar() {
      const deleteTool = document.getElementById('deleteTool')
      deleteTool.style.visibility = 'hidden'
    },
    handleCommentCardClick(el) {
      const highlightId = el.getAttribute('highlight-id')
      const highlight = document.querySelector("[data-pdf-annotate-id='" + highlightId + "']")
      this.handleCommentAnnotationClick(highlight)
      this.hideDeleteToolBar()
    },
    async handleCommentPositionsRestore() {
      const documentId = document.getElementById('viewer').getAttribute('document-id')
      const highlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const highlightArr = Array.prototype.slice.call(highlights)
      // Sort the highlights
      highlightArr.sort(this.compareTopAttributes)
      highlightArr.forEach(cmt => {
        this.handleCommentAnnotationClick(cmt)
      })
      this.hideDeleteToolBar()
      this.handleCommentAnnotationClick(highlightArr[0])
      // this.handleCommentAnnotationClick(highlightArr[highlightArr.length - 1])
      // this.handleCommentAnnotationClick(highlightArr[0])
      const selected = document.getElementsByClassName('comment-card-selected')
      if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
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
      this.hideTextToolBar()
      this.hideTextToolGroup()
      this.$refs.toolBar.updateActiveButton('')
    },
    // Migration code starts here
    async CommentText() {
      if (document.getElementById('comment-wrapper').offsetWidth == 0) {
        document.getElementById('comment-wrapper').style.display = 'block'
      }
      if (this.newComment.replace(/\s/g, '').length != 0) {
        // Save in-progress comment
        await this.addCommentText()
      }
      await this.commentText()
    },
    async addCommentText(e) {
      // let newComment = await UI.addCommentText(this.newComment, this.RENDER_OPTIONS.documentId);
      const commentWrapper = document.getElementById('add-new-comment')
      const topPos = parseInt(commentWrapper.style.top.substring(0, commentWrapper.style.top.length - 2))
      const newComment = await PDFJSAnnotate.getStoreAdapter().addComment(
        this.documentId,
        this.annotation,
        this.newComment,
        this.selectedText,
        topPos)
      this.undoHistory.push({ action: 'added', annotation: newComment })
      this.updateUndoList()
      await this.comments.push(newComment)
      await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      document.getElementById('add-new-comment').style.display = 'none'
      newComment.annotation.documentId = this.documentId
      var anno = {
        DocumentId: this.documentId,
        ReviewId: this.reviewId,
        Type: this.annotation.type,
        PageNum: typeof (this.annotation.pageNum) != 'undefined' ? this.annotation.pageNum : this.annotation.page,
        Top: typeof (this.annotation.top) != 'undefined' ? this.annotation.top : parseInt(this.annotation.y),
        Color: this.annotation.color,
        Uuid: this.annotation.uuid,
        Data: JSON.stringify(this.annotation)
      }
      var newCmt = {
        Text: newComment.text != null ? newComment.text : '',
        Content: newComment.content,
        TopPosition: parseInt(newComment.topPosition),
        Uuid: newComment.uuid,
        Data: JSON.stringify(newComment)
      }
      reviewService.addInTextComment(this.documentId, 1, newCmt, anno).then(async rs => {
        if (rs) {
          var cmts = null
          await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(r => {
            cmts = r
          })
          cmts[cmts.length - 1].id = rs.data['id']
          PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)
        }
        this.$refs.toolBar.toolBarButtonClick('cursor')
      })
      await this.updatePositionsAfterCommentAdded()

      this.newComment = ''
    },
    // Begin migrating code from select.js to document.vue
    async commentText(note) {
      // Highlight the text first
      if (typeof (note) == 'undefined') {
        await this.highlightText('comment-highlight')
      }
      this.target = this.getHighlightByCommentId(this.annotation.uuid)
      // Update positions of comment boxes
      this.updateCommentPositions(note)
      // Display add new form
      this.displayAddNewCommentForm(note)
      event.stopPropagation()
    },
    async highlightEvent(type) {
      if (type == 'comment-highlight') {
        await this.CommentText()
      } else {
        await this.highlightText('comment-highlight')
      }
    },
    async highlightText(type) {
      if (!this.svg) {
        return
      }
      const boundingRect = this.boundingRect

      var color = 'FFFF00'
      if (type == 'strikeout') { color = localStorage.getItem('colorChosen') ? color = localStorage.getItem('colorChosen') : 'FF0000' }
      if (type == 'highlight') {
        color = localStorage.getItem('colorChosen') ? color = localStorage.getItem('colorChosen') : 'FFFF00'
      }
      if (type == 'comment-highlight') {
        color = localStorage.getItem('colorChosen') ? color = localStorage.getItem('colorChosen') : 'FF00FF'
      }

      // Initialize the annotation
      const annotation = {
        type,
        color,
        rectangles: [...this.rects].map((r) => {
          let offset = 0
          if (type == 'strikeout') { offset = r.height / 2 }
          return scaleDown(this.svg, {
            y: r.top + offset - boundingRect.top,
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
      // format top with scale 100%
      annotation.color = color
      annotation.top = parseInt(this.target.style.top.substring(0, this.target.style.top.length - 2)) / this.RENDER_OPTIONS.scale
      annotation.pageNum = pageNumber
      annotation.pageHeight = parseInt(this.svg.getAttribute('height') / this.RENDER_OPTIONS.scale)
      this.annotation = annotation
      // Add the annotation
      await PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
        .then((annotation) => {
          appendChild(this.svg, annotation)
          this.undoHistory.push({ action: 'added', annotation: annotation })
          this.updateUndoList()
          this.hideTextToolBar()
          this.hideTextToolGroup()
          this.hideRectToolBar()
          this.hideDeleteToolBar()
          this.hideColorPickerTool()
          this.$refs.toolBar.updateActiveButton('')
          annotation.documentId = this.documentId
          if (type != 'comment-highlight') {
            var obj = {
              DocumentId: this.documentId,
              ReviewId: this.reviewId,
              Type: type,
              PageNum: annotation.pageNum,
              Top: annotation.top,
              Color: annotation.color,
              Uuid: annotation.uuid,
              Data: JSON.stringify(annotation)
            }
            this.$store.dispatch('review/addReviewAnnotation', obj).then(async rs => {
              if (this.$store.getters['review/getAddedAnnotation']) {
                annotation.id = this.$store.getters['review/getAddedAnnotation']['id']

                await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, annotation.uuid, annotation)
              }
            })
          }
          this.setStatusText()
        })
    },
    updateCommentPositions(note) {
      const commentCards = document.querySelectorAll('.comment-card')
      const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const commentPoints = document.querySelectorAll("svg[data-pdf-annotate-type='point']")
      const highlightArr = Array.prototype.slice.call(commentHighlights)
      var pointsArr = Array.prototype.slice.call(commentPoints)
      // pointsArr.forEach(r => {
      //   r.setAttribute('top', parseInt(r.getAttribute('y')))
      // })
      // Sort the highlights
      // highlightArr.sort(this.compareTopAttributes)
      // pointsArr.sort(this.compareTopAttributes)
      const combineArr = highlightArr.concat(pointsArr)
      combineArr.sort(this.compareTopAttributes)
      // Get order of the current highlight

      this.order = combineArr.findIndex(item => {
        if (this.annotation.uuid) {
          return item.dataset.pdfAnnotateId == this.annotation.uuid
        } else {
          return item.dataset.pdfAnnotateId == this.target.getAttribute('data-pdf-annotate-id')
        }
      })
      this.hasSpace = this.hasEnoughSpace(commentCards, note)
      if (!this.hasSpace) {
        // Update comment positions

        // get add new form's top position
        const svg = this.svg
        var rectTop = 0
        if (typeof (note) == 'undefined') {
          rectTop = parseInt(this.target.getAttribute('top')) * this.RENDER_OPTIONS.scale
        } else {
          rectTop = parseInt(note.y) * this.RENDER_OPTIONS.scale
        }
        // const rectTop = parseInt(this.target.style.top.substring(0, this.target.style.top.length - 2))
        const svgHeight = svg.getAttribute('height')
        const svgPageNum = svg.getAttribute('data-pdf-annotate-page')
        var svgTop = 0
        if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
        const topPos = parseInt(svgTop) + rectTop - 35
        var endPos = topPos
        var cmtSelected = document.querySelector('comment-card-selected')
        if (cmtSelected) {
          endPos += cmtSelected.offsetHeight
        } else {
          endPos += 126
        }

        if (this.order == 0) {
          // Move down all comment cards
          this.moveDownFromEndPos(commentCards, 0, endPos)
        } else if (this.order == combineArr.length - 1) {
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
            if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
            if (upperTop < gTop) {
              this.moveDownToTopPos(commentCards, this.order - 1, topPos)
            }
          }
          // Move down commentCards[i] -> commentCards[commentCards.length - 1]
          var lowerTop = 0
          if (typeof (note) == 'undefined') {
            lowerTop = parseInt(commentCards[this.order].style.top.substring(0, commentCards[this.order].style.top.length - 2))
          } else {
            lowerTop = parseInt(note.y)
          }
          // const lowerTop = parseInt(commentCards[this.order].style.top.substring(0, commentCards[this.order].style.top.length - 2))
          if (lowerTop < (endPos + 10)) {
            this.moveDownFromEndPos(commentCards, this.order, endPos)
          } else {
            // Check if move up is necessary
            const commentId = commentCards[this.order].getAttribute('highlight-id')
            const g = this.getHighlightByCommentId(commentId)
            let gTop = parseInt(g.getAttribute('top')) - 35
            if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
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
    async updateCommentCardPosition(uuid) {
      const newComment = document.querySelector(".comment-card[highlight-id='" + uuid + "']")

      if (!this.hasSpace) {
        const commentCards = document.querySelectorAll('.comment-card')
        const endPos = parseInt(newComment.getAttribute('top-position')) + newComment.offsetHeight
        if (this.order < (commentCards.length - 1)) { this.moveUpToEndPos(commentCards, this.order + 1, endPos) }
      }
    },
    displayAddNewCommentForm(note) {
      const newCommentWrapper = document.getElementById('add-new-comment')
      newCommentWrapper.style.visibility = 'visible'
      var rectTop = 0
      if (note) {
        rectTop = parseInt(note.y) * this.RENDER_OPTIONS.scale
      } else {
        rectTop = parseInt(this.target.getAttribute('top')) * this.RENDER_OPTIONS.scale
      }
      const svg = this.svg
      const svgHeight = svg.getAttribute('height')
      const svgPageNum = svg.getAttribute('data-pdf-annotate-page')
      let svgTop = 0
      if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
      var topPos = svgTop + rectTop - 35
      if (note) {
        topPos = svgTop + rectTop - 35
      }
      newCommentWrapper.style = 'width: 100%; position: absolute; left: -20px; top: ' + topPos + 'px;'
      this.hideTextToolBar()
      this.hideTextToolGroup()
      this.$refs.toolBar.updateActiveButton('')
      this.hideRectToolBar()
      this.hideDeleteToolBar()
      this.hideColorPickerTool()
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
    cancelCommentText(e) {
      if (e == 'edit') {
        this.comments.forEach(function(element) {
          element.isSelected = false
        })
        this.updateCommentPositions()
        return
      }
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
      PDFJSAnnotate.getStoreAdapter().deleteAnnotation(documentId, this.annotation.uuid, true)
        .then((isDeleted) => {
          if (isDeleted) {
            // remove highlight in UI
            document.querySelector("[data-pdf-annotate-id='" + this.annotation.uuid + "']").remove()
            if (document.querySelector("[data-target-id='" + this.annotation.uuid + "']")) {
              document.querySelector("[data-target-id='" + this.annotation.uuid + "']").remove()
            }
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
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
      // Get start index card's position
      const adjTop = this.comments[startIndex].topPosition
      const adjEnd = adjTop + commentCards[startIndex].offsetHeight
      // Move the start index card up first
      if ((adjTop - (adjEnd - topPos) - 10) > gTop) {
        this.comments[startIndex].topPosition = gTop
      } else {
        this.comments[startIndex].topPosition = adjTop - (adjEnd - topPos) - 10
      }
      if (startIndex > 0) {
        for (let m = startIndex - 1; m >= 0; m--) {
          const previousTop = this.comments[m + 1].topPosition
          const thisTop = this.comments[m].topPosition
          const thisEnd = thisTop + commentCards[m].offsetHeight
          const thisG = this.getHighlightByCommentId(this.comments[m].uuid)
          var thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35
          if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
          if ((thisTop - (thisEnd - previousTop) - 10) > thisGTop) {
            this.comments[m].topPosition = thisGTop
          } else {
            if (thisEnd > (previousTop - 10)) {
              this.comments[m].topPosition = thisTop - (thisEnd - previousTop) - 10
            } else {
              break
            }
          }
        }
      }
    },
    moveUpToEndPos(commentCards, startIndex, endPos) {
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }

      if ((endPos + 10) < gTop) {
        this.comments[startIndex].topPosition = gTop
      } else {
        const desiredTop = (endPos + 10) + gTop * 0
        // Move start index card up
        this.comments[startIndex].topPosition = desiredTop
      }

      if (commentCards.length > (startIndex + 1)) {
        for (let n = startIndex + 1; n < this.comments.length; n++) {
          const previousTop = this.comments[n - 1].topPosition
          const previousEnd = previousTop + commentCards[n - 1].offsetHeight
          const thisTop = this.comments[n].topPosition
          const thisG = this.getHighlightByCommentId(this.comments[n].uuid)
          var thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35

          // Check if move down is necessary
          if (thisTop > (previousEnd + 10)) {
            if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
            if (thisGTop > (previousEnd + 10)) {
              this.comments[n].topPosition = thisGTop
            } else {
              this.comments[n].topPosition = previousEnd + 10
            }
            // const thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10)
          } else {
            this.comments[n].topPosition = previousEnd + 10
            // break
          }
        }
      }
    },
    moveDownToTopPos(commentCards, startIndex, topPos) {
      // Get start index card's position
      // Check if move down is necessary
      // Only move cards down if the upper card is higher that its highlight
      const desiredTop = topPos - 10 - commentCards[startIndex].offsetHeight
      // Move upper adjacent card down
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
      if ((topPos - 10 - commentCards[startIndex].offsetHeight) > gTop) {
        this.comments[startIndex].topPosition = gTop
      } else {
        this.comments[startIndex].topPosition = desiredTop
      }
      // Move other cards down
      if (startIndex > 0) {
        for (let n = startIndex - 1; n >= 0; n--) {
          const previousTop = this.comments[n + 1].topPosition
          const thisG = this.getHighlightByCommentId(this.comments[n].uuid)
          var thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35
          if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
          // const thisTop = this.comments[n].topPosition
          // const thisEnd = thisTop + commentCards[n].offsetHeight
          // Check if move down is necessary
          if ((previousTop - 10 - commentCards[startIndex].offsetHeight) > thisGTop) {
            this.comments[n].topPosition = thisGTop
          } else {
            const thisDesiredTop = previousTop - commentCards[n].offsetHeight - 10
            this.comments[n].topPosition = thisDesiredTop
          }
        }
      }
      // }
    },
    moveDownFromEndPos(commentCards, startIndex, endPos) {
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
      if ((endPos + 10) < gTop) {
        this.comments[startIndex].topPosition = gTop
      } else {
        const desiredTop = (endPos + 10) + gTop * 0
        // Move start index card up
        this.comments[startIndex].topPosition = desiredTop
      }

      // // Move the start index card down first
      // if (typeof (this.comments[startIndex]) == 'undefined') {
      //   this.comments[this.comments.length - 1].topPosition = endPos + 10
      // } else {
      //   this.comments[startIndex].topPosition = endPos + 10
      // }
      // Move other cards down ward
      if (this.comments.length > (startIndex + 1)) {
        for (let j = startIndex + 1; j < this.comments.length; j++) {
          // const previousTop = this.comments[j - 1].topPosition
          // const previousEnd = previousTop + commentCards[j - 1].offsetHeight
          // const thisTop = this.comments[j].topPosition
          // if (thisTop < (previousEnd + 10)) {
          //   this.comments[j].topPosition = thisTop + (previousEnd - thisTop) + 10
          // } else {
          //   break
          // }
          const previousTop = this.comments[j - 1].topPosition
          const previousEnd = previousTop + commentCards[j - 1].offsetHeight
          const thisTop = this.comments[j].topPosition
          const thisG = this.getHighlightByCommentId(this.comments[j].uuid)
          var thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35

          // Check if move down is necessary
          if (thisTop > (previousEnd + 10)) {
            if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * g.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
            if (thisGTop > (previousEnd + 10)) {
              this.comments[j].topPosition = thisGTop
            } else {
              this.comments[j].topPosition = previousEnd + 10
            }
            // const thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10)
          } else {
            this.comments[j].topPosition = previousEnd + 10
            // break
          }
        }
      }
    },
    hasEnoughSpace(commentCards, note) {
      if (this.comments.length > 0) {
        const svg = this.svg
        var rectTop = 0
        if (typeof (note) == 'undefined') {
          rectTop = parseInt(this.target.getAttribute('top'))
        } else {
          rectTop = parseInt(note.y)
        }
        const svgHeight = svg.getAttribute('height')
        const svgPageNum = svg.getAttribute('data-pdf-annotate-page')

        let svgTop = 0
        if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
        const topPos = svgTop + rectTop * this.RENDER_OPTIONS.scale - 35
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
      if (a.getAttribute('page-num') > 1) { top1 += ((a.getAttribute('page-num') - 1) * a.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }

      let top2 = parseInt(b.getAttribute('top'))
      if (b.getAttribute('page-num') > 1) { top2 += ((b.getAttribute('page-num') - 1) * b.getAttribute('page-height') * this.RENDER_OPTIONS.scale) }
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
      this.preEditedAnno = Object.assign({}, comment)
      const highlight = document.querySelector("[data-pdf-annotate-id='" + comment.uuid + "']")
      this.svg = document.querySelector(`svg[data-pdf-annotate-page='${comment.annotation.page}']`)
      this.handleCommentAnnotationClick(highlight)
      comment.isSelected = true
      this.$forceUpdate()
      const refName = 'comment' + comment.uuid
      this.$refs[refName][0].$el.childNodes[0].focus()
      this.target = highlight
      this.updateCommentPositions()
    },
    updateCommentPositionWhileEditing() {
      const editCmt = document.querySelector('.comment-card-selected')
      const commentCards = document.querySelectorAll('.comment-card')
      const endPos = editCmt.offsetTop + editCmt.offsetHeight
      if (this.order < commentCards.length - 1) { this.moveDownFromEndPos(commentCards, this.order + 1, endPos) }
    },
    async editCommentCard(comment, isUndoRedo) {
      var editComment = {
        Text: comment.text ? comment.text : '',
        Content: comment.content,
        Id: this.preEditedAnno.id,
        Uuid: comment.uuid,
        TopPosition: comment.topPosition,
        Data: JSON.stringify(comment)
      }
      await PDFJSAnnotate.getStoreAdapter().getAnnotation(this.documentId, comment.uuid).then(r => {
        editComment.AnnotationId = r.id
      })
      reviewService.editComment(editComment).then(async rs => {
        if (rs) {
          // this.addToUndoList(rs.data, 'edit')

          this.comments.forEach(function(element) {
            element.isSelected = false
          })
          this.$forceUpdate()

          var cmts = null
          await window.PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(r => {
            cmts = r
            if (typeof (isUndoRedo) == 'undefined') {
              this.undoHistory.push({ action: 'edited', annotation: this.preEditedAnno })
            }
          })
          cmts.map(r => {
            if (r.id == comment.id) {
              r.content = comment.content
            }
          })
          PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)
        }
      })
    },
    async deleteCommentCard(uuid) {
      await PDFJSAnnotate.getStoreAdapter().deleteComment(this.documentId, uuid)
      this.comments = await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId)
      await this.comments.forEach(function(element) {
        element.isSelected = false
      })
      await this.handleCommentPositionsRestore()
    },
    hideTextToolBar() {
      const textTool = document.getElementById('textTool')
      textTool.style.display = 'none'
    },
    hideTextToolGroup() {
      const textTool = document.getElementById('textToolGroup')
      textTool.style.visibility = 'hidden'
    },
    getHighlightByCommentId(commentId) {
      return document.querySelector("[data-pdf-annotate-id='" + commentId + "']")
    },
    getSelectionRects() {
      const selection = window.getSelection()
      console.log('selection', selection)
      var range
      var rects
      if (selection.anchorNode) {
        range = selection.getRangeAt(0)
        rects = range.getClientRects()
      }
      if (rects) {
        if (rects.length > 0 &&
          rects[0].width > 0 &&
          rects[0].height > 0) {
          return rects
        }
      }
      return null
    },
    hasAParentWithClass(element, className) {
      if (typeof (element.classList) != 'undefined' && element.classList.length != 0) {
        for (let i = 0; i <= 5; i++) {
          if (element.classList.contains(className)) { return true } else { element = element.parentNode }
        }
      }
      return false
    },
    async submitReview() {
      this.rubricCriteria = this.$refs.tabRubric.getRubricData()
      var reviewData = []
      this.rubricCriteria.forEach(r => {
        if (r.mark) {
          reviewData.push({
            Comment: r.comment,
            CriteriaId: r.id,
            Score: r.mark,
            ReviewId: this.reviewId
          })
        }
      })
      reviewService.saveReviewFeedback(reviewData).then(rs => {
        console.log('review data: ', rs)
        this.$refs.tabRubric.disableRubric()
        this.disableToolbarSubmit()
      })
      reviewService.loadReviewFeedback(this.reviewId).then(rs => {
        console.log('review data: ', rs)
        localStorage.removeItem('reviewComment')
      })
    },
    calculateContainerHeight() {
      const headerHeight = document.getElementById('header').clientHeight
      const containerHeight = window.innerHeight - headerHeight
      const elContainer = document.getElementById('reviewContainer')

      elContainer.style.height = containerHeight + 'px'
      const rightPanel = document.getElementById('right-panel')
      const viewerContainer = document.getElementById('viewerContainer')
      viewerContainer.style.height = rightPanel.offsetHeight - document.getElementById('tool-bar').offsetHeight - 5 + 'px'

      if (this.showQuestion) {
        rightPanel.style.width = window.innerWidth - document.getElementById('left-panel').offsetWidth - 10 + 'px'
      } else {
        rightPanel.style.width = window.innerWidth - 10 + 'px'
      }
    },
    calculateStylePaddingScroll() {
      this.$refs.tabQuestion.calculateStylePaddingScroll()
    },
    setStatusText() {
      this.$refs.toolBar.setStatusText()
    },

    async hideQuestion(e) {
      this.showQuestion = e
      await this.handleCommentPositionsRestore()
      this.calculateContainerHeight()
    },
    async documentWidthCal() {
      var docWidth = document.getElementById('pageContainer1').offsetWidth
      if (this.comments.length == 0) {
        docWidth = document.getElementById('right-panel').offsetWidth - 20
        document.getElementById('comment-wrapper').style.display = 'none'
        if (this.scaleText == 'Fit to width') {
          this.RENDER_OPTIONS.scale = docWidth / 612
          this.fitDocumentWidth = true
        } else if (this.scaleText == 'Fit to page') {
          var docHeight = window.innerHeight - 70
          if ((docWidth / 612) < (docHeight / 792)) {
            this.RENDER_OPTIONS.scale = (docWidth / 612)
          } else {
            this.RENDER_OPTIONS.scale = (docHeight / 792)
          }
          this.scaleText = 'Fit to page'
          this.fitDocumentWidth = true
        }
      }
      this.calculateContainerHeight()
    },
    async handleScale(e) {
      this.RENDER_OPTIONS.scale = e
      this.reRenderPages()
    },

    async reRenderPages() {
      await UI.renderAllPages(this.NUM_PAGES, this.RENDER_OPTIONS)
      this.documentWidthCal()
    },
    expandColorPickerToggle(e) {
      this.expandColorPicker = e
      this.$refs.toolBar.expandColor(e)
    },
    async undoAnnotation() {
      this.isUndo = true
      var history = this.undoHistory.pop()
      if (!history) {
        return
      }
      const undoAnno = history.annotation
      if (history.action == 'deleted') {
        if (undoAnno.class == 'Comment') {
          if (undoAnno.annotation.type == 'point') {
            undoAnno.text = ','
          }
          await PDFJSAnnotate.getStoreAdapter().addComment(undoAnno.annotation.documentId,
            undoAnno.annotation,
            undoAnno,
            undoAnno.text,
            undoAnno.topPosition)
          await this.comments.push(undoAnno)
          await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
          document.getElementById('add-new-comment').style.display = 'none'
          var anno = {
            DocumentId: this.documentId,
            ReviewId: this.reviewId,
            Type: 'comment-highlight',
            PageNum: undoAnno.annotation.pageNum,
            Top: undoAnno.annotation.top,
            Color: undoAnno.annotation.color,
            Uuid: undoAnno.annotation.uuid,
            Data: JSON.stringify(undoAnno.annotation)
          }
          var newCmt = {
            Text: undoAnno.text,
            Content: undoAnno.content,
            TopPosition: undoAnno.topPosition,
            Uuid: undoAnno.uuid,
            Data: JSON.stringify(undoAnno)
          }
          reviewService.addInTextComment(this.documentId, this.reviewId, newCmt, anno).then(rs => {
            this.updateCommentId(rs)
          })
          this.handleCommentPositionsRestore()
          this.svg = document.querySelector(`svg[data-pdf-annotate-page='${undoAnno.annotation.page}']`)
          appendChild(this.svg, undoAnno.annotation)
          this.newComment = ''
          this.updatePositionsAfterCommentAdded()
        } else {
          await PDFJSAnnotate.getStoreAdapter().addAnnotation(this.documentId, undoAnno.page, undoAnno, true)
            .then(async(annotation) => {
              if (undoAnno.type != 'comment-highlight') {
                var obj = {
                  DocumentId: this.documentId,
                  ReviewId: this.reviewId,
                  Type: undoAnno.type,
                  PageNum: undoAnno.page,
                  Top: undoAnno.top,
                  Color: undoAnno.color,
                  Uuid: undoAnno.uuid,
                  Data: JSON.stringify(undoAnno)
                }
                this.$store.dispatch('review/addReviewAnnotation', obj).then(rs => {
                  var temp = this.$store.getters['review/getAddedAnnotation']
                  undoAnno.id = temp.id
                  this.svg = document.querySelector(`svg[data-pdf-annotate-page='${undoAnno.page}']`)
                  appendChild(this.svg, undoAnno)
                })
              }
            })
        }
        history.action = 'added'
      } else if (history.action == 'added') {
        history.action = 'deleted'
        if (undoAnno.annotation) {
          this.annotationDelete = history
          await this.deleteButtonClicked(undoAnno, true)
          this.removeElementById(undoAnno.uuid)
          return
        } else {
          await PDFJSAnnotate.getStoreAdapter().deleteAnnotation(this.documentId, undoAnno.uuid, true)
          reviewService.deleteAnnotation(undoAnno.id).then(rs => {
            this.removeElementById(undoAnno.uuid)
          })
        }
      } else if (history.action == 'edited') {
        if (undoAnno.annotation) {
          await this.editCommentCard(undoAnno, 'undo')
        }
      }
      this.redoHistory.push(history)
      this.updateRedoList()
      this.updateUndoList()
    },
    async redo() {
      this.isUndo = false
      var history = this.redoHistory.pop()
      if (history) {
        var redoAnno = history.annotation
        if (history.action == 'deleted') {
          if (redoAnno.class == 'Comment') {
            if (redoAnno.annotation.type == 'point') {
              redoAnno.text = ','
            }
            await PDFJSAnnotate.getStoreAdapter().addComment(redoAnno.annotation.documentId,
              redoAnno.annotation,
              redoAnno,
              redoAnno.text,
              redoAnno.topPosition)
            await this.comments.push(redoAnno)
            await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
            document.getElementById('add-new-comment').style.display = 'none'
            var anno = {
              DocumentId: this.documentId,
              ReviewId: this.reviewId,
              Type: 'comment-highlight',
              PageNum: redoAnno.annotation.pageNum,
              Top: redoAnno.annotation.top,
              Color: redoAnno.annotation.color,
              Uuid: redoAnno.annotation.uuid,
              Data: JSON.stringify(redoAnno.annotation)
            }
            var newCmt = {
              Text: redoAnno.text,
              Content: redoAnno.content,
              TopPosition: redoAnno.topPosition,
              Uuid: redoAnno.uuid,
              Data: JSON.stringify(redoAnno)
            }
            reviewService.addInTextComment(this.documentId, this.reviewId, newCmt, anno).then(rs => {
              this.updateCommentId(rs)
            })
            this.handleCommentPositionsRestore()
            this.svg = document.querySelector(`svg[data-pdf-annotate-page='${redoAnno.annotation.page}']`)
            appendChild(this.svg, redoAnno.annotation)
            this.newComment = ''
            this.updatePositionsAfterCommentAdded()
          } else {
            await PDFJSAnnotate.getStoreAdapter().addAnnotation(this.documentId, redoAnno.page, redoAnno, true)
              .then(async(annotation) => {
                if (redoAnno.type != 'comment-highlight') {
                  var obj = {
                    DocumentId: this.documentId,
                    ReviewId: this.reviewId,
                    Type: redoAnno.type,
                    PageNum: redoAnno.page,
                    Top: redoAnno.top,
                    Color: redoAnno.color,
                    Uuid: redoAnno.uuid,
                    Data: JSON.stringify(redoAnno)
                  }
                  this.$store.dispatch('review/addReviewAnnotation', obj).then(rs => {
                    var temp = this.$store.getters['review/getAddedAnnotation']
                    redoAnno.id = temp.id
                    this.svg = document.querySelector(`svg[data-pdf-annotate-page='${redoAnno.page}']`)
                    appendChild(this.svg, redoAnno)
                  })
                }
              })
          }
          history.action = 'added'
        } else if (history.action == 'added') {
          history.action = 'deleted'
          if (redoAnno.annotation) {
            await this.deleteButtonClicked(redoAnno)
            this.removeElementById(redoAnno.uuid)
            return
          } else {
            await PDFJSAnnotate.getStoreAdapter().deleteAnnotation(this.documentId, redoAnno.uuid, true)
            reviewService.deleteAnnotation(redoAnno.id).then(rs => {
              this.removeElementById(redoAnno.uuid)
            })
          }
        } else if (history.action == 'edited') {
          if (redoAnno.annotation) {
            await this.editCommentCard(redoAnno, 'redo')
          }
        }
      }
      this.undoHistory.push(history)
      this.updateUndoList()
      this.updateRedoList()
    },
    deleteAnnotation() {
      deleteAnnotations()
      this.undoHistory.push({ action: 'deleted', annotation: this.target })
      this.updateUndoList()
      this.hideRectToolBar()
      this.hideDeleteToolBar()
    },
    removeElementById(elementId) {
      const el = document.querySelector(`[data-pdf-annotate-id='${elementId}']`)
      if (el) {
        el.parentNode.removeChild(el)
      }
    },
    updateActiveButton(e) {
      this.$refs.toolBar.updateActiveButton(e)
    },
    async deleteButtonClicked(e) {
      this.$confirm('Are you sure you want to delete this comment ?').then(() => {
        this.deleteCommentCard(e.uuid)
        this.removeElementById(e.annotation.uuid)
      }).catch(() => {
        this.svg = document.querySelector(`svg[data-pdf-annotate-page='${e.annotation.page}']`)
        appendChild(this.svg, e.annotation)
      })
    },
    keyupHandler(event) {
      if (event.ctrlKey && event.code === 'KeyZ' && this.undoHistory.length > 0) {
        this.undoAnnotation()
      } else if (event.ctrlKey && event.code === 'KeyY' && this.redoHistory.length > 0) {
        this.redo()
      }
    },
    insertNoteComment(svg, note) {
      if (document.getElementById('comment-wrapper').offsetWidth == 0) {
        document.getElementById('comment-wrapper').style.display = 'block'
      }
      this.annotation = note
      this.svg = svg
      this.commentText(note)
    },
    async updateCommentId(rs) {
      var cmts = null
      await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(r => {
        cmts = r
      })
      cmts[cmts.length - 1].id = rs.data['id']
      PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)
    },
    updateRedoList() {
      this.$refs.toolBar.updateRedoList(this.redoHistory)
    },
    updateUndoList() {
      this.$refs.toolBar.updateUndoList(this.undoHistory)
    },
    disableToolbarSubmit() {
      this.$refs.toolBar.disableSubmit()
    }
    // End migration
  }
}
</script>

<style scoped>
@import '../../pdfjs/shared/document.css';
@import '../../pdfjs/shared/toolbar.css';
@import '../../pdfjs/shared/pdf_viewer.css';
@import '../../styles/review.css';
</style>
<style>
#mileStone{
  width:100%;
}
#mileStone .el-radio-button__inner {
  width: 100%;
}
button:focus{
 outline: none !important;
}
</style>
