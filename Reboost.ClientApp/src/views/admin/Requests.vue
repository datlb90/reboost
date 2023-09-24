<template>
  <div class="requests-container">
    <div class="title">
      <h2>Pro Review Requests</h2>
    </div>

    <el-row :gutter="20">
      <el-col :md="6" class="filter-container">
        <div>
          <el-input v-model="textSearch" size="mini" placeholder="Search for request" @input="search()" />
        </div>
      </el-col>
      <el-col :md="12" class="filter-container">
        <!-- <div class="filter-toolbar">
          <dropdown-menu v-model="filterSection" style="margin-right: 20px" :tittle="messageTranslates('adminQuestions', 'testSection')" @confirm="search()" @reset="resetFilterSection()" />
          <dropdown-menu v-model="filterType" style="margin-right: 20px" :tittle="messageTranslates('adminQuestions', 'type')" @confirm="search()" @reset="resetFilterType()" />
          <dropdown-menu v-model="filterStatus" :tittle="messageTranslates('adminQuestions', 'status')" @confirm="search()" @reset="resetFilterStatus()" />
        </div> -->

        <div class="tag-selection">
          <!-- <el-tag
            v-for="tag in selectionTag"
            :key="tag"
            size="mini"
            type="success"
            effect="dark"
            closable
            :disable-transitions="false"
            @close="handleClose(tag)"
          >
            {{ tag }}
          </el-tag> -->
        </div>
      </el-col>

      <el-col :md="6" class="filter-container">
        <div style="text-align: right;">
          <!-- <el-button size="mini" @click="clearFilter">{{ messageTranslates('adminQuestions', 'resetFilter') }}</el-button> -->
        </div>
      </el-col>
    </el-row>
    <el-row style="margin:10px 0px;">
      <el-col class="filter-container">
        <div style="text-align: right;">
          <!-- <el-button size="mini" @click="openAddQuestionDialog">{{ messageTranslates('adminQuestions', 'addQuestion') }}</el-button> -->
        </div>
      </el-col>
    </el-row>
    <el-table ref="filterTable" :data="requests" stripe style="width: 100%;">
      <el-table-column prop="requestId" sortable label="Id" width="60" />
      <el-table-column prop="learnerName" sortable label="Learner" />
      <el-table-column prop="raterName" sortable label="Rater" />
      <el-table-column prop="requestStatus" sortable label="Request Status" width="160" />
      <el-table-column prop="assignmentStatus" sortable label="Assignment Status" width="190" />
      <el-table-column prop="reviewStatus" sortable label="Review Status" width="160" />
      <!-- <el-table-column prop="submissionStatus" sortable label="Submission Status" width="180" /> -->
      <el-table-column prop="submissionId" sortable label="SID" width="80" />
      <el-table-column prop="questionId" sortable label="QID" width="80" />
      <el-table-column prop="docId" sortable label="DID" width="80" />
      <el-table-column prop="reviewId" sortable label="RID" width="80" />
      <!-- <el-table-column
        label="Requested Date"
        sortable
        prop="requestedDateTime"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ getTimeFromDateCreateToNow(scope.row.requestedDateTime) }}</span>
        </template>
      </el-table-column> -->
      <el-table-column
        label="Actions"
      >
        <template slot-scope="scope">
          <div>
            <el-button size="mini" @click="publishQuestion(scope.row)">View</el-button>
            <!-- <el-button class="action-button" size="mini" @click="handleEdit(scope.row)">{{ messageTranslates('adminQuestions', 'edit') }}</el-button>
            <el-button class="action-button" size="mini" @click="previewQuestion(scope.row)">{{ messageTranslates('adminQuestions', 'preview') }}</el-button>
            <el-button class="action-button" size="mini" @click="openAddNewSampleDialog(scope.row)">{{ messageTranslates('adminQuestions', 'addNewSample') }}</el-button>
            <el-button class="action-button" size="mini" @click="deleteQuestion(scope.row)">{{ messageTranslates('adminQuestions', 'delete') }}</el-button> -->
          </div>
        </template>
      </el-table-column>
    </el-table>

    <div class="pagination">
      <el-pagination
        background
        layout="total, sizes, prev, pager, next, jumper"
        :page-size="pageSize"
        :total="totalRow"
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
      />
    </div>
    <!-- <div>
      <add-edit-question ref="questionDialog" @refreshQuestion="getQuestionsData" />
      <question-preview ref="questionPreviewDialog" />
      <add-sample ref="sampleDialog" />
    </div> -->
    <!-- <el-dialog
      title="Delete"
      :visible.sync="deleteDialogVisible"
      width="30%"
    >
      <span>Delete this question will delete all of its samples. Do you want to continue ? This process can not be undone.</span>
      <span slot="footer" class="dialog-footer">
        <el-button size="mini" @click="deleteDialogVisible = false">Cancel</el-button>
        <el-button size="mini" type="primary" @click="deleteConfirmed()">Confirm</el-button>
      </span>
    </el-dialog> -->
  </div>
