<template>
  <div id="disputes" style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15">
        <div style="display: flex; justify-content:space-between">
          <h3>Disputes</h3>
        </div>
        <el-main>
          <el-table
            :data="disputesList"
            stripe
            border
            style="width: 100%"
            size="mini"
          >
            <el-table-column
              prop="name"
              label="Name"
              align="center"
            />
            <el-table-column
              prop="questionId"
              label="Question Id"
              align="center"
            />
            <el-table-column
              prop="reasons"
              label="Reasons"
              align="center"
            />
            <el-table-column align="center" label="Actions">
              <template slot-scope="scope">
                <el-button size="mini" @click="navigateToReview(scope.row)">
                  View
                </el-button>
              </template>
            </el-table-column>
          </el-table>
          <div class="pagination">
            <el-pagination
              background
              layout="prev, pager, next"
              :total="total"
              :page-size="pageSize"
              @current-change="handleCurrentChange"
            />
          </div>
        </el-main>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import reviewService from '../../services/review.service'

export default {
  name: 'Disputes',
  components: {},
  data() {
    return {
      disputesList: [],
      disputesListCached: [],
      listDisputesPerPage: [],
      pageSize: 15,
      total: 0,
      page: 1
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  mounted() {
    this.onLoad()
  },
  methods: {
    onLoad() {
      reviewService.getAllDisputes().then(r => {
        if (r) {
          this.disputesList = r
          this.disputesListCached = [...this.disputesList]
          this.loadList()
        }
      })
    },
    navigateToReview(e) {
      this.$router.push(e.reviewUrl)
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadList()
    },
    loadList() {
      this.listDisputesPerPage = this.disputesListCached.slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.total = this.disputesListCached.length
    }
  }
}
</script>

<style scoped>
.row-flex {
  display: flex;
  justify-content: center;
  margin: 25px 0;
}
h3{
    padding-left:25px
}
.pagination{
  margin: 20px;
  justify-content: center;
}
</style>
<style>
#reviews .el-table .cell {
    text-overflow: unset !important;
    word-break: break-word !important;

}
</style>
