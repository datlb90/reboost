<template>
  <div v-if="screenWidth > 780" id="practiceWritingContainer" :style="{ height: containerHeight, visibility: loadCompleted?'visible': 'hidden' }">
    <splitpanes class="default-theme" vertical style="height: 100%; width: 100%;">
      <pane>
        <el-tabs v-model="activeTab" type="border-card" style="height: 100%;" @tab-click="showDiscussion" @tab-remove="onTabRemove">
          <el-tab-pane name="description">
            <p slot="label" style="font-size: 14px; line-height: 2.6;" data-intro="Test intro">Chủ đề</p>
            <div v-if="getDataQuestion.title" style="height: 100%;display: flex; flex-direction: column">
              <div style="margin-bottom: 8px;">
                <el-row>
                  <el-col>
                    <div style="">
                      <div>
                        <div style="height: 32px;">
                          <div class="title-tab">
                            {{ getDataQuestion.id }}. {{ getDataQuestion.title }}
                          </div>
                        </div>
                        <div>
                          <el-tag
                            v-if="getDataQuestion.difficulty == 'Medium'"
                            type="warning"
                            size="medium"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.difficulty }}
                          </el-tag>
                          <el-tag
                            v-else-if="getDataQuestion.difficulty == 'Hard'"
                            type="danger"
                            size="medium"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.difficulty }}
                          </el-tag>
                          <el-tag
                            v-else-if="getDataQuestion.difficulty == 'Easy'"
                            size="medium"
                            type="success"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.difficulty }}
                          </el-tag>

                          <el-tag
                            v-else
                            size="medium"
                            type="info"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            Undefined
                          </el-tag>

                          <el-tag
                            size="medium"
                            type="info"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.section }}
                          </el-tag>

                          <el-tag
                            size="medium"
                            type="info"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.type }}
                          </el-tag>

                          <!-- <div style="font-size: 13px; color: #44ce6f; margin-right: 25px;">{{ getDataQuestion.test }} {{ getDataQuestion.section }}</div> -->
                          <!-- <div style="display: flex; margin-left: 5px; margin-right: 20px;">
                            <img class="img-icon" src="../../assets/img/like.png" alt="">
                            <label for="" style="font-weight: 200; font-size: 13px;">{{ getDataQuestion.like }}</label>
                          </div>
                          <div style="display: flex;  margin-right: 20px;">
                            <img class="img-icon" src="../../assets/img/dislike.png" alt="">
                            <label for="" style="font-weight: 200; font-size: 13px;">{{ getDataQuestion.dislike }}</label>
                          </div> -->
                        </div>
                      </div>
                    </div>
                    <div class="info" style="margin-top: 5px; font-size: 16px;" v-html="getDataQuestion.direction" />
                  </el-col>
                </el-row>
              </div>
              <div>
                <div>
                  <el-row style="margin-bottom: 8px;">
                    <div id="questionContent" class="tip" style="font-size: 16px;" v-html="getQuestion.content" />
                  </el-row>
                  <el-row v-if="isShowQuestion">
                    <div v-if="!isShowListeningTab && getReading != ''">
                      <el-card class="box-card sample-box">
                        <div style="height: 40px; border-bottom: 1px solid rgb(220 223 229); margin-bottom: 10px; ">
                          <div style="float: left; margin-top: 2px;">
                            <span style="font-size: 16px; font-weight: 600;"> Reading Passage</span>
                          </div>
                          <div style="float: right;">
                            <el-button size="mini" @click="toggleBtnShowTab()">Go to listening</el-button>
                          </div>
                        </div>
                        <div class="box-card__content">
                          <div>
                            <div v-html="getReading.content" />
                          </div>
                        </div>
                      </el-card>
                    </div>

                    <el-col v-if="getReading =='' && getListening != '' || isShowListeningTab || getReading == ''" :span="24">
                      <div v-if="isShowListeningTab" style="width: 100%;">
                        <el-card class="box-card sample-box">
                          <div style="height: 40px; border-bottom: 1px solid rgb(220 223 229); margin-bottom: 10px; ">
                            <div style="float: left; margin-top: 2px;">
                              <span style="font-size: 16px; font-weight: 600;"> Listening Lecture</span>
                            </div>
                            <div style="float: right;">
                              <el-button size="mini" @click="backClick()">Back to Reading</el-button>
                            </div>
                          </div>
                          <div class="box-card__content">
                            <div>
                              <audio v-if="getListening != ''" controls style="width: 100%; height: 35px; margin-bottom: 3px;">
                                <source :src="'/audio/'+getListening.content" type="audio/mpeg">
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
                              <div v-if="isShowQuestion && isShowScript && isShowListeningTab" class="body-transcript" style="margin: 0;">
                                <pre id="transcriptContent" v-html="getTranscript.content" />
                              </div>
                            </div>
                          </div>
                        </el-card>
                      </div>
                    </el-col>
                  </el-row>
                  <div v-if="isShowQuestion && (isShowChart || (getReading == '' && getChart != ''))">
                    <img :src="'/photo/' + getChart.content" :alt="getChart.content" style="max-height: 100%; max-width: 100%;">
                  </div>
                </div>
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane name="vocabulary" style="height: 100%; position: relative;">
            <p slot="label" style="font-size: 14px; line-height: 2.6;">Từ vựng</p>
            <div>
              <div v-if="getVocaburary != ''">
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div id="tipContent" v-html="getVocaburary.content" />
                </el-card>
              </div>
              <div v-else>
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div> We are working on this section, please check back soon for help on the writing topic including suggesstions on useful vocabulary that could be used to improve your band score.</div>
                </el-card>
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane name="idea" style="height: 100%; position: relative;">
            <p slot="label" style="font-size: 14px; line-height: 2.6;">Ý tưởng</p>
            <div>
              <div v-if="getTip != ''">
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div id="tipContent" v-html="getTip.content" />
                </el-card>
              </div>
              <div v-else>
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div> We are working on this section, please check back soon for help on the writing topic including suggesstions on development ideas,
                    instructions on how to structure your eassy.</div>
                </el-card>
              </div>
            </div>
          </el-tab-pane>

          <el-tab-pane name="sample" style="height: 100%; position: relative;">
            <p slot="label" style="font-size: 14px; line-height: 2.6;">Bài viết mẫu</p>
            <div>
              <tab-samples :question-id="questionId" />
            </div>
          </el-tab-pane>
        </el-tabs>
      </pane>
      <pane v-if="!tabDisCussionShowed">
        <el-tabs v-model="activeTab2" type="border-card" style="height: 100%;" @tab-remove="onTabRemove">
          <el-tab-pane name="essay">
            <p slot="label" style="font-size: 14px; line-height: 2.6;">Bài làm</p>
            <div style="height: 100%; display: flex; flex-direction: column;">
              <div class="header-passage">
                <div style="float: left; margin-right: 5px; ">
                  <div>
                    <el-tooltip v-if="isTesting" class="item" effect="light" content="Huỷ tính giờ" placement="bottom">
                      <el-button
                        type="info"
                        plain
                        icon="el-icon-close"
                        size="mini"
                        style="padding-left: 8px; padding-right: 8px; "
                        @click="cancelTest()"
                      />
                    </el-tooltip>

                    <el-tooltip v-if="isTesting && !isTestingPaused" class="item" effect="light" content="Bài làm sẽ được nộp khi hết giờ. Nhấn để tạm dừng." placement="bottom">
                      <el-button
                        type="info"
                        plain
                        size="mini"
                        style="margin-left: 2px; padding-left: 8px; padding-right: 8px;"
                        @click="pauseTest()"
                      >
                        <i style="font-weight: bold; margin-right: 2px;" class="el-icon-video-pause" />
                        {{ minute }} : {{ getSecond }}
                      </el-button>
                    </el-tooltip>

                    <el-tooltip v-if="isTesting && isTestingPaused" class="item" effect="light" content="Nhấn để tiếp tục tính giờ" placement="bottom">
                      <el-button
                        type="info"
                        plain
                        size="mini"
                        style="margin-left: 2px; padding-left: 8px; padding-right: 8px;     font-weight: bold;"
                        @click="resumeTest()"
                      >
                        <i style="font-weight: bold; margin-right: 2px;" class="el-icon-video-play" />
                        {{ minute }} : {{ getSecond }}
                      </el-button>
                    </el-tooltip>

                    <el-button v-if="!isTesting && !submissionId" size="mini" type="danger" plain @click="enableTestMode()">
                      <i style="font-weight: bold; margin-right: 2px;" class="el-icon-timer" />
                      Tính giờ
                    </el-button>

                  </div>
                </div>

                <el-button
                  v-if="hasReview && docId && reviewId"
                  style="float: left; margin-right: 5px"
                  size="mini"
                  type="primary"
                  plain
                  @click="viewFeedback()"
                >Xem phản hồi</el-button>
                <el-button
                  v-if="((submissionId && isEdit) || writingSubmitted) && !isProRequested && !hasReview"
                  style="float: left; margin-right: 5px"
                  size="mini"
                  type="primary"
                  plain
                  @click="requestReview()"
                >Nhận phản hồi</el-button>
                <div v-if="getQuestion != '' && !writingSubmitted" style="width: 150px; float: left;">
                  <el-tag
                    v-if="isShowCountWord && countWord != 0"
                    type="info"
                    size="medium"
                    closable
                    effect="plain"
                    :disable-transitions="true"
                    @close="toggleShowCount()"
                  >
                    Số từ: {{ countWord }}
                  </el-tag>

                  <el-button v-if="!isShowCountWord" size="mini" plain @click="toggleShowCount()">
                    Hiện số từ
                  </el-button>
                </div>
                <div v-if="getQuestion != ''">
                  <el-button
                    v-if="!writingSubmitted && hasSubmitionForThisQuestion && !isEdit"
                    style="float: right; margin-left: 5px"
                    size="mini"
                    :disabled="!(writingContent && writingContent.length > 0)"
                    :loading="isLoading"
                    type="primary"
                    @click="submit()"
                  >Nhận phản hồi</el-button>
                  <el-button
                    v-if="isEdit && !isFreeRequested && !isProRequested"
                    style="float: right; margin-left: 5px"
                    size="mini"
                    @click="editSubmission()"
                  >Sửa</el-button>
                  <el-button
                    v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                    size="mini"
                    :disabled="!(writingContent && writingContent.length > 0)"
                    :loading="isLoading"
                    type="primary"
                    style="float: right; margin-left: 5px;"
                    @click="submit()"
                  >Nhận phản hồi</el-button>
                  <!-- <el-button
                    v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                    size="mini"
                    :disabled="!(writingContent && writingContent.length > 0)"
                    style="float: right;"
                    @click="save()"
                  >Lưu lại</el-button> -->
                </div>
              </div>
              <div style="flex-grow: 1;">
                <textarea
                  v-model="writingContent"
                  :disabled="isEdit || writingSubmitted"
                  placeholder="Bắt đầu bài viết của bạn ở đây ..."
                  spellcheck="false"
                  class="textarea-style"
                  @keyup="countWords()"
                />
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane name="note" style="height: 100%; position: relative;">
            <p slot="label" style="font-size: 14px; line-height: 2.6;">Ghi chú</p>
            <div>
              <div>
                <el-tiptap
                  v-model="questionNote"
                  placeholder="Ghi chú của bạn cho chủ đề này, bao gồm việc lên ý tưởng, lập kế hoạch viết, xây dựng bố cục bài, cũng như lựa chọn về từ vựng và cấu trúc câu."
                  :extensions="extensions"
                  style="height: calc(100vh - 110px); font-size: 14px;"
                  @onUpdate="onNoteUpdate()"
                />
              </div>
            </div>

          </el-tab-pane>
          <el-tab-pane name="submissions" style="height: 100%; position: relative;">
            <p slot="label" style="font-size: 14px; line-height: 2.6;">Bài đã nộp</p>
            <div style="padding-right: 10px;">
              <el-table
                :data="submissions"
                stripe
                style="width: 100%"
                size="mini"
                :row-style="{'cursor': 'pointer'}"
                @row-click="onSubmissionRowClick"
              >
                <el-table-column
                  prop="id"
                  label="Id"
                  width="50"
                />
                <el-table-column
                  prop="submittedTimeStr"
                  label="Submitted Date"
                  min-width="155"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.submittedTimeStr }}</span>
                  </template>
                </el-table-column>

                <el-table-column
                  prop="status"
                  label="Status"
                  min-width="160"
                >
                  <template slot-scope="scope">
                    <el-tag>{{ scope.row.status }}</el-tag>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="timeTaken"
                  label="Time Taken"
                  min-width="120"
                >
                  <template slot-scope="scope">
                    <span>{{ getTimeTaken(scope.row.timeTaken) }}</span>
                  </template>
                </el-table-column>

                <el-table-column
                  label="Word Count"
                  min-width="100"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.text ? scope.row.text.trim().split(/\b\S+\b/).length - 1 : 0 }}</span>
                  </template>
                </el-table-column>

              </el-table>
            </div>
          </el-tab-pane>

          <el-tab-pane
            v-for="item in editableTabs"
            :key="item.id"
            :label="item.status"
            :name="'submission' + item.id"
            closable
          >
            <el-card class="box-card">
              <div slot="header" class="box-card__title" style="min-width: 380px;">
                <div style="height: 30px;">
                  <div style="width: 260px; float: left; margin-top: 5px; font-size: 14px; font-weight: bold;">
                    Ngày nộp: {{ item.submittedTimeStr }}
                  </div>
                  <div style="float: right;">
                    <el-tag size="medium">Status: {{ item.status }}</el-tag>
                  </div>
                </div>
                <div style="height: 30px;">
                  <div style="float: left; margin-right: 5px;">
                    <el-tag
                      type="info"
                      size="medium"
                      effect="plain"
                    >
                      Thời gian: {{ getTimeTaken(item.timeTaken) }}
                    </el-tag>
                  </div>
                  <div style="float: left; margin-right: 5px;">
                    <el-tag
                      type="info"
                      size="medium"
                      effect="plain"
                    >
                      Số từ: {{ item.text ? item.text.trim().split(/\b\S+\b/).length - 1 : 0 }}
                    </el-tag>
                  </div>
                  <div style="margin-top: 8px; float: right; " @click="copyTextToClipboard(item.text)">
                    <i class="far fa-clone" style="font-size: 18px; color: #9b9898; cursor: pointer;" />
                  </div>
                </div>
              </div>
              <div class="box-card__content">
                <div style="flex-grow: 1;">
                  <textarea v-model="item.text" :disabled="true" spellcheck="false" class="textarea-style" style="height: calc(100vh - 220px); border-top: 1px solid #e2e2e2;" />
                </div>
              </div>
            </el-card>
          </el-tab-pane>
        </el-tabs>
      </pane>
    </splitpanes>
    <div>
      <checkout
        ref="checkoutDialog"
        :submission-id="+submissionId"
        :question-id="+questionId"
      />
    </div>
    <el-dialog
      title="Time Out"
      :visible.sync="dialogVisible"
      width="30%"
    >
      <span>Your test time is over. Do you want to submit your current writing?</span>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">Cancel</el-button>
        <el-button type="primary" @click="submit">Submit</el-button>
      </span>
    </el-dialog>

    <!-- <v-tour name="myTour" :steps="steps" :callbacks="tourCallback" /> -->

  </div>
  <div v-else id="practiceWritingContainer">
    <div style="width: 100%; margin-top: 55px; height: calc(100vh - 60px); overflow: auto;">
      <div id="tabs-wrapper">
        <el-tabs v-model="activeTab" type="border-card" style="margin-bottom: 10px;" @tab-click="showDiscussion">
          <el-tab-pane label="Chủ đề" name="description">
            <div v-if="getDataQuestion.title" style="height: 100%;display: flex; flex-direction: column">
              <div style="margin-bottom: 8px;">
                <el-row>
                  <el-col>
                    <div style="">
                      <div>
                        <div style="height: 32px;">
                          <div class="title-tab">
                            {{ getDataQuestion.id }}. {{ getDataQuestion.title }}
                          </div>
                        </div>

                        <div>
                          <el-tag
                            v-if="getDataQuestion.difficulty == 'Medium'"
                            type="warning"
                            size="medium"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.difficulty }}
                          </el-tag>
                          <el-tag
                            v-else-if="getDataQuestion.difficulty == 'Hard'"
                            type="danger"
                            size="medium"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.difficulty }}
                          </el-tag>
                          <el-tag
                            v-else-if="getDataQuestion.difficulty == 'Easy'"
                            size="medium"
                            type="success"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.difficulty }}
                          </el-tag>

                          <el-tag
                            v-else
                            size="medium"
                            type="info"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            Undefined
                          </el-tag>

                          <el-tag
                            size="medium"
                            type="info"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.section }}
                          </el-tag>

                          <el-tag
                            size="medium"
                            type="info"
                            style="margin-bottom: 5px; margin-right: 5px;"
                          >
                            {{ getDataQuestion.type }}
                          </el-tag>

                        </div>
                      </div>

                    </div>

                    <div class="info" style="margin-top: 5px; font-size: 16px;" v-html="getDataQuestion.direction" />
                  </el-col>
                </el-row>

              </div>
              <div>

                <div>
                  <el-row style="margin-bottom: 8px;">
                    <div id="questionContent" class="tip" style="font-size: 16px;" v-html="getQuestion.content" />
                  </el-row>
                  <el-row v-if="isShowQuestion">
                    <div v-if="!isShowListeningTab && getReading != ''">
                      <el-card class="box-card sample-box">
                        <div style="height: 40px; border-bottom: 1px solid rgb(220 223 229); margin-bottom: 10px; ">
                          <div style="float: left; margin-top: 2px;">
                            <span style="font-size: 16px; font-weight: 600;"> Reading Passage</span>
                          </div>
                          <div style="float: right;">
                            <el-button size="mini" @click="toggleBtnShowTab()">Go to listening</el-button>
                          </div>
                        </div>
                        <div class="box-card__content">
                          <div>
                            <div v-html="getReading.content" />
                          </div>
                        </div>
                      </el-card>
                    </div>

                    <el-col v-if="getReading =='' && getListening != '' || isShowListeningTab || getReading == ''" :span="24">
                      <div v-if="isShowListeningTab" style="width: 100%;">
                        <el-card class="box-card sample-box">
                          <div style="height: 40px; border-bottom: 1px solid rgb(220 223 229); margin-bottom: 10px; ">
                            <div style="float: left; margin-top: 2px;">
                              <span style="font-size: 16px; font-weight: 600;"> Listening Lecture</span>
                            </div>
                            <div style="float: right;">
                              <el-button size="mini" @click="backClick()">Back to Reading</el-button>
                            </div>
                          </div>
                          <div class="box-card__content">
                            <div>
                              <audio v-if="getListening != ''" controls style="width: 100%; height: 35px; margin-bottom: 3px;">
                                <source :src="'/audio/'+getListening.content" type="audio/mpeg">
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
                              <div v-if="isShowQuestion && isShowScript && isShowListeningTab" class="body-transcript" style="margin: 0;">
                                <pre id="transcriptContent" v-html="getTranscript.content" />
                              </div>
                            </div>
                          </div>
                        </el-card>
                      </div>
                    </el-col>
                  </el-row>
                  <div v-if="isShowQuestion && (isShowChart || (getReading == '' && getChart != ''))">
                    <img :src="'/photo/' + getChart.content" :alt="getChart.content" style="max-height: 100%; max-width: 100%;">
                  </div>
                </div>
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane label="Từ vựng" name="vocabulary" style="height: 100%; position: relative;">
            <div>
              <div v-if="getVocaburary != ''">
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div id="tipContent" v-html="getVocaburary.content" />
                </el-card>
              </div>
              <div v-else>
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div> We are working on this section, please check back soon for help on the writing topic including suggesstions on useful vocabulary that could be used to improve your band score.</div>
                </el-card>
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane label="Ý tưởng" name="idea" style="height: 100%; position: relative;">
            <div>
              <div v-if="getTip != ''">
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div id="tipContent" v-html="getTip.content" />
                </el-card>
              </div>
              <div v-else>
                <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                  <div> We are working on this section, please check back soon for help on the writing topic including suggesstions on development ideas,
                    instructions on how to structure your eassy, and useful vocabulary that could be used to improve your band score.</div>
                </el-card>
              </div>
            </div>
          </el-tab-pane>

          <el-tab-pane label="Bài viết mẫu" name="sample" style="height: 100%; position: relative;">
            <div>
              <tab-samples :question-id="questionId" />
            </div>
          </el-tab-pane>
        </el-tabs>
      </div>

      <div style="height: 800px; display: flex; flex-direction: column;">
        <el-tabs v-model="activeTab2" type="border-card" style="height: 800px;" @tab-remove="onTabRemove">
          <el-tab-pane label="Bài làm" name="essay">
            <div style="height: 100%; display: flex; flex-direction: column;">
              <div class="header-passage">
                <div style="float: left; margin-right: 5px; ">
                  <div>
                    <el-tooltip v-if="isTesting" class="item" effect="light" content="Huỷ tính giờ" placement="bottom">
                      <el-button
                        type="info"
                        plain
                        icon="el-icon-close"
                        size="mini"
                        style="padding-left: 8px; padding-right: 8px; "
                        @click="cancelTest()"
                      />
                    </el-tooltip>

                    <el-tooltip v-if="isTesting && !isTestingPaused" class="item" effect="light" content="Bài làm sẽ được nộp khi hết giờ. Nhấn để tạm dừng." placement="bottom">
                      <el-button
                        type="info"
                        plain
                        size="mini"
                        style="margin-left: 2px; padding-left: 8px; padding-right: 8px;"
                        @click="pauseTest()"
                      >
                        <i style="font-weight: bold; margin-right: 2px;" class="el-icon-video-pause" />
                        {{ minute }} : {{ getSecond }}
                      </el-button>
                    </el-tooltip>

                    <el-tooltip v-if="isTesting && isTestingPaused" class="item" effect="light" content="Nhấn để tiếp tục tính giờ" placement="bottom">
                      <el-button
                        type="info"
                        plain
                        size="mini"
                        style="margin-left: 2px; padding-left: 8px; padding-right: 8px;     font-weight: bold;"
                        @click="resumeTest()"
                      >
                        <i style="font-weight: bold; margin-right: 2px;" class="el-icon-video-play" />
                        {{ minute }} : {{ getSecond }}
                      </el-button>
                    </el-tooltip>

                    <el-button v-if="!isTesting && !submissionId" size="mini" type="danger" plain @click="enableTestMode()">
                      <i style="font-weight: bold; margin-right: 2px;" class="el-icon-timer" />
                      Tính giờ
                    </el-button>
                  </div>
                </div>
                <el-button
                  v-if="hasReview && docId && reviewId"
                  style="float: left; margin-right: 5px"
                  size="mini"
                  type="primary"
                  plain
                  @click="viewFeedback()"
                >Xem phản hồi</el-button>
                <el-button
                  v-if="((submissionId && isEdit) || writingSubmitted) && !isProRequested && !hasReview"
                  style="float: left; margin-right: 5px"
                  size="mini"
                  type="primary"
                  plain
                  @click="requestReview()"
                >Nhận phản hồi</el-button>
                <div v-if="getQuestion != '' && !writingSubmitted" style="width: 150px; float: left;">
                  <el-tag
                    v-if="isShowCountWord && countWord != 0"
                    type="info"
                    size="medium"
                    closable
                    effect="plain"
                    :disable-transitions="true"
                    @close="toggleShowCount()"
                  >
                    Số từ: {{ countWord }}
                  </el-tag>

                  <el-button v-if="!isShowCountWord" size="mini" plain @click="toggleShowCount()">
                    Hiện số từ
                  </el-button>
                </div>
                <div v-if="getQuestion != ''">
                  <el-button
                    v-if="!writingSubmitted && hasSubmitionForThisQuestion && !isEdit"
                    style="float: right; margin-left: 5px"
                    size="mini"
                    :disabled="!(writingContent && writingContent.length > 0)"
                    :loading="isLoading"
                    type="primary"
                    @click="submit()"
                  >Nhận phản hồi</el-button>
                  <el-button
                    v-if="isEdit && !isFreeRequested && !isProRequested"
                    style="float: right; margin-left: 5px"
                    size="mini"
                    @click="editSubmission()"
                  >Sửa</el-button>
                  <el-button
                    v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                    size="mini"
                    :disabled="!(writingContent && writingContent.length > 0)"
                    :loading="isLoading"
                    type="primary"
                    style="float: right; margin-left: 5px;"
                    @click="submit()"
                  >Nhận phản hồi</el-button>
                  <!-- <el-button
                    v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                    size="mini"
                    :disabled="!(writingContent && writingContent.length > 0)"
                    style="float: right;"
                    @click="save()"
                  >Lưu lại</el-button> -->
                </div>
              </div>
              <div style="flex-grow: 1;">
                <textarea
                  v-model="writingContent"
                  :disabled="isEdit || writingSubmitted"
                  placeholder="Bắt đầu bài viết của bạn ở đây ..."
                  spellcheck="false"
                  class="textarea-style"
                  @keyup="countWords()"
                />
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane label="Ghi chú" name="note" style="height: 100%; position: relative;">
            <div>
              <div>
                <el-tiptap
                  v-model="questionNote"
                  placeholder="Ghi chú của bạn cho chủ đề này, bao gồm việc lên ý tưởng, lập kế hoạch viết, xây dựng bố cục bài, cũng như lựa chọn về từ vựng và cấu trúc câu."
                  :extensions="extensions"
                  style="height: calc(740px); font-size: 14px;"
                  @onUpdate="onNoteUpdate()"
                />
              </div>
            </div>
          </el-tab-pane>
          <el-tab-pane label="Bài đã nộp" name="submissions" style="height: 100%; position: relative;">
            <div style="padding-right: 10px;">
              <el-table
                :data="submissions"
                stripe
                style="width: 100%"
                size="mini"
                :row-style="{'cursor': 'pointer'}"
                @row-click="onSubmissionRowClick"
              >
                <el-table-column
                  prop="id"
                  label="Id"
                  width="50"
                />
                <el-table-column
                  prop="submittedTimeStr"
                  label="Submitted Date"
                  min-width="155"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.submittedTimeStr }}</span>
                  </template>
                </el-table-column>

                <el-table-column
                  prop="status"
                  label="Status"
                  min-width="160"
                >
                  <template slot-scope="scope">
                    <el-tag>{{ scope.row.status }}</el-tag>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="timeTaken"
                  label="Time Taken"
                  min-width="120"
                >
                  <template slot-scope="scope">
                    <span>{{ getTimeTaken(scope.row.timeTaken) }}</span>
                  </template>
                </el-table-column>

                <el-table-column
                  label="Word Count"
                  min-width="100"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.text ? scope.row.text.trim().split(/\b\S+\b/).length - 1 : 0 }}</span>
                  </template>
                </el-table-column>

              </el-table>
            </div>
          </el-tab-pane>

          <el-tab-pane
            v-for="item in editableTabs"
            :key="item.id"
            :label="item.status"
            :name="'submission' + item.id"
            closable
          >
            <el-card class="box-card">
              <div slot="header" class="box-card__title" style="min-width: 380px;">
                <div style="height: 30px;">
                  <div style="width: 260px; float: left; margin-top: 5px; font-size: 14px; font-weight: bold;">
                    Ngày nộp: {{ item.submittedTimeStr }}
                  </div>
                  <div style="float: right;">
                    <el-tag size="medium">Status: {{ item.status }}</el-tag>
                  </div>
                </div>
                <div style="height: 30px;">
                  <div style="float: left; margin-right: 5px;">
                    <el-tag
                      type="info"
                      size="medium"
                      effect="plain"
                    >
                      Thời gian: {{ getTimeTaken(item.timeTaken) }}
                    </el-tag>
                  </div>
                  <div style="float: left; margin-right: 5px;">
                    <el-tag
                      type="info"
                      size="medium"
                      effect="plain"
                    >
                      Số từ: {{ item.text ? item.text.trim().split(/\b\S+\b/).length - 1 : 0 }}
                    </el-tag>
                  </div>
                  <div style="margin-top: 8px; float: right; " @click="copyTextToClipboard(item.text)">
                    <i class="far fa-clone" style="font-size: 18px; color: #9b9898; cursor: pointer;" />
                  </div>
                </div>
              </div>
              <div class="box-card__content">
                <div style="flex-grow: 1;">
                  <textarea v-model="item.text" :disabled="true" spellcheck="false" class="textarea-style" style="height: calc(100vh - 220px); border-top: 1px solid #e2e2e2;" />
                </div>
              </div>
            </el-card>
          </el-tab-pane>
        </el-tabs>
      </div>
      <div>
        <checkout
          ref="checkoutDialog"
          :submission-id="+submissionId"
          :question-id="+questionId"
        />
      </div>
      <el-dialog
        title="Time Out"
        :visible.sync="dialogVisible"
        width="30%"
      >
        <span>Your test time is over. Do you want to submit your current writing?</span>
        <span slot="footer" class="dialog-footer">
          <el-button @click="dialogVisible = false">Cancel</el-button>
          <el-button type="primary" @click="submit">Submit</el-button>
        </span>
      </el-dialog>
    </div>
  </div>
