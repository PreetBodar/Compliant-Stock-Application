using Microsoft.AspNetCore.Mvc;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Controllers
{
    /// <summary>
    /// Module controller
    /// </summary>
    /// <remarks>
    /// injectecting module service
    /// </remarks>
    /// <param name="moduleService"></param>
    [Route("modules")]
    [ApiController]
    public class ModuleController(IModuleService moduleService) : Controller
    {
        private readonly IModuleService _moduleService = moduleService;

        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetAllModules()
        {
            try
            {
                var response = await _moduleService.GetAllModules();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Module by Id
        /// </summary>
        /// <param name="id"> Module Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetModule([FromRoute] int id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(id, nameof(id));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _moduleService.GetModule(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Add New Module
        /// </summary>
        /// <param name="moduleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> CreateModule([FromBody] ModuleRequestModel moduleRequest)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIfNull(moduleRequest, nameof(moduleRequest));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            validator.CheckString(moduleRequest.ModuleName, nameof(moduleRequest.ModuleName), 30);
            validator.CheckIfNull(moduleRequest.isActive, nameof(moduleRequest.isActive));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _moduleService.CreateModule(moduleRequest);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Update Module
        /// </summary>
        /// <param name="moduleRequest"></param>
        /// <param name="id">module Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult<StandardResponseModel>> UpdateModule([FromBody] ModuleRequestModel moduleRequest, [FromRoute] int id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(id, nameof(id));
            validator.CheckIfNull(moduleRequest, nameof(moduleRequest));
            validator.CheckString(moduleRequest.ModuleName, nameof(moduleRequest.ModuleName), 30);
            validator.CheckIfNull(moduleRequest.isActive, nameof(moduleRequest.isActive));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _moduleService.UpdateModule(moduleRequest, id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Delete module by id
        /// </summary>
        /// <param name="id"> module Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<StandardResponseModel>> DeleteModule([FromRoute] int id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(id, nameof(id));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _moduleService.DeleteModule(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }
    }
}
