<template>
  <div v-if="screenWidth > 780" id="practiceWritingContainer" :style="{ height: containerHeight, visibility: loadCompleted?'visible': 'hidden' }">
    <splitpanes class="default-theme" vertical style="height: 100%; width: 100%;">
      <pane>
        <el-tabs v-model="activeTab" type="border-card" style="height: 100%;" @tab-click="showDiscussion" @tab-remove="onTabRemove">
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
                          <!-- <div style="float: right;">
                            <div>

                              <el-checkbox v-model="isTest" size="mini" border style="margin-bottom: 0px;" @change="changedOption()"> Test Mode</el-checkbox>
                              <el-tag v-if="isTesting" class="mr-2" type="danger" size="medium" style="height: 30px; line-height: 28px;"><i class="el-icon-timer" style="font-size: 14px; margin-right: 4px;" />{{ minute }} : {{ second }}</el-tag>
                              <el-button v-if="showStartTestButton && !isTesting" class="mr-2" size="mini" style="padding: 8px 15px;" @click="startTest()">Start Test</el-button>
                            </div>
                          </div> -->
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
                    <div class="info" style="margin-top: 5px; font-size: 14px;" v-html="getDataQuestion.direction" />
                  </el-col>
                </el-row>

              </div>
              <div>

                <div>
                  <el-row style="margin-bottom: 8px;">
                    <div id="questionContent" class="tip" style="font-size: 14px;" v-html="getQuestion.content" />
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
          <el-tab-pane label="Ý tưởng & từ vựng" name="help" style="height: 100%; position: relative;">
            <div class="par-content">
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
          <el-tab-pane label="Bài mẫu" name="sample" style="height: 100%; position: relative;">
            <div class="par-content">
              <tab-samples :question-id="questionId" />
            </div>
          </el-tab-pane>
          <el-tab-pane label="Tiêu chí chuẩn" name="rubric" style="height: 100%; position: relative;">
            <div class="par-content">
              <tab-rubric :question-id="questionId" />
            </div>
          </el-tab-pane>
          <el-tab-pane label="Bài đã nộp" name="submissions" style="height: 100%; position: relative;">
            <div class="par-content" style="padding-right: 10px;">
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
                    Submitted at {{ item.submittedTimeStr }}
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
                      Time Taken: {{ getTimeTaken(item.timeTaken) }}
                    </el-tag>
                  </div>
                  <div style="float: left; margin-right: 5px;">
                    <el-tag
                      type="info"
                      size="medium"
                      effect="plain"
                    >
                      Word count: {{ item.text ? item.text.trim().split(/\b\S+\b/).length - 1 : 0 }}
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
      <pane v-if="!tabDisCussionShowed">
        <div style="height: 100%; display: flex; flex-direction: column;">
          <div class="header-passage">
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
            >Yêu cầu chấm bài</el-button>
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
                Word count: {{ countWord }}
              </el-tag>

              <el-button v-if="!isShowCountWord" size="mini" plain @click="toggleShowCount()">
                Show Word Count
              </el-button>
            </div>
            <div v-if="getQuestion != ''">
              <el-button
                v-if="!writingSubmitted && hasSubmitionForThisQuestion && !isEdit"
                style="float: right; margin-left: 5px"
                size="mini"
                :disabled="!(writingContent && writingContent.length > 0)"
                type="primary"
                @click="submit()"
              >Nộp bài</el-button>
              <el-button
                v-if="isEdit && !isFreeRequested && !isProRequested"
                style="float: right; margin-left: 5px"
                size="mini"
                @click="isEdit=false"
              >Sửa</el-button>
              <el-button
                v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                size="mini"
                :disabled="!(writingContent && writingContent.length > 0)"
                type="primary"
                style="float: right; margin-left: 5px;"
                @click="submit()"
              >Nộp bài</el-button>
              <el-button
                v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                size="mini"
                :disabled="!(writingContent && writingContent.length > 0)"
                style="float: right;"
                @click="save()"
              >Lưu lại</el-button>
            </div>
          </div>
          <div style="flex-grow: 1;">
            <textarea v-model="writingContent" :disabled="isEdit || writingSubmitted" placeholder="Bắt đầu bài viết của bạn ở đây ..." spellcheck="false" class="textarea-style" @keyup="countWords()" />
          </div>
        </div>
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
  </div>
  <div v-else id="practiceWritingContainer">
    <div style="width: 100%; margin-top: 55px; height: calc(100vh - 60px); overflow: auto;">
      <div id="tabs-wrapper">
        <el-tabs v-model="activeTab" type="border-card" style="margin-bottom: 10px;" @tab-click="showDiscussion" @tab-remove="onTabRemove">
          <el-tab-pane label="Description" name="description">
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
                          <!-- <div style="float: right;">
                            <div>
                              <el-tag v-if="isTesting" class="mr-2" type="danger" size="medium" style="height: 30px; line-height: 28px;"><i class="el-icon-timer" style="font-size: 14px; margin-right: 4px;" />{{ minute }} : {{ second }}</el-tag>
                              <el-button v-if="showStartTestButton && !isTesting" class="mr-2" size="mini" style="padding: 8px 15px;" @click="startTest()">Start Test</el-button>
                              <el-checkbox v-model="isTest" size="mini" border style="height: 30px; margin-bottom: 0px;" @change="changedOption()"> Test Mode</el-checkbox>
                            </div>
                          </div> -->
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
                    <div class="info" style="margin-top: 5px; font-size: 14px;" v-html="getDataQuestion.direction" />
                  </el-col>
                </el-row>

              </div>
              <div>

                <div>
                  <el-row style="margin-bottom: 8px;">
                    <div id="questionContent" class="tip" style="font-size: 14px;" v-html="getQuestion.content" />
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
          <el-tab-pane label="Help" name="help" style="height: 100%; position: relative;">
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
          <el-tab-pane label="Samples" name="sample" style="height: 100%; position: relative;">
            <div>
              <tab-samples :question-id="questionId" />
            </div>
          </el-tab-pane>
          <el-tab-pane label="Rubric" name="rubric" style="height: 100%; position: relative;">
            <div>
              <tab-rubric :question-id="questionId" />
            </div>
          </el-tab-pane>
          <el-tab-pane label="Submissions" name="submissions" style="height: 100%; position: relative;">
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
                    Submitted at {{ item.submittedTimeStr }}
                  </div>
                  <div style="float: right;">
                    <el-tag size="medium">{{ item.status }}</el-tag>
                  </div>
                </div>
                <div style="height: 30px;">
                  <div style="float: left; margin-right: 5px;">
                    <el-tag
                      type="info"
                      size="medium"
                      effect="plain"
                    >
                      Time Taken: {{ getTimeTaken(item.timeTaken) }}
                    </el-tag>
                  </div>
                  <div style="float: left; margin-right: 5px;">
                    <el-tag
                      type="info"
                      size="medium"
                      effect="plain"
                    >
                      Word count: {{ item.text ? item.text.trim().split(/\b\S+\b/).length - 1 : 0 }}
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

      <div style="height: 100%; display: flex; flex-direction: column;">
        <div style="height: 40px;  font-size: 13px; background-color: #f5f7fa;  border: 1px solid #e2e2e2; padding: 5px;">
          <el-button
            v-if="hasReview && docId && reviewId"
            style="float: left; margin-right: 5px"
            size="mini"
            type="primary"
            plain
            @click="viewFeedback()"
          >Xem phản hồi
          </el-button>
          <el-button
            v-if="((submissionId && isEdit) || writingSubmitted) && !isProRequested && !hasReview"
            style="float: left; margin-right: 5px"
            size="mini"
            type="primary"
            plain
            @click="requestReview()"
          >Yêu cầu chấm bài
          </el-button>
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
              Word count: {{ countWord }}
            </el-tag>

            <el-button v-if="!isShowCountWord" size="mini" plain @click="toggleShowCount()">
              Show Word Count
            </el-button>
          </div>
          <div v-if="getQuestion != ''">
            <el-button
              v-if="!writingSubmitted && hasSubmitionForThisQuestion && !isEdit"
              style="float: right; margin-left: 5px"
              size="mini"
              :disabled="!(writingContent && writingContent.length > 0)"
              type="primary"
              @click="submit()"
            >Submit</el-button>
            <el-button
              v-if="isEdit && !isFreeRequested && !isProRequested"
              style="float: right; margin-left: 5px"
              size="mini"
              @click="isEdit=false"
            >Edit</el-button>
            <el-button
              v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
              size="mini"
              :disabled="!(writingContent && writingContent.length > 0)"
              type="primary"
              style="float: right; margin-left: 5px;"
              @click="submit()"
            >Submit</el-button>
            <el-button
              v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
              size="mini"
              :disabled="!(writingContent && writingContent.length > 0)"
              style="float: right;"
              @click="save()"
            >Save</el-button>
          </div>
        </div>
        <div style="flex-grow: 1;">
          <textarea v-model="writingContent" :disabled="isEdit || writingSubmitted" placeholder="Bắt đầu bài viết của bạn ở đây ..." spellcheck="false" class="textarea-style" @keyup="countWords()" />
        </div>
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
import TabRubric from '../learner/PracticeWriting_TabRubric.vue'
import TabSamples from '../learner/PracticeWriting_TabSamples.vue'
import CheckOut from '../../components/controls/CheckOut.vue'
import {
  Splitpanes,
  Pane
} from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
import moment from 'moment-timezone'
export default {
  name: 'PracticeWriting',
  components: {
    'splitpanes': Splitpanes,
    'pane': Pane,
    'tab-rubric': TabRubric,
    'tab-samples': TabSamples,
    'checkout': CheckOut
  },
  data() {
    return {
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
      reviewId: null
    }
  },
  computed: {
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
    this.loadData()
  },
  destroyed() {
    clearInterval(this.setIntervalForScroll)
    clearInterval(this.timeSpentInterval)
  },
  methods: {
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
      this.$refs.checkoutDialog?.openDialog()
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
      if (this.activeTab == tabName) {
        if (index == 0) {
          this.activeTab = 'submissions'
        } else {
          if (this.editableTabs.length > 0) {
            this.activeTab = this.editableTabs[index - 1].name
          }
        }
      }
    },
    onSubmissionRowClick(row, column, event) {
      row.name = 'submission' + row.id
      var thisTab = this.editableTabs.find(t => t.id == row.id)
      if (!thisTab) { this.editableTabs.push(row) }

      this.activeTab = 'submission' + row.id
      console.log(this.activeTab)
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
          title: 'Error',
          message: 'Nothing to submit',
          type: 'error',
          duration: 1000
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
              title: 'Success',
              message: 'Saved successfully',
              type: 'success',
              duration: 2000
            })
          }
        })
      } else {
        data.status = 'Saved'
        documentService.saveDocument(data).then(rs => {
          if (rs) {
            this.$notify.success({
              title: 'Success',
              message: 'Saved successfully',
              type: 'success',
              duration: 2000
            })
            this.submissionId = rs.submissions[0]?.id
          }
        })
      }
    },
    submit() {
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
          title: 'Error',
          message: 'Nothing to submit',
          type: 'error',
          duration: 1000
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
      // this.isEdit = true // Edit button should not be visible after submitting?
      if (this.submissionId) {
        data.status = 'Submitted'
        documentService.updateDocumentBySubmissionId(this.submissionId, data).then(rs => {
          if (rs) {
            this.writingSubmitted = true
            this.$refs.checkoutDialog?.openDialog()
          }
        })
      } else {
        data.status = 'Submitted'
        documentService.submitDocument(data).then(rs => {
          if (rs) {
            this.hasSubmitionForThisQuestion = true
            this.submissionId = rs.submissions[0]?.id
            this.writingSubmitted = true
            this.$nextTick(() => {
              this.$refs.checkoutDialog?.openDialog()
            })
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
    changedOption() {
      if (this.isTest) {
        const time = +this.getDataQuestion.time.slice(0, 2)
        this.minute = time
        this.second = 0
      } else {
        this.isTesting = false
        clearInterval(this.timeSpentInterval)
      }
      // this.isShowQuestion = !this.isShowQuestion
      this.showStartTestButton = !this.showStartTestButton
      // if (!this.isTest) {
      //   this.isShowReading = true
      //   this.minute = 0
      //   this.second = 3
      //   this.closeTimer = false
      //   this.isShowScript = false
      //   this.isShowTimer = false
      // } else {
      //   if (!this.closeTimer) {
      //     this.isShowReading = false
      //   }
      // }
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
    startTest() {
      // const time = +this.getDataQuestion.time.slice(0, 2)
      this.isTesting = true
      this.minute = 19 // time
      this.second = 59
      // this.isShowQuestion = true

      this.timeSpentInterval = setInterval(() => {
        this.second--
        if (this.second < 0) {
          this.second = 59
          this.minute--
          if (this.minute < 0) {
            clearInterval(this.timeSpentInterval)
            this.isTesting = false
            this.dialogVisible = true
          }
        }

        if (this.minute == 0 && this.second == 0) {
          console.log('Submitted!')
        }
      }, 1000)
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
</style>
<style scoped>

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
  height: 40px;
  font-size: 13px;
  background-color: #f5f7fa;
  border: 1px solid #e2e2e2;
  padding: 5px;
  min-width: 500px;
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
  border-top: none;
  resize: none;
  outline: none;
  height: 100%;
}
.submited-message {
  width: calc(100% - 210px);
}

</style>
