<template>
  <div>
    <div v-if="discussion" class="discussion">
      <div class="title">1. Question Title</div>
      <div class="selection_search">
        <div class="selection">
          <span class="selection__Text" style="font-weight: bold;">Hot</span>
          <span class="selection__Text">Newest to Oldest</span>
          <span class="selection__Text">Most Votes</span>
        </div>
        <div class="search">
          <el-input
            v-model="search"
            size="mini"
            style="width: 210px; margin-right: 10px;"
            placeholder="Please topics or comments..."
          />
          <el-button type="info" size="mini">New<i style="margin-left: 5px;" class="el-icon-plus" /></el-button>
        </div>
      </div>
      <div class="dicussion-detail__body">
        <div>
          <el-row>
            <el-col :span="3">
              <div class="grid-content bg-purple" style="align-items: center;">
                <!-- <img src="../../assets/img/iot-banner-image/2.png" style="height: 50px; width: 50px;"> -->
                <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
              </div></el-col>
            <el-col :span="15">
              <div class="grid-content titleQuestions">
                <div class="titleQuestion font-weight-normal" @click="clickTitle">
                  One pass solution w/video whiteboard explaination
                </div>
                <div class="content font-weight-light">
                  asdasjdklasjdklasldasdlkasdlksajkldjaslkjdlkjlk
                </div>
              </div>
            </el-col>
            <el-col :span="3"><div class="grid-content submission">image</div></el-col>
            <el-col :span="3">
              <div class="grid-content view">118
                <i class="el-icon-view" />
              </div>
            </el-col>
          </el-row>
          <hr style="margin: 0;">
        </div>
      </div>
      <!-- <div>
        <el-row>
          <el-col :span="3">
            <div class="grid-content image">
              <img src="../../assets/img/iot-banner-image/2.png" style="height: 50px; width: 50px;">
            </div></el-col>
          <el-col :span="15">
            <div class="grid-content titleQuestions">
              <div class="titleQuestion font-weight-normal">
                One pass solution w/video whiteboard explaination
              </div>
              <div class="content font-weight-light">
                asdasjdklasjdklasldasdlkasdlksajkldjaslkjdlkjlk
              </div>
            </div>
          </el-col>
          <el-col :span="3"><div class="grid-content submission">image</div></el-col>
          <el-col :span="3">
            <div class="grid-content view">118
              <i class="el-icon-view" />
            </div>
          </el-col>
        </el-row>
        <hr style="margin: 0;">
      </div> -->
    </div>
    <div v-if="!discussion" class="dicussion-detail">
      <div class="dicussion-detail__title-discussion">
        <div class="">
          <el-page-header content="Discussion Topic #1" @back="onBack" />
        </div>
        <div class="dicussion-detail__title-discussion__btn-right">
          <i class="el-icon-edit-outline" />
          <i class="el-icon-bell" />
          <i class="el-icon-warning-outline" />
        </div>
      </div>
      <!-- <hr style="margin: 0; margin-top: 10px;"> -->
      <el-divider />
      <div class="dicussion-detail__body">
        <el-row>
          <el-col :span="4" style="width: 13.66667%;">
            <div class="grid-content bg-purple" style="margin-left: 30px">
              <el-button style="width: 42px" icon="el-icon-caret-top" plain size="mini" @click="onUp" />
              <div class="valueCurrent">
                <span>{{ valueCurrent }}</span>
              </div>
              <el-button style="margin: 0; width: 42px" icon="el-icon-caret-bottom" plain size="mini" @click="onDown" />
            </div>
          </el-col>
          <el-col :span="20">
            <div>
              <div class="comments-other__information">
                <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
                <span style="margin-left: 20px; color: gray">{{ username }}</span>
                <i class="el-icon-star-on" style="margin-left: 20px; color: gray">{{ numberStart }}</i>
                <span style="margin-left: 20px; color: gray">Last edit: {{ dateLastEdit }}</span>
                <span style="margin-left: 20px; color: gray">{{ views }}.0K VIEWS</span>
              </div>
              <div class="content">
                <pre style="font-size:14px">{{ content }}</pre>
              </div>
            </div>
          </el-col>
        </el-row>
      </div>
      <div class="dicussion-detail__comment">
        <div class="comment__header">
          <div class="comment__header__count">
            <i class="el-icon-chat-line-square"><span style="margin-left: 10px">Comments: 118</span></i>
          </div>
          <div class="comment__header__category">
            <span class="selection__Text" style="font-weight: bold;">Best</span>
            <span class="selection__Text">Most Votes</span>
            <span class="selection__Text">Newest to Oldest</span>
            <span class="selection__Text">Oldest to Newest</span>
          </div>
        </div>
        <div class="input-dicussion-detail__comment">
          <el-input
            v-model="contentComment"
            type="textarea"
            :autosize="{ minRows: 3, maxRows: 3}"
            placeholder="Type dicussion-detail__comment here... (Markdown is supported)"
          />
          <el-input v-model="input" :disabled="true" class="input-with-select" size="mini" style="width: 100% !important;">
            <el-button slot="append" type="info" size="mini">Post</el-button>
          </el-input>
        </div>
      </div>
      <div class="comments-other">
        <el-row>
          <el-col :span="2">
            <div class="grid-content bg-purple" style="margin-left: 15px; justify-content: flex-start;">
              <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
            </div>
          </el-col>
          <el-col :span="22">
            <div class="grid-content">
              <div class="comments-other__information" style="margin-top: 25px;">
                <span style="margin-left: 10px; color: gray">{{ username }}</span>
                <i class="el-icon-star-on" style="margin-left: 20px; color: gray">{{ numberStart }}</i>
                <span style="margin-left: 20px; color: gray">Last edit: {{ dateLastEdit }}</span>
                <span style="margin-left: 20px; color: gray">{{ views }}.0K VIEWS</span>
              </div>
              <div class="comments-other__content">
                <pre style="font-size: 16px">It's easily to understand if we use the dict in the other way</pre>
              </div>
              <div class="comments-other__tools">
                <div class="comments-other__tools__updown">
                  <i class="el-icon-caret-top" @click="onUpOtherComment" />
                  <div class="valueCurrent">
                    <span>{{ valueCurrent_2 }}</span>
                  </div>
                  <i class="el-icon-caret-bottom" @click="onDownOtherComment" />
                </div>
                <div class="comments-other__tools__left">
                  <div class="comments-other__tools__left__show-comments-other__tools__left__reply">
                    <i class="el-icon-chat-line-square">Show {{ numberReply }} replies</i>
                  </div>
                  <div class="comments-other__tools__left__reply">
                    <i class="el-icon-position">Reply</i>
                  </div>
                  <div class="comments-other__tools__left__share">
                    <i class="el-icon-share">Share</i>
                  </div>
                  <div class="comments-other__tools__left__report">
                    <i class="el-icon-warning-outline">Report</i>
                  </div>
                </div>
              </div>
            </div>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="2">
            <div class="grid-content bg-purple" style="margin-left: 15px; justify-content: flex-start;">
              <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
            </div>
          </el-col>
          <el-col :span="22">
            <div class="grid-content">
              <div class="comments-other__information" style="margin-top: 25px;">
                <span style="margin-left: 10px; color: gray">{{ username }}</span>
                <i class="el-icon-star-on" style="margin-left: 20px; color: gray">{{ numberStart }}</i>
                <span style="margin-left: 20px; color: gray">Last edit: {{ dateLastEdit }}</span>
                <span style="margin-left: 20px; color: gray">{{ views }}.0K VIEWS</span>
              </div>
              <div class="comments-other__content">
                <pre style="font-size: 16px">It's easily to understand if we use the dict in the other way</pre>
              </div>
              <div class="comments-other__tools">
                <div class="comments-other__tools__updown">
                  <i class="el-icon-caret-top" @click="onUpOtherComment" />
                  <div class="valueCurrent">
                    <span>{{ valueCurrent_2 }}</span>
                  </div>
                  <i class="el-icon-caret-bottom" @click="onDownOtherComment" />
                </div>
                <div class="comments-other__tools__left">
                  <div class="comments-other__tools__left__show-comments-other__tools__left__reply">
                    <i class="el-icon-chat-line-square">Show {{ numberReply }} replies</i>
                  </div>
                  <div class="comments-other__tools__left__reply">
                    <i class="el-icon-position">Reply</i>
                  </div>
                  <div class="comments-other__tools__left__share">
                    <i class="el-icon-comments-other__tools__left__share">Share</i>
                  </div>
                  <div class="comments-other__tools__left__report">
                    <i class="el-icon-warning-outline">Report</i>
                  </div>
                </div>
              </div>
            </div>
          </el-col>
        </el-row>
      </div>
      <div class="pagination">
        <el-pagination
          background
          layout="prev, pager, next"
          :total="1000"
        />
      </div>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      search: '',
      discussion: true,
      contentComment: '',
      input: '',
      valueCurrent: 113,
      valueCurrent_2: 113,
      username: 'Nathan_Fegard',
      numberReply: 118,
      numberStart: 118,
      dateLastEdit: Date.now(),
      views: 118,
      content: 'Introduce Yourself! Learn how to confidently give self introduction in English and how to introduce yourself in an email, in an interview or in an English class…with example sentences (questions & answers), video lesson and ESL printable worksheets.What do you say when you meet someone new? Introduce yourself of course! Introducing yourself is much more than saying your name. You need to tell some more comments-other__information about you in English. Introducing yourself to strangers can be tricky because what you say depends on the context, situation (introduce yourself in a job interview, introduce yourself in an email or give self introduction in English class).'
    }
  },
  methods: {
    clickTitle() {
      this.discussion = !this.discussion
    },
    onBack() {
      this.discussion = !this.discussion
    },
    onUp() {
      this.valueCurrent += 1
    },
    onDown() {
      this.valueCurrent -= 1
    },
    onUpOtherComment() {
      this.valueCurrent_2 += 1
    },
    onDownOtherComment() {
      this.valueCurrent_2 -= 1
    }
  }
}
</script>
<style scoped>
.title{
  font-weight: bold;
  font-size: 12px;
}
.selection_search{
  display: flex;
  justify-content: space-between;
  background-color: #d7d9dd;
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
.content{
  font-size: 11px;
  margin-top: 8px;
}
.view{
  display: -webkit-box;
}
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
  background-color: #efefef;
  align-items: center;
  height: 40px;
  border: solid 1px rgb(150, 150, 150);
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
pre {
  text-align: justify;
  white-space: break-spaces;
  font-family: inherit !important;
  margin-bottom: 0 !important;
}
.dicussion-detail__comment{
  margin-top: 20px;
}
</style>
