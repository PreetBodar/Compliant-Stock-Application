using System.Text.RegularExpressions;

namespace StockTradingApplication.Models.Enums
{
    public static class AppRegex
    {
       public static Regex Email = new Regex("^[a-zA-Z0-9]{1}[a-zA-Z0-9_.]{0,29}@[a-zA-Z]{1}[a-zA-Z.]{0,14}\\.[a-zA-Z]{2,3}$");
       public static Regex Phone = new Regex("^[0-9]{10}$");
       public static Regex Password = new Regex("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+=-]).{8,14}$");
       public static Regex Name = new Regex("^[a-zA-Z]+$");
       public static Regex UserName = new Regex("^[a-zA-Z0-9_]+$");
       public static Regex AccountNumber = new Regex("^[0-9]{9,18}$");
    }
}
