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
    [Route("api/[controller]")]
    public class RequestQueueController : ControllerBase
    {
        private readonly IRequestQueueService _requestQueueService;

        public RequestQueueController(IRequestQueueService requestQueueService)
        {
            _requestQueueService = requestQueueService;
        }

        [HttpGet]
        [Route("top")]
        public async Task<RequestQueue> GetTopPriorityRequest()
        {
            return await _requestQueueService.GetTopPriorityRequest();
        }
    }
}
