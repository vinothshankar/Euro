using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Euro
{
    public class EmailSender
    {
        public void SendEmail(string toEmailAddress)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(Constants.FromEmailAddress);
                message.To.Add(new MailAddress(toEmailAddress));
                message.Subject = "Welcome To Euroland";
                message.IsBodyHtml = false;
                message.Body = "You have succefully completed the coding test and all the best for the next round";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; 
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Constants.FromEmailAddress,Constants.FromEmailAccountPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
