<template>
  <div>
    <div class="title">1. Question Title</div>
    <div class="selection_search">
      <div class="selection">
        <span class="selectionText" style="font-weight: bold;">Hot</span>
        <span class="selectionText">Newest to Oldest</span>
        <span class="selectionText">Most Votes</span>
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
    <div class="body">
      <div>
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
      </div>
    </div>
    <div>
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
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      search: ''
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
.selectionText{
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
}
.content{
  font-size: 11px;
  margin-top: 8px;
}
.submission{

}
.view{
  display: -webkit-box;
}
</style>
