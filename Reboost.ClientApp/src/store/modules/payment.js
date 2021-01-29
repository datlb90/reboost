import paymentService from '../../services/payment.service'

const state = {
  payments: [],
  paymentMethods: [],
  products: [],
  prices: []
}

const actions = {
  loadPayments({ commit }) {
    return paymentService.getAll().then(result => {
      console.log('Action load payment', result)
      commit('SET_PAYMENT', result)
    })
  },
  loadPaymentMethods({ commit }, id) {
    if (!id) {
      console.log('user do not have a customerId')
      commit('SET_PAYMENTMETHOD', null)
      return null
    }
    return paymentService.getAllMethodByCustomerId(id).then(rs => {
      console.log('Action load payment methods: ', rs)
      commit('SET_PAYMENTMETHOD', rs)
    })
  },
  loadProducts({ commit }) {
    return paymentService.getAllProducts().then(rs => {
      console.log('Action load all products', rs)
      commit('SET_PRODUCTS', rs)
    })
  },
  loadPrices({ commit }) {
    return paymentService.getAllPrices().then(rs => {
      console.log('Action load all prices', rs)
      commit('SET_PRICES', rs)
    })
  }
}

const mutations = {
  SET_PAYMENT: (state, payment) => {
    state.payments = payment
  },
  SET_PAYMENTMETHOD: (state, paymentMethods) => {
    state.paymentMethods = paymentMethods
  },
  SET_PRODUCTS: (state, products) => {
    state.products = products
  },
  SET_PRICES: (state, prices) => {
    state.prices = prices
  }
}

const getters = {
  getAllPayments: state => state.payments,
  getAllPaymentMethods: state => state.paymentMethods,
  getAllProducts: state => state.products,
  getAllPrices: state => state.prices
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
