using ActiveUp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace FrameworkLibrary
{
    public class EmailHelper
    {
        public static IEnumerable<MailAddress> GetMailAddressesFromString(string emailAddressesString)
        {
            string[] emailAddresses = emailAddressesString.Split(';');

            var list = new List<MailAddress>();

            foreach (string emailAddress in emailAddresses)
                if (emailAddress.Trim() != "")
                    list.Add(new MailAddress(emailAddress.Trim()));

            return list;
        }

        public static string GetMailAddressesAsString(MailAddressCollection list)
        {
            string addresses = "";

            foreach (MailAddress item in list)
                if (item.Address.Trim() != "")
                    addresses += item.Address.Trim() + ";";

            if (addresses.EndsWith(";"))
                addresses = addresses.Substring(0, addresses.Length - 1);

            return addresses;
        }

        public static Return Send(string fromEmailAddress, IEnumerable<MailAddress> emailAddresses, string subject, string body, bool attemptSmtpMailer = true)
        {
            Return returnObj = new Return();
            var emailLog = new EmailLog();

            if (attemptSmtpMailer)
            {
                try
                {
                    MailMessage message = new MailMessage();

                    foreach (MailAddress address in emailAddresses)
                        message.To.Add(address);

                    message.Sender = new MailAddress(fromEmailAddress);

                    message.IsBodyHtml = true;
                    message.Subject = subject;
                    message.Body = body;

                    emailLog = GetEmailLogFromMailMessage(message);

                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                    client.Send(message);

                    emailLog.ServerMessage = "Successfully sent email";

                    EmailsMapper.Insert(emailLog);

                    return returnObj;
                }
                catch (Exception ex)
                {
                    ErrorHelper.LogException(ex);

                    returnObj.Error = ErrorHelper.CreateError(ex);

                    emailLog.ServerMessage = returnObj.Error.Message;

                    EmailsMapper.Insert(emailLog);

                    var directSentReturn = SendDirectMessage(fromEmailAddress, emailAddresses, subject, body);

                    if (directSentReturn.IsError)
                        return directSentReturn;
                    else
                        returnObj = BaseMapper.GenerateReturn();

                    return returnObj;
                }
            }
            else
            {
                return SendDirectMessage(fromEmailAddress, emailAddresses, subject, body);
            }
        }

        private static Return SendDirectMessage(string fromEmailAddress, IEnumerable<MailAddress> emailAddresses, string subject, string body)
        {
            var returnObj = new Return();
            var emailLog = new EmailLog();

            Message message = new Message();
            message.Subject = subject;
            message.From = new Address(fromEmailAddress);

            foreach (var mailAddress in emailAddresses)
            {
                message.To.Add(mailAddress.Address);
            }
            

            message.IsHtml = true;
            message.BodyHtml.Text = body;
            message.BodyText.Text = body;

            try
            {
                var mailMessage = new MailMessage();
                mailMessage.CopyFrom(message);

                var returnStr = ActiveUp.Net.Mail.SmtpClient.DirectSend(message);

                if(!string.IsNullOrEmpty(returnStr))
                {
                    emailLog.ServerMessage = returnObj?.Error?.Message;
                    EmailsMapper.Insert(emailLog);
                }

                return returnObj;
            }
            catch(Exception ex)
            {
                ErrorHelper.LogException(ex);

                returnObj.Error = ErrorHelper.CreateError(ex);

                emailLog.ServerMessage = returnObj?.Error?.Message;

                EmailsMapper.Insert(emailLog);

                return returnObj;
            }            
        }

        public static EmailLog GetEmailLogFromMailMessage(MailMessage mailObj)
        {
            EmailLog obj = new EmailLog();

            obj.SenderName = mailObj.Sender.DisplayName;
            obj.SenderEmailAddress = mailObj.Sender.Address;
            obj.ToEmailAddresses = EmailHelper.GetMailAddressesAsString(mailObj.To);
            obj.FromEmailAddress = mailObj.From.Address;
            obj.Message = mailObj.Body;
            obj.Subject = mailObj.Subject;
            obj.VisitorIP = HttpContext.Current.Request.UserHostAddress;
            obj.RequestUrl = URIHelper.GetCurrentVirtualPath(true);

            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return obj;
        }

        public static Return SendTemplate(IEnumerable<MailAddress> emailAddresses, string subject, string senderName, string senderEmailAddress, string pathToControl, params object[] constructorParameters)
        {
            string message = LoaderHelper.RenderControl(pathToControl, constructorParameters);
            return Send(senderEmailAddress, emailAddresses, subject, message);
        }
    }
}