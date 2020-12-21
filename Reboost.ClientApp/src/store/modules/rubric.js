import rubricService from '@/services/rubric.service'

const state = {
  rubrics: [],
  selectedRubric: {},
  listRubric: []
}

const actions = {
  loadRubrics({ commit }) {
    return rubricService.getAll().then(result => {
      result.forEach(rubric => {
        if (rubric.user) {
          rubric.fullName = rubric.user.firstName + ' ' + rubric.user.lastName
        }
      })

      console.log('Action load rubrics', result)

      commit('SET_RUBRICS', result)
    })
  },
  loadRubricById({ commit }, id) {
    return rubricService.getById(id).then(rs => {
      console.log('Rubric: ', rs)
      commit('SET_RUBRIC', rs)
    })
  },
  loadRubricByQuestionId({ commit }, id) {
    return rubricService.getByQuestionId(id).then(rs => {
      console.log('Rubric: ', rs)
      commit('SET_RUBRIC_BY_QUESTION_ID', rs)
    })
  },
  setSelectedRubric({ commit }, rubric) {
    commit('SET_RUBRIC', rubric)
  },
  updateRubric({ commit }, rubric) {
    commit('UPDATE_RUBRIC', rubric)
  }
}

const mutations = {
  SET_RUBRICS: (state, rubrics) => {
    state.rubrics = rubrics
  },
  SET_RUBRIC: (state, rubric) => {
    state.selectedRubric = rubric
  },
  UPDATE_RUBRIC: (state, rubric) => {
    state.selectedRubric = rubric
  },
  SET_RUBRIC_BY_QUESTION_ID: (state, rubric) => {
    state.listRubric = rubric
  }
}

const getters = {
  getAll: state => state.rubrics,
  getSelected: state => state.selectedRubric,
  getByQuestionId: state => state.listRubric
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
