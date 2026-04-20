using Microsoft.AspNetCore.Mvc;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Controllers
{
    /// <summary>
    /// Permission Controller
    /// </summary>
    [Route("permissions")]
    [ApiController]
    public class PermissionController(IPermissionService permissionService) : Controller
    {
        private readonly IPermissionService _permissionService = permissionService;

        /// <summary>
        /// Get permission by Id
        /// </summary>
        /// <param name="id">permission Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetPermission([FromRoute] int id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(id, nameof(id));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.GetPermission(id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get permission by Id
        /// </summary>
        /// <param name="id">permission Id</param>
        /// <returns></returns>
        [Route("module/{moduleId}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetPermissionByModuleId([FromRoute] int moduleId)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(moduleId, nameof(moduleId));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.GetPermissionByModuleId(moduleId);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Add module permissions for All Role
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <returns></returns>
        [Route("module/{moduleId}")]
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> AddPermission([FromRoute] int moduleId)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(moduleId, nameof(moduleId));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.AddPermission(moduleId);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Update Permission
        /// </summary>
        /// <param name="permissionRequest"></param>
        /// <param name="id">permission Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult<StandardResponseModel>> UpdatePermission([FromBody] PermissionRequestModel permissionRequest, [FromRoute] int id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(id, nameof(id));
            validator.CheckIfNull(permissionRequest, nameof(permissionRequest));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.UpdatePermission(permissionRequest,id);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Delete Module Permissions for all roles
        /// </summary>
        /// <param name="moduleId">module Id</param>
        /// <returns></returns>
        [Route("module/{moduleId}")]
        [HttpDelete]
        public async Task<ActionResult<StandardResponseModel>> DeletePermission([FromRoute] int moduleId)
        {
            AppValidator validator = new AppValidator();
            validator.CheckIntegerId(moduleId, nameof(moduleId));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.DeletePermission(moduleId);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Update all entries for permission for given role Id
        /// </summary>
        /// <param name="permissionRequest"></param>
        /// <param name="roleId">role Id</param>
        /// <returns></returns>
        [Route("role/{roleId}")]
        [HttpPatch]
        public async Task<ActionResult<StandardResponseModel>> BulkUpdateByRoleId([FromBody] PermissionRequestModel permissionRequest, [FromRoute] string roleId)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(roleId, nameof(roleId));
            validator.CheckIfNull(permissionRequest, nameof(permissionRequest));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.BulkUpdateByRoleId(roleId, permissionRequest!);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Export Permission to XL file
        /// </summary>
        /// <param name="roleId">role Id</param>
        /// <returns></returns>
        [Route("exportRolePermissions/{roleId}")]
        [HttpGet]
        public async Task<IActionResult> ExportPermissionsToXL([FromRoute] string roleId)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(roleId, nameof(roleId));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.ExportPermissionsToXL(roleId);
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return File((response.Data as byte[])!, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{response.Message}.xlsx");
                }   
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }


        /// <summary>
        /// Get Dynamic Permission Data with pagination, searching and sorting functionality
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">no of records on one page</param>
        /// <param name="orderByColumn">rolename , modulename, id</param>
        /// <param name="sortDirection">ascending, descending</param>
        /// <param name="searchText">rolename, modulename</param>
        /// <returns></returns>
        [Route("paged")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetDynamicPermissionData(
            int page = 1,
            int pageSize = 10,
            string? orderByColumn = null,
            string sortDirection = "ascending",
            string? searchText = null
            )
        {
            List<string> validOrderByColumns = new List<string> { "rolename", "modulename", "id"};

            AppValidator validator = new AppValidator();
            validator.CheckPaginationParameters(page, pageSize, orderByColumn, sortDirection, validOrderByColumns);
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _permissionService.GetDynamicPermissionData(page,pageSize,orderByColumn,sortDirection,searchText);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }
           
    }
}
