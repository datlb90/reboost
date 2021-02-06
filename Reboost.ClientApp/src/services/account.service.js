import http from '@/utils/axios'

const accountService = {
  isCompleteOnboard(userId) {
    return http.get(`payment/account/connected/${userId}`).then(rs => rs.data)
  }
}
export default accountService
