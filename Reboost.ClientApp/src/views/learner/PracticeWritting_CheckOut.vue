<template>
  <el-dialog
    id="practiceWritingCheckoutContainer"
    :visible.sync="dlVisible"
    width="30%"
    :before-close="handleClose"
    @opened="dialogOpened"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 5px 20px"><i class="el-icon-shopping-cart-1" />Checkout</div>
      <div class="divider--horizontal divider" />
    </div>
    <div class="dialog-body">
      <div class="co-header">
        <img style="width:50px; height:50px; margin: 0 10px;" src="@/assets/img/checkout-icon-premium.png" alt="">
        <div class="title-container">
          <span>Writting preview</span>
          <span class="title-price">$35.00</span>
        </div>
      </div>
      <div class="co-form" :class="{ active: activeStep == 1 }">
        <div class="co-form__section" :class="{ inactive: activeStep == 2 }">
          <div class="co-form__title" style="padding-top: 10px">
            <span class="step-title" :class="{ active: activeStep == 1 }">1</span>
            <span style="padding: 0 10px; font-weight: 600;">Payment Method</span>
            <el-button v-if="activeStep == 2" style="position: absolute;right: 1rem;" size="mini" type="primary" @click="editCheckOutInfo">Edit</el-button>
          </div>
          <div class="saperator" style="padding-top: 10px" />
          <div class="co-form__content" :class="{ hidden: activeStep == 2 }" style="padding-top: 10px">
            <div class="section-container">
              <label size="mini"> Choose a Payment Method: </label>
              <div class="input-section">
                <el-select v-model="value" size="mini" placeholder="New Payment Method">
                  <el-option
                    v-for="item in options"
                    :key="item.value"
                    :label="item.label"
                    :value="item.value"
                  />
                </el-select>
              </div>
            </div>
            <div class="section-container">
              <label size="mini"> Please fill in your bill information: </label>
              <div style="display:flex; padding:0 0 10px 0; max-width:60%">
                <el-input v-model="firstName" style="padding-right:10px;" size="mini" placeholder="First Name" />
                <el-input v-model="lastName" size="mini" placeholder="Last Name" />
              </div>
              <el-input v-model="email" style="max-width:60%" :disabled="true" size="mini" placeholder="Please input" />
            </div>
            <div class="section-container">
              <label size="mini"> Please fill in your credit information: </label>
              <div class="checkout">
                <form id="payment-form">
                  <div id="card-element"><!--Stripe.js injects the Card Element--></div>
                  <p id="card-error" role="alert" />
                </form>
              </div>
            </div>
          </div>
          <div v-if="activeStep == 2" class="co-form__content" style="padding-top: 10px; padding-bottom: 10px">
            <div>{{ email }}</div>
            <div>{{ firstName }} {{ lastName }}</div>
            <div>{{ credit }}</div>
          </div>
        </div>
        <div v-if="activeStep == 1" class="saperator" style="padding-bottom: 10px" />
        <div class="co-form__section" :class="{ active: activeStep == 2, inactive: activeStep ==1 }">
          <div class="co-form__title" style="padding-bottom: 10px; padding-top: 10px">
            <span class="step-title" :class="{ active: activeStep == 2 }">2</span>
            <span style="padding: 0 10px; font-weight: 600;">Review and comfirm</span>
          </div>
          <div class="saperator" />
          <div v-if="activeStep == 2" class="co-form__content" style="padding-top: 10px; padding-bottom: 10px">
            <div style="margin-bottom: 10px">Add a promotion code</div>
            <div><el-input v-model="promotionCode" style="padding-right:10px; width: 200px" size="mini" placeholder="Enter Code" /></div>
          </div>
        </div>
      </div>

    </div>
    <div slot="footer" class="dialog-footer">
      <el-button v-if="activeStep == 1" size="mini" type="primary" class="co-button" @click="nextStep()">Continue</el-button>
      <el-button v-if="activeStep == 2" :disabled="checkoutDisable" size="mini" type="warning" class="co-button checkout" @click="completeCheckout()">Checkout</el-button>
    </div>
  </el-dialog>

</template>

