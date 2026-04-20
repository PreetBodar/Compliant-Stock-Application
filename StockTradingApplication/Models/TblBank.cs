using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StockTradingApplication.Models;

public partial class TblBank
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = null!;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonIgnore]
    public Guid CreatedBy { get; set; }
    [JsonIgnore]
    public DateTime CreatedDatetime { get; set; }
    [JsonIgnore]
    public Guid? ModifyBy { get; set; }
    [JsonIgnore]
    public DateTime? ModifyDatetime { get; set; }
}
