import http from '@/utils/axios'

const articlesService = {
  createArticle(data) {
    return http.post('articles', data).then(rs => rs.data)
  },
  getAllArticles() {
    return http.get('articles').then(rs => rs.data)
  },
  getAllLabels() {
    return http.get('articles/labels').then(rs => rs.data)
  },
  getAllRelations() {
    return http.get('articles/relations').then(rs => rs.data)
  },
  getById(id) {
    return http.get(`articles/${id}`).then(rs => rs.data)
  },
  updateArticle(id, data) {
    return http.put(`articles/${id}`, data).then(rs => rs.data)
  },
  deleteArticle(id) {
    return http.delete(`articles/${id}`).then(rs => rs.data)
  }
}
export default articlesService
