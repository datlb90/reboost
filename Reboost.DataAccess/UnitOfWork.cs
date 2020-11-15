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

        Task<int> CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReboostDbContext _context;
        private DocumentRespository _documentRepository;
        private RequestQueueRepository _requestQueueRepository;
        private RaterRepository _raterRepository;
        private LookUpRepository _lookUpRepository;


        public UnitOfWork(ReboostDbContext context)
        {
            this._context = context;
        }

        public IDocumentRepository Documents => _documentRepository = _documentRepository ?? new DocumentRespository(_context);
        public IRequestQueueRepository RequestQueues => _requestQueueRepository = _requestQueueRepository ?? new RequestQueueRepository(_context);
        public IRaterRepository Raters => _raterRepository = _raterRepository ?? new RaterRepository(_context);
        public ILookUpRepository LookUps => _lookUpRepository = _lookUpRepository ?? new LookUpRepository(_context);

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
