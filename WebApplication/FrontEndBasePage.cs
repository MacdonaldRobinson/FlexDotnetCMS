using FrameworkLibrary;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using WebApplication.Services;

namespace WebApplication
{
    public class FrontEndBasePage : BasePage
    {
        private string currentPageVirtualPath = "";

        public WebFormHelper WebFormHelper = new WebFormHelper();


        public FrontEndBasePage()
        {
            this.masterFilesDirPath = "~/Views/MasterPages/";

            this.PreInit += new EventHandler(Page_PreInit);
            this.Init += new EventHandler(Page_Init);
            this.PreLoad += new EventHandler(Page_PreLoad);
            this.PreRender += new EventHandler(Page_PreRender);

        }

        public void AddCommonIncludes()
        {
            if (Master == null)
                return;

            if (AppSettings.IsRunningOnDev)
            {
                this.AddNoIndexAndNoFollowMetaTag();
            }
        }

        public void AddDefaultTemplateVars()
        {
            this.TemplateVars = GetDefaultTemplateVars(TemplateBaseUrl);
            //this.TemplateVars["PageID"] = this.Page.ToString();
            Page.MaintainScrollPositionOnPostBack = true;
        }

        public string TemplateBaseUrl
        {
            get
            {
                var masterPageFile = this.MasterPageFile;

                if (masterPageFile == null)
                    masterPageFile = URIHelper.ConvertToAbsPath(GetMasterPageFilePath());

                var tmp = URIHelper.GetUriSegments(masterPageFile).ToList();

                tmp.RemoveAt(tmp.Count - 1);
                var masterFolder = string.Join("/", tmp);

                var baseUrl = URIHelper.ConvertAbsUrlToTilda(URIHelper.ConvertAbsPathToAbsUrl(masterFolder));

                if (!baseUrl.EndsWith("/"))
                    baseUrl += "/";

                return URIHelper.ConvertToAbsUrl(baseUrl);
            }
        }

        public void AddJSFile(string path)
        {
            if (JsIncludesPlaceHolder == null)
                return;

            WebFormHelper.AddJSFile(path, JsIncludesPlaceHolder, TemplateBaseUrl);
        }

        public void AddNoIndexAndNoFollowMetaTag()
        {
            if (MetaIncludesPlaceHolder == null)
                return;

            StateBag bag = new StateBag();
            bag.Add("name", "robots");
            bag.Add("content", "noindex, nofollow");

            WebFormHelper.AddMetaTag(new AttributeCollection(bag), MetaIncludesPlaceHolder);
        }

        public void AddCSSFile(string path, Dictionary<string, string> attributes = null)
        {
            if (CssIncludesPlaceHolder == null)
                return;

            WebFormHelper.AddCSSFile(path, CssIncludesPlaceHolder, TemplateBaseUrl, attributes);
        }

        public static new Settings GetSettings()
        {
            return BasePage.GetSettings();
        }

        public void Page_PreInit(object sender, EventArgs e)
        {
            var httpRuntimeSection = new System.Web.Configuration.HttpRuntimeSection();
            var settings = GetSettings();

            if (settings.MaxRequestLength > 0)
                httpRuntimeSection.MaxRequestLength = settings.MaxRequestLength;

            WebFormHelper.ClearIncludesList();
            PreloadHelper.PreloadList.Clear();

            if (AppSettings.UseLoadFileServiceUrl)
                WebFormHelper.LoadFileServiceUrl = AppSettings.FileServiceHandlerUrl + AppSettings.LoadFileUriSegment;
            else
                WebFormHelper.LoadFileServiceUrl = "";

            WebFormHelper.CombineCssAndJsIncludes = AppSettings.CombineCssAndJsIncludes;

            if (Request["devAction"] != null)
            {
                switch (Request["devAction"])
                {
                    case "ClearAllCache":
                        ContextHelper.ClearAllMemoryCache();
                        break;
                }
            }

            if (this.MasterPageFile != null)
            {
                var masterFilePath = GetMasterPageFilePath();

                if (File.Exists(URIHelper.ConvertToAbsPath(masterFilePath)))
                    MasterPageFile = masterFilePath;
                else
                    MasterPageFile = "";
            }

            if (currentPageVirtualPath == "")
                currentPageVirtualPath = URIHelper.GetCurrentVirtualPath();

            if (FrameworkSettings.CurrentFrameworkBaseMedia?.CurrentMediaDetail == null)
                return;

            if (currentPageVirtualPath == "")
                currentPageVirtualPath = URIHelper.GetCurrentVirtualPath();

            if (!CanAccessSection())
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
        }

