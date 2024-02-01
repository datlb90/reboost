<template>
  <div id="samples" style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="24">
        <h3>{{ messageTranslates('adminSamples', 'samples') }}</h3>
        <el-main>
          <el-table
            :data="listSamplesPerPage"
            stripe
            border
            style="width: 100%"
            size="mini"
          >
            <el-table-column
              prop="id"
              :label="messageTranslates('adminSamples', 'id')"
              align="center"
              width="50"
            />
            <el-table-column
              prop="questionId"
              :label="messageTranslates('adminSamples', 'question')"
              align="center"
              width="80"
            />
            <el-table-column
              prop="bandScore"
              :label="messageTranslates('adminSamples', 'bandScore')"
              align="center"
              width="100"
            />
            <el-table-column
              prop="comment"
              :label="messageTranslates('adminSamples', 'comment')"
            >
              <template slot-scope="scope">
                <div v-html="scope.row.comment" />
              </template>
            </el-table-column>
            <el-table-column
              prop="sampleText"
              label="Sample"
            >
              <template slot-scope="scope">
                <div v-html="scope.row.sampleText" />
              </template>
            </el-table-column>
            <el-table-column
              :label="messageTranslates('adminSamples', 'action')"
              width="100"
            >
              <template slot-scope="scope">
                <!-- <el-button v-if="scope.row.status===sampleStatus.CONTRIBUTED" size="mini" @click="approveSample(scope.row.id)"> {{ messageTranslates('adminSamples', 'approve') }} </el-button> -->
                <el-button size="mini" type="danger" @click="deleteSample(scope.$index, scope.row.id)"> Delete </el-button>
              </template>
            </el-table-column>
          </el-table>
          <!-- <div class="pagination">
            <el-pagination
              background
              layout="prev, pager, next"
              :total="total"
              :page-size="pageSize"
              @current-change="handleCurrentChange"
            />
          </div> -->
        </el-main>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import { SAMPLE_STATUS } from '../../app.constant'
import sampleService from '../../services/sample.service'

export default {
  name: 'Samples',
  data() {
    return {
      samplesListCached: [],
      samplesList: [],
      listSamplesPerPage: [],
      pageSize: 10,
      total: 0,
      page: 1,
      sampleStatus: SAMPLE_STATUS
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    samples() {
      return this.$store.getters['question/getAllSample']
    }
  },
  mounted() {
    this.$store.dispatch('question/loadAllSamples').then(rs => {
      this.samples.sort((a, b) => b.id - a.id)
      this.samplesListCached = [...this.samples]

      console.log(this.samplesListCached)
      this.loadList()
    })
  },
  methods: {
    deleteSample(index, id) {
      this.$confirm('Are you sure you want to delete this sample?').then(() => {
        sampleService.deleteSampleById(id).then(rs => {
          if (rs) {
            this.samplesListCached.splice(index, 1)
            this.$notify.success({
              title: 'Success',
              message: 'Deleted successfully',
              type: 'success',
              duration: 2000
            })

            this.loadList()
          }
        })
      }).catch(() => {
      })
    },
    approveSample(id) {
      sampleService.approveSampleById(id).then(rs => {
        if (rs) {
          this.samplesListCached = this.samplesListCached.map(item => { return item.id === rs.id ? { ...item, status: rs.status } : item })

          this.$notify.success({
            title: 'Success',
            message: 'Approved successfully',
            type: 'success',
            duration: 2000
          })

          this.loadList()
        }
      })
    },
    loadList() {
      this.listSamplesPerPage = this.samplesListCached // .slice(this.pageSize * this.page - this.pageSize, this.pageSize * this.page)
      this.total = this.samplesListCached.length
    },
    handleCurrentChange(val) {
      this.page = val
      this.loadList()
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
#samples .el-table .cell {
    text-overflow: clip !important;
    word-break: break-word !important;
}
</style>
