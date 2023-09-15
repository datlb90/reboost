<template>
  <el-dialog
    id="practiceWritingCheckoutContainer"
    :visible.sync="dlVisible"
    width="500px"
    height="820px"
    :before-close="handleClose"
    @opened="dialogOpened"
    @closed="dialogClosed"
  >
    <div slot="title">
      <div style="padding: 5px 20px; font-size: 18px; text-align:center;">Pro Review Checkout</div>
      <div class="divider--horizontal divider" />
    </div>
    <div class="dialog-body">
      <div class="co-header">
        <img style="width: 50px; height: 40px; margin-bottom: 2px; margin-left: 18px;" src="@/assets/img/checkout-icon-premium.png" alt="">
        <div class="title-container">
          <span>Writing Review Service</span>
          <span class="title-price">${{ total }}.00</span>
        </div>
      </div>
      <div style="padding: 2px 20px;">
        <span style="font-size: 12px; color: grey;">A certified rater will review your writing and provide feedback within 24 hours</span>
      </div>
      <div class="co-form" :class="{ active: activeStep == 1 }">
        <div class="co-form__section" :class="{ inactive: activeStep == 2 }">
          <div class="co-form__title" style="padding-top: 10px">
            <span style="font-weight: 600;"> Select Payment Method</span>
            <el-button v-if="activeStep == 2" style="position: absolute;right: 1rem;" size="mini" type="primary" @click="editCheckOutInfo">Edit</el-button>
          </div>

          <!-- <div class="saperator" style="padding-top: 5px" /> -->
          <!-- <div v-if="!subcribe" id="paypal-button" class="paypal-icon__btn" /> -->
          <div v-if="!paid" id="paypal-button-container" />
          <!-- <div style="display:flex; justify-content:center">
            <div v-if="subcribe" id="paypal-subscribe-button-container" style="width:80%; margin:5px" />
          </div> -->

          <!-- <paypal-buttons :on-approve="onApprove" :create-order="createOrder" :on-shipping-change="onShippingChange" :on-error="onError" :style-object="style" /> -->
        </div>

        <div v-if="activeStep == 1" class="saperator" />

      </div>

    </div>
    <div slot="footer" class="dialog-footer">
      <!-- <div class="co-form-footer__title">
        <div>
          <span>Item:</span>
          <span class="title-price title-price__footer">${{ total }}</span>
        </div>
        <div>
          <span>Estimate Tax:</span>
          <span class="title-price title-price__footer">$0.00</span>
        </div>
        <div class="divider--horizontal divider" />
        <div class="title-footer__total">
          <span>Order total: </span>
          <span class="title-price title-price__footer">${{ total }}</span>
        </div>
      </div>
      <div class="footer--btn_container">
        <el-button v-if="activeStep == 1" :disabled="checkoutDisable" size="mini" type="primary" class="co-button" @click="dialogClosed">Cancel</el-button>
        <el-button v-if="activeStep == 1" :disabled="checkoutDisable" size="mini" type="primary" class="co-button" @click="nextStep()">Continue</el-button>
        <el-button v-if="activeStep == 2" size="mini" type="warning" class="co-button checkout" @click="completeCheckout()">Checkout</el-button>
      </div> -->
    </div>
    <!-- <div class="payment__divider">OR</div> -->

  </el-dialog>

</template>

<!-- <script src="https://www.paypal.com/sdk/js?client-id=Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I"></script> -->

