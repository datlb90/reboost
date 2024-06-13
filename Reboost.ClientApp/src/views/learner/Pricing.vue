<template>
  <section v-if="screenWidth > 780" id="pricing" class="features-area-two ptb-80" :style="user.id ? 'margin-top: 0px; min-height: 100vh; padding-top: 30px;' : 'margin-top: 10px; min-height: 100vh;'">
    <div class="container">
      <div class="section-title" style="margin-bottom: 40px;">
        <h1 v-if="user.id" style="font-size: 30px;">Bảng Giá</h1>
        <h1 v-else style="font-size: 30px;">Bảng Giá</h1>
        <div class="bar" style="margin: 10px auto;" />
        <div style="text-align: center;">
          <div class="menu-btns" style="margin-top: 20px; margin-left: 5px;">
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(6) }" @click="selectDuration(6)">
              6 tháng
            </a>
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(3) }" @click="selectDuration(3)">
              3 tháng
            </a>
            <a style="padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(1) }" @click="selectDuration(1)">
              1 tháng
            </a>
          </div>
        </div>
      </div>

      <div v-if="activeDuration == 6" class="row">
        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price" style="margin-bottom: 20px">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">3 Bài Chấm Miễn Phí</li>
                <li class="active">2 Phản Hồi Chi Tiết</li>
                <li class="active">1 Phản Hồi Chuyên Sâu</li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li>Hoàn Tiền 14 Ngày</li>
                </ul>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary disabled" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(1) }">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>99.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Chấm điểm 4 tiêu chí <br>- Nâng cấp lập luận <br>- Bài chữa tham khảo <br>- Nâng cấp từ vựng</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Sửa các lỗi từ vựng và ngữ pháp <br> nổi bật nhất trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li class="active">
                    Hoàn Tiền 14 Ngày
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>249.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Cung cấp phản hồi cụ thể và <br> chính xác hơn với ChatGPT-4</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Chỉ ra các lỗi sai và cách sửa <br> cho từng câu văn trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">Hỗ Trợ Bản Viết Tay</li>
                <li class="active">AI Chấm Sửa Trong Khi Viết

                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng AI hỗ trợ chấm sửa <br>khi làm bài viết mà không cần trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip>
                </li>
                <li class="active">Phản Hồi Cho IELTS Speaking
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng cung cấp phản hồi <br>cho IELST Speaking mà không phải trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip></li>
              </ul>
              <hr style="margin-left: 40px; margin-right: 40px;">
              <ul style="margin-bottom: 20px;">
                <li class="active">Luyện Tập Không Giới Hạn</li>
                <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                <li class="active">
                  Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Hoàn Tiền 14 Ngày
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price" style="margin-bottom: 20px">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">3 Bài Chấm Miễn Phí</li>
                <li class="active">2 Phản Hồi Chi Tiết</li>
                <li class="active">1 Phản Hồi Chuyên Sâu</li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li>Hoàn Tiền 14 Ngày</li>
                </ul>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary disabled" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(2) }">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>129.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Chấm điểm 4 tiêu chí <br>- Nâng cấp lập luận <br>- Bài chữa tham khảo <br>- Nâng cấp từ vựng</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Sửa các lỗi từ vựng và ngữ pháp <br> nổi bật nhất trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li class="active">
                    Hoàn Tiền 14 Ngày
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>299.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Cung cấp phản hồi cụ thể và <br> chính xác hơn với ChatGPT-4</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Chỉ ra các lỗi sai và cách sửa <br> cho từng câu văn trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">Hỗ Trợ Bản Viết Tay</li>
                <li class="active">AI Chấm Sửa Trong Khi Viết

                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng AI hỗ trợ chấm sửa <br>khi làm bài viết mà không cần trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip>
                </li>
                <li class="active">Phản Hồi Cho IELTS Speaking
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng cung cấp phản hồi <br>cho IELST Speaking mà không phải trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip></li>
              </ul>
              <hr style="margin-left: 40px; margin-right: 40px;">
              <ul style="margin-bottom: 20px;">
                <li class="active">Luyện Tập Không Giới Hạn</li>
                <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                <li class="active">
                  Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Hoàn Tiền 14 Ngày
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price" style="margin-bottom: 20px">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">3 Bài Chấm Miễn Phí</li>
                <li class="active">2 Phản Hồi Chi Tiết</li>
                <li class="active">1 Phản Hồi Chuyên Sâu</li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary disabled" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(3) }">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>149.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Chấm điểm 4 tiêu chí <br>- Nâng cấp lập luận <br>- Bài chữa tham khảo <br>- Nâng cấp từ vựng</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Sửa các lỗi từ vựng và ngữ pháp <br> nổi bật nhất trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>349.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Cung cấp phản hồi cụ thể và <br> chính xác hơn với ChatGPT-4</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Chỉ ra các lỗi sai và cách sửa <br> cho từng câu văn trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">Hỗ Trợ Bản Viết Tay</li>
                <li class="active">AI Chấm Sửa Trong Khi Viết

                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng AI hỗ trợ chấm sửa <br>khi làm bài viết mà không cần trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip>
                </li>
                <li class="active">Phản Hồi Cho IELTS Speaking
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng cung cấp phản hồi <br>cho IELST Speaking mà không phải trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip></li>
              </ul>
              <hr style="margin-left: 40px; margin-right: 40px;">
              <ul style="margin-bottom: 20px;">
                <li class="active">Luyện Tập Không Giới Hạn</li>
                <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                <li class="active">
                  Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
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
      v-if="screenWidth > 760"
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
        <div v-if="orderSummary" style="padding: 20px; padding-top: 0px;">
          <div>
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
              <el-table-column prop="price" label="Một tháng" width="110">
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
              <el-tag v-if="activeDuration > 1" type="success" style="margin-top: 10px; font-size: 13px; width: 100%;">Đơn hàng này được hoàn trả trong vòng 30 ngày với bất kỳ lý do gì.</el-tag>
              <el-tag type="info" style="margin-top: 10px; font-size: 13px; width: 100%;">Bằng cách mua gói phản hồi, bạn đồng ý với điều khoản và dịch vụ của Reboost.</el-tag>
            </div>
          </div>
        </div>
        <div slot="footer" class="dialog-footer" />
      </div>
    </el-dialog>

    <el-dialog
      v-else
      id="checkout-dialog"
      :visible.sync="checkoutDialogVisiable"
      :fullscreen="true"
      :style="'margin-top: 50px;'"
      @closed="dialogClosed"
    >
      <div slot="title">
        <div style="padding: 0px 20px; font-size: 18px; text-align:center;">Thanh Toán</div>
      </div>
      <div class="dialog-body">
        <div v-if="orderSummary">
          <div>
            <el-table :data="orderSummary">
              <el-table-column prop="planName" label="Gói Phản Hồi">
                <template v-if="scope.row" slot-scope="scope">
                  <div style="word-break: break-word;">{{ scope.row.planName }}</div>
                  <div style="margin-top: 5px;">{{ scope.row.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND'}) }}/tháng</div>
                </template>
              </el-table-column>
              <el-table-column prop="duration" label="Thời hạn" width="80">
                <template v-if="scope.row" slot-scope="scope">
                  <div>{{ scope.row.duration + ' tháng' }}</div>
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
            <div style="text-align: center; margin-top: 20px; font-size: 16px;">
              <div><b>Phương Thức Thanh Toán</b></div>
              <div style="text-align: center; padding-top: 20px;">
                <el-button plain style=" padding-left: 30px; padding-right: 30px;" @click="submitZaloPayRequest()">
                  <div style="font-size: 16px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                  <img style="height: 20px;" src="../../assets/logo/zalopay.png" alt="logo" class="main-header-logo">
                </el-button>

                <el-button plain style="margin-top: 15px; margin-left: 0px; padding-left: 30px; padding-right: 30px;" @click="submitVNPayRequest()">
                  <div style="font-size: 16px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                  <img style="height: 21px;" src="../../assets/logo/vnpay.png" alt="logo" class="main-header-logo">
                </el-button>
              </div>

            </div>
            <div style="margin-top: 30px; text-align: center;">
              <el-tag style="margin-top: 5px; font-size: 13px; width: 100%;">Email nhận biên lai: {{ user.email }}</el-tag>
              <el-tag v-if="activeDuration > 1" type="success" style="margin-top: 10px; font-size: 13px; width: 100%;">Đơn hàng này được hoàn trả trong 30 ngày với bất kỳ lý do gì.</el-tag>
              <el-tag type="info" style="margin-top: 10px; font-size: 13px; width: 100%; height: 100%;">Bằng cách mua gói phản hồi, bạn đồng ý với điều khoản và dịch vụ của Reboost.</el-tag>
            </div>
          </div>
        </div>
        <div slot="footer" class="dialog-footer" />
      </div>
    </el-dialog>

  </section>

  <section v-else id="pricing" class="features-area-two ptb-80" :style="user.id ? 'margin-top: 0px; min-height: 100vh; padding-top: 80px;' : 'margin-top: 10px; min-height: 100vh;'">
    <div class="container">
      <div class="section-title" style="margin-bottom: 40px;">
        <h1 v-if="user.id" style="font-size: 30px;">Bảng Giá</h1>
        <h1 v-else style="font-size: 30px;">Bảng Giá</h1>
        <div class="bar" style="margin: 10px auto;" />
        <div style="text-align: center;">
          <div class="menu-btns" style="margin-top: 20px; margin-left: 5px;">
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(6) }" @click="selectDuration(6)">
              6 tháng
            </a>
            <a style="margin-right: 20px; padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(3) }" @click="selectDuration(3)">
              3 tháng
            </a>
            <a style="padding: 10px 25px;" class="btn btn-light" :class="{ 'active': durationActive(1) }" @click="selectDuration(1)">
              1 tháng
            </a>
          </div>
        </div>
      </div>

      <div v-if="activeDuration == 6" class="row">
        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price" style="margin-bottom: 20px">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">3 Bài Chấm Miễn Phí</li>
                <li class="active">2 Phản Hồi Chi Tiết</li>
                <li class="active">1 Phản Hồi Chuyên Sâu</li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li>Hoàn Tiền 14 Ngày</li>
                </ul>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary disabled" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(1) }">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>99.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Chấm điểm 4 tiêu chí <br>- Nâng cấp lập luận <br>- Bài chữa tham khảo <br>- Nâng cấp từ vựng</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Sửa các lỗi từ vựng và ngữ pháp <br> nổi bật nhất trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li class="active">
                    Hoàn Tiền 14 Ngày
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>249.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Cung cấp phản hồi cụ thể và <br> chính xác hơn với ChatGPT-4</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Chỉ ra các lỗi sai và cách sửa <br> cho từng câu văn trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">Hỗ Trợ Bản Viết Tay</li>
                <li class="active">AI Chấm Sửa Trong Khi Viết

                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng AI hỗ trợ chấm sửa <br>khi làm bài viết mà không cần trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip>
                </li>
                <li class="active">Phản Hồi Cho IELTS Speaking
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng cung cấp phản hồi <br>cho IELST Speaking mà không phải trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip></li>
              </ul>
              <hr style="margin-left: 40px; margin-right: 40px;">
              <ul style="margin-bottom: 20px;">
                <li class="active">Luyện Tập Không Giới Hạn</li>
                <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                <li class="active">
                  Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Hoàn Tiền 14 Ngày
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price" style="margin-bottom: 20px">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">3 Bài Chấm Miễn Phí</li>
                <li class="active">2 Phản Hồi Chi Tiết</li>
                <li class="active">1 Phản Hồi Chuyên Sâu</li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li>Hoàn Tiền 14 Ngày</li>
                </ul>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary disabled" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(2) }">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>129.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Chấm điểm 4 tiêu chí <br>- Nâng cấp lập luận <br>- Bài chữa tham khảo <br>- Nâng cấp từ vựng</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Sửa các lỗi từ vựng và ngữ pháp <br> nổi bật nhất trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                  <li class="active">
                    Hoàn Tiền 14 Ngày
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>299.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Cung cấp phản hồi cụ thể và <br> chính xác hơn với ChatGPT-4</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Chỉ ra các lỗi sai và cách sửa <br> cho từng câu văn trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">Hỗ Trợ Bản Viết Tay</li>
                <li class="active">AI Chấm Sửa Trong Khi Viết

                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng AI hỗ trợ chấm sửa <br>khi làm bài viết mà không cần trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip>
                </li>
                <li class="active">Phản Hồi Cho IELTS Speaking
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng cung cấp phản hồi <br>cho IELST Speaking mà không phải trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip></li>
              </ul>
              <hr style="margin-left: 40px; margin-right: 40px;">
              <ul style="margin-bottom: 20px;">
                <li class="active">Luyện Tập Không Giới Hạn</li>
                <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                <li class="active">
                  Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Hoàn Tiền 14 Ngày
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Bạn được đảm bảo trong 14 ngày đầu tiên có thể <br> hoàn tiền bất kỳ lúc nào với bất kỳ lý do nào</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Luyện tập cơ bản</h3>
            </div>
            <div class="price" style="margin-bottom: 20px">
              <span>Miễn Phí</span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">3 Bài Chấm Miễn Phí</li>
                <li class="active">2 Phản Hồi Chi Tiết</li>
                <li class="active">1 Phản Hồi Chuyên Sâu</li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
              </ul>
            </div>

            <div class="pricing-footer">
              <a href="#" class="btn btn-primary disabled" @click="selectPlan(0)">
                {{ getOptionText(0) }}
              </a>
            </div>
          </div>
        </div>

        <div class="col-lg-4 col-md-6 col-sm-6">
          <div class="pricing-table" :class="{ 'active-plan': planActive(3) }">
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chi Tiết</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>149.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Chấm điểm 4 tiêu chí <br>- Nâng cấp lập luận <br>- Bài chữa tham khảo <br>- Nâng cấp từ vựng</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chi Tiết
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Sửa các lỗi từ vựng và ngữ pháp <br> nổi bật nhất trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li>Hỗ Trợ Bản Viết Tay</li>
                <li>AI Chấm Sửa Trong Khi Viết</li>
                <li>Phản Hồi Cho IELTS Speaking</li>
                <hr style="margin-left: 40px; margin-right: 40px;">
                <ul style="margin-bottom: 20px;">
                  <li class="active">Luyện Tập Không Giới Hạn</li>
                  <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                  <li class="active">
                    Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                    <el-tooltip class="item" effect="light" placement="right">
                      <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                      <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                    </el-tooltip>
                  </li>
                </ul>
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
            <div class="pricing-header" style="margin-bottom: 20px">
              <h3>Phản Hồi Chuyên Sâu</h3>
            </div>

            <div class="price" style="margin-bottom: 20px">
              <span><sup>đ</sup>349.000<span>/THÁNG</span></span>
            </div>

            <div class="pricing-features">
              <ul style="margin-bottom: 10px;">
                <li class="active">Không Giới Hạn Bài Chấm</li>
                <li class="active">
                  Phản Hồi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Cung cấp phản hồi cụ thể và <br> chính xác hơn với ChatGPT-4</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">
                  Sửa Lỗi Chuyên Sâu
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Chỉ ra các lỗi sai và cách sửa <br> cho từng câu văn trong bài viết</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
                <li class="active">Hỗ Trợ Bản Viết Tay</li>
                <li class="active">AI Chấm Sửa Trong Khi Viết

                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng AI hỗ trợ chấm sửa <br>khi làm bài viết mà không cần trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip>
                </li>
                <li class="active">Phản Hồi Cho IELTS Speaking
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">Trải nghiệm sớm chức năng cung cấp phản hồi <br>cho IELST Speaking mà không phải trả thêm phí</div>
                    <el-tag size="small">Early</el-tag>
                  </el-tooltip></li>
              </ul>
              <hr style="margin-left: 40px; margin-right: 40px;">
              <ul style="margin-bottom: 20px;">
                <li class="active">Luyện Tập Không Giới Hạn</li>
                <li class="active">Chủ Đề Mới Cập Nhật Mỗi Tuần</li>
                <li class="active">
                  Tài Liệu Hữu Dụng Cho Từng Chủ Đề
                  <el-tooltip class="item" effect="light" placement="right">
                    <div slot="content">- Từ Vựng Hữu Dụng<br>- Ý Tưởng Phát Triển <br>- Gợi Ý Bố Cục <br>- Bài Mẫu Kèm Phân Tích</div>
                    <el-button style="padding: 0; border: none;"><i class="fas fa-question-circle" /></el-button>
                  </el-tooltip>
                </li>
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
      v-if="screenWidth > 760"
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
        <div v-if="orderSummary" style="padding: 20px; padding-top: 0px;">
          <div>
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
              <el-table-column prop="price" label="Một tháng" width="110">
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
              <el-tag v-if="activeDuration > 1" type="success" style="margin-top: 10px; font-size: 13px; width: 100%;">Đơn hàng này được hoàn trả trong vòng 30 ngày với bất kỳ lý do gì.</el-tag>
              <el-tag type="info" style="margin-top: 10px; font-size: 13px; width: 100%;">Bằng cách mua gói phản hồi, bạn đồng ý với điều khoản và dịch vụ của Reboost.</el-tag>
            </div>
          </div>
        </div>
        <div slot="footer" class="dialog-footer" />
      </div>
    </el-dialog>

    <el-dialog
      v-else
      id="checkout-dialog"
      :visible.sync="checkoutDialogVisiable"
      :fullscreen="true"
      :style="'margin-top: 50px;'"
      @closed="dialogClosed"
    >
      <div slot="title">
        <div style="padding: 0px 20px; font-size: 18px; text-align:center;">Thanh Toán</div>
      </div>
      <div class="dialog-body">
        <div v-if="orderSummary">
          <div>
            <el-table :data="orderSummary">
              <el-table-column prop="planName" label="Gói Phản Hồi">
                <template v-if="scope.row" slot-scope="scope">
                  <div style="word-break: break-word;">{{ scope.row.planName }}</div>
                  <div style="margin-top: 5px;">{{ scope.row.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND'}) }}/tháng</div>
                </template>
              </el-table-column>
              <el-table-column prop="duration" label="Thời hạn" width="80">
                <template v-if="scope.row" slot-scope="scope">
                  <div>{{ scope.row.duration + ' tháng' }}</div>
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
            <div style="text-align: center; margin-top: 20px; font-size: 16px;">
              <div><b>Phương Thức Thanh Toán</b></div>
              <div style="text-align: center; padding-top: 20px;">
                <el-button plain style=" padding-left: 30px; padding-right: 30px;" @click="submitZaloPayRequest()">
                  <div style="font-size: 16px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                  <img style="height: 20px;" src="../../assets/logo/zalopay.png" alt="logo" class="main-header-logo">
                </el-button>

                <el-button plain style="margin-top: 15px; margin-left: 0px; padding-left: 30px; padding-right: 30px;" @click="submitVNPayRequest()">
                  <div style="font-size: 16px; float: left; margin-top: 4px; margin-right: 6px;">Thanh toán qua</div>
                  <img style="height: 21px;" src="../../assets/logo/vnpay.png" alt="logo" class="main-header-logo">
                </el-button>
              </div>

            </div>
            <div style="margin-top: 30px; text-align: center;">
              <el-tag style="margin-top: 5px; font-size: 13px; width: 100%;">Email nhận biên lai: {{ user.email }}</el-tag>
              <el-tag v-if="activeDuration > 1" type="success" style="margin-top: 10px; font-size: 13px; width: 100%;">Đơn hàng này được hoàn trả trong 30 ngày với bất kỳ lý do gì.</el-tag>
              <el-tag type="info" style="margin-top: 10px; font-size: 13px; width: 100%; height: 100%;">Bằng cách mua gói phản hồi, bạn đồng ý với điều khoản và dịch vụ của Reboost.</el-tag>
            </div>
          </div>
        </div>
        <div slot="footer" class="dialog-footer" />
      </div>
    </el-dialog>

  </section>

</template>

<script>
import paymentService from '../../services/payment.service'
export default {
  name: 'Pricing',
  data() {
    return {
      screenWidth: window.innerWidth,
      type: 'new', // new, renew, upgrade
      activePlan: 1, // 6 tháng chi tiết
      activeDuration: 6,
      user: this.$store.state.auth.user,
      userSubscription: null,
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
  async mounted() {
    document.title = 'Reboost - Bảng Giá'
    window.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth
    })
    // Get user subscription information that display the option accordingly
    if (this.user.id) {
      const selectedPlan = this.getUrlParameter('planId')
      if (selectedPlan == '1') {
        this.activePlan = 1
        this.activeDuration = 6
      } else if (selectedPlan == '2') {
        this.activePlan = 2
        this.activeDuration = 3
      } else if (selectedPlan == '3') {
        this.activePlan = 3
        this.activeDuration = 1
      } else if (selectedPlan == '4') {
        this.activePlan = 4
        this.activeDuration = 6
      } else if (selectedPlan == '5') {
        this.activePlan = 5
        this.activeDuration = 3
      } else if (selectedPlan == '6') {
        this.activePlan = 6
        this.activeDuration = 1
      }

      this.userSubscription = await paymentService.getUserSubscription(this.user.id)
      if (this.userSubscription) {
        this.activePlan = this.userSubscription.planId
        this.activeDuration = this.userSubscription.duration
        this.proratedAmount = this.userSubscription.proratedAmount
      }
    }
  },
  methods: {
    getUrlParameter(sParam) {
      const sPageURL = decodeURIComponent(window.location.search.substring(1))
      const sURLVariables = sPageURL.split('&')
      let sParameterName
      let i
      for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=')

        if (sParameterName[0] === sParam) {
          return sParameterName[1] === undefined ? true : sParameterName[1]
        }
      }
    },
    async submitZaloPayRequest() {
      const model = {
        userId: this.user.id,
        planId: this.orderSummary[0].planId,
        duration: this.orderSummary[0].duration,
        subscriptionType: this.orderSummary[0].type,
        amount: this.orderSummary[0].total,
        proratedAmount: this.proratedAmount
      }
      var zaloPayUrl = await paymentService.submitZaloPayRequest(model)
      window.location.href = zaloPayUrl
    },
    async submitVNPayRequest() {
      const model = {
        userId: this.user.id,
        planId: this.orderSummary[0].planId,
        duration: this.orderSummary[0].duration,
        subscriptionType: this.orderSummary[0].type,
        amount: this.orderSummary[0].total,
        proratedAmount: this.proratedAmount
      }
      var vnPayUrl = await paymentService.submitVNPayRequest(model)
      window.location.href = vnPayUrl
    },
    getTotal() {
      let total = this.orderSummary[0].duration * this.orderSummary[0].price
      if (this.proratedAmount != 0 && this.type != 'renew') { total = total - this.proratedAmount }
      return total
    },
    dialogClosed() {
      this.orderSummary = []
    },
    selectPlan(planId) {
      if (this.user.id) {
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
          price = 129000
        } else if (planId == 3) {
          planName = 'Phản Hồi Chi Tiết'
          duration = 1
          price = 149000
        } else if (planId == 4) {
          planName = 'Phản Hồi Chuyên Sâu'
          duration = 6
          price = 249000
        } else if (planId == 5) {
          planName = 'Phản Hồi Chuyên Sâu'
          duration = 3
          price = 299000
        } else if (planId == 6) {
          planName = 'Phản Hồi Chuyên Sâu'
          duration = 1
          price = 349000
        }

        const option = this.getOptionText(planId)

        if (option != 'Lựa Chọn') {
          if (option == 'Gia Hạn') {
            this.type = 'renew'
            if (this.screenWidth > 780) { planName = 'Gia Hạn ' + planName } else { planName = 'Gia Hạn Gói ' + planName }
          } else {
            this.type = 'upgrade'
            if (this.screenWidth > 780) { planName = 'Nâng Cấp ' + planName } else { planName = 'Nâng Cấp Lên Gói ' + planName }
          }
        }

        let total = duration * price
        if (this.proratedAmount != 0 && this.type != 'renew') { total = total - this.proratedAmount }

        const order = {
          planId: planId,
          planName: planName,
          duration: duration,
          price: price,
          total: total,
          type: this.type
        }
        this.orderSummary.push(order)

        // show plan checkout dialog
        this.checkoutDialogVisiable = true
        // check if this is add new, renew, or upgrade subscription
        // send user to the payment page accodingly
        // subscribe user to plan
      } else {
        return this.$router.push({ path: '/register?planId=' + planId })
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
      if (planId == 0 && this.user.id) { return true }
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
