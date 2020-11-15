<template>
  <div class="container">
    <div class="title"><h2>Rater Applications</h2></div>
    <div class="searchAndBtn">
      <div class="search">
        <el-input placeholder="Type to search" v-model="textSearch"></el-input>
      </div>
      <div class="btn-reset">
        <el-button @click="clearFilter">reset all filters</el-button>
      </div>
    </div>
    <el-table ref="filterTable" :data="displayData" style="width: 100%">
      <el-table-column
        prop="fullName"
        label="Application Name"
        width="160"
      >
      </el-table-column>
      <el-table-column
        prop="applicationDate"
        label="Application Date"
        sortable
        width="180"
        column-key="applicationDate"
        :filters="filterDate"
        :filter-method="filterHandler"
      >
      </el-table-column>
      <el-table-column prop="occupation" label="Occupation" width="140">
      </el-table-column>
      <el-table-column prop="firstLanguage" label="First Language" width="130">
      </el-table-column>
      <!-- <el-table-column prop="applyTo" label="Applied For" width="120">
      </el-table-column> -->
      <el-table-column
        prop="status"
        label="Status"
        :filters="[
          { text: 'Applied', value: 'Applied' },
          { text: 'Approved', value: 'Approved' },
          { text: 'Training Completed', value: 'Training Completed' },
          { text: 'Rejected', value: 'Rejected' }
        ]"
        :filter-method="filterTag"
        filter-placement="bottom-end"
      >
        <template slot-scope="scope">
          <el-tag
            :type="
              scope.row.status === 'Approved'
                ? 'success'
                : scope.row.status === 'Applied'
                ? 'primary'
                : scope.row.status === 'Training Completed'
                ? 'warning'
                : 'danger'
            "
            disable-transitions
            >{{ scope.row.status }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="Operations">
        <template slot-scope="scope">
          <el-button size="mini" @click="handleView(scope.$index, scope.row)">View</el-button>
          <el-button v-if="scope.row.status == 'Training Completed'" size="mini" type="warning">Review</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div class="pagination">
      <el-pagination
        background
        layout="prev, pager, next"
        @current-change="handleCurrentChange"
        :page-size="this.pageSize"
        :total="this.total">
      </el-pagination>
    </div>
  </div>
</template>
<script>
import { mapGetters } from 'vuex'
export default {
  data() {
    return {
      textSearch: null,
      filtered: [],
      page: 1,
      pageSize: 10,
      total: 10,
      filterDate: [],
      table: [],
      tableData: [
        {
          applicationName: 'dsadxcxc',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 4',
          applicationDate: '2016-25-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Training Completed',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-25-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Training Completed',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-11-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-11-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Training Completed',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Rejected',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Approved',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-1-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-1-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Training Completed',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Rejected',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Approved',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test sdas 1',
          applicationDate: '2016-11-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Training Completed',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Rejected',
          operations: 'View'
        },
        {
          applicationName: 'sdasdasdasd',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Approved',
          operations: 'Review'
        },
        {
          applicationName: 'czxczxcx',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Training Completed',
          operations: 'Review'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Rejected',
          operations: 'View'
        },
        {
          applicationName: 'Test User 1',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Approved',
          operations: 'Review'
        },
        {
          applicationName: 'fgdff',
          applicationDate: '2016-05-03',
          occupation: 'Teacher',
          firstLanguage: 'English',
          appliedFor: 'IELTS',
          status: 'Applied',
          operations: 'View'
        }
      ]
    }
  },
  mounted() {
    this.$store.dispatch('rater/loadRaters')
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
    let test = { text: '', value: ''}
    this.filterDate = []
    let i = 0
    this.getAllRater.forEach(rs => {
      this.$set(this.filterDate, i++, { text: rs.applicationDate, value: rs.applicationDate })
    })
    this.filterDate = this.filterDate.filter((item, index) => this.filterDate.indexOf(item) === index);
    return table
    },
    handleView(index, row) {
      console.log(index, row);
      this.$router.push({ name: 'RaterDetails', params: { id: row.id } })
    }
  },
  computed: {
    displayData() {
      const table = this.searching()
      return table.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    },
    getAllRater(){
      return this.$store.getters['rater/getAll'];
    }
    // ...mapGetters({
    //   getAllRater: 'rater/getAll'
    // })
  }
}
</script>
<style>
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
.pagination{
  margin: 2rem;
  width: 100%;
  justify-content: center;
}
</style>
