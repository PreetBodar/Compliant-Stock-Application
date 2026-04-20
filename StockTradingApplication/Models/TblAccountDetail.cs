using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblAccountDetail
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNo { get; set; } = null!;

    public double AccountBalance { get; set; }
}
