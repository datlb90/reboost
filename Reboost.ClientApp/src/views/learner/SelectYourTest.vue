<template>
  <el-row :style="{visibility: loadCompleted?'visible':'hidden'}">
    <el-col :span="18" :offset="3" class="padding-10" style="min-width: 480px;">
      <el-row>
        <el-col style="padding: 0 10px; margin-top: 20px;">
          <div style="font-size: 25px; font-weight: bold; color: #173852">Select Your Tests</div>
          <el-divider style="margin-top: 5px;" />
        </el-col>
      </el-row>
      <el-form ref="formData" :model="formData" style="width:90%;">
        <el-row>
          <el-col :span="12" class="padding-10" style="padding-right: 15px;">
            <el-row style="margin-bottom: 25px;">
              <el-col :span="24">
                <div class="image-ielts" :class="{'question-con': selectIELTS}" style="height: 225px; padding: 0; border: 1px solid #ebeef5;" @click="selectScoreIELTS()">
                  <img src="@/assets/img/LogoIELTS.png" alt="" style="height:100%; width: 100%;">
                </div>
              </el-col>
            </el-row>

            <div v-if="selectIELTS" style="margin-bottom: 25px;">
              <el-col :span="24" class="select-score-ielts question-con">
                <div style="margin-bottom:10px">
                  <span style="font-style:normal ;font-size: 15px; margin: 0 5px 5px;">Select You Current IELTS Writing Score</span>
                  <el-popover
                    placement="top-start"
                    title="Tips"
                    width="500"
                    trigger="hover"
                    content="Please use your best guess if you don't already have one"
                  >
                    <span slot="reference"><i class="el-icon-question" style="font-size:14px;" /></span>
                  </el-popover>
                </div>
                <el-form-item
                  size="mini"
                  :rules="[
                    { required: true, message: 'IELTS Test Scores is required'}
                  ]"
                >
                  <el-row :span="24" style="display: flex; flex-wrap: wrap; justify-content:center;">
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.WRITING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.ieltsTestScore.WRITING" placeholder="...">
                          <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Writting</label>
                      </el-input>
                    </el-form-item>
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.READING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.ieltsTestScore.READING" placeholder="...">
                          <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Reading</label>
                      </el-input>
                    </el-form-item>
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.LISTENING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.ieltsTestScore.LISTENING" placeholder="...">
                          <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Listening</label>
                      </el-input>
                    </el-form-item>
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.SPEAKING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.ieltsTestScore.SPEAKING" placeholder="...">
                          <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Speaking</label>
                      </el-input>
                    </el-form-item>
                  </el-row>
                </el-form-item>
              </el-col>
            </div>
          </el-col>

          <el-col :span="12" class="padding-10" style="padding-left: 15px;">
            <el-row style="margin-bottom: 25px;">
              <el-col :span="24">
                <div style="height: 225px; padding: 0; border: 1px solid #ebeef5;" class="image-ielts" :class="{'question-con': selectTOEFL}" @click="selectScoreTOEFL()">
                  <img src="@/assets/img/LogoTOEFL.png" alt="" style="height:100%; width: 100%;">
                </div>
              </el-col>
            </el-row>
            <div v-if="selectTOEFL" style="margin-bottom: 25px;">
              <el-col :span="24" class="select-score-ielts question-con">
                <div style="margin-bottom:10px">
                  <span style="font-style:normal ;font-size: 15px; margin: 0 5px 5px;">Select You Current IELTS Writing Score</span>
                  <el-popover
                    placement="top-start"
                    title="Tips"
                    width="500"
                    trigger="hover"
                    content="Please use your best guess if you don't already have one"
                  >
                    <span slot="reference"><i class="el-icon-question" style="font-size:14px;" /></span>
                  </el-popover>
                </div>
                <el-form-item
                  size="mini"
                  :rules="[
                    { required: true, message: 'TOEFL Test Scores is required'}
                  ]"
                >
                  <el-row :span="24" style="display: flex; flex-wrap: wrap; justify-content:center;">
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.WRITING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.toeflTestScore.WRITING" placeholder="...">
                          <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Writting</label>
                      </el-input>
                    </el-form-item>
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.READING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.toeflTestScore.READING" placeholder="...">
                          <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Reading</label>
                      </el-input>
                    </el-form-item>
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.LISTENING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.toeflTestScore.LISTENING" placeholder="...">
                          <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Listening</label>
                      </el-input>
                    </el-form-item>
                    <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.SPEAKING" :rules="[ { required: true, message: 'Required' } ]">
                      <el-input id="scoresSelector">
                        <el-select slot="append" v-model="formData.toeflTestScore.SPEAKING" placeholder="...">
                          <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                        </el-select>
                        <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Speaking</label>
                      </el-input>
                    </el-form-item>
                  </el-row>
                </el-form-item>
              </el-col>
            </div>
          </el-col>
        </el-row>
        <el-row>
          <el-col class="padding-10">
            <el-button size="mini" @click="submit('formData')">Continue</el-button>
          </el-col>
        </el-row>
      </el-form>
    </el-col>
  </el-row>
