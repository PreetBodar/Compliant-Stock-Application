using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Services.IServices
{
    public interface IRoleService
    {
        public Task<StandardResponseModel> GetAllRoles();
        public Task<StandardResponseModel> GetRole(string id);
        public Task<StandardResponseModel> CreateRole(string name);
        public Task<StandardResponseModel> UpdateRole(string name, string id); 
        public Task<StandardResponseModel> DeleteRole(string id);
        public Task<StandardResponseModel> AssignRoleToUser(string userId, string roleName);
        public Task<StandardResponseModel> GetRolesByUserId(string userId);
    }
}
