using System.Text.Json;
using System.Text.Json.Serialization;
using DocumentFormat.OpenXml.Wordprocessing;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.ExtraModels.ChartApiModel;

namespace StockTradingApplication.Models.ResponseModels
{
    public class StockResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Ticker { get; set; }

        public string Sector { get; set; }

        public string Country { get; set; }

        public string Mcap { get; set; }

        public string Currency { get; set; }

        public int Compliance { get; set; }

        public bool? IsActive { get; set; }

        public string Exchange { get; set; }

        public NotComplianceSectorsModel? NotComplianceSectors { get; set; }

        public bool? IsHaramRevenueOver5Per { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChartApiResultModel? Result { get; set; } = null;


        public StockResponseModel(TblStock stock,ChartApiResultModel? chartApiResult = null)
        {
            Id = stock.Id;
            Name = stock.Name;
            Ticker = stock.Ticker;
            Sector = stock.Sector;
            Country = stock.Country;
            Mcap = stock.Mcap;
            Currency = stock.Currency;
            Compliance = stock.Compliance;
            IsActive = stock.IsActive;
            Exchange = stock.Exchange;
            IsHaramRevenueOver5Per = stock.IsHaramRevenueOver5Per;
            try
            {
                NotComplianceSectors = JsonSerializer.Deserialize<NotComplianceSectorsModel>(stock.NotComplianceSectors);
            }
            catch
            {
                NotComplianceSectors = null;
            }

            if(chartApiResult != null)
            {
                Result = chartApiResult;
            }
        }
    }
}
