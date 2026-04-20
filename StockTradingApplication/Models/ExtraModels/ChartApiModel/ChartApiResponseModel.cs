using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ExtraModels.ChartApiModel
{
    public class ChartApiResponseModel
    {
        [JsonPropertyName("chart")]
        public ChartApiChartModel? Chart { get; set; }
    }

}