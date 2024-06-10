<template>
  <div v-if="screenWidth > 780" id="reviewContainer" :style="{cursor: activeButton== 'text'||activeButton== 'rectangle' || activeButton== 'point' ? 'crosshair':''}" style="border-top: 1px solid rgb(223 224 238); background: rgb(248, 249, 250);">
    <div id="content-wrapper" style="background: rgb(248, 249, 250); height: inherit; width: 100%;position: absolute; overflow: unset;">
      <div id="left-panel" :class="{'hideQuestion': !showQuestion}" style="height: calc(100vh - 50px);">
        <el-tabs v-model="selectedTab" type="border-card">
          <el-tab-pane name="rubric" label="Phản hồi cho bài viết">
            <div style="height: 100%;display: flex; flex-direction: column">
              <div id="parent-scroll" style="flex-grow: 1;position: relative;">
                <div id="child-scroll">
                  <div id="rubric">

                    <div v-if="loadCriteriaFeedbackCompleted || loadEssayScoreCompleted" style="height: 100%; overflow: auto; padding-bottom: 20px;">
                      <div v-if="loadEssayScoreCompleted">
                        <el-card
                          style="margin-bottom: 5px; border: 1px solid rgb(190, 190, 190);"
                          shadow="hover"
                        >
                          <div slot="header" class="clearfix">
                            <div>
                              <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                                <span>Overall Band Score</span>
                              </div>
                              <div style="float: right;">
                                <div>
                                  <div>
                                    <div v-if="essayScore.overallBandScore" class="band-score">
                                      Band: {{ essayScore.overallBandScore.toString().length == 1 ? essayScore.overallBandScore.toString() + '.0' : essayScore.overallBandScore }}
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div style="border: 1px solid rgb(188, 188, 188); padding: 10px; border-radius: 5px;">
                            <div v-if="essayScore.taskAchievementScore" class="criteria-score">
                              <div>
                                <b>
                                  Task Achievement:
                                  {{ essayScore.taskAchievementScore.toString().length == 1 ? essayScore.taskAchievementScore.toString() + '.0' : essayScore.taskAchievementScore }}
                                </b>
                              </div>
                              <div v-if="essayScore.highlightKeyFeatures" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.highlightKeyFeatures > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                                    essayScore.highlightKeyFeatures == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.highlightKeyFeatures.toString().length == 1 ? essayScore.highlightKeyFeatures.toString() + '.0' : essayScore.highlightKeyFeatures }}
                                </el-tag>
                                <div class="sub-criteria-label">Highlighting Key Features</div>
                              </div>
                              <div v-if="essayScore.compareAndContrast" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.compareAndContrast > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                                    essayScore.compareAndContrast == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.compareAndContrast.toString().length == 1 ? essayScore.compareAndContrast.toString() + '.0' : essayScore.compareAndContrast }}
                                </el-tag>
                                <div class="sub-criteria-label">Comparing and Contrasting Data</div>
                              </div>

                              <div v-if="essayScore.dataSelection" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.dataSelection > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                                    essayScore.dataSelection == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.dataSelection.toString().length == 1 ? essayScore.dataSelection.toString() + '.0' : essayScore.dataSelection }}
                                </el-tag>
                                <div class="sub-criteria-label">Data Selection and Relevance</div>
                              </div>
                              <div class="sub-criteria">
                                <el-tag v-if="essayScore.appropriateWordCount" type="success" size="small">
                                  <i class="criteria-score-check el-icon-check" />
                                </el-tag>
                                <el-tag v-else type="danger" size="small">
                                  <i class="el-icon-close criteria-score-close" />
                                </el-tag>
                                <span class="sub-criteria-label">Appropriate Word Count</span>
                              </div>
                            </div>

                            <div v-if="essayScore.taskResponseScore" class="criteria-score">
                              <div>
                                <b>
                                  Task Response:
                                  {{ essayScore.taskResponseScore.toString().length == 1 ? essayScore.taskResponseScore.toString() + '.0' : essayScore.taskResponseScore }}
                                </b>
                              </div>
                              <div v-if="essayScore.clarityOfPosition" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.clarityOfPosition > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                                    essayScore.clarityOfPosition == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.clarityOfPosition.toString().length == 1 ? essayScore.clarityOfPosition.toString() + '.0' : essayScore.clarityOfPosition }}
                                </el-tag>
                                <span class="sub-criteria-label">Clarity of Position</span>
                              </div>
                              <div v-if="essayScore.developmentOfIdeas" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.developmentOfIdeas > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                                    essayScore.developmentOfIdeas == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.developmentOfIdeas.toString().length == 1 ? essayScore.developmentOfIdeas.toString() + '.0' : essayScore.developmentOfIdeas }}
                                </el-tag>
                                <span class="sub-criteria-label">Development of Ideas</span>
                              </div>
                              <div v-if="essayScore.justificationOfOpinion" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.justificationOfOpinion > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                                    essayScore.justificationOfOpinion == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.justificationOfOpinion.toString().length == 1 ? essayScore.justificationOfOpinion.toString() + '.0' : essayScore.justificationOfOpinion }}
                                </el-tag>
                                <span class="sub-criteria-label">Justification of Opinions</span>
                              </div>
                              <div class="sub-criteria">
                                <el-tag v-if="essayScore.appropriateWordCount" type="success" size="small">
                                  <i class="criteria-score-check el-icon-check" />
                                </el-tag>
                                <el-tag v-else type="danger" size="small">
                                  <i class="el-icon-close criteria-score-close" />
                                </el-tag>
                                <span class="sub-criteria-label">Appropriate Word Count</span>
                              </div>
                            </div>

                            <div v-if="essayScore.coherenceScore" class="criteria-score">
                              <div>
                                <b>
                                  Coherence and Cohesion:
                                  {{ essayScore.coherenceScore.toString().length == 1 ? essayScore.coherenceScore.toString() + '.0' : essayScore.coherenceScore }}
                                </b>
                              </div>
                              <div v-if="essayScore.logicalOrganization" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.logicalOrganization > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                                    essayScore.logicalOrganization == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.logicalOrganization.toString().length == 1 ? essayScore.logicalOrganization.toString() + '.0' : essayScore.logicalOrganization }}
                                </el-tag>
                                <div class="sub-criteria-label">Logical Organization</div>
                              </div>
                              <div v-if="essayScore.paragraphing" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.paragraphing > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                                    essayScore.paragraphing == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.paragraphing.toString().length == 1 ? essayScore.paragraphing.toString() + '.0' : essayScore.paragraphing }}
                                </el-tag>
                                <div class="sub-criteria-label">Paragraphing</div>
                              </div>
                              <div v-if="essayScore.cohesiveDevices" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.cohesiveDevices > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                                    essayScore.cohesiveDevices == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.cohesiveDevices.toString().length == 1 ? essayScore.cohesiveDevices.toString() + '.0' : essayScore.cohesiveDevices }}
                                </el-tag>
                                <div class="sub-criteria-label">Use of Cohesive Devices</div>
                              </div>
                            </div>

                            <div v-if="essayScore.lexicalResourceScore" class="criteria-score">
                              <div>
                                <b>
                                  Lexical Resource:
                                  {{ essayScore.lexicalResourceScore.toString().length == 1 ? essayScore.lexicalResourceScore.toString() + '.0' : essayScore.lexicalResourceScore }}
                                </b>
                              </div>
                              <div v-if="essayScore.rangeOfVocabulary" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.rangeOfVocabulary > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                    essayScore.rangeOfVocabulary == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.rangeOfVocabulary.toString().length == 1 ? essayScore.rangeOfVocabulary.toString() + '.0' : essayScore.rangeOfVocabulary }}
                                </el-tag>
                                <div class="sub-criteria-label">Range of Vocabulary</div>
                              </div>
                              <div v-if="essayScore.accuracyOfWordChoice" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.accuracyOfWordChoice > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                    essayScore.accuracyOfWordChoice == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.accuracyOfWordChoice.toString().length == 1 ? essayScore.accuracyOfWordChoice.toString() + '.0' : essayScore.accuracyOfWordChoice }}
                                </el-tag>
                                <div class="sub-criteria-label">Accuracy of Word Choice</div>
                              </div>
                              <div v-if="essayScore.spellingAndFormation" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.spellingAndFormation > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                    essayScore.spellingAndFormation == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.spellingAndFormation.toString().length == 1 ? essayScore.spellingAndFormation.toString() + '.0' : essayScore.spellingAndFormation }}
                                </el-tag>
                                <div class="sub-criteria-label">Spelling and Word Formation</div>
                              </div>
                              <div v-if="essayScore.registerAndStyle" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.registerAndStyle > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                    essayScore.registerAndStyle == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.registerAndStyle.toString().length == 1 ? essayScore.registerAndStyle.toString() + '.0' : essayScore.registerAndStyle }}
                                </el-tag>
                                <div class="sub-criteria-label">Appropriateness of Register and Style</div>
                              </div>
                            </div>

                            <div v-if="essayScore.grammarScore" class="criteria-score">
                              <div>
                                <b>
                                  Grammatical Range and Accuracy:
                                  {{ essayScore.grammarScore.toString().length == 1 ? essayScore.grammarScore.toString() + '.0' : essayScore.grammarScore }}
                                </b>
                              </div>
                              <div v-if="essayScore.grammarRange" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.grammarRange > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                                    essayScore.grammarRange == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.grammarRange.toString().length == 1 ? essayScore.grammarRange.toString() + '.0' : essayScore.grammarRange }}
                                </el-tag>
                                <div class="sub-criteria-label">Range of Grammatical Structures</div>
                              </div>
                              <div v-if="essayScore.sentenceComplexity" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.sentenceComplexity > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                                    essayScore.sentenceComplexity == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.sentenceComplexity.toString().length == 1 ? essayScore.sentenceComplexity.toString() + '.0' : essayScore.sentenceComplexity }}
                                </el-tag>
                                <div class="sub-criteria-label">Sentence Complexity</div>
                              </div>
                              <div v-if="essayScore.grammarAccuracy" class="sub-criteria">
                                <el-tag
                                  :type="essayScore.grammarAccuracy > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                                    essayScore.grammarAccuracy == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                                    ? 'success' : 'danger'"
                                  size="small"
                                  class="sub-score-tag"
                                >
                                  {{ essayScore.grammarAccuracy.toString().length == 1 ? essayScore.grammarAccuracy.toString() + '.0' : essayScore.grammarAccuracy }}
                                </el-tag>
                                <div class="sub-criteria-label">Accuracy in Grammatical Forms</div>
                              </div>
                            </div>
                          </div>
                          <div v-if="review && review.reviewRequest && review.reviewRequest.feedbackLanguage == 'vn'" style="font-size: 13px; margin-top: 5px; padding:2px;">
                            <div style="background-color: rgb(244, 244, 245); border-color: rgb(233, 233, 235); color: rgb(144, 147, 153); font-size: 12px; border-width: 1px; border-style: solid; border-radius: 4px; padding: 10px;">
                              Xin lưu ý rằng điểm số được cung cấp không phải là điểm IELTS chính thức của bạn và chỉ nên được sử dụng cho mục đích học tập và cải thiện kỹ năng.
                            </div>
                          </div>
                          <div v-else style="font-size: 13px; margin-top: 5px;">
                            <div style="background-color: rgb(244, 244, 245); border-color: rgb(233, 233, 235); color: rgb(144, 147, 153); font-size: 12px; border-width: 1px; border-style: solid; border-radius: 4px; padding: 10px;">
                              Please note that the scores provided are not your official IELTS scores and should be used solely for learning and practicing purposes.
                            </div>
                          </div>
                        </el-card>
                      </div>
                      <div v-else>
                        <div style="background: rgb(248 249 250); height: 200px; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px; margin-bottom: 5px;">
                          <div class="el-loading-spinner" style="position: relative; top: 50px;">
                            <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                            <p class="el-loading-text" style="word-break: break-word;">Đang chấm điểm 4 tiêu chí</p>
                          </div>
                        </div>
                      </div>
                      <div v-if="loadCriteriaFeedbackCompleted">
                        <el-card
                          v-for="criteria in rubricCriteria"
                          :key="criteria.criteriaId"
                          style="margin-bottom: 5px; border: 1px solid rgb(190, 190, 190);"
                          shadow="hover"
                        >
                          <div slot="header" class="clearfix">
                            <div>
                              <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                                <span v-if="criteria.name == 'Critical Errors'">Nâng Cấp Từ Vựng Và Ngữ Pháp</span>
                                <span v-else-if="criteria.name == 'Arguments Assessment'">Củng Cố Lập Luận</span>
                                <span v-else-if="criteria.name == 'Vocabulary'">Từ Vựng Tham Khảo</span>
                                <span v-else-if="criteria.name == 'Improved Version'">Phiên Bản Cải Thiện</span>
                                <span v-else>{{ criteria.name }}</span>
                              </div>
                              <div style="float: right;">
                                <div v-if="criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment' && criteria.name != 'Vocabulary' && criteria.name != 'Improved Version'">
                                  <div v-if="isAiReview">
                                    <div v-if="!criteria.loading && criteria.mark" class="band-score">
                                      Band:
                                      {{ criteria.mark.toString().length == 1 ? criteria.mark.toString() + '.0' : criteria.mark }}
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div>
                            <div>
                              <div>
                                <pre style="border: #bcbcbc solid 1px; padding: 10px; border-radius: 5px;" v-html="criteria.comment" />
                              </div>
                            </div>
                          </div>
                        </el-card>
                        <el-card
                          v-if="isAiReview && rubricCriteria && rubricCriteria.length > 0"
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
                      <div v-else>
                        <div style="background: rgb(248 249 250); height: 200px; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                          <div class="el-loading-spinner" style="position: relative; top: 50px;">
                            <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                            <p class="el-loading-text" style="word-break: break-word;">Đang đánh giá bài viết và cung cấp phản hồi</p>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div v-else>
                      <div v-if="loadingReview" style="background: rgb(248 249 250); height: 92vh; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                        <div class="el-loading-spinner" style="position: relative; top: 220px;">
                          <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                          <p class="el-loading-text" style="word-break: break-word;">Đang tải chủ đề viết và các tiêu chí chấm bài</p>
                        </div>
                      </div>
                      <div v-else-if="hasGrade" style="background: rgb(248 249 250); height: 92vh; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                        <div class="el-loading-spinner" style="position: relative; top: 220px;">
                          <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                          <p class="el-loading-text" style="word-break: break-word;">Đang tải phản hồi cho bài viết</p>
                        </div>
                      </div>
                      <div v-else style="background: rgb(248 249 250); height: 92vh; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                        <div class="el-loading-spinner" style="position: relative; top: 220px;">
                          <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                          <p class="el-loading-text" style="word-break: break-word;">Đang chấm 4 tiêu chí, đánh giá lập luận, và cung cấp gợi ý cải thiện</p>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

          </el-tab-pane>
          <el-tab-pane name="question" label="Chủ đề">
            <tabQuestion
              v-if="review"
              ref="tabQuestion"
              :questionid="questionId"
            />
            <div
              v-if="!review"
              v-loading="true"
              style="height: 520px;"
              element-loading-text="Đang tải chủ đề và tiêu chí đánh giá"
              element-loading-background="white"
            />
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
          :is-rate="isRate"
          :is-rated="isRated"
          :is-author="isReviewAuth"
          :is-submit="isSubmit"
          :is-loading="isLoading"
          @expandColorPickerToggle="expandColorPicker=$event"
          @hideQuestion="hideQuestion($event)"
          @redoAnnotation="redo($event)"
          @undoAnnotation="undoAnnotation"
          @scaleChange="handleScale($event)"
          @highLightText="highlightEvent($event)"
          @toolBarButtonChange="toolBarButtonClick($event)"
          @rateBtnClick="rateReview"
          @openDialogRevise="reviseDialogVisible = true"
          @closeDialogRevise="reviseDialogVisible = false"
        />

        <div id="viewerContainer" style="height: calc(100vh - 50px); ">
          <div v-if="!completeLoading" style="width: calc(100% - 10px);">
            <div
              v-loading="true"
              style="height: 545px;"
              element-loading-text="Đang tìm các lỗi trong bài viết và lựa chọn phương án sửa phù hợp"
              element-loading-background="rgb(248 249 250)"
            />
          </div>
          <div
            v-else
            id="viewer"
            :class="{'freeText': activeButton== 'text'}"
            class="pdfViewer"
            :document-id="documentId"
          />
        </div>
      </div>

    </div>
    <div id="comment-wrapper">
      <el-card
        v-for="comment in comments"
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
        <div
          slot="header"
          class="clearfix"
          :style="comment.type.toLowerCase() == 'grammar' || comment.type.toLowerCase() == 'ngữ pháp' ? 'padding: 8px 12px; background: #ffd6d0;' : 'padding: 8px 12px; background: rgb(213 204 255);'"
        >
          <div>
            <div
              v-if="comment.category"
              style="font-weight: 500; font-size: 15px; text-align: left;  width: 220px; text-overflow: ellipsis; word-break: break-word;  overflow: hidden; white-space: nowrap;"
            >
              {{ comment.category }}
            </div>
          </div>
        </div>
        <div>
          <div
            v-show="!comment.isSelected "
            :id="'comment-text-' + comment.uuid"
            :style="{'-webkit-line-clamp': isInShowMoreList(comment.uuid).value==0 ? '3':'inherit'}"
            style="text-align: left; overflow-wrap: break-word; display: -webkit-box;overflow: hidden;-webkit-box-orient: vertical;"
          >
            <div>{{ comment.comment }}</div>
          </div>
          <div v-if="comment.fix" style="margin-top: 10px; border-top: #aeaeae dashed 1px; padding-top: 10px;">
            <div><b>Bản sửa:</b> "{{ comment.fix }}"</div>
            <!-- <div v-if="comment.explain && comment.explain != comment.comment && comment.category != 'Spelling Mistake' && !comment.comment.toLowerCase().includes(comment.fix.toLowerCase())">
              <b>Giải thích:</b> {{ comment.explain }}
            </div> -->
          </div>
          <div v-if="isInShowMoreList(comment.uuid)" class="show__more-container" @click="toggleShowMore(comment.uuid)">
            {{ isInShowMoreList(comment.uuid).value==0 ? 'Xem thêm':'Rút gọn' }}
          </div>
          <el-input
            v-show="comment.isSelected == true || comment.isSaved == false"
            :id="'comment-input-' + comment.uuid"
            :ref="'comment' + comment.uuid"
            v-model="comment.comment"
            type="textarea"
            resize="none"
            autosize
          />
          <div v-show="comment.isSelected == true || comment.isSaved == false" style="height: 20px; margin-top: 10px;">
            <el-button type="primary" :disabled="comment.comment.replace(/\s/g, '').length == 0" style="float: left; padding: 5px;" @click="editCommentCard(comment)">
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
      <!-- <span>
        Stroke
      </span> -->
      <ul class="group-color" style="margin-top: 15px;">
        <li v-for="item in listColor" :key="item.name" @click="changeColor(item.name)"><button :style="{'background-color': item.name, height:18+'px',width:18+'px', margin:'5px 5px 5px 5px', 'border-radius':'50%','outline': 'none','border': 'none'}" /></li>
      </ul>
    </div>
    <!-- /Inline color picker -->

  </div>
  <!-- Optimize for mobile -->
  <div v-else id="reviewContainer">
    <div style="width: 100%; margin-top: 55px; height: calc(100vh - 60px); overflow: auto;">
      <!-- Question, rubric, and rate tab -->
      <div id="tabs-wrapper">
        <el-tabs v-model="selectedTab" type="border-card" style="margin-bottom: 10px;">
          <el-tab-pane name="rubric" label="Phản hồi cho bài viết">
            <div id="parent-scroll" style="flex-grow: 1;position: relative;">
              <div id="child-scroll">
                <div id="rubric">
                  <!-- <div v-if="loadCriteriaFeedbackCompleted" style="height: 100%; overflow: auto; padding-bottom: 20px;">
                    <el-card
                      v-for="criteria in rubricCriteria"
                      :key="criteria.criteriaId"
                      style="margin-bottom: 5px; margin-left: 3px; border: 1px solid rgb(190, 190, 190);"
                      shadow="hover"
                    >
                      <div slot="header" class="clearfix">
                        <div>
                          <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                            <span v-if="criteria.name == 'Critical Errors'">Nâng Cấp Từ Vựng Và Ngữ Pháp</span>
                            <span v-else-if="criteria.name == 'Arguments Assessment'">Củng Cố Lập Luận</span>
                            <span v-else-if="criteria.name == 'Vocabulary'">Từ Vựng Tham Khảo</span>
                            <span v-else-if="criteria.name == 'Improved Version'">Phiên Bản Cải Thiện</span>
                            <span v-else> Tiêu Chí {{ criteria.name }}</span>
                          </div>
                          <div style="float: right;">
                            <div v-if="criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment' && criteria.name != 'Vocabulary' && criteria.name != 'Improved Version'">
                              <div v-if="isAiReview">
                                <div v-if="!criteria.loading && criteria.mark" class="band-score">
                                  Band:
                                  {{ criteria.mark.toString().length == 1 ? criteria.mark.toString() + '.0' : criteria.mark }}
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div>
                        <div>
                          <div>
                            <pre style="border: #bcbcbc solid 1px; padding: 10px; border-radius: 5px;" v-html="criteria.comment" />
                          </div>
                        </div>
                      </div>
                    </el-card>
                    <el-card
                      v-if="isAiReview && rubricCriteria && rubricCriteria.length > 0"
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
                  </div> -->
                  <div v-if="loadCriteriaFeedbackCompleted || loadEssayScoreCompleted" style="height: 100%; overflow: auto; padding-bottom: 20px;">
                    <div v-if="loadEssayScoreCompleted">
                      <el-card
                        style="margin-bottom: 5px; border: 1px solid rgb(190, 190, 190);"
                        shadow="hover"
                      >
                        <div slot="header" class="clearfix">
                          <div>
                            <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                              <span>Overall Band Score</span>
                            </div>
                            <div style="float: right;">
                              <div>
                                <div>
                                  <div v-if="essayScore.overallBandScore" class="band-score">
                                    Band: {{ essayScore.overallBandScore.toString().length == 1 ? essayScore.overallBandScore.toString() + '.0' : essayScore.overallBandScore }}
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div style="border: 1px solid rgb(188, 188, 188); padding: 10px; border-radius: 5px;">
                          <div v-if="essayScore.taskAchievementScore" class="criteria-score">
                            <div>
                              <b>
                                Task Achievement:
                                {{ essayScore.taskAchievementScore.toString().length == 1 ? essayScore.taskAchievementScore.toString() + '.0' : essayScore.taskAchievementScore }}
                              </b>
                            </div>
                            <div v-if="essayScore.highlightKeyFeatures" class="sub-criteria">
                              <el-tag
                                :type="essayScore.highlightKeyFeatures > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                                  essayScore.highlightKeyFeatures == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.highlightKeyFeatures.toString().length == 1 ? essayScore.highlightKeyFeatures.toString() + '.0' : essayScore.highlightKeyFeatures }}
                              </el-tag>
                              <div class="sub-criteria-label">Highlighting Key Features</div>
                            </div>
                            <div v-if="essayScore.compareAndContrast" class="sub-criteria">
                              <el-tag
                                :type="essayScore.compareAndContrast > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                                  essayScore.compareAndContrast == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.compareAndContrast.toString().length == 1 ? essayScore.compareAndContrast.toString() + '.0' : essayScore.compareAndContrast }}
                              </el-tag>
                              <div class="sub-criteria-label">Comparing and Contrasting Data</div>
                            </div>

                            <div v-if="essayScore.dataSelection" class="sub-criteria">
                              <el-tag
                                :type="essayScore.dataSelection > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                                  essayScore.dataSelection == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.dataSelection.toString().length == 1 ? essayScore.dataSelection.toString() + '.0' : essayScore.dataSelection }}
                              </el-tag>
                              <div class="sub-criteria-label">Data Selection and Relevance</div>
                            </div>
                            <div class="sub-criteria">
                              <el-tag v-if="essayScore.appropriateWordCount" type="success" size="small">
                                <i class="criteria-score-check el-icon-check" />
                              </el-tag>
                              <el-tag v-else type="danger" size="small">
                                <i class="el-icon-close criteria-score-close" />
                              </el-tag>
                              <span class="sub-criteria-label">Appropriate Word Count</span>
                            </div>
                          </div>

                          <div v-if="essayScore.taskResponseScore" class="criteria-score">
                            <div>
                              <b>
                                Task Response:
                                {{ essayScore.taskResponseScore.toString().length == 1 ? essayScore.taskResponseScore.toString() + '.0' : essayScore.taskResponseScore }}
                              </b>
                            </div>
                            <div v-if="essayScore.clarityOfPosition" class="sub-criteria">
                              <el-tag
                                :type="essayScore.clarityOfPosition > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                                  essayScore.clarityOfPosition == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.clarityOfPosition.toString().length == 1 ? essayScore.clarityOfPosition.toString() + '.0' : essayScore.clarityOfPosition }}
                              </el-tag>
                              <span class="sub-criteria-label">Clarity of Position</span>
                            </div>
                            <div v-if="essayScore.developmentOfIdeas" class="sub-criteria">
                              <el-tag
                                :type="essayScore.developmentOfIdeas > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                                  essayScore.developmentOfIdeas == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.developmentOfIdeas.toString().length == 1 ? essayScore.developmentOfIdeas.toString() + '.0' : essayScore.developmentOfIdeas }}
                              </el-tag>
                              <span class="sub-criteria-label">Development of Ideas</span>
                            </div>
                            <div v-if="essayScore.justificationOfOpinion" class="sub-criteria">
                              <el-tag
                                :type="essayScore.justificationOfOpinion > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                                  essayScore.justificationOfOpinion == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.justificationOfOpinion.toString().length == 1 ? essayScore.justificationOfOpinion.toString() + '.0' : essayScore.justificationOfOpinion }}
                              </el-tag>
                              <span class="sub-criteria-label">Justification of Opinions</span>
                            </div>
                            <div class="sub-criteria">
                              <el-tag v-if="essayScore.appropriateWordCount" type="success" size="small">
                                <i class="criteria-score-check el-icon-check" />
                              </el-tag>
                              <el-tag v-else type="danger" size="small">
                                <i class="el-icon-close criteria-score-close" />
                              </el-tag>
                              <span class="sub-criteria-label">Appropriate Word Count</span>
                            </div>
                          </div>

                          <div v-if="essayScore.coherenceScore" class="criteria-score">
                            <div>
                              <b>
                                Coherence and Cohesion:
                                {{ essayScore.coherenceScore.toString().length == 1 ? essayScore.coherenceScore.toString() + '.0' : essayScore.coherenceScore }}
                              </b>
                            </div>
                            <div v-if="essayScore.logicalOrganization" class="sub-criteria">
                              <el-tag
                                :type="essayScore.logicalOrganization > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                                  essayScore.logicalOrganization == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.logicalOrganization.toString().length == 1 ? essayScore.logicalOrganization.toString() + '.0' : essayScore.logicalOrganization }}
                              </el-tag>
                              <div class="sub-criteria-label">Logical Organization</div>
                            </div>
                            <div v-if="essayScore.paragraphing" class="sub-criteria">
                              <el-tag
                                :type="essayScore.paragraphing > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                                  essayScore.paragraphing == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.paragraphing.toString().length == 1 ? essayScore.paragraphing.toString() + '.0' : essayScore.paragraphing }}
                              </el-tag>
                              <div class="sub-criteria-label">Paragraphing</div>
                            </div>
                            <div v-if="essayScore.cohesiveDevices" class="sub-criteria">
                              <el-tag
                                :type="essayScore.cohesiveDevices > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                                  essayScore.cohesiveDevices == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.cohesiveDevices.toString().length == 1 ? essayScore.cohesiveDevices.toString() + '.0' : essayScore.cohesiveDevices }}
                              </el-tag>
                              <div class="sub-criteria-label">Use of Cohesive Devices</div>
                            </div>
                          </div>

                          <div v-if="essayScore.lexicalResourceScore" class="criteria-score">
                            <div>
                              <b>
                                Lexical Resource:
                                {{ essayScore.lexicalResourceScore.toString().length == 1 ? essayScore.lexicalResourceScore.toString() + '.0' : essayScore.lexicalResourceScore }}
                              </b>
                            </div>
                            <div v-if="essayScore.rangeOfVocabulary" class="sub-criteria">
                              <el-tag
                                :type="essayScore.rangeOfVocabulary > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                  essayScore.rangeOfVocabulary == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.rangeOfVocabulary.toString().length == 1 ? essayScore.rangeOfVocabulary.toString() + '.0' : essayScore.rangeOfVocabulary }}
                              </el-tag>
                              <div class="sub-criteria-label">Range of Vocabulary</div>
                            </div>
                            <div v-if="essayScore.accuracyOfWordChoice" class="sub-criteria">
                              <el-tag
                                :type="essayScore.accuracyOfWordChoice > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                  essayScore.accuracyOfWordChoice == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.accuracyOfWordChoice.toString().length == 1 ? essayScore.accuracyOfWordChoice.toString() + '.0' : essayScore.accuracyOfWordChoice }}
                              </el-tag>
                              <div class="sub-criteria-label">Accuracy of Word Choice</div>
                            </div>
                            <div v-if="essayScore.spellingAndFormation" class="sub-criteria">
                              <el-tag
                                :type="essayScore.spellingAndFormation > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                  essayScore.spellingAndFormation == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.spellingAndFormation.toString().length == 1 ? essayScore.spellingAndFormation.toString() + '.0' : essayScore.spellingAndFormation }}
                              </el-tag>
                              <div class="sub-criteria-label">Spelling and Word Formation</div>
                            </div>
                            <div v-if="essayScore.registerAndStyle" class="sub-criteria">
                              <el-tag
                                :type="essayScore.registerAndStyle > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                                  essayScore.registerAndStyle == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.registerAndStyle.toString().length == 1 ? essayScore.registerAndStyle.toString() + '.0' : essayScore.registerAndStyle }}
                              </el-tag>
                              <div class="sub-criteria-label">Appropriateness of Register and Style</div>
                            </div>
                          </div>

                          <div v-if="essayScore.grammarScore" class="criteria-score">
                            <div>
                              <b>
                                Grammatical Range and Accuracy:
                                {{ essayScore.grammarScore.toString().length == 1 ? essayScore.grammarScore.toString() + '.0' : essayScore.grammarScore }}
                              </b>
                            </div>
                            <div v-if="essayScore.grammarRange" class="sub-criteria">
                              <el-tag
                                :type="essayScore.grammarRange > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                                  essayScore.grammarRange == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.grammarRange.toString().length == 1 ? essayScore.grammarRange.toString() + '.0' : essayScore.grammarRange }}
                              </el-tag>
                              <div class="sub-criteria-label">Range of Grammatical Structures</div>
                            </div>
                            <div v-if="essayScore.sentenceComplexity" class="sub-criteria">
                              <el-tag
                                :type="essayScore.sentenceComplexity > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                                  essayScore.sentenceComplexity == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.sentenceComplexity.toString().length == 1 ? essayScore.sentenceComplexity.toString() + '.0' : essayScore.sentenceComplexity }}
                              </el-tag>
                              <div class="sub-criteria-label">Sentence Complexity</div>
                            </div>
                            <div v-if="essayScore.grammarAccuracy" class="sub-criteria">
                              <el-tag
                                :type="essayScore.grammarAccuracy > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                                  essayScore.grammarAccuracy == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                                  ? 'success' : 'danger'"
                                size="small"
                                class="sub-score-tag"
                              >
                                {{ essayScore.grammarAccuracy.toString().length == 1 ? essayScore.grammarAccuracy.toString() + '.0' : essayScore.grammarAccuracy }}
                              </el-tag>
                              <div class="sub-criteria-label">Accuracy in Grammatical Forms</div>
                            </div>
                          </div>
                        </div>
                        <div v-if="review && review.reviewRequest && review.reviewRequest.feedbackLanguage == 'vn'" style="font-size: 13px; margin-top: 5px; padding:2px;">
                          <div style="background-color: rgb(244, 244, 245); border-color: rgb(233, 233, 235); color: rgb(144, 147, 153); font-size: 12px; border-width: 1px; border-style: solid; border-radius: 4px; padding: 10px;">
                            Xin lưu ý rằng điểm số được cung cấp không phải là điểm IELTS chính thức của bạn và chỉ nên được sử dụng cho mục đích học tập và cải thiện kỹ năng.
                          </div>
                        </div>
                        <div v-else style="font-size: 13px; margin-top: 5px;">
                          <div style="background-color: rgb(244, 244, 245); border-color: rgb(233, 233, 235); color: rgb(144, 147, 153); font-size: 12px; border-width: 1px; border-style: solid; border-radius: 4px; padding: 10px;">
                            Please note that the scores provided are not your official IELTS scores and should be used solely for learning and practicing purposes.
                          </div>
                        </div>
                      </el-card>
                    </div>
                    <div v-else>
                      <div style="background: rgb(248 249 250); height: 200px; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px; margin-bottom: 5px;">
                        <div class="el-loading-spinner" style="position: relative; top: 50px;">
                          <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                          <p class="el-loading-text" style="word-break: break-word;">Đang chấm điểm 4 tiêu chí</p>
                        </div>
                      </div>
                    </div>
                    <div v-if="loadCriteriaFeedbackCompleted">
                      <el-card
                        v-for="criteria in rubricCriteria"
                        :key="criteria.criteriaId"
                        style="margin-bottom: 5px; border: 1px solid rgb(190, 190, 190);"
                        shadow="hover"
                      >
                        <div slot="header" class="clearfix">
                          <div>
                            <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                              <span v-if="criteria.name == 'Critical Errors'">Nâng Cấp Từ Vựng Và Ngữ Pháp</span>
                              <span v-else-if="criteria.name == 'Arguments Assessment'">Củng Cố Lập Luận</span>
                              <span v-else-if="criteria.name == 'Vocabulary'">Từ Vựng Tham Khảo</span>
                              <span v-else-if="criteria.name == 'Improved Version'">Phiên Bản Cải Thiện</span>
                              <span v-else>{{ criteria.name }}</span>
                            </div>
                            <div style="float: right;">
                              <div v-if="criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment' && criteria.name != 'Vocabulary' && criteria.name != 'Improved Version'">
                                <div v-if="isAiReview">
                                  <div v-if="!criteria.loading && criteria.mark" class="band-score">
                                    Band:
                                    {{ criteria.mark.toString().length == 1 ? criteria.mark.toString() + '.0' : criteria.mark }}
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div>
                          <div>
                            <div>
                              <pre style="border: #bcbcbc solid 1px; padding: 10px; border-radius: 5px;" v-html="criteria.comment" />
                            </div>
                          </div>
                        </div>
                      </el-card>
                      <el-card
                        v-if="isAiReview && rubricCriteria && rubricCriteria.length > 0"
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
                    <div v-else>
                      <div style="background: rgb(248 249 250); height: 200px; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                        <div class="el-loading-spinner" style="position: relative; top: 50px;">
                          <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                          <p class="el-loading-text" style="word-break: break-word;">Đang đánh giá bài viết và cung cấp phản hồi</p>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div v-else>
                    <div v-if="loadingReview" style="background: rgb(248 249 250); height: 92vh; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                      <div class="el-loading-spinner" style="position: relative; top: 220px;">
                        <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                        <p class="el-loading-text" style="word-break: break-word;">Đang tải chủ đề viết và các tiêu chí chấm bài</p>
                      </div>
                    </div>
                    <div v-else-if="hasGrade" style="background: rgb(248 249 250); height: 92vh; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                      <div class="el-loading-spinner" style="position: relative; top: 220px;">
                        <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                        <p class="el-loading-text" style="word-break: break-word;">Đang tải phản hồi cho bài viết</p>
                      </div>
                    </div>
                    <div v-else style="background: rgb(248 249 250); height: 92vh; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px;">
                      <div class="el-loading-spinner" style="position: relative; top: 220px;">
                        <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                        <p class="el-loading-text" style="word-break: break-word;">Đang đánh giá bài luận và cung cấp gợi ý cải thiện</p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane name="question" label="Chủ đề">
            <tabQuestion
              v-if="review"
              ref="tabQuestion"
              :questionid="questionId"
            />
            <div
              v-if="!review"
              v-loading="true"
              style="height: 400px;"
              element-loading-text="Đang tải chủ đề và tiêu chí đánh giá"
              element-loading-background="white"
            />
          </el-tab-pane>

        </el-tabs>
      </div>
      <div id="document-wrapper">
        <toolbar
          ref="toolBar"
          :review-page="this"
          :expandcolorpicker.sync="expandColorPicker"
          :documentid="documentId"
          :reviewid="reviewId"
          :renderoptions="RENDER_OPTIONS"
          :is-rate="isRate"
          :is-rated="isRated"
          :is-author="isReviewAuth"
          :is-submit="isSubmit"
          :is-loading="isLoading"
          @expandColorPickerToggle="expandColorPicker=$event"
          @hideQuestion="hideQuestion($event)"
          @redoAnnotation="redo($event)"
          @undoAnnotation="undoAnnotation"
          @scaleChange="handleScale($event)"
          @highLightText="highlightEvent($event)"
          @toolBarButtonChange="toolBarButtonClick($event)"
          @rateBtnClick="rateReview"
          @openDialogRevise="reviseDialogVisible = true"
          @closeDialogRevise="reviseDialogVisible = false"
        />

        <div id="viewerContainer">
          <div v-if="!completeLoading" style="width: calc(100% - 10px);">
            <div
              v-loading="true"
              style="height: 545px;"
              element-loading-text="Đang tìm các lỗi trong bài viết và lựa chọn phương án sửa phù hợp"
              element-loading-background="rgb(248 249 250)"
            />
          </div>
          <div
            v-else
            id="viewer"
            :class="{'freeText': activeButton== 'text'}"
            class="pdfViewer"
            :document-id="documentId"
          />

        </div>
      </div>

      <div id="comment-wrapper">
        <el-card
          v-for="comment in comments"
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
          <div
            slot="header"
            class="clearfix"
            :style="comment.type.toLowerCase() == 'grammar' || comment.type.toLowerCase() == 'ngữ pháp' ? 'padding: 8px 12px; background: #ffd6d0;' : 'padding: 8px 12px; background: rgb(213 204 255);'"
          >
            <div>
              <div
                v-if="comment.category"
                style="font-weight: 500; font-size: 15px; text-align: left;  width: 220px; text-overflow: ellipsis; word-break: break-word;  overflow: hidden; white-space: nowrap;"
              >
                {{ comment.category }}
              </div>
            </div>
          </div>
          <div>
            <div
              v-show="!comment.isSelected "
              :id="'comment-text-' + comment.uuid"
              :style="{'-webkit-line-clamp': isInShowMoreList(comment.uuid).value==0 ? '3':'inherit'}"
              style="text-align: left; overflow-wrap: break-word; display: -webkit-box;overflow: hidden;-webkit-box-orient: vertical;"
            >
              {{ comment.comment }}
            </div>
            <div v-if="comment.fix" style="margin-top: 10px; border-top: #aeaeae dashed 1px; padding-top: 10px;">
              <div><b>Bản sửa:</b> "{{ comment.fix }}"</div>
              <div v-if="comment.explain && comment.explain != comment.comment && comment.category != 'Spelling Mistake' ">
                <b>Giải thích:</b> {{ comment.explain }}
              </div>
            </div>
            <div v-if="isInShowMoreList(comment.uuid)" class="show__more-container" @click="toggleShowMore(comment.uuid)">
              {{ isInShowMoreList(comment.uuid).value==0 ? 'Xem thêm':'Rút gọn' }}
            </div>
            <el-input
              v-show="comment.isSelected == true || comment.isSaved == false"
              :id="'comment-input-' + comment.uuid"
              :ref="'comment' + comment.uuid"
              v-model="comment.comment"
              type="textarea"
              resize="none"
              autosize
            />
            <div v-show="comment.isSelected == true || comment.isSaved == false" style="height: 20px; margin-top: 10px;">
              <el-button type="primary" :disabled="comment.comment.replace(/\s/g, '').length == 0" style="float: left; padding: 5px;" @click="editCommentCard(comment)">
                Save
              </el-button>
              <el-button style="float: right; padding: 5px;" @click="cancelCommentText('edit',comment)">
                Cancel
              </el-button>
            </div>
          </div>
        </el-card>
      </div>

      <div id="text-tools-wrapper">
        <textToolGroup ref="textToolGroup" @highLightText="highlightEvent($event)" />
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
          <ul class="group-color" style="margin-top: 15px;">
            <li v-for="item in listColor" :key="item.name" @click="changeColor(item.name)"><button :style="{'background-color': item.name, height:18+'px',width:18+'px', margin:'5px 5px 5px 5px', 'border-radius':'50%','outline': 'none','border': 'none'}" /></li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import PDFJS from 'pdf-dist/webpack.js'
