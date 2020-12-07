<template>
  <el-row>
    <el-col :span="18" :offset="3" class="padding-10" style="min-width: 480px;">
      <el-row>
        <el-col style="padding: 0 10px; margin-top: 20px;">
          <div style="font-size: 25px; font-weight: bold; color: #173852">Select Your Tests</div>
          <el-divider style="margin-top: 5px;" />
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12" class="padding-10" style="padding-right: 15px;">
          <el-row style="margin-bottom: 25px;">
            <el-col :span="24">
              <div class="image-ielts" :class="{'question-con': selectIELTS}" style="height: 225px; padding: 0; border: 1px solid #ebeef5;" @click="selectScoreIELTS()">
                <img src="@/assets/img/LogoIELTS.png" alt="" style="height:100%; width: 100%;">
              </div>
            </el-col>
          </el-row>
          <el-row style="margin-bottom: 25px;" :class="{'show': selectIELTS, 'hide-container': !selectIELTS}">
            <el-col :span="24" class="select-score-ielts question-con">
              <span style="font-style:normal ;font-size: 15px; margin-right: 15px;">Select You Current IELTS Writing Score</span>
              <el-select v-model="scoreIELTS" style="width: 140px" placeholder="Select Score">
                <el-option v-for="item in score" :key="item" :label="item" :value="item" />
              </el-select>
            </el-col>
          </el-row>
          <el-row class="row-flex" :class="{'show': selectIELTS, 'hide-container': !selectIELTS}">
            <el-col :span="24">
              <div class="tip">
                <p>Please use your best guess if you don't already have one</p>
              </div>
            </el-col>
          </el-row>
        </el-col>
        <el-col :span="12" class="padding-10" style="padding-left: 15px;">
          <el-row style="margin-bottom: 25px;">
            <el-col :span="24">
              <div style="height: 225px; padding: 0; border: 1px solid #ebeef5;" class="image-ielts" :class="{'question-con': selectTOEFL}" @click="selectScoreTOEFL()">
                <img src="@/assets/img/LogoTOEFL.png" alt="" style="height:100%; width: 100%;">
              </div>
            </el-col>
          </el-row>
          <el-row style="margin-bottom: 25px;" :class="{'show': selectTOEFL, 'hide-container': !selectTOEFL}">
            <el-col :span="24" class="select-score-ielts question-con">
              <span style="font-style:normal ;font-size: 15px; margin-right: 15px;">Select You Current TOEFL Writing Score</span>
              <el-select v-model="scoreTOEFL" style="width: 140px" placeholder="Select Score">
                <el-option v-for="item in score" :key="item" :label="item" :value="item" />
              </el-select>
            </el-col>
          </el-row>
          <el-row class="row-flex" :class="{'show': selectTOEFL, 'hide-container': !selectTOEFL}">
            <el-col :span="24">
              <div class="tip">
                <p>Please use your best guess if you don't already have one</p>
              </div>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
      <el-row>
        <el-col class="padding-10">
          <el-button @click="submit">Continue</el-button>
        </el-col>
      </el-row>
    </el-col>
  </el-row>
</template>

<script>
import userService from '@/services/user.service'
export default {
  name: 'SelectYourTest',
  data() {
    return {
      textarea: '',
      initTextArea: {
        height: 500,
        selector: 'textarea', // change this value according to your HTML
        plugins: 'wordcount',
        toolbar: '',
        menubar: false
      },
      scoreIELTS: null,
      scoreTOEFL: null,
      selectIELTS: false,
      selectTOEFL: false
    }
  },
  computed: {
    score: function() {
      var x = []
      for (let i = 0; i <= 100; i++) { x.push(i) }
      return x
    },
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  methods: {
    submit() {
      var data = []
      if (this.scoreIELTS != null) {
        data.push({
          sectionId: 8,
          score: this.scoreIELTS,
          updatedDate: new Date()
        })
      }
      if (this.scoreTOEFL != null) {
        data.push({
          sectionId: 4,
          score: this.scoreTOEFL,
          updatedDate: new Date()
        })
      }
      if (this.scoreIELTS || this.scoreTOEFL) {
        userService.addScore(this.currentUser.id, data)
        this.$router.push('/questions')
      }
    },
    selectScoreIELTS() {
      this.selectIELTS = !this.selectIELTS
      this.scoreIELTS = null
    },
    selectScoreTOEFL() {
      this.selectTOEFL = !this.selectTOEFL
      this.scoreTOEFL = null
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
