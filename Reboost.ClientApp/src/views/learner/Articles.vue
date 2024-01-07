<template>
  <div id="articles">
    <el-row class="categories-cont">
      <el-col
        v-for="(item, index) in categories"
        :key="index"
        :class="selectedCategory===item.text ? 'active' : ''"
        :span="4"
        class="category--card"
      >
        <button
          class="category--btn"
          @click="selectCategory(item.text)"
        > {{ item.text }}</button>
      </el-col>
    </el-row>
    <el-row class="advertisement-top">
      <!-- <ins
        class="adsbygoogle"
        style="display:block"
        data-ad-client="ca-pub-9880795331210715"
        data-ad-slot="8468836821"
        data-ad-format="auto"
        data-full-width-responsive="true"
      /> -->
    </el-row>
    <el-row class="articles-container">
      <div class="articles-list__container col-lg-10 col-md-12">
        <div class="articles-list__container__type">
          <span class="articles-list__header active" @click="titleClick('123')">
            All interview Questions
          </span>
          <span class="articles-list__header" @click="titleClick('123')">
            System Design
          </span>
          <span class="articles-list__header" @click="titleClick('123')">
            Operating System
          </span>
        </div>
        <div class="articles-list__filter">
          <div class="selection-list">
            <span v-for="item in filterList" :key="item.value" class="selection__text" :class="{'active':item.value==selectedFilter}" @click="filterSelect(item.value)">{{ item.name }}</span>
          </div>
          <div class="searchText">
            <el-input
              v-model="searchText"
              size="mini"
              style="width: 210px; margin-right: 10px;"
              placeholder="Please enter title..."
              @input="searchArticles()"
            />
            <el-button type="info" size="mini" @click="openAddArticleDialog()">New<i style="margin-left: 5px;" class="el-icon-plus" /></el-button>
          </div>
        </div>
        <div class="articles-list__container_content">
          <div v-for="(item, index) in listArticlesPerPage" :key="index" class="article-detail__row">
            <div class="article-detail__info">
              <div class="author--avatar">
                <div class="grid-content" style="align-items: center;">
                  <el-avatar style="margin: 0 20px 0 15px;" src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
                </div>
              </div>
              <div class="article-detail__info">
                <div class="grid-content" style="text-align:left">
                  <div class="article__title">
                    <router-link :to="`/article/${item.id}`" style="padding: 0">{{ item.title }}</router-link>
                  </div>
                  <div class="article-detail__author-name">
                    {{ item.authorId }} created at: {{ getTimeFromDateCreateToNow(item.postedDate) }}
                  </div>
                </div>
              </div>
            </div>
            <div class="article--view-count">
              <div class="grid-content views">
                <i style="color:#e0e0e0;margin-right:5px" class="el-icon-view" />
                {{ item.views || 0 }}
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-lg-2 col-md-12" style="padding-right: 0;">
        <div class="articles-labels_container">
          <div class="label--header">
            Tags
          </div>
          <div class="label--body">
            <div class="label--search">
              <div v-if="selectedLabels.length>0" class="label-list selected-tag">
                <button v-for="(item, index) in selectedLabels" :key="index" class="label-btn" @click="unselectLabel(item)">
                  <div class="inner-btn--container">
                    <div class="pane__left-pane">
                      <span class="label-detail">
                        {{ item.name }}
                      </span>
                    </div>
                    <div class="pane__right-pane">
                      <button class="label-btn" style="border:none; padding:0; margin:0">
                        <i class="el-icon-error" />
                      </button>
                    </div>
                  </div>
                </button>
              </div>
              <el-input
                v-model="searchLabel"
                size="mini"
                suffix-icon="el-icon-search"
                placeholder="Search by label..."
                @input="searchLabels()"
              />
            </div>
            <div v-if="listLabelsShow.length>0" class="label-list">
              <button v-for="(item, index) in listLabelsShow" :key="index" class="label-btn" @click="selectLabel(item)">
                <div class="inner-btn--container">
                  <div class="pane__left-pane">
                    <span class="label-detail">
                      {{ item.name }}
                    </span>
                  </div>
                  <div class="pane__right-pane">
                    <span class="label-detail">
                      {{ item.count }}
                    </span>
                  </div>
                </div>
              </button>
            </div>
            <span v-if="listLabels.length > listLabelsShow.length" class="not-show-text">... {{ listLabels.length - listLabelsShow.length }} labels not show </span>
          </div>

        </div>
        <div class="advertisement--left">advertisement size: 198x175</div>
      </div>
    </el-row>
    <div class="pagination">
      <el-pagination
        background
        layout="prev, pager, next"
        :total="total"
        :page-size="pageSize"
        @current-change="handlePageChange"
      />
    </div>
    <addArticle ref="addEditArticles" @created="loadArticlesList()" />
  </div>
