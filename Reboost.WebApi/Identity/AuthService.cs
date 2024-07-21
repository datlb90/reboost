
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
using Microsoft.Extensions.Logging;
using Reboost.WebApi.Controllers;
using OpenAI_API.Moderation;
using Microsoft.AspNetCore.Mvc;

namespace Reboost.WebApi.Identity
{
    public interface IAuthService
    {
        Task<UserManagerResponse> ResendEmailVerification(string userId);

        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);

        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);

        Task<UserManagerResponse> LoginExternalAsync(AuthenticateResult result, string ipAddress);

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
        private ISubscriptionService _subscriptionService;
        private IStripeService _stripeService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration, 
            IMailService mailService, 
            IUserService userService,
            IStripeService stripeService,
            ISubscriptionService subscriptionService,
            ILogger<AuthService> logger)
        {
            _userManger = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _userService = userService;
            _stripeService = stripeService;
            _subscriptionService = subscriptionService;
            _logger = logger;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                if (model == null)
                    throw new NullReferenceException("Dữ liệu không hợp lệ. Vui lòng nhập đầy đủ dữ liệu.");

                // Check if phone number exist
                bool usernameExist = await _userService.UsernameExist(model.Email, model.PhoneNumber);

                if (usernameExist)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Địa chỉ email hoặc số điện thoại đã được sử dụng.");

                    _logger.LogInformation("Registration Error: username already exists.");

                    return new UserManagerResponse
                    {
                        Message = "Lỗi đăng ký tài khoản",
                        IsSuccess = false,
                        Errors = errors
                    };
                }
                else
                {
                    var identityUser = new ApplicationUser
                    {
                        Email = model.Email,
                        EmailConfirmed = false,
                        UserName = GetUsernameFromEmail(model.Email),
                        FirstName = model.FullName,
                        LastName = "",
                        IpAddress = model.IpAddress,
                        PhoneNumber = model.PhoneNumber,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        FreeToken = 2,
                        PremiumToken = 0
                    };

                    // Create the user account
                    var result = await _userManger.CreateAsync(identityUser, model.Password);

                    if (result.Succeeded)
                    {
                        var roleResult = await _userManger.AddToRoleAsync(identityUser, model.Role);

                        if (roleResult.Succeeded)
                        {
                            // Add user score
                            List<UserScores> scores = new List<UserScores>();
                            UserScores writing = new UserScores
                            {
                                SectionId = 8,
                                Score = 0,
                                UpdatedDate = DateTime.UtcNow,
                                UserId = identityUser.Id
                            };
                            scores.Add(writing);
                            UserScores speaking = new UserScores
                            {
                                SectionId = 7,
                                Score = 0,
                                UpdatedDate = DateTime.UtcNow,
                                UserId = identityUser.Id
                            };
                            scores.Add(speaking);
                            UserScores listening = new UserScores
                            {
                                SectionId = 6,
                                Score = 0,
                                UpdatedDate = DateTime.UtcNow,
                                UserId = identityUser.Id
                            };
                            scores.Add(listening);
                            UserScores reading = new UserScores
                            {
                                SectionId = 5,
                                Score = 0,
                                UpdatedDate = DateTime.UtcNow,
                                UserId = identityUser.Id
                            };
                            scores.Add(reading);
                            await _userService.AddScoreAsync(identityUser.Id, scores);

                            // Send account verificaton email
                            await SendEmailVerification(identityUser);
                            // Generate access token
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
                                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                             );

                            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                            // Get user role
                            var roles = await _userManger.GetRolesAsync(identityUser);
                            if (roles == null)
                            {
                                _logger.LogInformation("Registration Error: unable to retrieve user role.");
                                return new UserManagerResponse
                                {
                                    Message = "Đã xảy ra lỗi trong quá trình tạo tài khoản, xin vui lòng liên hệ với chúng tôi tại support@reboost.vn",
                                    IsSuccess = false,
                                };
                            }
                            // Record user login
                            await _userService.RecordUserLogin(identityUser.Id);

                            UserLoginModel userModel = new UserLoginModel
                            {
                                Id = identityUser.Id,
                                Username = identityUser.UserName,
                                Email = identityUser.Email,
                                Role = roles.FirstOrDefault(),
                                Token = tokenAsString,
                                ExpireDate = token.ValidTo,
                                FirstName = model.FullName,
                                LastName = "",
                                FreeToken = 2,
                                PremiumToken = 0,
                                Subscription = null
                            };

                            return new UserManagerResponse
                            {
                                user = userModel,
                                Message = "Tài khoản đã được tạo thành công",
                                IsSuccess = true,
                            };
                        }
                        else
                        {
                            _logger.LogInformation("Registration Error: unable to assign user role.");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Registration Error: unable to create user account");
                    }

                    // Registration error because of role assignment or account creation

                    List<string> errorList = new List<string>();

                    if (result != null && result.Errors != null && result.Errors.Count() > 0)
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
            }
            catch (Exception exception)
            {
                _logger.LogInformation("Registration Error: " + exception.InnerException.Message);

                List<string> errorList = new List<string>();
                errorList.Add("Đã xảy ra lỗi trong quá trình tạo tài khoản, xin vui lòng liên hệ với chúng tôi tại support@reboost.vn");
                return new UserManagerResponse
                {
                    Message = "Lỗi đăng ký tài khoản",
                    IsSuccess = false,
                    Errors = errorList
                };
            }
        }

        public async Task<UserManagerResponse> LoginExternalAsync(AuthenticateResult authResult, string ipAddress)
        {
            try
            {
                // Lookup our user and external provider info
                var externalUser = authResult.Principal;
                if (externalUser != null)
                {
                    var emailClaim = externalUser.FindFirst(ClaimTypes.Email);
                    if (emailClaim != null)
                    {
                        var role = authResult.Properties.Items["role"];
                        var email = emailClaim.Value;

                        _logger.LogInformation("External login email: " + email);

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
                                EmailConfirmed = true,
                                UserName = username,
                                FirstName = firstName == null ? username : firstName,
                                LastName = lastName == null ? "" : lastName,
                                IpAddress = ipAddress,
                                CreatedDate = DateTime.UtcNow,
                                UpdatedDate = DateTime.UtcNow,
                                FreeToken = 2,
                                PremiumToken = 0
                            };
                            var result = await _userManger.CreateAsync(identityUser);
                            if (result.Succeeded)
                            {
                                user = identityUser;
                                _logger.LogInformation("External user account created: " + user.UserName);
                                
                                // Assign role to user
                                await _userManger.AddToRoleAsync(user, role);

                                // Add user score
                                List<UserScores> scores = new List<UserScores>();
                                UserScores writing = new UserScores
                                {
                                    SectionId = 8,
                                    Score = 0,
                                    UpdatedDate = DateTime.UtcNow,
                                    UserId = user.Id
                                };
                                scores.Add(writing);
                                UserScores speaking = new UserScores
                                {
                                    SectionId = 7,
                                    Score = 0,
                                    UpdatedDate = DateTime.UtcNow,
                                    UserId = user.Id
                                };
                                scores.Add(speaking);
                                UserScores listening = new UserScores
                                {
                                    SectionId = 6,
                                    Score = 0,
                                    UpdatedDate = DateTime.UtcNow,
                                    UserId = user.Id
                                };
                                scores.Add(listening);
                                UserScores reading = new UserScores
                                {
                                    SectionId = 5,
                                    Score = 0,
                                    UpdatedDate = DateTime.UtcNow,
                                    UserId = user.Id
                                };
                                scores.Add(reading);
                                await _userService.AddScoreAsync(user.Id, scores);
                                // Send account verification email
                                await SendEmailVerification(user);

                                _logger.LogInformation("External account verification email sent.");
                            }
                            else
                            {
                                _logger.LogInformation("External login error: unable to create user account");
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

                        // Record user login
                        await _userService.RecordUserLogin(user.Id);

                        UserLoginModel userModel = new UserLoginModel
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Role = role,
                            Token = tokenAsString,
                            ExpireDate = token.ValidTo,
                            FreeToken = user.FreeToken,
                            PremiumToken = user.PremiumToken,
                            Subscription = await _subscriptionService.GetUserSubscriptionModel(user.Id)
                        };

                        return new UserManagerResponse
                        {
                            user = userModel,
                            Message = tokenAsString,
                            IsSuccess = true,
                        };
                    }
                    else
                    {
                        _logger.LogInformation("External login error: unable to find email claim.");
                    }
                }
                else
                {
                    _logger.LogInformation("External login error: unable to find external user.");
                }

                return new UserManagerResponse
                {
                    IsSuccess = false
                };
            }
            catch(Exception exception)
            {
                _logger.LogInformation("External login error: " + exception.InnerException.Message);
                return new UserManagerResponse
                {
                    IsSuccess = false
                };
            }
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            try
            {
                var user = _userManger.Users.Where(u => u.Email == model.Username || u.PhoneNumber == model.Username).FirstOrDefault();

                if (user == null)
                {
                    _logger.LogInformation("User login error: incorrect username - " + model.Username);
                    return new UserManagerResponse
                    {
                        Message = "Email hoặc số điện thoại không chính xác. Vui lòng nhập lại.",
                        IsSuccess = false,
                    };
                }

                var result = await _userManger.CheckPasswordAsync(user, model.Password);
                if (!result)
                {
                    _logger.LogInformation("User login error: incorrect password - " + model.Username);
                    return new UserManagerResponse
                    {
                        Message = "Mật khẩu không chính xác. Vui lòng nhập lại.",
                        IsSuccess = false,
                    };
                }

                if (user.EmailConfirmed == false)
                {
                    return new UserManagerResponse
                    {
                        Message = "Vui lòng xác minh địa chỉ email qua đường link đã được gửi tới email của bạn",
                        IsSuccess = false,
                    };
                }

                var roles = await _userManger.GetRolesAsync(user);

                if (roles == null)
                {
                    _logger.LogInformation("User login error: unable to retrieve user role.");
                    return new UserManagerResponse
                    {
                        Message = "Unable to retrieve user role",
                        IsSuccess = false,
                    };
                }

                var claims = new[]
                {
                    new Claim("Email", user.Email),
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

                // Record user login
                await _userService.RecordUserLogin(user.Id);

                UserLoginModel userModel = new UserLoginModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault(),
                    Token = tokenAsString,
                    ExpireDate = token.ValidTo,
                    FreeToken = user.FreeToken,
                    PremiumToken = user.PremiumToken,
                    Subscription = await _subscriptionService.GetUserSubscriptionModel(user.Id)
                };

                return new UserManagerResponse
                {
                    user = userModel,
                    Message = "Đăng nhập thành công",
                    IsSuccess = true,
                };
            }
            catch(Exception exception)
            {
                _logger.LogInformation("User login error: " + exception.InnerException.Message);
                return new UserManagerResponse
                {
                    Message = "Email hoặc số điện thoại không chính xác. Vui lòng nhập lại.",
                    IsSuccess = false,
                };
            }
        }

        public async Task<UserManagerResponse> ResendEmailVerification(string userId)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user != null)
            {
                // Send email with account confirmation
                var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(user);

                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                string url = $"{_configuration["AppUrl"]}/api/auth/confirm/email?id={user.Id}&code={validEmailToken}";

                string message = $"<p>Xin chào " + user.FirstName + ",</p>" +
                                $"<p>Chào mừng bạn đến với Reboost!</p>" +
                                $"<p>Để hoàn thiện quá trình đăng ký tài khoản, bạn vui lòng xác nhận địa chỉ email bằng cách nhấp vào đường dẫn dưới đây:</p>" +
                                $"<p><a href='{url}'>Đường dẫn xác nhận địa chỉ email</a></p>" +
                                $"<p>Nếu bạn không yêu cầu đăng ký tài khoản, bạn có thể bỏ qua email này.</p>" +
                                $"<p>Xin chân thành cảm ơn!</p>" +
                                $"<p>Reboost Support</p>";

                await _mailService.SendEmailAsync(user.Email, "Xác nhận địa chỉ email", message);

                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Email xác nhận đã được gửi lại thành công.",
                };

            }

            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Đã có lỗi xảy ra trong quá trình gửi lại email."
            };

        }

        public async Task SendEmailVerification(ApplicationUser user)
        {
            // Send email with account confirmation
            var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = $"{_configuration["AppUrl"]}/api/auth/confirm/email?id={user.Id}&code={validEmailToken}";

            string message = $"<p>Xin chào " + user.FirstName + ",</p>" +
                            $"<p>Chào mừng bạn đến với Reboost!</p>" +
                            $"<p>Để hoàn thiện quá trình đăng ký tài khoản, bạn vui lòng xác nhận địa chỉ email sử dụng đường dẫn dưới đây:</p>" +
                            $"<p><a href='{url}'>Đường dẫn xác nhận địa chỉ email</a></p>" +
                            $"<p>Nếu bạn không yêu cầu đăng ký tài khoản, bạn có thể bỏ qua email này.</p>" +
                            $"<p>Xin chân thành cảm ơn!</p>" +
                            $"<p>Reboost Support</p>";

            await _mailService.SendEmailAsync(user.Email, "Xác nhận địa chỉ email", message);
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                var user = await _userManger.FindByIdAsync(userId);
                if (user == null) {
                    _logger.LogInformation("Confirm email error: unable to find user.");
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Unable to find user."
                    };
                } 
                
                var decodedToken = WebEncoders.Base64UrlDecode(token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManger.ConfirmEmailAsync(user, normalToken);

                if (result.Succeeded)
                {
                    return new UserManagerResponse
                    {
                        Message = "Địa chỉ email đã được xác thực thành công",
                        IsSuccess = true,
                    };
                }
                else
                {
                    string errorDescription = "";
                    foreach(var error in result.Errors)
                    {
                        errorDescription += error.Description;
                    }
                    _logger.LogInformation("Confirm email error: unable to confirm email - " + errorDescription);

                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Đã có lỗi xảy ra trong quá trình xác thực email.",
                        Errors = result.Errors.Select(e => e.Description)
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.LogInformation("Confirm email error: " + exception.InnerException.Message);
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Đã có lỗi xảy ra trong quá trình xác thực email."
                };
            }
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
            try
            {
                var user = await _userManger.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogInformation("Reset password error: unable to find user.");
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Tài khoản không tồn tại",
                    };
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    _logger.LogInformation("Reset password error: passwords unmatched.");
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Mật khẩu và xác nhận mật khẩu không giống nhau",
                    };
                }
                    
                var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManger.ResetPasswordAsync(user, normalToken, model.NewPassword);

                if (result.Succeeded)
                {
                    return new UserManagerResponse
                    {
                        Message = "Mật khẩu đã được đặt lại thành công",
                        IsSuccess = true,
                    };
                }
                else
                {
                    string errorDescription = "";
                    foreach (var error in result.Errors)
                    {
                        errorDescription += error.Description;
                    }

                    _logger.LogInformation("Reset password error: unable reset password - " + errorDescription);
                    return new UserManagerResponse
                    {
                        Message = "Đã có lỗi xảy ra trong quá trình đặt lại mật khẩu",
                        IsSuccess = false,
                        Errors = result.Errors.Select(e => e.Description),
                    };
                }
               
            }
            catch(Exception exception)
            {
                _logger.LogInformation("Reset password error: " + exception.InnerException.Message);
                return new UserManagerResponse
                {
                    Message = "Đã có lỗi xảy ra trong quá trình đặt lại mật khẩu",
                    IsSuccess = false
                };
            }
        }

        private async Task<(IdentityUser user, string provider, string email, IEnumerable<Claim> claims)>FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var emailClaim = externalUser.FindFirst(ClaimTypes.Email);// ?? throw new Exception("Unknown email");

            if (emailClaim != null)
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