</template>

<script>

import documentService from '../../services/document.service'
import reviewService from '../../services/review.service'
import questionService from '../../services/question.service'
import TabSamples from '../learner/PracticeWriting_TabSamples.vue'
import CheckOut from '../../components/controls/CheckOut.vue'
import {
  Doc,
  Text,
  Paragraph,
  Heading,
  Bold,
  Underline,
  Italic,
  Strike,
  ListItem,
  BulletList,
  OrderedList,
  FontSize,
  Indent,
  // LineHeight,
  TextColor
} from 'element-tiptap'

import {
  Splitpanes,
  Pane
} from 'splitpanes'

import 'splitpanes/dist/splitpanes.css'

import moment from 'moment-timezone'
import { useShepherd } from 'vue-shepherd'

export default {
  name: 'PracticeWriting',
  components: {
    'splitpanes': Splitpanes,
    'pane': Pane,
    'tab-samples': TabSamples,
    'checkout': CheckOut
  },
  data() {
    return {
      freeToken: this.$store.state.auth.user.freeToken,
      premiumToken: this.$store.state.auth.user.premiumToken,
      userSubscription: this.$store.state.auth.user.subscription,
      submissionsTabs: 'Comments',
      userSubmissions: [],
      selectedSubmission: undefined,
      loadCompleted: false,
      containerHeight: 0,
      textarea: '',
      isShowTimer: false,
      isShowListeningTab: false,
      isShowChart: false,
      isShowScript: false,
      isShowReading: true,
      minute: 0,
      second: 3,
      closeTimer: false,
      writingContent: '',
      questionId: parseInt(this.$route.params.id),
      isTest: false,
      countWord: 0,
      isShowCountWord: true,
      hideDirection: 'Hide',
      tabDisCussionShowed: false,
      writingSubmitted: false,
      checkoutVisible: false,
      hasSubmitionForThisQuestion: false,
      timeSpent: 0,
      timeSpentInterval: null,
      timeout: null,
      submissionId: null,
      isFreeRequested: false,
      isProRequested: false,
      showStartTestButton: false,
      isTesting: false,
      dialogVisible: false,
      isStart: false,
      timeStart: null,
      idSubmissionStorage: null,
      isEdit: false,
      isShowQuestion: true,
      submissions: [],
      editableTabs: [],
      activeTab: 'description',
      screenWidth: window.innerWidth,
      hasReview: false,
      docId: null,
      reviewId: null,
      isTestingPaused: false,
      isLoading: false,
      activeTab2: 'essay',
      questionNote: null,
      extensions: [
        new Doc(),
        new Text(),
        new Paragraph(),
        new Heading({ level: 5 }),
        new Bold(),
        new Underline(),
        new Italic(),
        new Strike(),
        new ListItem(),
        new BulletList(),
        new OrderedList(),
        new FontSize(),
        new TextColor(),
        new Indent()
      ],
      tourCallback: {
        onStop: this.customOnStopTour,
        onSkip: this.customOnSkipTour
      }
    }
  },
  computed: {
    getSecond() {
      if (this.second < 10) {
        return '0' + this.second.toString()
      } else {
        return this.second.toString()
      }
    },
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    getDataQuestion() {
      var data = this.$store.getters['question/getSelected']
      if (data.direction) {
        data.direction = data.direction.trim()
        if (data.direction.substr(data.direction.length - 1) == '\n') {
          data.direction = data.direction.substring(0, data.direction.length - 1)
        }
      }
      return data
    },
    getDataQuestionParts() {
      return this.$store.getters['question/getSelected']['questionsPart']
    },
    getVocaburary() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Vocabulary')) {
          return this.getDataQuestionParts.find(u => u.name == 'Vocabulary')
        }
      }
      return ''
    },
    getTip() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Tip')) {
          return this.getDataQuestionParts.find(u => u.name == 'Tip')
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
        return 'audio_sample.mp3'
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
    getChart() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Chart')) {
          var chart = this.getDataQuestionParts.find(u => u.name == 'Chart')
          // chart.content = '../../assets/' + chart.content.trim()
          return chart
        }
      }
      return ''
    },
    getTranscript() {
      if (typeof (this.getDataQuestionParts) != 'undefined') {
        if (this.getDataQuestionParts.find(u => u.name == 'Transcript')) {
          return this.getDataQuestionParts.find(u => u.name == 'Transcript')
        }
      }
      return ''
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  async mounted() {
    console.log(this.questionId)
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    window.component = this
    this.submissionId = this.$route.params.submissionId
    this.$store.dispatch('question/loadQuestion', this.questionId).then(rs => {
      console.log(rs)
      document.title = 'Thực hành - ' + rs.title
      this.calculateContainerHeight()
      this.loadCompleted = true
    })
    window.addEventListener('resize', this.calculateContainerHeight.bind(this))
    if (this.submissionId) {
      this.idSubmissionStorage = this.currentUser.username + '_QuestionId' + this.questionId + '_SubmissionId' + this.submissionId
      this.isEdit = true
    }
    this.idLocalStorage = this.currentUser.username + '_QuestionId' + this.questionId
    await this.loadData()
    this.questionNote = localStorage.getItem(this.currentUser.username + '_QuestionId' + this.questionId + '_Note')

    this.setupTour()
  },
  destroyed() {
    clearInterval(this.setIntervalForScroll)
    clearInterval(this.timeSpentInterval)
  },
  methods: {
    setupTour() {
      var tourCompleted = this.getCookie('tour')
      if (!tourCompleted) {
        const tour = useShepherd({
          useModalOverlay: true
        })

        tour.addSteps([
          {
            attachTo: { element: '#tab-description', on: 'bottom'},
            // title: 'Chủ đề viết',
            text: 'Nơi chứa thông tin về yêu cầu của đề bài, dạng đề, và mức độc khó.',
            classes: 'intro-step',
            buttons: [
              {
                text: 'Bỏ qua',
                action: () => {
                  this.setCookie('tour', 'completed')
                  tour.complete()
                },
                secondary: true
              },
              {
                text: 'Tiếp tục',
                action: tour.next
              }
            ]
          },
          {
            attachTo: { element: '#tab-vocabulary', on: 'bottom'},
            text: 'Từ vựng hữu dụng cho chủ đề này.',
            classes: 'intro-step',
            buttons: [
              {
                text: 'Bỏ qua',
                action: () => {
                  this.setCookie('tour', 'completed')
                  tour.complete()
                },
                secondary: true
              },
              {
                text: 'Quay lại',
                action: tour.back,
                secondary: true
              },
              {
                text: 'Tiếp tục',
                action: tour.next
              }
            ]
          },
          {
            attachTo: { element: '#tab-idea', on: 'bottom'},
            text: 'Gợi ý về ý tưởng và hướng phát triển bài viết.',
            classes: 'intro-step',
            buttons: [
              {
                text: 'Bỏ qua',
                action: () => {
                  this.setCookie('tour', 'completed')
                  tour.complete()
                },
                secondary: true
              },
              {
                text: 'Quay lại',
                action: tour.back,
                secondary: true
              },
              {
                text: 'Tiếp tục',
                action: tour.next
              }
            ]
          },
          {
            attachTo: { element: '#tab-sample', on: 'bottom'},
            text: 'Các bài viết mẫu kèm theo phân tích chi tiết.',
            classes: 'intro-step',
            buttons: [
            {
                text: 'Bỏ qua',
                action: () => {
                  this.setCookie('tour', 'completed')
                  tour.complete()
                },
                secondary: true
              },
              {
                text: 'Quay lại',
                action: tour.back,
                secondary: true
              },
              {
                text: 'Tiếp tục',
                action: tour.next
              }
            ]
          },
          {
            attachTo: { element: '#tab-essay', on: 'bottom'},
            text: 'Nơi bạn viết bài luận của mình.',
            classes: 'intro-step',
            buttons: [
            {
                text: 'Bỏ qua',
                action: () => {
                  this.setCookie('tour', 'completed')
                  tour.complete()
                },
                secondary: true
              },
              {
                text: 'Quay lại',
                action: tour.back,
                secondary: true
              },
              {
                text: 'Tiếp tục',
                action: tour.next
              }
            ]
          },
          {
            attachTo: { element: '#tab-note', on: 'bottom'},
            text: 'Nơi bạn lên ý tưởng, lập kế hoạch viết, và lựa chọn từ vựng cho chủ đề này. Trước khi viết mỗi đề, hãy bắt đầu từ đây.',
            classes: 'intro-step',
            buttons: [
              {
                text: 'Bỏ qua',
                action: () => {
                  this.setCookie('tour', 'completed')
                  tour.complete()
                },
                secondary: true
              },
              {
                text: 'Quay lại',
                action: tour.back,
                secondary: true
              },
              {
                text: 'Tiếp tục',
                action: tour.next
              }
            ]
          },
          {
            attachTo: { element: '#tab-submissions', on: 'bottom'},
            text: 'Các bài viết đã nộp cho chủ đề này.',
            classes: 'intro-step',
            buttons: [
              {
                text: 'Quay lại',
                action: tour.back,
                secondary: true
              },
              {
                text: 'Hoàn tất',
                action: () => {
                  this.setCookie('tour', 'completed')
                  tour.complete()
                }
              }
            ]
          }
        ])
        tour.start()
      }
    },
    setCookie(name, value) {
      var expires = '; expires=Fri, 31 Dec 9999 23:59:59 GMT'
      document.cookie = name + '=' + (value || '') + expires + '; path=/'
    },
    getCookie(name) {
      var nameEQ = name + '='
      var ca = document.cookie.split(';')
      for (var i = 0; i < ca.length; i++) {
        var c = ca[i]
        while (c.charAt(0) === ' ') c = c.substring(1, c.length)
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length)
      }
      return null
    },
    eraseCookie(name) {
      document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;'
    },
    onNoteUpdate() {
      localStorage.setItem(this.currentUser.username + '_QuestionId' + this.questionId + '_Note', this.questionNote)
    },
    submit() {
      this.isLoading = true
      this.dialogVisible = false
      localStorage.removeItem(this.idLocalStorage)
      clearInterval(this.timeSpentInterval)
      var timeInSeconds
      if (this.isTesting) {
        timeInSeconds = (+this.getDataQuestion.time.slice(0, 2) - this.minute - 1) * 60 + 60 - this.second
      } else {
        timeInSeconds = moment().diff(this.timeStart, 'seconds')
      }
      if (!this.writingContent) {
        this.$notify.error({
          title: 'Bài viết để trống',
          message: 'Bạn hãy hoàn thiện bài viết',
          type: 'error',
          duration: 3000
        })
        return
      }
      if (this.timeSpent > 0) {
        timeInSeconds += this.timeSpent
      }
      var data = {
        filename: new Date().getFullYear().toString() + (new Date().getMonth() + 1).toString() + new Date().getDate().toString() + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString() + '.pdf',
        text: this.writingContent,
        userId: this.currentUser.id,
        questionId: +this.questionId,
        timeSpentInSeconds: timeInSeconds
      }
      this.timeSpent = 0
      this.timeStart = moment()

      // Create a new submission everytime user click Submit
      data.status = 'Submitted'
      documentService.submitDocument(data).then(rs => {
        this.isLoading = false
        if (rs) {
          this.hasSubmitionForThisQuestion = true
          this.submissionId = rs.submissions[0]?.id
          this.writingSubmitted = true

          if (this.freeToken > 0 || this.premiumToken > 0 || (this.userSubscription && new Date(this.userSubscription.endDate) > new Date())) {
            this.$nextTick(() => {
              // Hide View Review button if any
              this.hasReview = false
              // Allow editting right after submission
              this.isEdit = true
              // Update url for submission
              this.$router.push('/practice/' + this.questionId + '/' + this.submissionId)
              // Open the checkout dialog
              this.$refs.checkoutDialog?.openDialog()
            })
          } else {
            window.location.href = '/pricing'
          }
        }
      })
    },
    enableTestMode() {
      const time = this.getDataQuestion.time.slice(0, 2)
      this.isTesting = true
      this.minute = time
      this.second = 0
      this.timeSpentInterval = setInterval(() => {
        this.second--
        if (this.second < 0) {
          this.second = 59
          this.minute--
          if (this.minute < 0) {
            clearInterval(this.timeSpentInterval)
            this.isTesting = false
          }
        }
        if (this.minute == 0 && this.second == 0) {
          this.submit()
          this.isTesting = false
        }
      }, 1000)
    },
    pauseTest() {
      this.isTestingPaused = true
      clearInterval(this.timeSpentInterval)
    },
    resumeTest() {
      this.isTestingPaused = false
      this.timeSpentInterval = setInterval(() => {
        this.second--
        if (this.second < 0) {
          this.second = 59
          this.minute--
          if (this.minute < 0) {
            clearInterval(this.timeSpentInterval)
            this.isTesting = false
          }
        }
        if (this.minute == 0 && this.second == 0) {
          this.submit()
          this.isTesting = false
        }
      }, 1000)
    },
    cancelTest() {
      this.isTesting = false
      clearInterval(this.timeSpentInterval)
    },
    editSubmission() {
      this.isEdit = false
      this.writingSubmitted = false
      this.hasReview = false
    },
    viewFeedback() {
      const url = `/review/${this.questionId}/${this.docId}/${this.reviewId}`
      this.$router.push(url)
    },
    async loadData() {
      this.submissions = await questionService.getSubmissionsforQuestion(this.currentUser.id, this.questionId)
      console.log('Submissions: ', this.submissions)
      if (this.submissions && this.submissions.length > 0) {
        if (this.submissionId) {
          const thisSubmission = this.submissions.find(s => s.id == this.submissionId)
          console.log('This Submission: ', thisSubmission)
          if (thisSubmission) {
            this.timeSpent = this.timeSpent = thisSubmission.timeTaken
            this.writingContent = thisSubmission.text
            this.hasSubmitionForThisQuestion = true
            const reviewRequest = await reviewService.getReviewForSubmission(this.submissionId)
            console.log(reviewRequest)
            if (reviewRequest) {
              this.isFreeRequested = reviewRequest.feedbackType == 'Free'
              this.isProRequested = reviewRequest.feedbackType == 'Pro'
              if (reviewRequest.hasReview) {
                this.hasReview = true
                this.reviewId = reviewRequest.reviewId
                this.docId = thisSubmission.docId
              }
            }
          }
          if (localStorage.getItem(this.idSubmissionStorage) && localStorage.getItem(this.idSubmissionStorage) != '') {
            this.writingContent = localStorage.getItem(this.idSubmissionStorage)
          }
        } else {
          const savedSubmission = this.submissions.find(s => s.status == 'Saved')
          if (savedSubmission) {
            this.submissionId = savedSubmission.id
            this.timeSpent = savedSubmission.timeTaken
            this.writingContent = savedSubmission.text
            this.idSubmissionStorage = this.currentUser.username + '_QuestionId' + this.questionId + '_SubmissionId' + this.submissionId
            if (localStorage.getItem(this.idSubmissionStorage) && localStorage.getItem(this.idSubmissionStorage) != '') {
              this.writingContent = localStorage.getItem(this.idSubmissionStorage)
            }
          } else {
            this.writingContent = localStorage.getItem(this.idLocalStorage)
          }
        }
      } else {
        this.writingContent = localStorage.getItem(this.idLocalStorage)
      }

      if (this.writingContent == null || this.writingContent == 'null') this.writingContent = ''
      console.log('Writing Content: ', this.writingContent)
      this.countWords()
    },
    requestReview() {
      if (this.freeToken > 0 || this.premiumToken > 0 || (this.userSubscription && new Date(this.userSubscription.endDate) > new Date())) {
        this.$refs.checkoutDialog?.openDialog()
      } else {
        window.location.href = '/pricing'
      }
    },
    copyTextToClipboard(text) {
      console.log(text)
      navigator.clipboard.writeText(text)
      this.$notify.success({
        title: 'Success',
        message: 'Submission text was copied to the clipboard!',
        type: 'success',
        duration: 2000
      })
    },
    onTabRemove(tabName) {
      // Remove this tab
      var index = this.editableTabs.findIndex(t => t.name == tabName)
      if (index > -1) { // only splice array when item is found
        this.editableTabs.splice(index, 1) // 2nd parameter means remove one item only
      }
      // Select the active tab
      if (this.activeTab2 == tabName) {
        if (index == 0) {
          this.activeTab2 = 'submissions'
        } else {
          if (this.editableTabs.length > 0) {
            this.activeTab2 = this.editableTabs[index - 1].name
          }
        }
      }
    },
    onSubmissionRowClick(row, column, event) {
      row.name = 'submission' + row.id
      var thisTab = this.editableTabs.find(t => t.id == row.id)
      if (!thisTab) { this.editableTabs.push(row) }

      this.activeTab2 = 'submission' + row.id
    },
    calculateContainerHeight() {
      const headerHeight = document.getElementById('header').clientHeight
      const containerHeight = window.innerHeight - headerHeight
      const elContainer = document.getElementById('practiceWritingContainer')
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
    save() {
      if (!this.writingContent) {
        this.$notify.error({
          title: 'Không thể nộp bài',
          message: 'Bạn hãy hoàn thiện bài viết trước khi nộp',
          type: 'error',
          duration: 3000
        })
        return
      }

      var timeInSeconds
      if (this.isTesting) {
        timeInSeconds = (+this.getDataQuestion.time.slice(0, 2) - this.minute - 1) * 60 + 60 - this.second
      } else {
        timeInSeconds = moment().diff(this.timeStart, 'seconds')
      }

      if (this.timeSpent > 0) {
        timeInSeconds += this.timeSpent
      }

      var data = {
        filename: new Date().getFullYear().toString() + (new Date().getMonth() + 1).toString() + new Date().getDate().toString() + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString() + '.pdf',
        text: this.writingContent,
        userId: this.currentUser.id,
        questionId: +this.questionId,
        timeSpentInSeconds: timeInSeconds
      }

      if (this.submissionId) {
        data.status = 'Saved'
        documentService.updateDocumentBySubmissionId(this.submissionId, data).then(rs => {
          if (rs) {
            this.$notify.success({
              title: 'Đã lưu thành công',
              message: 'Bài viết đã được lưu thành công',
              type: 'success',
              duration: 3000
            })
          }
        })
      } else {
        data.status = 'Saved'
        documentService.saveDocument(data).then(rs => {
          if (rs) {
            this.$notify.success({
              title: 'Đã lưu thành công',
              message: 'Bài viết đã được lưu thành công',
              type: 'success',
              duration: 3000
            })
            this.submissionId = rs.submissions[0]?.id
          }
        })
      }
    },
    toggleBtnShowTab() {
      this.isShowTimer = true
      this.isShowReading = false
      this.timeSpent = 0
      this.isShowReading = false
      this.isShowTimer = false

      if (this.getListening != '') {
        this.isShowListeningTab = true
      }
      if (this.getChart != '') {
        this.isShowChart = true
      }
    },
    toggleDirection() {
      if (this.hideDirection == 'Hide') { this.hideDirection = 'Show' } else {
        this.hideDirection = 'Hide'
      }
    },
    backClick() {
      this.isShowListeningTab = false
      if (this.minute <= 0 && this.second == 59) {
        this.closeTimer = true
      }
      this.isShowReading = true
    },
    toggleBtnShowScript() {
      this.isShowScript = !this.isShowScript
    },
    toggleShowCount() {
      this.isShowCountWord = !this.isShowCountWord
    },
    countWords() {
      if (!this.isStart) {
        this.isStart = true
        this.timeStart = moment()
      }

      this.countWord = this.writingContent ? this.writingContent.trim().split(/\b\S+\b/).length - 1 : 0

      clearTimeout(this.timeout)
      this.timeout = setTimeout(() => {
        if (this.idSubmissionStorage) {
          localStorage.setItem(this.idSubmissionStorage, this.writingContent)
        }
        if (!this.$route.params.submissionId) {
          localStorage.setItem(this.idLocalStorage, this.writingContent)
        }
      }, 50)
    },
    showDiscussion(e) {
      if (e.label == 'Discussions') {
        this.tabDisCussionShowed = true
        this.$router.push('/practice/' + this.getDataQuestion.id + '/discuss').catch(() => {})
      } else {
        this.$router.push('/practice/' + this.getDataQuestion.id).catch(() => {})
        this.tabDisCussionShowed = false
      }
    },
    logSmt(e) {
      console.log(e)
    },
    onSubmissionChange(id) {
      this.$router.push(`/practice/${this.questionId}/${id}`)
      // const e = this.userSubmissions.find(r => r.reviewId === id)
      // this.$refs.ifReview.setAttribute('src', `http://localhost:3011/review-plain/${this.questionId}/${e.docId}/${e.reviewId}?plain=true`)
    },
    onCommentOrRubric(e) {
      console.log('Comment/Rubric', e)
      this.$refs.ifReview.contentWindow.postMessage(e, '*')
    },
    getTimeTaken(time) {
      var minutes = Math.floor(time / 60)
      var seconds = time - minutes * 60
      return minutes + ' min ' + seconds + ' sec '
    }
  }
}
</script>

