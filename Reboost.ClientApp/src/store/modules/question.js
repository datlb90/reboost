import questionService from '@/services/question.service'

const state = {
  questions: [],
  selectedQuestion: {},
  countQuestions: {},
  countQuestionsByUser: {},
  testByUser: {},
  statusQuestion: {}
}

const actions = {
  loadAllQuestionByUser({ commit }, uesrId) {
    return questionService.getAllByUser(uesrId).then(result => {
      commit('SET_QUESTIONS', result)
    })
  },
  loadQuestions({ commit }) {
    return questionService.getAll().then(result => {
      commit('SET_QUESTIONS', result)
    })
  },
  loadQuestion({ commit }, id) {
    return questionService.getById(id).then(result => {
      commit('SET_QUESTION', result)
    })
  },
  loadCountQuestionsByTasks({ commit }) {
    return questionService.getCountQuestionByTasks().then(result => {
      console.log('Action load count questions', result)
      commit('SET_COUNT_QUESTIONS', result)
    })
  },
  loadSummaryByUser({ commit }, id) {
    return questionService.getCountQuestionsByUser(id).then(result => {
      console.log('Action load count question by user', result)
      commit('SET_COUNT_QUESTION_BY_USER', result)
    })
  },
  loadTestByUser({ commit }, id) {
    return questionService.getTestByUser(id).then(result => {
      console.log('Action load test by user', result)
      commit('SET_TEST_BY_USER', result)
    })
  },
  loadStatusQuestion({ commit }, id) {
    return questionService.getStatusQuestion(id).then(result => {
      console.log('Action load status question', result)
      commit('SET_STATUS_QUESTION', result)
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
  },
  SET_TEST_BY_USER: (state, testByUser) => {
    state.testByUser = testByUser
  },
  SET_STATUS_QUESTION: (state, statusQuestion) => {
    state.statusQuestion = statusQuestion
  }
}

const getters = {
  getAll: state => state.questions,
  getSelected: state => state.selectedQuestion,
  getCountQuestionByTasks: state => state.countQuestions,
  getSummaryByUser: state => state.countQuestionsByUser,
  getTestByUser: state => state.testByUser,
  getStatusQuestion: state => state.statusQuestion
  // getSelected: state => state.selectedRater
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
