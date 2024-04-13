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
                  <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                    <span v-if="criteria.name == 'Critical Errors'">Nâng Cấp Từ Vựng Và Ngữ Pháp</span>
                    <span v-else-if="criteria.name == 'Arguments Assessment'">Củng Cố Lập Luận</span>
                    <span v-else-if="criteria.name == 'Overall Score & Feedback'">Đánh Giá Tổng Quan</span>
                    <span v-else> Tiêu Chí {{ criteria.name }}</span>
                  </div>
                </div>
              </div>
              <div>
                <div v-if="criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment'">
                  <div v-if="isAiReview">
                    <div v-if="!criteria.loading && criteria.mark" class="band-score">
                      Band:
                      {{ criteria.mark.toString().length == 1 ? criteria.mark.toString() + '.0' : criteria.mark }}
                    </div>
                  </div>
                  <div v-else>
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

                </div>
                <!-- <div v-if="criteria.name == 'Overall Score & Feedback'">
                  <el-select
                    v-model="criteria.mark"
                    placeholder="Band score"
                    size="mini"
                    style="width: 110px; margin-bottom: 10px;"
                    :readonly="readOnly || currentUser.role == 'Admin'"
                    :disabled="true"
                    @change="rubricMileStoneClick(reviewId, criteria, $event)"
                  >
                    <el-option
                      v-for="item in scoreOptions"
                      :key="item.value"
                      :label="'Band: ' + item.label"
                      :value="item.value"
                    />
                  </el-select>
                </div> -->
                <div>
                  <div v-if="criteria.loading" style="background: #f0f1f2; height: 120px; margin-top: 5px; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                    <div class="el-loading-spinner" style="position: relative; top: 10%;">
                      <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                      <p v-if="criteria.name == 'Critical Errors'" class="el-loading-text" style="word-break: break-word;">Đang tìm các lỗi có trong bài viết</p>
                      <p v-else-if="criteria.name == 'Arguments Assessment'" class="el-loading-text" style="word-break: break-word;">Đang đánh giá các lập luận</p>
                      <p v-else class="el-loading-text" style="word-break: break-word;">Đang chấm tiêu chí {{ criteria.name }}</p>
                    </div>
                  </div>
                  <div v-else>
                    <el-input
                      v-if="!isAiReview"
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
                    <pre v-if="isAiReview" style="border: #bcbcbc solid 1px; padding: 10px; border-radius: 5px;" v-html="criteria.comment" />
                  </div>

                </div>

              </div>
            </el-card>

            <el-card
              v-if="isAiReview && reviewSaved"
              style="margin-top: 10px; margin-bottom: 5px; margin-left: 3px; background: rgb(129 152 155);"
              shadow="hover"
            >
              <div slot="header" class="clearfix">
                <div style="color: white; float: left; font-size: 16px; font-weight: 500; width: calc(100% - 100px); text-overflow: ellipsis;  word-break: break-word; overflow: hidden; white-space: nowrap;">
                  <span>Đánh Giá Phản Hồi</span>
                </div>
              </div>
              <div>
                <div>
                  <div style="font-size: 15px; color: white;">Đánh giá mức độ hữu ích của phản hồi</div>
                </div>

                <div>
                  <el-rate v-model="rateValue" style="margin-top: 8px; margin-bottom: 4px; color: rgb(177 177 177);" :allow-half="true" />
                </div>

                <div>
                  <el-input
                    id="rubric-rating"
                    v-model="rateComment"
                    type="textarea"
                    :rows="5"
                    style="margin-top: 10px; margin-bottom: 5px;"
                    :maxlength="8000"
                    placeholder="Cảm nghĩ của bạn về điểm số và phản hồi cho bài viết"
                  />
                </div>
                <div style="margin-top: 5px;">
                  <el-button :disabled="rateValue == 0 && rateComment == ''" size="mini" @click="rateAIReview()">
                    Gửi đánh giá
                  </el-button>
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
      ],
      reviewSaved: false,
      rateValue: 0,
      rateComment: ''
    }
  },
  computed: {
  },
  async mounted() {
    console.log(this.isAiReview)
    this.loadRubric()
    // Update fb comment's width for safari
    setTimeout(function () {
      const iframes = document.getElementsByClassName('fb_iframe_widget_lift')
      if (iframes && iframes.length > 0) {
        iframes[0].style.width = '100%'
      }
    }, 2000)
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
        this.rubricCriteria = rs.map(criteria => ({ ...criteria, mark: null, isFocused: false, comment: '', userFeedback: null, loading: true}))
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
                  criteria.userFeedback = rc.userFeedback
                }
                criteria.loading = false
              })
              var cmt = { id: rc.criteriaId, content: rc.comment, reviewid: this.reviewId, questionid: this.questionid }
              retrievedComment.push(cmt)

              var ms = { id: rc.criteriaId, content: rc.name == 'Critical Errors' || rc.name == 'Arguments Assessment' ? 0 : rc.score, reviewid: this.reviewId, questionid: this.questionid }
              retrievedScore.push(ms)
            })
            if (this.currentUser.role !== 'Admin') { this.readOnly = false } else {
              this.readOnly = true
            }
            this.reviewSaved = true
            // Set localstorage so we don't have to load from db again
            localStorage.setItem('reviewRubricComment', JSON.stringify(retrievedComment))
            localStorage.setItem('reviewRubricScore', JSON.stringify(retrievedScore))
          } else {
            if (this.isAiReview) {
                const question = this.$store.getters['question/getSelected']
                const topic = question.questionsPart.find(q => q.name == 'Question').content
                let completedCount = 0

                // -- Start get essay score for version 1.3 ---
                if (question.section == 'Academic Writing Task 2') {
                  // 1. Get scores for the essay
                  // 1 call
                  const scoreModel = {
                    task: question.section,
                    topic: topic,
                    essay: this.documentText
                  }
                  reviewService.getEssayScore(scoreModel).then(scores => {
                    if (scores) {
                      this.rubricCriteria.forEach(criteria => {
                        if (criteria.name == 'Task Achievement') {
                          criteria.mark = scores.taskAchievementScore
                        } else if (criteria.name == 'Task Response') {
                          criteria.mark = scores.taskResponseScore
                        } else if (criteria.name == 'Coherence & Cohesion') {
                          criteria.mark = scores.coherenceScore
                        } else if (criteria.name == 'Lexical Resource') {
                          criteria.mark = scores.lexicalResourceScore
                        } else if (criteria.name == 'Grammatical Range & Accuracy') {
                          criteria.mark = scores.grammarScore
                        } else if (criteria.name == 'Overall Score & Feedback') {
                          criteria.mark = scores.overallScore
                        }
                      })
                      completedCount++
                      if (completedCount == 8) {
                        this.submitReview()
                      }
                    }
                  })
                }
                // -- End get essay score for version 1.3 ---

                this.rubricCriteria.forEach(criteria => {
                  // --- Start Version 1.4 Review ---
                  if (criteria.name == 'Task Achievement') {
                    const chart = question.questionsPart.find(q => q.name == 'Chart')
                    if (chart) {
                      reviewService.getChartDescription(chart.content).then(rs => {
                        const chartDescription = rs
                        const scoreModel = {
                          task: question.section,
                          topic: topic,
                          essay: this.documentText,
                          chartDescription: chartDescription
                        }
                        reviewService.getEssayScore(scoreModel).then(scores => {
                          if (scores) {
                            this.rubricCriteria.forEach(criteria => {
                              if (criteria.name == 'Task Achievement') {
                                criteria.mark = scores.taskAchievementScore
                              } else if (criteria.name == 'Task Response') {
                                criteria.mark = scores.taskResponseScore
                              } else if (criteria.name == 'Coherence & Cohesion') {
                                criteria.mark = scores.coherenceScore
                              } else if (criteria.name == 'Lexical Resource') {
                                criteria.mark = scores.lexicalResourceScore
                              } else if (criteria.name == 'Grammatical Range & Accuracy') {
                                criteria.mark = scores.grammarScore
                              } else if (criteria.name == 'Overall Score & Feedback') {
                                criteria.mark = scores.overallScore
                              }
                            })
                            completedCount++
                            if (completedCount == 8) {
                              this.submitReview()
                            }
                          }
                        })

                        const model = {
                          task: question.section,
                          topic: topic,
                          essay: this.documentText,
                          criteriaName: criteria.name,
                          feedbackLanguage: this.feedbackLanguage,
                          chartDescription: chartDescription
                        }

                        reviewService.getAIFeedbackForCriteriaV4(model).then(rs => {
                          criteria.comment = rs
                          // submit the review
                          criteria.loading = false
                          completedCount++
                          if (completedCount == 8) {
                              this.submitReview()
                          }
                        })
                      })
                    } else {
                      const model = {
                        task: question.section,
                        topic: topic,
                        essay: this.documentText,
                        criteriaName: criteria.name,
                        feedbackLanguage: this.feedbackLanguage
                      }

                      reviewService.getAIFeedbackForCriteriaV4(model).then(rs => {
                        criteria.comment = rs
                        // submit the review
                        criteria.loading = false
                        completedCount++
                        if (completedCount == 8) {
                            this.submitReview()
                        }
                      })
                    }
                  } else {
                    const model = {
                      task: question.section,
                      topic: topic,
                      essay: this.documentText,
                      criteriaName: criteria.name,
                      feedbackLanguage: this.feedbackLanguage
                    }

                    reviewService.getAIFeedbackForCriteriaV4(model).then(rs => {
                      criteria.comment = rs
                      // submit the review
                      criteria.loading = false
                      completedCount++
                      if (completedCount == 8) {
                        this.submitReview()
                      }
                    })
                  }
                  // --- End Version 1.4 Review ---

                  // --- Start Version 1.3 Review ---
                  // if (criteria.name == 'Critical Errors') {
                  //   // 2. Get feedback for errors
                  //   // 1 call
                  //   const split = this.documentText.split('\n')
                  //   const paragraphs = split.filter(p => p.length > 50)
                  //   const pCount = paragraphs.length
                  //   let errorCompletedCount = 0

                  //   const errors = []
                  //   for (let i = 0; i < paragraphs.length; i++) {
                  //     const paragraph = paragraphs[i]
                  //     const order = i + 1
                  //     const model = {
                  //       essay: paragraph,
                  //       feedbackLanguage: this.feedbackLanguage
                  //     }
                  //     reviewService.getFeedbackForErrors(model).then(rs => {
                  //       criteria.loading = false
                  //       criteria.comment += rs + '\n'

                  //       let paragraphLang = 'Đoạn'
                  //       if (this.feedbackLanguage != 'vn') { paragraphLang = 'Paragraph' }
                  //       errors[i] = paragraphLang + ' ' + order + ':\n' + rs + '\n'
                  //       errorCompletedCount++
                  //       if (errorCompletedCount == pCount) {
                  //         criteria.comment = errors.join('\n')
                  //         completedCount++
                  //         console.log(completedCount)
                  //         if (completedCount == 15) {
                  //           this.submitReview()
                  //         }
                  //       }
                  //     })
                  //   }
                  // } else if (criteria.name == 'Task Achievement') {
                  //   const chart = question.questionsPart.find(q => q.name == 'Chart')
                  //   if (chart) {
                  //     // Get chart content first
                  //     // To do: get chart description when adding new question
                  //     reviewService.getChartDescription(chart.content).then(rs => {
                  //       const chartDescription = rs

                  //       // Get score now
                  //       const scoreModel = {
                  //         task: question.section,
                  //         topic: topic,
                  //         essay: this.documentText,
                  //         chartDescription: chartDescription
                  //       }
                  //       reviewService.getEssayScore(scoreModel).then(scores => {
                  //         if (scores) {
                  //           this.rubricCriteria.forEach(criteria => {
                  //             if (criteria.name == 'Task Achievement') {
                  //               criteria.mark = scores.taskAchievementScore
                  //             } else if (criteria.name == 'Task Response') {
                  //               criteria.mark = scores.taskResponseScore
                  //             } else if (criteria.name == 'Coherence & Cohesion') {
                  //               criteria.mark = scores.coherenceScore
                  //             } else if (criteria.name == 'Lexical Resource') {
                  //               criteria.mark = scores.lexicalResourceScore
                  //             } else if (criteria.name == 'Grammatical Range & Accuracy') {
                  //               criteria.mark = scores.grammarScore
                  //             } else if (criteria.name == 'Overall Score & Feedback') {
                  //               criteria.mark = scores.overallScore
                  //             }
                  //           })
                  //           completedCount++
                  //           if (completedCount == 15) {
                  //             this.submitReview()
                  //           }
                  //         }
                  //       })

                  //       // 3. Get feedback for Task Achievement
                  //       // 3 calls
                  //       const model1 = {
                  //         task: question.section,
                  //         topic: topic,
                  //         essay: this.documentText,
                  //         criteriaName: 'Fulfilling the Prompt Requirements',
                  //         feedbackLanguage: this.feedbackLanguage,
                  //         chartDescription: chartDescription
                  //       }

                  //       reviewService.getAIFeedbackForCriteriaV3(model1).then(rs => {
                  //         rs = rs.replace('Detailed assessment:\n', 'Detailed assessment: ')
                  //         // show the comment and mark
                  //         criteria.comment += '- Fulfilling the Prompt Requirements:\n\n' + rs + '\n\n'
                  //         // submit the review
                  //         criteria.loading = false
                  //         completedCount++
                  //         if (completedCount == 15) {
                  //           this.submitReview()
                  //         }
                  //       })

                  //       const model2 = {
                  //         task: question.section,
                  //         topic: topic,
                  //         essay: this.documentText,
                  //         criteriaName: 'Highlighting Key Features',
                  //         feedbackLanguage: this.feedbackLanguage,
                  //         chartDescription: chartDescription
                  //       }

                  //       reviewService.getAIFeedbackForCriteriaV3(model2).then(rs => {
                  //         // show the comment and mark
                  //         criteria.comment += '- Highlighting Key Features:\n\n' + rs + '\n\n'
                  //         // submit the review
                  //         criteria.loading = false
                  //         completedCount++
                  //         if (completedCount == 15) {
                  //           this.submitReview()
                  //         }
                  //       })

                  //       const model3 = {
                  //         task: question.section,
                  //         topic: topic,
                  //         essay: this.documentText,
                  //         criteriaName: 'Comparing and Contrasting Data',
                  //         feedbackLanguage: this.feedbackLanguage,
                  //         chartDescription: chartDescription
                  //       }

                  //       reviewService.getAIFeedbackForCriteriaV3(model3).then(rs => {
                  //         // show the comment and mark
                  //         criteria.comment += '- Comparing and Contrasting Data:\n\n' + rs + '\n\n'
                  //         // submit the review
                  //         criteria.loading = false
                  //         completedCount++
                  //         if (completedCount == 15) {
                  //           this.submitReview()
                  //         }
                  //       })
                  //     })
                  //   } else {
                  //     // Get score and feedback for Task Achievement without the chart
                  //     const scoreModel = {
                  //       task: question.section,
                  //       topic: topic,
                  //       essay: this.documentText
                  //     }
                  //     reviewService.getEssayScore(scoreModel).then(scores => {
                  //       if (scores) {
                  //         this.rubricCriteria.forEach(criteria => {
                  //           if (criteria.name == 'Task Achievement') {
                  //             criteria.mark = scores.taskAchievementScore
                  //           } else if (criteria.name == 'Task Response') {
                  //             criteria.mark = scores.taskResponseScore
                  //           } else if (criteria.name == 'Coherence & Cohesion') {
                  //             criteria.mark = scores.coherenceScore
                  //           } else if (criteria.name == 'Lexical Resource') {
                  //             criteria.mark = scores.lexicalResourceScore
                  //           } else if (criteria.name == 'Grammatical Range & Accuracy') {
                  //             criteria.mark = scores.grammarScore
                  //           } else if (criteria.name == 'Overall Score & Feedback') {
                  //             criteria.mark = scores.overallScore
                  //           }
                  //         })
                  //         completedCount++
                  //         if (completedCount == 15) {
                  //           this.submitReview()
                  //         }
                  //       }
                  //     })

                  //     // 3. Get feedback for Task Achievement
                  //     // 3 calls
                  //     const model1 = {
                  //       task: question.section,
                  //       topic: topic,
                  //       essay: this.documentText,
                  //       criteriaName: 'Fulfilling the Prompt Requirements',
                  //       feedbackLanguage: this.feedbackLanguage
                  //     }

                  //     reviewService.getAIFeedbackForCriteriaV3(model1).then(rs => {
                  //       rs = rs.replace('Detailed assessment:\n', 'Detailed assessment: ')
                  //       // show the comment and mark
                  //       criteria.comment += '- Fulfilling the Prompt Requirements:\n\n' + rs + '\n\n'
                  //       // submit the review
                  //       criteria.loading = false
                  //       completedCount++
                  //       if (completedCount == 15) {
                  //         this.submitReview()
                  //       }
                  //     })

                  //     const model2 = {
                  //       task: question.section,
                  //       topic: topic,
                  //       essay: this.documentText,
                  //       criteriaName: 'Highlighting Key Features',
                  //       feedbackLanguage: this.feedbackLanguage
                  //     }

                  //     reviewService.getAIFeedbackForCriteriaV3(model2).then(rs => {
                  //       // show the comment and mark
                  //       criteria.comment += '- Highlighting Key Features:\n\n' + rs + '\n\n'
                  //       // submit the review
                  //       criteria.loading = false
                  //       completedCount++
                  //       if (completedCount == 15) {
                  //         this.submitReview()
                  //       }
                  //     })

                  //     const model3 = {
                  //       task: question.section,
                  //       topic: topic,
                  //       essay: this.documentText,
                  //       criteriaName: 'Comparing and Contrasting Data',
                  //       feedbackLanguage: this.feedbackLanguage
                  //     }

                  //     reviewService.getAIFeedbackForCriteriaV3(model3).then(rs => {
                  //       // show the comment and mark
                  //       criteria.comment += '- Comparing and Contrasting Data:\n\n' + rs + '\n\n'
                  //       // submit the review
                  //       criteria.loading = false
                  //       completedCount++
                  //       if (completedCount == 15) {
                  //         this.submitReview()
                  //       }
                  //     })
                  //   }

                  //   // const model4 = {
                  //   //   task: question.section,
                  //   //   topic: topic,
                  //   //   essay: this.documentText,
                  //   //   criteriaName: 'Data Selection and Relevance',
                  //   //   feedbackLanguage: this.feedbackLanguage,
                  //   //   chartDescription: chartDescription
                  //   // }

                  //   // reviewService.getAIFeedbackForCriteriaV3(model4).then(rs => {
                  //   //   // show the comment and mark
                  //   //   criteria.comment += '- Data Selection and Relevance:\n\n' + rs + '\n\n'
                  //   //   // submit the review
                  //   //   criteria.loading = false
                  //   //   completedCount++
                  //   //   console.log(completedCount)
                  //   //   if (completedCount == 7) {
                  //   //     // this.submittedReview()
                  //   //   }
                  //   // })
                  // } else if (criteria.name == 'Task Response') {
                  //   // 3. Get feedback for Task Achievement
                  //   // 3 calls
                  //   const model1 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Addressing All Parts',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model1).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Addressing All Parts of the Question:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model2 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Clarity of Position',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model2).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Clarity of Position:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model3 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Development of Ideas',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model3).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Development of Ideas:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })
                  // } else if (criteria.name == 'Coherence & Cohesion') {
                  //   // 3. Get feedback for Task Achievement
                  //   // 3 calls
                  //   const model1 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Logical Organization',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model1).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Logical Organization:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model2 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Paragraphing',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model2).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Paragraphing:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model3 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Use of Cohesive Devices',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model3).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Use of Cohesive Devices:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })
                  // } else if (criteria.name == 'Lexical Resource') {
                  //   // 3. Get feedback for Lexical Resource
                  //   // 3 calls
                  //   const model1 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Range of Vocabulary',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model1).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Range of Vocabulary:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model2 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Accuracy of Word Choice',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model2).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Accuracy of Word Choice:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model3 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Spelling and Word Formation',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model3).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Spelling and Word Formation:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })
                  // } else if (criteria.name == 'Grammatical Range & Accuracy') {
                  //   // 3. Get feedback for Grammatical Range & Accuracy
                  //   // 3 calls
                  //   const model1 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Range of Grammatical Structures',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model1).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Range of Grammatical Structures:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model2 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Sentence Complexity',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model2).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Sentence Complexity:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })

                  //   const model3 = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: 'Accuracy in Grammatical Forms',
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV3(model3).then(rs => {
                  //     // show the comment and mark
                  //     criteria.comment += '- Accuracy in Grammatical Forms:\n\n' + rs + '\n\n'
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })
                  // } else {
                  //   // This is for Argument Assessment and Overall Score & Feedback
                  //   // 2 calls
                  //   const model = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: criteria.name,
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV2(model).then(rs => {
                  //     let comment = rs
                  //     // get the score from the comment
                  //     if (criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment') {
                  //       var comments = comment.split('\n')
                  //       if (comments && comments.length > 0) {
                  //         // re-compose the comment
                  //         comments.splice(0, 2)
                  //         comment = comments.join('\n')
                  //       }
                  //     }
                  //     // show the comment and mark
                  //     criteria.comment = comment
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     console.log(completedCount)
                  //     if (completedCount == 15) {
                  //       this.submitReview()
                  //     }
                  //   })
                  // }
                  // --- End Version 1.3 Review ---

                  // --- Start Version 1.2 Review ---

                  // if (criteria.name == 'Task Achievement') {
                  //   const chart = question.questionsPart.find(q => q.name == 'Chart')
                  //   if (chart) {
                  //     reviewService.getChartDescription(chart.content).then(rs => {
                  //       const chartDescription = rs
                  //       const scoreModel = {
                  //         task: question.section,
                  //         topic: topic,
                  //         essay: this.documentText,
                  //         chartDescription: chartDescription
                  //       }
                  //       reviewService.getEssayScore(scoreModel).then(scores => {
                  //         if (scores) {
                  //           this.rubricCriteria.forEach(criteria => {
                  //             if (criteria.name == 'Task Achievement') {
                  //               criteria.mark = scores.taskAchievementScore
                  //             } else if (criteria.name == 'Task Response') {
                  //               criteria.mark = scores.taskResponseScore
                  //             } else if (criteria.name == 'Coherence & Cohesion') {
                  //               criteria.mark = scores.coherenceScore
                  //             } else if (criteria.name == 'Lexical Resource') {
                  //               criteria.mark = scores.lexicalResourceScore
                  //             } else if (criteria.name == 'Grammatical Range & Accuracy') {
                  //               criteria.mark = scores.grammarScore
                  //             } else if (criteria.name == 'Overall Score & Feedback') {
                  //               criteria.mark = scores.overallScore
                  //             }
                  //           })
                  //           completedCount++
                  //           if (completedCount == 8) {
                  //             this.submitReview()
                  //           }
                  //         }
                  //       })

                  //       const model = {
                  //         task: question.section,
                  //         topic: topic,
                  //         essay: this.documentText,
                  //         criteriaName: criteria.name,
                  //         feedbackLanguage: this.feedbackLanguage,
                  //         chartDescription: chartDescription
                  //       }

                  //       reviewService.getAIFeedbackForCriteriaV2(model).then(rs => {
                  //         let comment = rs
                  //         // get the score from the comment
                  //         if (criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment') {
                  //           var comments = comment.split('\n')
                  //           if (comments && comments.length > 0) {
                  //             // re-compose the comment
                  //             comments.splice(0, 2)
                  //             comment = comments.join('\n')
                  //           }
                  //         }
                  //         // show the comment and mark
                  //         criteria.comment = comment
                  //         // submit the review
                  //         criteria.loading = false
                  //         completedCount++
                  //         if (completedCount == 8) {
                  //             this.submitReview()
                  //         }
                  //       })
                  //     })
                  //   } else {
                  //     const model = {
                  //       task: question.section,
                  //       topic: topic,
                  //       essay: this.documentText,
                  //       criteriaName: criteria.name,
                  //       feedbackLanguage: this.feedbackLanguage
                  //     }

                  //     reviewService.getAIFeedbackForCriteriaV2(model).then(rs => {
                  //       let comment = rs
                  //       // get the score from the comment
                  //       if (criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment') {
                  //         var comments = comment.split('\n')
                  //         if (comments && comments.length > 0) {
                  //           // re-compose the comment
                  //           comments.splice(0, 2)
                  //           comment = comments.join('\n')
                  //         }
                  //       }
                  //       // show the comment and mark
                  //       criteria.comment = comment
                  //       // submit the review
                  //       criteria.loading = false
                  //       completedCount++
                  //       if (completedCount == 8) {
                  //           this.submitReview()
                  //       }
                  //     })
                  //   }
                  // } else {
                  //   const model = {
                  //     task: question.section,
                  //     topic: topic,
                  //     essay: this.documentText,
                  //     criteriaName: criteria.name,
                  //     feedbackLanguage: this.feedbackLanguage
                  //   }

                  //   reviewService.getAIFeedbackForCriteriaV2(model).then(rs => {
                  //     let comment = rs
                  //     // get the score from the comment
                  //     if (criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment') {
                  //       var comments = comment.split('\n')
                  //       if (comments && comments.length > 0) {
                  //         // re-compose the comment
                  //         comments.splice(0, 2)
                  //         comment = comments.join('\n')
                  //       }
                  //     }
                  //     // show the comment and mark
                  //     criteria.comment = comment
                  //     // submit the review
                  //     criteria.loading = false
                  //     completedCount++
                  //     if (completedCount == 8) {
                  //       this.submitReview()
                  //     }
                  //   })
                  // }
                  // --- End Version 1.2 Review ---

                  // --- Start Version 1.1 Review ---
                  // const model = {
                  //   task: question.section,
                  //   topic: topic,
                  //   essay: this.documentText,
                  //   criteriaName: criteria.name,
                  //   feedbackLanguage: this.feedbackLanguage
                  // }

                  // reviewService.getAIFeedbackForCriteriaV1(model).then(rs => {
                  //   criteria.loading = false
                  //   criteria.comment = rs.comment
                  //   criteria.mark = rs.bandScore
                  //   // completedCount++
                  //   // if (completedCount == 5) {
                  //   //   // update overall band score
                  //   //   const fourCriteria = this.rubricCriteria.filter(c => c.name != 'Overall Score & Feedback')
                  //   //   const index = this.rubricCriteria.findIndex(c => c.name == 'Overall Score & Feedback')
                  //   //   const average = fourCriteria.reduce((total, next) => total + next.mark, 0) / 4
                  //   //   this.rubricCriteria[index].mark = (Math.round(average * 2) / 2).toFixed(1)

                  //   //   // Save the criteria synchronously
                  //   //   var reviewData = []
                  //   //   this.rubricCriteria.forEach(r => {
                  //   //     if (r.mark) {
                  //   //       reviewData.push({
                  //   //         CriteriaName: r.name,
                  //   //         Comment: r.comment,
                  //   //         CriteriaId: r.id,
                  //   //         Score: r.mark,
                  //   //         ReviewId: this.reviewId
                  //   //       })
                  //   //     }
                  //   //   })

                  //   //   reviewService.saveReviewFeedback(this.reviewId, reviewData)
                  //   // }
                  // })
                  // --- End Version 1.1 Review ---
                })
              } else {
                this.rubricCriteria.forEach(criteria => {
                  criteria.loading = false
                })
              }
          }
        }
      } else {
        console.log('Error: rubric cannot be found!')
      }
    },
    submitReview() {
      console.log('submitted!')
      // Save the criteria synchronously
      var reviewData = []
      this.rubricCriteria.forEach(r => {
        reviewData.push({
          CriteriaName: r.name,
          Comment: r.comment,
          CriteriaId: r.id,
          Score: r.mark,
          ReviewId: this.reviewId,
          UserFeedback: r.userFeedback
        })
      })
      reviewService.saveReviewFeedback(this.reviewId, reviewData).then(rs => {
        this.reviewSaved = true
      })
    },
    rateAIReview() {
      reviewService.createAIReviewRating({
        UserId: 'AI Review Version 1.4',
        ReviewId: this.reviewId,
        Rate: parseFloat(this.rateValue),
        Comment: this.rateComment
      }).then(r => {
        if (r) {
          this.$notify.success({
            title: 'Cảm ơn đánh giá của bạn!',
            message: 'Dữ liệu bạn cung cấp sẽ được sử dụng để nâng cấp hệ thống chấm bài tự động và mang lại cho bạn những phản hồi tốt hơn.',
            type: 'success',
            duration: 7000
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
    sendFeedback(reviewId, criteria) {
      this.sendingFeedback = true
      this.getLocaleStorageData()
      var reviewData = []
      this.rubricCriteria.forEach(r => {
        reviewData.push({
          Comment: r.comment,
          CriteriaId: r.id,
          Score: r.mark,
          ReviewId: reviewId,
          UserFeedback: r.userFeedback
        })
      })
      reviewService.saveRubric(reviewId, reviewData).then(rs => {
        if (rs) {
          console.log(document.getElementById('rate-feedback-' + criteria.id))
          document.getElementById('rate-feedback-' + criteria.id).click()
          this.sendingFeedback = false
          this.$notify.success({
            title: 'Cảm ơn đánh giá của bạn!',
            message: 'Dữ liệu bạn cung cấp sẽ được sử dụng để nâng cấp hệ thống chấm bài tự động và mang lại cho bạn những phản hồi tốt hơn.',
            type: 'success',
            duration: 7000
          })
        }
        criteria.isFocused = false
      })
    },
    saveRubric(reviewId, criteria) {
      this.getLocaleStorageData()
      var reviewData = []
      this.rubricCriteria.forEach(r => {
        reviewData.push({
          Comment: r.comment,
          CriteriaId: r.id,
          Score: r.mark,
          ReviewId: reviewId,
          UserFeedback: r.userFeedback
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
            criteria.mark = criteria.name == 'Critical Errors' || criteria.name == 'Arguments Assessment' ? 0 : rc.content
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
.band-score{
  width: 85px;
  border: #478a9e solid 1px;
  margin-bottom: 10px;
  padding: 2px 10px;
  border-radius: 5px;
  background: #d6e3e6;
  font-size: 15px;
}
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
