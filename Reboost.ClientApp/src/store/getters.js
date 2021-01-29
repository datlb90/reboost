const getters = {
  sidebar: state => state.app.sidebar,
  size: state => state.app.size,
  device: state => state.app.device,
  visitedViews: state => state.tagsView.visitedViews,
  cachedViews: state => state.tagsView.cachedViews,
  token: state => state.user.token,
  avatar: state => state.user.avatar,
  name: state => state.user.name,
  introduction: state => state.user.introduction,
  roles: state => state.user.roles,
  permission_routes: state => state.permission.routes,
  errorLogs: state => state.errorLog.logs,
  raters: state => state.raters,
  questions: state => state.questions,
  user: state => state.user,
  role: state => state.user.role,
  apiWaitingCount: state => state.apiWaitingCount,
  rubric: state => state.rubric,
  discussions: state => state.discussions,
  payments: state => state.payments
}

export default getters

