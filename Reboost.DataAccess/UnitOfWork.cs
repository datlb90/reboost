using System;
using System.Threading.Tasks;
using Reboost.DataAccess.Repositories;

namespace Reboost.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentRepository Documents { get; }
        IRequestQueueRepository RequestQueues { get; }
        Task<int> CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReboostDbContext _context;
        private DocumentRespository _documentRepository;
        private RequestQueueRepository _requestQueueRepository;

        public UnitOfWork(ReboostDbContext context)
        {
            this._context = context;
        }

        public IDocumentRepository Documents => _documentRepository = _documentRepository ?? new DocumentRespository(_context);
        public IRequestQueueRepository RequestQueues => _requestQueueRepository = _requestQueueRepository ?? new RequestQueueRepository(_context);

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
