<template>
  <section id="pricing" class="features-area-two ptb-80" style="margin-top: 50px; min-height: 100vh;">
    <div class="container">
      <div class="section-title" style="margin-bottom: 40px;">
        <h1 style="font-size: 30px;">Bảng Giá</h1>
        <div class="bar" />
        <div style="text-align: center;">
          <div class="menu-btns" style="margin-top: 30px; margin-left: 20px;">
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(6) }" @click="selectDuration(6)">
              6 tháng
            </a>
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(3) }" @click="selectDuration(3)">
              3 tháng
            </a>
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(1) }" @click="selectDuration(1)">
              1 tháng
            </a>
          </div>
        </div>
      </div>

      <div v-if="activeDuration == 6" class="row">
        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table">
            <div class="pricing-header">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">5 Bài Chấm</li>
                <li class="active">Highest Speed</li>
                <li class="active">1 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li>Data Security and Backups</li>
                <li>Monthly Reports and Analytics</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(0) }" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(1) }">
            <div class="pricing-header">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>99.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">Không giới hạn bài chấm</li>
                <li class="active">Highest Speed</li>
                <li class="active">3 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li class="active">Data Security and Backups</li>
                <li>Monthly Reports and Analytics</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(1) }" @click="selectPlan(1)">
                {{ getOptionText(1) }}
              </a>
            </div>
          </div>
        </div>
        <div class="col-lg-4 col-md-6 col-sm-6 offset-lg-0 offset-md-3 offset-sm-3">
          <div class="pricing-table" :class="{ 'active-plan': planActive(4) }">
            <div class="pricing-header">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>199.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">15 GB Bandwidth</li>
                <li class="active">Highest Speed</li>
                <li class="active">5 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li class="active">Data Security and Backups</li>
                <li class="active">Monthly Reports and Analytics</li>
                <li v-if="!planDisabled(4)" class="active">Nhận lại <span><sup>đ</sup>{{ proratedAmount / 1000 }}.000<span /></span> từ gói hiện tại</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(4) }" @click="selectPlan(4)">
                {{ getOptionText(4) }}
              </a>
            </div>
          </div>
        </div>
      </div>

      <div v-else-if="activeDuration == 3" class="row">
        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table">
            <div class="pricing-header">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">5 Bài Chấm</li>
                <li class="active">Highest Speed</li>
                <li class="active">1 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li>Data Security and Backups</li>
                <li>Monthly Reports and Analytics</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(0) }" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(2) }">
            <div class="pricing-header">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>139.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">Không giới hạn bài chấm</li>
                <li class="active">Highest Speed</li>
                <li class="active">3 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li class="active">Data Security and Backups</li>
                <li>Monthly Reports and Analytics</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(2) }" @click="selectPlan(2)">
                {{ getOptionText(2) }}
              </a>
            </div>
          </div>
        </div>
        <div class="col-lg-4 col-md-6 col-sm-6 offset-lg-0 offset-md-3 offset-sm-3">
          <div class="pricing-table" :class="{ 'active-plan': planActive(5) }">
            <div class="pricing-header">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>249.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">15 GB Bandwidth</li>
                <li class="active">Highest Speed</li>
                <li class="active">5 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li class="active">Data Security and Backups</li>
                <li class="active">Monthly Reports and Analytics</li>
                <li v-if="!planDisabled(5)" class="active">Nhận lại <span><sup>đ</sup>{{ proratedAmount / 1000 }}.000<span /></span> từ gói hiện tại</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(5) }" @click="selectPlan(5)">
                {{ getOptionText(5) }}
              </a>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="row">
        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table">
            <div class="pricing-header">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">5 Bài Chấm</li>
                <li class="active">Highest Speed</li>
                <li class="active">1 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li>Data Security and Backups</li>
                <li>Monthly Reports and Analytics</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(0) }" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(3) }">
            <div class="pricing-header">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>179.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">Không giới hạn bài chấm</li>
                <li class="active">Highest Speed</li>
                <li class="active">3 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li class="active">Data Security and Backups</li>
                <li>Monthly Reports and Analytics</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(3) }" @click="selectPlan(3)">
                {{ getOptionText(3) }}
              </a>
            </div>
          </div>
        </div>
        <div class="col-lg-4 col-md-6 col-sm-6 offset-lg-0 offset-md-3 offset-sm-3">
          <div class="pricing-table" :class="{ 'active-plan': planActive(6) }">
            <div class="pricing-header">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>299.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul>
                <li class="active">15 GB Bandwidth</li>
                <li class="active">Highest Speed</li>
                <li class="active">5 GB Storage</li>
                <li class="active">Unlimited Website</li>
                <li class="active">Unlimited Users</li>
                <li class="active">24x7 Great Support</li>
                <li class="active">Data Security and Backups</li>
                <li class="active">Monthly Reports and Analytics</li>
                <li v-if="!planDisabled(6)" class="active">Nhận lại <span><sup>đ</sup>{{ proratedAmount / 1000 }}.000<span /></span> từ gói hiện tại</li>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(6) }" @click="selectPlan(6)">
                {{ getOptionText(6) }}
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>

  </section>
