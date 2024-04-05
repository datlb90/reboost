<template>
  <div v-if="screenWidth > 780" class="list-container">
    <el-alert
      v-if="showQuestionsIntro"
      type="info"
      :show-icon="true"
      center
      style="margin-bottom: 10px; margin-top: 10px;"
      @close="closeIntro()"
    >
      <span slot="title" style="font-size: 15px;">
        Đây là nơi hiển thị đa dạng chủ đề viết đã được phân loại. Hãy lựa chọn đề phù hợp với bạn sử dụng công cụ tìm kiếm có sẵn và làm ít nhất một bài mỗi ngày nhé.
      </span>
    </el-alert>

    <el-card v-if="!loadCompleted" style="padding: 10px; width: 100%; text-align: center;" class="box-card">
      <div v-loading="true" style="width: 100%; height: calc(100vh - 110px);" element-loading-text="Đang tải chủ đề viết" />
    </el-card>
    <el-card v-else style="padding: 10px; padding-top: 5px; width: 100%;" class="box-card">
      <div>
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
              Tất cả chủ đề: {{ questionsCount }}
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

            <!-- <el-button
              v-if="showRightArrow"
              type="text"
              size="medium"
              icon="el-icon-d-arrow-right"
              style="color: grey; padding-bottom: 8px; padding-top: 8px;"
              @click="moveRight()"
            /> -->

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
                    Task<i class="el-icon-arrow-down el-icon--right" />
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
                    Loại đề<i class="el-icon-arrow-down el-icon--right" />
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
                    Độ Khó<i class="el-icon-arrow-down el-icon--right" />
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
                    Trạng thái<i class="el-icon-arrow-down el-icon--right" />
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
              Đặt lại bộ lọc
            </el-button>
            <el-input
              v-model="textSearch"
              size="mini"
              placeholder="Nhập để tìm kiếm chủ đề"
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

        <el-table v-if="questions" ref="filterTable" :data="questions" stripe style="width: 100%;" empty-text="Không có chủ đề nào" @sort-change="sortChange" @row-click="rowClicked">
          <el-table-column
            prop="status"
            sortable
            fixed="left"
            width="45"
          >
            <template slot-scope="scope">
              <div style="width: 100%; text-align: center;">  <el-tooltip class="item" :content="scope.row.status" placement="bottom" style="text-align: center;">
                <i v-if="scope.row.status == 'Đã làm'" class="el-icon-success" style="font-size: 18px;" />
                <i v-else-if="scope.row.status == 'Đang làm'" class="el-icon-folder-checked" style="font-size: 18px;" />
                <i v-else-if="scope.row.status == 'Đã thử'" class="el-icon-edit" style="font-size: 18px;" />
                <i v-else class="el-icon-remove-outline" style="font-size: 18px;" />
              </el-tooltip>
              </div>
            </template>
          </el-table-column>

          <el-table-column
            label="Chủ đề"
            prop="title"
            sortable
            fixed="left"
            min-width="200"
          >
            <template slot-scope="scope">
              <span class="title-row cursor" style="word-break: break-word" @click="titleClicked(scope.row)">{{ scope.row.id }}. {{ scope.row.title }}</span>
            </template>
          </el-table-column>
          <el-table-column
            label="Task"
            prop="section"
            width="200"
            sortable
          >
            <template slot-scope="scope">
              <span style="word-break: break-word"> {{ scope.row.section }}</span>
            </template>
          </el-table-column>
          <el-table-column
            label="Loại đề"
            prop="type"
            width="190"
            sortable
          >
            <template slot-scope="scope">
              <span style="word-break: break-word">{{ scope.row.type }}</span>
            </template>
          </el-table-column>

          <el-table-column label="Độ khó" prop="difficulty" sortable width="100">
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
            width="50"
            prop="sample"
            sortable
          >
            <template slot-scope="scope">
              <div v-if="scope.row.sample" class="show-title">
                <el-tooltip class="item" effect="dark" content="Đã có tài liệu hỗ trợ và bài viết mẫu" placement="bottom">
                  <i class="el-icon-document" />
                </el-tooltip>
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

      </div>

      <el-card
        style="border: 1px solid rgb(190, 190, 190); margin-top: 5px;"
        shadow="hover"
      >
        <div slot="header" class="clearfix">
          <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; width: calc(100% - 100px); text-overflow: ellipsis;  word-break: break-word; overflow: hidden; white-space: nowrap;">
            <span>Đóng Góp Ý Kiến</span>
          </div>
        </div>
        <div>
          <div>
            <div style="font-size: 15px;">Mời bạn chia sẻ cảm nghĩ của mình về các chủ đề viết và tài nguyên hỗ trợ được cung cấp. Chúng tôi trân trọng mọi ý kiến đóng góp và cam kết sử dụng phản hồi của bạn để cải thiện chất lượng dịch vụ của Reboost. Xin chân thành cảm ơn!</div>
          </div>

          <div style="border-top: #bcbcbc solid 1px; margin-top: 20px; padding-top: 10px;">
            <div class="fb-comments" data-href="https://reboost.vn/questions" data-width="100%" data-numposts="10" />
          </div>

        </div>
      </el-card>
    </el-card>

  </div>
  <div v-else class="list-container">
    <el-alert
      v-if="showQuestionsIntro"
      type="info"
      :show-icon="true"
      center
      style="margin-bottom: 10px; margin-top: 10px;"
      @close="closeIntro()"
    >
      <span slot="title" style="font-size: 15px;">
        Đây là nơi hiển thị đa dạng chủ đề viết đã được phân loại. Hãy lựa chọn đề phù hợp với bạn sử dụng công cụ tìm kiếm có sẵn và làm ít nhất một bài mỗi ngày nhé.
      </span>
    </el-alert>

    <el-card v-if="!loadCompleted" style="width: 100%; text-align: center;" class="box-card">
      <div v-loading="true" style="width: 100%; height: calc(100vh - 110px);" element-loading-text="Đang tải chủ đề viết" />
    </el-card>

    <el-card v-else style="padding-top: 5px; width: 100%;" class="box-card">
      <div>
        <div class="top-navigator" style="height: 30px; margin-left: 5px; margin-right: 5px;">
          <el-button
            v-if="showLeftArrow"
            type="text"
            size="medium"
            icon="el-icon-d-arrow-left"
            style="float: left; color: grey; padding-bottom: 8px; padding-top: 8px; margin-right: 10px;"
            @click="moveLeft()"
          />
          <div id="topic-container" style="display: flex; float: left; width: calc(100% - 20px); overflow: hidden;">
            <el-tag
              type="info"
              :effect="allTopicEffect"
              style="font-size: 14px; margin-right: 5px; margin-bottom: 5px; cursor: pointer;"
              @click="onTopicClick(null)"
            >
              Tất cả chủ đề: {{ questionsCount }}
            </el-tag>
            <el-tag
              v-for="item in summary"
              :key="item.section"
              type="info"
              :effect="item.effect"
              style="font-size: 14px; margin-right: 5px; margin-bottom: 5px; cursor: pointer;"
              @click="onTopicClick(item)"
            >
              {{ item.section == 'Academic Writing Task 1' ? 'Task 1' : 'Task 2' }}: {{ item.count }}
            </el-tag>
          </div>
          <div>
            <!-- <el-button
              v-if="showRightArrow"
              type="text"
              size="medium"
              icon="el-icon-d-arrow-right"
              style="color: grey; padding-bottom: 8px; padding-top: 8px;"
              @click="moveRight()"
            /> -->

          </div>
        </div>

        <div style="margin-left: 5px; margin-right: 5px;">
          <el-button size="mini" style="float: right; margin-top: 5px; margin-left: 5px; color: #409EFF;" @click="clickPickOne">
            <i class="fas fa-random" style="margin-right: 5px;" />
            Ngẫu nhiên
          </el-button>

          <el-input
            v-model="textSearch"
            size="mini"
            :placeholder="messageTranslates('question', 'placeholderSearch')"
            style="float: left; width: 150px; margin-top: 5px;"
            @input="search()"
          />
          <el-button size="mini" style="float: left; margin-top: 5px; margin-left: 5px;" @click="clearFilter">
            Đặt lại
          </el-button>
        </div>

        <div style="height: 40px; margin-left: 5px; margin-right: 5px;">
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
                    Task<i class="el-icon-arrow-down el-icon--right" />
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
                    Loại đề<i class="el-icon-arrow-down el-icon--right" />
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
              </el-dropdown>

              <el-dropdown
                placement="bottom-start"
                :hide-on-click="false"
                style="float: left; margin-right: 15px;"
                @command="onFilterChange"
              >
                <span class="el-dropdown-link" style="cursor: pointer;">
                  <el-link :underline="false" type="info">
                    Trạng thái<i class="el-icon-arrow-down el-icon--right" />
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

        <el-table v-if="questions" ref="filterTable" :data="questions" stripe style="width: 100%;" empty-text="Không có chủ đề nào" @sort-change="sortChange" @row-click="rowClicked">
          <el-table-column
            prop="status"
            sortable
            fixed="left"
            width="30"
          >
            <template slot-scope="scope">
              <div style="width: 100%; text-align: left;">
                <el-tooltip class="item" :content="scope.row.status" placement="bottom" style="text-align: left;">
                  <i v-if="scope.row.status == 'Đã làm'" class="el-icon-success" style="font-size: 15px;" />
                  <i v-else-if="scope.row.status == 'Đang làm'" class="el-icon-folder-checked" style="font-size: 15px;" />
                  <i v-else-if="scope.row.status == 'Đã thử'" class="el-icon-edit" style="font-size: 15px;" />
                  <i v-else class="el-icon-remove-outline" style="font-size: 15px;" />
                </el-tooltip>
              </div>
            </template>
          </el-table-column>

          <el-table-column
            label="Chủ đề"
            prop="title"
            sortable
            fixed="left"
            min-width="120"
          >
            <template slot-scope="scope">
              <span class="title-row cursor" style="word-break: break-word;" @click="titleClicked(scope.row)">{{ scope.row.id }}. {{ scope.row.title }}</span>
            </template>
          </el-table-column>

          <el-table-column
            label="Task"
            prop="section"
            :width="questions.find(q => q.test == 'TOEFL') ? '100' : '80'"
            sortable
          >
            <template slot-scope="scope">
              <span v-if="scope.row.section == 'Independent Writing'" style="word-break: break-word;">
                Independent
              </span>
              <span v-else-if="scope.row.section == 'Integrated Writing'" style="word-break: break-word;">
                Integrated
              </span>
              <span v-else-if="scope.row.section == 'Academic Writing Task 1'" style="word-break: break-word">
                Task 1
              </span>
              <span v-else-if="scope.row.section == 'Academic Writing Task 2'" style="word-break: break-word">
                Task 2
              </span>
            </template>
          </el-table-column>
          <el-table-column label="Độ khó" prop="difficulty" sortable width="90">
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
            width="30"
            prop="sample"
            sortable
          >
            <template slot-scope="scope">
              <div v-if="scope.row.sample" class="show-title">
                <el-tooltip class="item" effect="dark" content="Đã có tài liệu hỗ trợ và bài viết mẫu" placement="bottom">
                  <i class="el-icon-document" />
                </el-tooltip>
              </div>
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
      </div>

      <el-card
        style="border: 1px solid rgb(190, 190, 190); margin-top: 5px;"
        shadow="hover"
      >
        <div slot="header" class="clearfix">
          <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; width: calc(100% - 100px); text-overflow: ellipsis;  word-break: break-word; overflow: hidden; white-space: nowrap;">
            <span>Đóng Góp Ý Kiến</span>
          </div>
        </div>
        <div>
          <div>
            <div style="font-size: 15px;">Mời bạn chia sẻ cảm nghĩ của mình về các chủ đề viết và tài nguyên hỗ trợ được cung cấp. Chúng tôi trân trọng mọi ý kiến đóng góp và cam kết sử dụng phản hồi của bạn để cải thiện chất lượng dịch vụ của Reboost. Xin chân thành cảm ơn!</div>
          </div>

          <div style="border-top: #bcbcbc solid 1px; margin-top: 20px; padding-top: 10px;">
            <div class="fb-comments" data-href="https://reboost.vn/questions" data-width="100%" data-numposts="10" />
          </div>

        </div>
      </el-card>

    </el-card>

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
      filterDifficulties: [],
      selectedDifficulty: [],
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
      allTopicEffect: 'dark',
      sortedQuestions: null,
      screenWidth: window.innerWidth,
      showQuestionsIntro: true,
      loadCompleted: false
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
  watch: {
    screenWidth(newWidth) {
      this.screenWidth = newWidth
    }
  },
  mounted() {
    document.title = 'Chủ đề viết'
    if (localStorage.getItem('noQuestionsIntro')) {
      this.showQuestionsIntro = false
    }
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    this.$store.dispatch('question/loadAllQuestionByUser', this.currentUser.id).then(questions => {
      this.questionCached = this.$store.getters['question/getAll']
      // Get attempted questions
      this.questionCached.forEach(question => {
        const writingContent = localStorage.getItem(this.currentUser.username + '_QuestionId' + question.id)
        if (writingContent != null && writingContent != 'null' && question.status == 'Chưa làm') {
          question.status = 'Đã thử'
        }
      })
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
      this.loadCompleted = true
      this.loadTable()
      this.loadSummary()
      this.$nextTick(function() {
        window.FB.XFBML.parse()
        this.showArrow()
      })
    })
  },
  methods: {
    rowClicked(row, column, event) {
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: row.id
        }
      })
    },
    closeIntro() {
      localStorage.setItem('noQuestionsIntro', true)
      this.showQuestionsIntro = false
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
        result = result.filter(q => q.title.toLowerCase().includes(this.textSearch.toLowerCase()))
      }
      result = result.sort((a, b) => a.id - b.id)
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
    titleClicked(row) {
      this.$router.push({
        name: 'PracticeWriting',
        params: {
          id: row.id
        }
      })
    },
    clickPickOne() {
      var listNoCompleted = this.questionCached.filter(r => r.status.trim() === 'Chưa làm')
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
      if (this.screenWidth > 780) { container.style.width = 'calc(100% - 226px)' } else { container.style.width = 'calc(100% - 50px)' }
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
        if (this.screenWidth > 780) { container.style.width = 'calc(100% - 200px)' } else { container.style.width = 'calc(100% - 20px)' }
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
        this.filterDifficulties = this.filterDifficulties.map(i => ({ ...i, checked: false }))
        // Set section filters
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
  text-overflow: ellipsis;
  word-break: break-word;
  overflow: hidden;
  white-space: nowrap;
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
