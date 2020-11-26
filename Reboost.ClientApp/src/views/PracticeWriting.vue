<template>
<el-row>
  <el-col :span="18" :offset="3" class="padding-10">
    <el-row>
      <el-col :span="24" class="padding-10">
        <el-tabs type="border-card">
          <el-tab-pane label="Topic" style="padding: 0 5px;">
            <el-row>
              <el-col style="padding: 0 10px; ">
                <div style="display: flex; justify-content: space-between;">
                  <div>
                    <div class="title-tab">1. Question Title</div>
                    <div style="margin-top: 5px; display: flex;">
                      <div style="font-size: 15px; color: #44ce6f; margin-right: 25px;">{{getDataQuestion.test}} {{getDataQuestion.title}}</div>
                      <div style="display: flex;  margin-right: 20px;">
                        <img class="img-icon" src="../assets/img/like.png" alt="">
                        <label for="" style="font-weight: 200; font-size: 13px;">{{getDataQuestion.like}}</label>
                      </div>
                      <div style="display: flex;  margin-right: 20px;">
                        <img class="img-icon" src="../assets/img/dislike.png" alt="">
                        <label for="" style="font-weight: 200; font-size: 13px;">{{getDataQuestion.dislike}}</label>
                      </div>
                      <div style="display: flex;  margin-right: 20px;">
                        <img class="img-icon" src="../assets/img/heart.png" alt="">
                        <label for="" style="font-weight: 200; font-size: 13px;">Add to List</label>
                      </div>
                      <div style="display: flex;  margin-right: 20px;">
                        <img class="img-icon" src="../assets/img/share.png" alt="">
                        <label for="" style="font-weight: 200; font-size: 13px;">Share</label>
                      </div>
                    </div>
                  </div>
                  <div style="display: flex; align-items: center;">
                    <el-button type="success" @click="test()">SUBMIT</el-button>
                  </div>
                </div>

                <hr style="margin: 20px 0;">
                <div class="tip">
                  <p style="line-height: 21px;"> <span style="font-weight: 600; font-size: 13px;">Direction: </span> Please use your best guess if you don't already have one</p>
                </div>
              </el-col>
            </el-row>
            <el-row>
              <el-col v-if="!isShowListeningTab && getReading != ''" :span="12" class="padding-10">
                <div style="width: 100%;">
                  <div class="header-passage">
                    READ A SHORT PASSAGE
                  </div>
                  <div class="body-passage">
                    <div class="body-practice">
                      <div class="tip" style="display: flex; align-items: center; justify-content: space-between;">
                        <div>
                          <p style="line-height: 21px;"> <span style="font-weight: 600; font-size: 13px;">Direction: </span> Please use your best guess if you don't already have one</p>
                        </div>
                        <div style="margin-left: 14px;" v-if="getListenting != ''">
                          <el-button size="medium" class="btn-start" @click="toggleBtnShowTab()">START</el-button>
                        </div>
                      </div>
                    </div>
                    <div class="body-practice" v-if="getReading != ''">
                      <p style="line-height: 21px; text-align: justify;"> {{ getReading.content }}</p>
                    </div>
                  </div>
                </div>
              </el-col>
              <el-col v-if="col == 12 && getReading =='' && getListenting != '' || isShowListeningTab" :span="12" class="padding-10">
                <div style="width: 100%;">
                  <div class="header-passage" style="display:flex; justify-content: space-between;">
                    LISTEN TO PART OF LECTURE ON THE SAME TOPIC
                    <div v-if="getReading != ''">
                      <el-button size="medium" class="btn-start" @click="toggleBtnShowTab()">Back</el-button>
                    </div>
                  </div>
                  <div class="body-passage">
                    <div class="body-practice" v-if="getListenting != ''">
                      <div>
                        <audio controls style="width: 100%;">
                          <source :src="'/assets/' + getListenting.content" type="audio/mpeg">
                          Your browser does not support the audio element.
                        </audio>
                      </div>
                    </div>
                    <div style="padding: 0 20px;">
                      <div class="script-select" style="padding: 5px 20px; border: 2px solid #eff0f2;" @click="toggleBtnShowScript">
                        <i class="el-icon-document-copy"></i>
                        Audio Script
                      </div>
                    </div>
                    <div v-if="!isShowScript" style="margin-bottom: 20px;"></div>
                    <div class="body-practice" v-if="isShowScript">
                      <div class="body-transcript">
                        <p style="line-height: 21px; text-align: justify;"> {{ getTranscript.content }}</p>
                      </div>
                    </div>
                  </div>
                </div>
              </el-col>
              <el-col :span="col" class="padding-10" style="height: 100%;">
                <el-row style="margin-bottom: 15px;">
                  <el-col :span="24" class="question-con">
                    <span style="font-weight: bold;">Question:</span>
                    {{ getQuestion.content }}
                  </el-col>
                </el-row>
                <el-row>
                  <el-col>
                    <div style="height: 100%">
                      <editor-text api-key="biyg6d0lkbqha5om2iu2q5i4qq0ecq58nlrn5n2wd9b5vtuh" style="height: 800px" :init="initTextArea" />
                    </div>
                  </el-col>
                </el-row>
              </el-col>
            </el-row>
          </el-tab-pane>
          <el-tab-pane label="Samples"></el-tab-pane>
          <el-tab-pane label="Discussions">Discussions</el-tab-pane>
          <el-tab-pane label="Similiar">Similiar</el-tab-pane>
        </el-tabs>
      </el-col>
    </el-row>
  </el-col>
