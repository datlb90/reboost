
import store from '../../store'
import reviewService from '../../services/review.service'
export function isAdmin() {
  const currentUser = store.getters['auth/getUser']
  if (currentUser.role == 'Admin') {
    return true
  }
  return false
}

export async function UserReviewAuthentication(reviewId) {
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

