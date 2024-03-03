<template>
  <div class="list-container" style="background: #edeeef; height: calc(100vh - 53px); padding-top: 30px; padding-right: 60px; padding-left: 60px; margin-top: 0px;">
    <el-row>
      <el-col class="col-border" style="margin-bottom: 20px; background: white; -webkit-box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06); box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06);">
        <el-steps style="margin-top: 10px;" :active="1" align-center>
          <el-step title="Bước 1" icon="el-icon-user" description="Tạo tài khoản" />
          <el-step title="Bước 2" icon="el-icon-upload" description="Nộp hồ sơ" />
          <el-step title="Bước 3" icon="el-icon-circle-check" description="Hoàn thành đào tạo" />
          <el-step title="Bước 4" icon="el-icon-edit-outline" description="Bắt đầu chấm bài" />
        </el-steps>
      </el-col>
    </el-row>
    <el-row>
      <el-col class="col-border" style="background: white; -webkit-box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06); box-shadow: 0 2px 28px 0 rgba(0, 0, 0, 0.06);">
        <el-form ref="formRegister" :model="formRegister" label-width="180px" style="width: 90%;">
          <el-form-item v-if="raterId" size="mini" label="Current Status">
            <el-tag
              :type="
                formRegister.status === 'Verified'
                  ? 'success'
                  : formRegister.status === 'Applied'
                    ? 'primary'
                    : formRegister.status === 'Denied'
                      ? 'danger'
                      : 'warning'
              "
              disable-transitions
            >{{ formRegister.status }}</el-tag>
          </el-form-item>
          <el-form-item v-if="raterId" size="mini" label="Applied Date" prop="appliedDate">
            <el-input
              v-model="formRegister.appliedDate"
              :disabled="true"
            />
          </el-form-item>
          <el-form-item
            size="mini"
            label="Họ và tên:"
            prop="firstName"
            style="margin-bottom: 5px;"
          >
            <div style="margin-top: 4px; font-size: 13px;">{{ formRegister.firstName }}</div>
          </el-form-item>
          <el-form-item
            size="mini"
            label="Địa chỉ email:"
            prop="firstName"
            style="margin-bottom: 5px;"
          >
            <div style="margin-top: 4px; font-size: 13px;">{{ formRegister.email }}</div>
          </el-form-item>

          <el-form-item size="mini" label="Đăng ký trở thành" style="margin-bottom: 10px;" prop="applyTo" :rules="[{ type: 'array', required: true, message: 'Bạn cần lựa chọn đăng ký trở thành giáo viên IELTS hoặc TOEFL' }]">
            <div style="transform:translateY(5px)">
              <el-checkbox-group v-model="formRegister.applyTo">
                <el-checkbox label="Giáo Viên IELTS" name="applyTo" />
                <el-checkbox label="Giáo Viên TOEFL" name="applyTo" />
              </el-checkbox-group>
            </div>
          </el-form-item>
          <div v-if="applyToIELTSChecked">
            <el-form-item
              size="mini"
              label="Chứng chỉ IELTS"
              prop="iELTSCertificatePhotos"
              :rules="[
                { required: true, message: 'Chứng chỉ IELTS là bắt buộc'}]"
            >
              <el-upload
                class="upload-demo"
                action=""
                :on-preview="previewImage"
                :file-list="formRegister.iELTSCertificatePhotos"
                :on-change="handleChangeIELTS"
                :on-remove="handleRemoveIELTS"
                :auto-upload="false"
                list-type="picture"
              >
                <el-button type="primary">Click to upload</el-button>
                <div slot="tip" class="el-upload__tip">
                  <p>Ảnh chụp kết quả thi IELTS hoặc chứng chỉ liên quan của bạn. Chỉ chấp nhận ảnh có dung lượng dưới 3mb với đuôi jpg, jpeg, gif, và png.</p>
                </div>
              </el-upload>
            </el-form-item>
          </div>
          <div v-if="applyToTOEFLChecked">
            <el-form-item
              size="mini"
              label="Chứng chỉ TOEFL"
              prop="tOEFLCertificatePhotos"
              :rules="[
                { required: true, message: 'Chứng chỉ TOEFL là bắt buộc'}]"
            >
              <el-upload
                class="upload-demo"
                action=""
                :on-preview="previewImage"
                :on-change="handleChangeTOEFL"
                :on-remove="handleRemoveTOEFL"
                :file-list="formRegister.tOEFLCertificatePhotos"
                :auto-upload="false"
                list-type="picture"
              >
                <el-button type="primary">Click to upload</el-button>
                <div slot="tip" class="el-upload__tip">
                  <p>Ảnh chụp kết quả thi TOEFL hoặc chứng chỉ liên quan của bạn. Chỉ chấp nhận ảnh có dung lượng dưới 3mb với đuôi jpg, jpeg, gif, và png.</p>
                </div>
              </el-upload>
            </el-form-item>
          </div>

          <el-form-item
            size="mini"
            label="Ảnh định danh"
            prop="iDCardPhotos"
            :rules="[
              { required: true, message: 'Ảnh chụp CMND hoặc CCCD là bắt buộc'}]"
          >
            <el-upload
              class="upload-demo"
              action=""
              :on-preview="previewImage"
              :file-list="formRegister.iDCardPhotos"
              :on-change="handleChangeIdPhoto"
              :on-remove="handleRemoveIdPhoto"
              :auto-upload="false"
              list-type="picture"
            >
              <el-button type="primary">Click to upload</el-button>
              <div slot="tip" class="el-upload__tip">
                <p>Ảnh chụp CMND hoặc CCCD có tên trùng với tên trên chứng chỉ của bạn. Chỉ chấp nhận ảnh có dung lượng dưới 3mb với đuôi jpg, jpeg, gif, và png.</p>
              </div>
            </el-upload>
          </el-form-item>
          <el-form-item
            size="mini"
            label="Giới thiệu"
          >
            <el-input v-model="formRegister.biography" type="textarea" :rows="5" placeholder="Hãy cho chúng tôi biết đôi chút về bạn và lý do bạn muốn trở thành giáo viên của Reboost" />
          </el-form-item>
          <el-form-item v-if="raterId" size="mini" label="Note">
            <el-input v-model="formRegister.note" type="textarea" :rows="5" placeholder="Note" />
          </el-form-item>
          <el-form-item size="mini" style="margin: 0;">
            <el-button v-if="!raterId" type="primary" size="mini" :loading="loading" @click="onSubmit('formRegister', 'create')">Nộp hồ sơ</el-button>
            <!-- <el-button v-if="!raterId" size="mini"></el-button> -->
            <el-button v-if="raterId" class="button" size="mini" type="primary" @click="onSubmit('formRegister', 'update')">Save</el-button>
            <el-button v-if="raterId" class="button" size="mini" type="success" @click="updateStatus('Approved')">Approve for training</el-button>
            <el-button v-if="raterId" class="button" size="mini" type="danger" @click="updateStatus('Rejected')">Reject</el-button>
          </el-form-item>
        </el-form>
      </el-col>
    </el-row>
    <div :class="{ 'isActive': toggleImagePopup }" class="image-container-preview" @click="closeImg($event)">
      <img v-if="popUpImageUrl.type==='images'" id="previewImg" ref="previewImg" :src="popUpImageUrl.url" class="image-fit" alt="">
      <video v-if="popUpImageUrl.type==='videos'" controls>
        <source :src="popUpImageUrl.url">
      </video>
      <div class="close-icon" @click="toggleImagePopup=!toggleImagePopup">
        <i class="el-icon-close" style="font-size: 1.5rem;" />
      </div>
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
import raterService from '../../services/rater.service'
import moment from 'moment-timezone'
// import Notification from 'element-ui'
import * as mapUtil from '@/utils/model-mapping'
import * as stringUtil from '@/utils/string'
import { PageName } from '@/app.constant'
import { UserRole } from '../../app.constant'

