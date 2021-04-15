import http from '@/utils/axios'

const reviewService = {
  getAnnotation(docId, reviewId) {
    return http.get(`/review/getAnnotation/${docId}/${reviewId}`).then(rs => rs.data)
  },
  saveAnnotations(docId, reviewId, annotation) {
    return http.post(`/review/saveAnnotation/${docId}/${reviewId}`, annotation)
  },
  addAnnotation(annotation) {
    return http.post(`/review/annotation`, annotation).then(rs => rs.data)
  },
  addInTextComment(docId, reviewId, comment, annotation) {
    return http.post(`/review/inTextComment/${docId}/${reviewId}`, { comment, annotation })
  },
  saveReviewFeedback(data) {
    return http.post(`/review/feedback`, data)
  },
  loadReviewFeedback(reviewId) {
    return http.get(`/review/feedback/${reviewId}`).then(rs => rs.data)
  },
  deleteInTextComment(id) {
    return http.post(`/review/comment/delete`, { id }).then(rs => rs.data)
  },
  deleteAnnotation(id) {
    return http.post(`/review/annotation/delete`, { id }).then(rs => rs.data)
  },
  editAnnotation(anno) {
    return http.post(`/review/edit`, anno).then(rs => rs.data)
  },
  editComment(comment) {
    return http.post(`/review/comment/edit`, comment).then(rs => rs.data)
  },
  createNewReviewSample(docId) {
    return http.post(`/review/createSampleReview/${docId}`).then(rs => rs.data)
  },
  getAllReviews() {
    return http.get('/review/all').then(rs => rs.data)
  },
  changeReviewStatus(id, status) {
    return http.post(`/review/status/change/${id}`, { status }).then(rs => rs.data)
  }
}

export default reviewService
