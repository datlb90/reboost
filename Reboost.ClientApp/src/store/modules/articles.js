import articlesService from '@/services/articles.service'

const state = {
  articles: [],
  selectedArticle: {},
  labels: [],
  relations: []
}

const actions = {
  loadAllArticles({ commit }) {
    return articlesService.getAllArticles().then(rs => {
      commit('SET_ARTICLES', rs)
    })
  },
  loadAllArticleLabels({ commit }) {
    return articlesService.getAllLabels().then(rs => {
      commit('SET_LABELS', rs)
    })
  },
  loadAllArticleRelations({ commit }) {
    return articlesService.getAllRelations().then(rs => {
      commit('SET_RELATIONS', rs)
    })
  }
}

const mutations = {
  SET_ARTICLES: (state, articles) => {
    state.articles = articles
  },
  SET_LABELS: (state, labels) => {
    state.labels = labels
  },
  SET_RELATIONS: (state, relations) => {
    state.relations = relations
  }
}

const getters = {
  getAll: state => state.articles,
  getAllLabels: state => state.labels,
  getAllRelations: state => state.relations
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}