<style>
.intro-step{
  margin-top: 12px;
}
.el-tabs__nav-next {
    right: 3px !important;
    top: -2px !important;
    font-size: 14px !important;
    color: #606060 !important;
}
.el-tabs__nav-prev {
    left: 3px !important;
    top: -2px !important;
    font-size: 14px !important;
    color: #606060 !important;
}
.el-tabs__item {
  padding: 0 15px !important;
  font-size: 13px !important;
}
.practice-tab-discussion .el-radio-button{
  margin-bottom: 0;
}
.hide-direction {
  font-weight: bold;
  color: #409EFF;
}
.hide-direction:hover{
  cursor: pointer;
}
.el-tabs--border-card {
  display: flex !important;
  flex-direction: column !important;
  height: 100% !important;
}

.el-tabs__content {
  flex-grow: 1 !important;
}

.el-tab-pane {
  height: 100%;
  min-height: 0 !important;
}
#practiceWritingCheckoutContainer .el-dialog .el-dialog__header{
  padding: 10px 0 0 0 !important;
  height: 60px;
}
#practiceWritingCheckoutContainer .el-dialog .el-dialog__body{
  padding: 0 !important;
}

#questionContent p {
  color: black;
  line-height: 1.5 !important;
}
#readingContent p {
  font-size: 14px;
  text-align: justify;
  white-space: break-spaces;
  font-family: inherit !important;
  margin-bottom: 0 !important;
  color: black;
}
#transcriptContent p {
  font-size: 14px;
  text-align: justify;
  white-space: break-spaces;
  font-family: inherit !important;
  margin-bottom: 0 !important;
  color: black;
}

