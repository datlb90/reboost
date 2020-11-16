<template>
<div style="margin-top:25px;">
  <el-row class="row-flex">
    <el-col :span="14" class="col-border">
      <el-steps :active="1" align-center>
        <el-step title="Step 1" icon="el-icon-user" description="Create an account"></el-step>
        <el-step title="Step 2" icon="el-icon-upload" description="Upload credentials"></el-step>
        <el-step title="Step 3" icon="el-icon-circle-check" description="Complete trainning"></el-step>
        <el-step title="Step 4" icon="el-icon-edit-outline" description="Start rating"></el-step>
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
            <el-checkbox-group v-model="formRegister.applyToBecome.IELTS" style="margin-right: 20px;">
              <el-checkbox label="IELTS Rater" name="type"></el-checkbox>
            </el-checkbox-group>
            <el-checkbox-group v-model="formRegister.applyToBecome.TOEFL">
              <el-checkbox label="TOEFL Rater" name="type"></el-checkbox>
            </el-checkbox-group>
          </div>
        </el-form-item>
        <div v-if="formRegister.applyToBecome.IELTS">
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
            <el-upload class="upload-demo" action="" :on-change="handleChangeIELTS" :on-remove="handleRemoveIELTS" :file-list="formRegister.iELTSCertificatePhotos" :auto-upload="false" list-type="picture">
              <el-button size="small" type="primary">Click to upload</el-button>
              <div slot="tip" class="el-upload__tip">
                <p>Please upload your IELTS test result, and any other supporting credidentials you may have. Files must be less than 500kb in size.</p>
              </div>
            </el-upload>
          </el-form-item>
        </div>
        <div v-if="formRegister.applyToBecome.TOEFL">
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
                <p>Please upload your TOEFL test result, and any other supporting credidentials you may have. Files must be less than 500kb in size.</p>
              </div>
            </el-upload>
          </el-form-item>
        </div>

        <el-form-item ref="photoId" label="Photo ID" prop="iDCardPhotos">
          <el-upload class="upload-demo" action="" :on-change="handleChangeIdPhoto" :on-remove="handleRemoveIdPhoto" :file-list="formRegister.iDCardPhotos" :auto-upload="false" list-type="picture">
            <el-button size="small" type="primary">Click to upload</el-button>
            <div slot="tip" class="el-upload__tip">
              <p>Please upload a form of photo identification such as ID card, driver license, or passport. The file must be less than 500kb in size.</p>
            </div>
          </el-upload>
        </el-form-item>
        <el-form-item label="Biography">
          <el-input type="textarea" v-model="formRegister.biography" :rows="5" placeholder="Tell us a little bit about yourself and the reason why you apply to become our rater"></el-input>
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

