<template>
  <div class="dicussion-detail">
    <div class="dicussion-detail__title-discussion">
      <div class="">
        <el-page-header :content="selectedDiscussion.title" @back="onBack" />
      </div>
      <div class="dicussion-detail__title-discussion__btn-right">
        <i :class="{ disabled: checkEdit }" class="el-icon-edit-outline" @click="addDialogVisible = true" />
        <i class="el-icon-bell" />
        <i class="el-icon-warning-outline" />
      </div>
    </div>
    <el-divider />
    <div class="dicussion-detail__body">
      <div>
        <div class="grid-content bg-purple">
          <el-button :disabled="questionOwners" :class="{ background: clickedUpVote}" style="width: 42px" icon="el-icon-caret-top" plain size="mini" @click="onUp" />
          <div class="valueCurrent">
            <span>{{ selectedDiscussion ? selectedDiscussion.votes : 0 }}</span>
          </div>
          <el-button :disabled="questionOwners" :class="{ background: clickedDownVote}" style="margin: 0; width: 42px; margin-bottom: 10px;" icon="el-icon-caret-bottom" plain size="mini" @click="onDown" />
        </div>
      </div>
      <div style="margin-left: 30px">
        <div>
          <div class="comments-other__information">
            <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
            <span style="margin-left: 20px; color: gray">{{ selectedDiscussion.user ? selectedDiscussion.user.lastName : '' }} {{ selectedDiscussion.user ? selectedDiscussion.user.firstName : '' }}</span>
            <span style="margin-left: 20px; color: gray">Last edit: {{ selectedDiscussion ? getTimeFromDateCreateToNow(dateLastEdit) : '' }}</span>
            <span style="margin-left: 20px; color: gray">{{ selectedDiscussion ? selectedDiscussion.views : '' }} views</span>
          </div>
          <div class="comments-other__content_main">
            <pre style="font-size:14px">{{ selectedDiscussion ? selectedDiscussion.content : '' }}</pre>
          </div>
        </div>
      </div>
    </div>
    <div class="dicussion-detail__comment">
      <div class="comment__header">
        <div class="comment__header__count">
          <i class="el-icon-chat-line-square"><span style="margin-left: 10px">Comments: {{ listComments.length }}</span></i>
        </div>
        <div class="comment__header__category">
          <span v-for="item in filterList" :key="item.value" class="selection__Text" :class="{'active':item.value==selectedFilter}" @click="filterSelect(item.value)">{{ item.name }}</span>
        </div>
      </div>
      <div class="input-dicussion-detail__comment">
        <div>
          <el-input
            v-model="contentComment"
            type="textarea"
            :autosize="{ minRows: 3, maxRows: 3}"
            placeholder="Type comment here... (Markdown is supported)"
          />
        </div>
        <div class="input-dicussion-detail__comment_submit">
          <div class="input-dicussion-detail__comment_submit_button">
            <el-button size="mini" type="info" @click="onPost()">Post</el-button>
          </div>
        </div>
      </div>
    </div>
    <div class="comments-other">
      <div v-for="comment in listCommentPerPage" :key="comment.id">
        <div style="display: flex;">
          <div>
            <div class="grid-content bg-purple" style="margin-left: 15px; justify-content: flex-start;">
              <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
            </div>
          </div>
          <div style="margin-left: 10px">
            <div class="grid-content">
              <div class="comments-other__information" style="margin-top: 10px;">
                <span style="margin-left: 10px; color: gray">{{ comment.user ? comment.user.lastName : '' }} {{ + comment.user ? comment.user.firstName : '' }}</span>
                <span style="margin-left: 20px; color: gray">Last edit: {{ getTimeFromDateCreateToNow(comment.updatedDate) }}</span>
              </div>
              <div class="comments-other__content">
                <pre style="font-size: 14px; margin: 0;">{{ comment.content }}</pre>
              </div>
              <div class="comments-other__tools">
                <div class="comments-other__tools__updown">
                  <i :class="{ colorIcon: comment.clickedUpVoteComment }" style="color: #afafafd6;" class="el-icon-caret-top" @click="onUpOtherComment(comment)" />
                  <div class="valueCurrent">
                    <span>{{ comment.votes }}</span>
                  </div>
                  <i :class="{ colorIcon: comment.clickedDownVoteComment }" style="color: #afafafd6;" class="el-icon-caret-bottom" @click="onDownOtherComment(comment)" />
                </div>
                <div class="comments-other__tools__left">
                  <div class="comments-other__tools__left__show-comments-other__tools__left__reply" @click="showComment(comment)">
                    <i class="el-icon-chat-line-square" /><span class="comment-tool-text">{{ comment.textReply }} {{ comment.discussions.length }} replies</span>
                  </div>
                  <div class="comments-other__tools__left__reply" @click="onShowPostCommentChild(comment)">
                    <i class="el-icon-position" /><span class="comment-tool-text">Reply</span>
                  </div>
                  <div class="comments-other__tools__left__share">
                    <i class="el-icon-share" /><span class="comment-tool-text">Share</span>
                  </div>
                  <div class="comments-other__tools__left__report">
                    <i class="el-icon-warning-outline" /><span class="comment-tool-text">Report</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-if="comment.showPostCommentChild" class="reply-comment">
          <div>
            <el-input
              v-model="contentCommentChild"
              type="textarea"
              :autosize="{ minRows: 3, maxRows: 3}"
              placeholder="Type comment here... (Markdown is supported)"
            />
          </div>
          <div class="input-dicussion-detail__comment_submit">
            <div class="input-dicussion-detail__comment_submit_button">
              <el-button size="mini" type="light" @click="onCancel(comment)">Cancel</el-button>
            </div>
            <div class="input-dicussion-detail__comment_submit_button">
              <el-button size="mini" type="info" @click="onPostCommentChild(comment)">Post</el-button>
            </div>
          </div>
        </div>
        <div v-for="commentChild in comment.discussions" :key="commentChild.id" :class="{ showComment: comment.showComment }" style="margin-left: 60px; display: none">
          <div>
            <div class="grid-content bg-purple" style="margin-left: 15px; justify-content: flex-start;">
              <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
            </div>
          </div>
          <div style="margin-left: 10px">
            <div class="grid-content">
              <div class="comments-other__information" style="margin-top: 10px;">
                <span style="margin-left: 10px; color: gray">{{ commentChild.user ? commentChild.user.lastName : '' }} {{ + commentChild.user ? commentChild.user.firstName : '' }}</span>
                <span style="margin-left: 20px; color: gray">Last edit: {{ getTimeFromDateCreateToNow(commentChild.updatedDate) }}</span>
              </div>
              <div class="comments-other__content">
                <pre style="font-size: 14px; margin: 0;">{{ commentChild.content }}</pre>
              </div>
              <div class="comments-other__tools">
                <div class="comments-other__tools__updown">
                  <i :class="{ colorIcon: commentChild.clickedUpVoteComment }" class="el-icon-caret-top" style="color: #afafafd6;" @click="onUpOtherComment(commentChild)" />
                  <div class="valueCurrent">
                    <span>{{ commentChild.votes }}</span>
                  </div>
                  <i :class="{ colorIcon: commentChild.clickedDownVoteComment }" class="el-icon-caret-bottom" style="color: #afafafd6;" @click="onDownOtherComment(commentChild)" />
                </div>
                <div class="comments-other__tools__left">
                  <!-- <div class="comments-other__tools__left__show-comments-other__tools__left__reply">
                    <i class="el-icon-chat-line-square">Show {{ numberReply }} replies</i>
                  </div> -->
                  <div class="comments-other__tools__left__reply" @click="onShowPostReplyComment(comment, commentChild)">
                    <i class="el-icon-position">Reply</i>
                  </div>
                  <div class="comments-other__tools__left__share" style="display: none;">
                    <i class="el-icon-share">Share</i>
                  </div>
                  <div class="comments-other__tools__left__report" style="display: none;">
                    <i class="el-icon-warning-outline">Report</i>
                  </div>
                </div>
              </div>
            </div>
          </div>
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
    <el-dialog
      title="Edit topic"
      :visible.sync="addDialogVisible"
      width="70%"
    >
      <el-input v-model="topicTitle" size="mini" style="width: 50%; padding-left: 10px" placeholder="Enter topic title..." />
      <el-input
        v-model="topicContent"
        class="text-box-content"
        type="textarea"
        size="mini"
        :autosize="{ minRows: 5, maxRows: 10}"
        placeholder="Input your topic content here ..."
      />
      <el-select
        v-model="selectedTags"
        multiple
        filterable
        default-first-option
        placeholder="Choose tags for your topic"
        style="padding: 10px 10px 0 10px;"
        size="mini"
      >
        <el-option
          v-for="tag in tags"
          :key="tag.id"
          :label="tag.name"
          :value="tag.id"
        />
      </el-select>

      <span slot="footer" class="dialog-footer">
        <el-button size="mini" @click="addDialogVisible = false">Close</el-button>
        <el-button size="mini" type="primary" @click="submitDiscussion()">Post <i class="el-icon-s-promotion" /></el-button>
      </span>
    </el-dialog>
  </div>
