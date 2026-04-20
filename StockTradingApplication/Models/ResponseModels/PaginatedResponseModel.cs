using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ResponseModels
{
    public class PaginatedResponseModel<T>
    {
        [JsonPropertyName("page")]
        public int Page {  get; set; }

        [JsonPropertyName("pages")]
        public int Pages {  get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("result")]
        public List<T> Result { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("filterCount")]
        public int FilterCount { get; set; }
    }
}
