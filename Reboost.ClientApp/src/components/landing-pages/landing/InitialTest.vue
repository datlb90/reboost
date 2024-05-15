<template>
  <!-- Start Boxes Area -->
  <section class="boxes-area" style="margin-top: 30px;">
    <div class="container">

      <div v-if="screenWidth > 992" id="initialTest" style="padding-top: 25px;">
        <div class="section-title">
          <h2>Luyện tập cùng Reboost</h2>
          <div class="bar" />
          <p style="max-width: 70%;">Reboost cung cấp miễn phí bộ chủ đề viết được cập nhật thường xuyên và đa dạng tài nguyên hữu dụng cho từng chủ đề bao gồm gợi ý từ vựng nâng band, hướng dẫn phân tích đề, và các bài viết mẫu.</p>
          <p style="max-width: 70%;">Hãy làm thử một đề phù hợp với bạn và nhận phản hồi ngay.</p>
        </div>

        <!-- <div style="text-align: center; margin-bottom: 20px; ">
          <el-radio-group id="radio-select-test" v-model="initTest" style="margin-top: 20px; margin-bottom: 25px; " text-color="#4b6f8a" @change="switchTest">
            <el-radio label="Đề Task 2" border />
            <el-radio label="Đề Task 1" border />
          </el-radio-group>
        </div> -->
        <splitpanes class="default-theme" vertical style="height: 800px; width: 100%; -webkit-box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06); box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06);">
          <pane>
            <el-tabs v-model="activeTab" type="border-card" style="height: 100%;" @tab-click="showDiscussion" @tab-remove="onTabRemove">
              <el-tab-pane name="description">
                <p slot="label" style="font-size: 14px; line-height: 2.6;">Đề bài</p>
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
              <el-tab-pane label="Ý tưởng" name="idea" style="height: 100%; position: relative;">
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
                        instructions on how to structure your eassy, and useful vocabulary that could be used to improve your band score.</div>
                    </el-card>
                  </div>
                </div>
              </el-tab-pane>
              <el-tab-pane label="Bài viết mẫu" name="sample" style="height: 100%; position: relative;">
                <p slot="label" style="font-size: 14px; line-height: 2.6;">Bài viết mẫu</p>
                <div>
                  <div>
                    <div v-if="getSamples && getSamples.length > 0">
                      <div v-for="(item, index) in getSamples" :key="item.id" style=" margin-bottom: 20px;">
                        <el-card class="box-card sample-box">
                          <div style="height: 50px; border-bottom: 1px solid rgb(220 223 229); margin-bottom: 20px; ">
                            <div style="float: left; margin-top: 5px;">
                              <span style="font-size: 16px; font-weight: 600;">Sample Response #{{ index+1 }}</span>
                            </div>
                            <div style="float: right;">
                              <el-tag>Band Score: {{ item.bandScore ? item.bandScore.toFixed(1) : "Not Available" }}</el-tag>
                            </div>
                          </div>
                          <div class="box-card__content">
                            <div>
                              <div class="sample-container" v-html="item.sampleText" />
                            </div>
                            <div v-if="item.comment" style="border-top: 1px solid rgb(220 223 229); margin-top: 20px; padding-top: 20px;">
                              <div class="sample-container" v-html="item.comment" />
                            </div>

                          </div>
                        </el-card>
                      </div>
                    </div>
                    <div v-else>
                      <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                        <div> We are working on collecting samples for this writing topic, please check back soon.</div>
                      </el-card>
                    </div>
                  </div>
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
                        v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                        size="mini"
                        :disabled="!(writingContent && writingContent.length > 0)"
                        :loading="isLoading"
                        type="primary"
                        style="float: right; margin-left: 5px;"
                        @click="submit()"
                      >Nộp bài</el-button>
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
                      style="height: calc(715px); font-size: 14px;"
                      @onUpdate="onNoteUpdate()"
                    />
                  </div>
                </div>
              </el-tab-pane>
            </el-tabs>
          </pane>
        </splitpanes>
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

      <div v-else id="initialTest">
        <div class="section-title" style="margin-bottom: 30px;  padding-top: 10px;">
          <div class="bar" />
        </div>
        <div style="text-align: center;">
          <h5>Làm đề thi thử và nhận phản hồi ngay</h5>
          <el-radio-group id="radio-select-test" v-model="initTest" style="margin-top: 10px; margin-bottom: 15px; " text-color="#4b6f8a" @change="switchTest">
            <el-radio label="Đề Task 2" border />
            <el-radio label="Đề Task 1" border />
          </el-radio-group>
        </div>
        <div style="width: 100%;">
          <div id="tabs-wrapper">
            <el-tabs v-model="activeTab" type="border-card" style="margin-bottom: 10px;" @tab-click="showDiscussion" @tab-remove="onTabRemove">
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
                        instructions on how to structure your eassy.</div>
                    </el-card>
                  </div>
                </div>
              </el-tab-pane>

              <el-tab-pane label="Bài viết mẫu" name="sample" style="height: 100%; position: relative;">
                <div>
                  <div>
                    <div v-if="getSamples && getSamples.length > 0">
                      <div v-for="(item, index) in getSamples" :key="item.id">
                        <el-card class="box-card sample-box">
                          <div style="height: 50px; border-bottom: 1px solid rgb(220 223 229); margin-bottom: 20px; ">
                            <div style="float: left; margin-top: 5px;">
                              <span style="font-size: 16px; font-weight: 600;">Sample Response #{{ index+1 }}</span>
                            </div>
                            <div style="float: right;">
                              <el-tag>Band Score: {{ item.bandScore ? item.bandScore.toFixed(1) : "Not Available" }}</el-tag>
                            </div>
                          </div>
                          <div class="box-card__content">
                            <div>
                              <div class="sample-container" v-html="item.sampleText" />
                            </div>
                            <div v-if="item.comment" style="border-top: 1px solid rgb(220 223 229); margin-top: 20px; padding-top: 20px;">
                              <div class="sample-container" v-html="item.comment" />
                            </div>

                          </div>
                        </el-card>
                      </div>
                    </div>
                    <div v-else>
                      <el-card class="box-card" style="font-size: 14px; padding: 20px;">
                        <div> We are working on collecting samples for this writing topic, please check back soon.</div>
                      </el-card>
                    </div>
                  </div>
                </div>
              </el-tab-pane>
            </el-tabs>
          </div>

          <div style="height: 600px; display: flex; flex-direction: column;">
            <el-tabs v-model="activeTab2" type="border-card" style="height: 100%;" @tab-remove="onTabRemove">
              <el-tab-pane name="essay">
                <p slot="label" style="font-size: 14px; line-height: 2.6;">Bài làm</p>
                <div style="height: 100%; display: flex; flex-direction: column;">
                  <div class="header-passage" style="min-width: auto;">
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
                        v-if="!writingSubmitted && !hasSubmitionForThisQuestion && !isEdit"
                        size="mini"
                        :disabled="!(writingContent && writingContent.length > 0)"
                        :loading="isLoading"
                        type="primary"
                        style="float: right; margin-left: 5px;"
                        @click="submit()"
                      >Nộp bài</el-button>
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
                      style="height: 540px; font-size: 14px;"
                      @onUpdate="onNoteUpdate()"
                    />
                  </div>
                </div>
              </el-tab-pane>
            </el-tabs>
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
    </div>
  </section>
  <!-- End Boxes Area -->
