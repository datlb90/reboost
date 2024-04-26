<template>
  <div v-if="screenWidth > 780" class="list-container">
    <el-alert
      v-if="showSubmissionsIntro"
      type="info"
      :show-icon="true"
      center
      style="margin-bottom: 10px; margin-top: 10px;"
      @close="closeIntro()"
    >
      <span slot="title" style="font-size: 15px;">
        Đây là nơi hiển thị các bài viết bạn đã hoàn thành. Bạn có thể xem lại bài, tham khảo đánh giá, hoặc yêu cầu phản hồi cho bài viết của mình.
      </span>
    </el-alert>

    <div v-if="!loadCompleted">
      <el-card style="padding: 10px; width: 100%; text-align: center;" class="box-card">
        <div v-loading="true" style="width: 100%;  height: calc(100vh - 110px);" element-loading-text="Đang tải bài viết của bạn" />
      </el-card>
    </div>

    <div v-if="loadCompleted && submissionsCached && submissionsCached.length == 0">
      <el-card style="padding: 10px; width: 100%; text-align: center;" class="box-card">

        <div style="margin-top: 20px; margin-bottom: 20px;">
          <h5>Bạn chưa hoàn thành bài viết nào, hãy tìm 1 đề phù hợp và bắt đầu làm ngay!</h5>

          <div style="margin-top: 20px;">
            <el-button type="primary" plain @click="viewQuestions()">Tìm chủ đề phù hợp</el-button>
            <el-button plain style="margin-left: 10px;" @click="clickPickOne()">Viết 1 đề ngẫu nhiên</el-button>
          </div>

        </div>
      </el-card>
    </div>

    <div v-if="loadCompleted && submissionsCached && submissionsCached.length > 0">
      <el-card style="padding: 10px; padding-top: 5px; width: 100%;" class="box-card">
        <div class="top-navigator" style="height: 35px;">
          <el-button
            v-if="showLeftArrow"
            type="text"
            size="medium"
            icon="el-icon-d-arrow-left"
            style="float: left; color: grey; padding-bottom: 8px; padding-top: 8px; margin-right: 10px;"
            @click="moveLeft()"
          />
          <div id="topic-container" style="display: flex; float: left; width: calc(100% - 200px); margin-right: 5px; overflow: hidden;">
            <el-tag
              :effect="allTopicEffect"
              type="info"
              style="font-size: 14px; margin-right: 5px; margin-bottom: 5px; cursor: pointer;"
              @click="onTopicClick(null)"
            >
              Tất cả bài viết: {{ submissionCount }}
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
              Viết 1 đề ngẫu nhiên
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
                :hide-on-click="true"
                style="float: left; margin-right: 15px;"
                @command="onFilterChange"
              >
                <span class="el-dropdown-link" style="cursor: pointer;">
                  <el-link :underline="false" type="info">
                    Loại đề<i class="el-icon-arrow-down el-icon--right" />
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
                    Dạng đề<i class="el-icon-arrow-down el-icon--right" />
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
              <!-- <el-dropdown
                placement="bottom-start"
                :hide-on-click="false"
                style="float: left; margin-right: 15px;"
                @command="onFilterChange"
              >
                <span class="el-dropdown-link" style="cursor: pointer;">
                  <el-link :underline="false" type="info">
                    Độ khó<i class="el-icon-arrow-down el-icon--right" />
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
              </el-dropdown> -->
            </div>
          </div>

          <div class="filter-container" style="width: calc(100% - 310px); float: right;">
            <el-button size="mini" style="float: right; margin-top: 5px; margin-left: 5px;" @click="clearFilter">
              Đặt lại bộ lọc
            </el-button>
            <el-input
              v-model="textSearch"
              size="mini"
              placeholder="Nhập để tìm kiếm"
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

        <el-table v-if="submissions" ref="filterTable" :data="submissions" empty-text="Không có bài viết nào" stripe style="width: 100%; margin-top: 5px;" @sort-change="sortChange" @row-click="rowClicked">
          <el-table-column
            label="Chủ đề"
            prop="questionId"
            sortable
            fixed="left"
            min-width="200"
          >
            <template slot-scope="scope">
              <span class="title-row cursor" style="word-break: break-word">
                {{ scope.row.questionId + '. ' + scope.row.topic }}
              </span>
            </template>
          </el-table-column>

          <el-table-column
            label="Ngày nộp"
            width="165"
          >
            <template slot-scope="scope">
              <span>{{ getTimeFromDateCreateToNow(scope.row.timeSubmitted) }}</span>
            </template>
          </el-table-column>

          <el-table-column
            label="Loại đề"
            prop="task"
            sortable
            width="200"
          >
            <template slot-scope="scope">
              <span style="word-break: break-word"> {{ scope.row.task }}</span>
            </template>
          </el-table-column>
          <el-table-column
            label="Dạng đề"
            prop="questionType"
            sortable
            width="190"
          >
            <template slot-scope="scope">
              <span style="word-break: break-word">{{ scope.row.questionType }}</span>
            </template>
          </el-table-column>

          <el-table-column label="Độ khó" prop="difficulty" sortable width="100">
            <template slot-scope="scope">
              <span style="word-break: break-word">{{ scope.row.difficulty }}</span>
            </template>
          </el-table-column>

          <el-table-column label="Band Score" prop="score" sortable width="150">
            <template slot-scope="scope">
              <!-- <span style="word-break: break-word">{{ scope.row.score }}</span> -->
              <!-- <div v-if="scope.row.score === 'Chưa chấm'" /> -->

              <el-tooltip
                v-if="scope.row.score === 'Chưa chấm'"
                class="item"
                effect="dark"
                content="Nhấp để yêu cầu đánh giá cho bài viết"
                placement="top"
              >
                <el-link style="color: #409fff;" @click.stop="requestReview(scope.row)">
                  Chưa có đánh giá
                </el-link>
              </el-tooltip>

              <el-tooltip
                v-else-if="scope.row.score === 'Chờ chấm'"
                class="item"
                effect="dark"
                content="Yêu cầu đánh giá đã được gửi đi, bạn sẽ được thông báo khi có phản hồi"
                placement="top"
              >
                <el-link :underline="false" type="info">
                  Chờ đánh giá
                </el-link>
              </el-tooltip>

              <el-tooltip
                v-else-if="scope.row.score === 'Đang chấm'"
                class="item"
                effect="dark"
                content="Bài viết của bạn đang được chấm. Vui lòng chờ trong giây lát."
                placement="top"
              >
                <el-link :underline="false" type="warning">
                  Đang chấm
                </el-link>
              </el-tooltip>

              <el-tooltip
                v-else
                class="item"
                effect="dark"
                content="Nhấp để xem đánh giá chi tiết cho bài viết của bạn"
                placement="top"
              >
                <el-link type="success" @click.stop="viewReview(scope.row)">
                  Band {{ scope.row.score }}
                </el-link>
              </el-tooltip>

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
          <checkout
            ref="checkoutDialog"
            :question-id="+selectedQuestionId"
            :submission-id="+selectedSubId"
            @reviewRequested="reviewRequested"
            @closed="checkoutVisible=false"
          />
        </div>
      </el-card>

    </div>

  </div>
  <div v-else class="list-container">
    <el-alert
      v-if="showSubmissionsIntro"
      type="info"
      :show-icon="true"
      center
      style="margin-bottom: 10px; margin-top: 5px;"
      @close="closeIntro()"
    >
      <span slot="title" style="font-size: 15px;">
        Đây là nơi hiển thị các bài viết bạn đã hoàn thành. Bạn có thể xem lại bài, tham khảo đánh giá, hoặc yêu cầu phản hồi cho bài viết của mình.
      </span>
    </el-alert>

    <div v-if="!loadCompleted">
      <el-card style="width: 100%; text-align: center;" class="box-card">
        <div v-loading="true" style="width: 100%;  height: calc(100vh - 110px);" element-loading-text="Đang tải bài viết của bạn" />
      </el-card>
    </div>

    <div v-if="loadCompleted && submissions && submissions.length == 0">
      <el-card style="width: 100%; text-align: center;" class="box-card">

        <div style="margin-top: 20px; margin-bottom: 20px;">
          <h5>Bạn chưa hoàn thành bài viết nào, hãy tìm 1 đề phù hợp và bắt đầu làm ngay!</h5>

          <div style="margin-top: 20px;">
            <el-button type="primary" plain @click="viewQuestions()">Tìm chủ đề phù hợp</el-button>
            <el-button plain style="margin-left: 10px;" @click="clickPickOne()">Viết 1 đề ngẫu nhiên</el-button>
          </div>

        </div>
      </el-card>
    </div>

    <div v-if="loadCompleted && submissions && submissions.length > 0">
      <el-card style="padding-top: 5px; width: 100%;" class="box-card">
        <div class="top-navigator" style="height: 35px;">
          <el-button
            v-if="showLeftArrow"
            type="text"
            size="medium"
            icon="el-icon-d-arrow-left"
            style="float: left; color: grey; padding-bottom: 8px; padding-top: 8px; margin-right: 10px;"
            @click="moveLeft()"
          />
          <div id="topic-container" style="display: flex; float: left; width: calc(100% - 30px); margin-right: 5px; overflow: hidden;">
            <el-tag
              :effect="allTopicEffect"
              type="info"
              style="font-size: 14px; margin-right: 5px; margin-bottom: 5px; cursor: pointer;"
              @click="onTopicClick(null)"
            >
              Tất cả: {{ submissionCount }}
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
              v-if="showRightArrow"
              type="text"
              size="medium"
              icon="el-icon-d-arrow-right"
              style="color: grey; padding-bottom: 8px; padding-top: 8px;"
              @click="moveRight()"
            />
          </div>
        </div>

        <div>
          <el-button style="float: right; margin-top: 5px; color: #409EFF;" size="mini" @click="clickPickOne">
            Ngẫu nhiên
          </el-button>

          <el-input
            v-model="textSearch"
            size="mini"
            placeholder="Nhập để tìm kiếm"
            style="float: left; width: 150px; margin-top: 5px;"
            @input="search()"
          />

          <el-button size="mini" style="float: left; margin-top: 5px; margin-left: 5px; " @click="clearFilter">
            Đặt lại
          </el-button>
        </div>

        <div style="height: 40px;">
          <div class="filter-container" style="width: 100%; float: left;">
            <div class="filter-toolbar" style="margin-top: 10px;">
              <el-dropdown
                placement="bottom-start"
                :hide-on-click="true"
                style="float: left; margin-right: 15px;"
                @command="onFilterChange"
              >
                <span class="el-dropdown-link" style="cursor: pointer;">
                  <el-link :underline="false" type="info">
                    Loại đề<i class="el-icon-arrow-down el-icon--right" />
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
                    Dạng đề<i class="el-icon-arrow-down el-icon--right" />
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

              <!-- <el-dropdown
                placement="bottom-start"
                :hide-on-click="false"
                style="float: left; margin-right: 15px;"
                @command="onFilterChange"
              >
                <span class="el-dropdown-link" style="cursor: pointer;">
                  <el-link :underline="false" type="info">
                    Độ khó<i class="el-icon-arrow-down el-icon--right" />
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
              </el-dropdown> -->
            </div>
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

        <el-table v-if="submissions" ref="filterTable" :data="submissions" empty-text="Không có bài viết nào" stripe style="width: 100%; margin-top: 5px;" @sort-change="sortChange" @row-click="rowClicked">
          <el-table-column
            label="Chủ đề đã viết"
            prop="questionId"
            sortable
            fixed="left"
            min-width="150"
          >
            <template slot-scope="scope">
              <span class="title-row cursor" style="word-break: break-word">
                {{ scope.row.questionId + '. ' + scope.row.topic }}
              </span>
            </template>
          </el-table-column>

          <el-table-column label="Band Score" prop="score" sortable width="150">
            <template slot-scope="scope">
              <!-- <span style="word-break: break-word">{{ scope.row.score }}</span> -->
              <!-- <div v-if="scope.row.score === 'Chưa chấm'" /> -->

              <el-tooltip
                v-if="scope.row.score === 'Chưa chấm'"
                class="item"
                effect="dark"
                content="Nhấp để yêu cầu đánh giá cho bài viết"
                placement="top"
              >
                <el-link style="color: #409fff;" @click.stop="requestReview(scope.row)">
                  Chưa có đánh giá
                </el-link>
              </el-tooltip>

              <el-tooltip
                v-else-if="scope.row.score === 'Chờ chấm'"
                class="item"
                effect="dark"
                content="Yêu cầu đánh giá đã được gửi đi, bạn sẽ được thông báo khi có phản hồi"
                placement="top"
              >
                <el-link :underline="false" type="info">
                  Chờ đánh giá
                </el-link>
              </el-tooltip>

              <el-tooltip
                v-else-if="scope.row.score === 'Đang chấm'"
                class="item"
                effect="dark"
                content="Bài viết của bạn đang được chấm. Vui lòng chờ trong giây lát."
                placement="top"
              >
                <el-link :underline="false" type="warning">
                  Đang chấm
                </el-link>
              </el-tooltip>

              <el-tooltip
                v-else
                class="item"
                effect="dark"
                content="Nhấp để xem đánh giá chi tiết cho bài viết của bạn"
                placement="top"
              >
                <el-link type="success" @click.stop="viewReview(scope.row)">
                  Band {{ scope.row.score }}
                </el-link>
              </el-tooltip>

            </template>
          </el-table-column>
        </el-table>

        <div class="pagination">
          <el-pagination
            background
            layout="total, prev, pager, next"
            :page-size="pageSize"
            :total="totalRow"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
          />
        </div>

        <div>
          <checkout
            ref="checkoutDialog"
            :question-id="+selectedQuestionId"
            :submission-id="+selectedSubId"
            @reviewRequested="reviewRequested"
            @closed="checkoutVisible=false"
          />
        </div>
      </el-card>

    </div>

  </div>
