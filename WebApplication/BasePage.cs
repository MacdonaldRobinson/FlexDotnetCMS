using FrameworkLibrary;
using HtmlAgilityPack;
using MaxMind.GeoIP2.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public class BasePage : System.Web.UI.Page
    {
        protected string masterFilesDirPath = "";
        private PageStatePersister pageStatePersister;

        protected override PageStatePersister PageStatePersister
        {
            get
            {
                // Unlike as exemplified in the MSDN docs, we cannot simply return a new PageStatePersister
                // every call to this property, as it causes problems
                return pageStatePersister ?? (pageStatePersister = new Handlers.CustomPageStatePersister(this));
            }
        }

        public BasePage()
        {
            if (FrameworkSettings.CurrentFrameworkBaseMedia == null)
                FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstanceByVirtualPath("", true);

            AddDefaultTemplateVars();
        }

        public void AddDefaultTemplateVars()
        {
            this.TemplateVars = GetDefaultTemplateVars("");
            //this.TemplateVars["PageID"] = this.Page.ToString();
            Page.MaintainScrollPositionOnPostBack = true;
        }

        public static Settings GetSettings()
        {
            return SettingsMapper.GetSettings();
        }

        private string HandleMobileLayout(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var items = htmlDoc.DocumentNode.SelectNodes("//*[@class='nonMobile']");

            if (items != null)
            {
                foreach (var item in items)
                    item.Remove();
            }

            htmlDoc.OptionWriteEmptyNodes = true;
            return htmlDoc.DocumentNode.InnerHtml;
        }

        private string HandleNonMobileLayout(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var items = htmlDoc.DocumentNode.SelectNodes("//*[@class='mobile']");

            if (items != null)
            {
                foreach (var item in items)
                    item.Remove();
            }
            return htmlDoc.DocumentNode.InnerHtml;
        }

        public bool CanAccessSection()
        {            
            if (CurrentMediaDetail.CheckEnforceRoleLimitationsOnFrontEnd())
            {
                return CurrentMediaDetail.CanUserAccessSection(FrameworkSettings.CurrentUser);                
            }

            return true;
        }

        public static bool IsInAdminSection
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("/admin/");
            }
        }

        public static bool IsMobileDevice
        {
            get
            {
                if (!AppSettings.EnableMobileDetection)
                    return false;

                var Request = HttpContext.Current.Request;

                if (Request.UserAgent == null)
                    return false;

                string strUserAgent = Request.UserAgent.ToString().ToLower();

                bool isMobileDevice = false;

                if (strUserAgent.Contains("ipad"))
                    return false;

                if (strUserAgent != null)
                {
                    if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("iphone") ||
                        strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
                        strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") ||
                        strUserAgent.Contains("palm") || strUserAgent.Contains("android"))
                    {
                        return true;
                    }
                }

                return isMobileDevice;
            }
        }

        public static bool IsMobile
        {
            get
            {
                var Request = HttpContext.Current.Request;

                string setVersion = Request["UserSelectedVersion"];

                if (setVersion == null)
                    setVersion = UserSelectedVersion.ToString();

                if (!string.IsNullOrEmpty(setVersion))
                {
                    var tmpUserSelectedVersion = RenderVersion.HTML;
                    Enum.TryParse(setVersion, out tmpUserSelectedVersion);

                    UserSelectedVersion = tmpUserSelectedVersion;

                    return (tmpUserSelectedVersion == RenderVersion.Mobile) ? true : false;
                }

                return false;
            }
        }

        public static FrameworkLibrary.RenderVersion UserSelectedVersion
        {
            get
            {
                var version = RenderVersion.HTML;
                /*var version = ContextHelper.Get("UserSelectedVersion", ContextType.Session);

                if (version == null)
                {*/
                    if (IsMobileDevice)
                        version = RenderVersion.Mobile;
                    else
                        version = RenderVersion.HTML;
                //}

                return (RenderVersion)version;
            }
            set
            {
                //ContextHelper.Set("UserSelectedVersion", value, ContextType.Session);
            }
        }

        public void AddToCart(IMediaDetail detail)
        {
            CurrentCart.AddItem(detail);
        }

        public Cart CurrentCart
        {
            get
            {
                return WebApplication.Controls.ShoppingCart.ShoppingCart.CurrentCart;
            }
        }

        /*public string GetRedirectToMediaDetailUrl(MediaType mediaType, long selectedMediaId, long? parentMediaId = null, long historyVersion = 0)
        {
            return GetRedirectToMediaDetailUrl(mediaType, selectedMediaId, parentMediaId, historyVersion);
        }*/

        public static string GetAdminUrl(long mediaTypeId, long selectedMediaId, long? parentMediaId = null, long historyVersion = 0, bool popupTemplate = false)
        {
            var url = $"{URIHelper.BaseUrl}Admin/Views/PageHandlers/Media/Detail.aspx?mediaTypeId={mediaTypeId}&selectedMediaId={selectedMediaId}&parentMediaId={parentMediaId}&historyVersion={historyVersion}";

            if(popupTemplate)
            {
                url = $"{url}&masterFilePath=~/Admin/Views/MasterPages/Popup.Master";
            }

            return url;
        }

        public static string GetAdminUrl(IMediaDetail detail, bool popupTemplate = false)
        {
            return GetAdminUrl(detail.MediaTypeID, detail.MediaID, detail.Media.ParentMediaID, detail.HistoryVersionNumber, popupTemplate);
        }

        public static void RedirectToAdminUrl(IMediaDetail detail, long historyVersion = 0)
        {
            RedirectToAdminUrl(detail.MediaTypeID, detail.MediaID, detail.Media.ParentMediaID, detail.Language, detail, historyVersion);
        }

        public static void RedirectToAdminUrl(long mediaTypeId, long selectedMediaId, long? parentMediaId = null, Language language = null, IMediaDetail detail = null, long historyVersion = 0)
        {
            if (language != null)
                FrameworkSettings.SetCurrentLanguage(language);

            var url = GetAdminUrl(mediaTypeId, selectedMediaId, parentMediaId, historyVersion);

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["masterFilePath"]))
                url = url + "&masterFilePath=" + HttpContext.Current.Request["masterFilePath"];

            HttpContext.Current.Response.Redirect(url, true);
        }


        public void AddToJSPreload(string preloadUrl)
        {
            PreloadHelper.AddToList(preloadUrl);
        }

        public string GetMasterPageFilePath()
        {
            string masterFilePath = HttpContext.Current.Request["masterFilePath"];

            if (masterFilePath == null)
                masterFilePath = "";

            var useMobile = ((!GetType().BaseType.Namespace.Contains(".Admin") && (IsMobile)));

            if (GetType().BaseType.Namespace.Contains(".Admin"))
            {
                if (File.Exists(URIHelper.ConvertToAbsPath(masterFilePath)))
                    return masterFilePath;

                return Page.MasterPageFile;
            }

            if (string.IsNullOrEmpty(masterFilePath) && FrameworkSettings.CurrentFrameworkBaseMedia?.CurrentMediaDetail != null)
            {
                var masterPage = ((MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail).GetMasterPage();

                if (masterPage != null)
                {
                    masterFilePath = masterPage.PathToFile;

                    if (useMobile)
                        masterFilePath = masterPage.GetMobileTemplate();

                    if(masterPage.UseLayout)
                    {
                        masterFilePath = "";
                    }
                }
                else
                {
                    if ((FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.Handler == null) || (FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.Handler == ""))
                    {
                        var mediaType = FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.MediaType;

                        if (mediaType.MasterPage == null)
                        {
                            var defaultMaster = MasterPagesMapper.GetDefaultMasterPage();

                            if (defaultMaster != null)
                            {
                                masterFilePath = defaultMaster.PathToFile;

                                if (useMobile)
                                    masterFilePath = defaultMaster.GetMobileTemplate();
                            }
                        }
                        else
                        {
                            masterFilePath = mediaType.MasterPage.PathToFile;

                            if (useMobile)
                                masterFilePath = mediaType.MasterPage.GetMobileTemplate();
                        }
                    }
                    else
                    {
                        masterPage = MasterPagesMapper.GetByPathToFile(Page.MasterPageFile);

                        if (useMobile)
                            masterFilePath = masterPage?.MobileTemplate;
                        else
                            masterFilePath = masterPage?.PathToFile;
                    }
                }
            }

            return masterFilePath;
        }

        public Website CurrentWebsite
        {
            get
            {
                return WebsitesMapper.GetWebsite();
            }
        }

        public Media CurrentMedia
        {
            get
            {
                return FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMedia;
            }
        }

        public IMediaDetail CurrentMediaDetail
        {
            get
            {
                return FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;
            }
        }

        public Language CurrentLanguage
        {
            get
            {
                return FrameworkSettings.CurrentFrameworkBaseMedia.CurrentLanguage;
            }
        }

        public User CurrentUser
        {
            get
            {
                return FrameworkSettings.CurrentFrameworkBaseMedia.CurrentUser;
            }
            set
            {
                FrameworkSettings.CurrentFrameworkBaseMedia.CurrentUser = value;
            }
        }

        public string CurrentPageVirtualPath { get; set; }

        public CityResponse CurrentVisitorLocation
        {
            get
            {
                if (AppSettings.EnableGeoLocation)
                {
                    GeoLocationHelper.APIKey = AppSettings.GeoLocationAPIKey;
                    return FrameworkSettings.CurrentFrameworkBaseMedia.CurrentVisitorLocation;
                }

                return null;
            }
        }

        public string CurrentVisitorIP
        {
            get
            {
                return FrameworkSettings.CurrentFrameworkBaseMedia.CurrentVisitorIP;
            }
        }

        public void CheckOut(IMediaDetail detail)
        {
            CheckInAll();
            var checkInItems = new List<KeyValuePair<IMediaDetail, User>>();

            foreach (KeyValuePair<IMediaDetail, User> item in CheckedOutItems)
            {
                if (item.Value.ID == CurrentUser.ID)
                    checkInItems.Add(item);
            }

            foreach (KeyValuePair<IMediaDetail, User> item in checkInItems)
            {
                CheckedIn(item.Key);
            }

            if (CheckedOutItems != null && detail != null && CurrentUser != null)
                CheckedOutItems.Add(detail, CurrentUser);
        }

        public void CheckedIn(IMediaDetail detail)
        {
            KeyValuePair<IMediaDetail, User> item = CheckedOutItems.Where(i => i.Key.ID == detail.ID).FirstOrDefault();

            detail = item.Key;
            User checkoutUser = item.Value;

            if (CheckedOutItems.TryGetValue(detail, out checkoutUser))
            {
                if (checkoutUser.ID == CurrentUser.ID)
                    CheckedOutItems.Remove(detail);
            }
        }

        public void CheckInAll()
        {
            var items = new Dictionary<IMediaDetail, User>();

            foreach (var item in CheckedOutItems)
                items.Add(item.Key, item.Value);

            foreach (KeyValuePair<IMediaDetail, User> item in items)
                CheckedIn(item.Key);
        }

        public KeyValuePair<IMediaDetail, User> IsCheckedOut(IMediaDetail detail)
        {
            foreach (KeyValuePair<IMediaDetail, User> item in CheckedOutItems)
            {
                if (item.Key.ID == detail.ID)
                    return item;
            }

            return default(KeyValuePair<IMediaDetail, User>);
        }

        public Dictionary<IMediaDetail, User> CheckedOutItems
        {
            get
            {
                return FrameworkSettings.CheckedOutItems;
            }
        }

        public static Dictionary<string, string> GetDefaultTemplateVars(string templateBaseUrl)
        {
            var templateVars = new Dictionary<string, string>();

            if (LanguagesMapper.GetAllActive().Count() > 1)
                templateVars["BaseUrlWithLanguage"] = URIHelper.BaseUrlWithLanguage;
            else
                templateVars["BaseUrlWithLanguage"] = URIHelper.BaseUrl;

            templateVars["BaseUrl"] = URIHelper.BaseUrl;

            //templateVars["CurrentUrl"] = URIHelper.ConvertToAbsUrl(URIHelper.GetCurrentVirtualPath());
            templateVars["LoadJsIncludesUrl"] = AppSettings.LoadJsIncludesUrl;
            templateVars["LoadCssIncludesUrl"] = AppSettings.LoadCssIncludesUrl;
            templateVars["TemplateBaseUrl"] = templateBaseUrl;
            templateVars["FlashVersionBaseUrl"] = URIHelper.BaseUrl + AppSettings.FlashVersionBaseUri;

            return templateVars;
        }

        public static bool IsAjaxRequest
        {
            get
            {
                var request = HttpContext.Current.Request;
                if (request == null)
                {
                    throw new ArgumentNullException("request");
                }
                var context = HttpContext.Current;
                var isCallbackRequest = false;// callback requests are ajax requests
                if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
                {
                    isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
                }
                return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
            }
        }

        private string currentPageVirtualPath = "";
        public string CurrentMediaVirtualPath
        {
            get
            {
                return currentPageVirtualPath.ToLower();
            }
            set
            {
                currentPageVirtualPath = value.ToLower();
            }
        }

        public List<string> UriSegments { get; set; } = new List<string>();


        public Control JsIncludesPlaceHolder
        {
            get
            {
                return Master.FindControl(AppSettings.JsIncludesPlaceHolderID);
            }
        }

        public Control CssIncludesPlaceHolder
        {
            get
            {
                return Master.FindControl(AppSettings.CssIncludesPlaceHolderID);
            }
        }

        public Control MetaIncludesPlaceHolder
        {
            get
            {
                return Master.FindControl(AppSettings.MetaIncludesPlaceHolderID);
            }
        }

        public Dictionary<string, string> TemplateVars { get; set; } = new Dictionary<string, string>();

        public void ClearIncludes()
        {
            if (JsIncludesPlaceHolder != null)
                JsIncludesPlaceHolder.Controls.Clear();

            if (CssIncludesPlaceHolder != null)
                CssIncludesPlaceHolder.Controls.Clear();

            if (MetaIncludesPlaceHolder != null)
                MetaIncludesPlaceHolder.Controls.Clear();
        }

        public Control FindControlRecursive(string id)
        {
            return WebFormHelper.FindControlRecursive(this.Page, id);
        }



        public void ExecuteRawJS(string js)
        {
            //ClientScript.RegisterClientScriptBlock(GetType(), Guid.NewGuid().ToString(), js, true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), js, true);
        }

        public void ExecuteRawJQuery(string js)
        {
            ClientScript.RegisterClientScriptBlock(GetType(), Guid.NewGuid().ToString(), "<script type='text/javascript'>$(document).ready(function(){ " + js + " });</script>");
        }

        public void JSRedirect(string location, string timeout = "1000")
        {
            ExecuteRawJS("jQuery(document).ready(function(){ setTimeout(\"window.location.href='" + location + "';\", " + timeout + ") });");
        }

        public void DisplayjGrowlMessage(jGrowlMessage message)
        {
            ExecuteRawJS(jGrowlHelper.GenerateCode(message, true));
        }

        public void DisplaySuccessMessage(string message)
        {
            DisplayjGrowlMessage(new jGrowlMessage("Success", message, jGrowlMessage.jGrowlMessageType.Success));
        }

        public void DisplayErrorMessage(string message, Elmah.Error error = null)
        {
            DisplayjGrowlMessage(new jGrowlMessage("Error", message, jGrowlMessage.jGrowlMessageType.Error, error));
        }

        public void DisplayFeedbackMessage(string message)
        {
            DisplayjGrowlMessage(new jGrowlMessage("Feedback", message, jGrowlMessage.jGrowlMessageType.Feedback));
        }

        public void DisplayjGrowlMessages(IEnumerable<jGrowlMessage> messages)
        {
            ExecuteRawJS(jGrowlHelper.GenerateCode(messages, true));
        }

        public Return SendNotificationToAdministrators(string message, string subject, List<User> toAdministrators = null)
        {
            if (toAdministrators == null)
                toAdministrators = new List<User>();

            toAdministrators.AddRange(UsersMapper.GetAllByRoleEnum(RoleEnum.Administrator));

            return SendEmailToUsers(toAdministrators, subject, message);
        }

        public Return SendEmailToUsers(IEnumerable<User> users, string subject, string message)
        {
            string emails = "";

            foreach (User user in users)
                emails += user.EmailAddress + ";";

            return EmailHelper.Send(AppSettings.SystemEmailAddress, EmailHelper.GetMailAddressesFromString(emails), subject, message);
        }

        public Return SendEmailToUser(User user, string message, string subject)
        {
            return EmailHelper.Send(AppSettings.SystemEmailAddress, EmailHelper.GetMailAddressesFromString(user.EmailAddress), subject, message);
        }

        public string CommentReplyToMessage(Comment reply)
        {
            string message = @"There was a reply made to your comment ( If you dont see it on the site, It might still be in moderation ): <br /><i>" + reply.ReplyToComment.Message + @"</i><br /><br />
                <strong>Comment Details -</strong><br /><br />
                <strong>Name:</strong>" + reply.Name + @"<br />
                <strong>Email:<strong>" + reply.Email + @"<br />
                <strong>Message:</strong><br />
                " + reply.Message + "";

            return message;
        }

        public void SendMediaReplyToComment(Comment newComment, Comment replyToComment)
        {
            var address = new List<MailAddress>();
            address.Add(new MailAddress(replyToComment.Email, replyToComment.Name));

            EmailHelper.Send(AppSettings.SystemEmailAddress, address, "There was a reply made to your comment", CommentReplyToMessage(newComment));
        }

        public void SendMediaCommentApprovalRequest(Media obj)
        {
            string message = "";
            var liveMediaDetail = obj.GetLiveMediaDetail();
            message = "There was a comment made on a news article with the title '" + liveMediaDetail.Title + "', click on the following link to approve or reject this comment: " + URIHelper.BaseUrl + "admin/" + liveMediaDetail.MediaType.Name + "/Edit.aspx?id=" + obj.ID;
            SendEmailToUser(liveMediaDetail.CreatedByUser, message, "Comment Approval Request");
        }
    }
}