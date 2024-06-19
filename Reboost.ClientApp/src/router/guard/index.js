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
        expireDate: decodeURIComponent(getCookie('expireDate')),
        freeToken: parseInt(decodeURIComponent(getCookie('freeToken'))),
        premiumToken: parseInt(decodeURIComponent(getCookie('premiumToken'))),
        subscription: JSON.parse(decodeURIComponent(getCookie('subscription')))
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

    // Create site links
    if (to.path == '/experience') {
      next('/?section=experience')
      return
    }
    if (to.path == '/benefit') {
      next('/?section=benefit')
      return
    }
    if (to.path == '/new-method') {
      next('/?section=new-method')
      return
    }
    if (to.path == '/features') {
      next('/?section=features')
      return
    }

    if (to.path == '/sample/feedback/basic' || to.path == '/login' || to.path == '/terms' || to.path == '/privacy' || to.path == '/data/deletion' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register' || to.path == '/forgot/password' || to.path == '/reset/password' || to.path == '/pricing' || to.path == '/confirm/email') {
      next()
      return
    }

    if (to.path == '/sample/feedback/basic' || to.path == '/login' || to.path == '/terms' || to.path == '/privacy' || to.path == '/data/deletion' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register' || to.path == '/forgot/password' || to.path == '/reset/password' || to.path == '/pricing' || to.path == '/confirm/email') {
      next()
      return
    }

    if (!currentUser || !currentUser.id) {
      if (to.path == '/sample/feedback/basic' || to.path == '/login' || to.path == '/terms' || to.path == '/privacy' || to.path == '/data/deletion' || to.path == '/' || to.path == '/register' || to.path == '/rater' || to.path == '/rater/login' || to.path == '/rater/register' || to.path == '/forgot/password' || to.path == '/reset/password' || to.path == '/pricing' || to.path == '/confirm/email') {
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
          await store.dispatch('auth/setSelectedTest')
          // Check if there is personal question or initial test for external login
          const personalQuestion = store.getters['question/getPersonalQuestion']
          const initialTest = store.getters['question/getInitialSubmission']
          if (personalQuestion || initialTest) {
            if (personalQuestion) {
              await store.dispatch('question/submitPersonalQuestion', currentUser.id)
            }
            if (initialTest) {
              await store.dispatch('question/submitInitialTest', currentUser.id)
            }
            const initialSubmission = await reviewService.getInitialSubmission(currentUser.id)
            if (initialSubmission) {
              const rs = await reviewService.createAutomatedReview({
                UserId: currentUser.id,
                SubmissionId: initialSubmission.id,
                FeedbackLanguage: initialSubmission.feedbackLanguage,
                ReviewType: 'detail'
              })

              // clear initial submission
              store.dispatch('question/clearPersonalQuestion')
              store.dispatch('question/clearInitialSubmission')
              // redirect user to the review page
              const url = `/review/${rs.questionId}/${rs.docId}/${rs.reviewId}`
              next({ path: url })
            } else {
              return next({ name: PageName.QUESTIONS })
            }
          } else {
            return next({ name: PageName.QUESTIONS })
          }

          // Check if user is requesting a review
          // Open the request review dialog
          // const personalQuestion = store.getters['question/getPersonalQuestion']
          // if (personalQuestion) {
          //   // Create the new submission and new AI review request
          //   const rs = await store.dispatch('question/submitPersonalQuestion', currentUser.id)
          //   console.log(rs)
          //   if (rs) {
          //     const url = `/review/${rs.questionId}/${rs.docId}/${rs.reviewId}`
          //     // Send user to the review page for getting feedback
          //     next({ path: url })
          //   } else {
          //     return next({ name: PageName.QUESTIONS })
          //   }
          // } else {
          //   // check for personal review request and initial test here
          //   // Check if user is submitting an initial test
          //   const initialSubmission = store.getters['question/getInitialSubmission']
          //   if (initialSubmission) {
          //     // Create the new submission and new AI review request
          //     const rs = await store.dispatch('question/submitInitialTest', currentUser.id)
          //     // Send user to the review page for getting feedback
          //     if (rs) {
          //       const url = `/review/${rs.questionId}/${rs.docId}/${rs.reviewId}`
          //       // Send user to the review page for getting feedback
          //       next({ path: url })
          //     } else {
          //       return next({ name: PageName.QUESTIONS })
          //     }
          //   } else {
          //     return next({ name: PageName.QUESTIONS })
          //   }
          // }
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
