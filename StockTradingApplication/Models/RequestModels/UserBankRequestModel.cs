using StockTradingApplication.Models.Enums;

namespace StockTradingApplication.Models.RequestModels
{
    public class UserBankRequestModel
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid UserId { get; set; }

        public Guid BankId { get; set; } = Guid.Empty;

        public string? BankAccountNo { get; set; }

        public AccountTypeEnum AccountType { get; set; } = 0;
            
        public bool? IsActive { get; set; }

        public string? AccountHolderName { get; set; }
    }
}
