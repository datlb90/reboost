<template>
  <div id="appDetail" style="padding: 50px 0 20px 0;">
    <div class="container">
      <el-form ref="form" :model="form" label-width="120px">
        <el-breadcrumb class="breadcrumb-header" separator-class="el-icon-arrow-right">
          <el-breadcrumb-item :to="{ path: '/admin/raters' }">Application Lists</el-breadcrumb-item>
          <el-breadcrumb-item>Application Details</el-breadcrumb-item>
        </el-breadcrumb>
        <div class="content-container">
          <div class="col-md-8 forms-container">
            <!-- <div class="form-group group-content">
            <label class="col-md-4 content-label" for=""> Current Status </label>
            <div class="col-md-8">
              <el-button type="primary" plain>Primary</el-button>
            </div>
          </div>
          <div class="form-group group-content">
            <label class="col-md-4 content-label" for=""> Applied Date </label>
            <div class="col-md-8">
                <el-date-picker
                v-model="form.dateApplied"
                type="date"
                placeholder="Pick a day">
                </el-date-picker>
            </div>
          </div> -->
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> First Name </label>
              <el-input v-model="form.firstName" class="col-md-8" placeholder="Please input your first name" />
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> Last Name </label>
              <el-input v-model="form.lastName" class="col-md-8" placeholder="Please input your last name" />
            </div>
            <!-- <div class="form-group group-content">
            <label class="col-md-4 content-label" for=""> Email Address </label>
            <el-input class="col-md-8" placeholder="Please input your email" v-model="form.email"></el-input>
          </div> -->
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> Occupation </label>
              <div class="col-md-8">
                <el-select v-model="form.occupation" placeholder="Select">
                  <el-option
                    v-for="item in options"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  />
                </el-select>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> Gender </label>
              <div class="col-md-8">
                <el-select v-model="form.gender" placeholder="Select">
                  <el-option
                    v-for="item in gender"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  />
                </el-select>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> First Language </label>
              <div class="col-md-8">
                <el-select v-model="form.firstLanguage" placeholder="Select">
                  <el-option
                    v-for="item in Languages"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  />
                </el-select>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> Apply to Become </label>
              <div class="col-md-8">
                <el-checkbox-group v-model="goalList">
                  <el-checkbox label="IELTS Rater" />
                  <el-checkbox label="TOEFL Rater" />
                </el-checkbox-group>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> IELTS Test Scores </label>
              <div class="col-md-8">
                <label class="scores"> Writing: {{ ieltsWriting }} </label>
                <label class="scores"> Reading: {{ ieltsReading }} </label>
                <label class="scores"> Listening: {{ ieltsListening }} </label>
                <label class="scores"> Speaking: {{ ieltsSpeaking }} </label>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-upload-label" for=""> IELTS Test Scores </label>
              <div class="col-md-8">
                <el-upload
                  action="https://jsonplaceholder.typicode.com/posts/"
                  accept="image/png,image/jpg,image/jpeg"
                  :on-preview="handlePreview"
                  :on-remove="handleRemove"
                  :on-change="picChange"
                  :file-list="ieltsfileList"
                  :limit="10"
                  :auto-upload="false"
                  list-type="picture"
                >
                  <el-button size="small" type="primary">Click to upload</el-button>
                  <div slot="tip" class="el-upload__tip">jpg/png files with a size less than 500kb</div>
                </el-upload>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-label" for=""> TOEFL Test Scores </label>
              <div class="col-md-8">
                <label class="scores"> Writing: {{ toeflWriting }} </label>
                <label class="scores"> Reading: {{ toeflReading }} </label>
                <label class="scores"> Listening: {{ toeflListening }} </label>
                <label class="scores"> Speaking: {{ toeflSpeaking }} </label>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-upload-label" for=""> TOEFL Test Scores </label>
              <div class="col-md-8">
                <el-upload
                  action=" "
                  accept="image/png,image/jpg,image/jpeg"
                  :on-preview="handlePreview"
                  :on-remove="handleRemove"
                  :on-change="picChange"
                  :file-list="toeflfileList"
                  :limit="10"
                  :auto-upload="false"
                  list-type="picture"
                >
                  <el-button size="small" type="primary">Click to upload</el-button>
                  <div slot="tip" class="el-upload__tip">jpg/png files with a size less than 500kb</div>
                </el-upload>
              </div>
            </div>
            <div class="form-group group-content">
              <label class="col-md-4 content-upload-label" for=""> Photo ID </label>
              <div class="col-md-8">
                <el-upload
                  action=" "
                  accept="image/png,image/jpg,image/jpeg"
                  :on-preview="handlePreview"
                  :on-remove="handleRemove"
                  :on-change="picChange"
                  :file-list="form.photos"
                  :limit="10"
                  :auto-upload="false"
                  list-type="picture"
                >
                  <el-button size="small" type="primary">Click to upload</el-button>
                  <div slot="tip" class="el-upload__tip">Just 1 image expected.</div>
                  <div slot="tip" class="el-upload__tip">Jpg/png files with a size less than 500kb</div>
                </el-upload>
              </div>
            </div>
            <div class="form-group group-content">
              <div class="col-md-4" />
              <div class="col-md-8">
                <el-button class="button" size="medium" type="primary" @click="save">Save</el-button>
                <el-button class="button" size="medium" type="success">Approve</el-button>
                <el-button class="button" size="medium" type="danger">Denied</el-button>
              </div>
            </div>
          </div>
        </div>
      </el-form>
    </div>
  </div>
