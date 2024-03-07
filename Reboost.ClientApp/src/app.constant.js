export const SCORES = {
  IELTS: {
    LISTENING: { sectionId: 5 },
    READING: { sectionId: 6 },
    SPEAKING: { sectionId: 7 },
    WRITING: { sectionId: 8 }
  },
  TOEFL: {
    LISTENING: { sectionId: 1 },
    READING: { sectionId: 2 },
    SPEAKING: { sectionId: 3 },
    WRITING: { sectionId: 4 }
  }
}
export const RATER_STATUS = {
  APPLIED: 'Applied',
  TRAINING: 'Approved for Training',
  DOCUMENT_REQUESTED: 'Document Requested',
  DOCUMENT_SUBMITTED: 'Document Submitted',
  REVISION_REQUESTED: 'Revision Requested',
  TRAINING_COMPLETED: 'Training Completed',
  APPROVED: 'Final Approval',
  REJECTED: 'Rejected',

  REVISION: 'Revision',

  IELTS_TRAINING_REVISION: 'IELTSTrainingRevision',
  TOEFL_TRAINING_REVISION: 'TOEFLTrainingRevision',

  IELTS_REVISION_COMPLETED: 'IELTSRevisionCompleted',
  TOEFL_REVISION_COMPLETED: 'TOEFLRevisionCompleted',
  IELTS_TRAINING_COMPLETED: 'IELTSTrainingSubmitted',
  TOEFT_TRAINING_COMPLETED: 'TOEFLTrainingSubmitted',
  IELTS_TRAINING_APPROVED: 'IELTSTrainingApproved',
  TOEFT_TRAINING_APPROVED: 'TOEFLTrainingApproved'
}
export const REVIEW_REQUEST_STATUS = {
  REQUESTED: 'Requested',
  IN_PROGRESS: 'In Progress',
  COMPLETED: 'Completed',
  RATED: 'Rated',
  WAITING: 'Waiting',
  PAID: 'Paid'
}
export const configs = {
  stripeApiKey: 'pk_test_51I9tu1D04tWYlOu2cVzeLBsGuDK4aRvfkR4tJb18W20hxgtf4989r2JeSbuua653nGkzY6IlFU7JTPYKyqne64VP00CLSFo42c'
  // stripeApiKey: 'pk_test_51IGmHtAuoWGVV1acLck6Xj6Qh79tivaobVsKsxjynHzxO9yKJ5cm7UYp0O4hrd5l7HUFmv4ot2eVpTxyl6IUUGjQ0037pavs1U'
}
export const PageName = {
  LOGIN: 'Login',
  REDIRECT: 'Redirect',
  RATER_LOGIN: 'RaterLogin',
  AFTER_LOGIN: 'AfterLogin',
  REGISTER: 'Register',
  REGISTER_RATER: 'RaterRegister',
  RATERS: 'ManageRaters',
  QUESTIONS: 'Questions',
  RATER_APPLY: 'RaterApply',
  RATER_STATUS: 'ApplicationStatus',
  SELECT_YOUR_TEST: 'SelectYourTest',
  REVIEW: 'Review',
  REVIEWS: 'Reviews',
  DISPUTES: 'Disputes',
  SUBMISSIONS: 'Submissions',
  LEARNER_DISPUTES: 'LearnerDisputes',
  ADMIN_QUESTIONS: 'AdminQuestions',
  NOT_FOUND: 'PageNotFound',
  SAMPLES: 'Samples',
  ARTICLES: 'Articles',
  LEARNER_ARTICLES: 'LearnerArticles',
  LEARNER_ARTICLE_DETAIL: 'LearnerArticleDetail'
}

export const UserRole = {
  ADMIN: 'admin',
  LEARNER: 'learner',
  RATER: 'rater'
}

export const SubmissionStatus = {
  PENDING: 'Pending',
  SUBMITTED: 'Submitted',
  REVIEW_REQUESTED: 'Review Requested',
  REVIEWED: 'Reviewed',
  COMPLETED: 'Completed'
}
export const RATER_TRAINING_STATUS = {
  APPROVED: 'Approved',
  IN_PROGRESS: 'In Progress',
  COMPLETED: 'Completed',
  REVISION_REQUEST: 'Revision Requested',
  REVISION_COMPLETED: 'Revision Completed'
}
export const DISPUTE_STATUS = {
  OPEN: 'Open',
  ACCEPTED: 'Accepted',
  DENIED: 'Denied',
  REFUNDED: 'Refunded'
}

export const QUESTION_STATUS = {
  ACTIVE: 'Active',
  CONTRIBUTED: 'Contributed',
  APPROVED: 'Approved'
}

export const SUBSCRIPTION_PLANS = [
  {
    id: 1,
    pId: 'P-6N9586386D9350704MFFTCIQ',
    name: 'year',
    price: 159.00,
    description: 'Our most popular plan previously sold for $299 and is now only $13/month. This plan saves you over 60% in comparison to the monthly plan.'
  },
  {
    id: 2,
    pId: 'P-2XC42867D76575918MFFP53Q',
    name: 'month',
    price: 39.00,
    description: 'Down from $39/month. Our monthly plan grants access to all premium features, the best plan for short-term subscribers.'
  }
]

export const TEST_TYPES = {
  IELTS: 'IELTS',
  TOEFL: 'TOEFL'
}

export const SAMPLE_STATUS = {
  CONTRIBUTED: 'Contributed',
  APPROVED: 'Approved'
}

export const ARTICLE_CATEGORIES = {
  IELTS_WRITINGS: 'IELTS Writings',
  TOEFL_WRITINGS: 'TOEFL Writings',
  TEST_PREPARATION: 'Test Preparation',
  STUDY_GUIDE: 'Study Guide',
  GENERAL_DISCUSSION: 'General Discussion',
  SUPPORT_FEEDBACK: 'Support & Feedback'
}
