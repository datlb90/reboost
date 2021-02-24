using System;
using System.Threading.Tasks;
using Reboost.DataAccess.Repositories;

namespace Reboost.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentRepository Documents { get; }
        IRequestQueueRepository RequestQueues { get; }
        IRaterRepository Raters { get; }
        ILookUpRepository LookUps { get; }
        IQuestionsRepository Questions { get; }
        IQuestionPartRepository QuestionParts { get; }
        ISampleRepository Samples { get; }
        IUserRepository Users { get; }
        IRubricRepository Rubrics { get; }
        ISubmissionRepository Submission { get; }
        IRaterCredentialRepository RaterCredential { get; }
        IDiscussionRepository Discussion { get; }
        IPaymentRepository Payment { get; }
        IReviewRepository Review { get; }
        Task<int> CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReboostDbContext _context;
        private DocumentRespository _documentRepository;
        private RequestQueueRepository _requestQueueRepository;
        private RaterRepository _raterRepository;
        private LookUpRepository _lookUpRepository;
        private QuestionsRepository _questionsRepository;
        private QuestionPartRepository _questionPartRepository;
        private SampleRepository _sampleRepository;
        private UserRepository _userRepository;
        private RubricRepository _rubricRepository;
        private SubmissionRepository _submissionRepository;
        private RaterCredentialRepository _raterCredentialRepository;
        private DiscussionRepository _discussionRepository;
        private PaymentRepository _paymentRepository;
        private ReviewRepository _reviewRepository;
        public UnitOfWork(ReboostDbContext context)
        {
            _context = context;
        }

        public IDocumentRepository Documents => _documentRepository = _documentRepository ?? new DocumentRespository(_context);
        public IRequestQueueRepository RequestQueues => _requestQueueRepository = _requestQueueRepository ?? new RequestQueueRepository(_context);
        public IRaterRepository Raters => _raterRepository = _raterRepository ?? new RaterRepository(_context);
        public ILookUpRepository LookUps => _lookUpRepository = _lookUpRepository ?? new LookUpRepository(_context);
        public IQuestionsRepository Questions => _questionsRepository = _questionsRepository ?? new QuestionsRepository(_context);
        public IQuestionPartRepository QuestionParts => _questionPartRepository = _questionPartRepository ?? new QuestionPartRepository(_context);
        public ISampleRepository Samples => _sampleRepository = _sampleRepository ?? new SampleRepository(_context);
        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);
        public IRubricRepository Rubrics => _rubricRepository = _rubricRepository ?? new RubricRepository(_context);
        public ISubmissionRepository Submission => _submissionRepository = _submissionRepository ?? new SubmissionRepository(_context);
        public IRaterCredentialRepository RaterCredential => _raterCredentialRepository = _raterCredentialRepository ?? new RaterCredentialRepository(_context);
        public IDiscussionRepository Discussion => _discussionRepository = _discussionRepository ?? new DiscussionRepository(_context);
        public IPaymentRepository Payment => _paymentRepository = _paymentRepository ?? new PaymentRepository(_context);
        public IReviewRepository Review => _reviewRepository = _reviewRepository ?? new ReviewRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
