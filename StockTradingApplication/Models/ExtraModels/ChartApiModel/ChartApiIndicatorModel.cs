using System.Text.Json.Serialization;
using DocumentFormat.OpenXml.EMMA;

namespace StockTradingApplication.Models.ExtraModels.ChartApiModel
{
    public class ChartApiIndicatorModel
    {
        [JsonPropertyName("quote")]
        public List<ChartApiQuoteModel>? Quote { get; set; }

        [JsonPropertyName("adjclose")]
        public List<ChartApiAdjCloseModel>? AdjClose { get; set; }
    }

    public class ChartApiQuoteModel
    {
        [JsonPropertyName("open")]
        public List<double>? Open { get; set; }

        [JsonPropertyName("low")]
        public List<double>? Low { get; set; }

        [JsonPropertyName("close")]
        public List<double>? Close { get; set; }

        [JsonPropertyName("high")]
        public List<double>? High { get; set; }

        [JsonPropertyName("volume")]
        public List<long>? Volume { get; set; }
    }

    public class ChartApiAdjCloseModel
    {
        [JsonPropertyName("adjclose")]
        public List<double>? AdjClose { get; set; }
    }
}
