import questionService from '@/services/question.service'
import sampleService from '../../services/sample.service'

const getDefaultState = () => {
  return {
    questions: [],
    selectedQuestion: {},
    countQuestions: {},
    countQuestionsByUser: {},
    testByUser: {},
    statusQuestion: {},
    sampleByQuestion: [],
    samples: [],
    personalQuestion: null,
    questionsForInitialTest: null,
    initialSubmission: null
  }
}

const state = getDefaultState()

const actions = {
  saveInitialTestData({ commit, state }, data) {
    commit('SET_INITIAL_SUBMISSION', data)
  },
  switchTest({ commit, state }, test) {
    const selectedTest = state.questionsForInitialTest.find(q => q.test == test)
    commit('SET_QUESTION', selectedTest)
  },
  loadQuestionsForInitialTest({ commit, state }) {
    if (state.questionsForInitialTest && state.questionsForInitialTest.length > 0) {
      const selectedTest = state.questionsForInitialTest.find(q => q.test == 'IELTS')
      commit('SET_QUESTION', selectedTest)
    } else {
      questionService.getQuestionsForInitialTest().then(result => {
        commit('SET_QUESTIONS_INIT_TEST', result)
        const selectedTest = result.find(q => q.test == 'IELTS')
        commit('SET_QUESTION', selectedTest)
      })
    }
  },
  savePersonalQuestion({ commit }, personalQuestion) {
    commit('SET_PERSONAL_QUESTION', personalQuestion)
  },
  clearInitialSubmission({ commit }) {
    commit('CLEAR_INITIAL_SUBMISSION')
  },
  clearPersonalQuestion({ commit }) {
    commit('CLEAR_PERSONAL_QUESTION')
  },
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
  },
  clearState({ commit }) {
    commit('CLEAR_STATE')
  }
  // getAddEditQuestionData({ commit }) {
  //   questionService.getAddEditQuestionData().then(result => {
  //     commit('SET_ADD_EDIT_DATA', result)
  //     return result
  //   })
  // }
}

const mutations = {
  SET_INITIAL_SUBMISSION: (state, submission) => {
    state.initialSubmission = submission
  },
  SET_QUESTIONS_INIT_TEST: (state, questions) => {
    state.questionsForInitialTest = questions
  },
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
  },
  SET_PERSONAL_QUESTION: (state, personalQuestion) => {
    state.personalQuestion = personalQuestion
  },
  CLEAR_PERSONAL_QUESTION: (state) => {
    state.personalQuestion = null
  },
  CLEAR_INITIAL_SUBMISSION: (state) => {
    state.initialSubmission = null
  },
  CLEAR_STATE(state) {
    Object.assign(state, getDefaultState())
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
  getAllSample: state => state.samples,
  getPersonalQuestion: state => state.personalQuestion,
  getInitialSubmission: state => state.initialSubmission
  // getSelected: state => state.selectedRater
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
