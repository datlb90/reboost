<template>
  <div id="reviewContainer" :style="{cursor: activeButton== 'text'||activeButton== 'rectangle' || activeButton== 'point' ? 'crosshair':''}" style="border-top: 1px solid rgb(223 224 238); padding: 5px; background: rgb(248, 249, 250);">

    <div id="content-wrapper" style="background: rgb(248, 249, 250); height: inherit; width: 100%;position: absolute; overflow: unset;">
      <div id="left-panel" :class="{'hideQuestion': !showQuestion}">
        <el-tabs type="border-card">
          <el-tab-pane label="Question">
            <tabQuestion ref="tabQuestion" :questionid="questionId" :reviewid="reviewId" />
          </el-tab-pane>
          <el-tab-pane label="Rubric">
            <tabRubric ref="tabRubric" :current-user="currentUser" :questionid="questionId" :reviewid="reviewId" @setStatusText="setStatusText" />
          </el-tab-pane>
        </el-tabs>
      </div>

      <div id="right-panel" :class="{'hideQuestion':!showQuestion}">
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
          @toolBarButtonChange="toolBarButtonClick($event)"
        />
        <div id="viewerContainer">
          <div
            id="viewer"
            :class="{'freeText': activeButton== 'text'}"
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
        :top="comment.annotation.top"
        :left="comment.annotation.left"
        :page-num="comment.annotation.page"
        :page-height="comment.annotation.pageHeight"
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
          <el-button v-if="currentUser.role!=='Admin'" style="right: 10px;padding:10px 0!important;" button-id="delete" class="action-card-btn"	title="Delete Comment" @click="isUndo = false; deleteButtonClicked(comment)">
            <i class="far fa-trash-alt" />
          </el-button>
          <el-button v-if="currentUser.role!=='Admin'" style="right: 30px;padding:10px 0!important;" button-id="edit" class="action-card-btn"	title="Edit Comment"	@click="displayEditComment(comment, idx)">
            <i class="far fa-edit" />
          </el-button>
        </div>
        <div>
          <div
            v-show="!comment.isSelected"
            :id="'comment-text-' + comment.uuid"
            :style="{'-webkit-line-clamp': isInShowMoreList(comment.uuid).value==0 ? '3':'inherit'}"
            style="text-align: left; overflow-wrap: break-word; white-space: pre-wrap; display: -webkit-box;overflow: hidden;-webkit-box-orient: vertical;"
          >
            {{ comment.content }}
          </div>
          <div v-if="isInShowMoreList(comment.uuid)" class="show__more-container" @click="toggleShowMore(comment.uuid)">
            {{ isInShowMoreList(comment.uuid).value==0 ? 'Show more':'Show less' }}
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
            <el-button type="primary" :disabled="comment.content.replace(/\s/g, '').length == 0" style="float: left; padding: 5px;" @click="editCommentCard(comment)">
              Save
            </el-button>
            <el-button style="float: right; padding: 5px;" @click="cancelCommentText('edit',comment)">
              Cancel
            </el-button>
          </div>
        </div>
      </el-card>
    </div>

    <!-- Inline text tools -->
    <textToolGroup ref="textToolGroup" @highLightText="highlightEvent($event)" />
    <!-- /Inline text tools -->
    <!-- Inline color picker -->
    <el-button-group id="rectTool" style="display: none;">
      <el-button class="textToolBtn" @click="showColorPickerTool('area')">
        <i class="fas fa-palette" />
      </el-button>
      <el-button :style="{'display': isTextbox ? 'block':'none'}" class="textToolBtn" @click="editFreeText()">
        <i class="fas fa-edit" />
      </el-button>
      <el-button :style="{'display': isRect ? 'block':'none'}" class="textToolBtn" @click="addRectComment()">
        <i class="fas fa-comment-alt" />
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
import TextToolGroup from '../../components/controls/TextToolGroup'
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
import { deleteAnnotations, editTextBox } from '@/pdfjs/UI/edit.js'
// import { highlightText } from '../../pdfjs/UI/highlight-text.js'

