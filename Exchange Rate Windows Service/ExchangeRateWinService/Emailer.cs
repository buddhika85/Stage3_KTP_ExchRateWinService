using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWinService
{
    // Used to send emails 
    internal class Emailer
    {

        /// <summary>
        /// helper method to send emails 
        /// </summary>
        private void SendEmail(string subject, string message, string source, string innerExcMessage, string senderEmail, string receiverEmail)
        {
            try
            {
                MailMessage mail = new MailMessage(senderEmail, receiverEmail);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.google.com";
                mail.Subject = subject;
                mail.Body = string.Format("Message - {0} <br />Source - {1} <br />Inner exception message - {2}<br />Please contact IT Support as the exchange rate calcuations are not accurate for the current day"); ;
                client.Send(mail);
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Email Sending failed", exc);
            }
        }



        /// <summary>
        /// method to send emails
        /// </summary>
        internal void InformViaEmail(string subject, string message, string source, string innerExcMessage)
        {
            try
            {
                string sender = CSVReader.ConfigCsv.SenderEmail;
                string receiver = CSVReader.ConfigCsv.ReceviverEmail;
                SendEmail(subject, message, source, innerExcMessage, sender, receiver);
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Email Sending failed", exc);
            }
        }
    }
}
