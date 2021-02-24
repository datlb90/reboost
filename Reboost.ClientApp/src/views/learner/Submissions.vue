<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15">
        <h3>All My Subsmissions</h3>
        <el-main>
          <el-table
            :data="listSubmissionsPerPage"
            stripe
            style="width: 100%"
          >
            <el-table-column
              label="Time Submitted"
              align="center"
            >
              <template slot-scope="scope">
                <span>{{ getTimeFromDateCreateToNow(scope.row.timeSubmitted) }}</span>
              </template>
            </el-table-column>
            <el-table-column
              prop="question"
              label="Question"
              align="center"
            />
            <el-table-column
              prop="status"
              label="Status"
              align="center"
            />
            <el-table-column width="110px" prop="timeTaken" align="center" label="Time Taken">
              <template slot-scope="scope">
                <span>{{ getTimeTaken(scope.row.timeTaken) }}</span>
              </template>
            </el-table-column>
            <el-table-column align="center" label="Review">
              <template slot-scope="scope">
                <span> <a href="#"> {{ scope.row.review }}</a></span>
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
      pageSize: 10,
      total: 0,
      page: 1
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
      questionService.getSubmissionsByUserId(this.currentUser.id).then(rs => {
        if (rs) {
          this.submissionsList = rs
          this.submissionsListCached = [...this.submissionsList]
          this.loadList()
        }
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
      console.log(this.listSubmissionsPerPage)
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
