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
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(0) }">
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
                <!-- <li v-if="proratedAmount != 0 && !planDisabled(4)" class="active">Nhận lại <span><sup>đ</sup>{{ proratedAmount / 1000 }}.000<span /></span> từ gói hiện tại</li> -->
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
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(0) }">
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
                <!-- <li v-if="proratedAmount != 0 && !planDisabled(4)" class="active">Nhận lại <span><sup>đ</sup>{{ proratedAmount / 1000 }}.000<span /></span> từ gói hiện tại</li> -->
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
              <a href="#" class="btn btn-primary" :class="{ 'disabled': planDisabled(0) }">
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
                <!-- <li v-if="proratedAmount != 0 && !planDisabled(4)" class="active">Nhận lại <span><sup>đ</sup>{{ proratedAmount / 1000 }}.000<span /></span> từ gói hiện tại</li> -->
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

    <el-dialog
      id="checkout-dialog"
      :visible.sync="checkoutDialogVisiable"
      width="620px"
      height="820px"
      @closed="dialogClosed"
    >
      <div slot="title">
        <div style="padding: 5px 20px; font-size: 18px; text-align:center;">Thanh Toán</div>
      </div>
      <div class="dialog-body">
        <div style="padding: 20px; padding-top: 0px;">
          <div>
            <!-- <div style="font-size: 16px;"><b>Chi Tiết Đơn Hàng:</b></div> -->
            <el-table :data="orderSummary">
              <el-table-column prop="planName" label="Gói Phản Hồi">
                <template v-if="scope.row" slot-scope="scope">
                  <div>{{ scope.row.planName }}</div>
                </template>
              </el-table-column>
              <el-table-column prop="duration" label="Thời hạn" width="80">
                <template v-if="scope.row" slot-scope="scope">
                  <div>{{ scope.row.duration + ' tháng' }}</div>
                </template>
              </el-table-column>
              <el-table-column prop="price" label="Giá cho 1 tháng" width="120">
                <template v-if="scope.row" slot-scope="scope">
                  <div>{{ scope.row.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND'}) }}</div>
                </template>
              </el-table-column>

              <el-table-column label="Tổng cộng" width="120">
                <template v-if="scope.row" slot-scope="scope">
                  <div style="float: left;">{{ (scope.row.duration * scope.row.price).toLocaleString('it-IT', { style: 'currency', currency: 'VND'}) }}</div>
                </template>
              </el-table-column>

            </el-table>

            <div v-if="proratedAmount != 0 && type != 'renew'" style="height: 25px; margin-top: 20px;">
              <div style="float: right; width: 110px; ">{{ proratedAmount.toLocaleString('it-IT', { style: 'currency', currency: 'VND'}) }}</div>
              <div style="float: right; width: calc(100% - 110px); text-align: right; padding-right: 10px;">Số tiền nhận lại từ gói hiện tại:</div>
            </div>

            <div v-if="orderSummary && orderSummary.length > 0" style="height: 25px; margin-top: 15px;">
              <div style="float: right; width: 110px; "><b>{{ (orderSummary[0].total).toLocaleString('it-IT', { style: 'currency', currency: 'VND'}) }}</b></div>
              <div style="float: right; width: calc(100% - 110px); text-align: right; padding-right: 10px;"><b>Tổng giá trị đơn hàng:</b></div>
            </div>

            <hr>
            <div style="text-align: center; margin-top: 10px; font-size: 16px;">
              <div><b>Phương Thức Thanh Toán</b></div>
              <div style="text-align: center; padding-top: 20px;">
                <el-button plain style="margin-right: 10px;" @click="submitZaloPayRequest()">
                  <div style="font-size: 16px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                  <img style="height: 20px;" src="../../assets/logo/zalopay.png" alt="logo" class="main-header-logo">
                </el-button>

                <el-button plain style="margin-left: 10px;" @click="submitVNPayRequest()">
                  <div style="font-size: 16px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                  <img style="height: 21px;" src="../../assets/logo/vnpay.png" alt="logo" class="main-header-logo">
                </el-button>
              </div>

            </div>

            <div style="margin-top: 30px; text-align: center;">
              <el-tag style="margin-top: 5px; font-size: 13px; width: 100%;">Email nhận biên lai: {{ user.email }}</el-tag>
              <el-tag v-if="activeDuration > 1" type="success" style="margin-top: 10px; font-size: 13px; width: 100%;">Đơn hàng được hoàn trả trong vòng 30 ngày với bất kỳ lý do gì.</el-tag>
              <el-tag type="info" style="margin-top: 10px; font-size: 13px; width: 100%;">Bằng cách mua gói phản hồi, bạn đồng ý với điều khoản và dịch vụ của Reboost.</el-tag>
            </div>
          </div>
        </div>
        <div slot="footer" class="dialog-footer" />
      </div>
    </el-dialog>

  </section>
</template>

<script>
  export default {
    name: 'Pricing',
    data() {
      return {
        screenWidth: window.innerWidth,
        type: 'new', // new, renew, upgrade
        activePlan: 1, // 6 tháng chi tiết
        activeDuration: 6,
        user: this.$store.state.auth.user,
        // userSubscription: null,
        userSubscription: {
          planId: 4, // 6 plans in total: 1, 2, 3, 4, 5, 6
          duration: 6, // 3 durations: 6, 3, 1
          proratedAmount: 200000 // unused amount (should be substracted by upgrade)
        },
        proratedAmount: 0,
        checkoutDialogVisiable: false,
        orderSummary: []
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
      getTotal() {
        let total = this.orderSummary[0].duration * this.orderSummary[0].price
        if (this.proratedAmount != 0 && this.type != 'renew') { total = total - this.proratedAmount }
        return total
      },
      dialogClosed() {
        this.orderSummary = []
      },
      selectPlan(planId) {
        if (!this.user.id) {
          // save plan id
          // send to register
          // send to payment page if plan id != 0
        } else {
          let planName = 'Phản Hồi Chi Tiết'
          let duration = 6
          let price = 99000
          if (planId == 1) {
            planName = 'Phản Hồi Chi Tiết'
            duration = 6
            price = 99000
          } else if (planId == 2) {
            planName = 'Phản Hồi Chi Tiết'
            duration = 3
            price = 139000
          } else if (planId == 3) {
            planName = 'Phản Hồi Chi Tiết'
            duration = 1
            price = 179000
          } else if (planId == 4) {
            planName = 'Phản Hồi Chuyên Sâu'
            duration = 6
            price = 199000
          } else if (planId == 5) {
            planName = 'Phản Hồi Chuyên Sâu'
            duration = 3
            price = 249000
          } else if (planId == 6) {
            planName = 'Phản Hồi Chuyên Sâu'
            duration = 1
            price = 299000
          }

          const option = this.getOptionText(planId)
          console.log(option)
          if (option != 'Lựa Chọn') {
            planName = option + ' ' + planName
            if (option == 'Gia Hạn') { this.type = 'renew' } else { this.type = 'upgrade' }
          }

          let total = duration * price
          if (this.proratedAmount != 0 && this.type != 'renew') { total = total - this.proratedAmount }

          const order = {
            planName: planName,
            duration: duration,
            price: price,
            total: total
          }
          this.orderSummary.push(order)

          // show plan checkout dialog
          this.checkoutDialogVisiable = true
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
        if (planId == 0) { return true }
        if (this.user && this.userSubscription) {
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
