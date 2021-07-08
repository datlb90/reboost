<template>
  <div class="container">
    <div class="title">
      <h2>Rater Applications</h2>
    </div>
    <div class="searchAndBtn">
      <div class="search">
        <el-input v-model="textSearch" size="mini" placeholder="Type to search" />
      </div>
      <div class="btn-reset">
        <el-button size="mini" @click="clearFilter">reset all filters</el-button>
      </div>
    </div>
    <el-table ref="filterTable" size="mini" :data="displayData" style="width: 100%">
      <el-table-column prop="fullName" label="Application Name" width="160" />
      <el-table-column prop="appliedDate" label="Application Date" sortable width="180" column-key="appliedDate" />
      <el-table-column prop="occupation" label="Occupation" width="140" />
      <el-table-column prop="firstLanguage" label="First Language" width="130" />
      <!-- <el-table-column prop="applyTo" label="Applied For" width="120">
      </el-table-column> -->
      <el-table-column
        prop="status"
        label="Status"
        :filters="[
          { text: RATER_STATUS.APPLIED, value: RATER_STATUS.APPLIED },
          { text: RATER_STATUS.APPROVED, value: RATER_STATUS.APPROVED },
          { text: RATER_STATUS.TRAINING, value: RATER_STATUS.TRAINING },
          { text: RATER_STATUS.REVISION, value: RATER_STATUS.REVISION },
          { text: RATER_STATUS.DOCUMENT_REQUESTED, value: RATER_STATUS.DOCUMENT_REQUESTED },
          { text: RATER_STATUS.DOCUMENT_SUBMITTED, value: RATER_STATUS.DOCUMENT_SUBMITTED },
          { text: RATER_STATUS.REVISION_REQUESTED, value: RATER_STATUS.REVISION_REQUESTED },
          { text: RATER_STATUS.REVISION_COMPLETED, value: RATER_STATUS.REVISION_COMPLETED },
          { text: RATER_STATUS.REJECTED, value: RATER_STATUS.REJECTED },
          { text: RATER_STATUS.TRAINING_APPROVED, value: RATER_STATUS.TRAINING_APPROVED },
          { text: RATER_STATUS.DOCUMENT_COMPLETED, value: RATER_STATUS.DOCUMENT_COMPLETED }
        ]"
        :filter-method="filterTag"
        filter-placement="bottom-end"
      >
        <template slot-scope="scope">
          <el-tag
            size="mini"
            :type="
              scope.row.status === RATER_STATUS.APPROVED
                ? 'success'
                : scope.row.status === RATER_STATUS.APPLIED || scope.row.status === RATER_STATUS.TRAINING
                  ? 'primary'
                  : scope.row.status === RATER_STATUS.DOCUMENT_REQUESTED || scope.row.status === RATER_STATUS.DOCUMENT_SUBMITTED || scope.row.status === RATER_STATUS.REVISION_REQUESTED
                    ? 'warning'
                    : 'danger'"
            disable-transitions
          >{{ scope.row.status }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column label="Operations">
        <template slot-scope="scope">
          <el-button size="mini" @click="handleView(scope.$index, scope.row)">View</el-button>
          <el-button v-if="completedTraining(scope.row,'IELTS')" :type="isApproved(scope.row,'IELTS')?'success':'info'" size="mini" @click="TrainingReview(scope.row,'IELTS')">IELTS</el-button>
          <el-button v-if="completedTraining(scope.row,'TOEFL')" :type="isApproved(scope.row,'TOEFL')?'success':'info'" size="mini" @click="TrainingReview(scope.row,'TOEFL')">TOEFL</el-button>
          <el-button v-if="scope.row.status == 'Training Completed'" size="mini" type="warning">Review</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div class="pagination">
      <el-pagination size="mini" background layout="prev, pager, next" :page-size="pageSize" :total="total" @current-change="handleCurrentChange" />
    </div>
  </div>
</template>

<script>
import { RATER_STATUS } from '../../app.constant'

export default {
  name: 'ManageRaters',
  data() {
    return {
      textSearch: null,
      filtered: [],
      page: 1,
      pageSize: 10,
      total: 10,
      filterDate: [],
      table: [],
      RATER_STATUS: RATER_STATUS
    }
  },
  computed: {
    displayData() {
      const table = this.searching()
      return table.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    },
    getAllRater() {
      return this.$store.getters['rater/getAll']
    },
    getAllReviews() {
      return this.$store.getters['review/getReviews']
    }
    // ...mapGetters({
    //   getAllRater: 'rater/getAll'
    // })
  },
  mounted() {
    this.$store.dispatch('rater/loadRaters')
    this.$store.dispatch('review/loadReviews')
  },
  methods: {
    resetDateFilter() {
      this.$refs.filterTable.clearFilter('applicationDate')
    },
    clearFilter() {
      this.$refs.filterTable.clearFilter()
      this.textSearch = ''
    },
    filterTag(value, row) {
      // var table
      // if (!this.textSearch) {
      //   table = this.getAllRater
      // } else {
      //   table = this.getAllRater.filter(data => data.fullName.toLowerCase().includes(this.textSearch.toLowerCase()))
      // }
      // this.total = table.filter(data => data.status.toLowerCase().includes(row.status.toLowerCase())).length
      return row.status === value
    },
    filterHandler(value, row, column) {
      const property = column['property']
      return row[property] === value
    },
    handleCurrentChange(val) {
      this.page = val
    },
    searching() {
      if (!this.textSearch) {
        this.total = this.getAllRater.length
        return this.getAllRater
      }
      var table = this.getAllRater.filter(data => data.fullName.toLowerCase().includes(this.textSearch.toLowerCase()))
      this.total = table.length
      this.filterDate = []
      let i = 0
      this.getAllRater.forEach(rs => {
        this.$set(this.filterDate, i++, {
          text: rs.applicationDate,
          value: rs.applicationDate
        })
      })
      this.filterDate = this.filterDate.filter((item, index) => this.filterDate.indexOf(item) === index)
      return table
    },
    handleView(index, row) {
      console.log(index, row)
      this.$router.push({
        name: 'ApplicationDetail',
        params: {
          id: row.id
        }
      })
    },
    completedTraining(e, status) {
      status += 'Training'

      var t = this.getAllReviews.filter(r => r.reviewerId == e.userId && r.reviewData.length > 0 && (r.status.includes(status)))[0]
      if (t) {
        return true
      }
      return false
    },
    isApproved(e, status) {
      status += 'TrainingApproved'
      var t = this.getAllReviews.filter(r => r.reviewerId == e.userId && r.reviewData.length > 0 && r.status == status)[0]
      if (t) {
        return true
      }
      return false
    },
    TrainingReview(e, type) {
      type += 'Training'
      var t = this.getAllReviews.filter(r => r.reviewerId == e.userId && r.reviewData.length > 0 && (r.status.includes(status)))[0]
      var pushUrl = t.status.includes('IELTSTraining') ? '/review/9/69/' + t.id : '/review/12/68/' + t.id
      this.$router.push(pushUrl)
    }
  }
}
</script>

<style scoped>
.container {
  margin-top: 20px;
}

.search {
  width: 40%;
}

.searchAndBtn {
  display: flex;
  justify-content: space-between;
  margin: 20px 0;
}

.pagination-container {
  background: #fff;
  padding: 32px 16px;
}

.pagination {
  margin: 2rem;
  width: 100%;
  justify-content: center;
}
</style>
