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
  TRAINING: 'Training',
  APPROVED: 'Approved',
  REJECTED: 'Rejected',
  REVISION: 'Revision',
  DOCUMENT_REQUESTED: 'DocumentRequested',
  DOCUMENT_SUBMITTED: 'DocumentSubmitted',
  REVISION_REQUESTED: 'RevisionRequested',
  REVISION_COMPLETED: 'RevisionCompleted',
  TRAINING_APPROVED: 'TrainingApproved',
  DOCUMENT_COMPLETED: 'DocumentCompleted'
}
export const REVIEW_REQUEST_STATUS = {
  IN_PROGRESS: 'In Progress',
  COMPLETED: 'Completed',
  RATED: 'Rated'
}
export const configs = {
  stripeApiKey: 'pk_test_51I9tu1D04tWYlOu2cVzeLBsGuDK4aRvfkR4tJb18W20hxgtf4989r2JeSbuua653nGkzY6IlFU7JTPYKyqne64VP00CLSFo42c'
  // stripeApiKey: 'pk_test_51IGmHtAuoWGVV1acLck6Xj6Qh79tivaobVsKsxjynHzxO9yKJ5cm7UYp0O4hrd5l7HUFmv4ot2eVpTxyl6IUUGjQ0037pavs1U'
}
export const PageName = {
  LOGIN: 'Login',
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
