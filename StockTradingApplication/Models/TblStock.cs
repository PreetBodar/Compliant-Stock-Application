using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblStock
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Ticker { get; set; } = null!;

    public string Sector { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Mcap { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public int Compliance { get; set; }

    public bool? IsActive { get; set; }

    public bool IsDeleted { get; set; } = false;

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public string Exchange { get; set; } = null!;

    public string NotComplianceSectors { get; set; } = null!;

    public bool? IsHaramRevenueOver5Per { get; set; }
}
