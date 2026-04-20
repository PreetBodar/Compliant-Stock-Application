using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntergrationTests
{
    internal class StandardResponseModel<T>
    {
        [JsonPropertyName("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        public static async Task<StandardResponseModel<T>> DeserializeResponse(HttpContent content)
        {
            try
            {
                var result = JsonSerializer.Deserialize<StandardResponseModel<T>>(await content.ReadAsStringAsync());
                return result ?? new StandardResponseModel<T> { StatusCode = HttpStatusCode.InternalServerError };
            }
            catch
            {
                return new StandardResponseModel<T> { StatusCode = HttpStatusCode.InternalServerError };
            }
        }
    }
}
