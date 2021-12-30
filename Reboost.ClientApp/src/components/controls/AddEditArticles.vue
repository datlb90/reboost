<template>
  <el-dialog
    :title="dialogTitle"
    :visible.sync="dialogVisible"
    width="800px"
  >
    <div id="addArticleDialog" class="dialog-content">
      <el-form
        ref="articleForm"
        :model="form"
        label-width="150px"
      >
        <el-form-item prop="title" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addArticle', 'title')">
          <el-input
            v-model="form.title"
            type="text"
            :placeholder="messageTranslates('addArticle', 'titlePlaceholder')"
          />
        </el-form-item>
        <div style="display:flex">
          <el-form-item prop="category" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addArticle', 'category')">
            <el-select v-model="form.category">
              <el-option v-for="(category, index) in categories" :key="index" :label="category.text" :value="category.text" />
            </el-select>
          </el-form-item>
          <el-form-item prop="label" :rules="[{ required: true }]" size="mini" :label="messageTranslates('addArticle', 'label')">
            <el-select
              v-model="form.label"
              multiple
              filterable
              allow-create
              default-first-option
            />
          </el-form-item>
        </div>
        <div style="display: flex">
          <el-form-item prop="author" :rules="[{ required:true }]" size="mini" :label="messageTranslates('addArticle', 'author')">
            <el-input v-model="form.author" type="text" />
          </el-form-item>
          <el-form-item prop="postedDate" :rules="[{ required:true }]" size="mini" :label="messageTranslates('addArticle', 'postedDate')">
            <el-date-picker
              v-model="form.postedDate"
              type="datetime"
              placeholder="Select posted date"
            />
          </el-form-item>
        </div>
        <el-form-item prop="originalUrl" :rules="[{ required:true }]" size="mini" :label="messageTranslates('addArticle', 'originalUrl')">
          <el-input v-model="form.originalUrl" type="text" :placeholder="messageTranslates('addArticle', 'originalUrlPlaceholder')" />
        </el-form-item>
        <el-form-item prop="question" :rules="[{ required:true }]" size="mini" :label="messageTranslates('addArticle', 'question')">
          <el-tiptap
            v-model="form.question"
            lang="en"
            placeholder="Content here…"
            :extensions="tiptapSettings"
            :char-counter-count="false"
          />
        </el-form-item>
        <el-form-item prop="relatedArticles" size="mini" :label="messageTranslates('addArticle', 'relatedArticles')">
          <el-select
            v-model="form.relatedArticles"
            multiple
            filterable
          >
            <el-option
              v-for="(item, index) in relations"
              :key="index"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
      </el-form>
    </div>
    <span slot="footer" class="dialog-footer">
      <el-button size="mini" @click="dialogVisible = false">Cancel</el-button>
      <el-button size="mini" type="primary" @click="submitArticle()">Create</el-button>
    </span>
  </el-dialog>
</template>
<script>
import { ARTICLE_CATEGORIES } from '../../app.constant'
import {
  Doc,
  Text,
  Paragraph,
  Heading,
  Bold,
  Underline,
  Italic,
  Strike,
  ListItem,
  BulletList,
  OrderedList
} from 'element-tiptap'
import articlesService from '../../services/articles.service'

export default {
  name: 'AddEditArticleDialog',
  data() {
    return {
      dialogTitle: 'Add New Article',
      dialogVisible: false,
      form: {
        id: null,
        title: null,
        category: null,
        label: [],
        author: null,
        postedDate: null,
        originalUrl: null,
        question: null,
        relatedArticles: []
      },
      isEdit: false,
      tiptapSettings: [
        new Doc(),
        new Text(),
        new Paragraph(),
        new Heading({ level: 5 }),
        new Bold(),
        new Underline(),
        new Italic(),
        new Strike(),
        new ListItem(),
        new BulletList(),
        new OrderedList()
      ]
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
    relations() {
      const rs = []
      if (this.articles.length > 0) {
        this.articles.forEach(r => {
          rs.push({ name: `#${r.id} - ${r.title}`, id: r.id })
        })
      }
      return rs
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    loadData() {
      this.$store.dispatch('articles/loadAllArticles')
    },
    openAddDialog() {
      this.dialogVisible = true
      setTimeout(() => {
        this.$refs['articleForm'].resetFields()
        this.isEdit = false
      }, 100)
    },
    openEditDialog(e) {
      this.dialogVisible = true

      setTimeout(() => {
        this.$refs['articleForm'].resetFields()
        this.isEdit = true

        this.form.id = e.id
        this.form.title = e.title
        this.form.category = e.category
        this.form.label = e.labels?.map(r => r.name)
        this.form.author = e.authorId
        this.form.postedDate = e.postedDate
        this.form.originalUrl = e.originalUrl
        this.form.question = e.question
        this.form.relatedArticles = e.relatedArticles?.map(r => r.id)
      }, 100)
    },
    submitArticle() {
      this.$refs['articleForm'].validate(async valid => {
        if (valid) {
          var postData = {
            Title: this.form.title,
            Category: this.form.category,
            Labels: this.form.label,
            Author: this.form.author,
            PostedDate: this.form.postedDate,
            OriginalUrl: this.form.originalUrl,
            Question: this.form.question,
            RelatedArticles: this.form.relatedArticles
          }

          if (!this.isEdit) {
            articlesService.createArticle(postData).then(rs => {
              if (rs) {
                this.$notify.success({
                  title: 'Article added.',
                  message: 'Article added successfully.',
                  type: 'success',
                  duration: 2000
                })
                this.$refs['articleForm'].resetFields()
                this.dialogVisible = false
                this.$emit('created')
              }
            })
          } else {
            articlesService.updateArticle(this.form.id, postData).then(rs => {
              if (rs) {
                this.$notify.success({
                  title: 'Article updated.',
                  message: 'Article updated successfully.',
                  type: 'success',
                  duration: 2000
                })
                this.$refs['articleForm'].resetFields()
                this.dialogVisible = false
                this.$emit('created')
              }
            })
          }
        }
      })
    }
  }
}
</script>
<style>
#addArticleDialog .el-dialog__header {
  text-align: center;
}

#addArticleDialog .el-tiptap-editor__menu-bar{
  background: #f0f8ff;
}

#addArticleDialog .el-tiptap-editor>.el-tiptap-editor__content {
  border-bottom: 1px solid #ebeef5;
}
</style>
