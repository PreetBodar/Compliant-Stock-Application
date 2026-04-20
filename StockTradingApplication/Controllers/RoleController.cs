using Microsoft.AspNetCore.Mvc;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Controllers
{
    /// <summary>
    /// Role Controller
    /// </summary>
    [Route("roles")]
    [ApiController]
    public class RoleController(IRoleService roleService) : Controller
    {
        private readonly IRoleService _roleService = roleService;

        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetAllRoles()
        {
            try
            {
                var response = await _roleService.GetAllRoles();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Role by id
        /// </summary>
        /// <param name="id">role Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetRole([FromRoute] string id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(id, nameof(id));
            if(validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _roleService.GetRole(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Create new Role
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> CreateRole([FromBody] RoleRequestModel roleRequest)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIfNull(roleRequest, nameof(roleRequest));
            validator.CheckName(roleRequest.Name, nameof(roleRequest.Name),30);
            if(validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _roleService.CreateRole(roleRequest.Name);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <param name="id">role Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult<StandardResponseModel>> UpdateRole([FromBody] RoleRequestModel roleRequest, [FromRoute] string id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(id, nameof(id));
            validator.CheckIfNull(roleRequest, nameof(roleRequest));
            validator.CheckName(roleRequest.Name, nameof(roleRequest.Name), 30);
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _roleService.UpdateRole(roleRequest.Name, id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<StandardResponseModel>> DeleteRole([FromRoute] string id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(id, nameof(id));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _roleService.DeleteRole(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// AssignRoleToUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [Route("assign")]
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> AssignRoleToUser([FromQuery] string userId,[FromQuery] string roleName)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(userId, nameof(userId));
            validator.CheckString(roleName, nameof(roleName),30);
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _roleService.AssignRoleToUser(userId, roleName);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get Roles By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("user/{userId}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetRolesByUserId([FromRoute] string userId)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(userId, nameof(userId));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _roleService.GetRolesByUserId(userId);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }
    }
}
