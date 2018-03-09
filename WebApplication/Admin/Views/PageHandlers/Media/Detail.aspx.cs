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

                /*if (parentMediaItem == null)
                    return;*/

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
                CreateDraft.Visible = false;

                var mediaType = MediaTypesMapper.GetByID(selectedItem.MediaTypeID);

                if (mediaType == null)
                    return;

                selectedItem.MainLayout = mediaType.MainLayout;
                selectedItem.SummaryLayout = mediaType.SummaryLayout;
                selectedItem.FeaturedLayout = mediaType.FeaturedLayout;

                selectedItem.UseMediaTypeLayouts = mediaType.UseMediaTypeLayouts;

                var liveMediaDetail = selectedItem.Media?.GetLiveMediaDetail();

                if (liveMediaDetail != null)
                {
                    selectedItem.CopyFrom(liveMediaDetail);

                    var fieldsNotInMediaType = liveMediaDetail.Fields.Where(i => i.MediaTypeFieldID == null);

                    if (fieldsNotInMediaType != null)
                    {
                        foreach (var field in fieldsNotInMediaType)
                        {
                            var newField = new MediaDetailField();
                            newField.CopyFrom(field);

                            if (field.FieldAssociations.Count > 0)
                                newField.FieldValue = "";
                            else
                                newField.FieldValue = field.FieldValue;

                            newField.DateCreated = DateTime.Now;
                            newField.DateLastModified = DateTime.Now;

                            selectedItem.Fields.Add(newField);
                        }
                    }

                    var fieldsThatCanBeCopied = liveMediaDetail.Fields.Where(i => !i.FieldAssociations.Any());

                    foreach (var field in fieldsThatCanBeCopied)
                    {
                        var foundField = selectedItem.Fields.FirstOrDefault(i => i.FieldCode == field.FieldCode);

                        if (foundField != null)
                        {
                            foundField.CopyFrom(field);
                        }
                    }
                }
            }
            else
            {
                Save.Text = "Save Page";
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
                    CreateDraft.Visible = false;
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

                CreateDraft.Visible = false;
                PublishNow.Visible = false;
                PublishLive.Visible = true;

                HistoryPanel.Visible = true;
                HistoryVersionNumber.Text = historyVersion.ToString();
            }

            if (CurrentUser.IsInRole(RoleEnum.Developer))
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
            WebApplication.BasePage.RedirectToAdminUrl(selectedItem);
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

                sectionTitle = "Creating: <span>New Media of Type '" + MediaTypesMapper.GetByID(mediaTypeId).Name + "' Under '" + under + "' with Language '" + CurrentLanguage.Name + "'</span>";
                title = StringHelper.StripHtmlTags(sectionTitle);
            }
            else
            {
                title = "Editing: " + selectedItem.LinkTitle;
                sectionTitle = "Editing: <span>" + selectedItem.LinkTitle + "</span>";
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
            if (!CurrentUser.HasPermission(PermissionsEnum.Publish))
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

            OnPublishExecuteCode();
        }

        private void OnPublishExecuteCode()
        {
            var onPublishExecuteCode = selectedItem.UseMediaTypeLayouts ? selectedItem.MediaType.OnPublishExecuteCode : selectedItem.OnPublishExecuteCode;

            if (!string.IsNullOrEmpty(onPublishExecuteCode))
            {
                var result = MediaDetailsMapper.ParseSpecialTags(selectedItem, onPublishExecuteCode);
                DisplayFeedbackMessage(result);
            }
        }

        private IMediaDetail CreateHistory(MediaDetail fromItem, bool isDraft)
        {
            if (fromItem == null)
                return null;

            IMediaDetail history = null;

            if ((selectedItem.ID != 0) && (historyVersion == 0))
            {
                history = MediaDetailsMapper.CreateObject(fromItem.MediaTypeID, fromItem.Media, fromItem.Media.ParentMedia, false);
                history.CopyFrom(BaseMapper.GetObjectFromContext(fromItem));
                history.IsDraft = isDraft;

                CopyProperties(history, fromItem);
            }

            return history;
        }

        public void CopyProperties(IMediaDetail toItem, IMediaDetail fromItem)
        {            
            foreach (var field in fromItem.Fields)
            {
                var newField = new MediaDetailField();

                var foundField = toItem.Fields.FirstOrDefault(i => i.FieldCode == field.FieldCode);

                if(foundField != null)
                {
                    newField = foundField;
                }

                newField.CopyFrom(field);

                foreach (var fieldAssociation in field.FieldAssociations)
                {
                    var newFieldAssociation = new FieldAssociation();
                    newFieldAssociation.CopyFrom(fieldAssociation);

                    var associatedMediaDetail = (MediaDetail)MediaDetailsMapper.GetByID(newFieldAssociation.AssociatedMediaDetailID);

                    if (associatedMediaDetail == null)
                        continue;

                    if (!associatedMediaDetail.MediaType.ShowInSiteTree)
                    {
                        newFieldAssociation.MediaDetail = (MediaDetail)MediaDetailsMapper.CreateObject(associatedMediaDetail.MediaType.ID, MediasMapper.CreateObject(), associatedMediaDetail.Media.ParentMedia);
                        newFieldAssociation.MediaDetail.CopyFrom(associatedMediaDetail);
                        newFieldAssociation.MediaDetail.DateCreated = newFieldAssociation.MediaDetail.DateLastModified = DateTime.Now;
                        newFieldAssociation.MediaDetail.CreatedByUser = newFieldAssociation.MediaDetail.LastUpdatedByUser = FrameworkSettings.CurrentUser;

                        CopyProperties(newFieldAssociation.MediaDetail, associatedMediaDetail);
                    }

                    /*if (newFieldAssociation.MediaDetail != null)
                    {
                        newFieldAssociation.MediaDetail.HistoryForMediaDetailID = fieldAssociation.AssociatedMediaDetailID;
                        newFieldAssociation.MediaDetail.HistoryVersionNumber = newFieldAssociation.MediaDetail.HistoryVersionNumber + 1;
                    }*/


                    newField.FieldAssociations.Add(newFieldAssociation);
                }

                if(newField.ID == 0)
                {
                    toItem.Fields.Add(newField);
                }
            }
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
                history.HistoryVersionNumber = selectedItem.History.OrderByDescending(i => i.HistoryVersionNumber).Take(1).FirstOrDefault().HistoryVersionNumber + 1;

            if (selectedItem.History.Count >= MediaDetailsMapper.MaxHistory)
            {
                int count = selectedItem.History.Count - MediaDetailsMapper.MaxHistory;
                IEnumerable<MediaDetail> items = selectedItem.History.OrderBy(i => i.HistoryVersionNumber).Take(count);

                foreach (MediaDetail item in items)
                {
                    if (!item.IsDraft)
                    {
                        MediaDetailsMapper.ClearObjectRelations(item);
                        MediaDetailsMapper.DeleteObjectFromContext(item);
                    }
                }
            }

            /*if (history.MainContent.Trim() == "")
                history.MainContent = "TBD";*/

            return MediaDetailsMapper.Insert(history);
        }

        protected void SaveAndPublish_OnClick(object sender, EventArgs e)
        {
            Save_OnClick(sender, e);
            PublishNow_OnClick(sender, e);

            if ((selectedItem != null) && (selectedItem.IsPublished))
                RedirectToAdminUrl(selectedItem.MediaTypeID, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
        }

        protected void LoadLatestDraft_OnClick(object sender, EventArgs e)
        {
            var latestDraft = selectedItem.History.Where(i => i.IsDraft).OrderByDescending(i => i.HistoryVersionNumber).FirstOrDefault();

            if (latestDraft != null)
                RedirectToAdminUrl(selectedItem, latestDraft.HistoryVersionNumber);
        }

        protected void PublishLive_OnClick(object sender, EventArgs e)
        {
            var returnObj = ((MediaDetail)selectedItem).PublishLive();

            if (!returnObj.IsError)
            {
                ContextHelper.Clear(ContextType.Cache);
                FileCacheHelper.ClearAllCache();

                //if (selectedItem.AbsoluteUrl != liveVersion.AbsoluteUrl)
                //    ChangeLinksForAllMediaDetails(liveVersion.AbsoluteUrl, selectedItem.AbsoluteUrl);

                OnPublishExecuteCode();

                RedirectToAdminUrl(selectedItem);
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

            if (!CurrentUser.HasPermission(PermissionsEnum.Publish))
            {
                DisplayErrorMessage("Error saving item", ErrorHelper.CreateError(new Exception("You do not have the appropriate permissions to save items")));
                return;
            }

            string commandArgument = ((LinkButton)sender).CommandArgument;

            bool isDraft = false;

            if (commandArgument == "CreateDraft")
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

                RedirectToAdminUrl(selectedItem, history.HistoryVersionNumber);

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

            var oldLinkTitle = selectedItem.LinkTitle;
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

            if ((oldPostPublishDate != selectedItem.PublishDate) && !CurrentUser.HasPermission(PermissionsEnum.Publish))
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

                    //selectedItem.Media.ReorderChildren();

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
                FileCacheHelper.DeleteGenerateNavCache();

                if (oldVirtualPath != selectedItem.VirtualPath || canRender != selectedItem.CanRender || oldLinkTitle != selectedItem.LinkTitle)
                {
                    ContextHelper.Clear(ContextType.Cache);
                    //FileCacheHelper.ClearCacheDir("generatenav");

                    //FileCacheHelper.ClearAllCache();

                    selectedItem.ClearAutoCalculatedVirtualPathCache();

                    if (commandArgument == "SaveAndPublish")
                    {
                        PublishNow_OnClick(sender, e);
                        //RedirectToMediaDetail(mediaTypeEnum, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
                    }
                    else
                    {
                        RedirectToAdminUrl(selectedItem.MediaTypeID, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
                    }

                    //ChangeLinksForAllMediaDetails(oldAbsoluteUrl, selectedItem.AbsoluteUrl);
                }

                DisplaySuccessMessage("Successfully Saved Item");

                ExecuteRawJS("ReloadPreviewPanel()");

                if (!selectedItem.IsHistory)
                {
                    SelectedMediaDetail = selectedItem;
                    SelectedMedia = MediasMapper.GetByID(selectedItem.MediaID);
                }

                if (((Request["selectedMediaId"] == null) || (Request["selectedMediaId"].ToString() == "0")) && (commandArgument != "SaveAndPublish"))
                {
                    RedirectToAdminUrl(selectedItem.MediaTypeID, selectedItem.MediaID, selectedItem.Media.ParentMediaID);
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