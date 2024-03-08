<template>
  <div class="list-container">
    <el-alert
      v-if="showReviewsIntro"
      title="Đây là nơi hiển thị những đánh giá của bạn. Hãy làm theo hướng dẫn và cung cấp thật nhiều đánh giá chất lượng nhé. Bạn sẽ bất ngờ vì lợi ích mà nó mang lại đấy!"
      type="success"
      center
      style="margin-bottom: 10px"
      @close="closeIntro()"
    />

    <div v-if="!loadCompleted">
      <el-card style="padding: 10px; width: 100%; text-align: center; box-shadow: 0 2px 12px 0 #88c4ce;" class="box-card">
        <div v-loading="true" style="width: 100%;  height: calc(100vh - 110px);" element-loading-text="Đang tải đánh giá của bạn" />
      </el-card>
    </div>

    <div v-if="loadCompleted && reviews && reviews.length == 0">
      <el-card style="padding: 10px; width: 100%; text-align: center; box-shadow: 0 2px 12px 0 #88c4ce;" class="box-card">

        <div style="margin-top: 20px; margin-bottom: 20px;">
          <h5>Bạn chưa có đánh giá nào, hãy cung cấp 1 đánh giá ngay!</h5>

          <div style="margin-top: 20px;">
            <el-button type="primary" plain :loading="isLoading" @click="onNewRequestClick()">Cung cấp đánh giá</el-button>
          </div>

        </div>
      </el-card>
    </div>

    <div v-if="loadCompleted && reviews && reviews.length > 0">
      <el-card style="padding: 10px; padding-top: 5px; width: 100%; box-shadow: 0 2px 12px 0 #88c4ce;" class="box-card">
        <div class="top-navigator" style="height: 35px;">
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
              Tất cả đánh giá: {{ reviewCount }}
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
              v-if="completeLoading && !hasPendingReview"
              size="medium"
              style="float: right; padding-bottom: 8px; padding-top: 8px; color: rgb(42 185 190);"
              :loading="isLoading"
              @click="onNewRequestClick"
            >
              <span v-if="currentUser.role == 'rater'">Đánh giá miễn phí</span>
              <span v-else>Cung cấp đánh giá</span>
            </el-button>

            <el-tooltip
              v-if="completeLoading && hasPendingReview"
              class="item"
              effect="dark"
              content="Hãy hoàn thành bài đánh giá của bạn trước khi cung cấp 1 đánh giá mới"
              placement="top"
            >
              <el-button :disabled="true" size="medium" style="float: right; padding-bottom: 8px; padding-top: 8px;" type="info" plain>
                <span v-if="currentUser.role == 'rater'">Đánh giá miễn phí</span>
                <span v-else>Cung cấp đánh giá</span>
              </el-button>
            </el-tooltip>

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
            </div>
          </div>

          <div class="filter-container" style="width: calc(100% - 310px); float: right;">
            <el-button size="mini" style="float: right; margin-top: 5px; margin-left: 5px;" @click="clearFilter">
              {{ messageTranslates('question', 'resetAll') }}
            </el-button>
            <el-input
              v-model="textSearch"
              size="mini"
              placeholder="Search Reviews"
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

        <div v-if="reviews && reviews.length > 0">
          <el-table ref="filterTable" :data="reviews" stripe style="width: 100%; margin-top: 5px;" @sort-change="sortChange" @row-click="rowClicked">
            <el-table-column
              prop="questionName"
              label="Chủ đề"
              sortable
              fixed="left"
              min-width="200"
            >
              <template slot-scope="scope">
                <span class="title-row cursor" style="word-break: break-word" @click="rowClicked(scope.row)">
                  {{ scope.row.id + '. ' + scope.row.questionName }}
                </span>
              </template>
            </el-table-column>

            <el-table-column
              prop="reviewType"
              label="Kiểu đánh giá"
              sortable
              width="200"
            >
              <template slot-scope="scope">
                <span v-if="scope.row.reviewType == 'Peer Review'" style="word-break: break-word">
                  <span v-if="currentUser.role == 'rater'" style="word-break: break-word">
                    Đánh giá miễn phí
                  </span>
                  <span v-else style="word-break: break-word">
                    Đánh giá ngang hàng
                  </span>
                </span>
                <span v-else-if="scope.row.reviewType == 'Paid Review'" style="word-break: break-word">
                  Đánh giá tính phí
                </span>
                <span v-else style="word-break: break-word">
                  Tự đánh giá
                </span>
              </template>
            </el-table-column>

            <el-table-column
              prop="testSection"
              label="Task"
              sortable
              width="200"
            >
              <template slot-scope="scope">
                <span style="word-break: break-word"> {{ scope.row.testSection }}</span>
              </template>
            </el-table-column>

            <el-table-column
              prop="questionType"
              label="Loại đề"
              sortable
              width="190"
            >
              <template slot-scope="scope">
                <span style="word-break: break-word">{{ scope.row.questionType }}</span>
              </template>
            </el-table-column>

            <el-table-column
              prop="status"
              label="Trạng Thái"
              width="160"
            >
              <template slot-scope="scope">
                <el-link
                  v-if="scope.row.status == 'In Progress'"
                  :underline="false"
                  type="warning"
                >Chưa hoàn thành
                </el-link>
                <el-link
                  v-else
                  :underline="false"
                  type="success"
                >Đã hoàn thành
                </el-link>
              </template>
            </el-table-column>

            <el-table-column
              prop="rating"
              label="Rating"
            >
              <template slot-scope="scope">
                <span v-if="scope.row.rating == 'Chưa có'">
                  Chưa có
                </span>
                <span v-else>
                  {{ scope.row.rating }} <i class="fas fa-star" style="color: gold;" />
                </span>
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
      </el-card>

    </div>

  </div>
