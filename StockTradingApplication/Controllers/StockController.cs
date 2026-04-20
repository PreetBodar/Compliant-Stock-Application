using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Controllers
{
    /// <summary>
    /// Stock Controller
    /// </summary>
    [Route("stocks")]
    [ApiController]
    public class StockController(IStockService stockService) : Controller
    {
        private readonly IStockService _stockService = stockService;

        /// <summary>
        /// Get Stock by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetStock([FromRoute] Guid id)
        {
            try
            {
                AppValidator validator = new AppValidator();
                validator.CheckGuid(id, "id");
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }
                var response = await _stockService.GetStock(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get All stocks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetAllStocks()
        {
            try
            {
                var response = await _stockService.GetAllStocks();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Add or Edit Stock
        /// </summary>
        /// <param name="stockRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> AddOrEditStock([FromForm] StockRequestModel stockRequest)
        {
            try
            {
                AppValidator validator = new AppValidator();
                validator.CheckIfNull(stockRequest, nameof(stockRequest));
                validator.CheckGuid(stockRequest.UserId, nameof(stockRequest.UserId));
                if (stockRequest.Id != null) validator.CheckGuid(stockRequest.Id, nameof(stockRequest.Id));
                if(stockRequest.Id == null)
                {
                    validator.CheckIfNull(stockRequest.Name, nameof(stockRequest.Name));
                    validator.CheckIfNull(stockRequest.Ticker, nameof(stockRequest.Ticker));
                    validator.CheckIfNull(stockRequest.Sector, nameof(stockRequest.Sector));
                    validator.CheckIfNull(stockRequest.Country, nameof(stockRequest.Country));
                    validator.CheckIfNull(stockRequest.Mcap, nameof(stockRequest.Mcap));
                    validator.CheckIfNull(stockRequest.Currency, nameof(stockRequest.Currency));
                    validator.CheckIfNull(stockRequest.Exchange, nameof(stockRequest.Exchange));
                    validator.CheckCompilance(stockRequest.Compliance);
                    validator.CheckIfNull(stockRequest.IsActive, nameof(stockRequest.IsActive));
                    validator.CheckIfNull(stockRequest.IsHaramRevenueOver5Per, nameof(stockRequest.IsHaramRevenueOver5Per));
                }
                validator.CheckLengthIfNotNull(stockRequest.Name, nameof(stockRequest.Name), 30);
                validator.CheckLengthIfNotNull(stockRequest.Ticker, nameof(stockRequest.Ticker), 10);
                validator.CheckLengthIfNotNull(stockRequest.Sector, nameof(stockRequest.Sector), 20);
                validator.CheckLengthIfNotNull(stockRequest.Country, nameof(stockRequest.Country), 30);
                validator.CheckLengthIfNotNull(stockRequest.Mcap, nameof(stockRequest.Mcap), 30);
                validator.CheckLengthIfNotNull(stockRequest.Currency, nameof(stockRequest.Currency), 10);
                validator.CheckLengthIfNotNull(stockRequest.Exchange, nameof(stockRequest.Exchange), 30);
                validator.CheckCompilance(stockRequest.Compliance, true);
                if (!string.IsNullOrWhiteSpace(stockRequest.Mcap) && !new Regex("^[0-9]+$").IsMatch(stockRequest.Mcap))
                {
                    validator.ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidMcap, nameof(stockRequest.Mcap), stockRequest.Mcap));
                }
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }

                var response = await _stockService.AddOrEditStock(stockRequest);
                return StatusCode((int)response.StatusCode, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Delete Stock by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<StandardResponseModel>> DeleteStock([FromRoute] Guid id)
        {
            try
            {
                AppValidator validator = new AppValidator();
                validator.CheckGuid(id, nameof(id));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }
                var response = await _stockService.DeleteStock(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }


        /// <summary>
        /// Get Dynamic stock data with pagination, search and sort functionality
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByColumn"> Name, Ticker, Sector, Country, Currency, Exchange </param>
        /// <param name="sortDirection">"ascending","descending"</param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [Route("paged")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetDynamicStockData(
            int page = 1,
            int pageSize = 5,
            string? orderByColumn = null,
            string sortDirection = "ascending",
            string? searchText = null
            )
        {
            List<string> validOrderByColumns = new List<string> { "name", "ticker", "sector", "country", "currency", "exchange" };
            AppValidator validator = new AppValidator();
            validator.CheckPaginationParameters(page, pageSize, orderByColumn, sortDirection, validOrderByColumns);
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _stockService.GetDynamicStockData(page, pageSize, orderByColumn, sortDirection, searchText);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Stock Quantity that User has
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("{stockId}/user/{userId}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetStockQuantityForUser([FromRoute] Guid stockId, [FromRoute] Guid userId)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(stockId, nameof(stockId));
            validator.CheckGuid(userId, nameof(userId));
            if(validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _stockService.GetStockQuantity(userId, stockId);
                return StatusCode((int)response.StatusCode, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Stock by ticker symbol
        /// </summary>
        /// <param name="ticker">company ticker symbol</param>
        /// <returns></returns>
        [Route("ticker/{ticker}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetStockByTicker([FromRoute] string ticker)
        {
            try
            {
                AppValidator validator = new AppValidator();
                validator.CheckString(ticker, nameof(ticker));
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }
                var response = await _stockService.GetStockByTicker(ticker);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Stock details from Chart Api
        /// </summary>
        /// <param name="ticker">company ticker</param>
        /// <returns></returns>
        [Route("chart/{ticker}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetFullStockDetailsFromChart([FromRoute] string ticker)
        {
            var validator = new AppValidator();
            validator.CheckString(ticker, nameof(ticker), 14);
            if(validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _stockService.GetFullStockDetailsFromChart(ticker);
                return StatusCode((int)response.StatusCode, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Company details from Fmp Api
        /// </summary>
        /// <param name="ticker">company ticker</param>
        /// <returns></returns>
        [Route("companyDetails/{ticker}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetCompanyDetailsFromFmpApi([FromRoute] string ticker)
        {
            var validator = new AppValidator();
            validator.CheckString(ticker, nameof(ticker), 14);
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _stockService.GetCompanyDetailsFromFmpApi(ticker);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Company details with Ratios from Fmp Api
        /// </summary>
        /// <param name="ticker">company ticker</param>
        /// <returns></returns>
        [Route("companydetails/{ticker}/ratios")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetCompanyDetailsWithRatios([FromRoute] string ticker)
        {
            var validator = new AppValidator();
            validator.CheckString(ticker, nameof(ticker), 14);
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _stockService.GetCompanyDetailsWithRatios(ticker);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

    }
}
