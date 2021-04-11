import Vue from 'vue'
import VueRouter from 'vue-router'

import About from '../views/About.vue'
import Document from '../views/Document.vue'
import Test from '../views/Test.vue'
import RaterApply from '../views/rater/Application.vue'
import Login from '../views/account/Login.vue'
import Register from '../views/account/Register.vue'
import RaterRegister from '../views/account/RaterRegister.vue'
import AdminHome from '../views/admin/AdminHome.vue'
import Review from '../views/learner/Review.vue'
import ReviewTest from '../views/learner/ReviewTest.vue'
// Pages
import Landing from '../components/landing-pages/Landing'
import RaterLanding from '../components/landing-pages/Rater'

import ITStartup from '../components/landing-pages/ITStartup'
import Developer from '../components/landing-pages/Developer'
import WebHosting from '../components/landing-pages/WebHosting'
import RepairCenter from '../components/landing-pages/RepairCenter'
import Iot from '../components/landing-pages/Iot'
import AiMachineLearning from '../components/landing-pages/AiMachineLearning'
import MachineLearning from '../components/landing-pages/MachineLearning'
import DigitalAgency from '../components/landing-pages/DigitalAgency'
import AboutStyleOne from '../components/other-pages/about/AboutStyleOne'
import AboutStyleTwo from '../components/other-pages/about/AboutStyleTwo'
import AboutStyleThree from '../components/other-pages/about/AboutStyleThree'
import Features from '../components/other-pages/features/Features'
import FeatureDetails from '../components/other-pages/features/FeatureDetails'
import ServicesOne from '../components/other-pages/services/ServicesOne'
import ServicesTwo from '../components/other-pages/services/ServicesTwo'
import ServicesThree from '../components/other-pages/services/ServicesThree'
import ServicesFour from '../components/other-pages/services/ServicesFour'
import ServicesFive from '../components/other-pages/services/ServicesFive'
import ServiceDetails from '../components/other-pages/services/ServiceDetails'
import ProjectStyleOne from '../components/other-pages/projects/ProjectStyleOne'
import ProjectStyleTwo from '../components/other-pages/projects/ProjectStyleTwo'
import ProjectDetails from '../components/other-pages/projects/ProjectDetails'

import Team from '../components/other-pages/team/Team'
import Pricing from '../components/other-pages/pricing/Pricing'
import Faq from '../components/other-pages/faq/Faq'
import NotFound from '../components/other-pages/not-found/NotFound'
import ComingSoon from '../components/other-pages/coming-soon/ComingSoon'
import BlogGrid from '../components/other-pages/blog/BlogGrid'
import BlogRightSidebar from '../components/other-pages/blog/BlogRightSidebar'
import BlogGridTwo from '../components/other-pages/blog/BlogGridTwo'
import BlogRightSidebarTwo from '../components/other-pages/blog/BlogRightSidebarTwo'
import BlogGridThree from '../components/other-pages/blog/BlogGridThree'
import BlogRightSidebarThree from '../components/other-pages/blog/BlogRightSidebarThree'
import BlogDetails from '../components/other-pages/blog/BlogDetails'
import Contact from '../components/other-pages/contact/Contact'
import Shop from '../components/other-pages/product/Shop'
import Cart from '../components/other-pages/product/Cart'
import Checkout from '../components/other-pages/product/Checkout'
import ItemDetails from '../components/other-pages/product/ItemDetails'
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

import PaymentInfo from '../views/rater/PaymentInfo.vue'
import Submissions from '../views/learner/Submissions.vue'

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
      name: 'RaterApply',
      component: RaterApply,
      meta: {
        plainLayout: false
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
      name: 'Login',
      component: Login,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/register',
      name: 'Register',
      component: Register,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/rater/register',
      name: 'RaterRegister',
      component: RaterRegister,
      meta: {
        plainLayout: true,
        landingPage: false
      }
    },
    {
      path: '/review/:questionId/:docId/:reviewId',
      name: 'Review',
      component: Review,
      meta: {
        plainLayout: false,
        landingPage: false
      }
    },
    {
      path: '/review/test',
      name: 'Review Test',
      component: ReviewTest,
      meta: {
        plainLayout: false,
        landingPage: false
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
      component: AdminHome
    },
    {
      path: '/admin/raters',
      name: 'ManageRaters',
      component: ManageRaters
    },
    {
      path: '/questions',
      name: 'Questions',
      component: Questions
    },
    {
      path: '/rater/application',
      name: 'RaterApplication',
      component: RaterApplication
    },
    {
      path: '/admin/raters/application/:id',
      component: ApplicationDetail,
      name: 'ApplicationDetail'
    },
    {
      path: '/rater/application/status/:id',
      component: ApplicationStatus,
      name: 'ApplicationStatus'
    },
    {
      path: '/PracticeWriting/:id',
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
      }]
    },
    {
      path: '/SelectYourTest',
      component: SelectYourTest,
      name: 'SelectYourTest'
    },
    {
      path: '/Subscribe',
      component: Subscribe,
      name: 'Subscribe'
    },
    {
      path: '/rater/startRating',
      component: StartRating,
      name: 'StartRating'
    },
    {
      path: '/rater/payout',
      component: Payout,
      name: 'Payout'
    },

    {

      path: '/paymentInfo',
      component: PaymentInfo,
      name: 'paymentInfo'
    },
    {
      path: '/submissions',
      component: Submissions,
      name: 'Submissions'
    },

    { path: '/it-startup', component: ITStartup },
    { path: '/developer', component: Developer },
    { path: '/web-hosting', component: WebHosting },
    { path: '/repair-center', component: RepairCenter },
    { path: '/iot', component: Iot },
    { path: '/ai-machine-learning', component: AiMachineLearning },
    { path: '/machine-learning', component: MachineLearning },
    { path: '/digital-agency', component: DigitalAgency },
    { path: '/about-style-one', component: AboutStyleOne },
    { path: '/about-style-two', component: AboutStyleTwo },
    { path: '/about-style-three', component: AboutStyleThree },
    { path: '/features', component: Features },
    { path: '/feature-details', component: FeatureDetails },
    { path: '/service-style-one', component: ServicesOne },
    { path: '/service-style-two', component: ServicesTwo },
    { path: '/service-style-three', component: ServicesThree },
    { path: '/service-style-four', component: ServicesFour },
    { path: '/service-style-five', component: ServicesFive },
    { path: '/service-details', component: ServiceDetails },
    { path: '/project-style-one', component: ProjectStyleOne },
    { path: '/project-style-two', component: ProjectStyleTwo },
    { path: '/project-details', component: ProjectDetails },
    { path: '/team', component: Team },
    { path: '/pricing', component: Pricing },
    { path: '/faq', component: Faq },
    { path: '/coming-soon', component: ComingSoon },
    { path: '/not-found', component: NotFound },
    { path: '/blog-grid', component: BlogGrid },
    { path: '/blog-right-sidebar', component: BlogRightSidebar },
    { path: '/blog-grid-two', component: BlogGridTwo },
    { path: '/blog-right-sidebar-two', component: BlogRightSidebarTwo },
    { path: '/blog-grid-three', component: BlogGridThree },
    { path: '/blog-right-sidebar-three', component: BlogRightSidebarThree },
    { path: '/blog-details', component: BlogDetails },
    { path: '/contact', component: Contact },
    { path: '/shop', component: Shop },
    { path: '/cart', component: Cart },
    { path: '/checkout', component: Checkout },
    { path: '/details', component: ItemDetails }
  ]
})

export default router

