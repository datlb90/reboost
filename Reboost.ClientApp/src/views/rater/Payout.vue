<template>
  <div style="margin-top:25px;">
    <el-row class="row-flex">
      <el-col :span="15">
        <el-main>
          <div class="con">
            <div class="con-user">
              <div class="con-user-profile">
                <div class="con-user-profile__photo" />
                <p>Rater until {{ dateFormat(currentUser.expireDate) }}</p>
                <h1>{{ currentUser.username }}</h1>
              </div>
              <div class="con-user-available block">
                <p>Your balance</p>
                {{ total }} $
              </div>
              <div class="con-user-payout block">
                <button v-if="paypalAccount" class="btn-payout" @click="payout"> Pay out now </button>
                <span v-if="!paypalAccount" id="paypal--connect__btn" />
              </div>
            </div>
            <div class="con-history">
              <div class="con-history__reviews">
                <div class="con-history__reviews__label">
                  Recent reviews
                </div>
              </div>
              <el-table
                :data="allBalances"
                style="width: 100%"
              >
                <el-table-column
                  prop="id"
                  label="#"
                  width="100"
                />
                <el-table-column
                  prop="reviewId"
                  label="Review Id"
                  width="200"
                />
                <el-table-column
                  prop="createdDate"
                  label="Created Date"
                  width="300"
                >
                  <template slot-scope="scope">
                    <span class="title-row cursor" style="word-break: break-word">{{ getTimeFromDateCreateToNow(scope.row.createdDate) }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="total"
                  label="Amount"
                />
                <el-table-column
                  prop="status"
                  label="Status"
                  width="200"
                />
              </el-table>
            </div>
          </div>
        </el-main>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import paymentService from '../../services/payment.service'
import moment from 'moment'
import raterService from '../../services/rater.service'
export default {
  name: 'StartRating',
  data() {
    return {
      disable: false,
      text: '',
      accountId: null,
      destination: null,
      loginUrl: '#',
      email: null,
      paypalAccount: null
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    balance() {
      return this.$store.getters['payment/getBalance']
    },
    allBalances() {
      return this.$store.getters['payment/getAllBalances']
    },
    total() {
      var total = this.balance.reduce((previousValue, currentValue) => previousValue + currentValue.total, 0)
      return total
    }
  },
  mounted() {
    this.loadPaypalBtn()
  },
  beforeCreate() {
    const papyaplScript = document.createElement('script')
    papyaplScript.src = 'https://www.paypalobjects.com/js/external/api.js'
    document.body.appendChild(papyaplScript)
  },
  beforeMount() {
    if (this.$router.currentRoute.query?.code) {
      this.connectPaypalAccountToRater(this.$router.currentRoute.query?.code).then(rs => {
        this.$router.push('/rater/payout')
        this.loadRaterPaypalAccount()
      })
    } else {
      this.$store.dispatch('payment/loadBalance')
      this.onLoad()
    }
  },
  methods: {
    onLoad() {
      this.$store.dispatch('payment/loadAllBalance').then(rs => {
      })

      if (this.$route.params.accountId) {
        this.accountId = this.$route.params.accountId
        this.$store.dispatch('payment/loadPaymentHistories', this.currentUser.id)
        paymentService.loginLink(this.accountId).then(rs => {
          this.loginUrl = rs.data.url
        })
      }
      this.loadRaterPaypalAccount()
    },
    loadRaterPaypalAccount() {
      raterService.getRaterPaypalAccount().then(rs => {
        this.paypalAccount = rs.paypalAccount
      })
    },
    loadPaypalBtn() {
      setTimeout(() => {
        window.paypal.use(['login'], function(login) {
          login.render({
            'appid': 'AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F',
            'authend': 'sandbox',
            'scopes': 'openid',
            'containerid': 'paypal--connect__btn',
            'responseType': 'code',
            'locale': 'en-us',
            'buttonType': 'LWP',
            'buttonShape': 'pill',
            'buttonSize': 'lg',
            'fullPage': 'true',
            'returnurl': 'https://reboost.azurewebsites.net/rater/payout'
          })
        })
      }, 500)
    },
    dateFormat(time) {
      return moment(new Date(time)).format('yyyy-MM-DD')
    },
    payout() {
      paymentService.paypalPayout().then(r => {
        this.$notify.success({
          title: 'Payout successed!',
          message: 'Payout successed!',
          type: 'success',
          duration: 5000
        })
        this.$store.dispatch('payment/loadBalance')
      })
    },
    getTimeFromDateCreateToNow(time) {
      return moment(new Date(time)).format('MMMM Do YYYY, hh:mm:ss a')
    },
    connectPaypalAccountToRater(e) {
      return new Promise(async(resolve, reject) => {
        const rs = await paymentService.getUserPaypalInfo(e)
        if (rs.data.emails.length > 0) {
          const email = rs.data.emails[0].value
          await raterService.updateRaterPaypalAccount(email)
          return resolve()
        }

        reject()
      })
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

.el-main{

}

.con-user {
    display: flex;
    width: 100%;
    padding: 40px 0;
}

.con-user .con-user-profile {
    flex: 1;
    padding-left: 65px;
    position: relative;
    color: #292f45;
}

.con-user .con-user-profile .con-user-profile__photo {
    position: absolute;
    width: 50px;
    height: 50px;
    top: 36%;
    left: 0;
    margin-top: -25px;
    border-radius: 50%;
    background-image: url(../../assets/img/avatar-large.svg);
    box-shadow: 0 2px 5px -1px rgb(50 50 93 / 25%), 0 1px 3px -1px rgb(0 0 0 / 30%);
}
.con-user h1 {
    font-size: 24px;
    font-weight: 400;
}
.con-user p {
    font-size: 13px;
    font-weight: 500;
    margin-bottom: 7px;
    color: #292f45;
}
.con-user h2 {
    color: #8898aa;
    font-size: 13px;
    font-weight: 500;
    margin-top: 8px;
}
.con-user a:hover {
    text-decoration: underline;
}
.block{
    margin-left: 40px;
    position: relative;
    color: #292f45;
}
.btn-payout{
    font-size: 16px;
    font-weight: 500;
    text-transform: unset;
    letter-spacing: 0;
    height: 43px;
    margin: 30px 0 0 0;
    margin: 2px 0 14px;
    min-width: 150px;
    text-align: center;
    background: #15b67c;
    color: white;
    padding: 0 22px;
    line-height: 44px;
    display: block;
    width: 100%;
    border-style: none;
    border-radius: 4px;
    box-shadow: 0 2px 5px -1px rgb(50 50 93 / 25%), 0 1px 3px -1px rgb(0 0 0 / 30%);
    cursor: pointer;
    outline: none;
}
.con-history__reviews{
    display: flex;
    margin-bottom: 14px;
    color: #292f45;
    font-weight: 500;
}
.con-history__reviews__label{
    flex:1
}
.con-history__reviews__button{
    flex:0.2
}
</style>
