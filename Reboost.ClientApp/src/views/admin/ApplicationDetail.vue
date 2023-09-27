<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15" class="col-border">
        <el-steps :active="formRegister.status === RATER_STATUS.APPLIED ? 1 : formRegister.status === RATER_STATUS.APPROVED ? 3 : 2" align-center>
          <el-step title="Step 1" icon="el-icon-user" :description="messageTranslates('appDetail', 'createAnAccount')" />
          <el-step title="Step 2" icon="el-icon-upload" :description="messageTranslates('appDetail', 'uploadCredential')" />
          <el-step title="Step 3" icon="el-icon-circle-check" :description="messageTranslates('appDetail', 'completeTraining')" />
          <el-step title="Step 4" icon="el-icon-edit-outline" :description="messageTranslates('appDetail', 'startRating')" />
        </el-steps>
      </el-col>
    </el-row>
    <el-row class="row-flex">
      <el-col :span="15" class="col-border">
        <el-form ref="formRegister" :model="formRegister" label-width="180px" style="width:90%;">

          <el-form-item v-if="raterId" size="mini" :label="messageTranslates('appDetail', 'currentStatus')">
            <el-tag
              :type="
                formRegister.status === RATER_STATUS.APPROVED || formRegister.status === RATER_STATUS.TRAINING || formRegister.status === RATER_STATUS.TRAINING_COMPLETED
                  ? 'success'
                  : formRegister.status === RATER_STATUS.APPLIED
                    ? 'primary'
                    : formRegister.status === RATER_STATUS.REJECTED
                      ? 'danger'
                      : 'warning'
              "
              disable-transitions
            >{{ constantTranslate('RATER_STATUS', formRegister.status) }}</el-tag>
          </el-form-item>

          <el-form-item v-if="raterId" size="mini" :label="messageTranslates('appDetail', 'appliedDate')" prop="appliedDate">
            <el-input
              v-model="formRegister.appliedDate"
              :disabled="true"
            />
          </el-form-item>

          <el-form-item
            size="mini"
            :label="messageTranslates('appDetail', 'firstName')"
            prop="firstName"
            :rules="[
              { required: true, message: messageTranslates('appDetail', 'firstNameRequired')}
            ]"
          >
            <el-input v-model="formRegister.firstName" type="text" />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('appDetail', 'lastName')"
            prop="lastName"
            :rules="[
              { required: true, message: messageTranslates('appDetail', 'lastNameRequired')}
            ]"
          >
            <el-input v-model="formRegister.lastName" type="text" />
          </el-form-item>

          <el-form-item v-if="raterId" size="mini" label="Email" prop="email">
            <el-input
              v-model="formRegister.email"
              :disabled="true"
            />
          </el-form-item>

          <el-form-item
            size="mini"
            :label="messageTranslates('appDetail', 'gender')"
            prop="gender"
            :rules="[
              { required: true, message: messageTranslates('appDetail', 'genderRequired')}
            ]"
          >
            <el-select v-model="formRegister.gender" :placeholder="messageTranslates('appDetail', 'pleaseSelectGender')" style="width: 100%;">
              <el-option v-for="item in gender" :key="item.id" :label="item.name" :value="item.name" />
            </el-select>
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('appDetail', 'occupation')"
            prop="occupation"
            :rules="[
              { required: true, message: messageTranslates('appDetail', 'occupationRequired')}
            ]"
          >
            <el-input v-model="formRegister.occupation" :placeholder="messageTranslates('appDetail', 'pleaseSelectOccupation')" />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('appDetail', 'firstLanguage')"
            prop="firstLanguage"
            :rules="[
              { required: true, message: messageTranslates('appDetail', 'firstLanguageRequired')}
            ]"
          >
            <el-select v-model="formRegister.firstLanguage" style="width:40%" filterable :placeholder="messageTranslates('appDetail', 'pleaseSelectFirstLanguage')">
              <el-option
                v-for="item in firstLanguage"
                :key="item.id"
                :label="item.name"
                :value="item.name"
              />
            </el-select>
          </el-form-item>
          <el-form-item size="mini" :label="messageTranslates('appDetail', 'applyTo')" prop="applyTo" :rules="[{ type: 'array', required: true, message: messageTranslates('appDetail', 'applyToRequired') }]">
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
              :label="messageTranslates('appDetail', 'ieltsTestScores')"
              :rules="[
                { required: true, message: messageTranslates('appDetail', 'ieltsTestScoresRequired')}
              ]"
            >
              <el-row :span="24" style="display: flex; flex-wrap: wrap;">
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.writing" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.writing" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'writing') }}</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.reading" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.reading" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'reading') }}</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.listening" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.listening" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'listening') }}</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="ieltsTestScore.speaking" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.ieltsTestScore.speaking" placeholder="...">
                      <el-option v-for="item in ieltsScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'speaking') }}</label>
                  </el-input>
                </el-form-item>
              </el-row>
            </el-form-item>
            <el-form-item
              size="mini"
              :label="messageTranslates('appDetail', 'ieltsCredentials')"
              prop="iELTSCertificatePhotos"
              :rules="[
                { required: true, message: messageTranslates('appDetail', 'ieltsCredentialsRequired')}]"
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
                <el-button type="primary">{{ messageTranslates('appDetail', 'clickUpload') }}</el-button>
                <div slot="tip" class="el-upload__tip">
                  <p>{{ messageTranslates('appDetail', 'ieltsUploadTips') }}</p>
                </div>
              </el-upload>
            </el-form-item>
          </div>
          <div v-if="applyToTOEFLChecked">
            <el-form-item
              size="mini"
              :label="messageTranslates('appDetail', 'toeflTestScores')"
              :rules="[
                { required: true, message: messageTranslates('appDetail', 'toeflTestScoresRequired')}
              ]"
            >
              <el-row :span="24" style="display: flex; flex-wrap: wrap;">
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.writing" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.writing" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'writing') }}</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.reading" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.reading" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'reading') }}</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.listening" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.listening" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'listening') }}</label>
                  </el-input>
                </el-form-item>
                <el-form-item id="scoresSelection" style="margin-right: 10px;" size="mini" prop="toeflTestScore.speaking" :rules="[ { required: true, message: messageTranslates('appDetail', 'required') } ]">
                  <el-input id="scoresSelector">
                    <el-select slot="append" v-model="formRegister.toeflTestScore.speaking" placeholder="...">
                      <el-option v-for="item in toeflScores" :key="item" :label="item" :value="item" />
                    </el-select>
                    <label slot="prepend" style="width: 35px; color: #909399; font-size: 12px; margin: 0;">{{ messageTranslates('appDetail', 'speaking') }}</label>
                  </el-input>
                </el-form-item>
              </el-row>
            </el-form-item>
            <el-form-item
              size="mini"
              :label="messageTranslates('appDetail', 'toeflCredentials')"
              prop="tOEFLCertificatePhotos"
              :rules="[
                { required: true, message: messageTranslates('appDetail', 'toeflCredentialsRequired')}]"
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
                <el-button type="primary">{{ messageTranslates('appDetail', 'clickUpload') }}</el-button>
                <div slot="tip" class="el-upload__tip">
                  <p>{{ messageTranslates('appDetail', 'toeflUploadTips') }}</p>
                </div>
              </el-upload>
            </el-form-item>
          </div>

          <el-form-item
            size="mini"
            :label="messageTranslates('appDetail', 'photoId')"
            prop="iDCardPhotos"
            :rules="[
              { required: true, message: messageTranslates('appDetail', 'photoIdRequired')}]"
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
              <el-button type="primary">{{ messageTranslates('appDetail', 'clickUpload') }}</el-button>
              <div slot="tip" class="el-upload__tip">
                <p>{{ messageTranslates('appDetail', 'photoIdUploadTips') }}</p>
              </div>
            </el-upload>
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('appDetail', 'biography')"
          >
            <el-input v-model="formRegister.biography" type="textarea" :rows="5" :placeholder="messageTranslates('appDetail', 'biographyPlaceholder')" />
          </el-form-item>
          <el-form-item v-if="raterId" size="mini" :label="messageTranslates('appDetail', 'note')">
            <el-input v-model="formRegister.note" type="textarea" :rows="5" :placeholder="messageTranslates('appDetail', 'notePlaceholder')" />
          </el-form-item>
          <el-form-item v-if="raterId && formRegister.status !== RATER_STATUS.REJECTED" size="mini" style="margin: 0;">
            <el-button v-if="!raterId" type="primary" size="mini" @click="onSubmit('formRegister', 'create')">{{ messageTranslates('appDetail', 'create') }}</el-button>
            <el-button v-if="!raterId" size="mini">{{ messageTranslates('appDetail', 'cancel') }}</el-button>
            <el-button v-if="raterId" class="button" size="mini" type="primary" @click="onSubmit('formRegister', 'update')">{{ messageTranslates('appDetail', 'save') }}</el-button>
            <el-button v-if="raterId && (formRegister.status === RATER_STATUS.APPLIED || formRegister.status === RATER_STATUS.DOCUMENT_SUBMITTED)" class="button" size="mini" type="success" @click="updateStatus(RATER_STATUS.TRAINING)">{{ messageTranslates('appDetail', 'approveTraining') }}</el-button>
            <!-- <el-button v-if="raterId && formRegister.status !== RATER_STATUS.APPLIED && formRegister.status !== RATER_STATUS.APPROVED && formRegister.status !== RATER_STATUS.TRAINING && (formRegister.status === RATER_STATUS.TRAINING_COMPLETED)" class="button" size="mini" type="success" @click="updateStatus(RATER_STATUS.APPROVED)">Approve</el-button> -->
            <el-button v-if="raterId && (formRegister.status === RATER_STATUS.APPLIED || formRegister.status === RATER_STATUS.DOCUMENT_SUBMITTED || formRegister.status === RATER_STATUS.TRAINING_COMPLETED) " class="button" size="mini" type="danger" @click="updateStatus(RATER_STATUS.REJECTED)">{{ messageTranslates('appDetail', 'reject') }}</el-button>
            <el-button v-if="raterId && (formRegister.status === RATER_STATUS.APPLIED || formRegister.status === RATER_STATUS.DOCUMENT_SUBMITTED)" class="button" size="mini" type="primary" @click="updateStatus(RATER_STATUS.DOCUMENT_REQUESTED)">{{ messageTranslates('appDetail', 'documentRequest') }}</el-button>
            <el-button v-if="completedTraining('IELTS') && formRegister.status !== RATER_STATUS.APPROVED" class="button" size="mini" type="primary" @click="redirectToTraining('IELTS')">{{ messageTranslates('appDetail', 'viewIELTSTraining') }}</el-button>
            <el-button v-if="completedTraining('TOEFL') && formRegister.status !== RATER_STATUS.APPROVED" class="button" size="mini" type="primary" @click="redirectToTraining('TOEFL')">{{ messageTranslates('appDetail', 'viewTOEFLTraining') }}</el-button>
            <!-- <el-dropdown v-if="completedTraining(formRegister,'IELTS') && formRegister.status !== RATER_STATUS.APPROVED" style="margin: 0 10px 0" size="mini" split-button type="primary" @click="redirectToTraining('IELTS')" @command="trainingDropdownClick">
              IELTS
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item :command="{type:'IELTSTraining',action:'approve'}">Approve for Training</el-dropdown-item>
                <el-dropdown-item :command="{type:'IELTSTraining',action:'reject'}">Revision</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
            <el-dropdown v-if="completedTraining(formRegister,'TOEFL') && formRegister.status !== RATER_STATUS.APPROVED" style="margin: 0 10px 0" size="mini" split-button type="primary" @click="redirectToTraining('TOEFL')" @command="trainingDropdownClick">
              TOEFL
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item :command="{type:'TOEFLTraining',action:'approve'}">Approve for Training</el-dropdown-item>
                <el-dropdown-item :command="{type:'TOEFLTraining',action:'reject'}">Revision</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown> -->
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
import reviewService from '../../services/review.service'
import moment from 'moment'
// import Notification from 'element-ui'
import * as mapUtil from '@/utils/model-mapping'
import * as stringUtil from '@/utils/string'
import { RATER_STATUS, RATER_TRAINING_STATUS } from '../../app.constant'

