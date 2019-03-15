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
		private string _apiKey = "";
		public MailChimpManager MailChimpManager { get; private set; } = null;

		public string MailChimpEndPoint { get; private set; } = "https://us3.api.mailchimp.com/3.0";

		public MailChimpHelper(string mailChimpAPI)
		{
			_apiKey = mailChimpAPI;
			MailChimpManager = new MailChimpManager(mailChimpAPI);
		}

		public string GenerateUrl(string uriSegment, string queryString = "")
		{
			return MailChimpEndPoint + uriSegment + "?apikey=" + _apiKey + "&" + queryString;
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

		public void DeleteUnUsedTags(string listId)
		{
			var listSegments = MailChimpManager.ListSegments.GetAllAsync(listId).Result;

			foreach (var segment in listSegments)
			{
				if (segment.MemberCount == 0)
				{
					var returnObj = MailChimpManager.ListSegments.DeleteAsync(listId, segment.Id.ToString());
				}
			}
		}

		public Member AddTagsToExistingMember(Member member, string listId, List<string> addTags)
		{
			var sanatizedTagList = new List<string>();

			foreach (var item in addTags)
			{
				if (string.IsNullOrEmpty(item))
					continue;

				sanatizedTagList.Add(item.Trim().ToLower());
			}

			var listSegments = MailChimpManager.ListSegments.GetAllAsync(listId).Result;

			var foundSegments = listSegments.Where(i => sanatizedTagList.Contains(i.Name));

			foreach (var item in foundSegments)
			{
				sanatizedTagList.Remove(item.Name.Trim().ToLower());
			}

			foreach (var newTag in sanatizedTagList)
			{
				MailChimpManager.ListSegments.AddAsync(listId, new Segment() { Name = newTag, EmailAddresses = new List<string>() { member.EmailAddress } });
			}

			//foreach (var currentTag in member.Tags)
			//{
			//	if (!addSegments.Any(i => i.Id == currentTag.Id))
			//	{
			//		MailChimpManager.ListSegments.DeleteMemberAsync(listId, currentTag.Id.ToString(), member.EmailAddress);
			//	}
			//}

			foreach (var segment in foundSegments)
			{
				MailChimpManager.ListSegments.AddMemberAsync(listId, segment.Id.ToString(), member);
			}

			member = MailChimpManager.Members.GetAsync(listId, member.EmailAddress).Result;

			DeleteUnUsedTags(listId);

			return member;
		}

		public Return AddUpdateEmailAddress(string listId, User user, Dictionary<string, object> mergeFields = null, List<string> addTags = null)
		{
			var returnObj = new Return();

			try
			{
				// Use the Status property if updating an existing member

				var memberExists = MailChimpManager.Members.ExistsAsync(listId, user.EmailAddress).Result;

				Member member = null;

				if (memberExists)
				{
					member = MailChimpManager.Members.GetAsync(listId, user.EmailAddress).Result; //new Member { EmailAddress = user.EmailAddress, Status = Status.Subscribed, StatusIfNew = Status.Subscribed };
				}
				else
				{
					member = new Member { EmailAddress = user.EmailAddress, Status = Status.Subscribed, StatusIfNew = Status.Subscribed };
				}

				if (mergeFields != null)
				{
					foreach (var mergeField in mergeFields)
					{
						member.MergeFields.Add(mergeField.Key, mergeField.Value);
					}
				}

				if (addTags != null)
				{
					var listSegments = MailChimpManager.ListSegments.GetAllAsync(listId).Result;
					var addSegments = listSegments.Where(i => addTags.Contains(i.Name));

					foreach (var currentTag in member.Tags)
					{
						if (!addSegments.Any(i => i.Id == currentTag.Id))
						{
							MailChimpManager.ListSegments.DeleteMemberAsync(listId, currentTag.Id.ToString(), member.EmailAddress);
						}
					}

					foreach (var segment in addSegments)
					{
						MailChimpManager.ListSegments.AddMemberAsync(listId, segment.Id.ToString(), member);
					}
				}

				/*var birthDay = new DateTime(DateTime.Now.Year, user.Month, user.Day);

                member.MergeFields.Add("FNAME", user.Name);				
				member.MergeFields.Add("CITY", user.City);
				member.MergeFields.Add("BIRTHDAY", birthDay.ToString("MM/dd"));
				member.MergeFields.Add("REGION", user.Region);*/

				//member.MergeFields.Add("LNAME", user.LastName);

				member = MailChimpManager.Members.AddOrUpdateAsync(listId, member).Result;

				if (addTags != null)
				{
					member = AddTagsToExistingMember(member, listId, addTags);
				}

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
			mailChimpHelper.AddUpdateEmailAddress("6923a0bab7", user, new Dictionary<string, object>());


			mailChimpHelper = new MailChimpHelper("9544ffb4a4ac084ccd52fd068ed0ce38-us15");
			mailChimpHelper.AddUpdateEmailAddress("6923a0bab7", user, new Dictionary<string, object>());
		}
	}
}
