using System.Net;
using StockTradingApplication.Models.Enums;

namespace StockTradingApplication.Models.ResponseModels
{
    public class StandardResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message {  get; set; } = string.Empty;
        public object? Data { get; set; }

        public StandardResponseModel(HttpStatusCode statusCode, string message, object? data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
        public StandardResponseModel(string message, object? data)
        {
            Message = message;
            Data = data;
        }

        public StandardResponseModel(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Message = null;
            Data = null;
        }

        public StandardResponseModel(Exception ex)
        {
            StatusCode = HttpStatusCode.InternalServerError;
            Message = ex.Message;
            Data = ex.InnerException?.Message ?? "";
        }
        public StandardResponseModel(List<BadRequestResponseModel> errorList)
        {
            StatusCode = HttpStatusCode.BadRequest;
            Message = AppMessages.ValidationFailed;
            Data = errorList;
        }
        
    }
}
