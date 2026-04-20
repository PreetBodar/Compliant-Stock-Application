using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Models.RequestModels
{
    public class RegisterRequestModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
