using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace StockTradingApplication.Models.ExtraModels
{
    public class FmpApiIncomeStatementModel
    {
       [JsonPropertyName("revenue")]
       public long Revenue { get; set; }

        [JsonPropertyName("interestIncome")]
        public long InterestIncome { get; set; }

        [JsonPropertyName("netIncome")]
        public long NetIncome { get; set; }
    }
}
