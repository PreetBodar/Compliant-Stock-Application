using Microsoft.AspNetCore.Mvc;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Controllers
{
    /// <summary>
    /// Blog Controller
    /// </summary>
    [Route("blogs")]
    [ApiController]
    public class BlogController(IBlogService blogService) : Controller
    {
        private readonly IBlogService _blogService = blogService;

        /// <summary>
        /// Get Blog by Id
        /// </summary>
        /// <param name="id">Blog Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetBlog([FromRoute] Guid id)
        {
            try
            {
                AppValidator validator = new AppValidator();
                validator.CheckGuid(id, "id");
                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }
                var response = await _blogService.GetBlog(id.ToString());
                return StatusCode((int)response.StatusCode, response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Get All blogs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetAllBlogs()
        {
            try
            {
                var response = await _blogService.GetAllBlog();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Add or Edit Blog
        /// </summary>
        /// <param name="blogRequest"></param>
        /// <returns></returns>
        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<ActionResult<StandardResponseModel>> AddOrEditBlog([FromForm] BlogRequestModel blogRequest)
        {
            try
            {
                AppValidator validator = new AppValidator();
                validator.CheckIfNull(blogRequest, nameof(blogRequest));
                validator.CheckEmail(blogRequest.Email, nameof(blogRequest.Email), 50);
                if(blogRequest.Id == null)
                {
                    validator.CheckIfNull(blogRequest.Title, nameof(blogRequest.Title));
                    validator.CheckIfNull(blogRequest.Html, nameof(blogRequest.Html));
                    validator.CheckIfNull(blogRequest.Css, nameof(blogRequest.Css));
                    validator.CheckIfNull(blogRequest.Route, nameof(blogRequest.Route));
                    validator.CheckIfNull(blogRequest.CoverImage, nameof(blogRequest.CoverImage));
                    validator.CheckIfNull(blogRequest.Category, nameof(blogRequest.Category));
                    validator.CheckIfNull(blogRequest.Description, nameof(blogRequest.Description));
                    validator.CheckIfNull(blogRequest.IsPublished, nameof(blogRequest.IsPublished));
                }
                validator.CheckLengthIfNotNull(blogRequest.Data, nameof(blogRequest.Data), 2000);
                validator.CheckLengthIfNotNull(blogRequest.MetaData, nameof(blogRequest.MetaData), 1000);
                validator.CheckLengthIfNotNull(blogRequest.Title, nameof(blogRequest.Title), 30);
                validator.CheckLengthIfNotNull(blogRequest.Html, nameof(blogRequest.Html), 10000);
                validator.CheckLengthIfNotNull(blogRequest.Css, nameof(blogRequest.Css), 2000);
                validator.CheckLengthIfNotNull(blogRequest.Route, nameof(blogRequest.Route), 20);
                validator.CheckLengthIfNotNull(blogRequest.Category, nameof(blogRequest.Category), 30);
                validator.CheckLengthIfNotNull(blogRequest.Description, nameof(blogRequest.Description), 100);
                if(blogRequest.CoverImage != null) validator.CheckImageFile(blogRequest.CoverImage, nameof(blogRequest.CoverImage));

                if (validator.ErrorList.Count > 0)
                {
                    return BadRequest(new StandardResponseModel(validator.ErrorList));
                }
                var response = await _blogService.AddOrEditBlog(blogRequest);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

        /// <summary>
        /// Delete Blog by Id
        /// </summary>
        /// <param name="id">Blog Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<StandardResponseModel>> DeleteBlog([FromRoute] Guid id)
        {
            AppValidator validator = new AppValidator();
            validator.CheckGuid(id, nameof(id));
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _blogService.DeleteBlog(id.ToString());
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }


        /// <summary>
        /// Get Dynamic Blog data with pagination, search and sort functionality
        /// </summary>
        /// <param name="page"> page number</param>
        /// <param name="pageSize"> number of records on one page </param>
        /// <param name="orderByColumn"> "title", "category", "createdat","updatedat","id"</param>
        /// <param name="sortDirection">"ascending","descending"</param>
        /// <param name="searchText">title , category, createdBy(email) , updatedby(email)</param>
        /// <returns></returns>
        [Route("paged")]
        [HttpGet]
        public async Task<ActionResult<StandardResponseModel>> GetDynamicBlogData(
            int page,
            int pageSize = 5,
            string? orderByColumn = null,
            string sortDirection = "ascending",
            string? searchText = null
            )
        {
            List<string> validOrderByColumns = new List<string> { "title", "category", "createdat", "updatedat","id" };
            AppValidator validator = new AppValidator();
            validator.CheckPaginationParameters(page,pageSize,orderByColumn,sortDirection,validOrderByColumns);
            if (validator.ErrorList.Count > 0)
            {
                return BadRequest(new StandardResponseModel(validator.ErrorList));
            }
            try
            {
                var response = await _blogService.GetDynamicBlogData(page, pageSize, orderByColumn, sortDirection, searchText);
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponseModel(ex));
            }
        }

    }
}