</template>
<script>
import disussionService from '../../services/discussion.service'
import moment from 'moment'
import { RequestSpamGuard } from '../../utils/guard'
export default {
  name: 'DiscussionDetail',
  data() {
    return {
      spamGuard: new RequestSpamGuard(),
      discussionId: null,
      selectedDiscussion: { user: { lastName: '', firstName: '' }},
      listComments: [],
      listCommentPerPage: [],
      numberReply: 0,
      dateLastEdit: Date.now(),
      contentComment: '',
      contentCommentChild: '',
      clickedUpVote: false,
      clickedDownVote: false,
      checkEdit: true,
      total: 10,
      pageSize: 10,
      page: 1,
      filterList: [
        { name: 'Best', value: 0 },
        { name: 'Most Votes', value: 1 },
        { name: 'Newest to Oldest', value: 2 },
        { name: 'Oldest to Newest', value: 3 }
      ],
      selectedFilter: 0,
      addDialogVisible: false,
      tags: [],
      topicTitle: '',
      topicContent: '',
      selectedTags: [],
      questionOwners: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    currentQuestion() {
      return this.$store.getters['question/getSelected']
    },
    getListComments() {
      return this.$store.getters['discussion/getComments']
    }
  },
  mounted() {
    this.loadCurrentDiscussions()
    if (this.currentQuestion) { this.$store.dispatch('discussion/increaseView', +this.discussionId) }
    this.loadComments()
    this.loadTags()
  },
  methods: {
    loadCurrentDiscussions() {
      this.discussionId = this.$route.params.discussId
      console.log('discussionId', +this.discussionId)
      this.$store.dispatch('discussion/loadDiscussionById', +this.discussionId).then(() => {
        this.selectedDiscussion = this.$store.getters['discussion/getSelected']
        this.dateLastEdit = new Date(this.selectedDiscussion['updatedDate'])
        this.topicTitle = this.selectedDiscussion.title
        this.topicContent = this.selectedDiscussion.content
        this.selectedTags = []
        for (var i = 0; i < this.selectedDiscussion.tags.length; i++) {
          var tag = this.tags.filter(t => t.name == this.selectedDiscussion.tags[i].name)
          this.selectedTags.push(tag[0].id)
        }
        if (this.currentUser.id == this.selectedDiscussion.user.id) {
          this.checkEdit = false
        }
        var voted = this.selectedDiscussion.discussionVote.filter(r => r.userId == this.currentUser.id)
        if (voted[0]) {
          if (voted[0].status == 1) {
            this.clickedUpVote = true
          } else if (voted[0].status == -1) {
            this.clickedDownVote = true
          }
        }
        if (this.selectedDiscussion.userId == this.currentUser.id) {
          this.questionOwners = true
        }
      })
    },
    loadComments() {
      this.$store.dispatch('discussion/loadComments', +this.discussionId).then(() => {
        this.listComments = this.$store.getters['discussion/getComments']
        this.listComments = this.listComments.map(i => ({ ...i, textReply: 'Show', showComment: false, showPostCommentChild: false }))
        this.listComments.forEach(rs => {
          var voted = rs.discussionVote.filter(r => r.userId == this.currentUser.id)
          if (voted[0]) {
            if (voted[0].status == 1) {
              rs.clickedUpVoteComment = true
              rs.clickedDownVoteComment = false
            } else if (voted[0].status == -1) {
              rs.clickedUpVoteComment = false
              rs.clickedDownVoteComment = true
            }
          }
          rs.discussions.forEach(rss => {
            var _voted = rss.discussionVote.filter(r => r.userId == this.currentUser.id)
            if (_voted[0]) {
              if (_voted[0].status == 1) {
                rss.clickedUpVoteComment = true
                rss.clickedDownVoteComment = false
              } else if (_voted[0].status == -1) {
                rss.clickedUpVoteComment = false
                rss.clickedDownVoteComment = true
              }
            } else {
              rss.clickedUpVoteComment = false
              rss.clickedDownVoteComment = false
            }
          })
        })
        this.total = this.listComments.length
        this.loadListComment()
      })
    },
    loadCommentsReplied(e) {
      this.$store.dispatch('discussion/loadComments', +this.discussionId).then(() => {
        this.listComments = this.$store.getters['discussion/getComments']
        this.listComments = this.listComments.map(i => ({ ...i, clickedUpVoteComment: false, clickedDownVoteComment: false, textReply: 'Show', showComment: false, showPostCommentChild: false }))
        this.listComments.filter(rs => rs.id == e.id)[0].showComment = true
        this.listComments.forEach(rs => {
          var voted = rs.discussionVote.filter(r => r.userId == this.currentUser.id)
          if (voted[0]) {
            if (voted[0].status == 1) {
              rs.clickedUpVoteComment = true
              rs.clickedDownVoteComment = false
            } else if (voted[0].status == -1) {
              rs.clickedUpVoteComment = false
              rs.clickedDownVoteComment = true
            }
          }
          rs.discussions.forEach(rss => {
            var _voted = rss.discussionVote.filter(r => r.userId == this.currentUser.id)
            if (_voted[0]) {
              if (_voted[0].status == 1) {
                rss.clickedUpVoteComment = true
                rss.clickedDownVoteComment = false
              } else if (_voted[0].status == -1) {
                rss.clickedUpVoteComment = false
                rss.clickedDownVoteComment = true
              }
            } else {
              rss.clickedUpVoteComment = false
              rss.clickedDownVoteComment = false
            }
          })
        })
        this.total = this.listComments.length
        this.loadListComment()
      })
    },
    onBack() {
      this.$router.push('/PracticeWriting/' + this.selectedDiscussion.questionId + '/discuss')
      // this.discussion = !this.discussion
    },
    onUp() {
      this.spamGuard.check()
        .then(() => {
          if (this.clickedDownVote) {
            this.selectedDiscussion.votes += 2
            this.clickedUpVote = true
            this.clickedDownVote = false
            var discussion = {
              DiscussionId: this.selectedDiscussion.id,
              UserId: this.currentUser.id,
              Status: 1
            }
            this.UpVote(discussion)
            return
          } else if (this.clickedUpVote) {
            this.selectedDiscussion.votes -= 1
            this.clickedUpVote = false
            discussion = {
              DiscussionId: this.selectedDiscussion.id,
              UserId: this.currentUser.id,
              Status: 0
            }
            this.UpVote(discussion)
            return
          }
          this.selectedDiscussion.votes += 1
          this.clickedUpVote = true
          this.clickedDownVote = false
          discussion = {
            DiscussionId: this.selectedDiscussion.id,
            UserId: this.currentUser.id,
            Status: 1
          }
          this.UpVote(discussion)
        })
        .catch(() => {
          this.$notify({
            title: 'Error',
            message: 'You are voting too frequently. Please wait.',
            type: 'error',
            duration: 3000
          })
        })
    },
    onDown() {
      this.spamGuard.check()
        .then(() => {
          if (this.clickedUpVote) {
            this.selectedDiscussion.votes -= 2
            this.clickedDownVote = true
            this.clickedUpVote = false
            var discussion = {
              DiscussionId: this.selectedDiscussion.id,
              UserId: this.currentUser.id,
              Status: -1
            }
            this.DownVote(discussion)
            return
          } else if (this.clickedDownVote) {
            this.selectedDiscussion.votes += 1
            this.clickedDownVote = false
            discussion = {
              DiscussionId: this.selectedDiscussion.id,
              UserId: this.currentUser.id,
              Status: 0
            }
            this.DownVote(discussion)
            return
          }
          this.selectedDiscussion.votes -= 1
          this.clickedUpVote = false
          this.clickedDownVote = true
          discussion = {
            DiscussionId: this.selectedDiscussion.id,
            UserId: this.currentUser.id,
            Status: -1
          }
          this.DownVote(discussion)
        })
        .catch(() => {
          this.$notify({
            title: 'Error',
            message: 'You are voting too frequently. Please wait.',
            type: 'error',
            duration: 3000
          })
        })
    },
    onUpOtherComment(discussion) {
      this.spamGuard.check()
        .then(() => {
          if (discussion.clickedDownVoteComment) {
            discussion.votes += 2
            discussion.clickedUpVoteComment = true
            discussion.clickedDownVoteComment = false
            var _discussion = {
              DiscussionId: discussion.id,
              UserId: this.currentUser.id,
              Status: 1
            }
            this.UpVote(_discussion)
            return
          } else if (discussion.clickedUpVoteComment) {
            discussion.votes -= 1
            discussion.clickedUpVoteComment = false
            _discussion = {
              DiscussionId: discussion.id,
              UserId: this.currentUser.id,
              Status: 0
            }
            console.log(discussion)
            this.UpVote(_discussion)
            return
          }
          discussion.votes += 1
          discussion.clickedUpVoteComment = true
          discussion.clickedDownVoteComment = false
          _discussion = {
            DiscussionId: discussion.id,
            UserId: this.currentUser.id,
            Status: 1
          }
          this.UpVote(_discussion)
        })
        .catch(() => {
          this.$notify({
            title: 'Error',
            message: 'You are voting too frequently. Please wait.',
            type: 'error',
            duration: 3000
          })
        })
    },
    onDownOtherComment(discussion) {
      this.spamGuard.check()
        .then(() => {
          if (discussion.clickedUpVoteComment) {
            discussion.votes -= 2
            discussion.clickedUpVoteComment = false
            discussion.clickedDownVoteComment = true
            var _discussion = {
              DiscussionId: discussion.id,
              UserId: this.currentUser.id,
              Status: -1
            }
            this.DownVote(_discussion)
            return
          } else if (discussion.clickedDownVoteComment) {
            discussion.votes += 1
            discussion.clickedDownVoteComment = false
            _discussion = {
              DiscussionId: discussion.id,
              UserId: this.currentUser.id,
              Status: 0
            }
            this.DownVote(_discussion)
            return
          }
          discussion.votes -= 1
          discussion.clickedUpVoteComment = false
          discussion.clickedDownVoteComment = true
          _discussion = {
            DiscussionId: discussion.id,
            UserId: this.currentUser.id,
            Status: -1
          }
          this.DownVote(_discussion)
        })
        .catch(() => {
          this.$notify({
            title: 'Error',
            message: 'You are voting too frequently. Please wait.',
            type: 'error',
            duration: 3000
          })
        })
    },
    UpdateDiscussion(discussion) {
      this.$store.dispatch('discussion/updateDiscussion', discussion).then(() => {
        console.log(discussion)
      })
    },
    UpVote(discussion) {
      this.$store.dispatch('discussion/upVote', discussion).then(() => {
        console.log(discussion)
      })
    },
    DownVote(discussion) {
      this.$store.dispatch('discussion/downVote', discussion).then(() => {
        console.log(discussion)
      })
    },
    getTimeFromDateCreateToNow(time) {
      return moment(new Date(time)).fromNow()
    },
    onPost() {
      if (!this.contentComment) {
        return
      }

      disussionService.insert({
        QuestionId: this.currentQuestion.id,
        ParentId: +this.discussionId,
        UserId: this.currentUser.id,
        Level: 1,
        Content: this.contentComment
      }).then(rs => {
        this.loadComments()
        this.contentComment = ''
      })
    },
    onPostCommentChild(e) {
      if (!this.contentCommentChild) {
        return
      }

      disussionService.insert({
        QuestionId: this.currentQuestion.id,
        ParentId: e.id,
        UserId: this.currentUser.id,
        Level: 2,
        Content: this.contentCommentChild
      }).then(rs => {
        this.loadCommentsReplied(e)
        this.contentCommentChild = ''
      })
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadListComment()
    },
    loadListComment() {
      this.listCommentPerPage = this.listComments.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
    },
    showComment(e) {
      e.showComment = !e.showComment
      if (e.discussions.length > 0) {
        e.textReply = !e.showComment ? 'Show' : 'Hide'
      }
    },
    filterSelect(value) {
      this.selectedFilter = value
      console.log(value)
      if (value == 1) {
        this.listComments.sort((a, b) => (a.votes > b.votes ? -1 : 1))
        this.loadListComment()
      } else if (value == 2) {
        this.listComments.sort((a, b) => (a.createdDate > b.createdDate ? -1 : 1))
        this.loadListComment()
      } else if (value == 3) {
        this.listComments.sort((a, b) => (a.createdDate < b.createdDate ? -1 : 1))
        this.loadListComment()
      } else {
        this.listComments = this.$store.getters['discussion/getComments']
        this.listComments = this.listComments.map(i => ({ ...i, clickedUpVoteComment: false, clickedDownVoteComment: false, textReply: 'Show', showComment: false }))
        this.total = this.listComments.length
        this.loadListComment()
      }
    },
    onShowPostCommentChild(e) {
      this.listComments.forEach(rs => { rs.showPostCommentChild = false })
      e.showPostCommentChild = true
      this.contentCommentChild = '@' + e.user.username + ' '
    },
    onShowPostReplyComment(comment, child) {
      this.listComments.forEach(rs => { rs.showPostCommentChild = false })
      comment.showPostCommentChild = true
      this.contentCommentChild = '@' + child.user.username + ' '
    },
    onCancel(e) {
      e.showPostCommentChild = false
    },
    loadTags() {
      this.$store.dispatch('discussion/loadAllTags').then(() => {
        this.tags = this.$store.getters['discussion/getAllTags']
      })
    },
    submitDiscussion() {
      var selected = []
      for (var item of this.selectedTags) {
        selected.push(this.tags.filter(t => t.id == item))
      }
      var _discussion = this.selectedDiscussion
      _discussion.title = this.topicTitle
      _discussion.content = this.topicContent
      _discussion.tags = selected.map(t => ({ Id: t[0].id, Name: t[0].name }))
      this.UpdateDiscussion(_discussion)
      console.log(_discussion)
      this.addDialogVisible = false
    }
  }
}
</script>
<style scoped>
.dicussion-detail__title-discussion{
  display: flex;
  justify-content: space-between;
}
.btnBack{
  border-radius: 0px !important;
  border-style: none !important;
  border-right-style: solid !important;
}
.btnBack-title{
  display: flex;
}
.name-discussion{
  margin-left: 10px;
}
.dicussion-detail__title-discussion__btn-right i{
  margin-left: 20px;
  cursor: pointer;
}
.comment__header{
  display: flex;
  justify-content: space-between;
  background-color: #f4f4f4;
  align-items: center;
  height: 40px;
  border: solid 1px #dcdcdc;
  border-right-style: none;
  border-left-style: none;
}
.comment__header__count{
  margin-left: 20px;
}
.el-divider--horizontal{
  margin: 10px 0;
}
.el-textarea{
  padding: 10px 10px 0 10px !important
}
.el-input-group{
  padding: 0 10px 0 10px !important
}
.valueCurrent{
  width: 42px;
  padding: 10px;
  text-align: center;
}
.comments-other__information{
  display: flex;
  align-items: center;
  font-size: 12px;
}
.comments-other{
  margin-top: 20px;
}
.comments-other__content{
  margin: 10px;
  margin-bottom: 5px;
}
.comments-other__tools{
  text-align: justify;
  margin-left: 10px;
  display: flex;
}
.comments-other__tools i{
  cursor: pointer;
}
.comments-other__tools__updown{
  display: flex;
  text-align: center;
  align-items: center;
}
.comments-other__tools__left{
  cursor: pointer;
  margin-left: 10px;
  align-items: center;
  display: flex;
}
.comments-other__tools__left div{
  margin-left: 20px;
  color: gray;
}
.pagination{
  margin: 20px;
  justify-content: center;
}
.selection__Text{
  cursor: pointer;
  margin-right: 10px;
}
.grid-content{
  min-height: 70px;
  display: flex;
  justify-content: center;
  flex-direction: column;
}
.image{
  margin-left: 16px;
}
.comments-other__content_main{
  margin-top: 10px;
}
.el-divider--horizontal {
    margin: 12px 0 !important;
}
pre{
  white-space: pre-wrap !important;
  font-family: "Times New Roman", Times, serif !important;
}
.input-dicussion-detail__comment_submit{
  padding: 0 10px;
  display: flex;
  justify-content: flex-end;
}
.input-dicussion-detail__comment_submit_empty{
  width: 96%;
  box-sizing: border-box;
  background-color: #FFF;
  border: 1px solid #DCDFE6;
  border-radius: 4px;
  border-top-style: none;
}
.dicussion-detail__body{
  display: flex;
}
.disabled {
    cursor: not-allowed !important;
}
.disabled:active {
    pointer-events: none;
}
.showComment{
  display: flex !important;
}
.active{
  font-weight: 700;
}
.reply-comment{
  padding-left: 60px;
}
.showPostCommentChild{
  display: block;
}
.text-box-content{
  padding: 10px 10px 0 10px;
}
.background {
  background-color: #717f8d;
}
.colorIcon{
  color: black !important;
}
.comment-tool-text{
  margin-left: 5px;
  font-size: 9pt;
}
</style>
