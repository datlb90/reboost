import http from '@/utils/axios'

const questionService = {
  getAll() {
    return http.get('/Questions').then(rs => rs.data)
  },
  getAllByUser(userId) {
    return http.get(`/questions/list/${userId}`).then(rs => rs.data)
  },
  getById(id) {
    return http.get(`/questions/getById/${id}`).then(rs => rs.data)
  },
  getCountQuestionByTasks() {
    return http.get('/questions/summary').then(rs => rs.data)
  },
  getCountQuestionsByUser(userId) {
    return http.get(`/questions/summaryPerUser/${userId}`).then(rs => rs.data)
  },
  getTestByUser(userId) {
    return http.get(`/questions/testForCurrentUsers/${userId}`).then(rs => rs.data)
  },
  getStatusQuestion(userId) {
    return http.get(`/questions/questionCompletedIdByUser/${userId}`).then(rs => rs.data)
  }
}

export default questionService
