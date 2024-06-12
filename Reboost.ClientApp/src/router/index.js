import Vue from 'vue'
import VueRouter from 'vue-router'

import About from '../views/About.vue'
import Terms from '../views/Terms.vue'
import IPN from '../views/IPN.vue'
import Privacy from '../views/PrivacyPolicy.vue'
import DataDeletion from '../views/DataDeletion.vue'
import Document from '../views/Document.vue'
import Test from '../views/Test.vue'
import RaterApply from '../views/rater/Application.vue'
import Login from '../views/account/Login.vue'
import Logout from '../views/account/Logout.vue'
import Register from '../views/account/Register.vue'
import RaterRegister from '../views/account/RaterRegister.vue'
import Redirect from '../views/account/Redirect.vue'
import ConfirmEmail from '../views/account/ConfirmEmail.vue'
import ForgotPassword from '../views/account/ForgotPassword.vue'
import ResetPassword from '../views/account/ResetPassword.vue'
import AdminHome from '../views/admin/AdminHome.vue'
import Review from '../views/learner/Review.vue'
import Feedback from '../views/learner/Feedback.vue'
import SampleFeedback from '../views/learner/SampleFeedback.vue'
import ReviewTest from '../views/learner/ReviewTest.vue'
// Pages
import Landing from '../components/landing-pages/Landing'
import RaterLanding from '../components/landing-pages/Rater'
import Pricing from '../views/learner/Pricing.vue'
import ManageRaters from '../views/admin/ManageRaters.vue'
import Questions from '../views/learner/Questions.vue'
import RaterApplication from '../views/rater/Application.vue'
import ApplicationDetail from '../views/admin/ApplicationDetail.vue'
import PracticeWriting from '../views/learner/PracticeWriting'
import SelectYourTest from '../views/learner/SelectYourTest'
import ApplicationStatus from '../views/rater/ApplicationStatus'
import DiscussionDetail from '../views/learner/PracticeWriting_TabDiscussion_Detail.vue'
import DiscussionList from '../views/learner/PracticeWriting_TabDiscussion_List.vue'
import Subscribe from '../views/rater/Subscribe.vue'
import StartRating from '../views/rater/StartRating.vue'
import Payout from '../views/rater/Payout.vue'
import Reviews from '../views/learner/Reviews.vue'
import PaymentInfo from '../views/rater/PaymentInfo.vue'
import Submissions from '../views/learner/Submissions.vue'
import Disputes from '../views/admin/Disputes.vue'
import LearnerDisputes from '../views/learner/Disputes.vue'
import AdminQuestions from '../views/admin/Questions.vue'
import AdminRequests from '../views/admin/Requests.vue'
import Samples from '../views/admin/Samples.vue'
import Articles from '../views/admin/Articles.vue'
import LearnerArticles from '../views/learner/Articles.vue'
import ArticleDetail from '../views/learner/ArticleDetail.vue'
import PaymentRedirect from '../views/learner/PaymentRedirect.vue'

// import PageNotFound from '../views/PageNotFound.vue'
import protect from './guard'
import { PageName, UserRole } from '@/app.constant'

