using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;

namespace Reboost.Service.Services
{
    public interface IDocumentService
    {
        Task<Documents> Create(Documents newDocument);

        Task<Documents> Update(Documents document);

        Task<Documents> Delete(Documents document);

        Task<IEnumerable<Documents>> GetAllDocument();

        Task<Documents> GetById(int id);

        Task<IEnumerable<Documents>> GetByFileName(string fileName);

        Task<IEnumerable<Documents>> GetByStatus(string status);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPDFService _pdfService;

        public DocumentService(IUnitOfWork unitOfWork, IPDFService pdfServive)
        {
            this._unitOfWork = unitOfWork;
            _pdfService = pdfServive;
        }

        public async Task<Documents> Create(Documents newDocument)
        {
            newDocument.Status = "Submitted";
            newDocument.CreatedDate = DateTime.Now;
            newDocument.Data = _pdfService.WriteParagraph(newDocument.Text);

            return await _unitOfWork.Documents.Create(newDocument);
        }

        public async Task<Documents> Update(Documents document)
        {
            return await _unitOfWork.Documents.Update(document);
        }

        public async Task<Documents> Delete(Documents document)
        {
            return await _unitOfWork.Documents.Delete(document);
        }

        public async Task<IEnumerable<Documents>> GetAllDocument()
        {
            return await _unitOfWork.Documents.GetAllAsync();
        }

        public async Task<Documents> GetById(int id)
        {
            return await _unitOfWork.Documents.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Documents>> GetByFileName(string fileName)
        {
            return await _unitOfWork.Documents.GetByFileName(fileName);
        }

        public async Task<IEnumerable<Documents>> GetByStatus(string status)
        {
            return await _unitOfWork.Documents.GetByStatus(status);
        }
    }
}
