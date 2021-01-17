import http from '@/utils/axios'

const discussionService = {
  getAll() {
    return http.get('/discussion').then(rs => rs.data)
  },
  getAllByQuestionId(Id) {
    return http.get(`/discussion/${Id}/`).then(rs => rs.data)
  },
  getById(id) {
    return http.get(`/discussion/get-id/${id}`).then(rs => rs.data)
  },
  insert(data) {
    return http.post('/discussion/create', data).then(rs => rs.data)
  },
  update(data) {
    return http.post('/discussion/update', data).then(rs => rs.data)
  },
  upVote(data) {
    return http.post('/discussion/upVote', data).then(rs => rs.data)
  },
  downVote(data) {
    return http.post('/discussion/downVote', data).then(rs => rs.data)
  },
  delete(id) {
    return http.post('/discussion/delete', id).then(rs => rs.data)
  },
  getAllTags() {
    return http.get('/discussion/getAllTags').then(rs => rs.data)
  },
  getComments(id) {
    return http.get(`/discussion/getComments/${id}`).then(rs => rs.data)
  },
  increaseView(id) {
    return http.get(`/discussion/increaseView/${id}`).then(rs => rs.data)
  }
}
export default discussionService
