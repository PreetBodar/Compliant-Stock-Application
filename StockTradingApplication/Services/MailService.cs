using System.Net;
using System.Net.Mail;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpClient smtpClient;
        private readonly MailAddress fromMail;
        public MailService(IConfiguration _config)
        {
            smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_config["Email:Mail"], _config["Email:Password"]);
            fromMail = new MailAddress(_config["Email:Mail"] ?? "");
        }

        /// <summary>
        /// Sends reset password mail to user email with new password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> SendResetPasswordMail(string email,string password)
        {
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = fromMail,
                    Body = string.Format(AppMessages.ResetPasswordMailBody,password),
                    IsBodyHtml = true,
                    Subject = AppMessages.ResetPasswordMailSubject
                };
                mailMessage.To.Add(new MailAddress(email));
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