export default {
  name: 'Application',
  data() {
    return {
      formRegister: {
        id: '',
        firstName: '',
        lastName: '',
        email: '',
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
        note: null,
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
      RATER_STATUS: RATER_STATUS,
      cloneFormDetail: null,
      raterTraining: null
    }
  },
  computed: {
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
      const w = require('../../assets/data.json')
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
        this.raterId = this.$route.params.id
        this.loadDetail(this.$route.params.id)
      }
    },
    loadDetail(id) {
      raterService.getById(id).then(rs => {
        console.log('result load detail', rs)
        this.$store.dispatch('rater/setSelectedRater', rs)
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
        // Email
        this.formRegister.email = rs.user.email

        // Create a clone of form's data
        this.cloneFormDetail = Object.assign({}, this.formRegister)
      })

      reviewService.getRaterTrainings(id).then(rs => {
        this.raterTraining = rs
      })
    },
    async onSubmit(formName, createOrUpdate, hideSaveNotify) {
      // Get changed fields
      var changedFieldsName = []
      for (var e in this.formRegister) {
        if (this.formRegister[e] !== this.cloneFormDetail[e]) {
          changedFieldsName.push(e)
        }
      }

      return new Promise((resolve, reject) => {
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
            formData.set('Status', RATER_STATUS.APPLIED)
            formData.set('AppliedDate', moment().format('yyyy-MM-DD hh:mm:ss'))
            formData.set('LastActivityDate', moment().format('yyyy-MM-DD hh:mm:ss'))
            formData.set('UserId', this.formRegister.userId)

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
                { sectionId: 5, score: this.formRegister.ieltsTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
                { sectionId: 6, score: this.formRegister.ieltsTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
                { sectionId: 8, score: this.formRegister.ieltsTestScore.writing, updatedDate: moment().format('yyyy-MM-DD') },
                { sectionId: 7, score: this.formRegister.ieltsTestScore.speaking, updatedDate: moment().format('yyyy-MM-DD') }
              ])
            }
            if (this.getApplyTo('TOEFL')) {
              scores.push(...[
                { sectionId: 1, score: this.formRegister.toeflTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
                { sectionId: 2, score: this.formRegister.toeflTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
                { sectionId: 4, score: this.formRegister.toeflTestScore.writing, updatedDate: moment().format('yyyy-MM-DD') },
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
                this.$notify.success({
                  title: 'Success',
                  message: 'Created successfully',
                  type: 'success',
                  duration: 2000
                })
                resolve(rs)
                this.$router.push('/rater/application/status/' + rs.id)
              })
            } else if (createOrUpdate == 'update') {
              formData.set('Id', this.formRegister.id)
              formData.set('Status', this.formRegister.status)

              // if change fields not includes photo then delete upload files
              if (!changedFieldsName.includes('iDCardPhotos') && !changedFieldsName.includes('iELTSCertificatePhotos') && !changedFieldsName.includes('tOEFLCertificatePhotos')) {
                formData.delete('UploadedFiles')
              }

              raterService.update(formData).then(rs => {
                if (typeof (hideSaveNotify) == 'undefined') {
                  this.$notify.success({
                    title: 'Success',
                    message: 'Update successfully',
                    type: 'success',
                    duration: 2000
                  })
                }
                resolve(rs)
              })
            }
          } else {
            this.$notify.error({
              title: 'Error',
              message: 'Error occured!',
              type: 'error',
              duration: 2000
            })

            reject()
          }
        })
      })
    },
    async updateStatus(status) {
      if (status === RATER_STATUS.DOCUMENT_REQUESTED && (!this.formRegister.note || this.formRegister.note?.trim().length === 0)) {
        this.$notify.error({
          title: 'Note Required!',
          message: 'Please input a note for rater.',
          type: 'error',
          duration: 2000
        })
        return
      }

      await this.onSubmit('formRegister', 'update', true).then(r => {
        raterService.updateStatus(this.raterId, status).then(rs => {
          this.formRegister.status = status
          if (rs.status == RATER_STATUS.TRAINING) {
            this.$notify.success({
              title: RATER_STATUS.TRAINING,
              message: 'Rater approved! Rater can start training now.',
              type: 'success',
              duration: 2000
            })
          } else if (rs.status == RATER_STATUS.REJECTED) {
            this.$notify.error({
              title: RATER_STATUS.REJECTED,
              message: 'Rater Rejected!',
              type: 'error',
              duration: 2000
            })
          } else if (rs.status == RATER_STATUS.DOCUMENT_REQUESTED) {
            this.$notify.info({
              title: RATER_STATUS.DOCUMENT_REQUESTED,
              message: 'Rater Document Requested!',
              type: 'info',
              duration: 2000
            })
          } else if (rs.status == RATER_STATUS.REVISION_REQUESTED) {
            this.$notify.info({
              title: RATER_STATUS.DOCUMENT_REQUESTED,
              message: 'Rater Revision Requested!',
              type: 'info',
              duration: 2000
            })
          } else if (rs.status == RATER_STATUS.APPROVED) {
            this.$notify.info({
              title: RATER_STATUS.APPROVED,
              message: 'Rater approved!',
              type: 'info',
              duration: 2000
            })
          }
        })
      })
    },
    handleRemoveIdPhoto(file, fileList) {
      this.formRegister.iDCardPhotos = fileList
    },
    handleChangeIdPhoto(file, fileList) {
      if (this.isImage(file.name) || this.isVideo(file.name)) {
        this.formRegister.iDCardPhotos = fileList
      } else {
        fileList.pop()
      }
    },
    handleChangeIELTS(file, fileList) {
      if (this.isImage(file.name) || this.isVideo(file.name)) {
        this.formRegister.iELTSCertificatePhotos = fileList
      } else {
        fileList.pop()
      }
    },
    handleRemoveIELTS(file, fileList) {
      this.formRegister.iELTSCertificatePhotos = fileList
    },
    handleChangeTOEFL(file, fileList) {
      if (this.isImage(file.name) || this.isVideo(file.name)) {
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
      if (this.isVideo(e.name)) {
        this.popUpImageUrl.type = 'videos'
      }
      this.popUpImageUrl.url = e.url
      this.toggleImagePopup = !this.toggleImagePopup
      console.log(e, this.isImage(e.name), this.isVideo(e.name))
    },
    closeImg(e) {
      if (e.target.firstChild != null) {
        this.toggleImagePopup = !this.toggleImagePopup
      }
    },
    completedTraining(type) {
      var t = this.raterTraining?.filter(r => r.test.trim() == type.trim() && (r.status == RATER_TRAINING_STATUS.COMPLETED || r.status == RATER_TRAINING_STATUS.REVISION_COMPLETED))[0]
      if (t) {
        return true
      }
      return false
    },
    async trainingDropdownClick(e) {
      await this.onSubmit('formRegister', 'update', true)

      var t = this.getAllReviews.filter(r => r.reviewerId == this.formRegister.userId && r.reviewData.length > 0 && r.status.includes(e.type))[0]

      var newStatus = e.action == 'approve' ? e.type + RATER_STATUS.APPROVED : e.type + RATER_STATUS.REVISION

      reviewService.changeReviewStatus(t.id, { status: newStatus }).then(rs => {
        if (rs.status.includes(RATER_STATUS.REVISION)) {
          this.$notify.error({
            title: RATER_STATUS.REVISION,
            message: 'Submitted Training Revision Requested!',
            type: 'error',
            duration: 2000
          })
        } else {
          if (e.action === 'approve') {
            this.$notify.success({
              title: RATER_STATUS.APPROVED,
              message: 'Submitted Training Approved!',
              type: 'success',
              duration: 2000
            })
          }
        }
      })
    },
    redirectToTraining(type) {
      var t = this.raterTraining.filter(r => r.test.trim() == type.trim() && (r.status == RATER_TRAINING_STATUS.COMPLETED || r.status == RATER_TRAINING_STATUS.REVISION_COMPLETED))[0]
      var redirectlink = type === 'IELTS' ? '/review/9/220/' : '/review/1/1/'
      redirectlink += t?.reviewId
      this.$router.push(redirectlink)
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

