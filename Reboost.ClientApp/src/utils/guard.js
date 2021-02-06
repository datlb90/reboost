export function RequestSpamGuard() {
  const mTimeOut = 10 * 1000
  const mLimit = 3

  let mRemaining = mTimeOut
  let mIntervalHld
  let mCurrentCount = 0

  const localStoreKey = 'SPAM_CHECK_KEY'

  const load = () => {
    const data = localStorage.getItem(localStoreKey)
    if (data) {
      const dataParsed = JSON.parse(data)

      if (dataParsed.remaining > 0) {
        mCurrentCount = dataParsed.count
        mRemaining = dataParsed.remaining
        mIntervalHld = setInterval(countDown, 1000)
      }
    }
  }

  const countDown = () => {
    save()
    if (mRemaining > 0) {
      mRemaining -= 1000
    } else {
      console.log('reseted')
      mCurrentCount = 0
      clearTimeout(mIntervalHld)
      mIntervalHld = undefined
      mRemaining = mTimeOut
    }
  }

  const save = () => {
    localStorage.setItem(
      localStoreKey,
      JSON.stringify({ count: mCurrentCount, remaining: mRemaining })
    )
  }

  this.check = () => {
    console.log('Spam check')
    return new Promise((resolve, reject) => {
      mCurrentCount++
      // save()

      if (!mIntervalHld) {
        mIntervalHld = setInterval(countDown, 1000)
      }
      if (mCurrentCount > mLimit) {
        reject()
      } else {
        resolve()
      }
    })
  }

  load()
}
