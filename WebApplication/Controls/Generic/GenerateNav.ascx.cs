using FrameworkLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace WebApplication.Controls
{
    public partial class GenerateNav : System.Web.UI.UserControl
    {
        private Media rootMedia = null;
        private bool renderRootMedia = false;
        private bool renderHiddenMediaTypes = false;
        private int renderDepth = 0;
        private int currentDepth = 0;
        private bool mustHaveSameGrandParents = false;
        private bool isFooterManu = false;
        private bool displayProtectedSections = false;
        private bool renderFooterMenuItems = false;
        private string rootULClasses = "";
        private string currentVirtualPath = URIHelper.GetCurrentVirtualPath();
        private Language currentLanguage = FrameworkSettings.GetCurrentLanguage();

        public GenerateNav()
        {
            DividerString = "";
            RenderParentItemInChildNav = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (rootMedia != null || IsBreadCrumbMenu)
                BindRootMedia(rootMedia);
        }
        private void BindRootMedia(Media rootMedia)
        {
            /*var cache = FileCacheHelper.GetGenerateNavCache();

            if (!cache.IsError)
            {
                var cacheData = cache.GetRawData<string>();

                if (cacheData != "")
                {
                    return;
                }
            }*/

            var items = new List<IMediaDetail>();

            if (!IsBreadCrumbMenu)
            {
                /*var itemsCacheKey = $"{rootMedia.ID}_Items";
                var itemsCacheData = (List<IMediaDetail>)ContextHelper.GetFromCache(itemsCacheKey);

                if (itemsCacheData != null)
                {
                    return;
                }
                else
                {*/
                    /*var items = MediaDetailsMapper.GetDataModel().MediaDetails.AsNoTracking().Where(i => i.Media.ParentMediaID == rootMedia.ID && i.HistoryVersionNumber == 0 && i.LanguageID == currentLanguage.ID && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderBy(i => i.Media.OrderIndex); //rootMedia.ChildMedias.SelectMany(m => m.MediaDetails.Where(i => i.HistoryVersionNumber == 0 && (i.ShowInMenu || i.RenderInFooter) && !i.IsDeleted && i.PostPublishDate <= DateTime.Now && (i.PostExpiryDate == null || i.PostExpiryDate > DateTime.Now))).OrderBy(i => i.Media.OrderIndex);
                    Bind(items);*/

                    //var mediaDetail = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.MediaID == rootMedia.ID && i.LanguageID == BasePage.CurrentLanguage.ID);

                    var mediaDetail = rootMedia.GetLiveMediaDetail();

                    if (mediaDetail == null || mediaDetail.MediaType == null)
                        return;

                    if (mediaDetail.MediaType.Name == MediaTypeEnum.RootPage.ToString())
                        rootMedia = WebsitesMapper.GetWebsite().Media;

                    var children = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.ShowInMenu && i.Media.ParentMediaID == rootMedia.ID && i.HistoryVersionNumber == 0 && i.LanguageID == currentLanguage.ID && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderBy(i => i.Media.OrderIndex).ToList(); //rootMedia.ChildMedias.SelectMany(m => m.MediaDetails.Where(i => i.HistoryVersionNumber == 0 && (i.ShowInMenu || i.RenderInFooter) && !i.IsDeleted && i.PostPublishDate <= DateTime.Now && (i.PostExpiryDate == null || i.PostExpiryDate > DateTime.Now))).OrderBy(i => i.Media.OrderIndex);                    
                    items.AddRange(children);
                    //items = mediaDetail.ChildMediaDetails.Where(i=>i.ShowInMenu).ToList();

                    if (!items.Any() && RenderParentNavIfNoChildren)
                    {
                        items = mediaDetail.Media.ParentMedia.GetLiveMediaDetail(mediaDetail.Language).ChildMediaDetails.Where(i => i.ShowInMenu).ToList();
                    }

                    //ContextHelper.SaveToCache(itemsCacheKey, itemsCacheData);
               // }
            }
            else
            {
                items = MediaDetailsMapper.GetAllParentMediaDetails(this.BasePage.CurrentMediaDetail, this.BasePage.CurrentLanguage).ToList();
            }

            Bind(items);
        }

        public void BindItems(IEnumerable<IMediaDetail> items)
        {
            Bind(items);
        }

        /*public override void RenderControl(HtmlTextWriter writer)
        {
            /*var cache = FileCacheHelper.GetGenerateNavCache();

            if(!cache.IsError)
            {
                var cacheData = cache.GetRawData<string>();
                writer.Write(cacheData);
            }
            else
            {
                System.IO.StringWriter str = new System.IO.StringWriter();
                HtmlTextWriter wrt = new HtmlTextWriter(str);

                base.RenderControl(wrt);

                string html = str.ToString();

                //FileCacheHelper.SaveGenerateToNav(html);

                writer.Write(html);
            }          
        }*/

        private void Bind(IEnumerable<IMediaDetail> items)
        {
            if (!IsBreadCrumbMenu)
            {
                if(!RenderHiddenPages)
                {
                    if (IsFooterMenu)
                        items = items.Where(i => i.RenderInFooter);
                    else
                        items = items.Where(i => i.ShowInMenu);
                }

                if (!renderHiddenMediaTypes)
                    items = items.Where(i => i.MediaType.ShowInMenu);

                items = items.OrderBy(i => i.Media.OrderIndex);

                if (renderRootMedia)
                {
                    var newlist = new List<IMediaDetail>();

                    var rootMediaDetail = RootMedia?.GetLiveMediaDetail();

                    if (rootMediaDetail != null)
                        newlist.Add(rootMediaDetail);

                    items = newlist.Concat(items);
                }
            }

            if(!RenderHiddenPages)
            {
                items = items.Where(i => i.ShowInMenu);
            }

            items = items.ToList();

            this.ItemsList.DataSource = items;
            this.ItemsList.DataBind();
        }

        protected void ItemsList_OnLayoutCreated(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            HtmlContainerControl ul = (HtmlContainerControl)list.FindControl("ul");
            var BackButton = (HtmlContainerControl)list.FindControl("BackButton");

            if ((BackButton != null) && (RenderBackButton))
                BackButton.Visible = true;

            if (ul.Attributes["class"] == null)
                ul.Attributes.Add("class", "");

            ul.Attributes["class"] += " level" + currentDepth.ToString();

            if(currentDepth == 1)
            {
                ul.Attributes["class"] += "";
            }

            if (currentDepth == 0)
            {
                ul.Attributes["class"] += " " + RootULClasses;
            }
            else
            {
                ul.Attributes["class"] += " " + SubULClasses;
            }

            ul.Attributes["class"] = ul.Attributes["class"].Trim();

            currentDepth = currentDepth + 1;
        }

        protected void ItemsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HyperLink Link = (HyperLink)e.Item.FindControl("Link");
                ListView ChildList = (ListView)e.Item.FindControl("ChildList");

                IMediaDetail detail = (IMediaDetail)e.Item.DataItem;

                if (!displayProtectedSections && detail.IsProtected)
                {
                    if (BasePage.CurrentUser == null)
                        e.Item.Visible = false;
                }

                Link.Text = detail.LinkTitle;

                string virtualPath = detail.AutoCalculatedVirtualPath;

                if (detail.UseDirectLink)
                {
                    virtualPath = detail.DirectLink;
                }

                if(currentDepth == 1)
                {
                    if (Link.CssClass == "")
                        Link.CssClass = TopLevelAnchorClasses;
                    else
                        Link.CssClass = Link.CssClass + " "+TopLevelAnchorClasses;
                }
                else
                {
                    if (Link.CssClass == "")
                        Link.CssClass = SubAnchorClasses;
                    else
                        Link.CssClass = Link.CssClass + " " + SubAnchorClasses;

                }

                if (detail.OpenInNewWindow)
                    Link.Target = "_blank";

                if (!virtualPath.StartsWith("{") && !virtualPath.EndsWith("/"))
                {
                    if (!virtualPath.Contains("."))
                        virtualPath += "/";
                }

                var path = virtualPath;

                if (!virtualPath.StartsWith("{"))
                {
                    path = URIHelper.ConvertToAbsUrl(virtualPath);

                    if (path.Contains("#") && path.EndsWith("/"))
                        path = path.Remove(path.Length - 1);
                }

                if(detail.RedirectToFirstChild)
                {
                    path = detail.ChildMediaDetails.Where(i => i.ShowInMenu).ElementAt(0).AbsoluteUrl;
                }

                Link.NavigateUrl = path;

                if (IsBreadCrumbMenu)
                {
                    if (URIHelper.GetCurrentVirtualPath() == virtualPath)
                    {
                        Link.NavigateUrl = "javascript:void(0);";
                    }
                }

                HtmlContainerControl li = (HtmlContainerControl)e.Item.FindControl("li");

                if (li.Attributes["class"] == null)
                    li.Attributes.Add("class", "index-" + e.Item.DataItemIndex.ToString());

                if (virtualPath == currentVirtualPath || detail.VirtualPath == currentVirtualPath)
                {
                    li.Attributes["class"] += " current";
                }
                else if (currentVirtualPath.Contains(virtualPath) || currentVirtualPath.Contains(detail.VirtualPath))
                {
                    li.Attributes["class"] += " currentParent";

                    if ((rootMedia != null) && (detail.MediaID == rootMedia.ID))
                        li.Attributes["class"] += " rootParent";
                }

                li.Attributes["class"] += $" MediaID-{detail.MediaID}";

                if (li.Attributes["class"].EndsWith("-"))
                    li.Attributes["class"] = li.Attributes["class"].Substring(0, li.Attributes["class"].Length - 1);

                if (!string.IsNullOrEmpty(detail.CssClasses))
                    li.Attributes["class"] += " " + detail.CssClasses;

                if(!string.IsNullOrEmpty(SubLIClasses))
                {
                    li.Attributes["class"] += " " + SubLIClasses;                    
                }

                li.Attributes["class"] = li.Attributes["class"].Trim();

                IEnumerable<IMediaDetail> childItems = new List<IMediaDetail>();

                if ((rootMedia != null) && (detail.MediaID != rootMedia.ID))
                {
                    //childItems = new List<IMediaDetail>();
                    //childItems = detail.ChildMediaDetails.Where(i=>i.ShowInMenu);
                    
                    //childItems = details.Media.ChildMedias.SelectMany(i => i.MediaDetails.Where(j => j.HistoryVersionNumber == 0 && !j.IsDeleted && j.PostPublishDate <= DateTime.Now && j.LanguageID == details.LanguageID));
                    //childItems = MediaDetailsMapper.FilterOutDeletedAndArchived(MediaDetailsMapper.GetAllChildMediaDetails(details.Media, details.Language));

                    if (!renderHiddenMediaTypes)
                        childItems = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.ShowInMenu && i.Media.ParentMediaID == detail.Media.ID && i.HistoryVersionNumber == 0 && i.LanguageID == detail.LanguageID && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderBy(i => i.Media.OrderIndex).ToList();
                    else
                        childItems = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.Media.ParentMediaID == detail.Media.ID && i.HistoryVersionNumber == 0 && i.LanguageID == detail.LanguageID && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderBy(i => i.Media.OrderIndex).ToList();
                }

                if(!detail.CssClasses.Contains("NoChildren") && childItems.Any())
                {
                    if ((currentDepth < renderDepth) || (renderDepth == -1))
                    {
                        //ChildList.LayoutTemplate = ItemsList.LayoutTemplate;
                        ChildList.ItemTemplate = ItemsList.ItemTemplate;
                        ChildList.ItemDataBound += new EventHandler<ListViewItemEventArgs>(ItemsList_OnItemDataBound);
                        ChildList.LayoutCreated += new EventHandler(ItemsList_OnLayoutCreated);

                        if ((childItems != null) && (IsFooterMenu))
                            childItems = childItems.Where(i => i.RenderInFooter);
                        else if (childItems != null)
                            childItems = childItems.Where(i => i.ShowInMenu);

                        if (!displayProtectedSections)
                        {
                            var list = childItems.OrderBy(i => i.Media.OrderIndex).ToList();

                            if (list.Count > 0)
                                li.Attributes["class"] += " has-children";

                            if ((currentDepth > 0) && (RenderParentItemInChildNav))
                            {
                                /*var parentDetails = new List<IMediaDetail>();
                                detail.CssClasses = "NoChildren";
                                parentDetails.Add(detail);*/

                                list = list.ToList();
                                detail.CssClasses = "NoChildren";
                                ((List<IMediaDetail>)list).Insert(0, detail);

                                //list = list.Concat(parentDetails);
                            }

                            ChildList.DataSource = list;
                            ChildList.DataBind();
                        }

                        currentDepth = currentDepth - 1;
                    }

                }
            }
        }

        public string RootVirtualPath
        {
            set
            {
                if (value == "~/")
                {
                    if (FrameworkSettings.CurrentFrameworkBaseMedia.CurrentWebsite != null)
                        value = FrameworkSettings.CurrentFrameworkBaseMedia.CurrentWebsite.VirtualPath;
                }

                rootMedia = MediasMapper.GetByMediaDetail(MediaDetailsMapper.GetByVirtualPath(value));
                //BindRootMedia(rootMedia);
                //UpdateItems();

                //Bind();
            }
        }

        public long RootMediaID
        {
            set
            {
                rootMedia = MediasMapper.GetByID(value);
            }
        }

        public bool IsBreadCrumbMenu { get; set; }

        public Media RootMedia
        {
            get
            {
                return rootMedia;
            }
            set
            {
                rootMedia = value;
                //BindRootMedia(value);
            }
        }

        public bool RenderRootMedia
        {
            get
            {
                return renderRootMedia;
            }
            set
            {
                renderRootMedia = value;
            }
        }

        public string RootULClasses
        {
            get
            {
                return rootULClasses;
            }
            set
            {
                rootULClasses = value;
            }
        }

        public string TopLevelAnchorClasses { get; set; }
        public string SubAnchorClasses { get; set; }
        public string SubLIClasses { get; set; }
        public string SubULClasses { get; set; }
        public bool RenderBackButton { get; set; }

        public bool DisplayProtectedSections
        {
            get
            {
                return displayProtectedSections;
            }
            set
            {
                displayProtectedSections = value;
            }
        }

        public bool IsFooterMenu
        {
            get
            {
                return isFooterManu;
            }
            set
            {
                isFooterManu = value;
            }
        }

        public bool RenderFooterMenuItems
        {
            get
            {
                return renderFooterMenuItems;
            }
            set
            {
                renderFooterMenuItems = value;
            }
        }

        public string DividerString { get; set; }        

        public bool RenderHiddenPages { get; set; }

        public bool RenderHiddenMediaTypes
        {
            get
            {
                return renderHiddenMediaTypes;
            }
            set
            {
                renderHiddenMediaTypes = value;
            }
        }

        public int RenderDepth
        {
            get
            {
                return renderDepth;
            }
            set
            {
                renderDepth = value;
            }
        }

        public bool RenderParentItemInChildNav { get; set; }
        public bool RenderParentNavIfNoChildren { get; set; }        

        private BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}