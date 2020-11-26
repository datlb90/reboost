<template>
<div style="margin-top:25px;">
  <el-row class="row-flex">
    <el-col :span="14" class="col-border">
      <el-steps :active="1" align-center>
        <el-step title="Step 1" icon="far fa-user-circle" description="Create an account"></el-step>
        <el-step title="Step 2" icon="fas fa-file-upload" description="Upload credentials"></el-step>
        <el-step title="Step 3" icon="el-icon-circle-check" description="Complete trainning"></el-step>
        <el-step title="Step 4" icon="el-icon-edit-outline" description="Start rating"></el-step>
      </el-steps>
    </el-col>
  </el-row>
  <el-row class="row-flex" >
    <el-col :span="14" class="col-border">
      <div class="margin-container">
        <div class="flex-box">
          <div class="label-container">
            Application status
          </div>
          <div class="">
            <el-tag 
              :type="
                status === 'In Review' 
                  ? 'primary'
                  :status === 'Verified'
                  ? 'success'
                  :status === 'Denied'
                  ? 'danger'
                  : 'danger'
              "
            >{{status}}</el-tag>
          </div>
        </div>
        <div :class = "[status === 'In Review' ? 'inReview' : 'hidden']">
          <p>Thank you for applying. Your application is curretly in review. We will notify you via email if your application is approved, denied, or if we need additional information.</p>
        </div>
        <div :class = "[status === 'Verified' ? 'verified' : 'hidden']">
          <p>Your application has been reviewed and verified. You are just one step away from becoming our rater. After completing our training process, you can start rating and earing extra money.
          </p>
        </div>
        <div :class = "[status === 'Denied' ? 'denied' : 'hidden']">
          <p>Unfortunately, your application was denied because your credentials do not match with the requirements for becoming a rater. We look forward to receiving your application again in the near future. If you have any questions or concerns regarding this, please feel free to contact us as support@reboost.ai.
          </p>
        </div>
        <div v-if="status==='Verified'" class="button-container">
          <el-button>Complete Training Now</el-button>
        </div>
        <div class="note-container">
          <div class="label-container">
            Note
          </div>
          <el-input
            type="textarea"
            :rows="2"
            placeholder="Please input"
            v-model="note">
          </el-input>
        </div>
      </div>
    </el-col>
  </el-row>
</div>
</template>

<script>
// @ is an alias to /src
import raterService from '../services/rater.service'
import lookupService from '../services/lookup.service'
import moment from 'moment';
import * as mapUtil from '@/utils/model-mapping';

export default {
  name: 'RaterDetail',
  data() {
    return {
      raterId: '',
      isUpload: false,
      inReview: true,
      verified: true,
      denied: true,
      status: 'In Review',
      note: ''
    }
  },
  methods: {
    onLoad(){
      if(this.$route.params.id){
        this.raterId = this.$route.params.id;
        this.loadDetail(this.$route.params.id);
      }
    },
    loadDetail(id){
      console.log('load detail', mapUtil);
      raterService.getById(id).then(rs => {
        console.log('result load detail', rs);
        this.status = rs.status;
      });
    }
  },
  computed: {

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

.inReview {
  padding: 8px 14px;
  background-color: #ecf8ff;
  border-radius: 4px;
  border-left: 5px solid #50bfff;
}

.verified{
  padding: 8px 14px;
  background-color: #ecffee;
  border-radius: 4px;
  border-left: 5px solid #12ee2f;
}

.denied{
  padding: 8px 14px;
  background-color: #ffecec;
  border-radius: 4px;
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
  margin: 10px
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
  margin: 20px 0 0 0;
}

</style>
