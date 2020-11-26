<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="14" class="col-border">
        <el-steps :active="1" align-center>
          <el-step title="Step 1" icon="el-icon-user" description="Create an account" />
          <el-step title="Step 2" icon="el-icon-upload" description="Upload credentials" />
          <el-step title="Step 3" icon="el-icon-circle-check" description="Complete trainning" />
          <el-step title="Step 4" icon="el-icon-edit-outline" description="Start rating" />
        </el-steps>
      </el-col>
    </el-row>
    <el-row class="row-flex">
      <el-col :span="14">
        <div class="tip">
          <p>Please fill out some basic information about yourself and upload your official test scores or any other supporting documents so we can verify your proficiency to become our rater.
          </p>
        </div>
      </el-col>
    </el-row>
    <el-row class="row-flex">
      <el-col :span="14" class="col-border padding-30">
        <el-form ref="formRegister" :model="formRegister" label-width="180px" style="width:90%;">

          <el-form-item v-if="raterId" label="Current Status">
            <el-tag
              :type="
                formRegister.status === 'Approved'
                  ? 'success'
                  : formRegister.status === 'Applied'
                    ? 'primary'
                    : formRegister.status === 'Training Completed'
                      ? 'warning'
                      : 'danger'
              "
              disable-transitions
            >{{ formRegister.status }}</el-tag>
        </el-form-item>
        
        <el-form-item v-if="raterId" label="Applied Date" prop="appliedDate">
          <el-date-picker v-model="formRegister.appliedDate" type="date" placeholder="Pick a day"></el-date-picker>
        </el-form-item>
      
        <el-form-item label="First Name" prop="firstName" :rules="[
          { required: true, message: 'First name is required'}
        ]">
          <el-input type="text" v-model="formRegister.firstName"></el-input>
        </el-form-item>
        <el-form-item label="Last Name" prop="lastName" :rules="[
          { required: true, message: 'Last name is required'}
        ]">
          <el-input type="text" v-model="formRegister.lastName"></el-input>
        </el-form-item>
        <el-form-item label="Gender" prop="gender" :rules="[
          { required: true, message: 'Gender is required'}
        ]">
          <el-select v-model="formRegister.gender" placeholder="Please select your gender" style="width: 100%;">
            <el-option v-for="item in gender" :key="item.id" :label="item.name" :value="item.name">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="Occupation" prop="occupation" :rules="[
          { required: true, message: 'Occupation is required'}
        ]">
          <el-select v-model="formRegister.occupation" placeholder="Please select an suits you best" style="width: 100%;">
            <el-option v-for="item in occupation" :key="item.id" :label="item.name" :value="item.name">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="First Language" prop="firstLanguage" :rules="[
          { required: true, message: 'First Language is required'}
        ]">
          <el-select v-model="formRegister.firstLanguage" placeholder="Please select an suits you best" style="width: 100%;">
            <el-option v-for="item in firstLanguage" :key="item.id" :label="item.name" :value="item.name">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="Apply to Become">
          <div style="display: flex; transform: translateY(40%);">
            <el-checkbox label="IELTS Rater" v-model="formRegister.applyTo.IELTS" style="margin-right: 20px;"></el-checkbox>
            <el-checkbox label="TOEFL Rater" v-model="formRegister.applyTo.TOEFL"></el-checkbox>
          </div>
        </el-form-item>
        <div v-if="formRegister.applyTo.IELTS">
          <el-form-item label="IELTS Test Scores">
            <el-row :gutter="24" style="display:flex; justify-content: space-between;">
              <el-col :span="6">
                <el-select v-model="formRegister.ieltsTestScore.writting" placeholder="Writting">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                </el-select>
              </el-col>
              <el-col :span="6">
                <el-select v-model="formRegister.ieltsTestScore.reading" placeholder="Writting">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                </el-select>
              </el-col>
              <el-col :span="6">
                <el-select v-model="formRegister.ieltsTestScore.listening" placeholder="Writting">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                </el-select>
              </el-col>
              <el-col :span="6">
                <el-select v-model="formRegister.ieltsTestScore.speaking" placeholder="Writting">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                </el-select>
              </el-col>
            </el-row>
          </el-form-item>
          <el-form-item label="IELTS Credentials" prop="iELTSCertificatePhotos">
            <el-upload class="upload-demo" action="" :file-list="formRegister.iELTSCertificatePhotos" :on-change="handleChangeIELTS" :on-remove="handleRemoveIELTS" :auto-upload="false" list-type="picture">
              <el-button size="small" type="primary">Click to upload</el-button>
              <div slot="tip" class="el-upload__tip">
                <p>Please upload your IELTS test result, and any other supporting credidentials you may have. Files must be less than 500kb in size.</p>
              </div>
            </el-upload>
          </el-form-item>
        </div>
        <div v-if="formRegister.applyTo.TOEFL">
          <el-form-item label="TOEFL Test Scores">
            <el-row :gutter="24" style="display:flex; justify-content: space-between;">
              <el-col :span="6">
                <el-select v-model="formRegister.toeflTestScore.writting" placeholder="Writting">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                  </el-select>
              </el-col>
              <el-col :span="6">
                <el-select v-model="formRegister.toeflTestScore.reading" placeholder="Reading">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                </el-select>
              </el-col>
              <el-col :span="6">
                <el-select v-model="formRegister.toeflTestScore.listening" placeholder="Listening">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                </el-select>
              </el-col>
              <el-col :span="6">
                <el-select v-model="formRegister.toeflTestScore.speaking" placeholder="Speaking">
                  <el-option v-for="item in score" :key="item" :label="item" :value="item">
                  </el-option>
                </el-select>
              </el-col>
            </el-row>
          </el-form-item>
          <el-form-item label="TOEFL Credentials" prop="tOEFLCertificatePhotos">
            <el-upload class="upload-demo" action=""
            :on-change="handleChangeTOEFL" 
            :on-remove="handleRemoveTOEFL" 
            :file-list="formRegister.tOEFLCertificatePhotos" 
            :auto-upload="false" list-type="picture">
              <el-button size="small" type="primary">Click to upload</el-button>
              <div slot="tip" class="el-upload__tip">
                <p>Please upload a form of photo identification such as ID card, driver license, or passport. The file must be less than 500kb in size.</p>
              </div>
            </el-upload>
          </el-form-item>
        </div>

        <el-form-item ref="photoId" label="Photo ID" prop="iDCardPhotos">
          <el-upload class="upload-demo" action="" :file-list="formRegister.iDCardPhotos" :on-change="handleChangeIdPhoto" :on-remove="handleRemoveIdPhoto" :auto-upload="false" list-type="picture">
            <el-button size="small" type="primary">Click to upload</el-button>
            <div slot="tip" class="el-upload__tip">
              <p>Please upload a form of photo identification such as ID card, driver license, or passport. The file must be less than 500kb in size.</p>
            </div>
          </el-upload>
        </el-form-item>
        <el-form-item label="Biography">
          <el-input type="textarea" v-model="formRegister.biography" :rows="5" placeholder="Tell us a little bit about yourself and the reason why you apply to become our rater"></el-input>
        </el-form-item>
        <el-form-item v-if="raterId" label="Note">
          <el-input type="textarea" v-model="formRegister.note" :rows="5" placeholder="Note"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button v-if="!raterId" type="primary" @click="onSubmit('formRegister', 'create')">Create</el-button>
          <el-button v-if="!raterId">Cancel</el-button>
          <el-button v-if="raterId" class="button" size="medium" type="primary" @click="onSubmit('formRegister', 'update')">Save</el-button>
          <el-button v-if="raterId" class="button" size="medium" type="success" @click="updateStatus('Approved')">Approve</el-button>
          <el-button v-if="raterId" class="button" size="medium" type="danger" @click="updateStatus('Rejected')">Reject</el-button>
        </el-form-item>
      </el-form>
    </el-col>
  </el-row>
