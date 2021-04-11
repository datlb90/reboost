import reviewService from '@/services/review.service.js'

const state = {
  annotation: {},
  reviews: [],
  anno: null
}

const actions = {
  loadReviews({ commit }, obj) {
    return reviewService.getAllReviews().then(result => {
      commit('SET_REVIEWS', result)
    })
  },
  loadReviewAnnotation({ commit }, obj) {
    return reviewService.getAnnotation(obj.docId, obj.reviewId).then(result => {
      commit('SET_REVIEW_ANNOTATION', result)
    })
  },
  addReviewAnnotation({ commit }, obj) {
    return reviewService.addAnnotation(obj).then(rs => {
      commit('ADD_REVIEW_ANNOTATION', rs)
    })
  },
  addInTextComment({ commit }, docId, reviewId, obj) {
    return reviewService.addInTextComment(docId, reviewId, obj).then(rs => {
      commit('ADD_REVIEW_ANNOTATION', rs)
    })
  }
}

const mutations = {
  SET_REVIEWS: (state, reviews) => {
    state.reviews = reviews
  },
  SET_REVIEW_ANNOTATION: (state, annotation) => {
    state.annotation = annotation
  },
  ADD_REVIEW_ANNOTATION: (state, annotation) => {
    state.anno = annotation
  }
}

const getters = {
  getAnnotation: state => state.annotation,
  getAddedAnnotation: state => state.anno,
  getReviews: state => state.reviews
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
