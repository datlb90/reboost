<template>
  <div class="newcontainer">
    <div>
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
        <el-button size="medium" style="float: right; padding-bottom: 8px; padding-top: 8px; color: #409EFF;" @click="clickPickOne">
          <i class="fas fa-random" style="margin-right: 5px;" />
          {{ messageTranslates('question', 'pickOne') }}
        </el-button>

        <el-button
          v-if="showRightArrow"
          type="text"
          size="medium"
          icon="el-icon-d-arrow-right"
          style="color: grey; padding-bottom: 8px; padding-top: 8px;"
          @click="moveRight()"
        />

      </div>
    </div>
    <div style="height: 40px;">
      <div class="filter-container" style="width: 310px; float: left;">
        <div class="filter-toolbar" style="margin-top: 10px;">
          <el-dropdown
            placement="bottom-start"
            :hide-on-click="false"
            style="float: left; margin-right: 15px;"
            @command="onFilterChange"
          >
            <span class="el-dropdown-link" style="cursor: pointer;">
              Test Sections<i class="el-icon-arrow-down el-icon--right" />
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
              Types<i class="el-icon-arrow-down el-icon--right" />
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
              Status<i class="el-icon-arrow-down el-icon--right" />
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

      <div class="filter-container" style="width: calc(100% - 310px); float: right;">
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
        style="margin-right: 5px; margin-bottom: 5px;"
        @close="handleClose(tag)"
      >
        {{ tag }}
      </el-tag>
    </div>

    <el-table v-if="questions" ref="filterTable" :data="questions" stripe style="width: 100%;" @sort-change="sortChange">
      <el-table-column prop="id" label="#" width="50" />
      <el-table-column
        :label="messageTranslates('question', 'titleTable')"
        prop="title"
        sortable
      >
        <template slot-scope="scope">
          <span class="title-row cursor" style="word-break: break-word" @click="rowClicked(scope.row)">{{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="Test"
        prop="test"
        sortable
        width="80"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.test }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="messageTranslates('question', 'testSectionTable')"
        prop="section"
        sortable
      >
        <template slot-scope="scope">
          <span style="word-break: break-word"> {{ scope.row.section }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="messageTranslates('question', 'typeTable')"
        prop="type"
        sortable
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.type }}</span>
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
      <el-table-column :label="messageTranslates('question', 'statusTable')" prop="status" sortable width="110">
        <template slot-scope="scope">
          <!-- <i v-if="scope.row.status == 'Completed'" class="el-icon-check check" /> -->
          <el-tag
            v-if="scope.row.status == 'Completed'"
            :key="scope.row.status"
            :type="typeSuccess"
            size="mini"
          >
            {{ scope.row.status }}
          </el-tag>
          <el-tag
            v-else-if="scope.row.status == 'Attempted'"
            :key="scope.row.status"
            type="warning"
            size="mini"
          >
            {{ scope.row.status }}
          </el-tag>
          <el-tag
            v-else-if="scope.row.status == 'Saved'"
            :key="scope.row.status"
            type="info"
            size="mini"
          >
            {{ scope.row.status }}
          </el-tag>
          <el-tag
            v-else
            :key="scope.row.status"
            size="mini"
          >
            {{ scope.row.status }}
          </el-tag>
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
  </div>
