<template>
  <div>
    <el-dialog
      id="contactDialog"
      :visible.sync="dialogVisible"
      width="650px"
      :fullscreen="screenWidth <= 780"
      @opened="dialogOpened"
      @closed="dialogClosed"
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
          label-width="120px"
          style="width:100%;"
        >
          <el-form-item
            size="mini"
            label="Họ và tên"
            prop="fullName"
            :rules="[{ required: true, message: 'Hãy điền họ và tên của bạn'}]"
          >
            <el-input v-model="formData.fullName" :disabled="currentUser.id != undefined" type="text" placeholder="Điền họ và tên của bạn" style="width: 90%;" />
          </el-form-item>
          <el-form-item
            size="mini"
            label="Địa chỉ email"
            prop="email"
            :rules="[{ required: true, message: 'Hãy điền địa chỉ email của bạn'}]"
          >
            <el-input v-model="formData.email" :disabled="currentUser.id != undefined" type="text" placeholder="Điền địa chỉ email của bạn" style="width: 90%;" />
          </el-form-item>
          <el-form-item
            size="mini"
            :label="messageTranslates('contactDialog', 'reason')"
            prop="reason"
            :rules="[{ required: true, message: 'Hãy chọn 1 lý do' }]"
          >
            <el-select v-model="formData.reason" placeholder="Chọn 1 lý do" style="width: 180px; z-index: 9999 !important;">
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
            label="Yêu cầu"
            prop="message"
            :rules="[{ required: true, message: 'Hãy điền yêu cầu của bạn'}]"
          >
            <el-input
              v-model="formData.message"
              type="textarea"
              :autosize="{ minRows: 6, maxRows: 8}"
              placeholder="Điền yêu cầu của bạn"
              style="width: 90%;"
            />
          </el-form-item>
          <el-form-item
            size="mini"
            label="Tệp đính kèm"
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
              <el-button>Nhấp để tải</el-button>
              <div slot="tip" class="el-upload__tip">Ảnh có định dạng jpg hoặc png với dung lượng nhỏ hơn 3mb</div>
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
      isLoading: false,
      screenWidth: window.innerWidth
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  mounted() {
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    if (this.currentUser.id) {
      this.formData.fullName = this.currentUser.firstName + ' ' + this.currentUser.lastName
      this.formData.email = this.currentUser.email
    }
  },
  methods: {
    openDialog() {
      this.dialogVisible = true
    },
    dialogOpened() {
    },
    dialogClosed() {
      this.$refs['contactForm'].resetFields()
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
    font-weight: 500;
}
</style>
<style>
.el-select-dropdown{
  z-index: 99999 !important;
}
.el-popup-parent--hidden > div > div > header{
  width: calc(100% + 15px) !important;
  position: static !important;
  padding-right: 15px;
}
.el-popup-parent--hidden > div > div > div > header{
  width: calc(100% + 15px) !important;
  padding-right: 15px;
}

.el-popup-parent--hidden > div > div > div > .ml-main-section{
  padding-top: 82px !important;
}

.el-popup-parent--hidden > div > div > #header.headroom.is-sticky + div > #banner{
  padding-top: 67px !important;
}

</style>

