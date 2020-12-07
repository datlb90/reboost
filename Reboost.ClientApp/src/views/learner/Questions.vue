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
            <span> {{ taskCompleted }} /{{ questionsCount }} Completed</span>
          </div>
          <div><span style="margin: 0 5px;">-</span></div>
          <div v-for="item in summary" :key="item">
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
        <el-input v-model="textSearch" placeholder="Type to search" @input="search()" />
      </div>
      <div class="btn-reset">
        <el-button @click="clearFilter">reset all filters</el-button>
      </div>
    </div>
    <el-table ref="filterTable" :data="questions" stripe style="width: 100%" @row-click="rowClicked">
      <el-table-column prop="id" label="#" width="50" />
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
      <el-table-column label="Status" prop="status" width="110" :filters="filterStatus" :filter-method="filterHandler">
        <template slot-scope="scope">
          <i v-if="scope.row.status == 'Completed'" class="el-icon-check check" />
        </template>
      </el-table-column>
    </el-table>
    <div class="pagination">
      <!-- <div class="current-page">
        <input v-model="rowPerPage" class="form-control text-box single-line" data-val="true" type="number" value="rowPerPage" style="width: 50px; height: 12px;" @change="handleChangeRowPerPage">
        <span style="margin-left: 10px">rows per page.</span>
      </div> -->
      <!-- <div class="pagination-page"> -->
      <el-pagination
        background
        layout="total, sizes, prev, pager, next, jumper"
        :page-size="pageSize"
        :total="totalRow"
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
      />
      <!-- <div style="margin-right: 10px"><span>Go to page</span></div>
        <div class="go-to-page">
          <el-input v-model="gotoPage"></el-input>
        </div> -->
      <!-- </div> -->
    </div>
  </div>
</template>
<script>
import _ from 'lodash'
export default {
  data() {
    return {
      textSearch: null,
      table: [],
      filterSample: [],
      completed: 5,
      total: 5,
      page: 1,
      currentPage: 1,
      gotoPage: 1,
      totalRow: 10,
      questionsCount: 0,
      rowPerPage: 5,
      pageSize: 10,
      filterStatus: [
        { text: 'Completed', value: 'Completed' },
        { text: 'To do', value: 'To do' }
      ],
      filterSection: [],
      filterType: [],
      countQuestions: null,
      loadTest: [],
      loadStatus: [],
      questions: [],
      questionCached: [],
      summary: []
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    // displayData() {
    //   var table = this.searching()
    //   return table.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    // },
    taskCompleted() {
      return this.summary.reduce((prev, cur) => (prev + cur.count), 0)
    }
    // handlingData() {
    //   var table = this.questions
    //   var listQuestionCompleted = this.getStatusQuestion
    //   table.forEach(rs => {
    //     var check = listQuestionCompleted.filter(r => rs.Id == r)
    //     if (check) {
    //       rs.status = 'Completed'
    //     } else {
    //       rs.status = 'To do'
    //     }
    //   })
    //   return table
    // },
    // handlingSummary() {
    //   const testName = this.loadTest
    //   var data = this.getCountQuestionByUser
    //   if (testName.length == 2) {
    //     return data
    //   }
    //   data = data.filter(rs => rs.name == testName[0])
    //   console.log('data', data)
    //   return data
    // }
    // getAllQuestion() {
    //   return this.$store.getters['question/getAll']
    // },
    // getCountQuestionByUser() {
    //   return this.$store.getters['question/getCountQuestionsByUser']
    // },
    // getTestByUser() {
    //   return this.$store.getters['question/getTestByUser']
    // },
    // getStatusQuestion() {
    //   return this.$store.getters['question/getStatusQuestion']
    // }
  },
  mounted() {
    this.$store.dispatch('question/loadAllQuestionByUser', this.currentUser.id).then(questions => {
      // console.log('QUESTION LOAD COMPLETED', this.$store.getters['question/getAll'])
      this.questionCached = this.$store.getters['question/getAll']
      this.totalRow = this.questionsCount = this.questionCached.length

      this.loadTable()
    })
    // this.$store.dispatch('question/loadQuestions').then(() => {
    //   this.questions = this.$store.getters['question/getAll']
    // })
    this.$store.dispatch('question/loadSummaryByUser', this.currentUser.id).then(() => {
      // this.countQuestions = this.$store.getters['question/getCountQuestionsByUser']
      console.log('QUESTION_SUMMARY', this.$store.getters['question/getSummaryByUser'])
      this.summary = this.$store.getters['question/getSummaryByUser']
    })
    // this.$store.dispatch('question/loadTestByUser', this.currentUser.id).then(() => {
    //   this.loadTest = this.$store.getters['question/getTestByUser']
    // })
    // this.$store.dispatch('question/loadStatusQuestion', this.currentUser.id).then(() => {
    //   this.loadStatus = this.$store.getters['question/getStatusQuestion']
    // })
  },
  methods: {
    loadTable() {
      if (this.textSearch) {
        const filtered = this.questionCached.filter(q => q.title.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
        this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      } else {
        this.questions = this.questionCached.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = this.questionCached.length
      }

      this.filterSection = Object.keys(_.groupBy(this.questions, 'section')).map(k => ({ text: k, value: k }))
      this.filterType = Object.keys(_.groupBy(this.questions, 'type')).map(k => ({ text: k, value: k }))
    },
    // loadSummary() {
    //   const usersByColor = users.reduce((acc, value) => {
    //     // Group initialization
    //     if (!acc[value.color]) {
    //       acc[value.color] = []
    //     }

    //     // Grouping
    //     acc[value.color].push(value)

    //     return acc
    //   }, {})
    // },
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
      this.pageSize = val
      this.loadTable()
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadTable()
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
    search() {
      this.loadTable()
      // const testName = this.loadTest
      // var table = this.handlingData
      // this.filterType = []
      // this.filterSection = []
      // for (const i of table) {
      //   var section = { text: i.section, value: i.section }
      //   var type = { text: i.type, value: i.type }
      //   this.filterSection.push(section)
      //   this.filterType.push(type)
      // }

      // this.filterSection = this.filterSection.filter((item, index) => this.filterSection.indexOf(item) === index)
      // this.filterType = this.filterType.filter((item, index) => this.filterType.indexOf(item) === index)

      // console.log(table)
      // if (testName.length == 2) {
      //   if (!this.textSearch) {
      //     this.totalRow = table.length
      //     return table
      //   }
      //   table = table.filter(data => data.title.toLowerCase().includes(this.textSearch.toLowerCase()))
      //   this.totalRow = table.length
      //   return table
      // } else {
      //   table = table.filter(data => data.test.includes(testName[0]))
      //   if (!this.textSearch) {
      //     this.totalRow = table.length
      //     return table
      //   }
      //   table = table.filter(data => data.title.toLowerCase().includes(this.textSearch.toLowerCase()))
      //   this.totalRow = table.length
      //   return table
      // }
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
.el-pagination {
  width: 100%;
  text-align: center;
}
</style>
