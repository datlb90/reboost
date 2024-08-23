import questionService from '@/services/question.service'
// import reviewService from '@/services/review.service'
import documentService from '@/services/document.service'
import sampleService from '../../services/sample.service'

const getDefaultState = () => {
  return {
    questions: [],
    selectedQuestion: {},
    countQuestions: {},
    countQuestionsByUser: {},
    testByUser: {},
    statusQuestion: {},
    sampleByQuestion: [],
    samples: [],
    personalQuestion: null,
    questionsForInitialTest: [
      {
        'id': 3074,
        'title': 'Global Protein and Calorie Intake',
        'section': 'Academic Writing Task 1',
        'taskId': 0,
        'test': 'IELTS',
        'testId': 0,
        'time': '20 minutes',
        'type': 'Bar Chart',
        'sample': true,
        'averageScore': '0.0',
        'submission': 0,
        'like': 0,
        'dislike': 0,
        'status': 'To do',
        'difficulty': 'Medium',
        'direction': 'You should spend about 20 minutes on this task.\n\nWrite at least 150 words.',
        'userId': null,
        'createdBy': null,
        'addedDate': '0001-01-01T00:00:00',
        'lastActivityDate': '0001-01-01T00:00:00',
        'questionsPart': [
          {
            'questionId': 3074,
            'name': 'Chart',
            'content': '3074.jpeg'
          },
          {
            'questionId': 3074,
            'name': 'Question',
            'content': '<p><span style="font-size: 16px"><span style="color: rgb(1, 5, 28);">The charts below show the protein and calorie intakes of people in different parts of the world.</span></span></p><p><span style="font-size: 16px"><span style="color: rgb(1, 5, 28);">Summarise the information by selecting and reporting the main features, and make comparisons where relevant.</span></span></p>'
          },
          {
            'questionId': 3074,
            'name': 'Tip',
            'content': '<p><strong><u><span style="font-size: 18px">Ý Tưởng Phát Triển Bài</span></u></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Mở đầu</span></strong><span style="font-size: 16px">: Giới thiệu chung về hai biểu đồ: biểu đồ thứ nhất thể hiện lượng protein tiêu thụ ở mỗi khu vực, còn biểu đồ thứ hai cho thấy lượng calo tiêu thụ hàng ngày.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Lượng Protein Tiêu Thụ</span></strong><span style="font-size: 16px">: So sánh lượng protein tiêu thụ giữa người dân ở Ấn Độ, Đông Phi, Mỹ Latinh và Bắc Mỹ, nhấn mạnh vào việc người dân Bắc Mỹ tiêu thụ lượng protein cao hơn nhiều so với các khu vực khác.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Protein Động Vật và Protein Khác</span></strong><span style="font-size: 16px">: Phân tích tỷ lệ protein động vật và protein khác (thực vật) tiêu thụ ở mỗi khu vực, nêu bật sự khác biệt lớn về nguồn gốc protein giữa Bắc Mỹ so với các khu vực khác.</span></p><p><span style="font-size: 16px">4. </span><strong><span style="font-size: 16px">Protein Lý Tưởng</span></strong><span style="font-size: 16px">: So sánh lượng protein tiêu thụ thực tế với mức protein lý tưởng (được thể hiện bằng đường kẻ ngang trong biểu đồ) và bình luận về sự chênh lệch.</span></p><p><span style="font-size: 16px">5. </span><strong><span style="font-size: 16px">Lượng Calo Tiêu Thụ</span></strong><span style="font-size: 16px">: Mô tả và so sánh lượng calo tiêu thụ hàng ngày ở mỗi khu vực, đặc biệt là sự vượt trội của Bắc Mỹ so với các khu vực khác.</span></p><p><span style="font-size: 16px">6. </span><strong><span style="font-size: 16px">Calo Lý Tưởng</span></strong><span style="font-size: 16px">: Đánh giá mức độ phù hợp của lượng calo tiêu thụ so với mức calo lý tưởng được đề xuất.</span></p><p><span style="font-size: 16px">7. </span><strong><span style="font-size: 16px">Sự Khác Biệt Địa Lý</span></strong><span style="font-size: 16px">: Bàn luận về những yếu tố văn hóa, kinh tế, và địa lý có thể ảnh hưởng đến sự khác biệt về chế độ ăn giữa các khu vực này.</span></p><p><span style="font-size: 16px">8. </span><strong><span style="font-size: 16px">Ảnh Hưởng Sức Khỏe</span></strong><span style="font-size: 16px">: Thảo luận về ảnh hưởng của việc tiêu thụ lượng protein và calo như biểu đồ thể hiện đối với sức khỏe của cư dân các khu vực.</span></p><p><span style="font-size: 16px">9. </span><strong><span style="font-size: 16px">Gợi Ý Cho Sự Cải Thiện</span></strong><span style="font-size: 16px">: Đề xuất các phương hướng và giải pháp để cải thiện tình hình dinh dưỡng, nhất là ở những nơi có sự chênh lệch lớn so với mức tiêu thụ lý tưởng.</span></p><p><span style="font-size: 16px">10. </span><strong><span style="font-size: 16px">Kết Luận</span></strong><span style="font-size: 16px">: Tóm tắt những điểm chính đã phân tích và bày tỏ quan điểm cá nhân về sự cần thiết của việc cân bằng chế độ ăn uống, cũng như ý thức về sức khỏe dinh dưỡng trên toàn cầu.</span></p><p><span style="font-size: 16px">Hãy bắt đầu từ những ý tưởng trên để phát triển một bài viết thuyết phục và toàn diện, đồng thời không quên dùng ngôn từ và cấu trúc câu phù hợp với tiêu chuẩn IELTS.</span></p><p><strong><u><span style="font-size: 18px">Gợi Ý Cho Cấu Trúc Bài</span></u></strong></p><p><strong><span style="font-size: 16px">Mở bài:</span></strong></p><p><span style="font-size: 16px">- Giới thiệu chung: Bắt đầu bằng cách giới thiệu chung về chủ đề của biểu đồ, mục đích của việc phân tích, và thông tin cơ bản về loại dữ liệu được thể hiện (lượng protein và calo tiêu thụ của người dân ở các khu vực khác nhau trên thế giới).</span></p><p><span style="font-size: 16px">- Mô tả biểu đồ: Mô tả sơ lược về cách thức trình bày của hai biểu đồ, bao gồm cả việc nhận xét ngắn gọn về mức tiêu thụ lý tưởng được chỉ ra trong mỗi biểu đồ.</span></p><p><strong><span style="font-size: 16px">Thân bài:</span></strong></p><p><span style="font-size: 16px">1. Đoạn văn 1: Phân tích biểu đồ thứ nhất - Lượng Protein Tiêu Thụ</span></p><p><span style="font-size: 16px">   - So sánh số liệu protein giữa các khu vực, nhấn mạnh vào sự chênh lệch đáng kể.</span></p><p><span style="font-size: 16px">   - Bình luận về sự phân bố giữa protein động vật và protein khác.</span></p><p><span style="font-size: 16px">2. Đoạn văn 2: Phân tích biểu đồ thứ hai - Lượng Calo Tiêu Thụ</span></p><p><span style="font-size: 16px">   - So sánh và đánh giá lượng calo tiêu thụ giữa các khu vực.</span></p><p><span style="font-size: 16px">   - Phân tích mức tiêu thụ thực tế so với mức tiêu thụ lý tưởng.</span></p><p><span style="font-size: 16px">3. Đoạn văn 3: Thảo luận về các yếu tố ảnh hưởng</span></p><p><span style="font-size: 16px">   - Đề cập đến các yếu tố văn hóa, kinh tế, và địa lý có thể đã góp phần tạo nên những khác biệt này.</span></p><p><span style="font-size: 16px">   - Bàn luận về ảnh hưởng tiềm ẩn đối với sức khỏe và đề xuất cách cải thiện.</span></p><p><strong><span style="font-size: 16px">Kết luận:</span></strong></p><p><span style="font-size: 16px">- Tóm lược: Tóm tắt lại những điểm chính từ thân bài, bao gồm cả những so sánh quan trọng và nhận định về xu hướng tiêu thụ.</span></p><p><span style="font-size: 16px">- Quan điểm cá nhân: Đưa ra quan điểm cá nhân về tầm quan trọng của việc cân bằng chế độ ăn uống, cũng như sự chênh lệch về dinh dưỡng giữa các khu vực.</span></p><p><span style="font-size: 16px">- Kêu gọi hành động: Nếu thích hợp, kết thúc bài viết bằng cách kêu gọi hành động hoặc sự chú ý đến vấn đề dinh dưỡng toàn cầu, nhấn mạnh sự cần thiết của việc có chính sách và giáo dục dinh dưỡng tốt hơn ở các khu vực cần thiết.</span></p><p></p><p></p><p></p>'
          },
          {
            'questionId': 3074,
            'name': 'Vocabulary',
            'content': '<p><span style="font-size: 16px"><span style="color: #000000;">1. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Disparity</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /dɪˈspær.ɪ.ti/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: sự chênh lệch</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: a great difference</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Advanced</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">disparity</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> in protein intake between different regions is stark.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: imbalance, inequality</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">2. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Substantially</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /səbˈstan.ʃəl.i/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: đáng kể</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: to a large degree</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: North America consumes </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">substantially</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> more animal protein than India.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: significantly, considerably</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">3. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Marginal</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˈmɑːr.dʒɪ.nəl/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: ít, không đáng kể</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: small and not important</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: There is only a </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">marginal</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> difference in calorie intake between East Africa and Latin America.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: minimal, slight</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">4. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Predominantly</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /prɪˈdɒm.ɪ.nənt.li/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: chủ yếu, phần lớn</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: mostly or mainly</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The diet in Latin America is </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">predominantly</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> based on other forms of protein rather than animal protein.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: primarily, mainly</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">5. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Dichotomy</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /daɪˈkɒt.ə.mi/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: sự phân chia</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: a division or contrast between two things that are represented as being opposed or entirely different</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Advanced</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The charts illustrate a clear </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">dichotomy</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> in dietary habits across the regions.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: contrast, split</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">6. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Outstrips</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /aʊtˈstrɪps/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: vượt trội, vượt qua</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: to be or become greater in amount, degree, or success than something or someone</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Advanced</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: Calorie intake in North America </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">outstrips</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> that of other regions.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: exceeds, surpasses</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">7. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Inter-regional</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˌɪn.təˈriː.dʒən.əl/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: giữa các vùng</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: between regions</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: An </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">inter-regional</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> comparison shows significant dietary differences.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: between regions</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">8. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Trends</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /trendz/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: xu hướng</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: general directions in which something is developing or changing</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">trends</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> indicate a move towards higher calorie diets.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: tendencies, patterns</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">9. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Surpasses</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /səˈpɑːs.ɪz/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">: vượt qua</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: to do or be better than</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: Protein consumption in North America </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">surpasses</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> the ideal protein intake.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: exceeds, outdoes</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">10. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Aggregate</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˈæɡ.rɪ.gət/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: tổng cộng</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: total, combined</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Advanced</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">aggregate</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> intake of calories in India falls below the ideal level.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: total, sum</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">11. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Deviation</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˌdiː.viˈeɪ.ʃən/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: sự lệch lạc</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: a departure from a standard or norm</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: There is a noticeable </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">deviation</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> from the ideal calorie intake in North America.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: departure, divergence</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">12. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Contrastingly</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /kənˈtrɑː.stɪŋ.li/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: ngược lại, trái ngược</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: in a way that is strikingly different</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Advanced</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Contrastingly</span></span></strong><span style="font-size: 16px"><span style="color: #000000;">, the protein intake in India is predominantly non-animal based.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: conversely, oppositely</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">13. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Inadequate</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ɪnˈæd.ɪ.kwət/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: không đủ</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: not enough or not good enough</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: Calorie intake in East Africa is </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">inadequate</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> when compared to the ideal standard.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: insufficient, deficient</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">14. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Sufficiency</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /səˈfɪʃ.ən.si/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: sự đủ đầy</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: an adequate amount or quantity; sufficiency</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Advanced</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">sufficiency</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> of protein intake in North America is apparent.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: adequacy, enough</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">15. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Proportion</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /prəˈpɔː.ʃən/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: tỉ lệ</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: a part or share of the whole</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">proportion</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> of animal protein is highest in North America.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: ratio, fraction</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">16. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Subsequent</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˈsʌb.sɪ.kwənt/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: sau đó, tiếp theo</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: happening after something else</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Subsequent</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> studies may reveal changes in these dietary patterns.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: following, ensuing</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">17. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Markedly</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˈmɑːkɪd.li/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: rõ ràng, đáng chú ý</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: in a way that is clearly noticeable</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The calorie intake in North America is </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">markedly</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> higher than in other regions.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: significantly, noticeably</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">18. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Fluctuations</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˌflʌk.tʃuˈeɪ.ʃənz/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: sự biến động</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: changes, especially continuous and not regular</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Advanced</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: There are seasonal </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">fluctuations</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> in calorie and protein intakes.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: variations, oscillations</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">19. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Optimal</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /ˈɒp.tɪ.məl/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: tốt nhất, lý tưởng nhất</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: best or most effective</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: The </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">optimal</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> level of calorie intake is not reached in several regions.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: ideal, best</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">20. </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">Discrepancy</span></span></strong></p><p><span style="font-size: 16px"><span style="color: #000000;">- Phát âm: /dɪˈskrep.ən.si/</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa: sự khác biệt</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Nghĩa tiếng Anh: a difference between two things that should be the same</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Cấp độ: Upper Intermediate</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Ví dụ: There is a significant </span></span><strong><span style="font-size: 16px"><span style="color: #000000;">discrepancy</span></span></strong><span style="font-size: 16px"><span style="color: #000000;"> in the dietary habits between India and North America.</span></span></p><p><span style="font-size: 16px"><span style="color: #000000;">- Từ đồng nghĩa: difference, divergence</span></span></p><p></p>'
          }
        ],
        'samples': [
          {
            'id': 353,
            'questionId': 3074,
            'sampleText': '<p><span style="font-size: 16px"><span style="color: rgb(13, 13, 13);">The provided bar charts compare the average protein and calorie intakes of people in India, East Africa, Latin America, and North America, with a benchmark indicating the ideal intake levels.</span></span></p><p><span style="font-size: 16px"><span style="color: rgb(13, 13, 13);">Starting with protein consumption, North Americans exceed the ideal intake of 70mg, with a significant portion sourced from animals. Latin Americans also surpass the ideal, but less dramatically, with a more balanced animal to other protein ratio. East Africans consume exactly the ideal protein quantity, heavily relying on non-animal sources. In contrast, India falls short of the ideal protein mark, with the majority of its modest intake coming from other proteins.</span></span></p><p><span style="font-size: 16px"><span style="color: rgb(13, 13, 13);">In terms of calories, all four regions consume less than the ideal 3,500 calories per day. North America leads with the highest intake, closely followed by Latin America, both exceeding 3,000 calories. East Africa and India have considerably lower calorie intakes, with India at the bottom, barely reaching 2,000 calories.</span></span></p><p><span style="font-size: 16px"><span style="color: rgb(13, 13, 13);">In summary, North America stands out with the highest intake of both protein and calories, considerably overshooting the protein requirement, while India has the lowest figures in both categories. Protein and calorie intake patterns reveal stark contrasts between the different regions, reflecting varied dietary habits and access to food resources.</span></span></p>',
            'bandScore': 8,
            'comment': '<p><strong><span style="font-size: 16px">1. Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">- Cung cấp thông tin dựa trên dữ liệu cụ thể từ biểu đồ.</span></p><p><span style="font-size: 16px">- Bố cục rõ ràng, thông tin được tổ chức logic.</span></p><p><span style="font-size: 16px">- Ngôn từ sử dụng đa dạng, từ vựng phong phú và chính xác.</span></p><p><span style="font-size: 16px">- Cấu trúc câu đa dạng, giúp truyền đạt thông tin hiệu quả.</span></p><p><span style="font-size: 16px">- Đoạn văn có sự phân chia tốt, thông tin trình bày có hệ thống.</span></p><p><strong><span style="font-size: 16px">2. Điểm yếu:</span></strong></p><p><span style="font-size: 16px">- Thiếu sự phân tích sâu hơn về nguyên nhân hoặc hậu quả của các dữ liệu biểu đồ.</span></p><p><span style="font-size: 16px">- Không đề cập đến các giả thuyết có thể giải thích sự chênh lệch trong lượng tiêu thụ giữa các khu vực.</span></p><p><span style="font-size: 16px">- Có thể cải thiện bằng cách bổ sung thông tin về tác động của sự tiêu thụ protein và calo đến sức khỏe và lối sống.</span></p><p><strong><span style="font-size: 16px">3. Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">Bài luận đã thành công trong việc trình bày thông tin quan trọng từ biểu đồ và thực hiện một so sánh có ý nghĩa giữa các khu vực khác nhau. Việc sử dụng ngôn ngữ đa dạng và chính xác cũng như cấu trúc câu phức tạp cho thấy khả năng viết lách tốt. Tuy nhiên, để đạt đến mức độ xuất sắc, bài luận cần mở rộng phạm vi phân tích và cung cấp thêm bối cảnh hoặc giả thuyết về các số liệu. Nhìn chung, bài viết đã gần đạt tiêu chuẩn của một bài luận band 8.0 trong IELTS, với một số cải tiến nhỏ cần thiết để đạt được độ hoàn thiện cao hơn.</span></p>',
            'lastActivityDate': '2024-04-29T21:03:04.6756653',
            'title': 'Global Protein and Calorie Intake'
          },
          {
            'id': 354,
            'questionId': 3074,
            'sampleText': '<p><span style="font-size: 16px">The bar charts illustrate the disparity in protein and calorie consumption among populations in India, East Africa, Latin America, and North America, with reference to ideal intake levels. Notably, protein intake is ideally pegged at 70mg, a figure that only East Africa meets precisely, predominantly through non-animal sources. North America exceeds this benchmark significantly, with a strong preference for animal proteins. Latin America, while also above the ideal, maintains a more balanced intake between animal and other protein sources. India, on the other hand, falls below the ideal mark with a predominant consumption of non-animal protein.</span></p><p><span style="font-size: 16px">Caloric intake shows a different trend, with all regions falling short of the ideal 3,500 calories per day. North America leads with the closest to ideal consumption, followed by Latin America, both above the 3,000-calorie mark. East Africa and India trail with considerably lower intakes, especially India, which barely surpasses the 2,000-calorie threshold.</span></p><p><span style="font-size: 16px">In conclusion, while protein intake varies with North America consuming the most, and India the least, calorie consumption is universally below the ideal, hinting at potential dietary inadequacies across these diverse regions. This data suggests a significant nutritional divide that could have implications for global health trends.</span></p>',
            'bandScore': 7,
            'comment': '<p><strong><span style="font-size: 16px">1. Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">- Bài luận cung cấp một cái nhìn tổng quan rõ ràng và chính xác về các xu hướng chính từ biểu đồ.</span></p><p><span style="font-size: 16px">- Có sự sắp xếp thông tin một cách logic, với một trình tự ý tốt, giúp người đọc dễ theo dõi.</span></p><p><span style="font-size: 16px">- Sử dụng đa dạng từ vựng và cấu trúc câu, góp phần vào sự rõ ràng và mạch lạc.</span></p><p><span style="font-size: 16px">- Cung cấp thông tin cụ thể và so sánh các dữ liệu một cách có ý nghĩa.</span></p><p><strong><span style="font-size: 16px">2. Điểm yếu:</span></strong></p><p><span style="font-size: 16px">- Một số chi tiết có thể được mở rộng thêm để cung cấp độ sâu và ngữ cảnh hơn cho người đọc.</span></p><p><span style="font-size: 16px">- Có thể thiếu sự phân tích về nguyên nhân hoặc hậu quả của các xu hướng tiêu thụ protein và calo.</span></p><p><span style="font-size: 16px">- Những lỗi ngữ pháp nhỏ có thể xuất hiện nhưng không ảnh hưởng nghiêm trọng đến ý nghĩa tổng thể của bài viết.</span></p><p><strong><span style="font-size: 16px">3. Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">Bài luận đã thành công trong việc trình bày thông tin từ biểu đồ một cách tổng quan và có hệ thống, với sự sử dụng tốt các từ nối và cấu trúc câu. Mặc dù cần thêm chi tiết để đạt được độ phong phú trong phân tích, tổng thể bài luận đã đáp ứng được yêu cầu của một bài viết band 7 IELTS, với việc truyền đạt thông tin một cách rõ ràng và có tổ chức, cùng với việc sử dụng từ vựng đa dạng và chính xác. Nhìn chung, bài viết đảm bảo thông tin được trình bày mạch lạc và có sự tiến triển, làm nổi bật các xu hướng chính một cách hiệu quả.</span></p>',
            'lastActivityDate': '2024-04-29T21:07:07.9311038',
            'title': 'Global Protein and Calorie Intake'
          },
          {
            'id': 355,
            'questionId': 3074,
            'sampleText': "<p><span style=\"font-size: 16px\">The bar charts compare the intake of proteins and calories in various regions, showing a striking difference between the diet patterns of India, East Africa, Latin America, and North America. It's observed that North Americans consume the most protein, exceeding the ideal level of 70mg, mainly from animal sources. In contrast, India has the lowest intake, not reaching the ideal level, and relies more on non-animal protein.</span></p><p><span style=\"font-size: 16px\">Calorie intake follows a similar regional disparity. North America nearly meets the ideal consumption with the highest calorie intake, while India's intake is substantially lower, marking the least among the surveyed regions. East Africa and Latin America stand in the middle, yet none of these regions meet the ideal calorie intake.</span></p><p><span style=\"font-size: 16px\">Overall, while the data shows North America as the leading consumer in both categories, it's clear that none of the regions manage to meet the ideal calorie intake. This could indicate a global nutritional challenge. Although the overview provided is adequate, more specific data could enhance the comparison. The essay maintains coherence with some range in vocabulary and varied sentence structures, although minor errors are present.</span></p>",
            'bandScore': 6,
            'comment': '<p><strong><span style="font-size: 16px">1. Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">- Bài luận đã xác định rõ các xu hướng tiêu thụ protein và calo ở các khu vực khác nhau.</span></p><p><span style="font-size: 16px">- Bài viết có cấu trúc tổ chức, với một dòng ý tiến triển một cách rõ ràng.</span></p><p><span style="font-size: 16px">- Sử dụng đủ loại từ vựng và cấu trúc câu đơn giản lẫn phức tạp, giúp bài viết không bị đơn điệu.</span></p><p><span style="font-size: 16px">- Thông tin được trình bày một cách logic, dễ theo dõi.</span></p><p><strong><span style="font-size: 16px">2. Điểm yếu:</span></strong></p><p><span style="font-size: 16px">- Cần cung cấp thêm thông tin chi tiết để làm rõ hơn các so sánh giữa các khu vực.</span></p><p><span style="font-size: 16px">- Có vài lỗi ngữ pháp dù không cản trở hiểu biết nhưng có thể làm giảm độ chính xác của bài viết.</span></p><p><span style="font-size: 16px">- Bài luận có thể cải thiện bằng cách bổ sung phân tích nguyên nhân hoặc hậu quả của các mẫu tiêu thụ được quan sát.</span></p><p><strong><span style="font-size: 16px">3. Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">Bài viết cung cấp cái nhìn tổng quan tương đối rõ ràng về sự tiêu thụ protein và calo tại các khu vực khác nhau, và có sự tiến triển logic. Tuy nhiên, để đạt điểm cao hơn trong đánh giá IELTS, bài luận cần thêm chi tiết và sự chắc chắn trong phân tích. Cấu trúc ngữ pháp cần được chăm chút kỹ lưỡng hơn nữa, dù rằng các lỗi hiện tại không ảnh hưởng đáng kể đến việc truyền đạt thông tin. Nhìn chung, bài viết đã đáp ứng các yêu cầu cơ bản của một bài luận band 6, cho thấy sự hiểu biết và khả năng sử dụng tiếng Anh ở mức độ tốt.</span></p>',
            'lastActivityDate': '2024-04-29T21:10:19.1555614',
            'title': 'Global Protein and Calorie Intake'
          }
        ],
        'rubrics': [
          {
            'id': 3,
            'name': 'Task Achievement',
            'description': 'The Task Achievement criterion in IELTS Academic Writing Task 1 evaluates how well you summarize key trends, compare data, cover all task requirements, and report features accurately from visuals like graphs or charts, focusing on your ability to identify and convey the most important information.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 13,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 14,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- The content is wholly unrelated to the task.\n- Any copied rubric must be discounted.'
              },
              {
                'id': 15,
                'bandScore': 2,
                'description': 'The content barely relates to the task.'
              },
              {
                'id': 16,
                'bandScore': 3,
                'description': '- The response does not address the requirements of the task (possibly because\nof misunderstanding of the data/diagram/situation).\n- Key features/bullet points which are presented may be largely irrelevant.\n- Limited information is presented, and this may be used repetitively.'
              },
              {
                'id': 17,
                'bandScore': 4,
                'description': '- The response is an attempt to address the task.\n- Few key features have been selected.\n- The format may be inappropriate.\n- Key features/bullet points which are presented may be irrelevant, repetitive,\ninaccurate or inappropriate.'
              },
              {
                'id': 18,
                'bandScore': 5,
                'description': '- The response generally addresses the requirements of the task. The\nformat may be inappropriate in places.\n- Key features which are selected are not adequately covered.\nThe recounting of detail is mainly mechanical. There may be no data to\nsupport the description.\n- There may be a tendency to focus on details (without referring to the\nbigger picture).\n- The inclusion of irrelevant, inappropriate or inaccurate material in key\nareas detracts from the task achievement.\n- There is limited detail when extending and illustrating the main points.'
              },
              {
                'id': 19,
                'bandScore': 6,
                'description': 'The response focuses on the requirements of the task and an appropriate\nformat is used.\n- Key features which are selected are covered and adequately\nhighlighted. A relevant overview is attempted. Information is appropriately\nselected and supported using figures/data.\n- Some irrelevant, inappropriate or inaccurate information may occur in\nareas of detail or when illustrating or extending the main points.\n- Some details may be missing (or excessive) and further extension or\nillustration may be needed.'
              },
              {
                'id': 20,
                'bandScore': 7,
                'description': '- The response covers the requirements of the task.\n- The content is relevant and accurate. There may be a few omissions or lapses.\nThe format is appropriate.\n- Key features which are selected are covered and clearly\nhighlighted but could be more fully or more appropriately illustrated or\nextended.\n- It presents a clear overview, the data are appropriately\ncategorised, and main trends or differences are identified.'
              },
              {
                'id': 21,
                'bandScore': 8,
                'description': '- The response covers all the requirements of the task appropriately, relevantly\nand sufficiently.\n- Key features are skilfully selected, and clearly presented,\nhighlighted and illustrated.\n- There may be occasional omissions or lapses in content.'
              },
              {
                'id': 23,
                'bandScore': 9,
                'description': '- All the requirements of the task are fully and appropriately satisfied.\n- There may be extremely rare lapses in content.'
              }
            ]
          },
          {
            'id': 4,
            'name': 'Coherence & Cohesion',
            'description': 'The Coherence & Cohesion criterion assesses your ability to organize and link information clearly and logically in IELTS Academic Writing Task 1. It focuses on effective paragraphing, logical sequencing of ideas, and the use of cohesive devices (like linking words and pronouns) to help the reader understand the relationships between ideas.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 25,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 26,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- The writing fails to communicate any message and appears to be by a virtual non writer.'
              },
              {
                'id': 28,
                'bandScore': 2,
                'description': '- There is little relevant message, or the entire response may be off topic.\n- There is little evidence of control of organisational features.'
              },
              {
                'id': 29,
                'bandScore': 3,
                'description': '- There is no apparent logical organisation .\n- Ideas are discernible but difficult to relate to each other.\n- Minimal use of sequencers or cohesive devices. Those used do not necessarily indicate a logical relationship between ideas.\n- There is difficulty in identifying referencing.'
              },
              {
                'id': 30,
                'bandScore': 4,
                'description': '- Information and ideas are evident but not arranged coherently, and there is no clear progression within the response.\n- Relationships between ideas can be unclear and/or inadequately marked. There is some use of basic cohesive devices, which may be inaccurate or repetitive.\n- There is inaccurate use or a lack of substitution or referencing.'
              },
              {
                'id': 31,
                'bandScore': 5,
                'description': '- Organisation is evident but is not wholly logical and there may be a lack of overall progression. Nevertheless, there is a sense of underlying coherence to the response.\n- The relationship of ideas can be followed but the sentences are not fluently linked to each other.\n- There may be limited/overuse of cohesive devices with some inaccuracy.\n- The writing may be repetitive due to inadequate and/or inaccurate use of reference and substitution.'
              },
              {
                'id': 32,
                'bandScore': 6,
                'description': '- Information and ideas are generally arranged coherently and there is a clear overall progression. \n- Cohesive devices are used to some good effect but cohesion within and/or between sentences may be faulty or mechanical due to misuse, overuse or omission. \n- The use of reference and substitution may lack flexibility or clarity and result in some repetition or error'
              },
              {
                'id': 33,
                'bandScore': 7,
                'description': '- Information and ideas are logically organised and there is a clear progression throughout the response. A few lapses may occur.\n\n- A range of cohesive devices including reference and substitution is used flexibly but with some inaccuracies or some over/under use.'
              },
              {
                'id': 34,
                'bandScore': 8,
                'description': '- The message can be followed with ease. \n- Information and ideas are logically sequenced, and cohesion is well managed. \n- Occasional lapses in coherence or cohesion may occur. \n- Paragraphing is used sufficiently and appropriately.'
              },
              {
                'id': 35,
                'bandScore': 9,
                'description': '- The message can be followed effortlessly.\n- Cohesion is used in such a way that it very rarely attracts attention.\n- Any lapses in coherence or cohesion are minimal.\n- Paragraphing is skilfully managed.'
              }
            ]
          },
          {
            'id': 5,
            'name': 'Lexical Resource',
            'description': 'The Lexical Resource criterion evaluates your range of vocabulary, accuracy in word choice, and ability to use words appropriately to express precise meanings in IELTS Academic Writing Task 1. It focuses on your ability to use a variety of vocabulary to describe data, trends, and processes clearly and accurately.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 36,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English throughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 37,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- No resource is apparent, except for a few isolated words.'
              },
              {
                'id': 38,
                'bandScore': 2,
                'description': '- The resource is extremely limited with few recognisable strings, apart from memorised phrases. \n- There is no apparent control of word formation and/or spelling.'
              },
              {
                'id': 39,
                'bandScore': 3,
                'description': '- The resource is inadequate (which may be due to the response being significantly underlength).\n- Possible over dependence on input material or memorised language. \n- Control of word choice and/or spelling is very limited, and errors predominate. These errors may severely impede meaning.'
              },
              {
                'id': 40,
                'bandScore': 4,
                'description': '- The resource is limited and inadequate for or unrelated to the task. Vocabulary is basic and may be used repetitively.\n- There may be inappropriate use of lexical chunks (e. memorised phrases, formulaic language and/or language from the input material).\n- Inappropriate word choice and/or errors in word formation and/or in spelling may impede meaning.'
              },
              {
                'id': 41,
                'bandScore': 5,
                'description': '- The resource is limited but minimally adequate for the task. \n- Simple vocabulary may be used accurately but the range does not permit much variation in expression. \n- There may be frequent lapses in the appropriacy of word choice, and a lack of flexibility is apparent in frequent simplifications and/or repetitions. \n- Errors in spelling and/or word formation may be noticeable and may cause some difficulty for the reader.'
              },
              {
                'id': 42,
                'bandScore': 6,
                'description': '- The resource is generally adequate and appropriate for the task.\n- The meaning is generally clear in spite of a rather restricted range or a lack of precision in word choice.\n- If the writer is a risk taker, there will be a wider range of vocabulary used but higher degrees of inaccuracy or inappropriacy.\n- There are some errors in spelling and/or word formation, but these do not impede communication.'
              },
              {
                'id': 43,
                'bandScore': 7,
                'description': '- The resource is sufficient to allow some flexibility and precision. \n- There is some ability to use less common and/or idiomatic items. \n- An awareness of style and collocation is evident, though inappropriacies occur. \n- There are only a few errors in spelling and/or word formation, and they do not detract from overall clarity.'
              },
              {
                'id': 44,
                'bandScore': 8,
                'description': '- A wide resource is fluently and flexibly used to convey precise meanings within the scope of the task.\n- There is skilful use of uncommon and/or idiomatic items when appropriate, despite occasional inaccuracies in word choice and collocation.\n- Occasional errors in spelling and/or word formation may occur, but have minimal impact on communication.'
              },
              {
                'id': 45,
                'bandScore': 9,
                'description': '- Full flexibility and precise use are evident within the scope of the task. \n- A wide range of vocabulary is used accurately and appropriately with very natural and sophisticated control of lexical features. \n- Minor errors in spelling and word formation are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 6,
            'name': 'Grammatical Range & Accuracy',
            'description': 'The Grammatical Range & Accuracy criterion assesses your use of sentence structures and grammatical accuracy in IELTS Academic Writing Task 1. It focuses on your ability to construct a range of sentence types correctly and use grammar precisely to convey information and ideas effectively.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 46,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 49,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- No rateable language is evident.'
              },
              {
                'id': 53,
                'bandScore': 2,
                'description': 'There is little or no evidence of sentence forms (except in memorised phrases).'
              },
              {
                'id': 54,
                'bandScore': 3,
                'description': '- Sentence forms are attempted, but errors in grammar and punctuation predominate (except in memorised phrases or those taken from the input material). This prevents most meaning from coming through. \n- Length may be insufficient to provide evidence of control of sentence forms.'
              },
              {
                'id': 55,
                'bandScore': 4,
                'description': '- A very limited range of structures is used. \n- Subordinate clauses are rare and simple sentences predominate. \n- Some structures are produced accurately but grammatical errors are frequent and may impede meaning. \n- Punctuation is often faulty or inadequate.'
              },
              {
                'id': 56,
                'bandScore': 5,
                'description': '- The range of structures is limited and rather repetitive.\n- Although complex sentences are attempted, they tend to be faulty, and the greatest accuracy is achieved on simple sentences.\n- Grammatical errors may be frequent and cause some difficulty for the reader.\n- Punctuation may be faulty.'
              },
              {
                'id': 57,
                'bandScore': 6,
                'description': '- A mix of simple and complex sentence forms is used but flexibility is limited.\n- Examples of more complex structures are not marked by the same level of accuracy as in simple structures.\n- Errors in grammar and punctuation occur, but rarely impede communication'
              },
              {
                'id': 58,
                'bandScore': 7,
                'description': '- A variety of complex structures is used with some flexibility and accuracy.\n- Grammar and punctuation are generally well controlled, and error free sentences are frequent.\n- A few errors in grammar may persist, but these do not impede communication.'
              },
              {
                'id': 59,
                'bandScore': 8,
                'description': '- A wide range of structures within the scope of the task is flexibly and accurately used.\n- The majority of sentences are error free, and punctuation is well managed.\n- Occasional, non systematic errors and inappropriacies occur, but have minimal impact on communication.'
              },
              {
                'id': 60,
                'bandScore': 9,
                'description': '- A wide range of structures within the scope of the task is used with full flexibility and control.\n- Punctuation and grammar are used appropriately throughout.\n- Minor errors are extremely rare and have minimal impact on communication'
              }
            ]
          },
          {
            'id': 34,
            'name': 'Critical Errors',
            'description': 'Critical Errors',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 267,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 35,
            'name': 'Overall Score & Feedback',
            'description': 'Overall Feedback',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 225,
                'bandScore': 0,
                'description': null
              },
              {
                'id': 226,
                'bandScore': 1,
                'description': null
              },
              {
                'id': 227,
                'bandScore': 2,
                'description': null
              },
              {
                'id': 228,
                'bandScore': 3,
                'description': null
              },
              {
                'id': 229,
                'bandScore': 4,
                'description': null
              },
              {
                'id': 230,
                'bandScore': 5,
                'description': null
              },
              {
                'id': 231,
                'bandScore': 6,
                'description': null
              },
              {
                'id': 232,
                'bandScore': 7,
                'description': null
              },
              {
                'id': 233,
                'bandScore': 8,
                'description': null
              },
              {
                'id': 234,
                'bandScore': 9,
                'description': null
              }
            ]
          },
          {
            'id': 42,
            'name': 'Arguments Assessment',
            'description': 'Arguments Assessment',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 283,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 44,
            'name': 'Vocabulary',
            'description': 'Vocabulary',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 285,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 46,
            'name': 'Improved Version',
            'description': 'Improved Version',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 287,
                'bandScore': 0,
                'description': null
              }
            ]
          }
        ]
      },
      {
        'id': 3075,
        'title': 'High Salaries for Senior Management',
        'section': 'Academic Writing Task 2',
        'taskId': 0,
        'test': 'IELTS',
        'testId': 0,
        'time': '40 minutes',
        'type': 'Agree or disagree',
        'sample': true,
        'averageScore': '0.0',
        'submission': 0,
        'like': 0,
        'dislike': 0,
        'status': 'To do',
        'difficulty': 'Medium',
        'direction': 'You should spend about 40 minutes on this task.\n\nProvide reasons for your answer. Include relevant examples from your own knowledge or experience.\n\nWrite at least 250 words.',
        'userId': null,
        'createdBy': null,
        'addedDate': '0001-01-01T00:00:00',
        'lastActivityDate': '0001-01-01T00:00:00',
        'questionsPart': [
          {
            'questionId': 3075,
            'name': 'Question',
            'content': '<p><span style="font-size: 16px">Some people think that it is a good thing for senior management positions to have much higher salaries than other workers in a company.To what extent do you agree or disagree?</span></p>'
          },
          {
            'questionId': 3075,
            'name': 'Tip',
            'content': '<p><strong><u><span style="font-size: 18px">Ý Tưởng Phát Triển Bài</span></u></strong></p><p>1<span style="font-size: 16px">. </span><strong><span style="font-size: 16px">Vai trò của quản lý cấp cao</span></strong><span style="font-size: 16px">: Bắt đầu bằng cách phân tích vai trò và trách nhiệm của quản lý cấp cao, đề cập đến những áp lực và yêu cầu đặt ra cho họ.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Khích lệ và động viên</span></strong><span style="font-size: 16px">: Thảo luận về cách mà một khoản lương cao có thể khích lệ và động viên các nhà quản lý cấp cao, tạo động lực để họ cống hiến và đạt được kết quả tốt cho công ty.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Chất lượng tuyển dụng</span></strong><span style="font-size: 16px">: Bàn luận về việc mức lương cao có thể thu hút những cá nhân có năng lực và kinh nghiệm xuất sắc gia nhập công ty.</span></p><p><span style="font-size: 16px">4. </span><strong><span style="font-size: 16px">Sự chênh lệch giàu nghèo</span></strong><span style="font-size: 16px">: Đề cập đến mối quan ngại về sự chênh lệch giàu nghèo trong công ty và ảnh hưởng của nó đến tình đoàn kết nội bộ.</span></p><p><span style="font-size: 16px">5. </span><strong><span style="font-size: 16px">Công bằng và bình đẳng</span></strong><span style="font-size: 16px">: Thảo luận về nguyên tắc công bằng và bình đẳng, liệu việc chênh lệch lương lớn có phản ánh đúng giá trị công việc hay không.</span></p><p><span style="font-size: 16px">6. </span><strong><span style="font-size: 16px">Hiệu quả công việc</span></strong><span style="font-size: 16px">: Phân tích liệu mức lương cao có thật sự dẫn đến hiệu quả công việc tốt hơn từ phía quản lý cấp cao hay không.</span></p><p><span style="font-size: 16px">7. </span><strong><span style="font-size: 16px">Động cơ làm việc</span></strong><span style="font-size: 16px">: Xem xét ảnh hưởng của mức lương đến động cơ làm việc của nhân viên cấp thấp hơn, liệu họ có cảm thấy được đánh giá đúng mức không.</span></p><p><span style="font-size: 16px">8. </span><strong><span style="font-size: 16px">Văn hóa công ty</span></strong><span style="font-size: 16px">: Bình luận về cách mà sự chênh lệch lương lớn có thể tác động đến văn hóa và môi trường làm việc của công ty.</span></p><p><span style="font-size: 16px">9. </span><strong><span style="font-size: 16px">Phản ứng của dư luận</span></strong><span style="font-size: 16px">: Đề cập đến cách công chúng và khách hàng nhìn nhận về mức lương của các nhà quản lý cấp cao, và liệu điều này có ảnh hưởng đến hình ảnh của công ty không.</span></p><p><span style="font-size: 16px">10. </span><strong><span style="font-size: 16px">Sự cân nhắc về mặt xã hội</span></strong><span style="font-size: 16px">: Thảo luận về trách nhiệm xã hội của công ty, liệu họ có nên cân nhắc đến mức độ chênh lệch trong thu nhập giữa nhà quản lý cấp cao và nhân viên thông thường hay không.</span></p><p><strong><u><span style="font-size: 18px">Gợi Ý Cho Cấu Trúc Bài</span></u></strong></p><p><strong><span style="font-size: 16px">Mở bài:</span></strong></p><p><span style="font-size: 16px">- Giới thiệu đề tài: Mô tả ngắn gọn vấn đề được nêu trong đề bài, là việc có ý kiến cho rằng mức lương cao của quản lý cấp cao là điều tốt.</span></p><p><span style="font-size: 16px">- Đưa ra thesis statement: Trình bày quan điểm của bạn về vấn đề này một cách rõ ràng, khẳng định bạn đồng ý hoặc không đồng ý hoặc một phần đồng ý với quan điểm đó.</span></p><p><strong><span style="font-size: 16px">Thân bài:</span></strong></p><p><span style="font-size: 16px">- Đoạn văn 1: Đề cập đến những lợi ích khi quản lý cấp cao có mức lương cao hơn hẳn so với nhân viên khác, chẳng hạn như việc thu hút tài năng, khích lệ, và tạo động lực.</span></p><p><span style="font-size: 16px">- Đoạn văn 2: Thảo luận về các vấn đề tiềm ẩn, như sự chênh lệch thu nhập có thể gây ra mất cân bằng, ghen tức và mất đoàn kết trong công ty.</span></p><p><span style="font-size: 16px">- Đoạn văn 3: Bàn luận về các giải pháp hoặc cách tiếp cận khác, như cơ chế thưởng dựa trên hiệu suất hoặc lợi nhuận công ty, để giảm thiểu sự chênh lệch mà không làm mất đi động lực của quản lý cấp cao.</span></p><p><strong><span style="font-size: 16px">Kết luận:</span></strong></p><p><span style="font-size: 16px">- Tóm tắt lại các luận điểm chính: Nêu lại các luận điểm đã đưa ra trong thân bài và mối liên hệ chúng với quan điểm cá nhân của bạn.</span></p><p><span style="font-size: 16px">- Đưa ra quan điểm cá nhân cuối cùng: Khẳng định lại quan điểm của bạn và giải thích tại sao bạn nghĩ như vậy, có thể kết hợp với một lời kêu gọi hành động hoặc đề xuất cải thiện.</span></p><p><span style="font-size: 16px">- Mở rộng vấn đề: Đề xuất vấn đề này trong bối cảnh lớn hơn, như vấn đề công bằng xã hội hoặc đạo đức kinh doanh, nếu cần.</span></p><p><span style="font-size: 16px">Nhớ rằng mỗi phần của bài viết nên chứa các câu chuyển tiếp mượt mà để giúp người đọc theo dõi dễ dàng hơn. Sử dụng dẫn chứng cụ thể và ví dụ để làm sáng tỏ các quan điểm của bạn.</span></p><p></p>'
          },
          {
            'questionId': 3075,
            'name': 'Vocabulary',
            'content': "<p><span style=\"font-size: 16px\">1. </span><strong><span style=\"font-size: 16px\">Meritocratic</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌmer.ɪ.təˈkræt.ɪk/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: theo công bằng năng lực</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Relating to a system in which people achieve success and progress based on their abilities and merit rather than on their social class or wealth.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: A </span><strong><span style=\"font-size: 16px\">meritocratic</span></strong><span style=\"font-size: 16px\"> system ensures that individuals are rewarded based on their performance rather than their background.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: merit-based</span></p><p><span style=\"font-size: 16px\">2. </span><strong><span style=\"font-size: 16px\">Disparity</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /dɪˈspær.ə.ti/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự chênh lệch</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: A great difference.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The salary </span><strong><span style=\"font-size: 16px\">disparity</span></strong><span style=\"font-size: 16px\"> between senior management and regular employees can cause dissatisfaction.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: inequality, gap</span></p><p><span style=\"font-size: 16px\">3. </span><strong><span style=\"font-size: 16px\">Compensation</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌkɒm.pənˈseɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: tiền bồi thường, tiền lương</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Payment or whatever is given or done to make up for something.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Senior managers often receive higher </span><strong><span style=\"font-size: 16px\">compensation</span></strong><span style=\"font-size: 16px\"> due to their critical decision-making roles.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: payment, remuneration</span></p><p><span style=\"font-size: 16px\">4. </span><strong><span style=\"font-size: 16px\">Incentivize</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ɪnˈsen.tɪ.vaɪz/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: khuyến khích</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: To encourage someone by offering incentives.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Higher salaries </span><strong><span style=\"font-size: 16px\">incentivize</span></strong><span style=\"font-size: 16px\"> senior managers to align their goals with the company’s objectives.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: encourage, motivate</span></p><p><span style=\"font-size: 16px\">5. </span><strong><span style=\"font-size: 16px\">Hierarchy</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈhaɪ.rɑːr.ki/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: hệ thống cấp bậc</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: A system in which members of an organization or society are ranked according to relative status or authority.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The corporate </span><strong><span style=\"font-size: 16px\">hierarchy</span></strong><span style=\"font-size: 16px\"> often influences the salary structures within the company.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: pecking order, ranking</span></p><p><span style=\"font-size: 16px\">6. </span><strong><span style=\"font-size: 16px\">Benchmarking</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈbenʧ.mɑːr.kɪŋ/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: đánh giá chuẩn mực</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Comparing one's business processes and performance metrics to industry bests or best practices from other companies.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Benchmarking</span></strong><span style=\"font-size: 16px\"> salaries helps ensure that pay remains competitive and fair across all levels.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: standardizing, evaluating</span></p><p><span style=\"font-size: 16px\">7. </span><strong><span style=\"font-size: 16px\">Equitable</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈek.wɪ.tə.bl/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: công bằng, bình đẳng</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Fair and impartial.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: It is crucial to establish an </span><strong><span style=\"font-size: 16px\">equitable</span></strong><span style=\"font-size: 16px\"> pay structure to maintain employee morale.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: fair, just</span></p><p><span style=\"font-size: 16px\">8. </span><strong><span style=\"font-size: 16px\">Remuneration</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /r</span></p><p><span style=\"font-size: 16px\">ɪˌmjuː.nəˈreɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: tiền thù lao</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Money paid for work or a service.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The </span><strong><span style=\"font-size: 16px\">remuneration</span></strong><span style=\"font-size: 16px\"> package for senior managers includes bonuses and stock options.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: payment, salary</span></p><p><span style=\"font-size: 16px\">9. </span><strong><span style=\"font-size: 16px\">Job Satisfaction</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /dʒɒb sæt.ɪsˈfæk.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự hài lòng trong công việc</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: A pleasurable or positive emotional state resulting from the appraisal of one's job or job experiences.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: High </span><strong><span style=\"font-size: 16px\">job satisfaction</span></strong><span style=\"font-size: 16px\"> often correlates with better performance and dedication.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: contentment at work</span></p><p><span style=\"font-size: 16px\">10. </span><strong><span style=\"font-size: 16px\">Motivational</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌməʊ.tɪˈveɪ.ʃən.əl/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: có tính chất khích lệ</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Relating to motivation.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Motivational</span></strong><span style=\"font-size: 16px\"> incentives such as potential promotions or raises are important for employee engagement.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: inspiring, stimulating</span></p><p><span style=\"font-size: 16px\">11. </span><strong><span style=\"font-size: 16px\">Allocate</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈæl.ə.keɪt/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: phân bổ</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: To distribute resources or duties for a particular purpose.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Resources must be </span><strong><span style=\"font-size: 16px\">allocated</span></strong><span style=\"font-size: 16px\"> efficiently to ensure maximum productivity.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: distribute, assign</span></p><p><span style=\"font-size: 16px\">12. </span><strong><span style=\"font-size: 16px\">Substantiate</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /səbˈstæn.ʃi.eɪt/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: chứng minh, làm rõ</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Provide evidence to support or prove the truth of.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Claims of improved efficiency must be </span><strong><span style=\"font-size: 16px\">substantiated</span></strong><span style=\"font-size: 16px\"> with clear metrics and data.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: verify, confirm</span></p><p><span style=\"font-size: 16px\">13. </span><strong><span style=\"font-size: 16px\">Stratification</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌstræt.ɪ.fɪˈkeɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự phân tầng</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: The arrangement or classification of something into different groups.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Salary </span><strong><span style=\"font-size: 16px\">stratification</span></strong><span style=\"font-size: 16px\"> in large organizations often reflects the complexity of roles.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: layering, segregation</span></p><p><span style=\"font-size: 16px\">14. </span><strong><span style=\"font-size: 16px\">Performance-based</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /pəˈfɔː.məns beɪst/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: dựa trên hiệu suất</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Relating to how well someone does their job or tasks.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Performance-based</span></strong><span style=\"font-size: 16px\"> incentives are essential for motivating staff and improving company results.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: merit-based, efficacy-related</span></p><p><span style=\"font-size: 16px\">15. </span><strong><span style=\"font-size: 16px\">Discretionary</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /dɪˈskreʃ.ən.ər.i/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: tuỳ ý, theo ý muốn</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Available for use at the discretion of the user.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Discretionary</span></strong><span style=\"font-size: 16px\"> bonuses allow managers to reward exceptional employee performance.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: optional, elective</span></p><p><span style=\"font-size: 16px\">16. </span><strong><span style=\"font-size: 16px\">Autonomy</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ɔːˈtɒn.ə.mi/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: qu</span></p><p><span style=\"font-size: 16px\">yền tự chủ</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: The right or condition of self-government.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Higher job positions often come with increased </span><strong><span style=\"font-size: 16px\">autonomy</span></strong><span style=\"font-size: 16px\"> and responsibility.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: independence, self-determination</span></p><p><span style=\"font-size: 16px\">17. </span><strong><span style=\"font-size: 16px\">Retain</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /rɪˈteɪn/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: giữ lại</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Continue to have (something); keep possession of.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Competitive salaries are crucial to </span><strong><span style=\"font-size: 16px\">retain</span></strong><span style=\"font-size: 16px\"> the best talent in the industry.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: keep, maintain</span></p><p><span style=\"font-size: 16px\">18. </span><strong><span style=\"font-size: 16px\">Cognitive</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈkɒɡ.nɪ.tɪv/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: liên quan đến nhận thức</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Relating to cognition, especially the mental action or process of acquiring knowledge and understanding through thought, experience, and the senses.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Cognitive</span></strong><span style=\"font-size: 16px\"> skills such as problem-solving and decision-making are essential for senior managers.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: mental, intellectual</span></p><p><span style=\"font-size: 16px\">19. </span><strong><span style=\"font-size: 16px\">Inequitable</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ɪnˈek.wɪ.tə.bl/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: không công bằng</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: Not fair, or not arranged according to the principles of equality and justice.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: An </span><strong><span style=\"font-size: 16px\">inequitable</span></strong><span style=\"font-size: 16px\"> distribution of salaries can lead to employee dissatisfaction and high turnover.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: unfair, unjust</span></p><p><span style=\"font-size: 16px\">20. </span><strong><span style=\"font-size: 16px\">Dissatisfaction</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌdɪs.sæt.ɪsˈfæk.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự không hài lòng</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: The feeling of not being satisfied or pleased because something is not as good as expected.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Salary </span><strong><span style=\"font-size: 16px\">dissatisfaction</span></strong><span style=\"font-size: 16px\"> is often the main reason for high employee turnover rates.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: discontent, unhappiness</span></p><p></p>"
          }
        ],
        'samples': [
          {
            'id': 356,
            'questionId': 3075,
            'sampleText': '<p></p><p><span style="font-size: 16px">It is commonly argued that high salaries for senior management compared to other employees within a company are justified. While this viewpoint holds merit in terms of rewarding experience and responsibility, the disparity in wages should also be critically examined. I partly agree with the proposition, recognizing both the necessity and potential drawbacks of such pay scales.</span></p><p><span style="font-size: 16px">Firstly, it is undeniable that senior managers play pivotal roles in the success of a company. They make strategic decisions, often under pressure, that can determine the future trajectory of the enterprise. For instance, a CEO of a tech giant, such as Apple or Google, influences not only their company but also economic and technological trends globally. Consequently, their compensation should reflect their substantial impact and the risks involved in their roles. This not only rewards their capabilities but also serves to attract top talent who can offer innovative ideas and leadership.</span></p><p><span style="font-size: 16px">However, excessive salary disparities can lead to significant issues within the organizational structure. For example, vast differences in pay can demoralize lower-tier employees, who may feel undervalued despite their contributions to the company’s operations. This sentiment can foster a negative work environment and decrease overall productivity. Moreover, it might lead to high turnover rates, which are costly and disruptive.</span></p><p><span style="font-size: 16px">Therefore, while it is reasonable for senior managers to earn more due to their greater responsibilities and influence, companies should strive to maintain a balanced approach to compensation. Implementing performance-based bonuses and profit-sharing schemes could mitigate feelings of inequality and incentivize all employees to contribute to their fullest potential.</span></p><p><span style="font-size: 16px">In conclusion, while I acknowledge the necessity of higher remunerations for senior management, maintaining fairness and motivation across all levels of a company is equally vital. Striking a balance between rewarding skills and responsibilities at the top and ensuring fair and motivating pay scales for other workers is essential for long-term corporate health and success.</span></p><p></p>',
            'bandScore': 8,
            'comment': '<p></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Mức độ phát triển ý tưởng</span></strong><span style="font-size: 16px">: Bài viết đã trình bày một quan điểm cân bằng và chi tiết, phân tích cả ưu và nhược điểm của việc trả lương cao cho những vị trí quản lý cấp cao.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Tổ chức và kết cấu</span></strong><span style="font-size: 16px">: Bài viết có cấu trúc rõ ràng, mỗi đoạn văn đều có một ý chính được phát triển một cách logic và thuyết phục.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Sử dụng ngôn từ</span></strong><span style="font-size: 16px">: Ngôn từ được sử dụng đa dạng và chính xác, phù hợp với yêu cầu của một bài viết band điểm cao. Các từ vựng chuyên ngành và cụm từ được sử dụng một cách hiệu quả, góp phần nâng cao chất lượng bài viết.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Sử dụng các thiết bị liên kết</span></strong><span style="font-size: 16px">: Bài viết sử dụng thành thạo các liên kết ngữ pháp và từ nối để đảm bảo tính liền mạch và dễ hiểu.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Điểm yếu:</span></strong></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Chi tiết hỗ trợ</span></strong><span style="font-size: 16px">: Mặc dù bài viết đã phát triển tốt các ý chính, nhưng có thể còn thiếu một số chi tiết hỗ trợ cụ thể hơn hoặc ví dụ thực tế để làm rõ hơn các điểm đã nêu, đặc biệt là trong phần phân tích các tác động tiêu cực của sự chênh lệch lương bổng.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Phát triển các ý phụ</span></strong><span style="font-size: 16px">: Có thể còn dư địa để mở rộng thêm về các giải pháp cụ thể hoặc thảo luận sâu hơn về cách thức cân bằng lương bổng trong công ty để tăng tính thuyết phục.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">   - Bài viết là một mẫu mực cho bài viết IELTS band điểm 8.0, với cách trình bày quan điểm rõ ràng, dẫn chứng thuyết phục và sử dụng ngôn từ hiệu quả. Bài viết đã thành công trong việc truyền đạt một quan điểm phức tạp một cách cân bằng và sâu sắc.</span></p><p><span style="font-size: 16px">   - Để cải thiện hơn nữa, tác giả có thể xem xét việc bổ sung thêm các dẫn chứng cụ thể và phát triển rộng hơn các giải pháp cho vấn đề đã nêu, qua đó tăng cường tính thuyết phục và độ sâu của bài viết.</span></p><p></p>',
            'lastActivityDate': '2024-04-29T22:04:23.79963',
            'title': 'High Salaries for Senior Management'
          },
          {
            'id': 357,
            'questionId': 3075,
            'sampleText': '<p><span style="font-size: 16px">It is often debated whether it is justifiable for senior management to earn significantly higher salaries compared to other company employees. I agree with this perspective to a large extent, as the responsibilities and pressures at the top are incomparably greater, though some caution in salary disparity is advised to maintain company harmony.</span></p><p><span style="font-size: 16px">Senior managers are tasked with steering the company through both prosperous and challenging times. Their decisions impact not only the immediate health of the business but its long-term viability and strategic direction. For example, a CEO must navigate through economic downturns, shifting market demands, and internal upheavals, all of which require a high level of expertise, experience, and personal investment. Therefore, their substantial salaries can be seen as a reflection of the heavy burdens they bear.</span></p><p><span style="font-size: 16px">However, excessively large gaps in pay between senior management and the rest of the staff can create a sense of inequality and demotivation among the workforce. Employees may feel undervalued or become less engaged, perceiving the rewards of their labor as unfairly distributed. For instance, when bonus cuts are made at lower levels while executive pay remains untouched, it can lead to resentment and reduced productivity.</span></p><p><span style="font-size: 16px">To mitigate such issues, companies should maintain a reasonable ratio between the highest and the average salaries within the company. This approach helps in keeping morale high and fostering a culture of fairness and respect, which are crucial for employee retention and satisfaction.</span></p><p><span style="font-size: 16px">In conclusion, while I support the notion that senior management deserves higher compensation due to their increased responsibilities, it is vital to keep salary disparities within reasonable limits. This balance is essential not only for internal harmony but also for the overall health and success of the company.</span></p><p></p>',
            'bandScore': 7,
            'comment': '<p></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Rõ ràng về lập trường</span></strong><span style="font-size: 16px">: Bài viết đã trình bày một quan điểm rõ ràng và mạnh mẽ ủng hộ việc trả lương cao cho những vị trí quản lý cấp cao nhưng cũng nhấn mạnh tới sự cần thiết của việc duy trì sự cân bằng.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Tổ chức bài viết</span></strong><span style="font-size: 16px">: Bài viết được tổ chức tốt với mở bài, thân bài và kết luận rõ ràng. Mỗi đoạn văn đều có một chức năng cụ thể, giúp người đọc dễ dàng theo dõi.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Sử dụng từ vựng</span></strong><span style="font-size: 16px">: Bài viết sử dụng một loạt từ vựng phù hợp và đa dạng, phản ánh khả năng sử dụng ngôn từ linh hoạt của tác giả.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Điểm yếu:</span></strong></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Tổng quát hóa</span></strong><span style="font-size: 16px">: Mặc dù bài viết đã nêu bật được các lập luận chính, nhưng đôi khi còn rơi vào lỗi tổng quát hóa, không cung cấp đủ chi tiết cụ thể để làm sáng tỏ các quan điểm hoặc ví dụ đưa ra.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Lỗi ngữ pháp nhỏ</span></strong><span style="font-size: 16px">: Bài viết có một số lỗi ngữ pháp nhỏ và sử dụng từ không chính xác ngẫu nhiên, điều này có thể làm giảm đi sự chính xác của bài viết.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">   - Bài viết là một mẫu mực cho bài viết ở mức điểm 7.0 trong kỳ thi IELTS, với cách trình bày quan điểm rõ ràng và có tổ chức. Bài viết đã thành công trong việc phát triển các ý tưởng một cách logic và dẫn dắt người đọc qua các luận điểm một cách có hiệu quả.</span></p><p><span style="font-size: 16px">   - Để cải thiện bài viết, tác giả có thể cân nhắc việc thêm vào các chi tiết cụ thể hơn và tránh sử dụng những lập luận chung chung không có sự hỗ trợ của dữ liệu hoặc ví dụ thực tế. Ngoài ra, việc kiểm tra kỹ lưỡng ngữ pháp và từ vựng sẽ giúp tăng tính chính xác và chuyên nghiệp của bài viết.</span></p><p></p>',
            'lastActivityDate': '2024-04-29T22:07:27.4036439',
            'title': 'High Salaries for Senior Management'
          },
          {
            'id': 358,
            'questionId': 3075,
            'sampleText': '<p><span style="font-size: 16px">Many believe that it is justified for senior management to receive significantly higher salaries than other employees in a company. I agree with this to some extent because of the responsibilities they handle, though this system is not without its flaws.</span></p><p><span style="font-size: 16px">Senior managers, such as CEOs or department heads, often have a lot more responsibilities compared to the regular staff. They make decisions that can affect the entire company and its future. For example, a manager might decide on a new market strategy that could either lead to substantial profits or considerable losses. Therefore, their higher salaries can be seen as compensation for their higher stakes and the stress they handle.</span></p><p><span style="font-size: 16px">However, this pay gap can also create problems within the company. When the salary difference is too large, it might lead to dissatisfaction among other employees. They might feel undervalued or less important, which can affect their performance and the company’s overall productivity. For instance, if a regular employee sees their manager earning much more for what they perceive as comparable effort, it might demotivate them from working hard.</span></p><p><span style="font-size: 16px">Additionally, while managers do have more responsibilities, the success of a company also heavily depends on the other employees. It might seem unfair if only the top executives are rewarded well. This could be improved by maybe giving bonuses based on the performance of all employees, not just the management, so everyone feels their contribution is valued.</span></p><p><span style="font-size: 16px">In conclusion, while I understand why senior management receives higher salaries, I think it is important to keep the salary gap reasonable. Ensuring that all employees are adequately compensated and feel valued is crucial for a healthy work environment and company success.</span></p><p></p>',
            'bandScore': 6,
            'comment': '<p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Rõ ràng về lập trường</span></strong><span style="font-size: 16px">: Bài viết đã nêu rõ quan điểm ủng hộ việc trả lương cao cho quản lý cấp cao nhưng cũng chỉ ra những vấn đề tiềm ẩn của hệ thống này.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Tổ chức</span></strong><span style="font-size: 16px">: Cấu trúc của bài viết tương đối rõ ràng, với sự phân chia thành mở bài, thân bài và kết luận, giúp người đọc theo dõi dễ dàng.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Truyền đạt ý tưởng</span></strong><span style="font-size: 16px">: Bài viết có khả năng truyền đạt ý tưởng tốt, các ý chính được nêu bật và giải thích một cách đơn giản.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Điểm yếu:</span></strong></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Chi tiết và phát triển ý</span></strong><span style="font-size: 16px">: Các ý tưởng trong bài viết chưa được phát triển một cách đầy đủ hoặc chi tiết. Điều này làm cho các luận điểm thiếu sức thuyết phục.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Lặp lại và đơn giản</span></strong><span style="font-size: 16px">: Cách sử dụng từ nối và cấu trúc câu có phần lặp lại và đơn giản, làm giảm đi sự phong phú và hấp dẫn của bài viết.</span></p><p><span style="font-size: 16px">   - </span><strong><span style="font-size: 16px">Sai sót ngữ pháp</span></strong><span style="font-size: 16px">: Có một số lỗi ngữ pháp trong bài viết, dù những lỗi này không ảnh hưởng nghiêm trọng đến ý nghĩa tổng thể nhưng vẫn làm giảm chất lượng của bài.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">   - Bài viết đạt yêu cầu của một bài viết band điểm 6.0 trong kỳ thi IELTS. Bài viết có sự rõ ràng trong tổ chức và truyền đạt ý tưởng nhưng cần được cải thiện về mặt chi tiết và sâu sắc của các luận điểm.</span></p><p><span style="font-size: 16px">   - Để nâng cao điểm số, tác giả cần phát triển thêm các ý tưởng, cung cấp thêm chi tiết và ví dụ cụ thể hơn, đồng thời cải thiện kỹ năng ngữ pháp và sử dụng từ ngữ để làm cho bài viết phong phú và thuyết phục hơn.</span></p><p></p>',
            'lastActivityDate': '2024-04-29T22:09:44.2907807',
            'title': 'High Salaries for Senior Management'
          }
        ],
        'rubrics': [
          {
            'id': 7,
            'name': 'Task Response',
            'description': 'The Task Response criterion in IELTS Academic Writing Task 2 assesses how well you develop and support your ideas in response to the question. It focuses on your ability to present a clear, relevant position, fully extend and support your main ideas, and answer all parts of the task prompt comprehensively.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 61,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 62,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1\n- The content is wholly unrelated to the prompt\n- Any copied rubric must be discounted'
              },
              {
                'id': 63,
                'bandScore': 2,
                'description': '- The content is barely related to the prompt.\n- No position can be identified.\n- There may be glimpses of one or two ideas without development.'
              },
              {
                'id': 64,
                'bandScore': 3,
                'description': '- No part of the prompt is adequately addressed, or the prompt has been misunderstood.\n- No relevant position can be identified, and/or there is little direct response to the question/s.\n- There are few ideas, and these may be irrelevant or insufficiently developed.'
              },
              {
                'id': 65,
                'bandScore': 4,
                'description': '- The prompt is tackled in a minimal way, or the answer is tangential, possibly due to some misunderstanding of the prompt.\n- The format may be inappropriate.\n- A position is discernible, but the reader has to read carefully to find it.\n- Main ideas are difficult to identify and such ideas that are identifiable may lack relevance, clarity and/or support.\n- Large parts of the response may be repetitive.'
              },
              {
                'id': 66,
                'bandScore': 5,
                'description': '- The main parts of the prompt are incompletely addressed. The format may be inappropriate in places.\n- The writer expresses a position, but the development is not always clear.\n- Some main ideas are put forward, but they are limited and are not sufficiently developed and/or there may be irrelevant detail.\n- There may be some repetition.'
              },
              {
                'id': 67,
                'bandScore': 6,
                'description': '- The main parts of the prompt are addressed (though some may be more fully covered than others). An appropriate format is used. \n- A position is presented that is directly relevant to the prompt, although the conclusions drawn may be unclear, unjustified or repetitive.\n- Main ideas are relevant, but some may be insufficiently developed or may lack clarity, while some supporting arguments and evidence may be less relevant or inadequate.'
              },
              {
                'id': 68,
                'bandScore': 7,
                'description': '- The main parts of the prompt are appropriately addressed.\n- A clear and developed position is presented.\n- Main ideas are extended and supported but there may be a tendency to over generalise or there may be a lack of focus and precision in supporting ideas/material.'
              },
              {
                'id': 69,
                'bandScore': 8,
                'description': '- The prompt is appropriately and sufficiently addressed.\n- A clear and well developed position is presented in response to the question/s.\n- Ideas are relevant, well extended and supported.\n- There may be occasional omissions or lapses in content.'
              },
              {
                'id': 70,
                'bandScore': 9,
                'description': '- The prompt is appropriately addressed and explored in depth.\n- A clear and fully developed position is presented which directly answers the question/s.\n- Ideas are relevant, fully extended and well supported.\n- Any lapses in content or support are extremely rare.'
              }
            ]
          },
          {
            'id': 8,
            'name': 'Coherence & Cohesion',
            'description': 'The Coherence & Cohesion criterion in IELTS Academic Writing Task 2 evaluates your ability to organize ideas logically and connect them smoothly. It focuses on clear overall structure, logical sequencing of paragraphs and ideas, and the effective use of cohesive devices (such as linking words and pronouns) to guide the reader through your argument or narrative seamlessly.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 71,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 72,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- The writing fails to communicate any message and appears to be by a virtual non writer.'
              },
              {
                'id': 73,
                'bandScore': 2,
                'description': '- There is little relevant message, or the entire response may be off topic.\n- There is little evidence of control of organisational features.'
              },
              {
                'id': 74,
                'bandScore': 3,
                'description': '- There is no apparent logical organisation . Ideas are discernible but difficult to relate to each other.\n- There is minimal use of sequencers or cohesive devices. Those used do not necessarily indicate a logical relationship between ideas.\n- There is difficulty in identifying referencing.\n- Any attempts at paragraphing are unhelpful.'
              },
              {
                'id': 75,
                'bandScore': 4,
                'description': '- Information and ideas are evident but not arranged coherently and there is no clear progression within the response.\n- Relationships between ideas can be unclear and/or inadequately marked. There is some use of basic cohesive devices, which may be inaccurate or repetitive.\n- There is inaccurate use or a lack of substitution or\nreferencing.\n- There may be no paragraphing and/or no clear main topic within paragraphs.'
              },
              {
                'id': 76,
                'bandScore': 5,
                'description': '- Organisation is evident but is not wholly logical and there may be a lack of overall progression.Nevertheless, there is a sense of underlying coherence to the response.\n- The relationship of ideas can be followed but the sentences are not fluently linked to each other.\n- There may be limited/overuse of cohesive devices with some inaccuracy.\n- The writing may be repetitive due to inadequate and/or inaccurate use of reference and substitution.\n- Paragraphing may be inadequate or missing.'
              },
              {
                'id': 77,
                'bandScore': 6,
                'description': '- Information and ideas are generally arranged coherently and there is a clear overall progression.\n- Cohesive devices are used to some good effect but cohesion within and/or between sentences may be faulty or mechanical due to misuse, overuse or omission.\n- The use of reference and substitution may lack flexibility or clarity and result in some repetition or error.\n- Paragraphing may not always be logical and/or the central topic may not always be clear.'
              },
              {
                'id': 78,
                'bandScore': 7,
                'description': '- Information and ideas are logically organised, and there is a clear progression throughout the response. (A few lapses may occur, but these are minor.)\n- A range of cohesive devices including reference and substitution is used flexibly but with some inaccuracies or some over/under use.\n- Paragraphing is generally used effectively to support overall coherence, and the sequencing of ideas within a paragraph is generally logical.'
              },
              {
                'id': 79,
                'bandScore': 8,
                'description': '- The message can be followed with ease.\n- Information and ideas are logically sequenced, and cohesion is well managed.\n- Occasional lapses in coherence and cohesion may occur.\n- Paragraphing is used sufficiently and appropriately.'
              },
              {
                'id': 80,
                'bandScore': 9,
                'description': '- The message can be followed effortlessly.\n- Cohesion is used in such a way that it very rarely attracts attention.\n- Any lapses in coherence or cohesion are minimal.\n- Paragraphing is skilfully managed.'
              }
            ]
          },
          {
            'id': 9,
            'name': 'Lexical Resource',
            'description': 'The Lexical Resource criterion in IELTS Academic Writing Task 2 measures your range of vocabulary, the precision of your word choices, and your ability to use words appropriately for various contexts. It assesses your skill in using vocabulary flexibly and accurately to express ideas and opinions, including the effective use of synonyms and collocations, without errors that hinder communication.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 81,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 82,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- No resource is apparent, except for a few isolated words.'
              },
              {
                'id': 83,
                'bandScore': 2,
                'description': '- The resource is extremely limited with few recognisable strings, apart from memorised phrases.\n- There is no apparent control of word formation and/or spelling.'
              },
              {
                'id': 84,
                'bandScore': 3,
                'description': '- The resource is inadequate (which may be due to the response being significantly underlength). Possible over-dependence on input material or memorised language.\n- Control of word choice and/or spelling is very limited, and errors predominate. These errors may severely impede meaning.'
              },
              {
                'id': 85,
                'bandScore': 4,
                'description': '- The resource is limited and inadequate for or unrelated to the task. Vocabulary is basic and may be used repetitively.\n- There may be inappropriate use of lexical chunks (e.g. memorised phrases, formulaic language and/or language from the input material). \n- Inappropriate word choice and/or errors in word formation and/or in spelling may impede meaning'
              },
              {
                'id': 86,
                'bandScore': 5,
                'description': '- The resource is limited but minimally adequate for the task.\n- Simple vocabulary may be used accurately but the range does not permit much variation in expression. \n- There may be frequent lapses in the appropriacy of word choice and a lack of flexibility is apparent in frequent simplifications and/or repetitions. \n- Errors in spelling and/or word formation may be noticeable and may cause some difficulty for the reader.'
              },
              {
                'id': 87,
                'bandScore': 6,
                'description': '- The resource is generally adequate and appropriate for the task.\n- The meaning is generally clear in spite of a rather restricted range or a lack of precision in word choice.\n- If the writer is a risk-taker, there will be a wider range of vocabulary used but higher degrees of inaccuracy or inappropriacy.\n- There are some errors in spelling and/or word formation, but these do not impede communication.'
              },
              {
                'id': 88,
                'bandScore': 7,
                'description': '- The resource is sufficient to allow some flexibility and precision.\n- There is some ability to use less common and/or idiomatic items.\n- An awareness of style and collocation is evident, though inappropriacies occur.\n- There are only a few errors in spelling and/or word formation and they do not detract from overall clarity.'
              },
              {
                'id': 89,
                'bandScore': 8,
                'description': '- A wide resource is fluently and flexibly used to convey precise meanings. \n- There is skilful use of uncommon and/or idiomatic items when appropriate, despite occasional inaccuracies in word choice and collocation.\n- Occasional errors in spelling and/or word formation may occur, but have minimal impact on communication.'
              },
              {
                'id': 90,
                'bandScore': 9,
                'description': '- Full flexibility and precise use are widely evident.\n- A wide range of vocabulary is used accurately and appropriately with very natural and sophisticated control of lexical features.\n- Minor errors in spelling and word formation are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 10,
            'name': 'Grammatical Range & Accuracy',
            'description': 'The Grammatical Range & Accuracy criterion in IELTS Academic Writing Task 2 evaluates your ability to use a wide range of grammar structures accurately. It focuses on your capacity to construct complex sentences effectively, use punctuation correctly, and apply grammatical forms with flexibility to clearly express detailed reasoning and arguments without making errors that obscure meaning.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 91,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 92,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- No rateable language is evident.'
              },
              {
                'id': 93,
                'bandScore': 2,
                'description': 'There is little or no evidence of sentence forms (except in memorised phrases).'
              },
              {
                'id': 94,
                'bandScore': 3,
                'description': '- Sentence forms are attempted, but errors in grammar and punctuation predominate (except in memorised phrases or those taken from the input material).\n- This prevents most meaning from coming through. Length may be insufficient to provide evidence of control of sentence forms.'
              },
              {
                'id': 95,
                'bandScore': 4,
                'description': 'A very limited range of structures is used.\n- Subordinate clauses are rare and simple sentences predominate.\n- Some structures are produced accurately but grammatical errors are frequent and may impede meaning.\n- Punctuation is often faulty or inadequate.'
              },
              {
                'id': 96,
                'bandScore': 5,
                'description': '- The range of structures is limited and rather repetitive.\n- Although complex sentences are attempted, they tend to be faulty, and the greatest accuracy is achieved on simple sentences.\n- Grammatical errors may be frequent and cause some difficulty for the reader.\n- Punctuation may be faulty.'
              },
              {
                'id': 97,
                'bandScore': 6,
                'description': '- A mix of simple and complex sentence forms is used but flexibility is limited.\n- Examples of more complex structures are not marked by the same level of accuracy as in simple structures.\n- Errors in grammar and punctuation occur, but rarely impede communication.'
              },
              {
                'id': 98,
                'bandScore': 7,
                'description': '- A variety of complex structures is used with some flexibility and accuracy.\n- Grammar and punctuation are generally well controlled, and error-free sentences are frequent.\n- A few errors in grammar may persist, but these do not impede communication.'
              },
              {
                'id': 99,
                'bandScore': 8,
                'description': '- A wide range of structures is flexibly and accurately used.\n- The majority of sentences are error-free, and punctuation is well managed.\n- Occasional, non-systematic errors and inappropriacies occur, but have minimal impact on communication.'
              },
              {
                'id': 100,
                'bandScore': 9,
                'description': '- A wide range of structures is used with full flexibility and control.\n- Punctuation and grammar are used appropriately throughout.\n- Minor errors are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 36,
            'name': 'Critical Errors',
            'description': 'Critical Errors',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 268,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 37,
            'name': 'Overall Score & Feedback',
            'description': 'Overall Feedback',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 235,
                'bandScore': 0,
                'description': null
              },
              {
                'id': 236,
                'bandScore': 1,
                'description': null
              },
              {
                'id': 237,
                'bandScore': 2,
                'description': null
              },
              {
                'id': 238,
                'bandScore': 3,
                'description': null
              },
              {
                'id': 239,
                'bandScore': 4,
                'description': null
              },
              {
                'id': 240,
                'bandScore': 5,
                'description': null
              },
              {
                'id': 241,
                'bandScore': 6,
                'description': null
              },
              {
                'id': 242,
                'bandScore': 7,
                'description': null
              },
              {
                'id': 243,
                'bandScore': 8,
                'description': null
              },
              {
                'id': 244,
                'bandScore': 9,
                'description': null
              }
            ]
          },
          {
            'id': 43,
            'name': 'Arguments Assessment',
            'description': 'Arguments Assessment',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 284,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 45,
            'name': 'Vocabulary',
            'description': 'Vocabulary',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 286,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 47,
            'name': 'Improved Version',
            'description': 'Improved Version',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 288,
                'bandScore': 0,
                'description': null
              }
            ]
          }
        ]
      },
      {
        'id': 3116,
        'title': 'Rainwater Collection for Drinking',
        'section': 'Academic Writing Task 1',
        'taskId': 0,
        'test': 'IELTS',
        'testId': 0,
        'time': '20 minutes',
        'type': 'Process',
        'sample': true,
        'averageScore': '0.0',
        'submission': 0,
        'like': 0,
        'dislike': 0,
        'status': 'To do',
        'difficulty': 'Hard',
        'direction': 'You should spend about 20 minutes on this task.\n\nWrite at least 150 words.',
        'userId': null,
        'createdBy': null,
        'addedDate': '0001-01-01T00:00:00',
        'lastActivityDate': '0001-01-01T00:00:00',
        'questionsPart': [
          {
            'questionId': 3116,
            'name': 'Chart',
            'content': '3116.png'
          },
          {
            'questionId': 3116,
            'name': 'Question',
            'content': '<p><span style="font-size: 16px"><span style="color: rgb(79, 79, 101);">The diagram shows how rainwater is collected for the use of drinking water in an Australian town. Summarize the information by selecting and reporting the main features and make comparisons where relevant.</span></span></p>'
          },
          {
            'questionId': 3116,
            'name': 'Tip',
            'content': '<p><strong><u><span style="font-size: 18px">Ý Tưởng Phát Triển Bài</span></u></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Giới Thiệu Biểu Đồ</span></strong><span style="font-size: 16px">: Bắt đầu bằng cách giới thiệu biểu đồ hiển thị quy trình thu thập và xử lý nước mưa thành nước uống.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Thu Thập Nước Mưa</span></strong><span style="font-size: 16px">: Mô tả cách nước mưa được thu từ mái nhà và dẫn vào hệ thống thông qua các rãnh thoát nước.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Lọc và Xử Lý Sơ Bộ</span></strong><span style="font-size: 16px">: Giải thích quá trình lọc sơ bộ khi nước đi qua bộ lọc nước trước khi được chuyển đến cơ sở xử lý nước.</span></p><p><span style="font-size: 16px">4. </span><strong><span style="font-size: 16px">Hóa Chất Xử Lý Nước</span></strong><span style="font-size: 16px">: Thảo luận về giai đoạn sử dụng hóa chất trong quy trình xử lý, làm sạch nước và diệt khuẩn.</span></p><p><span style="font-size: 16px">5. </span><strong><span style="font-size: 16px">Bể Chứa Nước</span></strong><span style="font-size: 16px">: Mô tả bể chứa, nơi nước mưa đã được xử lý được lưu trữ trước khi sử dụng làm nước uống.</span></p><p><span style="font-size: 16px">6. </span><strong><span style="font-size: 16px">Quy Trình Tái Chế Nước Mưa</span></strong><span style="font-size: 16px">: Nhấn mạnh sự bền vững của việc tái chế nước mưa và ý nghĩa của nó đối với nguồn cung cấp nước ở thị trấn.</span></p><p><span style="font-size: 16px">7. </span><strong><span style="font-size: 16px">Hiệu Quả của Hệ Thống Lọc</span></strong><span style="font-size: 16px">: So sánh lượng nước mưa thu được với lượng nước thực tế sau khi lọc để đánh giá hiệu suất của hệ thống.</span></p><p><span style="font-size: 16px">8. </span><strong><span style="font-size: 16px">Tiêu Chuẩn Nước Uống</span></strong><span style="font-size: 16px">: Đề cập đến các tiêu chuẩn an toàn có thể áp dụng để đảm bảo nước sau khi xử lý đủ điều kiện làm nước uống.</span></p><p><span style="font-size: 16px">9. </span><strong><span style="font-size: 16px">Các Giai Đoạn Xử Lý</span></strong><span style="font-size: 16px">: Mô tả từng bước của quy trình xử lý từ thu thập, lọc, thêm hóa chất, đến lưu trữ.</span></p><p><span style="font-size: 16px">10. </span><strong><span style="font-size: 16px">Kết Luận và Tác Động</span></strong><span style="font-size: 16px">: Tóm tắt các đặc điểm chính của quy trình và thảo luận về tác động của việc thu thập nước mưa đối với cộng đồng và môi trường.</span></p><p><strong><u><span style="font-size: 18px">Gợi Ý Cho Cấu Trúc Bài</span></u></strong></p><p><strong><span style="font-size: 16px">### Mở Bài</span></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Giới Thiệu</span></strong><span style="font-size: 16px">: Mở đầu bằng cách giới thiệu biểu đồ, nhấn mạnh nó miêu tả quy trình thu thập nước mưa và chuyển đổi thành nước uống ở một thị trấn Úc.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Mục Đích</span></strong><span style="font-size: 16px">: Nêu bật mục đích của quy trình này, đặc biệt quan trọng trong việc cung cấp nước sạch và bảo vệ môi trường.</span></p><p><strong><span style="font-size: 16px">### Thân Bài</span></strong></p><p><strong><span style="font-size: 16px">Đoạn 1: Thu Thập Nước Mưa</span></strong></p><p><span style="font-size: 16px">- Mô tả cách nước mưa được thu từ mái nhà thông qua hệ thống rãnh và dẫn vào hệ thống lọc.</span></p><p><strong><span style="font-size: 16px">Đoạn 2: Quá Trình Lọc và Xử Lý</span></strong></p><p><span style="font-size: 16px">- Phân tích các bước lọc nước bao gồm việc dùng bộ lọc và thêm hóa chất để diệt khuẩn và đảm bảo an toàn.</span></p><p><strong><span style="font-size: 16px">Đoạn 3: Bể Chứa và Phân Phối</span></strong></p><p><span style="font-size: 16px">- Giải thích về bể chứa, nơi nước sẽ được giữ trước khi phân phối cho sử dụng, và cách thức phân phối nước đến người dân.</span></p><p><strong><span style="font-size: 16px">### Kết Luận</span></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Tổng kết</span></strong><span style="font-size: 16px">: Tóm tắt lại quy trình chính từ thu thập nước mưa đến lúc trở thành nước uống.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Nhận Xét</span></strong><span style="font-size: 16px">: Đưa ra nhận xét về tầm quan trọng của việc tái sử dụng nước mưa, cũng như ảnh hưởng tích cực đến nguồn tài nguyên nước và môi trường.</span></p><p></p>'
          },
          {
            'questionId': 3116,
            'name': 'Vocabulary',
            'content': "<p><span style=\"font-size: 16px\">1. </span><strong><span style=\"font-size: 16px\">Rainwater harvesting</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈreɪn.wɔː.tər ˈhɑː.vɪ.stɪŋ/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: thu gom nước mưa</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the collection and storage of rainwater for reuse</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The diagram details the </span><strong><span style=\"font-size: 16px\">rainwater harvesting</span></strong><span style=\"font-size: 16px\"> process used for drinking water in an Australian town.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: rainwater collection, water capture</span></p><p><span style=\"font-size: 16px\">2. </span><strong><span style=\"font-size: 16px\">Filtration system</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /fɪlˈtreɪ.ʃən ˈsɪs.təm/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: hệ thống lọc</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: a process that removes impurities from water by means of a physical barrier</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The </span><strong><span style=\"font-size: 16px\">filtration system</span></strong><span style=\"font-size: 16px\"> is a crucial component of the town's water purification infrastructure.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: filtering system, purification system</span></p><p><span style=\"font-size: 16px\">3. </span><strong><span style=\"font-size: 16px\">Potable water</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈpəʊ.tə.bəl ˈwɔː.tər/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: nước uống được</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: water that is safe to drink</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The end goal of the process is to provide </span><strong><span style=\"font-size: 16px\">potable water</span></strong><span style=\"font-size: 16px\"> for the town's residents.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: drinking water, safe water</span></p><p><span style=\"font-size: 16px\">4. </span><strong><span style=\"font-size: 16px\">Purification</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /pjʊˌrɪ.fɪˈkeɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự làm sạch</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the removal of contaminants from something</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Purification</span></strong><span style=\"font-size: 16px\"> of rainwater is essential before it can be used as drinking water.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: cleansing, sanitization</span></p><p><span style=\"font-size: 16px\">5. </span><strong><span style=\"font-size: 16px\">Collection point</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /kəˈlek.ʃən pɔɪnt/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: điểm thu gom</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: a designated location where rainwater is gathered</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Rainwater is first gathered at a </span><strong><span style=\"font-size: 16px\">collection point</span></strong><span style=\"font-size: 16px\"> on the roof of houses.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: gathering point, accumulation area</span></p><p><span style=\"font-size: 16px\">6. </span><strong><span style=\"font-size: 16px\">Storage tank</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈstɔː.rɪdʒ tæŋk/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: bể chứa</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: a container for storing liquid</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: After initial treatment, the water is stored in a large </span><strong><span style=\"font-size: 16px\">storage tank</span></strong><span style=\"font-size: 16px\">.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: reservoir, holding tank</span></p><p><span style=\"font-size: 16px\">7. </span><strong><span style=\"font-size: 16px\">Disinfection</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌdɪs.ɪnˈfek.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự khử trùng</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the process of cleaning something using chemicals that kill bacteria and other organisms</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Disinfection</span></strong><span style=\"font-size: 16px\"> with chemicals ensures that the water is free from pathogens.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: sterilization, sanitization</span></p><p><span style=\"font-size: 16px\">8. </span><strong><span style=\"font-size: 16px\">Conduit</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈkɒn.dʊɪt/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: ống dẫn</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: a pipe or channel through which something passes</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p></p><p><span style=\"font-size: 16px\">- Ví dụ: A </span><strong><span style=\"font-size: 16px\">conduit</span></strong><span style=\"font-size: 16px\"> transports rainwater from the collection point to the water treatment facility.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: channel, duct</span></p><p><span style=\"font-size: 16px\">9. </span><strong><span style=\"font-size: 16px\">Precipitation capture</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /prɪˌsɪp.ɪˈteɪ.ʃən ˈkæp.tʃər/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: thu gom mưa</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the process of collecting water from rain</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The diagram shows how </span><strong><span style=\"font-size: 16px\">precipitation capture</span></strong><span style=\"font-size: 16px\"> is the first step in the process.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: rain capture, water collection</span></p><p><span style=\"font-size: 16px\">10. </span><strong><span style=\"font-size: 16px\">Sedimentation</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌsed.ɪˈmenˈteɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự lắng cặn</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the process by which particulate matter settles to the bottom of a liquid and forms sediment</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Sedimentation</span></strong><span style=\"font-size: 16px\"> may occur in the storage tank as impurities settle.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: settling, deposition</span></p><p><span style=\"font-size: 16px\">11. </span><strong><span style=\"font-size: 16px\">Gravity-fed</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈɡræv.ɪ.ti fɛd/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: dùng trọng lực để dẫn nước</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: utilizing gravity to move water through a system</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The rainwater system is </span><strong><span style=\"font-size: 16px\">gravity-fed</span></strong><span style=\"font-size: 16px\">, allowing water to flow without the need for pumps.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: gravity-driven, gravitational</span></p><p><span style=\"font-size: 16px\">12. </span><strong><span style=\"font-size: 16px\">Chemical treatment</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈkem.ɪ.kəl ˈtriːt.mənt/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: xử lý hóa chất</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the application of chemicals to water to remove contaminants or bacteria</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Chemical treatment</span></strong><span style=\"font-size: 16px\"> is a necessary stage to ensure the water's safety.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: chemical purification, chemical cleansing</span></p><p><span style=\"font-size: 16px\">13. </span><strong><span style=\"font-size: 16px\">Environmental sustainability</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ɪnˌvaɪ.rənˈmen.təl ˌsʌs.teɪˈnæb.ɪl.ɪ.ti/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: tính bền vững của môi trường</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the quality of not being harmful to the environment and depleting natural resources, thereby supporting long-term ecological balance</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Environmental sustainability</span></strong><span style=\"font-size: 16px\"> is at the heart of the rainwater harvesting system depicted.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: eco-sustainability, green sustainability</span></p><p><span style=\"font-size: 16px\">14. </span><strong><span style=\"font-size: 16px\">Drainage</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈdreɪ.nɪdʒ/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: hệ thống thoát nước</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the system by which water or other liquids are drained from a place</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The </span><strong><span style=\"font-size: 16px\">drainage</span></strong><span style=\"font-size: 16px\"> system collects excess rainwater from rooftops.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: waste water system, sewerage</span></p><p><span style=\"font-size: 16px\">15. </span><strong><span style=\"font-size: 16px\">Potabilization</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /pəˌtæb.ɪl.ɪˈzeɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: làm cho nước trở nên có thể uống được</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the process of making water potable, i.e., safe to drink</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The water undergoes </span><strong><span style=\"font-size: 16px\">potabilization</span></strong><span style=\"font-size: 16px\"> before being suitable for drinking.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: water treatment, water purification</span></p><p><span style=\"font-size: 16px\">16. **Retention basin</span></p><p><span style=\"font-size: 16px\">**</span></p><p><span style=\"font-size: 16px\">- Phát âm: /rɪˈten.ʃən ˈbeɪ.sɪn/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: hồ chứa</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: a large storage space for holding water until it can be released or treated</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The storage section serves as a </span><strong><span style=\"font-size: 16px\">retention basin</span></strong><span style=\"font-size: 16px\"> for the collected rainwater.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: holding basin, reservoir</span></p><p><span style=\"font-size: 16px\">17. </span><strong><span style=\"font-size: 16px\">Recirculation</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌriː.sɜː.kjʊˈleɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: tái tuần hoàn</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the process of moving water back through a system for further treatment or use</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The </span><strong><span style=\"font-size: 16px\">recirculation</span></strong><span style=\"font-size: 16px\"> of water through the filters ensures its purity.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: reprocessing, cycling</span></p><p><span style=\"font-size: 16px\">18. </span><strong><span style=\"font-size: 16px\">Aqueous solution</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈæk.wi.əs səˈluː.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: dung dịch nước</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: a solution in which the solvent is water</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">- Ví dụ: Chemicals are mixed into the </span><strong><span style=\"font-size: 16px\">aqueous solution</span></strong><span style=\"font-size: 16px\"> for disinfection purposes.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: watery mixture, water-based solution</span></p><p><span style=\"font-size: 16px\">19. </span><strong><span style=\"font-size: 16px\">Sanitation</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˌsæn.ɪˈteɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: sự vệ sinh</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the development and application of sanitary measures for the sake of cleanliness, protecting health, etc.</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: </span><strong><span style=\"font-size: 16px\">Sanitation</span></strong><span style=\"font-size: 16px\"> of rainwater is a key stage in preparing it for consumption.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: hygiene, cleanliness</span></p><p><span style=\"font-size: 16px\">20. </span><strong><span style=\"font-size: 16px\">Watercourse</span></strong></p><p><span style=\"font-size: 16px\">- Phát âm: /ˈwɔː.tə.kɔːs/</span></p><p><span style=\"font-size: 16px\">- Nghĩa: đường dẫn nước</span></p><p><span style=\"font-size: 16px\">- Nghĩa tiếng Anh: the route which a stream of water takes through the ground</span></p><p><span style=\"font-size: 16px\">- Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">- Ví dụ: The diagram includes a </span><strong><span style=\"font-size: 16px\">watercourse</span></strong><span style=\"font-size: 16px\"> that leads from the filtration system to the public water supply.</span></p><p><span style=\"font-size: 16px\">- Từ đồng nghĩa: channel, waterway</span></p><p></p>"
          }
        ],
        'samples': [
          {
            'id': 368,
            'questionId': 3116,
            'sampleText': '<p><span style="font-size: 16px">The diagram illustrates the process of collecting and purifying rainwater for drinking purposes in an Australian town. The process is depicted in several stages, beginning with rainwater collection and culminating in its storage for drinking.</span></p><p><span style="font-size: 16px">Initially, rainwater is gathered from house rooftops, flowing through gutters into a centralized drainage system that directs the water towards a primary treatment facility. This initial stage is crucial as it utilizes gravitational force, ensuring that no pumping mechanisms are required for water conveyance.</span></p><p><span style="font-size: 16px">Once collected, the water passes through a series of purification steps to ensure its safety for drinking. The first step in this sequence involves the addition of chemicals, which likely serve to neutralize any biological contaminants present in the water. Subsequently, the water is directed through a water filter, which removes particulate matter and further refines the quality of the water to meet health standards.</span></p><p><span style="font-size: 16px">The final stage of the process involves storing the treated water in a large storage tank. From here, the purified water is then distributed to the residents of the town for consumption.</span></p><p><span style="font-size: 16px">Overall, the system effectively captures and purifies rainwater using a series of well-integrated steps, highlighting an efficient method of sustainable water use in the community. This process not only ensures the provision of safe drinking water but also emphasizes the importance of resource conservation in urban areas.</span></p>',
            'bandScore': 8,
            'comment': '<p><strong><span style="font-size: 16px">1. Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">- Bài viết cung cấp một cái nhìn tổng quan chi tiết và rõ ràng về quy trình thu gom và xử lý nước mưa để sử dụng làm nước uống ở một thị trấn ở Úc.</span></p><p><span style="font-size: 16px">- Ngôn từ được sử dụng một cách linh hoạt và chính xác, với sự đa dạng trong cấu trúc câu, giúp thông tin được truyền đạt một cách hiệu quả và dễ hiểu.</span></p><p><span style="font-size: 16px">- Bài viết có sự liên kết chặt chẽ giữa các phần, thể hiện sự chuyển tiếp mượt mà từ giai đoạn này sang giai đoạn khác của quá trình.</span></p><p><strong><span style="font-size: 16px">2. Điểm yếu:</span></strong></p><p><span style="font-size: 16px">- Bài viết có thể mở rộng hơn nữa bằng cách đưa ra các so sánh về hiệu quả của phương pháp này so với các phương pháp thu thập và xử lý nước khác.</span></p><p><span style="font-size: 16px">- Có thể bổ sung thông tin về tầm quan trọng của từng bước trong quy trình, ví dụ như tác dụng cụ thể của các hóa chất được sử dụng trong quá trình xử lý nước.</span></p><p><span style="font-size: 16px">- Mặc dù ngôn từ được sử dụng tốt nhưng vẫn có thể tăng cường sự hấp dẫn bằng cách sử dụng các từ ngữ và cụm từ đa dạng hơn nữa.</span></p><p><strong><span style="font-size: 16px">3. Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">Bài luận đã thành công trong việc mô tả quy trình thu gom và xử lý nước mưa để sử dụng làm nước uống, cung cấp cái nhìn toàn diện về các bước chính. Sự rõ ràng trong trình bày và sự linh hoạt trong ngôn từ làm cho bài viết này trở nên thông tin và dễ hiểu. Tuy nhiên, để nâng cao giá trị thông tin và thuyết phục, bài viết có thể khai thác sâu hơn về các chi tiết cụ thể và bổ sung so sánh với các phương pháp khác. Nhìn chung, bài viết đã đáp ứng yêu cầu của một bài luận band 8, cho thấy sự hiểu biết sâu sắc và khả năng phân tích dữ liệu một cách hiệu quả.</span></p>',
            'lastActivityDate': '2024-04-30T17:02:23.4609701',
            'title': 'Rainwater Collection for Drinking'
          },
          {
            'id': 369,
            'questionId': 3116,
            'sampleText': "<p><span style=\"font-size: 16px\">The diagram details the method by which rainwater is collected and transformed into drinking water in an Australian town. The process is divided into several distinct stages, demonstrating a comprehensive system from collection to consumption.</span></p><p><span style=\"font-size: 16px\">Initially, rainwater is gathered from house rooftops, channeling through drains into a central filtration system. This initial step is crucial as it harnesses natural rainfall directly from residential areas, promoting sustainability.</span></p><p><span style=\"font-size: 16px\">Once collected, the rainwater undergoes preliminary treatment where chemicals are added to purify the water. This stage is essential for removing impurities and ensuring the water is safe for drinking. Following chemical treatment, the water is then passed through a water filter to further cleanse it of any remaining contaminants.</span></p><p><span style=\"font-size: 16px\">The final stage of the process involves the storage of the cleaned water in a large tank, ready for distribution. This stored water is then piped to households in the town as drinking water, completing the cycle of rain to tap.</span></p><p><span style=\"font-size: 16px\">This efficient system not only provides a sustainable source of drinking water but also demonstrates the town's commitment to environmentally friendly practices. The process is logically organized, ensuring that the water remains clean and safe throughout each step before it reaches the consumer.</span></p>",
            'bandScore': 7,
            'comment': '<p><strong><span style="font-size: 16px">1. Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">- Bài viết cung cấp một cái nhìn chi tiết về quy trình thu thập và xử lý nước mưa để sử dụng làm nước uống, từ bắt đầu cho đến kết thúc.</span></p><p><span style="font-size: 16px">- Cấu trúc của bài viết rất rõ ràng và có tổ chức, với sự chuyển tiếp logic giữa các giai đoạn xử lý nước.</span></p><p><span style="font-size: 16px">- Sử dụng ngôn từ phù hợp và hiệu quả, miêu tả từng bước một cách dễ hiểu, thể hiện khả năng kiểm soát tốt về mặt ngôn ngữ và cấu trúc câu.</span></p><p><strong><span style="font-size: 16px">2. Điểm yếu:</span></strong></p><p><span style="font-size: 16px">- Bài viết có thể bổ sung thêm chi tiết về mỗi giai đoạn, như cụ thể hóa các hóa chất được sử dụng hoặc hiệu quả cụ thể của bộ lọc nước.</span></p><p><span style="font-size: 16px">- Mặc dù thông tin được trình bày mạch lạc, nhưng bài viết có thể tăng cường thêm bằng cách đưa ra phân tích hoặc bình luận về tầm quan trọng của việc tái sử dụng nước mưa trong bối cảnh môi trường hiện nay.</span></p><p><span style="font-size: 16px">- Thiếu thông tin về những thách thức hoặc hạn chế có thể xảy ra trong quá trình xử lý và lưu trữ nước.</span></p><p><strong><span style="font-size: 16px">3. Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">Bài viết đã hiệu quả trong việc trình bày quy trình thu gom và xử lý nước mưa để sử dụng làm nước uống, thể hiện sự hiểu biết về các bước cần thiết để biến nước mưa thành nước uống an toàn. Ngôn từ và cấu trúc được sử dụng khá tốt, giúp người đọc dễ dàng theo dõi và hiểu rõ quy trình. Tuy nhiên, để đạt đến mức độ hoàn chỉnh hơn, bài viết nên mở rộng thêm phần phân tích và cung cấp thông tin chi tiết hơn về mặt kỹ thuật và môi trường. Nhìn chung, đây là một nỗ lực tốt để giải thích một quy trình phức tạp một cách ngắn gọn và rõ ràng.</span></p>',
            'lastActivityDate': '2024-04-30T17:04:56.9076945',
            'title': 'Rainwater Collection for Drinking'
          },
          {
            'id': 370,
            'questionId': 3116,
            'sampleText': "<p><span style=\"font-size: 16px\">The diagram illustrates the method by which rainwater is collected and processed to become drinkable in an Australian town. The process starts with rainwater being collected from the rooftops of houses. This water then flows through drains into a larger system designed for initial filtering and chemical treatment.</span></p><p><span style=\"font-size: 16px\">After the rainwater is collected, it undergoes a crucial purification step where chemicals are added to eliminate any harmful substances. Following this chemical treatment, the water is directed to a filtration system, which helps remove smaller particulates and further ensures the water's purity.</span></p><p><span style=\"font-size: 16px\">The final step in this process is storing the treated water. It is kept in large storage tanks until it is needed for use. From these tanks, the purified water is then supplied to the local community for drinking.</span></p><p><span style=\"font-size: 16px\">This system not only provides a sustainable source of clean water but also demonstrates an efficient use of natural rainfall. The diagram effectively captures each key phase of the process, highlighting the town’s initiative in sustainable water management.</span></p>",
            'bandScore': 6,
            'comment': '<p>1. <strong>Điểm mạnh:</strong></p><p>   - Bài viết cung cấp cái nhìn tổng quan rõ ràng về quá trình thu thập và xử lý nước mưa để sử dụng làm nước uống.</p><p>   - Thông tin được trình bày một cách có tổ chức, từ việc thu gom, xử lý hóa học, lọc và lưu trữ, giúp người đọc dễ dàng theo dõi các bước của quá trình.</p><p>   - Ngôn từ sử dụng trong bài viết phù hợp và dễ hiểu, giúp làm nổi bật các giai đoạn chính của quy trình.</p><p>2. <strong>Điểm yếu:</strong></p><p>   - Bài viết thiếu chi tiết về các loại hóa chất được sử dụng trong quá trình xử lý nước và cách thức mà chúng tác động lên nước mưa để làm sạch nước.</p><p>   - Có thể bổ sung thêm thông tin về hiệu quả của từng bước xử lý hoặc so sánh với các phương pháp khác để tăng tính thông tin và sâu sắc cho bài viết.</p><p>   - Bài viết còn thiếu các ví dụ cụ thể hoặc số liệu thống kê để hỗ trợ cho các nhận định được đưa ra.</p><p>3. <strong>Đánh giá chung:</strong></p><p>   - Bài viết đã thành công trong việc mô tả quá trình xử lý nước mưa từ bắt đầu đến cuối, cung cấp một cái nhìn hữu ích về cách thức một thị trấn ở Úc tái sử dụng nguồn nước mưa. </p><p>   - Tuy nhiên, để đạt được điểm số cao hơn trong khuôn khổ IELTS, bài viết cần phải cải thiện bằng cách bổ sung thêm chi tiết kỹ thuật, phân tích sâu hơn về quá trình, và có thể so sánh với các phương pháp khác.</p><p>   - Nhìn chung, bài viết đã phác thảo rõ ràng quy trình và có sự liên kết tốt giữa các phần, nhưng cần tăng cường mức độ chi tiết và sự thuyết phục để trở nên hoàn thiện hơn.</p>',
            'lastActivityDate': '2024-04-30T21:55:51.4498361',
            'title': 'Rainwater Collection for Drinking'
          }
        ],
        'rubrics': [
          {
            'id': 3,
            'name': 'Task Achievement',
            'description': 'The Task Achievement criterion in IELTS Academic Writing Task 1 evaluates how well you summarize key trends, compare data, cover all task requirements, and report features accurately from visuals like graphs or charts, focusing on your ability to identify and convey the most important information.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 13,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 14,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- The content is wholly unrelated to the task.\n- Any copied rubric must be discounted.'
              },
              {
                'id': 15,
                'bandScore': 2,
                'description': 'The content barely relates to the task.'
              },
              {
                'id': 16,
                'bandScore': 3,
                'description': '- The response does not address the requirements of the task (possibly because\nof misunderstanding of the data/diagram/situation).\n- Key features/bullet points which are presented may be largely irrelevant.\n- Limited information is presented, and this may be used repetitively.'
              },
              {
                'id': 17,
                'bandScore': 4,
                'description': '- The response is an attempt to address the task.\n- Few key features have been selected.\n- The format may be inappropriate.\n- Key features/bullet points which are presented may be irrelevant, repetitive,\ninaccurate or inappropriate.'
              },
              {
                'id': 18,
                'bandScore': 5,
                'description': '- The response generally addresses the requirements of the task. The\nformat may be inappropriate in places.\n- Key features which are selected are not adequately covered.\nThe recounting of detail is mainly mechanical. There may be no data to\nsupport the description.\n- There may be a tendency to focus on details (without referring to the\nbigger picture).\n- The inclusion of irrelevant, inappropriate or inaccurate material in key\nareas detracts from the task achievement.\n- There is limited detail when extending and illustrating the main points.'
              },
              {
                'id': 19,
                'bandScore': 6,
                'description': 'The response focuses on the requirements of the task and an appropriate\nformat is used.\n- Key features which are selected are covered and adequately\nhighlighted. A relevant overview is attempted. Information is appropriately\nselected and supported using figures/data.\n- Some irrelevant, inappropriate or inaccurate information may occur in\nareas of detail or when illustrating or extending the main points.\n- Some details may be missing (or excessive) and further extension or\nillustration may be needed.'
              },
              {
                'id': 20,
                'bandScore': 7,
                'description': '- The response covers the requirements of the task.\n- The content is relevant and accurate. There may be a few omissions or lapses.\nThe format is appropriate.\n- Key features which are selected are covered and clearly\nhighlighted but could be more fully or more appropriately illustrated or\nextended.\n- It presents a clear overview, the data are appropriately\ncategorised, and main trends or differences are identified.'
              },
              {
                'id': 21,
                'bandScore': 8,
                'description': '- The response covers all the requirements of the task appropriately, relevantly\nand sufficiently.\n- Key features are skilfully selected, and clearly presented,\nhighlighted and illustrated.\n- There may be occasional omissions or lapses in content.'
              },
              {
                'id': 23,
                'bandScore': 9,
                'description': '- All the requirements of the task are fully and appropriately satisfied.\n- There may be extremely rare lapses in content.'
              }
            ]
          },
          {
            'id': 4,
            'name': 'Coherence & Cohesion',
            'description': 'The Coherence & Cohesion criterion assesses your ability to organize and link information clearly and logically in IELTS Academic Writing Task 1. It focuses on effective paragraphing, logical sequencing of ideas, and the use of cohesive devices (like linking words and pronouns) to help the reader understand the relationships between ideas.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 25,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 26,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- The writing fails to communicate any message and appears to be by a virtual non writer.'
              },
              {
                'id': 28,
                'bandScore': 2,
                'description': '- There is little relevant message, or the entire response may be off topic.\n- There is little evidence of control of organisational features.'
              },
              {
                'id': 29,
                'bandScore': 3,
                'description': '- There is no apparent logical organisation .\n- Ideas are discernible but difficult to relate to each other.\n- Minimal use of sequencers or cohesive devices. Those used do not necessarily indicate a logical relationship between ideas.\n- There is difficulty in identifying referencing.'
              },
              {
                'id': 30,
                'bandScore': 4,
                'description': '- Information and ideas are evident but not arranged coherently, and there is no clear progression within the response.\n- Relationships between ideas can be unclear and/or inadequately marked. There is some use of basic cohesive devices, which may be inaccurate or repetitive.\n- There is inaccurate use or a lack of substitution or referencing.'
              },
              {
                'id': 31,
                'bandScore': 5,
                'description': '- Organisation is evident but is not wholly logical and there may be a lack of overall progression. Nevertheless, there is a sense of underlying coherence to the response.\n- The relationship of ideas can be followed but the sentences are not fluently linked to each other.\n- There may be limited/overuse of cohesive devices with some inaccuracy.\n- The writing may be repetitive due to inadequate and/or inaccurate use of reference and substitution.'
              },
              {
                'id': 32,
                'bandScore': 6,
                'description': '- Information and ideas are generally arranged coherently and there is a clear overall progression. \n- Cohesive devices are used to some good effect but cohesion within and/or between sentences may be faulty or mechanical due to misuse, overuse or omission. \n- The use of reference and substitution may lack flexibility or clarity and result in some repetition or error'
              },
              {
                'id': 33,
                'bandScore': 7,
                'description': '- Information and ideas are logically organised and there is a clear progression throughout the response. A few lapses may occur.\n\n- A range of cohesive devices including reference and substitution is used flexibly but with some inaccuracies or some over/under use.'
              },
              {
                'id': 34,
                'bandScore': 8,
                'description': '- The message can be followed with ease. \n- Information and ideas are logically sequenced, and cohesion is well managed. \n- Occasional lapses in coherence or cohesion may occur. \n- Paragraphing is used sufficiently and appropriately.'
              },
              {
                'id': 35,
                'bandScore': 9,
                'description': '- The message can be followed effortlessly.\n- Cohesion is used in such a way that it very rarely attracts attention.\n- Any lapses in coherence or cohesion are minimal.\n- Paragraphing is skilfully managed.'
              }
            ]
          },
          {
            'id': 5,
            'name': 'Lexical Resource',
            'description': 'The Lexical Resource criterion evaluates your range of vocabulary, accuracy in word choice, and ability to use words appropriately to express precise meanings in IELTS Academic Writing Task 1. It focuses on your ability to use a variety of vocabulary to describe data, trends, and processes clearly and accurately.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 36,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English throughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 37,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- No resource is apparent, except for a few isolated words.'
              },
              {
                'id': 38,
                'bandScore': 2,
                'description': '- The resource is extremely limited with few recognisable strings, apart from memorised phrases. \n- There is no apparent control of word formation and/or spelling.'
              },
              {
                'id': 39,
                'bandScore': 3,
                'description': '- The resource is inadequate (which may be due to the response being significantly underlength).\n- Possible over dependence on input material or memorised language. \n- Control of word choice and/or spelling is very limited, and errors predominate. These errors may severely impede meaning.'
              },
              {
                'id': 40,
                'bandScore': 4,
                'description': '- The resource is limited and inadequate for or unrelated to the task. Vocabulary is basic and may be used repetitively.\n- There may be inappropriate use of lexical chunks (e. memorised phrases, formulaic language and/or language from the input material).\n- Inappropriate word choice and/or errors in word formation and/or in spelling may impede meaning.'
              },
              {
                'id': 41,
                'bandScore': 5,
                'description': '- The resource is limited but minimally adequate for the task. \n- Simple vocabulary may be used accurately but the range does not permit much variation in expression. \n- There may be frequent lapses in the appropriacy of word choice, and a lack of flexibility is apparent in frequent simplifications and/or repetitions. \n- Errors in spelling and/or word formation may be noticeable and may cause some difficulty for the reader.'
              },
              {
                'id': 42,
                'bandScore': 6,
                'description': '- The resource is generally adequate and appropriate for the task.\n- The meaning is generally clear in spite of a rather restricted range or a lack of precision in word choice.\n- If the writer is a risk taker, there will be a wider range of vocabulary used but higher degrees of inaccuracy or inappropriacy.\n- There are some errors in spelling and/or word formation, but these do not impede communication.'
              },
              {
                'id': 43,
                'bandScore': 7,
                'description': '- The resource is sufficient to allow some flexibility and precision. \n- There is some ability to use less common and/or idiomatic items. \n- An awareness of style and collocation is evident, though inappropriacies occur. \n- There are only a few errors in spelling and/or word formation, and they do not detract from overall clarity.'
              },
              {
                'id': 44,
                'bandScore': 8,
                'description': '- A wide resource is fluently and flexibly used to convey precise meanings within the scope of the task.\n- There is skilful use of uncommon and/or idiomatic items when appropriate, despite occasional inaccuracies in word choice and collocation.\n- Occasional errors in spelling and/or word formation may occur, but have minimal impact on communication.'
              },
              {
                'id': 45,
                'bandScore': 9,
                'description': '- Full flexibility and precise use are evident within the scope of the task. \n- A wide range of vocabulary is used accurately and appropriately with very natural and sophisticated control of lexical features. \n- Minor errors in spelling and word formation are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 6,
            'name': 'Grammatical Range & Accuracy',
            'description': 'The Grammatical Range & Accuracy criterion assesses your use of sentence structures and grammatical accuracy in IELTS Academic Writing Task 1. It focuses on your ability to construct a range of sentence types correctly and use grammar precisely to convey information and ideas effectively.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 46,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 49,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- No rateable language is evident.'
              },
              {
                'id': 53,
                'bandScore': 2,
                'description': 'There is little or no evidence of sentence forms (except in memorised phrases).'
              },
              {
                'id': 54,
                'bandScore': 3,
                'description': '- Sentence forms are attempted, but errors in grammar and punctuation predominate (except in memorised phrases or those taken from the input material). This prevents most meaning from coming through. \n- Length may be insufficient to provide evidence of control of sentence forms.'
              },
              {
                'id': 55,
                'bandScore': 4,
                'description': '- A very limited range of structures is used. \n- Subordinate clauses are rare and simple sentences predominate. \n- Some structures are produced accurately but grammatical errors are frequent and may impede meaning. \n- Punctuation is often faulty or inadequate.'
              },
              {
                'id': 56,
                'bandScore': 5,
                'description': '- The range of structures is limited and rather repetitive.\n- Although complex sentences are attempted, they tend to be faulty, and the greatest accuracy is achieved on simple sentences.\n- Grammatical errors may be frequent and cause some difficulty for the reader.\n- Punctuation may be faulty.'
              },
              {
                'id': 57,
                'bandScore': 6,
                'description': '- A mix of simple and complex sentence forms is used but flexibility is limited.\n- Examples of more complex structures are not marked by the same level of accuracy as in simple structures.\n- Errors in grammar and punctuation occur, but rarely impede communication'
              },
              {
                'id': 58,
                'bandScore': 7,
                'description': '- A variety of complex structures is used with some flexibility and accuracy.\n- Grammar and punctuation are generally well controlled, and error free sentences are frequent.\n- A few errors in grammar may persist, but these do not impede communication.'
              },
              {
                'id': 59,
                'bandScore': 8,
                'description': '- A wide range of structures within the scope of the task is flexibly and accurately used.\n- The majority of sentences are error free, and punctuation is well managed.\n- Occasional, non systematic errors and inappropriacies occur, but have minimal impact on communication.'
              },
              {
                'id': 60,
                'bandScore': 9,
                'description': '- A wide range of structures within the scope of the task is used with full flexibility and control.\n- Punctuation and grammar are used appropriately throughout.\n- Minor errors are extremely rare and have minimal impact on communication'
              }
            ]
          },
          {
            'id': 34,
            'name': 'Critical Errors',
            'description': 'Critical Errors',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 267,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 35,
            'name': 'Overall Score & Feedback',
            'description': 'Overall Feedback',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 225,
                'bandScore': 0,
                'description': null
              },
              {
                'id': 226,
                'bandScore': 1,
                'description': null
              },
              {
                'id': 227,
                'bandScore': 2,
                'description': null
              },
              {
                'id': 228,
                'bandScore': 3,
                'description': null
              },
              {
                'id': 229,
                'bandScore': 4,
                'description': null
              },
              {
                'id': 230,
                'bandScore': 5,
                'description': null
              },
              {
                'id': 231,
                'bandScore': 6,
                'description': null
              },
              {
                'id': 232,
                'bandScore': 7,
                'description': null
              },
              {
                'id': 233,
                'bandScore': 8,
                'description': null
              },
              {
                'id': 234,
                'bandScore': 9,
                'description': null
              }
            ]
          },
          {
            'id': 42,
            'name': 'Arguments Assessment',
            'description': 'Arguments Assessment',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 283,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 44,
            'name': 'Vocabulary',
            'description': 'Vocabulary',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 285,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 46,
            'name': 'Improved Version',
            'description': 'Improved Version',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 287,
                'bandScore': 0,
                'description': null
              }
            ]
          }
        ]
      },
      {
        'id': 3119,
        'title': 'Comparison of Australian Road Tunnels',
        'section': 'Academic Writing Task 1',
        'taskId': 0,
        'test': 'IELTS',
        'testId': 0,
        'time': '20 minutes',
        'type': 'Map',
        'sample': true,
        'averageScore': '0.0',
        'submission': 0,
        'like': 0,
        'dislike': 0,
        'status': 'To do',
        'difficulty': 'Medium',
        'direction': 'You should spend about 20 minutes on this task.\n\nWrite at least 150 words.',
        'userId': null,
        'createdBy': null,
        'addedDate': '0001-01-01T00:00:00',
        'lastActivityDate': '0001-01-01T00:00:00',
        'questionsPart': [
          {
            'questionId': 3119,
            'name': 'Chart',
            'content': '3119.png'
          },
          {
            'questionId': 3119,
            'name': 'Question',
            'content': '<p><span style="font-size: 16px"><span style="color: rgb(79, 79, 101);">The diagrams below give information about two road tunnels in two Australian cities. Summarize the information by selecting and reporting the main features and make comparisons where relevant.</span></span></p>'
          },
          {
            'questionId': 3119,
            'name': 'Tip',
            'content': '<p><strong><u><span style="font-size: 18px">Ý Tưởng Phát Triển Bài</span></u></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Mở Đầu</span></strong><span style="font-size: 16px">: Giới thiệu chung về hai đường hầm đường bộ, bao gồm vị trí ở hai thành phố khác nhau của Úc.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Thời Gian Xây Dựng</span></strong><span style="font-size: 16px">: So sánh về thời gian xây dựng của hai đường hầm, nêu rõ sự khác biệt về khoảng thời gian hoàn thành.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Chi Phí Xây Dựng</span></strong><span style="font-size: 16px">: Phân tích và so sánh chi phí xây dựng của mỗi đường hầm, làm nổi bật sự chênh lệch giữa chúng.</span></p><p><span style="font-size: 16px">4. </span><strong><span style="font-size: 16px">Chiều Dài Đường Hầm</span></strong><span style="font-size: 16px">: Mô tả chiều dài của từng đường hầm và so sánh giữa hai.</span></p><p><span style="font-size: 16px">5. </span><strong><span style="font-size: 16px">Độ Sâu Đường Hầm</span></strong><span style="font-size: 16px">: So sánh độ sâu chôn dưới lòng đất của cả hai đường hầm.</span></p><p><span style="font-size: 16px">6. </span><strong><span style="font-size: 16px">Chất Liệu Đường Hầm</span></strong><span style="font-size: 16px">: Đề cập đến loại vật liệu chủ yếu được sử dụng để xây dựng mỗi đường hầm, như cát cho đường hầm đầu tiên và đá với sét cho đường hầm thứ hai.</span></p><p><span style="font-size: 16px">7. </span><strong><span style="font-size: 16px">Số Lượng Làn Đường</span></strong><span style="font-size: 16px">: Nêu bật số lượng làn đường và khả năng chứa xe trong mỗi đường hầm.</span></p><p><span style="font-size: 16px">8. </span><strong><span style="font-size: 16px">Mục Đích Sử Dụng</span></strong><span style="font-size: 16px">: Bàn luận về mục đích sử dụng của từng đường hầm, có thể liên quan đến việc giảm tắc nghẽn giao thông ở mỗi thành phố.</span></p><p><span style="font-size: 16px">9. </span><strong><span style="font-size: 16px">Cảnh Quan Xung Quanh</span></strong><span style="font-size: 16px">: Phân tích cảnh quan xung quanh mỗi đường hầm, như đường hầm đầu tiên được xây dựng dưới sông và đường hầm thứ hai dưới thành phố.</span></p><p><span style="font-size: 16px">10. </span><strong><span style="font-size: 16px">Kết Luận và Ý Nghĩa</span></strong><span style="font-size: 16px">: Kết thúc bằng cách tổng kết thông tin chính và bình luận về tầm quan trọng của hai công trình đối với hệ thống giao thông ở mỗi thành phố.</span></p><p><strong><u><span style="font-size: 18px">Gợi Ý Cho Cấu Trúc Bài</span></u></strong></p><p><strong><span style="font-size: 16px">### Mở Bài</span></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Giới Thiệu</span></strong><span style="font-size: 16px">: Mô tả biểu đồ cung cấp thông tin về hai đường hầm đường bộ ở hai thành phố Úc, bao gồm thông tin về chi phí xây dựng, thời gian hoàn thành, và đặc điểm kỹ thuật.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Mục Đích Bài Viết</span></strong><span style="font-size: 16px">: Phát biểu mục đích của bài viết, đó là so sánh và phân tích thông tin quan trọng từ biểu đồ.</span></p><p><strong><span style="font-size: 16px">### Thân Bài</span></strong></p><p><strong><span style="font-size: 16px">Đoạn 1: Thông Tin Chung Về Hai Đường Hầm</span></strong></p><p><span style="font-size: 16px">- Trình bày thông tin về thời gian xây dựng và chi phí của hai đường hầm, từ năm 1986 đến 1998 và từ năm 2002 đến 2006, cũng như chi phí lần lượt là $555 triệu và $1,1 tỷ.</span></p><p><strong><span style="font-size: 16px">Đoạn 2: Đặc Điểm Kỹ Thuật</span></strong></p><p><span style="font-size: 16px">- So sánh đặc điểm kỹ thuật của hai đường hầm, bao gồm độ sâu chôn dưới lòng đất (1,5 mét so với 2,5 mét) và chiều dài (2,2 km).</span></p><p><strong><span style="font-size: 16px">Đoạn 3: Cấu Trúc Và Vật Liệu</span></strong></p><p><span style="font-size: 16px">- Mô tả cấu trúc và loại vật liệu được sử dụng để xây dựng, như đường hầm đầu tiên xây dưới lớp cát và đường hầm thứ hai xây dưới lớp đá và sét.</span></p><p><strong><span style="font-size: 16px">Đoạn 4: Tính Năng Và Mục Đích Sử Dụng</span></strong></p><p><span style="font-size: 16px">- Phân tích tính năng và mục đích sử dụng của hai đường hầm, như việc giảm tắc nghẽn giao thông và cải thiện kết nối giao thông ở mỗi thành phố.</span></p><p><strong><span style="font-size: 16px">### Kết Luận</span></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Tóm Tắt</span></strong><span style="font-size: 16px">: Tổng kết lại các thông tin chính đã phân tích trong thân bài, nhấn mạnh vào sự khác biệt về chi phí, thời gian hoàn thành, và đặc điểm kỹ thuật giữa hai đường hầm.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Ý Nghĩa Của Hai Đường Hầm</span></strong><span style="font-size: 16px">: Đánh giá ý nghĩa của việc xây dựng hai đường hầm này đối với việc phát triển hạ tầng giao thông ở Úc.</span></p><p></p>'
          },
          {
            'questionId': 3119,
            'name': 'Vocabulary',
            'content': "<p><span style=\"font-size: 16px\">1. </span><strong><span style=\"font-size: 16px\">Infrastructure project</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /ˈɪn.frəˌstrʌk.tʃər ˈprɒdʒ.ekt/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: dự án cơ sở hạ tầng</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: a large-scale public work construction venture</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: The two tunnels represent significant </span><strong><span style=\"font-size: 16px\">infrastructure projects</span></strong><span style=\"font-size: 16px\"> in their respective cities.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: construction undertaking, public works development</span></p><p><span style=\"font-size: 16px\">2. </span><strong><span style=\"font-size: 16px\">Subterranean passage</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /ˌsʌb.təˈreɪ.ni.ən ˈpæs.ɪdʒ/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: đường hầm dưới lòng đất</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: an underground path or tunnel for transportation</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: Each city has constructed a </span><strong><span style=\"font-size: 16px\">subterranean passage</span></strong><span style=\"font-size: 16px\"> to facilitate traffic flow.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: underground tunnel, below-ground corridor</span></p><p><span style=\"font-size: 16px\">3. </span><strong><span style=\"font-size: 16px\">Construction span</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /kənˈstrʌk.ʃən spæn/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: khoảng thời gian xây dựng</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: the length of time over which the building process occurs</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: The first tunnel's </span><strong><span style=\"font-size: 16px\">construction span</span></strong><span style=\"font-size: 16px\"> was from 1986 to 1998.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: building period, construction duration</span></p><p><span style=\"font-size: 16px\">4. </span><strong><span style=\"font-size: 16px\">Budget allocation</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /ˈbʌdʒ.ɪt æl.əˈkeɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: phân bổ ngân sách</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: the distribution of financial resources for a specific purpose</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: The </span><strong><span style=\"font-size: 16px\">budget allocation</span></strong><span style=\"font-size: 16px\"> for the second tunnel was considerably higher than the first.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: financial distribution, funds assignment</span></p><p><span style=\"font-size: 16px\">5. </span><strong><span style=\"font-size: 16px\">Tunnel length</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /ˈtʌn.əl leŋθ/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: chiều dài đường hầm</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: the measure of the tunnel from one end to the other</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: The first tunnel has a </span><strong><span style=\"font-size: 16px\">tunnel length</span></strong><span style=\"font-size: 16px\"> of 2.2 kilometers.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: stretch of tunnel, tunnel extension</span></p><p><span style=\"font-size: 16px\">6. </span><strong><span style=\"font-size: 16px\">Geological composition</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /ˌdʒiː.əˈlɒdʒ.ɪ.kəl kɒm.pəˈzɪʃ.ən/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: thành phần địa chất</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: the types of rocks and minerals that make up a particular area</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: The diagrams highlight the different </span><strong><span style=\"font-size: 16px\">geological compositions</span></strong><span style=\"font-size: 16px\"> of the areas surrounding each tunnel.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: earth composition, strata makeup</span></p><p><span style=\"font-size: 16px\">7. </span><strong><span style=\"font-size: 16px\">Excavation depth</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /ˌeks.kəˈveɪ.ʃən dɛpθ/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: độ sâu khai quật</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: the depth to which the ground is dug to create a tunnel</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: The </span><strong><span style=\"font-size: 16px\">excavation depth</span></strong><span style=\"font-size: 16px\"> for the tunnel under the bridge was 1.5 meters.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: digging depth, burrowing extent</span></p><p><span style=\"font-size: 16px\">8. </span><strong><span style=\"font-size: 16px\">Vehicle capacity</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm:</span></p><p><span style=\"font-size: 16px\"> /ˈviː.ɪ.kəl kəˈpæs.ɪ.ti/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sức chứa xe cộ</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: the number of vehicles that can pass through the tunnel simultaneously</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: Both diagrams display tunnels with significant </span><strong><span style=\"font-size: 16px\">vehicle capacity</span></strong><span style=\"font-size: 16px\">.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: traffic volume capability, car accommodation</span></p><p><span style=\"font-size: 16px\">9. </span><strong><span style=\"font-size: 16px\">Cost comparison</span></strong></p><p><span style=\"font-size: 16px\">   - Phát âm: /kɒst kəmˈpær.ɪ.sən/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: so sánh chi phí</span></p><p><span style=\"font-size: 16px\">   - Nghĩa tiếng Anh: an evaluation of the expenses related to two or more items</span></p><p><span style=\"font-size: 16px\">   - Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Ví dụ: A </span><strong><span style=\"font-size: 16px\">cost comparison</span></strong><span style=\"font-size: 16px\"> shows that the second tunnel required more investment.</span></p><p><span style=\"font-size: 16px\">   - Từ đồng nghĩa: expense analysis, financial contrast</span></p><p><span style=\"font-size: 16px\">10. </span><strong><span style=\"font-size: 16px\">Engineering feat</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˈen.dʒɪ.nɪə.rɪŋ fiːt/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: kỳ công kỹ thuật</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: a remarkable accomplishment in the field of engineering</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: Each tunnel represents an impressive </span><strong><span style=\"font-size: 16px\">engineering feat</span></strong><span style=\"font-size: 16px\"> in its own right.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: technical accomplishment, construction marvel</span></p><p><span style=\"font-size: 16px\">11. </span><strong><span style=\"font-size: 16px\">Inauguration year</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ɪˌnɔː.ɡjəˈreɪ.ʃən jɪər/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: năm khánh thành</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: the year in which a structure is officially opened for use</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: The </span><strong><span style=\"font-size: 16px\">inauguration year</span></strong><span style=\"font-size: 16px\"> of the first tunnel was 1998.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: opening year, commencement date</span></p><p><span style=\"font-size: 16px\">12. </span><strong><span style=\"font-size: 16px\">Soil stratum</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˈsɔɪl ˈstræt.əm/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: tầng đất</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: a layer of soil or rock of a particular composition</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: The </span><strong><span style=\"font-size: 16px\">soil stratum</span></strong><span style=\"font-size: 16px\"> through which the tunnel was built consisted of sand or stone and clay.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: earth layer, geological layer</span></p><p><span style=\"font-size: 16px\">13. </span><strong><span style=\"font-size: 16px\">Traffic flow</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˈtræf.ɪk floʊ/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: dòng xe cộ</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: the movement of vehicles along roads</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: The tunnels were designed to ease </span><strong><span style=\"font-size: 16px\">traffic flow</span></strong><span style=\"font-size: 16px\"> in their respective cities.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: vehicular movement, traffic circulation</span></p><p><span style=\"font-size: 16px\">14. </span><strong><span style=\"font-size: 16px\">Underwater construction</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˌʌn.dərˈwɔː.tər kənˈstrʌk.ʃən/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: xây dựng dưới nước</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: the process of building structures below the water's surface</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: The first tunnel was an example of </span><strong><span style=\"font-size: 16px\">underwater construction</span></strong><span style=\"font-size: 16px\"> due to its proximity to the body of water.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: submerged building, aquatic engineering</span></p><p><span style=\"font-size: 16px\">15. </span><strong><span style=\"font-size: 16px\">Public expenditure</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˈpʌb.lɪk ɪkˈspen.dɪ.tʃər/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: chi tiêu công</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: the spending of government funds, derived from taxation and other sources</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: </span><strong><span style=\"font-size: 16px\">Public expenditure</span></strong><span style=\"font-size: 16px\"> on the tunnel projects was in the millions of dollars.</span></p><p><span style=\"font-size: 16px\">    - Từ đ</span></p><p><span style=\"font-size: 16px\">ồng nghĩa: government spending, fiscal outlay</span></p><p><span style=\"font-size: 16px\">16. </span><strong><span style=\"font-size: 16px\">Municipal development</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /mjuːˈnɪs.ɪ.pəl dɪˈvel.əp.mənt/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự phát triển đô thị</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: the process of urban planning and growth</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: Both tunnels were crucial for the </span><strong><span style=\"font-size: 16px\">municipal development</span></strong><span style=\"font-size: 16px\"> of the cities.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: urban expansion, city growth</span></p><p><span style=\"font-size: 16px\">17. </span><strong><span style=\"font-size: 16px\">Feasibility study</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˌfiː.zəˈbɪl.ɪ.ti ˈstʌd.i/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: nghiên cứu khả thi</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: an assessment of the practicality of a proposed plan or method</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: A </span><strong><span style=\"font-size: 16px\">feasibility study</span></strong><span style=\"font-size: 16px\"> would have been conducted before the tunnels were built.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: viability analysis, practicality assessment</span></p><p><span style=\"font-size: 16px\">18. </span><strong><span style=\"font-size: 16px\">Construction methodologies</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /kənˈstrʌk.ʃən ˌmeθ.əˈdɒl.ə.dʒiz/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: phương pháp xây dựng</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: the techniques and planning applied in the building process</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: The tunnels showcase different </span><strong><span style=\"font-size: 16px\">construction methodologies</span></strong><span style=\"font-size: 16px\"> based on their designs.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: building techniques, engineering practices</span></p><p><span style=\"font-size: 16px\">19. </span><strong><span style=\"font-size: 16px\">Architectural innovation</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˌɑː.kɪˈtek.tʃər.əl ˌɪn.əˈveɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: đổi mới kiến trúc</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: new and original methods in the design and construction of buildings</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Advanced</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: Both projects required </span><strong><span style=\"font-size: 16px\">architectural innovation</span></strong><span style=\"font-size: 16px\"> to overcome environmental challenges.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: design advancement, structural creativity</span></p><p><span style=\"font-size: 16px\">20. </span><strong><span style=\"font-size: 16px\">Operational capacity</span></strong></p><p><span style=\"font-size: 16px\">    - Phát âm: /ˌɒp.əˈreɪ.ʃən.əl kəˈpæs.ɪ.ti/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: công suất hoạt động</span></p><p><span style=\"font-size: 16px\">    - Nghĩa tiếng Anh: the maximum amount that something can contain or accommodate during its use</span></p><p><span style=\"font-size: 16px\">    - Cấp độ: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Ví dụ: The </span><strong><span style=\"font-size: 16px\">operational capacity</span></strong><span style=\"font-size: 16px\"> of the second tunnel was designed to handle increased traffic density.</span></p><p><span style=\"font-size: 16px\">    - Từ đồng nghĩa: functional capability, working capacity</span></p><p></p>"
          }
        ],
        'samples': [
          {
            'id': 359,
            'questionId': 3119,
            'sampleText': '<p><span style="font-size: 16px">The illustrations provide insights into the construction and structure of two significant road tunnels located in Australian cities. The first tunnel, completed between 1986 and 1998, stretches for 2.2 kilometers and dips to a depth of 1.5 meters below the surface. It was constructed within sandy substrata and came at a cost of $555 million. Notably, the tunnel allows for two lanes of vehicular traffic, accommodating cars in both directions.</span></p><p><span style="font-size: 16px">In contrast, the second tunnel, constructed more recently between 2002 and 2006, boasts a depth of 2.5 meters beneath a foundation of stone and clay. Remarkably, the cost of this tunnel reached $1.1 billion, exactly double the expense of the earlier tunnel. This structure also facilitates two-way traffic flow; however, the depiction suggests a busier passage with diverse vehicles including a police car, an ambulance, and other service vehicles.</span></p><p><span style="font-size: 16px">Comparing the two, the latter tunnel is deeper and incurred a substantially higher construction cost, reflective of inflation or advancements in engineering and safety standards over time. Both serve as vital arteries in their respective cities, yet the escalation in expenditure suggests enhanced features or increased complexity in the later project.</span></p>',
            'bandScore': 8,
            'comment': '<p><strong><span style="font-size: 16px">1. Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">- Bài viết trình bày thông tin chi tiết về hai đường hầm, bao gồm chiều dài, độ sâu, cấu trúc địa chất, chi phí xây dựng và thời gian hoàn thành.</span></p><p><span style="font-size: 16px">- Sử dụng ngôn từ đa dạng và chính xác, cùng với cấu trúc câu phức tạp, làm nổi bật sự khác biệt giữa hai đường hầm.</span></p><p><span style="font-size: 16px">- Thông tin được sắp xếp một cách có hệ thống, dễ theo dõi, và bài viết có sự liên kết chặt chẽ giữa các ý.</span></p><p><strong><span style="font-size: 16px">2. Điểm yếu:</span></strong></p><p><span style="font-size: 16px">- Bài viết có thể mở rộng hơn nữa về sự so sánh giữa hai đường hầm, như tác động đến giao thông và cuộc sống đô thị.</span></p><p><span style="font-size: 16px">- Bài viết có phần thiên về mô tả mà chưa phân tích sâu về lý do đằng sau sự chênh lệch giá thành và cấu trúc.</span></p><p><span style="font-size: 16px">- Có thể cung cấp thêm bối cảnh hoặc thông tin về lưu lượng giao thông hoặc tầm quan trọng của các đường hầm này đối với các thành phố tương ứng.</span></p><p><strong><span style="font-size: 16px">3. Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">Bài luận đã hiệu quả trong việc cung cấp cái nhìn tổng quan về hai đường hầm ở Úc, với việc so sánh các chi tiết chính như thời gian xây dựng, chi phí và cấu trúc. Bài viết sử dụng ngôn từ mạch lạc và cấu trúc câu đa dạng, tạo ra một bản tóm tắt thông tin hữu ích và dễ hiểu. Tuy nhiên, để nâng cao chất lượng và độ sâu của bài viết, cần bổ sung thêm phân tích và thông tin bối cảnh. Tổng thể, bài viết có chất lượng tốt và phản ánh khả năng xử lý thông tin phức tạp một cách có hiệu quả.</span></p>',
            'lastActivityDate': '2024-04-29T22:26:34.21561',
            'title': 'Comparison of Australian Road Tunnels'
          },
          {
            'id': 360,
            'questionId': 3119,
            'sampleText': '<p><span style="font-size: 16px">The diagrams depict two substantial road tunnels in Australian cities, detailing their structural depths, lengths, and financial costs. The first, completed over a 12-year period ending in 1998, measures 2.2 kilometers and reaches a depth of 1.5 meters, constructed within sandy terrain at a cost of $555 million. The second tunnel, completed between 2002 and 2006, delves deeper at 2.5 meters in a stone and clay environment and is notably more expensive, with its construction totaling $1.1 billion.</span></p><p><span style="font-size: 16px">Each tunnel showcases a two-lane configuration, facilitating a two-way vehicular movement, but the second tunnel indicates a more complex traffic mix, including emergency and public service vehicles, possibly reflecting increased traffic demands or a broader urban service remit. </span></p><p><span style="font-size: 16px">The twofold increase in cost for the more recent tunnel may suggest advancements in construction technology or increased infrastructure requirements. Despite minor grammatical inconsistencies, the essay effectively outlines the key differences between the two tunnels, with a coherent progression from structural details to financial implications.</span></p>',
            'bandScore': 7,
            'comment': '<p><strong><span style="font-size: 16px">1. Điểm mạnh:</span></strong></p><p><span style="font-size: 16px">- Bài viết cung cấp một cái nhìn tổng quan rõ ràng về hai đường hầm, với thông tin cụ thể về chiều dài, độ sâu và chi phí xây dựng.</span></p><p><span style="font-size: 16px">- Sử dụng một loạt từ vựng và cấu trúc câu khác nhau, làm cho bài viết phong phú và hấp dẫn.</span></p><p><span style="font-size: 16px">- Thông tin được tổ chức logic, cho thấy sự tiến triển và mạch lạc trong cách trình bày các chi tiết từ cấu trúc đến chi phí.</span></p><p><strong><span style="font-size: 16px">2. Điểm yếu:</span></strong></p><p><span style="font-size: 16px">- Một số lỗi ngữ pháp nhỏ có thể xuất hiện, mặc dù không ảnh hưởng nghiêm trọng đến sự hiểu biết chung của bài viết.</span></p><p><span style="font-size: 16px">- Có thể thiếu sự phân tích sâu hơn về lý do tại sao chi phí của đường hầm thứ hai lại cao gấp đôi so với đường hầm đầu tiên.</span></p><p><span style="font-size: 16px">- Bài luận chưa đề cập đến tác động của hai đường hầm đối với lưu lượng giao thông hoặc phát triển đô thị.</span></p><p><strong><span style="font-size: 16px">3. Đánh giá chung:</span></strong></p><p><span style="font-size: 16px">Bài viết đã thành công trong việc mô tả các đặc điểm chính của hai đường hầm và so sánh chúng một cách có ý nghĩa. Nó thể hiện sự tiến triển logic trong việc trình bày thông tin và sử dụng tốt các nguồn từ vựng và cấu trúc câu. Tuy nhiên, để tăng cường độ chính xác và đưa ra bản phân tích đầy đủ hơn, bài viết cần tránh những sai sót nhỏ về ngữ pháp và thêm vào phân tích về nguyên nhân của sự chênh lệch về chi phí giữa hai dự án. Nhìn chung, bài luận này phản ánh khả năng xử lý thông tin tốt, cung cấp cái nhìn tổng quan hiệu quả về chủ đề được giao.</span></p>',
            'lastActivityDate': '2024-04-29T22:29:41.5245873',
            'title': 'Comparison of Australian Road Tunnels'
          },
          {
            'id': 361,
            'questionId': 3119,
            'sampleText': "<p><span style=\"font-size: 16px\"><span style=\"color: rgb(13, 13, 13);\">The provided diagrams detail two road tunnels in Australia, emphasizing their construction periods, costs, and structural characteristics. The earlier tunnel, constructed between 1986 and 1998, extends for 2.2 kilometers with a 1.5-meter depth in sandy soil, costing $555 million. The latter tunnel, built from 2002 to 2006, is embedded deeper at 2.5 meters in stone and clay and incurs a cost of $1.1 billion.</span></span></p><p><span style=\"font-size: 16px\"><span style=\"color: rgb(13, 13, 13);\">Both tunnels accommodate bidirectional traffic, but the second one appears busier, with varied types of vehicles depicted. It's double the cost of the first, suggesting inflation or advancements in construction techniques and safety features over the intervening years.</span></span></p><p><span style=\"font-size: 16px\"><span style=\"color: rgb(13, 13, 13);\">The essay presents a clear comparison of the two tunnels, demonstrating organized thought and a straightforward narrative flow. It uses a mix of simple and complex sentences with a sufficient range of vocabulary. While there are some errors, they do not significantly obstruct the essay's readability or the transmission of key information.</span></span></p>",
            'bandScore': 6,
            'comment': null,
            'lastActivityDate': '2024-04-29T22:32:32.6191002',
            'title': 'Comparison of Australian Road Tunnels'
          }
        ],
        'rubrics': [
          {
            'id': 3,
            'name': 'Task Achievement',
            'description': 'The Task Achievement criterion in IELTS Academic Writing Task 1 evaluates how well you summarize key trends, compare data, cover all task requirements, and report features accurately from visuals like graphs or charts, focusing on your ability to identify and convey the most important information.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 13,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 14,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- The content is wholly unrelated to the task.\n- Any copied rubric must be discounted.'
              },
              {
                'id': 15,
                'bandScore': 2,
                'description': 'The content barely relates to the task.'
              },
              {
                'id': 16,
                'bandScore': 3,
                'description': '- The response does not address the requirements of the task (possibly because\nof misunderstanding of the data/diagram/situation).\n- Key features/bullet points which are presented may be largely irrelevant.\n- Limited information is presented, and this may be used repetitively.'
              },
              {
                'id': 17,
                'bandScore': 4,
                'description': '- The response is an attempt to address the task.\n- Few key features have been selected.\n- The format may be inappropriate.\n- Key features/bullet points which are presented may be irrelevant, repetitive,\ninaccurate or inappropriate.'
              },
              {
                'id': 18,
                'bandScore': 5,
                'description': '- The response generally addresses the requirements of the task. The\nformat may be inappropriate in places.\n- Key features which are selected are not adequately covered.\nThe recounting of detail is mainly mechanical. There may be no data to\nsupport the description.\n- There may be a tendency to focus on details (without referring to the\nbigger picture).\n- The inclusion of irrelevant, inappropriate or inaccurate material in key\nareas detracts from the task achievement.\n- There is limited detail when extending and illustrating the main points.'
              },
              {
                'id': 19,
                'bandScore': 6,
                'description': 'The response focuses on the requirements of the task and an appropriate\nformat is used.\n- Key features which are selected are covered and adequately\nhighlighted. A relevant overview is attempted. Information is appropriately\nselected and supported using figures/data.\n- Some irrelevant, inappropriate or inaccurate information may occur in\nareas of detail or when illustrating or extending the main points.\n- Some details may be missing (or excessive) and further extension or\nillustration may be needed.'
              },
              {
                'id': 20,
                'bandScore': 7,
                'description': '- The response covers the requirements of the task.\n- The content is relevant and accurate. There may be a few omissions or lapses.\nThe format is appropriate.\n- Key features which are selected are covered and clearly\nhighlighted but could be more fully or more appropriately illustrated or\nextended.\n- It presents a clear overview, the data are appropriately\ncategorised, and main trends or differences are identified.'
              },
              {
                'id': 21,
                'bandScore': 8,
                'description': '- The response covers all the requirements of the task appropriately, relevantly\nand sufficiently.\n- Key features are skilfully selected, and clearly presented,\nhighlighted and illustrated.\n- There may be occasional omissions or lapses in content.'
              },
              {
                'id': 23,
                'bandScore': 9,
                'description': '- All the requirements of the task are fully and appropriately satisfied.\n- There may be extremely rare lapses in content.'
              }
            ]
          },
          {
            'id': 4,
            'name': 'Coherence & Cohesion',
            'description': 'The Coherence & Cohesion criterion assesses your ability to organize and link information clearly and logically in IELTS Academic Writing Task 1. It focuses on effective paragraphing, logical sequencing of ideas, and the use of cohesive devices (like linking words and pronouns) to help the reader understand the relationships between ideas.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 25,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 26,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- The writing fails to communicate any message and appears to be by a virtual non writer.'
              },
              {
                'id': 28,
                'bandScore': 2,
                'description': '- There is little relevant message, or the entire response may be off topic.\n- There is little evidence of control of organisational features.'
              },
              {
                'id': 29,
                'bandScore': 3,
                'description': '- There is no apparent logical organisation .\n- Ideas are discernible but difficult to relate to each other.\n- Minimal use of sequencers or cohesive devices. Those used do not necessarily indicate a logical relationship between ideas.\n- There is difficulty in identifying referencing.'
              },
              {
                'id': 30,
                'bandScore': 4,
                'description': '- Information and ideas are evident but not arranged coherently, and there is no clear progression within the response.\n- Relationships between ideas can be unclear and/or inadequately marked. There is some use of basic cohesive devices, which may be inaccurate or repetitive.\n- There is inaccurate use or a lack of substitution or referencing.'
              },
              {
                'id': 31,
                'bandScore': 5,
                'description': '- Organisation is evident but is not wholly logical and there may be a lack of overall progression. Nevertheless, there is a sense of underlying coherence to the response.\n- The relationship of ideas can be followed but the sentences are not fluently linked to each other.\n- There may be limited/overuse of cohesive devices with some inaccuracy.\n- The writing may be repetitive due to inadequate and/or inaccurate use of reference and substitution.'
              },
              {
                'id': 32,
                'bandScore': 6,
                'description': '- Information and ideas are generally arranged coherently and there is a clear overall progression. \n- Cohesive devices are used to some good effect but cohesion within and/or between sentences may be faulty or mechanical due to misuse, overuse or omission. \n- The use of reference and substitution may lack flexibility or clarity and result in some repetition or error'
              },
              {
                'id': 33,
                'bandScore': 7,
                'description': '- Information and ideas are logically organised and there is a clear progression throughout the response. A few lapses may occur.\n\n- A range of cohesive devices including reference and substitution is used flexibly but with some inaccuracies or some over/under use.'
              },
              {
                'id': 34,
                'bandScore': 8,
                'description': '- The message can be followed with ease. \n- Information and ideas are logically sequenced, and cohesion is well managed. \n- Occasional lapses in coherence or cohesion may occur. \n- Paragraphing is used sufficiently and appropriately.'
              },
              {
                'id': 35,
                'bandScore': 9,
                'description': '- The message can be followed effortlessly.\n- Cohesion is used in such a way that it very rarely attracts attention.\n- Any lapses in coherence or cohesion are minimal.\n- Paragraphing is skilfully managed.'
              }
            ]
          },
          {
            'id': 5,
            'name': 'Lexical Resource',
            'description': 'The Lexical Resource criterion evaluates your range of vocabulary, accuracy in word choice, and ability to use words appropriately to express precise meanings in IELTS Academic Writing Task 1. It focuses on your ability to use a variety of vocabulary to describe data, trends, and processes clearly and accurately.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 36,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English throughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 37,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- No resource is apparent, except for a few isolated words.'
              },
              {
                'id': 38,
                'bandScore': 2,
                'description': '- The resource is extremely limited with few recognisable strings, apart from memorised phrases. \n- There is no apparent control of word formation and/or spelling.'
              },
              {
                'id': 39,
                'bandScore': 3,
                'description': '- The resource is inadequate (which may be due to the response being significantly underlength).\n- Possible over dependence on input material or memorised language. \n- Control of word choice and/or spelling is very limited, and errors predominate. These errors may severely impede meaning.'
              },
              {
                'id': 40,
                'bandScore': 4,
                'description': '- The resource is limited and inadequate for or unrelated to the task. Vocabulary is basic and may be used repetitively.\n- There may be inappropriate use of lexical chunks (e. memorised phrases, formulaic language and/or language from the input material).\n- Inappropriate word choice and/or errors in word formation and/or in spelling may impede meaning.'
              },
              {
                'id': 41,
                'bandScore': 5,
                'description': '- The resource is limited but minimally adequate for the task. \n- Simple vocabulary may be used accurately but the range does not permit much variation in expression. \n- There may be frequent lapses in the appropriacy of word choice, and a lack of flexibility is apparent in frequent simplifications and/or repetitions. \n- Errors in spelling and/or word formation may be noticeable and may cause some difficulty for the reader.'
              },
              {
                'id': 42,
                'bandScore': 6,
                'description': '- The resource is generally adequate and appropriate for the task.\n- The meaning is generally clear in spite of a rather restricted range or a lack of precision in word choice.\n- If the writer is a risk taker, there will be a wider range of vocabulary used but higher degrees of inaccuracy or inappropriacy.\n- There are some errors in spelling and/or word formation, but these do not impede communication.'
              },
              {
                'id': 43,
                'bandScore': 7,
                'description': '- The resource is sufficient to allow some flexibility and precision. \n- There is some ability to use less common and/or idiomatic items. \n- An awareness of style and collocation is evident, though inappropriacies occur. \n- There are only a few errors in spelling and/or word formation, and they do not detract from overall clarity.'
              },
              {
                'id': 44,
                'bandScore': 8,
                'description': '- A wide resource is fluently and flexibly used to convey precise meanings within the scope of the task.\n- There is skilful use of uncommon and/or idiomatic items when appropriate, despite occasional inaccuracies in word choice and collocation.\n- Occasional errors in spelling and/or word formation may occur, but have minimal impact on communication.'
              },
              {
                'id': 45,
                'bandScore': 9,
                'description': '- Full flexibility and precise use are evident within the scope of the task. \n- A wide range of vocabulary is used accurately and appropriately with very natural and sophisticated control of lexical features. \n- Minor errors in spelling and word formation are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 6,
            'name': 'Grammatical Range & Accuracy',
            'description': 'The Grammatical Range & Accuracy criterion assesses your use of sentence structures and grammatical accuracy in IELTS Academic Writing Task 1. It focuses on your ability to construct a range of sentence types correctly and use grammar precisely to convey information and ideas effectively.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 46,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 49,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- No rateable language is evident.'
              },
              {
                'id': 53,
                'bandScore': 2,
                'description': 'There is little or no evidence of sentence forms (except in memorised phrases).'
              },
              {
                'id': 54,
                'bandScore': 3,
                'description': '- Sentence forms are attempted, but errors in grammar and punctuation predominate (except in memorised phrases or those taken from the input material). This prevents most meaning from coming through. \n- Length may be insufficient to provide evidence of control of sentence forms.'
              },
              {
                'id': 55,
                'bandScore': 4,
                'description': '- A very limited range of structures is used. \n- Subordinate clauses are rare and simple sentences predominate. \n- Some structures are produced accurately but grammatical errors are frequent and may impede meaning. \n- Punctuation is often faulty or inadequate.'
              },
              {
                'id': 56,
                'bandScore': 5,
                'description': '- The range of structures is limited and rather repetitive.\n- Although complex sentences are attempted, they tend to be faulty, and the greatest accuracy is achieved on simple sentences.\n- Grammatical errors may be frequent and cause some difficulty for the reader.\n- Punctuation may be faulty.'
              },
              {
                'id': 57,
                'bandScore': 6,
                'description': '- A mix of simple and complex sentence forms is used but flexibility is limited.\n- Examples of more complex structures are not marked by the same level of accuracy as in simple structures.\n- Errors in grammar and punctuation occur, but rarely impede communication'
              },
              {
                'id': 58,
                'bandScore': 7,
                'description': '- A variety of complex structures is used with some flexibility and accuracy.\n- Grammar and punctuation are generally well controlled, and error free sentences are frequent.\n- A few errors in grammar may persist, but these do not impede communication.'
              },
              {
                'id': 59,
                'bandScore': 8,
                'description': '- A wide range of structures within the scope of the task is flexibly and accurately used.\n- The majority of sentences are error free, and punctuation is well managed.\n- Occasional, non systematic errors and inappropriacies occur, but have minimal impact on communication.'
              },
              {
                'id': 60,
                'bandScore': 9,
                'description': '- A wide range of structures within the scope of the task is used with full flexibility and control.\n- Punctuation and grammar are used appropriately throughout.\n- Minor errors are extremely rare and have minimal impact on communication'
              }
            ]
          },
          {
            'id': 34,
            'name': 'Critical Errors',
            'description': 'Critical Errors',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 267,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 35,
            'name': 'Overall Score & Feedback',
            'description': 'Overall Feedback',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 225,
                'bandScore': 0,
                'description': null
              },
              {
                'id': 226,
                'bandScore': 1,
                'description': null
              },
              {
                'id': 227,
                'bandScore': 2,
                'description': null
              },
              {
                'id': 228,
                'bandScore': 3,
                'description': null
              },
              {
                'id': 229,
                'bandScore': 4,
                'description': null
              },
              {
                'id': 230,
                'bandScore': 5,
                'description': null
              },
              {
                'id': 231,
                'bandScore': 6,
                'description': null
              },
              {
                'id': 232,
                'bandScore': 7,
                'description': null
              },
              {
                'id': 233,
                'bandScore': 8,
                'description': null
              },
              {
                'id': 234,
                'bandScore': 9,
                'description': null
              }
            ]
          },
          {
            'id': 42,
            'name': 'Arguments Assessment',
            'description': 'Arguments Assessment',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 283,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 44,
            'name': 'Vocabulary',
            'description': 'Vocabulary',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 285,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 46,
            'name': 'Improved Version',
            'description': 'Improved Version',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 287,
                'bandScore': 0,
                'description': null
              }
            ]
          }
        ]
      },
      {
        'id': 3684,
        'title': 'Increased Freedom for Children',
        'section': 'Academic Writing Task 2',
        'taskId': 0,
        'test': 'IELTS',
        'testId': 0,
        'time': '40 minutes',
        'type': 'Opinion',
        'sample': true,
        'averageScore': '0.0',
        'submission': 0,
        'like': 0,
        'dislike': 0,
        'status': 'To do',
        'difficulty': 'Easy',
        'direction': 'You should spend about 40 minutes on this task.\n\nProvide reasons for your answer. Include relevant examples from your own knowledge or experience.\n\nWrite at least 250 words.',
        'userId': null,
        'createdBy': null,
        'addedDate': '0001-01-01T00:00:00',
        'lastActivityDate': '0001-01-01T00:00:00',
        'questionsPart': [
          {
            'questionId': 3684,
            'name': 'Question',
            'content': '<p><span style="font-size: 16px"><span style="color: rgb(79, 79, 101);">In many parts of the world, children are given more freedom than in the past. Is this a positive or negative development?</span></span></p>'
          },
          {
            'questionId': 3684,
            'name': 'Tip',
            'content': '<p><strong><u><span style="font-size: 18px">Ý Tưởng Phát Triển Bài</span></u></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Phát triển kỹ năng cá nhân</span></strong><span style="font-size: 16px">: Trẻ em có nhiều tự do hơn có thể tự khám phá và phát triển các kỹ năng sống quan trọng như ra quyết định độc lập và giải quyết vấn đề.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Tăng cường sáng tạo</span></strong><span style="font-size: 16px">: Khi có nhiều tự do, trẻ em có thể thử nghiệm và sáng tạo hơn trong các trò chơi và hoạt động, điều này thúc đẩy sự phát triển trí tuệ.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Sự phát triển tình cảm xã hội</span></strong><span style="font-size: 16px">: Tự do giúp trẻ em học cách tương tác với bạn bè và xây dựng mối quan hệ xã hội, từ đó phát triển kỹ năng giao tiếp và cảm xúc.</span></p><p><span style="font-size: 16px">4. </span><strong><span style="font-size: 16px">Độc lập và tự chủ</span></strong><span style="font-size: 16px">: Tự do giúp trẻ học cách quản lý thời gian và hoạt động của mình, làm tăng khả năng tự lập và tự chủ trong cuộc sống sau này.</span></p><p><span style="font-size: 16px">5. </span><strong><span style="font-size: 16px">Tự tin và quyết đoán</span></strong><span style="font-size: 16px">: Trẻ em được tự do lựa chọn và thể hiện ý kiến sẽ phát triển sự tự tin và khả năng quyết đoán.</span></p><p><span style="font-size: 16px">6. </span><strong><span style="font-size: 16px">Tiềm ẩn mất an toàn</span></strong><span style="font-size: 16px">: Tăng tự do cho trẻ em có thể gây ra những rủi ro về an toàn khi trẻ chưa đủ kỹ năng để đối phó với một số tình huống nguy hiểm.</span></p><p><span style="font-size: 16px">7. </span><strong><span style="font-size: 16px">Thách thức trong giám sát</span></strong><span style="font-size: 16px">: Việc trẻ em có nhiều tự do hơn đòi hỏi phụ huynh phải cân nhắc kỹ lưỡng giữa việc cho tự do và việc giám sát để đảm bảo an toàn cho trẻ.</span></p><p><span style="font-size: 16px">8. </span><strong><span style="font-size: 16px">Ảnh hưởng đến kỷ luật</span></strong><span style="font-size: 16px">: Một số người cho rằng trẻ em ngày nay có quá nhiều tự do có thể dẫn đến sự thiếu kỷ luật và không tôn trọng người lớn.</span></p><p><span style="font-size: 16px">9. </span><strong><span style="font-size: 16px">Sự phát triển thể chất</span></strong><span style="font-size: 16px">: Tự do tham gia vào các hoạt động ngoài trời như chơi thể thao, leo núi có thể giúp trẻ em phát triển tốt hơn về mặt thể chất.</span></p><p><span style="font-size: 16px">10. </span><strong><span style="font-size: 16px">Sự cân bằng giữa tự do và hướng dẫn</span></strong><span style="font-size: 16px">: Thảo luận về tầm quan trọng của việc cân bằng giữa việc cấp tự do và việc cung cấp sự hướng dẫn cần thiết cho trẻ em để họ phát triển một cách toàn diện và an toàn.</span></p><p><strong><u><span style="font-size: 18px">Gợi Ý Cho Cấu Trúc Bài</span></u></strong></p><p><span style="font-size: 16px">### Mở Bài</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Giới thiệu vấn đề</span></strong><span style="font-size: 16px">: Bắt đầu bằng cách nhấn mạnh sự thay đổi trong cách giáo dục và nuôi dưỡng trẻ em so với quá khứ, với việc trẻ em hiện nay được hưởng nhiều tự do hơn.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Đặt câu hỏi nghiên cứu</span></strong><span style="font-size: 16px">: Nêu câu hỏi liệu sự thay đổi này có phải là một phát triển tích cực hay tiêu cực đối với xã hội hiện đại.</span></p><p><span style="font-size: 16px">### Thân Bài</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Đoạn 1 - Lợi ích của việc tăng tự do</span></strong><span style="font-size: 16px">:</span></p><p><span style="font-size: 16px">  - Trình bày về cách tự do giúp trẻ phát triển kỹ năng cá nhân như sự độc lập, sáng tạo, và kỹ năng xã hội.</span></p><p><span style="font-size: 16px">  - Đưa ra dẫn chứng và ví dụ cụ thể về lợi ích của việc có không gian tự do để khám phá và học hỏi.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Đoạn 2 - Những thách thức và rủi ro</span></strong><span style="font-size: 16px">:</span></p><p><span style="font-size: 16px">  - Phân tích các vấn đề tiềm ẩn khi trẻ em được quá nhiều tự do, như sự thiếu giám sát có thể dẫn đến các hành vi nguy hiểm hoặc thiếu kỷ luật.</span></p><p><span style="font-size: 16px">  - Thảo luận về sự cần thiết của sự cân bằng giữa tự do và hướng dẫn, cũng như vai trò của cha mẹ và người giám hộ trong việc đảm bảo an toàn cho trẻ.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Đoạn 3 - Đề xuất hướng đi và giải pháp</span></strong><span style="font-size: 16px">:</span></p><p><span style="font-size: 16px">  - Đề xuất các biện pháp để cải thiện sự cân bằng giữa cấp tự do và cung cấp sự hướng dẫn cần thiết, như phương pháp giáo dục hiện đại hơn, hoạt động ngoại khóa, và chương trình phát triển kỹ năng sống.</span></p><p><span style="font-size: 16px">  - Nêu bật tầm quan trọng của việc giáo dục cộng đồng và nâng cao nhận thức cho cha mẹ về cách thức nuôi dạy con cái phù hợp.</span></p><p><span style="font-size: 16px">### Kết Luận</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Tổng kết ý chính</span></strong><span style="font-size: 16px">: Nhắc lại các điểm đã được thảo luận về lợi ích và những thách thức của việc trẻ em có nhiều tự do hơn.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Phát biểu ý kiến cá nhân</span></strong><span style="font-size: 16px">: Đưa ra quan điểm cá nhân về việc liệu sự tăng tự do cho trẻ em có phải là một phát triển tích cực hay không, dựa trên bằng chứng và phân tích đã nêu.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Khuyến khích hành động</span></strong><span style="font-size: 16px">: Kêu gọi các bên liên quan, bao gồm gia đình, nhà trường, và chính quyền, hợp tác để đạt được sự cân bằng tốt nhất giữa tự do và an toàn cho trẻ em.</span></p>'
          },
          {
            'questionId': 3684,
            'name': 'Vocabulary',
            'content': "<p><span style=\"font-size: 16px\">1. </span><strong><span style=\"font-size: 16px\">Autonomy</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ɔːˈtɒnəmi/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự tự chủ</span></p><p><span style=\"font-size: 16px\">   - English Definition: The right or condition of self-government, freedom to make your own decisions.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Greater </span><strong><span style=\"font-size: 16px\">autonomy</span></strong><span style=\"font-size: 16px\"> allows children to develop confidence and self-reliance.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: independence, self-government</span></p><p><span style=\"font-size: 16px\">2. </span><strong><span style=\"font-size: 16px\">Cognitive development</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /kɒɡˈnɪtɪv dɪˈveləpmənt/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự phát triển nhận thức</span></p><p><span style=\"font-size: 16px\">   - English Definition: The construction of thought processes, including remembering, problem-solving, and decision-making, from childhood through adolescence to adulthood.</span></p><p><span style=\"font-size: 16px\">   - Level: Advanced</span></p><p><span style=\"font-size: 16px\">   - Example: Increased freedom can accelerate </span><strong><span style=\"font-size: 16px\">cognitive development</span></strong><span style=\"font-size: 16px\"> in children.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: mental advancement, intellectual growth</span></p><p><span style=\"font-size: 16px\">3. </span><strong><span style=\"font-size: 16px\">Social skills</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˈsəʊʃəl skɪlz/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: kỹ năng xã hội</span></p><p><span style=\"font-size: 16px\">   - English Definition: Skills facilitating interaction and communication with others.</span></p><p><span style=\"font-size: 16px\">   - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Developing </span><strong><span style=\"font-size: 16px\">social skills</span></strong><span style=\"font-size: 16px\"> is essential for children's overall well-being.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: interpersonal skills, communication skills</span></p><p><span style=\"font-size: 16px\">4. </span><strong><span style=\"font-size: 16px\">Risk assessment</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /rɪsk əˈsesmənt/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: đánh giá rủi ro</span></p><p><span style=\"font-size: 16px\">   - English Definition: The identification, evaluation, and estimation of the levels of risk involved in a situation.</span></p><p><span style=\"font-size: 16px\">   - Level: Advanced</span></p><p><span style=\"font-size: 16px\">   - Example: Allowing children freedom helps them improve their </span><strong><span style=\"font-size: 16px\">risk assessment</span></strong><span style=\"font-size: 16px\"> capabilities.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: hazard evaluation, risk analysis</span></p><p><span style=\"font-size: 16px\">5. </span><strong><span style=\"font-size: 16px\">Overprotection</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˌəʊ.və.prəˈtek.ʃən/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự bảo vệ thái quá</span></p><p><span style=\"font-size: 16px\">   - English Definition: Protection of someone excessively, especially in a way that limits their freedom.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: </span><strong><span style=\"font-size: 16px\">Overprotection</span></strong><span style=\"font-size: 16px\"> can hinder children's ability to manage difficulties on their own.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: excessive guarding, overcare</span></p><p><span style=\"font-size: 16px\">6. </span><strong><span style=\"font-size: 16px\">Empowerment</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ɪmˈpaʊəmənt/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: trao quyền</span></p><p><span style=\"font-size: 16px\">   - English Definition: The process of becoming stronger and more confident, especially in controlling one's life.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Child </span><strong><span style=\"font-size: 16px\">empowerment</span></strong><span style=\"font-size: 16px\"> is crucial for fostering independence.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: authorization, enablement</span></p><p><span style=\"font-size: 16px\">7. </span><strong><span style=\"font-size: 16px\">Resilience</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /rɪˈzɪliəns/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự kiên cường</span></p><p><span style=\"font-size: 16px\">   - English Definition: The capacity to recover quickly from difficulties; toughness.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Children's </span><strong><span style=\"font-size: 16px\">resilience</span></strong><span style=\"font-size: 16px\"> can be built through facing and overcoming challenges.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: toughness, strength</span></p><p><span style=\"font-size: 16px\">8. </span><strong><span style=\"font-size: 16px\">Neglect</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /nɪˈɡlekt/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự sao nhãng</span></p><p><span style=\"font-size: 16px\">   - English Definition: The state or fact of being uncared for.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Excessive freedom could lead to </span><strong><span style=\"font-size: 16px\">neglect</span></strong><span style=\"font-size: 16px\">, affecting the child's development.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: disregard, abandonment</span></p><p><span style=\"font-size: 16px\">9. </span><strong><span style=\"font-size: 16px\">Boundaries</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˈbaʊndəriz/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: giới hạn</span></p><p><span style=\"font-size: 16px\">   - English Definition: Limits of acceptable behavior or a dividing line.</span></p><p><span style=\"font-size: 16px\">   - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Setting clear </span><strong><span style=\"font-size: 16px\">boundaries</span></strong><span style=\"font-size: 16px\"> is essential even when giving children freedom.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: limits, borders</span></p><p><span style=\"font-size: 16px\">10. </span><strong><span style=\"font-size: 16px\">Supervision</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˌsuː.pəˈvɪʒ.ən/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự giám sát</span></p><p><span style=\"font-size: 16px\">    - English Definition: The action of supervising someone or something.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Adequate </span><strong><span style=\"font-size: 16px\">supervision</span></strong><span style=\"font-size: 16px\"> ensures that children's freedom does not compromise their safety.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: oversight, monitoring</span></p><p><span style=\"font-size: 16px\">11. </span><strong><span style=\"font-size: 16px\">Independence</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation</span></p><p><span style=\"font-size: 16px\">: /ˌɪn.dɪˈpen.dəns/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự độc lập</span></p><p><span style=\"font-size: 16px\">    - English Definition: The fact or state of being independent.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Early experiences of </span><strong><span style=\"font-size: 16px\">independence</span></strong><span style=\"font-size: 16px\"> prepare children for adult responsibilities.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: self-sufficiency, autonomy</span></p><p><span style=\"font-size: 16px\">12. </span><strong><span style=\"font-size: 16px\">Maturity</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /məˈtʃʊər.ɪ.ti/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự trưởng thành</span></p><p><span style=\"font-size: 16px\">    - English Definition: The state, fact, or period of being mature.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Maturity</span></strong><span style=\"font-size: 16px\"> in children can be developed through responsible freedom.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: adulthood, full development</span></p><p><span style=\"font-size: 16px\">13. </span><strong><span style=\"font-size: 16px\">Exposure</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ɪkˈspəʊ.ʒər/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự tiếp xúc</span></p><p><span style=\"font-size: 16px\">    - English Definition: The state of being exposed to contact with something.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Exposure</span></strong><span style=\"font-size: 16px\"> to diverse experiences can broaden children's horizons.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: contact, experience</span></p><p><span style=\"font-size: 16px\">14. </span><strong><span style=\"font-size: 16px\">Discipline</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈdɪs.ɪ.plɪn/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: kỷ luật</span></p><p><span style=\"font-size: 16px\">    - English Definition: The practice of training people to obey rules or a code of behavior.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Discipline</span></strong><span style=\"font-size: 16px\"> is essential in balancing freedom with responsibilities.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: control, regulation</span></p><p><span style=\"font-size: 16px\">15. </span><strong><span style=\"font-size: 16px\">Self-regulation</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˌselfˈreg.jʊˈleɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: tự điều chỉnh</span></p><p><span style=\"font-size: 16px\">    - English Definition: The ability to monitor and control our own behavior, emotions, or thoughts, altering them in accordance with the demands of the situation.</span></p><p><span style=\"font-size: 16px\">    - Level: Advanced</span></p><p><span style=\"font-size: 16px\">    - Example: Increased freedom helps children develop </span><strong><span style=\"font-size: 16px\">self-regulation</span></strong><span style=\"font-size: 16px\"> skills.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: self-control, self-management</span></p><p><span style=\"font-size: 16px\">16. </span><strong><span style=\"font-size: 16px\">Vulnerability</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˌvʌl.nəˈræb.ɪ.lə.ti/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: tính dễ bị tổn thương</span></p><p><span style=\"font-size: 16px\">    - English Definition: The quality or state of being exposed to the possibility of being attacked or harmed.</span></p><p><span style=\"font-size: 16px\">    - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Parents must consider children's </span><strong><span style=\"font-size: 16px\">vulnerability</span></strong><span style=\"font-size: 16px\"> when granting them freedoms.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: susceptibility, openness</span></p><p><span style=\"font-size: 16px\">17. </span><strong><span style=\"font-size: 16px\">Empathy</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈem.pə.θi/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự đồng cảm</span></p><p><span style=\"font-size: 16px\">    - English Definition: The ability to understand and share the feelings of another.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Developing </span><strong><span style=\"font-size: 16px\">empathy</span></strong><span style=\"font-size: 16px\"> is crucial for children's emotional and social growth.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: understanding, compassion</span></p><p><span style=\"font-size: 16px\">18. </span><strong><span style=\"font-size: 16px\">Consequences</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈkɒn.sɪ.kwənsɪz/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: hậu quả</span></p><p><span style=\"font-size: 16px\">    - English Definition: A result or effect, typically one that is unwelcome or unpleasant.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Children must learn about the </span><strong><span style=\"font-size: 16px\">consequences</span></strong><span style=\"font-size: 16px\"> of their actions.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: results, outcomes</span></p><p><span style=\"font-size: 16px\">19. </span><strong><span style=\"font-size: 16px\">Guidance</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈɡaɪ.dəns/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự hướng dẫn</span></p><p><span style=\"font-size: 16px\">    - English Definition: Advice or information aimed at resolving a problem or difficulty.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Effective </span><strong><span style=\"font-size: 16px\">guidance</span></strong><span style=\"font-size: 16px\"> from adults helps children navigate their freedoms safely.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: direction, advice</span></p><p><span style=\"font-size: 16px\">20. </span><strong><span style=\"font-size: 16px\">Personal growth</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈpɜː.sə.nəl grəʊθ/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: sự phát triển cá nhân</span></p><p><span style=\"font-size: 16px\">    - English Definition: The process of improving oneself through such activities as enhancing employment skills, increasing consciousness, or building wealth.</span></p><p><span style=\"font-size: 16px\">    - Level: Advanced</span></p><p><span style=\"font-size: 16px\">    - Example: Freedom plays a significant role in </span><strong><span style=\"font-size: 16px\">personal growth</span></strong><span style=\"font-size: 16px\"> during childhood.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: self-improvement, development</span></p><p></p>"
          }
        ],
        'samples': [
          {
            'id': 400,
            'questionId': 3684,
            'sampleText': '<p><span style="font-size: 16px">In recent years, there has been a noticeable shift towards granting children more freedom in various cultures around the world. This trend represents a positive development as it fosters independence and decision-making skills in young people, which are crucial attributes for personal and professional success in adulthood.</span></p><p><span style="font-size: 16px">One of the principal benefits of increased freedom is that it helps children develop a strong sense of autonomy. Autonomy is integral to building self-confidence and self-esteem, as children learn to trust their capabilities and judgment. For instance, when children choose their extracurricular activities or manage their homework schedules, they learn to set priorities and understand the consequences of their decisions, preparing them for the complexities of adult life.</span></p><p><span style="font-size: 16px">Moreover, increased freedom enhances problem-solving skills. As children navigate challenges independently, they learn valuable lessons in resilience and adaptability. This kind of experiential learning is often more impactful than protective or overly directive parenting styles, which can stifle personal growth and inhibit creativity.</span></p><p><span style="font-size: 16px">However, this development must be balanced with appropriate parental guidance. While freedom is essential, it should not equate to a lack of supervision or support. Parents and educators should strive to maintain a balance where children feel empowered to make decisions, but also know that guidance is available when needed. This approach ensures that the freedom given is constructive and safe.</span></p><p><span style="font-size: 16px">In conclusion, the trend of allowing children more freedom is a positive development that prepares them for future challenges by fostering independence, enhancing problem-solving skills, and building resilience. As long as this freedom is coupled with responsible guidance, it can lead to healthier, more capable, and more adaptable adults. This balance is crucial for nurturing the full potential of the next generation.</span></p>',
            'bandScore': 8,
            'comment': null,
            'lastActivityDate': '2024-06-17T22:26:10.9620808',
            'title': 'Increased Freedom for Children'
          }
        ],
        'rubrics': [
          {
            'id': 7,
            'name': 'Task Response',
            'description': 'The Task Response criterion in IELTS Academic Writing Task 2 assesses how well you develop and support your ideas in response to the question. It focuses on your ability to present a clear, relevant position, fully extend and support your main ideas, and answer all parts of the task prompt comprehensively.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 61,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 62,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1\n- The content is wholly unrelated to the prompt\n- Any copied rubric must be discounted'
              },
              {
                'id': 63,
                'bandScore': 2,
                'description': '- The content is barely related to the prompt.\n- No position can be identified.\n- There may be glimpses of one or two ideas without development.'
              },
              {
                'id': 64,
                'bandScore': 3,
                'description': '- No part of the prompt is adequately addressed, or the prompt has been misunderstood.\n- No relevant position can be identified, and/or there is little direct response to the question/s.\n- There are few ideas, and these may be irrelevant or insufficiently developed.'
              },
              {
                'id': 65,
                'bandScore': 4,
                'description': '- The prompt is tackled in a minimal way, or the answer is tangential, possibly due to some misunderstanding of the prompt.\n- The format may be inappropriate.\n- A position is discernible, but the reader has to read carefully to find it.\n- Main ideas are difficult to identify and such ideas that are identifiable may lack relevance, clarity and/or support.\n- Large parts of the response may be repetitive.'
              },
              {
                'id': 66,
                'bandScore': 5,
                'description': '- The main parts of the prompt are incompletely addressed. The format may be inappropriate in places.\n- The writer expresses a position, but the development is not always clear.\n- Some main ideas are put forward, but they are limited and are not sufficiently developed and/or there may be irrelevant detail.\n- There may be some repetition.'
              },
              {
                'id': 67,
                'bandScore': 6,
                'description': '- The main parts of the prompt are addressed (though some may be more fully covered than others). An appropriate format is used. \n- A position is presented that is directly relevant to the prompt, although the conclusions drawn may be unclear, unjustified or repetitive.\n- Main ideas are relevant, but some may be insufficiently developed or may lack clarity, while some supporting arguments and evidence may be less relevant or inadequate.'
              },
              {
                'id': 68,
                'bandScore': 7,
                'description': '- The main parts of the prompt are appropriately addressed.\n- A clear and developed position is presented.\n- Main ideas are extended and supported but there may be a tendency to over generalise or there may be a lack of focus and precision in supporting ideas/material.'
              },
              {
                'id': 69,
                'bandScore': 8,
                'description': '- The prompt is appropriately and sufficiently addressed.\n- A clear and well developed position is presented in response to the question/s.\n- Ideas are relevant, well extended and supported.\n- There may be occasional omissions or lapses in content.'
              },
              {
                'id': 70,
                'bandScore': 9,
                'description': '- The prompt is appropriately addressed and explored in depth.\n- A clear and fully developed position is presented which directly answers the question/s.\n- Ideas are relevant, fully extended and well supported.\n- Any lapses in content or support are extremely rare.'
              }
            ]
          },
          {
            'id': 8,
            'name': 'Coherence & Cohesion',
            'description': 'The Coherence & Cohesion criterion in IELTS Academic Writing Task 2 evaluates your ability to organize ideas logically and connect them smoothly. It focuses on clear overall structure, logical sequencing of paragraphs and ideas, and the effective use of cohesive devices (such as linking words and pronouns) to guide the reader through your argument or narrative seamlessly.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 71,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 72,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- The writing fails to communicate any message and appears to be by a virtual non writer.'
              },
              {
                'id': 73,
                'bandScore': 2,
                'description': '- There is little relevant message, or the entire response may be off topic.\n- There is little evidence of control of organisational features.'
              },
              {
                'id': 74,
                'bandScore': 3,
                'description': '- There is no apparent logical organisation . Ideas are discernible but difficult to relate to each other.\n- There is minimal use of sequencers or cohesive devices. Those used do not necessarily indicate a logical relationship between ideas.\n- There is difficulty in identifying referencing.\n- Any attempts at paragraphing are unhelpful.'
              },
              {
                'id': 75,
                'bandScore': 4,
                'description': '- Information and ideas are evident but not arranged coherently and there is no clear progression within the response.\n- Relationships between ideas can be unclear and/or inadequately marked. There is some use of basic cohesive devices, which may be inaccurate or repetitive.\n- There is inaccurate use or a lack of substitution or\nreferencing.\n- There may be no paragraphing and/or no clear main topic within paragraphs.'
              },
              {
                'id': 76,
                'bandScore': 5,
                'description': '- Organisation is evident but is not wholly logical and there may be a lack of overall progression.Nevertheless, there is a sense of underlying coherence to the response.\n- The relationship of ideas can be followed but the sentences are not fluently linked to each other.\n- There may be limited/overuse of cohesive devices with some inaccuracy.\n- The writing may be repetitive due to inadequate and/or inaccurate use of reference and substitution.\n- Paragraphing may be inadequate or missing.'
              },
              {
                'id': 77,
                'bandScore': 6,
                'description': '- Information and ideas are generally arranged coherently and there is a clear overall progression.\n- Cohesive devices are used to some good effect but cohesion within and/or between sentences may be faulty or mechanical due to misuse, overuse or omission.\n- The use of reference and substitution may lack flexibility or clarity and result in some repetition or error.\n- Paragraphing may not always be logical and/or the central topic may not always be clear.'
              },
              {
                'id': 78,
                'bandScore': 7,
                'description': '- Information and ideas are logically organised, and there is a clear progression throughout the response. (A few lapses may occur, but these are minor.)\n- A range of cohesive devices including reference and substitution is used flexibly but with some inaccuracies or some over/under use.\n- Paragraphing is generally used effectively to support overall coherence, and the sequencing of ideas within a paragraph is generally logical.'
              },
              {
                'id': 79,
                'bandScore': 8,
                'description': '- The message can be followed with ease.\n- Information and ideas are logically sequenced, and cohesion is well managed.\n- Occasional lapses in coherence and cohesion may occur.\n- Paragraphing is used sufficiently and appropriately.'
              },
              {
                'id': 80,
                'bandScore': 9,
                'description': '- The message can be followed effortlessly.\n- Cohesion is used in such a way that it very rarely attracts attention.\n- Any lapses in coherence or cohesion are minimal.\n- Paragraphing is skilfully managed.'
              }
            ]
          },
          {
            'id': 9,
            'name': 'Lexical Resource',
            'description': 'The Lexical Resource criterion in IELTS Academic Writing Task 2 measures your range of vocabulary, the precision of your word choices, and your ability to use words appropriately for various contexts. It assesses your skill in using vocabulary flexibly and accurately to express ideas and opinions, including the effective use of synonyms and collocations, without errors that hinder communication.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 81,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 82,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- No resource is apparent, except for a few isolated words.'
              },
              {
                'id': 83,
                'bandScore': 2,
                'description': '- The resource is extremely limited with few recognisable strings, apart from memorised phrases.\n- There is no apparent control of word formation and/or spelling.'
              },
              {
                'id': 84,
                'bandScore': 3,
                'description': '- The resource is inadequate (which may be due to the response being significantly underlength). Possible over-dependence on input material or memorised language.\n- Control of word choice and/or spelling is very limited, and errors predominate. These errors may severely impede meaning.'
              },
              {
                'id': 85,
                'bandScore': 4,
                'description': '- The resource is limited and inadequate for or unrelated to the task. Vocabulary is basic and may be used repetitively.\n- There may be inappropriate use of lexical chunks (e.g. memorised phrases, formulaic language and/or language from the input material). \n- Inappropriate word choice and/or errors in word formation and/or in spelling may impede meaning'
              },
              {
                'id': 86,
                'bandScore': 5,
                'description': '- The resource is limited but minimally adequate for the task.\n- Simple vocabulary may be used accurately but the range does not permit much variation in expression. \n- There may be frequent lapses in the appropriacy of word choice and a lack of flexibility is apparent in frequent simplifications and/or repetitions. \n- Errors in spelling and/or word formation may be noticeable and may cause some difficulty for the reader.'
              },
              {
                'id': 87,
                'bandScore': 6,
                'description': '- The resource is generally adequate and appropriate for the task.\n- The meaning is generally clear in spite of a rather restricted range or a lack of precision in word choice.\n- If the writer is a risk-taker, there will be a wider range of vocabulary used but higher degrees of inaccuracy or inappropriacy.\n- There are some errors in spelling and/or word formation, but these do not impede communication.'
              },
              {
                'id': 88,
                'bandScore': 7,
                'description': '- The resource is sufficient to allow some flexibility and precision.\n- There is some ability to use less common and/or idiomatic items.\n- An awareness of style and collocation is evident, though inappropriacies occur.\n- There are only a few errors in spelling and/or word formation and they do not detract from overall clarity.'
              },
              {
                'id': 89,
                'bandScore': 8,
                'description': '- A wide resource is fluently and flexibly used to convey precise meanings. \n- There is skilful use of uncommon and/or idiomatic items when appropriate, despite occasional inaccuracies in word choice and collocation.\n- Occasional errors in spelling and/or word formation may occur, but have minimal impact on communication.'
              },
              {
                'id': 90,
                'bandScore': 9,
                'description': '- Full flexibility and precise use are widely evident.\n- A wide range of vocabulary is used accurately and appropriately with very natural and sophisticated control of lexical features.\n- Minor errors in spelling and word formation are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 10,
            'name': 'Grammatical Range & Accuracy',
            'description': 'The Grammatical Range & Accuracy criterion in IELTS Academic Writing Task 2 evaluates your ability to use a wide range of grammar structures accurately. It focuses on your capacity to construct complex sentences effectively, use punctuation correctly, and apply grammatical forms with flexibility to clearly express detailed reasoning and arguments without making errors that obscure meaning.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 91,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 92,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- No rateable language is evident.'
              },
              {
                'id': 93,
                'bandScore': 2,
                'description': 'There is little or no evidence of sentence forms (except in memorised phrases).'
              },
              {
                'id': 94,
                'bandScore': 3,
                'description': '- Sentence forms are attempted, but errors in grammar and punctuation predominate (except in memorised phrases or those taken from the input material).\n- This prevents most meaning from coming through. Length may be insufficient to provide evidence of control of sentence forms.'
              },
              {
                'id': 95,
                'bandScore': 4,
                'description': 'A very limited range of structures is used.\n- Subordinate clauses are rare and simple sentences predominate.\n- Some structures are produced accurately but grammatical errors are frequent and may impede meaning.\n- Punctuation is often faulty or inadequate.'
              },
              {
                'id': 96,
                'bandScore': 5,
                'description': '- The range of structures is limited and rather repetitive.\n- Although complex sentences are attempted, they tend to be faulty, and the greatest accuracy is achieved on simple sentences.\n- Grammatical errors may be frequent and cause some difficulty for the reader.\n- Punctuation may be faulty.'
              },
              {
                'id': 97,
                'bandScore': 6,
                'description': '- A mix of simple and complex sentence forms is used but flexibility is limited.\n- Examples of more complex structures are not marked by the same level of accuracy as in simple structures.\n- Errors in grammar and punctuation occur, but rarely impede communication.'
              },
              {
                'id': 98,
                'bandScore': 7,
                'description': '- A variety of complex structures is used with some flexibility and accuracy.\n- Grammar and punctuation are generally well controlled, and error-free sentences are frequent.\n- A few errors in grammar may persist, but these do not impede communication.'
              },
              {
                'id': 99,
                'bandScore': 8,
                'description': '- A wide range of structures is flexibly and accurately used.\n- The majority of sentences are error-free, and punctuation is well managed.\n- Occasional, non-systematic errors and inappropriacies occur, but have minimal impact on communication.'
              },
              {
                'id': 100,
                'bandScore': 9,
                'description': '- A wide range of structures is used with full flexibility and control.\n- Punctuation and grammar are used appropriately throughout.\n- Minor errors are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 36,
            'name': 'Critical Errors',
            'description': 'Critical Errors',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 268,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 37,
            'name': 'Overall Score & Feedback',
            'description': 'Overall Feedback',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 235,
                'bandScore': 0,
                'description': null
              },
              {
                'id': 236,
                'bandScore': 1,
                'description': null
              },
              {
                'id': 237,
                'bandScore': 2,
                'description': null
              },
              {
                'id': 238,
                'bandScore': 3,
                'description': null
              },
              {
                'id': 239,
                'bandScore': 4,
                'description': null
              },
              {
                'id': 240,
                'bandScore': 5,
                'description': null
              },
              {
                'id': 241,
                'bandScore': 6,
                'description': null
              },
              {
                'id': 242,
                'bandScore': 7,
                'description': null
              },
              {
                'id': 243,
                'bandScore': 8,
                'description': null
              },
              {
                'id': 244,
                'bandScore': 9,
                'description': null
              }
            ]
          },
          {
            'id': 43,
            'name': 'Arguments Assessment',
            'description': 'Arguments Assessment',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 284,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 45,
            'name': 'Vocabulary',
            'description': 'Vocabulary',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 286,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 47,
            'name': 'Improved Version',
            'description': 'Improved Version',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 288,
                'bandScore': 0,
                'description': null
              }
            ]
          }
        ]
      },
      {
        'id': 3685,
        'title': 'Overconsumption of Natural Resources',
        'section': 'Academic Writing Task 2',
        'taskId': 0,
        'test': 'IELTS',
        'testId': 0,
        'time': '40 minutes',
        'type': 'Explanation',
        'sample': true,
        'averageScore': '0.0',
        'submission': 0,
        'like': 0,
        'dislike': 0,
        'status': 'To do',
        'difficulty': 'Hard',
        'direction': 'You should spend about 40 minutes on this task.\n\nProvide reasons for your answer. Include relevant examples from your own knowledge or experience.\n\nWrite at least 250 words.',
        'userId': null,
        'createdBy': null,
        'addedDate': '0001-01-01T00:00:00',
        'lastActivityDate': '0001-01-01T00:00:00',
        'questionsPart': [
          {
            'questionId': 3685,
            'name': 'Question',
            'content': '<p><span style="font-size: 16px"><span style="color: rgb(79, 79, 101);">The natural resources such as oil, forests and fresh water are being consumed at an alarming rate. What problems does it cause? How can we solve these problems?</span></span></p>'
          },
          {
            'questionId': 3685,
            'name': 'Tip',
            'content': '<p><strong><u><span style="font-size: 18px">Ý Tưởng Phát Triển Bài</span></u></strong></p><p><span style="font-size: 16px">1. </span><strong><span style="font-size: 16px">Cạn kiệt tài nguyên</span></strong><span style="font-size: 16px">: Phân tích cách việc sử dụng quá mức tài nguyên như dầu mỏ, rừng và nước ngọt dẫn đến nguy cơ cạn kiệt, ảnh hưởng lâu dài tới khả năng tiếp cận tài nguyên của các thế hệ tương lai.</span></p><p><span style="font-size: 16px">2. </span><strong><span style="font-size: 16px">Suy giảm đa dạng sinh học</span></strong><span style="font-size: 16px">: Trình bày về tác động tiêu cực của việc khai thác rừng quá mức đến hệ sinh thái, làm giảm đa dạng sinh học và phá vỡ chuỗi thức ăn.</span></p><p><span style="font-size: 16px">3. </span><strong><span style="font-size: 16px">Biến đổi khí hậu</span></strong><span style="font-size: 16px">: Giải thích mối liên hệ giữa khai thác nhiên liệu hóa thạch và sự gia tăng nồng độ khí nhà kính, dẫn đến biến đổi khí hậu và các hiện tượng thời tiết cực đoan.</span></p><p><span style="font-size: 16px">4. </span><strong><span style="font-size: 16px">Ô nhiễm môi trường</span></strong><span style="font-size: 16px">: Bàn luận về việc sử dụng các nguồn tài nguyên một cách không bền vững, dẫn đến ô nhiễm không khí, nước và đất, ảnh hưởng xấu đến sức khỏe con người và môi trường sống.</span></p><p><span style="font-size: 16px">5. </span><strong><span style="font-size: 16px">Xung đột và căng thẳng</span></strong><span style="font-size: 16px">: Phân tích cách thiếu hụt tài nguyên có thể dẫn đến xung đột giữa các quốc gia hoặc trong nội bộ một quốc gia, khi các nhóm khác nhau tranh giành tài nguyên hạn chế.</span></p><p><span style="font-size: 16px">6. </span><strong><span style="font-size: 16px">Giải pháp tái chế và tái sử dụng</span></strong><span style="font-size: 16px">: Đề xuất việc thực hiện các chương trình tái chế và tái sử dụng rộng rãi hơn để giảm bớt áp lực lên các nguồn tài nguyên tự nhiên.</span></p><p><span style="font-size: 16px">7. </span><strong><span style="font-size: 16px">Phát triển và áp dụng công nghệ xanh</span></strong><span style="font-size: 16px">: Khuyến khích sử dụng công nghệ tiên tiến để khai thác và sử dụng tài nguyên một cách hiệu quả hơn, giảm thiểu tác động tiêu cực đến môi trường.</span></p><p><span style="font-size: 16px">8. </span><strong><span style="font-size: 16px">Chính sách quản lý tài nguyên bền vững</span></strong><span style="font-size: 16px">: Thảo luận về tầm quan trọng của việc chính phủ đưa ra các chính sách nhằm hạn chế khai thác tài nguyên và thúc đẩy việc sử dụng bền vững.</span></p><p><span style="font-size: 16px">9. </span><strong><span style="font-size: 16px">Nâng cao nhận thức cộng đồng</span></strong><span style="font-size: 16px">: Tạo ra các chiến dịch giáo dục để nâng cao nhận thức của công chúng về tầm quan trọng của việc bảo vệ tài nguyên thiên nhiên và khuyến khích hành vi tiêu dùng có trách nhiệm.</span></p><p><span style="font-size: 16px">10. </span><strong><span style="font-size: 16px">Hợp tác quốc tế</span></strong><span style="font-size: 16px">: Nhấn mạnh tầm quan trọng của việc hợp tác quốc tế trong việc quản lý và bảo vệ tài nguyên toàn cầu, đặc biệt là các nguồn tài nguyên chung như đại dương và không khí.</span></p><p><strong><u><span style="font-size: 18px">Gợi Ý Cho Cấu Trúc Bài</span></u></strong></p><p><span style="font-size: 16px">### Mở Bài</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Giới thiệu chủ đề</span></strong><span style="font-size: 16px">: Bắt đầu bằng cách nêu rõ sự gia tăng trong việc tiêu thụ tài nguyên thiên nhiên như dầu mỏ, rừng và nước ngọt trên toàn cầu và mô tả tình trạng này là "đáng báo động."</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Mục đích của bài viết</span></strong><span style="font-size: 16px">: Giới thiệu rằng bài viết sẽ khám phá các vấn đề do việc tiêu thụ tài nguyên quá mức gây ra và đề xuất các giải pháp để giải quyết những vấn đề này.</span></p><p><span style="font-size: 16px">### Thân Bài</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Đoạn 1 - Vấn đề gây ra</span></strong><span style="font-size: 16px">:</span></p><p><span style="font-size: 16px">  - Trình bày các vấn đề môi trường như suy giảm đa dạng sinh học, ô nhiễm môi trường và biến đổi khí hậu.</span></p><p><span style="font-size: 16px">  - Đưa ra ví dụ cụ thể về những hậu quả của việc khai thác tài nguyên quá mức, như sự cạn kiệt nguồn nước tại các vùng khô hạn.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Đoạn 2 - Giải pháp đề xuất</span></strong><span style="font-size: 16px">:</span></p><p><span style="font-size: 16px">  - Phân tích các giải pháp như tái chế, sử dụng công nghệ xanh và áp dụng chính sách quản lý tài nguyên bền vững từ phía chính phủ.</span></p><p><span style="font-size: 16px">  - Giải thích cách thực hiện các giải pháp này và lợi ích mà chúng mang lại, không chỉ cho môi trường mà còn cho kinh tế và xã hội.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Đoạn 3 - Vai trò của cá nhân và cộng đồng</span></strong><span style="font-size: 16px">:</span></p><p><span style="font-size: 16px">  - Thảo luận về tầm quan trọng của việc nâng cao nhận thức và thay đổi hành vi tiêu dùng của cá nhân và cộng đồng.</span></p><p><span style="font-size: 16px">  - Kêu gọi hành động và sự tham gia của mọi người trong các chương trình bảo vệ môi trường và tiết kiệm tài nguyên.</span></p><p><span style="font-size: 16px">### Kết Luận</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Tóm tắt các điểm chính</span></strong><span style="font-size: 16px">: Nhắc lại các vấn đề chính do tiêu thụ tài nguyên thiên nhiên quá mức gây ra và các giải pháp đã được đề xuất.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Nhấn mạnh tầm quan trọng của việc giải quyết vấn đề</span></strong><span style="font-size: 16px">: Bày tỏ quan điểm cá nhân về tầm quan trọng của việc mỗi cá nhân và xã hội cần có trách nhiệm trong việc bảo vệ tài nguyên thiên nhiên.</span></p><p><span style="font-size: 16px">- </span><strong><span style="font-size: 16px">Kêu gọi hành động</span></strong><span style="font-size: 16px">: Kết thúc bài viết bằng một lời kêu gọi mạnh mẽ đối với tất cả các bên liên quan để hành động cụ thể, đóng góp vào mục tiêu phát triển bền vững.</span></p><p></p>'
          },
          {
            'questionId': 3685,
            'name': 'Vocabulary',
            'content': "<p><strong><u><span style=\"font-size: 18px\">Từ Vựng Hữu Dụng</span></u></strong></p><p><span style=\"font-size: 16px\">1. </span><strong><span style=\"font-size: 16px\">Unsustainable</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˌʌn.səˈsteɪ.nə.bəl/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: không bền vững</span></p><p><span style=\"font-size: 16px\">   - English Definition: Not able to be maintained at the current rate or level.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: </span><strong><span style=\"font-size: 16px\">Unsustainable</span></strong><span style=\"font-size: 16px\"> resource usage threatens future generations.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: unmanageable, inexhaustible</span></p><p><span style=\"font-size: 16px\">2. </span><strong><span style=\"font-size: 16px\">Depletion</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /dɪˈpliː.ʃən/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự cạn kiệt</span></p><p><span style=\"font-size: 16px\">   - English Definition: Reduction in the number or quantity of something.</span></p><p><span style=\"font-size: 16px\">   - Level: Advanced</span></p><p><span style=\"font-size: 16px\">   - Example: Water </span><strong><span style=\"font-size: 16px\">depletion</span></strong><span style=\"font-size: 16px\"> is becoming a critical problem worldwide.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: exhaustion, consumption</span></p><p><span style=\"font-size: 16px\">3. </span><strong><span style=\"font-size: 16px\">Deforestation</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˌdiːˌfɒr.ɪˈsteɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự phá rừng</span></p><p><span style=\"font-size: 16px\">   - English Definition: The action of clearing a wide area of trees.</span></p><p><span style=\"font-size: 16px\">   - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: </span><strong><span style=\"font-size: 16px\">Deforestation</span></strong><span style=\"font-size: 16px\"> contributes to the loss of biodiversity.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: forest clearance, logging</span></p><p><span style=\"font-size: 16px\">4. </span><strong><span style=\"font-size: 16px\">Ecological footprint</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ɪˌkɒl.əˈdʒɪ.kəl ˈfʊt.prɪnt/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: dấu chân sinh thái</span></p><p><span style=\"font-size: 16px\">   - English Definition: The impact of a person or community on the environment, expressed as the amount of land required to sustain their use of natural resources.</span></p><p><span style=\"font-size: 16px\">   - Level: Advanced</span></p><p><span style=\"font-size: 16px\">   - Example: Reducing our </span><strong><span style=\"font-size: 16px\">ecological footprint</span></strong><span style=\"font-size: 16px\"> is essential for sustainability.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: environmental impact, carbon footprint</span></p><p><span style=\"font-size: 16px\">5. </span><strong><span style=\"font-size: 16px\">Renewable resources</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /rɪˈnjuː.ə.bəl rɪˈsɔːrsɪz/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: tài nguyên tái tạo</span></p><p><span style=\"font-size: 16px\">   - English Definition: Natural resources that can be replenished in a short amount of time.</span></p><p><span style=\"font-size: 16px\">   - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Solar and wind are examples of </span><strong><span style=\"font-size: 16px\">renewable resources</span></strong><span style=\"font-size: 16px\">.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: sustainable resources, green energy</span></p><p><span style=\"font-size: 16px\">6. </span><strong><span style=\"font-size: 16px\">Carbon emissions</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˈkɑː.bən ɪˈmɪʃ.ənz/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: khí thải carbon</span></p><p><span style=\"font-size: 16px\">   - English Definition: Carbon dioxide and carbon monoxide in the atmosphere, produced by vehicles and industrial processes.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: Reducing </span><strong><span style=\"font-size: 16px\">carbon emissions</span></strong><span style=\"font-size: 16px\"> is vital to address global warming.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: CO2 output, greenhouse gases</span></p><p><span style=\"font-size: 16px\">7. </span><strong><span style=\"font-size: 16px\">Biodiversity loss</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˌbaɪ.əʊ.daɪˈvɜː.sɪ.ti lɒs/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: sự mất mát đa dạng sinh học</span></p><p><span style=\"font-size: 16px\">   - English Definition: The reduction or disappearance of biological diversity in an environment.</span></p><p><span style=\"font-size: 16px\">   - Level: Advanced</span></p><p><span style=\"font-size: 16px\">   - Example: </span><strong><span style=\"font-size: 16px\">Biodiversity loss</span></strong><span style=\"font-size: 16px\"> affects ecosystem stability and productivity.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: species extinction, habitat degradation</span></p><p><span style=\"font-size: 16px\">8. </span><strong><span style=\"font-size: 16px\">Overexploitation</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˌəʊ.vər.ek.splɔɪˈteɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: khai thác quá mức</span></p><p><span style=\"font-size: 16px\">   - English Definition: Excessive use of natural resources, causing depletion or damage.</span></p><p><span style=\"font-size: 16px\">   - Level: Advanced</span></p><p><span style=\"font-size: 16px\">   - Example: </span><strong><span style=\"font-size: 16px\">Overexploitation</span></strong><span style=\"font-size: 16px\"> of fisheries has led to dwindling fish stocks.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: overuse, overharvesting</span></p><p><span style=\"font-size: 16px\">9. </span><strong><span style=\"font-size: 16px\">Conservation efforts</span></strong></p><p><span style=\"font-size: 16px\">   - Pronunciation: /ˌkɒn.səˈveɪ.ʃən ˈef.əts/</span></p><p><span style=\"font-size: 16px\">   - Nghĩa: nỗ lực bảo tồn</span></p><p><span style=\"font-size: 16px\">   - English Definition: Actions taken to preserve, protect, or restore the natural environment.</span></p><p><span style=\"font-size: 16px\">   - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">   - Example: </span><strong><span style=\"font-size: 16px\">Conservation efforts</span></strong><span style=\"font-size: 16px\"> are crucial for protecting endangered species.</span></p><p><span style=\"font-size: 16px\">   - Synonyms: environmental protection, preservation activities</span></p><p><span style=\"font-size: 16px\">10. </span><strong><span style=\"font-size: 16px\">Sustainable development</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /səˈsteɪ.nə.bəl dɪˈvel.əp.mənt/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: phát triển bền vững</span></p><p><span style=\"font-size: 16px\">    - English Definition: Economic development that is conducted without depletion of natural resources.</span></p><p><span style=\"font-size: 16px\">    - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Sustainable development</span></strong><span style=\"font-size: 16px\"> aims to meet current needs without compromising future generations.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: responsible growth, green development</span></p><p><span style=\"font-size: 16px\">11. </span><strong><span style=\"font-size: 16px\">Climate change</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈklaɪ.mət tʃeɪndʒ/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: biến đổi khí hậu</span></p><p><span style=\"font-size: 16px\">    - English Definition: Changes in global or regional climate patterns, particularly from the mid to late 20th century onwards.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Climate change</span></strong><span style=\"font-size: 16px\"> is exacerbated by high levels of carbon emissions.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: global warming, atmospheric change</span></p><p><span style=\"font-size: 16px\">12. </span><strong><span style=\"font-size: 16px\">Recycling programs</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈriː.saɪ.klɪŋ ˈprəʊ.ɡræmz/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: chương trình tái chế</span></p><p><span style=\"font-size: 16px\">    - English Definition: Systems established to process materials to make them suitable for reuse.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Recycling programs</span></strong><span style=\"font-size: 16px\"> help reduce waste and conserve resources.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: waste recovery, reuse schemes</span></p><p><span style=\"font-size: 16px\">13. </span><strong><span style=\"font-size: 16px\">Resource management</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /rɪˈsɔːs ˈmæn.ɪdʒ.mənt/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: quản lý tài nguyên</span></p><p><span style=\"font-size: 16px\">    - English Definition: The efficient and effective deployment of an organization's resources when they are needed.</span></p><p><span style=\"font-size: 16px\">    - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Effective </span><strong><span style=\"font-size: 16px\">resource management</span></strong><span style=\"font-size: 16px\"> can mitigate the effects of resource depletion.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: resource allocation, resource utilization</span></p><p><span style=\"font-size: 16px\">14. </span><strong><span style=\"font-size: 16px\">Environmental impact</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ɪnˌvaɪ.rənˈmen.təl ˈɪm.pækt/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: tác động môi trường</span></p><p><span style=\"font-size: 16px\">    - English Definition: The effect of human activities and natural events on the natural world.</span></p><p><span style=\"font-size: 16px\">    - Level: Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: The </span><strong><span style=\"font-size: 16px\">environmental impact</span></strong><span style=\"font-size: 16px\"> of deforestation is profound and multifaceted.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: ecological effect, environmental influence</span></p><p><span style=\"font-size: 16px\">15. </span><strong><span style=\"font-size: 16px\">Water scarcity</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈwɔː.tə ˈskeə.sɪ.ti/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: tình trạng khan hiếm nước</span></p><p><span style=\"font-size: 16px\">    - English Definition: The lack of sufficient available water resources to meet the demands of water usage within a region.</span></p><p><span style=\"font-size: 16px\">    - Level: Advanced</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Water scarcity</span></strong><span style=\"font-size: 16px\"> is becoming a pressing issue in many arid regions.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: water shortage, drought</span></p><p><span style=\"font-size: 16px\">16. </span><strong><span style=\"font-size: 16px\">Alternative energy</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ɔːlˈtɜː.nə.tɪv ˈen.ə.dʒi/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: năng lượng thay thế</span></p><p><span style=\"font-size: 16px\">    - English Definition: Energy generated in ways that do not deplete natural resources or harm the environment.</span></p><p><span style=\"font-size: 16px\">    - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Alternative energy</span></strong><span style=\"font-size: 16px\"> sources like wind and solar power are key to sustainable development.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: renewable energy, green energy</span></p><p><span style=\"font-size: 16px\">17. </span><strong><span style=\"font-size: 16px\">Ecological balance</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ɪˌkɒl.əˈdʒɪ.kəl ˈbæl.əns/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: cân bằng sinh thái</span></p><p><span style=\"font-size: 16px\">    - English Definition: A state of dynamic equilibrium within a community of organisms in which genetic, species, and ecosystem diversity remain relatively stable.</span></p><p><span style=\"font-size: 16px\">    - Level: Advanced</span></p><p><span style=\"font-size: 16px\">    - Example: Maintaining </span><strong><span style=\"font-size: 16px\">ecological balance</span></strong><span style=\"font-size: 16px\"> is essential for sustaining life on Earth.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: environmental equilibrium, ecological stability</span></p><p><span style=\"font-size: 16px\">18. </span><strong><span style=\"font-size: 16px\">Pollution control</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /pəˈluː.ʃən kənˈtrəʊl/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: kiểm soát ô nhiễm</span></p><p><span style=\"font-size: 16px\">    - English Definition: Regulations and standards aimed at reducing the release of pollutants into the environment.</span></p><p><span style=\"font-size: 16px\">    - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Pollution control</span></strong><span style=\"font-size: 16px\"> measures are urgently needed to reduce air and water pollution.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: contamination management, pollution reduction</span></p><p><span style=\"font-size: 16px\">19. </span><strong><span style=\"font-size: 16px\">Green initiatives</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation:</span></p><p><span style=\"font-size: 16px\"> /ɡriːn ɪˈnɪʃ.ə.tɪvz/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: các sáng kiến xanh</span></p><p><span style=\"font-size: 16px\">    - English Definition: Projects or strategies aimed at promoting environmental sustainability.</span></p><p><span style=\"font-size: 16px\">    - Level: Upper Intermediate</span></p><p><span style=\"font-size: 16px\">    - Example: Many companies are adopting </span><strong><span style=\"font-size: 16px\">green initiatives</span></strong><span style=\"font-size: 16px\"> to reduce their ecological footprints.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: eco-friendly projects, sustainable practices</span></p><p><span style=\"font-size: 16px\">20. </span><strong><span style=\"font-size: 16px\">Habitat restoration</span></strong></p><p><span style=\"font-size: 16px\">    - Pronunciation: /ˈhæb.ɪ.tæt ˌres.təˈreɪ.ʃən/</span></p><p><span style=\"font-size: 16px\">    - Nghĩa: phục hồi môi trường sống</span></p><p><span style=\"font-size: 16px\">    - English Definition: The process of returning a natural environment to its original condition after damage or disruption.</span></p><p><span style=\"font-size: 16px\">    - Level: Advanced</span></p><p><span style=\"font-size: 16px\">    - Example: </span><strong><span style=\"font-size: 16px\">Habitat restoration</span></strong><span style=\"font-size: 16px\"> is crucial for the recovery of endangered species.</span></p><p><span style=\"font-size: 16px\">    - Synonyms: ecological restoration, environmental rehabilitation</span></p><p></p>"
          }
        ],
        'samples': [
          {
            'id': 401,
            'questionId': 3685,
            'sampleText': '<p><span style="font-size: 16px">The rapid depletion of natural resources such as oil, forests, and fresh water poses significant challenges for sustainable development and environmental stability. This essay will explore the problems caused by this alarming consumption rate and propose viable solutions to mitigate these issues.</span></p><p><span style="font-size: 16px">The overconsumption of natural resources leads to several critical problems. Firstly, it exacerbates environmental degradation. Deforestation, for instance, results in loss of biodiversity, disruption of ecosystems, and increased carbon emissions, which contribute to climate change. Similarly, the excessive use of fossil fuels like oil leads to heightened levels of pollution and accelerates global warming. Secondly, the depletion of fresh water resources threatens the health and survival of millions of people and can lead to water scarcity crises, affecting agriculture, industry, and domestic usage.</span></p><p><span style="font-size: 16px">To address these problems, a multi-faceted approach is required. One effective solution is the adoption of sustainable practices. For example, transitioning to renewable energy sources such as solar and wind can significantly reduce reliance on oil. This shift not only conserves oil reserves but also minimizes environmental pollution. Additionally, implementing advanced agricultural techniques that use water more efficiently can help preserve fresh water resources. Techniques such as drip irrigation and rainwater harvesting are proven methods that reduce water consumption and dependency.</span></p><p><span style="font-size: 16px">Furthermore, reforestation and afforestation initiatives are vital. Planting trees and restoring forested areas can help reverse the effects of deforestation, enhance biodiversity, and act as carbon sinks, thereby mitigating climate change.</span></p><p><span style="font-size: 16px">In conclusion, while the rapid consumption of natural resources poses serious environmental and social challenges, adopting sustainable practices, transitioning to renewable energy, and promoting water conservation and forest restoration are effective strategies to address these issues. By implementing these solutions, we can ensure the health of our planet and the well-being of future generations.</span></p>',
            'bandScore': 8,
            'comment': null,
            'lastActivityDate': '2024-06-17T22:27:18.436553',
            'title': 'Overconsumption of Natural Resources'
          }
        ],
        'rubrics': [
          {
            'id': 7,
            'name': 'Task Response',
            'description': 'The Task Response criterion in IELTS Academic Writing Task 2 assesses how well you develop and support your ideas in response to the question. It focuses on your ability to present a clear, relevant position, fully extend and support your main ideas, and answer all parts of the task prompt comprehensively.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 61,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 62,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1\n- The content is wholly unrelated to the prompt\n- Any copied rubric must be discounted'
              },
              {
                'id': 63,
                'bandScore': 2,
                'description': '- The content is barely related to the prompt.\n- No position can be identified.\n- There may be glimpses of one or two ideas without development.'
              },
              {
                'id': 64,
                'bandScore': 3,
                'description': '- No part of the prompt is adequately addressed, or the prompt has been misunderstood.\n- No relevant position can be identified, and/or there is little direct response to the question/s.\n- There are few ideas, and these may be irrelevant or insufficiently developed.'
              },
              {
                'id': 65,
                'bandScore': 4,
                'description': '- The prompt is tackled in a minimal way, or the answer is tangential, possibly due to some misunderstanding of the prompt.\n- The format may be inappropriate.\n- A position is discernible, but the reader has to read carefully to find it.\n- Main ideas are difficult to identify and such ideas that are identifiable may lack relevance, clarity and/or support.\n- Large parts of the response may be repetitive.'
              },
              {
                'id': 66,
                'bandScore': 5,
                'description': '- The main parts of the prompt are incompletely addressed. The format may be inappropriate in places.\n- The writer expresses a position, but the development is not always clear.\n- Some main ideas are put forward, but they are limited and are not sufficiently developed and/or there may be irrelevant detail.\n- There may be some repetition.'
              },
              {
                'id': 67,
                'bandScore': 6,
                'description': '- The main parts of the prompt are addressed (though some may be more fully covered than others). An appropriate format is used. \n- A position is presented that is directly relevant to the prompt, although the conclusions drawn may be unclear, unjustified or repetitive.\n- Main ideas are relevant, but some may be insufficiently developed or may lack clarity, while some supporting arguments and evidence may be less relevant or inadequate.'
              },
              {
                'id': 68,
                'bandScore': 7,
                'description': '- The main parts of the prompt are appropriately addressed.\n- A clear and developed position is presented.\n- Main ideas are extended and supported but there may be a tendency to over generalise or there may be a lack of focus and precision in supporting ideas/material.'
              },
              {
                'id': 69,
                'bandScore': 8,
                'description': '- The prompt is appropriately and sufficiently addressed.\n- A clear and well developed position is presented in response to the question/s.\n- Ideas are relevant, well extended and supported.\n- There may be occasional omissions or lapses in content.'
              },
              {
                'id': 70,
                'bandScore': 9,
                'description': '- The prompt is appropriately addressed and explored in depth.\n- A clear and fully developed position is presented which directly answers the question/s.\n- Ideas are relevant, fully extended and well supported.\n- Any lapses in content or support are extremely rare.'
              }
            ]
          },
          {
            'id': 8,
            'name': 'Coherence & Cohesion',
            'description': 'The Coherence & Cohesion criterion in IELTS Academic Writing Task 2 evaluates your ability to organize ideas logically and connect them smoothly. It focuses on clear overall structure, logical sequencing of paragraphs and ideas, and the effective use of cohesive devices (such as linking words and pronouns) to guide the reader through your argument or narrative seamlessly.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 71,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 72,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1. \n- The writing fails to communicate any message and appears to be by a virtual non writer.'
              },
              {
                'id': 73,
                'bandScore': 2,
                'description': '- There is little relevant message, or the entire response may be off topic.\n- There is little evidence of control of organisational features.'
              },
              {
                'id': 74,
                'bandScore': 3,
                'description': '- There is no apparent logical organisation . Ideas are discernible but difficult to relate to each other.\n- There is minimal use of sequencers or cohesive devices. Those used do not necessarily indicate a logical relationship between ideas.\n- There is difficulty in identifying referencing.\n- Any attempts at paragraphing are unhelpful.'
              },
              {
                'id': 75,
                'bandScore': 4,
                'description': '- Information and ideas are evident but not arranged coherently and there is no clear progression within the response.\n- Relationships between ideas can be unclear and/or inadequately marked. There is some use of basic cohesive devices, which may be inaccurate or repetitive.\n- There is inaccurate use or a lack of substitution or\nreferencing.\n- There may be no paragraphing and/or no clear main topic within paragraphs.'
              },
              {
                'id': 76,
                'bandScore': 5,
                'description': '- Organisation is evident but is not wholly logical and there may be a lack of overall progression.Nevertheless, there is a sense of underlying coherence to the response.\n- The relationship of ideas can be followed but the sentences are not fluently linked to each other.\n- There may be limited/overuse of cohesive devices with some inaccuracy.\n- The writing may be repetitive due to inadequate and/or inaccurate use of reference and substitution.\n- Paragraphing may be inadequate or missing.'
              },
              {
                'id': 77,
                'bandScore': 6,
                'description': '- Information and ideas are generally arranged coherently and there is a clear overall progression.\n- Cohesive devices are used to some good effect but cohesion within and/or between sentences may be faulty or mechanical due to misuse, overuse or omission.\n- The use of reference and substitution may lack flexibility or clarity and result in some repetition or error.\n- Paragraphing may not always be logical and/or the central topic may not always be clear.'
              },
              {
                'id': 78,
                'bandScore': 7,
                'description': '- Information and ideas are logically organised, and there is a clear progression throughout the response. (A few lapses may occur, but these are minor.)\n- A range of cohesive devices including reference and substitution is used flexibly but with some inaccuracies or some over/under use.\n- Paragraphing is generally used effectively to support overall coherence, and the sequencing of ideas within a paragraph is generally logical.'
              },
              {
                'id': 79,
                'bandScore': 8,
                'description': '- The message can be followed with ease.\n- Information and ideas are logically sequenced, and cohesion is well managed.\n- Occasional lapses in coherence and cohesion may occur.\n- Paragraphing is used sufficiently and appropriately.'
              },
              {
                'id': 80,
                'bandScore': 9,
                'description': '- The message can be followed effortlessly.\n- Cohesion is used in such a way that it very rarely attracts attention.\n- Any lapses in coherence or cohesion are minimal.\n- Paragraphing is skilfully managed.'
              }
            ]
          },
          {
            'id': 9,
            'name': 'Lexical Resource',
            'description': 'The Lexical Resource criterion in IELTS Academic Writing Task 2 measures your range of vocabulary, the precision of your word choices, and your ability to use words appropriately for various contexts. It assesses your skill in using vocabulary flexibly and accurately to express ideas and opinions, including the effective use of synonyms and collocations, without errors that hinder communication.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 81,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 82,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- No resource is apparent, except for a few isolated words.'
              },
              {
                'id': 83,
                'bandScore': 2,
                'description': '- The resource is extremely limited with few recognisable strings, apart from memorised phrases.\n- There is no apparent control of word formation and/or spelling.'
              },
              {
                'id': 84,
                'bandScore': 3,
                'description': '- The resource is inadequate (which may be due to the response being significantly underlength). Possible over-dependence on input material or memorised language.\n- Control of word choice and/or spelling is very limited, and errors predominate. These errors may severely impede meaning.'
              },
              {
                'id': 85,
                'bandScore': 4,
                'description': '- The resource is limited and inadequate for or unrelated to the task. Vocabulary is basic and may be used repetitively.\n- There may be inappropriate use of lexical chunks (e.g. memorised phrases, formulaic language and/or language from the input material). \n- Inappropriate word choice and/or errors in word formation and/or in spelling may impede meaning'
              },
              {
                'id': 86,
                'bandScore': 5,
                'description': '- The resource is limited but minimally adequate for the task.\n- Simple vocabulary may be used accurately but the range does not permit much variation in expression. \n- There may be frequent lapses in the appropriacy of word choice and a lack of flexibility is apparent in frequent simplifications and/or repetitions. \n- Errors in spelling and/or word formation may be noticeable and may cause some difficulty for the reader.'
              },
              {
                'id': 87,
                'bandScore': 6,
                'description': '- The resource is generally adequate and appropriate for the task.\n- The meaning is generally clear in spite of a rather restricted range or a lack of precision in word choice.\n- If the writer is a risk-taker, there will be a wider range of vocabulary used but higher degrees of inaccuracy or inappropriacy.\n- There are some errors in spelling and/or word formation, but these do not impede communication.'
              },
              {
                'id': 88,
                'bandScore': 7,
                'description': '- The resource is sufficient to allow some flexibility and precision.\n- There is some ability to use less common and/or idiomatic items.\n- An awareness of style and collocation is evident, though inappropriacies occur.\n- There are only a few errors in spelling and/or word formation and they do not detract from overall clarity.'
              },
              {
                'id': 89,
                'bandScore': 8,
                'description': '- A wide resource is fluently and flexibly used to convey precise meanings. \n- There is skilful use of uncommon and/or idiomatic items when appropriate, despite occasional inaccuracies in word choice and collocation.\n- Occasional errors in spelling and/or word formation may occur, but have minimal impact on communication.'
              },
              {
                'id': 90,
                'bandScore': 9,
                'description': '- Full flexibility and precise use are widely evident.\n- A wide range of vocabulary is used accurately and appropriately with very natural and sophisticated control of lexical features.\n- Minor errors in spelling and word formation are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 10,
            'name': 'Grammatical Range & Accuracy',
            'description': 'The Grammatical Range & Accuracy criterion in IELTS Academic Writing Task 2 evaluates your ability to use a wide range of grammar structures accurately. It focuses on your capacity to construct complex sentences effectively, use punctuation correctly, and apply grammatical forms with flexibility to clearly express detailed reasoning and arguments without making errors that obscure meaning.',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 91,
                'bandScore': 0,
                'description': 'Should only be used where a candidate did not attend or attempt the question in any way, used a language other than English t hro ughout, or where there is proof that a candidate’s answer has been totally memorised.'
              },
              {
                'id': 92,
                'bandScore': 1,
                'description': '- Responses of 20 words or fewer are rated at Band 1.\n- No rateable language is evident.'
              },
              {
                'id': 93,
                'bandScore': 2,
                'description': 'There is little or no evidence of sentence forms (except in memorised phrases).'
              },
              {
                'id': 94,
                'bandScore': 3,
                'description': '- Sentence forms are attempted, but errors in grammar and punctuation predominate (except in memorised phrases or those taken from the input material).\n- This prevents most meaning from coming through. Length may be insufficient to provide evidence of control of sentence forms.'
              },
              {
                'id': 95,
                'bandScore': 4,
                'description': 'A very limited range of structures is used.\n- Subordinate clauses are rare and simple sentences predominate.\n- Some structures are produced accurately but grammatical errors are frequent and may impede meaning.\n- Punctuation is often faulty or inadequate.'
              },
              {
                'id': 96,
                'bandScore': 5,
                'description': '- The range of structures is limited and rather repetitive.\n- Although complex sentences are attempted, they tend to be faulty, and the greatest accuracy is achieved on simple sentences.\n- Grammatical errors may be frequent and cause some difficulty for the reader.\n- Punctuation may be faulty.'
              },
              {
                'id': 97,
                'bandScore': 6,
                'description': '- A mix of simple and complex sentence forms is used but flexibility is limited.\n- Examples of more complex structures are not marked by the same level of accuracy as in simple structures.\n- Errors in grammar and punctuation occur, but rarely impede communication.'
              },
              {
                'id': 98,
                'bandScore': 7,
                'description': '- A variety of complex structures is used with some flexibility and accuracy.\n- Grammar and punctuation are generally well controlled, and error-free sentences are frequent.\n- A few errors in grammar may persist, but these do not impede communication.'
              },
              {
                'id': 99,
                'bandScore': 8,
                'description': '- A wide range of structures is flexibly and accurately used.\n- The majority of sentences are error-free, and punctuation is well managed.\n- Occasional, non-systematic errors and inappropriacies occur, but have minimal impact on communication.'
              },
              {
                'id': 100,
                'bandScore': 9,
                'description': '- A wide range of structures is used with full flexibility and control.\n- Punctuation and grammar are used appropriately throughout.\n- Minor errors are extremely rare and have minimal impact on communication.'
              }
            ]
          },
          {
            'id': 36,
            'name': 'Critical Errors',
            'description': 'Critical Errors',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 268,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 37,
            'name': 'Overall Score & Feedback',
            'description': 'Overall Feedback',
            'hasScore': true,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 235,
                'bandScore': 0,
                'description': null
              },
              {
                'id': 236,
                'bandScore': 1,
                'description': null
              },
              {
                'id': 237,
                'bandScore': 2,
                'description': null
              },
              {
                'id': 238,
                'bandScore': 3,
                'description': null
              },
              {
                'id': 239,
                'bandScore': 4,
                'description': null
              },
              {
                'id': 240,
                'bandScore': 5,
                'description': null
              },
              {
                'id': 241,
                'bandScore': 6,
                'description': null
              },
              {
                'id': 242,
                'bandScore': 7,
                'description': null
              },
              {
                'id': 243,
                'bandScore': 8,
                'description': null
              },
              {
                'id': 244,
                'bandScore': 9,
                'description': null
              }
            ]
          },
          {
            'id': 43,
            'name': 'Arguments Assessment',
            'description': 'Arguments Assessment',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 284,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 45,
            'name': 'Vocabulary',
            'description': 'Vocabulary',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 286,
                'bandScore': 0,
                'description': null
              }
            ]
          },
          {
            'id': 47,
            'name': 'Improved Version',
            'description': 'Improved Version',
            'hasScore': false,
            'orderId': 0,
            'bandScoreDescriptions': [
              {
                'id': 288,
                'bandScore': 0,
                'description': null
              }
            ]
          }
        ]
      }
    ],
    initialSubmission: null
  }
}

