using Microsoft.AspNetCore.Mvc;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AuthenticationController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> RegisterUser([FromBody] RegisterRequestModel registerRequest)
        {
            try
            {
                AppValidator validator = new AppValidator();
                validator.CheckIfNull(registerRequest, nameof(registerRequest));
                validator.CheckName(registerRequest.FirstName, nameof(registerRequest.FirstName), 20);
                validator.CheckName(registerRequest.LastName, nameof(registerRequest.LastName), 20);
                validator.CheckName(registerRequest.UserName, nameof(registerRequest.UserName), 20, true);
                validator.CheckEmail(registerRequest.Email, nameof(registerRequest.Email), 50);
                validator.CheckPhoneNumber(registerRequest.PhoneNumber, nameof(registerRequest.PhoneNumber));
                validator.CheckPassword(registerRequest.Password, nameof(registerRequest.Password));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _authService.RegisterUser(registerRequest);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(exception));
            }
        }

        /// <summary>
        /// Login for User
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> LoginUser([FromBody] LoginRequestModel loginRequest)
        {
            try
            {
                AppValidator validator = new ();
                validator.CheckIfNull(loginRequest, nameof(loginRequest));
                validator.CheckEmail(loginRequest.Email, nameof(loginRequest.Email), 50);
                validator.CheckPassword(loginRequest.Password, nameof(loginRequest.Password));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _authService.LoginUser(loginRequest);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(exception));
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("password/reset")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> ResetPassword([FromQuery] string email)
        {
            try
            {
                AppValidator validator = new ();
                validator.CheckEmail(email, nameof(email), 50);
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _authService.ResetPassword(email);
                return StatusCode((int)response.StatusCode, response);
            }
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(exception));
            }
        }
    }
}
