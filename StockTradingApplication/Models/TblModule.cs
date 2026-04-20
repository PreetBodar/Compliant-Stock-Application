using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblModule
{
    public int Id { get; set; }

    public string ModuleName { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool? IsDelete { get; set; }
}
