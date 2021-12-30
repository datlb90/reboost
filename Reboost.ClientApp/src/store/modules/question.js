import questionService from '@/services/question.service'
import sampleService from '../../services/sample.service'

const state = {
  questions: [],
  selectedQuestion: {},
  countQuestions: {},
  countQuestionsByUser: {},
  testByUser: {},
  statusQuestion: {},
  sampleByQuestion: [],
  samples: []
}

const actions = {
  loadAllQuestionByUser({ commit }, userId) {
    return questionService.getAllByUser(userId).then(result => {
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
      commit('SET_COUNT_QUESTIONS', result)
    })
  },
  loadSummaryByUser({ commit }, id) {
    return questionService.getCountQuestionsByUser(id).then(result => {
      commit('SET_COUNT_QUESTION_BY_USER', result)
    })
  },
  loadTestByUser({ commit }, id) {
    return questionService.getTestByUser(id).then(result => {
      commit('SET_TEST_BY_USER', result)
    })
  },
  loadStatusQuestion({ commit }, id) {
    return questionService.getStatusQuestion(id).then(result => {
      commit('SET_STATUS_QUESTION', result)
    })
  },
  loadSampleByQuestion({ commit }, id) {
    return questionService.getSampleByQuestion(id).then(result => {
      commit('SET_SAMPLE_QUESTION', result)
    })
  },
  clearSelectedQuestion({ commit }) {
    commit('CLEAR_SELECTED_QUESTION')
  },
  loadAllSamples({ commit }) {
    return sampleService.getAllSamples().then(rs => {
      commit('SET_ALL_SAMPLE', rs)
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
  },
  SET_SAMPLE_QUESTION: (state, sampleByQuestion) => {
    state.sampleByQuestion = sampleByQuestion
  },
  CLEAR_SELECTED_QUESTION: (state) => {
    state.selectedQuestion = {}
  },
  SET_ALL_SAMPLE: (state, samples) => {
    state.samples = samples
  }
}

const getters = {
  getAll: state => state.questions,
  getSelected: state => state.selectedQuestion,
  getCountQuestionByTasks: state => state.countQuestions,
  getSummaryByUser: state => state.countQuestionsByUser,
  getTestByUser: state => state.testByUser,
  getStatusQuestion: state => state.statusQuestion,
  getSampleByQuestion: state => state.sampleByQuestion,
  getAllSample: state => state.samples
  // getSelected: state => state.selectedRater
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
