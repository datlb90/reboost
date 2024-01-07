<template>
  <div>
    <div class="tab-content">
      <div class="title">{{ currentQuestion.title }}</div>
      <div class="selection_search">
        <div class="selection">
          <span v-for="item in filterList" :key="item.value" class="selection__Text" :class="{'active':item.value==selectedFilter}" @click="filterSelect(item.value)">{{ item.name }}</span>
        </div>
        <div class="search">
          <el-input
            v-model="search"
            size="mini"
            style="width: 210px; margin-right: 10px;"
            placeholder="Please enter topics or content..."
            @input="searchDiscussion()"
          />
          <el-button type="info" size="mini" @click="addDialogVisible = true">New<i style="margin-left: 5px;" class="el-icon-plus" /></el-button>
        </div>
      </div>
      <div class="dicussion-detail__body">
        <div v-for="item in listDiscussPerPage" :key="item.id" class="dis-row">
          <div class="left-content">
            <div class="left-context__avatar">
              <div class="grid-content bg-purple" style="align-items: center;">
                <el-avatar style="margin: 0 20px 0 15px;" src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
              </div>
            </div>
            <div class="left-context__title">
              <div class="grid-content titleQuestions">
                <div class="titleQuestion font-weight-normal">
                  <div class="title-tag-container">
                    <router-link :to="getDiscussionDetailUrl(item)" class="nav-link">{{ item ? item.title : '' }}</router-link>
                    <!-- <el-tag
                      v-for="tag in item.tags"
                      :key="tag.id"
                      class="tag-title"
                      type="info"
                      effect="plain"
                      size="mini"
                    >
                      {{ tag.name }}
                    </el-tag> -->
                  </div>
                </div>
                <div class="info-content font-weight-light">
                  {{ item.user ? item.user.firstName + item.user.lastName : '' }} create at: {{ item ? getTimeFromDateCreateToNow(item.createdDate) : '' }} |
                  {{ listReplyInfo.filter(r=>r.parentId == item.id)[0] ? 'Last reply: '+listReplyInfo.filter(r=>r.parentId == item.id)[0].user.username + ' '+
                    getTimeFromDateCreateToNow(listReplyInfo.filter(r=>r.parentId == item.id)[0].createdDate) : 'No reply yet.' }}
                </div>
              </div>
            </div>
          </div>
          <div class="right-content">
            <div class="grid-content" style="flex-flow: row; align-items: center;margin:0px 20px">
              <i style="color:#e0e0e0; font-size: 1.4rem;" class="el-icon-caret-top" />
              <div style="padding-left:5px">{{ item ? item.votes : '' }}</div>
            </div>
            <div class="grid-content view">
              <i style="color:#e0e0e0;" class="el-icon-view" />
              {{ item ? item.views : '' }}
            </div>
          </div>
        </div>
        <div class="pagination">
          <el-pagination
            background
            layout="prev, pager, next"
            :total="total"
            :page-size="pageSize"
            @current-change="handleCurrentChange"
          />
        </div>
      </div>
    </div>
    <el-dialog
      title="Add new topic"
      :visible.sync="addDialogVisible"
      width="70%"
    >
      <el-form ref="formPostDiscussion" :model="formPostDiscussion" size="mini">
        <el-form-item
          prop="topicTitle"
          :rules="[{ required: true, message: 'Please input title', trigger: 'blur' }]"
        >
          <el-input v-model="formPostDiscussion.topicTitle" size="mini" style="width: 50%;" placeholder="Enter topic title..." />
        </el-form-item>
        <el-form-item
          prop="topicContent"
          :rules="[{ required: true, message: 'Please input content', trigger: 'blur' }]"
        >
          <el-input
            v-model="formPostDiscussion.topicContent"
            type="textarea"
            size="mini"
            :autosize="{ minRows: 5, maxRows: 10}"
            placeholder="Input your topic content here ..."
          />
        </el-form-item>
        <el-select
          v-model="selectedTags"
          multiple
          filterable
          default-first-option
          placeholder="Choose tags for your topic"
          style="width: 50%;"
          size="mini"
        >
          <el-option
            v-for="tag in tags"
            :key="tag.id"
            :label="tag.name"
            :value="tag.id"
          />
        </el-select>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button size="mini" @click="addDialogVisible = false">Close</el-button>
        <el-button size="mini" type="primary" @click="submitDiscussion()">Post <i class="el-icon-s-promotion" /></el-button>
      </span>
    </el-dialog>
  </div>
