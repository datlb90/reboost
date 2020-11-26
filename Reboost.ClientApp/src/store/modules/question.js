import questionService from '@/services/question.service'

const state = {
    questions: [],
    selectedQuestion: {},
    countQuestions: {}
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
            console.log('Action load count questions', result);
            commit('SET_COUNT_QUESTIONS', result)
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
}

const getters = {
    getAll: state => state.questions,
    getSelected: state => state.selectedQuestion,
    getCountQuestionByTasks: state => state.countQuestions
    //getSelected: state => state.selectedRater
}


export default {
    namespaced: true,
    state,
    mutations,
    getters,
    actions
}