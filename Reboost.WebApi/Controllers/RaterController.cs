using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Service.Services;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaterController : BaseController<IRaterService>
    {
        private readonly IMapper _mapper;
        private IUserService _userService;
        private IMailService _mailService;

        public RaterController(IRaterService service, IMapper mapper, IUserService userService, IMailService mailService) : base(service)
        {
            _mapper = mapper;
            _userService = userService;
            _mailService = mailService;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Raters>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<Raters> GetByIdAsync(int id)
        {
            var rater = await _service.GetByIdAsync(id);
            var applyTo = await _service.GetApplyTo(id);
            var raterModel = _mapper.Map<RaterResponseModel>(rater);
            raterModel.ApplyTo = applyTo;

            return raterModel;
        }
        [Authorize]
        [HttpGet]
        [Route("byCurrentUser")]
        public async Task<Raters> GetByCurrentUserAsync()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            var rater = await _service.GetByUserIdAsync(currentUser.Id);

            return rater;
        }
        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Raters> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id);
        }
        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<Raters> CreateAsync([FromForm] RaterRequestModel model)
        {
            var _rater = _mapper.Map<Raters>(model);
            return await _service.CreateAsync(_rater, model.UploadedFiles);
        }
        [Authorize]
        [HttpPost]
        [Route("update")]
        public async Task<Raters> UpdateAsync([FromForm] RaterRequestModel model)
        {
            var _rater = _mapper.Map<Raters>(model);
            return await _service.UpdateAsync(_rater, model.UploadedFiles);
        }
        [Authorize]
        [HttpPost]
        [Route("update/credential")]
        public async Task<Raters> UpdateCredentialAsync([FromForm] RaterRequestModel model)
        {
            var _rater = _mapper.Map<Raters>(model);
            return await _service.UpdateCredentialAsync(_rater, model.UploadedFiles);
        }
        [Authorize]
        [HttpGet("update/status/{id}/{status}")]
        public async Task<Raters> UpdateStatus(int id, string status) 
        {
            var rs = await _service.UpdateStatusAsync(id, status);
            var user = await _userService.GetByIdAsync(rs.UserId);

            await _mailService.SendEmailAsync(user.Email, "Application's Status Updated", "Your application has just been " + rs.Status + ". Please visit your application page for more infomation.");

            return rs;
        }
        [Authorize]
        [HttpGet("rating")]
        public async Task<decimal> GetRatingAsync()
        {
            var currentUserClaim = HttpContext.User;
            var email = currentUserClaim.FindFirst("Email");
            var currentUser = await _userService.GetByEmailAsync(email.Value);

            return await _service.GetRaterRatingAsync(currentUser.Id);
        }
    }
}