</template>
<script>
import disussionService from '../../services/discussion.service'
import moment from 'moment-timezone'
export default {
  components: {},
  data() {
    return {
      formPostDiscussion: {
        topicTitle: '',
        topicContent: ''
      },
      search: '',
      discussion: true,
      addDialogVisible: false,
      topicTitle: '',
      topicContent: '',
      selectedTags: [],
      discussionList: [],
      discussionListCached: [],
      listDiscussPerPage: [],
      searchDelayHandle: null,
      listReplyInfo: [],
      listParentId: [],
      pageSize: 10,
      total: 0,
      page: 1,
      tags: [],
      filterList: [
        { name: 'Hot', value: 0 },
        { name: 'Newest to Oldest', value: 1 },
        { name: 'Most Votes', value: 2 }],
      selectedFilter: 0
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    getAllDiscussion() {
      var data = this.$store.getters['discussion/getAllByQuestionId']
      return data
    },
    currentQuestion() {
      return this.$store.getters['question/getSelected']
    }
  },
  mounted() {
    this.loadDiscussions()
    this.loadTags()
  },
  methods: {
    filterSelect(value) {
      this.selectedFilter = value
      if (value == 1) {
        this.discussionListCached.sort((a, b) => (a.createdDate > b.createdDate ? -1 : 1))
        this.loadList()
      } else if (value == 2) {
        this.discussionListCached.sort((a, b) => (a.votes > b.votes ? -1 : 1))
        this.loadList()
      } else {
        this.discussionListCached = this.$store.getters['discussion/getAllByQuestionId'].filter(d => d.title.toLowerCase().indexOf(this.search.toLowerCase()) >= 0 ||
        d.content.toLowerCase().indexOf(this.search.toLowerCase()) >= 0)
        this.loadList()
      }
    },
    loadDiscussions() {
      this.$store.dispatch('discussion/loadDiscussionByQuestionId', +this.currentQuestion.id).then(() => {
        this.discussionList = this.$store.getters['discussion/getAllByQuestionId']
        this.discussionListCached = [...this.discussionList]
        this.loadList()
        console.log('all discussion', this.discussionList)
        this.getLatestReplyInfo()
      })
    },
    loadTags() {
      this.$store.dispatch('discussion/loadAllTags').then(() => {
        this.tags = this.$store.getters['discussion/getAllTags']
      })
    },
    submitDiscussion() {
      this.$refs['formPostDiscussion'].validate((valid) => {
        if (valid) {
          var selected = []
          for (var item of this.selectedTags) {
            selected.push(this.tags.filter(t => t.id == item))
          }
          disussionService.insert({
            questionId: this.currentQuestion.id,
            userId: this.currentUser.id,
            title: this.formPostDiscussion.topicTitle,
            content: this.formPostDiscussion.topicContent,
            tags: selected.map(t => ({ Id: t[0].id, Name: t[0].name })),
            level: 0
          }).then(rs => {
            this.formPostDiscussion.topicTitle = ''
            this.formPostDiscussion.topicContent = ''
            this.loadDiscussions()
            this.$notify.success({
              title: 'Discussion added.',
              message: 'Discussion added successfully.',
              type: 'success',
              duration: 2000
            })
            this.addDialogVisible = false
          })
        } else {
          return false
        }
      })
    },
    searchDiscussion() {
      if (this.searchDelayHandle) {
        clearTimeout(this.searchDelayHandle)
      }
      this.searchDelayHandle = setTimeout(() => {
        this.discussionListCached = this.discussionList.filter(d => d.title.toLowerCase().indexOf(this.search.toLowerCase()) >= 0 ||
        d.content.toLowerCase().indexOf(this.search.toLowerCase()) >= 0)
        this.loadList()
      }, 500)
    },
    getDiscussionDetailUrl(item) {
      return '/practice/' + this.currentQuestion.id + '/discuss/' + item['id']
    },
    getTimeFromDateCreateToNow(time) {
      var tz = moment.tz.guess()
      return moment.utc(time).tz(tz).format('DD/MM/YYYY LT')
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadList()
    },
    loadList() {
      this.listDiscussPerPage = this.discussionListCached.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.total = this.discussionListCached.length
    },
    loadComments(id) {
      console.log('item.id:', id)
      this.$store.dispatch('discussion/loadComments', +id).then(() => {
        var comments = this.$store.getters['discussion/getComments']
        if (comments.length > 0) {
          comments = comments.sort((a, b) => (a.createdDate > b.createdDate ? -1 : 1))
          console.log('discussion id:', id, 'comments :', comments)
          this.listReplyInfo.push(comments[0])
        }
      })
    },
    getLatestReplyInfo() {
      for (const item of this.discussionList) {
        this.loadComments(item.id)
      }
      console.log('All comment', this.listReplyInfo, this.listParentId)
    }
  }
}
</script>
<style scoped>
.active{
  font-weight: 700;
}
.tab-content{
  padding: 0 70px 0 70px;
}
.title{
  font-weight: bold;
  font-size: 14px;
  margin: 0 0 10px 10px;
}
.selection_search{
  display: flex;
  justify-content: space-between;
  background-color: #f4f4f4;
  align-items: center;
  height: 40px;
}
.selection{
  margin-left: 10px;
}
.search{
  margin-right: 10px;
  display: flex;
  float: right;
}
.selection__Text{
  cursor: pointer;
  margin-right: 10px;
}
.grid-content{
  min-height: 70px;
  text-align: center;
  display: flex;
  justify-content: center;
  flex-direction: column;
}
.image{
  margin-left: 16px;
}
.titleQuestions{
  text-align: left;
}
.titleQuestion{
  font-size: 14px;
  cursor: pointer;
}
.info-content{
  font-size: 11px;
  margin-top: 8px;
  display: flex;
}
.view{
  display: -webkit-box;
}

.pagination{
  margin: 20px;
  justify-content: center;
}
pre {
  text-align: justify;
  white-space: break-spaces;
  font-family: inherit !important;
  margin-bottom: 0 !important;
}
.dicussion-detail__comment{
  margin-top: 20px;
}
.text-box-content{
  padding: 10px 10px 0 10px;
}
.dis-row{
  display: flex;
  justify-content: space-between;
  border-bottom: 1px solid #e2e2e2;
  padding-right: 20px;
}
.left-content{
  display: flex;
}
.left-context__avatar{
  margin-right: 10px;
}
.right-content{
  display: flex;
}
.titleQuestion .nav-link{
  padding: 0 !important;
}
.title-tag-container{
  display: flex;
}
.tag-title{
  margin: 0 5px;
}
.tag-title:hover {
    border-color:rgb(119, 133, 139) !important;
}
</style>

