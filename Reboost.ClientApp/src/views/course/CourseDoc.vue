<!-- CourseDocComponent.vue -->
<template>
  <div style="float: left; height: calc(100vh - 52px); width: calc(100% - 350px); padding: 40px; background: #f3f2ef; overflow-y: auto;">
    <el-card class="list-container box-card" style="padding: 30px; width: 100%; max-width: 930px;">
      <div>
        <p>The International English Language Testing System (IELTS) Academic exam assesses your proficiency in the English language, specifically for academic and higher education environments. Understanding the exam format is crucial for effective preparation and achieving a high score. This lesson provides a comprehensive overview of the IELTS Academic exam structure, including each section's purpose, format, and timing.</p>

        <div class="section">
          <h2>Overview of the IELTS Academic Exam</h2>
          <p>The IELTS Academic exam consists of four sections:</p>
          <ol>
            <li><strong>Listening</strong></li>
            <li><strong>Reading</strong></li>
            <li><strong>Writing</strong></li>
            <li><strong>Speaking</strong></li>
          </ol>
          <p>The total test time is approximately <span class="time">2 hours and 45 minutes</span>. The Listening, Reading, and Writing sections are completed in one sitting, while the Speaking test may be scheduled on the same day or within seven days before or after the other tests.</p>
        </div>

        <div class="section">
          <h2>Section 1: Listening</h2>
          <h3>Format and Content</h3>
          <p>The Listening section has <strong>four recorded monologues and conversations</strong>. You will hear each recording only once.</p>
          <ul>
            <li><strong>Recording 1:</strong> A conversation between two people in an everyday social context.</li>
            <li><strong>Recording 2:</strong> A monologue in a social context, e.g., a speech about local facilities.</li>
            <li><strong>Recording 3:</strong> A conversation among up to four people in an educational or training context.</li>
            <li><strong>Recording 4:</strong> A monologue on an academic subject, e.g., a university lecture.</li>
          </ul>
          <h3>Timing</h3>
          <p>Total time: <span class="time">30 minutes</span> plus <span class="time">10 minutes</span> transfer time.</p>
          <h3>Question Types</h3>
          <ul>
            <li>Multiple choice</li>
            <li>Matching</li>
            <li>Plan/map/diagram labeling</li>
            <li>Form/note/table/flow-chart/summary completion</li>
            <li>Sentence completion</li>
          </ul>
          <h3>Answering</h3>
          <p>You will write your answers on the question paper as you listen. After the recordings end, you have 10 minutes to transfer your answers to the answer sheet.</p>
        </div>

        <div class="section">
          <h2>Section 2: Reading</h2>
          <h3>Format and Content</h3>
          <p>The Reading section consists of <strong>three long texts</strong>, which range from descriptive and factual to discursive and analytical. These texts are authentic and are taken from books, journals, magazines, and newspapers.</p>
          <h3>Timing</h3>
          <p>Total time: <span class="time">60 minutes</span>.</p>
          <h3>Question Types</h3>
          <ul>
            <li>Multiple choice</li>
            <li>Identifying information (True/False/Not Given)</li>
            <li>Identifying writer’s views/claims (Yes/No/Not Given)</li>
            <li>Matching information</li>
            <li>Matching headings</li>
            <li>Matching features</li>
            <li>Matching sentence endings</li>
            <li>Sentence completion</li>
            <li>Summary/note/table/flow-chart completion</li>
            <li>Diagram label completion</li>
            <li>Short-answer questions</li>
          </ul>
          <h3>Answering</h3>
          <p>You must write your answers on the answer sheet during the 60 minutes; no extra time is given.</p>
        </div>

        <div class="section">
          <h2>Section 3: Writing</h2>
          <h3>Format and Content</h3>
          <p>The Writing section has <strong>two tasks</strong> and must be completed in <span class="time">60 minutes</span>.</p>
          <h4>Task 1</h4>
          <p>You are presented with a graph, table, chart, or diagram and are asked to describe, summarize, or explain the information in your own words. Minimum of <strong>150 words</strong>.</p>
          <h4>Task 2</h4>
          <p>You are asked to write an essay in response to a point of view, argument, or problem. Minimum of <strong>250 words</strong>.</p>
          <h3>Timing</h3>
          <ul>
            <li><strong>Task 1:</strong> Suggested time is 20 minutes.</li>
            <li><strong>Task 2:</strong> Suggested time is 40 minutes.</li>
          </ul>
          <h3>Assessment Criteria</h3>
          <p>Your answers are assessed based on:</p>
          <ul>
            <li>Task Achievement (Task 1) / Task Response (Task 2)</li>
            <li>Coherence and Cohesion</li>
            <li>Lexical Resource (Vocabulary)</li>
            <li>Grammatical Range and Accuracy</li>
          </ul>
        </div>

        <div class="section">
          <h2>Section 4: Speaking</h2>
          <h3>Format and Content</h3>
          <p>The Speaking test is a <strong>face-to-face oral interview</strong> with an examiner and consists of three parts.</p>
          <h4>Part 1: Introduction and Interview</h4>
          <p>Duration: <span class="time">4-5 minutes</span></p>
          <p>The examiner introduces themselves and asks you to confirm your identity. They then ask general questions about familiar topics such as home, family, work, studies, and interests.</p>
          <h4>Part 2: Individual Long Turn</h4>
          <p>Duration: <span class="time">3-4 minutes</span> (including preparation time)</p>
          <p>You are given a task card with a topic and have one minute to prepare. You then speak on the topic for up to two minutes. The examiner may ask one or two questions on the same topic.</p>
          <h4>Part 3: Two-way Discussion</h4>
          <p>Duration: <span class="time">4-5 minutes</span></p>
          <p>The examiner asks further questions related to the topic in Part 2, discussing more abstract ideas and issues.</p>
          <h3>Assessment Criteria</h3>
          <p>Your performance is assessed on:</p>
          <ul>
            <li>Fluency and Coherence</li>
            <li>Lexical Resource</li>
            <li>Grammatical Range and Accuracy</li>
            <li>Pronunciation</li>
          </ul>
        </div>

        <div class="section">
          <h2>Scoring and Band Descriptors</h2>
          <p>The IELTS exam is scored on a <strong>9-band scale</strong>, with each section receiving a band score. The overall band score is the average of the four sections.</p>
          <h3>Band Scores</h3>
          <ul>
            <li><strong>Band 9:</strong> Expert user</li>
            <li><strong>Band 8:</strong> Very good user</li>
            <li><strong>Band 7:</strong> Good user</li>
            <li><strong>Band 6:</strong> Competent user</li>
            <li><strong>Band 5:</strong> Modest user</li>
            <li><strong>Band 4:</strong> Limited user</li>
            <li><strong>Band 3:</strong> Extremely limited user</li>
            <li><strong>Band 2:</strong> Intermittent user</li>
            <li><strong>Band 1:</strong> Non-user</li>
            <li><strong>Band 0:</strong> Did not attempt the test</li>
          </ul>
          <p>Understanding the band descriptors can help you identify areas for improvement and set realistic goals.</p>
        </div>

        <div class="section">
          <h2>Test Day Tips</h2>
          <ul>
            <li><strong>Arrive Early:</strong> Ensure you arrive at the test center at least 30 minutes before the scheduled time.</li>
            <li><strong>Identification:</strong> Bring the same identification document you used when registering.</li>
            <li><strong>Listen Carefully:</strong> Follow all instructions given by the test invigilators.</li>
            <li><strong>Time Management:</strong> Keep track of time during each section to ensure you can answer all questions.</li>
            <li><strong>No Breaks:</strong> Be prepared to sit for the Listening, Reading, and Writing sections without breaks.</li>
          </ul>
        </div>

        <div class="section">
          <h2>Conclusion</h2>
          <p>Understanding the IELTS Academic exam format is the first step toward effective preparation. Familiarize yourself with each section's structure, question types, and timing to build confidence and improve your performance. Remember to practice regularly and seek feedback to identify areas for improvement.</p>
        </div>

      </div>

      <div>
        <p>Hệ thống Kiểm tra Anh ngữ Quốc tế (IELTS) Học Thuật đánh giá khả năng sử dụng tiếng Anh của bạn, đặc biệt trong môi trường học thuật và giáo dục đại học. Hiểu rõ định dạng kỳ thi là rất quan trọng để chuẩn bị hiệu quả và đạt điểm cao. Bài học này cung cấp một cái nhìn tổng quan chi tiết về cấu trúc kỳ thi IELTS Học Thuật, bao gồm mục đích, định dạng và thời gian của mỗi phần.</p>

        <div class="section">
          <h2>Tổng Quan Về Kỳ Thi IELTS Học Thuật</h2>
          <p>Kỳ thi IELTS Học Thuật bao gồm bốn phần:</p>
          <ol>
            <li><strong>Nghe</strong></li>
            <li><strong>Đọc</strong></li>
            <li><strong>Viết</strong></li>
            <li><strong>Nói</strong></li>
          </ol>
          <p>Tổng thời gian thi khoảng <span class="time">2 giờ 45 phút</span>. Các phần Nghe, Đọc và Viết được hoàn thành trong một ngày thi, trong khi phần thi Nói có thể được sắp xếp cùng ngày hoặc trong vòng bảy ngày trước hoặc sau các bài thi khác.</p>
        </div>

        <div class="section">
          <h2>Phần 1: Nghe</h2>
          <h3>Định Dạng và Nội Dung</h3>
          <p>Phần Nghe có <strong>bốn đoạn hội thoại và độc thoại được ghi âm</strong>. Bạn sẽ chỉ nghe mỗi đoạn một lần.</p>
          <ul>
            <li><strong>Bản ghi 1:</strong> Một cuộc hội thoại giữa hai người trong ngữ cảnh xã hội hàng ngày.</li>
            <li><strong>Bản ghi 2:</strong> Một độc thoại trong ngữ cảnh xã hội, ví dụ: một bài nói về các cơ sở địa phương.</li>
            <li><strong>Bản ghi 3:</strong> Một cuộc hội thoại giữa tối đa bốn người trong ngữ cảnh giáo dục hoặc đào tạo.</li>
            <li><strong>Bản ghi 4:</strong> Một độc thoại về một chủ đề học thuật, ví dụ: một bài giảng đại học.</li>
          </ul>
          <h3>Thời Gian</h3>
          <p>Tổng thời gian: <span class="time">30 phút</span> cộng thêm <span class="time">10 phút</span> để chuyển đáp án.</p>
          <h3>Các Loại Câu Hỏi</h3>
          <ul>
            <li>Trắc nghiệm</li>
            <li>Nối</li>
            <li>Ghi nhãn sơ đồ/bản đồ/biểu đồ</li>
            <li>Hoàn thành biểu mẫu/ghi chú/bảng/sơ đồ/biểu đồ</li>
            <li>Hoàn thành câu</li>
          </ul>
          <h3>Trả Lời</h3>
          <p>Bạn sẽ viết câu trả lời trên đề thi khi nghe. Sau khi các bản ghi kết thúc, bạn có 10 phút để chuyển câu trả lời vào phiếu trả lời.</p>
        </div>

        <div class="section">
          <h2>Phần 2: Đọc</h2>
          <h3>Định Dạng và Nội Dung</h3>
          <p>Phần Đọc bao gồm <strong>ba đoạn văn dài</strong>, từ miêu tả và thực tế đến nghị luận và phân tích. Các đoạn văn này có thể được trích xuất từ sách, tạp chí, ấn phẩm chuyên ngành và báo chí.</p>
          <h3>Thời Gian</h3>
          <p>Tổng thời gian: <span class="time">60 phút</span>.</p>
          <h3>Các Loại Câu Hỏi</h3>
          <ul>
            <li>Trắc nghiệm</li>
            <li>Xác định thông tin (Đúng/Sai/Không có trong bài)</li>
            <li>Xác định quan điểm/tuyên bố của tác giả (Có/Không/Không có trong bài)</li>
            <li>Nối thông tin</li>
            <li>Nối tiêu đề</li>
            <li>Nối đặc điểm</li>
            <li>Nối kết thúc câu</li>
            <li>Hoàn thành câu</li>
            <li>Hoàn thành tóm tắt/ghi chú/bảng/sơ đồ</li>
            <li>Hoàn thành ghi nhãn biểu đồ</li>
            <li>Câu hỏi trả lời ngắn</li>
          </ul>
          <h3>Trả Lời</h3>
          <p>Bạn phải viết câu trả lời vào phiếu trả lời trong thời gian 60 phút; không có thêm thời gian.</p>
        </div>

        <div class="section">
          <h2>Phần 3: Viết</h2>
          <h3>Định Dạng và Nội Dung</h3>
          <p>Phần Viết có <strong>hai bài tập</strong> và phải được hoàn thành trong <span class="time">60 phút</span>.</p>
          <h4>Bài tập 1</h4>
          <p>Bạn được cung cấp một biểu đồ, bảng, biểu đồ hoặc sơ đồ và được yêu cầu mô tả, tóm tắt hoặc giải thích thông tin bằng lời của bạn. Tối thiểu <strong>150 từ</strong>.</p>
          <h4>Bài tập 2</h4>
          <p>Bạn được yêu cầu viết một bài luận đáp lại một quan điểm, lập luận hoặc vấn đề. Tối thiểu <strong>250 từ</strong>.</p>
          <h3>Thời Gian</h3>
          <ul>
            <li><strong>Bài tập 1:</strong> Thời gian đề nghị là 20 phút.</li>
            <li><strong>Bài tập 2:</strong> Thời gian đề nghị là 40 phút.</li>
          </ul>
          <h3>Tiêu Chí Đánh Giá</h3>
          <p>Bài viết của bạn được đánh giá dựa trên:</p>
          <ul>
            <li>Task Achievement - Hoàn thành yêu cầu (Bài tập 1) / Task Response - Trả lời đề bài (Bài tập 2)</li>
            <li>Coherence and Cohesion - Tính mạch lạc và liên kết</li>
            <li>Lexical Resource - Nguồn từ vựng</li>
            <li>Grammatical Range and Accuracy - Phạm vi và độ chính xác ngữ pháp</li>
          </ul>
        </div>

        <div class="section">
          <h2>Phần 4: Nói</h2>
          <h3>Định Dạng và Nội Dung</h3>
          <p>Bài thi Nói là một <strong>bài phỏng vấn trực tiếp</strong> với giám khảo và bao gồm ba phần.</p>
          <h4>Phần 1: Giới thiệu và Phỏng vấn</h4>
          <p>Thời lượng: <span class="time">4-5 phút</span></p>
          <p>Giám khảo giới thiệu bản thân và yêu cầu bạn xác nhận danh tính. Sau đó, họ hỏi các câu hỏi chung về các chủ đề quen thuộc như gia đình, công việc, học tập và sở thích.</p>
          <h4>Phần 2: Nói Dài Cá Nhân</h4>
          <p>Thời lượng: <span class="time">3-4 phút</span> (bao gồm thời gian chuẩn bị)</p>
          <p>Bạn được cung cấp một thẻ chủ đề và có một phút để chuẩn bị. Sau đó, bạn nói về chủ đề đó trong tối đa hai phút. Giám khảo có thể hỏi một hoặc hai câu hỏi về cùng chủ đề.</p>
          <h4>Phần 3: Thảo Luận Hai Chiều</h4>
          <p>Thời lượng: <span class="time">4-5 phút</span></p>
          <p>Giám khảo hỏi thêm các câu hỏi liên quan đến chủ đề ở Phần 2, thảo luận về các ý tưởng và vấn đề trừu tượng hơn.</p>
          <h3>Tiêu Chí Đánh Giá</h3>
          <p>Phần trình bày của bạn được đánh giá dựa trên:</p>
          <ul>
            <li>Fluency and Coherence - Sự trôi chảy và mạch lạc</li>
            <li>Lexical Resource - Nguồn từ vựng</li>
            <li>Grammatical Range and Accuracy - Phạm vi và độ chính xác ngữ pháp</li>
            <li>Pronunciation - Phát âm</li>
          </ul>
        </div>

        <div class="section">
          <h2>Điểm Số và Mô Tả Băng Điểm</h2>
          <p>Kỳ thi IELTS được chấm trên thang điểm <strong>9 băng</strong>, mỗi phần thi nhận được một băng điểm. Điểm tổng thể là trung bình của bốn phần thi.</p>
          <h3>Băng Điểm</h3>
          <ul>
            <li><strong>Băng 9:</strong> Người sử dụng chuyên nghiệp</li>
            <li><strong>Băng 8:</strong> Người sử dụng rất tốt</li>
            <li><strong>Băng 7:</strong> Người sử dụng tốt</li>
            <li><strong>Băng 6:</strong> Người sử dụng khá</li>
            <li><strong>Băng 5:</strong> Người sử dụng trung bình</li>
            <li><strong>Băng 4:</strong> Người sử dụng hạn chế</li>
            <li><strong>Băng 3:</strong> Người sử dụng cực kỳ hạn chế</li>
            <li><strong>Băng 2:</strong> Người sử dụng ngắt quãng</li>
            <li><strong>Băng 1:</strong> Không biết sử dụng</li>
            <li><strong>Băng 0:</strong> Không tham gia thi</li>
          </ul>
          <p>Hiểu rõ mô tả băng điểm có thể giúp bạn xác định các lĩnh vực cần cải thiện và đặt ra mục tiêu thực tế.</p>
        </div>

        <div class="section">
          <h2>Mẹo Trong Ngày Thi</h2>
          <ul>
            <li><strong>Đến Sớm:</strong> Đảm bảo bạn đến trung tâm thi ít nhất 30 phút trước giờ thi.</li>
            <li><strong>Giấy Tờ Tùy Thân:</strong> Mang theo giấy tờ tùy thân bạn đã sử dụng khi đăng ký.</li>
            <li><strong>Nghe Kỹ:</strong> Tuân theo tất cả hướng dẫn của giám thị.</li>
            <li><strong>Quản Lý Thời Gian:</strong> Theo dõi thời gian trong mỗi phần để đảm bảo bạn có thể trả lời tất cả câu hỏi.</li>
            <li><strong>Không Có Giờ Nghỉ:</strong> Chuẩn bị ngồi cho các phần Nghe, Đọc và Viết mà không có giờ nghỉ.</li>
          </ul>
        </div>

        <div class="section">
          <h2>Kết Luận</h2>
          <p>Hiểu rõ định dạng kỳ thi IELTS Học Thuật là bước đầu tiên để chuẩn bị hiệu quả. Làm quen với cấu trúc của mỗi phần, các loại câu hỏi và thời gian để xây dựng sự tự tin và cải thiện hiệu suất của bạn. Hãy nhớ luyện tập thường xuyên và tìm kiếm phản hồi để xác định các lĩnh vực cần cải thiện.</p>
        </div>

      </div>

      <div>
        <h1>Course Overview</h1>

        <p>This course is designed for learners at the initial stage of English proficiency (Band 0.0 to 4.0) who aim to develop foundational skills necessary for IELTS Academic Writing Task 1. The course focuses on building basic grammar, vocabulary, understanding data representation, and developing writing skills to describe and summarize visual information effectively.</p>

        <h2>Course Objectives</h2>
        <ul>
          <li>Understand the format and requirements of IELTS Academic Writing Task 1.</li>
          <li>Develop basic grammatical structures and sentence formation skills.</li>
          <li>Acquire essential vocabulary for describing graphs, charts, and tables.</li>
          <li>Learn to identify main features and trends in visual data.</li>
          <li>Practice writing clear and coherent summaries of visual information.</li>
        </ul>

        <h2>Target Audience</h2>
        <p>This course is ideal for:</p>
        <ul>
          <li>Beginners with little to no experience in IELTS Academic Writing.</li>
          <li>Learners aiming to improve from Band 0.0 to 4.0 in writing.</li>
          <li>Students who need to build foundational writing skills for academic purposes.</li>
        </ul>

        <h2>Course Structure</h2>

        <div class="module">
          <h3>Module 1: Introduction to IELTS Writing Task 1</h3>
          <ul>
            <li>Understanding the IELTS exam format.</li>
            <li>Overview of Writing Task 1 requirements.</li>
            <li>Types of visual information (graphs, charts, tables, diagrams, processes).</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 2: Basic Grammar and Sentence Structure</h3>
          <ul>
            <li>Simple sentence formation.</li>
            <li>Subject-verb agreement.</li>
            <li>Introduction to tenses (present simple, past simple).</li>
            <li>Using conjunctions to link ideas.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 3: Essential Vocabulary for Data Description</h3>
          <ul>
            <li>Common verbs (increase, decrease, rise, fall, remain steady).</li>
            <li>Adjectives and adverbs for describing changes (slight, significant, gradual, sharp).</li>
            <li>Prepositions used in data description (from, to, by, at).</li>
            <li>Numbers and percentages.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 4: Identifying Main Features and Trends</h3>
          <ul>
            <li>Reading and interpreting visual data.</li>
            <li>Identifying key features (highest, lowest points, overall trends).</li>
            <li>Summarizing information without details.</li>
            <li>Practice exercises on trend identification.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 5: Describing Trends and Comparisons</h3>
          <ul>
            <li>Using appropriate language to describe trends.</li>
            <li>Comparative and superlative forms.</li>
            <li>Constructing sentences for comparisons.</li>
            <li>Practice writing comparisons between data sets.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 6: Structuring the Writing Task 1 Response</h3>
          <ul>
            <li>Introduction to report structure (introduction, overview, details).</li>
            <li>Writing an effective introduction (paraphrasing the question).</li>
            <li>Writing a clear overview paragraph.</li>
            <li>Organizing detailed paragraphs logically.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 7: Practice and Application</h3>
          <ul>
            <li>Guided practice with sample tasks.</li>
            <li>Analyzing sample answers at Band 4.0 level.</li>
            <li>Peer review and feedback sessions.</li>
            <li>Timed writing practice.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 8: Review and Final Assessment</h3>
          <ul>
            <li>Review of key concepts and vocabulary.</li>
            <li>Final practice test simulating exam conditions.</li>
            <li>Individual feedback and improvement plans.</li>
          </ul>
        </div>

        <h2>Learning Outcomes</h2>
        <p>By the end of this course, students will be able to:</p>
        <ul>
          <li>Understand and interpret basic visual data representations.</li>
          <li>Write simple and grammatically correct sentences describing data.</li>
          <li>Use essential vocabulary to describe trends and comparisons.</li>
          <li>Structure a Writing Task 1 response appropriately.</li>
          <li>Summarize main features of visual data effectively.</li>
        </ul>

        <h2>Course Duration</h2>
        <p>The course spans over 8 weeks, with each module designed to be completed in one week. Recommended study time is 3-4 hours per week, including practice exercises and assignments.</p>

        <h2>Materials and Resources</h2>
        <ul>
          <li>Course lessons with exercises and sample tasks.</li>
          <li>Access to online practice tests and resources.</li>
          <li>Supplementary grammar and vocabulary reference materials.</li>
        </ul>

        <h2>Assessment Methods</h2>
        <ul>
          <li>Your writing practice will be assessed by Reboost's auto-assessment system.</li>
        </ul>
      </div>

      <div>
        <h1>Tổng Quan Khóa Học</h1>

        <p>Khóa học này được thiết kế dành cho người học ở giai đoạn khởi đầu về khả năng tiếng Anh (Band 0.0 đến 4.0) với mục tiêu phát triển các kỹ năng nền tảng cần thiết cho IELTS Academic Writing Task 1. Khóa học tập trung vào xây dựng ngữ pháp cơ bản, từ vựng, hiểu biết về biểu diễn dữ liệu và phát triển kỹ năng viết để miêu tả và tóm tắt thông tin hình ảnh một cách hiệu quả.</p>

        <h2>Mục Tiêu Khóa Học</h2>
        <ul>
          <li>Hiểu cấu trúc và yêu cầu của IELTS Academic Writing Task 1.</li>
          <li>Phát triển kỹ năng ngữ pháp cơ bản và cấu trúc câu.</li>
          <li>Nắm bắt từ vựng cần thiết để miêu tả biểu đồ, bảng biểu và đồ thị.</li>
          <li>Học cách nhận diện các đặc điểm chính và xu hướng trong dữ liệu hình ảnh.</li>
          <li>Thực hành viết các bản tóm tắt rõ ràng và mạch lạc về thông tin hình ảnh.</li>
        </ul>

        <h2>Đối Tượng Khóa Học</h2>
        <p>Khóa học này lý tưởng cho:</p>
        <ul>
          <li>Người mới bắt đầu với ít hoặc không có kinh nghiệm về IELTS Academic Writing.</li>
          <li>Người học muốn cải thiện kỹ năng để đạt Band 4.0 trong phần viết.</li>
          <li>Sinh viên cần xây dựng kỹ năng viết nền tảng cho mục đích học thuật.</li>
        </ul>

        <h2>Cấu Trúc Khóa Học</h2>

        <div class="module">
          <h3>Module 1: Giới Thiệu về IELTS Writing Task 1</h3>
          <ul>
            <li>Hiểu cấu trúc kỳ thi IELTS.</li>
            <li>Tổng quan về yêu cầu của Writing Task 1.</li>
            <li>Các loại thông tin hình ảnh (biểu đồ, bảng biểu, đồ thị, sơ đồ, quy trình).</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 2: Ngữ Pháp Cơ Bản và Cấu Trúc Câu</h3>
          <ul>
            <li>Hình thành câu đơn giản.</li>
            <li>Sự hòa hợp giữa chủ ngữ và động từ.</li>
            <li>Giới thiệu về thì (hiện tại đơn, quá khứ đơn).</li>
            <li>Sử dụng liên từ để kết nối ý.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 3: Từ Vựng Cần Thiết cho Miêu Tả Dữ Liệu</h3>
          <ul>
            <li>Động từ thông dụng (increase, decrease, rise, fall, remain steady).</li>
            <li>Tính từ và trạng từ để miêu tả sự thay đổi (slight, significant, gradual, sharp).</li>
            <li>Giới từ dùng trong miêu tả dữ liệu (from, to, by, at).</li>
            <li>Số liệu và phần trăm.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 4: Nhận Diện Đặc Điểm Chính và Xu Hướng</h3>
          <ul>
            <li>Đọc và diễn giải dữ liệu hình ảnh.</li>
            <li>Nhận diện các đặc điểm chính (điểm cao nhất, thấp nhất, xu hướng tổng quan).</li>
            <li>Tóm tắt thông tin mà không đi vào chi tiết.</li>
            <li>Bài tập thực hành về nhận diện xu hướng.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 5: Miêu Tả Xu Hướng và So Sánh</h3>
          <ul>
            <li>Sử dụng ngôn ngữ phù hợp để miêu tả xu hướng.</li>
            <li>Dạng so sánh và so sánh hơn nhất.</li>
            <li>Xây dựng câu cho so sánh.</li>
            <li>Thực hành viết so sánh giữa các tập dữ liệu.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 6: Cấu Trúc Bài Viết Task 1</h3>
          <ul>
            <li>Giới thiệu về cấu trúc báo cáo (giới thiệu, tổng quan, chi tiết).</li>
            <li>Viết phần giới thiệu hiệu quả (paraphrase đề bài).</li>
            <li>Viết đoạn tổng quan rõ ràng.</li>
            <li>Sắp xếp các đoạn chi tiết một cách logic.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 7: Thực Hành và Ứng Dụng</h3>
          <ul>
            <li>Thực hành có hướng dẫn với các bài tập mẫu.</li>
            <li>Phân tích các bài mẫu ở mức Band 4.0.</li>
            <li>Phiên đánh giá và phản hồi lẫn nhau.</li>
            <li>Thực hành viết trong thời gian quy định.</li>
          </ul>
        </div>

        <div class="module">
          <h3>Module 8: Ôn Tập và Đánh Giá Cuối Khóa</h3>
          <ul>
            <li>Ôn tập các khái niệm và từ vựng chính.</li>
            <li>Bài kiểm tra thực hành cuối cùng mô phỏng điều kiện thi.</li>
            <li>Phản hồi cá nhân và kế hoạch cải thiện.</li>
          </ul>
        </div>

        <h2>Kết Quả Học Tập</h2>
        <p>Sau khi hoàn thành khóa học, học viên sẽ có thể:</p>
        <ul>
          <li>Hiểu và diễn giải các biểu diễn dữ liệu hình ảnh cơ bản.</li>
          <li>Viết các câu đơn giản và đúng ngữ pháp để miêu tả dữ liệu.</li>
          <li>Sử dụng từ vựng cần thiết để miêu tả xu hướng và so sánh.</li>
          <li>Cấu trúc một bài viết Writing Task 1 một cách phù hợp.</li>
          <li>Tóm tắt các đặc điểm chính của dữ liệu hình ảnh một cách hiệu quả.</li>
        </ul>

        <h2>Thời Lượng Khóa Học</h2>
        <p>Khóa học kéo dài trong 8 tuần, mỗi module được thiết kế để hoàn thành trong một tuần. Thời gian học đề xuất là 3-4 giờ mỗi tuần, bao gồm các bài tập thực hành và bài tập về nhà.</p>

        <h2>Tài Liệu và Các Tài Nguyên Hỗ Trợ</h2>
        <ul>
          <li>Các bài học của khóa học kèm theo các bài tập và nhiệm vụ mẫu.</li>
          <li>Truy cập vào các bài kiểm tra thực hành và nguồn tài liệu trực tuyến.</li>
          <li>Tài liệu tham khảo ngữ pháp và từ vựng bổ sung.</li>
        </ul>

        <h2>Phương Pháp Đánh Giá</h2>
        <ul>
          <li>Các bài thực hành của bạn sẽ được đánh giá bởi hệ thống chấm bài tự động của Reboost.</li>
        </ul>
      </div>

      <div v-if="question" class="practice">
        <h1>Practice: {{ question.title }}</h1>
        <div class="question">
          <h2>Đề Bài</h2>
          <hr>
          <div v-if="question.content">
            {{ question.content }}
          </div>
          <div v-if="question.chart" style="text-align: center; margin-top: 20px;">
            <img :src="'/photo/' + question.chart" :alt="question.title" style="max-height: 100%; max-width: 100%;">
          </div>
        </div>

        <!-- <div class="respose">
          <h2>Bài Làm Của Bạn</h2>
          <hr>
          <div>
            <div style="height: 100%; display: flex; flex-direction: column;">
              <div class="header-passage">
                <div style="float: left; margin-right: 5px; ">
                  <div>
                    <el-button v-if="!isTesting && !submissionId" size="mini" type="danger" plain @click="enableTestMode()">
                      <i style="font-weight: bold; margin-right: 2px;" class="el-icon-timer" />
                      Tính giờ
                    </el-button>
                  </div>
                </div>

                <el-button
                  style="float: right;"
                  size="mini"
                  type="primary"
                  plain
                  @click="requestReview()"
                >Nộp Bài</el-button>
                <div style="width: 150px; float: left;">
                  <el-tag
                    v-if="isShowCountWord && countWord != 0"
                    type="info"
                    size="medium"
                    closable
                    effect="plain"
                    :disable-transitions="true"
                    @close="toggleShowCount()"
                  >
                    Số từ: {{ countWord }}
                  </el-tag>

                  <el-button v-if="!isShowCountWord" size="mini" plain @click="toggleShowCount()">
                    Hiện số từ
                  </el-button>
                </div>

              </div>
              <div style="flex-grow: 1;">
                <textarea
                  v-model="writingContent"
                  placeholder="Bắt đầu bài viết của bạn ở đây ..."
                  spellcheck="false"
                  class="textarea-style"
                  @keyup="countWords()"
                />
              </div>
            </div>
          </div>

        </div> -->

        <!-- <div class="feedback">
          <h2>Phản Hồi & Đánh Giá</h2>
          <hr>
          <div>

            <div v-if="loadEssayScoreCompleted">
              <el-card
                style="margin-bottom: 5px; border: 1px solid rgb(190, 190, 190);"
                shadow="hover"
              >
                <div slot="header" class="clearfix">
                  <div>
                    <div style="float: left; font-size: 16px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                      <span>Overall Band Score</span>
                    </div>
                    <div style="float: right;">
                      <div>
                        <div>
                          <div v-if="essayScore.overallBandScore" class="band-score">
                            Band: {{ essayScore.overallBandScore.toString().length == 1 ? essayScore.overallBandScore.toString() + '.0' : essayScore.overallBandScore }}
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div style="border: 1px solid rgb(188, 188, 188); padding: 10px; border-radius: 5px;">
                  <div v-if="essayScore.taskAchievementScore" class="criteria-score">
                    <div>
                      <b>
                        Task Achievement:
                        {{ essayScore.taskAchievementScore.toString().length == 1 ? essayScore.taskAchievementScore.toString() + '.0' : essayScore.taskAchievementScore }}
                      </b>
                    </div>
                    <div v-if="essayScore.highlightKeyFeatures" class="sub-criteria">
                      <el-tag
                        :type="essayScore.highlightKeyFeatures > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                          essayScore.highlightKeyFeatures == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.highlightKeyFeatures.toString().length == 1 ? essayScore.highlightKeyFeatures.toString() + '.0' : essayScore.highlightKeyFeatures }}
                        </span>
                      </el-tag>
                      <div class="sub-criteria-label">Highlighting Key Features</div>
                    </div>
                    <div v-if="essayScore.compareAndContrast" class="sub-criteria">
                      <el-tag
                        :type="essayScore.compareAndContrast > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                          essayScore.compareAndContrast == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.compareAndContrast.toString().length == 1 ? essayScore.compareAndContrast.toString() + '.0' : essayScore.compareAndContrast }}
                        </span>
                      </el-tag>
                      <div class="sub-criteria-label">Comparing and Contrasting Data</div>
                    </div>

                    <div v-if="essayScore.dataSelection" class="sub-criteria">
                      <el-tag
                        :type="essayScore.dataSelection > Math.min(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection) ||
                          essayScore.dataSelection == Math.max(essayScore.highlightKeyFeatures, essayScore.compareAndContrast, essayScore.dataSelection)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.dataSelection.toString().length == 1 ? essayScore.dataSelection.toString() + '.0' : essayScore.dataSelection }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Data Selection and Relevance</div>
                    </div>
                    <div class="sub-criteria">
                      <el-tag v-if="essayScore.appropriateWordCount" type="success" size="small">
                        <i class="criteria-score-check el-icon-check" />
                      </el-tag>
                      <el-tag v-else type="danger" size="small">
                        <i class="el-icon-close criteria-score-close" />
                      </el-tag>
                      <span class="sub-criteria-label">Appropriate Word Count</span>
                    </div>
                  </div>

                  <div v-if="essayScore.taskResponseScore" class="criteria-score">
                    <div>
                      <b>
                        Task Response:
                        {{ essayScore.taskResponseScore.toString().length == 1 ? essayScore.taskResponseScore.toString() + '.0' : essayScore.taskResponseScore }}
                      </b>
                    </div>
                    <div v-if="essayScore.clarityOfPosition" class="sub-criteria">
                      <el-tag
                        :type="essayScore.clarityOfPosition > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                          essayScore.clarityOfPosition == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.clarityOfPosition.toString().length == 1 ? essayScore.clarityOfPosition.toString() + '.0' : essayScore.clarityOfPosition }}
                        </span>
                      </el-tag>
                      <span class="sub-criteria-label">Clarity of Position</span>
                    </div>
                    <div v-if="essayScore.developmentOfIdeas" class="sub-criteria">
                      <el-tag
                        :type="essayScore.developmentOfIdeas > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                          essayScore.developmentOfIdeas == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.developmentOfIdeas.toString().length == 1 ? essayScore.developmentOfIdeas.toString() + '.0' : essayScore.developmentOfIdeas }}
                        </span>
                      </el-tag>
                      <span class="sub-criteria-label">Development of Ideas</span>
                    </div>
                    <div v-if="essayScore.justificationOfOpinion" class="sub-criteria">
                      <el-tag
                        :type="essayScore.justificationOfOpinion > Math.min(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion) ||
                          essayScore.justificationOfOpinion == Math.max(essayScore.clarityOfPosition, essayScore.developmentOfIdeas, essayScore.justificationOfOpinion)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.justificationOfOpinion.toString().length == 1 ? essayScore.justificationOfOpinion.toString() + '.0' : essayScore.justificationOfOpinion }}
                        </span>

                      </el-tag>
                      <span class="sub-criteria-label">Justification of Opinions</span>
                    </div>
                    <div class="sub-criteria">
                      <el-tag v-if="essayScore.appropriateWordCount" type="success" size="small">
                        <i class="criteria-score-check el-icon-check" />
                      </el-tag>
                      <el-tag v-else type="danger" size="small">
                        <i class="el-icon-close criteria-score-close" />
                      </el-tag>
                      <span class="sub-criteria-label">Appropriate Word Count</span>
                    </div>
                  </div>

                  <div v-if="essayScore.coherenceScore" class="criteria-score">
                    <div>
                      <b>
                        Coherence and Cohesion:
                        {{ essayScore.coherenceScore.toString().length == 1 ? essayScore.coherenceScore.toString() + '.0' : essayScore.coherenceScore }}
                      </b>
                    </div>
                    <div v-if="essayScore.logicalOrganization" class="sub-criteria">
                      <el-tag
                        :type="essayScore.logicalOrganization > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                          essayScore.logicalOrganization == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.logicalOrganization.toString().length == 1 ? essayScore.logicalOrganization.toString() + '.0' : essayScore.logicalOrganization }}
                        </span>
                      </el-tag>
                      <div class="sub-criteria-label">Logical Organization</div>
                    </div>
                    <div v-if="essayScore.paragraphing" class="sub-criteria">
                      <el-tag
                        :type="essayScore.paragraphing > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                          essayScore.paragraphing == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.paragraphing.toString().length == 1 ? essayScore.paragraphing.toString() + '.0' : essayScore.paragraphing }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Paragraphing</div>
                    </div>
                    <div v-if="essayScore.cohesiveDevices" class="sub-criteria">
                      <el-tag
                        :type="essayScore.cohesiveDevices > Math.min(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices) ||
                          essayScore.cohesiveDevices == Math.max(essayScore.logicalOrganization, essayScore.paragraphing, essayScore.cohesiveDevices)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.cohesiveDevices.toString().length == 1 ? essayScore.cohesiveDevices.toString() + '.0' : essayScore.cohesiveDevices }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Use of Cohesive Devices</div>
                    </div>
                  </div>

                  <div v-if="essayScore.lexicalResourceScore" class="criteria-score">
                    <div>
                      <b>
                        Lexical Resource:
                        {{ essayScore.lexicalResourceScore.toString().length == 1 ? essayScore.lexicalResourceScore.toString() + '.0' : essayScore.lexicalResourceScore }}
                      </b>
                    </div>
                    <div v-if="essayScore.rangeOfVocabulary" class="sub-criteria">
                      <el-tag
                        :type="essayScore.rangeOfVocabulary > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                          essayScore.rangeOfVocabulary == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.rangeOfVocabulary.toString().length == 1 ? essayScore.rangeOfVocabulary.toString() + '.0' : essayScore.rangeOfVocabulary }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Range of Vocabulary</div>
                    </div>
                    <div v-if="essayScore.accuracyOfWordChoice" class="sub-criteria">
                      <el-tag
                        :type="essayScore.accuracyOfWordChoice > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                          essayScore.accuracyOfWordChoice == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.accuracyOfWordChoice.toString().length == 1 ? essayScore.accuracyOfWordChoice.toString() + '.0' : essayScore.accuracyOfWordChoice }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Accuracy of Word Choice</div>
                    </div>
                    <div v-if="essayScore.spellingAndFormation" class="sub-criteria">
                      <el-tag
                        :type="essayScore.spellingAndFormation > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                          essayScore.spellingAndFormation == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.spellingAndFormation.toString().length == 1 ? essayScore.spellingAndFormation.toString() + '.0' : essayScore.spellingAndFormation }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Spelling and Word Formation</div>
                    </div>
                    <div v-if="essayScore.registerAndStyle" class="sub-criteria">
                      <el-tag
                        :type="essayScore.registerAndStyle > Math.min(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle) ||
                          essayScore.registerAndStyle == Math.max(essayScore.rangeOfVocabulary, essayScore.accuracyOfWordChoice, essayScore.spellingAndFormation, essayScore.registerAndStyle)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.registerAndStyle.toString().length == 1 ? essayScore.registerAndStyle.toString() + '.0' : essayScore.registerAndStyle }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Appropriateness of Register and Style</div>
                    </div>
                  </div>

                  <div v-if="essayScore.grammarScore" class="criteria-score">
                    <div>
                      <b>
                        Grammatical Range and Accuracy:
                        {{ essayScore.grammarScore.toString().length == 1 ? essayScore.grammarScore.toString() + '.0' : essayScore.grammarScore }}
                      </b>
                    </div>
                    <div v-if="essayScore.grammarRange" class="sub-criteria">
                      <el-tag
                        :type="essayScore.grammarRange > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                          essayScore.grammarRange == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.grammarRange.toString().length == 1 ? essayScore.grammarRange.toString() + '.0' : essayScore.grammarRange }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Range of Grammatical Structures</div>
                    </div>
                    <div v-if="essayScore.sentenceComplexity" class="sub-criteria">
                      <el-tag
                        :type="essayScore.sentenceComplexity > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                          essayScore.sentenceComplexity == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.sentenceComplexity.toString().length == 1 ? essayScore.sentenceComplexity.toString() + '.0' : essayScore.sentenceComplexity }}
                        </span>

                      </el-tag>
                      <div class="sub-criteria-label">Sentence Complexity</div>
                    </div>
                    <div v-if="essayScore.grammarAccuracy" class="sub-criteria">
                      <el-tag
                        :type="essayScore.grammarAccuracy > Math.min(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy) ||
                          essayScore.grammarAccuracy == Math.max(essayScore.grammarRange, essayScore.sentenceComplexity, essayScore.grammarAccuracy)
                          ? 'success' : 'danger'"
                        size="small"
                        class="sub-score-tag"
                      >
                        <span>
                          {{ essayScore.grammarAccuracy.toString().length == 1 ? essayScore.grammarAccuracy.toString() + '.0' : essayScore.grammarAccuracy }}
                        </span>
                      </el-tag>
                      <div class="sub-criteria-label">Accuracy in Grammatical Forms</div>
                    </div>
                  </div>
                </div>

                <div v-if="review && review.reviewRequest && review.reviewRequest.feedbackLanguage == 'vn'" style="margin-top: 10px;">
                  <div style="background-color: rgb(255 241 210); border-color: rgb(233, 233, 235); color: rgb(75 111 138); font-size: 15px; border-width: 1px; border-style: solid; border-radius: 4px; padding: 10px; border-left: #efa800 solid 4px;">
                    Xin lưu ý rằng điểm số được cung cấp không phải là điểm IELTS chính thức của bạn và chỉ mang tính tham khảo. Bạn nên sử dụng phản hồi để cải thiện bài luận của mình thay vì tập trung vào điểm số.
                  </div>
                </div>
                <div v-else style="margin-top: 10px;">
                  <div style="background-color: rgb(255 241 210); border-color: rgb(233, 233, 235); color: rgb(75 111 138); font-size: 15px; border-width: 1px; border-style: solid; border-radius: 4px; padding: 10px; border-left: #efa800 solid 4px;">
                    Please note that the scores provided are not your official IELTS scores and should be used solely for learning and practice purposes. You should focus on the feedback that helps you improve the essay rather than on the score.
                  </div>
                </div>

              </el-card>
            </div>
            <div v-else>
              <div style="background: rgb(248 249 250); height: 200px; border: #bcbcbc solid 1px; padding-top: 40px; border-radius: 5px; margin-bottom: 5px;">
                <div class="el-loading-spinner" style="position: relative; top: 50px;">
                  <svg viewBox="25 25 50 50" class="circular"><circle cx="50" cy="50" r="20" fill="none" class="path" /></svg>
                  <p class="el-loading-text" style="word-break: break-word;">Đang chấm điểm 4 tiêu chí</p>
                </div>
              </div>
            </div>

            <div
              v-for="criteria in rubricCriteria"
              :key="criteria.criteriaId"
            >
              <el-card

                style="margin-bottom: 5px; border: 1px solid rgb(190, 190, 190);"
                shadow="hover"
              >
                <div slot="header" class="clearfix">
                  <div>
                    <div style="float: left; font-size: 18px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                      <strong v-if="criteria.name == 'Critical Errors'">Nâng Cấp Từ Vựng Và Ngữ Pháp</strong>
                      <strong v-else-if="criteria.name == 'Arguments Assessment'">Củng Cố Lập Luận</strong>
                      <strong v-else-if="criteria.name == 'Vocabulary'">Từ Vựng Tham Khảo</strong>
                      <strong v-else-if="criteria.name == 'Improved Version'">Bài Chữa Tham Khảo</strong>
                      <strong v-else>{{ criteria.name }}</strong>
                    </div>
                    <div style="float: right;">
                      <div v-if="criteria.name != 'Critical Errors' && criteria.name != 'Arguments Assessment' && criteria.name != 'Vocabulary' && criteria.name != 'Improved Version'">
                        <div v-if="isAiReview">
                          <div v-if="!criteria.loading && criteria.mark" class="band-score">
                            Band:
                            {{ criteria.mark.toString().length == 1 ? criteria.mark.toString() + '.0' : criteria.mark }}
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div>
                  <div>
                    <div>
                      <div v-if="criteria.name == 'Vocabulary' || criteria.name == 'Improved Version'">
                        <pre style="border: #bcbcbc solid 1px; padding: 10px; border-radius: 5px; background: #f7f7f7;" v-html="criteria.comment" />
                      </div>
                      <div v-else style="border: #bcbcbc solid 1px; padding: 10px; border-radius: 5px; background: #f7f7f7;" v-html="criteria.comment" />
                    </div>
                  </div>
                </div>
              </el-card>
            </div>

            <div>
              <el-card
                style="margin-bottom: 5px; border: 1px solid rgb(190, 190, 190);"
                shadow="hover"
              >
                <div slot="header" class="clearfix">
                  <div>
                    <div style="float: left; font-size: 18px; color: #4a6f8a; font-weight: 500; word-break: break-word; overflow: hidden; white-space: nowrap;">
                      <strong>Nâng Cấp Từ Vựng Và Ngữ Pháp</strong>
                    </div>
                  </div>
                </div>
                <div>
                  <div>
                    <div>
                      <el-card
                        v-for="comment in comments"
                        :key="comment.uuid"
                        class="intext-comment"
                        style="margin-bottom: 10px;"
                      >
                        <div
                          slot="header"
                          class="clearfix"
                          :style="comment.type.toLowerCase() == 'grammar' || comment.type.toLowerCase() == 'ngữ pháp' ? 'padding: 8px 12px; background: #ffd6d0;' : 'padding: 8px 12px; background: rgb(213 204 255);'"
                        >
                          <div>
                            <div
                              v-if="comment.category"
                              style="font-weight: 500; font-size: 15px; text-align: left;  width: 220px; text-overflow: ellipsis; word-break: break-word;  overflow: hidden; white-space: nowrap;"
                            >
                              {{ comment.category }}
                            </div>
                          </div>
                        </div>
                        <div>
                          <div style="margin-bottom: 10px; border-bottom: #aeaeae dashed 1px; padding-bottom: 10px;">
                            <span style="color: #ffa600;">{{ comment.text }}</span>  <i class="el-icon-right" /> <span>{{ comment.fix }}</span>
                          </div>
                          <div
                            v-show="!comment.isSelected "
                            :id="'comment-text-' + comment.uuid"
                            :style="{'-webkit-line-clamp': isInShowMoreList(comment.uuid).value==0 ? '3':'inherit'}"
                            style="text-align: left; overflow-wrap: break-word; display: -webkit-box;overflow: hidden;-webkit-box-orient: vertical;"
                          >
                            <div>{{ comment.comment }}</div>
                          </div>
                        </div>
                      </el-card>
                    </div>
                  </div>
                </div>
              </el-card>
            </div>

          </div>

        </div> -->

      </div>
    </el-card>

    <!-- Document content -->
  </div>