<script>
// const PayPalButton = window.paypal.Buttons.driver('vue', window.Vue)
// @ is an alias to /src
// import http from '@/utils/axios'
// import { loadStripe } from '@stripe/stripe-js'
import { loadScript } from '@paypal/paypal-js'
import paymentService from '../../services/payment.service'
import { REVIEW_REQUEST_STATUS, SUBSCRIPTION_PLANS } from '../../app.constant'
import reviewService from '../../services/review.service'
import * as moment from 'moment'
import { openPayment } from '../../assets/epay/js/paymentClient'
// import { configs } from '../../app.constant'
export default {
  name: 'Checkout',
  // components: {
  //   'paypal-buttons': PayPalButton
  // },
  props: { visible: { type: Boolean, default: true },
    subcribe: { type: Object, default: null },
    submissionId: { type: Number, default: null },
    questionId: { type: Number, default: null }
  },
  data() {
    return {
      dlVisible: false,
      value: '0',
      firstName: '',
      lastName: '',
      email: null,
      credit: '**** **** **** 1234',
      promotionCode: '',
      activeStep: 1,
      card: null,
      stripe: null,
      clientSecret: null,
      checkoutDisable: true,
      newMethod: false,
      selectedMethod: null,
      amount: 15.00,
      paymentIntent: null,
      total: null,
      epaySelected: false,
      domain: 'https://sandbox.megapay.vn:2810',
      timeStamp: '20211209150000',
      merTrxId: 'EPAY000001301190',
      merId: 'EPAY000001',
      amountVND: '200000',
      encodeKey: 'rf8whwaejNhJiQG2bsFubSzccfRc/iRYyGUn6SPmT6y/L7A2XABbu9y4GvCoSTOTpvJykFi6b1G0crU8et2O0Q==',
      merchantToken: '',
      paid: false
    }
  },
  computed: {
    currentUser() {
      return this.$store.getters['auth/getUser']
    },
    currentMethods() {
      return this.$store.getters['payment/getAllPaymentMethods']
    },
    currentPrices() {
      return this.$store.getters['payment/getAllPrices']
    }
    // createOrder: function() {
    //   return (data) => {
    //     console.log(data)
    //     // Order is created on the server and the order id is returned
    //     // return fetch("/my-server/create-paypal-order", {
    //     //   method: "POST",
    //     //   headers: {
    //     //     "Content-Type": "application/json",
    //     //   },
    //     //   // use the "body" param to optionally pass additional order information
    //     //   // like product skus and quantities
    //     //   body: JSON.stringify({
    //     //     cart: [
    //     //       {
    //     //         sku: "YOUR_PRODUCT_STOCK_KEEPING_UNIT",
    //     //         quantity: "YOUR_PRODUCT_QUANTITY",
    //     //       },
    //     //     ],
    //     //   }),
    //     // })
    //     // .then((response) => response.json())
    //     // .then((order) => order.id);
    //   }
    // },
    // onApprove: function() {
    //   return (data) => {
    //     console.log(data)
    //     // Order is captured on the server
    //     // return fetch("/my-server/capture-paypal-order", {
    //     //   method: "POST",
    //     //   headers: {
    //     //     "Content-Type": "application/json",
    //     //   },
    //     //   body: JSON.stringify({
    //     //     orderID: data.orderID
    //     //   })
    //     // })
    //     // .then((response) => response.json());
    //   }
    // },
    // onShippingChange: function() {
    //   return (data, actions) => {
    //     if (data.shipping_address.country_code !== 'US') {
    //       return actions.reject()
    //     }
    //     return actions.resolve()
    //   }
    // },
    // onError: function() {
    //   return (err) => {
    //     console.error(err)
    //     window.location.href = '/404'
    //   }
    // },
    // style: function() {
    //   return {
    //     shape: 'pill',
    //     color: 'gold',
    //     layout: 'horizontal',
    //     label: 'paypal',
    //     tagline: false
    //   }
    // }
  },
  watch: {
    visible: function(newVal, oldVal) {
      this.dlVisible = newVal
    },
    currentMethods: function(val) {
      if (!val || val.length == 0) {
        this.changeMethod('0')
      } else {
        console.log('CURRENT MENOTH CHANGED', val[0].id)
        this.changeMethod(val[0].id)
        this.value = val[0].id
      }
    }
  },
  beforeCreate() {
    // const jquery = document.createElement('script')
    // jquery.src = 'https://code.jquery.com/jquery-3.6.0.min.js'
    // document.head.appendChild(jquery)

    // const script = document.createElement('script')
    // script.src = 'https://www.paypalobjects.com/api/checkout.js'
    // document.body.appendChild(script)

    // const epayScript = document.createElement('script')
    // epayScript.src = 'https://sandbox.megapay.vn:2810/pg_was/js/payment/layer/paymentClient.js'
    // document.head.appendChild(epayScript)

    // const sub_script = document.createElement('script')
    // sub_script.src = 'https://www.paypal.com/sdk/js?client-id=Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I'
    // sub_script.setAttribute('data-namespace', 'paypal_sdk')
    // document.body.appendChild(sub_script)
  },
  async mounted() {
    this.email = this.currentUser.email
    if (this.currentUser.stripeCustomerId) {
      this.$store.dispatch('payment/loadPaymentMethods', this.currentUser.stripeCustomerId)
    } else {
      this.$store.dispatch('payment/loadPaymentMethods', null)
    }
  },
  async created() {
    this.merchantToken = this.sha256(this.timeStamp + this.merTrxId + this.merId + this.amountVND + this.encodeKey)
    console.log(this.merchantToken)
  },
  methods: {
    createOrder: function(data, actions) {
      console.log('Creating order...')
      return actions.order.create({
        purchase_units: [
          {
            amount: {
              value: 15.00
            }
          }
        ]
      })
    },
    onApprove: function(data, actions) {
      console.log('Order approved...')
      return actions.order.capture().then(order => {
        console.log(order)
        const purchaseUnits = order.purchase_units
        if (purchaseUnits.length > 0) {
          const captures = purchaseUnits[0].payments.captures
          if (captures.length > 0) {
            this.paid = true
            this.createPaymentHistory(captures[0].id)
            this.paymentSuccessed()
          }
        }
      })
    },
    submitForm() {
      openPayment(1, this.domain)
    },
    sha256(s) {
      var chrsz = 8
      var hexcase = 0

      function safe_add(x, y) {
        var lsw = (x & 0xFFFF) + (y & 0xFFFF)
        var msw = (x >> 16) + (y >> 16) + (lsw >> 16)
        return (msw << 16) | (lsw & 0xFFFF)
      }

      function S(X, n) { return (X >>> n) | (X << (32 - n)) }
      function R(X, n) { return (X >>> n) }
      function Ch(x, y, z) { return ((x & y) ^ ((~x) & z)) }
      function Maj(x, y, z) { return ((x & y) ^ (x & z) ^ (y & z)) }
      function Sigma0256(x) { return (S(x, 2) ^ S(x, 13) ^ S(x, 22)) }
      function Sigma1256(x) { return (S(x, 6) ^ S(x, 11) ^ S(x, 25)) }
      function Gamma0256(x) { return (S(x, 7) ^ S(x, 18) ^ R(x, 3)) }
      function Gamma1256(x) { return (S(x, 17) ^ S(x, 19) ^ R(x, 10)) }

      function core_sha256(m, l) {
        var K = [0x428A2F98, 0x71374491, 0xB5C0FBCF, 0xE9B5DBA5, 0x3956C25B, 0x59F111F1, 0x923F82A4, 0xAB1C5ED5, 0xD807AA98,
          0x12835B01, 0x243185BE, 0x550C7DC3, 0x72BE5D74, 0x80DEB1FE, 0x9BDC06A7, 0xC19BF174, 0xE49B69C1, 0xEFBE4786, 0xFC19DC6,
          0x240CA1CC, 0x2DE92C6F, 0x4A7484AA, 0x5CB0A9DC, 0x76F988DA, 0x983E5152, 0xA831C66D, 0xB00327C8, 0xBF597FC7, 0xC6E00BF3,
          0xD5A79147, 0x6CA6351, 0x14292967, 0x27B70A85, 0x2E1B2138, 0x4D2C6DFC, 0x53380D13, 0x650A7354, 0x766A0ABB, 0x81C2C92E,
          0x92722C85, 0xA2BFE8A1, 0xA81A664B, 0xC24B8B70, 0xC76C51A3, 0xD192E819, 0xD6990624, 0xF40E3585, 0x106AA070, 0x19A4C116,
          0x1E376C08, 0x2748774C, 0x34B0BCB5, 0x391C0CB3, 0x4ED8AA4A, 0x5B9CCA4F, 0x682E6FF3, 0x748F82EE, 0x78A5636F, 0x84C87814,
          0x8CC70208, 0x90BEFFFA, 0xA4506CEB, 0xBEF9A3F7, 0xC67178F2]
        var HASH = [0x6A09E667, 0xBB67AE85, 0x3C6EF372, 0xA54FF53A, 0x510E527F, 0x9B05688C, 0x1F83D9AB, 0x5BE0CD19]
        var W = new Array(64)
        var a, b, c, d, e, f, g, h //, i, j
        var T1, T2

        m[l >> 5] |= 0x80 << (24 - l % 32)
        m[((l + 64 >> 9) << 4) + 15] = l

        for (var x = 0; x < m.length; x += 16) {
          a = HASH[0]
          b = HASH[1]
          c = HASH[2]
          d = HASH[3]
          e = HASH[4]
          f = HASH[5]
          g = HASH[6]
          h = HASH[7]

          for (var y = 0; y < 64; y++) {
            if (y < 16) W[y] = m[y + x]
            else W[y] = safe_add(safe_add(safe_add(Gamma1256(W[y - 2]), W[y - 7]), Gamma0256(W[y - 15])), W[y - 16])

            T1 = safe_add(safe_add(safe_add(safe_add(h, Sigma1256(e)), Ch(e, f, g)), K[y]), W[y])
            T2 = safe_add(Sigma0256(a), Maj(a, b, c))

            h = g
            g = f
            f = e
            e = safe_add(d, T1)
            d = c
            c = b
            b = a
            a = safe_add(T1, T2)
          }

          HASH[0] = safe_add(a, HASH[0])
          HASH[1] = safe_add(b, HASH[1])
          HASH[2] = safe_add(c, HASH[2])
          HASH[3] = safe_add(d, HASH[3])
          HASH[4] = safe_add(e, HASH[4])
          HASH[5] = safe_add(f, HASH[5])
          HASH[6] = safe_add(g, HASH[6])
          HASH[7] = safe_add(h, HASH[7])
        }
        return HASH
      }

      function str2binb(str) {
        var bin = []
        var mask = (1 << chrsz) - 1
        for (var i = 0; i < str.length * chrsz; i += chrsz) {
          bin[i >> 5] |= (str.charCodeAt(i / chrsz) & mask) << (24 - i % 32)
        }
        return bin
      }

      function Utf8Encode(string) {
        string = string.replace(/\r\n/g, '\n')
        var utftext = ''

        for (var n = 0; n < string.length; n++) {
          var c = string.charCodeAt(n)

          if (c < 128) {
            utftext += String.fromCharCode(c)
          } else if ((c > 127) && (c < 2048)) {
            utftext += String.fromCharCode((c >> 6) | 192)
            utftext += String.fromCharCode((c & 63) | 128)
          } else {
            utftext += String.fromCharCode((c >> 12) | 224)
            utftext += String.fromCharCode(((c >> 6) & 63) | 128)
            utftext += String.fromCharCode((c & 63) | 128)
          }
        }

        return utftext
      }

      function binb2hex(binarray) {
        var hex_tab = hexcase ? '0123456789ABCDEF' : '0123456789abcdef'
        var str = ''
        for (var i = 0; i < binarray.length * 4; i++) {
          str += hex_tab.charAt((binarray[i >> 2] >> ((3 - i % 4) * 8 + 4)) & 0xF) +
 hex_tab.charAt((binarray[i >> 2] >> ((3 - i % 4) * 8)) & 0xF)
        }
        return str
      }

      s = Utf8Encode(s)
      return binb2hex(core_sha256(str2binb(s), s.length * chrsz))
    },

    async dialogOpened() {
      if (this.subcribe) {
        this.amount = this.subcribe.price
      }
      this.total = this.amount

      loadScript({ 'client-id': 'Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I' }).then((paypal) => {
        paypal
          .Buttons({
            createOrder: this.createOrder,
            onApprove: this.onApprove
          })
          .render('#paypal-button-container')
      })

      // paymentService.getIntent(this.amount).then(rs => {
      //   this.paymentIntent = rs
      //   this.clientSecret = rs.client_secret
      //   console.log('this.paymentIntent', this.paymentIntent)
      // })

      // this.stripe = await loadStripe(configs.stripeApiKey)
      // var elements = this.stripe.elements()
      // var style = {
      //   base: {
      //     color: '#32325d',
      //     fontFamily: 'Arial, sans-serif',
      //     fontSmoothing: 'antialiased',
      //     fontSize: '16px',
      //     '::placeholder': {
      //       color: '#32325d'
      //     }
      //   },
      //   invalid: {
      //     fontFamily: 'Arial, sans-serif',
      //     color: '#fa755a',
      //     iconColor: '#fa755a'
      //   }
      // }
      // this.card = elements.create('card', { style: style })
      // this.card.mount('#card-element')
      // this.card.on('change', (event) => {
      // // Disable the Pay button if there are no card details in the Element
      //   this.checkoutDisable = !!event.error
      //   document.querySelector('#card-error').textContent = event.error ? event.error.message : ''
      // })

      // this.loadPaypalBtn()
    },
    handleClose(done) {
      this.$confirm('Are you sure to exit?')
        .then(_ => {
          done()
          const paypalContaner = document.getElementById('paypal-button-container')
          paypalContaner.innerHTML = ''
        })
        .catch(_ => {})
    },
    checkoutVisibles(e) {
      if (e == 'checkout') {
        this.checkoutVisible = true
      }
    },
    nextStep() {
      if (this.newMethod) {
        this.stripe.createPaymentMethod({
          type: 'card',
          card: this.card,
          billing_details: {
            name: this.firstName + ' ' + this.lastName
          }
        })
          .then(rs => {
            console.log('create payment method result', rs)
            this.selectedMethod = rs.paymentMethod
            this.lastName = this.selectedMethod.billing_details.name
            this.credit = '**** **** **** ' + this.selectedMethod.card.last4
            this.activeStep = this.activeStep == 1 ? 2 : 1
          })
      } else {
        this.activeStep = this.activeStep == 1 ? 2 : 1
      }
    },
    completeCheckout() {
      // var paymentMethod = null
      if (this.newMethod) {
        // paymentMethod = {
        //   card: this.card,
        //   billing_details: {
        //     name: this.firstName + ' ' + this.lastName
        //   }
        // }
        // this.stripe.createPaymentMethod({
        //   type: 'card',
        //   card: this.card,
        //   billing_details: {
        //     name: this.firstName + ' ' + this.lastName
        //   }
        // })
        //   .then(result => {

        //   })

        // console.log('create payment method result', result)
        paymentService.attachMethod(this.selectedMethod.id).then(rs => {
          // paymentMethod = rs.id
          console.log('attach pm result', rs)
          this.selectedMethod = rs

          if (this.subcribe == null) {
            this.confirmPayment(this.selectedMethod.id)
          } else {
            this.$emit('subscribed', this.selectedMethod)
          }
        })
      } else {
        // paymentMethod = this.selectedMethod.id
        if (this.subcribe == null) {
          this.confirmPayment(this.selectedMethod.id)
          this.clearSection()
        } else {
          this.$emit('subscribed', this.selectedMethod)
          this.clearSection()
        }
      }
    },
    confirmPayment(paymentMethod) {
      this.stripe.confirmCardPayment(this.clientSecret, {
        payment_method: paymentMethod
      })
        .then((result) => {
          if (result.error) {
            console.error(result)
            this.$notify.error({
              title: 'Error',
              message: result.error.message,
              type: 'error',
              duration: 5000
            })
          } else {
            // The payment succeeded!
            paymentService.insertPayment({
              UserId: this.currentUser.id,
              PaymentId: result.paymentIntent.id,
              Description: result.paymentIntent.description ? result.paymentIntent.description : 'No Description',
              Amount: 35,
              Currency: result.paymentIntent.currency,
              Status: result.paymentIntent.status,
              PaymentDate: moment().format('YYYY-MM-DD') })
            this.clearSection()
          }
        })
    },
    clearSection() {
      this.firstName = ''
      this.lastName = ''
      this.credit = '**** **** **** 1234'
      this.promotionCode = ''
      this.activeStep = 1
      this.card = null
      this.stripe = null
      this.clientSecret = null
      this.newMethod = false
      this.selectedMethod = null
    },
    dialogClosed() {
      this.$emit('closed')
    },
    editCheckOutInfo() {
      this.activeStep = this.activeStep == 2 ? 1 : 2
    },
    changeMethod(e) {
      if (+e == 0) {
        this.newMethod = true
        this.checkoutDisable = true
        this.selectedMethod = null
      } else {
        this.newMethod = false
        this.checkoutDisable = false
        this.selectedMethod = this.currentMethods.find(m => m.id == e)
        this.lastName = this.selectedMethod.billing_details.name
        this.credit = '**** **** **** ' + this.selectedMethod.card.last4
      }
    },
    loadPaypalBtn() {
      console.log(window.paypal)
      if (document.getElementById('paypal-button') && !document.getElementById('paypal-button').childNodes.length > 0) {
        if (!this.subcribe) {
          window.paypal.Button.render({
            // Configure environment
            env: 'sandbox',
            client: {
              sandbox: 'Aa5nScHY-XCvNuR8KYRHA4BySu_7-91JTnBfvZ0vj9Zto8107b40nHn8-vGADPJBx5XAJS_IHL_WWK3I',
              production: 'demo_production_client_id'
            },
            // Customize button (optional)
            locale: 'en_US',
            style: {
              shape: 'pill',
              color: 'gold',
              layout: 'horizontal',
              label: 'paypal',
              tagline: false
            },

            // Enable Pay Now checkout flow (optional)
            commit: true,

            // Set up a payment
            payment: function(data, actions) {
              return actions.payment.create({
                transactions: [{
                  amount: {
                    total: '90.50',
                    currency: 'USD'
                  }
                }]
              })
            },
            // Execute the payment
            onAuthorize: e => {
              this.createPaymentHistory(e.paymentID)
              this.paymentSuccessed()
            }
          }, '#paypal-button')
        }
      }
      if (SUBSCRIPTION_PLANS.filter(r => r.name === this.subcribe?.name).length > 0) {
        const btnExisted = document.getElementById('paypal-subscribe-button-container')

        if (btnExisted.childNodes.length > 0) {
          btnExisted.childNodes.forEach(e => {
            e.remove()
          })
        }

        setTimeout(() => {
          if (this.subcribe.id === 1) {
            this.createYearSubcribePaypalBtn()
          } else {
            this.createMonthSubcribePaypalBtn()
          }
        }, 50)
      }
    },
    createYearSubcribePaypalBtn() {
      window.paypal_sdk.Buttons({
        style: {
          shape: 'pill',
          color: 'gold',
          layout: 'horizontal',
          label: 'subscribe'
        },

        createSubscription: function(data, actions) {
          return actions.subscription.create({
            // Pass a const to plan_id, can not use variable
            'plan_id': 'P-6N9586386D9350704MFFTCIQ'
          })
        },

        onApprove: (data, actions) => {
          this.subscribed(data)
        }
      }).render('#paypal-subscribe-button-container')
    },
    createMonthSubcribePaypalBtn() {
      window.paypal_sdk.Buttons({
        style: {
          shape: 'pill',
          color: 'gold',
          layout: 'horizontal',
          label: 'subscribe'
        },

        createSubscription: function(data, actions) {
          return actions.subscription.create({
            // Pass a const to plan_id, can not use variable
            'plan_id': 'P-2XC42867D76575918MFFP53Q'
          })
        },

        onApprove: (data, actions) => {
          this.subscribed(data)
        }
      }).render('#paypal-subscribe-button-container')
    },
    subscribed(data) {
      if (this.subcribe.name === 'month') {
        const postData = {
          MonthSubs: data.subscriptionID
        }
        paymentService.createUpdateLearnerSubscriptions(postData).then(rs => {
          this.$notify.success({
            title: 'Subcribe Successed!',
            message: 'Successed!',
            type: 'success',
            duration: 1500
          })
        })
      }
      if (this.subcribe.name === 'year') {
        const postData = {
          YearSubs: data.subscriptionID
        }
        paymentService.createUpdateLearnerSubscriptions(postData).then(rs => {
          this.$notify.success({
            title: 'Subcribe Successed!',
            message: 'Successed!',
            type: 'success',
            duration: 1500
          })
        })
      }
    },
    paymentSuccessed() {
      this.$notify.success({
        title: 'Payment Success',
        message: 'Your payment for the writing service was processed successfully!',
        type: 'success',
        duration: 1500
      })
      this.requestProReview()
    },
    requestProReview() {
      reviewService.createProReviewRequest({
        UserId: this.currentUser.id,
        SubmissionId: +this.submissionId,
        FeedbackType: 'Pro',
        Status: REVIEW_REQUEST_STATUS.WAITING
      }).then(rs => {
        this.dialogClosed()
        this.$notify.success({
          title: 'Pro Review Request',
          message: 'The pro review request has been successfully submitted!',
          type: 'success',
          duration: 1500
        })
        this.$emit('proReviewRequested')
      })
    },
    createPaymentHistory(transactionId) {
      const data = {
        UserId: this.currentUser.id,
        QuestionId: this.questionId,
        PaypalTransactionId: transactionId,
        Amount: '15.00'
      }
      paymentService.createPaymentHistory(data).then(rs => {
        console.log('payment history created: ', rs)
      })
    }
  }
}
</script>

