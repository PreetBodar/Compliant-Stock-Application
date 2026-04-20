using Microsoft.AspNetCore.Identity;

namespace StockTradingApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Discriminator { get; set; } = "IdentityUser";

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; } = string.Empty;

        public byte[]? ProfilePicture { get; set; }
    }
}
