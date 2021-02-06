<template>
  <div>
    <div class="banner">
      <div class="banner__header">
        <img src="https://i.pinimg.com/originals/2a/5b/de/2a5bdecf7c05a386b03a99dd14ebec0e.png" alt="Reboost Logo">
        <h1 class="text-center">Get started with a LeetCode subscription that works for you.</h1>
      </div>
      <div class="banner__quote">
        <p>"LeetCode is recommended by many interviewees and leading tech company recruiters, including"</p>
      </div>
      <div class="banner__carousel">
        <el-carousel indicator-position="outside" height="127px">
          <el-carousel-item v-for="item in 4" :key="item" style="height: 100px">
            <h3>{{ item }}</h3>
          </el-carousel-item>
        </el-carousel>
      </div>
    </div>
    <div class="banner">
      <el-row :gutter="10" class="plan-con">
        <el-col v-for="item in currentProducts" :key="item.id" class="row">
          <div class="plan-selection col-md-12">
            <div style="width:80%">
              <div class="price-panel" :class="currentPrices.filter(r => r.product == item.id)[0].recurring.interval.toLowerCase() == 'year' ? 'yearly' : 'monthly'">
                <div class="header">
                  <p class="title">{{ item.name }}</p>
                  <p class="sub-title">Subscription</p>
                  <p class="price">${{ formatPrice(item.id) }}
                    <span class="text">/{{ formatInterval(item.id) }}</span>
                  </p>
                </div>  <!-- End of header -->
                <div class="body">
                  <div>
                    {{ item.description }}
                  </div>
                  <small class="currency-notice">(prices are marked in USD)</small>
                </div>  <!-- End of body -->
                <div class="footer">
                  <button v-if="!subscribedPlans.find(p => p.product == item.id)" type="button" class="button button-default subscribe-button" @click="planSelected(item.id)">Subscribe</button>
                  <button v-else type="button" class="button button-default subscribe-button" disabled="true">Subscribed</button>
                </div>  <!-- End of footer -->
              </div>
            </div>
          </div>
        </el-col>

      </el-row>
    </div>
    <div>
      <checkout :subcribe="priceId" :visible="checkoutVisible" @closed="checkoutVisible=false" @subscribed="subcribe($event)" />
    </div>
  </div>
</template>

