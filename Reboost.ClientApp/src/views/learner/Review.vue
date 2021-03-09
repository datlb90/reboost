<template>
  <div id="reviewContainer" style="border-top: 1px solid rgb(223 224 238); padding: 5px; background: rgb(248, 249, 250);">

    <!-- <el-card class="box-card" style="margin-bottom: 5px;">
      <div>
        <p style="width: 98%;">
          <b>TOEFL Independent Writing Topic 1</b> - Do you agree or disagree with the following statement?
          Always telling the truth is the most important consideration in any relationship. Use specific reasons and examples to support your answer.
        </p>

        <p style="width: 98%;">
          <b>Reading:</b> "A child’s education has never been about learning information and basic skills only. It has always included teaching the next generation how to be good members of society. Therefore, this cannot be the responsibility of the parents alone.

          In order to be a good member of any society the individual must respect and obey the rules of their community and share their values. Educating children to understand the need to obey rules and respect others always begins in the home and is widely thought to be the responsibility of parents. They will certainly be the first to help children learn what is important in life, how they are expected to behave and what role they will play in their world.

          However, learning to understand and share the value system of a whole society cannot be achieved just in the home. Once a child goes to school, they are entering a wider community where teachers and peers will have just as much influence as their parents do at home. At school, children will experience working and living with people from a whole variety of backgrounds from the wider society. This experience should teach them how to co-operate with each other and how to contribute to the life of their community.

          But to be a valuable member of any community is not like learning a simple skill. It is something that an individual goes on learning throughout life and it is the responsibility of every member of a society to take responsibility for helping the younger generation to become active and able members of that society."

        </p>

      </div>
    </el-card> -->

    <div id="content-wrapper" style="background: rgb(248, 249, 250); height: 92.5vh; width: 100%;position: absolute; overflow: auto;">
      <!-- <div class="tip">
        <p>
          <b>TOEFL Independent Writing Topic #1</b> - Do you agree or disagree with the following statement? Always telling the truth is the most important consideration in any relationship. Use specific reasons and examples to support your answer.
        </p>
      </div> -->

      <!-- <el-tag
        v-if="showDirection"
        key="direction"
        closable
        type=""
        :disable-transitions="false"

      >
        <p>
          <b>Direction:</b> Read the question description of the left and provide feedback to the response on the write. Both in-text comments and rubric feedback are required.
        </p>
      </el-tag> -->

      <div id="left-panel" :class="{'hideQuestion': !showQuestion}" style="float: left; width: 30%; position: sticky; top: 0px; min-width: 320px; height: 100%;">
        <el-button :disabled="readOnly" type="primary" size="mini" style="position: absolute; right: 8px;top: 6px;z-index: 1;" @click="submitReview()">Submit Review</el-button>
        <el-tabs type="border-card">
          <el-tab-pane label="Question">
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
                    <p>{{ getQuestion.content }}</p></div>
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
          </el-tab-pane>
          <el-tab-pane label="Rubric">
            <div style="height: 100%;display: flex; flex-direction: column">
              <div id="parent-scroll" style="flex-grow: 1;position: relative;">
                <div id="child-scroll" class="par-content default">
                  <div id="rubric">
                    <div style="height: 100%; overflow: auto;">
                      <el-card v-for="(criteria, criteriaIndex) in rubricCriteria" :key="criteria.id" style="margin-bottom: 5px; margin-left: 3px;">
                        <div slot="header" class="clearfix">
                          <span style="font-size: 16px; font-weight: bold;">
                            {{ criteria.name }}
                          </span>
                          <div style="font-size: 14px; margin-top: 10px;">
                            {{ criteria.description }}
                          </div>
                        </div>
                        <div>
                          <div>
                            <el-radio-group
                              :id="criteria.id"
                              v-model="criteria.mark"
                              size="mini"
                              style="margin-bottom: 10px; min-width: 240px;"
                              :disabled="readOnly"
                              @input="rubricMileStoneClick(criteria.id, $event)"
                            >
                              <el-tooltip
                                v-for="milestone in criteria.bandScoreDescriptions.slice()"
                                :key="milestone.id"
                                class="item"
                                effect="light"
                                placement="top"
                              >
                                <div slot="content" style="max-width: 500px;">
                                  {{ milestone.description }}
                                </div>
                                <el-radio-button
                                  :key="milestone.id"
                                  :label="milestone.bandScore"
                                />
                              </el-tooltip>
                            </el-radio-group>
                          </div>
                          <div>
                            <el-input
                              v-model="criteria.comment"
                              :criteria-index="criteriaIndex"
                              type="textarea"
                              placeholder="Enter text here"
                              :rows="3"
                              :maxlength="8000"
                              class="criteria-comment"
                              :readonly="readOnly"
                              @input="reviewCommentChange(criteria.comment, criteria.id)"
                            />
                          </div>
                        </div>
                      </el-card>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane label="Help">Questions and Answers</el-tab-pane>
        </el-tabs>
      </div>

      <div id="right-panel" :class="{'hideQuestion':!showQuestion}" style="float: left; position: absolute; margin-left: max(325px, 30.5%); width: 69.2%; background: rgb(248, 249, 250);">
        <div id="tool-bar" class="toolbar">
          <div class="tool-group-buttons-container" style="">
            <div data-element="highlightToolGroupButton" class="tool-group-button">
              <div class="toolbar-btn-wrapper">
                <el-tooltip class="item" effect="dark" content="Hide Question & Rubric (H)" placement="bottom">
                  <button class="toolbar-btn" @click="hideQuestion">
                    <div :class="{'hideQuestion':!showQuestion}" class="icon">
                      <i class="toolbar-icon fas fa-angle-double-left" />
                    </div>
                  </button>
                </el-tooltip>

              </div>

              <div class="toolbar-btn-wrapper">
                <el-tooltip class="item" effect="dark" content="Note (N)" placement="bottom">
                  <button class="toolbar-btn" data-tooltype="note" type="button" @click="toolBarButtonClick('note')">
                    <div class="icon">
                      <i class="toolbar-icon far fa-sticky-note" />
                    </div>
                  </button>
                </el-tooltip>

              </div>
              <div class="toolbar-btn-wrapper">
                <el-tooltip class="item" effect="dark" content="Free Text (T)" placement="bottom">
                  <button class="toolbar-btn" data-tooltype="text" type="button" @click="toolBarButtonClick('text')">
                    <div class="icon">
                      <i class="toolbar-icon fas fa-pen-alt" />
                    </div>
                  </button>
                </el-tooltip>
              </div>

              <div class="toolbar-btn-wrapper">
                <el-tooltip class="item" effect="dark" content="Rectangle (R)" placement="bottom">
                  <button class="toolbar-btn" data-tooltype="rectangle" type="button" @click="toolBarButtonClick('rectangle')">
                    <div class="icon">
                      <i class="far fa-square" />
                    </div>
                  </button>
                </el-tooltip>

              </div>
            </div>

            <el-dropdown size="mini" split-button style="margin-left: 10px;" @command="handleScale">
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

          </div>

          <!-- <button class="cursor" type="button" title="Cursor" data-tooltype="cursor">
            ➚
          </button>

          <div class="spacer" />

          <button class="rectangle" type="button" title="Rectangle" data-tooltype="area" />
          <button class="highlight" type="button" title="Highlight" data-tooltype="highlight" />
          <button class="strikeout" type="button" title="Strikeout" data-tooltype="strikeout" />

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
          </button> -->

        </div>
        <div
          id="viewer"
          class="pdfViewer"
          :document-id="documentId"
        />
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
import http from '@/utils/axios'
import reviewService from '@/services/review.service.js'
import rubricService from '@/services/rubric.service.js'

