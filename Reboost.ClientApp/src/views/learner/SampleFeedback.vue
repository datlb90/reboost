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
              v-if="task"
              ref="tabQuestion"
              :questionid="questionId"
            />
            <div
              v-if="!task"
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
              element-loading-text="Đang tìm lỗi trong bài viết và lựa chọn phương án sửa phù hợp"
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
              element-loading-text="Đang tìm lỗi trong bài viết và lựa chọn phương án sửa phù hợp"
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
// import docService from '@/services/document.service.js'
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
      questionId: 11936,
      documentId: 14390,
      reviewId: 16811,
      RENDER_OPTIONS: {
        documentId: null,
        pdfDocument: null,
        scale: 1,
        rotate: 0
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
      rateComment: '',
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
      screenWidth: window.innerWidth,
      isSelfReview: false,
      task: null,
      documentText: null,
      textNodes: [],
      docData: null,
      intextCommentCompleted: true,
      hasGrade: false,
      errors: null,
      loadCriteriaFeedbackCompleted: true,
      loadingReview: false,
      freeToken: this.$store.state.auth.user.freeToken,
      premiumToken: this.$store.state.auth.user.premiumToken,
      userSubscription: this.$store.state.auth.user.subscription,
      chartDescription: null,
      loadEssayScoreCompleted: true,
      loadFeedbackCompleted: true,
      loadErrorsCompleted: true,
      essayScore: {
          'reviewId': 16811,
          'overallBandScore': 6.5,
          'taskAchievementScore': null,
          'taskResponseScore': 6.5,
          'coherenceScore': 6,
          'lexicalResourceScore': 6.5,
          'grammarScore': 6,
          'fulfillRequirements': null,
          'highlightKeyFeatures': null,
          'compareAndContrast': null,
          'dataSelection': null,
          'addressingAllParts': null,
          'clarityOfPosition': 7,
          'developmentOfIdeas': 6.5,
          'justificationOfOpinion': 6.5,
          'appropriateWordCount': true,
          'logicalOrganization': 6.5,
          'paragraphing': 6,
          'cohesiveDevices': 6,
          'referencing': null,
          'rangeOfVocabulary': 6.5,
          'accuracyOfWordChoice': 6,
          'spellingAndFormation': 6,
          'registerAndStyle': 6.5,
          'grammarRange': 6,
          'sentenceComplexity': 6,
          'grammarAccuracy': 6,
          'createdDate': '2024-06-17T18:19:09.597',
          'updatedDate': '2024-06-17T18:19:09.597',
          'id': 343
      },
      review: {
          'id': 0,
          'reviewRequest': {
              'userId': '9230fa91-cf20-4f3f-b640-4c975dfa7ba4',
              'submissionId': 14390,
              'feedbackType': 'AI',
              'requestedDateTime': '2024-06-17T22:18:57.972854',
              'completedDateTime': '2024-06-17T22:19:29.180318',
              'status': 'Completed',
              'feedbackLanguage': 'vn',
              'specialRequest': null,
              'reviewType': 'deep',
              'submission': {
                  'userId': '9230fa91-cf20-4f3f-b640-4c975dfa7ba4',
                  'questionId': 11936,
                  'docId': 14390,
                  'type': 'Submission',
                  'submittedDate': '2024-06-17T22:18:56.335124',
                  'updatedDate': '2024-06-17T22:19:29.180316',
                  'timeSpentInSeconds': 0,
                  'status': 'Reviewed',
                  'document': null,
                  'question': null,
                  'reviewRequests': [],
                  'id': 14390
              },
              'id': 14060
          },
          'rater': null,
          'review': {
              'requestId': 14060,
              'reviewerId': 'AI',
              'revieweeId': '9230fa91-cf20-4f3f-b640-4c975dfa7ba4',
              'submissionId': 14390,
              'finalScore': 6.5,
              'status': 'Completed',
              'timeSpentInSeconds': 0,
              'lastActivityDate': '2024-06-17T22:19:29.181839',
              'reviewData': [],
              'id': 16811
          },
          'submission': {
              'userId': '9230fa91-cf20-4f3f-b640-4c975dfa7ba4',
              'questionId': 11936,
              'docId': 14390,
              'type': 'Submission',
              'submittedDate': '2024-06-17T22:18:56.335124',
              'updatedDate': '2024-06-17T22:19:29.180316',
              'timeSpentInSeconds': 0,
              'status': 'Reviewed',
              'document': null,
              'question': null,
              'reviewRequests': [
                  {
                      'userId': '9230fa91-cf20-4f3f-b640-4c975dfa7ba4',
                      'submissionId': 14390,
                      'feedbackType': 'AI',
                      'requestedDateTime': '2024-06-17T22:18:57.972854',
                      'completedDateTime': '2024-06-17T22:19:29.180318',
                      'status': 'Completed',
                      'feedbackLanguage': 'vn',
                      'specialRequest': null,
                      'reviewType': 'deep',
                      'id': 14060
                  }
              ],
              'id': 14390
          },
          'questionId': 0,
          'reviewId': 16811,
          'docId': 0,
          'status': null,
          'questionName': null,
          'test': null,
          'testSection': null,
          'questionType': null,
          'error': null,
          'rating': null,
          'reviewType': null
      },
      question: {
          'id': 11936,
          'title': 'IELTS Task 2 Topic',
          'section': 'Academic Writing Task 2',
          'taskId': 4,
          'test': 'IELTS',
          'testId': 2,
          'time': '40 minutes',
          'type': 'My Topic',
          'sample': false,
          'averageScore': '0.0',
          'submission': 0,
          'like': 0,
          'dislike': 0,
          'status': 'Personal',
          'difficulty': 'Undefined',
          'direction': 'You should spend about 40 minutes on this task.\n\nProvide reasons for your answer. Include relevant examples from your own knowledge or experience.\n\nWrite at least 250 words.',
          'userId': '9230fa91-cf20-4f3f-b640-4c975dfa7ba4',
          'createdBy': null,
          'testDate': null,
          'addedDate': '0001-01-01T00:00:00',
          'lastActivityDate': '0001-01-01T00:00:00',
          'questionsPart': [
              {
                  'questionId': 11936,
                  'name': 'Question',
                  'content': 'Social media has revolutionized the way we communicate, but it also has negative effects on society. Discuss the advantages and disadvantages of social media.\r\n'
              }
          ]
      },
      loadedAnnotation: {
          'annotations': [
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 390.501953125,
                  'rectangles': [
                      {
                          'y': 187,
                          'x': 389.501953125,
                          'width': 63.0234375,
                          'height': 18
                      }
                  ],
                  'top': 187,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'b292d820-cd40-4423-b893-4956c3d32f66',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60460
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 407.201171875,
                  'rectangles': [
                      {
                          'y': 205,
                          'x': 406.201171875,
                          'width': 60.375,
                          'height': 18
                      }
                  ],
                  'top': 205,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'e7f72555-9005-4623-bb7e-af9afba7fd88',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60461
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 497.23046875,
                  'rectangles': [
                      {
                          'y': 187,
                          'x': 496.23046875,
                          'width': 60.3515625,
                          'height': 18
                      }
                  ],
                  'top': 187,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'd856bbda-5848-43f6-8086-78323168c19b',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60462
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 328.474609375,
                  'rectangles': [
                      {
                          'y': 205,
                          'x': 327.474609375,
                          'width': 78.392578125,
                          'height': 18
                      }
                  ],
                  'top': 205,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '09819602-c7cb-4b5c-94da-2a4a91b3befe',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60463
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 191.728515625,
                  'rectangles': [
                      {
                          'y': 61,
                          'x': 190.728515625,
                          'width': 107.7421875,
                          'height': 18
                      }
                  ],
                  'top': 61,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '8b06aa88-8b72-41d5-a3dc-cfd37fd75150',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60464
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 481.896484375,
                  'rectangles': [
                      {
                          'y': 61,
                          'x': 480.896484375,
                          'width': 52.359375,
                          'height': 18
                      },
                      {
                          'y': 79,
                          'x': 54,
                          'width': 83.701171875,
                          'height': 18
                      }
                  ],
                  'top': 61,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '09e95524-dc8b-420c-91db-6940fd37bada',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60465
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 289.099609375,
                  'rectangles': [
                      {
                          'y': 223,
                          'x': 288.099609375,
                          'width': 85.716796875,
                          'height': 18
                      }
                  ],
                  'top': 223,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '546e494f-3b24-4dfb-98dd-abbe1d434f8b',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60466
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 101.01953125,
                  'rectangles': [
                      {
                          'y': 223,
                          'x': 100.01953125,
                          'width': 69.69140625,
                          'height': 18
                      }
                  ],
                  'top': 223,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '30131525-c6fd-4f99-9ef3-70d71ffdae7f',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60467
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 126.373046875,
                  'rectangles': [
                      {
                          'y': 241,
                          'x': 125.373046875,
                          'width': 43.69921875,
                          'height': 18
                      }
                  ],
                  'top': 241,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '5384c1b2-46f7-41e7-9b3d-5b8eb1219e8e',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60468
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 170.40625,
                  'rectangles': [
                      {
                          'y': 241,
                          'x': 169.40625,
                          'width': 61.6875,
                          'height': 18
                      }
                  ],
                  'top': 241,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '6fa6c643-c94f-4a3a-b3c2-300a315450c2',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60469
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 336.466796875,
                  'rectangles': [
                      {
                          'y': 79,
                          'x': 306.466796875,
                          'width': 89.03125,
                          'height': 18
                      }
                  ],
                  'top': 79,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '90638299-7302-433e-8836-37f1f87d6dc6',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60470
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 329.142578125,
                  'rectangles': [
                      {
                          'y': 277,
                          'x': 328.142578125,
                          'width': 77.70703125,
                          'height': 18
                      }
                  ],
                  'top': 277,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'b53ee876-503a-4e98-b8ff-bf096c6b6f82',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60473
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 291.8125,
                  'rectangles': [
                      {
                          'y': 295,
                          'x': 290.8125,
                          'width': 35.009765625,
                          'height': 18
                      }
                  ],
                  'top': 295,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'd11c02c4-ec49-4286-8540-c077d5931a55',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60474
              },

              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 327.15625,
                  'rectangles': [
                      {
                          'y': 295,
                          'x': 326.15625,
                          'width': 113.07421875,
                          'height': 18
                      }
                  ],
                  'top': 295,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'e86960f2-1885-47ef-92a4-97a3fe6c59a8',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60476
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 123.70703125,
                  'rectangles': [
                      {
                          'y': 313,
                          'x': 122.70703125,
                          'width': 81.708984375,
                          'height': 18
                      }
                  ],
                  'top': 313,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'e4a5e0f0-a79d-4a32-8b6c-e2945b01b086',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60477
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 291.80078125,
                  'rectangles': [
                      {
                          'y': 313,
                          'x': 290.80078125,
                          'width': 67.03125,
                          'height': 18
                      }
                  ],
                  'top': 313,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'c3b050d6-859d-4f8e-b4ed-a0d1376e420c',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60478
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 431.189453125,
                  'rectangles': [
                      {
                          'y': 313,
                          'x': 430.189453125,
                          'width': 85.72265625,
                          'height': 18
                      }
                  ],
                  'top': 313,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '1094ace4-b484-4f3c-b50a-f4545722e9f7',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60479
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 381.8359375,
                  'rectangles': [
                      {
                          'y': 61,
                          'x': 380.8359375,
                          'width': 79.7109375,
                          'height': 18
                      }
                  ],
                  'top': 61,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '3d503f65-4d22-4bf9-8085-622261d6fce8',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60480
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 517.24609375,
                  'rectangles': [
                      {
                          'y': 313,
                          'x': 516.24609375,
                          'width': 19.681640625,
                          'height': 18
                      },
                      {
                          'y': 331,
                          'x': 54,
                          'width': 41.689453125,
                          'height': 18
                      }
                  ],
                  'top': 313,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '589da4d0-aff6-4c6c-8174-75fefea45c6f',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60481
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 167.0546875,
                  'rectangles': [
                      {
                          'y': 331,
                          'x': 166.0546875,
                          'width': 55.013671875,
                          'height': 18
                      }
                  ],
                  'top': 331,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'ec5dcfb0-31f7-4309-9752-d42b6450e1c8',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60482
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 120.37890625,
                  'rectangles': [
                      {
                          'y': 349,
                          'x': 119.37890625,
                          'width': 52.37109375,
                          'height': 18
                      }
                  ],
                  'top': 349,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '5a3f5e1c-857e-4aa1-a56d-e0128851ace6',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60484
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 173.083984375,
                  'rectangles': [
                      {
                          'y': 349,
                          'x': 172.083984375,
                          'width': 9,
                          'height': 18
                      }
                  ],
                  'top': 349,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '3e32c1e7-2a1c-4219-9530-986b95b0c9c5',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60485
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 55,
                  'rectangles': [
                      {
                          'y': 115,
                          'x': 54,
                          'width': 66.375,
                          'height': 18
                      }
                  ],
                  'top': 115,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'd2a04799-9ba7-4aa4-a328-96f7256fc79b',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60486
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 397.896484375,
                  'rectangles': [
                      {
                          'y': 349,
                          'x': 396.896484375,
                          'width': 129.7265625,
                          'height': 18
                      },
                      {
                          'y': 367,
                          'x': 54,
                          'width': 41.68359375,
                          'height': 18
                      }
                  ],
                  'top': 349,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '04944c49-b729-41e2-be86-4e7c16d85c4a',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60487
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 277.1171875,
                  'rectangles': [
                      {
                          'y': 367,
                          'x': 276.1171875,
                          'width': 81.732421875,
                          'height': 18
                      }
                  ],
                  'top': 367,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '194d26fd-5853-4f1f-8889-615c8657a9bf',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60488
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 101.482421875,
                  'rectangles': [
                      {
                          'y': 385,
                          'x': 100.482421875,
                          'width': 45.005859375,
                          'height': 18
                      }
                  ],
                  'top': 385,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '11fffca0-81c7-44f9-b3ec-5114522af46b',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60489
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 410.93359375,
                  'rectangles': [
                      {
                          'y': 385,
                          'x': 409.93359375,
                          'width': 75.046875,
                          'height': 18
                      }
                  ],
                  'top': 385,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '5eb48b7b-fe04-4d5c-8ffd-5ab5302efaf2',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60490
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 376.31640625,
                  'rectangles': [
                      {
                          'y': 403,
                          'x': 375.31640625,
                          'width': 15.673828125,
                          'height': 18
                      }
                  ],
                  'top': 403,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'fba08ea9-db35-42ad-872e-40024d47c7a6',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60492
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 113.69921875,
                  'rectangles': [
                      {
                          'y': 421,
                          'x': 112.69921875,
                          'width': 57.029296875,
                          'height': 18
                      }
                  ],
                  'top': 421,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '298e4f86-2b84-457d-967e-3a1fccf28371',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60493
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 157.697265625,
                  'rectangles': [
                      {
                          'y': 439,
                          'x': 156.697265625,
                          'width': 59.0390625,
                          'height': 18
                      }
                  ],
                  'top': 439,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '1718992d-72d3-4719-ad18-122455d046b0',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60496
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 231.748046875,
                  'rectangles': [
                      {
                          'y': 475,
                          'x': 230.748046875,
                          'width': 58.34765625,
                          'height': 18
                      }
                  ],
                  'top': 475,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '0ef25493-be70-484f-b9aa-2741bb8a957b',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60497
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 418.48046875,
                  'rectangles': [
                      {
                          'y': 475,
                          'x': 417.48046875,
                          'width': 26.34375,
                          'height': 18
                      }
                  ],
                  'top': 475,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '5bd628d9-3891-4ae0-80ba-b425aa073403',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60498
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 465.173828125,
                  'rectangles': [
                      {
                          'y': 475,
                          'x': 464.173828125,
                          'width': 87.7265625,
                          'height': 18
                      },
                      {
                          'y': 493,
                          'x': 54,
                          'width': 74.361328125,
                          'height': 18
                      }
                  ],
                  'top': 475,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'd82ce5be-435d-41bf-8094-b467641f921d',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60499
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 239.1015625,
                  'rectangles': [
                      {
                          'y': 151,
                          'x': 238.1015625,
                          'width': 62.373046875,
                          'height': 18
                      }
                  ],
                  'top': 151,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'c98b9d22-754e-4dcf-a529-5ce30f114474',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60500
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 239.7578125,
                  'rectangles': [
                      {
                          'y': 493,
                          'x': 238.7578125,
                          'width': 103.06640625,
                          'height': 18
                      }
                  ],
                  'top': 493,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'e3748de7-f257-4baa-9a5b-fe2a5f476de5',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60501
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 385.84375,
                  'rectangles': [
                      {
                          'y': 151,
                          'x': 384.84375,
                          'width': 35.68359375,
                          'height': 18
                      }
                  ],
                  'top': 151,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '4ae56df5-038c-4f3e-b650-0fdc166ed31e',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60502
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 431.880859375,
                  'rectangles': [
                      {
                          'y': 493,
                          'x': 430.880859375,
                          'width': 73.04296875,
                          'height': 18
                      }
                  ],
                  'top': 493,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '4fa6ede4-11e9-4fa9-91ad-ee6f00907d34',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60503
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 146.40625,
                  'rectangles': [
                      {
                          'y': 169,
                          'x': 145.40625,
                          'width': 73.705078125,
                          'height': 18
                      }
                  ],
                  'top': 169,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '170a943f-82ca-4673-8063-879506302c0f',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60504
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 354.525390625,
                  'rectangles': [
                      {
                          'y': 169,
                          'x': 353.525390625,
                          'width': 103.728515625,
                          'height': 18
                      }
                  ],
                  'top': 169,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '4a4c6cd0-43cd-4268-a772-5f7556a903c1',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60505
              },
              {
                  'type': 'comment-highlight',
                  'color': 'red',
                  'left': 84.337890625,
                  'rectangles': [
                      {
                          'y': 187,
                          'x': 83.337890625,
                          'width': 72.369140625,
                          'height': 18
                      }
                  ],
                  'top': 187,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '93c8b377-82c1-4fce-bd5f-a36b0d153845',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60506
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 458.587890625,
                  'rectangles': [
                      {
                          'y': 169,
                          'x': 457.587890625,
                          'width': 73.69921875,
                          'height': 18
                      },
                      {
                          'y': 187,
                          'x': 54,
                          'width': 25.669921875,
                          'height': 18
                      }
                  ],
                  'top': 169,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': '7c0706ed-d164-4cff-8dbc-070d3674e773',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60507
              },
              {
                  'type': 'comment-highlight',
                  'color': 'blue',
                  'left': 447.033203125,
                  'rectangles': [
                      {
                          'y': 97,
                          'x': 446.033203125,
                          'width': 59.033203125,
                          'height': 18
                      }
                  ],
                  'top': 97,
                  'pageNum': 1,
                  'pageHeight': 792,
                  'class': 'Annotation',
                  'uuid': 'f828ad98-2635-45f8-99fe-ced5f9ad7937',
                  'page': 1,
                  'documentId': 14390,
                  'id': 60508
              }
          ],
          'comments': [
              {
                  'class': 'Comment',
                  'uuid': 'b292d820-cd40-4423-b893-4956c3d32f66',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 390.501953125,
                      'rectangles': [
                          {
                              'y': 187,
                              'x': 389.501953125,
                              'width': 63.0234375,
                              'height': 18
                          }
                      ],
                      'top': 187,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'b292d820-cd40-4423-b893-4956c3d32f66',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'necessarily' không cần thiết trong ngữ cảnh này và làm câu trở nên phức tạp không cần thiết.",
                  'text': 'necessarily',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'always',
                  'reason': "Từ 'necessarily' không cần thiết trong ngữ cảnh này và làm câu trở nên phức tạp không cần thiết.",
                  'explain': "Từ 'always' phù hợp hơn vì nó đơn giản và rõ ràng hơn trong ngữ cảnh này.",
                  'topPosition': 187,
                  'documentId': 14390,
                  'id': 59974
              },
              {
                  'class': 'Comment',
                  'uuid': 'e7f72555-9005-4623-bb7e-af9afba7fd88',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 407.201171875,
                      'rectangles': [
                          {
                              'y': 205,
                              'x': 406.201171875,
                              'width': 60.375,
                              'height': 18
                          }
                      ],
                      'top': 205,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'e7f72555-9005-4623-bb7e-af9afba7fd88',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Sử dụng mạo từ 'the' không cần thiết ở đây vì không có ngữ cảnh cụ thể nào đã được đề cập trước đó về tính năng này.",
                  'text': 'the feature',
                  'type': 'grammar',
                  'category': 'Article Misuse',
                  'fix': 'a feature',
                  'reason': "Sử dụng mạo từ 'the' không cần thiết ở đây vì không có ngữ cảnh cụ thể nào đã được đề cập trước đó về tính năng này.",
                  'explain': "Sử dụng mạo từ 'a' để chỉ một tính năng không xác định cụ thể, phù hợp với ngữ cảnh chung chung của câu.",
                  'topPosition': 205,
                  'documentId': 14390,
                  'id': 59975
              },
              {
                  'class': 'Comment',
                  'uuid': 'd856bbda-5848-43f6-8086-78323168c19b',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 497.23046875,
                      'rectangles': [
                          {
                              'y': 187,
                              'x': 496.23046875,
                              'width': 60.3515625,
                              'height': 18
                          }
                      ],
                      'top': 187,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'd856bbda-5848-43f6-8086-78323168c19b',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'waste time' không phù hợp vì nó mang nghĩa tiêu cực và không chính xác trong ngữ cảnh này.",
                  'text': 'waste time',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'spend time',
                  'reason': "Cụm từ 'waste time' không phù hợp vì nó mang nghĩa tiêu cực và không chính xác trong ngữ cảnh này.",
                  'explain': "Cụm từ 'spend time' phù hợp hơn vì nó mang nghĩa trung lập và chính xác hơn trong ngữ cảnh này.",
                  'topPosition': 187,
                  'documentId': 14390,
                  'id': 59973
              },
              {
                  'class': 'Comment',
                  'uuid': '8b06aa88-8b72-41d5-a3dc-cfd37fd75150',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 191.728515625,
                      'rectangles': [
                          {
                              'y': 61,
                              'x': 190.728515625,
                              'width': 107.7421875,
                              'height': 18
                          }
                      ],
                      'top': 61,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '8b06aa88-8b72-41d5-a3dc-cfd37fd75150',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ này không hoàn toàn chính xác trong ngữ cảnh này. 'Tendency' thường được dùng để chỉ một xu hướng hoặc thói quen cá nhân hơn là một hiện tượng xã hội rộng lớn.",
                  'text': 'the tendency to use',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'the increasing use of',
                  'reason': "Cụm từ này không hoàn toàn chính xác trong ngữ cảnh này. 'Tendency' thường được dùng để chỉ một xu hướng hoặc thói quen cá nhân hơn là một hiện tượng xã hội rộng lớn.",
                  'explain': "Cụm từ 'the increasing use of' chính xác hơn để mô tả sự gia tăng trong việc sử dụng mạng xã hội như một hiện tượng xã hội rộng lớn.",
                  'topPosition': 61,
                  'documentId': 14390,
                  'id': 59976
              },
              {
                  'class': 'Comment',
                  'uuid': '09819602-c7cb-4b5c-94da-2a4a91b3befe',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 328.474609375,
                      'rectangles': [
                          {
                              'y': 205,
                              'x': 327.474609375,
                              'width': 78.392578125,
                              'height': 18
                          }
                      ],
                      'top': 205,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '09819602-c7cb-4b5c-94da-2a4a91b3befe',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Trong câu này, 'Facebook' là chủ ngữ số ít, nhưng động từ 'has updated' không phù hợp vì nó không rõ ràng ai là người thực hiện hành động.",
                  'text': 'it has updated',
                  'type': 'grammar',
                  'category': 'Subject-verb Agreement',
                  'fix': 'Facebook has updated',
                  'reason': "Trong câu này, 'Facebook' là chủ ngữ số ít, nhưng động từ 'has updated' không phù hợp vì nó không rõ ràng ai là người thực hiện hành động.",
                  'explain': "Cần phải rõ ràng rằng 'Facebook' là chủ ngữ thực hiện hành động cập nhật, vì vậy cần sử dụng 'Facebook has updated' để đảm bảo sự rõ ràng và đúng ngữ pháp.",
                  'topPosition': 205,
                  'documentId': 14390,
                  'id': 59977
              },
              {
                  'class': 'Comment',
                  'uuid': '09e95524-dc8b-420c-91db-6940fd37bada',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 481.896484375,
                      'rectangles': [
                          {
                              'y': 61,
                              'x': 480.896484375,
                              'width': 52.359375,
                              'height': 18
                          },
                          {
                              'y': 79,
                              'x': 54,
                              'width': 83.701171875,
                              'height': 18
                          }
                      ],
                      'top': 61,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '09e95524-dc8b-420c-91db-6940fd37bada',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': 'Cụm từ này không sai nhưng có thể không phải là cách diễn đạt tự nhiên nhất trong ngữ cảnh này.',
                  'text': 'means of communication',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'ways we communicate',
                  'reason': 'Cụm từ này không sai nhưng có thể không phải là cách diễn đạt tự nhiên nhất trong ngữ cảnh này.',
                  'explain': "Cụm từ 'ways we communicate' nghe tự nhiên hơn và dễ hiểu hơn trong ngữ cảnh này.",
                  'topPosition': 61,
                  'documentId': 14390,
                  'id': 59982
              },
              {
                  'class': 'Comment',
                  'uuid': '5384c1b2-46f7-41e7-9b3d-5b8eb1219e8e',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 126.373046875,
                      'rectangles': [
                          {
                              'y': 241,
                              'x': 125.373046875,
                              'width': 43.69921875,
                              'height': 18
                          }
                      ],
                      'top': 241,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '5384c1b2-46f7-41e7-9b3d-5b8eb1219e8e',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'through' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ sự di chuyển qua một không gian vật lý hoặc một quá trình.",
                  'text': 'through',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'via',
                  'reason': "Từ 'through' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ sự di chuyển qua một không gian vật lý hoặc một quá trình.",
                  'explain': "Từ 'via' phù hợp hơn vì nó thường được dùng để chỉ phương tiện hoặc cách thức giao tiếp, đặc biệt là trong ngữ cảnh công nghệ và mạng xã hội.",
                  'topPosition': 241,
                  'documentId': 14390,
                  'id': 59978
              },
              {
                  'class': 'Comment',
                  'uuid': '546e494f-3b24-4dfb-98dd-abbe1d434f8b',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 289.099609375,
                      'rectangles': [
                          {
                              'y': 223,
                              'x': 288.099609375,
                              'width': 85.716796875,
                              'height': 18
                          }
                      ],
                      'top': 223,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '546e494f-3b24-4dfb-98dd-abbe1d434f8b',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'within a second' không chính xác vì nó không phản ánh đúng tốc độ thực tế của việc kết nối trên mạng xã hội.",
                  'text': 'within a second',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'instantly',
                  'reason': "Cụm từ 'within a second' không chính xác vì nó không phản ánh đúng tốc độ thực tế của việc kết nối trên mạng xã hội.",
                  'explain': "Sử dụng 'instantly' để diễn tả tốc độ nhanh chóng và phù hợp hơn với ngữ cảnh của việc kết nối trên mạng xã hội.",
                  'topPosition': 223,
                  'documentId': 14390,
                  'id': 59979
              },
              {
                  'class': 'Comment',
                  'uuid': '30131525-c6fd-4f99-9ef3-70d71ffdae7f',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 101.01953125,
                      'rectangles': [
                          {
                              'y': 223,
                              'x': 100.01953125,
                              'width': 69.69140625,
                              'height': 18
                          }
                      ],
                      'top': 223,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '30131525-c6fd-4f99-9ef3-70d71ffdae7f',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'network with' không phù hợp trong ngữ cảnh này vì nó thường được sử dụng trong các tình huống chuyên nghiệp hơn là mạng xã hội cá nhân.",
                  'text': 'network with',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'connect with',
                  'reason': "Cụm từ 'network with' không phù hợp trong ngữ cảnh này vì nó thường được sử dụng trong các tình huống chuyên nghiệp hơn là mạng xã hội cá nhân.",
                  'explain': "Sử dụng 'connect with' phù hợp hơn trong ngữ cảnh mạng xã hội cá nhân, nơi người dùng kết nối với bạn bè của họ.",
                  'topPosition': 223,
                  'documentId': 14390,
                  'id': 59980
              },
              {
                  'class': 'Comment',
                  'uuid': '6fa6c643-c94f-4a3a-b3c2-300a315450c2',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 170.40625,
                      'rectangles': [
                          {
                              'y': 241,
                              'x': 169.40625,
                              'width': 61.6875,
                              'height': 18
                          }
                      ],
                      'top': 241,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '6fa6c643-c94f-4a3a-b3c2-300a315450c2',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'social sites' không phải là cách diễn đạt phổ biến trong tiếng Anh. Thay vào đó, 'social media' là cụm từ thông dụng hơn.",
                  'text': 'social sites',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'social media',
                  'reason': "Cụm từ 'social sites' không phải là cách diễn đạt phổ biến trong tiếng Anh. Thay vào đó, 'social media' là cụm từ thông dụng hơn.",
                  'explain': "Cụm từ 'social media' là cách diễn đạt phổ biến và chính xác hơn để chỉ các nền tảng mạng xã hội.",
                  'topPosition': 241,
                  'documentId': 14390,
                  'id': 59981
              },
              {
                  'class': 'Comment',
                  'uuid': '90638299-7302-433e-8836-37f1f87d6dc6',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 336.466796875,
                      'rectangles': [
                          {
                              'y': 79,
                              'x': 335.466796875,
                              'width': 61.03125,
                              'height': 18
                          }
                      ],
                      'top': 79,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '90638299-7302-433e-8836-37f1f87d6dc6',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'more persuasive' không phù hợp trong ngữ cảnh này vì nó thường được dùng để miêu tả một người hoặc một lập luận có khả năng thuyết phục, chứ không phải là một lập luận mạnh mẽ hơn.",
                  'text': 'persuasive',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'stronger',
                  'reason': "Từ 'more persuasive' không phù hợp trong ngữ cảnh này vì nó thường được dùng để miêu tả một người hoặc một lập luận có khả năng thuyết phục, chứ không phải là một lập luận mạnh mẽ hơn.",
                  'explain': "Từ 'stronger' phù hợp hơn vì nó miêu tả một lập luận có sức mạnh hoặc tính thuyết phục cao hơn.",
                  'topPosition': 79,
                  'documentId': 14390,
                  'id': 59983
              },
              {
                  'class': 'Comment',
                  'uuid': 'b53ee876-503a-4e98-b8ff-bf096c6b6f82',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 329.142578125,
                      'rectangles': [
                          {
                              'y': 277,
                              'x': 328.142578125,
                              'width': 77.70703125,
                              'height': 18
                          }
                      ],
                      'top': 277,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'b53ee876-503a-4e98-b8ff-bf096c6b6f82',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'deteriorate for' không đúng ngữ pháp. Động từ 'deteriorate' không đi kèm với giới từ 'for'.",
                  'text': 'deteriorate for',
                  'type': 'grammar',
                  'category': 'Preposition Mistake',
                  'fix': 'deteriorate',
                  'reason': "Cụm từ 'deteriorate for' không đúng ngữ pháp. Động từ 'deteriorate' không đi kèm với giới từ 'for'.",
                  'explain': "Động từ 'deteriorate' không cần giới từ đi kèm trong ngữ cảnh này.",
                  'topPosition': 277,
                  'documentId': 14390,
                  'id': 59986
              },
              {
                  'class': 'Comment',
                  'uuid': 'd11c02c4-ec49-4286-8540-c077d5931a55',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 291.8125,
                      'rectangles': [
                          {
                              'y': 295,
                              'x': 290.8125,
                              'width': 35.009765625,
                              'height': 18
                          }
                      ],
                      'top': 295,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'd11c02c4-ec49-4286-8540-c077d5931a55',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'curtail' không phù hợp trong ngữ cảnh này vì nó thường mang nghĩa giảm bớt hoặc hạn chế một cái gì đó, thường là về số lượng hoặc mức độ.",
                  'text': 'curtail',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'reduce',
                  'reason': "Từ 'curtail' không phù hợp trong ngữ cảnh này vì nó thường mang nghĩa giảm bớt hoặc hạn chế một cái gì đó, thường là về số lượng hoặc mức độ.",
                  'explain': "Từ 'reduce' phù hợp hơn vì nó mang nghĩa giảm bớt một cách tổng quát, phù hợp với ngữ cảnh giảm nhu cầu gặp mặt trực tiếp.",
                  'topPosition': 295,
                  'documentId': 14390,
                  'id': 59987
              },

              {
                  'class': 'Comment',
                  'uuid': 'e86960f2-1885-47ef-92a4-97a3fe6c59a8',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 327.15625,
                      'rectangles': [
                          {
                              'y': 295,
                              'x': 326.15625,
                              'width': 113.07421875,
                              'height': 18
                          }
                      ],
                      'top': 295,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'e86960f2-1885-47ef-92a4-97a3fe6c59a8',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'the need for meeting' cần một giới từ khác để diễn đạt đúng ý.",
                  'text': 'the need for meeting',
                  'type': 'grammar',
                  'category': 'Preposition Mistake',
                  'fix': 'the need to meet',
                  'reason': "Cụm từ 'the need for meeting' cần một giới từ khác để diễn đạt đúng ý.",
                  'explain': "Sử dụng 'to meet' thay vì 'for meeting' để diễn đạt đúng cấu trúc ngữ pháp và ý nghĩa.",
                  'topPosition': 295,
                  'documentId': 14390,
                  'id': 59989
              },
              {
                  'class': 'Comment',
                  'uuid': 'e4a5e0f0-a79d-4a32-8b6c-e2945b01b086',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 123.70703125,
                      'rectangles': [
                          {
                              'y': 313,
                              'x': 122.70703125,
                              'width': 81.708984375,
                              'height': 18
                          }
                      ],
                      'top': 313,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'e4a5e0f0-a79d-4a32-8b6c-e2945b01b086',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'misunderstood' là dạng quá khứ phân từ, không phù hợp trong ngữ cảnh này.",
                  'text': 'misunderstood',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'misunderstanding',
                  'reason': "Từ 'misunderstood' là dạng quá khứ phân từ, không phù hợp trong ngữ cảnh này.",
                  'explain': "Sử dụng 'misunderstanding' để diễn đạt đúng ý nghĩa của việc thông tin và biểu cảm bị hiểu lầm.",
                  'topPosition': 313,
                  'documentId': 14390,
                  'id': 59990
              },
              {
                  'class': 'Comment',
                  'uuid': '1094ace4-b484-4f3c-b50a-f4545722e9f7',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 431.189453125,
                      'rectangles': [
                          {
                              'y': 313,
                              'x': 430.189453125,
                              'width': 85.72265625,
                              'height': 18
                          }
                      ],
                      'top': 313,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '1094ace4-b484-4f3c-b50a-f4545722e9f7',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'comprehending' không phù hợp trong ngữ cảnh này vì nó mang nghĩa hiểu biết sâu sắc, trong khi ở đây chỉ cần hiểu đơn giản.",
                  'text': 'comprehending',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'understanding',
                  'reason': "Từ 'comprehending' không phù hợp trong ngữ cảnh này vì nó mang nghĩa hiểu biết sâu sắc, trong khi ở đây chỉ cần hiểu đơn giản.",
                  'explain': "Từ 'understanding' phù hợp hơn vì nó mang nghĩa hiểu đơn giản, phù hợp với ngữ cảnh của câu.",
                  'topPosition': 313,
                  'documentId': 14390,
                  'id': 59991
              },
              {
                  'class': 'Comment',
                  'uuid': 'c3b050d6-859d-4f8e-b4ed-a0d1376e420c',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 291.80078125,
                      'rectangles': [
                          {
                              'y': 313,
                              'x': 290.80078125,
                              'width': 67.03125,
                              'height': 18
                          }
                      ],
                      'top': 313,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'c3b050d6-859d-4f8e-b4ed-a0d1376e420c',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'expressions' không phù hợp với ngữ cảnh của thông tin bị hiểu lầm.",
                  'text': 'expressions',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'communication',
                  'reason': "Từ 'expressions' không phù hợp với ngữ cảnh của thông tin bị hiểu lầm.",
                  'explain': "Sử dụng 'communication' để diễn đạt đúng ý nghĩa của việc thông tin và giao tiếp bị hiểu lầm.",
                  'topPosition': 313,
                  'documentId': 14390,
                  'id': 59992
              },
              {
                  'class': 'Comment',
                  'uuid': '3d503f65-4d22-4bf9-8085-622261d6fce8',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 381.8359375,
                      'rectangles': [
                          {
                              'y': 61,
                              'x': 380.8359375,
                              'width': 79.7109375,
                              'height': 18
                          }
                      ],
                      'top': 61,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '3d503f65-4d22-4bf9-8085-622261d6fce8',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ này có thể quá mạnh trong ngữ cảnh này. 'Revolutionizing' ngụ ý một sự thay đổi hoàn toàn và triệt để, trong khi sự thay đổi có thể không đến mức đó.",
                  'text': 'revolutionizing',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'transforming',
                  'reason': "Từ này có thể quá mạnh trong ngữ cảnh này. 'Revolutionizing' ngụ ý một sự thay đổi hoàn toàn và triệt để, trong khi sự thay đổi có thể không đến mức đó.",
                  'explain': "Từ 'transforming' phù hợp hơn vì nó mô tả một sự thay đổi đáng kể nhưng không nhất thiết phải hoàn toàn triệt để.",
                  'topPosition': 61,
                  'documentId': 14390,
                  'id': 59993
              },
              {
                  'class': 'Comment',
                  'uuid': '589da4d0-aff6-4c6c-8174-75fefea45c6f',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 517.24609375,
                      'rectangles': [
                          {
                              'y': 313,
                              'x': 516.24609375,
                              'width': 19.681640625,
                              'height': 18
                          },
                          {
                              'y': 331,
                              'x': 54,
                              'width': 41.689453125,
                              'height': 18
                          }
                      ],
                      'top': 313,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '589da4d0-aff6-4c6c-8174-75fefea45c6f',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Sử dụng mạo từ 'the' không cần thiết vì 'context' ở đây không chỉ đến một ngữ cảnh cụ thể nào.",
                  'text': 'the context',
                  'type': 'grammar',
                  'category': 'Article Misuse',
                  'fix': 'context',
                  'reason': "Sử dụng mạo từ 'the' không cần thiết vì 'context' ở đây không chỉ đến một ngữ cảnh cụ thể nào.",
                  'explain': "Bỏ mạo từ 'the' để câu trở nên tự nhiên hơn và không chỉ đến một ngữ cảnh cụ thể.",
                  'topPosition': 313,
                  'documentId': 14390,
                  'id': 59994
              },
              {
                  'class': 'Comment',
                  'uuid': 'ec5dcfb0-31f7-4309-9752-d42b6450e1c8',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 167.0546875,
                      'rectangles': [
                          {
                              'y': 331,
                              'x': 166.0546875,
                              'width': 55.013671875,
                              'height': 18
                          }
                      ],
                      'top': 331,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'ec5dcfb0-31f7-4309-9752-d42b6450e1c8',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'may arise' không sai nhưng có thể làm câu trở nên không rõ ràng về thời gian xảy ra.",
                  'text': 'may arise',
                  'type': 'grammar',
                  'category': 'Tense Error',
                  'fix': 'can arise',
                  'reason': "Cụm từ 'may arise' không sai nhưng có thể làm câu trở nên không rõ ràng về thời gian xảy ra.",
                  'explain': "Sử dụng 'can arise' để làm rõ khả năng xảy ra của xung đột trong bất kỳ thời điểm nào.",
                  'topPosition': 331,
                  'documentId': 14390,
                  'id': 59995
              },
              {
                  'class': 'Comment',
                  'uuid': '3e32c1e7-2a1c-4219-9530-986b95b0c9c5',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 173.083984375,
                      'rectangles': [
                          {
                              'y': 349,
                              'x': 172.083984375,
                              'width': 9,
                              'height': 18
                          }
                      ],
                      'top': 349,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '3e32c1e7-2a1c-4219-9530-986b95b0c9c5',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'if' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ điều kiện, trong khi ngữ cảnh này cần một từ chỉ mục đích.",
                  'text': 'if',
                  'type': 'grammar',
                  'category': 'Preposition Mistake',
                  'fix': 'when',
                  'reason': "Từ 'if' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ điều kiện, trong khi ngữ cảnh này cần một từ chỉ mục đích.",
                  'explain': "Từ 'when' phù hợp hơn vì nó chỉ thời điểm hoặc tình huống mà hành động xảy ra, phù hợp với ngữ cảnh nói về việc tìm kiếm đối tác.",
                  'topPosition': 349,
                  'documentId': 14390,
                  'id': 59997
              },
              {
                  'class': 'Comment',
                  'uuid': '5a3f5e1c-857e-4aa1-a56d-e0128851ace6',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 120.37890625,
                      'rectangles': [
                          {
                              'y': 349,
                              'x': 119.37890625,
                              'width': 52.37109375,
                              'height': 18
                          }
                      ],
                      'top': 349,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '5a3f5e1c-857e-4aa1-a56d-e0128851ace6',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'the youth' không hoàn toàn chính xác trong ngữ cảnh này vì nó thường được dùng để chỉ một nhóm người trẻ tuổi nói chung, không phải là một nhóm cụ thể.",
                  'text': 'the youth',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'young people',
                  'reason': "Cụm từ 'the youth' không hoàn toàn chính xác trong ngữ cảnh này vì nó thường được dùng để chỉ một nhóm người trẻ tuổi nói chung, không phải là một nhóm cụ thể.",
                  'explain': "Cụm từ 'young people' phù hợp hơn vì nó chỉ rõ nhóm người trẻ tuổi mà không mang tính chất tổng quát như 'the youth'.",
                  'topPosition': 349,
                  'documentId': 14390,
                  'id': 59998
              },
              {
                  'class': 'Comment',
                  'uuid': 'd2a04799-9ba7-4aa4-a328-96f7256fc79b',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 55,
                      'rectangles': [
                          {
                              'y': 115,
                              'x': 54,
                              'width': 66.375,
                              'height': 18
                          }
                      ],
                      'top': 115,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'd2a04799-9ba7-4aa4-a328-96f7256fc79b',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'being social' không phải là cách diễn đạt tự nhiên trong tiếng Anh để miêu tả việc tham gia vào các hoạt động xã hội.",
                  'text': 'being social',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'engaging in social activities',
                  'reason': "Cụm từ 'being social' không phải là cách diễn đạt tự nhiên trong tiếng Anh để miêu tả việc tham gia vào các hoạt động xã hội.",
                  'explain': "Cụm từ 'engaging in social activities' phù hợp hơn vì nó miêu tả rõ ràng và tự nhiên hơn việc tham gia vào các hoạt động xã hội.",
                  'topPosition': 115,
                  'documentId': 14390,
                  'id': 59999
              },
              {
                  'class': 'Comment',
                  'uuid': '04944c49-b729-41e2-be86-4e7c16d85c4a',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 397.896484375,
                      'rectangles': [
                          {
                              'y': 349,
                              'x': 396.896484375,
                              'width': 129.7265625,
                              'height': 18
                          },
                          {
                              'y': 367,
                              'x': 54,
                              'width': 41.68359375,
                              'height': 18
                          }
                      ],
                      'top': 349,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '04944c49-b729-41e2-be86-4e7c16d85c4a',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ này sử dụng sai dạng động từ. 'Receive' cần được chuyển thành dạng danh động từ (gerund) để phù hợp với cấu trúc câu.",
                  'text': 'Except for the passively receive',
                  'type': 'grammar',
                  'category': 'Verb Form Error',
                  'fix': 'Except for passively receiving',
                  'reason': "Cụm từ này sử dụng sai dạng động từ. 'Receive' cần được chuyển thành dạng danh động từ (gerund) để phù hợp với cấu trúc câu.",
                  'explain': "Dạng danh động từ 'receiving' phù hợp hơn vì nó diễn tả hành động nhận thông điệp một cách thụ động.",
                  'topPosition': 349,
                  'documentId': 14390,
                  'id': 60000
              },
              {
                  'class': 'Comment',
                  'uuid': '194d26fd-5853-4f1f-8889-615c8657a9bf',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 277.1171875,
                      'rectangles': [
                          {
                              'y': 367,
                              'x': 276.1171875,
                              'width': 81.732421875,
                              'height': 18
                          }
                      ],
                      'top': 367,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '194d26fd-5853-4f1f-8889-615c8657a9bf',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Động từ 'evaluate' không cần giới từ 'about' đi kèm.",
                  'text': 'evaluate about',
                  'type': 'grammar',
                  'category': 'Preposition Mistake',
                  'fix': 'evaluate',
                  'reason': "Động từ 'evaluate' không cần giới từ 'about' đi kèm.",
                  'explain': "Động từ 'evaluate' tự nó đã đủ nghĩa mà không cần thêm giới từ 'about'.",
                  'topPosition': 367,
                  'documentId': 14390,
                  'id': 60001
              },
              {
                  'class': 'Comment',
                  'uuid': '11fffca0-81c7-44f9-b3ec-5114522af46b',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 101.482421875,
                      'rectangles': [
                          {
                              'y': 385,
                              'x': 100.482421875,
                              'width': 45.005859375,
                              'height': 18
                          }
                      ],
                      'top': 385,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '11fffca0-81c7-44f9-b3ec-5114522af46b',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'criteria' là dạng số nhiều của 'criterion' và không cần thêm 's'.",
                  'text': 'criterias',
                  'type': 'vocabulary',
                  'category': 'Spelling Mistake',
                  'fix': 'criteria',
                  'reason': "Từ 'criteria' là dạng số nhiều của 'criterion' và không cần thêm 's'.",
                  'explain': "Từ 'criteria' đã là dạng số nhiều, nên không cần thêm 's'.",
                  'topPosition': 385,
                  'documentId': 14390,
                  'id': 60002
              },
              {
                  'class': 'Comment',
                  'uuid': '5eb48b7b-fe04-4d5c-8ffd-5ab5302efaf2',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 410.93359375,
                      'rectangles': [
                          {
                              'y': 385,
                              'x': 409.93359375,
                              'width': 75.046875,
                              'height': 18
                          }
                      ],
                      'top': 385,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '5eb48b7b-fe04-4d5c-8ffd-5ab5302efaf2',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'dominance of' không phù hợp trong ngữ cảnh này vì nó mang nghĩa quá mạnh và không tự nhiên khi nói về việc sử dụng mạng xã hội.",
                  'text': 'dominance of',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'prevalence of',
                  'reason': "Cụm từ 'dominance of' không phù hợp trong ngữ cảnh này vì nó mang nghĩa quá mạnh và không tự nhiên khi nói về việc sử dụng mạng xã hội.",
                  'explain': "Cụm từ 'prevalence of' phù hợp hơn vì nó diễn tả sự phổ biến và thường xuyên xuất hiện của mạng xã hội một cách tự nhiên và chính xác hơn.",
                  'topPosition': 385,
                  'documentId': 14390,
                  'id': 60003
              },
              {
                  'class': 'Comment',
                  'uuid': 'fba08ea9-db35-42ad-872e-40024d47c7a6',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 376.31640625,
                      'rectangles': [
                          {
                              'y': 403,
                              'x': 375.31640625,
                              'width': 15.673828125,
                              'height': 18
                          }
                      ],
                      'top': 403,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'fba08ea9-db35-42ad-872e-40024d47c7a6',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'so' không cần thiết vì câu đã có từ 'because' để chỉ lý do.",
                  'text': 'so',
                  'type': 'grammar',
                  'category': 'Conjunction Misuse',
                  'fix': '',
                  'reason': "Từ 'so' không cần thiết vì câu đã có từ 'because' để chỉ lý do.",
                  'explain': "Việc loại bỏ từ 'so' sẽ làm câu gọn gàng hơn và tránh sự lặp lại không cần thiết của các từ nối.",
                  'topPosition': 403,
                  'documentId': 14390,
                  'id': 60005
              },
              {
                  'class': 'Comment',
                  'uuid': '298e4f86-2b84-457d-967e-3a1fccf28371',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 113.69921875,
                      'rectangles': [
                          {
                              'y': 421,
                              'x': 112.69921875,
                              'width': 57.029296875,
                              'height': 18
                          }
                      ],
                      'top': 421,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '298e4f86-2b84-457d-967e-3a1fccf28371',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'in order to' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ mục đích, trong khi ở đây cần một cụm từ chỉ sự khó khăn.",
                  'text': 'in order to',
                  'type': 'grammar',
                  'category': 'Preposition Mistake',
                  'fix': 'to',
                  'reason': "Cụm từ 'in order to' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ mục đích, trong khi ở đây cần một cụm từ chỉ sự khó khăn.",
                  'explain': "Sử dụng 'to' sẽ làm câu rõ ràng hơn và phù hợp với ngữ cảnh của việc ngăn chặn hiện tượng này.",
                  'topPosition': 421,
                  'documentId': 14390,
                  'id': 60006
              },
              {
                  'class': 'Comment',
                  'uuid': '1718992d-72d3-4719-ad18-122455d046b0',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 157.697265625,
                      'rectangles': [
                          {
                              'y': 439,
                              'x': 156.697265625,
                              'width': 59.0390625,
                              'height': 18
                          }
                      ],
                      'top': 439,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '1718992d-72d3-4719-ad18-122455d046b0',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'apparently' không phù hợp trong ngữ cảnh này vì nó mang nghĩa 'có vẻ như' hoặc 'rõ ràng là', không phù hợp với ý nghĩa của câu.",
                  'text': 'apparently',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'inevitably',
                  'reason': "Từ 'apparently' không phù hợp trong ngữ cảnh này vì nó mang nghĩa 'có vẻ như' hoặc 'rõ ràng là', không phù hợp với ý nghĩa của câu.",
                  'explain': "Từ 'inevitably' phù hợp hơn vì nó mang nghĩa 'không thể tránh khỏi', phù hợp với ý nghĩa của câu rằng các mối quan tâm sẽ trở nên phổ biến hơn.",
                  'topPosition': 439,
                  'documentId': 14390,
                  'id': 60009
              },
              {
                  'class': 'Comment',
                  'uuid': '0ef25493-be70-484f-b9aa-2741bb8a957b',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 231.748046875,
                      'rectangles': [
                          {
                              'y': 475,
                              'x': 230.748046875,
                              'width': 58.34765625,
                              'height': 18
                          }
                      ],
                      'top': 475,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '0ef25493-be70-484f-b9aa-2741bb8a957b',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'affirmative' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ sự đồng ý hoặc khẳng định, không phải để mô tả tác động tích cực.",
                  'text': 'affirmative',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'positive',
                  'reason': "Từ 'affirmative' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ sự đồng ý hoặc khẳng định, không phải để mô tả tác động tích cực.",
                  'explain': "Từ 'positive' phù hợp hơn để mô tả các tác động tích cực của mạng xã hội.",
                  'topPosition': 475,
                  'documentId': 14390,
                  'id': 60010
              },
              {
                  'class': 'Comment',
                  'uuid': '5bd628d9-3891-4ae0-80ba-b425aa073403',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 418.48046875,
                      'rectangles': [
                          {
                              'y': 475,
                              'x': 417.48046875,
                              'width': 26.34375,
                              'height': 18
                          }
                      ],
                      'top': 475,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '5bd628d9-3891-4ae0-80ba-b425aa073403',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'alter' không phù hợp trong ngữ cảnh này vì nó thường có nghĩa là thay đổi một chút, trong khi ở đây cần một từ mạnh hơn để chỉ sự thay đổi hoàn toàn cách giao tiếp.",
                  'text': 'alter',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'transform',
                  'reason': "Từ 'alter' không phù hợp trong ngữ cảnh này vì nó thường có nghĩa là thay đổi một chút, trong khi ở đây cần một từ mạnh hơn để chỉ sự thay đổi hoàn toàn cách giao tiếp.",
                  'explain': "Từ 'transform' phù hợp hơn để chỉ sự thay đổi hoàn toàn cách giao tiếp do mạng xã hội mang lại.",
                  'topPosition': 475,
                  'documentId': 14390,
                  'id': 60011
              },
              {
                  'class': 'Comment',
                  'uuid': 'd82ce5be-435d-41bf-8094-b467641f921d',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 465.173828125,
                      'rectangles': [
                          {
                              'y': 475,
                              'x': 464.173828125,
                              'width': 87.7265625,
                              'height': 18
                          },
                          {
                              'y': 493,
                              'x': 54,
                              'width': 74.361328125,
                              'height': 18
                          }
                      ],
                      'top': 475,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'd82ce5be-435d-41bf-8094-b467641f921d',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'outdated way to communicate' không tự nhiên trong tiếng Anh. Thường thì người ta sẽ nói 'outdated methods of communication'.",
                  'text': 'outdated way to communicate',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'outdated methods of communication',
                  'reason': "Cụm từ 'outdated way to communicate' không tự nhiên trong tiếng Anh. Thường thì người ta sẽ nói 'outdated methods of communication'.",
                  'explain': "Cụm từ 'outdated methods of communication' tự nhiên hơn và đúng ngữ pháp hơn trong tiếng Anh.",
                  'topPosition': 475,
                  'documentId': 14390,
                  'id': 60012
              },
              {
                  'class': 'Comment',
                  'uuid': 'c98b9d22-754e-4dcf-a529-5ce30f114474',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 239.1015625,
                      'rectangles': [
                          {
                              'y': 151,
                              'x': 238.1015625,
                              'width': 62.373046875,
                              'height': 18
                          }
                      ],
                      'top': 151,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'c98b9d22-754e-4dcf-a529-5ce30f114474',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': 'Thì hiện tại hoàn thành không phù hợp vì không có mốc thời gian cụ thể và hành động này vẫn đang tiếp diễn.',
                  'text': 'have made',
                  'type': 'grammar',
                  'category': 'Tense Error',
                  'fix': 'make',
                  'reason': 'Thì hiện tại hoàn thành không phù hợp vì không có mốc thời gian cụ thể và hành động này vẫn đang tiếp diễn.',
                  'explain': "Thì hiện tại đơn 'make' phù hợp hơn vì nó diễn tả một sự thật chung hoặc một hành động thường xuyên xảy ra.",
                  'topPosition': 151,
                  'documentId': 14390,
                  'id': 60013
              },
              {
                  'class': 'Comment',
                  'uuid': 'e3748de7-f257-4baa-9a5b-fe2a5f476de5',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 239.7578125,
                      'rectangles': [
                          {
                              'y': 493,
                              'x': 238.7578125,
                              'width': 103.06640625,
                              'height': 18
                          }
                      ],
                      'top': 493,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'e3748de7-f257-4baa-9a5b-fe2a5f476de5',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'disadvantages of it' không tự nhiên và không cần thiết phải dùng 'it' ở đây.",
                  'text': 'disadvantages of it',
                  'type': 'grammar',
                  'category': 'Article Misuse',
                  'fix': 'its disadvantages',
                  'reason': "Cụm từ 'disadvantages of it' không tự nhiên và không cần thiết phải dùng 'it' ở đây.",
                  'explain': "Cụm từ 'its disadvantages' ngắn gọn và tự nhiên hơn trong ngữ cảnh này.",
                  'topPosition': 493,
                  'documentId': 14390,
                  'id': 60014
              },
              {
                  'class': 'Comment',
                  'uuid': '4fa6ede4-11e9-4fa9-91ad-ee6f00907d34',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 431.880859375,
                      'rectangles': [
                          {
                              'y': 493,
                              'x': 430.880859375,
                              'width': 73.04296875,
                              'height': 18
                          }
                      ],
                      'top': 493,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '4fa6ede4-11e9-4fa9-91ad-ee6f00907d34',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'ones' không cần thiết và làm câu trở nên rườm rà.",
                  'text': 'positive ones',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'positives',
                  'reason': "Từ 'ones' không cần thiết và làm câu trở nên rườm rà.",
                  'explain': "Từ 'positives' ngắn gọn và rõ ràng hơn, phù hợp với ngữ cảnh.",
                  'topPosition': 493,
                  'documentId': 14390,
                  'id': 60015
              },
              {
                  'class': 'Comment',
                  'uuid': '4ae56df5-038c-4f3e-b650-0fdc166ed31e',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 385.84375,
                      'rectangles': [
                          {
                              'y': 151,
                              'x': 384.84375,
                              'width': 35.68359375,
                              'height': 18
                          }
                      ],
                      'top': 151,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '4ae56df5-038c-4f3e-b650-0fdc166ed31e',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'easier' không đủ mạnh để diễn tả mức độ cải thiện trong việc giao tiếp qua các nền tảng ảo.",
                  'text': 'easier',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'much easier',
                  'reason': "Từ 'easier' không đủ mạnh để diễn tả mức độ cải thiện trong việc giao tiếp qua các nền tảng ảo.",
                  'explain': "Cụm từ 'much easier' nhấn mạnh hơn mức độ cải thiện, làm cho câu văn rõ ràng và thuyết phục hơn.",
                  'topPosition': 151,
                  'documentId': 14390,
                  'id': 60016
              },
              {
                  'class': 'Comment',
                  'uuid': '170a943f-82ca-4673-8063-879506302c0f',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 146.40625,
                      'rectangles': [
                          {
                              'y': 169,
                              'x': 145.40625,
                              'width': 73.705078125,
                              'height': 18
                          }
                      ],
                      'top': 169,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '170a943f-82ca-4673-8063-879506302c0f',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'at these sites' không sai nhưng có thể gây hiểu nhầm. 'used by these applications' sẽ rõ ràng khi nhắc tới các công nghệ hiện đại được sử dụng.",
                  'text': 'at these sites',
                  'type': 'grammar',
                  'category': 'Preposition Mistake',
                  'fix': 'used by these applications',
                  'reason': "Cụm từ 'at these sites' không sai nhưng có thể gây hiểu nhầm. 'In these locations' sẽ rõ ràng hơn trong việc chỉ ra các địa điểm cụ thể.",
                  'explain': "Sử dụng 'used by these applications' sẽ rõ ràng hơn và phù hợp hơn trong ngữ cảnh này",
                  'topPosition': 169,
                  'documentId': 14390,
                  'id': 60017
              },
              {
                  'class': 'Comment',
                  'uuid': '4a4c6cd0-43cd-4268-a772-5f7556a903c1',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 354.525390625,
                      'rectangles': [
                          {
                              'y': 169,
                              'x': 353.525390625,
                              'width': 103.728515625,
                              'height': 18
                          }
                      ],
                      'top': 169,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '4a4c6cd0-43cd-4268-a772-5f7556a903c1',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'make a connection' không hoàn toàn chính xác trong ngữ cảnh này. 'Establish a connection' là cụm từ thường được sử dụng hơn khi nói về việc tạo ra một kết nối.",
                  'text': 'make a connection',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'establish a connection',
                  'reason': "Cụm từ 'make a connection' không hoàn toàn chính xác trong ngữ cảnh này. 'Establish a connection' là cụm từ thường được sử dụng hơn khi nói về việc tạo ra một kết nối.",
                  'explain': "Sử dụng 'establish a connection' sẽ chính xác hơn và phù hợp hơn trong ngữ cảnh này, vì nó là cụm từ thường được sử dụng khi nói về việc tạo ra một kết nối.",
                  'topPosition': 169,
                  'documentId': 14390,
                  'id': 60018
              },
              {
                  'class': 'Comment',
                  'uuid': '93c8b377-82c1-4fce-bd5f-a36b0d153845',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'red',
                      'left': 84.337890625,
                      'rectangles': [
                          {
                              'y': 187,
                              'x': 83.337890625,
                              'width': 72.369140625,
                              'height': 18
                          }
                      ],
                      'top': 187,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '93c8b377-82c1-4fce-bd5f-a36b0d153845',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'Compared to' không phù hợp trong ngữ cảnh này vì nó thường được sử dụng để so sánh hai đối tượng cụ thể, trong khi đoạn văn này đang nói về phương pháp truyền thống và phương pháp hiện đại một cách chung chung.",
                  'text': 'Compared to',
                  'type': 'grammar',
                  'category': 'Preposition Mistake',
                  'fix': 'In comparison with',
                  'reason': "Cụm từ 'Compared to' không phù hợp trong ngữ cảnh này vì nó thường được sử dụng để so sánh hai đối tượng cụ thể, trong khi đoạn văn này đang nói về phương pháp truyền thống và phương pháp hiện đại một cách chung chung.",
                  'explain': "Cụm từ 'In comparison with' phù hợp hơn vì nó so sánh hai phương pháp một cách tổng quát và không cụ thể hóa đối tượng so sánh.",
                  'topPosition': 187,
                  'documentId': 14390,
                  'id': 60019
              },
              {
                  'class': 'Comment',
                  'uuid': '7c0706ed-d164-4cff-8dbc-070d3674e773',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 458.587890625,
                      'rectangles': [
                          {
                              'y': 169,
                              'x': 457.587890625,
                              'width': 73.69921875,
                              'height': 18
                          },
                          {
                              'y': 187,
                              'x': 54,
                              'width': 25.669921875,
                              'height': 18
                          }
                      ],
                      'top': 169,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': '7c0706ed-d164-4cff-8dbc-070d3674e773',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Cụm từ 'within a short time' không sai nhưng 'in a short period of time' sẽ tự nhiên hơn và thường được sử dụng hơn trong văn viết học thuật.",
                  'text': 'within a short time',
                  'type': 'vocabulary',
                  'category': 'Collocation Error',
                  'fix': 'in a short period of time',
                  'reason': "Cụm từ 'within a short time' không sai nhưng 'in a short period of time' sẽ tự nhiên hơn và thường được sử dụng hơn trong văn viết học thuật.",
                  'explain': "Sử dụng 'in a short period of time' sẽ tự nhiên hơn và phù hợp hơn trong ngữ cảnh này, vì nó là cụm từ thường được sử dụng trong văn viết học thuật.",
                  'topPosition': 169,
                  'documentId': 14390,
                  'id': 60020
              },
              {
                  'class': 'Comment',
                  'uuid': 'f828ad98-2635-45f8-99fe-ced5f9ad7937',
                  'annotation': {
                      'type': 'comment-highlight',
                      'color': 'blue',
                      'left': 447.033203125,
                      'rectangles': [
                          {
                              'y': 97,
                              'x': 446.033203125,
                              'width': 59.033203125,
                              'height': 18
                          }
                      ],
                      'top': 97,
                      'pageNum': 1,
                      'pageHeight': 792,
                      'class': 'Annotation',
                      'uuid': 'f828ad98-2635-45f8-99fe-ced5f9ad7937',
                      'page': 1,
                      'documentId': 14390
                  },
                  'comment': "Từ 'individuals' không phù hợp trong ngữ cảnh này vì nó quá chung chung và không rõ ràng.",
                  'text': 'individuals',
                  'type': 'vocabulary',
                  'category': 'Word Choice Error',
                  'fix': 'people',
                  'reason': "Từ 'individuals' không phù hợp trong ngữ cảnh này vì nó quá chung chung và không rõ ràng.",
                  'explain': "Từ 'people' phù hợp hơn vì nó đơn giản và dễ hiểu hơn trong ngữ cảnh này.",
                  'topPosition': 97,
                  'documentId': 14390,
                  'id': 60021
              }
          ],
          'reviewer': null
      },
      rubricCriteria: [
          {
              'criteriaId': 7,
              'name': 'Task Response',
              'mark': 6.5,
              'comment': '1. Điểm mạnh:\n- Bài luận đã nêu rõ được cả hai mặt lợi và hại của việc sử dụng mạng xã hội, đáp ứng yêu cầu của đề bài.\n- Tác giả đã đưa ra các ví dụ cụ thể như Facebook và hẹn hò trực tuyến để minh họa cho các luận điểm của mình, giúp bài viết trở nên thuyết phục hơn.\n\n2. Điểm cần cải thiện:\n- Bài luận chưa trả lời đầy đủ mọi khía cạnh của đề bài. Cần bổ sung thêm các khía cạnh khác của mạng xã hội như tác động đến sức khỏe tâm lý, sự riêng tư và bảo mật thông tin cá nhân.\n- Quan điểm của tác giả chưa được trình bày một cách rõ ràng và nhất quán. Cần có một câu chủ đề rõ ràng ở phần mở bài để định hướng cho toàn bộ bài viết.\n- Các ý tưởng chưa được mở rộng đủ để hỗ trợ các luận điểm chính. Ví dụ, khi nói về việc mạng xã hội làm giảm chất lượng giao tiếp, cần phân tích sâu hơn về cách thức và lý do cụ thể.\n- Các ý tưởng cần được hỗ trợ bởi lý lẽ logic, bằng chứng hoặc ví dụ cụ thể hơn. Ví dụ, khi nói về việc mạng xã hội giúp kết nối nhanh chóng, có thể bổ sung thêm số liệu thống kê hoặc nghiên cứu để minh chứng.\n\nGợi ý cải thiện:\n- Bổ sung thêm các khía cạnh khác của mạng xã hội như tác động đến sức khỏe tâm lý, sự riêng tư và bảo mật thông tin cá nhân.\n- Đưa ra một câu chủ đề rõ ràng ở phần mở bài để định hướng cho toàn bộ bài viết.\n- Mở rộng và phát triển các ý tưởng bằng cách phân tích sâu hơn và cung cấp thêm lý lẽ logic, bằng chứng hoặc ví dụ cụ thể.\n- Khi nói về các tác động tiêu cực như hiểu lầm thông tin và bắt nạt trực tuyến, cần cung cấp thêm số liệu hoặc nghiên cứu để minh chứng cho luận điểm của mình.'
          },
          {
              'criteriaId': 8,
              'name': 'Coherence & Cohesion',
              'mark': 6,
              'comment': '1. Điểm mạnh:\n- Bài luận có cấu trúc rõ ràng với phần mở bài, thân bài và kết luận. Người viết đã nêu rõ ràng luận điểm chính trong phần mở bài và kết luận.\n- Các đoạn văn trong thân bài được phân chia hợp lý, mỗi đoạn tập trung vào một khía cạnh cụ thể của vấn đề (lợi ích và tác hại của mạng xã hội).\n- Người viết đã sử dụng một số từ nối và cụm từ liên kết như "On the one hand", "For example", "As a result", "Nevertheless", "Furthermore", "To sum up" để kết nối các ý tưởng và đoạn văn.\n\n2. Điểm cần cải thiện:\n- Cấu trúc của bài luận có thể được cải thiện để dòng chảy logic mạch lạc hơn. Ví dụ, đoạn văn thứ hai và thứ ba có thể được sắp xếp lại để làm rõ hơn sự đối lập giữa lợi ích và tác hại của mạng xã hội. Đoạn văn thứ hai nên tập trung hoàn toàn vào lợi ích, trong khi đoạn văn thứ ba nên tập trung vào tác hại.\n- Một số đoạn văn thiếu sự tập trung và câu chủ đề rõ ràng. Ví dụ, đoạn văn thứ ba bắt đầu bằng việc nói về chất lượng cuộc trò chuyện nhưng sau đó lại chuyển sang vấn đề bắt nạt trên mạng. Để cải thiện, đoạn văn này nên có một câu chủ đề rõ ràng và tập trung vào một ý tưởng chính. Ví dụ: "Một trong những tác hại lớn của mạng xã hội là làm giảm chất lượng cuộc trò chuyện."\n- Một số thiết bị liên kết được sử dụng không thích hợp hoặc thiếu tự nhiên. Ví dụ, câu "As a consequence, more concerns will apparently become universal." có thể được thay thế bằng "Consequently, this issue is becoming increasingly widespread." để tự nhiên hơn. Cụm từ "Due to the lack of genuine interaction" có thể được thay thế bằng "Because of the lack of face-to-face interaction" để rõ ràng hơn.\n\nTóm lại, bài luận cần cải thiện về sự tập trung của các đoạn văn và sử dụng các thiết bị liên kết một cách tự nhiên hơn để đạt được tiêu chí Coherence and Cohesion tốt hơn.'
          },
          {
              'criteriaId': 9,
              'name': 'Lexical Resource',
              'mark': 6.5,
              'comment': '1. Điểm mạnh:\n- Bài luận sử dụng một số từ vựng phong phú và đa dạng, chẳng hạn như "revolutionizing," "cutting-edge technology," "deteriorate," "genuine interaction," "misunderstood information," "cyber bullying," và "dominance."\n- Tác giả đã cố gắng sử dụng các cụm từ phức tạp và cấu trúc câu đa dạng để diễn đạt ý tưởng, điều này cho thấy sự nỗ lực trong việc nâng cao vốn từ vựng.\n\n2. Điểm cần cải thiện:\n- Một số từ và cụm từ được sử dụng quá mức hoặc lặp lại không cần thiết. Ví dụ, từ "individuals" xuất hiện nhiều lần trong bài luận. Có thể thay thế bằng các từ đồng nghĩa như "people," "persons," hoặc "users" để tránh lặp lại.\n- Một số từ và cụm từ không chính xác hoặc không phù hợp. Ví dụ:\n  - "the tendency to use social media" có thể thay bằng "the prevalence of social media use" để chính xác hơn.\n  - "technological advancement" nên thay bằng "social media" để phù hợp với ngữ cảnh của bài luận.\n  - "curtail the need for meeting in person" có thể thay bằng "reduce the necessity of face-to-face interactions" để rõ ràng hơn.\n  - "passively receive the messages" nên thay bằng "passively receiving messages" để đúng ngữ pháp.\n  - "evaluate about the background of family and different criterias" nên thay bằng "evaluate the family background and various criteria" để chính xác hơn.\n- Ngôn ngữ trong bài luận cần trang trọng hơn. Ví dụ:\n  - "having a conversation through social sites" có thể thay bằng "engaging in communication through social media platforms."\n  - "more concerns will apparently become universal" nên thay bằng "more concerns are likely to become widespread."\n\nTóm lại, bài luận cần cải thiện việc sử dụng từ vựng để tránh lặp lại, sử dụng từ chính xác và phù hợp hơn, và duy trì ngôn ngữ trang trọng để đáp ứng tiêu chí Lexical Resource của bài luận học thuật.'
          },
          {
              'criteriaId': 10,
              'name': 'Grammatical Range & Accuracy',
              'mark': 6,
              'comment': '1. Điểm mạnh:\n- Bài luận sử dụng một số cấu trúc ngữ pháp phức tạp, chẳng hạn như câu điều kiện và câu ghép, giúp thể hiện khả năng sử dụng ngữ pháp đa dạng.\n- Các câu trong bài luận phần lớn được viết đúng ngữ pháp và dễ hiểu, không có lỗi ngữ pháp nghiêm trọng làm ảnh hưởng đến ý nghĩa của câu.\n\n2. Điểm cần cải thiện:\n- Bài luận có xu hướng sử dụng cấu trúc câu đơn giản và lặp lại, cần đa dạng hóa hơn nữa các cấu trúc câu để tăng tính phong phú và hấp dẫn. Ví dụ, thay vì viết "The cutting-edge technology at these sites provides opportunities to make a connection within a short time," có thể viết "Thanks to the cutting-edge technology at these sites, individuals are now provided with opportunities to make connections within a short time."\n- Một số câu có thể được viết lại dưới dạng câu phức để tăng cường sự rõ ràng và hấp dẫn. Ví dụ, câu "Due to the lack of genuine interaction, it can curtail the need for meeting in person, which may increase the misunderstood information and expressions" có thể viết lại thành "Because of the lack of genuine interaction, the need for meeting in person is curtailed, which in turn may lead to increased misunderstandings and misinterpretations."\n- Có một số lỗi ngữ pháp nhỏ cần sửa chữa:\n  - "Compared to the traditional methods, individuals may not necessarily need to waste time waiting for a response." Câu này nên sửa thành "Compared to traditional methods, individuals may not necessarily need to waste time waiting for a response."\n  - "Except for the passively receive the messages, it is challenging to evaluate about the background of family and different criterias." Câu này nên sửa thành "Except for passively receiving the messages, it is challenging to evaluate the background of the family and different criteria."\n  - "Because these acts can be done anonymously, so it is challenging for the authorities in order to prevent this phenomenon from being controlled." Câu này nên sửa thành "Because these acts can be done anonymously, it is challenging for the authorities to prevent this phenomenon from being controlled."\n\nNhìn chung, bài luận cần đa dạng hóa cấu trúc câu và chú ý sửa các lỗi ngữ pháp nhỏ để đạt điểm cao hơn trong tiêu chí Grammatical Range & Accuracy.'
          },
          {
              'criteriaId': 36,
              'name': 'Critical Errors',
              'mark': 0,
              'comment': "1. 'the tendency to use' --> 'the increasing use of'\n- Giải thích: Cụm từ này không hoàn toàn chính xác trong ngữ cảnh này. 'Tendency' thường được dùng để chỉ một xu hướng hoặc thói quen cá nhân hơn là một hiện tượng xã hội rộng lớn.\n\n2. 'revolutionizing' --> 'transforming'\n- Giải thích: Từ này có thể quá mạnh trong ngữ cảnh này. 'Revolutionizing' ngụ ý một sự thay đổi hoàn toàn và triệt để, trong khi sự thay đổi có thể không đến mức đó.\n\n3. 'means of communication' --> 'ways we communicate'\n- Giải thích: Cụm từ này không sai nhưng có thể không phải là cách diễn đạt tự nhiên nhất trong ngữ cảnh này.\n\n4. 'persuasive' --> 'stronger'\n- Giải thích: Từ 'persuasive' không phù hợp trong ngữ cảnh này vì nó thường được dùng để miêu tả một người hoặc một lập luận có khả năng thuyết phục, chứ không phải là một lập luận mạnh mẽ hơn.\n\n5. 'discourages' --> 'reduces'\n- Giải thích: Từ 'discourages' không hoàn toàn chính xác trong ngữ cảnh này vì nó mang nghĩa là làm nản lòng hoặc ngăn cản, trong khi ý muốn diễn đạt là công nghệ làm giảm sự tương tác xã hội.\n\n6. 'being social' --> 'engaging in social activities'\n- Giải thích: Cụm từ 'being social' không phải là cách diễn đạt tự nhiên trong tiếng Anh để miêu tả việc tham gia vào các hoạt động xã hội.\n\n7. 'On the one hand' --> 'Firstly'\n- Giải thích: Cụm từ này thường được sử dụng khi so sánh hai ý kiến đối lập, nhưng đoạn văn này không cung cấp ý kiến đối lập.\n\n8. 'have made' --> 'make'\n- Giải thích: Thì hiện tại hoàn thành không phù hợp vì không có mốc thời gian cụ thể và hành động này vẫn đang tiếp diễn.\n\n9. 'easier' --> 'much easier'\n- Giải thích: Từ 'easier' không đủ mạnh để diễn tả mức độ cải thiện trong việc giao tiếp qua các nền tảng ảo.\n\n10. 'cutting-edge' --> 'advanced'\n- Giải thích: Từ 'cutting-edge' thường được sử dụng để mô tả công nghệ tiên tiến, nhưng trong ngữ cảnh này, từ 'advanced' sẽ phù hợp hơn vì nó bao hàm ý nghĩa về sự phát triển và tiến bộ.\n\n11. 'at these sites' --> 'in these locations'\n- Giải thích: Cụm từ 'at these sites' không sai nhưng có thể gây hiểu nhầm. 'In these locations' sẽ rõ ràng hơn trong việc chỉ ra các địa điểm cụ thể.\n\n12. 'make a connection' --> 'establish a connection'\n- Giải thích: Cụm từ 'make a connection' không hoàn toàn chính xác trong ngữ cảnh này. 'Establish a connection' là cụm từ thường được sử dụng hơn khi nói về việc tạo ra một kết nối.\n\n13. 'within a short time' --> 'in a short period of time'\n- Giải thích: Cụm từ 'within a short time' không sai nhưng 'in a short period of time' sẽ tự nhiên hơn và thường được sử dụng hơn trong văn viết học thuật.\n\n14. 'Compared to' --> 'In comparison with'\n- Giải thích: Cụm từ 'Compared to' không phù hợp trong ngữ cảnh này vì nó thường được sử dụng để so sánh hai đối tượng cụ thể, trong khi đoạn văn này đang nói về phương pháp truyền thống và phương pháp hiện đại một cách chung chung.\n\n15. 'individuals' --> 'people'\n- Giải thích: Từ 'individuals' không phù hợp trong ngữ cảnh này vì nó quá chung chung và không rõ ràng.\n\n16. 'necessarily' --> 'always'\n- Giải thích: Từ 'necessarily' không cần thiết trong ngữ cảnh này và làm câu trở nên phức tạp không cần thiết.\n\n17. 'waste time' --> 'spend time'\n- Giải thích: Cụm từ 'waste time' không phù hợp vì nó mang nghĩa tiêu cực và không chính xác trong ngữ cảnh này.\n\n18. 'it has updated' --> 'Facebook has updated'\n- Giải thích: Trong câu này, 'Facebook' là chủ ngữ số ít, nhưng động từ 'has updated' không phù hợp vì nó không rõ ràng ai là người thực hiện hành động.\n\n19. 'the feature' --> 'a feature'\n- Giải thích: Sử dụng mạo từ 'the' không cần thiết ở đây vì không có ngữ cảnh cụ thể nào đã được đề cập trước đó về tính năng này.\n\n20. 'network with' --> 'connect with'\n- Giải thích: Cụm từ 'network with' không phù hợp trong ngữ cảnh này vì nó thường được sử dụng trong các tình huống chuyên nghiệp hơn là mạng xã hội cá nhân.\n\n21. 'within a second' --> 'instantly'\n- Giải thích: Cụm từ 'within a second' không chính xác vì nó không phản ánh đúng tốc độ thực tế của việc kết nối trên mạng xã hội.\n\n22. 'through' --> 'via'\n- Giải thích: Từ 'through' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ sự di chuyển qua một không gian vật lý hoặc một quá trình.\n\n23. 'social sites' --> 'social media'\n- Giải thích: Cụm từ 'social sites' không phải là cách diễn đạt phổ biến trong tiếng Anh. Thay vào đó, 'social media' là cụm từ thông dụng hơn.\n\n24. 'it' --> 'technology'\n- Giải thích: Từ 'it' không rõ ràng và không phù hợp trong ngữ cảnh này.\n\n25. 'deteriorate for' --> 'deteriorate'\n- Giải thích: Cụm từ 'deteriorate for' không đúng ngữ pháp. Động từ 'deteriorate' không đi kèm với giới từ 'for'.\n\n27. 'curtail' --> 'reduce'\n- Giải thích: Từ 'curtail' không phù hợp trong ngữ cảnh này vì nó thường mang nghĩa giảm bớt hoặc hạn chế một cái gì đó, thường là về số lượng hoặc mức độ.\n\n28. 'the need for meeting' --> 'the need to meet'\n- Giải thích: Cụm từ 'the need for meeting' cần một giới từ khác để diễn đạt đúng ý.\n\n29. 'misunderstood' --> 'misunderstanding'\n- Giải thích: Từ 'misunderstood' là dạng quá khứ phân từ, không phù hợp trong ngữ cảnh này.\n\n30. 'expressions' --> 'communication'\n- Giải thích: Từ 'expressions' không phù hợp với ngữ cảnh của thông tin bị hiểu lầm.\n\n31. 'comprehending' --> 'understanding'\n- Giải thích: Từ 'comprehending' không phù hợp trong ngữ cảnh này vì nó mang nghĩa hiểu biết sâu sắc, trong khi ở đây chỉ cần hiểu đơn giản.\n\n32. 'the context' --> 'context'\n- Giải thích: Sử dụng mạo từ 'the' không cần thiết vì 'context' ở đây không chỉ đến một ngữ cảnh cụ thể nào.\n\n33. 'may arise' --> 'can arise'\n- Giải thích: Cụm từ 'may arise' không sai nhưng có thể làm câu trở nên không rõ ràng về thời gian xảy ra.\n\n34. 'tendency' --> 'trend'\n- Giải thích: Từ 'tendency' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ xu hướng hoặc khuynh hướng chung, không phải là hành động cụ thể.\n\n35. 'the youth' --> 'young people'\n- Giải thích: Cụm từ 'the youth' không hoàn toàn chính xác trong ngữ cảnh này vì nó thường được dùng để chỉ một nhóm người trẻ tuổi nói chung, không phải là một nhóm cụ thể.\n\n36. 'if' --> 'when'\n- Giải thích: Từ 'if' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ điều kiện, trong khi ngữ cảnh này cần một từ chỉ mục đích.\n\n37. 'Except for the passively receive' --> 'Except for passively receiving'\n- Giải thích: Cụm từ này sử dụng sai dạng động từ. 'Receive' cần được chuyển thành dạng danh động từ (gerund) để phù hợp với cấu trúc câu.\n\n38. 'evaluate about' --> 'evaluate'\n- Giải thích: Động từ 'evaluate' không cần giới từ 'about' đi kèm.\n\n39. 'criterias' --> 'criteria'\n- Giải thích: Từ 'criteria' là dạng số nhiều của 'criterion' và không cần thêm 's'.\n\n40. 'dominance of' --> 'prevalence of'\n- Giải thích: Cụm từ 'dominance of' không phù hợp trong ngữ cảnh này vì nó mang nghĩa quá mạnh và không tự nhiên khi nói về việc sử dụng mạng xã hội.\n\n42. 'so' --> ''\n- Giải thích: Từ 'so' không cần thiết vì câu đã có từ 'because' để chỉ lý do.\n\n43. 'in order to' --> 'to'\n- Giải thích: Cụm từ 'in order to' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ mục đích, trong khi ở đây cần một cụm từ chỉ sự khó khăn.\n\n45. 'apparently' --> 'inevitably'\n- Giải thích: Từ 'apparently' không phù hợp trong ngữ cảnh này vì nó mang nghĩa 'có vẻ như' hoặc 'rõ ràng là', không phù hợp với ý nghĩa của câu.\n\n46. 'affirmative' --> 'positive'\n- Giải thích: Từ 'affirmative' không phù hợp trong ngữ cảnh này vì nó thường được dùng để chỉ sự đồng ý hoặc khẳng định, không phải để mô tả tác động tích cực.\n\n47. 'alter' --> 'transform'\n- Giải thích: Từ 'alter' không phù hợp trong ngữ cảnh này vì nó thường có nghĩa là thay đổi một chút, trong khi ở đây cần một từ mạnh hơn để chỉ sự thay đổi hoàn toàn cách giao tiếp.\n\n48. 'outdated way to communicate' --> 'outdated methods of communication'\n- Giải thích: Cụm từ 'outdated way to communicate' không tự nhiên trong tiếng Anh. Thường thì người ta sẽ nói 'outdated methods of communication'.\n\n49. 'disadvantages of it' --> 'its disadvantages'\n- Giải thích: Cụm từ 'disadvantages of it' không tự nhiên và không cần thiết phải dùng 'it' ở đây.\n\n50. 'positive ones' --> 'positives'\n- Giải thích: Từ 'ones' không cần thiết và làm câu trở nên rườm rà.\n\n"
          },
          {
              'criteriaId': 43,
              'name': 'Arguments Assessment',
              'mark': 0,
              'comment': "Đoạn văn 1:\n- Ý chính: Social media has made communicating easier and faster.\n- Lập luận: Lập luận này mạnh vì nó chỉ ra rằng công nghệ tiên tiến giúp kết nối nhanh chóng và dễ dàng hơn so với các phương pháp truyền thống. Tuy nhiên, lập luận này có thể yếu nếu không có ví dụ cụ thể hoặc số liệu thống kê để minh chứng.\n- Gợi ý: Để củng cố lập luận, có thể thêm ví dụ cụ thể về các ứng dụng hoặc nền tảng mạng xã hội khác nhau và cách chúng cải thiện giao tiếp, cũng như số liệu thống kê về thời gian phản hồi trung bình trên các nền tảng này.\n- Bản cải thiện: One of the primary advantages of social media is that it has made communication easier and faster. Advanced technology on these platforms allows users to connect within seconds, eliminating the long wait times associated with traditional methods. For instance, Facebook's instant messaging feature enables users to network with friends almost instantaneously. Additionally, platforms like WhatsApp and Instagram have similar features that facilitate quick and efficient communication. As a result, social media significantly broadens the range of connections and enhances the speed of interactions.\n\nĐoạn văn 2:\n- Ý chính: The widespread use of social media can deteriorate the quality of conversations and lead to misunderstandings.\n- Lập luận: Lập luận này mạnh vì nó chỉ ra rằng thiếu tương tác trực tiếp có thể dẫn đến hiểu lầm và xung đột. Tuy nhiên, lập luận này có thể yếu nếu không có ví dụ cụ thể hoặc nghiên cứu để minh chứng.\n- Gợi ý: Để củng cố lập luận, có thể thêm ví dụ cụ thể về các tình huống hiểu lầm trên mạng xã hội và số liệu thống kê về tỷ lệ xung đột hoặc hiểu lầm do giao tiếp qua mạng xã hội.\n- Bản cải thiện: Despite the convenience, the widespread use of social media can deteriorate the quality of conversations and lead to misunderstandings. The lack of face-to-face interaction often results in misinterpretations of messages and expressions, which can escalate conflicts. For example, online dating platforms frequently lead to misunderstandings because users cannot fully gauge each other's backgrounds and intentions through text alone. Furthermore, cyberbullying is a significant issue that arises from the anonymity provided by social media, making it difficult for authorities to control and prevent such behavior. Consequently, the negative impacts on communication quality and the potential for conflict are substantial."
          },
          {
              'criteriaId': 45,
              'name': 'Vocabulary',
              'mark': 0,
              'comment': "1. Social networking sites: các trang mạng xã hội.\n   - Definition: Websites that allow users to create personal profiles, connect with others, and share information and media.\n   - Example: Social networking sites like Facebook and Twitter have revolutionized the way people communicate and interact online.\n\n2. Technological advancement: sự tiến bộ công nghệ.\n   - Definition: Progress and improvement in technology that leads to new innovations and capabilities.\n   - Example: The rapid technological advancement in recent years has significantly impacted how we communicate through social media platforms.\n\n3. Virtual platforms: nền tảng ảo.\n   - Definition: Online spaces where users can interact, communicate, and share information in a digital environment.\n   - Example: Virtual platforms such as Instagram and Snapchat have become popular channels for social media communication.\n\n4. Cutting-edge technology: công nghệ tiên tiến.\n   - Definition: State-of-the-art technology that represents the latest advancements and innovations in a particular field.\n   - Example: Social media platforms constantly incorporate cutting-edge technology to enhance user experience and engagement.\n\n5. Genuine interaction: tương tác chân thực.\n   - Definition: Meaningful and authentic communication or engagement between individuals that is sincere and honest.\n   - Example: Face-to-face conversations often allow for more genuine interaction compared to online exchanges on social media.\n\n6. Misunderstood information: thông tin bị hiểu lầm.\n   - Definition: Data or messages that are not correctly interpreted or comprehended, leading to confusion or misinterpretation.\n   - Example: Misunderstood information shared on social media can result in conflicts and misunderstandings among users.\n\n7. Cyberbullying: bắt nạt trực tuyến.\n   - Definition: Bullying or harassment that occurs online through electronic communication platforms such as social media.\n   - Example: Cyberbullying on social media can have serious consequences for individuals, affecting their mental health and well-being.\n\n8. Anonymously: ẩn danh.\n   - Definition: Without revealing one's identity or remaining unknown or unidentifiable.\n   - Example: Some individuals engage in cyberbullying anonymously to avoid being held accountable for their actions on social media.\n\n9. Phenomenon: hiện tượng.\n   - Definition: A fact or event that can be observed and studied, often with widespread implications or significance.\n   - Example: The rise of social media as a dominant communication platform is a phenomenon that has transformed how people interact and connect globally.\n\n10. Concerns: lo ngại.\n    - Definition: Worries, fears, or issues that cause unease or apprehension about potential negative outcomes.\n    - Example: There are growing concerns about the impact of social media on mental health and well-being, particularly among young users."
          },
          {
              'criteriaId': 47,
              'name': 'Improved Version',
              'mark': 0,
              'comment': "It is a common belief that the rise of social media is revolutionizing the way we communicate today. However, there is a more persuasive argument that this technological advancement has had negative effects on society because it discourages individuals from engaging in face-to-face interactions.\n\nOn the one hand, virtual platforms have made communication easier than ever before. The cutting-edge technology on these sites provides opportunities to connect within a short time. Compared to traditional methods, individuals no longer need to waste time waiting for a response. For example, Facebook has updated features that allow users to network with their preferred friends instantly. As a result, having conversations through social sites can greatly expand one's range of connections.\n\nNevertheless, the widespread use of social media can deteriorate the quality of conversations. Due to the lack of genuine interaction, it can reduce the need for meeting in person, which may increase misunderstandings and misinterpretations. Without fully comprehending the context, conflicts may arise between individuals. For instance, online dating is a common trend among the youth who want to find a suitable partner. However, merely receiving messages passively makes it challenging to evaluate the background and various criteria of potential partners. Furthermore, cyberbullying can also occur as social media becomes more dominant. Because these acts can be done anonymously, it is challenging for authorities to control and prevent this phenomenon. As a consequence, more concerns will inevitably become widespread.\n\nTo sum up, while there are some positive impacts of social media in transforming outdated communication methods, it is evident that the disadvantages far outweigh the benefits."
          }
      ],
      doc: {
          'data': {
              'filename': '9230fa91-cf20-4f3f-b640-4c975dfa7ba4_0.pdf',
              'data': 'JVBERi0xLjQKJeLjz9MKMiAwIG9iago8PC9MZW5ndGggMTA3MC9GaWx0ZXIvRmxhdGVEZWNvZGU+PnN0cmVhbQp4nJVWy47jNhC8+yt43ABeZyfBYnPdTXaQXBIEMJBzm2xJzFBsDR/2OF+fatKe2IAGwR5sSLbYj6rqaj1vvuw3Hz+aTz9+Mnu3+WDeP/ykF98/PpiHH8x+2Lz7rRifDRkr8yzRHDh4HkyZqOCLTeHoONqzKWJqZpPFegpmZudJDyY+SqjFS/T/+Di2MzNTzEaG7/Z/I+WHu8zvNE+N3pKeMVFO5Oicd+ZXOfGR01YjJO41zYKrhVOulP2RDaWxzhzLpTy2U5QgI4KF1VzkjhQttyMTZXyciTwiNYLxMLAtqDO2pric0bwlbdIX43y2UhONnI2Pzh+9qxSyGZLMq7kOrO13eHarT5jVX/+IDTOJjPKi25qjTwWpzBKoDJJmLRvlzuTY3ICHZAxUOCkYyhue5Z3ZI5atRf9/v5qP3cj/QXc2nWdl1hf0uiRBq7iQZREUEn3xuAP7Mz1x00mMwE3JO/ky+Yjf8oRHV7MVP6Oon2VeKLHTME1UiZzXEE1JZRKXt3coz3SGNArIspwzJR9wzz3AiXJBCARezXgi38ABHKgscV4kZtTwiHt+oXkJvFXOH8nyQeRpq2yrOOriqGgKFDgwlQrtNZ1RCHLKq8kgltTAiVxOkp4aJBrBJyDJAyftekgeU5Rv8GKg6Hbmc+4l1lC2yrLWTW9MTcRw5D40ZUpSx+k6i504hWxMqBtQ+Whxlbn1kiiCcRlumMvfIlDzu84lIgVQ0cYTjUAh6A/jRMsSrsOMHMBSC3FcOHlJALQRoYeewazHkPVKXrt5o5ZfKl/VEsg+6amRY/UYEx8RnFojjTwL+duaCvnQnm9C0awzc5MCMFcT0cdPk7eTlria9A632ecK70u5iDjE0FnsfWJMISX0n3NrwPwFYqUWM9QA9DGk+G8C5RdDfIvRwi+lA4qbATCWTiP0jhoOkBRzvJ2LrmIfc1FbUxkHBcR1O7ix8fVZvFq5DC3pGSVPxrebM8ZKfVXMgHzoUIlNsiSvFGJ4S+S0M19fLC/lldKFsvpyWEczYXjVaPtKwBjDSxthKNROmCqOY0NIDB8pVM1EB8VRTxzA+gidoxrUO9CsHgDkV1M5DzNPavM2eZUeKVZVVZt0iWyNPR9glQclSHOqZgCoGLGQjsoiAEeZfVRkNePtoluHE36Y1aUt3OXLZXN0KyVl0jZXRlAwRFHieZaaw3mLyCsgXCBdX2PgSVI3YmhZklPXF3WYY9+GCLZAcIJNB33qjjJ9H6nKkiDL1W2suuFzZdXParK2c/GU5RTVskJQKZCCG9qGRBKDtdAG+Js23V5MrjN8dnvBu296hAYkCErD4NuIQTMeC6Mt53siwFnhrj3opPv1ifTl5H/eNfgqPNbt9voGgThY9O01obRV3y1soKTxT+zHqQtdYLJaF8i8M6yv+82fm38BhSsgZwplbmRzdHJlYW0KZW5kb2JqCjQgMCBvYmoKPDwvVHlwZS9QYWdlL01lZGlhQm94WzAgMCA2MTIgNzkyXS9SZXNvdXJjZXM8PC9Gb250PDwvRjEgMSAwIFI+Pj4+L0NvbnRlbnRzIDIgMCBSL1BhcmVudCAzIDAgUj4+CmVuZG9iagoxIDAgb2JqCjw8L1R5cGUvRm9udC9TdWJ0eXBlL1R5cGUxL0Jhc2VGb250L0hlbHZldGljYS9FbmNvZGluZy9XaW5BbnNpRW5jb2Rpbmc+PgplbmRvYmoKMyAwIG9iago8PC9UeXBlL1BhZ2VzL0NvdW50IDEvS2lkc1s0IDAgUl0+PgplbmRvYmoKNSAwIG9iago8PC9UeXBlL0NhdGFsb2cvUGFnZXMgMyAwIFI+PgplbmRvYmoKNiAwIG9iago8PC9Qcm9kdWNlcihpVGV4dFNoYXJwkiA1LjUuMTMuMiCpMjAwMC0yMDIwIGlUZXh0IEdyb3VwIE5WIFwoQUdQTC12ZXJzaW9uXCkpL0NyZWF0aW9uRGF0ZShEOjIwMjQwNjE3MTgxODU1LTA0JzAwJykvTW9kRGF0ZShEOjIwMjQwNjE3MTgxODU1LTA0JzAwJyk+PgplbmRvYmoKeHJlZgowIDcKMDAwMDAwMDAwMCA2NTUzNSBmIAowMDAwMDAxMjY1IDAwMDAwIG4gCjAwMDAwMDAwMTUgMDAwMDAgbiAKMDAwMDAwMTM1MyAwMDAwMCBuIAowMDAwMDAxMTUzIDAwMDAwIG4gCjAwMDAwMDE0MDQgMDAwMDAgbiAKMDAwMDAwMTQ0OSAwMDAwMCBuIAp0cmFpbGVyCjw8L1NpemUgNy9Sb290IDUgMCBSL0luZm8gNiAwIFIvSUQgWzxlOWI4YzRlNjNkYjVjN2EzY2U0ZTUxZDhjMmVlYjIwYT48ZTliOGM0ZTYzZGI1YzdhM2NlNGU1MWQ4YzJlZWIyMGE+XT4+CiVpVGV4dC01LjUuMTMuMgpzdGFydHhyZWYKMTYxNAolJUVPRgo=',
              'status': 'Submitted',
              'pageCount': 0,
              'text': 'It is a common belief that the tendency to use social media is revolutionizing the means of communication nowadays. However, there is a more persuasive argument that technological advancement has had negative effects on society because it discourages individuals from being social.\r\n\r\nOn the one hand, virtual platforms have made communicating easier than before. The cutting-edge technology at these sites provides opportunities to make a connection within a short time. Compared to the traditional methods, individuals may not necessarily need to waste time waiting for a response. For example, on Facebook, it has updated the feature that allows users to network with their preferred friends within a second. As a result, having a conversation through social sites may greatly increase the range of connections. \r\n\r\n Nevertheless, the widespread application of it may deteriorate for the quality of conversations. Due to the lack of genuine interaction, it can curtail the need for meeting in person, which may increase the misunderstood information and expressions. Without fully comprehending the context, the conflicts may arise between individuals. For instance, online dating is a common tendency of the youth if they want to find an appropriate partner. Except for the passively receive the messages, it is challenging to evaluate about the background of family and different criterias. Furthermore, cyber bullying can also occur while dominance of social media takes place. Because these acts can be done anonymously, so it is challenging for the authorities in order to prevent this phenomenon from being controlled. As a consequence, more concerns will apparently become universal.\r\n\r\nTo sum up, while there are some affirmative impacts of social media alter the outdated way to communicate, it is evident that the disadvantages of it far outweigh the positive ones.\r\n',
              'createdDate': '2024-06-17T22:18:55.059846',
              'submissions': [],
              'id': 14390
          },
          'status': 200,
          'statusText': 'OK',
          'headers': {
              'content-length': '4633',
              'content-type': 'application/json; charset=utf-8'
          },
          'config': {
              'url': 'document/14390',
              'method': 'get',
              'headers': {
                  'Accept': 'application/json, text/plain, */*',
                  'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImxlYXJuZXI3MTFAdGVzdC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjkyMzBmYTkxLWNmMjAtNGYzZi1iNjQwLTRjOTc1ZGZhN2JhNCIsIlJvbGUiOiJsZWFybmVyIiwiZXhwIjoxNzIxMjQ0NTg3LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjY5OTAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjY5OTAifQ.-0NDvY1cS9Y5ub4qBNt8UuUHlVicDLd4ueaGzHBZQzo'
              },
              'baseURL': 'http://localhost:6990/api',
              'transformRequest': [
                  null
              ],
              'transformResponse': [
                  null
              ],
              'timeout': 600000,
              'xsrfCookieName': 'XSRF-TOKEN',
              'xsrfHeaderName': 'X-XSRF-TOKEN',
              'maxContentLength': -1
          },
          'request': {}
      }
    }
  },
  computed: {

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

    this.isAiReview = true
    this.hasGrade = true

    this.loadingReview = true
    const question = this.question
    this.task = question.section
    document.title = 'Bài chấm mẫu'

    const doc = this.doc
    this.documentText = doc.data.text
    this.docData = this.base64ToArrayBuffer(doc.data.data)
    this.loadFeedbackCompleted = true

    // load annotations if already saved in db
    PDFJSAnnotate.getStoreAdapter().loadAnnotations(this.documentId, this.loadedAnnotation)

    this.completeLoading = true
    await this.render()

    this.handleCommentPositionsRestore()
    this.intextCommentCompleted = true
    this.$refs.toolBar?.handleScale('fitPage')
    this.$refs.toolBar?.insertExpandMenu()

     // Dispatch render event after DOM manipulations
     document.dispatchEvent(new Event('render-event'))
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
    subscriptionCheck() {
      const feedbackType = this.review.reviewRequest.reviewType
      if (this.userSubscription == null) {
        if (feedbackType == 'detail' && this.freeToken <= 0) {
          this.$notify.info({
            title: 'Đã hết lượt chấm miễn phí',
            message: 'Lượt chấm miễn phí cho phản hồi chi tiết đã được sử dụng hết.',
            type: 'info',
            duration: 5000
          })
          this.$router.push('/pricing')
          return false
        } else if (feedbackType == 'deep' && this.premiumToken <= 0) {
          this.$notify.error({
            title: 'Đã hết lượt chấm miễn phí',
            message: 'Lượt chấm miễn phí cho phản hồi chuyên sâu đã được sử dụng hết.',
            type: 'info',
            duration: 5000
          })
          this.$router.push('/pricing')
          return false
        }
      }
    },
    rateAIReview() {
      reviewService.createAIReviewRating({
        UserId: 'AI Review Version 1.5',
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
      const containerHeight = window.innerHeight
      const elContainer = document.getElementById('reviewContainer')

      if (elContainer.style) {
        elContainer.style.height = containerHeight + 'px'
      }
      if (this.screenWidth > 780) {
        const rightPanel = document.getElementById('right-panel')
        const viewerContainer = document.getElementById('viewerContainer')
        viewerContainer.style.height = rightPanel.offsetHeight - document.getElementById('tool-bar').offsetHeight - 5 + 'px'
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
