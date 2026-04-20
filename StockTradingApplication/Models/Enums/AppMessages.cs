namespace StockTradingApplication.Models.Enums
{
    public class AppMessages
    {
        public static string EmailAlreadyRegistered = "Email is already registered";
        public static string RegistrationSuccess = "Registration successful";
        public static string RegistrationFailed = "Unable to Register";
        public static string UserNameAlreadyTaken = "Username already taken";
        public static string EmailNotRegistered = "Email is not registered";
        public static string InvalidCredentials = "Invalid Credentials";
        public static string InternalServerError = "Internal Server error";
        public static string LoginFailed = "Login operation failed";
        public static string LoginSuccess = "Logged in successfully";
        public static string SendMailFailed = "Unable to send mail";
        public static string PasswordSentToMail = "New password has been sent to your mail";
        public static string RoleNotFound = "Role not found";
        public static string RoleAlreadyExist = "Role with same name already exist";
        public static string ModuleNotFound = "Module not found with given module id";
        public static string ModuleAlreadyExist = "Module with same name already exist";
        public static string PermissionNotFound = "Permission not found";
        public static string PermissionAlreadyExist = "Permission with same role and module already exist";
        public static string Updated = "Updated Successfully";
        public static string Added = "Added successfully";
        public static string Deleted = "Deleted successfully";
        public static string UpdateFailed = "Update Failed";
        public static string AddFailed = "Failed to Add";
        public static string DeleteFailed = "Delete failed";
        public static string DataFetched = "Data fetched successfully";
        public static string ValidationFailed = "Unable to pass validation criteria";
        public static string RouteAlreadyTaken = "This Route is already taken";
        public static string BlogNotFound = "Blog not found";
        public static string ImageNotFound = "Image not found";
        public static string StockNotFound = "Stock not found";
        public static string UserNotFound = "User not found";
        public static string StockAlreadyExist = "Stock with same Ticker & Exchange already exist";
        public static string StockTransactionNotFound = "No transaction found with this stock for given user";
        public static string NecessaryDataNotFound = "Necessary data not found to process your request";
        public static string BankNotFound = "Bank not found";
        public static string BankAlreadyExist = "Bank already exist with same name";
        public static string UserBankDetailsNotFound = "User Bank Details not found";
        public static string UserBankDetailsAlreadyFound = "account number already registered";


        public static string ResetPasswordMailBody = "<h3> Here's your new Password : <h3><br><b>{0}</b>";
        public static string ResetPasswordMailSubject = "New Password for your Login";
    }
}
