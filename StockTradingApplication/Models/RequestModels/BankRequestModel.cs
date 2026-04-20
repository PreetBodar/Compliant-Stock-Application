namespace StockTradingApplication.Models.RequestModels
{
    public class BankRequestModel
    {
        public Guid? Id { get; set; } = Guid.Empty; 
        public Guid UserId { get ; set; }
        public string? BankName {  get; set; }
        public bool? IsActive { get; set; }
    }
}
