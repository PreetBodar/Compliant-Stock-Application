using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ExtraModels.ChartApiModel
{
    public class ChartApiCurrentTradingPeriodModel
    {
        [JsonPropertyName("pre")]
        public ChartApiTradingPeriodModel? Pre {  get; set; }

        [JsonPropertyName("regular")]
        public ChartApiTradingPeriodModel? Regular { get; set; }

        [JsonPropertyName("post")]
        public ChartApiTradingPeriodModel? Post { get; set; }
    }
}
