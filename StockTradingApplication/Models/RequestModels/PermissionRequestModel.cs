namespace StockTradingApplication.Models.RequestModels
{
    public class PermissionRequestModel
    {
        public bool? IsView { get; set; } 

        public bool? IsAdd { get; set; } 

        public bool? IsEdit { get; set; } 

        public bool? IsDelete { get; set; } 
    }
}
