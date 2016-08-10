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
        private string rootUlClasses = "";
        private string currentVirtualPath = URIHelper.GetCurrentVirtualPath();
        private Language currentLanguage = FrameworkSettings.GetCurrentLanguage();

        public GenerateNav()
        {
            DividerString = "";
            RenderParentItemInChildNav = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (rootMedia != null)
                BindRootMedia(rootMedia);
        }

        private void BindRootMedia(Media rootMedia)
        {
            var mediaDetail = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.MediaID == rootMedia.ID && i.LanguageID == BasePage.CurrentLanguage.ID);

            if (mediaDetail.MediaType.Name == MediaTypeEnum.RootPage.ToString())
                rootMedia = WebsitesMapper.GetWebsite().Media;

            var items = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.Media.ParentMediaID == rootMedia.ID && i.HistoryVersionNumber == 0 && i.LanguageID == currentLanguage.ID && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderBy(i => i.Media.OrderIndex); //rootMedia.ChildMedias.SelectMany(m => m.MediaDetails.Where(i => i.HistoryVersionNumber == 0 && (i.ShowInMenu || i.RenderInFooter) && !i.IsDeleted && i.PostPublishDate <= DateTime.Now && (i.PostExpiryDate == null || i.PostExpiryDate > DateTime.Now))).OrderBy(i => i.Media.OrderIndex);
            Bind(items);
        }

        public void BindItems(IEnumerable<IMediaDetail> items)
        {
            Bind(items);
        }

        private void Bind(IEnumerable<IMediaDetail> items)
        {
            if (!IsBreadCrumbMenu)
            {
                if (IsFooterMenu)
                    items = items.Where(i => i.RenderInFooter);
                else
                    items = items.Where(i => i.ShowInMenu);

                if (!renderHiddenMediaTypes)
                    items = items.Where(i => i.MediaType.ShowInMenu);

                items = items.OrderBy(i => i.Media.OrderIndex);

                if (renderRootMedia)
                {
                    var newlist = new List<IMediaDetail>();

                    if (BasePage.CurrentWebsite != null)
                        newlist.Add(BasePage.CurrentWebsite);

                    items = newlist.Concat(items);
                }
            }

            this.ItemsList.DataSource = items;
            this.ItemsList.DataBind();
        }

        protected void ItemsList_OnLayoutCreated(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            HtmlContainerControl ul = (HtmlContainerControl)list.FindControl("ul");

            if (ul.Attributes["class"] == null)
                ul.Attributes.Add("class", "");

            ul.Attributes["class"] += " level" + currentDepth.ToString();

            if (currentDepth == 0)
                ul.Attributes["class"] += " " + RootUlClasses;

            ul.Attributes["class"] = ul.Attributes["class"].Trim();

            currentDepth = currentDepth + 1;
        }

        protected void ItemsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HyperLink Link = (HyperLink)e.Item.FindControl("Link");
                var divider = (Literal)e.Item.FindControl("Divider");
                divider.Text = DividerString;
                ListView ChildList = (ListView)e.Item.FindControl("ChildList");

                IMediaDetail details = (IMediaDetail)e.Item.DataItem;

                if (!displayProtectedSections && details.IsProtected)
                {
                    if (BasePage.CurrentUser == null)
                        e.Item.Visible = false;
                }

                Link.Text = details.LinkTitle;

                string virtualPath = details.AutoCalculatedVirtualPath;

                if (details.UseDirectLink)
                {
                    if (details.DirectLink.Contains("#footer"))
                    {
                        virtualPath = HttpContext.Current.Request.Url + "#footer";
                    }
                    else
                    {
                        virtualPath = details.DirectLink;
                    }
                }

                if (details.OpenInNewWindow)
                    Link.Target = "_blank";

                if (!virtualPath.EndsWith("/"))
                {
                    if (!virtualPath.Contains("."))
                        virtualPath += "/";
                }

                var path = URIHelper.ConvertToAbsUrl(virtualPath);

                if (path.Contains("#") && path.EndsWith("/"))
                    path = path.Remove(path.Length - 1);

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

                if (virtualPath == currentVirtualPath || details.VirtualPath == currentVirtualPath)
                {
                    li.Attributes["class"] += " current";
                }
                else if (currentVirtualPath.Contains(virtualPath) || currentVirtualPath.Contains(details.VirtualPath))
                {
                    li.Attributes["class"] += " currentParent";

                    if ((rootMedia != null) && (details.MediaID == rootMedia.ID))
                        li.Attributes["class"] += " rootParent";
                }

                li.Attributes["class"] += " " + details.VirtualPath.Replace("/", "-").Replace("~", "home");

                if (li.Attributes["class"].EndsWith("-"))
                    li.Attributes["class"] = li.Attributes["class"].Substring(0, li.Attributes["class"].Length - 1);

                if (!string.IsNullOrEmpty(details.CssClasses))
                    li.Attributes["class"] += " " + details.CssClasses;

                li.Attributes["class"] = li.Attributes["class"].Trim();

                IEnumerable<IMediaDetail> childItems = new List<IMediaDetail>();

                if ((rootMedia != null) && (details.MediaID != rootMedia.ID))
                {
                    childItems = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.Media.ParentMediaID == details.Media.ID && i.HistoryVersionNumber == 0 && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now)).OrderBy(i => i.Media.OrderIndex);
                    //childItems = details.Media.ChildMedias.SelectMany(i => i.MediaDetails.Where(j => j.HistoryVersionNumber == 0 && !j.IsDeleted && j.PostPublishDate <= DateTime.Now && j.LanguageID == details.LanguageID));
                    //childItems = MediaDetailsMapper.FilterOutDeletedAndArchived(MediaDetailsMapper.GetAllChildMediaDetails(details.Media, details.Language));

                    if (!renderHiddenMediaTypes)
                        childItems = childItems.Where(i => i.ShowInMenu || i.RenderInFooter);
                }

                if ((currentDepth < renderDepth) || (renderDepth == -1))
                {
                    ChildList.LayoutTemplate = ItemsList.LayoutTemplate;
                    ChildList.ItemTemplate = ItemsList.ItemTemplate;
                    ChildList.ItemDataBound += new EventHandler<ListViewItemEventArgs>(ItemsList_OnItemDataBound);
                    ChildList.LayoutCreated += new EventHandler(ItemsList_OnLayoutCreated);

                    if ((childItems != null) && (IsFooterMenu))
                        childItems = childItems.Where(i => i.RenderInFooter);
                    else if (childItems != null)
                        childItems = childItems.Where(i => i.ShowInMenu);

                    if (!displayProtectedSections)
                    {
                        var list = childItems.OrderBy(i => i.Media.OrderIndex).AsEnumerable();

                        if (RenderParentItemInChildNav)
                        {
                            var parentDetails = new List<IMediaDetail>();
                            parentDetails.Add(details);

                            list = list.Concat(parentDetails);
                        }

                        ChildList.DataSource = list;
                        ChildList.DataBind();
                    }

                    currentDepth = currentDepth - 1;
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

        public string RootUlClasses
        {
            get
            {
                return rootUlClasses;
            }
            set
            {
                rootUlClasses = value;
            }
        }

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

        private BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}