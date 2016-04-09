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

        public static Return Send(IEnumerable<MailAddress> emailAddresses, string subject, string body, string senderName, string senderEmailAddress)
        {
            Return returnObj = new Return();
            var emailLog = new EmailLog();
            try
            {
                MailMessage message = new MailMessage();

                foreach (MailAddress address in emailAddresses)
                    message.To.Add(address);

                message.Sender = new MailAddress(senderEmailAddress, senderName);

                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;

                emailLog = GetEmailLogFromMailMessage(message);

                SmtpClient client = new SmtpClient();

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
            return Send(emailAddresses, subject, message, senderName, senderEmailAddress);
        }
    }
}