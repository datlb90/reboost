import { Question } from './client/question'
import { Review } from './rater/review'
import { Submissions } from './client/submission'
import { WhyChooseUs } from './home/whyChooseUs'
import { Login } from './client/login'
import { RaterRegister } from './rater/raterRegister'
import { Register } from './client/register'
import { Disputes } from './admin/disputes'
import { CONSTANT } from './constant'
import { AddEditQuestion } from './controls/addEditQuestion'
import { Banner } from './home/banner'
import { BoxesArea } from './home/boxesArea'
import { HowItWork2 } from './home/howItWork2'
import { Funfacts } from './home/funfacts'
import { FeaturesArea } from './home/featuresArea'
import { Features } from './home/features'
import { RevisionCenter } from './home/revisionCenter'
import { Matching } from './home/matching'
import { Feedback } from './home/feedback'
import { Blog } from './home/blog'
import { FAQ } from './home/faq'
import { AddQuestionSampleDialog } from './controls/addQuestionSampleDialog'
import { ApplicationDetail } from './admin/applicationDetail'
import { ContactDialog } from './controls/contactDialog'
import { QuestionPreview } from './controls/questionPreview'
import { ViewerToolBar } from './controls/viewerToolBar'
import { ManageRaters } from './admin/manageRaters'
import { Questions } from './admin/questions'
import { Samples } from './admin/samples'
import { AdminArticles } from './admin/articles'
import { AddArticle } from './admin/addArticle'
import { LandingHeader } from './home/landingHeader'
import { Checkout } from './checkout/checkout'
import { UnlimittedTopics } from './home/unlimittedTopics'
import { ProRater } from './home/proRater'
import { Pricing } from './home/pricing'

const langVN = {
  question: Question,
  submission: Submissions,
  review: Review,
  login: Login,
  banner: Banner,
  raterRegister: RaterRegister,
  boxesArea: BoxesArea,
  howItWork2: HowItWork2,
  whyChooseUs: WhyChooseUs,
  funfacts: Funfacts,
  featuresArea: FeaturesArea,
  features: Features,
  revisionCenter: RevisionCenter,
  matching: Matching,
  feedback: Feedback,
  blog: Blog,
  faq: FAQ,
  register: Register,
  disputes: Disputes,
  constant: CONSTANT,
  addEditQuestion: AddEditQuestion,
  addQuestionSampleDialog: AddQuestionSampleDialog,
  appDetail: ApplicationDetail,
  contactDialog: ContactDialog,
  questionPreview: QuestionPreview,
  viewerToolBar: ViewerToolBar,
  manageRaters: ManageRaters,
  adminQuestions: Questions,
  adminSamples: Samples,
  adminArticles: AdminArticles,
  addArticle: AddArticle,
  landingHeader: LandingHeader,
  checkout: Checkout,
  unlimittedTopics: UnlimittedTopics,
  proRater: ProRater,
  pricing: Pricing
}

export default langVN
