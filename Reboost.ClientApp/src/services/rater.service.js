import http from '@/utils/axios'

const raterService = {
  getAll() {
    return http.get('/rater').then(rs => rs.data)
  },
  getById(id) {
    return http.get(`/rater/${id}`).then(rs => rs.data)
  },
  insert(data) {
    const formData = new FormData()
    const specialKeys = ['iDCardPhotos', 'iELTSCertificatePhotos', 'tOEFLCertificatePhotos']

    for (const key in data) {
      if (specialKeys.includes(key)) {
        for (const f of data[key]) {
          formData.append(key, f)
        }
      } else {
        formData.set(key, data[key])
      }
    }
    return http.post('/rater/create', formData, { headers: { 'Content-Type': 'multipart/form-data' }}).then(rs => rs.data)
  },
  update(data) {
    const formData = new FormData()
    const specialKeys = ['iDCardPhotos', 'iELTSCertificatePhotos', 'tOEFLCertificatePhotos']

    for (const key in data) {
      if (specialKeys.includes(key)) {
        for (const f of data[key]) {
          formData.append(key, f)
        }
      } else {
        formData.set(key, data[key])
      }
    }
    return http.post('/rater/update', formData, { headers: { 'Content-Type': 'multipart/form-data' }}).then(rs => rs.data)
  },
  updateStatus(id, status) {
    return http.get(`/rater/status/${id}/${status}`).then(rs => rs.data)
  }
}

export default raterService