</template>

<script>
import moment from 'moment-timezone'
import questionService from '@/services/question.service'

export default {
  data() {
    return {
      docName: this.$route.params.docName,
      question: null,
      questionId: 3074,
      writingContent: null,
      isShowCountWord: true,
      countWord: 0

    }
  },
  watch: {
    '$route.params.docName'(newDocName, oldDocName) {
      if (newDocName !== oldDocName) {
        this.docName = newDocName
      }
    }
  },
  async mounted() {
    // Todo: replace with get question by display name (probal-protein-intake)
    this.question = await questionService.getById(this.questionId)
    console.log('Question:', this.question)

    if (this.question.questionsPart && this.question.questionsPart.length > 0) {
      var contentPart = this.question.questionsPart.find(q => q.name == 'Question')
      if (contentPart) {
        this.question.content = contentPart.content
      }

      var chartPart = this.question.questionsPart.find(q => q.name == 'Chart')
      if (chartPart) {
        this.question.chart = chartPart.content
      }
    }
  },
  created() {
     // Ensure the docTitle is correctly set when the component is created
     this.docName = this.$route.params.docName
  },
  methods: {
    countWords() {
      if (!this.isStart) {
        this.isStart = true
        this.timeStart = moment()
      }

      this.countWord = this.writingContent ? this.writingContent.trim().split(/\b\S+\b/).length - 1 : 0

      clearTimeout(this.timeout)
      this.timeout = setTimeout(() => {
        if (this.idSubmissionStorage) {
          localStorage.setItem(this.idSubmissionStorage, this.writingContent)
        }
        if (!this.$route.params.submissionId) {
          localStorage.setItem(this.idLocalStorage, this.writingContent)
        }
      }, 50)
    }
  }
}
</script>

