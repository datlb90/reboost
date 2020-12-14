<template>
  <div class="newcontainer">
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
          <div v-for="item in summary" :key="item.section">
            <span class="filter">{{ item.section }}: {{ item.count }}</span>
          </div>
        </div>
        <div class="btnPickOne">
          <el-button size="mini" @click="clickPickOne">Pick One
            <i class="fas fa-random" />
          </el-button>
        </div>
      </div>
      <hr>
    </div>
    <div class="searchAndBtn">
      <div class="search">
        <div>
          <el-input v-model="textSearch" placeholder="Type to search" @input="search()" />
        </div>
        <div class="filter-toolbar">
          <dropdown-menu id="ddFilterSection" v-model="filterSection" style="margin-right: 20px" :tittle="'Test Section'" @confirm="loadTable()" @reset="loadTable()" />
          <dropdown-menu v-model="filterType" style="margin-right: 20px" :tittle="'Type'" @confirm="loadTable()" @reset="loadTable()" />
          <dropdown-menu v-model="filterStatus" :tittle="'Status'" @confirm="loadTable()" @reset="loadTable()" />
        </div>
      </div>
      <div class="btn-reset">
        <el-button size="mini" @click="clearFilter">reset all filters</el-button>
      </div>
    </div>
    <el-table ref="filterTable" :data="questions" stripe style="width: 100%">
      <el-table-column type="index" label="#" width="50" />
      <el-table-column label="Title">
        <template slot-scope="scope">
          <span class="title-row cursor" @click="rowClicked(scope.row)">{{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="Test Section"
      >
        <template slot-scope="scope">
          {{ scope.row.test }} {{ scope.row.section }}
        </template>
      </el-table-column>
      <el-table-column
        prop="type"
        label="Type"
      />
      <el-table-column
        width="80"
        label="Sample"
      >
        <template slot-scope="scope">
          <div v-if="scope.row.sample" class="showTitle">
            <el-tooltip class="item" effect="dark" content="Sample responses are available for this topic." placement="bottom">
              <i class="el-icon-document" style="color: blue;" />
            </el-tooltip>
          </div>
        </template>
      </el-table-column>
      <el-table-column label="Status" prop="status" width="110">
        <template slot-scope="scope">
          <!-- <i v-if="scope.row.status == 'Completed'" class="el-icon-check check" /> -->
          <el-tag
            v-if="scope.row.status == 'Completed'"
            :key="scope.row.status"
            :type="typeSuccess"
            size="mini"
            effect="dark"
          >
            {{ scope.row.status }}
          </el-tag>
          <el-tag
            v-else
            :key="scope.row.status"
            size="mini"
            effect="dark"
          >
            {{ scope.row.status }}
          </el-tag>
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
import DropdownMenu from '../../components/controls/DropdownMenu'
export default {
  components: {
    'dropdown-menu': DropdownMenu
  },
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
      pageSize: 50,
      filterStatus: [
        { text: 'Completed', value: 'Completed' },
        { text: 'To do', value: 'To do' }
      ],
      typeSuccess: 'success',
      filterSection: [],
      filterType: [],
      countQuestions: null,
      loadTest: [],
      loadStatus: [],
      questions: [],
      questionCached: [],
      summary: [],
      selectedSection: -1,
      selectedType: -1,
      selectedStatus: -1
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    taskCompleted() {
      return this.summary.reduce((prev, cur) => (prev + cur.count), 0)
    }
  },
  mounted() {
    this.$store.dispatch('question/loadAllQuestionByUser', this.currentUser.id).then(questions => {
      this.questionCached = this.$store.getters['question/getAll']
      this.totalRow = this.questionsCount = this.questionCached.length
      this.filterSection = Object.keys(_.groupBy(this.questionCached, 'section')).map(k => ({ text: k }))
      this.filterType = Object.keys(_.groupBy(this.questionCached, 'type')).map(k => ({ text: k }))
      this.loadTable()
    })
    this.$store.dispatch('question/loadSummaryByUser', this.currentUser.id).then(() => {
      this.summary = this.$store.getters['question/getSummaryByUser']
    })
  },
  methods: {
    loadTable() {
      let filtered = this.filter()
      if (this.textSearch) {
        filtered = filtered.filter(q => q.title.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
        this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      } else {
        this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      }
    },
    clearFilter() {
      this.textSearch = ''
      this.filterSection.forEach(element => {
        element.checked = false
      })
      this.filterType.forEach(element => {
        element.checked = false
      })
      this.filterStatus.forEach(element => {
        element.checked = false
      })
      this.loadTable()
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
    handleChangeRowPerPage(val) {
      console.log(typeof this.rowPerPage)
      this.pageSize = +this.rowPerPage
    },
    rowClicked(row) {
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: row.id
        }
      })
    },
    clickPickOne() {
      var chosenNumber = Math.floor(Math.random() * this.questions.length)
      var id = this.questions[chosenNumber].id
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: id
        }
      })
    },
    search() {
      this.loadTable()
    },
    filter() {
      const result = []
      const _filteredSection = this.filterSection.filter(s => s.checked).map(s => s.text)
      const _filteredType = this.filterType.filter(s => s.checked).map(s => s.text)
      const _filteredStatus = this.filterStatus.filter(s => s.checked).map(s => s.text)

      for (const q of this.questionCached) {
        let pass = true
        if (_filteredSection.length > 0 && !_filteredSection.includes(q.section)) {
          pass = false
        }
        if (_filteredType.length > 0 && !_filteredType.includes(q.type)) {
          pass = false
        }
        if (_filteredStatus.length > 0 && !_filteredStatus.includes(q.status)) {
          pass = false
        }

        if (pass) {
          result.push(q)
        }
      }

      // this.page = 1
      // this.questions = result.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      // this.totalRow = result.length

      // result = result.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)

      return result
    }
    // filterBySection(data) {
    //   const filtered = this.questionCached.filter(r => data.includes(r.section))
    //   // this.page = 1
    //   // this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    //   // this.totalRow = filtered.length

    //   // console.log('ON FILTER SECTION', eventData)
    // },
    // onFilterType(text, index) {
    //   const filtered = this.questionCached.filter(r => r.type == text)
    //   this.page = 1
    //   this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    //   this.totalRow = filtered.length
    //   this.selectedType = index
    // },
    // onFilterStatus(text, index) {
    //   const filtered = this.questionCached.filter(r => r.status == text)
    //   this.page = 1
    //   this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    //   this.totalRow = filtered.length
    //   this.selectedStatus = index
    // }
  }
}
</script>
<style scoped>
.newcontainer {
  padding: 0 120px;
  margin-top: 20px;
}
el-table{
  word-break: normal;
}
.search {
  width: 70%;
  display: flex;
  align-items: center;
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
.cursor{
  cursor: pointer;
}
.showTitle{
  cursor: pointer;
}
.showTitleText{
  visibility: hidden;
  width: 320px;
  background-color: black;
  color: #fff;
  text-align: center;
  border-radius: 6px;
  padding: 5px 0;
  /* Position the tooltip */
  position: absolute;
  z-index: 2000;
}
.showTitle:hover .showTitleText{
    visibility: visible;
    margin-left: -160px;
    margin-top: 20px;
}
.filterTable{
  display: flex;
  margin-left: 10px;
}
.iconCheck{
  width: 10%;
}
.textDropdown{
  width: 90%;
  margin-left: 5px;
}
.filter-toolbar{
      display: inherit;
    margin-left: 40px;
    z-index: 1;
}

</style>
