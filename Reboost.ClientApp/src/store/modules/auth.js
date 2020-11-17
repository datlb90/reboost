import authService from '@/services/auth.service'

const state = {
  user: {
    id: null,
    username: null,
    email: null,
    role: null,
    token: null,
    expireDate: null
  }
}

const actions = {
  async authLogin({ state, commit }, data) {
    const result = await authService.login(data)
    if (result) {
      commit('SET_USER', result.user)
      return result.user
    }
    return null
  }
}

const mutations = {
  SET_USER: (state, user) => {
    state.user = user
  }
}

const getters = {
  getUser: state => state.user
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