export default {
  name: 'Document',
  components: {
    'toolbar': ToolBar,
    'tabQuestion': TabQuestion,
    'tabRubric': TabRubric,
    'textToolGroup': TextToolGroup
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
      preEditedAnno: null,
      isEditing: false,
      showMoreList: [],
      isTextbox: false,
      isRect: false,
      isRendering: false
    }
  },
  computed: {
    loadedAnnotation() {
      const data = this.$store.getters['review/getAnnotations']
      if (!data) {
        return null
      }

      return {
        annotations: data.annotations.map(a => ({ ...JSON.parse(a.data), id: a.id, top: a.top, pageNum: a.pageNum, color: a.color })),
        comments: data.comments.map(c => ({ ...JSON.parse(c.data), documentId: this.documentId, id: c.id }))
      }
    },
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  beforeMount() {
    if (this.$route.params.docId && this.$route.params.reviewId && this.$route.params.questionId) {
      this.questionId = +this.$route.params.questionId
      this.reviewId = +this.$route.params.reviewId
      this.documentId = +this.$route.params.docId
      this.RENDER_OPTIONS.documentId = this.documentId
    }
  },
  async mounted() {
    window['PDFJSAnnotate'] = PDFJSAnnotate
    window['APP'] = this
    localStorage.setItem(`${this.documentId}/tooltype`, 'cursor')
    localStorage.setItem(`${this.documentId}/color`, '#ff0000')
    PDFJSAnnotate.getStoreAdapter().clearAnnotations(this.documentId)

    // Render stuff
    this.$store.dispatch('review/loadReviewAnnotation', { docId: this.documentId, reviewId: this.reviewId }).then(async() => {
      PDFJSAnnotate.getStoreAdapter().loadAnnotations(this.documentId, this.loadedAnnotation)

      await this.render()
      this.$refs.toolBar.handleScale('fitPage')
      this.$refs.toolBar.insertExpandMenu()
      this.handleCommentPositionsRestore()
      this.hideDeleteToolBar()
      // this.ToolbarButtons()
      // this.ScaleAndRotate();
    })

    if (this.currentUser.role === 'Admin') {
      document.getElementById('viewerContainer').style.userSelect = 'none'
      this.$refs.toolBar.disableAnnotationCreate()
      this.disableToolbarSubmit()
    } else {
      enableEdit()
    }
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
    async annotationAdded(documentId, pageNumber, annotation) {
      if (annotation.type == 'textbox' || annotation.type == 'area') {
        this.disableToolbarButtons()
        annotation.documentId = this.documentId
        this.annotation = annotation
        var obj = {
          DocumentId: this.documentId,
          ReviewId: this.reviewId,
          Type: annotation.type,
          PageNum: annotation.page,
          Top: annotation.y,
          Color: annotation.color,
          Uuid: annotation.uuid,
          Data: JSON.stringify(annotation)
        }
        await this.$store.dispatch('review/addReviewAnnotation', obj).then(async rs => {
          var temp = this.$store.getters['review/getAddedAnnotation']
          annotation.id = temp.id
          await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, annotation.uuid, annotation, 'added')
          this.undoHistory.push({ action: 'added', annotation: annotation })
          this.updateUndoList()
          this.setStatusText()
        })
      } else if (annotation.type == 'point') {
        this.disableToolbarButtons()
      }
    },
    annotationEdited(documentId, annotationId, annotation, actionType, previousAnno) {
      const anno = {
        Id: annotation.id,
        DocumentId: documentId,
        ReviewId: this.reviewId,
        Type: annotation.type,
        Color: annotation.color,
        Uuid: annotation.uuid,
        PageNum: annotation.pageNum,
        Top: annotation.top,
        Data: JSON.stringify(annotation)
      }
      reviewService.editAnnotation(anno).then(rs => {
        // this.hideRectToolBar()
        this.hideDeleteToolBar()
        if (rs && !actionType) {
          this.undoHistory.push({ action: 'edited', annotation: previousAnno })
          this.updateUndoList()
        }
        this.setStatusText()
      })
    },
    handleAnnotationDelete(target) {
      if (target.type == 'comment-highlight' || target.type == 'point') {
        PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(r => {
          var cmt = r.filter(t => { return t.uuid == target.uuid })[0]
          if (cmt) {
            this.deleteButtonClicked(cmt)
            this.hideRectToolBar()
            this.hideDeleteToolBar()
          }
        })
      } else {
        this.hideRectToolBar()
        this.hideDeleteToolBar()
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
      // await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      await this.comments.sort(this.compareTopAnnoAttributes)
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
      addEventListener('annotation:edit', this.annotationEdited)
      addEventListener('annotation:insertNoteComment', this.insertNoteComment)
      addEventListener('text:disable', this.disableToolbarButtons)
      addEventListener('comment:updateCommentPositionAfterEditAnnotation', this.updateCommentPositionAfterEditAnnotation)

      function handlePageScroll(e) {
        // document.getElementById('left-panel').style.top = e.target.scrollTop + 'px'
        // eslint-disable-next-line no-console
        that.hideRectToolBar()
        that.hideColorPickerTool()
        that.hideTextToolGroup()
        that.hideDeleteToolBar()
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
      async function annotationClick(target) {
        if (!target) {
          return
        }
        await PDFJSAnnotate.getStoreAdapter().getAnnotation(that.documentId, target.getAttribute('data-pdf-annotate-id')).then(r => {
          that.annotation = r
          that.$refs.toolBar.handleAnnotationClicked(that.annotation)
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
      this.activeButton = e
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
        this.hideRectToolBar()
        this.hideDeleteToolBar()
      })
    },
    async handleCommentAnnotationClick(target) {
      this.hideRectToolBar()
      const text = window.getSelection().toString().trim()
      const newCommentWrapper = document.getElementById('add-new-comment')
      if (newCommentWrapper && newCommentWrapper.style.display != 'none') {
        return
      }
      // Display text tool bar if text is selected.
      this.isTextbox = null
      this.isRect = false
      if (!text && (typeof (target) != 'undefined')) {
        const type = target.getAttribute('data-pdf-annotate-type')
        this.annotationClicked = target
        if (type == 'strikeout') {
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
            var posXX = rect.left
            var posYY = parseInt(rect.top) + rect.height + 10
            deleteTool.style = 'position: absolute; top: ' + posYY + 'px; left: ' + posXX + 'px;'
          }
        } else if (type == 'highlight' && this.currentUser.role != 'Admin') {
          this.colorChosen = target.getAttribute('stroke')
          uuid = target.getAttribute('data-pdf-annotate-id')
          this.annotation = this.loadedAnnotation.annotations.filter(r => { return r.uuid === uuid })[0]
          var rects = target.getBoundingClientRect()
          // Display rect tool bar if text is selected.
          if (target.getAttribute('top') == '') {
            // window.getSelection().removeAllRanges()
            this.hideDeleteToolBar()
          } else {
            const rectTool = document.getElementById('rectTool')
            var posXrect = rects.left
            var posYrect = parseInt(rects.top) + rects.height + 10
            rectTool.style = 'position: absolute; top: ' + posYrect + 'px; left: ' + posXrect + 'px;'
          }
        }
        if (type == 'comment-highlight' || type == 'point' || type == 'comment-area') {
          const selectedHighlight = document.getElementsByClassName('comment-highlight-selected')
          const selectedRect = document.getElementsByClassName('rectangle-selected')
          if (selectedHighlight.length > 0) { selectedHighlight[0].classList.remove('comment-highlight-selected') }
          if (selectedRect.length > 0) {
            selectedRect[0].classList.remove('rectangle-selected')
          }
          if (type == 'comment-area') {
            target.classList.add('rectangle-selected')
          } else {
            target.classList.add('comment-highlight-selected')
          }

          const commentCards = document.querySelectorAll('.comment-card')
          const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
          const commentPoints = document.querySelectorAll("svg[data-pdf-annotate-type='point']")
          const commentAreas = document.querySelectorAll("rect[data-pdf-annotate-type='comment-area']")
          const areaArr = Array.prototype.slice.call(commentAreas)
          const highlightArr = Array.prototype.slice.call(commentHighlights)
          var pointsArr = Array.prototype.slice.call(commentPoints)

          // Sort the highlightsvar pointsArr = Array.prototype.slice.call(commentPoints)
          // pointsArr.forEach(r => {
          //   r.setAttribute('top', parseInt(r.getAttribute('y')))
          // })
          // highlightArr.sort(this.compareTopAttributes)
          var combineArr = highlightArr.concat(pointsArr)
          combineArr = combineArr.concat(areaArr)
          combineArr.sort(this.compareTopAttributes)
          // this.comments.sort(this.compareTopAnnoAttributes)
          // Get order of the current highlight
          this.order = combineArr.findIndex(item => {
            return item.dataset.pdfAnnotateId == target.getAttribute('data-pdf-annotate-id')
          })
          var gTop = parseInt(parseInt(target.getAttribute('top')) * this.RENDER_OPTIONS.scale) - 34
          const svgHeight = parseInt(target.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12
          const svgPageNum = parseInt(target.getAttribute('page-num'))
          if (svgPageNum > 1) { gTop += ((svgPageNum - 1) * svgHeight) }
          const cTop = this.comments[this.order].topPosition

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
          } else {
            const endPos = gTop + commentCards[this.order].offsetHeight
            if (this.order < commentCards.length - 1) { this.moveUpToEndPos(commentCards, this.order + 1, endPos) }
          }
        } else if (type == 'area' && this.currentUser.role != 'Admin') {
          target.classList.add('rectangle-selected')
          this.isRect = true
          this.isTextbox = null
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
        } else if (type == 'textbox' && this.currentUser.role != 'Admin') {
          this.isTextbox = target
          this.isRect = false
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
      const previousAnno = Object.assign({}, this.annotation)
      var uuid = this.annotationClicked.getAttribute('data-pdf-annotate-id')
      var type = this.annotationClicked.getAttribute('data-pdf-annotate-type')
      if (type != 'highlight') {
        this.annotationClicked.setAttribute('stroke', e)
      }
      if (type == 'textbox') {
        this.annotationClicked.childNodes[0].style.color = e
      } else if (type == 'highlight') {
        if (this.annotationClicked.hasChildNodes()) {
          const child = this.annotationClicked.childNodes
          for (let i = 0; i < child.length; i++) {
            this.annotationClicked.childNodes[i].setAttribute('fill', e)
          }
        }
      }
      this.annotation.color = e
      await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, uuid, this.annotation, undefined, previousAnno)
      // anno = {
      //   Id: this.annotation.id,
      //   DocumentId: this.documentId,
      //   ReviewId: this.reviewId,
      //   Type: this.annotation.type,
      //   Color: this.annotation.color,
      //   Uuid: this.annotation.uuid,
      //   PageNum: this.annotation.pageNum,
      //   Top: this.annotation.top,
      //   Data: JSON.stringify(this.annotation)
      // }

      // reviewService.editAnnotation(anno).then(rs => {
      // })
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
    async handleCommentPositionsRestore(e) {
      const documentId = document.getElementById('viewer').getAttribute('document-id')
      const highlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const points = document.querySelectorAll("svg[data-pdf-annotate-type='point']")
      const commentAreas = document.querySelectorAll("rect[data-pdf-annotate-type='comment-area']")
      const areaArr = Array.prototype.slice.call(commentAreas)
      const highlightArr = Array.prototype.slice.call(highlights)
      var pointsArr = Array.prototype.slice.call(points)

      var combineArr = highlightArr.concat(pointsArr)
      combineArr = combineArr.concat(areaArr)
      combineArr.sort(this.compareTopAttributes)
      combineArr.forEach(cmt => {
        this.handleCommentAnnotationClick(cmt)
      })
      this.hideDeleteToolBar()
      this.handleCommentAnnotationClick(combineArr[0])
      const comments = await this.updateCommentAnnotations(documentId, e)
      await PDFJSAnnotate.getStoreAdapter().updateComments(documentId, comments)
      const selected = document.getElementsByClassName('comment-card-selected')
      if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
      const annoSelected = document.querySelector('.comment-highlight-selected')
      if (annoSelected) {
        annoSelected.classList.remove('comment-highlight-selected')
      }
      const rectSelected = document.querySelectorAll('.rectangle-selected')
      if (rectSelected.length > 0) {
        for (let i = 0; i < rectSelected.length; i++) {
          rectSelected[i].classList.remove('rectangle-selected')
        }
      }
    },
    async handleCommentPositionsRestoreAfterMove(target, e) {
      this.hideDeleteToolBar()
      const documentId = document.getElementById('viewer').getAttribute('document-id')
      const highlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const points = document.querySelectorAll("svg[data-pdf-annotate-type='point']")
      const commentAreas = document.querySelectorAll("rect[data-pdf-annotate-type='comment-area']")
      const areaArr = Array.prototype.slice.call(commentAreas)
      const highlightArr = Array.prototype.slice.call(highlights)
      var pointsArr = Array.prototype.slice.call(points)

      var combineArr = highlightArr.concat(pointsArr)
      combineArr = combineArr.concat(areaArr)
      combineArr.sort(this.compareTopAttributes)
      combineArr.forEach(cmt => {
        this.handleCommentAnnotationClick(cmt)
      })

      const order = combineArr.findIndex(item => {
        return item.dataset.pdfAnnotateId == target.getAttribute('data-pdf-annotate-id')
      })
      console.log('target order', order, target.getAttribute('data-pdf-annotate-id'))
      if (combineArr.length > 2) {
        console.log('lenght', combineArr.length)
        if (order == 0) {
          this.handleCommentAnnotationClick(combineArr[order + 1])
        } else {
          this.handleCommentAnnotationClick(combineArr[order])
        }
      }
      const comments = await this.updateCommentAnnotations(documentId, e)
      await PDFJSAnnotate.getStoreAdapter().updateComments(documentId, comments)
      const selected = document.getElementsByClassName('comment-card-selected')
      if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }

      if (combineArr.length > 2) {
        console.log('lenght', combineArr.length)
        if (order == 0) {
          this.handleCommentAnnotationClick(combineArr[order + 1])
        } else {
          this.handleCommentAnnotationClick(combineArr[order])
        }
      }

      const annoSelected = document.querySelector('.comment-highlight-selected')
      if (annoSelected) {
        annoSelected.classList.remove('comment-highlight-selected')
      }
      const rectSelected = document.querySelectorAll('.rectangle-selected')
      if (rectSelected.length > 0) {
        for (let i = 0; i < rectSelected.length; i++) {
          rectSelected[i].classList.remove('rectangle-selected')
        }
      }
    },
    async handleCommentPositionsAfterHandleScale() {
      this.isRendering = true
      const selected = document.getElementsByClassName('comment-card-selected')
      if (selected.length == 1) {
        const uuid = selected[0].getAttribute('highlight-id')
        const cmtAnno = this.getHighlightByCommentId(uuid)
        this.handleCommentAnnotationClick(cmtAnno)
        this.hideDeleteToolBar()
      } else {
        this.handleCommentPositionsRestore()
      }
    },
    async updateCommentAnnotations(documentId, e) {
      const comments = await PDFJSAnnotate.getStoreAdapter().getComments(documentId)
      comments.forEach(comment => {
        const thisCard = document.querySelector(".comment-card[highlight-id='" + comment.uuid + "']")
        comment.topPosition = parseInt(thisCard.getAttribute('top-position'))

        if (typeof (e) == 'undefined') {
          const commentTextContainer = document.getElementById('comment-text-' + comment.uuid)
          if (commentTextContainer.clientHeight > (parseInt(window.getComputedStyle(commentTextContainer).lineHeight, 10) * 3)) {
            commentTextContainer.style.webkitLineClamp = '3'
            this.showMoreList.push({ id: comment.uuid, value: 0 })
          }
        }
      })
      this.hideDeleteToolBar()
      return comments
    },
    async HighlightText() {
      await this.highlightText('highlight')
    },
    async StrikethroughText() {
      await this.highlightText('strikeout')
      this.hideTextToolBar()
      this.hideTextToolGroup()
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

      await this.undoHistory.push({ action: 'added', annotation: newComment })
      this.updateUndoList()
      await this.comments.push(newComment)
      // await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      await this.comments.sort(this.compareTopAnnoAttributes)
      // this.comments.sort(this.compareTopAnnoAttributes)
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
        Data: JSON.stringify(this.annotation),
        Id: this.annotation.id
      }
      var newCmt = {
        Text: newComment.text != null ? newComment.text : '',
        Content: newComment.content,
        TopPosition: newComment.topPosition,
        Uuid: newComment.uuid,
        Data: JSON.stringify(newComment)
      }
      if (this.annotation.type == 'area') {
        this.annotation.type = 'comment-area'
        anno.Type = 'comment-area'
        await this.updateTypeOfRect('comment-area')
      }
      if (this.annotation.type == 'point') {
        this.$refs.toolBar.handleAnnotationClicked(null)
      }
      this.disableToolbarButtons()
      reviewService.addInTextComment(this.documentId, 1, newCmt, anno).then(async rs => {
        if (rs.data) {
          this.updateCommentId(rs)
        }
      })
      await this.updatePositionsAfterCommentAdded()
      // await this.handleCommentPositionsRestore()
      const target = this.getHighlightByCommentId(this.annotation.uuid)
      this.handleCommentAnnotationClick(target)
      this.newComment = ''
    },
    // Begin migrating code from select.js to document.vue
    async commentText(note) {
      // Highlight the text first
      if (typeof (note) == 'undefined') {
        await this.highlightText('comment-highlight')
      }
      // if (!this.target) {
      //   this.target = this.getHighlightByCommentId(this.annotation.uuid)
      // }
      // this.target = this.getHighlightByCommentId(this.annotation.uuid)
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
        await this.highlightText(type)
      }
    },
    async highlightText(type) {
      if (!this.svg) {
        return
      }
      const boundingRect = this.boundingRect

      var color = 'FFFF00'
      if (type == 'strikeout') {
        // color = localStorage.getItem(`${this.RENDER_OPTIONS.documentId}/color`) ? color = localStorage.getItem(`${this.RENDER_OPTIONS.documentId}/color`) : 'FF0000'
        color = 'FF0000'
      }
      if (type == 'highlight') {
        color = 'FFFF00'
      }
      if (type == 'comment-highlight') {
        // color = 'FF00FF'
        color = '0000FF'
      }

      // Initialize the annotation
      const annotation = {
        type,
        color,
        left: (this.rects[0].left - boundingRect.left) / this.RENDER_OPTIONS.scale,
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
        annotation.rectangles = this.removeMaxHeigthRects(annotation.rectangles)
      }

      const { documentId, pageNumber } = getMetadata(this.svg)
      // format top with scale 100%
      annotation.color = color
      annotation.top = annotation.rectangles[0].y
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

                await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, annotation.uuid, annotation, 'added')
              }
            })
          }
          this.setStatusText()
        })
    },
    updateCommentPositions(note) {
      // const commentCardTemp = document.querySelectorAll('.comment-card')
      // var commentCards = Array.prototype.slice.call(commentCardTemp)
      // commentCards.sort(this.compareTopAttributes)
      const commentCards = document.querySelectorAll('.comment-card')
      const commentHighlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
      const commentPoints = document.querySelectorAll("svg[data-pdf-annotate-type='point']")
      const commentAreas = document.querySelectorAll("rect[data-pdf-annotate-type='comment-area']")
      const areaArr = Array.prototype.slice.call(commentAreas)
      const highlightArr = Array.prototype.slice.call(commentHighlights)
      var pointsArr = Array.prototype.slice.call(commentPoints)
      var combineArr = highlightArr.concat(pointsArr)
      combineArr = combineArr.concat(areaArr)
      combineArr.sort(this.compareTopAttributes)
      // Get order of the current highlight
      if (this.annotation) {
        this.target = this.getHighlightByCommentId(this.annotation.uuid)
      }
      this.order = combineArr.findIndex(item => {
        return item.dataset.pdfAnnotateId == this.target.getAttribute('data-pdf-annotate-id')
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
        const svgHeight = parseInt(svg.getAttribute('height')) + 12
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
            this.moveUpToEndPos(commentCards, this.order, endPos)
          } else {
            // // Check if move down is necessary
            // const commentId = commentCards[this.order - 1].getAttribute('highlight-id')
            // const g = this.getHighlightByCommentId(commentId)
            // let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
            // if (g.getAttribute('page-num') > 1) { gTop += (g.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 10) }
            // if (upperTop < gTop) {
            //   this.moveDownToTopPos(commentCards, this.order - 1, topPos)
            // }
            this.moveDownToTopPos(commentCards, this.order - 1, topPos)
            this.moveDownFromEndPos(commentCards, this.order, endPos)
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
            if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) + 12) * this.RENDER_OPTIONS.scale) }
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
        // await this.comments.sort(this.compareTopAttributes)
        const commentCardTemp = document.querySelectorAll('.comment-card')
        var commentCards = Array.prototype.slice.call(commentCardTemp)
        commentCards.sort(this.compareTopAttributes)
        const endPos = parseInt(newComment.getAttribute('top-position')) + newComment.offsetHeight
        if (this.order < (commentCards.length - 1)) { this.moveUpToEndPos(commentCards, this.order + 1, endPos) }
      }
      const that = this

      const commentTextContainer = document.getElementById('comment-text-' + this.annotation.uuid)
      if (commentTextContainer.clientHeight > (parseInt(window.getComputedStyle(commentTextContainer).lineHeight, 10) * 3)) {
        commentTextContainer.style.webkitLineClamp = '3'
        if (this.showMoreList.filter(item => item.id === this.annotation.uuid) == 0) {
          this.showMoreList.push({ id: this.annotation.uuid, value: 0 })
        }
      }

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
      if (this.target.getAttribute('page-num')) {
        this.svg = document.querySelector(`svg[data-pdf-annotate-page='${this.target.getAttribute('page-num')}']`)
      }
      const svg = this.svg
      const svgHeight = svg.getAttribute('height')
      const svgPageNum = svg.getAttribute('data-pdf-annotate-page')
      let svgTop = 0
      if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * (parseInt(svgHeight) + 12)) }
      var topPos = svgTop + rectTop - 35
      newCommentWrapper.style = 'width: 100%; position: absolute; left: -20px; top: ' + topPos + 'px;'
      this.hideTextToolBar()
      this.hideTextToolGroup()
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
    cancelCommentText(e, comment) {
      this.isEditing = null
      if (e == 'edit') {
        this.comments.forEach(function(element) {
          element.isSelected = false
        })
        comment.content = this.preEditedAnno.content
        this.updateCommentPositions()
        return
      }
      this.newComment = ''
      const documentId = this.documentId
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
      if (this.annotation.type != 'area') {
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
      } else {
        const draftCommentText = document.querySelector('[draft-comment-id]')
        if (draftCommentText) {
          const draftUuid = draftCommentText.getAttribute('draft-comment-id')
          if (document.querySelector("[data-pdf-annotate-id='" + draftUuid + "']")) {
            document.querySelector("[data-pdf-annotate-id='" + draftUuid + "']").remove()
          }
        }
        this.annotationClicked.setAttribute('data-pdf-annotate-type', 'area')
      }
      this.disableToolbarButtons()
      // Remove the annotation
    },
    unselectHighlightComment() {
      const selectedHighlight = document.getElementsByClassName('comment-highlight-selected')
      if (selectedHighlight.length > 0) { selectedHighlight[0].classList.remove('comment-highlight-selected') }
      const selectedRect = document.getElementsByClassName('rectangle-selected')
      if (selectedRect.length > 0) { selectedRect[0].classList.remove('rectangle-selected') }
      const selectedComment = document.getElementsByClassName('comment-card-selected')
      if (selectedComment.length > 0) { selectedComment[0].classList.remove('comment-card-selected') }
    },
    moveUpFromTopPos(commentCards, startIndex, topPos) {
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      if (!g) { return }
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) + 12) * this.RENDER_OPTIONS.scale) }
      // Get start index card's position
      const adjTop = this.comments[startIndex].topPosition
      const adjEnd = adjTop + commentCards[startIndex].offsetHeight
      if (adjEnd + 10 < topPos) {
        return
      }
      if (this.isRendering) {
      // Move the start index card up first
        if ((adjTop - (adjEnd - topPos) - 10) > gTop) {
          this.comments[startIndex].topPosition = gTop
        } else {
          this.comments[startIndex].topPosition = adjTop - (adjEnd - topPos) - 10
        }
      } else {
        this.comments[startIndex].topPosition = adjTop - (adjEnd - topPos) - 10
      }
      if (startIndex > 0) {
        for (let m = startIndex - 1; m >= 0; m--) {
          const previousTop = this.comments[m + 1].topPosition
          const thisTop = this.comments[m].topPosition
          const thisEnd = thisTop + commentCards[m].offsetHeight
          if (thisEnd + 10 < previousTop) {
            return
          }
          const thisG = this.getHighlightByCommentId(this.comments[m].uuid)
          if (!thisG) { return }
          var thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35
          if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) + 12) * this.RENDER_OPTIONS.scale) }
          if (this.isRendering) {
            if ((thisTop - (thisEnd - previousTop) - 10) > thisGTop) {
              this.comments[m].topPosition = thisGTop
            } else {
              this.comments[m].topPosition = thisTop - (thisEnd - previousTop) - 10
            }
          } else {
            this.comments[m].topPosition = thisTop - (thisEnd - previousTop) - 10
          }
        }
      }
    },
    moveUpToEndPos(commentCards, startIndex, endPos) {
      const g = this.getHighlightByCommentId(this.comments[startIndex].uuid)
      if (!g) { return }
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12)) }

      if ((endPos + 10) < gTop) {
        if (this.comments[startIndex].topPosition < gTop) {
          return
        }
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
          if (!thisG) { return }
          let thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35

          // Check if move down is necessary
          if (thisTop > (previousEnd + 10)) {
            if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12)) }
            if (this.comments[n].topPosition < thisGTop) { return }
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
      if (!g) { return }
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12)) }
      if ((topPos - 10 - commentCards[startIndex].offsetHeight) > gTop) {
        if (this.comments[startIndex].topPosition > gTop) {
          return
        }
        this.comments[startIndex].topPosition = gTop
      } else {
        this.comments[startIndex].topPosition = desiredTop
      }
      // Move other cards down
      if (startIndex > 0) {
        for (let n = startIndex - 1; n >= 0; n--) {
          const previousTop = this.comments[n + 1].topPosition
          const thisG = this.getHighlightByCommentId(this.comments[n].uuid)
          if (!thisG) { return }
          let thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35
          if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12)) }
          if (this.comments[n].topPosition > thisGTop) { return }
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
      if (!g) { return }
      let gTop = parseInt(g.getAttribute('top')) * this.RENDER_OPTIONS.scale - 35
      if (g.getAttribute('page-num') > 1) { gTop += ((g.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12)) }
      if (this.comments[startIndex].topPosition > endPos + 10) {
        return
      }
      const desiredTop = (endPos + 10) + gTop * 0
      if (this.isRendering) {
        if ((endPos + 10) < gTop) {
          this.comments[startIndex].topPosition = gTop
        } else {
        // Move start index card up
          this.comments[startIndex].topPosition = desiredTop
        }
      } else {
        this.comments[startIndex].topPosition = desiredTop
      }

      // Move other cards down ward
      if (this.comments.length > (startIndex + 1)) {
        for (let j = startIndex + 1; j < this.comments.length; j++) {
          const previousTop = this.comments[j - 1].topPosition
          const previousEnd = previousTop + commentCards[j - 1].offsetHeight
          if (this.comments[j].topPosition > previousEnd + 10) {
            return
          }
          const thisG = this.getHighlightByCommentId(this.comments[j].uuid)
          if (!thisG) { return }
          var thisGTop = parseInt(thisG.getAttribute('top') * this.RENDER_OPTIONS.scale) - 35

          // Check if move down is necessary
          if (thisG.getAttribute('page-num') > 1) { thisGTop += ((thisG.getAttribute('page-num') - 1) * (parseInt(g.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12)) }
          if (this.isRendering) {
            if (thisGTop > (previousEnd + 10)) {
              this.comments[j].topPosition = thisGTop
            } else {
              this.comments[j].topPosition = previousEnd + 10
            }
          } else {
            this.comments[j].topPosition = previousEnd + 10
          }
          // const thisDesiredTop = thisGTop >= (previousEnd + 10) ? thisGTop : (previousEnd + 10)
        }
      }
    },
    hasEnoughSpace(commentCards, note) {
      if (this.comments.length > 0) {
        if (note) {
          this.svg = document.querySelector(`svg[data-pdf-annotate-page='${note.page}']`)
        }
        const svg = this.svg
        var rectTop = 0
        if (typeof (note) == 'undefined') {
          rectTop = parseInt(this.target.getAttribute('top')) * this.RENDER_OPTIONS.scale
        } else {
          rectTop = parseInt(note.y) * this.RENDER_OPTIONS.scale
        }
        // 12 = page's margin + page's border * 2
        const svgHeight = parseInt(svg.getAttribute('height')) + 12
        const svgPageNum = parseInt(svg.getAttribute('data-pdf-annotate-page'))

        let svgTop = 0
        if (svgPageNum > 1) { svgTop += ((svgPageNum - 1) * svgHeight) }
        const topPos = parseInt(svgTop) + rectTop - 35
        const endPos = topPos + 126
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
      let top1 = parseInt(a.getAttribute('top')) * this.RENDER_OPTIONS.scale
      if (a.getAttribute('page-num') > 1) { top1 += ((a.getAttribute('page-num') - 1) * (parseInt(a.getAttribute('page-height'))) * this.RENDER_OPTIONS.scale + 12) }

      let top2 = parseInt(b.getAttribute('top')) * this.RENDER_OPTIONS.scale
      if (b.getAttribute('page-num') > 1) { top2 += ((b.getAttribute('page-num') - 1) * (parseInt(b.getAttribute('page-height'))) * this.RENDER_OPTIONS.scale + 12) }
      if (top1 > top2) return 1
      if (top2 > top1) return -1
      if (top2 == top1) {
        // compare order attributes
        const left1 = parseInt(a.getAttribute('left'))
        const left2 = parseInt(b.getAttribute('left'))
        if (left1 > left2) return 1
        if (left2 > left1) return -1
      }
      return 0
    },
    compareTopAnnoAttributes(a, b) {
      let top1 = a.annotation.top * this.RENDER_OPTIONS.scale
      if (a.annotation.page > 1) { top1 += ((a.annotation.pageNum - 1) * ((a.annotation.pageHeight) * this.RENDER_OPTIONS.scale) + 12) }

      let top2 = b.annotation.top * this.RENDER_OPTIONS.scale
      if (b.annotation.page > 1) { top2 += ((b.annotation.pageNum - 1) * ((a.annotation.pageHeight) * this.RENDER_OPTIONS.scale) + 12) }
      if (top1 > top2) return 1
      if (top2 > top1) return -1
      if (top2 == top1) {
        // compare order attributes
        const left1 = parseInt(a.annotation.left)
        const left2 = parseInt(b.annotation.left)
        if (left1 > left2) return 1
        if (left2 > left1) return -1
      }
      return 0
    },
    removeMaxHeigthRects(arr) {
      const max = Math.max(...arr.map(a => parseInt(a.height)))
      return arr.filter(a => a.height < max)
    },
    displayEditComment(comment) {
      this.preEditedAnno = Object.assign({}, comment)
      this.isEditing = comment.uuid
      this.comments.forEach(function(element) {
        element.isSelected = false
      })
      const highlight = document.querySelector("[data-pdf-annotate-id='" + comment.uuid + "']")
      this.svg = document.querySelector(`svg[data-pdf-annotate-page='${comment.annotation.page}']`)
      this.handleCommentAnnotationClick(highlight)
      comment.isSelected = true
      this.$forceUpdate()
      const refName = 'comment' + comment.uuid
      this.$refs[refName][0].$el.childNodes[0].focus()
      this.$refs[refName][0].$el.childNodes[0].addEventListener('keyup', (e) => {
        this.updateCommentPositionWhileEditing(e)
      })
      this.target = highlight
      this.updateCommentPositions()
    },
    updateCommentPositionWhileEditing(e) {
      if (e.keyCode == 13) {
        const editCmt = document.querySelector('.comment-card-selected')
        const commentCards = document.querySelectorAll('.comment-card')
        const endPos = editCmt.offsetTop + editCmt.offsetHeight
        if (this.order < commentCards.length - 1) { this.moveDownFromEndPos(commentCards, this.order + 1, endPos) }
      }
    },
    async editCommentCard(comment, isUndoRedo) {
      var editComment = {
        Text: comment.text ? comment.text : '',
        Content: comment.content,
        Id: comment.id,
        Uuid: comment.uuid,
        TopPosition: comment.topPosition,
        Data: JSON.stringify(comment)
      }

      await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(cmts => {
        editComment.Id = cmts.filter(r => { return r.uuid === editComment.Uuid })[0]['id']
      })
      await PDFJSAnnotate.getStoreAdapter().getAnnotation(this.documentId, editComment.Uuid).then(anno => {
        editComment.AnnotationId = anno.id
      })

      // Udpate comment position
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
      await PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)

      this.comments.forEach(function(element) {
        element.isSelected = false
      })
      this.$forceUpdate()

      const editCmt = document.querySelector('.comment-card-selected')
      if (editCmt) {
        const cmtAnno = document.querySelector(`[data-pdf-annotate-id="${editCmt.getAttribute('highlight-id')}"]`)
        if (cmtAnno) {
          const commentTextContainer = document.getElementById('comment-input-' + comment.uuid)
          if (parseInt(window.getComputedStyle(commentTextContainer).height, 10) > (parseInt(window.getComputedStyle(commentTextContainer).lineHeight, 10) * 4)) {
            commentTextContainer.style.webkitLineClamp = '3'
            if (this.showMoreList.filter(item => item.id === comment.uuid) == 0) {
              this.showMoreList.push({ id: comment.uuid, value: 0 })
            }
          } else {
            commentTextContainer.style.webkitLineClamp = 'inherit'
            this.showMoreList = this.showMoreList.filter(item => item.id !== editCmt.getAttribute('highlight-id'))
          }
          await this.showMoreList.map(rs => { rs.value = 0 })
          this.hideDeleteToolBar()
          await this.handleCommentAnnotationClick(cmtAnno)
        }
      }

      reviewService.editComment(editComment).then(async rs => {
        if (rs) {
          // this.addToUndoList(rs.data, 'edit')
          this.$refs.toolBar.setStatusText()
        }
      })
      this.isEditing = null
    },
    async deleteCommentCard(uuid) {
      await PDFJSAnnotate.getStoreAdapter().deleteComment(this.documentId, uuid)
      this.comments = await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId)
      await this.comments.forEach(function(element) {
        element.isSelected = false
      })
      // await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      await this.comments.sort(this.compareTopAnnoAttributes)
      this.handleCommentPositionsRestore()
    },
    hideTextToolBar() {
      const textTool = document.getElementById('textTool')
      if (textTool) {
        textTool.style.display = 'none'
      }
    },
    hideTextToolGroup() {
      this.$refs.textToolGroup.HideTextToolGroup()
    },
    getHighlightByCommentId(commentId) {
      return document.querySelector("[data-pdf-annotate-id='" + commentId + "']")
    },
    getSelectionRects() {
      const selection = window.getSelection()
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
      if (!this.rubricCriteria) {
        this.$notify.error({
          title: 'Field Required',
          message: 'Please fill all rubric fields!',
          duration: 2000
        })
      } else {
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
        const annotationsCount = await this.countAnnotations()
        if (annotationsCount < 3) {
          this.$notify.error({
            title: 'Annotations required',
            message: 'At least 3 annotations required!',
            duration: 2000
          })
          return
        }
        reviewService.saveReviewFeedback(this.reviewId, reviewData).then(rs => {
          this.disableToolbarSubmit()
          if (rs.data == '') {
            this.$notify.success({
              title: 'Submit success',
              message: 'Submitted!',
              duration: 2000
            })
          } else {
            this.$notify.error({
              title: 'Submit failed',
              message: 'Current review not exist!',
              duration: 2000
            })
          }
          this.$router.push('/reviews')
        })
        reviewService.loadReviewFeedback(this.reviewId).then(rs => {
          localStorage.removeItem('reviewComment')
        })
      }
    },
    async countAnnotations() {
      var count = 0
      for (let i = 1; i <= this.NUM_PAGES; i += 1) {
        await PDFJSAnnotate.getStoreAdapter().getAnnotations(this.documentId, i).then(rs => {
          if (rs.annotations.length > 0) {
            count += rs.annotations.length
          }
        })
      }
      return count
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
        rightPanel.style.width = window.innerWidth - document.getElementById('left-panel').offsetWidth - 17 + 'px'
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
      this.hideDeleteToolBar()
      this.$refs.toolBar.insertExpandMenu()
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
      await this.reRenderPages()
      const overlayDoc = document.getElementById('pdf-annotate-edit-overlay')
      if (overlayDoc && typeof (overlayDoc) != 'undefined') {
        overlayDoc.parentNode.removeChild(overlayDoc)
      }
      this.hideRectToolBar()
      this.hideDeleteToolBar()
      this.hideTextToolBar()
      this.hideTextToolGroup()

      this.$refs.toolBar.insertExpandMenu()
    },

    async reRenderPages() {
      await UI.renderAllPages(this.NUM_PAGES, this.RENDER_OPTIONS)
      this.documentWidthCal()
      await this.handleCommentPositionsAfterHandleScale()
      this.isRendering = false
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
          } else if (undoAnno.annotation.type == 'area') {
            undoAnno.annotation.type = 'comment-area'
            undoAnno.text = ','
          }
          await PDFJSAnnotate.getStoreAdapter().addComment(undoAnno.annotation.documentId,
            undoAnno.annotation,
            undoAnno,
            undoAnno.text,
            undoAnno.topPosition)
          await this.comments.push(undoAnno)
          // await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
          await this.comments.sort(this.compareTopAnnoAttributes)
          document.getElementById('add-new-comment').style.display = 'none'
          var anno = {
            DocumentId: this.documentId,
            ReviewId: this.reviewId,
            Type: undoAnno.annotation.type,
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

          this.svg = document.querySelector(`svg[data-pdf-annotate-page='${undoAnno.annotation.page}']`)
          appendChild(this.svg, undoAnno.annotation)
          this.annotation = undoAnno.annotation
          this.newComment = ''
          this.updatePositionsAfterCommentAdded()
          await this.handleCommentPositionsRestore()
        } else {
          await PDFJSAnnotate.getStoreAdapter().addAnnotation(this.documentId, undoAnno.page, undoAnno, true)
            .then(async(annotation) => {
              if (undoAnno.type != 'comment-highlight' && undoAnno.type != 'comment-area' && undoAnno.type != 'point') {
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
        if (undoAnno.class == 'Comment') {
          await this.editCommentCard(undoAnno, 'undo')
        } else if ((undoAnno.class == 'Annotation')) {
          await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, undoAnno.uuid, undoAnno, 'undo')
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
            } else if (redoAnno.annotation.type == 'area') {
              redoAnno.annotation.type = 'comment-area'
              redoAnno.text = ','
            }
            await PDFJSAnnotate.getStoreAdapter().addComment(redoAnno.annotation.documentId,
              redoAnno.annotation,
              redoAnno,
              redoAnno.text,
              redoAnno.topPosition)
            await this.comments.push(redoAnno)
            // await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
            await this.comments.sort(this.compareTopAnnoAttributes)
            document.getElementById('add-new-comment').style.display = 'none'
            var anno = {
              DocumentId: this.documentId,
              ReviewId: this.reviewId,
              Type: redoAnno.annotation.type,
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
            this.svg = document.querySelector(`svg[data-pdf-annotate-page='${redoAnno.annotation.page}']`)
            appendChild(this.svg, redoAnno.annotation)
            this.annotation = redoAnno.annotation
            this.newComment = ''
            this.updatePositionsAfterCommentAdded()
            await this.handleCommentPositionsRestore()
          } else {
            await PDFJSAnnotate.getStoreAdapter().addAnnotation(this.documentId, redoAnno.page, redoAnno, true)
              .then(async(annotation) => {
                if (redoAnno.type != 'comment-highlight' && redoAnno.type != 'comment-area' && redoAnno.type != 'point') {
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
          } else if ((redoAnno.class == 'Annotation')) {
            await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, redoAnno.uuid, redoAnno, 'redo')
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
    async deleteButtonClicked(e) {
      this.$confirm('Are you sure you want to delete this comment ?').then(() => {
        this.deleteCommentCard(e.uuid)
        this.removeElementById(e.annotation.uuid)
      }).catch(() => {
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
      document.getElementById('add-new-comment').setAttribute('draft-comment-id', note.uuid)
      this.annotation = note
      this.svg = svg
      this.commentText(note)
    },
    async updateCommentPositionAfterEditAnnotation(annotationId) {
      const target = this.getHighlightByCommentId(annotationId)
      let annotation = null
      let editComment = null
      let cmts = null
      await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(r => {
        cmts = r
      })
      await PDFJSAnnotate.getStoreAdapter().getAnnotation(this.documentId, annotationId).then(async anno => {
        annotation = anno
      })
      if (!annotation) { return }
      const type = annotation.type
      if (type == 'comment-area' || type == 'point') {
        const rect = this.getHighlightByCommentId(annotationId)
        rect.setAttribute('top', annotation.top)
        rect.setAttribute('left', annotation.left)

        const cmtRect = document.querySelector(".comment-card[highlight-id='" + annotationId + "']")
        cmtRect.setAttribute('top', annotation.top)
        cmtRect.setAttribute('left', annotation.left)

        cmts.forEach(cmt => {
          if (cmt.uuid == annotationId) {
            cmt.annotation = annotation
            editComment = cmt
          }
        })
        await PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)
        this.comments = await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId)
        await this.comments.sort(this.compareTopAnnoAttributes)
        this.editCommentCard(editComment, 'undo')
        console.log('target', target)
        await this.handleCommentPositionsRestoreAfterMove(target)
        await this.handleCommentAnnotationClick(target)
      }
    },
    async updateCommentId(rs) {
      var cmts = null

      await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId).then(r => {
        cmts = r
      })
      await PDFJSAnnotate.getStoreAdapter().getAnnotation(this.documentId, rs.data['uuid']).then(async anno => {
        anno.id = rs.data['annotationId']
        await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, anno.uuid, anno, 'added')
      })
      cmts.map(r => {
        if (r.uuid == rs.data['uuid']) {
          r.id = rs.data['id']
        }
      })
      await PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)
    },
    updateRedoList() {
      this.$refs.toolBar.updateRedoList(this.redoHistory)
    },
    updateUndoList() {
      this.$refs.toolBar.updateToolbarUndoList(this.undoHistory)
    },
    disableToolbarSubmit() {
      this.$refs.toolBar.disableSubmit()
      this.$refs.tabRubric.disableRubric()
    },
    isInShowMoreList(e) {
      if (this.showMoreList.filter(r => { return r.id == e }).length > 0 && this.isEditing != e) {
        return this.showMoreList.filter(r => { return r.id == e })[0]
      }
      return false
    },
    async toggleShowMore(uuid) {
      this.showMoreList.map(r => {
        if (r.id == uuid) {
          r.value = r.value == 0 ? 1 : 0
        }
      })
      const highlight = document.querySelector("[data-pdf-annotate-id='" + uuid + "']")
      setTimeout(async() => {
        await this.handleCommentAnnotationClick(highlight)
      }, 500)
    },
    editFreeText() {
      editTextBox(this.isTextbox, this.svg)
      // 123
    },
    async addRectComment() {
      if (document.getElementById('comment-wrapper').offsetWidth == 0) {
        document.getElementById('comment-wrapper').style.display = 'block'
      }
      this.target = this.annotationClicked
      const uuid = this.annotationClicked.getAttribute('data-pdf-annotate-id')
      this.annotationClicked.setAttribute('data-pdf-annotate-type', 'comment-area')
      await PDFJSAnnotate.getStoreAdapter().getAnnotation(this.documentId, uuid).then(r => {
        this.annotation = r
      })

      // this.annotation = await this.loadedAnnotation.annotations.filter(r => { return r.uuid === uuid })[0]
      const rect = this.annotation
      if (!this.svg) {
        this.svg = document.querySelector(`svg[data-pdf-annotate-page='${rect.page}']`)
      }
      // await this.updateTypeOfRect('comment-area')
      this.commentText(rect)
    },
    async updateTypeOfRect(typeAnno) {
      // var anno = null
      var uuid = this.annotationClicked.getAttribute('data-pdf-annotate-id')
      // var type = this.annotationClicked.getAttribute('data-pdf-annotate-type')
      this.annotation.type = typeAnno
      // if (type == 'area') {
      //   this.annotationClicked.setAttribute('data-pdf-annotate-type', typeAnno)
      //   anno = {
      //     Id: this.annotation.id,
      //     DocumentId: this.documentId,
      //     ReviewId: this.reviewId,
      //     Type: typeAnno,
      //     Color: this.annotation.color,
      //     Uuid: this.annotation.uuid,
      //     PageNum: this.annotation.pageNum,
      //     Top: this.annotation.top,
      //     Data: JSON.stringify(this.annotation)
      //   }
      // }

      await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, uuid, this.annotation, undefined)

      // reviewService.editAnnotation(anno).then(rs => {
      // })
    },
    disableToolbarButtons() {
      this.activeButton = 'cursor'
      this.$refs.toolBar.disableButtons()
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
.show__more-container{
  font-size: 0.8em;
  color: blue;
  text-decoration: underline;
  cursor: pointer;
  width: fit-content;
}
.free-text__actiion{
  cursor: cell;
}
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
.freeText .page .textLayer > div{
  cursor: crosshair !important;
}
</style>
