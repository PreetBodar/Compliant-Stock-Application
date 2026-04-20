using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Services.IServices
{
    public interface IBankService
    {
        public Task<StandardResponseModel> AddOrEditBank(BankRequestModel bankRequest);
        public Task<StandardResponseModel> GetBank(Guid id);
        public Task<StandardResponseModel> GetAllBanks();
        public Task<StandardResponseModel> DeleteBank(Guid id);
        public Task<StandardResponseModel> GetDynamicBankData(
            int page,
            int pageSize,
            string? orderByColumn,
            string sortDirection,
            string? searchText
            );
        public Task<StandardResponseModel> AddOrEditUserBankDetails(UserBankRequestModel userBankRequest);
        public Task<StandardResponseModel> GetUserBankDetails(Guid id);

    }
}
