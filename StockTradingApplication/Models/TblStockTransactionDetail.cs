using System;
using System.Collections.Generic;

namespace StockTradingApplication.Models;

public partial class TblStockTransactionDetail
{
    public Guid Id { get; set; }

    public DateTime StockBuyDateTime { get; set; }

    public DateTime StockSaleDateTime { get; set; }

    public string TransactionType { get; set; } = null!;

    public int Quantity { get; set; }

    public int SaleQuantity { get; set; }

    public Guid UserId { get; set; }

    public Guid BrokerId { get; set; }

    public double BuyPrice { get; set; }

    public double SalePrice { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public Guid StockId { get; set; }

    public string OrderType { get; set; } = null!;

    public string TimeInForce { get; set; } = null!;

    public double ActualPrice { get; set; }

    public double Price { get; set; }

    public int OrderTypeId { get; set; }
}