</template>
<script>
import moment from 'moment-timezone'
import reviewService from '../../services/review.service'
import { PageName, REVIEW_REQUEST_STATUS } from '../../app.constant'
import _ from 'lodash'
export default {
  name: 'Reviews',
  components: {},
  data() {
    return {
      tableData: [],
      reviewsCached: [],
      reviews: [],
      listReviewsPerPage: [],
      pageSize: 10,
      total: 0,
      page: 1,
      currentPage: 1,
      gotoPage: 1,
      REVIEW_REQUEST_STATUS: REVIEW_REQUEST_STATUS,
      hasPendingReview: false,
      totalRow: 10,
      reviewCount: 10,
      filterSections: [],
      selectedSections: [],
      filterTypes: [],
      selectedTypes: [],
      filterStatuses: [],
      selectedStatus: [],
      sortedReviews: null,
      textSearch: null,
      summary: [],
      selectionTag: [],
      showLeftArrow: false,
      showRightArrow: false,
      allTopicEffect: 'dark',
      completeLoading: false,
      showReviewsIntro: true,
      loadCompleted: false,
      isLoading: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  mounted() {
    if (localStorage.getItem('noReviewsIntro')) {
      this.showReviewsIntro = false
    }
    reviewService.getReviewsByUser().then(rs => {
      this.loadCompleted = true
      if (rs) {
        this.reviews = rs
        this.reviewsCached = [...this.reviews]
        console.log('Reviews: ', this.reviewsCached)
        this.totalRow = this.reviewCount = this.reviewsCached.length
        this.filterSections = Object.keys(_.groupBy(this.reviewsCached, 'testSection')).map(k => ({ text: k, checked: false }))
        this.filterTypes = Object.keys(_.groupBy(this.reviewsCached, 'questionType')).map(k => ({ text: k, checked: false }))
        this.filterStatuses = Object.keys(_.groupBy(this.reviewsCached, 'status')).map(k => ({ text: k, checked: false }))
        this.loadTable()
        this.loadSummary()
        this.$nextTick(function() {
          this.showArrow()
        })
      }
    })
    reviewService.getPendingReview().then(r => {
      if (r) { this.hasPendingReview = true }
      this.completeLoading = true
    })
  },
  methods: {
    rowClicked(row, column, event) {
      const url = `/review/${row.id}/${row.docId}/${row.reviewId}`
      this.$router.push(url)
    },
    closeIntro() {
      localStorage.setItem('noReviewsIntro', true)
      this.showReviewsIntro = false
    },
    loadTable() {
      this.sortedReviews = null
      const filtered = this.filter()
      this.reviews = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.totalRow = filtered.length
    },
    filter() {
      let result = []
      this.selectionTag = []
      const _filteredSection = this.filterSections.filter(s => s.checked).map(s => s.text)
      const _filteredType = this.filterTypes.filter(s => s.checked).map(s => s.text)
      const _filteredStatus = this.filterStatuses.filter(s => s.checked).map(s => s.text)

      this.selectionTag = _filteredSection.concat(_filteredType, _filteredStatus)

      for (const q of this.reviewsCached) {
        let pass = true
        if (_filteredSection.length > 0 && !_filteredSection.includes(q.testSection)) {
          pass = false
        }
        if (_filteredType.length > 0 && !_filteredType.includes(q.questionType)) {
          pass = false
        }
        if (_filteredStatus.length > 0 && !_filteredStatus.includes(q.status)) {
          pass = false
        }
        if (pass) {
          result.push(q)
        }
      }
      if (this.textSearch) {
        result = result.filter(q => q.questionName.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
      }
      // result = result.sort((a, b) => a.id - b.id)
      return result
    },
    loadSummary() {
      var counts = this.reviewsCached.reduce((p, c) => {
        var name = c.testSection
        if (!p.hasOwnProperty(c.testSection)) {
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
        const propA = a[sort.prop] ? a[sort.prop].trim().toUpperCase() : ''
        const propB = b[sort.prop] ? b[sort.prop].trim().toUpperCase() : ''
        if (propA < propB) {
          if (sort.order === 'descending') { return -1 } else { return 1 }
        }
        if (propA > propB) {
          if (sort.order === 'descending') { return 1 } else { return -1 }
        }
        return 0
      })
      this.sortedReviews = filtered
      this.reviews = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.totalRow = filtered.length
    },
    clearFilter() {
      this.textSearch = ''
      this.filterSections = this.filterSections.map(i => ({ ...i, checked: false }))
      this.filterTypes = this.filterTypes.map(i => ({ ...i, checked: false }))
      this.filterStatuses = this.filterStatuses.map(i => ({ ...i, checked: false }))
      this.loadTable()
    },
    handleSizeChange(val) {
      this.pageSize = val
      this.loadTable()
    },
    handleCurrentChange(val) {
      this.page = val
      if (this.sortedReviews) {
        this.reviews = this.sortedReviews.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = this.sortedReviews.length
      } else {
        this.loadTable()
      }
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
      if (container && container.scrollWidth > container.offsetWidth) { this.showRightArrow = true }
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
        // Set section filters
        this.filterSections.forEach((e) => { e.checked = false })
        this.filterSections.find(s => s.text == topic.section).checked = true

        this.search()
      }
    },
    getTimeFromDateCreateToNow(time) {
      if (!time) {
        return 'No Submission'
      }
      var tz = moment.tz.guess()
      return moment.utc(time).tz(tz).format('DD/MM/YYYY LT')
    },
    navigateToReviewRequest(object) {
      const url = `/review/${object.submission.questionId}/${object.submission.docId}/${object.reviewId}`
      this.$router.push(url)
    },
    onNewRequestClick() {
      this.isLoading = true
      reviewService.newRequest().then(rs => {
        if (rs) {
          this.$router.push({ name: PageName.REVIEW, params: { questionId: rs.reviewRequest.submission.questionId, docId: rs.reviewRequest.submission.docId, reviewId: rs.reviewId }})
        } else {
          this.$notify.error({
            title: 'Đã xảy ra lỗi',
            message: 'Xin vui lòng liên hệ với chúng tôi qua support@reboost.vn',
            type: 'error',
            duration: 3000
          })
        }
        this.isLoading = false
      }).catch(rs => {
        this.isLoading = false
      })
    },
    onPendingClick() {
      this.$router.push({ name: PageName.REVIEW, params: { questionId: this.pendingReview.reviewRequest.submission.questionId, docId: this.pendingReview.reviewRequest.submission.docId, reviewId: this.pendingReview.reviewId }})
    },
    getStatusVariant(status) {
      let type = 'primary'
      switch (status.trim()) {
        case 'In Progress': type = 'warning'; break
        case 'Completed': type = 'success'; break
        default:
          type = 'primary'
      }
      return type
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

.new-container {
  padding: 0 200px;
  margin-top: 10px;
}

@media only screen and (max-width: 1200px) {
  .filter-container{
    padding: 5px 0;
  }
  .new-container {
    padding: 0 100px;
    margin-top: 10px;
  }
}

@media only screen and (max-width: 990px) {
  .filter-container{
    padding: 5px 0;
  }
  .new-container {
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
.hover-row > .el-table__cell > .cell > .title-row{
  color: rgb(42 185 190);
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
.el-table::before {
  height: 0 !important;
}
</style>