.el-tiptap-editor .ProseMirror{
  height: 100%;
}
</style>
<style scoped>

.el-tabs--border-card>.el-tabs__header .el-tabs__item>p {
    color: #909399;
}

.el-tabs--border-card>.el-tabs__header .el-tabs__item.is-active>p {
    color: #409EFF;
}

.el-tabs--border-card>.el-tabs__header .el-tabs__item>p:hover {
    color: #409EFF;
}

.payment-method-title{
  display: flex;
  align-items: center;
}
.dialog-body{
  padding: 10px 0 10px 0;
  display: flex;
  align-items: center;
  border-bottom: solid 1px rgb(187, 187, 187);
}

.par-content {
  position: absolute;
  /* padding-right: 10px; */
  top: 0;
  left: 0;
  /* overflow-y: scroll; */
  height: 100%;
  width: 100%;
  -ms-overflow-style: none;
  /* IE and Edge */
  scrollbar-width: none;
}

/* .par-content::-webkit-scrollbar {
  width: 7px;
} */

/* Handle */
.par-content::-webkit-scrollbar-thumb {
  background: #999;
  border-radius: 4px;
}

/* Handle on hover */
.par-content::-webkit-scrollbar-thumb:hover {
  background: #777;
}

.padding-10 {
  padding: 0px;
}
.default{
  padding: 0;
  margin: 0;
}

