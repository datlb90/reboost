<template>
  <!-- Start Main Banner -->
  <div id="banner" class="main-banner" style="height: 860px; padding-top: 180px; padding-bottom: 80px;">
    <div class="container">

      <el-steps :active="2" style="margin-top: 60px; margin-bottom: 60px; color: #6084a4 !important;">
        <el-step title="Step 1" description="Create an account" icon="far fa-user-circle" />
        <el-step title="Step 2" description="Upload credentials" icon="fas fa-file-upload" />
        <el-step title="Step 3" description="Complete tranning" icon="far fa-check-circle" />
        <el-step title="Step 4" description="Start rating" icon="fas fa-edit" />
      </el-steps>

      <div class="row">
        <div class="col-lg-7 col-md-12">
          <div class="ml-banner-content" style="margin-top: 10px;">
            <h1>Become a Rater with Reboost</h1>
            <el-steps :active="4" style="margin-top: 60px; margin-bottom: 60px; color: #6084a4 !important;">
              <el-step title="Step 1" description="Create an account" icon="far fa-user-circle" />
              <el-step title="Step 2" description="Upload credentials" icon="fas fa-file-upload" />
              <el-step title="Step 3" description="Complete tranning" icon="far fa-check-circle" />
              <el-step title="Step 4" description="Start rating" icon="fas fa-edit" />
            </el-steps>
            <p style="max-width: 100%;">Our raters are teachers, instructors, test makers, certified examiners, and industry professionals, who are all passionate about their areas of expertise and eager to help test takers improve their writing skill and test score.</p>
            <p style="max-width: 100%;">Our online evaluation platform makes connecting with test takers and earning extra income simple, convenient, and flexible.</p>
          </div>
        </div>

        <div class="col-lg-5 col-md-12">
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

              <button type="submit" class="btn btn-primary" style="width: 100%; margin-top: 10px;">Apply Now</button>
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

<style>
.el-step__icon.is-icon{
  font-size: 30px;
}

.el-step__head.is-finish {
    color: #6e8fac !important;
    border-color: #6e8fac !important;
}

.el-step__title.is-finish{
   color: #6e8fac !important;
}

.el-step__description.is-finish {
    color: #6e8fac !important;
}

</style>

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
