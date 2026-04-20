using System.Text.Json;
using RestSharp;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Services
{
    /// <summary>
    /// External Api Service
    /// </summary>
    public class ExternalApiService : IExternalApiService
    {
        private readonly RestClient client;
        public ExternalApiService()
        {
            client = new RestClient();
        }

        /// <summary>
        /// Send Get request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<(T? Data,bool Success,Exception? Exception)> SendGetRequest<T>(string url)
        {
            try
            {
                var request = new RestRequest(url, Method.Get);
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful && response?.Content != null)
                {
                    T? result = JsonSerializer.Deserialize<T>(response.Content);
                    return (result, true, null);
                }
                return (default, false, null);

            }
            catch (Exception ex)
            {
                return (default, false, ex);
            }
        }
    }
}
