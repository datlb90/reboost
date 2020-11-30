import questionService from '@/services/question.service'

const state = {
  questions: [],
  selectedQuestion: {},
  countQuestions: {},
  countQuestionsByUser: {}
}

const actions = {
  loadQuestions({ commit }) {
    questionService.getAll().then(result => {
      commit('SET_QUESTIONS', result)
    })
  },
  loadQuestion({ commit }, id) {
    questionService.getById(id).then(result => {
      commit('SET_QUESTION', result)
    })
  },
  loadCountQuestionsByTasks({ commit }) {
    questionService.getCountQuestionByTasks().then(result => {
      console.log('Action load count questions', result)
      commit('SET_COUNT_QUESTIONS', result)
    })
  },
  loadCountQuestionByUser({ commit }, id) {
    questionService.getCountQuestionsByUser(id).then(result => {
      console.log('Action load count question by user', result)
      commit('SET_COUNT_QUESTION_BY_USER', result)
    })
  }
}

const mutations = {
  SET_QUESTIONS: (state, questions) => {
    state.questions = questions
  },
  SET_QUESTION: (state, question) => {
    state.selectedQuestion = question
  },
  SET_COUNT_QUESTIONS: (state, countQuestions) => {
    state.countQuestions = countQuestions
  },
  SET_COUNT_QUESTION_BY_USER: (state, countQuestionsByUser) => {
    state.countQuestionsByUser = countQuestionsByUser
  }
}

const getters = {
  getAll: state => state.questions,
  getSelected: state => state.selectedQuestion,
  getCountQuestionByTasks: state => state.countQuestions,
  getCountQuestionsByUser: state => state.countQuestionsByUser
  // getSelected: state => state.selectedRater
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
