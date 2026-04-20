using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblPermission
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string RoleId { get; set; } = null!;

    public bool IsView { get; set; }

    public bool IsAdd { get; set; }

    public bool IsEdit { get; set; }

    public bool IsDelete { get; set; }
}