export default {
  name: 'Application',
  data() {
    return {
      fileList: [{
          name: 'food.jpeg',
          url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100'
        },
        {
          name: 'food2.jpeg',
          url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100'
        }
      ],
      formRegister: {
        firstName: '',
        lastName: '',
        gender: '',
        occupation: '',
        status: '',
        firstLanguage: '',
        applyToBecome: {
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
        appliedDate: new Date()
      },
      occupation: [],
      firstLanguage: [],
      gender: [],
      raterId: ''
    }
  },
  methods: {
    onLoad(){
      lookupService.getByType('OCCUPATION').then(rs => {
        this.occupation = rs;
      });
      lookupService.getByType('FIRST_LANGUAGE').then(rs => {
        this.firstLanguage = rs;
      });
      lookupService.getByType('GENDER').then(rs => {
        this.gender = rs;
      });

      if(this.$route.params.id){
        this.raterId = this.$route.params.id;
        this.loadDetail(this.$route.params.id);
      }
    },
    loadDetail(id){
      console.log('load detail', mapUtil);
      raterService.getById(id).then(rs => {
        console.log('result load detail', rs);
        this.formRegister = mapUtil.map(rs, this.formRegister);
        
        //Score
        if(rs.scores){
          this.formRegister.ieltsTestScore.listening = rs.scores.find(s => s.subject == 'IELTS' && s.skill == 'Listening').score;
          this.formRegister.ieltsTestScore.reading = rs.scores.find(s => s.subject == 'IELTS' && s.skill == 'Reading').score;
          this.formRegister.ieltsTestScore.writting = rs.scores.find(s => s.subject == 'IELTS' && s.skill == 'Writting').score;
          this.formRegister.ieltsTestScore.speaking = rs.scores.find(s => s.subject == 'IELTS' && s.skill == 'Speaking').score;

          this.formRegister.toeflTestScore.listening = rs.scores.find(s => s.subject == 'TOEFL' && s.skill == 'Listening').score;
          this.formRegister.toeflTestScore.reading = rs.scores.find(s => s.subject == 'TOEFL' && s.skill == 'Reading').score;
          this.formRegister.toeflTestScore.writting = rs.scores.find(s => s.subject == 'TOEFL' && s.skill == 'Writting').score;
          this.formRegister.toeflTestScore.speaking = rs.scores.find(s => s.subject == 'TOEFL' && s.skill == 'Speaking').score;
        }
        
        //Apply to
        if(rs.applyTo){
          let items = rs.applyTo.split(',');
          for(let key in this.formRegister.applyToBecome){
            let exist = items.find(i => i == key);
            if(exist){
              this.formRegister.applyToBecome[key] = true;
            }
          }
        }
        
      });
    },
    onSubmit(formName, createOrUpdate) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          let data = {...this.formRegister}
          var valPhoto = [];
          data.iDCardPhotos.forEach(u => valPhoto.push(u.raw));
          data.iDCardPhotos = valPhoto;

          var valIelts = [];
          data.iELTSCertificatePhotos.forEach(u => valIelts.push(u.raw));
          data.iELTSCertificatePhotos = valIelts;

          var valToefl = [];
          data.tOEFLCertificatePhotos.forEach(u => valToefl.push(u.raw));
          data.tOEFLCertificatePhotos = valToefl;

          let scores = [
            { subject: 'IELTS', skill: 'Listening', score: data.ieltsTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
            { subject: 'IELTS', skill: 'Reading', score: data.ieltsTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
            { subject: 'IELTS', skill: 'Writting', score: data.ieltsTestScore.writting, updatedDate: moment().format('yyyy-MM-DD') },
            { subject: 'IELTS', skill: 'Speaking', score: data.ieltsTestScore.speaking, updatedDate: moment().format('yyyy-MM-DD') },
            { subject: 'TOEFL', skill: 'Listening', score: data.toeflTestScore.listening, updatedDate: moment().format('yyyy-MM-DD') },
            { subject: 'TOEFL', skill: 'Reading', score: data.toeflTestScore.reading, updatedDate: moment().format('yyyy-MM-DD') },
            { subject: 'TOEFL', skill: 'Writting', score: data.toeflTestScore.writting, updatedDate: moment().format('yyyy-MM-DD') },
            { subject: 'TOEFL', skill: 'Speaking', score: data.toeflTestScore.speaking, updatedDate: moment().format('yyyy-MM-DD') }
          ];

          data.scoreJSON = JSON.stringify(scores);
          data.status = 'Applied';
          data.appliedDate = moment().format('yyyy-MM-DD hh:mm:ss');

          let _applyTo = [];
          for(let key in data.applyToBecome){
            if(data.applyToBecome[key] == true){
              _applyTo.push(key);
            }
          }
          data.applyTo = _applyTo.join(',');

          delete data.ieltsTestScore;
          delete data.toeflTestScore;

          if(createOrUpdate == 'create'){
            raterService.insert(data).then(rs => {
            this.$toasted.show('Upload success', {
              icons: 'shopping_cart',
              theme: 'bubble',
              position: 'top-right',
              duration: 3000
            })
            })
          }
          else if(createOrUpdate == 'update'){
            console.log('update rater information');
            raterService.update(data).then(rs => {
            this.$toasted.show('Update success', {
              icons: 'shopping_cart',
              theme: 'bubble',
              position: 'top-right',
              duration: 3000
            })
            })
          }

          
        } else {
          console.log('error submit!!');
          return false;
        }
      });
    },
    updateStatus(status){
      raterService.updateStatus(this.raterId, status).then(rs => {
        this.formRegister.status = status;
        this.$toasted.show('Update success', {
          icons: 'shopping_cart',
          theme: 'bubble',
          position: 'top-right',
          duration: 3000
        })
      })
    },
    handleRemoveIdPhoto(file, fileList) {
      this.formRegister.iDCardPhotos = fileList;
    },
    handleChangeIdPhoto(file, fileList) {
      this.formRegister.iDCardPhotos = fileList;
    },
    handleChangeIELTS(file, fileList) {
      this.formRegister.iELTSCertificatePhotos = fileList;
    },
    handleRemoveIELTS(file, fileList) {
      this.formRegister.iELTSCertificatePhotos = fileList;
    },
    handleChangeTOEFL(file, fileList) {
      this.formRegister.tOEFLCertificatePhotos = fileList;
    },
    handleRemoveTOEFL(file, fileList) {
      this.formRegister.tOEFLCertificatePhotos = fileList;
    }
  },
  computed: {
    score: function () {
      var x = [];
      for (let i = 0; i <= 100; i++)
        x.push(i);
      return x
    },
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
  mounted(){
    this.onLoad();
  }
}
</script>

<style>
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
