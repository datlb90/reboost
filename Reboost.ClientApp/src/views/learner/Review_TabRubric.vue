<template>
  <div style="height: 100%;display: flex; flex-direction: column">
    <div id="parent-scroll" style="flex-grow: 1;position: relative;">
      <div id="child-scroll">
        <div id="rubric">
          <div style="height: 100%; overflow: auto; padding-bottom: 20px;">
            <el-card
              v-for="(criteria, criteriaIndex) in rubricCriteria"
              :key="criteria.id"
              style="margin-bottom: 5px; margin-left: 3px; border: 1px solid rgb(190, 190, 190);"
              shadow="hover"
            >
              <div slot="header" class="clearfix">
                <div>
                  <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; width: calc(100% - 90px); text-overflow: ellipsis;  word-break: break-word; overflow: hidden; white-space: nowrap;">
                    {{ criteria.name }}
                  </div>
                  <div style="float: right;">
                    <el-tooltip v-if="criteria.name != 'Overall Score & Feedback'" placement="right" effect="light" popper-class="rubric-description" sty>
                      <div slot="content">
                        <div>
                          <el-table
                            :data="criteria.bandScoreDescriptions"
                            border
                            style="width: 650px;"
                            height="800"
                          >
                            <el-table-column
                              prop="bandScore"
                              label="Band"
                              width="70"
                            />
                            <el-table-column
                              prop="description"
                              label="Description"
                            />
                          </el-table>
                        </div>
                      </div>
                      <el-button :id="'save-btn-' + criteria.id" type="info" plain icon="el-icon-info" size="mini">Rubric</el-button>
                    </el-tooltip>
                    <div v-if="criteria.name == 'Overall Score & Feedback'">
                      <el-select
                        v-model="criteria.mark"
                        placeholder="Band"
                        size="mini"
                        style="width: 82px;"
                        :readonly="readOnly || currentUser.role == 'Admin'"
                        :disabled="readOnly || currentUser.role == 'Admin'"
                        @change="rubricMileStoneClick(reviewId, criteria, $event)"
                      >
                        <el-option
                          v-for="item in scoreOptions"
                          :key="item.value"
                          :label="item.label"
                          :value="item.value"
                        />
                      </el-select>

                    </div>

                  </div>
                </div>
              </div>
              <div>
                <div v-if="criteria.name != 'Critical Errors' && criteria.name != 'Overall Score & Feedback'">
                  <el-radio-group
                    :id="criteria.id"
                    v-model="criteria.mark"
                    size="mini"
                    style="min-width: 240px; display:flex; justify-content: space-around;"
                    :disabled="readOnly || currentUser.role == 'Admin'"
                    @input="rubricMileStoneClick(reviewId, criteria, $event)"
                  >
                    <el-radio-button
                      v-for="milestone in criteria.bandScoreDescriptions.slice()"
                      id="mileStone"
                      :key="milestone.id"
                      :label="milestone.bandScore"
                    />
                  </el-radio-group>
                </div>
                <div>
                  <div v-if="criteria.loading" style="background: #f0f1f2; height: 120px; margin-top: 5px; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                    <div class="el-loading-spinner" style="position: relative; top: 10%;">
                      <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                      <p class="el-loading-text" style="word-break: break-word;">Đang chấm tiêu chí {{ criteria.name }}</p>
                    </div>
                  </div>
                  <div v-else>
                    <el-input
                      v-model="criteria.comment"
                      :criteria-index="criteriaIndex"
                      type="textarea"
                      :placeholder="isAiReview ? 'Cung cấp bình luận cho tiêu chí này. Bạn có thể đánh giá về những phần đã làm tốt và điểm cần cải thiện của bài viết.' : 'Vui lòng chờ trong giây lát'"
                      :autosize="{ minRows: 5, maxRows: 10 }"
                      :maxlength="8000"
                      class="criteria-comment"
                      :readonly="readOnly || currentUser.role == 'Admin'"
                      @focus="onFocus(criteria)"
                      @blur="onBlur($event, criteria)"
                      @input="reviewCommentChange(criteria.comment, criteria.id)"
                    />
                  </div>

                </div>

              </div>
            </el-card>
          </div>
        </div>
      </div>
    </div>

    <el-dialog
      v-if="selectedCriteria"
      title="Rubric Criteria Description"
      :visible.sync="dialogVisible"
      width="50%"
      :before-close="handleClose"
      style="z-index: 100000;"
    >
      <span>RubricName</span>
      <div>
        <el-table
          :data="selectedCriteria.bandScoreDescriptions"
          border
          style="width: 900px"
        >
          <el-table-column
            prop="bandScore"
            label="Band"
            width="70"
          />
          <el-table-column
            prop="description"
            label="Desciption"
          />
        </el-table>
      </div>

    </el-dialog>

  </div>
