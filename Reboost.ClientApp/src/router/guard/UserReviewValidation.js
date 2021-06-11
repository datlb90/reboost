
import store from '../../store'
import reviewService from '../../services/review.service'
import { RATER_STATUS } from '../../app.constant'
export function isAdmin() {
  const currentUser = store.getters['auth/getUser']
  if (currentUser.role == 'Admin') {
    return true
  }
  return false
}

export async function userReviewAuthentication(reviewId) {
  return new Promise((resolve, reject) => {
    reviewService.userReviewAuth(reviewId).then(r => {
      if (typeof (r) != 'undefined' && r.status == 200) {
        resolve(false)
      } else {
        resolve(true)
      }
    })
  })
}

export async function isApprovedRater() {
  return new Promise((resolve, reject) => {
    const currentUser = store.getters['auth/getUser']

    if (currentUser.role && (currentUser.role.toLowerCase() === 'rater' || currentUser.role.toLowerCase() === 'admin')) {
      reviewService.raterApprovedCheck().then(r => {
        console.log(r)
        if (r.status === RATER_STATUS.APPROVED || currentUser.role.toLowerCase() === 'admin') {
          resolve({ code: 2, rater: r })
        }
        if (!r) {
          resolve({ code: 0, rater: r })
        }
        if (r.status === RATER_STATUS.REJECTED) {
          resolve({ code: -1, rater: r })
        } else {
          resolve({ code: 1, rater: r })
        }
      })
    } else {
      resolve({ code: -1, rater: null })
    }
  })
}

export async function revieweeReviewAuthentication(reviewId) {
  return new Promise((resolve, reject) => {
    reviewService.revieweeReviewAuth(reviewId).then(r => {
      console.log('reivewee auth', r)
      if (typeof (r) != 'undefined' && r.status == 200) {
        resolve(r.data)
      } else {
        resolve(0)
      }
    })
  })
}

