using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ResponseModels;

namespace StockTradingApplication.Models.ExtraModels
{
    /// <summary>
    /// Common Validation class
    /// </summary>
    public class AppValidator
    {
        public List<BadRequestResponseModel> ErrorList { get; set; } = new List<BadRequestResponseModel>();

        /// <summary>
        /// Check if object is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public void CheckIfNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.MissingObject, parameterName, null));
            }
        }

        /// <summary>
        /// null and maxLength check for string types
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <param name="maxLength"></param>
        public void CheckString(string? value,string parameterName,int? maxLength = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.MissingParameter, parameterName, value));
            }
            if(maxLength != null && value?.Length > maxLength )
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.StringTooLong, parameterName, value));
            }
        }

        /// <summary>
        /// Check if name is valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <param name="maxLength"></param>
        public void CheckName(string? value,string parameterName,int maxLength,bool isUserName = false)
        {
            CheckString(value, parameterName, maxLength);
            if(!isUserName && !AppRegex.Name.IsMatch(value ?? ""))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidName, parameterName, value));
            }
            if (isUserName && !AppRegex.UserName.IsMatch(value ?? ""))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidUserName, parameterName, value));
            }
        }

        /// <summary>
        /// Check int typed Id 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public void CheckIntegerId(int? value,string parameterName)
        {
            if(value == null)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.MissingParameter, parameterName, value));
            }
            if(value!= null &&  value < 1)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidId,parameterName, value));
            }
        }

        /// <summary>
        /// check for valid account number
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isForUpdate"></param>
        public void CheckAccountNumber(string? value,bool isForUpdate)
        {
            if(!AppRegex.AccountNumber.IsMatch(value ?? ""))
            {
                if (!(isForUpdate && value == null))
                {
                    ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidAccountNumber, "bankAccountNumber", value));
                }
            }
        }

        /// <summary>
        /// Check if string is valid guid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public void CheckGuid(string? value,string parameterName)
        {
            CheckString(value, parameterName);
            if(!Guid.TryParse(value,out Guid guid))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidGuid, parameterName, value));
            }
        }

        /// <summary>
        /// check if guid is null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public void CheckGuid(Guid? value,string parameterName)
        {
            if(value == null || value == Guid.Empty)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.MissingParameter, parameterName, value));
            }
        }

        /// <summary>
        /// Check is email is valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <param name="maxLength"></param>
        public void CheckEmail(string? value,string parameterName,int? maxLength)
        {
            CheckString(value, parameterName, maxLength);
            if(!AppRegex.Email.IsMatch(value ?? ""))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidEmail, parameterName, value));            }
        }

        /// <summary>
        /// Check if password is valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public void CheckPassword(string? value, string parameterName)
        {
            CheckString(value, parameterName);
            if (!AppRegex.Password.IsMatch(value ?? ""))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidEmail,parameterName, value));
            }
        }

        /// <summary>
        /// Check if phone number is valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        public void CheckPhoneNumber(string? value, string parameterName)
        {
            CheckString(value, parameterName);
            if (!AppRegex.Phone.IsMatch(value ?? ""))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidPhoneNumber,parameterName, value));
            }
        }
    
        /// <summary>
        /// check if parameters for pagination are valid
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByColumn"></param>
        /// <param name="sortDirection"></param>
        /// <param name="validOrderByColumns"></param>
        public void CheckPaginationParameters(int page,int pageSize,string? orderByColumn, string sortDirection, List<string> validOrderByColumns)
        {
            if (page < 1)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidPage, "page", page));
            }
            if (pageSize < 1)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidPageSize, "pageSize", pageSize));
            }
            if (orderByColumn != null && !validOrderByColumns.Contains(orderByColumn.ToLower()))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidOrderBy, "orderByColumn", orderByColumn));
            }
            if (!(sortDirection.ToLower() == "ascending" || sortDirection.ToLower() == "descending"))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidSortDirections, "sortDirection", sortDirection));
            }
        }

        /// <summary>
        /// Check if it is valid image file
        /// </summary>
        /// <param name="image"></param>
        /// <param name="parameterName"></param>
        public void CheckImageFile(IFormFile? image,string parameterName)
        {
            List<string> imageExtensions = new List<string> { ".jpg", ".png", ".svg" };
         
            if (image == null)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.MissingParameter, parameterName, null));
            }
            if (image != null && !imageExtensions.Contains(Path.GetExtension(image.FileName) ?? ""))
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidExtension, parameterName, image.FileName));
            }
            if (image != null && image.Length > 5000000)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.ImageTooLarge, parameterName, image.FileName));
            }
        }

        /// <summary>
        /// Check length of string if parameter is not null 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameterName"></param>
        /// <param name="maxLength"></param>
        public void CheckLengthIfNotNull(string? value, string parameterName, int maxLength)
        {
            if(value != null && value.Length > maxLength)
            {
                ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.StringTooLong, parameterName, value));
            }
        }

        /// <summary>
        /// Check if valid compliance
        /// </summary>
        /// <param name="value"></param>
        public void CheckCompilance(ComplianceEnum value,bool isForUpdate = false)
        {
            if (!(value == ComplianceEnum.FullyCompliant || value == ComplianceEnum.PartiallyCompliant || value == ComplianceEnum.NotCompliant))
            {
                if(isForUpdate && value != 0)
                {
                    ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidCompilance, "compilance", value));
                }
                if (!isForUpdate)
                {
                    ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidCompilance, "compilance", value));
                }
            }
        }

        /// <summary>
        /// Check if account type is valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isForUpdate"></param>
        public void CheckAccountType(AccountTypeEnum value,bool isForUpdate = false)
        {
            if (!(value == AccountTypeEnum.Checking || value == AccountTypeEnum.Saving || value == AccountTypeEnum.Brokerage))
            {
                if (isForUpdate && value != 0)
                {
                    ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidAccountType, "accountType", value));
                }
                if (!isForUpdate)
                {
                    ErrorList.Add(new BadRequestResponseModel(BadRequestResponseModel.InvalidAccountType, "accountType", value));
                }
            }
        }
    }
}
