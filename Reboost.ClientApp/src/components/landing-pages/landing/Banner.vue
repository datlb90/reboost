<template>
  <!-- Start Main Banner -->
  <div id="banner" class="ml-main-section" style=" padding-top: 150px; padding-bottom: 80px;">
    <div class="container">
      <div class="row">
        <div class="col-lg-6 col-md-12">
          <div class="ml-banner-content" style="margin-top: 50px;">
            <h1>A New Way to Learn</h1>
            <p>Reboost is the best platform to help you boost your language writing test scores.
              No matter which test you take, we will get you fully prepared and guarantee your score improvement by offering:</p>
            <ul>
              <li>Unlimited practice tests</li>
              <li>Active community that encourages you to practice more</li>
              <li>Opportunity to learn by providing feedback</li>
              <li>Useful and constructive feedback to help you improve</li>
            </ul>

            <!-- <a href="#" class="btn btn-secondary">Get Started</a> -->
          </div>
        </div>

        <!-- <div class="col-lg-6 col-md-12">
          <div class="ml-banner-image" style="position: initial;">
            <img v-wow src="https://www.exordo.com/img/score-cards-top.png" class="wow fadeIn" data-wow-delay="2s" alt="image" style="left: 20px;">
          </div>
        </div> -->

        <div class="col-lg-5 offset-lg-1">
          <div class="banner-form ml-3">
            <form>
              <div class="form-group">
                <label>Username</label>
                <input type="text" class="form-control" placeholder="Enter username">
              </div>

              <div class="form-group">
                <label>Email</label>
                <input type="email" class="form-control" placeholder="Enter your email">
              </div>

              <div class="form-group">
                <label>Password</label>
                <input type="password" class="form-control" placeholder="Create a password">
              </div>

              <button type="submit" class="btn btn-primary" style="width: 100%; margin-top: 10px;">Register Now</button>
            </form>
          </div>
        </div>

      </div>
    </div>

    <div class="shape1"><img src="../../../assets/img/shape1.png" alt="shape"></div>
    <div class="shape3"><img src="../../../assets/img/shape3.svg" alt="shape"></div>
    <div class="shape4"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape6 rotateme"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape7"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape8 rotateme"><img src="../../../assets/img/shape2.svg" alt="shape"></div>
  </div>
  <!-- End ML Main Banner -->
</template>

<script>
export default {
  name: 'Banner'
}
</script>
<style scoped>
.banner-form{
  background: #ffffff;
    -webkit-box-shadow: 0 2px 48px 0 rgba(0, 0, 0, 0.08);
    box-shadow: 0 2px 48px 0 rgba(0, 0, 0, 0.08);
    padding: 50px 30px;
    border-radius: 5px;
}

.ml-main-section .ml-banner-content ul {
    padding: 0;
    margin: 0;
    list-style-type: none;
}

.ml-main-section .ml-banner-content ul li{
     margin-bottom: 12px;
     position: relative;
     color: #6084a4;
     font-size: 16px;
     padding-left: 30px;
}

.ml-main-section .ml-banner-content ul li::before {
    position: absolute;
    left: 0;
    top: 50%;
    -webkit-transform: translateY(-50%);
    transform: translateY(-50%);
    width: 20px;
    height: 2px;
    background: #44ce6f;
    content: '';
}

.ml-banner-content p {
    font-size: 16px;
    max-width: 490px;
}

</style>
