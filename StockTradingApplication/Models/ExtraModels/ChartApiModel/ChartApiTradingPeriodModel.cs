using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ExtraModels.ChartApiModel
{
    public class ChartApiTradingPeriodModel
    {
        [JsonPropertyName("timezone")]
        public string? TimeZone { get; set; }

        [JsonPropertyName("start")]
        public long? Start { get; set; }

        [JsonPropertyName("end")]
        public long? End { get; set; }

        [JsonPropertyName("gmtoffset")]
        public long? Gmtoffset { get; set; }
    }
}
