using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Models.RequestModels
{
    public class LoginRequestModel
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
