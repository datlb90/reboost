import http from '@/utils/axios'
import Axios from 'axios'

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
    return http.get('/payment/loginLink/' + customerId)
  },
  getCustomerSubscriptions(customerId) {
    return http.get('/payment/subscribe/' + customerId).then(rs => rs.data)
  },
  getDefaultPaymentMethod(customerId) {
    return http.get('/payment/defaultPaymentMethod/' + customerId).then(rs => rs.data)
  },
  updateDefaultPaymentMethod(customerId, defaultPaymentMethodId) {
    return http.get('/payment/updateDefaultPaymentMethod/' + customerId + '/' + defaultPaymentMethodId)
  },
  getCustomerId() {
    return http.get('/payment/customerId').then(rs => rs.data)
  },
  paypalPayout() {
    return http.post(`/payment/paypal/payout`).then(rs => rs.data)
  },
  getRaterAvailableBalances() {
    return http.get(`/payment/balance/available`).then(rs => rs.data)
  },
  getAllBalances() {
    return http.get(`/payment/balance`).then(rs => rs.data)
  },

  // Get user's paypal refresh token through authentication code from returnUrl
  async getRefreshToken(authCode) {
    const result = await Axios({
      url: 'https://api-m.sandbox.paypal.com/v1/oauth2/token',
      method: 'post',
      headers: {
        'Content-Type': 'x-www-form-urlencoded'
      },
      // For Basic Authorization (curl -u), set via auth:
      auth: {
        username: 'AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F',
        password: 'EOyXbN3RRGLv6S5y9PwMIiIpIsMjsBhH2FcO1kxpZH8PT69O8JtEza67L43XO19os0mNpjvFCzqCxVYV'
      },
      // This will urlencode the data correctly:
      data: new URLSearchParams({
        grant_type: 'authorization_code',
        code: authCode
      })
    })
    return result
  },

  // Get user's paypal access token (1 times using) through refresh token
  async getAccessToken(refresh_token) {
    const result = await Axios({
      url: 'https://api-m.sandbox.paypal.com/v1/oauth2/token',
      method: 'post',
      auth: {
        username: 'AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F',
        password: 'EOyXbN3RRGLv6S5y9PwMIiIpIsMjsBhH2FcO1kxpZH8PT69O8JtEza67L43XO19os0mNpjvFCzqCxVYV'
      },
      data: new URLSearchParams({
        grant_type: 'refresh_token',
        refresh_token: refresh_token
      })
    })
    return result
  },

  // Get user's paypal account info base on authentication code from returnUrl after login or signUp success
  async getUserPaypalInfo(authCode) {
    const refresh_token = await this.getRefreshToken(authCode)
    const access_token = await this.getAccessToken(refresh_token.data.refresh_token)
    const userInfo = await Axios({
      url: 'https://api-m.sandbox.paypal.com/v1/identity/oauth2/userinfo?schema=paypalv1.1',
      method: 'get',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + access_token.data.access_token
      }
    })
    return userInfo
  },

  createPaymentHistory(data) {
    return http.post('/payment/paypal/paymentHistory', data).then(rs => rs.data)
  },

  async getPaypalSubscribeInfo(subId) {
    const result = await Axios({
      url: `https://api-m.sandbox.paypal.com/v1/billing/plans/${subId}`,
      method: 'get',
      auth: {
        username: 'AamTDpWxcBAOJ4twRr0E_XVy1z2uJ3AxTU9BejLB0sJCM3Om2RuApDwSZce6Lterg8aaNl-XWIyxw__F',
        password: 'EOyXbN3RRGLv6S5y9PwMIiIpIsMjsBhH2FcO1kxpZH8PT69O8JtEza67L43XO19os0mNpjvFCzqCxVYV'
      },
      headers: {
        'Content-Type': 'application/json'
      }
    })
    return result
  },

  getLearnerSubscriptions() {
    return http.get('/payment/paypal/subscribe').then(rs => rs.data)
  },

  createUpdateLearnerSubscriptions(data) {
    return http.get('/payment/paypal/subscribe', data).then(rs => rs.data)
  }

}
export default paymentService
