using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace StockTradingApplication.Models;

public partial class TblUser 
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public string? ProfileImage { get; set; }

    public string? ProfileImageDisplayName { get; set; }

    public bool IsDelete { get; set; } = false;

    public DateTime CreatedDtTm { get; set; }

    public DateTime ModifiedDtTm { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

    public string? AccountNo { get; set; }

    public int VerificationCode { get; set; }
}
