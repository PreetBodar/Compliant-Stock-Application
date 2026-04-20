namespace StockTradingApplication.Models.ResponseModels
{
    public class BadRequestResponseModel
    {
        public int ErrorCode { get; set; }
        public string Parameter { get; set; } = string.Empty;
        public object? Value { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public BadRequestResponseModel(int errorCode, string message)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
        }

        public BadRequestResponseModel(BadRequestResponseModel model, string parameter = "", object? value = null)
        {
            ErrorCode = model.ErrorCode;
            ErrorMessage = model.ErrorMessage;
            Parameter = parameter;
            Value = value;
        }

        public static BadRequestResponseModel MissingObject => new(10001, "Object is invalid or null");
        public static BadRequestResponseModel MissingParameter => new(10002, "Parameter is missing value");
        public static BadRequestResponseModel InvalidEmail => new(10003, "Invalid Format for email");
        public static BadRequestResponseModel InvalidPassword => new(10004, "Password must contain atleast one uppercase,lowercase,digit and special character. Length (8-14)");
        public static BadRequestResponseModel InvalidPhoneNumber => new(10005, "Phone number must have 10 digits");
        public static BadRequestResponseModel EmailAlreadyRegistered => new(10006, "This email is already registered");
        public static BadRequestResponseModel InvalidName => new(10007, "Name have alphabets only");
        public static BadRequestResponseModel EmailNotRegistered => new(10008, "This email is not registered");
        public static BadRequestResponseModel IncorrectPassword => new(10009, "Password is Incorrect");
        public static BadRequestResponseModel UsernameTaken => new(10010, "Username is already taken");
        public static BadRequestResponseModel InvalidGuid => new(10011, "Id is invalid");
        public static BadRequestResponseModel InvalidId => new(10012, "Id cannot be less than 1");
        public static BadRequestResponseModel InvalidPage => new(10013, "page cannot be less than 1");
        public static BadRequestResponseModel InvalidPageSize => new(10014, "page size cannot be less than 1");
        public static BadRequestResponseModel InvalidOrderBy => new(10015, "Invalid column for Order by");
        public static BadRequestResponseModel InvalidSortDirections => new(10016, "sort direction must be ascending or descending");
        public static BadRequestResponseModel InvalidExtension => new(10017, "file extension is not supported");
        public static BadRequestResponseModel ImageTooLarge => new(10018, "Image size must be less than 5 mb");
        public static BadRequestResponseModel InvalidCompilance => new(10019, "Invalid value for compilance");
        public static BadRequestResponseModel StringTooLong => new(10020, "String has exceeded the maximum character limit");
        public static BadRequestResponseModel InvalidUserName => new(100021, "username have alphabets,number and underscore only");
        public static BadRequestResponseModel InvalidMcap => new(10022, "Market cap only has digits");
        public static BadRequestResponseModel InvalidAccountType => new(10023, "Invalid value for Account Type");
        public static BadRequestResponseModel InvalidAccountNumber => new(10024, "Invalid value for Account Number");

    }
}
