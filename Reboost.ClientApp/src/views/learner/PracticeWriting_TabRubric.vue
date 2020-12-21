<template>
  <ul id="rubricList">
    <li v-for="item in tableData" :key="item.id">
      <el-card class="box-card">
        <div slot="header">
          <span class="box-card__title">{{ item.name }}</span>
        </div>
        <el-table
          :data="item.bandScoreDescriptions"
          border
          style="width: 100%"
        >
          <el-table-column
            prop="bandScore"
            label="Band"
            width="70"
          />
          <el-table-column
            prop="description"
            label="Desciption"
          />
        </el-table>
      </el-card>
    </li>
  </ul>

</template>
<script>
export default {
  data() {
    return {
      tableData: []
    }
  },
  mounted() {
    var rubricId = this.$route.params.id
    this.$store.dispatch('rubric/loadRubricByQuestionId', +rubricId).then(() => {
      this.tableData = this.$store.getters['rubric/getByQuestionId']
      console.log(this.tableData)
    })
  }
}
</script>
<style>
  #rubricList .el-table .cell {
      word-break: break-word !important;
  }
</style>
<style scoped>
  ul {
    margin:0;
    padding: 0;
    list-style-type: none;
  }
  li{
      margin-bottom: 10px;
  }

  .box-card {
    width: 100%;
  }
  .box-card__title{
      font-size: 16px;
      font-weight: 600;
  }
</style>
