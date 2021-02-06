import http from '@/utils/axios'

const paymentService = {
  getIntent(data) {
    console.log('amount: ', data)
    return http.post('/payment/intent', { amount: data }).then(rs => rs.data)
  },
  getAllMethodByCustomerId(userId) {
    return http.get(`/payment/method/list/${userId}`).then(rs => rs.data)
  },
  insertPayment(data) {
    return http.post('/payment/create', data).then(rs => rs.data)
  },
  getAllPaymentByUserId(userId) {
    return http.get(`/payment/${userId}`).then(rs => rs.data)
  },
  getAllProducts() {
    return http.get('/payment/products/list').then(rs => rs.data)
  },
  getAllPrices() {
    return http.get('/payment/prices/list').then(rs => rs.data)
  },
  attachMethod(data) {
    return http.post('/payment/method/attach', { methodId: data }).then(rs => rs.data)
  },
  subscribe(data) {
    return http.post('/payment/subscribe', { methodId: data.methodId, priceId: data.priceId }).then(rs => rs.data)
  },
  createAccount(userId) {
    return http.get(`/payment/account/create/${userId}`).then(rs => rs.data)
  },
  getStripeAccount(userId) {
    return http.get(`/payment/account/${userId}`).then(rs => rs.data)
  },
  transferMoney(data) {
    return http.post('/payment/transfer', data).then(rs => rs.data)
  },
  payout(data) {
    return http.post('/payment/payout', data).then(rs => rs.data)
  },
  transferHistories(userId) {
    // return http.get('/payment/transfer').then(rs => rs.data)
    return http.get('/payment/out/' + userId).then(rs => rs.data)
  },
  balance(customerId) {
    return http.get('/payment/balance/' + customerId).then(rs => rs.data)
  },
  loginLink(customerId) {
    console.log('VAO SERVICE')
    return http.get('/payment/loginLink/' + customerId)
  },
  getCustomerSubscriptions(customerId) {
    return http.get('/payment/subscribe/' + customerId).then(rs => rs.data)
  }
}
export default paymentService
