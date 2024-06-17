import http from '@/utils/axios'

const questionService = {
  getWritingTextFromImage(data) {
    return http.post('/questions/image/to/text', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(rs => rs.data)
  },
  getQuestionsForInitialTest() {
    return http.get('/questions/initial/test').then(rs => rs.data)
  },
  createInitialSubmission(model) {
    return http.post('/questions/initial/submission', model).then(rs => rs.data)
  },
  createPersonalSubmission(model) {
    return http.post('/questions/peronsal/submission', model).then(rs => rs.data)
  },
  getAll() {
    return http.get('/questions').then(rs => { return rs ? rs.data : [] })
  },
  getAllByUser(userId) {
    return http.get(`/questions/list/${userId}`).then(rs => { return rs ? rs.data : [] })
  },
  getById(id) {
    return http.get(`/questions/getById/${id}`).then(rs => { return rs ? rs.data : {} })
  },
  getCountQuestionByTasks() {
    return http.get('/questions/summary').then(rs => { return rs ? rs.data : [] })
  },
  getCountQuestionsByUser(userId) {
    return http.get(`/questions/summaryPerUser/${userId}`).then(rs => { return rs ? rs.data : [] })
  },
  getTestByUser(userId) {
    return http.get(`/questions/testForCurrentUsers/${userId}`).then(rs => { return rs ? rs.data : [] })
  },
  getStatusQuestion(userId) {
    return http.get(`/questions/questionCompletedIdByUser/${userId}`).then(rs => { return rs ? rs.data : [] })
  },
  getSampleByQuestion(questionId) {
    return http.get(`/questions/sampleForQuestion/${questionId}`).then(rs => { return rs ? rs.data : [] })
  },
  getSubmissionsByUserId(userId) {
    return http.get(`/questions/submissions/${userId}`).then(rs => { return rs ? rs.data : [] })
  },
  getSubmissionsforQuestion(userId, questionId) {
    return http.get(`/questions/submissions/${userId}/${questionId}`).then(rs => { return rs ? rs.data : [] })
  },
  getAddEditQuestionData() {
    return http.get('/questions/add-edit/data').then(rs => rs.data)
  },
  createQuestion(data) {
    return http.post('/questions', data).then(rs => rs.data)
  },
  updateQuestion(data) {
    return http.post('/questions/update', data).then(rs => rs.data)
  },
  publishQuestion(id) {
    return http.get(`/questions/publish/${id}`).then(rs => rs.data)
  },
  approveQuestion(id) {
    return http.get(`/questions/approve/${id}`).then(rs => rs.data)
  },
  deleteQuestion(id) {
    return http.delete(`/questions/${id}`).then(rs => rs.data)
  }
}

export default questionService
