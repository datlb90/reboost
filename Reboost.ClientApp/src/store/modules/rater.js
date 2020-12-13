import raterService from '@/services/rater.service'

const state = {
  raters: [],
  selectedRater: {}
}

const actions = {
  loadRaters({ commit }) {
    return raterService.getAll().then(result => {
      result.forEach(rater => {
        if (rater.user) {
          rater.fullName = rater.user.firstName + ' ' + rater.user.lastName
        }
      })

      console.log('Action load raters', result)

      commit('SET_RATERS', result)
    })
  },
  loadRater({ commit }, id) {
    return raterService.getById(id).then(rs => {
      console.log('Rater: ', rs)
      commit('SET_RATER', rs)
    })
  },
  setSelectedRater({ commit }, rater) {
    commit('SET_RATER', rater)
  },
  updateRater({ commit }, rater) {
    commit('UPDATE_RATER', rater)
  }
}

const mutations = {
  SET_RATERS: (state, raters) => {
    state.raters = raters
  },
  SET_RATER: (state, rater) => {
    state.selectedRater = rater
  },
  UPDATE_RATER: (state, rater) => {
    state.selectedRater = rater
  }
}

const getters = {
  getAll: state => state.raters,
  getSelected: state => state.selectedRater
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
