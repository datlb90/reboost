using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reboost.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reboost.WebApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.Security.Claims;
using Reboost.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IMailService _mailService;
        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IMailService mailService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _authService = authService;
            _mailService = mailService;
            _configuration = configuration;
            _logger = logger;
        }

        // /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(model);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            return BadRequest("Đã có lỗi xảy ra trong quá trình đăng ký. Bạn vui lòng thử lại và liên hệ với chúng tôi nếu vẫn không thể đăng ký được."); // Status code: 400
        }

        // /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Đã có lỗi xảy ra trong quá trình đăng nhập.");
        }

        // /api/auth/external
        [HttpPost("external/{provider}/{role}")]
        public IActionResult ExternalLogin(string provider, string role, [FromQueryAttribute] string returnUrl)
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "role", role },
                    { "scheme", provider },
                }
            };

            return Challenge(props, provider);
        }

        [HttpGet("external/callback")]
        public async Task<IActionResult> Callback()
        {
            try
            {
                // read external identity from the temporary cookie
                var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
                if (result?.Succeeded != true)
                {
                    _logger.LogInformation("External login callback error: authentication failed.");
                    HttpContext.Response.Cookies.Append("auth_error", "Authentication Failed");
                    Response.Redirect("/auth/error");
                }

                var response = await _authService.LoginExternalAsync(result);
                if (response.IsSuccess)
                {
                    // Send user info to frontend using Cookies
                    HttpContext.Response.Cookies.Append("userId", response.user.Id, new CookieOptions { IsEssential = true });
                    HttpContext.Response.Cookies.Append("username", response.user.Username, new CookieOptions { IsEssential = true });
                    HttpContext.Response.Cookies.Append("email", response.user.Email, new CookieOptions { IsEssential = true });
                    HttpContext.Response.Cookies.Append("firstName", response.user.FirstName, new CookieOptions { IsEssential = true });
                    HttpContext.Response.Cookies.Append("lastName", response.user.LastName, new CookieOptions { IsEssential = true });
                    HttpContext.Response.Cookies.Append("role", response.user.Role, new CookieOptions { IsEssential = true });
                    HttpContext.Response.Cookies.Append("token", response.Message, new CookieOptions { IsEssential = true });
                    HttpContext.Response.Cookies.Append("expireDate", response.user.ExpireDate.ToString(), new CookieOptions { IsEssential = true });
                    var returnUrl = HttpUtility.UrlDecode(result.Properties.Items["returnUrl"]) ?? "";
                    HttpContext.Response.Cookies.Append("returnUrl", returnUrl, new CookieOptions { IsEssential = true });
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect("/after-login");
                    }
                    else if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        _logger.LogInformation("External login callback error: invalid return URL.");
                        // user might have clicked on a malicious link - should be logged
                        HttpContext.Response.Cookies.Append("auth_error", "Invalid return URL");
                        return Redirect("/login");
                    }
                }
                else
                {
                    _logger.LogInformation("External login callback error: unable to login.");
                    HttpContext.Response.Cookies.Append("auth_error", "Unable to login");
                    return Redirect("/login");
                }
            }
            catch(Exception exception)
            {
                _logger.LogInformation("External login callback error: " + exception.InnerException.Message);
                return Redirect("/login");
            }
        }

        [HttpGet("external/user")]
        public async Task<IActionResult> GetExternalUser()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            var currentUserClaim = HttpContext.User;
            var userEmail = currentUserClaim.FindFirst("Email");
            //var currentUser = await _userService.GetByEmailAsync(userEmail.Value);
            
            return Ok();
        }

        // /api/auth/confirm_email?userid&token
        [HttpGet("confirm/email")]
        public async Task<IActionResult> ConfirmEmail(string id, string code)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(code))
                return NotFound();

            var result = await _authService.ConfirmEmailAsync(id, code);

            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["ClientUrl"]}/login?email=confirmed");
            }

            return BadRequest(result);
        }

        // api/auth/forgot/password
        [HttpPost("forgot/password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return NotFound();

            var result = await _authService.ForgotPasswordAsync(model);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        // api/auth/reset/password
        [HttpPost("reset/password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ResetPasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Dữ liệu cung cấp không hợp lệ");
        }
    }
}