</template>

<script>
import documentService from '@/services/document.service'
import authService from '@/services/auth.service'
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
// import moment from 'moment-timezone'
export default {
  name: 'InitialTest',
  components: {
    'splitpanes': Splitpanes,
    'pane': Pane
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
      questionId: null,
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
      unRatedList: [],
      isFreeRequested: false,
      isProRequested: false,
      showStartTestButton: false,
      isTesting: false,
      dialogVisible: false,
      timeStart: null,
      idSubmissionStorage: null,
      isEdit: false,
      isShowQuestion: true,
      submissions: [],
      editableTabs: [],
      activeTab: 'description',
      screenWidth: window.innerWidth,
      initTest: 'Đề Task 2',
      listExpand: [],
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
        // new LineHeight()
      ]
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
    getSamples() {
        return this.$store.getters['question/getSelected']['samples']
    },
    getRubric() {
        return this.$store.getters['question/getSelected']['rubrics']
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
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    window.component = this
    this.$store.dispatch('question/loadQuestionsForInitialTest').then(rs => {
      this.loadCompleted = true
      this.questionId = this.$store.getters['question/getSelected'].id
      this.questionNote = localStorage.getItem('Init_QuestionId' + this.questionId + '_Note')
      this.idSubmissionStorage = 'initialTestSubmission'
      this.loadData()
    })
  },
  destroyed() {
    clearInterval(this.setIntervalForScroll)
    clearInterval(this.timeSpentInterval)
  },
  methods: {
    onNoteUpdate() {
      localStorage.setItem('Init_QuestionId' + this.questionId + '_Note', this.questionNote)
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
    toggleExpand(e) {
      if (!this.listExpand.includes(e)) {
        this.listExpand.push(e)
      } else {
        this.listExpand.splice(this.listExpand.indexOf(e), 1)
      }
    },
    switchTest() {
        this.$store.dispatch('question/switchTest', this.initTest)
        this.activeTab = 'description'
        this.questionId = this.$store.getters['question/getSelected'].id
        this.questionNote = localStorage.getItem('Init_QuestionId' + this.questionId + '_Note')
    },
    async loadData() {
        if (localStorage.getItem(this.idSubmissionStorage) && localStorage.getItem(this.idSubmissionStorage) != '') {
            this.writingContent = localStorage.getItem(this.idSubmissionStorage)
        }
        if (this.writingContent == null || this.writingContent == 'null') this.writingContent = ''
        console.log('Writing Content: ', this.writingContent)
        this.countWords()
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
    submit() {
      this.isLoading = true
      this.dialogVisible = false
      clearInterval(this.timeSpentInterval)

      if (!this.writingContent) {
        this.$notify.error({
          title: 'Đã xảy ra lỗi',
          message: 'Hãy hoàn thiện bài viết trước khi nộp bài',
          type: 'error',
          duration: 2000
        })
        return
      }

      var submissionData = {
        filename: new Date().getFullYear().toString() + (new Date().getMonth() + 1).toString() + new Date().getDate().toString() + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString() + '.pdf',
        text: this.writingContent,
        userId: null,
        questionId: this.questionId,
        timeSpentInSeconds: 0,
        test: this.getDataQuestion.test
      }
      // clear personal question on initial test submission
      this.$store.dispatch('question/clearPersonalQuestion')
      // save lại submission data in storage và chuyển qua trang đăng ký
      this.$store.dispatch('question/saveInitialTestData', submissionData).then(rs => {
        console.log('submission saved')
        if (!authService.isAuthenticated()) {
          this.isLoading = false
          // Redirect to the register page for authentication
          return this.$router.push({ path: '/register' })
          // Sau khi đăng ký xong uddate submission với user id
          // Chạy ai review và show feedback cho người dùng
        } else {
          this.isLoading = false
          // User should never get here
          // Prevent data lost
          console.log('create submission')
          this.currentUser = this.$store.getters['auth/getUser']
          submissionData.userId = this.currentUser.id
          documentService.submitDocument(submissionData).then(rs => {
            if (rs) {
              this.$notify.success({
                  title: 'Success',
                  message: 'Bài viết của bạn đã được nộp thành công',
                  type: 'success',
                  duration: 3000
              })
              return this.$router.push({ path: '/submissions' })
            }
          })
        }
      })
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
      this.countWord = this.writingContent ? this.writingContent.trim().split(/\b\S+\b/).length - 1 : 0

      // Save writing content in localstorage
      clearTimeout(this.timeout)
      this.timeout = setTimeout(() => {
        if (this.idSubmissionStorage) {
          localStorage.setItem(this.idSubmissionStorage, this.writingContent)
        }
      }, 50)
    },
    showDiscussion(e) {
    //   if (e.label == 'Discussions') {
    //     this.tabDisCussionShowed = true
    //     this.$router.push('/practice/' + this.getDataQuestion.id + '/discuss').catch(() => {})
    //   } else {
    //     this.$router.push('/practice/' + this.getDataQuestion.id).catch(() => {})
    //     this.tabDisCussionShowed = false
    //   }
    },
    logSmt(e) {
      console.log(e)
    },
    startTest() {
      // const time = +this.getDataQuestion.time.slice(0, 2)
      this.isTesting = true
      this.minute = 39 // time
      if (this.initTest == 'Đề Task 1') { this.minute = 19 }
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
          this.submit()
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
#tipContent p {
  line-height: 1.5;
  color: rgb(13, 13, 13);
  margin-bottom: 6px;
}

#tipContent span {
  color: rgb(13, 13, 13);
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