</template>
<script>
import _ from 'lodash'
export default {
  data() {
    return {
      textSearch: null,
      table: [],
      completed: 5,
      total: 5,
      page: 1,
      currentPage: 1,
      gotoPage: 1,
      totalRow: 10,
      questionsCount: 0,
      rowPerPage: 5,
      pageSize: 50,
      typeSuccess: 'success',
      filterSections: [],
      selectedSections: [],
      filterTypes: [],
      selectedTypes: [],
      filterStatuses: [],
      selectedStatus: [],
      countQuestions: null,
      loadTest: [],
      loadStatus: [],
      questions: [],
      questionCached: [],
      summary: [],
      selectionTag: [],
      checkSample: false,
      showLeftArrow: false,
      showRightArrow: false,
      allTopicEffect: 'dark'
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
      // Get attempted questions
      this.questionCached.forEach(question => {
        const writingContent = localStorage.getItem(this.currentUser.username + '_QuestionId' + question.id)
        if (writingContent != null && writingContent != 'null' && question.status == 'To do') {
          question.status = 'Attempted'
        }
      })
      console.log('Questions: ', this.questionCached)
      this.totalRow = this.questionsCount = this.questionCached.length
      this.filterSections = Object.keys(_.groupBy(this.questionCached, 'section')).map(k => ({ text: k, checked: false }))
      this.filterTypes = Object.keys(_.groupBy(this.questionCached, 'type')).map(k => ({ text: k, checked: false }))
      this.filterStatuses = Object.keys(_.groupBy(this.questionCached, 'status')).map(k => ({ text: k, checked: false }))
      this.loadTable()
      this.loadSummary()
      this.$nextTick(function() {
        this.showArrow()
      })
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
      this.questions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.totalRow = filtered.length
    },
    clearFilter() {
      this.checkSample = false
      this.textSearch = ''
      this.filterSections = this.filterSections.map(i => ({ ...i, checked: false }))
      this.filterTypes = this.filterTypes.map(i => ({ ...i, checked: false }))
      this.filterStatuses = this.filterStatuses.map(i => ({ ...i, checked: false }))
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
      var listNoCompleted = this.questionCached.filter(r => r.status.trim() !== 'Completed')
      var chosenNumber = Math.floor(Math.random() * listNoCompleted.length)
      var id = listNoCompleted[chosenNumber].id
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: id
        }
      })
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
    filter() {
      let result = []
      this.selectionTag = []
      const _filteredSection = this.filterSections.filter(s => s.checked).map(s => s.text)
      const _filteredType = this.filterTypes.filter(s => s.checked).map(s => s.text)
      const _filteredStatus = this.filterStatuses.filter(s => s.checked).map(s => s.text)

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
      if (this.checkSample) {
        result = result.filter(rs => rs.sample == true)
      }
      result = result.sort((a, b) => a.id - b.id)
      return result
    },
    handleClose(tag) {
      this.selectionTag.splice(this.selectionTag.indexOf(tag), 1)
      const _filteredSection = this.filterSections.filter(s => s.text == tag)
      const _filteredType = this.filterTypes.filter(s => s.text == tag)
      const _filteredStatus = this.filterStatuses.filter(s => s.text == tag)
      if (_filteredSection.length > 0) {
        this.filterSections.find(s => s.text == tag).checked = false
      } else if (_filteredType.length > 0) {
        this.filterTypes.find(s => s.text == tag).checked = false
      } else if (_filteredStatus.length > 0) {
        console.log(this.filterStatuses, tag)
        this.filterStatuses.find(s => s.text == tag).checked = false
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
        this.filterSections.forEach((e) => { e.checked = false })
        this.filterSections.find(s => s.text == topic.section).checked = true
        this.search()
      }
    }
  }
}
</script>
<style scoped>
#button-right{
  position: absolute;
  align-items: center;
  align-content: center;
  vertical-align: middle;
  justify-content: center;
  display: flex;
  align-items: center;
  z-index: 1000;
  color: black;
  height: 100vh;
  max-height: 1000px;
  width: 50px;
  background-color: white;
  opacity: 0.5;
  right: 0;
}

.newcontainer {
  padding: 0 200px;
  margin-top: 10px;
}

@media only screen and (max-width: 1200px) {
  .filter-container{
    padding: 5px 0;
  }
  .newcontainer {
    padding: 0 100px;
    margin-top: 10px;
  }
}

@media only screen and (max-width: 990px) {
  .filter-container{
    padding: 5px 0;
  }
  .newcontainer {
    padding: 0 10px;
    margin-top: 10px;
  }
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
  margin-bottom: 5px;
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
  flex-wrap: wrap;
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
  /* font-weight: bold; */
  text-overflow: ellipsis;
    word-break: break-word;
    overflow: hidden;
    white-space: nowrap;
}
.title-row:hover{
  color: #409EFF
}
.el-pagination {
  width: 100%;
  text-align: center;
}
.cursor{
  cursor: pointer;
}
.show-title{
  cursor: pointer;
}
.show-title i{
  margin-bottom: 0;
}
.show-title-text{
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
.show-title:hover .show-title-text{
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
    display: flex;
    z-index: 1;
    align-items: center;
}
/* .el-tag + .el-tag {
  margin-left: 10px;
} */
/* .tag-selection{
  margin-top: 10px;
} */
.el-table::before {
  height: 0 !important;
}
</style>
