import Vue from 'vue'
import Router from 'vue-router'
import Home from '../views/Home.vue'
import Document from '../views/Document.vue'
import Test from '../views/Test.vue'
import store from '@/store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
Vue.use(Router)

// import Layout from '../layout/Index.vue';
/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
      plainLayout: false
    }
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '/document',
    name: 'Document',
    component: Document
    // meta: {
    // 	plainLayout: true
    // }
  },
  {
    path: '/test',
    name: 'Test',
    component: Test,
    meta: {
      plainLayout: true
    }
  }
]

export const asyncRoutes = [
  { path: '*', redirect: '/404', hidden: true }
]

// export default router;

const createRouter = () => new Router({
  mode: 'history', // require service support
  // scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
// export function resetRouter() {
// 	const newRouter = createRouter();
// 	router.matcher = newRouter.matcher; // reset router
// }

export default router

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login', '/auth-redirect'] // no redirect whitelist

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title
  // npndocument.title = getPageTitle(to.meta.title);

  // determine whether the user has logged in
  const hasToken = true// getToken();
  if (hasToken) {
    if (to.path === '/login') {
      // if is logged in, redirect to the home page
      next({ path: '/' })
      // NProgress.done(); // hack: https://github.com/PanJiaChen/vue-element-admin/pull/2939
    } else {
      // determine whether the user has obtained his permission roles through getInfo
      const hasRoles = false// store.getters.roles && store.getters.roles.length > 0;
      if (hasRoles) {
        next()
      } else {
        // // generate accessible routes map based on roles
        // const accessRoutes = await store.dispatch('permission/generateRoutes');

        // // dynamically add accessible routes
        // router.addRoutes(accessRoutes);

        // // hack method to ensure that addRoutes is complete
        // // set the replace: true, so the navigation will not leave a history record
        // //next({ ...to, replace: true });
        // next();

        try {
          // get user info
          // note: roles must be a object array! such as: ['admin'] or ,['developer','editor']
          // const { roles } = await store.dispatch('user/getInfo');

          // generate accessible routes map based on roles
          const accessRoutes = await store.dispatch('permission/generateRoutes')

          // dynamically add accessible routes
          router.addRoutes(accessRoutes)

          // hack method to ensure that addRoutes is complete
          // set the replace: true, so the navigation will not leave a history record
          // next({ ...to, replace: true });
          next()
        } catch (error) {
          // remove token and go to login page to re-login
          // await store.dispatch('user/resetToken');
          Message.error(error || 'Has Error')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  } else {
    /* has no token*/

    if (whiteList.indexOf(to.path) !== -1) {
      // in the free login whitelist, go directly
      next()
    } else {
      // other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      // NProgress.done();
    }
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})