const state = getDefaultState()

const actions = {
  async submitPersonalQuestion({ commit, state }, userId) {
    const formData = new FormData()
    formData.set('UserId', userId)
    formData.set('TaskName', state.personalQuestion.TaskName)
    formData.set('TaskId', state.personalQuestion.TaskId)
    formData.set('Test', state.personalQuestion.Test)
    formData.set('Text', state.personalQuestion.Text)
    formData.set('FeedbackLanguage', state.personalQuestion.FeedbackLanguage)
    const question = state.personalQuestion.Parts.find(q => q.Name == 'Question')
    if (question) {
      // Setup question content to send to the backend
      formData.set('Question.QuestionParts[0][Name]', 'Question')
      formData.set('Question.QuestionParts[0][Content]', question.Content)
      formData.set('Question.QuestionParts[0][Order]', 1)
      formData.set(`Question.QuestionParts[0][QuestionId]`, 0)

      const chart = state.personalQuestion.Parts.find(q => q.Name == 'Chart')
      if (chart) {
        // Setup chart to send to the backend
        formData.set(`Question.QuestionParts[1][Name]`, 'Chart')
        formData.set(`Question.QuestionParts[1][Order]`, 2)
        formData.set(`Question.QuestionParts[1][Content]`, chart.Content)
        formData.set(`Question.QuestionParts[1][FileName]`, chart.FileName)
        formData.set(`Question.UploadedFile`, chart.UploadedFile)
      }
      await questionService.createInitialSubmission(formData)
    }
  },
  async submitInitialTest({ state, commit }, userId) {
    state.initialSubmission.userId = userId
    await documentService.createInitialSubmission(state.initialSubmission)
  },
  saveInitialTestData({ commit, state }, data) {
    commit('SET_INITIAL_SUBMISSION', data)
  },
  selectTopic({ commit, state }, selectedTopic) {
    const selectedTest = state.questionsForInitialTest.find(q => q.title == selectedTopic)
    commit('SET_QUESTION', selectedTest)
  },
  switchTest({ commit, state }, test) {
    if (test == 'Đề Task 1') {
      const selectedTest = state.questionsForInitialTest.find(q => q.section == 'Academic Writing Task 1')
      commit('SET_QUESTION', selectedTest)
    } else {
      const selectedTest = state.questionsForInitialTest.find(q => q.section == 'Academic Writing Task 2')
      commit('SET_QUESTION', selectedTest)
    }
  },
  async loadQuestionsForInitialTest({ commit, state }) {
    const selectedTest = state.questionsForInitialTest.find(q => q.section == 'Academic Writing Task 2')
    commit('SET_QUESTION', selectedTest)
    return selectedTest
    // if (state.questionsForInitialTest && state.questionsForInitialTest.length > 0) {
    //   const selectedTest = state.questionsForInitialTest.find(q => q.section == 'Academic Writing Task 2')
    //   commit('SET_QUESTION', selectedTest)
    //   return selectedTest
    // } else {
    //   const result = await questionService.getQuestionsForInitialTest()
    //   console.log('Questions:', result)
    //   commit('SET_QUESTIONS_INIT_TEST', result)
    //   const selectedTest = result.find(q => q.section == 'Academic Writing Task 2')
    //   commit('SET_QUESTION', selectedTest)
    //   return selectedTest
    // }
  },
  savePersonalQuestion({ commit }, personalQuestion) {
    commit('SET_PERSONAL_QUESTION', personalQuestion)
  },
  clearInitialSubmission({ commit }) {
    commit('CLEAR_INITIAL_SUBMISSION')
  },
  clearPersonalQuestion({ commit }) {
    commit('CLEAR_PERSONAL_QUESTION')
  },
  loadAllQuestionByUser({ commit }, userId) {
    return questionService.getAllByUser(userId).then(result => {
      commit('SET_QUESTIONS', result)
    })
  },
  loadQuestions({ commit }) {
    return questionService.getAll().then(result => {
      commit('SET_QUESTIONS', result)
    })
  },
  loadQuestion({ commit }, id) {
    return questionService.getById(id).then(result => {
      commit('SET_QUESTION', result)
      return result
    })
  },
  loadCountQuestionsByTasks({ commit }) {
    return questionService.getCountQuestionByTasks().then(result => {
      commit('SET_COUNT_QUESTIONS', result)
    })
  },
  loadSummaryByUser({ commit }, id) {
    return questionService.getCountQuestionsByUser(id).then(result => {
      commit('SET_COUNT_QUESTION_BY_USER', result)
    })
  },
  loadTestByUser({ commit }, id) {
    return questionService.getTestByUser(id).then(result => {
      commit('SET_TEST_BY_USER', result)
    })
  },
  loadStatusQuestion({ commit }, id) {
    return questionService.getStatusQuestion(id).then(result => {
      commit('SET_STATUS_QUESTION', result)
    })
  },
  loadSampleByQuestion({ commit }, id) {
    return questionService.getSampleByQuestion(id).then(result => {
      commit('SET_SAMPLE_QUESTION', result)
    })
  },
  clearSelectedQuestion({ commit }) {
    commit('CLEAR_SELECTED_QUESTION')
  },
  loadAllSamples({ commit }) {
    return sampleService.getAllSamples().then(rs => {
      commit('SET_ALL_SAMPLE', rs)
    })
  },
  clearState({ commit }) {
    commit('CLEAR_STATE')
  }
  // getAddEditQuestionData({ commit }) {
  //   questionService.getAddEditQuestionData().then(result => {
  //     commit('SET_ADD_EDIT_DATA', result)
  //     return result
  //   })
  // }
}

