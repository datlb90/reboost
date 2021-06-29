import authService from '@/services/auth.service'
import { PageName, UserRole } from '@/app.constant'
import store from '../../store'

export default async router => {
  router.beforeEach(async(to, from, next) => {
    console.log('router before each', to, from)
    const currentUser = store.getters['auth/getUser']

    if (!currentUser.id) {
      if (to.path == '/login') {
        return next()
      }
      // return next('/login')
    }

    if (
      store.getters['auth/getSelectedTest'].length == 0 &&
      currentUser.role.trim() === UserRole.LEARNER &&
      to.path != '/SelectYourTest'
    ) {
      if (to.path == '/login') {
        next()
        return
      }
      return next('/SelectYourTest')
    }

    // Check logged in
    if (to.meta && to.meta.loginRequired) {
      if (!authService.isAuthenticated()) {
        return next('/login')
      }
    }

    // Check user role
    if (to.meta.role) {
      const currentUser = authService.getCurrentUser()
      if (
        !currentUser ||
        !currentUser.role ||
        currentUser.role !== to.meta.role
      ) {
        return next('/notfound')
      }

      next()
    }

    // Navigate to appropriate page base on user role after login
    if (currentUser && currentUser.role) {
      const role = currentUser.role
      if (to.name === PageName.AFTER_LOGIN) {
        console.log('AFTER LOGIN', currentUser)
        if (role === UserRole.ADMIN) {
          return next({ name: PageName.RATERS })
        } else if (role === UserRole.RATER) {
          return next({ name: PageName.RATER_APPLY })
        } else if (role === UserRole.LEARNER) {
          console.log('AFTER LOGIN as Leaner', role)
          await store.dispatch('auth/setSelectedTest').then(rs => {
            const tests = store.getters['auth/getSelectedTest']
            if (tests.length > 0) {
              return next({ name: PageName.QUESTIONS })
            }
            return next({ name: PageName.SELECT_YOUR_TEST })
          })
        } else {
          next({ name: PageName.NOT_FOUND })
        }
      }
    }

    next()
  })
}
