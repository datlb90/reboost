
using Reboost.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Reboost.DataAccess.Entities;
using Reboost.Service.Services;
using Stripe;
using System.Text.RegularExpressions;

namespace Reboost.WebApi.Identity
{
    public interface IAuthService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);

        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);

        Task<UserManagerResponse> LoginExternalAsync(AuthenticateResult result);

        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);

        Task<UserManagerResponse> ForgotPasswordAsync(ForgotPasswordModel model);

        Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model);
    }

    public class AuthService : IAuthService
    {
        private UserManager<ApplicationUser> _userManger;
        private IConfiguration _configuration;
        private IMailService _mailService;
        private IUserService _userService;
        private IStripeService _stripeService;
        public AuthService(
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration, 
            IMailService mailService, 
            IUserService userService,
            IStripeService stripeService)
        {
            _userManger = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _userService = userService;
            _stripeService = stripeService;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Dữ liệu không hợp lệ. Vui lòng nhập đầy đủ dữ liệu.");
            
            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = GetUsernameFromEmail(model.Email),
                FirstName = model.FullName,
                LastName = "",
                PhoneNumber = model.PhoneNumber
            };
            var result = await _userManger.CreateAsync(identityUser, model.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManger.AddToRoleAsync(identityUser, model.Role);

                if (roleResult.Succeeded)
                {
                    // Send account verificaton email
                    await SendEmailVerification(identityUser);
                    
                    var claims = new[]
                    {
                        new Claim("Email", model.Email),
                        new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                        new Claim("Role", model.Role)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:JwtSecret"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["AuthSettings:JwtIssuer"],
                        audience: _configuration["AuthSettings:JwtAudience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(30),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                    string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                    var roles = await _userManger.GetRolesAsync(identityUser);
                    if (roles == null)
                    {
                        return new UserManagerResponse
                        {
                            Message = "Đã xảy ra lỗi trong quá trình tạo tài khoản, xin vui lòng liên hệ với chúng tôi tại support@reboost.vn",
                            IsSuccess = false,
                        };
                    }
                    UserLoginModel userModel = new UserLoginModel
                    {
                        Id = identityUser.Id,
                        Username = identityUser.UserName,
                        Email = identityUser.Email,
                        Role = roles.FirstOrDefault(), // Each user has only one role
                        Token = tokenAsString,
                        ExpireDate = token.ValidTo,
                        FirstName = model.FullName,
                        LastName = ""
                    };
                    return new UserManagerResponse
                    {
                        user = userModel,
                        Message = "Tài khoản đã được tạo thành công",
                        IsSuccess = true,
                    };
                }
            }

            List<string> errorList = new List<string>();
            

            if(result != null && result.Errors != null && result.Errors.Count() > 0)
            {
                if (result.Errors.First().Code == "DuplicateUserName")
                {
                    errorList.Add("Email đã được sử dụng, bạn hãy chọn một email khác.");
                }
            }
            else
            {
                errorList.Add("Đã xảy ra lỗi trong quá trình tạo tài khoản, xin vui lòng liên hệ với chúng tôi tại support@reboost.vn");
            }

            return new UserManagerResponse
            {
                Message = "Lỗi đăng ký tài khoản",
                IsSuccess = false,
                Errors = errorList
            };
        }
        public async Task<UserManagerResponse> LoginExternalAsync(AuthenticateResult authResult)
        {
            // Lookup our user and external provider info
            var externalUser = authResult.Principal;
            var emailClaim = externalUser.FindFirst(ClaimTypes.Email);
            if (emailClaim != null)
            {
                var role = authResult.Properties.Items["role"];
                var email = emailClaim.Value;
                // Find the external user in db
                var user = await _userManger.FindByEmailAsync(email);
                if (user == null)
                {
                    var claims = externalUser.Claims.ToList();
                    string firstName = claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                    string lastName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                    string username = GetUsernameFromEmail(email);
                    // Create the new user
                    var identityUser = new ApplicationUser
                    {
                        Email = email,
                        UserName = username,
                        FirstName = firstName == null ? username : firstName,
                        LastName = lastName == null ? "" : lastName
                    };
                    var result = await _userManger.CreateAsync(identityUser);
                    if (result.Succeeded)
                    {
                        user = identityUser;
                        // Assign role to user
                        await _userManger.AddToRoleAsync(user, role);
                        // Send account verification email
                        await SendEmailVerification(user);
                    }
                    else
                    {
                        return new UserManagerResponse
                        {
                            IsSuccess = false
                        };
                    }
                }
                var userClaims = new[]
                    {
                        new Claim("Email", email),
                        new Claim(ClaimTypes.NameIdentifier,  user.Id),
                        new Claim("Role", role)
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:JwtSecret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:JwtIssuer"],
                    audience: _configuration["AuthSettings:JwtAudience"],
                    claims: userClaims,
                    expires: DateTime.UtcNow.AddDays(30),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
                UserLoginModel userModel = new UserLoginModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = role,
                    Token = tokenAsString,
                    ExpireDate = token.ValidTo
                };
                return new UserManagerResponse
                {
                    user = userModel,
                    Message = tokenAsString,
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                IsSuccess = false
            };
        }
        public async Task SendEmailVerification(ApplicationUser user)
        {
            // Send email with account confirmation
            var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            //string url = $"{_configuration["AppUrl"]}/api/auth/confirm/email?id={user.Id}&code={validEmailToken}";

            //await _mailService.SendEmailAsync(user.Email, "Xác nhận địa chỉ email", $"<h3>Welcome to Reboost!</h3>" +
            //    $"<p>To finish setting up your Reboost account, we just need to make sure this email address is yours.</p>" +
            //    $"<p>To verify your email address, please click on this link: <a href='{url}'>confirm my email</a></p>" +
            //    $"<p>If you didn't make this request, you can safely ignore this email.</p>" +
            //    $"<p>Thanks,</p><p>Reboost Support Team</p>");

            string url = $"{_configuration["AppUrl"]}/api/auth/confirm/email?id={user.Id}&code={validEmailToken}";

            string message = $"<p>Xin chào " + user.FirstName + ",</p>" +
                            $"<p>Chào mừng bạn đên với Reboost!</p>" +
                            $"<p>Để hoàn thiện quá trình đăng ký tài khoản, bạn vui lòng xác nhận địa chỉ email sử dụng đường dẫn dưới đây:</p>" +
                            $"<p><a href='{url}'>Đường dẫn xác nhận địa chỉ email</a></p>" +
                            $"<p>Nếu bạn không yêu cầu đăng ký tài khoản, bạn có thể bỏ qua email này.</p>" +
                            $"<p>Xin chân thành cảm ơn!</p>" +
                            $"<p>Reboost Support</p>";

            await _mailService.SendEmailAsync(user.Email, "Xác nhận địa chỉ email", message);
        }

        private async Task<(IdentityUser user, string provider, string email, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var emailClaim = externalUser.FindFirst(ClaimTypes.Email);// ?? throw new Exception("Unknown email");

            if(emailClaim != null)
            {
                // remove the user id claim so we don't include it as an extra claim if/when we provision the user
                var claims = externalUser.Claims.ToList();
                var provider = result.Properties.Items["scheme"];
                var email = emailClaim.Value;

                // find the external user
                var user = await _userManger.FindByEmailAsync(email);
                return (user, provider, email, claims);
            }
            else
            {
                return (null, null, null, null);
            }
            
        }


        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!regex.IsMatch(model.Email))
            {
                return new UserManagerResponse
                {
                    Message = "Địa chỉ email không chính xác. Vui lòng nhập lại.",
                    IsSuccess = false,
                };
            }

            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "Địa chỉ email không chính xác. Vui lòng nhập lại.",
                    IsSuccess = false,
                };
            }

            var result = await _userManger.CheckPasswordAsync(user, model.Password);
            if (!result)
                return new UserManagerResponse
                {
                    Message = "Mật khẩu không chính xác. Vui lòng nhập lại.",
                    IsSuccess = false,
                };

            if(user.EmailConfirmed == false)
            {
                return new UserManagerResponse
                {
                    Message = "Vui lòng xác minh địa chỉ email qua đường link đã được gửi tới email của bạn",
                    IsSuccess = false,
                };
            }

            var roles = await _userManger.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Role", roles.FirstOrDefault())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:JwtSecret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:JwtIssuer"],
                audience: _configuration["AuthSettings:JwtAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            if (roles == null)
            {
                return new UserManagerResponse
                {
                    Message = "User has no role",
                    IsSuccess = false,
                };
            }

            User _user = await _userService.GetByEmailAsync(user.Email);
            //var account = await _stripeService.GetAccount(_user.Id);

            UserLoginModel userModel = new UserLoginModel
            {
                Id = user.Id,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault(), // Each user has only one role
                Token = tokenAsString,
                ExpireDate = token.ValidTo,
                StripeCustomerId = _user.StripeCustomerID
            };

            StripeList<Subscription> listSubs = await _stripeService.GetSubscriptionAsync(userModel.StripeCustomerId);
            userModel.IsSubcribed = listSubs.Data.Count > 0;
            

            return new UserManagerResponse
            {
                user = userModel,
                Message = "Login Success!",
                IsSuccess = true,
            };
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return new UserManagerResponse
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true,
                };

            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Email did not confirm",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Tài khoản không tồn tại",
                };

            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["ClientUrl"]}/reset/password?email={model.Email}&token={validToken}";

            string message = $"<p>Xin chào " + user.FirstName + ",</p>" +
                            $"<p>Bạn đã yêu cầu đặt lại mật khẩu trên Reboost.</p>" +
                            $"<p>Vui lòng sử dụng đường dẫn bên dưới để tạo mật khẩu mới cho tài khoản của bạn:</p>" +
                            $"<p><a href='{url}'>Đường dẫn để thay đổi mật khẩu</a></p>" +
                            $"<p>Nếu bạn không yêu cầu đổi mật khẩu, bạn có thể bỏ qua email này.</p>" +
                            $"<p>Xin chân thành cảm ơn!</p>" +
                            $"<p>Reboost Support</p>";

            await _mailService.SendEmailAsync(model.Email, "Yêu cầu đặt lại mật khẩu", message);

            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = "Chúng tôi đã gửi hướng dẫn thay đổi mật khẩu qua email của bạn"
            };
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Tài khoản không tồn tại",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Mật khẩu và xác nhận mật khẩu không giống nhau",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new UserManagerResponse
                {
                    Message = "Mật khẩu đã được đặt lại thành công",
                    IsSuccess = true,
                };

            return new UserManagerResponse
            {
                Message = "Đã có lỗi xảy ra trong quá trình đặt lại mật khẩu",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        public static string GetUsernameFromEmail(string text, string stopAt = "@")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
    }
}
