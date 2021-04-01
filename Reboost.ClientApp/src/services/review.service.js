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
  }
}

export default reviewService
