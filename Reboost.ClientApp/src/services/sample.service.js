import http from '@/utils/axios'

const sampleService = {
  createQuestionSample(data) {
    return http.post('/sample', data).then(rs => rs.data)
  },
  getAllSamples() {
    return http.get('/sample').then(rs => rs.data)
  },
  approveSampleById(id) {
    return http.get(`/sample/approve/${id}`).then(rs => rs.data)
  }
}

export default sampleService
