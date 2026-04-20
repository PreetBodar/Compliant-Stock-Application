using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ExtraModels.ChartApiModel
{
    public class ChartApiMetaModel
    {
        [JsonPropertyName("currency")]
        public string? Currency {  get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("exchangeName")]
        public string? ExchangeName { get; set; }

        [JsonPropertyName("fullExchangeName")]
        public string? FullExchangeName { get; set; }

        [JsonPropertyName("instrumentType")]
        public string? InstrumentType { get; set; }

        [JsonPropertyName("firstTradeDate")]
        public long? FirstTradeDate { get; set; }

        [JsonPropertyName("regularMarketTime")]
        public long? RegularMarketTime { get; set; }

        [JsonPropertyName("hasPrePostMarketData")]
        public bool? HasPrePostMarketData { get; set; }

        [JsonPropertyName("gmtoffset")]
        public long? Gmtoffset { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }

        [JsonPropertyName("exchangeTimezoneName")]
        public string? ExchangeTimezoneName { get; set; }

        [JsonPropertyName("regularMarketPrice")]
        public double? RegularMarketPrice { get; set; }

        [JsonPropertyName("fiftyTwoWeekHigh")]
        public double? FiftyTwoWeekHigh { get; set; }

        [JsonPropertyName("fiftyTwoWeekLow")]
        public double? FiftyTwoWeekLow { get; set; }

        [JsonPropertyName("regularMarketDayHigh")]
        public double? RegularMarketDayHigh { get; set; }

        [JsonPropertyName("regularMarketDayLow")]
        public double? RegularMarketDayLow { get; set; }

        [JsonPropertyName("regularMarketVolume")]
        public long? RegularMarketVolume { get; set; }

        [JsonPropertyName("longName")]
        public string? LongName { get; set; }

        [JsonPropertyName("shortName")]
        public string? ShortName { get; set; }

        [JsonPropertyName("chartPreviousClose")]
        public double? ChartPreviousClose { get; set; }

        [JsonPropertyName("priceHint")]
        public int? PriceHint { get; set; }

        [JsonPropertyName("currentTradingPeriod")]
        public ChartApiCurrentTradingPeriodModel? CurrentTradingPeriod { get; set; }

        [JsonPropertyName("dataGranularity")]
        public string? DataGranularity { get; set; }

        [JsonPropertyName("range")]
        public string? Range { get; set; }

        [JsonPropertyName("validRanges")]
        public List<string>? ValidRanges { get; set; }

    }
}
