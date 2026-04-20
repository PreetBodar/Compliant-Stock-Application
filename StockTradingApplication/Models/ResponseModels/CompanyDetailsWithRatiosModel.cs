using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ExtraModels;

namespace StockTradingApplication.Models.ResponseModels
{
    public class CompanyDetailsWithRatiosModel
    {
        public string? CompanyName { get; set; }
        public string? Market { get; set; }
        public double? InitialPrice { get; set; }
        public string? Sector {  get; set; }
        public string? Industry {  get; set; }
        public long? MarketCap {  get; set; }
        public string? Currency { get; set; }
        public string? Country { get; set; }
        public double? DebtToAssetRatio {  get; set; }
        public double? InterestIncomePer { get; set; }
        public NotComplianceSectorsModel Sectors { get; set; }
        public bool isHaramRevenueOver5Per { get; set; } = false;
        public string? Description { get; set; }
        public double? InterestBearingDebt { get; set; }
        public double? DebtToRevenuePerc { get; set; }
        public double? CashRatioPerc {  get; set; }
        public ComplianceEnum Compliance { get; set; } = ComplianceEnum.FullyCompliant;
    }
}
