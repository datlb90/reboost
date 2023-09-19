<template>
  <div style="height: 100%;display: flex; flex-direction: column">
    <div id="parent-scroll" style="flex-grow: 1;position: relative;">
      <div id="child-scroll" class="par-content default">
        <div id="rubric">
          <div style="height: 100%; overflow: auto;">
            <el-card
              v-for="(criteria, criteriaIndex) in rubricCriteria"
              :key="criteria.id"
              style="margin-bottom: 5px; margin-left: 3px;"
            >
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
                    style="min-width: 240px; display:flex; justify-content: space-around;margin-bottom:10px;"
                    :disabled="readOnly || currentUser.role == 'Admin'"
                    @input="rubricMileStoneClick(reviewid, criteria, $event)"
                  >
                    <el-tooltip
                      v-for="milestone in criteria.bandScoreDescriptions.slice()"
                      :key="milestone.id"
                      style="margin:0;border:none;"
                      class="item"
                      effect="light"
                      placement="top"
                    >
                      <div slot="content" style="max-width: 500px;">
                        {{ milestone.description }}
                      </div>
                      <el-radio-button
                        id="mileStone"
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
                    :readonly="readOnly || currentUser.role == 'Admin'"
                    @focus="onFocus(criteria)"
                    @blur="onBlur($event, criteria)"
                    @input="reviewCommentChange(criteria.comment, criteria.id)"
                  />
                </div>
                <div v-if="!readOnly && currentUser.role != 'Admin' && criteria.isFocused && criteria.comment && criteria.comment.length > 0" style="margin-top: 10px;">
                  <el-button :id="'save-btn-' + criteria.id" type="primary" plain size="mini" @click="saveRubric(reviewid, criteria)">Save</el-button>
                </div>
              </div>
            </el-card>
          </div>
        </div>
      </div>
    </div>
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
    reviewid: { type: Number, default: null },
    currentUser: { type: Object, default: null }
  },
  data() {
    return {
      rubricCriteria: [],
      readOnly: false
    }
  },
  computed: {
  },
  async mounted() {
    this.loadRubric()
  },
  methods: {
    loadRubric() {
      console.log(this.reviewid)
      rubricService.getByQuestionId(this.questionid).then(rs => {
        this.rubricCriteria = rs.map(criteria => ({ ...criteria, mark: null, isFocused: false, comment: '' }))
        // Get rubric data from localstorage first
        var retrievedComment = localStorage.getItem('reviewRubricComment')
        var retrievedScore = localStorage.getItem('reviewRubricScore')
        retrievedComment = JSON.parse(retrievedComment)
        retrievedScore = JSON.parse(retrievedScore)
        // If there are existing comments or scores, load from localstorage
        if (retrievedComment || retrievedScore) {
          this.getLocaleStorageData()
        } else {
          // If there is nothing in localstorage, load from database
          reviewService.loadReviewFeedback(this.reviewid).then(rs => {
            if (rs.length > 0) {
              var comments = []
              var milestones = []
              rs.forEach(rc => {
                this.rubricCriteria.map(criteria => {
                  if (criteria.id == rc.criteriaId) {
                    criteria.comment = rc.comment
                    criteria.mark = rc.score
                  }
                })
                var cmt = { id: rc.criteriaId, content: rc.comment, documentId: this.documentId, reviewid: this.reviewid, questionid: this.questionid }
                comments.push(cmt)

                var ms = { id: rc.criteriaId, content: rc.score, documentId: this.documentId, reviewid: this.reviewid, questionid: this.questionid }
                milestones.push(ms)
              })
              if (this.currentUser.role !== 'Admin') { this.readOnly = false } else {
                this.readOnly = false
              }
              // Set localstorage so we don't have to load from db again
              localStorage.setItem('reviewRubricComment', JSON.stringify(comments))
              localStorage.setItem('reviewRubricScore', JSON.stringify(milestones))
            }
          })
        }
      })
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
            if (criteria.id == rc.id && rc.documentId == this.documentId && rc.reviewid == this.reviewid) {
              criteria.comment = rc.content
            }
          }
          )
        })
      }
      retrievedScore?.forEach(rc => {
        this.rubricCriteria.map(criteria => {
          if (criteria.id == rc.id && rc.documentId == this.documentId && rc.reviewid == this.reviewid) {
            criteria.mark = rc.content
          }
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
        var temp = retrievedObject?.filter(r => r.id == criteria.id && r.documentId == this.documentId && r.reviewid == this.reviewid)
        if (temp && temp.length > 0) {
          retrievedObject.map(r => {
            if (r.id == criteria.id) {
              r.content = mileStone
              r.documentId = this.documentId
              r.reviewid = this.reviewid
              r.questionid = this.questionid
            }
          })
        } else {
          var cmt = { id: criteria.id, content: mileStone, documentId: this.documentId, reviewid: this.reviewid, questionid: this.questionid }
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
        var temp = retrievedObject.filter(r => r.id == criteriaId && r.documentId == this.documentId && r.reviewid == this.reviewid)
        if (temp.length > 0) {
          retrievedObject.map(r => {
            if (r.id == criteriaId) {
              r.content = e
              r.documentId = this.documentId
              r.reviewid = this.reviewid
              r.questionid = this.questionid
            }
          })
        } else {
          var cmt = { id: criteriaId, content: e, documentId: this.documentId, reviewid: this.reviewid, questionid: this.questionid }
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
        console.log('invalid data')
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
