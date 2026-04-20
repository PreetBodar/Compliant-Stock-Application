namespace StockTradingApplication.Services.IServices
{
    public interface IMailService
    {
        public Task<bool> SendResetPasswordMail(string email, string password);
    }
}
