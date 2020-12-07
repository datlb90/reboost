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
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [ApiController]
    [Route("api/data/ingestion")]
    public class DataIngestionController : ControllerBase
    {
        private readonly IDataIngestionService _dataIngestionService;

        public DataIngestionController(IDataIngestionService dataIngestionService)
        {
            _dataIngestionService = dataIngestionService;
        }

        [HttpGet]
        [Route("questions/samples")]
        public async Task IngestQuestionsAndSamples()
        {
            await _dataIngestionService.IngestQuestionsAndSamples();
        }
    }
}
