import http from '@/utils/axios'

const documentService = {
  createInitialSubmission(data) {
    return http.post('/document/initial', data).then(rs => rs.data)
  },
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
  },
  getDocument(id) {
    return http.get(`document/${id}`)
  },
  saveDocument(data) {
    return http.post('/document/save', data).then(rs => rs.data)
  },
  getSavedDocument(userId, questionId) {
    return http.get(`document/saved/user/${userId}/question/${questionId}`).then(rs => rs.data)
  }
}

export default documentService
