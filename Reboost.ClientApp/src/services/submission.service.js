import http from '@/utils/axios'

export default {
  getByUser(userId, questionId) {
    return http.get(`/submission/getByUser/${userId}/${questionId}`).then(rs => rs.data)
  }
}