<script>
// @ is an alias to /src
// import http from '@/utils/axios'
import { loadStripe } from '@stripe/stripe-js'
import paymentService from '../../services/payment.service'
export default {
  name: 'Checkout',
  props: { visible: { type: Boolean, default: true }},
  data() {
    return {
      dlVisible: false,
      options: [{
        value: 'Option1',
        label: 'Option1'
      }, {
        value: 'Option2',
        label: 'Option2'
      }, {
        value: 'Option3',
        label: 'Option3'
      }],
      value: '',
      firstName: '',
      lastName: '',
      email: null,
      credit: '**** **** **** 1234',
      promotionCode: '',
      activeStep: 1,
      card: null,
      stripe: null,
      clientSecret: null,
      checkoutDisable: true
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    }
  },
  watch: {
    visible: function(newVal, oldVal) { // watch it
      this.dlVisible = newVal
    }
  },
  async created() {

  },
  async mounted() {
    this.email = this.currentUser.email
    paymentService.getIntent().then(rs => {
      console.log('Get Intent result', rs)
      this.clientSecret = rs.clientSecret
    })
  },
  methods: {
    async dialogOpened() {
      this.stripe = await loadStripe('pk_test_51I9tu1D04tWYlOu2cVzeLBsGuDK4aRvfkR4tJb18W20hxgtf4989r2JeSbuua653nGkzY6IlFU7JTPYKyqne64VP00CLSFo42c')
      var elements = this.stripe.elements()
      var style = {
        base: {
          color: '#32325d',
          fontFamily: 'Arial, sans-serif',
          fontSmoothing: 'antialiased',
          fontSize: '16px',
          '::placeholder': {
            color: '#32325d'
          }
        },
        invalid: {
          fontFamily: 'Arial, sans-serif',
          color: '#fa755a',
          iconColor: '#fa755a'
        }
      }
      this.card = elements.create('card', { style: style })
      this.card.mount('#card-element')
      this.card.on('change', (event) => {
      // Disable the Pay button if there are no card details in the Element
        this.checkoutDisable = !!event.error
        document.querySelector('#card-error').textContent = event.error ? event.error.message : ''
      })
    },
    handleClose(done) {
      this.$confirm('Are you sure to exit?')
        .then(_ => {
          done()
        })
        .catch(_ => {})
    },
    checkoutVisibles(e) {
      if (e == 'checkout') {
        this.checkoutVisible = true
      }
    },
    nextStep() {
      this.activeStep = this.activeStep == 1 ? 2 : 1
    },
    completeCheckout() {
      console.log('ClientSecret', this.clientSecret)
      this.stripe.confirmCardPayment(this.clientSecret, {
        payment_method: {
          card: this.card
        }
      })
        .then(function(result) {
          if (result.error) {
            // Show error to your customer
            console.error(result)
          } else {
            // The payment succeeded!
            console.log(result)
          }
        })
    },
    dialogClosed() {
      this.$emit('closed')
    },
    editCheckOutInfo() {
      this.activeStep = this.activeStep == 2 ? 1 : 2
    }
  }
}
</script>
<style scoped>
.checkout{

}
.hidden{
    display: none;
}
.dialog-body{
  overflow: hidden;
}
.co-header{
  width: 100%;
  padding: 10px 0 10px 0;
  display: flex;
  align-items: center;
  border-bottom: solid 1px rgb(187, 187, 187);
}
.co-form{
    background-color: #f1f1f1;
    display: block;
    margin-left: -5px;
    margin-right: -5px;
}
.co-form.active{
  box-shadow: inset 0px 0px 5px 0px #9a9a9a;
}
.co-form__title{
  display: flex;
  align-items: center;
}
.co-form__content{
  padding: 0px 25px;
}
.co-form__section.active{
  box-shadow: inset 0px 0px 5px 0px #9a9a9a;
}
.co-form__section.inactive{
  background-color: white;
}
.step-title{
    margin: 0 0 0 10px;
    padding: 5px 12px;
    border-radius: 50%;
    font-weight: 600;
    color: #969292;
    border: 1px solid rgb(210 210 210);
}
.step-title.active{
  color: white;
  background-color: rgb(53, 198, 255);
}
.divider--horizontal {
    display: block;
    height: 1px;
    width: 100%;
    background-color: rgb(202 203 205);
}
.saperator{
  height: 1px;
  border-bottom: 1px solid rgb(202 203 205);
}
.divider {
  margin: 10px 0 0 0;
  position: relative;
}
.title-price{
  color: #999;
  font-size: 12px;
}

.title-container{
padding: 0 10px;
width:100%;
display: flex;
justify-content: space-between;
}
.input-section{
  max-width: 65%;
}
.section-container{

}

input {
  border-radius: 6px;
  margin-bottom: 6px;
  padding: 12px;
  border: 1px solid rgba(50, 50, 93, 0.1);
  height: 44px;
  font-size: 16px;
  width: 100%;
  background: white;
}
#card-error {
  color: rgb(105, 115, 134);
  text-align: left;
  font-size: 13px;
  line-height: 17px;
  margin-top: 12px;
}
#card-element {
    border-radius: 4px;
    padding: 5px;
    border: 1px solid #DCDFE6;
    /* height: 44px; */
    width: 100%;
    background: white;
}
</style>