</template>
<script>

import reviewService from '@/services/review.service.js'
import rubricService from '@/services/rubric.service.js'
export default ({
  name: 'TabRubric',
  components: {

  },
  props: {
    questionid: { type: Number, default: null },
    reviewId: { type: Number, default: null },
    currentUser: { type: Object, default: null },
    isAiReview: { type: Boolean, default: false },
    documentText: { type: String, default: null },
    feedbackLanguage: { type: String, default: null }
  },
  data() {
    return {
      rubricCriteria: [],
      readOnly: false,
      dialogVisible: false,
      selectedCriteria: null,
      scoreOptions: null,
      toeflScores: [
        {
          value: 5,
          label: '5.0'
        }, {
          value: 4.5,
          label: '4.5'
        }, {
          value: 4,
          label: '4.0'
        }, {
          value: 3.5,
          label: '3.5'
        }, {
          value: 3,
          label: '3.0'
        }, {
          value: 2.5,
          label: '2.5'
        }, {
          value: 2,
          label: '2.0'
        }, {
          value: 1.5,
          label: '1.5'
        }, {
          value: 1,
          label: '1.0'
        }, {
          value: 0,
          label: '0'
        }
      ],
      ieltsSCores: [
        {
          value: 9,
          label: '9.0'
        }, {
          value: 8.5,
          label: '8.5'
        }, {
          value: 8,
          label: '8.0'
        }, {
          value: 7.5,
          label: '7.5'
        }, {
          value: 7,
          label: '7.0'
        }, {
          value: 6.5,
          label: '6.5'
        }, {
          value: 6,
          label: '6.0'
        }, {
          value: 5.5,
          label: '5.5'
        }, {
          value: 5,
          label: '5.0'
        }, {
          value: 4.5,
          label: '4.5'
        }, {
          value: 4,
          label: '4.0'
        }, {
          value: 3.5,
          label: '3.5'
        }, {
          value: 3,
          label: '3.0'
        }, {
          value: 2.5,
          label: '2.5'
        }, {
          value: 2,
          label: '2.0'
        }, {
          value: 1.5,
          label: '1.5'
        }, {
          value: 1,
          label: '1.0'
        }, {
          value: 0,
          label: '0'
        }
      ]
    }
  },
  computed: {
  },
  async mounted() {
    console.log(this.isAiReview)
    this.loadRubric()
  },
  methods: {
    showDescriptionDialog(criteria) {
      var rightPanel = document.getElementById('right-panel')
      rightPanel.style.display = 'none'
      this.selectedCriteria = criteria
      this.dialogVisible = true
    },
    async loadRubric() {
      localStorage.removeItem('reviewRubricComment')
      localStorage.removeItem('reviewRubricScore')
      const rs = await rubricService.getByQuestionId(this.questionid)
      if (rs) {
        this.rubricCriteria = rs.map(criteria => ({ ...criteria, mark: null, isFocused: false, comment: '', loading: true}))
        if (this.rubricCriteria[0] && this.rubricCriteria[0].bandScoreDescriptions.length == 6) { this.scoreOptions = this.toeflScores } else { this.scoreOptions = this.ieltsSCores }
        // Get rubric data from localstorage first
        var hasComment = false
        var retrievedComment = localStorage.getItem('reviewRubricComment')
        retrievedComment = JSON.parse(retrievedComment)
        if (retrievedComment) {
          hasComment = retrievedComment.some(r => r.reviewid == this.reviewId)
        } else {
          retrievedComment = []
        }
        var hasScore = false
        var retrievedScore = localStorage.getItem('reviewRubricScore')
        retrievedScore = JSON.parse(retrievedScore)
        if (retrievedScore) {
          hasScore = retrievedScore.some(r => r.reviewid == this.reviewId)
        } else {
          retrievedScore = []
        }
        // If there are existing comments or scores, load from localstorage
        if (hasComment || hasScore) {
          this.getLocaleStorageData()
        } else {
          // If there is nothing in localstorage, load from database
          const reviewFeedback = await reviewService.loadReviewFeedback(this.reviewId)
          if (reviewFeedback && reviewFeedback.length > 0) {
            reviewFeedback.forEach(rc => {
              this.rubricCriteria.map(criteria => {
                if (criteria.id == rc.criteriaId) {
                  criteria.comment = rc.comment
                  criteria.mark = rc.score
                }
                criteria.loading = false
              })
              var cmt = { id: rc.criteriaId, content: rc.comment, reviewid: this.reviewId, questionid: this.questionid }
              retrievedComment.push(cmt)

              var ms = { id: rc.criteriaId, content: rc.name == 'Critical Errors' ? 0 : rc.score, reviewid: this.reviewId, questionid: this.questionid }
              retrievedScore.push(ms)
            })
            if (this.currentUser.role !== 'Admin') { this.readOnly = false } else {
              this.readOnly = true
            }
            // Set localstorage so we don't have to load from db again
            localStorage.setItem('reviewRubricComment', JSON.stringify(retrievedComment))
            localStorage.setItem('reviewRubricScore', JSON.stringify(retrievedScore))
          } else {
            if (this.isAiReview) {
                console.log('Get feedback now')
                const question = this.$store.getters['question/getSelected']
                const topic = question.questionsPart.find(q => q.name == 'Question').content
                let chartDescription = ''
                const chart = question.questionsPart.find(q => q.name == 'Chart')
                if (chart) {
                  chartDescription = await reviewService.getChartDescription(chart.content)
                }
                let completedCount = 0
                this.rubricCriteria.forEach(criteria => {
                  const model = {
                    task: question.section,
                    topic: topic,
                    essay: this.documentText,
                    criteriaName: criteria.name,
                    feedbackLanguage: this.feedbackLanguage,
                    chartDescription: chartDescription
                  }

                  reviewService.getAIFeedbackForCriteria(model).then(rs => {
                    criteria.loading = false
                    criteria.comment = rs.comment
                    criteria.mark = rs.bandScore
                    completedCount++
                    if (completedCount == 5) {
                      // update overall band score
                      const fourCriteria = this.rubricCriteria.filter(c => c.name != 'Overall Score & Feedback')
                      const index = this.rubricCriteria.findIndex(c => c.name == 'Overall Score & Feedback')
                      const average = fourCriteria.reduce((total, next) => total + next.mark, 0) / 4
                      this.rubricCriteria[index].mark = (Math.round(average * 2) / 2).toFixed(1)

                      // Save the criteria synchronously
                      var reviewData = []
                      this.rubricCriteria.forEach(r => {
                        if (r.mark) {
                          reviewData.push({
                            CriteriaName: r.name,
                            Comment: r.comment,
                            CriteriaId: r.id,
                            Score: r.mark,
                            ReviewId: this.reviewId
                          })
                        }
                      })

                      reviewService.saveReviewFeedback(this.reviewId, reviewData)
                    }
                  })
                })
              } else {
                this.rubricCriteria.forEach(criteria => {
                  console.log(criteria)
                  criteria.loading = false
                })
              }
          }
        }
      } else {
        console.log('Error: rubric cannot be found!')
      }

      console.log('Rubric Criteria: ', this.rubricCriteria)
    },
    onFocus(criteria) {
      criteria.isFocused = true
    },
    onBlur(event, criteria) {
      const target = event.relatedTarget
      if (target) {
        const targetId = target.id
        const thisId = 'save-btn-' + criteria.id
        if (targetId === thisId) {
          return
        }
      }
      criteria.isFocused = false
    },
    saveRubric(reviewId, criteria) {
      this.getLocaleStorageData()
      var reviewData = []
      this.rubricCriteria.forEach(r => {
        reviewData.push({
          Comment: r.comment,
          CriteriaId: r.id,
          Score: r.mark,
          ReviewId: reviewId
        })
      })
      reviewService.saveRubric(reviewId, reviewData).then(rs => {
        if (rs) {
          this.setStatusText('Saved')
        }
        criteria.isFocused = false
      })
    },
    getLocaleStorageData() {
      var retrievedComment = localStorage.getItem('reviewRubricComment')
      var retrievedScore = localStorage.getItem('reviewRubricScore')

      retrievedComment = JSON.parse(retrievedComment)
      retrievedScore = JSON.parse(retrievedScore)

      // console.log('comment', retrievedComment)
      // console.log('score', retrievedScore)

      if (retrievedComment) {
        retrievedComment.forEach(rc => {
          this.rubricCriteria.map(criteria => {
            if (criteria.id == rc.id && rc.reviewid == this.reviewId) {
              criteria.comment = rc.content
            }
            criteria.loading = false
          })
        })
      }
      retrievedScore?.forEach(rc => {
        this.rubricCriteria.map(criteria => {
          if (criteria.id == rc.id && rc.reviewid == this.reviewId) {
            criteria.mark = criteria.name == 'Critical Errors' ? 0 : rc.content
          }
          criteria.loading = false
        })
      })
    },
    rubricMileStoneClick(reviewId, criteria, mileStone) {
      if (this.rubicCommentDelay) {
        clearTimeout(this.rubicCommentDelay)
      }
      this.rubicCommentDelay = setTimeout(() => {
        var retrievedObject = localStorage.getItem('reviewRubricScore')
        if (!retrievedObject) {
          var t = []
          localStorage.setItem('reviewRubricScore', JSON.stringify(t))
          retrievedObject = '[]'
        }

        retrievedObject = JSON.parse(retrievedObject)
        var temp = retrievedObject?.filter(r => r.id == criteria.id && r.reviewid == this.reviewId)
        if (temp && temp.length > 0) {
          retrievedObject.map(r => {
            if (r.id == criteria.id && r.reviewid == this.reviewId) {
              r.content = mileStone
              r.reviewid = this.reviewId
              r.questionid = this.questionid
            }
          })
        } else {
          var cmt = { id: criteria.id, content: mileStone, reviewid: this.reviewId, questionid: this.questionid }
          retrievedObject.push(cmt)
        }
        localStorage.setItem('reviewRubricScore', JSON.stringify(retrievedObject))

        this.setStatusText('Saved')

        this.saveRubric(reviewId, criteria)
      }, 20)
    },
    reviewCommentChange(e, criteriaId) {
      if (this.rubicCommentDelay) {
        clearTimeout(this.rubicCommentDelay)
      }
      this.rubicCommentDelay = setTimeout(() => {
        var retrievedObject = localStorage.getItem('reviewRubricComment')
        if (!retrievedObject) {
          var t = []
          localStorage.setItem('reviewRubricComment', JSON.stringify(t))
          retrievedObject = '[]'
        }

        retrievedObject = JSON.parse(retrievedObject)
        var temp = retrievedObject.filter(r => r.id == criteriaId && r.reviewid == this.reviewId)
        if (temp.length > 0) {
          retrievedObject.map(r => {
            if (r.id == criteriaId && r.reviewid == this.reviewId) {
              r.content = e
              r.reviewid = this.reviewId
              r.questionid = this.questionid
            }
          })
        } else {
          var cmt = { id: criteriaId, content: e, reviewid: this.reviewId, questionid: this.questionid }
          retrievedObject.push(cmt)
        }
        localStorage.setItem('reviewRubricComment', JSON.stringify(retrievedObject))

        this.setStatusText('Saved')
      }, 20)
    },
    setStatusText(label) {
      this.$emit('setStatusText')
    },
    getRubricData() {
      var invalidData = this.rubricCriteria.filter(r => { return r.comment === '' || r.mark == null })
      if (invalidData.length > 0) {
        return false
      }
      return this.rubricCriteria
    },
    disableRubric() {
      this.readOnly = true
    }
  }
})
</script>

<style scoped>
@import '../../styles/review.css';
</style>

<style>
.el-tabs--border-card>.el-tabs__content{
  padding: 10px !important;
}
.el-radio-button__orig-radio:disabled:checked+.el-radio-button__inner{
  color: #FFF !important;
  background-color: #409EFF !important;
  border-color: #409EFF !important;
  -webkit-box-shadow: -1px 0 0 0 #409EFF !important;
  box-shadow: -1px 0 0 0 #409EFF !important;
}
.rubric-description > table{
  width: 800px;
}
</style>
