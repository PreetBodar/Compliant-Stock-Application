using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblTransaction
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid UserBankId { get; set; }

    public string TransactionType { get; set; } = null!;

    public double Amount { get; set; }

    public string Remarks { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public Guid? ModifyBy { get; set; }

    public DateTime? ModifyDatetime { get; set; }

    public Guid? UserBankTransactionId { get; set; }

    public string UserBankAccountNo { get; set; } = null!;
}
