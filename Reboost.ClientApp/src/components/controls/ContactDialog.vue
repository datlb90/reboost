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
        <!-- <div class="divider--horizontal divider" /> -->
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
            :rules="[{ required: true, message: 'Please enter your full name'}]"
          >
            <el-input v-model="formData.fullName" :disabled="currentUser.id" type="text" placeholder="Enter your full name" style="width: 90%;" />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'email')"
            prop="email"
            :rules="[{ required: true, message: 'Please enter your email address'}]"
          >
            <el-input v-model="formData.email" :disabled="currentUser.id" type="text" placeholder="Enter your email address" style="width: 90%;" />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'reason')"
            prop="reason"
            :rules="[{ required: true, message: 'Please select a reason' }]"
          >
            <el-select v-model="formData.reason" placeholder="Select a reason" style="width: 180px;">
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
            :label="messageTranslates('contactDialog', 'message')"
            prop="message"
            :rules="[{ required: true, message: 'Message is required'}]"
          >
            <el-input
              v-model="formData.message"
              type="textarea"
              :autosize="{ minRows: 5, maxRows: 8}"
              placeholder="Detail your enquiry here"
              style="width: 90%;"
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
              :multiple="true"
              :limit="3"
              :on-exceed="handleExceed"
              accept=".jpg,.png,.jpeg,.JPG,.PGN,.JPEG"
            >
              <el-button>{{ messageTranslates('contactDialog', 'clickToUpload') }}</el-button>
              <div slot="tip" class="el-upload__tip">jpg/png files with a size less than 3mb</div>
            </el-upload>
          </el-form-item>

          <el-form-item
            size="mini"
          >
            <el-button size="mini" type="primary" :disabled="isLoading" style="margin-top: 10px;" @click="submitMessage">{{ messageTranslates('contactDialog', 'submit') }}</el-button>
            <el-button size="mini" :disabled="isLoading" style="margin-top: 10px;" @click="dialogVisible=false">{{ messageTranslates('contactDialog', 'cancel') }}</el-button>
          </el-form-item>
        </el-form>
      </div>
      <!-- <div slot="footer" style="text-align: center;">
        <el-button size="mini" @click="dialogVisible=false">{{ messageTranslates('contactDialog', 'cancel') }}</el-button>
        <el-button size="mini" type="primary" @click="submitMessage">{{ messageTranslates('contactDialog', 'submit') }}</el-button>
      </div> -->
    </el-dialog>
  </div>
</template>
<script>
import * as stringUtil from '@/utils/string'
import userService from '../../services/user.service'

export default {
  name: 'ContactDialog',
  data() {
    return {
      dialogVisible: false,
      formData: {
        fullName: null,
        email: null,
        reason: '',
        message: '',
        files: []
      },
      reasons: ['Report a problem', 'Suggest new features', 'Other reasons'],
      isLoading: false
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
    }
    // console.log('', this.reasons, this.roles)
  },
  methods: {
    openDialog() {
      this.dialogVisible = true
      this.$refs['contactForm'].resetFields()
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
          postData.set('Message', this.formData.message)

          for (const p of this.formData.files) {
            if (p.raw) {
              postData.append(`UploadedFiles`, p.raw)
            } else {
              postData.append(`UploadedFiles`, stringUtil.base64ToArrayBuffer(p.url, p.name))
            }
          }

          userService.supportRequest(postData).then(rs => {
            if (rs) {
              this.$notify.success({
                title: 'Support Request Submitted',
                message: 'Your inquiry has been submitted successfully',
                type: 'success',
                duration: 2000
              })
              this.dialogVisible = false
              this.formData.fullName = null
              this.formData.email = null
              this.formData.reason = ''
              this.formData.message = ''
              this.formData.files = []
              this.$refs['contactForm'].resetFields()
            }
          })
        }
      })
    },
    handleChangeFile(file, fileList) {
      if (file.size > 3e6) {
        this.$notify.error({
          title: 'File size exceed',
          message: 'Please upload file files with a size less than 3mb.',
          type: 'error',
          duration: 2000
        })
        const index = fileList.findIndex(f => f.uid === file.uid)
        fileList.splice(index, 1)
      }
      this.formData.files = fileList
    },
    handleRemoveFile(file, fileList) {
      this.formData.files = fileList
    },
    handleExceed(files, fileList) {
      this.$notify.error({
        title: 'File limit exceed',
        message: 'Please upload no more than 3 files.',
        type: 'error',
        duration: 2000
      })
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
