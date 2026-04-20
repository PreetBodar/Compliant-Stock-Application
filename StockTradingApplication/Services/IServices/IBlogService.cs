using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Services.IServices
{
    public interface IBlogService
    {
        public Task<StandardResponseModel> GetBlog(string id);
        public Task<StandardResponseModel> GetAllBlog();
        public Task<StandardResponseModel> DeleteBlog(string id);
        public Task<StandardResponseModel> AddOrEditBlog(BlogRequestModel blogRequest);
        public Task<StandardResponseModel> GetDynamicBlogData(
            int page,
            int pageSize,
            string? orderByColumn,
            string sortDirection,
            string? searchText );
    }
}
