using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StockTradingApplication.Models;

public partial class TblUserBankDetail
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid BankId { get; set; }

    public string BankAccountNo { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public bool IsActive { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
    [JsonIgnore]
    public Guid CreatedBy { get; set; }
    [JsonIgnore]
    public DateTime CreatedDatetime { get; set; }
    [JsonIgnore]
    public Guid? ModifyBy { get; set; }
    [JsonIgnore]
    public DateTime? ModifyDatetime { get; set; }

    public string AccountHolderName { get; set; } = null!;
}
