using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore;
using StockTradingApplication.Models;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;
using System.Net;

namespace StockTradingApplication.Services
{
    public class BlogService : IBlogService
    {
        private readonly StockTradingApplicationContext _context;
        public BlogService(StockTradingApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Blog by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetBlog(string id)
        {
            try
            {
                TblBlog? blog = await _context.TblBlogs.FirstOrDefaultAsync(blog => blog.Id == id && !blog.IsDeleted);
                if(blog == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.BlogNotFound, null);
                }
                blog.CoverImage = await GetImageAsBase64(blog.CoverImage);
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new BlogResponseModel(blog));
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get All Blogs
        /// </summary>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetAllBlog()
        {
            try
            {
                List<TblBlog> blogList = await _context.TblBlogs.Where(blog => !blog.IsDeleted).ToListAsync();
                List<BlogResponseModel> result = new List<BlogResponseModel>();
                foreach (var blog in blogList)
                {
                    blog.CoverImage = await GetImageAsBase64(blog.CoverImage);
                    result.Add(new BlogResponseModel(blog));
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, result);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Delete Blog by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> DeleteBlog(string id)
        {
            try
            {
                TblBlog? blog = await _context.TblBlogs.FirstOrDefaultAsync(blog => blog.Id == id && !blog.IsDeleted);
                if (blog == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.BlogNotFound, null);
                }
                blog.IsDeleted = true;
                _context.TblBlogs.Update(blog);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get dynamic blog data with pagination, search and sort functionality 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByColumn"></param>
        /// <param name="sortDirection"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetDynamicBlogData(
            int page, 
            int pageSize, 
            string? orderByColumn, 
            string sortDirection, 
            string? searchText )
        {
            try
            {
                var blogList = _context.TblBlogs.Where(blog => !blog.IsDeleted).AsQueryable();
                //search
                if(searchText != null)
                {
                    searchText = searchText.ToLower();
                    blogList = blogList.Where(blog => blog.Title.ToLower().Contains(searchText)
                                                   || blog.UpdatedBy.ToLower().Contains(searchText)
                                                   || blog.CreatedBy.ToLower().Contains(searchText)
                                                   || blog.Category.ToLower().Contains(searchText)
                                              );
                }
                //orderby
                if(orderByColumn != null)
                {
                    bool isDescending = sortDirection.ToLower() == "descending";
                    blogList = orderByColumn.ToLower() switch
                    {
                        "title" => isDescending ? blogList.OrderByDescending(blog => blog.Title) : blogList.OrderBy(blog => blog.Title),
                        "createdat" => isDescending ? blogList.OrderByDescending(blog => blog.CreatedAt) : blogList.OrderBy(blog => blog.CreatedAt),
                        "category" => isDescending ? blogList.OrderByDescending(blog => blog.Category) : blogList.OrderBy(blog => blog.Category),
                        "updatedat" => isDescending ? blogList.OrderByDescending(blog => blog.UpdatedAt) : blogList.OrderBy(blog => blog.UpdatedAt),
                        _ => isDescending ? blogList.OrderByDescending(blog => blog.Id) : blogList.OrderBy(blog => blog.Id)
                    };
                }
                //pagination
                int totalCount = await blogList.CountAsync();
                int pages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var filteredData = await blogList.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                List<BlogResponseModel> responseBlogList = new List<BlogResponseModel>();
                //setting image as base64 string
                foreach (TblBlog blog in filteredData)
                {
                    blog.CoverImage = await GetImageAsBase64(blog.CoverImage);
                    responseBlogList.Add(new BlogResponseModel(blog));
                }

                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new
                {
                    page,
                    pages,
                    pageSize,
                    Result = responseBlogList,
                    totalCount,
                    FilterCount = responseBlogList.Count,
                });
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Add or Edit Blog 
        /// </summary>
        /// <param name="blogRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> AddOrEditBlog(BlogRequestModel blogRequest)
        {
            try
            {

                TblBlog? blog;
                bool isForUpdate = blogRequest.Id != null;
                //for update
                if(isForUpdate)
                {
                    blog = await _context.TblBlogs.FirstOrDefaultAsync(blog => blogRequest.Id == blog.Id && !blog.IsDeleted);
                    if(blog == null)
                    {
                        return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.BlogNotFound,null);
                    }
                }
                //for add
                else
                {
                    blog = new TblBlog();
                    blog.Id = Guid.NewGuid().ToString();
                    blog.CreatedBy = blogRequest.Email;
                    blog.CreatedAt = DateTime.Now;
                }

                //check if user email is registered
                if(!await _context.TblUsers.AnyAsync(user => user.Email.ToLower() == blogRequest.Email.ToLower() && !user.IsDelete))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.EmailNotRegistered, null);
                }

                blog.Title = blogRequest.Title ?? blog.Title;
                blog.Html = blogRequest.Html ?? blog.Html;
                blog.Css = blogRequest.Css ?? blog.Css;
                blog.Data = blogRequest.Data ?? blog.Data;
                blog.IsPublished = blogRequest.IsPublished ?? blog.IsPublished;
                blog.MetaData = blogRequest.MetaData ?? blog.MetaData;
                blog.Route = blogRequest.Route ?? blog.Route;
                blog.UpdatedBy = blogRequest.Email;
                blog.UpdatedAt = DateTime.Now;
                blog.Category = blogRequest.Category ?? blog.Category;
                blog.Description = blogRequest.Description ?? blog.Description;
                blog.CoverImage = blogRequest.CoverImage == null ? blog.CoverImage : await SaveImageAsync(blogRequest.CoverImage);
                blog.IsDeleted = false;

                //check if route is available
                if (await _context.TblBlogs.AnyAsync(item => item.Route.ToLower() == blog.Route.ToLower() 
                           && !item.IsDeleted && item.Id != blog.Id)){
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.RouteAlreadyTaken, null);
                }

                if(isForUpdate) _context.TblBlogs.Update(blog);
                else await _context.TblBlogs.AddAsync(blog);
                await _context.SaveChangesAsync();

                return new StandardResponseModel(isForUpdate? HttpStatusCode.OK : HttpStatusCode.Created,
                                                 isForUpdate? AppMessages.Updated: AppMessages.Added,
                                                 new BlogResponseModel(blog,true));
            }
            catch(Exception exception)
            {
                return new StandardResponseModel(exception);
            }
        }

        /// <summary>
        /// Save Image to Images 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static async Task<string> SaveImageAsync(IFormFile image)
        {
            string fileName = Path.GetFileNameWithoutExtension(image.FileName) + DateTime.Now.ToString("ddMMyy_hhmmss") + Path.GetExtension(image.FileName);
            string path = Path.Combine("Images", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
                fileStream.Close();
            }

            return fileName;
        }

        /// <summary>
        /// Get base64 string of image
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static async Task<string> GetImageAsBase64(string filename)
        {
            try
            {
                using (FileStream filestream = new FileStream($"Images/{filename}", FileMode.Open))
                {
                    var bytes = new byte[filestream.Length];
                    await filestream.ReadAsync(bytes);
                    string base64ImageString = Convert.ToBase64String(bytes);
                    return base64ImageString;
                }
            }
            catch
            {
                return AppMessages.ImageNotFound;
            }
        }
    }
}
