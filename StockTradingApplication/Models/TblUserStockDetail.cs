using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblUserStockDetail
{
    public Guid Id { get; set; }

    public Guid StockId { get; set; }

    public Guid UserId { get; set; }

    public long Quantity { get; set; }

    public double AveragePrice { get; set; }

    public double TotalInvestment { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public DateTime RecentlyStockBuyDateTime { get; set; }
}
