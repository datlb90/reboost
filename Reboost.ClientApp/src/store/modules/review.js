import reviewService from '@/services/review.service.js'

const state = {
  annotation: {}
}

const actions = {
  loadReviewAnnotation({ commit }, obj) {
    return reviewService.getAnnotation(obj.docId, obj.reviewId).then(result => {
      commit('SET_REVIEW_ANNOTATION', result)
    })
  },
  addReviewAnnotation({ commit }, obj) {
    console.log('ADD_REVIEW_ANNOTATION', obj)
    return reviewService.addAnnotation(obj).then(rs => {
      commit('ADD_REVIEW_ANNOTATION', rs)
    })
  },
  addInTextComment({ commit }, docId, reviewId, obj) {
    console.log('ADD_IN_TEXT_COMMENT', docId, reviewId, obj)
    return reviewService.addInTextComment(docId, reviewId, obj).then(rs => {
      commit('ADD_REVIEW_ANNOTATION', rs)
    })
  }
}

const mutations = {
  SET_REVIEW_ANNOTATION: (state, annotation) => {
    state.annotation = annotation
  },
  ADD_REVIEW_ANNOTATION: (state, annotation) => {

  }
}

const getters = {
  getAnnotation: state => state.annotation
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
