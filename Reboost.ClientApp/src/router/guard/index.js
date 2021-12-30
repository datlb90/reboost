import authService from '@/services/auth.service'
import reviewService from '../../services/review.service'
import { PageName, UserRole } from '@/app.constant'
import store from '../../store'
import { isApprovedRater, revieweeReviewAuthentication } from '../guard/UserReviewValidation'

export default async(router) => {
  router.beforeEach(async(to, from, next) => {
    const currentUser = store.getters['auth/getUser']

    if (to.name === PageName.NOT_FOUND) {
      next()
      return
    }

    // Check logged in
    if (to.meta && to.meta.loginRequired) {
      if (!authService.isAuthenticated() || !currentUser?.id) {
        return next({ path: `/login?returnUrl=${to.path}` })
      }
    }

    // Check user role
    if (to.meta.role) {
      const currentUser = authService.getCurrentUser()
      if (!currentUser || !currentUser.role || currentUser.role !== to.meta.role) {
        return next('/notfound')
      }
      next()
    }

    if (to.path == '/login' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register') {
      next()
      return
    }

    if (!currentUser || !currentUser.id) {
      if (to.path == '/login' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register') {
        next()
        return
      }
      next('/login')
      return
    }

    if (currentUser.role.trim() === UserRole.LEARNER && ((to.path.includes('/rater/') || to.path.includes('/admin/')) && !to.path.includes('/rater/register'))) {
      next({ name: PageName.NOT_FOUND })
      return
    }

    if (currentUser.role.trim() === UserRole.RATER && to.path.includes('/admin/')) {
      next({ name: PageName.NOT_FOUND })
      return
    }

    if (store.getters['auth/getSelectedTest'].length == 0 &&
    currentUser.role.trim() === UserRole.LEARNER &&
    (to.path != '/SelectYourTest')) {
      if (to.path == '/login') {
        next()
        return
      }
      return next('/SelectYourTest')
    }

    // Navigate to appropriate page base on user role after login
    if (currentUser.id && currentUser.role) {
      const role = currentUser.role
      if (to.name === PageName.AFTER_LOGIN) {
        if (role === UserRole.ADMIN) {
          return next({ name: PageName.RATERS })
        } else if (role === UserRole.RATER) {
          const check = await isApprovedRater()
          switch (check.code) {
            case -1:
              next({ name: PageName.RATER_STATUS, params: { id: check.rater.id }})
              break
            case 0:
              next({ name: PageName.RATER_APPLY })
              break
            case 1:
              next({ path: `rater/application/status/${check.rater.id}` })
              break
            case 2:
              next({ name: PageName.REVIEWS })
              break
            default:
              next()
          }
          return
        } else if (role === UserRole.LEARNER) {
          await store.dispatch('auth/setSelectedTest').then(rs => {
            const tests = store.getters['auth/getSelectedTest']
            if (tests.length > 0) {
              return next({ name: PageName.QUESTIONS })
            }
            return next({ name: PageName.SELECT_YOUR_TEST })
          })
        } else {
          next({ name: PageName.NOT_FOUND })
          return
        }
      }
    }

    // Check review auth
    if (to.name === PageName.REVIEW) {
      const check = await revieweeReviewAuthentication(to.params.reviewId)
      if (check === 0) {
        next({ name: PageName.NOT_FOUND })
        return
      }
    }

    // Link to pro review request
    if (to.path.includes('/review/pro/')) {
      if (currentUser.role === UserRole.RATER) {
        reviewService.getLinkToReviewByProRequestId(to.params.id).then(rs => {
          next({ path: `/review/${rs.submission.questionId}/${rs.submission.docId}/${rs.review.id}` })
          return
        })
      } else {
        next({ name: PageName.NOT_FOUND })
        return
      }
    }

    next()
  })
}
