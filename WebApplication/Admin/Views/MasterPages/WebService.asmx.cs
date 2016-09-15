using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace WebApplication.Admin.Views.MasterPages
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public void CreateChild(long id)
        {
            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.SaveItems))
            {
                throw new Exception("You do not have the appropriate permissions to create items");
            }

            var mediaDetail = MediaDetailsMapper.GetByID(id);

            AdminBasePage.SelectedMediaDetail = mediaDetail;
            AdminBasePage.SelectedMedia = mediaDetail.Media;
        }

        [WebMethod(EnableSession = true)]
        public void Delete(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            SetDeleteStatus(detail, true);
        }

        [WebMethod(EnableSession = true)]
        public void UnDelete(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            SetDeleteStatus(detail, false);
        }

        [WebMethod(EnableSession = true)]
        public void DeletePermanently(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
            {
                HandleDeletePermanentlyRecursive(detail.Media);
                ContextHelper.ClearAllMemoryCache();
                FileCacheHelper.ClearAllCache();
            }
        }

        [WebMethod(EnableSession = true)]
        public string Duplicate(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
            {
                var mediaDetail = HandleDuplicate(detail, detail.Media.ParentMedia);

                ContextHelper.ClearAllMemoryCache();

                var url = WebApplication.BasePage.GetRedirectToMediaDetailUrl(mediaDetail.MediaTypeID, mediaDetail.MediaID);
                return url;
            }

            return "";
        }

        [WebMethod(EnableSession = true)]
        public void ShowInMenu(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            SetShowInMenuStatus((MediaDetail)detail, true);
        }

        [WebMethod(EnableSession = true)]
        public void HideFromMenu(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            SetShowInMenuStatus((MediaDetail)detail, false);
        }

        [WebMethod(EnableSession = true)]
        public void MoveUp(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            detail.Media.MoveUp();
        }

        [WebMethod(EnableSession = true)]
        public void MoveDown(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            detail.Media.MoveDown();
        }

        [WebMethod(EnableSession = true)]
        public string HandleNodeDragDrop(long sourceMediaDetailId, long parentMediaDetailId, int newPosition)
        {
            var sourceMedia = BaseMapper.GetObjectFromContext(MediaDetailsMapper.GetByID(sourceMediaDetailId).Media);
            var parentMedia = BaseMapper.GetObjectFromContext(MediaDetailsMapper.GetByID(parentMediaDetailId).Media);

            var oldParentId = sourceMedia.ParentMediaID;

            sourceMedia.MoveToIndex(newPosition);

            sourceMedia.ParentMedia = parentMedia;

            if(oldParentId != parentMedia.ID)
            {
                foreach (var item in sourceMedia.MediaDetails)
                {
                    item.CachedVirtualPath = item.CalculatedVirtualPath();
                }
            }

            var returnObj = MediasMapper.Update(sourceMedia);

            return returnObj.ToJSON();
        }

        private void SetShowInMenuStatus(MediaDetail detail, bool showInMenu)
        {
            if ((detail == null) || (detail.ShowInMenu == showInMenu))
                return;

            detail = BaseMapper.GetObjectFromContext(detail);
            detail.ShowInMenu = showInMenu;

            Return returnObj = MediaDetailsMapper.Update(detail);

            if (returnObj.IsError)
                throw returnObj.Error.Exception;
            else
            {
                ContextHelper.ClearAllMemoryCache();
                FileCacheHelper.ClearAllCache();
            }
        }

        public IMediaDetail HandleDuplicate(IMediaDetail detail, Media parentMedia)
        {
            var duplicatedItem = MediaDetailsMapper.CreateObject(detail.MediaTypeID, null, parentMedia);
            duplicatedItem.CopyFrom(detail, new List<string> { "MediaID", "Media" });
            duplicatedItem.CachedVirtualPath = duplicatedItem.CalculatedVirtualPath();

            /*if (duplicatedItem.ID == 0)
            {
                var mediaType = MediaTypesMapper.GetByID(detail.MediaTypeID);
                var createdItems = mediaType.MediaDetails.Where(i => !i.IsHistory && i.ParentMediaID == detail.ParentMediaID && i.LanguageID == detail.LanguageID).Select(i => i);

                var newIndex = createdItems.Count() + 1;

                duplicatedItem.LinkTitle = detail.Language.DisplayName + " - " + mediaType.Name + " " + newIndex;
            }*/

            duplicatedItem.Media.OrderIndex = parentMedia.ChildMedias.Count(i => i.ID != 0);

            foreach (var item in detail.Fields)
            {
                var mediaDetailField = new MediaDetailField();
                mediaDetailField.FieldCode = item.FieldCode;
                mediaDetailField.FieldLabel = item.FieldLabel;
                mediaDetailField.UseMediaTypeFieldFrontEndLayout = item.UseMediaTypeFieldFrontEndLayout;
                mediaDetailField.FrontEndLayout = item.FrontEndLayout;
                mediaDetailField.RenderLabelAfterControl = item.RenderLabelAfterControl;
                mediaDetailField.AdminControl = item.AdminControl;
                mediaDetailField.FieldValue = item.FieldValue;
                mediaDetailField.GroupName = item.GroupName;
                mediaDetailField.GetAdminControlValue = item.GetAdminControlValue;
                mediaDetailField.SetAdminControlValue = item.SetAdminControlValue;
                mediaDetailField.DateCreated = mediaDetailField.DateLastModified = DateTime.Now;

                duplicatedItem.Fields.Add(mediaDetailField);
            }

            var returnObj = MediaDetailsMapper.Insert(duplicatedItem);

            if (returnObj.IsError)
                throw returnObj.Error.Exception;
            else
            {
                foreach (var child in detail.Media.ChildMedias)
                {
                    var mediaDetailsToCopy = child.MediaDetails.Where(i => !i.IsDraft && !i.IsHistory);

                    foreach (var childDetail in mediaDetailsToCopy)
                    {
                        HandleDuplicate(childDetail, duplicatedItem.Media);
                    }
                }
            }

            return duplicatedItem;
        }

        private void HandleDeletePermanently(Media item)
        {
            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.DeleteItemsPermanently))
            {
                throw new Exception("You do not have the appropriate permissions to delete items permanently");
            }

            if (item == null)
                return;

            MediaDetail detail = (MediaDetail)GetAtleastOneByMedia(item);
            Media parentMedia = null;

            if (item.ParentMediaID != null)
                parentMedia = MediasMapper.GetByID((long)item.ParentMediaID);

            Return returnObj = BaseMapper.GenerateReturn("No action performed");

            if (detail == null)
            {
                if (item.ChildMedias.Count == 0)
                    returnObj = MediasMapper.DeletePermanently(item);
            }
            else
            {
                item = BaseMapper.GetObjectFromContext(item);
                if ((item.MediaDetails.Count == 1) && (item.ChildMedias.Count > 0))
                {
                    throw new Exception("You cannot delete this item because it has child items");
                }

                detail = BaseMapper.GetObjectFromContext(detail);
                returnObj = MediaDetailsMapper.DeletePermanently(detail);

                if (!returnObj.IsError)
                {
                    ContextHelper.ClearAllMemoryCache();
                    FileCacheHelper.ClearAllCache();
                }
            }
        }

        private void HandleDeletePermanentlyRecursive(Media item)
        {
            var parentItem = item.ParentMedia;

            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.DeleteItemsPermanently))
            {
                throw new Exception("You do not have the appropriate permissions to delete items permanently");
            }

            if (item == null)
                return;

            var childItems = item.ChildMedias.ToList();

            var returnObj = new Return();

            foreach (var childItem in childItems)
            {
                var childItemMediaDetails = childItem.MediaDetails.Where(i => i.Language.ID == AdminBasePage.CurrentLanguage.ID).ToList();

                foreach (var childItemMediaDetail in childItemMediaDetails)
                {
                    if ((childItemMediaDetail.Media != null) && (childItemMediaDetail.Media.ChildMedias.Count > 0))
                        HandleDeletePermanentlyRecursive(childItemMediaDetail.Media);
                    else
                    {
                        returnObj = MediaDetailsMapper.DeletePermanently(childItemMediaDetail);

                        if (returnObj.IsError)
                            break;
                    }
                }
            }

            if (!returnObj.IsError)
            {
                var itemMediaDetails = item.MediaDetails.Where(i => i.Language.ID == AdminBasePage.CurrentLanguage.ID).ToList();
                var allHistory = itemMediaDetails.Where(i => i.HistoryVersionNumber != 0);
                var liveVersion = itemMediaDetails.Where(i => i.HistoryVersionNumber == 0);

                foreach (var itemMediaDetail in allHistory)
                {
                    returnObj = MediaDetailsMapper.DeletePermanently(itemMediaDetail);

                    if (returnObj.IsError)
                    {
                        throw returnObj.Error.Exception;
                    }
                }

                foreach (var itemMediaDetail in liveVersion)
                {
                    returnObj = MediaDetailsMapper.DeletePermanently(itemMediaDetail);

                    if (returnObj.IsError)
                    {
                        throw returnObj.Error.Exception;
                    }
                }
            }

            if ((!returnObj.IsError) && (parentItem != null))
            {
                var selectItem = parentItem;

                if (parentItem.ChildMedias.Count > 0)
                    selectItem = parentItem.ChildMedias.ElementAt(0);

                if (selectItem == null)
                    selectItem = parentItem;

                parentItem.ReorderChildren();

                //AdminBasePage.SelectedMedia = selectItem;
            }
        }

        private IMediaDetail GetAtleastOneByMedia(Media item)
        {
            return MediaDetailsMapper.GetAtleastOneByMedia(item, AdminBasePage.CurrentLanguage);
        }

        private void SetDeleteStatus(IMediaDetail detail, bool isDeleted)
        {
            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.DeleteItems))
            {
                throw new Exception("You do not have the appropriate permissions to delete/undelete items");
            }

            detail = BaseMapper.GetObjectFromContext((MediaDetail)detail);
            detail.IsDeleted = isDeleted;

            Return returnObj = MediaDetailsMapper.Update(detail);

            if (returnObj.IsError)
                throw returnObj.Error.Exception;
            else
            {
                ContextHelper.ClearAllMemoryCache();
                FileCacheHelper.ClearAllCache();
            }
        }

        [WebMethod]
        public int ReOrderMediaFields(List<FrameworkLibrary.MediaDetailField> items)
        {
            var index = 0;
            foreach (var item in items)
            {
                var mediaField = BaseMapper.GetDataModel().Fields.SingleOrDefault(i => i.ID == item.ID);

                if (mediaField != null)
                {
                    mediaField.OrderIndex = index;

                    index++;
                }
            }

            return BaseMapper.GetDataModel().SaveChanges();
        }
    }
}