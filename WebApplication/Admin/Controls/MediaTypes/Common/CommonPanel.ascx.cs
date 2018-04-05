using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes
{
    public partial class CommonPanel : BaseMediaDetailPanel, IMediaDetailPanel
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        public override void SetObject(IMediaDetail obj)
        {
            //base.SetObject(obj);
            SEOSettingsTab.SetObject(obj);
            PublishSettingsTab.SetObject(obj);

            BindMediaFields();

            LoadTabs();

            UpdateFieldsFromObject();
        }

        public void SetPanelFields(Panel panel)
        {
            PanelFieldsPlaceHolder.Controls.Add(panel);
        }

        private void BindMediaFields()
        {
            var groups = SelectedItem.Fields.GroupBy(i => i.GroupName).OrderBy(i => i.Key);

            if (groups.Any())
            {
                FieldGroupTabContents.DataSource = groups;
                FieldGroupTabContents.DataBind();
            }
        }

        public void LoadTabs()
        {
            AddFieldsTab();
            AddLayoutsTab();

            //AddSEOSettingsTab();
            AddLinkSettingsTab();

            //AddTagsTab();
            //AddPublishSettingsTab();
            AddAdvancedSettingsTab();

			AddInjectHtmlTab();

            //AddShoppingCartSettingsTab();

            if (SelectedItem.Media.Comments.Any())
                AddCommentsTab();

            if ((this.SelectedItem != null) && ((this.SelectedItem.ID != 0)))
            {
                AddRolesUsersTab();
                AddHistoryTab();
            }

            ChildrensTab();

            Tabs.DataSource = base.Tabs.Select(i => i.Name);
            Tabs.DataBind();

            TabPanels.DataSource = base.Tabs;
            TabPanels.DataBind();

            //UpdateFieldsFromObject();
        }

        public void AddLinkSettingsTab()
        {
            //if (BasePage.CurrentUser.IsInRole(RoleEnum.Administrator))
            //{
                AddTab("Link", "~/Admin/Controls/MediaTypes/Common/Tabs/LinkSettingsTab.ascx");
            //}
        }

        public void AddInjectHtmlTab()
        {
            if (BasePage.CurrentUser.IsInRole(RoleEnum.Developer))
            {
                AddTab("Inject Html", "~/Admin/Controls/MediaTypes/Common/Tabs/InjectHtmlTab.ascx");
            }
        }

        public void ChildrensTab()
        {
            AddTab("Children", "~/Admin/Controls/MediaTypes/Common/Tabs/ChildrensTab.ascx");
        }

        public void AddSEOSettingsTab()
        {
            AddTab("SEO Settings", "~/Admin/Controls/MediaTypes/Common/Tabs/SEOSettingsTab.ascx");
        }

        public void AddShoppingCartSettingsTab()
        {
            AddTab("Shopping Cart Settings", "~/Admin/Controls/MediaTypes/Common/Tabs/ShoppingCartSettingsTab.ascx");
        }

        public void AddTagsTab()
        {
            AddTab("Tags", "~/Admin/Controls/MediaTypes/Common/Tabs/TagsTab.ascx");
        }

        public void AddCommentsTab()
        {
            var pendingComments = SelectedItem.Media.Comments.Where(i => i.Status == StatusEnum.Pending.ToString()).Count();
            AddTab("Comments(" + pendingComments + ")", "~/Admin/Controls/MediaTypes/Common/Tabs/CommentsTab.ascx");
        }

        public void AddAdvancedSettingsTab()
        {
            if (BasePage.CurrentUser.IsInRole(RoleEnum.Developer))
            {
                AddTab("Settings", "~/Admin/Controls/MediaTypes/Common/Tabs/AdvancedSettingsTab.ascx");
            }
        }

        public void AddHistoryTab()
        {
            AddTab("History", "~/Admin/Controls/MediaTypes/Common/Tabs/HistoryTab.ascx");
        }

        public void AddPublishSettingsTab()
        {
            AddTab("Publish Settings", "~/Admin/Controls/MediaTypes/Common/Tabs/PublishSettingsTab.ascx");
        }

        public void AddRolesUsersTab()
        {
            if (BasePage.CurrentUser.IsInRole(RoleEnum.Developer))
            {
                AddTab("Roles", "~/Admin/Controls/MediaTypes/Common/Tabs/RolesUsersTab.ascx");
            }
        }

        public void AddFieldsTab()
        {
            if (BasePage.CurrentUser.IsInRole(RoleEnum.Developer))
            {
                AddTab("Fields", "~/Admin/Controls/MediaTypes/Common/Tabs/FieldsTab.ascx");
            }
        }

        public void AddLayoutsTab()
        {
            if (BasePage.CurrentUser.IsInRole(RoleEnum.Developer))
            {
                AddTab("Layouts", "~/Admin/Controls/MediaTypes/Common/Tabs/LayoutsTab.ascx");
            }
        }

        public void AddTab(string tabText, string tabControlPath)
        {
            var tab = new Tab(tabText);
            dynamic control = LoadControl(tabControlPath);
            control.SetObject(SelectedItem);

            tab.TabControl = control;

            control.ID = Page.UniqueID + "_UserControl" + "_" + StringHelper.CreateSlug(tabText);

            base.Tabs.Add(tab);
        }

        private void UpdateTags()
        {
            if (IsPostBack)
                return;

            if (SelectedItem.Media != null)
                TagsSelector.SetMedia(SelectedItem.Media);
        }

        public void UpdateObjectFromFields()
        {
            IMediaDetail item = (IMediaDetail)SelectedItem;
            item.LinkTitle = LinkTitle.Text;

            var keyValuePair = new Dictionary<string, string>();
            keyValuePair.Add("TemplateBaseUrl", BasePage.TemplateVars["TemplateBaseUrl"]);
            keyValuePair.Add("BaseUrl", BasePage.TemplateVars["BaseUrl"]);
            
            //TagsSelector.SetDefaultTag(item.LinkTitle);

            IEnumerable<Tag> tags = TagsSelector.GetTags();

            item.Media.MediaTags.Clear();

            var counter = 0;
            foreach (Tag tag in tags)
            {
                if (!item.Media.MediaTags.Select(i => i.Tag).Contains(tag))
                {
                    var contextTag = tag;

                    if (tag.ID == 0)
                    {
                        contextTag.DateCreated = contextTag.DateLastModified = DateTime.Now;
                        contextTag.ThumbnailPath = "";
                    }
                    else
                    {
                        //contextTag = BaseMapper.GetObjectFromContext(tag);
                        contextTag = tag;
                    }

                    var newMediaTag = new MediaTag();
                    newMediaTag.MediaID = item.MediaID;

                    if (contextTag.ID != 0)
                        newMediaTag.TagID = contextTag.ID;
                    else
                        newMediaTag.Tag = contextTag;

                    newMediaTag.OrderIndex = counter;

                    item.Media.MediaTags.Add(newMediaTag);

                    counter++;
                }
            }

            if (item.Media != null)
                TagsSelector.SetMedia(item.Media);

            UpdateObjectFromTabsFields();

            UpdateObjectFromMediaFields();

            SEOSettingsTab.UpdateObjectFromFields();
            PublishSettingsTab.UpdateObjectFromFields();
        }



        private void UpdateObjectFromMediaFields()
        {
            foreach (var FieldGroupTabContent in FieldGroupTabContents.Items)
            {
                var MediaFieldsList = (ListView)FieldGroupTabContent.FindControl("MediaFieldsList");

                var index = 0;
                foreach (var item in MediaFieldsList.Items)
                {
                    var fieldIdField = (HiddenField)item.FindControl("FieldID");
                    var dynamicField = (PlaceHolder)item.FindControl("DynamicField");
                    var fieldId = long.Parse(fieldIdField.Value.ToString());

                    //var dataItem = MediasMapper.GetDataModel().Fields.SingleOrDefault(i => i.ID == fieldId);

                    var dataItem = (MediaDetailField)((IEnumerable<object>)MediaFieldsList.DataSource).ElementAt(index);
                    index++;

                    if (dynamicField.Controls.Count == 0)
                        return;

                    var control = dynamicField.Controls[0];

                    if (control.Controls.Count > 0)
                    {
                        foreach (Control ctrl in control.Controls)
                        {
                            if(!(ctrl is LiteralControl))
                            {
                                control = ctrl;
                            }
                        }                        
                    }

                    var adminPanel = WebFormHelper.FindControlRecursive(dynamicField, "AdminPanel");

                    if (adminPanel != null)
                        control = adminPanel.Parent;

                    if (control is WebApplication.Admin.Controls.Fields.IFieldControl)
                    {
                        var valAsString = ((WebApplication.Admin.Controls.Fields.IFieldControl)control).GetValue().ToString();

                        if (!string.IsNullOrEmpty(valAsString))
                        {
                            valAsString = MediaDetailsMapper.ConvertUrlsToShortCodes(valAsString);

                            if(valAsString.Contains(URIHelper.BaseUrl))
                                valAsString = valAsString.Replace(URIHelper.BaseUrl, "{BaseUrl}");
                        }

                        dataItem.FieldValue = MediaDetailsMapper.ConvertUrlsToShortCodes(valAsString);
                    }
                    else
                    {
                        var fieldValue = "";

                        if (dataItem.GetAdminControlValue.Contains("@"))
                        {
                            fieldValue = ParserHelper.ParseData(dataItem.GetAdminControlValue, new { Control = control });
                        }
                        else
                        {
                            fieldValue = ParserHelper.ParseData("{" + dataItem.GetAdminControlValue + "}", control);
                        }

                        if (fieldValue != "{" + dataItem.GetAdminControlValue + "}")
                        {
                            fieldValue = MediaDetailsMapper.ConvertUrlsToShortCodes(fieldValue);
                            dataItem.FieldValue = fieldValue.Replace(URIHelper.BaseUrl, "{BaseUrl}");
                        }
                        else
                        {
                            dataItem.FieldValue = "";
                        }
                    }
                }
            }

            /*var returnObj = MediaDetailsMapper.Update(SelectedItem);

            if (returnObj.IsError)
                BasePage.DisplayErrorMessage("Error", returnObj.Error);*/
        }

        private void UpdateMediaFieldsFromObject()
        {
            foreach (var FieldGroupTabContent in FieldGroupTabContents.Items)
            {
                var MediaFieldsList = (ListView)FieldGroupTabContent.FindControl("MediaFieldsList");

                var index = 0;
                foreach (var item in MediaFieldsList.Items)
                {
                    var mediaDetailField = (MediaDetailField)((IEnumerable<object>)MediaFieldsList.DataSource).ElementAt(index);
                    index++;

                    var fieldIdField = (HiddenField)item.FindControl("FieldID");
                    var dynamicField = (PlaceHolder)item.FindControl("DynamicField");
                    var fieldId = long.Parse(fieldIdField.Value.ToString());

                    if (dynamicField.Controls.Count == 0)
                        return;

                    var control = dynamicField.Controls[0];

                    if (control.Controls.Count > 0)
                    {
                        foreach (Control ctrl in control.Controls)
                        {
                            if (!(ctrl is LiteralControl))
                            {
                                control = ctrl;
                            }
                        }
                    }

                    var adminPanel = WebFormHelper.FindControlRecursive(dynamicField, "AdminPanel");

                    if (adminPanel != null)
                        control = adminPanel.Parent;

                    var fieldValue = mediaDetailField.FieldValue.Replace("{BaseUrl}", URIHelper.BaseUrl);

                    if (control is WebApplication.Admin.Controls.Fields.IFieldControl)
                    {
                        var ctrl = ((WebApplication.Admin.Controls.Fields.IFieldControl)control);
                        ctrl.FieldID = fieldId;
                        ctrl.SetValue(fieldValue);
                    }
                    else
                    {
                        if (mediaDetailField.FieldValue != "{" + mediaDetailField.SetAdminControlValue + "}")
                        {
                            if (mediaDetailField.SetAdminControlValue.Contains("@"))
                            {
                                try
                                {
                                    var returnData = ParserHelper.ParseData(mediaDetailField.SetAdminControlValue, new { Control = control, Field = mediaDetailField, NewValue = fieldValue });
                                }
                                catch (Exception ex)
                                {
                                    BasePage.DisplayErrorMessage("Error", ErrorHelper.CreateError(ex));
                                }
                            }
                            else
                            {
                                ParserHelper.SetValue(control, mediaDetailField.SetAdminControlValue, fieldValue);
                            }
                        }
                    }
                }

            }
        }

        public void UpdateFieldsFromObject()
        {
            IMediaDetail item = (IMediaDetail)SelectedItem;

            if ((item.ID == 0) && (item.LinkTitle == null || item.LinkTitle == ""))
            {
                var mediaType = MediaTypesMapper.GetByID(item.MediaTypeID);
                var createdItems = mediaType.MediaDetails.Where(i => !i.IsHistory && i.Media.ParentMediaID == item.Media.ParentMediaID && i.LanguageID == AdminBasePage.CurrentLanguage.ID).Select(i => i);

                var newIndex = createdItems.Count() + 1;

                item.LinkTitle = AdminBasePage.CurrentLanguage.DisplayName + " - " + mediaType.Name + " " + newIndex;
            }

            LinkTitle.Text = item.LinkTitle;

            var virtualPath = item.AutoCalculatedVirtualPath;

            /*if (LanguagesMapper.GetAllActive().Count() > 1)
                virtualPath = URIHelper.ConvertAbsUrlToTilda(URIHelper.ConvertToAbsUrl(virtualPath).Replace(URIHelper.BaseUrl, URIHelper.BaseUrlWithLanguage));*/

            VirtualPath.Text = virtualPath;
            VirtualPath.NavigateUrl = URIHelper.ConvertToAbsUrl(virtualPath) + "?version=" + item.HistoryVersionNumber;

            UpdateTags();

            UpdateTabsFieldsFromObject();

            UpdateMediaFieldsFromObject();

            SEOSettingsTab.UpdateFieldsFromObject();
            PublishSettingsTab.UpdateFieldsFromObject();
        }

        protected void MediaFieldsList_ItemDataBound(object sender, System.Web.UI.WebControls.ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var mediaDetailField = (MediaDetailField)e.Item.DataItem;
            var FieldID = (HiddenField)e.Item.FindControl("FieldID");
            var FieldLabel = (Literal)e.Item.FindControl("FieldLabel");
            var DynamicField = (PlaceHolder)e.Item.FindControl("DynamicField");
            var FieldWrapper = (Panel)e.Item.FindControl("FieldWrapper");

            FieldID.Value = mediaDetailField.ID.ToString();
            FieldWrapper.CssClass += " " + mediaDetailField.FieldCode;
            //FieldLabel.Text = mediaDetailField.FieldLabel;

            //var propertyName = mediaDetailField.GetValueCode;

            Control dynamicField = null;
            Control control = null;
            string code = "";
            try
            {
                dynamicField = this.ParseControl(MediaDetailsMapper.ParseSpecialTags(SelectedItem, mediaDetailField.AdminControl, 0, new RazorFieldParams() { MediaDetail = SelectedItem, Field = mediaDetailField }));
            }
            catch (Exception ex)
            {
                BasePage.DisplayErrorMessage("Error Creating Field with ID: " + FieldID.Value, ErrorHelper.CreateError(ex));
                //ErrorHelper.LogException(ex);
            }
            finally
            {
                if (dynamicField != null)
                    DynamicField.Controls.Add(dynamicField);
            }
        }

        private AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        protected void TabPanels_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var tab = (Tab)e.Item.DataItem;
            var TabControlHolder = (PlaceHolder)e.Item.FindControl("TabControlHolder");

            TabControlHolder.Controls.Add((Control)tab.TabControl);
        }

        protected void FieldGroupTabContents_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var MediaFieldsList = (ListView)e.Item.FindControl("MediaFieldsList");
            var controlsList = ((IEnumerable<object>)e.Item.DataItem).ToList();

            if (MediaFieldsList != null)
            {
                MediaFieldsList.DataSource = controlsList.OrderBy(i => ((MediaDetailField)i).OrderIndex);
                MediaFieldsList.DataBind();
            }
        }
    }
}