</template>

  <script>
  export default {
    name: 'Pricing',
    data() {
      return {
        screenWidth: window.innerWidth,
        activePlan: 1, // 6 tháng chi tiết
        activeDuration: 6,
        user: this.$store.state.auth.user,
        // userSubscription: null
        userSubscription: {
          planId: 2, // 6 plans in total: 1, 2, 3, 4, 5, 6
          duration: 3, // 3 durations: 6, 3, 1
          proratedAmount: 200000 // unused amount (should be substracted by upgrade)
        },
        proratedAmount: 0
      }
    },
    watch: {
      screenWidth(newWidth) {
        this.screenWidth = newWidth
      }
    },
    mounted() {
      window.addEventListener('resize', () => {
        this.screenWidth = window.innerWidth
      })

      // Get user subscription information that display the option accordingly
      // this.userSubscription = await subscriptionService.getUserSubscription(this.user.id)
      if (this.userSubscription) {
        this.activePlan = this.userSubscription.planId
        this.activeDuration = this.userSubscription.duration
        this.proratedAmount = this.userSubscription.proratedAmount
      }
    },
    methods: {
      selectPlan(planId) {
        if (!this.user) {
          // save plan id
          // send to register
          // send to payment page if plan id != 0
        } else {
          // check if this is add new, renew, or upgrade subscription
          // send user to the payment page accodingly
          // subscribe user to plan

        }
      },
      getOptionText(planId) {
        if (this.user && this.userSubscription) {
          if (planId == 0) { return 'Lựa Chọn' }
          if ((this.activePlan <= 3 && planId <= 3) || (this.activePlan >= 4 && planId >= 4)) { return 'Gia Hạn' }
          if (this.activePlan <= 3 && planId >= 4) { return 'Nâng Cấp' }
        }
        return 'Lựa Chọn'
      },
      planDisabled(planId) {
        if (this.user && this.userSubscription) {
          if (planId == 0) { return true }
          if (this.userSubscription.duration > this.activeDuration) {
            if (this.activePlan <= 3 && planId >= 4) { return true }
          }
          if (this.activePlan >= 4 && planId <= 3) { return true }
        }
        return false
      },
      planActive(planId) {
        if (planId == this.activePlan) { return true }
        return false
      },
      durationActive(duration) {
        if (duration == this.activeDuration) { return true }
        return false
      },
      selectDuration(duration) {
        this.activeDuration = duration
      }
    }
  }
  </script>

  <style scoped>
  .nav-item:active{
    color: #44ce6f;
  }

  .lang-dropdown-menu{
    z-index: 10000 !important;
  }

  .nav-item a {
    font-weight: 500;
    font-size: 15px;

  }

  .menu-btns .btn.btn-light {
      background: transparent;
      border: 2px dashed #cdf1d8;
  }

  .menu-btns .btn.btn-light.active {
    background: #27bebe;
    color: white;
    border: 2px dashed #cdf1d8;
  }

  .menu-btns .btn.btn-light::before {
    background: #27bebe;
  }

  .menu-btns .btn.btn-light::after {
    background: #27bebe;
  }

  .menu-btns .btn.btn-light:hover{
    color: white;
  }

  .menu-btns .btn.btn-light:hover,
  .menu-btns .btn.btn-light:focus {
    border-color: #27bebe;
  }
  </style>
