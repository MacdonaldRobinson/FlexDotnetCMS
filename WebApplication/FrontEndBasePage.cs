using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Web.Security;
using WebApplication.Services;

namespace WebApplication
{
    public class FrontEndBasePage : BasePage
    {
        public FrontEndBasePage()
        {
            this.masterFilesDirPath = "~/Views/MasterPages/";
        }

        public static new Settings GetSettings()
        {
            return BasePage.GetSettings();
        }

        public void Page_Init(object sender, EventArgs e)
        {
            var currentVirtualPath = URIHelper.GetCurrentVirtualPath();
            if (URIHelper.IsSame(FormsAuthentication.LoginUrl, currentVirtualPath))
                return;

            if (FrameworkBaseMedia.CurrentMediaDetail == null)
                Response.Redirect("~/");

            /*if (!IsPostBack)
            {
                if ((!URIHelper.IsSpecialRequest(currentVirtualPath)) && !CurrentMediaDetail.IsCacheDataStale(UserSelectedVersion) && CurrentMediaDetail.EnableCaching && AppSettings.EnableOutputCaching && !CurrentMediaDetail.VirtualPath.Contains("/admin/") && !CurrentMediaDetail.VirtualPath.Contains("/login/") && !CurrentMediaDetail.VirtualPath.Contains("/search/"))
                {
                    switch (UserSelectedVersion)
                    {
                        case RenderVersion.Mobile:
                            {
                                BaseService.WriteRaw(CurrentMediaDetail.MobileCacheData + "<!--Loaded From Cache-->");
                                break;
                            }
                        case RenderVersion.HTML:
                            {
                                BaseService.WriteRaw(CurrentMediaDetail.HtmlCacheData + "<!--Loaded From Cache-->");
                                break;
                            }
                    }
                }
            }*/

            string id = CurrentMediaDetail.VirtualPath.Trim().Replace("~/", "").Replace("/", "-");

            if (id.EndsWith("-"))
                id = id.Substring(0, id.Length - 1);

            this.TemplateVars["BodyClass"] = id;
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            switch (Request.QueryString["format"])
            {
                case "json":
                    {
                        var depth = (Request.QueryString["depth"] != null) ? long.Parse(Request.QueryString["depth"]) : 1;

                        RenderJSON(depth);
                    }
                    break;
                case "rss":
                    RenderRss();
                    break;
            }
        }

        public void RenderQRCode()
        {
            //Services.Barcode barcode = new Services.Barcode();
            //barcode.GenerateQRCode(URIHelper.ConvertToAbsUrl(this.CurrentMediaDetail.VirtualPath));
        }

        public void RenderJSON(long depth)
        {
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/json";
            Response.StatusCode = 200;

            var json = ParserHelper.ParseData(this.CurrentMediaDetail.ToJSON(depth), TemplateVars);

            Response.Write(json);

            Response.Flush();
            Response.End();
        }

        public void RenderRss(IEnumerable<RssItem> rssItems = null, string rssTitle = "", string rssDescription = "", string rssLink = "")
        {
            if (rssItems == null)
            {
                rssItems = MediaDetailsMapper.GetRssItems(MediaDetailsMapper.FilterByCanRenderStatus(MediaDetailsMapper.FilterOutHiddenAndDeleted(MediaDetailsMapper.GetAllChildMediaDetails(FrameworkBaseMedia.CurrentMedia, FrameworkBaseMedia.CurrentLanguage)), true));
            }

            if (rssLink == "")
            {
                if (FrameworkBaseMedia.CurrentMediaDetail != null)
                    rssLink = URIHelper.ConvertToAbsUrl(FrameworkBaseMedia.CurrentMediaDetail.VirtualPath);
                else
                    rssLink = URIHelper.ConvertToAbsUrl(URIHelper.GetCurrentVirtualPath());
            }

            if (rssTitle == "")
            {
                rssTitle = FrameworkBaseMedia.CurrentMediaDetail.Title;
            }

            if (rssDescription == "")
            {
                rssDescription = FrameworkBaseMedia.CurrentMediaDetail.ShortDescription;
            }

            Rss rss = new Rss(rssTitle, rssLink, rssDescription);
            rss.Items = rssItems;

            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/xml";

            RssHelper.WriteRss(rss, Response.OutputStream);

            Response.Flush();
            Response.End();
        }
    }
}