<style scoped>
p {
  color: black;
}
.container {
    max-width: 800px;
    margin: auto;
    background: #fff;
    padding: 30px;
    border-radius: 5px;
    font-size: 16px;
}
h1, h2, h3 , h4, h5{
    color: #333;
}
h1 {
    text-align: center;
    margin-bottom: 30px;
}
h2 {
    margin-top: 40px;
    margin-bottom: 20px;
    font-size: 24px;
    font-weight: 500;
}
h3 {
  font-size: 20px;
  font-weight: 500;
}
h4 {
  font-size: 18px;
  font-weight: 500;
}
ul {
    margin-left: 20px;
}
li {
    margin-bottom: 10px;
}
.module {
    margin-bottom: 30px;
}
.note {
    background-color: #eaf2f8;
    padding: 15px;
    border-left: 4px solid #2980b9;
    margin: 20px 0;
}
table {
    width: 100%;
    border-collapse: collapse;
    margin: 20px 0;
}
table, th, td {
    border: 1px solid #bdc3c7;
}
th, td {
    padding: 12px;
    text-align: left;
}
.highlight {
    background-color: #f1c40f;
    padding: 2px 5px;
    border-radius: 3px;
}
code {
    background-color: #ecf0f1;
    padding: 2px 5px;
    border-radius: 3px;
    font-family: Consolas, monospace;
}
.textarea-style {
  width: 100%;
  padding: 15px;
  border: 1px solid #e2e2e2;
  resize: none;
  outline: none;
  height: 600px;
  border-radius: 5px;
}

.header-passage {
  margin-bottom: 5px;
  font-size: 13px;
  min-width: 400px;
}

</style>
