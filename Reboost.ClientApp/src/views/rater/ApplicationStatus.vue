<template>
  <div style="margin-top:25px;" :style="{visibility: loadCompleted?'visible':'hidden'}">
    <el-row class="row-flex">
      <el-col :span="15" class="col-border">
        <el-steps :active="2" align-center>
          <el-step title="Step 1" icon="el-icon-user" description="Create an account" />
          <el-step title="Step 2" icon="el-icon-upload" description="Upload credentials" />
          <el-step title="Step 3" icon="el-icon-circle-check" description="Complete trainning" />
          <el-step title="Step 4" icon="el-icon-edit-outline" description="Start rating" />
        </el-steps>
      </el-col>
    </el-row>
    <el-row class="row-flex">
      <el-col :span="15" class="col-border">
        <div class="margin-container">
          <div class="flex-box">
            <div class="label-container">
              Application status
            </div>

            <el-tag
              :type="
                status === RATER_STATUS.APPLIED
                  ? 'primary'
                  :status === RATER_STATUS.APPROVED || status === RATER_STATUS.TRAINING
                    ? 'success'
                    :status === RATER_STATUS.REJECTED
                      ? 'danger'
                      : status===RATER_STATUS.DOCUMENT_REQUESTED
                        ? 'warning' : 'warning'
              "
            >{{ status }}</el-tag>

          </div>
          <div :class="[status === RATER_STATUS.APPLIED ? 'inReview' : 'hidden']">
            <p>Thank you for applying. Your application is curretly in review. We will notify you via email if your application is approved, denied, or if we need additional information.</p>
          </div>
          <div :class="[status === RATER_STATUS.TRAINING || status === RATER_STATUS.APPROVED ? 'verified' : 'hidden']">
            <p>Your application has been reviewed and verified. You are just one step away from becoming our rater. After completing our training process, you can start rating and earing extra money.
            </p>
          </div>
          <div :class="[status === RATER_STATUS.REJECTED ? 'denied' : 'hidden']">
            <p>Unfortunately, your application was denied because your credentials do not match with the requirements for becoming a rater. We look forward to receiving your application again in the near future. If you have any questions or concerns regarding this, please feel free to contact us as support@reboost.ai.
            </p>
          </div>
          <div v-if="note && note.length && (status===RATER_STATUS.REVISION_REQUESTED || status===RATER_STATUS.DOCUMENT_REQUESTED)" class="note-container">
            <div class="label-container" style="width: 155px">
              Note
            </div>
            <el-input
              v-model="note"
              type="textarea"
              :rows="2"
              :disabled="true"
            />
          </div>
          <div v-if="status===RATER_STATUS.TRAINING || status === RATER_STATUS.REVISION_REQUESTED" style="margin-left:150px" class="button-container">
            <el-tooltip v-if="applyToList.includes('IELTS')" :content="isApprove('IELTS')?'You have passed this training':'Start your IELTS Training'" placement="top">
              <el-button :type="isApprove('IELTS')?'success':'primary'" style="margin:0" size="mini" @click="redirectToTraining('IELTS')">Complete IELTS Training</el-button>
            </el-tooltip>
            <el-tooltip v-if="applyToList.includes('TOEFL')" :content="isApprove('TOEFL')?'You have passed this training':'Start your TOEFL Training'" placement="top">
              <el-button :type="isApprove('TOEFL')?'success':'primary'" style="margin: 0 10px 0" size="mini" @click="redirectToTraining('TOEFL')">Complete TOEFL Training</el-button>
            </el-tooltip>
          </div>
          <div>
            <el-form v-if="status===RATER_STATUS.DOCUMENT_REQUESTED" ref="formRegister" class="file-upload-form" :model="formRegister" label-position="left" label-width="150px" style="width:100%;">
              <el-form-item
                v-if="applyToList.includes('IELTS')"
                size="mini"
                label="IELTS Credentials"
                prop="iELTSCertificatePhotos"
                :rules="[
                  { required: true, message: 'IELTS Credentials Photos is required'}]"
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
                    <p>Please upload your IELTS test result, and any other supporting credidentials you may have. Files must be less than 500kb in size.</p>
                  </div>
                </el-upload>
              </el-form-item>
              <el-form-item
                v-if="applyToList.includes('TOEFL')"
                size="mini"
                label="TOEFL Credentials"
                prop="tOEFLCertificatePhotos"
                :rules="[
                  { required: true, message: 'TOEFL Credentials Photos is required'}]"
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
                    <p>Please upload your TOEFL test result, and any other supporting credidentials you may have. Files must be less than 500kb in size.</p>
                  </div>
                </el-upload>
              </el-form-item>
              <el-form-item
                size="mini"
                label="Photo ID"
                prop="iDCardPhotos"
                :rules="[
                  { required: true, message: 'IDPhotos is required'}]"
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
                    <p>Please upload a form of photo identification such as ID card, driver license, or passport. The file must be less than 500kb in size.</p>
                  </div>
                </el-upload>
              </el-form-item>
            </el-form>
            <el-button v-if="status===RATER_STATUS.DOCUMENT_REQUESTED" style="margin-top:10px" size="mini" type="primary" @click="onSubmit('formRegister', 'update')">Save</el-button>
          </div>
        </div>
      </el-col>
    </el-row>
    <div :class="{ 'isActive': toggleImagePopup }" class="image-container-preview" @click="closeImg($event)">
      <img id="previewImg" ref="previewImg" :src="popUpImageUrl" class="image-fit" alt="">
      <div class="close-icon" @click="toggleImagePopup=!toggleImagePopup">
        <i class="el-icon-close" style="font-size: 1.5rem;" />
      </div>
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
import raterService from '@/services/rater.service'
import * as mapUtil from '@/utils/model-mapping'
import reviewService from '@/services/review.service.js'
import * as stringUtil from '@/utils/string'
import { RATER_STATUS } from '../../app.constant'

