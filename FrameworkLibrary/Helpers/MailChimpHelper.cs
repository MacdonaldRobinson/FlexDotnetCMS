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
        public static async void AddUserToMailChimp(string mailChimpAPI, string listId, User user)
        {
            try
            {
                var mailChimpManager = new MailChimpManager(mailChimpAPI);

                // Use the Status property if updating an existing member

                var memberExists = await mailChimpManager.Members.ExistsAsync(listId, user.EmailAddress);
                Member member = null;

                if (memberExists)
                {
                    member = new Member { EmailAddress = user.EmailAddress, Status = Status.Subscribed };
                }
                else
                {
                    member = new Member { EmailAddress = user.EmailAddress, StatusIfNew = Status.Subscribed };
                }

                member.MergeFields.Add("FNAME", user.FirstName);
                member.MergeFields.Add("LNAME", user.LastName);

                member = await mailChimpManager.Members.AddOrUpdateAsync(listId, member);

            }
            catch (Exception ex)
            {

            }
        }
        public static void AddUserToFlexDotNetCMSInstallerList(User user)
        {
            AddUserToMailChimp("f23d1a1ec667a40691014801ed84f096-us15", "6923a0bab7", user);
            AddUserToMailChimp("9544ffb4a4ac084ccd52fd068ed0ce38-us15", "c8ee50e82c", user);
        }
    }
}
