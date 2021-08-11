
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

        Task<UserManagerResponse> ForgetPasswordAsync(string email);

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
                throw new NullReferenceException("Reigster Model is null");

            //if (model.Password != model.ConfirmPassword)
            //    return new UserManagerResponse
            //    {
            //        Message = "Confirm password doesn't match the password",
            //        IsSuccess = false,
            //    };


            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = GetUsernameFromEmail(model.Email),
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManger.CreateAsync(identityUser, model.Password);
            if (result.Succeeded)
            {

                var roleResult = await _userManger.AddToRoleAsync(identityUser, model.Role);

                if (roleResult.Succeeded)
                {
                    var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(identityUser);

                    var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);



                    string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userid={identityUser.Id}&token={validEmailToken}";

                    await _mailService.SendEmailAsync(identityUser.Email, "Confirm your email", $"<h1>Welcome to Reboost</h1>" +
                        $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");


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
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                    string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                    var roles = await _userManger.GetRolesAsync(identityUser);
                    if (roles == null)
                    {
                        return new UserManagerResponse
                        {
                            Message = "User has no role",
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
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    };

                    return new UserManagerResponse
                    {
                        user = userModel,
                        Message = "Account Registration Success!",
                        IsSuccess = true,
                    };


                }

               
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };

        }

        public async Task<UserManagerResponse> LoginExternalAsync(AuthenticateResult authResult)
        {
            // lookup our user and external provider info
            var externalUser = authResult.Principal;

            // try to determine the unique email
            var emailClaim = externalUser.FindFirst(ClaimTypes.Email);// ?? throw new Exception("Unknown email");

            if (emailClaim != null)
            {
                // remove the user id claim so we don't include it as an extra claim if/when we provision the user
                var claims = externalUser.Claims.ToList();
                var provider = authResult.Properties.Items["scheme"];
                var email = emailClaim.Value;

                // find the external user
                var user = await _userManger.FindByEmailAsync(email);

                string userId = null;
                if (user == null)
                {
                    string firstName = claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                    string lastName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;

                    // Create new user
                    var identityUser = new ApplicationUser
                    {
                        Email = email,
                        UserName = GetUsernameFromEmail(email)
                        //FirstName = firstName == null ? email : firstName,
                        //LastName = lastName == null ? "" : lastName
                    };

                    var result = await _userManger.CreateAsync(identityUser);
                    if (result.Succeeded)
                    {
                        userId = identityUser.Id;
                        user = identityUser;
                    }
                        
                }
                else
                {
                    userId = user.Id;
                }
                var role = authResult.Properties.Items["role"];
                var roleResult = await _userManger.AddToRoleAsync(user, role);

                if (roleResult.Succeeded)
                {
                    var userClaims = new[]
                    {
                        new Claim("Email", email),
                        new Claim(ClaimTypes.NameIdentifier, userId)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:JwtSecret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["AuthSettings:JwtIssuer"],
                        audience: _configuration["AuthSettings:JwtAudience"],
                        claims: userClaims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                    string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                    var roles = await _userManger.GetRolesAsync(user);
                    if (roles == null)
                    {
                        return new UserManagerResponse
                        {
                            Message = "User has no role",
                            IsSuccess = false,
                        };
                    }

                    UserLoginModel userModel = new UserLoginModel
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Role = roles.FirstOrDefault(), // Each user has only one role
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
                
            }

            return new UserManagerResponse
            {
                IsSuccess = false
            };
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
                    Message = "Invalid email",
                    IsSuccess = false,
                };
            }

            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false,
                };
            }

            var result = await _userManger.CheckPasswordAsync(user, model.Password);
            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };

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
                expires: DateTime.Now.AddDays(30),
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

        public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

            await _mailService.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click here</a></p>");

            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = "Reset password URL has been sent to the email successfully!"
            };
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new UserManagerResponse
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new UserManagerResponse
            {
                Message = "Something went wrong",
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
