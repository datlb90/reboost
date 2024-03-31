import http from '@/utils/axios'

const reviewService = {
  getAIFeedbackForCriteriaV2(model) {
    return http.post('/review/ai/feedback/criteria/v2', model).then(rs => rs.data)
  },
  getChartDescription(fileName) {
    return http.get('/review/get/chart/description/' + fileName).then(rs => rs.data)
  },
  getAIFeedbackForCriteriaV1(model) {
    return http.post('/review/ai/feedback/criteria', model).then(rs => rs.data)
  },
  createAutomatedReview(request) {
    return http.post('/review/automated', request).then(rs => rs.data)
  },
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
  saveRubric(reviewId, data) {
    return http.post(`/review/rubric/${reviewId}`, data).then(rs => rs.data)
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
  createReviewTrainingSample(type) {
    return http.post(`/review/createReviewTraining/${type}`).then(rs => rs.data)
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
  changeTrainingStatus(id, data) {
    return http.post(`/review/training/status/change/${id}`, data).then(rs => rs.data)
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
    return http.post(`/review/rate`, data).then(rs => rs.data)
  },
  getReviewRating(reviewId) {
    return http.get(`/review/rate/${reviewId}`).then(rs => rs.data)
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
  getReviewForSubmission(submissionId) {
    return http.get(`/review/submission/request/${submissionId}`).then(rs => rs.data)
  },
  getReviewRequestBySubmissionId(submissionId) {
    return http.get(`/review/getRequest/${submissionId}`).then(rs => rs.data)
  },
  createProReviewRequest(request) {
    return http.post('/review/request/pro', request).then(rs => rs.data)
  },
  getLinkToReviewByProRequestId(id) {
    return http.get(`/review/request/pro/${id}`).then(rs => rs.data)
  },
  getRaterTrainings(id) {
    return http.get(`/review/training/${id}`).then(rs => rs.data)
  },
  getAllDisputes() {
    return http.get('/review/dispute').then(rs => rs.data)
  },
  getAllLearnerDisputes() {
    return http.get('/review/dispute/learner').then(rs => rs.data)
  },
  createDisputes(data) {
    return http.post('/review/dispute', data).then(rs => rs.data)
  },
  getDisputeByReviewId(id) {
    return http.get(`/review/dispute/${id}`).then(rs => rs.data)
  },
  updateDispute(data) {
    return http.post(`/review/dispute/update`, data).then(rs => rs.data)
  },
  getReviewRequestModel() {
    return http.get('/review/request/model').then(rs => rs.data)
  },
  reassignReviewRequest(requestId, raterId) {
    return http.get(`/review/reassign/${requestId}/${raterId}`).then(rs => rs.data)
  },
  recordPayment(reviewId) {
    return http.put(`/review/record/payment/${reviewId}`).then(rs => rs.data)
  }
}

export default reviewService
