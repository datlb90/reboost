import http from '@/utils/axios'

const lookupService = {
  getByType(type) {
    return http.get(`/lookup/getByType/${type}`).then(rs => rs.data)
  }
}

export default lookupService
