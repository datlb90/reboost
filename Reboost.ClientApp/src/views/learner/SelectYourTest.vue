<template>
  <div class="list-container">

    <el-alert
      type="info"
      :show-icon="true"
      center
      style="margin-bottom: 10px; margin-top: 10px;"
    >
      <span slot="title" style="font-size: 15px;">
        Chào mừng bạn đến với Reboost! Để bắt đầu luyện tập, hãy chọn bài thi phù hợp với bạn.
      </span>
    </el-alert>

    <el-card style="width: 100%; padding: 10px;" class="box-card">

      <div style="margin-bottom: 20px;">
        <div
          class="image-ielts"
          :class="{'question-con': selectIELTS}"
          style="text-align: center; height: 225px; padding: 50px; border: 1px solid #ebeef5; border-radius: 5px;"
          @click="selectScoreIELTS()"
        >
          <img src="@/assets/img/ielts-logo.png" alt="" style="height:100%; width: 380px;">
        </div>
      </div>

      <div style="margin-bottom: 20px; margin-top: 10px;">
        <div
          style=" text-align: center; height: 225px; padding: 50px; border: 1px solid #ebeef5;"
          class="image-ielts"
          :class="{'question-con': selectTOEFL}"
          @click="selectScoreTOEFL()"
        >
          <img src="@/assets/img/toefl-logo.png" alt="" style="height:100%; width: 400px;">
        </div>
      </div>

      <div style="width: 210px; margin: auto;">
        <el-button v-if="selectIELTS || selectTOEFL" size="medium" type="primary" :loading="loading" plain @click="submit('formData')">
          Xác nhận lựa chọn
        </el-button>
      </div>

    </el-card>
  </div>

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
          WRITING: 0,
          READING: 0,
          LISTENING: 0,
          SPEAKING: 0
        },
        toeflTestScore: {
          WRITING: 0,
          READING: 0,
          LISTENING: 0,
          SPEAKING: 0
        }
      },
      selectIELTS: false,
      selectTOEFL: false,
      toeflScores: [],
      ieltsScores: [],
      loading: false
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
      this.loading = true
      var scores = []
      if (this.selectIELTS) {
        for (const key in this.formData.ieltsTestScore) {
          scores.push({
            sectionId: SCORES.IELTS[key].sectionId,
            score: 0,
            updatedDate: moment().format('yyyy-MM-DD')
          })
          // if (this.formData.ieltsTestScore[key]) {
          //   scores.push({
          //     sectionId: SCORES.IELTS[key].sectionId,
          //     score: this.formData.ieltsTestScore[key],
          //     updatedDate: moment().format('yyyy-MM-DD')
          //   })
          // }
        }
      }
      if (this.selectTOEFL) {
        for (const key in this.formData.toeflTestScore) {
          scores.push({
            sectionId: SCORES.TOEFL[key].sectionId,
            score: 0, // this.formData.toeflTestScore[key],
            updatedDate: moment().format('yyyy-MM-DD')
          })
          // if (this.formData.toeflTestScore[key]) {
          //   scores.push({
          //     sectionId: SCORES.TOEFL[key].sectionId,
          //     score: this.formData.toeflTestScore[key],
          //     updatedDate: moment().format('yyyy-MM-DD')
          //   })
          // }
        }
      }
      userService.addScore(this.currentUser.id, scores).then(rs => {
        this.$store.dispatch('auth/setSelectedTest').then(() => {
          if (rs.userScores.length > 0) {
            this.$router.push('/questions')
            this.loading = false
          }
        })
      })

      // this.$refs[formName].validate((valid) => {
      //   if (valid) {
      //     if (this.selectIELTS) {
      //       for (const key in this.formData.ieltsTestScore) {
      //         if (this.formData.ieltsTestScore[key]) {
      //           scores.push({
      //             sectionId: SCORES.IELTS[key].sectionId,
      //             score: this.formData.ieltsTestScore[key],
      //             updatedDate: moment().format('yyyy-MM-DD')
      //           })
      //         }
      //       }
      //     }
      //     if (this.selectTOEFL) {
      //       for (const key in this.formData.toeflTestScore) {
      //         if (this.formData.toeflTestScore[key]) {
      //           scores.push({
      //             sectionId: SCORES.TOEFL[key].sectionId,
      //             score: this.formData.toeflTestScore[key],
      //             updatedDate: moment().format('yyyy-MM-DD')
      //           })
      //         }
      //       }
      //     }
      //     userService.addScore(this.currentUser.id, scores).then(rs => {
      //       this.$store.dispatch('auth/setSelectedTest').then(() => {
      //         if (rs.userScores.length > 0) {
      //           this.$router.push('/questions')
      //         }
      //       })
      //     })
      //   }
      // })
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
  background-color: #ecf8ff;
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