import ToolBar from '../../components/controls/Viewer_ToolBar'
import TabQuestion from './Review_TabQuestion'
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
import { addEventListener, removeEventListener } from '@/pdfjs/UI/event'
import appendChild from '@/pdfjs/render/appendChild'
import docService from '@/services/document.service.js'
import reviewService from '@/services/review.service.js'

import { enableTextSelection } from '@/pdfjs/UI/select-text.js'
import initColorPicker from '../../pdfjs/shared/initColorPicker'
import { deleteAnnotations, editTextBox } from '@/pdfjs/UI/edit.js'
import moment from 'moment-timezone'

export default {
  name: 'Document',
  components: {
    'toolbar': ToolBar,
    'tabQuestion': TabQuestion,
    'textToolGroup': TextToolGroup
  },
  data() {
    return {
      reviseDialogVisible: false,
      form: {
        noteRevision: null
      },
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
      isRendering: false,
      commentsNotSaved: [],
      isView: false,
      isRate: false,
      rateValue: 0,
      rateComment: null,
      selectedTab: 'rubric',
      isReviewAuth: false,
      isRated: false,
      isSubmit: false,
      isDisputed: false,
      showRubric: true,
      isAddingNewComment: false,
      completeLoading: false,
      isAiReview: false,
      isLoading: false,
      review: null,
      screenWidth: window.innerWidth,
      isSelfReview: false,
      task: null,
      documentText: null,
      textNodes: [],
      docData: null,
      intextCommentCompleted: false,
      hasGrade: false,
      errors: null,
      rubricCriteria: null,
      loadCriteriaFeedbackCompleted: false,
      loadingReview: false,
      freeToken: this.$store.state.auth.user.freeToken,
      premiumToken: this.$store.state.auth.user.premiumToken,
      userSubscription: this.$store.state.auth.user.subscription,
      chartDescription: null,
      essayScore: null,
      loadEssayScoreCompleted: false,
      loadFeedbackCompleted: false,
      loadErrorsCompleted: false
    }
  },
  computed: {
    loadedAnnotation() {
      const data = this.$store.getters['review/getAnnotations']
      if (!data || (!data.annotations && !data.comments && !data.reviewer)) {
        return null
      }
      return {
        annotations: data.annotations.map(a => ({ ...JSON.parse(a.data), id: a.id, top: a.top, pageNum: a.pageNum, color: a.color })),
        comments: data.comments.map(c => ({ ...JSON.parse(c.data), documentId: this.documentId, id: c.id })),
        reviewer: data.user
      }
    },
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    getDateNow() {
      var tz = moment.tz.guess()
      return moment.utc().tz(tz).format('DD/MM/YYYY LT')
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  async beforeMount() {
    if (this.$route.params.docId && this.$route.params.reviewId && this.$route.params.questionId) {
      this.questionId = parseInt(this.$route.params.questionId)
      this.reviewId = parseInt(this.$route.params.reviewId)
      this.documentId = parseInt(this.$route.params.docId)
      this.isView = this.$route.params.isViewOrRate === 'view' || this.$route.params.isViewOrRate === 'rate'
      this.RENDER_OPTIONS.documentId = this.documentId
      this.isRate = this.$route.params.isViewOrRate === 'rate'
    }
  },
  async mounted() {
    this.zoomOutMobile()
    this.loadingReview = true
    if (this.$route.query.plain) {
      this.showQuestion = false
    }

    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })

    window.component = this
    window['PDFJSAnnotate'] = PDFJSAnnotate
    window['APP'] = this
    localStorage.setItem(`${this.documentId}/tooltype`, 'cursor')
    localStorage.setItem(`${this.documentId}/color`, '#ff0000')
    PDFJSAnnotate.getStoreAdapter().clearAnnotations(this.documentId)

    // Load review data and review request
    this.review = await reviewService.getById(this.$route.params.reviewId)

    if (this.review) {
      if (this.review.review.reviewerId === 'AI') {
        this.isAiReview = true
      }
      if (this.review.review.finalScore) { this.hasGrade = true }
    }

    this.loadingReview = false

    // Load question and get task info
    const question = await this.$store.dispatch('question/loadQuestion', this.questionId)
    this.task = question.section
    document.title = 'Đánh giá - ' + question.title

    // Load document and its data
    const doc = await docService.getDocument(this.documentId)
    this.documentText = doc.data.text
    this.docData = this.base64ToArrayBuffer(doc.data.data)

    // await this.loadRubric()
    if (!this.hasGrade) {
      const chart = question.questionsPart.find(q => q.name == 'Chart')
      if (chart) {
        this.chartDescription = await reviewService.getChartDescription(chart.content)
      }
    }
    this.getReviewScores()
    this.getReviewFeedback()

    // Get in-text comments
    await this.$store.dispatch('review/loadReviewAnnotation', { docId: this.documentId, reviewId: this.reviewId })
    if (this.loadedAnnotation && this.loadedAnnotation.annotations && this.loadedAnnotation.annotations.length > 0) {
      console.log(this.loadedAnnotation.annotations)
      // load in-text comments saved in db
      PDFJSAnnotate.getStoreAdapter().loadAnnotations(this.documentId, this.loadedAnnotation)
      this.intextCommentCompleted = true
      this.finalizeFeedback()
    } else {
      // Get new list of in-text comments and create the annotations
      const model = {
        userId: this.$store.state.auth.user.id,
        task: question.section,
        topic: question.questionsPart.find(q => q.name == 'Question').content,
        essay: this.documentText,
        feedbackLanguage: this.review.reviewRequest.feedbackLanguage
      }
      const response = await reviewService.getIntextComments(model)
      if (response) {
        this.loadErrorsCompleted = true
        this.errors = response.errors
        console.log('Errors:', this.errors)
        this.finalizeFeedback()
      } else {
        this.loadErrorsCompleted = true
        this.finalizeFeedback()
        this.$notify.error({
          title: 'Không thể sửa lỗi trong bài viết',
          message: 'Đã có sự cố xảy ra trong quá trình sửa lỗi',
          type: 'error',
          duration: 5000
        })
      }
    }

    // render the document
    this.completeLoading = true
    await this.render()

    if (this.errors && this.errors.length > 0) {
      this.highlightErrors()
    } else {
      this.handleCommentPositionsRestore()
      this.intextCommentCompleted = true
    }

    this.$refs.toolBar?.handleScale('fitPage')
    this.$refs.toolBar?.insertExpandMenu()
  },
  beforeCreate() {
    document.body.style = 'overflow: hidden'
  },
  created() {
    window.addEventListener('beforeunload', this.beforeWindowUnload)
  },
  beforeDestroy() {
    window.removeEventListener('beforeunload', this.beforeWindowUnload)
  },
  destroyed() {
    clearInterval(this.setIntervalForScroll)
    document.removeEventListener('keyup', this.keyupHandler)
    this.unRegisterEvents()
    document.body.style.overflow = null
  },
  beforeRouteLeave (to, from, next) {
    // If the grading has not finished yet
    // if (!this.intextCommentCompleted || !this.loadCriteriaFeedbackCompleted || !this.loadEssayScoreCompleted) {
      if (!this.loadFeedbackCompleted) {
      this.$confirm('Bài viết chưa được chấm xong nên toàn bộ phản hồi có thể sẽ bị mất. Bạn chắc chứ?', {
        confirmButtonText: 'OK',
        cancelButtonText: 'Cancel',
        type: 'warning'
      }).then(() => {
        next()
      }).catch(() => {
      })
    } else {
      next()
    }
  },
  methods: {
    finalizeFeedback() {
      if (!this.hasGrade && this.loadErrorsCompleted && this.essayScore && this.rubricCriteria) {
        this.loadFeedbackCompleted = true
        // 1. Populate data for the critical errors criteria
        if (this.errors && this.errors.length > 0) {
          let errorFeedback = ''
          let explain = 'Explain'
          if (this.review.reviewRequest.feedbackLanguage == 'vn') { explain = 'Giải thích' }
          for (let i = 0; i < this.errors.length; i++) {
            const order = i + 1
            const error = order.toString() + ". '" + this.errors[i].error + "' --> '" + this.errors[i].fix + "'\n- " + explain + ': ' + this.errors[i].comment + '\n\n'
            errorFeedback += error
          }
          const criticalError = this.rubricCriteria.find(c => c.name == 'Critical Errors')
          criticalError.comment = errorFeedback
        }

        // 2. populate the score for each criteria
        const taskAchivement = this.rubricCriteria.find(c => c.name == 'Task Achievement')
        if (taskAchivement) { taskAchivement.mark = this.essayScore.taskAchievementScore }
        const taskResponse = this.rubricCriteria.find(c => c.name == 'Task Response')
        if (taskResponse) { taskResponse.mark = this.essayScore.taskResponseScore }
        const coherence = this.rubricCriteria.find(c => c.name == 'Coherence & Cohesion')
        coherence.mark = this.essayScore.coherenceScore
        const lexical = this.rubricCriteria.find(c => c.name == 'Lexical Resource')
        lexical.mark = this.essayScore.lexicalResourceScore
        const grammar = this.rubricCriteria.find(c => c.name == 'Grammatical Range & Accuracy')
        grammar.mark = this.essayScore.grammarScore

        // 3. Save feedback into database
        var reviewData = []
        this.rubricCriteria.forEach(r => {
          reviewData.push({
            CriteriaName: r.name,
            Comment: r.comment,
            CriteriaId: r.criteriaId,
            Score: r.mark != null ? r.mark : 0,
            ReviewId: this.reviewId,
            UserFeedback: null
          })
        })
        console.log('Review Data:', reviewData)
        reviewService.saveRubric(this.reviewId, reviewData)

        console.log('Review Saved')
      } else if (this.loadedAnnotation && this.loadedAnnotation.annotations && this.loadedAnnotation.annotations.length > 0 && this.essayScore && this.rubricCriteria) {
        this.loadFeedbackCompleted = true
        console.log('Feedback load completed')
      }
    },
    zoomOutMobile() {
      const viewport = document.querySelector('meta[name="viewport"]')

      if (viewport) {
        viewport.content = 'initial-scale=1'
        viewport.content = 'width=device-width'
      }
    },
    async getReviewScores() {
      const question = this.$store.getters['question/getSelected']
      const topic = question.questionsPart.find(q => q.name == 'Question').content
      const essay = this.documentText
      // create a review model
      const model = {
        userId: this.$store.state.auth.user.id,
        questionId: this.questionId,
        reviewId: this.reviewId,
        topic: topic,
        essay: essay,
        task: question.section,
        hasGrade: this.hasGrade,
        chartDescription: this.chartDescription,
        feedbackLanguage: this.review.reviewRequest.feedbackLanguage
      }
      // get review feedback
      reviewService.getEssayScore(model).then(rs => {
        if (rs) {
          console.log('Review Scores:', rs)
          this.essayScore = rs

          this.loadEssayScoreCompleted = true
          this.finalizeFeedback()
        } else {
          this.$notify.error({
            title: 'Không thể chấm bài luận',
            message: 'Đã có sự cố xảy ra trong quá trình chấm bài',
            type: 'error',
            duration: 5000
          })
        }
      })
    },
    async getReviewFeedback() {
      const question = this.$store.getters['question/getSelected']
      const topic = question.questionsPart.find(q => q.name == 'Question').content
      const essay = this.documentText
      // create a review model
      const model = {
        userId: this.$store.state.auth.user.id,
        questionId: this.questionId,
        reviewId: this.reviewId,
        topic: topic,
        essay: essay,
        task: question.section,
        hasGrade: this.hasGrade,
        chartDescription: this.chartDescription,
        feedbackLanguage: this.review.reviewRequest.feedbackLanguage,
        feedbackType: this.review.reviewRequest.reviewType
      }
      // get review feedback
      reviewService.getReviewFeedback(model).then(rs => {
        if (rs) {
          this.rubricCriteria = rs
          console.log(this.rubricCriteria)
          if (!this.hasGrade) {
            // Update user's free token in store

            if (!this.userSubscription) {
              if (model.feedbackType == 'detail' && this.freeToken > 0) { this.$store.dispatch('auth/updateToken', this.freeToken - 1) } else if (model.feedbackType == 'deep' && this.premiumToken > 0) { this.$store.dispatch('auth/updatePremiumToken', this.premiumToken - 1) }
            }
          }

          this.loadCriteriaFeedbackCompleted = true
          this.finalizeFeedback()
        } else {
          this.$notify.error({
            title: 'Không thể tải phản hồi',
            message: 'Đã có sự cố xảy ra trong quá trình tải phản hồi',
            type: 'error',
            duration: 5000
          })
        }
      })
    },
    beforeWindowUnload(e) {
      // if (!this.intextCommentCompleted || !this.loadCriteriaFeedbackCompleted || !this.loadEssayScoreCompleted) {
      if (!this.loadFeedbackCompleted) {
        // Cancel the event
        e.preventDefault()
        // Chrome requires returnValue to be set
        e.returnValue = ''
      }
    },
    getTextNodes() {
      var textLayer = document.getElementsByClassName('textLayer')[0]
      var walker = document.createTreeWalker(
        textLayer,
        NodeFilter.SHOW_TEXT,
        null,
        false
      )
      while (walker.nextNode()) {
        this.textNodes.push(walker.currentNode)
      }
    },
    async highlightErrors() {
      // this.generateAnnotation()
      // getTextNodes() should be called before highlightErrors()
      this.svg = document.getElementsByClassName('annotationLayer')[0]
      if (!this.svg) {
        return
      }
      const { documentId, pageNumber } = getMetadata(this.svg)
      const boundingRect = this.svg.getBoundingClientRect()
      this.getTextNodes()
      // const searchPhrases = ['demographics of Iceland', 'the other profiles', 'still maintaining the status quo. Furthermore, showing a decade of consistency, the data for the oldest group slightly rose until the end. Regarding the other age brackets,']

      let count = 1
      // this.errors.forEach(error => {
      for (let m = 0; m < this.errors.length; m++) {
        const error = this.errors[m]
        // Trim the search phrase and split into word
        const phrase = error.error
        const phrases = phrase.trim().split(' ')
        // k is the counter for iterating through phrases
        let k = 0
        // rects is list of hilighted rectangles for the search phrase. Each rect associated with each line.
        let rects = []

        // Looping through the lines of text in the document
        for (let i = 0; i < this.textNodes.length; i++) {
          // Record matched and unmatched string for each line
          let matchedString = ''
          let unmatchedString = ''

          // Get all words in a line
          const words = this.textNodes[i].data.split(' ')
          // iterating through earch word
          for (let j = 0; j < words.length; j++) {
            // If current word in the text line matches the current word in the search phrase
            if (words[j] == phrases[k] || words[j] == (phrases[k] + '.') || words[j] == (phrases[k] + ',')) {
              // append to matched string
              if (k == phrase.length - 1) {
                // last word in search phrase
                matchedString += phrases[k]
              } else {
                matchedString += phrases[k] + ' '
              }
              // increase search phrase counter
              k++
              // ends the text line interation if the whole search phrase is found
              if (k == phrases.length) { break }
            } else {
              // If the current word in the text line does not match the current word in the search phrase,
              // check if matchedString has a value, if it is the case, remove the matched string, and check for match again
              // else, append the word in the text line to unmatched string
              if (matchedString != '') {
                // add the matched string to the unmatched string
                unmatchedString += matchedString
                // reset the matched string
                matchedString = ''
                // reset search phrase count
                k = 0
              }
              if (rects.length > 0) {
                rects = [] // reset the rects
                k = 0 // check again
              }
              unmatchedString += words[j] + ' '
            }
          }
          // After interating through each text line, we must have a matched or unmatched string, or both
          if (matchedString != '') {
            // If there is a matched string for the line, we generate the highlighted rectangle for this line
            // console.log('Matched string:', matchedString)
            // Get the width of the matched string
            const matchedWidth = this.getTextWidth(matchedString.trim())
            // console.log('Matched width:', matchedWidth)

            // console.log('Unmatched string:', unmatchedString)
            // Get the width of the unmatched string
            const unmatchedWidth = this.getTextWidth(unmatchedString)
            // console.log('Unmatched width:', unmatchedWidth)

            // Get the rectange bouding the text line
            // this.textNodes[i] will always have a parent div node
            const textRect = this.textNodes[i].parentNode.getBoundingClientRect()
            // console.log('Bouding div rect:', textRect)
            // the rect for this text line based on the 2 calculated widths and the textRect
            const rect = {
              x: textRect.x + unmatchedWidth,
              y: textRect.y,
              width: matchedWidth,
              height: textRect.height,
              top: textRect.top,
              right: textRect.x + unmatchedWidth + matchedWidth,
              bottom: textRect.bottom,
              left: textRect.x + unmatchedWidth
            }
            // console.log('Highlighted rect:', rect)
            rects.push(rect)
            // rects.push(textRect)
          }

          if (k == phrases.length) { break }
        }

        if (rects.length > 0) {
                  // Create the highlighted rectangles
          const rectagles = [...rects].map((r) => {
              const offset = 0
              return scaleDown(this.svg, {
                y: r.top + offset - boundingRect.top,
                x: r.left - boundingRect.left - 1,
                width: r.width + 3,
                height: r.height
              })
            })
            .filter((r) => r.width > 0 && r.height > 0 && r.x > -1 && r.y > -1)

          // console.log('Rectangles: ', rectagles)

          // red for grammar
          var color = 'red'

          if (error.category.toLowerCase() == 'word choice') { error.type = 'vocabulary' }
          if (error.type.toLowerCase() == 'vocabulary') { color = 'blue' }

          var type = 'comment-highlight'
          // Initialize the annotation
          const annotation = {
            type,
            color,
            left: (rects[0].left - boundingRect.left) / this.RENDER_OPTIONS.scale,
            rectangles: rectagles
          }
          // format top with scale 100%
          annotation.color = color
          annotation.top = annotation.rectangles[0].y
          annotation.pageNum = pageNumber
          annotation.pageHeight = parseInt(this.svg.getAttribute('height') / this.RENDER_OPTIONS.scale)

          // Add the annotation
          PDFJSAnnotate.getStoreAdapter().addAnnotation(documentId, pageNumber, annotation)
          .then(annotation => {
            appendChild(this.svg, annotation)
            // add the comment asoociated with the newly created annotation
            PDFJSAnnotate.getStoreAdapter().addComment(this.documentId, annotation, error.error, error.type,
              this.titleCase(error.category), error.comment, error.fix, error.reason, error.explain, annotation.top)
            .then(newComment => {
              newComment.annotation.documentId = this.documentId
              this.comments.push(newComment)
              this.$nextTick(() => {
                const thisComment = document.querySelector(".comment-card[highlight-id='" + annotation.uuid + "']")
                const that = this
                // handle onclick event for this comment
                thisComment.addEventListener('click', function commentCardClick() {
                  that.handleCommentCardClick(this)
                })

                // Add intext comment into database
                var anno = {
                  DocumentId: this.documentId,
                  ReviewId: this.reviewId,
                  Type: annotation.type,
                  PageNum: typeof (annotation.pageNum) != 'undefined' ? annotation.pageNum : annotation.page,
                  Top: typeof (annotation.top) != 'undefined' ? annotation.top : parseInt(annotation.y),
                  Color: annotation.color,
                  Uuid: annotation.uuid,
                  Data: JSON.stringify(annotation),
                  Id: annotation.id
                }

                var newCmt = {
                  Text: newComment.text,
                  Type: newComment.type,
                  Category: newComment.category,
                  Content: newComment.comment,
                  Fix: newComment.fix,
                  Reason: newComment.reason,
                  Explain: newComment.explain,
                  TopPosition: newComment.topPosition,
                  Uuid: newComment.uuid,
                  Data: JSON.stringify(newComment)
                }
                reviewService.addInTextComment(this.documentId, this.reviewId, newCmt, anno)

                // Highlight the first comment
                count++
                if (count == this.errors.length) {
                  this.handleCommentPositionsRestore()
                }
              })
            })
          })
        }
      }

      // this.handleCommentPositionsRestore()
    },
    async handleCommentAnnotationClick(target) {
      // Display text tool bar if text is selected.
      if (target != null) {
        // Get the type of the annotation from the target
        // Currently only suppprt comment highlight
        const type = target.getAttribute('data-pdf-annotate-type')
        this.annotationClicked = target
        if (type == 'comment-highlight') {
          // Get the currently selected elements
          const selectedVocabulary = document.getElementsByClassName('vocabulary-highlight-selected')
          // Loop while there are elements in the collection
          while (selectedVocabulary.length > 0) {
            selectedVocabulary[0].classList.remove('vocabulary-highlight-selected')
          }

          // Get the currently selected elements
          const selectedGrammar = document.getElementsByClassName('grammar-highlight-selected')

          // Loop while there are elements in the collection
          while (selectedGrammar.length > 0) {
            selectedGrammar[0].classList.remove('grammar-highlight-selected')
          }

          // add selected class to all groups associated with this annotation
          const annotationId = target.getAttribute('data-pdf-annotate-id')
          // Instead of geting the order from the highlights, get the index directly from the comment using the annotation id
          const order = this.comments.findIndex(c => c.annotation.uuid == annotationId)
          if (this.comments[order]) {
            const errorType = this.comments[order].type
            const groups = document.querySelectorAll("g[data-pdf-annotate-id='" + annotationId + "']")
            groups.forEach(element => {
              if (errorType.toLowerCase() === 'vocabulary') { element.classList.add('vocabulary-highlight-selected') } else { element.classList.add('grammar-highlight-selected') }
            })
          }
          const commentCards = document.querySelectorAll('.comment-card')
          // Get the top position of the annotation
          var gTop = parseInt(parseInt(target.getAttribute('top')) * this.RENDER_OPTIONS.scale) - 34

          // Get the height of the svg
          const svgHeight = parseInt(target.getAttribute('page-height')) * this.RENDER_OPTIONS.scale + 12
          const svgPageNum = parseInt(target.getAttribute('page-num'))

          // Update the top position if there are more than 1 page
          if (svgPageNum > 1) { gTop += ((svgPageNum - 1) * svgHeight) }

          // If the comment associated with this annotation exist.
          if (order > -1 && this.comments[order] && commentCards[order]) {
            // Get the comment's top position
            const cTop = this.comments[order].topPosition

            // remove current selected comment card class if any
            const selected = document.getElementsByClassName('comment-card-selected')
            if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
            // Set this card as selected
            // console.log('this.order', this.order)
            commentCards[order].classList.add('comment-card-selected')
            // cmtSelected.classList.add('comment-card-selected')
            // this.updateCommentCardPosition(this.annotationClicked.getAttribute('data-pdf-annotate-id'))
            if (cTop != gTop) {
            // Move the comment up to gTop
              this.comments[order].topPosition = gTop
              // cmtSelected.setAttribute('top-position', gTop)
              // cmtSelected.style.top = gTop + 'px'

              const selected = document.getElementsByClassName('comment-card-selected')
              if (selected.length > 0) { selected[0].classList.remove('comment-card-selected') }
              commentCards[order].classList.add('comment-card-selected')

              // cmtSelected.classList.add('comment-card-selected')
              const endPos = gTop + commentCards[order].offsetHeight

              // this.updateCommentCardPosition(this.annotationClicked.getAttribute('data-pdf-annotate-id'))
              // const endPos = gTop + cmtSelected.offsetHeight
              if (cTop > gTop) {
              // Move up other uppen comments
                if (order > 0) { this.moveUpFromTopPos(commentCards, order - 1, gTop) }
                // Move up other lower comments
                if (order < commentCards.length - 1) { this.moveUpToEndPos(commentCards, order + 1, endPos) }
              } else if (cTop <= gTop) {
              // Move down other uppen comments
                if (order > 0) { this.moveDownToTopPos(commentCards, order - 1, gTop) }
                // Move down other lower comments
                if (order < commentCards.length - 1) { this.moveDownFromEndPos(commentCards, order + 1, endPos) }
              }
            } else {
              const endPos = gTop + commentCards[order].offsetHeight
              if (order < commentCards.length - 1) { this.moveUpToEndPos(commentCards, order + 1, endPos) }
            }
          }
        }
      }
    },
    async handleCommentPositionsRestore(e) {
      this.svg = document.getElementsByClassName('annotationLayer')[0]
      // re-order annotation
      if (this.svg) {
        [...this.svg.children]
        .sort(this.compareTopAttributes)
        .forEach(node => this.svg.appendChild(node))

        this.comments.sort(this.orderCommentByAnnotation)

        this.$nextTick(() => {
          const highlights = document.querySelectorAll("g[data-pdf-annotate-type='comment-highlight']")
          this.handleCommentAnnotationClick(highlights[0])
        })
      }
    },
    orderCommentByAnnotation(a, b) {
      if (a.annotation.top > b.annotation.top) return 1
      if (a.annotation.top < b.annotation.top) return -1
      if (a.annotation.top == b.annotation.top) {
        // compare left position
        if (a.annotation.left > b.annotation.left) return 1
        if (a.annotation.left < b.annotation.left) return -1
      }
      return 0
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
    titleCase(str) {
      var splitStr = str.toLowerCase().split(' ')
      for (var i = 0; i < splitStr.length; i++) {
          // You do not need to check if i is larger than splitStr length, as your for does that for you
          // Assign it back to the array
          splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1)
      }
      // Directly return the joined string
      return splitStr.join(' ')
    },
    getTextWidth(text) {
      const inputText = text
      const font = '12px sans-serif'

      const canvas = document.createElement('canvas')
      const context = canvas.getContext('2d')
      context.font = font
      return context.measureText(inputText).width
    },
    async render() {
      const self = this
      const pdf = await PDFJS.getDocument(this.docData).promise
      self.RENDER_OPTIONS.documentId = this.documentId
      self.RENDER_OPTIONS.pdfDocument = pdf
      self.viewer = document.getElementById('viewer')
      self.viewer.innerHTML = ''
      self.NUM_PAGES = pdf.numPages

      var docWidth = 0
      if (this.screenWidth > 780) { docWidth = document.getElementById('right-panel').offsetWidth - document.getElementById('comment-wrapper').offsetWidth } else docWidth = document.getElementById('document-wrapper').offsetWidth - document.getElementById('comment-wrapper').offsetWidth

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
        element.isSaved = true
      })

      // await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
      await this.comments.sort(this.compareTopAnnoAttributes)
      this.handleCommentPositionsRestore()
      this.hideDeleteToolBar()
      this.registerEvents()
      enableTextSelection(this)
      await this.reRenderPages()
    },
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
      if (target.type == 'comment-highlight' || target.type == 'point' || target.type == 'comment-area') {
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
    deleteComment(docId, comment) {
      this.handleCommentDelete(comment)
    },
    registerEvents() {
      const that = this
      const renderedPages = {}

      const commentCards = document.querySelectorAll('.comment-card')
      commentCards.forEach(item => {
        item.addEventListener('click', commentCardClick)
      })

      // window.removeEventListener('beforeunload', restoreCommentPositions)
      window.addEventListener('resize', this.calculateContainerHeight.bind(this))
      document.addEventListener('keyup', this.keyupHandler)
      document.getElementById('viewerContainer').addEventListener('scroll', handlePageScroll)

      addEventListener('annotation:click', annotationClick)
      addEventListener('annotation:delete', this.handleAnnotationDelete)
      addEventListener('comment:delete', this.deleteComment)
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
          that.$refs.toolBar?.handleAnnotationClicked(r)
        })
        that.handleCommentAnnotationClick(target)
      }
      // function annotationDelete(target) {
      //   that.handleAnnotationDelete(target)
      // }

      function commentCardClick(e) {
        if (e.target.getAttribute('button-id') != 'delete') {
          that.handleCommentCardClick(this)
        }
      }
      // async function restoreCommentPositions() {
      //   await that.handleCommentPositionsRestore()
      // }
    },
    unRegisterEvents() {
      const that = this
      const renderedPages = {}
      const commentCards = document.querySelectorAll('.comment-card')
      commentCards.forEach(item => {
        item.addEventListener('click', commentCardClick)
      })

      // window.removeEventListener('beforeunload', restoreCommentPositions)
      window.removeEventListener('resize', this.calculateContainerHeight.bind(this))
      document.removeEventListener('keyup', this.keyupHandler)

      document.getElementById('viewerContainer')?.removeEventListener('scroll', handlePageScroll)

      removeEventListener('annotation:click', annotationClick)
      removeEventListener('annotation:delete', this.handleAnnotationDelete)
      removeEventListener('comment:delete', this.deleteComment)
      removeEventListener('annotation:add', this.annotationAdded)
      removeEventListener('annotation:edit', this.annotationEdited)
      removeEventListener('annotation:insertNoteComment', this.insertNoteComment)
      removeEventListener('text:disable', this.disableToolbarButtons)
      removeEventListener('comment:updateCommentPositionAfterEditAnnotation', this.updateCommentPositionAfterEditAnnotation)

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
          that.$refs.toolBar?.handleAnnotationClicked(r)
        })
        that.handleCommentAnnotationClick(target)
      }
      // function annotationDelete(target) {
      //   that.handleAnnotationDelete(target)
      // }
      // function deleteComment(docId, comment) {
      //   that.handleCommentDelete(comment)
      // }
      function commentCardClick(e) {
        if (e.target.getAttribute('button-id') != 'delete') {
          that.handleCommentCardClick(this)
        }
      }
      // async function restoreCommentPositions() {
      //   await that.handleCommentPositionsRestore()
      // }
    },
    async TextStuff() {
      const self = this
      let textSize
      let textColor

      function initText() {
        // const size = document.querySelector('.toolbar .text-size');
        // [8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96].forEach(s => {
        //   size.appendChild(new Option(s, s))
        // })

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
          // document.querySelector('.toolbar .text-size').value = textSize
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
      if (combineArr.length > 2) {
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
        await this.handleCommentPositionsRestore()
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
        if (thisCard) {
          comment.topPosition = parseInt(thisCard.getAttribute('top-position'))
        }

        if (typeof (e) == 'undefined') {
          const commentTextContainer = document.getElementById('comment-text-' + comment.uuid)
          if (commentTextContainer) {
            if (commentTextContainer.clientHeight > (parseInt(window.getComputedStyle(commentTextContainer).lineHeight, 10) * 3)) {
              commentTextContainer.style.webkitLineClamp = '3'
              this.showMoreList.push({ id: comment.uuid, value: 0 })
            }
          }
        }
      })
      this.hideDeleteToolBar()
      return comments
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
      // this.comments.forEach(function(element) {
      //   element.isSelected = false
      // })
      const highlight = document.querySelector("[data-pdf-annotate-id='" + comment.uuid + "']")
      this.svg = document.querySelector(`svg[data-pdf-annotate-page='${comment.annotation.page}']`)

      comment.isSelected = true
      this.$forceUpdate()

      const textArea = document.getElementById('comment-input-' + comment.uuid)
      textArea.focus()
      this.inputHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2))
      const that = this
      textArea.addEventListener('keydown', function() {
        const commentCards = document.querySelectorAll('.comment-card')
        if (commentCards.length > 0) {
          setTimeout(function() {
            const newHeight = parseInt(textArea.style.height.substring(0, textArea.style.height.length - 2))
            const editCommentWrapper = document.querySelector(`[highlight-id='${comment.uuid}']`)
            const topPos = parseInt(editCommentWrapper.style.top.substring(0, editCommentWrapper.style.top.length - 2))
            const endPos = topPos + editCommentWrapper.offsetHeight
            if (that.inputHeight < newHeight) {
              if (that.order < commentCards.length - 1) { that.moveDownFromEndPos(commentCards, that.order + 1, endPos) }
            } else if (that.inputHeight > newHeight) {
              if (that.order < commentCards.length) { that.moveUpToEndPos(commentCards, that.order + 1, endPos) }
            }
            that.inputHeight = newHeight
          }, 100)
        }
      })

      const refName = 'comment' + comment.uuid
      this.$refs[refName][0].$el.childNodes[0].focus()
      this.$refs[refName][0].$el.childNodes[0].addEventListener('keyup', (e) => {
        this.updateCommentPositionWhileEditing(e)
      })
      this.target = highlight
      this.handleCommentAnnotationClick(highlight)
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
      if (comment.isSaved == false) {
        const uuid = comment.uuid
        // const commentWrapper = document.querySelector(".comment-card[highlight-id='" + uuid + "']")
        // const topPos = parseInt(commentWrapper.style.top.substring(0, commentWrapper.style.top.length - 2))

        this.comments.forEach(cmt => {
          if (cmt.uuid == uuid) {
            cmt.isSaved = true
            cmt.isSelected = false
          }
        })

        this.commentsNotSaved.splice(this.commentsNotSaved.indexOf(uuid), 1)
        // await this.comments.sort((a, b) => (a.topPosition >= b.topPosition) ? 1 : -1)
        await this.comments.sort(this.compareTopAnnoAttributes)
        this.$forceUpdate()
        // this.comments.sort(this.compareTopAnnoAttributes)
        let newComment
        this.comments.forEach(cmt => {
          if (cmt.uuid == uuid) {
            newComment = cmt
          }
        })
        newComment.annotation.documentId = this.documentId
        var anno = {
          DocumentId: this.documentId,
          ReviewId: this.reviewId,
          Type: newComment.annotation.type,
          PageNum: typeof (newComment.annotation.pageNum) != 'undefined' ? newComment.annotation.pageNum : newComment.annotation.page,
          Top: typeof (newComment.annotation.top) != 'undefined' ? newComment.annotation.top : parseInt(newComment.annotation.y),
          Color: newComment.annotation.color,
          Uuid: newComment.annotation.uuid,
          Data: JSON.stringify(newComment.annotation),
          Id: newComment.annotation.id
        }
        var newCmt = {
          Text: newComment.text != null ? newComment.text : '',
          Content: newComment.content,
          TopPosition: newComment.topPosition,
          Uuid: newComment.uuid,
          Data: JSON.stringify(newComment)
        }
        if (newComment.annotation.type == 'comment-area') {
          await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, uuid, newComment.annotation, undefined)
        }
        if (this.annotation.type == 'point') {
          this.$refs.toolBar?.handleAnnotationClicked(null)
        }
        this.disableToolbarButtons()
        reviewService.addInTextComment(this.documentId, this.reviewId, newCmt, anno).then(async rs => {
          if (rs.data) {
            this.updateCommentId(rs)
          }
        })
        const target = this.getHighlightByCommentId(newComment.annotation.uuid)
        if (this.newComment.replace(/\s/g, '').length != 0) {
          // Save in-progress comment
          await this.addCommentText(false)
        }
        this.handleCommentAnnotationClick(target)
        this.newComment = ''
      } else {
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

        this.comments.forEach(cmt => {
          if (cmt.uuid == comment.uuid) {
            cmt.isSelected = false
          }
        })
        this.$forceUpdate()

        const editCmt = document.querySelector('.comment-card-selected')
        if (editCmt) {
          const cmtAnno = document.querySelector(`[data-pdf-annotate-id="${editCmt.getAttribute('highlight-id')}"]`)
          if (cmtAnno) {
            const commentTextContainer = document.getElementById('comment-input-' + comment.uuid)
            if (commentTextContainer) {
              if (parseInt(window.getComputedStyle(commentTextContainer).height, 10) > (parseInt(window.getComputedStyle(commentTextContainer).lineHeight, 10) * 4)) {
                commentTextContainer.style.webkitLineClamp = '3'
                if (this.showMoreList.filter(item => item.id === comment.uuid) == 0) {
                  this.showMoreList.push({ id: comment.uuid, value: 0 })
                }
              } else {
                commentTextContainer.style.webkitLineClamp = 'inherit'
                this.showMoreList = this.showMoreList.filter(item => item.id !== editCmt.getAttribute('highlight-id'))
              }
            }

            await this.showMoreList.map(rs => { rs.value = 0 })
            this.hideDeleteToolBar()
            await this.handleCommentAnnotationClick(cmtAnno)
          }
        }

        reviewService.editComment(editComment).then(async rs => {
          if (rs) {
          // this.addToUndoList(rs.data, 'edit')
            this.$refs.toolBar?.setStatusText()
          }
        })
        this.isEditing = null
      }
    },
    async deleteCommentCard(uuid) {
      await PDFJSAnnotate.getStoreAdapter().deleteComment(this.documentId, uuid, true)
      this.comments = await PDFJSAnnotate.getStoreAdapter().getComments(this.documentId)
      this.comments.forEach(function(element) {
        element.isSelected = false
        element.isSaved = true
      })

      if (this.commentsNotSaved) {
        this.comments.forEach(cmt => {
          this.commentsNotSaved.forEach(el => {
            if (el == cmt.uuid) {
              cmt.isSaved = false
              cmt.isSelected = true
            }
          })
        })
      }
      this.$forceUpdate()

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
      this.$refs.textToolGroup?.HideTextToolGroup()
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
    async countAnnotations() {
      var count = 0
      for (let i = 1; i <= this.NUM_PAGES; i += 1) {
        await PDFJSAnnotate.getStoreAdapter().getAnnotations(this.documentId, i).then(rs => {
          console.log(rs)
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

      if (elContainer.style) {
        elContainer.style.height = containerHeight + 'px'
      }
      if (this.screenWidth > 780) {
        const rightPanel = document.getElementById('right-panel')
        const viewerContainer = document.getElementById('viewerContainer')
        viewerContainer.style.height = rightPanel.offsetHeight - document.getElementById('tool-bar').offsetHeight - 5 + 'px'

        // if (this.showQuestion) {
        //   rightPanel.style.width = window.innerWidth - document.getElementById('left-panel').offsetWidth - 17 + 'px'
        // } else {
        //   rightPanel.style.width = window.innerWidth - 10 + 'px'
        // }
      } else {
        // const documentWrapper = document.getElementById('document-wrapper')
        // console.log(documentWrapper.offsetHeight)
        // console.log(document.getElementById('tool-bar').offsetHeight)
        // const viewerContainer1 = document.getElementById('viewerContainer')
        // viewerContainer1.style.height = '800px'
        // viewerContainer1.style.height = documentWrapper.offsetHeight - document.getElementById('tool-bar').offsetHeight - 5 + 'px'
        // console.log(viewerContainer1.offsetHeight)
      }
    },
    calculateStylePaddingScroll() {
      this.$refs.tabQuestion?.calculateStylePaddingScroll()
    },
    setStatusText() {
      this.$refs.toolBar?.setStatusText()
    },

    async hideQuestion(e) {
      this.showQuestion = e
      await this.handleCommentPositionsRestore()
      this.calculateContainerHeight()
      this.hideDeleteToolBar()
      this.$refs.toolBar?.insertExpandMenu()
    },
    async documentWidthCal() {
      var container = document.getElementById('pageContainer1')
      if (container) {
        var docWidth = document.getElementById('pageContainer1').offsetWidth
        if (this.comments.length == 0) {
          if (this.screenWidth > 780) { docWidth = document.getElementById('right-panel').offsetWidth - 20 } else { docWidth = document.getElementById('document-wrapper').offsetWidth - 20 }

          // document.getElementById('comment-wrapper').style.display = 'none'
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

      this.$refs.toolBar?.insertExpandMenu()
    },

    async reRenderPages() {
      await UI.renderAllPages(this.NUM_PAGES, this.RENDER_OPTIONS)
      this.documentWidthCal()
      await this.handleCommentPositionsAfterHandleScale()
      this.isRendering = false
    },
    expandColorPickerToggle(e) {
      this.expandColorPicker = e
      this.$refs.toolBar?.expandColor(e)
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
          this.isAddingNewComment = false
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
            this.isAddingNewComment = false
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
      this.$confirm('Bình luận này sẽ bị xoá vĩnh viễn. Bạn chắc chứ?').then(() => {
        this.deleteCommentCard(e.uuid)
        this.removeElementById(e.annotation.uuid)
      }).catch(() => {
        if (e.class === 'Comment') {
          PDFJSAnnotate.getStoreAdapter().addAnnotation(e.annotation.documentId, e.annotation.page, e.annotation, true)
            .then((r) => {
              appendChild(this.svg, r)
              PDFJSAnnotate.getStoreAdapter().addComment(
                e.annotation.documentId,
                e.annotation,
                e.content,
                e.text,
                e.topPosition)
            })
        }
      })
    },
    keyupHandler(event) {
      if (event.ctrlKey && event.code === 'KeyZ' && this.undoHistory.length > 0) {
        this.undoAnnotation()
      } else if (event.ctrlKey && event.code === 'KeyY' && this.redoHistory.length > 0) {
        this.redo()
      }
    },
    async insertNoteComment(svg, note) {
      if (document.getElementById('comment-wrapper').offsetWidth == 0) {
        document.getElementById('comment-wrapper').style.display = 'block'
      }
      if (this.newComment.replace(/\s/g, '').length != 0) {
        // Save in-progress comment
        await this.addCommentText(false)
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
      let type = annotation.type
      if (annotation.type == 'area') {
        type = 'comment-area'
      }

      if (type == 'comment-area' || type == 'point') {
        const rect = this.getHighlightByCommentId(annotationId)
        rect.setAttribute('top', annotation.top)
        rect.setAttribute('left', annotation.left)
        let cmt
        this.comments.forEach(el => {
          if (el.uuid == annotationId) {
            cmt = el
          }
        })

        const cmtRect = document.querySelector(".comment-card[highlight-id='" + annotationId + "']")
        if (cmtRect) {
          if (cmt.isSaved) {
            cmtRect.setAttribute('top', annotation.top)
            cmtRect.setAttribute('left', annotation.left)

            cmts.forEach(cmt => {
              if (cmt.uuid == annotationId) {
                cmt.annotation = annotation
                editComment = cmt
              }
            })
            await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, annotationId, annotation, undefined)
            await PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)
            this.comments.forEach(cmt => {
              if (cmt.uuid == annotationId) {
                cmt.annotation = annotation
                editComment = cmt
              }
            })
            await this.comments.sort(this.compareTopAnnoAttributes)
            this.editCommentCard(editComment, 'undo')
          } else {
            cmtRect.setAttribute('top', annotation.top)
            cmtRect.setAttribute('left', annotation.left)

            cmts.forEach(cmt => {
              if (cmt.uuid == annotationId) {
                cmt.annotation = annotation
                editComment = cmt
              }
            })
            await PDFJSAnnotate.getStoreAdapter().updateComments(this.documentId, cmts)
            this.comments.forEach(cmt => {
              if (cmt.uuid == annotationId) {
                cmt.annotation = annotation
                editComment = cmt
              }
            })
            await this.comments.sort(this.compareTopAnnoAttributes)
          }
        }

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
      this.$refs.toolBar?.updateRedoList(this.redoHistory)
    },
    updateUndoList() {
      this.$refs.toolBar?.updateToolbarUndoList(this.undoHistory)
    },
    disableToolbarSubmit() {
      this.$refs.toolBar?.disableSubmit()
      document.getElementById('viewerContainer').style.userSelect = 'none'
      this.$refs.toolBar?.disableAnnotationCreate()
      // disableEdit()
      this.isView = true
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
    async updateTypeOfRect(typeAnno, status) {
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
      if (!status) {
        await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, uuid, this.annotation, 'added')
      } else {
        await PDFJSAnnotate.getStoreAdapter().editAnnotation(this.documentId, uuid, this.annotation, undefined)
      }
      // reviewService.editAnnotation(anno).then(rs => {
      // })
    },
    disableToolbarButtons() {
      this.activeButton = 'cursor'
      this.$refs.toolBar?.disableButtons()
    },
    rateReview() {
      this.selectedTab = 'rate'
    },
    onSubmitRevise() {
      this.$refs['formNote'].validate((valid) => {
        if (valid) {
          this.$refs.toolBar?.rejectTraining(this.form.noteRevision)
        }
      })
    },
    commentUserName() {
      if (this.loadedAnnotation && this.loadedAnnotation.reviewer) {
        return this.loadedAnnotation.reviewer.firstName + ' ' + this.loadedAnnotation.reviewer.lastName
      }
      return this.currentUser.firstName + ' ' + this.currentUser.lastName
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

.band-score{
  width: 85px;
  border: #478a9e solid 1px;
  padding: 2px 10px;
  border-radius: 5px;
  background: #d6e3e6;
  font-size: 15px;
}

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

.criteria-score{
  font-size: 15px;
  border-bottom: grey 1px dashed;
  margin-bottom: 10px;
  padding-bottom: 15px;
}

.criteria-score:last-of-type{
  border-bottom: none;
  margin-bottom: 0px;
  padding-bottom: 0px;
}

.sub-criteria {
  margin-top: 3px;
  display: inline-flex;
  width: 100%;
}

.criteria-score-check {
  color: rgb(68, 163, 21);
  margin-left: 2px;
  margin-right: 2px;
  font-size: 14px;
  font-weight: bold;
}

.criteria-score-close {
  right: 0px !important;
  color: rgb(227, 51, 51);
  margin-left: 1px;
  margin-right: 1px;
  font-size: 16px !important;
  font-weight: bold;
}

.sub-criteria-label {
  float: left;
  margin-left: 5px;
  margin-top: 2px;
}

.sub-score-tag {
  font-size: 13px;
  line-height: 23px;
}
</style>
<style>
/* .el-loading-mask {
  background-color: rgb(248 249 250) !important;
} */
.el-tabs__content{
  overflow: auto !important;
}
.comment-card >.el-card__header, .add-new-comment >.el-card__header {
  /* background: #b3d4f4; */
  padding: 0px !important;
}

@keyframes cursor-blink {
  0% {
    opacity: 0;
  }
}

#pdf-annotate-text-input::before {
  content: "";
  width: 2px;
  height: 100%;
  /* background: #787878; */
  display: inline-block;
  animation: cursor-blink 1.5s steps(2) infinite;
}

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
