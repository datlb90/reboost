
import http from '@/utils/axios'

const authService = {
  login(data) {
    return http.post('/auth/login', data)
      .then(response => {
        return response.data
      })
      .catch(error => {
        console.log(error)
      })
  }
}

export default authService
