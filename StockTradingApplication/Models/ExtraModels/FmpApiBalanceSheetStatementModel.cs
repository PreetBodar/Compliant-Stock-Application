using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ExtraModels
{
    public class FmpApiBalanceSheetStatementModel
    {
        [JsonPropertyName("cashAndCashEquivalents")]
        public long CashAndCashEquivalents { get; set; }

        [JsonPropertyName("totalAssets")]
        public long TotalAssets { get; set; }

        [JsonPropertyName("totalDebt")]
        public long TotalDebt { get; set; }

        [JsonPropertyName("netDebt")]
        public long NetDebt { get; set; }

        [JsonPropertyName("totalLiabilities")]
        public long TotalLiabilities { get; set; }
    }
}