<script>
// import { loadStripe } from '@stripe/stripe-js'
import paymentService from '../../services/payment.service'
import CheckOut from '../../components/controls/CheckOut.vue'
export default {
  name: 'Subscribe',
  components: {
    'checkout': CheckOut
  },
  props: { visible: { type: Boolean, default: true }},
  data() {
    return {
      productsList: [],
      pricesList: [],
      checkoutVisible: false,
      priceId: '',
      subscribedPlans: []
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    currentProducts() {
      return this.$store.getters['payment/getAllProducts']
    },
    currentPrices() {
      return this.$store.getters['payment/getAllPrices']
    }
  },
  watch: {
    visible: function(newVal, oldVal) { // watch it
      this.dlVisible = newVal
    }
  },
  async mounted() {
    this.$store.dispatch('payment/loadProducts')
    this.$store.dispatch('payment/loadPrices')
    paymentService.getCustomerSubscriptions(this.currentUser.stripeCustomerId).then(rs => {
      console.log('USER SUBSCRIPTION FULL', rs)
      this.subscribedPlans = rs.data.map(s => s.items.data[0].plan)
      console.log('USER SUBSCRIPTION', this.subscribedPlans)
    })
  },
  methods: {
    formatPrice(id) {
      var value = this.currentPrices.filter(r => r.product == id)[0].unit_amount / 100
      const val = (value / 1).toFixed(2).replace('.', ',')
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.')
    },
    formatInterval(id) {
      var value = this.currentPrices.filter(r => r.product == id)[0].recurring.interval.toLowerCase()
      return value == 'year' ? 'yr' : 'mo'
    },
    planSelected(id) {
      console.log(this.currentPrices.filter(r => r.product == id)[0].id)
      this.priceId = this.currentPrices.filter(r => r.product == id)[0].id
      this.checkoutVisible = true
    },
    subcribe(e) {
      console.log('subcribe:', e)
      paymentService.subscribe({ methodId: e.id, priceId: this.priceId }).then(s => {
        console.log('Subscription: ', s)
        this.priceId = ''
      })
    }
  }
}
</script>
<style scoped>
.banner {
  padding: 0 120px;
  margin-top: 20px;
  text-align: center;
}
img {
    width: 150px;
    height: 150px;
    vertical-align: middle;
    border-style: none;
}
.banner h1 {
    font-weight: 200;
}
.banner__carousel {
    position: relative;
    height: 200px;
}
.el-carousel__item h3 {
    color: #475669;
    font-size: 14px;
    opacity: 0.75;
    line-height: 100px;
    margin: 0;
}
.el-carousel__container {
    position: relative;
    height: 100px;
}
.el-carousel__item:nth-child(2n) {
    background-color: #99a9bf;
}
.el-carousel__item:nth-child(2n+1) {
    background-color: #d3dce6;
}
.monthly {
  background-color: #fea116;
}
.title {
  color: white;
}
.price-panel .header {
    border-top-right-radius: 5px;
    border-top-left-radius: 5px;
    background-color: #fea116;
    padding-left: 30px;
    padding-right: 30px;
    position: relative;
    text-align: justify;
}
.price-panel {
    background: white;
    webkit-transition: box-shadow .4s;
    -webkit-transition: -webkit-box-shadow .4s;
    transition: -webkit-box-shadow .4s;
    transition: box-shadow .4s;
    transition: box-shadow .4s, -webkit-box-shadow .4s;
    transition: box-shadow .4s,-webkit-box-shadow .4s;
    border-radius: 5px;
    -webkit-box-shadow: 0 5px 15px 0 rgba(0,0,0,0.08);
    box-shadow: 0 5px 15px 0 rgba(0,0,0,0.08);
    width: 100%;
    min-height: 390px;
    margin: 0 0 30px;
}
.price-panel .body {
    padding-top: 15px;
    margin-left: 30px;
    margin-right: 30px;
    font-weight: 300;
    font-size: 16px;
}
.price-panel .footer {
    margin-left: 30px;
    margin-right: 30px;
    padding-top: 25px;
    padding-bottom: 30px;
    position: relative;
}
.price-panel .header .title {
    color: white;
    font-size: 35px;
    font-weight: 300;
    padding-top: 15px;
}
.price-panel .header .sub-title {
    color: white;
    opacity: .6;
    font-size: 14px;
    font-weight: 300;
    margin-top: -15px;
    padding-bottom: 20px;
}
.plan-selection .left .price-panel .price {
    right: 80px;
}
.price-panel .header .price {
    position: absolute;
    right: 30px;
    color: white;
    font-weight: 300;
    font-size: 35px;
    height: 0;
    margin-top: -85px;
}
.price-panel .header .price .text {
    font-size: 14px;
    opacity: .6;
}
.price-panel .body .currency-notice {
    display: block;
    margin: 10px 0 0;
}
.yearly .header {
    background-color: black;
}
.plan-selection{
  display: flex;
  text-align: justify;
  justify-content: space-around;
  padding: 0 50px;
}
.price-panel .footer .subscribe-button:hover {
    border-color: #fea116;
    background: #fea116;
    color: white;
}
.price-panel .footer .subscribe-button {
    bottom: -10px;
    padding: 10px 20px 10px 20px;
    border-radius: 5px;
}
.button.button-default, .button.button-default:active, .button.button-default:active:focus, .button.button-default:focus {
    border: 1px solid #ddd;
}
.button{
  background-color: white;
}
.yearly .subscribe-button:hover {
    border-color: black !important;
    background: black !important;
}
.subscribe-button{
  position: absolute;
  bottom: 50px;
}
.plan-con{
  display: flex;
  justify-content: center;
}
@media only screen and (max-width: 1050px) {
  .plan-con{
    display: block !important;
  }
}
</style>
