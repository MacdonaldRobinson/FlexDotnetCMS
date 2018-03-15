using MailChimp.Net;
using MailChimp.Net.Core;
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
        public MailChimpManager MailChimpManager { get; private set; } = null;

        public MailChimpHelper(string mailChimpAPI)
        {
            MailChimpManager = new MailChimpManager(mailChimpAPI);
        }

        public Return CreateAndSendCampaign(string listId, string fromName, string fromEmailAddress, string subject, string title, string htmlMessage)
        {
            var returnObj = BaseMapper.GenerateReturn();

            var campaign = new Campaign
            {
                Type = CampaignType.Regular,
                Recipients = new Recipient
                {
                    ListId = listId
                },
                Settings = new Setting
                {
                    SubjectLine = subject,
                    Title = title,
                    FromName = fromName,
                    ReplyTo = fromEmailAddress
                },
                Tracking = new Tracking
                {
                    Opens = true,
                    HtmlClicks = true,
                    TextClicks = true
                },
                ReportSummary = new ReportSummary(),
                DeliveryStatus = new DeliveryStatus(),
            };

            try
            {
                var mailChimpCampaign = MailChimpManager.Campaigns.AddOrUpdateAsync(campaign).Result;

                var ContentRequest = new ContentRequest
                {
                    Html = htmlMessage
                };

                var content = MailChimpManager.Content.AddOrUpdateAsync(mailChimpCampaign.Id, ContentRequest).Result;

                MailChimpManager.Campaigns.SendAsync(mailChimpCampaign.Id);

                return returnObj;
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                returnObj.Error = ErrorHelper.CreateError(ex);
                return returnObj;
            }
        }

        public Campaign GetCampaignDetails(string campainId)
        {
            var campaign = MailChimpManager.Campaigns.GetAsync(campainId).Result;
            return campaign;
        }

        public async void AddUserToMailChimp(string mailChimpAPI, string listId, User user)
        {
            try
            {
                // Use the Status property if updating an existing member

                var memberExists = await MailChimpManager.Members.ExistsAsync(listId, user.EmailAddress);

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

                member = await MailChimpManager.Members.AddOrUpdateAsync(listId, member);

            }
            catch (Exception ex)
            {

            }
        }

        public IEnumerable<Member> GetMembersInList(string listId)
        {
            var membersList = new List<Member>();
            var members = MailChimpManager.Members.GetAllAsync(listId).Result;

            return members;
        }

        public void AddUserToFlexDotNetCMSInstallerList(User user)
        {
            AddUserToMailChimp("f23d1a1ec667a40691014801ed84f096-us15", "6923a0bab7", user);
            AddUserToMailChimp("9544ffb4a4ac084ccd52fd068ed0ce38-us15", "c8ee50e82c", user);
        }
    }
}