<style scoped src="@/assets/epay/css/paymentClient.css">
</style>

<style scoped>
#paypal-button-container{
  width: 90%;
  margin: auto;
  overflow: auto;
  max-height: 400px;
  margin-top: 10px;
}
.hidden{
    display: none;
}
.dialog-body{
  overflow: hidden;
}
.co-header{
  width: 100%;
  padding: 5px 0 5px 0;
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
  display: block;
  text-align: center;
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
  font-size: 14px;
}
.title-container{
  margin-right: 10px;
  padding: 0 10px;
  width:100%;
  display: flex;
  justify-content: space-between;
}
.input-section{
  max-width: 65%;
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
.dialog-footer{
  align-items: center;
  margin-bottom: 20px;
}
.co-form-footer__title{
  position: relative;
  text-align: left;
  width: 100%;
}
.title-price__footer{
    position: absolute;
    right: 1px;
}
.title-footer__total{
  color: red ;
  font-size: 16px;
}
.title-footer__total .title-price{
  color: red;
  font-size: 13px;
}
.paypal-icon__btn{
  margin: 15px 0 0 0;
  text-align: center;
}
.payment__divider{
  margin-top: 10px;
  text-align: center;
}
.footer--btn_container{
  display: flex;
  justify-content: flex-end;
  margin-top: 15px;
}
</style>
