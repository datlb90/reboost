<template>
  <div>
    <router-view />
  </div>
</template>

<script>
export default {
  components: {},
  data() {
    return {
    }
  }
}
</script>
<style scoped>
.title{
  font-weight: bold;
  font-size: 12px;
  margin-bottom: 10px;
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
.content{
  font-size: 11px;
  margin-top: 8px;
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
</style>
<style>
.tags-input-wrapper-default {
  padding: .5em .25em;
  background: #fff;
  border: 1px solid transparent;
  border-radius: .25em;
  border-color: #dbdbdb;
}
.tags-input-wrapper-default.active {
  border-color:#007bff;
}
.tags-input {
  padding: 3px 3px 3px 12px;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
}
::placeholder{
  color:#DCDFE6 ;
}
.tags-input input[type="text"] {
    color: #495057;
}
.tags-input input {
    flex: 1;
    background: transparent;
    border: none;
}
.tags-input input:focus{
  outline: none;
}
.typeahead-badges > span {
    cursor: pointer;
    margin-right: 0.3em;
}
.tags-input-typeahead-item-default {
    color: #fff;
    background-color: #343a40;
}
.typeahead-hide-btn {
    color: #999 !important;
    font-style: italic;
}
.tags-input-badge {
    position: relative;
    display: inline-block;
    padding: 0.25em 0.4em;
    font-size: 75%;
    font-weight: 700;
    line-height: 1;
    text-align: center;
    white-space: nowrap;
    vertical-align: baseline;
    border-radius: 0.25em;
    overflow: hidden;
    text-overflow: ellipsis;
}
.el-dialog__body{
  padding: 15px 20px !important;
}
</style>
