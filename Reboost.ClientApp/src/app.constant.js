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
  IN_PROGRESS: 'In Progress',
  COMPLETED: 'Completed',
  RATED: 'Rated',
  WAITING: 'Waiting'
}
export const configs = {
  stripeApiKey: 'pk_test_51I9tu1D04tWYlOu2cVzeLBsGuDK4aRvfkR4tJb18W20hxgtf4989r2JeSbuua653nGkzY6IlFU7JTPYKyqne64VP00CLSFo42c'
  // stripeApiKey: 'pk_test_51IGmHtAuoWGVV1acLck6Xj6Qh79tivaobVsKsxjynHzxO9yKJ5cm7UYp0O4hrd5l7HUFmv4ot2eVpTxyl6IUUGjQ0037pavs1U'
}
export const PageName = {
  LOGIN: 'Login',
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
  SUBMISSIONS: 'Submissions',
  NOT_FOUND: 'PageNotFound'
}
export const Role = {
  ADMIN: 'Admin'
}
export const UserRole = {
  ADMIN: 'Admin',
  LEARNER: 'Learner',
  RATER: 'Rater'
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
