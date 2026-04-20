using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Services.IServices
{
    public interface IStockService
    {
        public Task<StandardResponseModel> GetAllStocks();
        public Task<StandardResponseModel> GetStock(Guid id);
        public Task<StandardResponseModel> GetStockByTicker(string ticker);
        public Task<StandardResponseModel> AddOrEditStock(StockRequestModel stockRequest);
        public Task<StandardResponseModel> DeleteStock(Guid id);
        public Task<StandardResponseModel> GetDynamicStockData(
            int page,
            int pageSize,
            string? orderByColumn,
            string sortDirection,
            string? searchText );

        public Task<StandardResponseModel> GetStockQuantity(Guid userId, Guid stockId);
        public Task<StandardResponseModel> GetFullStockDetailsFromChart(string ticker);
        public Task<StandardResponseModel> GetCompanyDetailsFromFmpApi(string ticker);
        public Task<StandardResponseModel> GetCompanyDetailsWithRatios(string ticker);

    }
}
