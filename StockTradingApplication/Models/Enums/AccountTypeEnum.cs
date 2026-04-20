using Microsoft.EntityFrameworkCore;

namespace StockTradingApplication.Models.Enums
{
    /// <summary>
    /// Account Types
    /// </summary>
    public enum AccountTypeEnum
    {
        /// <summary>
        /// Saving
        /// </summary>
        Saving = 1,
        /// <summary>
        /// Brokerage
        /// </summary>
        Brokerage = 2,
        /// <summary>
        /// Checking
        /// </summary>
        Checking = 3,
    }
}
