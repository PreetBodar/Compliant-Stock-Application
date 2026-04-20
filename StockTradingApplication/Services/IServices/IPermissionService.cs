using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Services.IServices
{
    public interface IPermissionService
    {
        public Task<StandardResponseModel> GetPermission(int id);
        public Task<StandardResponseModel> AddPermission(int moduleId);
        public Task<StandardResponseModel> UpdatePermission(PermissionRequestModel permissionRequest, int id);
        public Task<StandardResponseModel> DeletePermission(int moduleId);
        public Task<StandardResponseModel> BulkUpdateByRoleId(string roleId, PermissionRequestModel permissionRequest);
        public Task<StandardResponseModel> ExportPermissionsToXL(string roleId);
        public Task<StandardResponseModel> GetPermissionByModuleId(int moduleId);
        public Task<StandardResponseModel> GetDynamicPermissionData(
            int page,
            int pageSize,
            string? orderBy,
            string sortDirection,
            string? searchText
            );
    }
}
