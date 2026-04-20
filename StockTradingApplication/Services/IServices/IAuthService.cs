using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Services.IServices
{
    public interface IAuthService
    {
        public Task<StandardResponseModel> RegisterUser(RegisterRequestModel registerRequest);
        public Task<StandardResponseModel> LoginUser(LoginRequestModel loginRequest);
        public Task<StandardResponseModel> ResetPassword(string email);
    }
}
