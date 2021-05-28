<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15">
        <div style="display: flex; justify-content:space-between">
          <h3>My Reviews</h3>
          <el-button style="margin-right: 25px;" size="mini">
            Make New Review
          </el-button>
        </div>
        <el-main>
          <el-table
            :data="listReviewsPerPage"
            stripe
            border
            style="width: 100%"
            size="mini"
          >
            <el-table-column
              prop="questionName"
              label="Question"
              align="center"
            />
            <el-table-column
              prop="testSection"
              label="Test Section"
              align="center"
            />
            <el-table-column
              prop="questionType"
              label="Question Type"
              align="center"
            />

            <el-table-column
              width="110px"
              align="center"
              prop="reviewRequest.status"
              label="Status"
            />
            <el-table-column
              label="Date Completed"
              align="center"
            >
              <template slot-scope="scope">
                <span>{{ getTimeFromDateCreateToNow(scope.row.review.lastActivityDate) }}</span>
              </template>
            </el-table-column>
            <el-table-column align="center" label="Actions">
              <template slot-scope="scope">
                <el-button size="mini" @click="navigateToReviewRequest(scope.row)">
                  View
                </el-button>
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
import moment from 'moment'
import reviewService from '../../services/review.service'
import { REVIEW_REQUEST_STATUS } from '../../app.constant'
export default {
  name: 'Reviews',
  components: {},
  data() {
    return {
      tableData: [],
      reviewsListCached: [],
      reviewsList: [],
      listReviewsPerPage: [],
      pageSize: 15,
      total: 0,
      page: 1,
      REVIEW_REQUEST_STATUS: REVIEW_REQUEST_STATUS
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
    onLoad() {
      reviewService.getReviewsByUser().then(rs => {
        if (rs) {
          this.reviewsList = rs
          this.reviewsListCached = [...this.reviewsList]
          this.loadList()
          console.log('reviews', rs)
        }
      })
    },
    getTimeFromDateCreateToNow(time) {
      if (!time) {
        return 'No Submission'
      }
      return moment(new Date(time)).fromNow()
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadList()
    },
    loadList() {
      this.listReviewsPerPage = this.reviewsListCached.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.total = this.reviewsListCached.length
    },
    navigateToReviewRequest(object) {
      const url = `/review/${object.submission.questionId}/${object.submission.docId}/${object.reviewId}`
      this.$router.push(url)
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
</style>
