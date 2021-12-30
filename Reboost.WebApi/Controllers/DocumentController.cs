using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _docService;
        private readonly ISubmissionService _submissionService;
        private readonly IReviewService _reviewService;

        public DocumentController(IDocumentService docService, ISubmissionService submissionService, IReviewService reviewService)
        {
            _docService = docService;
            _submissionService = submissionService;
            _reviewService = reviewService;
        }

        [HttpPost]
        [Route("")]
        public async Task<Documents> Create(DocumentRequestModel model)
        {
            var newDoc = await _docService.Create(model);
            var newSub = await _submissionService.CreateAsync(new Submissions {
                DocId = newDoc.Id,
                UserId = model.UserId,
                QuestionId = model.QuestionId,
                SubmittedDate = DateTime.Now,
                Type = "Submission",
                TimeSpentInSeconds = model.TimeSpentInSeconds,
                Status = "Submitted",
                UpdatedDate = DateTime.Now
            });

            await _reviewService.Create(new { revieweeId = model.UserId, submissionId = newSub.Id });

            return newDoc;
        }

        [HttpPost]
        [Route("pending")]
        public async Task<Documents> CreatePending(DocumentRequestModel model)
        {
            var newDoc = await _docService.Create(model);
            await _submissionService.CreateAsync(new Submissions
            {
                DocId = newDoc.Id,
                UserId = model.UserId,
                QuestionId = model.QuestionId,
                SubmittedDate = DateTime.Now,
                Type = "Submission",
                TimeSpentInSeconds = model.TimeSpentInSeconds,
                Status = "Pending",
                UpdatedDate = DateTime.Now
            });

            return newDoc;
        }

        [HttpPut]
        [Route("")]
        public async Task<Documents> Update(Documents document)
        {
            return await _docService.Update(document);
        }

        [HttpDelete]
        [Route("")]
        public async Task<Documents> Delete(Documents document)
        {
            return await _docService.Delete(document);
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Documents>> GetAllDocument()
        {
            return await _docService.GetAllDocument();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Documents> GetById(int id)
        {
            return await _docService.GetById(id);
        }

        [HttpGet]
        [Route("file_name/{fileName}")]
        public async Task<IEnumerable<Documents>> GetByFileName(string fileName)
        {
            return await _docService.GetByFileName(fileName);
        }

        [HttpGet]
        [Route("status/{status}")]
        public async Task<IEnumerable<Documents>> GetByStatus(string status)
        {
            return await _docService.GetByStatus(status);
        }

        [HttpGet]
        [Route("search/user/{userId}/question/{questionId}")]
        public async Task<IEnumerable<Documents>> SearchByUser(string userId, int questionId) {
            return await _docService.SearchByUser(userId, questionId);
        }

        [HttpGet]
        [Route("submission/{id}")]
        public async Task<Documents> GetSubmissionByIdAsync(int id)
        {
            return await _docService.GetSubmissionById(id);
        }

        [HttpPut]
        [Route("submission/{id}")]
        public async Task<Documents> UpdateDocumentBySubmissionId(int id, DocumentRequestModel model)
        {
            return await _docService.UpdateDocumentBySubmissionId(id, model);
        }
    }
}
