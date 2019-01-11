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

		public Return AddEmailAddressToMailChimp(string listId, User user, Dictionary<string, object> mergeFields)
		{
			var returnObj = new Return();

			try
			{
				// Use the Status property if updating an existing member

				var memberExists = MailChimpManager.Members.ExistsAsync(listId, user.EmailAddress).Result;

				Member member = null;

				if (memberExists)
				{
					member = new Member { EmailAddress = user.EmailAddress, Status = Status.Subscribed, StatusIfNew = Status.Pending };
				}
				else
				{
					member = new Member { EmailAddress = user.EmailAddress, Status = Status.Pending, StatusIfNew = Status.Pending };
				}

				foreach (var mergeField in mergeFields)
				{
					member.MergeFields.Add(mergeField.Key, mergeField.Value);
				}

				/*var birthDay = new DateTime(DateTime.Now.Year, user.Month, user.Day);

                member.MergeFields.Add("FNAME", user.Name);				
				member.MergeFields.Add("CITY", user.City);
				member.MergeFields.Add("BIRTHDAY", birthDay.ToString("MM/dd"));
				member.MergeFields.Add("REGION", user.Region);*/

				//member.MergeFields.Add("LNAME", user.LastName);

				member = MailChimpManager.Members.AddOrUpdateAsync(listId, member).Result;

				returnObj.SetRawData(member);

				return returnObj;


			}
			catch (Exception ex)
			{
				returnObj.Error = ErrorHelper.CreateError(ex.Message, ex.InnerException?.Message);
				return returnObj;
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
			var mailChimpHelper = new MailChimpHelper("f23d1a1ec667a40691014801ed84f096-us15");
			mailChimpHelper.AddEmailAddressToMailChimp("6923a0bab7", user, new Dictionary<string, object>());


			mailChimpHelper = new MailChimpHelper("9544ffb4a4ac084ccd52fd068ed0ce38-us15");
			mailChimpHelper.AddEmailAddressToMailChimp("6923a0bab7", user, new Dictionary<string, object>());			
        }
    }
}
