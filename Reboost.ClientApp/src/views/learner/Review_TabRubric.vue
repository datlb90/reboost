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
                    @input="rubricMileStoneClick(criteria.id, $event)"
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
      rubricService.getByQuestionId(this.questionid).then(rs => {
        // this.criteriaFeedback['id']
        this.rubricCriteria = rs.map(criteria => ({ ...criteria, mark: null, comment: '' }))
        reviewService.loadReviewFeedback(this.reviewid).then(rs => {
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
            if (this.currentUser.role !== 'Admin') { this.readOnly = false } else {
              this.readOnly = false
            }
          } else {
            this.getLocaleStorageData()
          }
        })
      })
    },
    getLocaleStorageData() {
      var retrievedComment = localStorage.getItem('reviewRubricComment')
      var retrievedScore = localStorage.getItem('reviewRubricScore')

      retrievedComment = JSON.parse(retrievedComment)
      retrievedScore = JSON.parse(retrievedScore)

      console.log('comment', retrievedComment)
      console.log('score', retrievedScore)

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
    rubricMileStoneClick(id, mileStone) {
      if (this.rubicCommentDelay) {
        clearTimeout(this.rubicCommentDelay)
      }
      this.rubicCommentDelay = setTimeout(() => {
        var retrievedObject = localStorage.getItem('reviewRubricScore')
        if (!retrievedObject) {
          var t = []
          localStorage.setItem('reviewRubricScore', JSON.stringify(t))
          retrievedObject = []
        }

        retrievedObject = JSON.parse(retrievedObject)
        var temp = retrievedObject?.filter(r => r.id == id && r.documentId == this.documentId && r.reviewid == this.reviewid)
        if (temp.length > 0) {
          retrievedObject.map(r => {
            if (r.id == id) {
              r.content = mileStone
              r.documentId = this.documentId
              r.reviewid = this.reviewid
              r.questionid = this.questionid
            }
          })
        } else {
          var cmt = { id: id, content: mileStone, documentId: this.documentId, reviewid: this.reviewid, questionid: this.questionid }
          retrievedObject.push(cmt)
        }
        localStorage.setItem('reviewRubricScore', JSON.stringify(retrievedObject))

        this.setStatusText('Saved')
      }, 20)
    },
    reviewCommentChange(e, criteriaId) {
      console.log('rubric comment changed')
      if (this.rubicCommentDelay) {
        clearTimeout(this.rubicCommentDelay)
      }
      this.rubicCommentDelay = setTimeout(() => {
        // var retrievedObject = localStorage.getItem('reviewRubricComment')
        // if (!retrievedObject) {
        //   var t = []
        //   localStorage.setItem('reviewRubricComment', JSON.stringify(t))
        //   retrievedObject = []
        // }

        // retrievedObject = JSON.parse(retrievedObject)
        // var temp = retrievedObject.filter(r => r.id == criteriaId && r.documentId == this.documentId && r.reviewid == this.reviewid)
        // if (temp.length > 0) {
        //   retrievedObject.map(r => {
        //     if (r.id == criteriaId) {
        //       r.content = e
        //       r.documentId = this.documentId
        //       r.reviewid = this.reviewid
        //       r.questionid = this.questionid
        //     }
        //   })
        // } else {
        //   var cmt = { id: criteriaId, content: e, documentId: this.documentId, reviewid: this.reviewid, questionid: this.questionid }
        //   retrievedObject.push(cmt)
        // }
        // localStorage.setItem('reviewRubricComment', JSON.stringify(retrievedObject))

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
