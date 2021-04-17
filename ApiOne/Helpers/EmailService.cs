using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Helpers
{
    public class EmailService
    {
        public static async Task<bool> SendMail(MailMessage mailMessage)
        {
            try
            {
                var port = Startup.StaticConfig.GetValue<int>("Email:Port");
                var appSmtpClient = Startup.StaticConfig.GetValue<string>("Email:SmtpClient");
                var appMail = Startup.StaticConfig.GetValue<string>("Email:mail");
                var password = Startup.StaticConfig.GetValue<string>("Email:password");
                var smtpClient = new SmtpClient(appSmtpClient)
                {
                    Port = port,
                    Credentials = new NetworkCredential(appMail, password),
                    EnableSsl = true,
                };
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                Debug.WriteLine(ex.GetBaseException() + ex.FailedRecipient);
                return false;
            }
        }
    }
}
