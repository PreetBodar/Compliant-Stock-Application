namespace StockTradingApplication.Services.IServices
{
    public interface IExternalApiService
    {
        public Task<(T? Data, bool Success, Exception? Exception)> SendGetRequest<T>(string url);
    }
}
