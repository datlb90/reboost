<template>
  <div>
    <!-- Start Coming Soon Area -->
    <section class="coming-soon-area">
      <div class="d-table">
        <div class="d-table-cell">
          <div class="container">
            <div class="coming-soon-content">
              <h1>Under Construction</h1>
              <p>Our website is currently undergoing scheduled maintenance. We Should be back shortly. Thank you for your patience.</p>

              <form>
                <input id="email" type="email" name="email" class="email-input" placeholder="Enter your email">
                <button type="submit" class="submit-btn">Notify Us</button>
              </form>

              <div id="timer">
                <div id="days">{{ days }} <span>Days</span></div>
                <div id="hours">{{ hours }} <span>Hours</span></div>
                <div id="minutes">{{ minutes }} <span>Minutes</span></div>
                <div id="seconds">{{ seconds }} <span>Seconds</span></div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <ul class="social-list">
        <li class="list-heading">Follow us for update:</li>
        <li><a href="#" class="facebook"><feather type="facebook" /></a></li>
        <li><a href="#" class="twitter"><feather type="twitter" /></a></li>
        <li><a href="#" class="linkedin"><feather type="linkedin" /></a></li>
        <li><a href="#" class="instagram"><feather type="instagram" /></a></li>
      </ul>
    </section>
    <!-- End Coming Soon Area -->
  </div>
</template>

<script>
export default {
  name: 'ComingSoon',
  data() {
    return {
      days: '',
      hours: '',
      minutes: '',
      seconds: ''
    }
  },
  created() { // Turn data into viewable values
    setInterval(() => {
      this.commingSoonTime()
    }, 1000)
  },
  methods: {
    commingSoonTime() {
      const endTime = new Date('August 23, 2020 17:00:00 PDT')
      const endTimeParse = (Date.parse(endTime)) / 1000
      const now = new Date()
      const nowParse = (Date.parse(now) / 1000)
      const timeLeft = endTimeParse - nowParse
      const days = Math.floor(timeLeft / 86400)
      let hours = Math.floor((timeLeft - (days * 86400)) / 3600)
      let minutes = Math.floor((timeLeft - (days * 86400) - (hours * 3600)) / 60)
      let seconds = Math.floor((timeLeft - (days * 86400) - (hours * 3600) - (minutes * 60)))
      if (hours < '10') { hours = '0' + hours }
      if (minutes < '10') { minutes = '0' + minutes }
      if (seconds < '10') { seconds = '0' + seconds }
      this.days = days
      this.hours = hours
      this.minutes = minutes
      this.seconds = seconds
    }
  }
}
</script>
