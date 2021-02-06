import http from '@/utils/axios'

const documentService = {
  submitDocument(data) {
    return http.post('/document', data).then(rs => rs.data)
  },
  search(userId, questionId) {
    return http.get(`document/search/user/${userId}/question/${questionId}`).then(rs => rs.data)
  }
}

export default documentService
