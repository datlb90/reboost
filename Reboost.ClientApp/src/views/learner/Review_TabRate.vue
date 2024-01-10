<template>
  <div style="height: 100%;display: flex; flex-direction: column">
    <div id="rate">
      <div>
        <div class="content-con">
          <div style="margin: 0 0 10px 5px">
            <span class="title">Review Rating </span>
            <el-rate v-model="rateValue" style="margin-top:10px;     color: rgb(177 177 177);" :allow-half="true" :disabled="isRated" />
          </div>
          <div style="padding-left: 5px;">
            <!-- <span class="title">Comment </span> -->
            <el-input
              id="rubric-rating"
              v-model="rateComment"
              type="textarea"
              :rows="10"
              style="margin-top:5px; margin-bottom: 5px;"
              :maxlength="8000"
              placeholder="Provide feedback to your rater"
              :disabled="isRated"
            />
          </div>
          <div style="margin: 10px 0 10px 5px;">
            <el-button v-if="!isReviewAuth" :disabled="isRated || rateValue == 0 || rateComment == ''" size="mini " type="primary" @click="rateReview()">
              Submit Rating
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
    rateReview() {
      reviewService.createReviewRating({
        ReviewId: +this.$route.params.reviewId,
        Rate: parseFloat(this.rateValue),
        Comment: this.rateComment
      }).then(r => {
        if (r) {
          this.isRated = true
          this.$emit('rated')
          this.$notify.success({
            title: 'Success',
            message: 'Rated successfully',
            type: 'success',
            duration: 1500
          })
        }
        this.$router.push('/submissions')
      })
    }
  }
})
</script>
<style>
#rubric-rating{
   overflow: auto;
}
</style>
<style scoped>
@import '../../styles/review.css';
.title {
  font-size: 16px;
  font-weight: 500;
}

</style>

