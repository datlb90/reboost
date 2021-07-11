import http from '@/utils/axios'

const documentService = {
  submitDocument(data) {
    return http.post('/document', data).then(rs => rs.data)
  },
  submitPendingDocument(data) {
    return http.post('/document/pending', data).then(rs => rs.data)
  },
  search(userId, questionId) {
    return http.get(`document/search/user/${userId}/question/${questionId}`).then(rs => rs.data)
  },
  getBySubmissionId(id) {
    return http.get(`document/submission/${id}`).then(rs => rs.data)
  },
  updateDocumentBySubmissionId(id, data) {
    return http.put(`document/submission/${id}`, data).then(rs => rs.data)
  }
}

export default documentService