</template>
<script>
import reviewService from '../../services/review.service'
// import { REVIEW_REQUEST_STATUS } from '../../app.constant'
import questionService from '../../services/question.service'
import CheckOut from '../../components/controls/CheckOut.vue'
// import moment from 'moment'
import moment from 'moment-timezone'
import _ from 'lodash'
export default {
  name: 'Submissions',
  components: {
    'checkout': CheckOut
  },
  data() {
    return {
      tableData: [],
      submissionsCached: [],
      submissions: [],
      listSubmissionsPerPage: [],
      pageSize: 10,
      total: 0,
      page: 1,
      currentPage: 1,
      gotoPage: 1,
      checkoutVisible: false,
      unRatedList: [],
      selectedSubId: null,
      selectedQuestionId: null,
      totalRow: 10,
      submissionCount: 10,
      filterSections: [],
      selectedSections: [],
      filterTypes: [],
      selectedTypes: [],
      filterStatuses: [],
      selectedStatus: [],
      sortedSubmission: null,
      textSearch: null,
      summary: [],
      selectionTag: [],
      showLeftArrow: false,
      showRightArrow: false,
      allTopicEffect: 'dark',
      screenWidth: window.innerWidth,
      showSubmissionsIntro: true,
      loadCompleted: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  mounted() {
    document.title = 'Bài viết của tôi'
    if (localStorage.getItem('noSubmissionsIntro')) {
      this.showSubmissionsIntro = false
    }
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    questionService.getSubmissionsByUserId(this.currentUser.id).then(rs => {
      console.log(rs)
      this.loadCompleted = true
      if (rs) {
        this.submissions = rs
        this.submissionsCached = [...this.submissions]
        console.log('Submissions: ', this.submissionsCached)
        this.totalRow = this.submissionCount = this.submissionsCached.length
        this.filterSections = Object.keys(_.groupBy(this.submissionsCached, 'task')).map(k => ({ text: k, checked: false }))
        this.filterTypes = Object.keys(_.groupBy(this.submissionsCached, 'questionType')).map(k => ({ text: k, checked: false }))
        this.filterStatuses = Object.keys(_.groupBy(this.submissionsCached, 'status')).map(k => ({ text: k, checked: false }))
        this.loadTable()
        this.loadSummary()
        this.$nextTick(function() {
          this.showArrow()
        })
      }
    })
  },
  methods: {
    viewReview(row) {
      this.$router.push(`review/${row.questionId}/${row.docId}/${row.reviewId}`)
    },
    requestReview(row) {
      this.selectedQuestionId = row.questionId
      this.selectedSubId = row.id
      this.$nextTick(() => {
        this.$refs.checkoutDialog?.openDialog()
      })
    },
    rowClicked(row, column, event) {
      this.$router.push(`practice/${row.questionId}/${row.id}`)
    },
    clickPickOne() {
      const questions = this.$store.getters['question/getAll']
      console.log(questions)
      var listNoCompleted = questions.filter(r => r.status.trim() === 'Chưa làm')
      var chosenNumber = Math.floor(Math.random() * listNoCompleted.length)
      var id = listNoCompleted[chosenNumber].id
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: id
        }
      })
    },
    viewQuestions() {
      this.$router.push('/questions')
    },
    closeIntro() {
      localStorage.setItem('noSubmissionsIntro', true)
      this.showSubmissionsIntro = false
    },
    getUrlParameter(sParam) {
      const sPageURL = decodeURIComponent(window.location.search.substring(1))
      const sURLVariables = sPageURL.split('&')
      let sParameterName
      let i
      for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=')

        if (sParameterName[0] === sParam) {
          return sParameterName[1] === undefined ? true : sParameterName[1]
        }
      }
    },
    loadTable() {
      this.sortedSubmission = null
      const filtered = this.filter()
      this.submissions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.totalRow = this.submissionsCached.length
    },
    filter() {
      let result = []
      this.selectionTag = []
      const _filteredSection = this.filterSections.filter(s => s.checked).map(s => s.text)
      const _filteredType = this.filterTypes.filter(s => s.checked).map(s => s.text)
      const _filteredStatus = this.filterStatuses.filter(s => s.checked).map(s => s.text)

      this.selectionTag = _filteredSection.concat(_filteredType, _filteredStatus)

      for (const q of this.submissionsCached) {
        let pass = true
        if (_filteredSection.length > 0 && !_filteredSection.includes(q.task)) {
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
        result = result.filter(q => q.topic.toLowerCase().includes(this.textSearch.toLowerCase()))
      }
      // result = result.sort((a, b) => a.id - b.id)
      return result
    },
    loadSummary() {
      var counts = this.submissionsCached.reduce((p, c) => {
        var name = c.task
        if (!p.hasOwnProperty(c.task)) {
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
        // const propA = a[sort.prop] ? a[sort.prop].trim().toUpperCase() : ''
        // const propB = b[sort.prop] ? b[sort.prop].trim().toUpperCase() : ''
        const propA = a[sort.prop]
        const propB = b[sort.prop]
        if (propA < propB) {
          if (sort.order === 'descending') { return -1 } else { return 1 }
        }
        if (propA > propB) {
          if (sort.order === 'descending') { return 1 } else { return -1 }
        }
        return 0
      })
      this.sortedSubmission = filtered
      this.submissions = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
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
      if (this.sortedSubmission) {
        this.submissions = this.sortedSubmission.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = this.sortedSubmission.length
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
        this.textSearch = ''
        this.filterTypes = this.filterTypes.map(i => ({ ...i, checked: false }))
        this.filterStatuses = this.filterStatuses.map(i => ({ ...i, checked: false }))
        // Set section filters
        this.filterSections.forEach((e) => { e.checked = false })
        this.filterSections.find(s => s.text == topic.section).checked = true

        this.search()
      }
    },
    getStatusVariant(status) {
      let type = 'primary'
      switch (status.trim()) {
        case 'Pending': type = 'danger'; break
        case 'Completed': type = 'success'; break
        case 'Submitted': type = 'primary'; break
        case 'Review Requested': type = 'warning'; break
        case 'Reviewed': type = 'danger'; break

        default:
          type = 'primary'
      }
      return type
    },
    getStatusName(status) {
      if (status.trim() === 'Reviewed') return 'Unrated'
      return status
    },
    getTimeFromDateCreateToNow(time) {
      var tz = moment.tz.guess()
      return moment.utc(time).tz(tz).format('DD/MM/YYYY LT')
    },
    getTimeTaken(time) {
      var minutes = Math.floor(time / 60)
      var seconds = time - minutes * 60
      return minutes + ' min ' + seconds + ' sec '
    },
    actionClick(action, e) {
      this.selectedSubId = e.id
      if (action.trim() === 'Request Review') {
        this.selectedQuestionId = e.questionId
        this.$nextTick(() => {
          this.$refs.checkoutDialog?.openDialog()
        })
      } else if (action == 'View Submission') {
        this.$router.push(`practice/${e.questionId}/${e.id}`)
      } else if (action == 'View Review') {
        reviewService.getOrCreateReviewBySubmissionId(e.id).then(rs => {
          this.$router.push(`review/${e.questionId}/${rs.docId}/${rs.reviewId}`)
        })
      }
    },
    reviewRequested() {
      this.submissionsCached.forEach(r => {
        if (r.id === this.selectedSubId) {
          r.score = 'Chờ chấm'
        }
      })
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
  color: #409EFF
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
