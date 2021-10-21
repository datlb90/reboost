<template>
  <div>
    <el-dialog
      id="contactDialog"
      :visible.sync="dialogVisible"
      width="35%"
      @opened="dialogOpened()"
    >
      <div slot="title">
        <div class="title-container">
          {{ messageTranslates('contactDialog', 'title') }}
        </div>
        <div class="divider--horizontal divider" />
      </div>
      <div class="body-container">
        <el-form
          ref="contactForm"
          :model="formData"
          label-position="right"
          label-width="150px"
          style="width:100%;"
        >
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'fullName')"
            prop="fullName"
            :rules="[{ required: true, message: 'Full Name is required'}]"
          >
            <el-input v-model="formData.fullName" :disabled="currentUser.id" type="text" />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'email')"
            prop="email"
            :rules="[{ required: true, message: 'Email is required'}]"
          >
            <el-input v-model="formData.email" :disabled="currentUser.id" type="text" />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'reason')"
            prop="reason"
            :rules="[{ required: true, message: 'Reason is required' }]"
          >
            <el-select v-model="formData.reason" :placeholder="messageTranslates('contactDialog', 'reason')">
              <el-option
                v-for="reason in reasons"
                :key="reason"
                :label="reason"
                :value="reason"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'userRole')"
            prop="role"
            :rules="[{ required: true, message: 'Role is required'}]"
          >
            <el-select v-model="formData.role" :disabled="currentUser.id" placeholder="User Role">
              <el-option
                v-for="role in roles"
                :key="role"
                :label="role"
                :value="role"
              />
            </el-select>
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'message')"
            prop="message"
            :rules="[{ required: true, message: 'Message is required'}]"
          >
            <el-input
              v-model="formData.message"
              type="textarea"
              :autosize="{ minRows: 5, maxRows: 8}"
            />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'uploadFile')"
            prop="files"
          >
            <el-upload
              class="upload-demo"
              action=""
              :on-change="handleChangeFile"
              :on-remove="handleRemoveFile"
              :file-list="formData.files"
              :auto-upload="false"
            >
              <el-button type="primary">{{ constantTranslate('DISPUTE_STATUS', 'CLICK_TO_UPLOAD') }}</el-button>
            </el-upload>
          </el-form-item>
        </el-form>
      </div>
      <div slot="footer" style="text-align: center;">
        <el-button>{{ constantTranslate('DISPUTE_STATUS', 'cancel') }}</el-button>
        <el-button type="primary" @click="submitMessage">{{ constantTranslate('DISPUTE_STATUS', 'submit') }}</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import * as stringUtil from '@/utils/string'
import raterService from '../../services/rater.service'

export default {
  name: 'ContactDialog',
  data() {
    return {
      dialogVisible: false,
      formData: {
        fullName: null,
        email: null,
        reason: '',
        role: '',
        message: '',
        files: []
      },
      reasons: ['Report Bug', 'Reason 2', 'Reason 3'],
      roles: ['Student', 'Teacher', 'Pupil']
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  mounted() {
    if (this.currentUser.id) {
      this.formData.fullName = this.currentUser.firstName + this.currentUser.lastName
      this.formData.email = this.currentUser.email
      this.formData.role = this.currentUser.role
    }
    console.log('', this.reasons, this.roles)
  },
  methods: {
    openDialog() {
      this.dialogVisible = true
    },
    dialogOpened() {
    },
    submitMessage() {
      this.$refs['contactForm'].validate((valid) => {
        if (valid) {
          const postData = new FormData()
          postData.set('Fullname', this.formData.fullName)
          postData.set('Email', this.formData.email)
          postData.set('Reason', this.formData.reason)
          postData.set('Role', this.formData.role)
          postData.set('Message', this.formData.message)

          for (const p of this.formData.files) {
            if (p.raw) {
              postData.append(`UploadedFiles`, p.raw)
            } else {
              postData.append(`UploadedFiles`, stringUtil.base64ToArrayBuffer(p.url, p.name))
            }
          }

          raterService.contactRequest(postData).then(rs => {
            if (rs) {
              this.$notify.success({
                title: 'Contact Reported!',
                message: 'Contact Reported!',
                type: 'success',
                duration: 2000
              })
              this.dialogVisible = false
              this.dialogVisible = false
              this.formData.fullName = null
              this.formData.email = null
              this.formData.reason = ''
              this.formData.role = ''
              this.formData.message = ''
              this.formData.files = []
            }
          })
        }
      })
    },
    handleChangeFile(file, fileList) {
      this.formData.files = fileList
    },
    handleRemoveFile(file, fileList) {
      this.formData.files = fileList
    }
  }

}
</script>
<style scoped>
.title-container {
    width: 100%;
    text-align: center;
    font-size: 16px;
}
.divider--horizontal {
    display: block;
    height: 1px;
    width: 100%;
    background-color: rgb(202 203 205);
}
.divider {
  margin: 10px 0 0 0;
  position: relative;
}
</style>
<style>
.el-select-dropdown {
  z-index: 30000 !important;
}
</style>
