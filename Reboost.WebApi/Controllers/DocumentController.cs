using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController
    {
        private readonly IDocumentService _docService;

        public DocumentController(IDocumentService docService)
        {
            _docService = docService;
        }

        [HttpPost]
        [Route("")]
        public async Task<Documents> Create(Documents newDocument)
        {
            return await _docService.Create(newDocument);
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
        public async Task<IEnumerable<Documents>> GetByStatus(int status)
        {
            return await _docService.GetByStatus(status);
        }
    }
}
