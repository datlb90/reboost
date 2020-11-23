import NProgress from 'nprogress'

const state = {
  apiWaitingCount: 0
}

const actions = {
  startLoading({ commit, state }) {
    NProgress.start()
    commit('INCREASE_COUNT')
  },
  doneLoading({ commit, state }) {
    commit('DECREASE_COUNT')
    if (state.apiWaitingCount == 0) { NProgress.done() }
  },
  resetState({ commit, state }) {
    commit('RESET_LOADING_STATE')
  }
}

const mutations = {
  INCREASE_COUNT(state) {
    state.apiWaitingCount++
  },

  DECREASE_COUNT(state) {
    state.apiWaitingCount--
  },

  RESET_LOADING_STATE(state) {
    state.apiWaitingCount = 0
  }
}

const getters = {
  getApiWaitingCount: state => state.apiWaitingCount
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}

