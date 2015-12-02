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

        ///// <summary>
        ///// helper method to send emails 
        ///// </summary>
        //private void SendEmail(string subject, string message, string source, string innerExcMessage, string senderEmail, string receiverEmail)
        //{
        //    try
        //    {
        //        MailMessage mail = new MailMessage(senderEmail, receiverEmail);
        //        SmtpClient client = new SmtpClient();
        //        client.Port = 25;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;
        //        client.Host = "smtp.google.com";
        //        mail.Subject = subject;
        //        mail.Body = string.Format("Message - {0} <br />Source - {1} <br />Inner exception message - {2}<br />Please contact IT Support as the exchange rate calcuations are not accurate for the current day"); ;
        //        client.Send(mail);
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.LogExceptions("Email Sending failed", exc);
        //    }
        //}



        /// <summary>
        /// method to send emails
        /// </summary>
        internal void InformViaEmail(string subject, string message, string source, string innerExcMessage)
        {
            try
            {
                string sender = CSVReader.ConfigCsv.SenderEmail;
                string receiver = CSVReader.ConfigCsv.ReceviverEmail;
                string senderPassword = CSVReader.ConfigCsv.SenderEmailPassword;
                if ((innerExcMessage == null && source == null) || (innerExcMessage == string.Empty && source == string.Empty))
                {
                    // information email
                    SendEmail(subject, message, sender, receiver, senderPassword);
                }
                else 
                {
                    // exception email
                    SendEmail(subject, message, source, innerExcMessage, sender, receiver, senderPassword);
                }
                
            }
            catch (Exception exc)
            {
                Logger.LogExceptions("Email Sending failed", exc);
            }
        }


        /// <summary>
        /// Used to send an informative email
        /// </summary>
        private void SendEmail(string subject, string message, string sender, string receiver, string senderPassword)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(sender, senderPassword);
                               
                MailMessage mm = new MailMessage(sender, receiver, subject, message);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);

            }
            catch (Exception exc)
            {
                Logger.LogExceptions(string.Format("Email Sending failed - {0}", DateTime.Now), exc);
            }
        }




        /// <summary>
        /// Used to email about an exception
        /// </summary>
        private void SendEmail(string subject, string message, string source, string innerExcMessage, string sender, string receiver, string senderPassword)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(sender, senderPassword);

                message = string.Format("Message - {0} <br />Source - {1} <br />Inner exception message - {2}<br />Please contact IT Support as the exchange rate calcuations are not accurate for the current day", 
                    message, source, innerExcMessage); 

                MailMessage mm = new MailMessage(sender, receiver, subject, message);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                
            }
            catch (Exception exc)
            {
                Logger.LogExceptions(string.Format("Email Sending failed - {0}", DateTime.Now), exc);
            }
        }

    }
}
