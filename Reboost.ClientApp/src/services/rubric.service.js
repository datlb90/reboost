import http from '@/utils/axios'

const rubricService = {
  getAll() {
    return http.get('/rubric').then(rs => rs.data)
  },
  getById(id) {
    return http.get(`/rubric/${id}`).then(rs => rs.data)
  },
  getByQuestionId(id) {
    return http.get(`/rubric/getByQuestionId/${id}`).then(rs => rs.data)
  },
  insert(data) {
    return http.post('/rubric/create', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(rs => rs.data)
  },
  update(data) {
    return http.post('/rubric/update', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(rs => rs.data)
  }
}

export default rubricService
