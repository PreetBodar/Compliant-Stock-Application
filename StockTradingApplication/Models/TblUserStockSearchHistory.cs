using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblUserStockSearchHistory
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid StockId { get; set; }

    public DateTime SearchDateTime { get; set; }
}
