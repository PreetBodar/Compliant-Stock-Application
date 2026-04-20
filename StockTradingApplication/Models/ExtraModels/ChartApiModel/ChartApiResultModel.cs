using System.Text.Json.Serialization;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Models.ExtraModels.ChartApiModel
{
    public class ChartApiResultModel
    {
        [JsonPropertyName("meta")]
        public ChartApiMetaModel? Meta { get; set; }

        [JsonPropertyName("timestamp")]
        public List<long>? Timestamp { get; set; }

        [JsonPropertyName("indicators")]
        public ChartApiIndicatorModel? Indicators { get; set; }
    }
}
