using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Services.IServices
{
    public interface IModuleService
    {
        public Task<StandardResponseModel> GetAllModules();
        public Task<StandardResponseModel> GetModule(int id);
        public Task<StandardResponseModel> CreateModule(ModuleRequestModel moduleRequest);
        public Task<StandardResponseModel> UpdateModule(ModuleRequestModel moduleRequest, int id);
        public Task<StandardResponseModel> DeleteModule(int id);
    }
}
