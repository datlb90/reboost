import authService from '@/services/auth.service'
import reviewService from '../../services/review.service'
import { PageName, UserRole } from '@/app.constant'
import store from '../../store'
import { isApprovedRater, revieweeReviewAuthentication } from '../guard/UserReviewValidation'

function getCookie(name) {
  const value = `; ${document.cookie}`
  const parts = value.split(`; ${name}=`)
  if (parts.length === 2) return parts.pop().split(';').shift()
}

function deleteCookie(name) {
  document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;'
}

export default async (router) => {
  router.beforeEach(async (to, from, next) => {
    // Check if the user has been externally authenticated
    const externalUserId = decodeURIComponent(getCookie('userId'))
    if (externalUserId != 'undefined') {
      const user = {
        id: decodeURIComponent(getCookie('userId')),
        username: decodeURIComponent(getCookie('username')),
        email: decodeURIComponent(getCookie('email')),
        firstName: decodeURIComponent(getCookie('firstName')),
        lastName: decodeURIComponent(getCookie('lastName')),
        role: decodeURIComponent(getCookie('role')),
        token: decodeURIComponent(getCookie('token')),
        expireDate: decodeURIComponent(getCookie('expireDate'))
      }

      const returnUrl = decodeURIComponent(getCookie('returnUrl'))

      store.dispatch('auth/setUser', user)
      // Delete all cookies for external user
      deleteCookie('userId')
      deleteCookie('username')
      deleteCookie('email')
      deleteCookie('firstName')
      deleteCookie('lastName')
      deleteCookie('role')
      deleteCookie('token')
      deleteCookie('expireDate')
      deleteCookie('returnUrl')
      console.log(returnUrl)
      if (returnUrl == 'undefined' || returnUrl == '' || returnUrl == '/') {
        return next('/after-login')
      } else {
        return next(returnUrl)
      }
    }

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

    if (to.path == '/login' || to.path == '/terms' || to.path == '/privacy' || to.path == '/data/deletion' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register') {
      next()
      return
    }
    if (to.path == '/login' || to.path == '/terms' || to.path == '/privacy' || to.path == '/data/deletion' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register') {
      next()
      return
    }
    if (!currentUser || !currentUser.id) {
      if (to.path == '/login' || to.path == '/terms' || to.path == '/privacy' || to.path == '/data/deletion' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register') {
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
    // Navigate to appropriate page base on user role after login
    if (currentUser.id && currentUser.role) {
      const role = currentUser.role
      if (to.name === PageName.AFTER_LOGIN) {
        if (role === UserRole.ADMIN) {
          return next({ name: PageName.ADMIN_QUESTIONS })
        } else if (role === UserRole.RATER) {
          const check = await isApprovedRater()
          switch (check.code) {
            case -1:
              next({ name: PageName.RATER_STATUS, params: { id: check.rater.id } })
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
