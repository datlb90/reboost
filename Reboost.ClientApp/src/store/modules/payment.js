import paymentService from '../../services/payment.service'

const state = {
  payments: [],
  paymentMethods: [],
  products: [],
  prices: [],
  histories: [],
  balance: {},
  allBalances: [],
  defaultPaymentMethod: null
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
  },
  loadPaymentHistories({ commit }, userId) {
    console.log('Action load all transfer histories request', userId)
    return paymentService.transferHistories(userId).then(rs => {
      console.log('Action load all transfer histories result', rs)
      commit('SET_TRANSFER_HISTORIES', rs)
    })
  },
  loadBalance({ commit }) {
    return paymentService.getRaterAvailableBalances().then(rs => {
      commit('SET_BALANCES', rs)
    })
  },
  loadAllBalance({ commit }) {
    return paymentService.getAllBalances().then(r => {
      commit('SET_ALL_BALANCES', r)
    })
  },
  loadDefaultPaymentMethod({ commit }, id) {
    if (!id) {
      console.log('user do not have a customerId')
      commit('SET_DEFAULTPAYMENTMETHOD', null)
      return null
    }
    return paymentService.getDefaultPaymentMethod(id).then(rs => {
      console.log('Action load payment methods: ', rs)
      commit('SET_DEFAULTPAYMENTMETHOD', rs)
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
  },
  SET_TRANSFER_HISTORIES: (state, transfers) => {
    state.histories = transfers
  },
  SET_BALANCES: (state, balance) => {
    state.balance = balance
  },
  SET_DEFAULTPAYMENTMETHOD: (state, defaultPaymentMethod) => {
    state.defaultPaymentMethod = defaultPaymentMethod
  },
  SET_ALL_BALANCES: (state, balances) => {
    state.allBalances = balances
  }
}

const getters = {
  getAllPayments: state => state.payments,
  getAllPaymentMethods: state => state.paymentMethods,
  getAllProducts: state => state.products,
  getAllPrices: state => state.prices,
  getAllTransferHistories: state => state.histories,
  getBalance: state => state.balance,
  getAllBalances: state => state.allBalances,
  getDefaultPaymentMethod: state => state.defaultPaymentMethod
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
