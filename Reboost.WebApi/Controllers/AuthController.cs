using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Reboost.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reboost.WebApi.Email;
using Reboost.WebApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.Security.Claims;

namespace Reboost.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IMailService _mailService;
        private IConfiguration _configuration;

        public AuthController(IAuthService authService, IMailService mailService, IConfiguration configuration)
        {
            _authService = authService;
            _mailService = mailService;
            _configuration = configuration;
        }

        // /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code: 200 

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400
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
                    //await _mailService.SendEmailAsync(model.Email, "New login", "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at " + DateTime.Now + "</p>");
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }

        // /api/auth/external
        [HttpPost("external/{provider}/{returnUrl}")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var props = new AuthenticationProperties
            {

                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", provider },
                    }
            };

            return Challenge(props, provider);
        }

        [HttpGet("external/callback")]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (result?.Succeeded != true)
            {
                HttpContext.Response.Cookies.Append("auth_error", "Authentication Failed");
                Response.Redirect("/auth/error");
            }

            var response = await _authService.LoginExternalAsync(result);
            if (response.IsSuccess)
            {
                HttpContext.Response.Cookies.Append("email", response.Email, new CookieOptions { IsEssential = true });
                HttpContext.Response.Cookies.Append("token", response.Message, new CookieOptions { IsEssential = true });
                HttpContext.Response.Cookies.Append("expireDate", response.ExpireDate.ToString(), new CookieOptions { IsEssential = true });

                var returnUrl = HttpUtility.UrlDecode(result.Properties.Items["returnUrl"]) ?? "~/";
                if (string.IsNullOrEmpty(returnUrl) || returnUrl == "~/")
                {
                    return Redirect("/");
                }
                else if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    // user might have clicked on a malicious link - should be logged
                    HttpContext.Response.Cookies.Append("auth_error", "Invalid return URL");
                    return Redirect("/auth/error");
                }
            }
            else
            {
                HttpContext.Response.Cookies.Append("auth_error", "Unable to login");
                return Redirect("/auth/error");
            }
           
        }

        

        // /api/auth/confirm_email?userid&token
        [HttpGet("confirm_email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _authService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");
            }

            return BadRequest(result);
        }

        // api/auth/forget_password
        [HttpPost("forget_password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _authService.ForgetPasswordAsync(email);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        // api/auth/reset_password
        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ResetPasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }


    }
}
