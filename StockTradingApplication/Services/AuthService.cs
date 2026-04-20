using Microsoft.AspNetCore.Identity;
using StockTradingApplication.Models;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;
using System.Net;

namespace StockTradingApplication.Services
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly StockTradingApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;
        public AuthService(StockTradingApplicationContext context,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        /// <summary>
        /// Register new User
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> RegisterUser(RegisterRequestModel registerRequest)
        {
            try
            { 
                //check for duplicate email
                if(_context.AspNetUsers.Any(user => user.NormalizedEmail == registerRequest.Email.ToUpper()))
                {
                    return new StandardResponseModel(
                        HttpStatusCode.Conflict,
                        AppMessages.EmailAlreadyRegistered,
                        new BadRequestResponseModel(BadRequestResponseModel.EmailAlreadyRegistered, "email", registerRequest.Email));
                }
                //check for duplicate username
                if (_context.AspNetUsers.Any(user => user.NormalizedUserName == registerRequest.UserName.ToUpper()))
                {
                    return new StandardResponseModel(
                        HttpStatusCode.Conflict,
                        AppMessages.UserNameAlreadyTaken,
                        new BadRequestResponseModel(BadRequestResponseModel.UsernameTaken, "username", registerRequest.UserName));
                }
                //register user to identity
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = registerRequest.UserName,
                    Email = registerRequest.Email,
                    PhoneNumber = registerRequest.PhoneNumber,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName
                };
                var result = await _userManager.CreateAsync(applicationUser,registerRequest.Password);
                if (!result.Succeeded)
                {
                    return new StandardResponseModel(HttpStatusCode.InternalServerError, AppMessages.RegistrationFailed, result.Errors);
                }
                //store user in tblUser
                Guid id = Guid.NewGuid();
                DateTime now = DateTime.UtcNow;
                TblUser user = new TblUser
                {
                    Id = id,
                    Email = registerRequest.Email,
                    PhoneNumber = registerRequest.PhoneNumber,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    CreatedBy = id,
                    ModifiedBy = id,
                    CreatedDtTm = now,
                    ModifiedDtTm = now
                };
                await _context.TblUsers.AddAsync(user);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.Created, AppMessages.RegistrationSuccess, new
                {
                    user.Id,
                    user.Email,
                    user.PhoneNumber,
                    user.FirstName,
                    user.LastName,
                });
            }
            catch(Exception exception)
            {
                return new StandardResponseModel(exception);
            }
        }

        /// <summary>
        /// Login for User
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> LoginUser(LoginRequestModel loginRequest)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginRequest.Email);
                //check if email exist
                if(user == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.EmailNotRegistered,
                        new BadRequestResponseModel(BadRequestResponseModel.EmailNotRegistered, "email", loginRequest.Email));
                }
                //if password not correct
                else if(!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                {
                    return new StandardResponseModel(HttpStatusCode.Unauthorized, AppMessages.InvalidCredentials,
                        new BadRequestResponseModel(BadRequestResponseModel.IncorrectPassword, "password", loginRequest.Password));
                }
                //Correct credentials
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);
                    if (!result.Succeeded)
                    {
                        return new StandardResponseModel(HttpStatusCode.InternalServerError, AppMessages.LoginFailed, AppMessages.InternalServerError );
                    }
                    return new StandardResponseModel(HttpStatusCode.OK, AppMessages.LoginSuccess, new
                    {
                        user.Id,
                        user.Email,
                        user.UserName
                    });
                }
            }
            catch(Exception exception)
            {
                return new StandardResponseModel(exception);
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> ResetPassword(string email)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(email);
                //check if email exist
                if (user == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.EmailNotRegistered,
                        new BadRequestResponseModel(BadRequestResponseModel.EmailNotRegistered, "email", email));
                }
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string newPassword = GenerateRandomPassword();
                await _userManager.ResetPasswordAsync(user, token, newPassword);

                bool isMailSent = await _mailService.SendResetPasswordMail(email, newPassword);
                if (!isMailSent)
                {
                    return new StandardResponseModel(HttpStatusCode.InternalServerError, AppMessages.SendMailFailed, AppMessages.InternalServerError);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.PasswordSentToMail, null);
                
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// method to generate random strong password
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomPassword()
        {
            Random random = new ();
            string newPassword = ((char)random.Next(65, 90)).ToString() + 
                                 ((char)random.Next(97, 122)).ToString() + "@" + random.Next(100, 999).ToString() +
                                 ((char)random.Next(65, 90)).ToString() + ((char)random.Next(97, 122)).ToString() +
                                 "@" + random.Next(100, 999).ToString();
            int randomIndex = random.Next(1, 11);
            newPassword = newPassword[0] + string.Concat(newPassword.Substring(randomIndex), newPassword.Substring(1, randomIndex - 1));
            return newPassword;
        }
    }
}
