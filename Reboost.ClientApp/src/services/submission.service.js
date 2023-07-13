import http from '@/utils/axios'

const apiPrefix = 'submission'

export default {
  getByUser(userId, questionId) {
    return http.get(`/${apiPrefix}/getByUser/${userId}/${questionId}`).then(rs => rs.data)
  }
}