export default {
  name: 'Document',
  data() {
    return {
      rubricCriteria: [],
      criteriaFeedback: {},
      showDirection: true,
      viewer: null,
      PAGE_HEIGHT: 1,
      NUM_PAGES: 0,
      documentId: 12,
      reviewId: 1,
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
      isShowScript: false,
      questionId: 1,
      loadCompleted: false,
      readOnly: false,
      showQuestion: true,
      scaleRatio: 1,
      defaultScale: 1,
      scaleText: '100%',
      currentTooltype: null
    }
  },
  computed: {

    loadedAnnotation() {
      const data = this.$store.getters['review/getAnnotation']
      if (!data) {
        return null
      }

      return {
        annotations: data.annotations.map(a => JSON.parse(a.data)),
        comments: data.comments.map(c => ({ ...JSON.parse(c.data), documentId: this.documentId }))
      }
    },
    getDataQuestionParts() {
      return this.$store.getters['question/getSelected']['questionsPart']
    },
    getTranscript() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Transcript')) {
          return this.getDataQuestionParts.find(u => u.name == 'Transcript')
        }
      }
      return ''
    },
    getQuestionSection() {
      if (this.$store.getters['question/getSelected']) {
        console.log('this.$store.getters', this.$store.getters['question/getSelected'])
        return this.$store.getters['question/getSelected']['test'] + ' ' + this.$store.getters['question/getSelected']['section'] + ' - ' + this.$store.getters['question/getSelected']['title']
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
    window['PDFJSAnnotate'] = PDFJSAnnotate
    window['APP'] = this
    PDFJSAnnotate.getStoreAdapter().clearAnnotations(this.documentId)
    if (localStorage.getItem('showQuestionDirection')) {
      this.showDirection = false
    }
    // Render stuff
    this.$store.dispatch('review/loadReviewAnnotation', { docId: this.documentId, reviewId: this.reviewId }).then(() => {
      PDFJSAnnotate.getStoreAdapter().loadAnnotations(this.documentId, this.loadedAnnotation)

      this.render()
      this.TextStuff()
      this.ToolbarButtons()
      this.ClearToolbarButton()
      // this.ScaleAndRotate();
      // this.ClearToolbarButton();
    })
    this.$store.dispatch('question/loadQuestion', +this.questionId).then(rs => {
      this.calculateContainerHeight()
      this.getDataQuestionParts()
    })

    const renderedPages = {}
    const self = this
    document
      .getElementById('content-wrapper')
      .addEventListener('scroll', function(e) {
        // document.getElementById('left-panel').style.top = e.target.scrollTop + 'px'
        // eslint-disable-next-line no-console
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
    window.addEventListener('resize', this.calculateContainerHeight.bind(this))
    this.setIntervalForScroll = setInterval(() => {
      this.calculateStylePaddingScroll()
    }, 80)

    this.loadRubric()
  },
  beforeCreate: function() {
    document.body.style = 'overflow: hidden'
  },
  created() {
    window.addEventListener('resize', this.resizeEventHandle)
  },
  destroyed() {
    clearInterval(this.setIntervalForScroll)
    window.removeEventListener('resize', this.resizeEventHandle)
  },
  methods: {
    loadRubric() {
      rubricService.getByQuestionId(this.questionId).then(rs => {
        // this.criteriaFeedback['id']
        this.rubricCriteria = rs.map(criteria => ({ ...criteria, mark: null, comment: '' }))
        reviewService.loadReviewFeedback(this.reviewId).then(rs => { // Hardcode ReviewId
          console.log('loadReviewFeedback', rs)
          if (rs.length > 0) {
            rs.forEach(rc => {
              this.rubricCriteria.map(criteria => {
                if (criteria.id == rc.criteriaId) {
                  criteria.comment = rc.comment
                  criteria.mark = rc.score
                }
              }
              )
            })
            this.readOnly = true
          } else {
            this.getLocaleStorageData()
          }
        })
      })
    },
    rubricMileStoneClick(id, mileStone) {
      var retrievedObject = localStorage.getItem('reviewScore')
      if (!retrievedObject) {
        var t = []
        localStorage.setItem('reviewScore', JSON.stringify(t))
      }

      retrievedObject = JSON.parse(retrievedObject)
      var temp = retrievedObject.filter(r => r.id == id)
      if (temp.length > 0) {
        retrievedObject.map(r => {
          if (r.id == id) {
            r.content = mileStone
          }
        })
      } else {
        var cmt = { id: id, content: mileStone }
        retrievedObject.push(cmt)
      }
      localStorage.setItem('reviewScore', JSON.stringify(retrievedObject))
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

        page.setAttribute('style', `overflow:hidden;width:${docWidth}px;`)
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

      const viewer = document.getElementById('viewer')
      const commentWrapper = document.getElementById('comment-wrapper')
      commentWrapper.style.left = this.commentleftPos + 'px'
      viewer.appendChild(commentWrapper)

      this.comments = await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId)
      await this.comments.forEach(function(element) {
        element.isSelected = false
      })
      await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      this.handleCommentPositionsRestore()
      this.TextSelection()
      this.documentWidthCal()
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
      const tooltype =
        localStorage.getItem(`${self.RENDER_OPTIONS.documentId}/tooltype`) ||
        'cursor'
      if (tooltype) {
        setActiveToolbarItem(
          tooltype,
          document.querySelector(`.toolbar button[data-tooltype=${tooltype}]`),
          this.currentTooltype
        )
        this.currentTooltype = tooltype
      }

      function setActiveToolbarItem(type, button, currentType) {
        const active = document.querySelector('.toolbar button.active')
        if (active) {
          active.classList.remove('active')
          switch (currentType) {
            case 'text':
              UI.disableText()
              break
            case 'point':
              UI.disablePoint()
              break
            case 'area':
            case 'highlight':
            case 'rectangle':
              UI.disableRect()
              break
            case 'strikeout':
              UI.disableRect()
              break
          }
          if (type === currentType) {
            switch (tooltype) {
              case 'text':
                UI.disableText()
                break
              case 'point':
                UI.disablePoint()
                break
              case 'area':
              case 'highlight':
              case 'rectangle':
                UI.disableRect()
                break
              case 'strikeout':
                UI.disableRect()
                break
            }
          } else {
            button.classList.add('active')
            switch (tooltype) {
              case 'text':
                UI.enableText()
                break
              case 'point':
                UI.enablePoint()
                break
              case 'area':
              case 'highlight':
              case 'rectangle':
                UI.enableRect()
                break
              case 'strikeout':
                UI.enableRect()
                break
            }
          }
        } else {
          button.classList.add('active')
          switch (tooltype) {
            case 'text':
              UI.enableText()
              break
            case 'point':
              UI.enablePoint()
              break
            case 'area':
            case 'highlight':
            case 'rectangle':
              UI.enableRect()
              break
            case 'strikeout':
              UI.enableRect()
              break
          }
        }

        // const active = document.querySelector('.toolbar button.active')

        // if (active && tooltype === type) {
        //   active.classList.remove('active')
        //   switch (tooltype) {
        //     case 'text':
        //       UI.disableText()
        //       break
        //     case 'point':
        //       UI.disablePoint()
        //       break
        //     case 'area':
        //     case 'highlight':
        //     case 'strikeout':
        //       UI.disableRect()
        //       break
        //   }
        // } else {
        //   if (!active) {
        //     if (button) {
        //       button.classList.add('active')
        //     }

        //     if (tooltype !== type) {
        //       localStorage.setItem(
        //         `${self.RENDER_OPTIONS.documentId}/tooltype`,
        //         type
        //       )
        //       tooltype = type
        //       switch (type) {
        //         case 'text':
        //           UI.enableText()
        //           break
        //         case 'point':
        //           UI.enablePoint()
        //           break
        //         case 'area':
        //         case 'highlight':
        //         case 'strikeout':
        //           UI.enableRect(type)
        //           break
        //       }
        //     }
        //   }
        // }
      }

      // function handleToolbarClick(e) {
      //   if (e.target.nodeName === 'BUTTON') {
      //     setActiveToolbarItem(
      //       e.target.getAttribute('data-tooltype'),
      //       e.target
      //     )
      //   }
      // }

      // document
      //   .querySelector('.toolbar')
      //   .addEventListener('click', handleToolbarClick)
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

          let gTop = parseInt(target.getAttribute('top')) - 34
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
      this.handleCommentAnnotationClick(highlightArr[highlightArr.length - 1])
      this.handleCommentAnnotationClick(highlightArr[0])
      const selected = document.getElementsByClassName('comment-card-selected')
      if (selected.length >= 0) { selected[0].classList.remove('comment-card-selected') }
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
      newComment.annotation.documentId = this.documentId
      var anno = {
        DocumentId: this.documentId,
        ReviewId: this.reviewId,
        Type: 'comment-highlight',
        PageNum: this.annotation.pageNum,
        Top: this.annotation.top,
        Color: this.annotation.color,
        Uuid: this.annotation.uuid,
        Data: JSON.stringify(this.annotation)
      }
      var newCmt = {
        Text: newComment.text,
        Content: newComment.content,
        TopPosition: newComment.topPosition,
        Uuid: newComment.uuid,
        Data: JSON.stringify(newComment)
      }
      reviewService.addInTextComment(this.documentId, 1, newCmt, anno).then(rs => {
      })
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
            this.$store.dispatch('review/addReviewAnnotation', obj).then(rs => {
            })
          }
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
      newCommentWrapper.style = 'width: 100%; position: absolute; left: -20px; top: ' + topPos + 'px;'
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
    },
    async submitReview() {
      // Promise.all([
      //   PDFJSAnnotate.getStoreAdapter().getAnnotations(this.documentId, this.reviewId),
      //   PDFJSAnnotate.getStoreAdapter().getComments(this.documentId, this.reviewId)
      // ]).then(([{ annotations }, comments]) => {
      //   reviewService.saveAnnotations(1, 1, {
      //     annotations: annotations.map(a => ({ ...a, data: JSON.stringify(a) })),
      //     comments: comments.map(c => ({ ...c, data: JSON.stringify(c) }))
      //   }).then(rs => {
      //     this.$notify({
      //       title: 'Success',
      //       message: 'Submit success',
      //       type: 'success',
      //       duration: 1000
      //     })
      //   })
      // })

      const annots = await PDFJSAnnotate.getStoreAdapter().getAnnotations(this.documentId, this.reviewId)
      console.log(annots)

      // var reviewData = []
      // this.rubricCriteria.forEach(r => {
      //   if (r.mark) {
      //     reviewData.push({
      //       Comment: r.comment,
      //       CriteriaId: r.id,
      //       Score: r.mark,
      //       ReviewId: 2 // Hardcode
      //     })
      //   }
      // })
      // reviewService.loadReviewFeedback(2).then(rs => {
      //   console.log('review data: ', rs)
      //   localStorage.removeItem('reviewComment')
      // })
      // reviewService.saveReviewFeedback(reviewData).then(rs => {
      //   console.log('review data: ', rs)
      // })
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
        document.getElementById('child-scroll').style.paddingRight = '0'
      } else {
        document.getElementById('child-scroll').style.paddingRight = '10px'
      }
    },
    notShowDirection() {
      localStorage.setItem('showQuestionDirection', true)
      this.showDirection = false
    },
    reviewCommentChange(e, criteriaId) {
      var retrievedObject = localStorage.getItem('reviewComment')
      if (!retrievedObject) {
        var t = []
        localStorage.setItem('reviewComment', JSON.stringify(t))
      }

      retrievedObject = JSON.parse(retrievedObject)
      var temp = retrievedObject.filter(r => r.id == criteriaId)
      if (temp.length > 0) {
        retrievedObject.map(r => {
          if (r.id == criteriaId) {
            r.content = e
          }
        })
      } else {
        var cmt = { id: criteriaId, content: e }
        retrievedObject.push(cmt)
      }
      localStorage.setItem('reviewComment', JSON.stringify(retrievedObject))
    },
    getLocaleStorageData() {
      console.log('this.rubricCriteria', this.rubricCriteria)
      var retrievedComment = localStorage.getItem('reviewComment')
      var retrievedScore = localStorage.getItem('reviewScore')

      retrievedComment = JSON.parse(retrievedComment)
      retrievedScore = JSON.parse(retrievedScore)

      retrievedComment.forEach(rc => {
        this.rubricCriteria.map(criteria => {
          if (criteria.id == rc.id) {
            criteria.comment = rc.content
          }
        }
        )
      })
      retrievedScore.forEach(rc => {
        this.rubricCriteria.map(criteria => {
          if (criteria.id == rc.id) {
            criteria.mark = rc.content
          }
        }
        )
      })
      console.log('this.rubricCriteria', this.rubricCriteria)
    },
    async hideQuestion() {
      this.showQuestion = !this.showQuestion
      document.getElementById('right-panel').style.width = 69.2 + '%'
      this.defaultScale = 1

      if (!this.showQuestion) {
        document.getElementById('right-panel').style.width = 100 + '%'
      }

      if (this.scaleText != 'Fit to width' && this.scaleText != 'Fit to page') {
        this.RENDER_OPTIONS.scale = this.defaultScale
        this.RENDER_OPTIONS.scale *= this.scaleRatio
      } else {
        (this.scaleText == 'Fit to width') ? this.handleScale('fitWidth') : this.handleScale('fitPage')
      }
      await UI.renderPage(1, this.RENDER_OPTIONS)
      this.documentWidthCal()
    },
    documentWidthCal() {
      var docWidth = document.getElementById('right-panel').offsetWidth - document.getElementById('comment-wrapper').offsetWidth - 15

      if (document.getElementsByClassName('canvasWrapper')[0].offsetWidth < docWidth) {
        docWidth = document.getElementsByClassName('canvasWrapper')[0].offsetWidth
      }

      const commentWrapper = document.getElementById('comment-wrapper')
      commentWrapper.style.left = docWidth + 'px'
      // document.getElementById('tool-bar').style.width = docWidth + 'px'

      for (let i = 0; i < this.NUM_PAGES; i++) {
        const page = document.getElementById(`pageContainer${i + 1}`)
        page.setAttribute('style', `overflow:hidden;width:${docWidth}px;`)
      }
    },
    async handleScale(e) {
      this.RENDER_OPTIONS.scale = this.defaultScale

      if (e != 'fitWidth' && e != 'fitPage') {
        this.RENDER_OPTIONS.scale *= (+e)
        this.scaleRatio = +e
        this.scaleText = this.scaleRatio * 100 + '%'
        await UI.renderPage(1, this.RENDER_OPTIONS)
      } else {
        var docWidth = document.getElementById('viewer').offsetWidth - document.getElementById('comment-wrapper').offsetWidth - 15
        var docHeight = document.getElementById('viewer').offsetHeight - document.getElementById('comment-wrapper').offsetWidth - 15

        // Default Layer width & height at 1: 595, 842

        if (e == 'fitWidth') {
          this.RENDER_OPTIONS.scale = (docWidth / 595)
          this.scaleText = 'Fit to width'
        } else {
          if ((docWidth / 595) < (docHeight / 842)) {
            this.RENDER_OPTIONS.scale = (docWidth / 595)
          } else {
            this.RENDER_OPTIONS.scale = (docHeight / 842)
          }
          this.scaleText = 'Fit to page'
        }
        console.log(e, 'this.RENDER_OPTIONS.scale', this.RENDER_OPTIONS.scale)
        await UI.renderPage(1, this.RENDER_OPTIONS)
      }
      this.documentWidthCal()
    },
    async increaseScale() {
      if (this.scaleRatio < 2) {
        this.scaleRatio += 0.1
        this.RENDER_OPTIONS.scale = this.defaultScale
        this.RENDER_OPTIONS.scale *= this.scaleRatio
        this.scaleText = parseInt(this.scaleRatio * 100) + '%'
        await UI.renderPage(1, this.RENDER_OPTIONS)
        this.documentWidthCal()
      }
    },
    async decreaseScale() {
      if (this.scaleRatio > 0.6) {
        this.scaleRatio -= 0.1
        this.RENDER_OPTIONS.scale = this.defaultScale
        this.RENDER_OPTIONS.scale *= this.scaleRatio
        this.scaleText = parseInt(this.scaleRatio * 100) + '%'
        await UI.renderPage(1, this.RENDER_OPTIONS)
        this.documentWidthCal()
      }
    },
    async resizeEventHandle() {
      document.getElementById('right-panel').offsetWidth
      this.documentWidthCal()
      if (this.scaleText != 'Fit to width' && this.scaleText != 'Fit to page') {
        this.RENDER_OPTIONS.scale = this.defaultScale
        this.RENDER_OPTIONS.scale *= this.scaleRatio
      } else {
        (this.scaleText == 'Fit to width') ? this.handleScale('fitWidth') : this.handleScale('fitPage')
      }
    },
    toolBarButtonClick(e) {
      localStorage.setItem(`${this.RENDER_OPTIONS.documentId}/tooltype`, e)
      this.ToolbarButtons()
    }

    // End migration
  }
}
</script>

