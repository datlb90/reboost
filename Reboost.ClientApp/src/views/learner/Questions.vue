<template>
  <div class="container">
    <div class="title">
      <h2>All Writing Topics</h2>
    </div>
    <div>
      <hr>
      <div style="display: flex; justify-content: space-between; align-items: center;">
        <div class="left-content">
          <div class="completed">
            <span> {{ taskCompleted }} /{{ totalRow }} Completed</span>
          </div>
          <div><span style="margin: 0 5px;">-</span></div>
          <div v-for="item in getCountQuestionByUser" :key="item">
            <span class="filter">{{ item.section }}: {{ item.count }}</span>
          </div>
        </div>
        <div class="btnPickOne">
          <el-button @click="clearFilter">Pick One
            <i class="el-icon-search" />
          </el-button>
        </div>
      </div>
      <hr>
    </div>
    <div class="searchAndBtn">
      <div class="search">
        <el-input v-model="textSearch" placeholder="Type to search" />
      </div>
      <div class="btn-reset">
        <el-button @click="clearFilter">reset all filters</el-button>
      </div>
    </div>
    <el-table ref="filterTable" :data="displayData" stripe style="width: 100%" @row-click="rowClicked">
      <el-table-column prop="id" label="#" width="35" />
      <el-table-column label="Title">
        <template slot-scope="scope">
          <span class="title-row">{{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="test"
        label="Test"
        width="60"
        column-key="test"
      />
      <el-table-column
        prop="section"
        label="Section"
        column-key="section"
        width="170"
        :filters="filterSection"
        :filter-method="filterHandler"
      />
      <el-table-column
        prop="type"
        label="Type"
        column-key="type"
        width="140"
        :filters="filterType"
        :filter-method="filterHandler"
      />
      <el-table-column
        width="80"
        label="Sample"
      >
        <template slot-scope="scope">
          <i v-if="scope.row.sample" class="el-icon-document" style="color: blue;" />
        </template>
      </el-table-column>
      <!-- <el-table-column
        prop="averageScore"
        label="Average Score"
        sortable
        width="150"
      /> -->
      <el-table-column
        prop="submission"
        label="Submission"
        sortable
        width="130"
      />
      <el-table-column prop="like" label="Like" width="75" sortable />
      <el-table-column label="Status" prop="status" width="70" />
    </el-table>
    <div class="pagination">
      <div class="current-page">
        <!-- <el-input-number v-model="rowPerPage" controls-position="right" @change="handleChangeRowPerPage" :min="1"></el-input-number> -->
        <!-- <input type="numer" min="1" v-model="currentPage">
        <ejs-numerictextbox v-bind:value='value'></ejs-numerictextbox> -->
        <input v-model="rowPerPage" class="form-control text-box single-line" data-val="true" type="number" value="rowPerPage" style="width: 70px;" @change="handleChangeRowPerPage">
        <span style="margin-left: 10px">rows per page.</span>
      </div>
      <div class="pagination-page">
        <el-pagination
          background
          layout="total, prev, pager, next, jumper"
          :page-size="pageSize"
          :total="totalRow"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
        <!-- <div style="margin-right: 10px"><span>Go to page</span></div>
        <div class="go-to-page">
          <el-input v-model="gotoPage"></el-input>
        </div> -->
      </div>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      textSearch: null,
      table: [],
      filterTest: [],
      filterSample: [],
      completed: 5,
      total: 5,
      nameFilter: 'Independent Writing 10',
      page: 1,
      currentPage: 1,
      gotoPage: 1,
      totalRow: 10,
      rowPerPage: 5,
      pageSize: 5,
      // filterTest: [
      //   { text: "TOEFL", value: "TOEFL"},
      //   { text: "IELTS", value: "IELTS"}
      // ],
      filterSection: [
        { text: 'Independent Writing', value: 'Independent Writing' },
        { text: 'Integrated Writing', value: 'Integrated Writing' },
        { text: 'Academic Writing Task 1', value: 'Academic Writing Task 1' },
        { text: 'Academic Writing Task 2', value: 'Academic Writing Task 2' },
        { text: 'General Training Task 1', value: 'General Training Task 1' },
        { text: 'General Training Task 2', value: 'General Training Task 2' }
      ],
      filterType: [
        { text: 'Agree/Disagree', value: 'Agree/Disagree' },
        { text: 'Summary', value: 'Summary' },
        { text: 'Diagram Report', value: 'Diagram Report\r\n' },
        { text: 'Discuss Views', value: 'Discuss Views' },
        { text: 'Writing Letter', value: 'Writing Letter' }
      ]
    }
  },
  computed: {
    displayData() {
      var table = this.searching()
      return table.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    },
    taskCompleted() {
      var data = this.getCountQuestionByUser
      var count = 0
      data.forEach(element => {
        count += element.count
      })
      return count
    },
    getAllQuestion() {
      return this.$store.getters['question/getAll']
    },
    getCountQuestionByUser() {
      return this.$store.getters['question/getCountQuestionsByUser']
    }
  },
  mounted() {
    this.$store.dispatch('question/loadQuestions')
    this.$store.dispatch('question/loadCountQuestionByUser', 'c7b56124-7d74-4017-841d-0447084240e4')
  },
  methods: {
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
    handleSizeChange(val) {
      console.log(`${val} items per page`)
    },
    handleCurrentChange(val) {
      this.page = val
    },
    // handleCurrentChange(val) {
    //   this.page = val
    // },
    handleChangeRowPerPage(val) {
      console.log(typeof this.rowPerPage)
      this.pageSize = +this.rowPerPage
    },
    rowClicked(row) {
      console.log('row', row)
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: row.id
        }
      })
    },
    searching() {
      const test = window.location.pathname
      const new_test = test.slice(11,).toUpperCase()
      if (!this.textSearch) {
        var table = this.getAllQuestion
        table = table.filter(t => t.test == new_test)
        this.totalRow = table.length
        return table
      }
      table = this.getAllQuestion.filter(data => data.title.toLowerCase().includes(this.textSearch.toLowerCase()))
      table = table.filter(t => t.test == new_test)
      this.totalRow = table.length
      return table
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
.left-content{
  display: flex;
  width: 85%;
}
.check {
  color: green;
  font-size: 20px;
  font-weight: bold;
  display: flex;
}
.completed{
  background-color: rgb(0, 81, 255);
  color: white;
  padding: 10px;
  border-radius: 20px;
  height: 12px;
  display: flex;
  align-items: center;
  margin-right: 5px;
  cursor: pointer;
  font-size: 9px;
}
.filter{
  background-color: rgb(35, 196, 3);
  font-size: 9px;
  color: white;
  padding: 10px;
  border-radius: 20px;
  height: 12px;
  display: flex;
  margin-right: 5px;
  align-items: center;
  cursor: pointer;
}
.pagination{
  display: flex;
  justify-content: space-between;
  margin-left: 0;
}
.pagination-page{
  display: flex;
  align-items: center;
}
.go-to-page{
  width: 50px;
}
.current-page{
  display: flex;
  align-items: center;
}
.title-row{
  font-weight: bold;
}
</style>
