import http from '@/utils/axios'

const userService = {
  getAll() {
    return http.get('/user').then(rs => rs.data)
  },
  addScore(id, data) {
    return http.post(`/user/addScore/${id}`, data).then(rs => rs.data)
  },
  getUserScore(id) {
    return http.get(`/user/getUserScore/${id}`).then(rs => rs.data)
  },
  hasSubmissionOnTaskOf(userId, questionId) {
    return http.get(`/user/hasSubmissionOnTaskOf/${userId}/${questionId}`).then(rs => rs.data)
  },
  supportRequest(data) {
    return http.post('/user/support', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(rs => rs.data)
  }
}

export default userService
