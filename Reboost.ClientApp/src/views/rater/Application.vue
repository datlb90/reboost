<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15" class="col-border">
        <el-steps :active="1" align-center>
          <el-step title="Step 1" icon="el-icon-user" description="Create an account" />
          <el-step title="Step 2" icon="el-icon-upload" description="Upload credentials" />
          <el-step title="Step 3" icon="el-icon-circle-check" description="Complete trainning" />
          <el-step title="Step 4" icon="el-icon-edit-outline" description="Start rating" />
        </el-steps>
      </el-col>
    </el-row>
    <el-row class="row-flex">
      <el-col :span="15" class="col-border">
        <el-form ref="formRegister" :model="formRegister" label-width="180px" style="width:90%;">

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
            label="First Name"
            prop="firstName"
            :rules="[
              { required: true, message: 'First name is required'}
            ]"
          >
            <el-input v-model="formRegister.firstName" type="text" />
          </el-form-item>
          <el-form-item
            size="mini"
            label="Last Name"
            prop="lastName"
            :rules="[
              { required: true, message: 'Last name is required'}
            ]"
          >
            <el-input v-model="formRegister.lastName" type="text" />
          </el-form-item>
          <el-form-item
            size="mini"
            label="Gender"
            prop="gender"
            :rules="[
              { required: true, message: 'Gender is required'}
            ]"
          >
            <el-select v-model="formRegister.gender" placeholder="Please select your gender" style="width: 100%;">
              <el-option v-for="item in gender" :key="item.id" :label="item.name" :value="item.name" />
            </el-select>
          </el-form-item>
          <el-form-item
            size="mini"
            label="Occupation"
            prop="occupation"
            :rules="[
              { required: true, message: 'Occupation is required'}
            ]"
          >
            <el-input v-model="formRegister.occupation" placeholder="Please input your occupation" />
          </el-form-item>
          <el-form-item
            size="mini"
            label="First Language"
            prop="firstLanguage"
            :rules="[
              { required: true, message: 'First Language is required'}
            ]"
          >
            <el-select v-model="formRegister.firstLanguage" style="width:250px" filterable placeholder="Please select your first language">
              <el-option
                v-for="item in firstLanguage"
                :key="item.id"
                :label="item.name"
                :value="item.name"
              />
            </el-select>
          </el-form-item>
          <el-form-item size="mini" label="Apply to Become" prop="applyTo" :rules="[{ type: 'array', required: true, message: 'Apply to is required' }]">
            <div style="transform:translateY(5px)">
              <el-checkbox-group v-model="formRegister.applyTo">
                <el-checkbox label="IELTS Rater" name="applyTo" />
                <el-checkbox label="TOEFL Rater" name="applyTo" />
              </el-checkbox-group>
            </div>
          </el-form-item>
          <div v-if="applyToIELTSChecked">
            <el-form-item
              size="mini"
              label="IELTS Test Scores"
              :rules="[
                { required: true, message: 'IELTS Test Scores is required'}
              ]"
            >
              <el-row :span="24" style="display: flex; flex-wrap: wrap;">
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.writting" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.writting" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Writting</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.reading" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.reading" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Reading</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.listening" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.listening" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Listening</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.speaking" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.speaking" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Speaking</label>
                  </el-input>
                </el-form-item>
              </el-row></el-form-item>
            <el-form-item
              size="mini"
              label="IELTS Credentials"
              prop="iELTSCertificatePhotos"
              :rules="[
                { required: true, message: 'IELTS Credentials Photos is required'}]"
            >
              <el-upload class="upload-demo" action="" :on-preview="previewImage" :file-list="formRegister.iELTSCertificatePhotos" :on-change="handleChangeIELTS" :on-remove="handleRemoveIELTS" :auto-upload="false" list-type="picture">
                <el-button type="primary">Click to upload</el-button>
                <div slot="tip" class="el-upload__tip">
                  <p>Please upload your IELTS test result, and any other supporting credidentials you may have. Files must be less than 500kb in size.</p>
                </div>
              </el-upload>
            </el-form-item>
          </div>
          <div v-if="applyToTOEFLChecked">
            <el-form-item
              size="mini"
              label="TOEFL Test Scores"
              :rules="[
                { required: true, message: 'TOEFL Test Scores is required'}
              ]"
            >
              <el-row :span="24" style="display: flex; flex-wrap: wrap;">
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.writting" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.writting" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Writting</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.reading" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.reading" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Reading</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.listening" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.listening" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Listening</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.speaking" :rules="[ { required: true, message: 'Required' } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.speaking" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">Speaking</label>
                  </el-input>
                </el-form-item>
              </el-row>
            </el-form-item>
            <el-form-item
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
          </div>

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
          <el-form-item
            size="mini"
            label="Biography"
          >
            <el-input v-model="formRegister.biography" type="textarea" :rows="5" placeholder="Tell us a little bit about yourself and the reason why you apply to become our rater" />
          </el-form-item>
          <el-form-item v-if="raterId" size="mini" label="Note">
            <el-input v-model="formRegister.note" type="textarea" :rows="5" placeholder="Note" />
          </el-form-item>
          <el-form-item size="mini" style="margin: 0;">
            <el-button v-if="!raterId" type="primary" size="mini" @click="onSubmit('formRegister', 'create')">Create</el-button>
            <el-button v-if="!raterId" size="mini">Cancel</el-button>
            <el-button v-if="raterId" class="button" size="mini" type="primary" @click="onSubmit('formRegister', 'update')">Save</el-button>
            <el-button v-if="raterId" class="button" size="mini" type="success" @click="updateStatus('Approved')">Approve</el-button>
            <el-button v-if="raterId" class="button" size="mini" type="danger" @click="updateStatus('Rejected')">Reject</el-button>
          </el-form-item>
        </el-form>
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
import raterService from '../../services/rater.service'
import moment from 'moment'
// import Notification from 'element-ui'
import * as mapUtil from '@/utils/model-mapping'
import * as stringUtil from '@/utils/string'

export default {
  name: 'Application',
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
          writting: null,
          reading: null,
          listening: null,
          speaking: null
        },
        toeflTestScore: {
          writting: null,
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
      popUpImageUrl: null,
      portraitImg: true
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
      const w = require('../../assets/defaultFormRaterData.json')
      this.firstLanguage = w.Languages
      this.gender = w.Gender

      for (let i = 0; i <= 9; i += 0.5) {
        this.ieltsScores.push(i.toFixed(1))
      }
      this.ieltsScores.reverse()
      for (let i = 0; i <= 30; i += 1) {
        this.toeflScores.push(i)
      }
      this.toeflScores.reverse()
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
        console.log('ID', this.$route.params.id)
        this.raterId = this.$route.params.id
        this.loadDetail(this.$route.params.id)
      }
    },
    loadDetail(id) {
      console.log('load detail', mapUtil)
      raterService.getById(id).then(rs => {
        console.log('result load detail', rs)
        this.formRegister = mapUtil.map(rs, this.formRegister)
        this.formRegister.appliedDate = rs.appliedDate.toString()
        this.formRegister.appliedDate = this.formRegister.appliedDate.slice(0, 10)
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

        // Score
        if (rs.user.userScores) {
          if (this.getApplyTo('IELTS')) {
            this.formRegister.ieltsTestScore.listening = rs.user.userScores.find(s => s.sectionId == 5).score
            this.formRegister.ieltsTestScore.reading = rs.user.userScores.find(s => s.sectionId == 6).score
            this.formRegister.ieltsTestScore.speaking = rs.user.userScores.find(s => s.sectionId == 7).score
            this.formRegister.ieltsTestScore.writting = rs.user.userScores.find(s => s.sectionId == 8).score
          }
          if (this.getApplyTo('TOEFL')) {
            this.formRegister.toeflTestScore.listening = rs.user.userScores.find(s => s.sectionId == 1).score
            this.formRegister.toeflTestScore.reading = rs.user.userScores.find(s => s.sectionId == 2).score
            this.formRegister.toeflTestScore.speaking = rs.user.userScores.find(s => s.sectionId == 3).score
            this.formRegister.toeflTestScore.writting = rs.user.userScores.find(s => s.sectionId == 4).score
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
          const formData = new FormData()

          formData.set('User[firstName]', this.formRegister.firstName)
          formData.set('User[lastName]', this.formRegister.lastName)
          formData.set('Gender', this.formRegister.gender)
          formData.set('Occupation', this.formRegister.occupation)
          formData.set('Biography', this.formRegister.biography)
          formData.set('Note', this.formRegister.note)
          formData.set('firstLanguage', this.formRegister.firstLanguage)
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
                formData.append(`UploadedFiles`, stringUtil.stof(p.url, p.name))
              }
            }
          }

          const scores = []

          if (this.getApplyTo('IELTS')) {
            scores.push(...[
              { sectionId: 5, score: this.formRegister.ieltsTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 6, score: this.formRegister.ieltsTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 8, score: this.formRegister.ieltsTestScore.writting, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 7, score: this.formRegister.ieltsTestScore.speaking, updatedDate: moment().format('yyyy-MM-DD') }
            ])
          }
          if (this.getApplyTo('TOEFL')) {
            scores.push(...[
              { sectionId: 1, score: this.formRegister.toeflTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 2, score: this.formRegister.toeflTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 4, score: this.formRegister.toeflTestScore.writting, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 3, score: this.formRegister.toeflTestScore.speaking, updatedDate: moment().format('yyyy-MM-DD') }
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
              this.$notify({
                title: 'Success',
                message: 'Created success',
                type: 'success',
                duration: 2000
              })
              this.$router.push('/rater/application/status/' + rs.id)
            })
          } else if (createOrUpdate == 'update') {
            formData.set('Id', this.formRegister.id)
            raterService.update(formData).then(rs => {
              this.$notify({
                title: 'Success',
                message: 'Update success',
                type: 'success',
                duration: 2000
              })
            })
          }
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
    },
    updateStatus(status) {
      raterService.updateStatus(this.raterId, status).then(rs => {
        this.formRegister.status = status
        this.$notify.error({
          title: 'Error',
          message: 'Error occured!',
          duration: 2000
        })
      })
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
    getApplyTo(testName) {
      return this.formRegister.applyTo.find(a => a.indexOf(testName) >= 0) != undefined
    },
    previewImage(e) {
      this.popUpImageUrl = e.url
      this.toggleImagePopup = !this.toggleImagePopup
      console.log(this.$refs.previewImg)
    },
    closeImg(e) {
      if (e.target.firstChild != null) {
        this.toggleImagePopup = !this.toggleImagePopup
      }
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

