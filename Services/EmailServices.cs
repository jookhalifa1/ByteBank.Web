using ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmailServices : IEmailServices
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("youseefs272@gmail.com", "bgudsioydomcsqrq"), // must email here equal  mailmessage because gmail dont understand this is spam 
                EnableSsl = true,

            };


            var mail = new MailMessage
            {
                From = new MailAddress("youseefs272@gmail.com"),
                Subject = subject,
                Body=message,
                IsBodyHtml = false


            };
            mail.To.Add(email);
            await client.SendMailAsync(mail);


        }
    }
}