const mutations = {
  SET_INITIAL_SUBMISSION: (state, submission) => {
    state.initialSubmission = submission
  },
  SET_QUESTIONS_INIT_TEST: (state, questions) => {
    state.questionsForInitialTest = questions
  },
  SET_QUESTIONS: (state, questions) => {
    state.questions = questions
  },
  SET_QUESTION: (state, question) => {
    state.selectedQuestion = question
  },
  SET_COUNT_QUESTIONS: (state, countQuestions) => {
    state.countQuestions = countQuestions
  },
  SET_COUNT_QUESTION_BY_USER: (state, countQuestionsByUser) => {
    state.countQuestionsByUser = countQuestionsByUser
  },
  SET_TEST_BY_USER: (state, testByUser) => {
    state.testByUser = testByUser
  },
  SET_STATUS_QUESTION: (state, statusQuestion) => {
    state.statusQuestion = statusQuestion
  },
  SET_SAMPLE_QUESTION: (state, sampleByQuestion) => {
    state.sampleByQuestion = sampleByQuestion
  },
  CLEAR_SELECTED_QUESTION: (state) => {
    state.selectedQuestion = {}
  },
  SET_ALL_SAMPLE: (state, samples) => {
    state.samples = samples
  },
  SET_PERSONAL_QUESTION: (state, personalQuestion) => {
    state.personalQuestion = personalQuestion
  },
  CLEAR_PERSONAL_QUESTION: (state) => {
    state.personalQuestion = null
  },
  CLEAR_INITIAL_SUBMISSION: (state) => {
    state.initialSubmission = null
  },
  CLEAR_STATE(state) {
    const personalQuestion = state.personalQuestion
    const initialSubmission = state.initialSubmission
    Object.assign(state, getDefaultState())
    state.personalQuestion = personalQuestion
    state.initialSubmission = initialSubmission
  }
}

const getters = {
  getAll: state => state.questions,
  getSelected: state => state.selectedQuestion,
  getCountQuestionByTasks: state => state.countQuestions,
  getSummaryByUser: state => state.countQuestionsByUser,
  getTestByUser: state => state.testByUser,
  getStatusQuestion: state => state.statusQuestion,
  getSampleByQuestion: state => state.sampleByQuestion,
  getAllSample: state => state.samples,
  getPersonalQuestion: state => state.personalQuestion,
  getInitialSubmission: state => state.initialSubmission,
  getInitialQuestions: state => state.questionsForInitialTest
  // getSelected: state => state.selectedRater
}

export default {
  namespaced: true,
  state,
  mutations,
  getters,
  actions
}