export default {
  name: 'Application',
  beforeRouteEnter(to, from, next) {
    raterService.getByCurrentUser().then(rs => {
      console.log('current user rater', rs)
      if (rs && rs.status) {
        return next({ name: PageName.RATER_STATUS, params: { id: rs.id }})
      }
      next()
    })
  },
  data() {
    return {
      formRegister: {
        id: '',
        firstName: '',
        lastName: '',
        gender: '',
        occupation: '',
        status: '',
        firstLanguage: '',
        applyTo: [],
        ieltsTestScore: {
          writing: null,
          reading: null,
          listening: null,
          speaking: null
        },
        toeflTestScore: {
          writing: null,
          reading: null,
          listening: null,
          speaking: null
        },
        iDCardPhotos: [],
        iELTSCertificatePhotos: [],
        tOEFLCertificatePhotos: [],
        biography: '',
        note: '',
        appliedDate: new Date()
      },
      ieltsScores: [],
      toeflScores: [],
      occupation: [],
      firstLanguage: [],
      gender: [],
      raterId: '',
      toeflScoresIsNUll: true,
      ieltsScoresIsNUll: true,
      toggleImagePopup: false,
      popUpImageUrl: {
        type: null,
        url: null
      },
      portraitImg: true,
      loading: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    applyToIELTSChecked() {
      return this.getApplyTo('IELTS')
    },
    applyToTOEFLChecked() {
      return this.getApplyTo('TOEFL')
    }
  },
  mounted() {
    this.onLoad()
  },
  methods: {
    onLoad() {
      // const w = require('../../assets/data.json')
      // this.firstLanguage = w.Languages
      // this.gender = w.Gender

      for (let i = 0; i <= 9; i += 0.5) {
        this.ieltsScores.push(i.toFixed(1))
      }
      this.ieltsScores.reverse()

      for (let i = 0; i <= 30; i += 1) {
        this.toeflScores.push(i)
      }
      this.toeflScores.reverse()

      if (this.currentUser?.role === UserRole.RATER) {
        this.formRegister.firstName = this.currentUser.firstName
        this.formRegister.lastName = this.currentUser.lastName
        this.formRegister.email = this.currentUser.email
        this.formRegister.applyTo.push('Giáo Viên IELTS')
      }
      // lookupService.getByType('OCCUPATION').then(rs => {
      //   this.occupation = rs;
      // });
      // lookupService.getByType('FIRST_LANGUAGE').then(rs => {
      //   this.firstLanguage = rs;
      // });
      // lookupService.getByType('GENDER').then(rs => {
      //   this.gender = rs;
      // });

      if (this.$route.params.id) {
        this.raterId = this.$route.params.id
        this.loadDetail(this.$route.params.id)
      } else {
        this.getApplyTo('IELTS')
      }
    },
    loadDetail(id) {
      raterService.getById(id).then(rs => {
        console.log('result load detail', rs)
        this.formRegister = mapUtil.map(rs, this.formRegister)
        this.formRegister.appliedDate = rs.appliedDate.toString()
        this.formRegister.appliedDate = this.formRegister.appliedDate.slice(0, 10)

        // Files
        if (rs.raterCredentials) {
          for (const f of rs.raterCredentials) {
            if (f.credentialType == 'IDPhoto') {
              this.formRegister.iDCardPhotos.push({ name: f.fileName, url: `data:${this.isImage(f.fileName) ? 'image' : 'video'}/${this.getExtension(f.fileName)};base64,` + f.data })
            } else if (f.credentialType == 'TOEFLPhoto') {
              this.formRegister.tOEFLCertificatePhotos.push({ name: f.fileName, url: `data:${this.isImage(f.fileName) ? 'image' : 'video'}/${this.getExtension(f.fileName)};base64,` + f.data })
            } else if (f.credentialType == 'IELTSPhoto') {
              this.formRegister.iELTSCertificatePhotos.push({ name: f.fileName, url: `data:${this.isImage(f.fileName) ? 'image' : 'video'}/${this.getExtension(f.fileName)};base64,` + f.data })
            }
          }
        }

        // Score
        if (rs.user.userScores) {
          if (this.getApplyTo('IELTS')) {
            this.formRegister.ieltsTestScore.listening = rs.user.userScores.find(s => s.sectionId == 5).score
            this.formRegister.ieltsTestScore.reading = rs.user.userScores.find(s => s.sectionId == 6).score
            this.formRegister.ieltsTestScore.speaking = rs.user.userScores.find(s => s.sectionId == 7).score
            this.formRegister.ieltsTestScore.writing = rs.user.userScores.find(s => s.sectionId == 8).score
          }
          if (this.getApplyTo('TOEFL')) {
            this.formRegister.toeflTestScore.listening = rs.user.userScores.find(s => s.sectionId == 1).score
            this.formRegister.toeflTestScore.reading = rs.user.userScores.find(s => s.sectionId == 2).score
            this.formRegister.toeflTestScore.speaking = rs.user.userScores.find(s => s.sectionId == 3).score
            this.formRegister.toeflTestScore.writing = rs.user.userScores.find(s => s.sectionId == 4).score
          }
        }

        // Apply to
        if (rs.applyTo) {
          this.formRegister.applyTo = rs.applyTo.map(i => i + ' Rater')
        }

        // First name
        this.formRegister.firstName = rs.user.firstName
        // Last name
        this.formRegister.lastName = rs.user.lastName
      })
    },
    onSubmit(formName, createOrUpdate) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          this.loading = true
          const formData = new FormData()
          formData.set('User[firstName]', this.formRegister.firstName)
          formData.set('User[lastName]', this.formRegister.lastName)
          formData.set('Gender', 'other')
          formData.set('Occupation', 'n/a')
          formData.set('Biography', 'n/a')
          formData.set('Note', 'n/a')
          formData.set('firstLanguage', 'n/a')
          formData.set('Status', 'Applied')
          formData.set('AppliedDate', moment().format('yyyy-MM-DD hh:mm:ss'))
          formData.set('LastActivityDate', moment().format('yyyy-MM-DD hh:mm:ss'))
          formData.set('UserId', this.currentUser.id)

          let count = 0
          for (const p of this.formRegister.iDCardPhotos) {
            formData.set(`RaterCredentials[${count}][TestId]`, '0')
            formData.set(`RaterCredentials[${count}][CredentialType]`, 'IDPhoto')
            formData.set(`RaterCredentials[${count}][FileName]`, p.name)
            formData.set(`RaterCredentials[${count}][FileExtension]`, stringUtil.getFileExtension(p.name))

            if (p.raw) {
              formData.append(`UploadedFiles`, p.raw)
            } else {
              formData.append(`UploadedFiles`, stringUtil.base64ToArrayBuffer(p.url, p.name))
            }
            count++
          }

          if (this.getApplyTo('IELTS')) {
            for (const p of this.formRegister.iELTSCertificatePhotos) {
              formData.set(`RaterCredentials[${count}][TestId]`, '2')
              formData.set(`RaterCredentials[${count}][CredentialType]`, 'IELTSPhoto')
              formData.set(`RaterCredentials[${count}][FileName]`, p.name)
              formData.set(`RaterCredentials[${count}][FileExtension]`, stringUtil.getFileExtension(p.name))

              if (p.raw) {
                formData.append(`UploadedFiles`, p.raw)
              } else {
                formData.append(`UploadedFiles`, stringUtil.base64ToArrayBuffer(p.url, p.name))
              }

              count++
            }
          }

          if (this.getApplyTo('TOEFL')) {
            for (const p of this.formRegister.tOEFLCertificatePhotos) {
              formData.set(`RaterCredentials[${count}][TestId]`, '1')
              formData.set(`RaterCredentials[${count}][CredentialType]`, 'TOEFLPhoto')
              formData.set(`RaterCredentials[${count}][FileName]`, p.name)
              formData.set(`RaterCredentials[${count}][FileExtension]`, stringUtil.getFileExtension(p.name))

              if (p.raw) {
                formData.append(`UploadedFiles`, p.raw)
              } else {
                formData.append(`UploadedFiles`, stringUtil.base64ToArrayBuffer(p.url, p.name))
              }
            }
          }

          const scores = []

          if (this.getApplyTo('IELTS')) {
            scores.push(...[
              { sectionId: 5, score: 0, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 6, score: 0, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 8, score: 0, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 7, score: 0, updatedDate: moment().format('yyyy-MM-DD') }
            ])
          }
          if (this.getApplyTo('TOEFL')) {
            scores.push(...[
              { sectionId: 1, score: 0, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 2, score: 0, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 4, score: 0, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 3, score: 0, updatedDate: moment().format('yyyy-MM-DD') }
            ])
          }

          count = 0

          for (const s of scores) {
            formData.set(`User[UserScores][${count}][SectionId]`, s.sectionId)
            formData.set(`User[UserScores][${count}][Score]`, s.score)
            formData.set(`User[UserScores][${count}][UpdatedDate]`, s.updatedDate)
            count++
          }

          if (createOrUpdate == 'create') {
            raterService.insert(formData).then(rs => {
              this.$notify.success({
                title: 'Success',
                message: 'Hồ sơ đăng ký của bạn đã được nộp thành công. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.',
                type: 'success',
                duration: 5000
              })
              this.loading = false
              this.$router.push('/rater/application/status/' + rs.id)
            })
          } else if (createOrUpdate == 'update') {
            formData.set('Id', this.formRegister.id)
            raterService.update(formData).then(rs => {
              this.loading = false
              this.$notify.success({
                title: 'Success',
                message: 'Updated successfully',
                type: 'success',
                duration: 2000
              })
            })
          }
        } else {
          this.$notify.error({
            title: 'Error',
            message: 'Đã có lỗi xảy ra trong quá trình đăng ký',
            type: 'error',
            duration: 2000
          })
          this.loading = false
          return false
        }
      })
    },
    updateStatus(status) {
      raterService.updateStatus(this.raterId, status).then(rs => {
        this.formRegister.status = status
        this.$notify.error({
          title: 'Error',
          message: 'Đã có lỗi xảy ra trong quá trình đăng ký',
          type: 'error',
          duration: 2000
        })
      })
    },
    handleRemoveIdPhoto(file, fileList) {
      this.formRegister.iDCardPhotos = fileList
    },
    handleChangeIdPhoto(file, fileList) {
      if (this.isImage(file.name)) {
        this.formRegister.iDCardPhotos = fileList
      } else {
        fileList.pop()
      }
    },
    handleChangeIELTS(file, fileList) {
      if (this.isImage(file.name)) {
        this.formRegister.iELTSCertificatePhotos = fileList
      } else {
        fileList.pop()
      }
    },
    handleRemoveIELTS(file, fileList) {
      this.formRegister.iELTSCertificatePhotos = fileList
    },
    handleChangeTOEFL(file, fileList) {
      if (this.isImage(file.name)) {
        this.formRegister.tOEFLCertificatePhotos = fileList
      } else {
        fileList.pop()
      }
    },
    handleRemoveTOEFL(file, fileList) {
      this.formRegister.tOEFLCertificatePhotos = fileList
    },
    getApplyTo(testName) {
      return this.formRegister.applyTo.find(a => a.indexOf(testName) >= 0) != undefined
    },
    previewImage(e) {
      if (this.isImage(e.name)) {
        this.popUpImageUrl.type = 'images'
      }
      // if (this.isVideo(e.name)) {
      //   this.popUpImageUrl.type = 'videos'
      // }
      this.popUpImageUrl.url = e.url
      this.toggleImagePopup = !this.toggleImagePopup
      console.log(this.$refs.previewImg)
    },
    closeImg(e) {
      if (e.target.firstChild != null) {
        this.toggleImagePopup = !this.toggleImagePopup
      }
    },
    getExtension(filename) {
      var parts = filename.split('.')
      return parts[parts.length - 1]
    },
    isImage(filename) {
      var ext = this.getExtension(filename)
      switch (ext.toLowerCase()) {
        case 'jpg':
        case 'jpeg':
        case 'gif':
        case 'png':
          // etc
          return true
      }
      return false
    },
    isVideo(filename) {
      var ext = this.getExtension(filename)
      switch (ext.toLowerCase()) {
        case 'm4v':
        case 'avi':
        case 'mpg':
        case 'mp4':
          // etc
          return true
      }
      return false
    }
  }
}
</script>

