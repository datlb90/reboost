import http from '@/utils/axios'

const documentService = {
  submitDocument(data) {
    return http.post('/document', data).then(rs => rs.data)
  }
}

export default documentService
