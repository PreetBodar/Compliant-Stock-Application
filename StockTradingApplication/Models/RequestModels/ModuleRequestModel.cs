using Microsoft.AspNetCore.Components.Web;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Models.RequestModels
{
    /// <summary>
    /// Module Request Model
    /// </summary>
    public class ModuleRequestModel
    {
        /// <summary>
        /// Module Name
        /// </summary>
        public string ModuleName { get; set; } = string.Empty;

        /// <summary>
        /// Is Module Active
        /// </summary>
        public bool isActive { get; set; }

    }
}
