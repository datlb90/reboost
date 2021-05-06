import http from '@/utils/axios'

const raterService = {
  getAll() {
    return http.get('/rater').then(rs => rs.data)
  },
  getByCurrentUser() {
    return http.get(`/rater/byCurrentUser`).then(rs => rs.data)
  },
  getById(id) {
    return http.get(`/rater/${id}`).then(rs => rs.data)
  },
  insert(data) {
    return http.post('/rater/create', data, { headers: { 'Content-Type': 'multipart/form-data' }}).then(rs => rs.data)
  },
  update(data) {
    return http.post('/rater/update', data, { headers: { 'Content-Type': 'multipart/form-data' }}).then(rs => rs.data)
  },
  updateCredential(data) {
    return http.post('/rater/update/credential', data, { headers: { 'Content-Type': 'multipart/form-data' }}).then(rs => rs.data)
  },
  updateStatus(id, status) {
    return http.get(`/rater/update/status/${id}/${status}`).then(rs => rs.data)
  }
}

export default raterService