export default {
  name: 'ApplicationStatus',
  data() {
    return {
      loadCompleted: false,
      raterId: '',
      isUpload: false,
      inReview: true,
      verified: true,
      denied: true,
      status: '',
      note: undefined,
      applyToList: [],
      iDCardPhotos: [],
      formRegister: {
        id: '',
        iDCardPhotos: [],
        iELTSCertificatePhotos: [],
        tOEFLCertificatePhotos: []
      },
      popUpImageUrl: null,
      toggleImagePopup: false,
      RATER_STATUS: RATER_STATUS
    }
  },
  computed: {
    getReviews() {
      return this.$store.getters['review/getReviewsById']
    },
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  mounted() {
    this.onLoad()
    this.$store.dispatch('review/loadReviewsById')
  },
  methods: {
    onLoad() {
      if (this.$route.params.id) {
        this.raterId = this.$route.params.id
        this.loadDetail(this.$route.params.id)
      }
    },
    loadDetail(id) {
      console.log('load detail', mapUtil)
      raterService.getById(id).then(rs => {
        this.formRegister = mapUtil.map(rs, this.formRegister)
        this.loadCompleted = true
        console.log('result load detail', rs)
        rs.applyTo.forEach(e => {
          this.applyToList.push(e)
        })
        this.status = rs.status
        this.note = rs.note

        // Files
        if (rs.raterCredentials) {
          for (const f of rs.raterCredentials) {
            if (f.credentialType == 'IDPhoto') {
              this.formRegister.iDCardPhotos.push({ name: f.fileName, url: 'data:image/png;base64,' + f.data })
            } else if (f.credentialType == 'TOEFLPhoto') {
              this.formRegister.tOEFLCertificatePhotos.push({ name: f.fileName, url: 'data:image/png;base64,' + f.data })
            } else if (f.credentialType == 'IELTSPhoto') {
              this.formRegister.iELTSCertificatePhotos.push({ name: f.fileName, url: 'data:image/png;base64,' + f.data })
            }
          }
        }
      })
    },
    redirectToTraining(e) {
      reviewService.createNewReviewSample(e.toLowerCase()).then(r => {
        if (r != 'failed') {
          var pushUrl = e.toLowerCase() == 'toefl' ? '/review/12/68/' + r : '/review/9/69/' + r
          this.$router.push(pushUrl)
        }
      })
    },
    isApprove(type) {
      const list = this.getReviews
      type += 'TrainingApproved'
      if (list.filter(r => { return r.status == type }).length > 0) {
        return true
      }
      return false
    },
    handleRemoveIdPhoto(file, fileList) {
      this.formRegister.iDCardPhotos = fileList
    },
    handleChangeIdPhoto(file, fileList) {
      this.formRegister.iDCardPhotos = fileList
    },
    handleChangeIELTS(file, fileList) {
      this.formRegister.iELTSCertificatePhotos = fileList
    },
    handleRemoveIELTS(file, fileList) {
      this.formRegister.iELTSCertificatePhotos = fileList
    },
    handleChangeTOEFL(file, fileList) {
      this.formRegister.tOEFLCertificatePhotos = fileList
    },
    handleRemoveTOEFL(file, fileList) {
      this.formRegister.tOEFLCertificatePhotos = fileList
    },
    previewImage(e) {
      this.popUpImageUrl = e.url
      this.toggleImagePopup = !this.toggleImagePopup
    },
    closeImg(e) {
      if (e.target.firstChild != null) {
        this.toggleImagePopup = !this.toggleImagePopup
      }
    },
    getApplyTo(testName) {
      return this.formRegister.applyTo.find(a => a.indexOf(testName) >= 0) != undefined
    },
    onSubmit(formName, createOrUpdate) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          const formData = new FormData()
          formData.set('UserId', this.currentUser.id)
          formData.set('Id', this.formRegister.id)
          formData.set('Status', RATER_STATUS.DOCUMENT_SUBMITTED)
          let count = 0
          for (const p of this.formRegister.iDCardPhotos) {
            console.log('ID')

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
            console.log('IELTS')
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
            console.log('TOEFL')
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

          raterService.updateCredential(formData).then(rs => {
            this.status = rs.status
            this.$notify({
              title: 'Success',
              message: 'Update success',
              type: 'success',
              duration: 2000
            })
          })
        } else {
          console.log('error submit!!')
          this.$notify.error({
            title: 'Error',
            message: 'Error occured!',
            duration: 2000
          })
          return false
        }
      })
    }
  }
}
</script>

<style scoped>
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

.inReview {
  padding: 5px 10px;
  font-size: 12px;
  border-radius: 4px;
  background-color: #ecf8ff;
  border-left: 5px solid #50bfff;
}

.verified{
  padding: 5px 10px;
  font-size: 12px;
  border-radius: 4px;
  background-color: #ecffee;
  border-left: 5px solid #12ee2f;
}

.denied{
  padding: 5px 10px;
  font-size: 12px;
  border-radius: 4px;
  background-color: #ffecec;
  border-left: 5px solid #ee1212;
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

.label-container{
  margin: 0 10px
}

.margin-container{
  margin:10px 30px 10px 30px;
}

.flex-box{
  display: flex;
  align-items: center;
  padding-bottom: 10px;
}

.hidden{
  display: none !important;
}

.button-container{
  margin: 20px 0 0 0;
}

.note-container{
  display: flex;
  margin: 10px 0 0 0;
}
.file-upload-form{
  margin-top:20px;
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
</style>
