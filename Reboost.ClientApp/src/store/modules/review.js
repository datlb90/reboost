import reviewService from '@/services/review.service.js'

const state = {
  annotation: {}
}

const actions = {
  loadReviewAnnotation({ commit }, obj) {
    return reviewService.getAnnotation(obj.docId, obj.reviewId).then(result => {
      console.log('Action load annotation', result)
      commit('SET_REVIEW_ANNOTATION', result)
    })
  }
}

const mutations = {
  SET_REVIEW_ANNOTATION: (state, annotation) => {
    state.annotation = annotation
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
