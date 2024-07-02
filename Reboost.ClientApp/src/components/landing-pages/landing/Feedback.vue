<template>
  <!-- Start ML Feedback Area -->
  <section id="testimony" class="ml-feedback-area ptb-80" style="padding-bottom: 60px;">
    <div class="container">
      <div class="section-title">
        <h2>Cảm nhận của người dùng</h2>
        <div class="bar" />
        <p style="max-width: 600px;">Reboost đã và đang giúp ích cho hàng ngàn thí sinh trong quá trình luyện tập chuẩn bị cho bài thi viết IELTS. Hãy cùng lắng nghe cảm nhận của họ.</p>
      </div>

      <div class="ml-feedback-slides">
        <carousel
          :autoplay="true"
          :loop="true"
          :smart-speed="1000"
          :autoplay-speed="true"
          :autoplay-timeout="5000"
          :per-page-custom="[[0, 1],[576, 1],[768, 1],[1024, 1], [1200, 1]]"
        >
          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/ngoc-anh.png" alt="image">
                <h3>Ngọc Anh</h3>
                <span>IELTS Writing 7.0</span>
              </div>
              <p>Em thấy phản hồi của Reboost rất hữu ích ạ 💯💯, do em mới làm quen với ielts và tự học nên chưa có nhiều kinh nghiệm cũng như kĩ năng làm bài tốt, app giúp em biết em còn yếu và thiếu gì trong bài viết, nói chung là em rất grateful ạ. ❤</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/duy-thanh.png" alt="image">
                <h3>Duy Thanh</h3>
                <span>IELTS Writing 8.0</span>
              </div>
              <p>Reboost cung cấp phân tích rất kĩ về mặt dùng từ và chấm ngữ pháp rất đúng. Ứng dụng cho mình cảm giác như được đánh giá bởi các giáo viên chuyên dạy IELTS.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/thao-nguyen.png" alt="image">
                <h3>Thảo Nguyên</h3>
                <span>IELTS Writing 7.5</span>
              </div>
              <p>Reboost đỉnh lắm luôn ạ. Web chấm kĩ theo đủ các tiêu chí và phát hiện lỗi sai, giúp người viết sửa lại và đưa ra thêm những cấu trúc nâng cao hơn để tham khảo nữa ạ. Em còn ấn tượng khi web cung cấp cả ý tưởng, từ vựng và bài viết mẫu ở phần thực hành nữa.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/nam-cuong.png" alt="image">
                <h3>Nam Cường</h3>
                <span>IELTS Writing 8.0</span>
              </div>
              <p>Sau khi sửa bài theo feedback của Reboost thì bài làm của em được nhận xét bới giáo viên nước ngoài em đang học là khá trơn tru và vốn từ vựng và grammar không bị sai sót nhiều nữa ạ.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/tran-ngoc-anh.png" alt="image">
                <h3>Trần Ngọc Anh</h3>
                <span>IELTS Writing: 7.5</span>
              </div>
              <p>E thấy web rất tuyệt ạ, có recommend cho e rất nhiều từ vựng và viết lại bài gợi ý cho em rất hay ạ &lt;333.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/xuan-nhi.png" alt="image">
                <h3>Xuân Nhi</h3>
                <span>IELTS Writing 8.0</span>
              </div>
              <p>Em thấy Web rất hữu ích và chấm khá đúng điểm của em luôn ạ, em đã thử chấm bằng nhiều web chính thống khác và ra điểm khớp 95% ạ.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/nguyen-thai-nguyen.png" alt="image">
                <h3>Thái Nguyên</h3>
                <span>IELTS Writing 8.0</span>
              </div>
              <p>Nhờ Reboost mà em giác ngộ được nhiều điều lắm. Ứng dụng rất hữu ích cho việc luyện viết IELTS!</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/thanh-sang.png" alt="image">
                <h3>Thanh Sang</h3>
                <span>IELTS Writing 8.5</span>
              </div>
              <p>Mình thấy ứng dụng rất giúp ích mình trong việc cải thiện cách viết bài.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/thanh-tam.png" alt="image">
                <h3>Thanh Tâm</h3>
                <span>IELTS Writing 7.0</span>
              </div>
              <p>Mình cảm thấy Reboost là 1 ứng dụng rất hữu ích trong quá trình học và luyện writing của bản thân.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/le-thu.png" alt="image">
                <h3>Lê Thư</h3>
                <span>IELTS Writing 7.5</span>
              </div>
              <p>Ứng dụng góp ý cho em rất chi tiết và cụ thể. Bình thường em đi học thầy cũng chưa góp ý được vậy.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/nguyen-tu.png" alt="image">
                <h3>Lê Thư</h3>
                <span>IELTS Writing 7.0</span>
              </div>
              <p>Mình rất thích Reboost vì ứng dụng cung cấp bài chấm mẫu và list từ vựng rất hữu dụng. Giao diện của app cũng rất dễ xài nữa.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/pham-thu-ha.png" alt="image">
                <h3>Phạm Thu Hà</h3>
                <span>IELTS Writing 8.5</span>
              </div>
              <p>Reboost cung cấp bài chữa rất chi tiết cho em. Nhờ có Reboost mà giờ em có thể cải thiện kỹ năng viết của mình.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/quy-ngo.png" alt="image">
                <h3>Quý Ngô</h3>
                <span>IELTS Writing 8.5</span>
              </div>
              <p>Phản hồi mình nhận được từ Reboost rất hữu ích và dễ hiểu!</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/bao-nguyen.png" alt="image">
                <h3>Bảo Nguyên</h3>
                <span>IELTS Writing 7.0</span>
              </div>
              <p>Bài viết của em được đánh giá đúng thực lực, ngoài ra còn nhận được feedback rất hữu dụng nữa. Cảm ơn Reboost!</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/vinh-nghi.png" alt="image">
                <h3>Vĩnh Nghi</h3>
                <span>IELTS Writing 8.0</span>
              </div>
              <p>Rất cảm ơn Reboost đã khiến em đỡ vất vả hơn trong việc luyện Writing. Ứng dụng giúp rất nhiều cho em trong việc sửa và cải thiện các tiêu chí.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/kim-anh.png" alt="image">
                <h3>Kim Anh</h3>
                <span>IELTS Writing 8.0</span>
              </div>
              <p>Mình vừa luyện viết vừa nhận phản hồi trên Reboost. App cung cấp rất nhiều tài nguyên hữu dụng và phản hồi rất chi tiết.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>

          <slide>
            <div class="single-ml-feedback-item">
              <div class="client-info">
                <img src="../../../assets/img/user-feedback/ngoc-linh.png" alt="image">
                <h3>Ngọc Anh</h3>
                <span>IELTS Writing 8.0</span>
              </div>
              <p>Reboost cung cấp các bài chấm chữa chi tiết và bổ ích giúp mình tiến bộ rất nhiều trong quá trình luyện viết ạ.</p>
              <div class="rating">
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
                <feather type="star" />
              </div>
            </div>
          </slide>
        </carousel>
      </div>
    </div>

    <div class="shape1"><img src="../../../assets/img/shape1.png" alt="shape"></div>
    <div class="shape2 rotateme"><img src="../../../assets/img/shape2.svg" alt="shape"></div>
    <div class="shape3"><img src="../../../assets/img/shape3.svg" alt="shape"></div>
    <div class="shape4"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape6 rotateme"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape7"><img src="../../../assets/img/shape4.svg" alt="shape"></div>
    <div class="shape8 rotateme"><img src="../../../assets/img/shape2.svg" alt="shape"></div>
  </section>
  <!-- End ML Feedback Area -->
</template>

<script>
import { Carousel, Slide } from 'vue-carousel'
export default {
  name: 'Feedback',
  components: { Carousel, Slide }
}
</script>
