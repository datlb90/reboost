<template>
  <div class="list-container" />
</template>
<script>
import paymentService from '../services/payment.service'
export default {
  name: 'IPN',
  data() {
    return {
      orderStatus: null,
      paymentSuccess: false,
      paymentError: false,
      feedbackAvailable: false,
      review: null,
      order: null
    }
  },
  async mounted() {
    // this.verifyVnPayStatus()
  },
  async created() {

  },
  methods: {
    async verifyVnPayStatus() {
      const orderId = this.getUrlParameter('vnp_TxnRef')
      if (orderId) {
        const model = {
          orderId: orderId,
          vnpAmount: this.getUrlParameter('vnp_Amount'),
          vnpayTranId: this.getUrlParameter('vnp_TransactionNo'),
          vnpResponseCode: this.getUrlParameter('vnp_ResponseCode'),
          vnpTransactionStatus: this.getUrlParameter('vnp_TransactionStatus'),
          vnpSecureHash: this.getUrlParameter('vnp_SecureHash'),
          queryString: decodeURIComponent(window.location.search.substring(1))
        }
        await paymentService.VnPayCallback(model)
      }
    },
    getUrlParameter(sParam) {
      const sPageURL = decodeURIComponent(window.location.search.substring(1))
      const sURLVariables = sPageURL.split('&')
      let sParameterName
      let i
      for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=')

        if (sParameterName[0] === sParam) {
          return sParameterName[1] === undefined ? true : sParameterName[1]
        }
      }
    }
  }
}
</script>
