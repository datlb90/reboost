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
  userReviewAuth(reviewId) {
    return http.get(`/review/${reviewId}/auth`).then(rs => rs)
  },
  ReviewAuth(reviewId) {
    return http.get(`/review/reviewee/${reviewId}/auth`).then(rs => rs)
  },
  raterApprovedCheck() {
    return http.post(`/review/rater/auth`).then(rs => rs.data)
  },
  addInTextComment(docId, reviewId, comment, annotation) {
    return http.post(`/review/inTextComment/${docId}/${reviewId}`, { comment, annotation })
  },
  saveReviewFeedback(reviewId, data) {
    return http.post(`/review/feedback/${reviewId}`, data).then(rs => rs.data)
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
  getReviewsById() {
    return http.get('/review/getById').then(rs => rs.data)
  },
  getById(id) {
    return http.get(`/review/${id}`).then(rs => rs.data)
  },
  changeReviewStatus(id, status) {
    return http.post(`/review/status/change/${id}`, { status }).then(rs => rs.data)
  },
  createReviewRequest(request) {
    return http.post('/review/request', request).then(rs => rs.data)
  },
  getReviewsByUser() {
    return http.get('/review/byRaterId').then(rs => rs.data)
  },
  getOrCreateReviewByRequestId(id) {
    return http.get(`/review/request/${id}`).then(rs => rs.data)
  },
  getOrCreateReviewBySubmissionId(id) {
    return http.get(`/review/submission/${id}`).then(rs => rs.data)
  },
  createReviewRating(data) {
    return http.post(`/review/reviewRating`, data).then(rs => rs.data)
  },
  getReviewRating(reviewId) {
    return http.get(`/review/reviewRating/${reviewId}`).then(rs => rs.data)
  },
  newRequest() {
    return http.get(`/review/new-review`).then(rs => rs.data)
  },
  getUnratedReview() {
    return http.get('/review/unrated').then(rs => rs.data)
  },
  getPendingReview() {
    return http.get('/review/pending').then(rs => rs.data)
  },
  getReviewRequestBySubmissionId(submissionId) {
    return http.get(`/review/getRequest/${submissionId}`).then(rs => rs.data)
  },
  submitRate(data) {
    return http.post('/review/submitRate', data).then(rs => rs.data)
  }
}

export default reviewService
