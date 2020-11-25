<template>
  <div class="container">
    <div class="title">
      <h2>All Writing Topics</h2>
    </div>
    <div>
      <hr />
      <div style="display: flex; justify-content: space-between; align-items: center;">
        <div class="left-content">
          <div class="completed">
            <span>{{ getCountQuestionByTasks['Completed'] }}/{{ this.totalRow }} Completed</span>
          </div>
          <div><span style="margin: 0 5px;">-</span></div>
          <div v-for="(value, name) in getCountQuestionByTasks" :key="name.value">
            <div v-if="name != 'Completed'" class="filter">
              <span>{{ name }}: {{ value }}</span>
            </div>
            </div>
          </div>
        <div class="btnPickOne">
          <el-button @click="clearFilter">Pick One
            <i class="el-icon-search"></i>
          </el-button>
        </div>
      </div>
      <hr />
    </div>
    <div class="searchAndBtn">
      <div class="search">
        <el-input placeholder="Type to search" v-model="textSearch"></el-input>
      </div>
      <div class="btn-reset">
        <el-button @click="clearFilter">reset all filters</el-button>
      </div>
    </div>
    <el-table ref="filterTable" :data="displayData" @row-click="rowClicked" style="width: 100%">
      <el-table-column width="40">
        <template slot-scope="scope">
          <i v-if="scope.row.status == 'Active'" class="el-icon-check check"></i>
        </template>
      </el-table-column>
      <el-table-column prop="id" label="#" width="40"> </el-table-column>
      <el-table-column prop="title" label="Title" width="200"> </el-table-column>
      <el-table-column
        prop="test"
        label="Test"
        column-key="test"
        :filters="filterTest"
        :filter-method="filterHandler"
      >
      </el-table-column>
      <el-table-column
        prop="section"
        label="Section"
        column-key="section"
        :filters="filterSection"
        :filter-method="filterHandler"
      >
      </el-table-column>
      <el-table-column
        prop="type"
        label="Type"
        column-key="type"
        :filters="filterType"
        :filter-method="filterHandler"
      >
      </el-table-column>
      <el-table-column
        label="Sample"
        :filters="filterSample"
        :filter-method="filterHandler"
      >
        <template slot-scope="scope">
          <i v-if="scope.row.sample" class="el-icon-document" style="color: blue;"></i>
        </template>
      </el-table-column>
      <el-table-column
        prop="averageScore"
        label="Average Score"
        sortable
        width="150"
      >
      </el-table-column>
      <el-table-column
        prop="submission"
        label="Submission"
        sortable
        width="140"
      >
      </el-table-column>
      <el-table-column prop="like" label="Like" sortable>
      </el-table-column>
    </el-table>
    <div class="pagination">
      <div class="current-page">
        <!-- <el-input-number v-model="rowPerPage" controls-position="right" @change="handleChangeRowPerPage" :min="1"></el-input-number> -->
        <!-- <input type="numer" min="1" v-model="currentPage">
        <ejs-numerictextbox v-bind:value='value'></ejs-numerictextbox> -->
        <input class="form-control text-box single-line" v-model="rowPerPage" data-val="true" type="number" value="rowPerPage" style="width: 70px;">
        <span style="margin-left: 10px">rows per page.</span>
      </div>
      <div class="pagination-page">
        <el-pagination
          background
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          layout="total, prev, pager, next, jumper"
          :page-size="pageSize"
          :total="totalRow">
        </el-pagination>
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
      filterSection: [],
      filterType: [],
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
      summary: {
        // "Independent Writing": 1,
        // "Integrated Writing\r\n": 1,
        // "Academic Writing Task 1\r\n": 1,
        // "Academic Writing Task 2\r\n": 1,
        // "General Training Task 1\r\n": 1,
        // "General Training Task 2\r\n": 1,
        // "Completed": 1
      }
    };
  },
  mounted() {
    this.$store.dispatch("question/loadQuestions");
    this.$store.dispatch("question/loadCountQuestionsByTasks");
  },
  methods: {
    clearFilter() {
      this.$refs.filterTable.clearFilter();
      this.textSearch = "";
    },
    filterTag(value, row) {
      return row.status === value;
    },
    filterHandler(value, row, column) {
      const property = column["property"];
      return row[property] === value;
    },
    handleSizeChange(val) {
      console.log(`${val} items per page`);
    },
    handleCurrentChange(val) {
      this.page = val
    },
    // handleCurrentChange(val) {
    //   this.page = val
    // },
    handleChangeRowPerPage(val){
      this.pageSize = val
    },
    rowClicked(row) {
      console.log("row",row)
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: row.id
        }
      })
    },
    searching() {
      if (!this.textSearch) {
        this.totalRow = this.getAllQuestion.length
        return this.getAllQuestion
      }
      var table = this.getAllQuestion.filter(data => data.title.toLowerCase().includes(this.textSearch.toLowerCase()))
      this.totalRow = table.length
      return table
    },
  },
  computed: {
    displayData() {
      var table = this.searching()
      return table.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    },
    getAllQuestion() {
      return this.$store.getters["question/getAll"];
    },
    getCountQuestionByTasks() {
      return this.$store.getters["question/getCountQuestionByTasks"];
    }
  }
};
</script>
<style scoped>
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
</style>