// Guard
import { userReviewAuthentication } from './guard/UserReviewValidation'

Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  linkExactActiveClass: 'active',
  scrollBehavior() {
    return { x: 0, y: 0 }
  },
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Landing,
      meta: {
        plainLayout: false,
        landingPage: true
      }
    },
    {
      path: '/rater',
      name: 'Rater',
      component: RaterLanding,
      meta: {
        plainLayout: false,
        raterLanding: true
      }
    },
    {
      path: '/rater/apply',
      name: PageName.RATER_APPLY,
      component: RaterApply,
      meta: {
        plainLayout: false,
        loginRequired: true
      }
    },
    {
      path: '/terms',
      name: 'Terms',
      component: Terms,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/privacy',
      name: 'Privacy Policy',
      component: Privacy,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },

    {
      path: '/IPN',
      name: 'IPN',
      component: IPN,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/data/deletion',
      name: 'Data Deletion',
      component: DataDeletion,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/about',
      name: 'About',
      component: About
    },
    {
      path: '/document',
      name: 'Document',
      component: Document,
      meta: {
        plainLayout: false
      }
    },
    {
      path: '/login',
      name: PageName.LOGIN,
      component: Login,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/rater/login',
      name: PageName.LOGIN,
      component: Login,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/after-login',
      name: PageName.AFTER_LOGIN
    },
    {
      path: '/logout',
      name: 'Logout',
      component: Logout
    },
    {
      path: '/register',
      name: PageName.REGISTER,
      component: Register,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/rater/register',
      name: PageName.REGISTER_RATER,
      component: RaterRegister,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/redirect',
      name: PageName.REDIRECT,
      component: Redirect,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/confirm/email',
      name: 'Confirm Email',
      component: ConfirmEmail,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/forgot/password',
      name: 'Forgot Password',
      component: ForgotPassword,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/reset/password',
      name: 'Reset Password',
      component: ResetPassword,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/sample/feedback/basic',
      name: 'Sample Basic Feedback',
      component: SampleFeedback,
      meta: {
        plainLayout: true,
        loginRequired: false
      }
    },
    {
      path: '/review/:questionId/:docId/:reviewId',
      name: PageName.REVIEW,
      component: Feedback,
      beforeEnter: async (to, from, next) => {
        const check = await userReviewAuthentication(to.params.reviewId)
        if (check) {
          next({ name: PageName.NOT_FOUND })
        } else {
          next()
        }
      },
      meta: {
        plainLayout: false,
        landingPage: false,
        loginRequired: true
      }
    },
    // {
    //   path: '/review/:questionId/:docId/:reviewId',
    //   name: PageName.REVIEW,
    //   component: Review,
    //   beforeEnter: async (to, from, next) => {
    //     const check = await userReviewAuthentication(to.params.reviewId)
    //     if (check) {
    //       next({ name: PageName.NOT_FOUND })
    //     } else {
    //       next()
    //     }
    //   },
    //   meta: {
    //     plainLayout: false,
    //     landingPage: false,
    //     loginRequired: true
    //   }
    // },
    {
      path: '/review-plain/:questionId/:docId/:reviewId',
      name: PageName.REVIEW,
      component: Review,
      beforeEnter: async (to, from, next) => {
        const check = await userReviewAuthentication(to.params.reviewId)
        if (check) {
          next({ name: PageName.NOT_FOUND })
        } else {
          next()
        }
      },
      meta: {
        plainLayout: true,
        landingPage: false,
        loginRequired: true
      }
    },
    {
      path: '/review/:questionId/:docId/:reviewId/:isViewOrRate',
      name: PageName.REVIEW,
      component: Review,
      beforeEnter: async (to, from, next) => {
        next()
        // if (to.params.isViewOrRate === 'view' || to.params.isViewOrRate === 'rate') {
        //   const check = await revieweeReviewAuthentication(to.params.reviewId)
        //   if (check === 0) {
        //     next({ path: 'notfound' })
        //   } else {
        //     if (check === 1 && to.params.isViewOrRate === 'view') {
        //       next()
        //     }
        //     if (check === 2 && to.params.isViewOrRate === 'rate') {
        //       next()
        //     }
        //     if (check === 2 && to.params.isViewOrRate === 'view') {
        //       next({ path: 'notfound' })
        //     }
        //   }
        // } else {
        //   next({ path: 'notfound' })
        // }
      },
      meta: {
        plainLayout: false,
        landingPage: false,
        loginRequired: true
      }
    },
    {
      path: '/review/test',
      name: 'Review Test',
      component: ReviewTest,
      meta: {
        plainLayout: false,
        landingPage: false,
        loginRequired: true
      }
    },
    {
      path: '/review/pro/:id',
      name: 'Get Review',
      meta: {
        plainLayout: false,
        landingPage: false,
        loginRequired: true
      }
    },
    {
      path: '/reviews',
      name: PageName.REVIEWS,
      component: Reviews,
      meta: {
        plainLayout: false,
        landingPage: false,
        loginRequired: true
      }
    },
    {
      path: '/payment/redirect',
      name: 'Payment Redirect',
      component: PaymentRedirect,
      meta: {
        plainLayout: false,
        landingPage: false,
        loginRequired: true
      }
    },
    {
      path: '/pricing',
      name: 'Pricing',
      component: Pricing,
      meta: {
        isPricing: true,
        loginRequired: false
      }
    },
    {
      path: '/admin/disputes',
      name: PageName.DISPUTES,
      component: Disputes,
      meta: {
        plainLayout: false,
        landingPage: false,
        loginRequired: true
      }
    },
    {
      path: '/test',
      name: 'Test',
      component: Test,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },

    {
      path: '/admin',
      name: 'AdminHome',
      component: AdminHome,
      loginRequired: true
    },
    {
      path: '/admin/raters',
      name: PageName.RATERS,
      meta: {
        loginRequired: true,
        role: UserRole.ADMIN
      },
      component: ManageRaters
    },
    {
      path: '/questions',
      name: PageName.QUESTIONS,
      component: Questions,
      meta: {
        loginRequired: true,
        role: UserRole.LEARNER
      }
    },
    {
      path: '/rater/application',
      name: 'RaterApplication',
      component: RaterApplication,
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/admin/raters/application/:id',
      component: ApplicationDetail,
      name: 'ApplicationDetail',
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/rater/application/status/:id',
      component: ApplicationStatus,
      name: PageName.RATER_STATUS,
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/practice/:id',
      component: PracticeWriting,
      name: 'PracticeWriting',
      children: [{
        path: 'discuss/:discussId',
        component: DiscussionDetail,
        name: 'DiscussionDetail'
      },
      {
        path: 'discuss',
        component: DiscussionList,
        name: 'DiscussionList'
      }],
      meta: {
        loginRequired: true,
        role: UserRole.LEARNER
      }
    },
    {
      path: '/practice/:id/:submissionId',
      component: PracticeWriting,
      name: 'PracticeWriting',
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/select/test',
      component: SelectYourTest,
      name: PageName.SELECT_YOUR_TEST,
      meta: {
        loginRequired: true,
        role: UserRole.LEARNER
      }
    },
    {
      path: '/Subscribe',
      component: Subscribe,
      name: 'Subscribe',
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/rater/startRating',
      component: StartRating,
      name: 'StartRating',
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/rater/payout',
      component: Payout,
      name: 'Payout',
      meta: {
        loginRequired: true,
        role: UserRole.RATER
      }
    },
    {

      path: '/paymentInfo',
      component: PaymentInfo,
      name: 'paymentInfo',
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/submissions',
      component: Submissions,
      name: PageName.SUBMISSIONS,
      meta: {
        loginRequired: true,
        role: UserRole.LEARNER
      }
    },
    {
      path: '/disputes',
      component: LearnerDisputes,
      name: PageName.LEARNER_DISPUTES,
      meta: {
        loginRequired: true,
        role: UserRole.LEARNER
      }
    },
    {
      path: '/admin/questions',
      component: AdminQuestions,
      name: PageName.ADMIN_QUESTIONS,
      meta: {
        loginRequired: true,
        role: UserRole.ADMIN
      }
    },
    {
      path: '/admin/requests',
      component: AdminRequests,
      name: 'Requests Management',
      meta: {
        loginRequired: true,
        role: UserRole.ADMIN
      }
    },
    {
      path: '/admin/samples',
      component: Samples,
      name: PageName.SAMPLES,
      meta: {
        loginRequired: true,
        role: UserRole.ADMIN
      }
    },
    {
      path: '/admin/articles',
      component: Articles,
      name: PageName.ARTICLES,
      meta: {
        loginRequired: true,
        role: UserRole.ADMIN
      }
    },
    {
      path: '/articles',
      component: LearnerArticles,
      name: PageName.LEARNER_ARTICLES,
      meta: {
        loginRequired: true
      }
    },
    {
      path: '/article/:id',
      component: ArticleDetail,
      name: PageName.LEARNER_ARTICLE_DETAIL,
      meta: {
        loginRequired: true
      }
    }
  ]
})

protect(router)

export default router

