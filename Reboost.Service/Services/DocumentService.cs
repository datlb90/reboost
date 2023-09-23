using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;

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
        Task<IEnumerable<Documents>> SearchByUser(string userId, int questionId);
        Task<Documents> GetSubmissionById(int id);
        Task<Documents> UpdateDocumentBySubmissionId(int id, DocumentRequestModel data);
        Task<Documents> GetSavedDocument(string userId, int questionId);
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
        public async Task<IEnumerable<Documents>> SearchByUser(string userId, int questionId)
        {
            var rs = await _unitOfWork.Documents.SearchByUser(userId, questionId);
            return rs;
        }
        public async Task<Documents> GetSubmissionById(int id)
        {
            var rs = await _unitOfWork.Documents.GetSubmissionByIdAsync(id);
            return rs;
        }
        public async Task<Documents> UpdateDocumentBySubmissionId(int id, DocumentRequestModel data)
        {
            // data.Status = "Submitted"; Need Investigation
            data.Data = _pdfService.WriteParagraph(data.Text);

            var rs = await _unitOfWork.Documents.UpdateDocumentBySubmissionId(id, data);

            return rs;
        }
        public async Task<Documents> GetSavedDocument(string userId, int questionId)
        {
            var rs = await _unitOfWork.Documents.GetSavedDocument(userId, questionId);
            return rs;
        }
    }
}
