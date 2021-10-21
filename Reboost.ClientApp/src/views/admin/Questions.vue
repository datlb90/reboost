<template>
  <div class="container">
    <div class="title">
      <h2>{{ messageTranslates('adminQuestions', 'allQuestion') }}</h2>
    </div>

    <el-row :gutter="20">
      <el-col :md="6" class="filter-container">
        <div>
          <el-input v-model="textSearch" size="mini" :placeholder="messageTranslates('adminQuestions', 'typeToSearch')" @input="search()" />
        </div>
      </el-col>
      <el-col :md="12" class="filter-container">
        <div class="filter-toolbar">
          <dropdown-menu v-model="filterSection" style="margin-right: 20px" :tittle="messageTranslates('adminQuestions', 'testSection')" @confirm="search()" @reset="resetFilterSection()" />
          <dropdown-menu v-model="filterType" style="margin-right: 20px" :tittle="messageTranslates('adminQuestions', 'type')" @confirm="search()" @reset="resetFilterType()" />
          <dropdown-menu v-model="filterStatus" :tittle="messageTranslates('adminQuestions', 'status')" @confirm="search()" @reset="resetFilterStatus()" />
        </div>

        <div class="tag-selection">
          <el-tag
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
          </el-tag>
        </div>
      </el-col>

      <el-col :md="6" class="filter-container">
        <div style="text-align: right;">
          <el-button size="mini" @click="clearFilter">{{ messageTranslates('adminQuestions', 'resetFilter') }}</el-button>
        </div>
      </el-col>
    </el-row>
    <el-row style="margin:10px 0px;">
      <el-col class="filter-container">
        <div style="text-align: right;">
          <el-button size="mini" @click="openAddQuestionDialog">{{ messageTranslates('adminQuestions', 'addQuestion') }}</el-button>
        </div>
      </el-col>
    </el-row>
    <el-table ref="filterTable" :data="questions" stripe style="width: 100%;">
      <el-table-column prop="id" sortable label="#" width="60" />
      <el-table-column prop="title" sortable :label="messageTranslates('adminQuestions', 'questionName')">
        <template slot-scope="scope">
          <span class="title-row cursor" style="word-break: break-word" @click="rowClicked(scope.row)">{{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="messageTranslates('adminQuestions', 'testSection')"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.test }} {{ scope.row.section }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="messageTranslates('adminQuestions', 'questionType')"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.type }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="messageTranslates('adminQuestions', 'createdBy')"
        width="120"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.createdBy }}</span>
        </template>
      </el-table-column>

      <el-table-column :label="messageTranslates('adminQuestions', 'status')" prop="status" width="110">
        <template slot-scope="scope">
          <!-- <i v-if="scope.row.status == 'Completed'" class="el-icon-check check" /> -->
          <el-tag
            v-if="scope.row.status == 'Completed'"
            :key="scope.row.status"
            :type="typeSuccess"
            size="mini"
            effect="dark"
          >
            {{ constantTranslate('QUESTION_STATUS', scope.row.status) }}
          </el-tag>
          <el-tag
            v-else
            :key="scope.row.status"
            size="mini"
            effect="dark"
          >
            {{ constantTranslate('QUESTION_STATUS', scope.row.status) }}
          </el-tag>
        </template>
      </el-table-column>

      <el-table-column
        :label="messageTranslates('adminQuestions', 'addedDate')"
        sortable
        prop="addedDate"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ getTimeFromDateCreateToNow(scope.row.addedDate) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="messageTranslates('adminQuestions', 'action')"
        width="180"
      >
        <template slot-scope="scope">
          <div style="display: flex; flex-direction: column;">
            <el-button style="margin-left:10px;" class="action-button" size="mini" @click="publishQuestion(scope.row)">{{ messageTranslates('adminQuestions', 'publish') }}</el-button>
            <el-button class="action-button" size="mini" @click="handleEdit(scope.row)">{{ messageTranslates('adminQuestions', 'edit') }}</el-button>
            <el-button class="action-button" size="mini" @click="previewQuestion(scope.row)">{{ messageTranslates('adminQuestions', 'preview') }}</el-button>
            <el-button class="action-button" size="mini" @click="openAddNewSampleDialog(scope.row)">{{ messageTranslates('adminQuestions', 'addNewSample') }}</el-button>
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
    <div>
      <add-edit-question ref="questionDialog" @refreshQuestion="getQuestionsData" />
      <question-preview ref="questionPreviewDialog" />
      <add-sample ref="sampleDialog" />
    </div>
  </div>
</template>
<script>
import _ from 'lodash'
import DropdownMenu from '../../components/controls/DropdownMenu'
import moment from 'moment'
import AddEditQuestion from '../../components/controls/AddEditQuestion.vue'
import QuestionPreview from '../../components/controls/QuestionPreview.vue'
import AddQuestionSample from '../../components/controls/AddQuestionSampleDialog.vue'
export default {
  name: 'AdminQuestions',
  components: {
    'dropdown-menu': DropdownMenu,
    'add-edit-question': AddEditQuestion,
    'question-preview': QuestionPreview,
    'add-sample': AddQuestionSample
  },
  data() {
    return {
      textSearch: null,
      questionCached: [],
      page: 1,
      totalRow: 10,
      questionsCount: 0,
      selectionTag: [],
      filterStatus: [],
      filterSection: [],
      filterType: [],
      questions: [],
      pageSize: 20,
      dialogVisible: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    getAllQuestions() {
      return this.$store.getters['question/getAll']
    }
  },
  mounted() {
    this.getQuestionsData()
  },
  methods: {
    getQuestionsData() {
      this.$store.dispatch('question/loadQuestions').then(rs => {
        this.questionCached = this.getAllQuestions
        this.totalRow = this.questionsCount = this.questionCached.length
        this.filterSection = Object.keys(_.groupBy(this.questionCached, 'section')).map(k => ({ text: k }))
        this.filterType = Object.keys(_.groupBy(this.questionCached, 'type')).map(k => ({ text: k }))
        this.filterStatus = Object.keys(_.groupBy(this.questionCached, 'status')).map(k => ({ text: k }))

        this.loadTable()
        console.log('1', this.selectionTag, this.filterStatus, this.filterSection, this.filterType)
      })
    },
    loadTable() {
      let filtered = this.filter().sort((a, b) => b.id - a.id)
      if (this.textSearch) {
        filtered = filtered.filter(q => q.title.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
        this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      } else {
        this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
        console.log('questions: ', filtered, this.questions)
      }
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
    }
  }
}
</script>
<style scoped>
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
</style>

