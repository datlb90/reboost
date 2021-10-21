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
  }
}
export default articlesService
