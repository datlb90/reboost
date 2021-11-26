<template>
  <div class="container">
    <div class="title">
      <h2>{{ messageTranslates('adminArticles', 'title') }}</h2>
    </div>
    <el-row :gutter="12">
      <el-col :sm="6" class="filter-container">
        <div>
          <el-input v-model="textSearch" size="mini" :placeholder="messageTranslates('adminArticles', 'textSearch')" @input="search()" />
        </div>
      </el-col>
      <el-col :md="12" class="filter-container">
        <div class="filter-toolbar">
          <dropdown-menu v-model="filterCategory" style="margin-right: 20px" :tittle="messageTranslates('adminArticles', 'category')" @confirm="search()" @reset="resetFilterSection()" />
          <!-- <dropdown-menu v-model="filterType" style="margin-right: 20px" :tittle="messageTranslates('adminArticles', 'type')" @confirm="search()" @reset="resetFilterType()" />
          <dropdown-menu v-model="filterStatus" :tittle="messageTranslates('adminArticles', 'status')" @confirm="search()" @reset="resetFilterStatus()" /> -->
        </div>
        <div class="tag-selection">
          <el-tag
            v-for="(tag, index) in selectionTag"
            :key="index"
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
          <el-button size="mini" @click="clearFilter">{{ messageTranslates('adminArticles', 'resetFilter') }}</el-button>
        </div>
      </el-col>
    </el-row>
    <el-row style="margin:10px 0px;">
      <el-col class="filter-container">
        <div style="text-align: right;">
          <el-button size="mini" @click="openAddArticleDialog">{{ messageTranslates('adminArticles', 'addArticle') }}</el-button>
        </div>
      </el-col>
    </el-row>
    <el-table ref="filterTable" size="mini" :data="articlesList" stripe style="width: 100%;">
      <el-table-column prop="id" sortable label="#" width="60" />
      <el-table-column prop="title" sortable :label="messageTranslates('adminArticles', 'articleTitle')">
        <template slot-scope="scope">
          <span class="title-row cursor" style="word-break: break-word">{{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="messageTranslates('adminArticles', 'articleCategory')">
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.category }} </span>
        </template>
      </el-table-column>
      <el-table-column :label="messageTranslates('adminArticles', 'articleLabel')">
        <template slot-scope="scope">
          <el-tag v-for="(item, index) in getMatchLabel(scope.row)" :key="index">{{ item.name }}</el-tag>
        </template>
      </el-table-column>
      <!-- <el-table-column :label="messageTranslates('adminArticles', 'articleAuthor')">
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ scope.row.author }} </span>
        </template>
      </el-table-column> -->
      <el-table-column
        :label="messageTranslates('adminArticles', 'articlePostedDate')"
        sortable
        prop="postedDate"
      >
        <template slot-scope="scope">
          <span style="word-break: break-word">{{ getTimeFromDateCreateToNow(scope.row.postedDate) }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="messageTranslates('adminArticles', 'articleOriginalUrl')">
        <template slot-scope="scope">
          <a :href="scope.row.originalUrl" style="word-break: break-word">{{ scope.row.originalUrl }} </a>
        </template>
      </el-table-column>
      <!-- <el-table-column :label="messageTranslates('adminArticles', 'articleQuestion')">
        <template slot-scope="scope">
          <span style="word-break: break-word" v-html="scope.row.question" />
        </template>
      </el-table-column> -->
      <el-table-column :label="messageTranslates('adminArticles', 'articleRelate')">
        <template slot-scope="scope">
          <el-tag v-for="(item, index) in getMatchRelated(scope.row)" :key="index">{{ item.relatedArticle }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column :label="messageTranslates('adminArticles', 'articleAction')" width="150">
        <template slot-scope="scope">
          <div style="display: flex; flex-direction: column;">
            <el-button style="margin-left:10px;" class="action-button" size="mini" @click="viewArticle(scope.row)">View detail</el-button>
            <el-button class="action-button" size="mini" @click="deleteArticle(scope.row)">Delete</el-button>
            <el-button class="action-button" size="mini" @click="editArticle(scope.row)">Edit</el-button>
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
    <add-edit-article ref="articleDialog" @created="loadData()" />
    <el-dialog
      title="View Dialog"
      :visible.sync="dialogArticleVisible"
      width="1000px"
    >
      <div v-if="selectedArticle" class="article-detail">
        <el-breadcrumb separator-class="el-icon-arrow-right">
          <el-breadcrumb-item :to="{ path: '/articles' }">All</el-breadcrumb-item>
          <el-breadcrumb-item>{{ selectedArticle.category }}</el-breadcrumb-item>
          <el-breadcrumb-item>{{ selectedArticle.title }}</el-breadcrumb-item>
        </el-breadcrumb>
        <div class="advertisement--top"> width: 100%, height:150px</div>
        <div style="display:flex">
          <div class="article--cont col-lg-10 col-md-12">
            <div class="article article--title">
              {{ selectedArticle.title }}
            </div>
            <div class="article article--info">
              {{ selectedArticle.authorId }} - Posted Date: {{ getTimeFromDateCreateToNow(selectedArticle.postedDate) }} | Views: {{ selectedArticle.views }}
            </div>
            <div class="article article--content">
              <span>Question</span>
              <div v-html="selectedArticle.question" />
            </div>
          </div>
          <div class=" col-lg-2 col-md-12" style="padding-right: 0">
            <div class="advertisement--right">height: 250px</div>
          </div>
        </div>
      </div>
    </el-dialog>
    <el-dialog
      title="Delete Article"
      :visible.sync="dialogDeleteArticleVisible"
      width="300px"
    >
      <span>Delete this article ?</span>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogDeleteArticleVisible = false">Cancel</el-button>
        <el-button type="primary" @click="confirmDeletion()">Confirm</el-button>
      </span>
    </el-dialog>
  </div>
</template>
<script>
import AddEditArticleDialog from '../../components/controls/AddEditArticles.vue'
import DropdownMenu from '../../components/controls/DropdownMenu'
import { ARTICLE_CATEGORIES } from '../../app.constant'
import moment from 'moment'
import articlesService from '../../services/articles.service'

export default {
  name: 'Articles',
  components: {
    'dropdown-menu': DropdownMenu,
    'add-edit-article': AddEditArticleDialog
  },
  data() {
    return {
      textSearch: null,
      dialogArticleVisible: false,
      dialogDeleteArticleVisible: false,
      filterCategory: [],
      filterType: [],
      filterStatus: [],
      selectionTag: [],
      articlesCached: [],
      articlesList: [],
      totalRow: 0,
      articlesCount: 0,
      page: 1,
      pageSize: 10,
      selectedArticle: null
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    categories() {
      var result = []
      for (const value of Object.entries(ARTICLE_CATEGORIES)) {
        result.push({ text: value[1] })
      }
      return result
    },
    articles() {
      return this.$store.getters['articles/getAll']
    },
    labels() {
      return this.$store.getters['articles/getAllLabels']
    },
    relations() {
      return this.$store.getters['articles/getAllRelations']
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    loadData() {
      this.$store.dispatch('articles/loadAllArticles').then(rs => {
        this.articlesCached = this.articles
        this.totalRow = this.articlesCount = this.articlesCached.length
        this.filterCategory = this.categories.map(k => ({ text: k.text }))
        this.loadTable()
        this.dialogDeleteArticleVisible = false
      })
      this.$store.dispatch('articles/loadAllArticleLabels')
      this.$store.dispatch('articles/loadAllArticleRelations')
    },
    search() {
      this.page = 1
      this.loadTable()
    },
    resetFilterSection() {

    },
    resetFilterType() {

    },
    resetFilterStatus() {

    },
    filter() {
      let result = []
      this.selectionTag = []
      const _filteredSection = this.filterCategory.filter(s => s.checked).map(s => s.text)

      this.selectionTag = _filteredSection

      for (const q of this.articlesCached) {
        let pass = true
        if (_filteredSection.length > 0 && !_filteredSection.includes(q.category)) {
          pass = false
        }

        if (pass) {
          result.push(q)
        }
      }

      result = result.sort((a, b) => a.id - b.id)
      return result
    },
    loadTable() {
      this.articlesCached = this.articles
      let filtered = this.filter().sort((a, b) => b.id - a.id)
      if (this.textSearch) {
        filtered = filtered.filter(q => q.title.toLowerCase().indexOf(this.textSearch.toLowerCase()) >= 0)
        this.articlesList = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      } else {
        this.articlesList = filtered.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
        this.totalRow = filtered.length
      }
    },
    handleClose(tag) {
      this.selectionTag.splice(this.selectionTag.indexOf(tag), 1)

      const _filteredSection = this.filterCategory.filter(s => s.text == tag)
      const _filteredType = this.filterType.filter(s => s.text == tag)
      const _filteredStatus = this.filterStatus.filter(s => s.text == tag)

      if (_filteredSection.length > 0) {
        this.filterCategory.find(s => s.text == tag).checked = false
      } else if (_filteredType.length > 0) {
        this.filterType.find(s => s.text == tag).checked = false
      } else if (_filteredStatus.length > 0) {
        this.filterStatus.find(s => s.text == tag).checked = false
      }
      this.loadTable()
    },
    clearFilter() {
      this.textSearch = ''
      this.filterCategory = this.filterCategory.map(i => ({ ...i, checked: false }))

      this.loadTable()
    },
    handleSizeChange(val) {
      this.pageSize = val
      this.loadTable()
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadTable()
    },
    openAddArticleDialog() {
      this.$refs.articleDialog.openAddDialog()
    },
    getTimeFromDateCreateToNow(time) {
      return moment(new Date(time)).format('MMMM Do YYYY, hh:mm:ss a')
    },
    getMatchLabel(article) {
      return this.labels.filter(r => r.articleId === article.id)
    },
    getMatchRelated(article) {
      return this.relations.filter(r => r.articleId === article.id)
    },
    viewArticle(e) {
      this.$router.push(`/article/${e.id}`)
    },
    editArticle(e) {
      this.$refs.articleDialog.openEditDialog(e)
    },
    deleteArticle(e) {
      this.selectedArticle = e
      this.dialogDeleteArticleVisible = true
    },
    confirmDeletion() {
      articlesService.deleteArticle(this.selectedArticle.id).then(rs => {
        if (rs) {
          this.$notify.success({
            title: 'Article deleted.',
            message: 'Article deleted successfully.',
            type: 'success',
            duration: 2000
          })
          this.loadData()
        } else {
          this.dialogDeleteArticleVisible = false
        }
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
</style>

