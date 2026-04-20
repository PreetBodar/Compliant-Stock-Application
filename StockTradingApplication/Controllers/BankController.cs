using Microsoft.AspNetCore.Mvc;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Controllers
{
    /// <summary>
    /// Bank Controller
    /// </summary>
    [ApiController]
    [Route("banks")]
    public class BankController(IBankService bankService) : Controller
    {
        private readonly IBankService _bankService = bankService;

        /// <summary>
        /// Add or edit bank
        /// </summary>
        /// <param name="bankRequest"></param>
        /// <remarks>Dont send Id for add</remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> AddOrEditBank([FromBody] BankRequestModel bankRequest)
        {
            try 
            {
                var validator = new AppValidator();
                validator.CheckIfNull(bankRequest, nameof(bankRequest));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }
                validator.CheckGuid(bankRequest.UserId, nameof(bankRequest.UserId));
                if (bankRequest.Id != Guid.Empty) validator.CheckGuid(bankRequest.Id, nameof(bankRequest.Id));
                if(bankRequest.Id == Guid.Empty)
                {
                    validator.CheckIfNull(bankRequest.BankName, nameof(bankRequest.BankName));
                    validator.CheckIfNull(bankRequest.IsActive, nameof(bankRequest.IsActive));
                }
                validator.CheckLengthIfNotNull(bankRequest.BankName, nameof(bankRequest.BankName),40);
                if(validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _bankService.AddOrEditBank(bankRequest);
                return StatusCode((int)response.StatusCode, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Add bank details for user 
        /// </summary>
        /// <remarks>
        ///     Account Types : 
        ///     1 - Saving
        ///     2 - Brokerage
        ///     3 - Checking
        /// </remarks>
        /// <param name="userBankRequest"></param>
        /// <returns></returns>
        [Route("user")]
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> AddOrEditUserBankDetails([FromBody] UserBankRequestModel userBankRequest)
        {
            try
            {
                var validator = new AppValidator();
                validator.CheckIfNull(userBankRequest, nameof(userBankRequest));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                validator.CheckGuid(userBankRequest.UserId, nameof(userBankRequest.UserId));
                if (userBankRequest.Id != Guid.Empty) validator.CheckGuid(userBankRequest.Id, nameof(userBankRequest.Id));
                else
                {
                    validator.CheckGuid(userBankRequest.BankId, nameof(userBankRequest.BankId));
                    validator.CheckIfNull(userBankRequest.BankAccountNo, nameof(userBankRequest.BankAccountNo));
                    validator.CheckIfNull(userBankRequest.IsActive, nameof(userBankRequest.IsActive));
                    validator.CheckIfNull(userBankRequest.AccountHolderName, nameof(userBankRequest.AccountHolderName));
                    validator.CheckAccountType(userBankRequest.AccountType);
                    validator.CheckIfNull(userBankRequest.BankAccountNo, nameof(userBankRequest.BankAccountNo));
                }
                validator.CheckAccountType(userBankRequest.AccountType, true);
                validator.CheckLengthIfNotNull(userBankRequest.AccountHolderName, nameof(userBankRequest.AccountHolderName),40);
                validator.CheckAccountNumber(userBankRequest.BankAccountNo,true);

                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _bankService.AddOrEditUserBankDetails(userBankRequest);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get bank details for user
        /// </summary>
        /// <param name="id">user bank Id.</param>
        /// <returns></returns>
        [Route("user/{id}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetUserBankDetails([FromRoute] Guid id)
        {
            try
            {
                var validator = new AppValidator();
                validator.CheckGuid(id, nameof(id));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _bankService.GetUserBankDetails(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Bank by id
        /// </summary>
        /// <param name="id">Bank id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetBank([FromRoute] Guid id)
        {
            try
            {
                var validator = new AppValidator();
                validator.CheckGuid(id, nameof(id));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _bankService.GetBank(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// delete Bank by id
        /// </summary>
        /// <param name="id">Bank id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<StandardResponseModel>> DeleteBank([FromRoute] Guid id)
        {
            try
            {
                var validator = new AppValidator();
                validator.CheckGuid(id, nameof(id));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _bankService.DeleteBank(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get All Banks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetAllBanks()
        {
            try
            {
                var response = await _bankService.GetAllBanks();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get dyanmic bank data with pagination,search and sort
        /// </summary>
        /// <param name="page">page no</param>
        /// <param name="pageSize">no. of records per page</param>
        /// <param name="orderByColumn"> columns => bankname </param>
        /// <param name="sortDirection"> ascending / descending </param>
        /// <param name="searchText"> bank name</param>
        /// <returns></returns>
        [Route("paged")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetDynamicBankData(
            int page = 1,
            int pageSize = 5,
            string? orderByColumn = null,
            string sortDirection = "ascending",
            string? searchText = null
            )
        {
            try
            {
                var validOrderByColumns = new List<string> { "bankname" };
                var validator = new AppValidator();
                validator.CheckPaginationParameters(page, pageSize, orderByColumn, sortDirection, validOrderByColumns);
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }
                var response = await _bankService.GetDynamicBankData(page,pageSize,orderByColumn,sortDirection,searchText);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }
    }
}