<style scoped>
@import '../../pdfjs/shared/document.css';
@import '../../pdfjs/shared/toolbar.css';
@import '../../pdfjs/shared/pdf_viewer.css';

.tip {
  padding: 8px 14px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
}
pre {
  font-size: 14px;
  text-align: justify;
  white-space: break-spaces;
  font-family: inherit !important;
  margin-bottom: 0 !important;
}
.rotate-icon {
  transform: rotateZ(180deg);
}

.par-content {
  position: absolute;
  padding-right: 10px;
  top: 0;
  left: 0;
  overflow-y: scroll;
  height: 100%;
  width: 100%;
  -ms-overflow-style: none;
  /* IE and Edge */
  scrollbar-width: none;
}

.par-content::-webkit-scrollbar {
  width: 7px;
}

/* Handle */
.par-content::-webkit-scrollbar-thumb {
  background: #999;
  border-radius: 4px;
}

/* Handle on hover */
.par-content::-webkit-scrollbar-thumb:hover {
  background: #777;
}
.default{
  padding: 0;
  margin: 0;
}
.content-con{
  margin: 10px 0 10px 0;
  padding: 10px;
  border: #dcddde solid 1px;
  border-radius: 5px;
}
.body-transcript {
  margin-top: 0;
  padding: 10px;
  border: 1px solid #e2e2e2;
}
.hideQuestion{
  visibility: hidden;
}
.pdfViewer .page{
  overflow: scroll !important;
  border: 1px solid #b8b8b8 !important;
}
#right-panel.hideQuestion{
  visibility: visible;
  margin-left: 0 !important;
}
.hideQuestion .fa-angle-double-left{
visibility: visible;
transform: rotate(180deg);
}
#viewer{
  border: 1px solid #d7dae1;
  padding: 10px 0 0 0;
}
</style>
