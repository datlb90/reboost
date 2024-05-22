import authService from '@/services/auth.service'
import userService from '@/services/user.service'
import paymentService from '@/services/payment.service'

const getDefaultState = () => {
  return {
    user: {
      id: null,
      username: null,
      email: null,
      role: null,
      token: null,
      expireDate: null,
      firstName: null,
      lastName: null,
      freeToken: null,
      subscription: null
    },
    selectedTest: []
  }
}

const state = getDefaultState()

const actions = {
  setSubscription({ state, commit }, subscription) {
    if (subscription) {
      commit('SET_SUBSCRIPTION', subscription)
    }
  },
  updateToken({ state, commit }, token) {
    if (token >= 0) {
      commit('SET_FREE_TOKEN', token)
    }
  },
  async login({ state, commit }, data) {
    const result = await authService.login(data)
    if (result) {
      commit('SET_USER', result.user)
      return result.user
    }
    return null
  },
  async register({ state, commit }, data) {
    const result = await authService.register(data)
    if (result) {
      commit('SET_USER', result.user)
      return result.user
    }
    return null
  },
  async logout({ state, commit, dispatch }) {
    dispatch('question/clearState', null, { root: true })
    dispatch('review/clearState', null, { root: true })
    dispatch('rater/clearState', null, { root: true })
    commit('RESET_AUTH_STATE')

    const questionsIntro = localStorage.getItem('noQuestionsIntro')
    const submissionsIntro = localStorage.getItem('noSubmissionsIntro')
    const reviewsIntro = localStorage.getItem('noReviewsIntro')

    window.localStorage.clear()

    if (questionsIntro) { localStorage.setItem('noQuestionsIntro', true) }
    if (submissionsIntro) { localStorage.setItem('noSubmissionsIntro', true) }
    if (reviewsIntro) { localStorage.setItem('noReviewsIntro', true) }
  },
  async clearUser({ state, commit }) {
    commit('RESET_AUTH_STATE')
  },
  async setSelectedTest({ state, commit }) {
    await userService.getUserScore(state.user.id).then(rs => {
      if (rs.length > 0) {
        const toeflSections = [1, 2, 3, 4]
        const ieltsSections = [5, 6, 7, 8]

        var ieltsFlag = true
        var toeflFlag = true

        var listTest = []

        for (var s in ieltsSections) {
          if (rs.filter(r => r['sectionId'] == ieltsSections[s]).length == 0) {
            ieltsFlag = false
            break
          }
        }
        for (var sc in toeflSections) {
          if (rs.filter(r => r['sectionId'] == toeflSections[sc]).length == 0) {
            toeflFlag = false
            break
          }
        }

        if (ieltsFlag) {
          listTest.push('ielts')
        }
        if (toeflFlag) {
          listTest.push('toefl')
        }

        commit('SET_SELECTED_TEST', listTest)
        return listTest
      } else {
        commit('SET_SELECTED_TEST', [])
        return []
      }
    })
  },
  async loadCustomerId({ commit }) {
    return paymentService.getCustomerId().then(rs => {
      commit('SET_CUSTOMER_ID', rs)
    })
  },
  setUser({ commit }, user) {
    commit('SET_USER', user)
  }
}

const mutations = {
  SET_FREE_TOKEN(state, token) {
    state.user.freeToken = token
  },
  SET_SUBSCRIPTION(state, subscription) {
    state.user.subscription = subscription
  },
  SET_USER: (state, user) => {
    state.user = user
  },
  CLEAR_USER(state) {
    state.user = {}
    state.selectedTest = []
  },
  SET_SELECTED_TEST(state, test) {
    state.selectedTest = test
  },
  SET_CUSTOMER_ID(state, id) {
    state.user.stripeCustomerId = id
  },
  RESET_AUTH_STATE(state) {
    Object.assign(state, getDefaultState())
  }
}

const getters = {
  getUser: state => state.user,
  getSelectedTest: state => state.selectedTest
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