        public void Page_Init(object sender, EventArgs e)
        {
            AddCommonIncludes();

            var currentVirtualPath = URIHelper.GetCurrentVirtualPath();
            if (URIHelper.IsSame(FormsAuthentication.LoginUrl, currentVirtualPath))
                return;

            if (FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail == null)
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

        public void Page_PreLoad(object sender, EventArgs e)
        {
            if ((FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail != null) && (Page != null))
            {
                /*
                 * TODO: Reimplement
                 * Page.MetaDescription = StringHelper.StripExtraSpaces(StringHelper.StripHtmlTags(FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.GetMetaDescription()));
                Page.MetaKeywords = StringHelper.StripExtraSpaces(StringHelper.StripHtmlTags(FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.GetMetaKeywords()));
                Page.Title = FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.GetPageTitle();*/
            }
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            WebFormHelper.GetJSIncludes(this.Page);

            if (AppSettings.CombineCssAndJsIncludes)
            {
                var slug = StringHelper.CreateSlug(Request.Url.PathAndQuery);
                var cssLoader = WebFormHelper.GenerateCssFileTag(AppSettings.LoadCssIncludesUrl + "_" + slug, TemplateBaseUrl, null, true);
                var jsLoader = WebFormHelper.GenerateJsFileTag(AppSettings.LoadJsIncludesUrl + "_" + slug, TemplateBaseUrl, null, true);

                JsIncludesPlaceHolder.Controls.Add(jsLoader);
                CssIncludesPlaceHolder.Controls.Add(cssLoader);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            System.IO.StringWriter str = new System.IO.StringWriter();
            HtmlTextWriter wrt = new HtmlTextWriter(str);

            // render html
            base.Render(wrt); //CAPTURE THE CURRENT PAGE HTML SOURCE AS STRING
            //wrt.Close();

            string html = str.ToString();

            if(IsAjaxRequest)
            {
                writer.Write(html);
                return;
            }

            /*if (CurrentMediaDetail != null && !html.Contains("<html"))
            {
                var masterPage = CurrentMediaDetail.GetMasterPage();

                if (masterPage != null)
                {
                    if (masterPage.UseLayout)
                    {
                        html = masterPage.Layout.Replace("{PageContent}", html);
                        html = LoaderHelper.RenderPage(this, html);
                    }
                }
            }*/

            /*if (!IsInAdminSection)
            {
                if ((AppSettings.EnableMobileDetection) && (FrontEndBasePage.IsMobile))
                    html = HandleMobileLayout(html);
                else
                    html = HandleNonMobileLayout(html);
            }*/

            if (!IsPostBack)
            {
                if (AppSettings.MinifyOutput)
                    html = StringHelper.StripExtraSpacesBetweenMarkup(html);
            }

            if (CurrentMediaDetail != null)
            {
                if (!IsAjaxRequest && !IsInAdminSection && Request["VisualLayoutEditor"] != "true")
                {
                    html = MediaDetailsMapper.ParseSpecialTags(CurrentMediaDetail, html);
                }
            }

            /*if (!IsInAdminSection)
            {
                HtmlNode.ElementsFlags.Remove("form");
                document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);

                var forms = document.DocumentNode.SelectNodes("//form");

                if (forms != null && forms.Count > 1)
                {
                    forms.RemoveAt(0);
                    foreach (HtmlNode item in forms)
                    {
                        item.ParentNode.InnerHtml = item.ParentNode.InnerHtml.Replace("form", "div data-form");
                    }

                    html = document.DocumentNode.WriteContentTo();
                }
            }*/


            /*var settings = SettingsMapper.GetSettings();

            if (settings.EnableGlossaryTerms && !IsInAdminSection)
            {
                var document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);

                var selectedNodes = document.DocumentNode.SelectNodes("//p/text()|//li/text()");
                var terms = GlossaryTermsMapper.GetAll();

                if (selectedNodes != null)
                {
                    foreach (HtmlNode node in selectedNodes)
                    {
                        foreach (var term in terms)
                        {
                            var tempTerm = term.Term.Trim();

                            node.InnerHtml = Regex.Replace(node.InnerHtml, @"\b" + Regex.Escape(tempTerm) + @"\b" + "(?![^<]*</[a-z]+>)", me =>
                            {
                                var template = "<span data-toggle=\"tooltip\" title=\"" + term.Definition + "\">" + me.Value + "</span>";
                                return template;
                            }, RegexOptions.IgnoreCase);
                        }
                    }
                }

                html = document.DocumentNode.WriteContentTo();
            }*/

            html = ParserHelper.ParseData(html, TemplateVars);

            if (CurrentMediaDetail != null)
            {
                if (!IsAjaxRequest)
                {
                    if (AppSettings.EnableOutputCaching && CurrentMediaDetail.EnableCaching && CurrentMediaDetail.CanRender)
                    {
                        if (AppSettings.EnableLevel1MemoryCaching)
                        {
                            CurrentMediaDetail.SaveToMemoryCache(UserSelectedVersion, html, Request.Url.Query);
                        }

                        if (AppSettings.EnableLevel2FileCaching)
                        {
                            CurrentMediaDetail.SaveToFileCache(UserSelectedVersion, html, Request.Url.Query);
                        }

                        if (AppSettings.EnableLevel3RedisCaching)
                        {
                            CurrentMediaDetail.SaveToRedisCache(UserSelectedVersion, html, Request.Url.Query);
                        }
                    }

                    ContextHelper.SetToSession("CurrentMediaDetail", CurrentMediaDetail);
                }
            }

            /*if(AppSettings.ForceSSL)
            {
                html = html.Replace("http:", "https:");
            }*/

            html = ParserHelper.ReplaceHrefAndSrcsToAbsoluteUrls(html);

            writer.Write(html);
        }

        public void RenderQRCode()
        {
            //Services.Barcode barcode = new Services.Barcode();
            //barcode.GenerateQRCode(URIHelper.ConvertToAbsUrl(this.CurrentMediaDetail.VirtualPath));
        }

        public static void HandleFormatQueryString(IMediaDetail mediaDetail, string formatQueryString, string depthQueryString)
        {
            var Request = HttpContext.Current.Request;
            switch (formatQueryString.ToLower().Trim())
            {
                case "json":
                    {
                        var depth = (!string.IsNullOrEmpty(depthQueryString)) ? long.Parse(depthQueryString) : 1;

                        RenderJSON(mediaDetail, depth);
                    }
                    break;
                case "rss":
                    RenderRss();
                    break;
            }
        }

        public static void RenderJSON(IMediaDetail mediaDetail, long depth)
        {
            var Response = HttpContext.Current.Response;

            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/json";
            Response.StatusCode = 200;

            var json = mediaDetail.ToJson(depth);

            Response.Write(json);

            Response.Flush();
            Response.End();
        }

        public static void RenderRss(IEnumerable<RssItem> rssItems = null, string rssTitle = "", string rssDescription = "", string rssLink = "")
        {
            if (rssItems == null)
            {
                rssItems = MediaDetailsMapper.GetRssItems(MediaDetailsMapper.GetAllChildMediaDetails(FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMedia.ID, FrameworkSettings.CurrentFrameworkBaseMedia.CurrentLanguage.ID).Where(i=>i.CanRender && i.ShowInMenu));
            }

            if (rssLink == "")
            {
                if (FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail != null)
                    rssLink = URIHelper.ConvertToAbsUrl(FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.VirtualPath);
                else
                    rssLink = URIHelper.ConvertToAbsUrl(URIHelper.GetCurrentVirtualPath());
            }

            if (rssTitle == "")
            {
                rssTitle = FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.Title;
            }

            if (rssDescription == "")
            {
                rssDescription = FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.GetMetaDescription();
            }

            Rss rss = new Rss(rssTitle, rssLink, rssDescription);
            rss.Items = rssItems;

            var Response = HttpContext.Current.Response;

            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/xml";

            RssHelper.WriteRss(rss, Response.OutputStream);

            Response.Flush();
            Response.End();
        }
    }
}