</template>
<script>
import _ from 'lodash'
import moment from 'moment'
// import reviewService from '../../services/review.service'
// import questionService from '../../services/question.service'
export default {
  name: 'AdminRequests',
  components: {
    // 'dropdown-menu': DropdownMenu,
    // 'add-edit-question': AddEditQuestion,
    // 'question-preview': QuestionPreview,
    // 'add-sample': AddQuestionSample
  },
  data() {
    return {
      textSearch: null,
      requestCached: [],
      page: 1,
      totalRow: 10,
      requestsCount: 0,
      selectionTag: [],
      filterStatus: [],
      filterSection: [],
      filterType: [],
      requests: [],
      pageSize: 20,
      dialogVisible: false,
      deleteDialogVisible: false,
      selectedQuestion: null
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    getAllRequests() {
      return this.$store.getters['review/getAllRequests']
    }
  },
  mounted() {
    this.getRequestsData()
    console.log(this.requests)
  },
  methods: {
    getRequestsData() {
      this.$store.dispatch('review/loadRequests').then(rs => {
        this.requestCached = this.getAllRequests
        console.log(this.requestCached)
        this.totalRow = this.requestsCount = this.requestCached.length
        this.filterSection = Object.keys(_.groupBy(this.requestCached, 'section')).map(k => ({ text: k }))
        this.filterType = Object.keys(_.groupBy(this.requestCached, 'type')).map(k => ({ text: k }))
        this.filterStatus = Object.keys(_.groupBy(this.requestCached, 'status')).map(k => ({ text: k }))
        this.loadTable()
      })
    },
    loadTable() {
      // let filtered = this.filter().sort((a, b) => b.id - a.id)
      let filtered = this.requestCached
      if (this.textSearch) {
        filtered = filtered.filter(q => q.title.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
        this.requests = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      } else {
        this.requests = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      }
      console.log(this.requests)
    },
    clearFilter() {
      this.textSearch = ''
      this.filterSection = this.filterSection.map(i => ({ ...i, checked: false }))
      this.filterType = this.filterType.map(i => ({ ...i, checked: false }))
      this.filterStatus = this.filterStatus.map(i => ({ ...i, checked: false }))
      this.loadTable()
    },
    search() {
      this.page = 1
      this.loadTable()
    },
    filter() {
      let result = []
      this.selectionTag = []
      const _filteredSection = this.filterSection.filter(s => s.checked).map(s => s.text)
      const _filteredType = this.filterType.filter(s => s.checked).map(s => s.text)
      const _filteredStatus = this.filterStatus.filter(s => s.checked).map(s => s.text)

      this.selectionTag = _filteredSection.concat(_filteredType, _filteredStatus)

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

      result = result.sort((a, b) => a.id - b.id)
      return result
    },
    handleClose(tag) {
      this.selectionTag.splice(this.selectionTag.indexOf(tag), 1)

      const _filteredSection = this.filterSection.filter(s => s.text == tag)
      const _filteredType = this.filterType.filter(s => s.text == tag)
      const _filteredStatus = this.filterStatus.filter(s => s.text == tag)

      if (_filteredSection.length > 0) {
        this.filterSection.find(s => s.text == tag).checked = false
      } else if (_filteredType.length > 0) {
        this.filterType.find(s => s.text == tag).checked = false
      } else if (_filteredStatus.length > 0) {
        this.filterStatus.find(s => s.text == tag).checked = false
      }
      this.loadTable()
    },
    getTimeFromDateCreateToNow(time) {
      return moment(new Date(time)).format('MMMM Do YYYY, hh:mm:ss a')
    },
    handelClick(index, data) {
      console.log(index, data)
    },
    handleSizeChange(val) {
      this.pageSize = val
      this.loadTable()
    },
    rowClicked(e) {
      console.log('question: ', e)
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadTable()
    },
    openAddQuestionDialog() {
        this.$refs.questionDialog?.openDialog(false)
    },
    handleEdit(e) {
        this.$refs.questionDialog?.openEditDialog(e)
    },
    publishQuestion(e) {
        this.$refs.questionDialog?.publishQuestion(e)
    },
    previewQuestion(e) {
        this.$refs.questionPreviewDialog?.showDialog(e)
    },
    openAddNewSampleDialog(e) {
        this.$refs.sampleDialog?.openDialog(e.id)
    },
    deleteQuestion(e) {
      this.selectedQuestion = e
      this.deleteDialogVisible = true
    },
    deleteConfirmed() {
    //   questionService.deleteQuestion(this.selectedQuestion.id).then(rs => {
    //     if (rs) {
    //       this.$notify.success({
    //         title: 'Question deleted.',
    //         message: 'Question deleted successfully.',
    //         type: 'success',
    //         duration: 2000
    //       })
    //       this.getQuestionsData()
    //     }
    //     this.deleteDialogVisible = false
    //   })
    }
  }
}
</script>
  <style scoped>

  .requests-container{
    max-width: 80%;
    margin: auto;
  }
  @media only screen and (max-width: 880px) {
    .filter-container{
      padding: 5px 0;
    }
  }
  .container {
    margin-top: 20px;
  }
  .filter-toolbar{
      display: flex;
      z-index: 1;
      align-items: center;
  }
  .filterTable{
    display: flex;
    margin-left: 10px;
  }
  .action-button{
      margin: 5px;
      width: 80%;
  }
  .pagination-container {
    background: #fff;
    padding: 32px 16px;
  }
  .pagination {
    margin: 2rem;
  }
  .el-pagination {
    width: 100%;
    text-align: center;
  }
  .el-dialog__body span {
    word-break: break-word;
  }
  </style>

