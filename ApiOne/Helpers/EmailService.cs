using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        public EmailService(IHubContext<NotificationHub> hubContext)
        {
            _notificationHub = hubContext;
        }
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();
        static bool mailSent = false;

        public static void SendMail(MailMessage mailMessage)
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
                smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                smtpClient.SendMailAsync(mailMessage);
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            
            if (e.Cancelled)
            {
                Debug.WriteLine("[{0}] Send canceled.");
            }
            if (e.Error != null)
            {
                Debug.WriteLine("[{0}] {1}", e.Error.ToString());
            }
            else
            {
                Debug.WriteLine("Message sent.");
            }

        }
    }
}
