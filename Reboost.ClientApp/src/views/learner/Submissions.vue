<template>
  <div id="submission" style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15">
        <h3>My Subsmissions</h3>
        <el-main>
          <el-table
            :data="listSubmissionsPerPage"
            stripe
            border
            style="width: 100%"
            size="mini"
          >
            <el-table-column
              prop="id"
              label="Id"
              align="center"
              width="50"
            />
            <el-table-column
              prop="question"
              label="Question"
              align="center"
            />
            <el-table-column
              prop="status"
              label="Status"
              align="center"
            >
              <template slot-scope="scope">
                <el-tag :type="getStatusVariant(scope.row.status)">{{ scope.row.status }}</el-tag>
              </template>
            </el-table-column>
            <el-table-column
              label="Time Submitted"
              align="center"
            >
              <template slot-scope="scope">
                <span>{{ getTimeFromDateCreateToNow(scope.row.timeSubmitted) }}</span>
              </template>
            </el-table-column>

            <el-table-column width="110px" prop="timeTaken" align="center" label="Time Taken">
              <template slot-scope="scope">
                <span>{{ getTimeTaken(scope.row.timeTaken) }}</span>
              </template>
            </el-table-column>
            <el-table-column align="center" label="Actions">
              <template slot-scope="scope">
                <div class="action-column-cell">
                  <el-button size="mini" @click="actionClick(scope.row.action, scope.row)">{{ scope.row.action }}</el-button>
                </div>
                <div v-if="scope.row.status.trim() == 'Submitted'" class="action-column-cell">
                  <el-button size="mini" @click="actionClick('Request Review', scope.row)">Request Review</el-button>
                </div>
              </template>
            </el-table-column>
          </el-table>
          <div class="pagination">
            <el-pagination
              background
              layout="prev, pager, next"
              :total="total"
              :page-size="pageSize"
              @current-change="handleCurrentChange"
            />
          </div>
        </el-main>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import reviewService from '../../services/review.service'
import { REVIEW_REQUEST_STATUS } from '../../app.constant'
import questionService from '../../services/question.service'
import moment from 'moment'
export default {
  name: 'Submissions',
  components: {},
  data() {
    return {
      tableData: [],
      submissionsListCached: [],
      submissionsList: [],
      listSubmissionsPerPage: [],
      pageSize: 15,
      total: 0,
      page: 1,
      unRatedList: null
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  mounted() {
    this.onLoad()
  },
  methods: {
    getStatusVariant(status) {
      let type = 'primary'
      switch (status.trim()) {
        case 'Pending': type = 'danger'; break
        case 'Completed': type = 'info'; break
        case 'Submitted': type = 'default'; break
        case 'Review Requested': type = 'warning'; break
        case 'Reviewed': type = 'success'; break

        default:
          type = 'primary'
      }
      return type
    },
    onLoad() {
      questionService.getSubmissionsByUserId(this.currentUser.id).then(rs => {
        if (rs) {
          this.submissionsList = rs
          this.submissionsListCached = [...this.submissionsList]
          this.loadList()
        }
      })
      reviewService.getUnratedReview().then(rs => {
        this.unRatedList = rs
      })
    },
    getTimeFromDateCreateToNow(time) {
      return moment(new Date(time)).fromNow()
    },
    getTimeTaken(time) {
      return moment(time).get('minute') + ' min ' + moment(time).get('second') + ' sec '
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadList()
    },
    loadList() {
      this.listSubmissionsPerPage = this.submissionsListCached.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.total = this.submissionsListCached.length
    },
    actionClick(action, e) {
      if (action.trim() === 'Request Review') {
        // if (this.unRatedList.length > 0) {
        //   this.$notify.error({
        //     title: 'You have unrated review',
        //     message: 'Please rate them and request later',
        //     duration: 2000
        //   })
        // } else
        // {
        reviewService.createReviewRequest({
          UserId: this.currentUser.id,
          SubmissionId: e.id,
          FeedbackType: 'Free',
          Status: REVIEW_REQUEST_STATUS.IN_PROGRESS
        }).then(rs => {
          this.$notify.success({
            title: 'Submission Requested',
            message: 'Requested!',
            duration: 1000
          })
          this.submissionsListCached.forEach(r => {
            if (r.id === e.id) {
              r.status = 'Review Requested'
              r.action = 'View'
            }
          })
          this.loadList()
        })
        // }
      } else if (action == 'View Submission') {
        this.$router.push(`PracticeWriting/${e.questionId}`)
      } else if (action == 'View Review') {
        reviewService.getOrCreateReviewBySubmissionId(e.id).then(rs => {
          this.$router.push(`review/${rs.reviewRequest.submission.questionId}/${rs.reviewRequest.submission.docId}/${rs.reviewId}/rate`)
        })
      }
    }
  }
}
</script>
<style scoped>
.row-flex {
  display: flex;
  justify-content: center;
  margin: 25px 0;
}
h3{
    padding-left:25px
}
.pagination{
  margin: 20px;
  justify-content: center;
}
.action-column-cell{
  width: 100%;
}
.action-column-cell > button{
  width: 100%;
}
</style>
<style>
#submission .el-table .cell {
    text-overflow: clip !important;
}
</style>
