<template>

  <div class="list-container">
    <div class="top-navigator" style="height: 30px;">
      <el-button
        v-if="showLeftArrow"
        type="text"
        size="medium"
        icon="el-icon-d-arrow-left"
        style="float: left; color: grey; padding-bottom: 8px; padding-top: 8px; margin-right: 10px;"
        @click="moveLeft()"
      />
      <div id="topic-container" style="display: flex; float: left; width: calc(100% - 200px); overflow: hidden;">
        <el-tag
          type="info"
          :effect="allTopicEffect"
          style="font-size: 14px; margin-right: 5px; margin-bottom: 5px; cursor: pointer;"
          @click="onTopicClick(null)"
        >
          All Topics: {{ questionsCount }}
        </el-tag>
        <el-tag
          v-for="item in summary"
          :key="item.section"
          type="info"
          :effect="item.effect"
          style="font-size: 14px; margin-right: 5px; margin-bottom: 5px; cursor: pointer;"
          @click="onTopicClick(item)"
        >
          {{ item.section }}: {{ item.count }}
        </el-tag>
      </div>

      <div>
        <el-button
          size="mini"
          style="float: right; padding-bottom: 8px; padding-top: 8px; color: #409EFF;"
          @click="openAddQuestionDialog"
        >
          {{ messageTranslates('adminQuestions', 'addQuestion') }}
        </el-button>
      </div>
    </div>
    <div style="height: 40px;">
      <div class="filter-container" style="width: 400px; float: left;">
        <div class="filter-toolbar" style="margin-top: 10px;">
          <el-dropdown
            placement="bottom-start"
            :hide-on-click="false"
            style="float: left; margin-right: 15px;"
            @command="onFilterChange"
          >
            <span class="el-dropdown-link" style="cursor: pointer;">
              <el-link :underline="false" type="info">
                Test Sections<i class="el-icon-arrow-down el-icon--right" />
              </el-link>
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-for="item in filterSections"
                :key="item.text"
                :command="item"
                :icon="item.checked ? 'el-icon-success' : 'el-icon-remove-outline'"
              >{{ item.text }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <el-dropdown
            placement="bottom-start"
            :hide-on-click="false"
            style="float: left; margin-right: 15px;"
            @command="onFilterChange"
          >
            <span class="el-dropdown-link" style="cursor: pointer;">
              <el-link :underline="false" type="info">
                Types<i class="el-icon-arrow-down el-icon--right" />
              </el-link>
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-for="item in filterTypes"
                :key="item.text"
                :command="item"
                :icon="item.checked ? 'el-icon-success' : 'el-icon-remove-outline'"
              >{{ item.text }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>

          <el-dropdown
            placement="bottom-start"
            :hide-on-click="false"
            style="float: left; margin-right: 15px;"
            @command="onFilterChange"
          >
            <span class="el-dropdown-link" style="cursor: pointer;">
              <el-link :underline="false" type="info">
                Difficulty<i class="el-icon-arrow-down el-icon--right" />
              </el-link>
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-for="item in filterDifficulties"
                :key="item.text"
                :command="item"
                :icon="item.checked ? 'el-icon-success' : 'el-icon-remove-outline'"
              >{{ item.text }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>

          <el-dropdown
            placement="bottom-start"
            :hide-on-click="false"
            style="float: left; margin-right: 15px;"
            @command="onFilterChange"
          >
            <span class="el-dropdown-link" style="cursor: pointer;">
              <el-link :underline="false" type="info">
                Status<i class="el-icon-arrow-down el-icon--right" />
              </el-link>
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-for="item in filterStatuses"
                :key="item.text"
                :command="item"
                :icon="item.checked ? 'el-icon-success' : 'el-icon-remove-outline'"
              >{{ item.text }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <i class="el-icon-document" style="cursor: pointer;" alt="Sample" @click="filterSample" />
        </div>
      </div>

      <div class="filter-container" style="width: calc(100% - 400px); float: right;">
        <el-button size="mini" style="float: right; margin-top: 5px; margin-left: 5px;" @click="clearFilter">
          {{ messageTranslates('question', 'resetAll') }}
        </el-button>
        <el-input
          v-model="textSearch"
          size="mini"
          :placeholder="messageTranslates('question', 'placeholderSearch')"
          style="float: right; width: 200px; margin-top: 5px;"
          @input="search()"
        />
      </div>
    </div>

    <div v-if="selectionTag && selectionTag.length > 0" class="tag-selection">
      <el-tag
        v-for="tag in selectionTag"
        :key="tag"
        size="small"
        type="info"
        closable
        :disable-transitions="false"
        style="margin-right: 5px; margin-top: 5px;"
        @close="handleClose(tag)"
      >
        {{ tag }}
      </el-tag>
    </div>

    <el-table
      v-if="questions"
      ref="filterTable"
      :data="questions"
      stripe
      style="width: 100%;"
      @sort-change="sortChange"
    >
      <el-table-column
        :label="messageTranslates('question', 'titleTable')"
        prop="title"
        sortable
        fixed="left"
        min-width="200"
      >
        <template slot-scope="scope">
          <span class="title-row cursor" style="word-break: break-word" @click="rowClicked(scope.row)">{{ scope.row.id }}. {{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="Test"
        prop="test"
        width="75"
        sortable
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.test }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="Section"
        prop="section"
        width="200"
        sortable
      >
        <template slot-scope="scope">
          <span style="word-break: break-word"> {{ scope.row.section }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="messageTranslates('question', 'typeTable')"
        prop="type"
        width="190"
        sortable
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.type }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="Status"
        prop="status"
        width="110"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word"> {{ scope.row.status }}</span>
        </template>
      </el-table-column>

      <el-table-column label="Difficulty" prop="difficulty" sortable width="110">
        <template slot-scope="scope">
          <el-tag
            v-if="scope.row.difficulty == 'Medium'"
            :key="scope.row.difficulty"
            type="warning"
            size="mini"
          >
            {{ scope.row.difficulty }}
          </el-tag>
          <el-tag
            v-else-if="scope.row.difficulty == 'Hard'"
            :key="scope.row.difficulty"
            type="danger"
            size="mini"
          >
            {{ scope.row.difficulty }}
          </el-tag>
          <el-tag
            v-else-if="scope.row.difficulty == 'Undefined'"
            :key="scope.row.difficulty"
            type="info"
            size="mini"
          >
            {{ scope.row.difficulty }}
          </el-tag>
          <el-tag
            v-else
            :key="scope.row.difficulty"
            size="mini"
            type="success"
          >
            {{ scope.row.difficulty }}
          </el-tag>
        </template>
      </el-table-column>

      <el-table-column
        width="100"
        :label="messageTranslates('question', 'sampleTable')"
        prop="sample"
        sortable
      >
        <template slot-scope="scope">
          <div v-if="scope.row.sample" class="show-title">
            <el-tooltip class="item" effect="dark" content="Sample responses are available for this topic." placement="bottom">
              <i class="el-icon-document" />
            </el-tooltip>
          </div>
        </template>
      </el-table-column>
      <el-table-column
        width="40"
        fixed="right"
      >
        <template slot-scope="scope">
          <el-dropdown trigger="click" placement="bottom" style="float: right; cursor: pointer;" class="action" @command="command => handleActionCommand(command, scope.row)">
            <span class="el-dropdown-link action">
              <i class="el-icon-menu" style="font-size: 16px; padding: 10px;" />
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item command="edit"> Edit</el-dropdown-item>
              <el-dropdown-item command="sample"> Add Sample</el-dropdown-item>
              <el-dropdown-item command="preview"> Preview</el-dropdown-item>
              <el-dropdown-item command="publish"> Publish</el-dropdown-item>
              <el-dropdown-item command="delete"> Delete</el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
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
      <add-sample ref="sampleDialog" @refreshQuestion="getQuestionsData" />
    </div>
    <el-dialog
      title="Delete"
      :visible.sync="deleteDialogVisible"
      width="30%"
    >
      <span>Delete this question will delete all of its samples. Do you want to continue ? This process can not be undone.</span>
      <span slot="footer" class="dialog-footer">
        <el-button size="mini" @click="deleteDialogVisible = false">Cancel</el-button>
        <el-button size="mini" type="primary" @click="deleteConfirmed()">Confirm</el-button>
      </span>
    </el-dialog>
  </div>
</template>
<script>
import _ from 'lodash'
import moment from 'moment-timezone'
import AddEditQuestion from '../../components/controls/AddEditQuestion.vue'
import QuestionPreview from '../../components/controls/QuestionPreview.vue'
import AddQuestionSample from '../../components/controls/AddQuestionSampleDialog.vue'
import questionService from '../../services/question.service'
export default {
  name: 'AdminQuestions',
  components: {
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
      filterSections: [],
      selectedSections: [],
      filterTypes: [],
      selectedTypes: [],
      filterStatuses: [],
      selectedStatus: [],
      questions: [],
      pageSize: 20,
      dialogVisible: false,
      deleteDialogVisible: false,
      selectedQuestion: null,
      table: [],
      completed: 5,
      total: 5,
      currentPage: 1,
      gotoPage: 1,
      rowPerPage: 5,
      typeSuccess: 'success',
      filterDifficulties: [],
      selectedDifficulty: [],
      countQuestions: null,
      loadTest: [],
      loadStatus: [],
      summary: [],
      checkSample: false,
      showLeftArrow: false,
      showRightArrow: false,
      allTopicEffect: 'dark',
      sortedQuestions: null
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
      this.$store.dispatch('question/loadQuestions', this.currentUser.id).then(questions => {
        this.questionCached = this.$store.getters['question/getAll']

        console.log('Questions: ', this.questionCached)
        this.totalRow = this.questionsCount = this.questionCached.length
        this.filterSections = Object.keys(_.groupBy(this.questionCached, 'section')).map(k => ({ text: k, checked: false }))
        this.filterTypes = Object.keys(_.groupBy(this.questionCached, 'type')).map(k => ({ text: k, checked: false }))
        this.filterStatuses = Object.keys(_.groupBy(this.questionCached, 'status')).map(k => ({ text: k, checked: false }))
        this.filterDifficulties = Object.keys(_.groupBy(this.questionCached, 'difficulty')).map(k => ({ text: k, checked: false }))
        // Sort difficulty
        var order = ['Easy', 'Medium', 'Hard']
        this.filterDifficulties = this.filterDifficulties.sort(function(a, b) {
          return order.indexOf(a.text) - order.indexOf(b.text)
        })

        this.loadTable()
        this.loadSummary()
        this.$nextTick(function() {
          this.showArrow()
        })
      })
    },
    loadTable() {
      this.sortedQuestions = null
      const filtered = this.filter()
      this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.totalRow = filtered.length
    },
    filter() {
      let result = []
      this.selectionTag = []
      const _filteredSection = this.filterSections.filter(s => s.checked).map(s => s.text)
      const _filteredType = this.filterTypes.filter(s => s.checked).map(s => s.text)
      const _filteredStatus = this.filterStatuses.filter(s => s.checked).map(s => s.text)
      const _filteredDifficulty = this.filterDifficulties.filter(s => s.checked).map(s => s.text)

      this.selectionTag = _filteredSection.concat(_filteredType, _filteredStatus, _filteredDifficulty)

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
        if (_filteredDifficulty.length > 0 && !_filteredDifficulty.includes(q.difficulty)) {
          pass = false
        }
        if (pass) {
          result.push(q)
        }
      }
      if (this.checkSample) {
        result = result.filter(rs => rs.sample == true)
      }
      if (this.textSearch) {
        result = result.filter(q => q.title.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
      }
      result = result.sort((a, b) => a.lastActivityDate - b.lastActivityDate)
      return result
    },
    loadSummary() {
      var counts = this.questionCached.reduce((p, c) => {
        var name = c.section
        if (!p.hasOwnProperty(c.section)) {
          p[name] = 0
        }
        p[name]++
        return p
      }, {})

      this.summary = Object.keys(counts).map(k => {
        return { section: k, count: counts[k], effect: 'light' }
      })

      console.log('Summary: ', this.summary)
    },
    sortChange(sort) {
      this.$refs.filterTable.clearSort()
      const filtered = this.filter()
      filtered.sort(function(a, b) {
        if (sort.prop === 'sample') {
          const propA = a[sort.prop]
          const propB = b[sort.prop]
          if (sort.order === 'descending') {
            return (propA === propB) ? 0 : propA ? -1 : 1
          } else {
            return (propA === propB) ? 0 : propA ? 1 : -1
          }
        } else {
          const propA = a[sort.prop] ? a[sort.prop].trim().toUpperCase() : ''
          const propB = b[sort.prop] ? b[sort.prop].trim().toUpperCase() : ''
          if (propA < propB) {
            if (sort.order === 'descending') { return -1 } else { return 1 }
          }
          if (propA > propB) {
            if (sort.order === 'descending') { return 1 } else { return -1 }
          }
          return 0
        }
      })

      this.sortedQuestions = filtered
      console.log('Filtered questions:', filtered)
      this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.totalRow = filtered.length
    },
    clearFilter() {
      this.checkSample = false
      this.textSearch = ''
      this.filterSections = this.filterSections.map(i => ({ ...i, checked: false }))
      this.filterTypes = this.filterTypes.map(i => ({ ...i, checked: false }))
      this.filterStatuses = this.filterStatuses.map(i => ({ ...i, checked: false }))
      this.filterDifficulties = this.filterDifficulties.map(i => ({ ...i, checked: false }))
      this.loadTable()
    },
    filterHandler(value, row, column) {
      const property = column['property']
      return row[property] === value
    },
    handleSizeChange(val) {
      this.pageSize = val
      this.loadTable()
    },
    handleCurrentChange(val) {
      this.page = val
      if (this.sortedQuestions) {
        this.questions = this.sortedQuestions.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = this.sortedQuestions.length
      } else {
        this.loadTable()
      }
    },
    handleChangeRowPerPage(val) {
      console.log(typeof this.rowPerPage)
      this.pageSize = +this.rowPerPage
    },
    getTimeFromDateCreateToNow(time) {
      var tz = moment.tz.guess()
      return moment.utc(time).tz(tz).format('DD/MM/YYYY LT')
    },
    handelClick(index, data) {
      console.log(index, data)
    },
    rowClicked(e) {
      // this.$router.push({
      //   name: 'PracticeWriting',
      //   params: {
      //     id: e.id
      //   }
      // })
    },
    search() {
      this.page = 1
      this.loadTable()
    },
    onFilterChange(command) {
      if (command.checked) {
        command.checked = false
      } else {
        command.checked = true
      }
      this.search()
    },
    handleClose(tag) {
      this.selectionTag.splice(this.selectionTag.indexOf(tag), 1)
      const _filteredSection = this.filterSections.filter(s => s.text == tag)
      const _filteredType = this.filterTypes.filter(s => s.text == tag)
      const _filteredStatus = this.filterStatuses.filter(s => s.text == tag)
      const _filteredDifficulty = this.filterDifficulties.filter(s => s.text == tag)
      if (_filteredSection.length > 0) {
        this.filterSections.find(s => s.text == tag).checked = false
      } else if (_filteredType.length > 0) {
        this.filterTypes.find(s => s.text == tag).checked = false
      } else if (_filteredStatus.length > 0) {
        this.filterStatuses.find(s => s.text == tag).checked = false
      } else if (_filteredDifficulty.length > 0) {
        this.filterDifficulties.find(s => s.text == tag).checked = false
      }
      this.loadTable()
    },
    resetFilterSection() {
      this.filterSections = this.filterSections.map(i => ({ ...i, checked: false }))
      this.loadTable()
    },
    resetFilterType() {
      this.filterTypes = this.filterTypes.map(i => ({ ...i, checked: false }))
      this.loadTable()
    },
    resetFilterStatus() {
      this.filterStatuses = this.filterStatuses.map(i => ({ ...i, checked: false }))
      this.loadTable()
    },
    resetFilterDifficulty() {
      this.filterDifficulties = this.filterDifficulties.map(i => ({ ...i, checked: false }))
      this.loadTable()
    },
    filterSample() {
      this.checkSample = !this.checkSample
      this.page = 1
      this.loadTable()
    },
    moveRight() {
      const container = document.getElementById('topic-container')
      const distance = 300
      container.scrollBy({
        left: distance,
        behavior: 'smooth'
      })
      this.showLeftArrow = true
      container.style.width = 'calc(100% - 226px)'
    },
    moveLeft() {
      const container = document.getElementById('topic-container')
      const distance = -300
      container.scrollBy({
        left: distance,
        behavior: 'smooth'
      })
      if (container.scrollLeft <= 100) {
        this.showLeftArrow = false
        container.style.width = 'calc(100% - 200px)'
      }
    },
    showArrow() {
      const container = document.getElementById('topic-container')
      if (container.scrollWidth > container.offsetWidth) { this.showRightArrow = true }
    },
    onTopicClick(topic) {
      if (topic == null) {
        this.allTopicEffect = 'dark'
        this.summary.forEach((e) => { e.effect = 'light' })
        this.clearFilter()
      } else {
        this.summary.forEach((e) => { e.effect = 'light' })
        this.allTopicEffect = 'light'
        topic.effect = 'dark'
        // Clear other filters
        this.checkSample = false
        this.textSearch = ''
        this.filterTypes = this.filterTypes.map(i => ({ ...i, checked: false }))
        this.filterStatuses = this.filterStatuses.map(i => ({ ...i, checked: false }))
        this.filterDifficulties = this.filterDifficulties.map(i => ({ ...i, checked: false }))
        // Set section filters
        this.filterSections.forEach((e) => { e.checked = false })
        this.filterSections.find(s => s.text == topic.section).checked = true

        this.search()
      }
    },
    openAddQuestionDialog() {
      this.$refs.questionDialog?.openDialog(false)
    },
    handleActionCommand(command, e) {
      if (command === 'publish') {
        this.$refs.questionDialog?.publishQuestion(e)
      } else if (command === 'edit') {
        this.$refs.questionDialog?.openEditDialog(e)
      } else if (command === 'preview') {
        this.$refs.questionPreviewDialog?.showDialog(e)
      } else if (command === 'sample') {
        this.$refs.sampleDialog?.openDialog(e.id)
      } else if (command === 'delete') {
        this.selectedQuestion = e
        this.deleteDialogVisible = true
      }
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
      questionService.deleteQuestion(this.selectedQuestion.id).then(rs => {
        if (rs) {
          this.$notify.success({
            title: 'Question deleted.',
            message: 'Question deleted successfully.',
            type: 'success',
            duration: 2000
          })
          this.getQuestionsData()
        }
        this.deleteDialogVisible = false
      })
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
.el-dialog__body span {
  word-break: break-word;
}
</style>

