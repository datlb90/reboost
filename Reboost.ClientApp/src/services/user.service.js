import http from '@/utils/axios'

const userService = {
  getAll() {
    return http.get('/user').then(rs => rs.data)
  },
  addScore(id, data) {
    return http.post(`/user/addScore/${id}`, data).then(rs => rs.data)
  }
}

export default userService
