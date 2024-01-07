<template>
  <div class="container">
    <div class="title">
      <h2>{{ messageTranslates('manageRaters', 'raterApplication') }}</h2>
    </div>
    <el-row :gutter="40">
      <el-col :md="6">
        <el-input v-model="textSearch" size="mini" :placeholder="messageTranslates('manageRaters', 'typeToSearch')" @input="search()" />
      </el-col>
      <el-col :md="12" class="filter-container">
        <div class="filter-toolbar">
          <dropdown-menu v-model="filterStatus" :tittle="messageTranslates('manageRaters', 'status')" @confirm="search()" />
        </div>
        <div class="tag-selection">
          <el-tag
            v-for="tag in selectionTag"
            :key="tag"
            class="mr-1"
            size="mini"
            type="success"
            effect="dark"
            closable
            :disable-transitions="false"
            @close="handleClose(tag)"
          >
            {{ constantTranslate('RATER_STATUS', tag) }}
          </el-tag>
        </div>
      </el-col>
      <el-col :md="6">
        <el-button size="mini" @click="clearFilter">{{ messageTranslates('manageRaters', 'resetFilters') }}</el-button>
      </el-col>
    </el-row>
    <el-table ref="filterTable" size="mini" :data="raters" style="width: 100%" @sort-change="changeTableSort">
      <el-table-column prop="fullName" :label="messageTranslates('manageRaters', 'applicationName')" width="160" />
      <el-table-column prop="appliedDate" :label="messageTranslates('manageRaters', 'applicationDate')" sortable="custom" width="180" />
      <el-table-column prop="occupation" :label="messageTranslates('manageRaters', 'occupation')" width="140" />
      <el-table-column prop="firstLanguage" :label="messageTranslates('manageRaters', 'firstLanguage')" width="130" />
      <!-- <el-table-column prop="applyTo" label="Applied For" width="120">
      </el-table-column> -->
      <el-table-column
        prop="status"
        :label="messageTranslates('manageRaters', 'status')"
        filter-placement="bottom-end"
      >
        <template slot-scope="scope">
          <el-tag
            size="mini"
            :type="
              scope.row.status === RATER_STATUS.APPLIED
                ? 'primary'
                : scope.row.status === RATER_STATUS.APPROVED || scope.row.status === RATER_STATUS.TRAINING || scope.row.status === RATER_STATUS.TRAINING_COMPLETED
                  ? 'success'
                  :status === RATER_STATUS.REJECTED
                    ? 'danger'
                    : scope.row.status === RATER_STATUS.DOCUMENT_REQUESTED || scope.row.status === RATER_STATUS.DOCUMENT_SUBMITTED || scope.row.status === RATER_STATUS.REVISION_REQUESTED
                      ? 'warning'
                      : 'warning'"
            disable-transitions
          >{{ constantTranslate('RATER_STATUS', scope.row.status) }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column :label="messageTranslates('manageRaters', 'operations')">
        <template slot-scope="scope">
          <el-button size="mini" @click="handleView(scope.$index, scope.row)">{{ messageTranslates('manageRaters', 'view') }}</el-button>
          <!-- <el-button v-if="scope.row.status == RATER_STATUS.TRAINING_COMPLETED" size="mini" type="warning">Review</el-button> -->
        </template>
      </el-table-column>
    </el-table>
    <div class="pagination">
      <el-pagination size="mini" background layout="prev, pager, next" :page-size="pageSize" :total="totalRow" @size-change="handleSizeChange" @current-change="handleCurrentChange" />
    </div>
  </div>
</template>

<script>
import { RATER_STATUS } from '../../app.constant'
import DropdownMenu from '../../components/controls/DropdownMenu'
import moment from 'moment-timezone'

export default {
  name: 'ManageRaters',
  components: {
    'dropdown-menu': DropdownMenu
  },
  data() {
    return {
      textSearch: null,
      filtered: [],
      page: 1,
      pageSize: 10,
      filterDate: [],
      table: [],
      RATER_STATUS: RATER_STATUS,
      selectionTag: [],
      raters: [],
      raterCached: [],
      totalRow: 10,
      ratersCount: 0,
      filterStatus: [
        { text: RATER_STATUS.APPLIED, value: RATER_STATUS.APPLIED, checked: false },
        { text: RATER_STATUS.DOCUMENT_REQUESTED, value: RATER_STATUS.DOCUMENT_REQUESTED, checked: false },
        { text: RATER_STATUS.DOCUMENT_SUBMITTED, value: RATER_STATUS.DOCUMENT_SUBMITTED, checked: false },
        { text: RATER_STATUS.TRAINING, value: RATER_STATUS.TRAINING, checked: false },
        { text: RATER_STATUS.TRAINING_COMPLETED, value: RATER_STATUS.TRAINING_COMPLETED, checked: false },
        { text: RATER_STATUS.REVISION_REQUESTED, value: RATER_STATUS.REVISION_REQUESTED, checked: false },
        { text: RATER_STATUS.APPROVED, value: RATER_STATUS.APPROVED, checked: false },
        { text: RATER_STATUS.REJECTED, value: RATER_STATUS.REJECTED, checked: false }
      ]
    }
  },
  computed: {
    getAllRater() {
      var tz = moment.tz.guess()
      var data = this.$store.getters['rater/getAll'].map(i => ({ ...i, appliedDate: moment.utc(i.appliedDate).tz(tz).format('DD/MM/YYYY LT'), sortTime: moment.utc(i.appliedDate).tz(tz)._d }))
      return data
    },
    getAllReviews() {
      return this.$store.getters['review/getReviews']
    }
  },
  mounted() {
    this.$store.dispatch('rater/loadRaters').then(() => {
      this.raterCached = this.getAllRater
      this.totalRow = this.ratersCount = this.raterCached.length
      this.loadTable()
    })
    this.$store.dispatch('review/loadReviews')
  },
  methods: {
    loadTable() {
      let filtered = this.filter()
      if (this.textSearch) {
        filtered = filtered.filter(q => q.fullName.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
        this.raters = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      } else {
        this.raters = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      }
    },
    resetDateFilter() {
      this.$refs.filterTable.clearFilter('applicationDate')
    },
    clearFilter() {
      this.$refs.filterTable.clearFilter()
      this.filterStatus = this.filterStatus.map(i => ({ ...i, checked: false }))
      this.textSearch = ''
      this.loadTable()
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
    searching() {
      if (!this.textSearch) {
        this.total = this.getAllRater.length
        return this.getAllRater
      }
      var table = this.getAllRater.filter(data => data.fullName.toLowerCase().includes(this.textSearch.toLowerCase()))
      this.total = table.length
      return table
    },
    handleView(index, row) {
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
      var pushUrl = t.status.includes('IELTSTraining') ? '/review/9/220/' + t.id : '/review/12/219/' + t.id
      this.$router.push(pushUrl)
    },
    search() {
      this.page = 1
      this.loadTable()
    },
    filter() {
      let result = []
      this.selectionTag = []
      const _filteredStatus = this.filterStatus.filter(s => s.checked).map(s => s.text)

      this.selectionTag = _filteredStatus

      for (const q of this.raterCached) {
        let pass = true
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
      // result = result.sort((a, b) => a.id - b.id)
      return result
    },
    handleClose(tag) {
      this.selectionTag.splice(this.selectionTag.indexOf(tag), 1)
      const _filteredStatus = this.filterStatus.filter(s => s.text == tag)
      if (_filteredStatus.length > 0) {
        this.filterStatus.find(s => s.text == tag).checked = false
      }
      this.loadTable()
    },
    changeTableSort(column) {
      var sortingType = column.order

      if (sortingType == 'descending') {
        this.raterCached = this.raterCached.sort((a, b) => b.sortTime - a.sortTime)
        this.loadTable()
      } else {
        this.raterCached = this.raterCached.sort((a, b) => a.sortTime - b.sortTime)
        this.loadTable()
      }
    }
  }
}
</script>

<style scoped>
@media only screen and (max-width: 990px) {
  .filter-container{
    padding: 5px 0;
  }
}
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
.tag-selection{
  margin-top: 10px;
}
.filter-toolbar{
    display: flex;
    z-index: 1;
    align-items: center;
}
</style>
