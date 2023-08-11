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
                    :disabled="true"
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
                    :readonly="true"
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
export default {
  props: {
    questionId: { type: Number, default: null },
    reviewId: { type: Number, default: null }
  },
  data: () => ({
    rubricCriteria: [],
    tableData: []
  }),
  mounted() {
    if (this.questionId && this.reviewId) {
      this.loadRubric()
    }
  },
  methods: {
    loadRubric() {
      rubricService.getByQuestionId(this.questionId).then(rs => {
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
          }
        })
      })
    }
  }
}
</script>
