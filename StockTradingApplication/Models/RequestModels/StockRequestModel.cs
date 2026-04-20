using StockTradingApplication.Models.Enums;

namespace StockTradingApplication.Models.RequestModels
{
    public class StockRequestModel
    {
        /// <summary>
        /// Stock Id (keep blank to add new stock)
        /// </summary>
        public string? Id { get; set; } = null;

        /// <summary>
        /// User Id
        /// </summary>
        public Guid? UserId { get; set; } = null;

        /// <summary>
        /// Name of Stock
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Ticker of Stock
        /// </summary>
        public string? Ticker { get; set; }

        /// <summary>
        /// Sector Name 
        /// </summary>
        public string? Sector { get; set; }

        /// <summary>
        /// Country Name
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Market Cap of stock
        /// </summary>
        public string? Mcap { get; set; }

        /// <summary>
        /// Currency (INR/ USD etc)
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Compilance
        /// </summary>
        public ComplianceEnum Compliance { get; set; } = 0;

        /// <summary>
        /// Is Stock Active
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Name of Exchange for stock (eg. NASDAQ)
        /// </summary>
        public string? Exchange { get; set; }

        /// <summary>
        /// Is Haram Revenue over 5 percent?
        /// </summary>
        public bool? IsHaramRevenueOver5Per { get; set; }

    }
}
