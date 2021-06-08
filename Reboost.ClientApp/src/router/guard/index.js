import authService from '@/services/auth.service'
import { PageName, UserRole } from '@/app.constant'
import userService from '@/services/user.service'

export default (router) => {
  router.beforeEach((to, from, next) => {
    console.log('router before each', to, from)

    // Check logged in
    if (to.meta && to.meta.loginRequired) {
      if (!authService.isAuthenticated()) {
        return next('/login')
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

    // Navigate to appropriate page base on user role after login
    const user = authService.getCurrentUser()
    if (user && user.role) {
      const role = authService.getCurrentUser().role
      if (to.name === PageName.AFTER_LOGIN) {
        console.log('AFTER LOGIN')
        if (role === UserRole.ADMIN) {
          return next({ name: PageName.RATERS })
        } else if (role === UserRole.RATER) {
          return next({ name: PageName.RATER_APPLY })
        } else if (role === UserRole.LEARNER) {
          const user = authService.getCurrentUser()

          userService.getUserScore(user.id).then(scores => {
            if (scores.length > 0) {
              return next({ name: PageName.QUESTIONS })
            }

            return next({ name: PageName.SELECT_YOUR_TEST })
          })
        }

        next({ name: PageName.NOT_FOUND })
      }
    }

    next()
  })
}
