<template>
  <div>
    <ul id="rubricList">
      <li v-for="item in tableData" :key="item.name">
        <el-card class="box-card" style="width: 100%">
          <div slot="header" class="clearfix">
            <b class="card-title">{{ item.name }}</b>
            <i v-if="!listExpand.includes(item.name)" class="el-icon-arrow-down" style="position: absolute;right: 25px;" @click="toggleExpand(item.name)" />
            <i v-if="listExpand.includes(item.name)" class="el-icon-arrow-up" style="position: absolute;right: 25px;" @click="toggleExpand(item.name)" />
          </div>
          <el-table
            :class="{ hide: listExpand.includes(item.name)}"
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
    <div v-if="tableData.length == 0" style="width: 100%; text-align: center;">
      <span class="tab-none-content">No rubric</span>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      tableData: [],
      listExpand: []
    }
  },
  mounted() {
    var rubricId = this.$route.params.id
    this.$store.dispatch('rubric/loadRubricByQuestionId', +rubricId).then(() => {
      this.tableData = this.$store.getters['rubric/getByQuestionId']
    })
  },
  methods: {
    toggleExpand(e) {
      if (!this.listExpand.includes(e)) {
        this.listExpand.push(e)
      } else {
        this.listExpand.splice(this.listExpand.indexOf(e), 1)
      }
    }
  }
}
</script>
<style>
  #rubricList .el-table .cell {
    white-space: pre-wrap;
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
.hide{
  display: none !important;
}
  .tab-none-content{
    color: rgba(176, 185, 196, 0.7);
  }
</style>
