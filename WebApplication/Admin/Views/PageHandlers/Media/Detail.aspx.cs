using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.MediaArticle
{
    public partial class Detail : AdminBasePage
    {
        private Media selectedMediaItem;
        private IMediaDetail tmpSelectedItem;
        private IMediaDetail selectedItem;
        private IMediaDetail historyVersionItem;
        private Media parentMediaItem;
        private long mediaTypeId;
        private long historyVersion = 0;

        public Return CanAccessItem { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            long id;
            long parentId;

            long.TryParse(Request["historyVersion"], out historyVersion);
            long.TryParse(Request["mediaTypeId"], out mediaTypeId);

            if (long.TryParse(Request["selectedMediaId"], out id))
            {
                selectedMediaItem = MediasMapper.GetByID(id);
                SelectedMedia = selectedMediaItem;

                if (selectedMediaItem != null)
                {
                    selectedItem = MediaDetailsMapper.GetByMedia(selectedMediaItem, CurrentLanguage);

                    if (historyVersion > 0)
                    {
                        selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
                        historyVersionItem = BaseMapper.GetObjectFromContext((MediaDetail)MediaDetailsMapper.GetByMedia(selectedMediaItem, CurrentLanguage, historyVersion));
                    }

                    if ((selectedItem != null) && (historyVersionItem != null))
                    {
                        tmpSelectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
                        selectedItem = historyVersionItem;
                    }
                }
            }

            if (long.TryParse(Request["parentMediaId"], out parentId))
                parentMediaItem = MediasMapper.GetByID(parentId);

            if (selectedItem == null)
            {
                if (parentMediaItem != null)
                    SelectedMedia = parentMediaItem;

                if (parentMediaItem == null)
                    return;

                selectedItem = MediaDetailsMapper.CreateObject(mediaTypeId, selectedMediaItem, parentMediaItem);
            }

            Return canAccessReturnObj = BaseMapper.GenerateReturn();

            if (selectedItem.ID != 0)
            {
                canAccessReturnObj = MediaDetailsMapper.CanAccessMediaDetail(selectedItem, CurrentUser);
                if (canAccessReturnObj.IsError)
                {
                    DisplayErrorMessage("Cannot edit item", canAccessReturnObj.Error);

                    CanAccessItem = canAccessReturnObj;

                    return;
                }
                else
                {
                    KeyValuePair<IMediaDetail, User> checkedOutItem = IsCheckedOut(selectedItem);

                    if (checkedOutItem.Key == null)
                        CheckOut(selectedItem);
                    else
                    {
                        if (checkedOutItem.Value.ID != CurrentUser.ID)
                        {
                            Return returnObj = BaseMapper.GenerateReturn("Cannot edit item", "The item has been checked out by user: ( " + checkedOutItem.Value.UserName + " )");

                            CanAccessItem = returnObj;

                            DisplayErrorMessage("Error", returnObj.Error);

                            return;
                        }
                    }
                }
            }

            SelectedMediaDetail = selectedItem;
            SelectedMedia = selectedMediaItem;

            if (selectedMediaItem == null)
                SelectedMedia = parentMediaItem;

            if (selectedItem.ID == 0)
            {
                Save.Text = "Create";
                SaveAndPublish.Text = "Create And Publish";

                LoadLatestDraft.Visible = false;
                SaveAsDraft.Visible = false;

                var mediaType = MediaTypesMapper.GetByID(selectedItem.MediaTypeID);

                selectedItem.MainLayout = mediaType.MainLayout;
                selectedItem.SummaryLayout = mediaType.SummaryLayout;
                selectedItem.FeaturedLayout = mediaType.FeaturedLayout;

                selectedItem.UseMediaTypeLayouts = mediaType.UseMediaTypeLayouts;

                foreach (var field in mediaType.Fields)
                {
                    var newField = new MediaDetailField();
                    newField.FieldCode = field.FieldCode;
                    newField.FieldLabel = field.FieldLabel;
                    newField.AdminControl = field.AdminControl;
                    newField.GroupName = field.GroupName;
                    newField.RenderLabelAfterControl = field.RenderLabelAfterControl;
                    newField.GetAdminControlValue = field.GetAdminControlValue;
                    newField.SetAdminControlValue = field.SetAdminControlValue;
                    newField.FieldValue = field.FieldValue;
                    newField.FrontEndLayout = field.FrontEndLayout;
                    newField.MediaTypeField = field;
                    newField.UseMediaTypeFieldFrontEndLayout = true;

                    newField.DateCreated = DateTime.Now;
                    newField.DateLastModified = DateTime.Now;

                    selectedItem.Fields.Add(newField);
                }
            }
            else
            {
                Save.Text = "Save";
                SaveAndPublish.Text = "Save And Publish";
            }

            if ((historyVersion > 0) && (historyVersionItem != null) && (!historyVersionItem.IsDraft))
            {
                //SavePanel.Visible = false;
            }
            else
            {
                var draftItems = selectedItem.History.Where(i => i.IsDraft);

                if (draftItems.Count() > 0)
                {
                    LoadLatestDraft.Visible = true;
                    SaveAsDraft.Visible = false;
                }

                SaveAndPublish.Visible = true;
                SavePanel.Visible = true;

                HistoryPanel.Visible = false;
            }

            if (historyVersion > 0)
            {
                SavePanel.Visible = true;

                if (!selectedItem.IsDraft)
                    Save.Visible = false;

                SaveAsDraft.Visible = false;
                PublishNow.Visible = false;
                PublishLive.Visible = true;

                HistoryPanel.Visible = true;
                HistoryVersionNumber.Text = historyVersion.ToString();
            }

            if (CurrentUser.IsInRole(RoleEnum.Administrator))
            {
                EditMediaType.NavigateUrl = "~/Admin/Views/PageHandlers/MediaTypes/Detail.aspx?id=" + SelectedMediaDetail.MediaTypeID;
                EditMediaType.Visible = true;
            }

            Panel.SetObject(SelectedMediaDetail);

            if (Request.QueryString["masterFilePath"] != null)
            {
                PreviewPanel.Visible = false;
                RemovePreviewPanelScript.Visible = true;
            }
            else
            {
                PreviewPanel.Visible = true;
                RemovePreviewPanelScript.Visible = false;
            }

            UpdateSectionTitles();

        }

        public IMediaDetailPanel Panel
        {
            get
            {
                if (PanelsPlaceHolder.Controls.Count == 0)
                {
                    var pathToPanel = GetPathToPanel(MediaTypesMapper.GetByID(SelectedMediaDetail.MediaTypeID).Name);

                    var panelControl = LoaderHelper.LoadControl(pathToPanel);

                    if (panelControl == null)
                    {
                        pathToPanel = GetPathToPanel(MediaTypeEnum.Page.ToString());
                        panelControl = LoaderHelper.LoadControl(pathToPanel);
                    }

                    if (panelControl is IMediaDetailPanel)
                        PanelsPlaceHolder.Controls.Add(panelControl);
                }

                if (PanelsPlaceHolder.Controls.Count == 1)
                    return (IMediaDetailPanel)PanelsPlaceHolder.Controls[0];

                return null;
            }
        }

        public string GetPathToPanel(string mediaTypeName)
        {
            var pathToPanel = "~/Admin/Controls/MediaTypes/" + mediaTypeName + "/" + mediaTypeName + "Panel.ascx";

            return pathToPanel;
        }

        public string SelectTabIndex
        {
            get
            {
                var selectTabIndex = Request["selectTabIndex"];

                if (string.IsNullOrEmpty(selectTabIndex))
                    return "0";

                return selectTabIndex;
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            if (selectedItem == null)
                return;

            if (selectedItem.ID == 0)
                SaveAndPublish.Visible = true;
            else
                SaveAndPublish.Visible = false;

            if ((selectedItem.ID != 0) && (!selectedItem.IsPublished) && (historyVersion == 0))
            {
                PublishNow.Visible = true;
            }
            else
            {
                PublishNow.Visible = false;
            }
        }

        protected void ViewCurrentVersion_OnClick(object sender, EventArgs e)
        {
            WebApplication.BasePage.RedirectToMediaDetail(selectedItem);
        }

        /*protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack)
                UpdateFieldsFromObject();
        }*/

        private void UpdateSectionTitles()
        {
            string title = "";
            string sectionTitle = "";

            if (selectedItem.ID == 0)
            {
                string under = "Root";
                IMediaDetail atleastOne = null;

                if (parentMediaItem != null)
                    atleastOne = MediaDetailsMapper.GetAtleastOneByMedia(parentMediaItem, CurrentLanguage);

                if ((parentMediaItem != null) && (atleastOne != null))
                    under = atleastOne.LinkTitle;

                sectionTitle = title = "Creating New Media of Type '" + MediaTypesMapper.GetByID(mediaTypeId).Name + "' Under '" + under + "' with Language '" + CurrentLanguage.Name + "'";
            }
            else
            {
                title = "Editing: " + selectedItem.Title;
                sectionTitle = "Editing: <span>" + selectedItem.Title + "</span>";
            }

            this.Page.Title = title;
            this.SectionTitle.Text = sectionTitle;
        }

        private void UpdateObjectFromFields(IMediaDetail item)
        {
            item.MediaTypeID = mediaTypeId;

            item.LanguageID = CurrentLanguage.ID;

            item.LastUpdatedByUserID = CurrentUser.ID;

            Panel.UpdateObjectFromFields();

            /*if (item.MediaID != 0)
                item.Media = BaseMapper.GetObjectFromContext(item.Media);*/
        }

        private void UpdateFieldsFromObject()
        {
            if (selectedItem == null)
                return;

            UpdateSectionTitles();
            Panel.UpdateFieldsFromObject();
        }

        protected void PublishNow_OnClick(object sender, EventArgs e)
        {
            if (!CurrentUser.HasPermission(PermissionsEnum.PublishItems))
            {
                DisplayErrorMessage("Error publishing item", ErrorHelper.CreateError(new Exception("You do not have the appropriate permissions to publish items")));
                return;
            }

            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);

            if (selectedItem == null)
                return;

            var mediaType = MediaTypesMapper.GetByID(selectedItem.MediaTypeID);

            selectedItem.PublishDate = DateTime.Now;

            if (selectedItem.ExpiryDate != null)
                selectedItem.ExpiryDate = null;

            selectedItem.ShowInMenu = mediaType.ShowInMenu;
            selectedItem.ShowInSearchResults = mediaType.ShowInSearchResults;
            selectedItem.EnableCaching = mediaType.EnableCaching;
            selectedItem.ShowInSiteTree = mediaType.ShowInSiteTree;

            Return returnObj = MediaDetailsMapper.Update(selectedItem);

            if (returnObj.IsError)
                DisplayErrorMessage("Error Publishing Item", returnObj.Error);
            else
            {
                DisplaySuccessMessage("Successfully Published Item");

                UpdateFieldsFromObject();

                PublishNow.Visible = false;
                ContextHelper.Clear(ContextType.Cache);
                FileCacheHelper.ClearAllCache();
            }

            UpdateFieldsFromObject();
        }

        private IMediaDetail CreateHistory(MediaDetail fromItem, bool isDraft)
        {
            if (fromItem == null)
                return null;

            IMediaDetail history = null;

            if ((selectedItem.ID != 0) && (historyVersion == 0))
            {
                history = MediaDetailsMapper.CreateObject(fromItem.MediaTypeID, fromItem.Media, fromItem.Media.ParentMedia);
                history.CopyFrom(BaseMapper.GetObjectFromContext(fromItem));
                history.IsDraft = isDraft;
                //history.HistoryForMediaDetail = fromItem;

                foreach (var field in fromItem.Fields)
                {
                    var newField = new MediaDetailField();
                    newField.CopyFrom(field);

                    foreach (var fieldAssociation in field.FieldAssociations)
                    {
                        var newFieldAssociation = new FieldAssociation();
                        newFieldAssociation.CopyFrom(fieldAssociation);

                        var associatedMediaDetail = (MediaDetail)MediaDetailsMapper.GetByID(newFieldAssociation.AssociatedMediaDetailID);

                        if (associatedMediaDetail == null)
                            continue;
                        
                        newFieldAssociation.MediaDetail = (MediaDetail)MediaDetailsMapper.CreateObject(associatedMediaDetail.MediaType.ID, MediasMapper.CreateObject(), associatedMediaDetail.Media.ParentMedia);
                        newFieldAssociation.MediaDetail.DateCreated = newFieldAssociation.MediaDetail.DateLastModified = DateTime.Now;
                        newFieldAssociation.MediaDetail.CreatedByUser = newFieldAssociation.MediaDetail.LastUpdatedByUser = FrameworkSettings.CurrentUser;

                        /*if (newFieldAssociation.MediaDetail != null)
                        {
                            newFieldAssociation.MediaDetail.HistoryForMediaDetailID = fieldAssociation.AssociatedMediaDetailID;
                            newFieldAssociation.MediaDetail.HistoryVersionNumber = newFieldAssociation.MediaDetail.HistoryVersionNumber + 1;
                        }*/


                        newField.FieldAssociations.Add(newFieldAssociation);
                    }


                    history.Fields.Add(newField);
                }
            }

            return history;
        }

        private IMediaDetail CreateHistory(bool isDraft)
        {
            return CreateHistory((MediaDetail)selectedItem, isDraft);
        }

        private Return SaveHistory(IMediaDetail history)
        {
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            history.HistoryForMediaDetailID = selectedItem.ID;

            if (selectedItem.History.Count == 0)
                history.HistoryVersionNumber = 1;
            else
                history.HistoryVersionNumber = selectedItem.History.OrderByDescending(i => i.HistoryVersionNumber).Take(1).SingleOrDefault().HistoryVersionNumber + 1;

            if (selectedItem.History.Count >= MediaDetailsMapper.MaxHistory)
            {
                int count = selectedItem.History.Count - MediaDetailsMapper.MaxHistory;
                IEnumerable<MediaDetail> items = selectedItem.History.OrderBy(i => i.HistoryVersionNumber).Take(count);

                foreach (MediaDetail item in items)
                {
                    if (!item.IsDraft)
                        MediaDetailsMapper.DeletePermanently(item);
                }
            }

            if (history.MainContent.Trim() == "")
                history.MainContent = "TBD";

            return MediaDetailsMapper.Insert(history);
        }

        protected void SaveAndPublish_OnClick(object sender, EventArgs e)
        {
            Save_OnClick(sender, e);
            PublishNow_OnClick(sender, e);

            if ((selectedItem != null) && (selectedItem.IsPublished))
                RedirectToMediaDetail(selectedItem.MediaTypeID, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
        }

        protected void LoadLatestDraft_OnClick(object sender, EventArgs e)
        {
            var latestDraft = selectedItem.History.Where(i => i.IsDraft).OrderByDescending(i => i.HistoryVersionNumber).FirstOrDefault();

            if (latestDraft != null)
                RedirectToMediaDetail(selectedItem, latestDraft.HistoryVersionNumber);
        }

        protected void PublishLive_OnClick(object sender, EventArgs e)
        {
            var liveVersion = BaseMapper.GetObjectFromContext(selectedItem.HistoryForMediaDetail);
            selectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);
            IEnumerable<MediaDetail> items = liveVersion.History.ToList();

            foreach (var item in items)
            {
                if (item.ID != selectedItem.ID)
                {
                    var tmpItem = BaseMapper.GetObjectFromContext(item);
                    item.HistoryForMediaDetailID = selectedItem.ID;
                }
            }

            selectedItem.HistoryVersionNumber = 0;
            selectedItem.HistoryForMediaDetail = null;
            selectedItem.IsDraft = false;
            selectedItem.PublishDate = DateTime.Now;
            //selectedItem.ShowInMenu = true;

            foreach (var fieldAssociations in selectedItem.FieldAssociations)
            {
                var index = 1;
                foreach (var history in fieldAssociations.MediaDetail.History)
                {
                    history.HistoryForMediaDetail = fieldAssociations.MediaDetail;
                    history.HistoryVersionNumber = 1;

                    index++;
                }

                fieldAssociations.MediaDetail.HistoryForMediaDetail = null;
                fieldAssociations.MediaDetail.HistoryVersionNumber = 0;
            }

            foreach (var field in selectedItem.Fields)
            {
                foreach (var fieldAssociations in field.FieldAssociations)
                {
                    var index = 1;

                    foreach (var mediaDetail in fieldAssociations.MediaDetail.Media.MediaDetails)
                    {
                        mediaDetail.HistoryForMediaDetail = fieldAssociations.MediaDetail;
                        mediaDetail.HistoryVersionNumber = 1;

                        index++;
                    }

                    fieldAssociations.MediaDetail.HistoryForMediaDetail = null;
                    fieldAssociations.MediaDetail.HistoryVersionNumber = 0;
                }
            }


            liveVersion.HistoryVersionNumber = items.OrderByDescending(i => i.HistoryVersionNumber).FirstOrDefault().HistoryVersionNumber + 1;
            liveVersion.HistoryForMediaDetail = (MediaDetail)selectedItem;

            var associations = BaseMapper.GetDataModel().FieldAssociations.Where(i => i.AssociatedMediaDetailID == liveVersion.ID);

            foreach (var association in associations)
            {
                association.MediaDetail = (MediaDetail)selectedItem;
            }

            Return returnObj = MediaDetailsMapper.Update(selectedItem);

            if (!returnObj.IsError)
            {
                liveVersion.HistoryForMediaDetailID = selectedItem.ID;
                returnObj = MediaDetailsMapper.Update(liveVersion);

                if (!returnObj.IsError)
                {
                    ContextHelper.Clear(ContextType.Cache);
                    FileCacheHelper.ClearAllCache();

                    //if (selectedItem.AbsoluteUrl != liveVersion.AbsoluteUrl)
                    //    ChangeLinksForAllMediaDetails(liveVersion.AbsoluteUrl, selectedItem.AbsoluteUrl);

                    RedirectToMediaDetail(selectedItem);
                }
                else
                {
                    DisplayErrorMessage("Error Pushing LIVE", returnObj.Error);
                }
            }
            else
            {
                DisplayErrorMessage("Error Pushing LIVE", returnObj.Error);
            }
        }

        //private void ChangeLinksForAllMediaDetails(string oldAbsoluteUrl, string newAbsoluteUrl)
        //{
        //    if (string.IsNullOrEmpty(oldAbsoluteUrl) || oldAbsoluteUrl == newAbsoluteUrl)
        //        return;

        //    var oldTemplateVarUrl = ParserHelper.ParseData(oldAbsoluteUrl, this.TemplateVars, true);
        //    var newTemplateVarUrl = ParserHelper.ParseData(newAbsoluteUrl, this.TemplateVars, true);

        //    var foundItems = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MainContent.Contains(oldTemplateVarUrl) || i.ShortDescription.Contains(oldTemplateVarUrl));

        //    if (foundItems.Any())
        //    {
        //        foreach (var item in foundItems)
        //        {
        //            item.ShortDescription = item.ShortDescription.Replace(oldTemplateVarUrl, newTemplateVarUrl);
        //            item.MainContent = item.MainContent.Replace(oldTemplateVarUrl, newTemplateVarUrl);
        //        }

        //        var numberOfItemsEffected = MediaDetailsMapper.GetDataModel().SaveChanges();
        //    }
        //}

        protected void Save_OnClick(object sender, EventArgs e)
        {
            if (CanAccessItem != null && CanAccessItem.IsError)
            {
                DisplayErrorMessage("Error saving item", CanAccessItem.Error);
                return;
            }

            if (!CurrentUser.HasPermission(PermissionsEnum.SaveItems))
            {
                DisplayErrorMessage("Error saving item", ErrorHelper.CreateError(new Exception("You do not have the appropriate permissions to save items")));
                return;
            }

            string commandArgument = ((LinkButton)sender).CommandArgument;

            bool isDraft = false;

            if (commandArgument == "SaveAsDraft")
                isDraft = true;

            IMediaDetail history = CreateHistory(isDraft);

            Return returnObj = BaseMapper.GenerateReturn();

            if ((history != null) && (history.IsDraft))
            {
                history.CopyFrom(selectedItem);
                UpdateObjectFromFields(history);
                history.IsDraft = isDraft;

                returnObj = SaveHistory(history);

                if (returnObj.IsError)
                    DisplayErrorMessage("Error Saving Item", returnObj.Error);
                else
                {
                    DisplaySuccessMessage("Successfully Saved Item as Draft");

                    UpdateFieldsFromObject();
                }

                RedirectToMediaDetail(selectedItem, history.HistoryVersionNumber);

                return;
            }

            var tmpSelectedItem = BaseMapper.GetObjectFromContext((MediaDetail)selectedItem);

            if (tmpSelectedItem != null)
                selectedItem = tmpSelectedItem;

            if (parentMediaItem != null)
                parentMediaItem = BaseMapper.GetObjectFromContext(parentMediaItem);
            else
            {
                parentMediaItem = selectedItem?.Media?.ParentMedia;
            }

            if (selectedItem == null)
                selectedItem = MediaDetailsMapper.CreateObject(mediaTypeId, selectedMediaItem, parentMediaItem);

            var oldVirtualPath = selectedItem.VirtualPath;
            var canRender = selectedItem.CanRender;
            var oldAbsoluteUrl = selectedItem.AbsoluteUrl;
            var oldPostPublishDate = selectedItem.PublishDate;

            UpdateObjectFromFields(selectedItem);

            var validate = selectedItem.Validate();

            if (validate.IsError)
            {
                DisplayErrorMessage("Error saving item", validate.Error);
                return;
            }

            if ((oldPostPublishDate != selectedItem.PublishDate) && !CurrentUser.HasPermission(PermissionsEnum.PublishItems))
            {
                DisplayErrorMessage("Error saving item", ErrorHelper.CreateError(new Exception("You do not have the appropriate permissions to publish items")));
                return;
            }

            if (selectedItem.ID == 0)
            {
                selectedItem.CreatedByUserID = CurrentUser.ID;

                if ((parentMediaItem != null) && (selectedItem.Media.ID == 0))
                    selectedItem.Media.OrderIndex = parentMediaItem.ChildMedias.Count(i => i.ID != 0);


                returnObj = MediaDetailsMapper.Insert(selectedItem);

                if (!returnObj.IsError)
                {
                    ContextHelper.Clear(ContextType.Cache);
                    FileCacheHelper.ClearAllCache();
                }
                else
                {
                    DisplayErrorMessage("Error", returnObj.Error);
                }
            }
            else
            {
                if (!isDraft)
                {
                    if (history != null)
                        returnObj = SaveHistory(history);

                    selectedItem.Media.ReorderChildren();

                    if (!returnObj.IsError)
                    {
                        returnObj = MediaDetailsMapper.Update(selectedItem);
                    }
                }
            }

            if (returnObj.IsError)
                DisplayErrorMessage("Error Saving Item", returnObj.Error);
            else
            {
                if (history != null)
                {
                    selectedItem.History.Add((MediaDetail)history);
                    Return historyReturnObj = MediaDetailsMapper.Update(selectedItem);
                }

                selectedItem.RemoveFromCache();

                if (oldVirtualPath != selectedItem.VirtualPath || canRender != selectedItem.CanRender)
                {
                    ContextHelper.Clear(ContextType.Cache);
                    FileCacheHelper.ClearAllCache();

                    //ChangeLinksForAllMediaDetails(oldAbsoluteUrl, selectedItem.AbsoluteUrl);
                }

                DisplaySuccessMessage("Successfully Saved Item");

                if (!selectedItem.IsHistory)
                {
                    SelectedMediaDetail = selectedItem;
                    SelectedMedia = MediasMapper.GetByID(selectedItem.MediaID);
                }

                if (((Request["selectedMediaId"] == null) || (Request["selectedMediaId"].ToString() == "0")) && (commandArgument != "SaveAndPublish"))
                {
                    RedirectToMediaDetail(selectedItem.MediaTypeID, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
                }
                else
                {
                    if (commandArgument == "SaveAndPublish")
                    {
                        PublishNow_OnClick(sender, e);
                        //RedirectToMediaDetail(mediaTypeEnum, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
                    }

                    UpdateFieldsFromObject();

                    /*else
                        Response.Redirect(Request.Url.AbsoluteUri);*/

                    //UpdateFieldsFromObject();
                    //RedirectToMediaDetail(mediaTypeEnum, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
                }
            }
        }
    }
}