
import http from '@/utils/axios'
import store from '@/store'

const authService = {
  resendConfirmationEmail(userId) {
    return http.get('/auth/resend/confirm/email/' + userId).then(rs => rs.data)
  },
  resetPassword(model) {
    return http.post(`/auth/reset/password`, model).then(rs => rs.data)
  },
  forgotPassword(model) {
    return http.post(`/auth/forgot/password`, model).then(rs => rs.data)
  },
  login(data) {
    return http.post('/auth/login', data)
      .then(response => {
        return response.data
      })
      .catch(error => {
        console.log(error)
      })
  },
  loginFacebook(returnUrl) {
    return http.post('/auth/external/facebook/Learner/' + returnUrl)
  },
  loginGoogle(returnUrl) {
    return http.post('/auth/external/google/Learner/' + returnUrl)
  },
  getExternalUser() {
    return http.get('/auth/external/user')
  },
  logout() {
    store.dispatch('auth/logout')
  },
  register(data) {
    return http.post('/auth/register', data)
      .then(response => {
        return response.data
      })
      .catch(error => {
        console.log(error)
      })
  },
  isAuthenticated() {
    const user = store.getters['auth/getUser']
    console.log(user)
    return user && user.token
  },
  getCurrentUser() {
    return store.getters['auth/getUser']
  }
}

export default authService
