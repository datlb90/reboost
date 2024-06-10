import questionService from '@/services/question.service'
import reviewService from '@/services/review.service'
import documentService from '@/services/document.service'
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
  async submitPersonalQuestion({ commit, state }, userId) {
    const formData = new FormData()
    formData.set('UserId', userId)
    formData.set('TaskName', state.personalQuestion.TaskName)
    formData.set('TaskId', state.personalQuestion.TaskId)
    formData.set('Test', state.personalQuestion.Test)
    formData.set('Text', state.personalQuestion.Text)

    const question = state.personalQuestion.Parts.find(q => q.Name == 'Question')
    if (question) {
      // Setup question content to send to the backend
      formData.set('Question.QuestionParts[0][Name]', 'Question')
      formData.set('Question.QuestionParts[0][Content]', question.Content)
      formData.set('Question.QuestionParts[0][Order]', 1)
      formData.set(`Question.QuestionParts[0][QuestionId]`, 0)

      const chart = state.personalQuestion.Parts.find(q => q.Name == 'Chart')
      if (chart) {
        // Setup chart to send to the backend
        formData.set(`Question.QuestionParts[1][Name]`, 'Chart')
        formData.set(`Question.QuestionParts[1][Order]`, 2)
        formData.set(`Question.QuestionParts[1][Content]`, chart.Content)
        formData.set(`Question.QuestionParts[1][FileName]`, chart.FileName)
        formData.set(`Question.UploadedFile`, chart.UploadedFile)
      }
      const submission = await questionService.createPersonalSubmission(formData)
      if (submission) {
        const rs = await reviewService.createAutomatedReview({
          UserId: userId,
          SubmissionId: submission.id,
          FeedbackLanguage: state.personalQuestion.FeedbackLanguage,
          ReviewType: 'detail'
        })
        commit('CLEAR_PERSONAL_QUESTION')
        return rs
      }
      commit('CLEAR_PERSONAL_QUESTION')
      return null
    }
  },
  async submitInitialTest({ state, commit }, userId) {
    state.initialSubmission.userId = userId
    // Create a new submission
    const response = await documentService.submitDocument(state.initialSubmission)
    if (response && response.submissions.length > 0) {
      const rs = await reviewService.createAutomatedReview({
        UserId: userId,
        SubmissionId: response.submissions[0].id,
        FeedbackLanguage: 'vn',
        ReviewType: 'detail'
      })
      commit('CLEAR_INITIAL_SUBMISSION')
      return rs
    }
    commit('CLEAR_INITIAL_SUBMISSION')
    return null
  },
  saveInitialTestData({ commit, state }, data) {
    commit('SET_INITIAL_SUBMISSION', data)
  },
  selectTopic({ commit, state }, selectedTopic) {
    const selectedTest = state.questionsForInitialTest.find(q => q.title == selectedTopic)
    commit('SET_QUESTION', selectedTest)
  },
  switchTest({ commit, state }, test) {
    if (test == 'Đề Task 1') {
      const selectedTest = state.questionsForInitialTest.find(q => q.section == 'Academic Writing Task 1')
      commit('SET_QUESTION', selectedTest)
    } else {
      const selectedTest = state.questionsForInitialTest.find(q => q.section == 'Academic Writing Task 2')
      commit('SET_QUESTION', selectedTest)
    }
  },
  async loadQuestionsForInitialTest({ commit, state }) {
    if (state.questionsForInitialTest && state.questionsForInitialTest.length > 0) {
      const selectedTest = state.questionsForInitialTest.find(q => q.section == 'Academic Writing Task 2')
      commit('SET_QUESTION', selectedTest)
      return selectedTest
    } else {
      const result = await questionService.getQuestionsForInitialTest()
      commit('SET_QUESTIONS_INIT_TEST', result)
      const selectedTest = result.find(q => q.section == 'Academic Writing Task 2')
      commit('SET_QUESTION', selectedTest)
      return selectedTest
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
      return result
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
    const personalQuestion = state.personalQuestion
    Object.assign(state, getDefaultState())
    state.personalQuestion = personalQuestion
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
  getInitialSubmission: state => state.initialSubmission,
  getInitialQuestions: state => state.questionsForInitialTest
  // getSelected: state => state.selectedRater
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
