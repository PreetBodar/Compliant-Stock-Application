using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ExtraModels.ChartApiModel
{
    public class ChartApiChartModel
    {
        [JsonPropertyName("result")]
        public List<ChartApiResultModel>? Result { get; set; }

        [JsonPropertyName("error")]
        public object? Error { get; set; }
    }
}