</template>

<script>
import userService from '@/services/user.service'
import moment from 'moment'
import { SCORES } from '../../app.constant'
export default {
  name: 'SelectYourTest',
  data() {
    return {
      loadCompleted: false,
      textarea: '',
      initTextArea: {
        height: 500,
        selector: 'textarea',
        plugins: 'wordcount',
        toolbar: '',
        menubar: false
      },
      formData: {
        ieltsTestScore: {
          WRITING: null,
          READING: null,
          LISTENING: null,
          SPEAKING: null
        },
        toeflTestScore: {
          WRITING: null,
          READING: null,
          LISTENING: null,
          SPEAKING: null
        }
      },
      selectIELTS: false,
      selectTOEFL: false,
      toeflScores: [],
      ieltsScores: []
    }
  },
  computed: {
    currentUser() {
      var data = this.$store.getters['auth/getUser']
      console.log(data)
      return data
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    loadData() {
      for (let i = 0; i <= 9; i += 0.5) {
        this.ieltsScores.push(i.toFixed(1))
      }
      this.ieltsScores.reverse()
      for (let i = 0; i <= 30; i += 1) {
        this.toeflScores.push(i)
      }
      this.toeflScores.reverse()
      userService.getUserScore(this.currentUser.id).then(scores => {
        this.loadCompleted = true
        console.log(scores)

        if (scores.length > 0) {
          for (const key in this.formData.ieltsTestScore) {
            for (const s of scores) {
              if (s.sectionId == SCORES.IELTS[key].sectionId) {
                this.formData.ieltsTestScore[key] = s.score
                this.selectIELTS = true
              }
            }
          }
          for (const key in this.formData.toeflTestScore) {
            for (const s of scores) {
              if (s.sectionId == SCORES.TOEFL[key].sectionId) {
                this.formData.toeflTestScore[key] = s.score
                this.selectTOEFL = true
              }
            }
          }
        }
      })
    },
    submit(formName) {
      var scores = []
      this.$refs[formName].validate((valid) => {
        if (valid) {
          if (this.selectIELTS) {
            for (const key in this.formData.ieltsTestScore) {
              if (this.formData.ieltsTestScore[key]) {
                scores.push({
                  sectionId: SCORES.IELTS[key].sectionId,
                  score: this.formData.ieltsTestScore[key],
                  updatedDate: moment().format('yyyy-MM-DD')
                })
              }
            }
          }
          if (this.selectTOEFL) {
            for (const key in this.formData.toeflTestScore) {
              if (this.formData.toeflTestScore[key]) {
                scores.push({
                  sectionId: SCORES.TOEFL[key].sectionId,
                  score: this.formData.toeflTestScore[key],
                  updatedDate: moment().format('yyyy-MM-DD')
                })
              }
            }
          }
          userService.addScore(this.currentUser.id, scores).then(rs => {
            this.$store.dispatch('auth/setSelectedTest').then(() => {
              if (rs.userScores.length > 0) {
                this.$router.push('/questions')
              }
            })
          })
        }
      })
    },
    selectScoreIELTS() {
      this.selectIELTS = !this.selectIELTS
    },
    selectScoreTOEFL() {
      this.selectTOEFL = !this.selectTOEFL
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
  background-color: #fff;
  border-radius: 4px;
  font-size: 12px;
  font-style: italic;
}

.header-passage {
  padding: 0 20px;
  min-height: 40px;
  line-height: 37px;
  font-size: 15px;
  background-color: #cccccc;
  border: 1px solid #bfbfbf;
}

.body-passage {
  height: 200px;
  border-bottom: 1px solid #bfbfbf;
  border-left: 1px solid #bfbfbf;
  border-right: 1px solid #bfbfbf;
}

.header-start {
  padding: 20px;
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

.image-ielts {
  height: 100%;
}

.select-score-ielts {
  padding: 15px 30px;
  border: 1px solid #ebeef5;
}

.tip {
  padding: 20px 14px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
}

.show {
  display: block;
}

.hide-container {
  display: none;
}

.image-ielts:hover {
  cursor: pointer;
  opacity: 0.9;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, .1);
}
</style>
<style>

#scoresSelection .el-input .el-input-group__prepend{
  padding: 0 30px 0 15px;
}

.previewImageContainer.isActive{
  display: flex;
}
.closePopUpIMG{
  position: fixed;
  font-size: 2rem;
  top:5px;
  right: 5px;
}
.padding{
  padding: 5px;
}
</style>