.question-con {
  padding: 5px 10px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
  border: 1px solid #ebeef5;
  background-color: #fff;
  border-radius: 4px;
}

.header-passage {
  /* height: 40px; */
  margin-bottom: 10px;
  font-size: 13px;
  /* background-color: #f5f7fa; */
  /* border-bottom: 1px solid #e2e2e2; */
  /* padding: 5px; */
  min-width: 400px;
}

.body-passage {
  border-bottom: 2px solid #eff0f2;
  border-left: 2px solid #eff0f2;
  border-right: 2px solid #eff0f2;
}

.header-practice {
  font-size: 14px;
  font-weight: 600;
  color: rgb(23, 56, 82);
}

.body-practice {
  padding: 0;
}

.body-transcript {
  margin-top: 0;
  padding: 10px;
  border: 1px solid #e2e2e2;
}

.btn-start {
  background-color: #e8e8e8;
  width: 90px;
  height: 30px;
  padding: 0;
  border-radius: 0;
}

.footer-end {
  display: flex;
  justify-content: flex-end;
  padding: 0 20px;
  margin-bottom: 40px;
}

.title-tab {
  float: left;
  font-size: 18px;
  font-weight: 600;
  color: rgb(23, 56, 82);
}

.img-icon {
  height: 17px;
  width: 20px;
  padding-right: 5px;
}

.tip {
  padding: 5px 10px;
  font-size: 12px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
}

.info {
  padding: 5px 10px;
  font-size: 12px;
  background-color: #f4f4f5;
  border-radius: 4px;
  border-left: 5px solid #afafaf;
}

.script-select:hover {
  cursor: pointer;
  background-color: #f5f7fa;
}

.el-tab-pane {
  min-height: 650px;
}

.splitpanes--vertical>.splitpanes__splitter {
  width: 55px;
  background: linear-gradient(90deg, #ccc, #111);
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

.textarea-style {
  width: 100%;
  padding: 15px;
  border: 1px solid #e2e2e2;
  /* border-top: none; */
  resize: none;
  outline: none;
  height: 100%;
  border-radius: 5px;
}
.submited-message {
  width: calc(100% - 210px);
}

</style>