</template>
<script>
import { ARTICLE_CATEGORIES } from '../../app.constant'
import moment from 'moment-timezone'
import AddEditArticle from '../../components/controls/AddEditArticles.vue'

export default {
  name: 'LearnerArticles',
  components: {
    'addArticle': AddEditArticle
  },
  data() {
    return {
      selectedCategory: '',
      filterList: [
        { name: 'Hot', value: 0 },
        { name: 'Newest to Oldest', value: 1 },
        { name: 'Most Views', value: 2 }],
      selectedFilter: 0,
      searchText: '',
      searchLabel: '',
      articlesListCached: [],
      listArticlesPerPage: [],
      pageSize: 10,
      total: 0,
      page: 1,
      articlesCount: 0,
      searchDelayHandle: null,
      selectedLabels: [],
      listLabels: [],
      listLabelsShow: []
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    categories() {
      var result = [{ text: 'All' }]
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
    // const adsbygoogle = window.adsbygoogle || []
    // adsbygoogle.push({})
    this.selectCategory('All')
    this.loadArticlesList()
  },
  methods: {
    loadArticlesList() {
      this.$store.dispatch('articles/loadAllArticles').then(rs => {
        this.articlesListCached = this.articles
        this.total = this.articlesCount = this.articlesListCached.length
        this.loadList()
      })
      this.$store.dispatch('articles/loadAllArticleLabels').then(rs => {
        this.setListLabels()
      })
    },
    loadList() {
      this.listArticlesPerPage = this.articlesListCached.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.total = this.articlesListCached.length
    },
    selectCategory(e) {
      this.selectedCategory = e
      this.articlesListCached = this.articles
      if (e !== 'All') {
        this.articlesListCached = this.articles.filter(r => r.category === e)
      }
      this.loadList()
    },
    titleClick(e) {
      console.log('e', e)
    },
    filterSelect(value) {
      if (value != null) {
        this.selectedFilter = value
      }
      if (this.selectedFilter == 1) {
        this.articlesListCached.sort((a, b) => (a.postedDate > b.postedDate ? -1 : 1))
      } else if (this.selectedFilter == 2) {
        this.articlesListCached.sort((a, b) => (a.views > b.views ? -1 : 1))
      } else {
        this.articlesListCached = this.articles.filter(d => d.title.toLowerCase().indexOf(this.searchText.toLowerCase()) >= 0)
      }
      if (this.selectedLabels.length > 0) {
        this.articlesListCached = []
        this.articles.forEach(a => {
          if (a.labels.filter(l => this.selectedLabels.filter(s => s.name === l.name).length > 0).length > 0) {
            this.articlesListCached.push(a)
          }
        })
      }
      this.loadList()
    },
    getTimeFromDateCreateToNow(time) {
      var tz = moment.tz.guess()
      return moment.utc(time).tz(tz).format('DD/MM/YYYY LT')
    },
    handlePageChange(page) {
      this.page = page
      this.loadList()
    },
    openAddArticleDialog() {
      this.$refs.addEditArticles.openAddDialog()
    },
    searchArticles() {
      if (this.searchDelayHandle) {
        clearTimeout(this.searchDelayHandle)
      }
      this.searchDelayHandle = setTimeout(() => {
        this.articlesListCached = this.articles.filter(d => d.title.toLowerCase().indexOf(this.searchText.toLowerCase()) >= 0)
        this.loadList()
      }, 500)
    },
    setListLabels() {
      this.listLabels = []
      this.labels.forEach(label => {
        if (this.listLabels.filter(lb => lb?.name === label.name).length === 0) {
          this.listLabels.push({ ...label, count: 1 })
        } else {
          this.listLabels = this.listLabels.map(lb => {
            if (lb.name === label.name) {
              lb.count++
            }
            return lb
          })
        }
      })

      if (this.searchLabel) {
        this.listLabels = this.listLabels.filter(l => l.name.trim().toLowerCase().includes(this.searchLabel.trim().toLowerCase()))
      }
      this.listLabels.sort((a, b) => (a.count > b.count ? -1 : 1))
      this.listLabelsShow = this.listLabels.slice(0, 5)
    },
    searchLabels() {
      this.setListLabels()
    },
    unselectLabel(item) {
      const index = this.selectedLabels.indexOf(item)
      if (index > -1) {
        this.selectedLabels.splice(index, 1)
      }
      this.listLabels.push(item)
      this.listLabels.sort((a, b) => (a.count > b.count ? -1 : 1))
      this.listLabelsShow = this.listLabels.slice(0, 5)
      this.filterSelect(null)
    },
    selectLabel(item) {
      this.selectedLabels.push(item)
      const index = this.listLabels.indexOf(item)
      if (index > -1) {
        this.listLabels.splice(index, 1)
      }
      this.selectedLabels.sort((a, b) => (a.count > b.count ? -1 : 1))
      this.listLabelsShow = this.listLabels.slice(0, 5)
      this.filterSelect(null)
    }
  }
}
</script>
<style scoped>
#articles{
  padding: 0 120px;
  margin-top: 20px;
}
.categories-cont{
  display: flex;
  margin: 10px 0;
}
.category--card{
  min-height: 65px;
  height: fit-content;
  padding: 5px;
  font-size: 18px;
  background-color: #ffffff;
  text-align: center;
  border: 1px solid #eaeaea;
  border-radius: 2px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04);
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}
.category--btn{
  border: none;
  background-color: transparent;
  height:60px;
  width:100%
}
.active .category--btn{
  color: #fff
}
.category--card.active{
  color: white;
  background-color: #888888;
}
.category--card + .category--card {
  margin-left: 10px;
}
.advertisement-top{
  width:100%;
  height: 120px;
  border: 1px solid black;
}
.articles {
  width: 100%;
  padding: 10px 0 0 0;
}
.articles-container {
  display: flex;
  margin-top: 10px;
}
.articles-list__container {
  padding: 0 !important;
  border: 1px solid #eaeaea;
  border-radius: 2px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04);
}
.articles-list__container__type{
  padding: 10px;
}
.articles-list__header {
  font-size: 16px;
  padding: 0 10px;
  font-weight: 500;
  color: #888888;
}
.articles-list__header.active {
  color: black;
}
.label--header{
  font-size: 16px;
  padding: 10px;
  font-weight: 500;
  border-bottom: 1px solid #eaeaea;
}
.label--body {
  padding: 10px;
}
.label--search{
  margin-bottom: 5px;
}
.articles__header--title + .articles__header--title{
  margin-left:10px;
}
.articles-labels_container{
  padding: 0 !important;
  border: 1px solid #eaeaea;
  border-radius: 2px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04);
  height: fit-content;
  max-height: 450px;
}
.articles-list__filter{
  /* margin-left: 10px; */
  padding: 0 10px 0 10px;
  display: flex;
  justify-content: space-between;
  background-color: #f4f4f4;
  align-items: center;
  height: 40px;
}
.selection__text{
  cursor: pointer;
  margin-right: 10px;
}
.active{
  font-weight: 700;
}
.selection-list{
  margin-left: 10px;
}
.article-detail__row{
  display: flex;
  justify-content: space-between;
  border-bottom: 1px solid #e2e2e2;
  padding-right: 20px;
}
.article-detail__info{
  display: flex;
}
.author--avatar{
  margin-left: 10px;
}
.article-detail__info{

}
.grid-content{
  min-height: 70px;
  text-align: center;
  display: flex;
  justify-content: center;
  flex-direction: column;
}
.article__title{
  font-size: 14px;
  cursor: pointer;
}
.article-detail__author-name{
  font-size: 11px;
  margin-top: 8px;
  display: flex;
}
.article--view-count{
  display: flex;
}
.views{
  flex-flow: row;
  align-items: center;
  margin: 0 20px;
}
.pagination{
  margin: 20px;
  justify-content: center;
}
.label-list{
  display: flex;
  flex-wrap: wrap;
  padding: 5px 0;
  margin-left: -10px;
}
.label-btn{
  margin-left: 10px;
  margin-top: 10px;
  border: 1px solid rgba(0, 0, 0, 0.05);
  border-radius: 3px;
  color: rgb(55, 71, 79);
  font-size: 12px;
  text-overflow: ellipsis;
  overflow-x: hidden;
  white-space: nowrap;
  cursor: pointer;
  background-color: rgb(250, 250, 250);
  padding: 0px;
  max-width: 200px;
  transition: background-color 0.18s ease-in-out 0s, border-color 0.18s ease-in-out 0s;
}
.label-btn:hover{
  border-color: rgb(144, 164, 174);
}
.selected-tag .label-btn{
  background-color: rgb(96, 125, 139);
  color: #ffffff;
}
.selected-tag .label-detail{
  color: #ffffff;
}
.inner-btn--container{
  display:flex;
  -webkit-box-align: center;
  align-items: center;
}
.pane__left-pane{
  display: flex;
  padding: 5px;
  border-right: 1px solid rgba(0, 0, 0, 0.05);
  transition: border-color 0.18s ease-in-out 0s;
  min-width: 0px;
}
.pane__right-pane{
  padding: 5px;
}
.label-detail{
  font-size: 12px;
  color: rgb(55, 71, 79);
  overflow-x: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
.not-show-text {
  color: rgb(158, 158, 158);
  display: block;
  font-size: 12px;
  padding-top: 10px;
}
.advertisement--left{
  margin-top: 10px;
  border: 1px solid #eaeaea;
  border-radius: 2px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04);
  height: fit-content;
  width: 100%;
  height: 175px;
}
</style>
