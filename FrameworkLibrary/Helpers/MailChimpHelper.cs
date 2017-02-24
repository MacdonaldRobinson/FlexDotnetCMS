using MailChimp.Net;
using MailChimp.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public class MailChimpHelper
    {
        public static async void AddEmailAddressToFlexDotNetCMSInstallerList(string emailAddress)
        {
            try
            {
                var mailChimpManager = new MailChimpManager("f23d1a1ec667a40691014801ed84f096-us15");

                var listId = "6923a0bab7";
                // Use the Status property if updating an existing member

                var memberExists = await mailChimpManager.Members.ExistsAsync(listId, emailAddress);
                Member member  = null;

                if (memberExists)
                {
                    member = new Member { EmailAddress = emailAddress, Status = Status.Subscribed };
                }
                else
                {
                    member = new Member { EmailAddress = emailAddress, StatusIfNew = Status.Subscribed };
                }

                member = await mailChimpManager.Members.AddOrUpdateAsync(listId, member);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