</el-row>
</template>

<script>
// @ is an alias to /src
// import http from '@/utils/axios'
import Editor from '@tinymce/tinymce-vue'
export default {
  name: 'PracticeWriting',
  components: {
    'editor-text': Editor
  },
  data() {
    return {
      textarea: '',
      initTextArea: {
        height: 400,
        selector: 'textarea', // change this value according to your HTML
        plugins: 'wordcount',
        toolbar: '',
        content_style: "body { line-height: 5px; font-size: 15px; }",
        menubar: false,
        branding: false
      },
      isShowListeningTab: false,
      isShowScript: false
    }
  },
  methods: {
    test() {},
    toggleBtnShowTab() {
      this.isShowListeningTab = !this.isShowListeningTab;
    },
    toggleBtnShowScript(){
      this.isShowScript = ! this.isShowScript;
    }
  },
  mounted() {
    this.$store.dispatch('question/loadQuestion', +this.$route.params.id);
  },
  computed: {
    getDataQuestion() {
      var data = this.$store.getters['question/getSelected'];
      return data;
    },
    getDataQuestionParts() {
      return this.$store.getters['question/getSelected']['questionsPart']
    },
    getReading() {
      if (typeof (this.getDataQuestionParts) != "undefined")
        if (this.getDataQuestionParts.find(u => u.name == "Reading"))
          return this.getDataQuestionParts.find(u => u.name == "Reading")
      return "";
    },
    getListenting() {
      if (typeof (this.getDataQuestionParts) != "undefined")
        if (this.getDataQuestionParts.find(u => u.name == "Listening")) {
          return this.getDataQuestionParts.find(u => u.name == "Listening")
        }
      return "";
    },
    getQuestion() {
      if (typeof (this.getDataQuestionParts) != "undefined")
        if (this.getDataQuestionParts.find(u => u.name == "Question"))
          return this.getDataQuestionParts.find(u => u.name == "Question")
      return "";
    },
    col() {
      if (typeof (this.getDataQuestionParts) != "undefined") {
        if (!this.getDataQuestionParts.find(u => u.name == "Reading" || u.name == "Listening")) {
          return 24;
        }
      }
      return 12;
    },
    // getChart(){
    //   return this.getDataQuestionParts.find(u => u.name == "Chart")
    // },
    getTranscript() {
      if (typeof (this.getDataQuestionParts) != "undefined")
        if (this.getDataQuestionParts.find(u => u.name == "Transcript"))
          return this.getDataQuestionParts.find(u => u.name == "Transcript")
      return "";
    }
  }
}
</script>

<style scoped>
.padding-10 {
  padding: 10px;
}

.question-con {
  padding: 10px 30px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
  border: 1px solid #ebeef5;
  background-color: #fff;
  border-radius: 4px;
  font-size: 12px;
  font-style: italic;
}

.header-passage {
  padding: 0 20px;
  min-height: 40px;
  line-height: 37px;
  font-size: 13px;
  background-color: #f5f7fa;
  border: 2px solid #eff0f2;
}

.body-passage {
  border-bottom: 2px solid #eff0f2;
  border-left: 2px solid #eff0f2;
  border-right: 2px solid #eff0f2;
}

.body-practice {
  padding: 15px 20px;
}

.body-transcript {
  padding: 20px;
  border: 2px solid #eff0f2;
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
  font-size: 21px;
  font-weight: 600;
  color: rgb(23, 56, 82);
}

.img-icon {
  height: 17px;
  width: 20px;
  padding-right: 5px;
}

.tip {
  padding: 5px 14px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
}

.script-select:hover{
  cursor: pointer;
  background-color: #f5f7fa;
}
</style>
