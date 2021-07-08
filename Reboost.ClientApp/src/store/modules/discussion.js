import discussionService from '@/services/discussion.service'

const state = {
  discussions: [],
  discussionsByQuestionId: [],
  comments: [],
  selectedDiscussion: {},
  tags: []
}

const actions = {
  loadDiscussions({ commit }) {
    return discussionService.getAll().then(result => {
      console.log('Action load discussions', result)

      commit('SET_DISCUSSIONS', result)
    })
  },
  loadDiscussionByQuestionId({ commit }, id) {
    return discussionService.getAllByQuestionId(id).then(rs => {
      console.log('Discussion: ', rs)
      commit('SET_DISCUSSION_BY_QUESTION_ID', rs)
    })
  },
  loadAllTags({ commit }) {
    return discussionService.getAllTags().then(rs => {
      console.log('Action load all tags:', rs)
      commit('SET_TAGS', rs)
    })
  },
  loadDiscussionById({ commit }, id) {
    return discussionService.getById(id).then(rs => {
      console.log('Discussion: ', rs)
      commit('SET_DISCUSSION', rs)
    })
  },
  setSelectedDiscussion({ commit }, discussion) {
    commit('SET_DISCUSSION', discussion)
  },
  updateDiscussion({ commit }, discussion) {
    discussionService.update(discussion).then(result => {
      commit('UPDATE_DISCUSSION', discussion)
    })
  },
  upVote({ commit }, discussion) {
    return discussionService.upVote(discussion).then(result => {
      commit('UP_VOTE', discussion)
    })
  },
  downVote({ commit }, discussion) {
    return discussionService.downVote(discussion).then(result => {
      commit('DOWN_VOTE', discussion)
    })
  },
  loadComments({ commit }, id) {
    return discussionService.getComments(id).then(rs => {
      commit('GET_COMMENTS', rs)
    })
  },
  increaseView({ commit }, id) {
    return discussionService.increaseView(id).then(rs => {
      console.log('View Count: ', rs['Views'])
      commit('SET_DISCUSSION', rs)
    })
  }
}

const mutations = {
  SET_DISCUSSIONS: (state, discussions) => {
    state.discussions = discussions
  },
  SET_DISCUSSION_BY_QUESTION_ID: (state, discussions) => {
    state.discussionsByQuestionId = discussions
  },
  SET_DISCUSSION: (state, discussion) => {
    state.selectedDiscussion = discussion
  },
  UPDATE_DISCUSSION: (state, discussion) => {
    state.selectedDiscussion = discussion
  },
  UP_VOTE: (state, discussion) => {
    state.selectedDiscussion = discussion
  },
  DOWN_VOTE: (state, discussion) => {
    state.selectedDiscussion = discussion
  },
  SET_TAGS: (state, tags) => {
    state.tags = tags
  },
  GET_COMMENTS: (state, comments) => {
    state.comments = comments
  }
}

const getters = {
  getAll: state => state.discussions,
  getAllByQuestionId: state => state.discussionsByQuestionId,
  getSelected: state => state.selectedDiscussion,
  getAllTags: state => state.tags,
  getComments: state => state.comments
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