<style scoped>
.image-fit{
  max-height: 100%;max-width: 100%; object-fit: cover;
}
.close-icon {
  position: absolute;
  right: 5px;
  top: 5px;
  padding: 4px 0 0 0;
  text-align: center;
  align-items: center;
  width: 32px;
  height: 32px;
  background-color: #E4E6EB;
  border-radius: 50%;
  cursor: pointer;
}

.image-container-preview{
  display: none;
  position: fixed;
  top:0;
  z-index: 9999;
  background-color: rgba(0,0,0,0.5);
  width: 100%;
  height: 100%;
  align-items: center;
  justify-content: center;
}

.image-container-preview.isActive{
  display: flex;
}

.col-border {
  border: 1px solid #c5c5c5;
  padding: 25px 0;
  border-radius: 4px;
}

.row-flex {
  display: flex;
  justify-content: center;
  margin: 25px 0;
}

.tip {
  padding: 8px 14px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
}

.padding-30 {
  padding: 30px;
}

.padding-tb-15 {
  padding: 15px 0;
}

.text-right {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  height: 40px;
}

</style>

<style>

#scoresSelector{
  width: 0px;
  padding: 0px;
  border: none;
}

#scoresSelector+.el-input-group__append > .el-select {
  width: 60px;
}
#scoresSelector+.el-input-group__append > .el-select .el-input{
  width: 60px;
}
#scoresSelector+.el-input-group__append{
  background-color: #FFF !important ;
}
#scoresSelector+.el-input-group__append > .el-select .el-input .el-input__inner{
  padding: 0 20px 0 10px !important
}

#scoresSelection .el-form-item__content .el-input .el-input-group__prepend  {
  background-color:#f5f7fa !important;
  color: #909399 !important;
  border: 1px solid #dcdfe6 !important;
  padding: 0px 28px 0 10px !important;
  box-sizing: border-box !important;
}

#scoresSelection > .el-form-item__content > .el-input > .el-input-group__prepend > .el-button{
  width: 100px;
}

.el-input-group {
  line-height: normal;
  display: inline-table;
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  width: 0px !important;
  padding: 0 !important;
}

</style>

