<template>
  <div style="height: 100%;display: flex; flex-direction: column">
    <div id="rate">
      <div style="height: 100%; overflow: auto;">
        <div class="content-con">
          <div style="margin: 0 0 10px 5px">
            <span class="title">Rate </span>
            <el-rate v-model="rateValue" style="margin-top:10px" :allow-half="true" :disabled="isRated" />
          </div>
          <div style="padding-left:5px">
            <span class="title">Comment: </span>
            <el-input
              v-model="rateComment"
              style="margin-top:5px"
              :maxlength="8000"
              placeholder="Please input your comment"
              :rows="3"
              :disabled="isRated"
            />
          </div>
          <div style="margin: 10px 0 10px 5px;">
            <el-button v-if="!isReviewAuth" :disabled="isRated" size="mini " type="primary" @click="rateReviewer()">
              Submit
            </el-button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import reviewService from '@/services/review.service.js'

export default ({
  name: 'TabRate',
  props: {
    reviewid: { type: Number, default: null },
    isReviewAuth: { type: Boolean, default: false }
  },
  data() {
    return {
      rateValue: 0,
      rateComment: '',
      isRated: false
    }
  },
  async mounted() {

  },
  methods: {
    updateData(e) {
      this.rateValue = e.value
      this.rateComment = e.comment
      this.isRated = e.rated
    },
    rateReviewer() {
      reviewService.createReviewRating({
        ReviewId: +this.$route.params.reviewId,
        Rate: parseFloat(this.rateValue),
        Comment: this.rateComment
      }).then(r => {
        if (r) {
          this.isRated = true
          this.$notify.success({
            title: 'Success',
            message: 'Rated successfully',
            type: 'success',
            duration: 1500
          })
        }
      })
    }
  }
})
</script>
<style scoped>
@import '../../styles/review.css';
.title {
  font-size: 16px; font-weight: bold;
}
</style>

