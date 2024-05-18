<template>
  <section id="pricing" class="features-area-two ptb-80" style="margin-top: 60px; min-height: 100vh;">
    <div class="container">
      <div class="section-title" style="margin-bottom: 40px;">
        <h1 style="font-size: 30px;">Bảng Giá</h1>
        <div class="bar" />
        <div style="text-align: center;">
          <div class="menu-btns" style="margin-top: 30px;">
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light active">
              6 tháng
            </a>
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light">
              3 tháng
            </a>
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light">
              1 tháng
            </a>
          </div>
        </div>
      </div>

      <div class="row">
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
              <a href="#" class="btn btn-primary">Select Plan</a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table active-plan">
            <div class="pricing-header">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>100.000<span>/THÁNG</span></span>
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
              <a href="#" class="btn btn-primary">Select Plan</a>
            </div>
          </div>
        </div>
        <div class="col-lg-4 col-md-6 col-sm-6 offset-lg-0 offset-md-3 offset-sm-3">
          <div class="pricing-table">
            <div class="pricing-header">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price">
              <span><sup>đ</sup>250.000<span>/THÁNG</span></span>
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
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary">Select Plan</a>
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
        selectedMonth: '6 tháng'
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