</template>
<script>
export default {
  name: 'ApplicationDetail',
  data() {
    return {
      picture: [],
      options: [{
        value: 'Teacher',
        label: 'Teacher'
      }, {
        value: 'Student',
        label: 'Student'
      }, {
        value: 'FreeLancer',
        label: 'FreeLancer'
      }],
      value: 'Teacher',
      Languages: [{
        value: 'English',
        label: 'English'
      }, {
        value: 'Vietnamese',
        label: 'Vietnamese'
      }, {
        value: 'Chinese',
        label: 'Chinese'
      }, {
        value: 'Japanese',
        label: 'Japanese'
      }],
      gender: [{
        value: 'Male',
        label: 'Male'
      }, {
        value: 'Female',
        label: 'Female'
      }],
      language: 'Vietnamese',
      goalList: ['IELTS Rater', 'TOEFL Rater'],
      ieltsWriting: 8.0,
      ieltsReading: 7.5,
      ieltsListening: 7.5,
      ieltsSpeaking: 8.0,
      ieltsfileList: [],
      toeflfileList: [],
      toeflWriting: 8.0,
      toeflReading: 7.5,
      toeflListening: 7.5,
      toeflSpeaking: 8.0,
      photoId: []
    }
  },
  computed: {
    form: {
      get() {
        return this.$store.getters['rater/getSelected']
      },
      set(value) {
        this.$store.dispatch('rater/setSelectedRater', value)
      }
    }
  },
  mounted() {
    this.$store.dispatch('rater/loadRater', 1)
  },
  methods: {
    handleRemove(file, fileList) {
      console.log(file, fileList)
    },
    handlePreview(file) {
      console.log(file)
    },
    loadRaterDetail() {

    },
    save() {
      console.log(this.form)
      console.log(this.picture)
      console.log(this.toeflfileList)
    },
    beforeUpload(file) {
      const _this = this
      const is1M = file.size / 1024 / 1024 < 1 // limit is less than 1M
      const isSize = new Promise(function(resolve, reject) {
        const width = 654 // limit the image size to 654X270
        const height = 270
        const _URL = window.URL || window.webkitURL
        const img = new Image()
        img.onload = function() {
          const valid = img.width === width && img.height === height
          valid ? resolve() : reject()
        }
        img.src = _URL.createObjectURL(file)
      }).then(() => {
        return file
      }, () => {
        _this.$message.error('The image size is limited to 654 x 270, the size cannot exceed 1MB')
        return Promise.reject()
      })
      if (!is1M) {
        _this.$message.error('The image size is limited to 654 x 270, the size cannot exceed 1MB')
      }
      return isSize & is1M
    },
    picChange(file, fileList) {
      console.log(fileList)
    }
  }
}
</script>
<style scoped>
.el-breadcrumb__inner a, .el-breadcrumb__inner.is-link{
  text-decoration: underline;
}
.breadcrumb-header{
    font-size: 28px;
}
.content-container{
    min-height: 500px;
    border: 1pt solid rgb(145, 143, 143);
    border-radius: 5px;
    margin: 30px 0 0 0;
}
.forms-container{
    margin: 40px 0 0 0;
}
.content-label{
    align-self: flex-end;
    text-align: right;
}
.group-content{
    display: flex;
}
.scores{
    padding: 0 20px 0 0;
}
.content-upload-label{
    text-align: right;
}
</style>