</div>
</template>

<script>
// @ is an alias to /src
import raterService from '../../services/rater.service'
import lookupService from '../../services/lookup.service'
import moment from 'moment';
import * as mapUtil from '@/utils/model-mapping';
import * as stringUtil from '@/utils/string';

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
        applyTo: {
          IELTS: false,
          TOEFL: false
        },
        ieltsTestScore: {
          writting: 0,
          reading: 0,
          listening: 0,
          speaking: 0
        },
        toeflTestScore: {
          writting: 0,
          reading: 0,
          listening: 0,
          speaking: 0
        },
        iDCardPhotos: [],
        iELTSCertificatePhotos: [],
        tOEFLCertificatePhotos: [],
        biography: '',
        note: '',
        appliedDate: new Date()
      },
      occupation: [],
      firstLanguage: [],
      gender: [],
      raterId: ''
    }
  },
  computed: {
    score: function() {
      var x = []
      for (let i = 0; i <= 100; i++) { x.push(i) }
      return x
    }
    // gender: function () {
    //   return ['male', 'female']
    // },
    // occupation: function () {
    //   var x = [];
    //   for (let i = 0; i <= 5; i++)
    //     x.push(i);
    //   return x
    // },
    // firstLanguage() {
    //   var x = [];
    //   for (let i = 0; i <= 5; i++)
    //     x.push(i);
    //   return x
    // }
  },
  mounted() {
    this.onLoad()
  },
  methods: {
    onLoad(){
      const w = require('../../assets/defaultFormRaterData.json')
      this.firstLanguage = w.Languages
      this.occupation = w.Occupation
      this.gender = w.Gender
      
      // lookupService.getByType('OCCUPATION').then(rs => {
      //   this.occupation = rs;
      // });
      // lookupService.getByType('FIRST_LANGUAGE').then(rs => {
      //   this.firstLanguage = rs;
      // });
      // lookupService.getByType('GENDER').then(rs => {
      //   this.gender = rs;
      // });

      if(this.$route.params.id){
        console.log("ID",this.$route.params.id)
        this.raterId = this.$route.params.id;
        this.loadDetail(this.$route.params.id);
      }
    },
    loadDetail(id) {
      console.log('load detail', mapUtil)
      raterService.getById(id).then(rs => {
        console.log('result load detail', rs);
        this.formRegister = mapUtil.map(rs, this.formRegister);

        //Files
        if(rs.raterCredentials){
          for(let f of rs.raterCredentials){
            if(f.credentialType == 'IDPhoto'){
              this.formRegister.iDCardPhotos.push({ name: f.fileName, url: 'data:image/png;base64,' + f.data });
            }
            else if(f.credentialType == 'TOEFLPhoto'){
              this.formRegister.tOEFLCertificatePhotos.push({ name: f.fileName, url: 'data:image/png;base64,' + f.data });
            }
            else if(f.credentialType == 'IELTSPhoto'){
              this.formRegister.iELTSCertificatePhotos.push({ name: f.fileName, url: 'data:image/png;base64,' + f.data });
            }
          }
        }
        
        //Score
        if(rs.user.userScores){
          if(this.formRegister.applyTo.IELTS){
            this.formRegister.ieltsTestScore.listening = rs.user.userScores.find(s => s.sectionId == 5).score;
            this.formRegister.ieltsTestScore.reading = rs.user.userScores.find(s => s.sectionId == 6).score;
            this.formRegister.ieltsTestScore.speaking = rs.user.userScores.find(s => s.sectionId == 7).score;
            this.formRegister.ieltsTestScore.writting = rs.user.userScores.find(s => s.sectionId == 8).score;
          }
          if(this.formRegister.applyTo.TOEFL){
            this.formRegister.toeflTestScore.listening = rs.user.userScores.find(s => s.sectionId == 1).score;
            this.formRegister.toeflTestScore.reading = rs.user.userScores.find(s => s.sectionId == 2).score;
            this.formRegister.toeflTestScore.speaking = rs.user.userScores.find(s => s.sectionId == 3).score;
            this.formRegister.toeflTestScore.writting = rs.user.userScores.find(s => s.sectionId == 4).score;
          }
        }
        
        //Apply to
        // if(rs.applyTo){
        //   let items = rs.applyTo.split(',');
        //   for(let key in this.formRegister.applyTo){
        //     let exist = items.find(i => i == key);
        //     if(exist){
        //       this.formRegister.applyTo[key] = true;
        //     }
        //   }
        // }

        //First name
        this.formRegister.firstName = rs.user.firstName;
        //Last name
        this.formRegister.lastName = rs.user.lastName;
        
      });
    },
    onSubmit(formName, createOrUpdate) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          console.log(this.formRegister);

          const formData = new FormData();
          
          formData.set("User[firstName]", this.formRegister.firstName);
          formData.set("User[lastName]", this.formRegister.lastName);
          formData.set("Gender", this.formRegister.gender);
          formData.set("Occupation", this.formRegister.occupation);
          formData.set("Biography", this.formRegister.biography);
          formData.set("Note", this.formRegister.note);
          formData.set("firstLanguage", this.formRegister.firstLanguage);
          formData.set("Status", 'Applied');
          formData.set("AppliedDate", moment().format('yyyy-MM-DD hh:mm:ss'));
          formData.set("LastActivityDate", moment().format('yyyy-MM-DD hh:mm:ss'));
          formData.set("UserId", 'c7b56124-7d74-4017-841d-0447084240e4'); //TODO: get current user id

          let count = 0;
          for(let p of this.formRegister.iDCardPhotos){
            formData.set(`RaterCredentials[${count}][TestId]`, '0');
            formData.set(`RaterCredentials[${count}][CredentialType]`, 'IDPhoto');
            formData.set(`RaterCredentials[${count}][FileName]`, p.name);
            formData.set(`RaterCredentials[${count}][FileExtension]`, stringUtil.getFileExtension(p.name));

            if(p.raw){
              formData.append(`UploadedFiles`, p.raw);
            }
            else{
              formData.append(`UploadedFiles`, stringUtil.stof(p.url, p.name));
            }
            count++;
          }
          
          if(this.formRegister.applyTo.IELTS){
            for(let p of this.formRegister.iELTSCertificatePhotos){
              formData.set(`RaterCredentials[${count}][TestId]`, '2');
              formData.set(`RaterCredentials[${count}][CredentialType]`, 'IELTSPhoto');
              formData.set(`RaterCredentials[${count}][FileName]`, p.name);
              formData.set(`RaterCredentials[${count}][FileExtension]`, stringUtil.getFileExtension(p.name));

              if(p.raw){
                formData.append(`UploadedFiles`, p.raw);
              }
              else{
                formData.append(`UploadedFiles`, stringUtil.stof(p.url, p.name));
              }
              
              count++;
            }
          }
          
          if(this.formRegister.applyTo.IELTS){
            for(let p of this.formRegister.tOEFLCertificatePhotos){
              formData.set(`RaterCredentials[${count}][TestId]`, '1');
              formData.set(`RaterCredentials[${count}][CredentialType]`, 'TOEFLPhoto');
              formData.set(`RaterCredentials[${count}][FileName]`, p.name);
              formData.set(`RaterCredentials[${count}][FileExtension]`, stringUtil.getFileExtension(p.name));

              if(p.raw){
                formData.append(`UploadedFiles`, p.raw);
              }
              else{
                formData.append(`UploadedFiles`, stringUtil.stof(p.url, p.name));
              }
            }
          }

          let scores = [];

          if(this.formRegister.applyTo.IELTS){
            scores.push(...[
              { sectionId: 5, score: this.formRegister.ieltsTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 6, score: this.formRegister.ieltsTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 8, score: this.formRegister.ieltsTestScore.writting, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 7, score: this.formRegister.ieltsTestScore.speaking, updatedDate: moment().format('yyyy-MM-DD') }
            ]);
          }
          if(this.formRegister.applyTo.TOEFL){
            scores.push(...[
              { sectionId: 1, score: this.formRegister.toeflTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 2, score: this.formRegister.toeflTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 4, score: this.formRegister.toeflTestScore.writting, updatedDate: moment().format('yyyy-MM-DD') },
              { sectionId: 3, score: this.formRegister.toeflTestScore.speaking, updatedDate: moment().format('yyyy-MM-DD') }
            ]);
          }

          count = 0;

          for(let s of scores){
            formData.set(`User[UserScores][${count}][SectionId]`, s.sectionId);
            formData.set(`User[UserScores][${count}][Score]`, s.score);
            formData.set(`User[UserScores][${count}][UpdatedDate]`, s.updatedDate);
            count++;
          }

          if(createOrUpdate == 'create'){
            raterService.insert(formData).then(rs => {
            this.$toasted.show('Upload success', {
              icons: 'shopping_cart',
              theme: 'bubble',
              position: 'top-right',
              duration: 3000
            })
            })
          }
          else if(createOrUpdate == 'update'){
            formData.set("Id", this.formRegister.id);
            raterService.update(formData).then(rs => {
            this.$toasted.show('Update success', {
              icons: 'shopping_cart',
              theme: 'bubble',
              position: 'top-right',
              duration: 3000
            })
            })
          }
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    updateStatus(status) {
      raterService.updateStatus(this.raterId, status).then(rs => {
        this.formRegister.status = status
        this.$toasted.show('Update success', {
          icons: 'shopping_cart',
          theme: 'bubble',
          position: 'top-right',
          duration: 3000
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
