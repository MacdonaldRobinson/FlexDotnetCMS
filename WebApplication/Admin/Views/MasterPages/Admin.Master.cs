using FrameworkLibrary;
using FrameworkLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin
{
    public partial class Admin : BaseMasterPage
    {
        private Website currentWebsite = WebsitesMapper.GetWebsite();
        private long numberOfActiveLanguages = LanguagesMapper.GetAllActive().Count();

        /*protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }*/

        protected void Page_Init(object sender, EventArgs e)
        {
            InitPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSiteTreeView();
        }

        private void InitPage()
        {
            var settings = SettingsMapper.GetSettings();
            var rootMediaDetail = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.MediaType.Name == MediaTypeEnum.RootPage.ToString() && i.LanguageID == AdminBasePage.CurrentLanguage.ID);

            if (rootMediaDetail == null)
            {
                CreateItem.Visible = true;
            }

            if (settings.EnableGlossaryTerms)
            {
                GlossaryTermsNavItem.Visible = true;
            }

            var allNodes = SiteTree.GetAllNodes();
            var nodes = allNodes.Where(i => (i.Value == currentWebsite.ID.ToString() || i.Value == currentWebsite.Media.ParentMediaID.ToString()));

            foreach (var node in nodes)
            {
                node.ExpandParents();
                node.Expand();
            }

            if (AdminBasePage.SelectedMediaDetail != null)
            {
                var selectedMediaDetailId = AdminBasePage.SelectedMediaDetail.ID.ToString();
                var node = allNodes.Find(i => i.Value == selectedMediaDetailId);
                if (node != null)
                {
                    node.ExpandParents();
                    node.Select();
                }
            }

            if (numberOfActiveLanguages < 2)
                LanguageSwitcher.Visible = false;
            else
                LanguageSwitcher.Visible = true;
        }

        private void BindTree(IMediaDetail item, CustomTreeNode parentNode, bool renderChildren = true)
        {
            CustomTreeNode rootNode = new CustomTreeNode(item.SectionTitle, item.ID.ToString(), item.MediaID);
            UpdateTreeNode(rootNode, item);

            if (parentNode == null)
            {
                SiteTree.Nodes.Add(rootNode);
            }
            else
            {
                parentNode.ChildNodes.Add(rootNode);
            }

            if(renderChildren)
            {
                foreach (var mediaDetail in item.ChildMediaDetails)
                {
                    if (mediaDetail != null && mediaDetail.ID != 0 && mediaDetail.MediaType.ShowInSiteTree && mediaDetail.MediaType.ShowInSiteTree)
                    {
                        BindTree(mediaDetail, rootNode);
                    }
                }
            }
        }

        private IEnumerable<Media> GetAllMedias()
        {
            return MediasMapper.GetDataModel().AllMedia.Where(i => i.MediaDetails.Any(j => j.MediaType.ShowInSiteTree && j.HistoryVersionNumber == 0));
        }


        private bool SearchWithinMediaDetail(IMediaDetail mediaDetail, string filterText)
        {
            if (mediaDetail == null)
                return false;

            if (mediaDetail.HistoryVersionNumber == 0 && mediaDetail.MediaID.ToString() == filterText || mediaDetail.LinkTitle.ToLower().Trim().Contains(filterText) || mediaDetail.SectionTitle.ToLower().Trim().Contains(filterText) || mediaDetail.MainContent.ToLower().Trim().Contains(filterText) || mediaDetail.MainLayout.ToLower().Trim().Contains(filterText) || mediaDetail.Fields.Any(j => j.FieldValue.Contains(filterText)))
                return true;

            foreach (var fieldAssociation in mediaDetail.Fields.SelectMany(i=>i.FieldAssociations))
            {
                if (fieldAssociation.MediaDetail.ID == mediaDetail.ID)
                    continue;

                if(SearchWithinMediaDetail(fieldAssociation.MediaDetail, filterText))
                {
                    return true;
                }
            }

            return false;
        }

        public void BindSiteTreeView()
        {
            if (Filter.Text == "")
            {
                var root = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.Media.ParentMedia == null && i.HistoryVersionNumber == 0);
                BindTree(root, null);
            }
            else
            {
                var filterText = Filter.Text.ToLower().Trim();
                var foundItems = GetAllMedias().Select(i => i.LiveMediaDetail).Where(i => SearchWithinMediaDetail(i, filterText));

                SiteTree.Nodes.Clear();

                foreach (var item in foundItems)
                {
                    BindTree(item, null, false);
                    //SiteTree.Nodes.Add(new CustomTreeNode(item.LinkTitle, item.ID.ToString(), item.MediaID, URIHelper.ConvertToAbsUrl(WebApplication.BasePage.GetRedirectToMediaDetailUrl(item.MediaTypeID, item.Media.ID))));
                }
            }
        }

        protected void LoginStatus_OnLoggedOut(object sender, EventArgs e)
        {
            this.BasePage.CheckInAll();
        }

        private void UpdateTreeNode(CustomTreeNode node, IMediaDetail detail)
        {
            if (detail == null)
                return;

            node.Value = detail.ID.ToString();
            //node.Attributes.Add("FrontEndUrl", detail.AbsoluteUrl);

            var nodeText = "";

            if (detail.LanguageID != AdminBasePage.CurrentLanguage.ID)
            {
                nodeText = detail.LinkTitle + " - " + LanguagesMapper.GetByID(detail.Language.ID).Name;
                node.LIClasses.Add("doesNotExistInLanguage");
            }
            else
            {
                nodeText = $"{detail.LinkTitle} <small>({detail.MediaID})</small>";
            }

            if (detail.IsDeleted)
            {
                node.LIClasses.Add("isDeleted");
            }

            if ((!detail.ShowInMenu) && (!detail.RenderInFooter))
                node.LIClasses.Add("isHidden");

            if ((!detail.CanRender) || (!detail.IsPublished))
                node.LIClasses.Add("unPublished");

            if ((AdminBasePage.SelectedMediaDetail != null) && (AdminBasePage.SelectedMediaDetail.ID.ToString() == node.Value))
            {
                node.LIClasses.Add("selected");
                node.LinkClasses.Add("jstree-clicked");

                while (node.Parent != null)
                {
                    ((CustomTreeNode)node.Parent).LinkClasses.Add("jstree-open");
                }
            }

            if (detail.MediaType.Name == MediaTypeEnum.RootPage.ToString() || detail.MediaType.Name == MediaTypeEnum.Website.ToString())
            {
                node.LIClasses.Add("jstree-open");
            }

            node.Text = nodeText;

            node.LinkAttributes.Add("data-frontendurl", detail.Media.PermaLink);
            //node.LinkAttributes.Add("data-frontendurl", detail.AbsoluteUrl);
            node.LIAttributes.Add("data-mediaDetailId", detail.ID.ToString());

            node.NavigateUrl = URIHelper.ConvertToAbsUrl(WebApplication.BasePage.GetRedirectToMediaDetailUrl(detail.MediaTypeID, detail.MediaID));
        }
    }
}