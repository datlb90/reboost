<template>
  <div class="article-detail">
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/articles' }">All</el-breadcrumb-item>
      <el-breadcrumb-item>{{ article.category }}</el-breadcrumb-item>
      <el-breadcrumb-item>{{ article.title }}</el-breadcrumb-item>
    </el-breadcrumb>
    <div class="advertisement--top"> width: 100%, height:150px</div>
    <div style="display:flex">
      <div class="article--cont col-lg-10 col-md-12">
        <div class="article article--title">
          {{ article.title }}
        </div>
        <div class="article article--info">
          {{ article.authorId }} - Posted Date: {{ getTimeFromDateCreateToNow(article.postedDate) }} | Views: {{ article.views }}
        </div>
        <div class="article article--content">
          <span>Question</span>
          <div v-html="article.question" />
        </div>
      </div>
      <div class=" col-lg-2 col-md-12" style="padding-right: 0">
        <div class="advertisement--right">height: 250px</div>
      </div>
    </div>
  </div>
</template>
<script>
import moment from 'moment'

export default {
  name: 'ArticleDetail',
  data() {
    return {
      articleId: null
    }
  },
  computed: {
    article() {
      return this.$store.getters['articles/getCurrentArticle']
    }
  },
  beforeCreate() {
    this.articleId = this.$route.params.id
    if (this.articleId) {
      this.$store.dispatch('articles/loadArticleById', +this.articleId)
    }
  },
  mounted() {

  },
  methods: {
    getTimeFromDateCreateToNow(time) {
      return moment(new Date(time)).fromNow()
    }
  }
}
</script>
<style>
.article-detail {
  padding: 0 120px;
  margin-top: 20px;
}
.advertisement--top{
    margin: 10px 0;
    width:100%;
    height: 150px;
    border: 1px solid black;
}
.article{
  padding: 5px 10px;
  border-bottom: 1px solid #eaeaea;
}
.article--cont{
  padding: 0 !important;
  border: 1px solid #eaeaea;
  border-radius: 2px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .12), 0 0 6px rgba(0, 0, 0, .04);
  height: fit-content;
}
.article--title{
  font-size:16px;
  font-weight: bold;
}
.article--info{
    font-size: 12px;
}
.advertisement--right{
    padding: 0;
    height: 250px;
    border: 1px solid black;
}
</style>
