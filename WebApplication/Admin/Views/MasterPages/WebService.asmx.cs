using FrameworkLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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


    public class JsTreeNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public A_Attr a_attr { get; set; } = new A_Attr();
        public Li_Attr li_attr { get; set; } = new Li_Attr();
        public State state { get; set; } = new State();
        public bool children { get; set; }
    }

    public class State
    {
        public bool opened { get; set; } = false;
        public bool disabled { get; set; } = false;
        public bool selected { get; set; } = false;
    }

    public class A_Attr
    {
        public string href { get; set; }

        [JsonProperty(PropertyName = "class")]
        public string _class { get; set; } = "";
        public string frontendurl { get; set; }
    }

    public class Li_Attr
    {
        [JsonProperty(PropertyName = "class")]
        public string _class { get; set; } = "";
        public string mediaDetailId { get; set; }
    }

    public class WebService : WebApplication.Services.BaseService
    {
        private void UpdateTreeNode(JsTreeNode node, IMediaDetail detail)
        {
            if (detail == null)
                return;

            node.id = detail.MediaID.ToString();
            node.parent = (detail.Media.ParentMediaID == null) ? "#" : detail.Media.ParentMediaID.ToString();
            node.text = detail.SectionTitle;

            //node.children =( MediaDetailsMapper.GetAtleastOneChildByMedia(detail.Media, AdminBasePage.CurrentLanguage).Where(i => i.MediaType.ShowInSiteTree).Count() > 0);
            node.children = (BaseMapper.GetDataModel().MediaDetails.Count(i => i.MediaType.ShowInSiteTree && i.HistoryVersionNumber == 0 && i.Media.ParentMediaID == detail.MediaID) > 0);

            node.text = detail.SectionTitle.ToString();
            //node.Attributes.Add("FrontEndUrl", detail.AbsoluteUrl);

            var nodeText = "";

            if (detail.LanguageID != AdminBasePage.CurrentLanguage.ID)
            {
                nodeText = detail.LinkTitle + " - " + LanguagesMapper.GetByID(detail.Language.ID).Name;
                node.li_attr._class = "doesNotExistInLanguage";
            }
            else
            {
                nodeText = $"{detail.LinkTitle} <small>({detail.MediaID})</small>";
            }

            if (detail.IsDeleted)
            {
                node.li_attr._class += " isDeleted";
            }

            if ((!detail.ShowInMenu) && (!detail.RenderInFooter))
                node.li_attr._class += " isHidden";

            if ((!detail.CanRender) || (!detail.IsPublished))
                node.li_attr._class += " unPublished";

            if(AdminBasePage.SelectedMediaDetail != null && AdminBasePage.SelectedMediaDetail.ID == detail.ID)
            {
                node.li_attr._class += " selected";
            }

            var draft = detail.GetLatestDraft();

            if (detail.HasDraft && draft != null)
            {
                node.li_attr._class += " hasDraft";

                var autoPublishCode = "";                

                if (draft.DateLastModified > detail.DateLastModified)
                {
                    node.li_attr._class += " draftIsNewer";
                }

                if ((draft.PublishDate - detail.PublishDate) > TimeSpan.FromSeconds(10))
                {
                    autoPublishCode = $"<i class='fa fa-clock-o' aria-hidden='true' title='This draft is set to auto-publish at: {draft.PublishDate}'></i> ";
                }

                nodeText += $"<small class='hasDraftWrapper'>{autoPublishCode}Has Draft</small>";
            }

            var pendingComments = detail.Media.Comments.Count(i => i.Status == StatusEnum.Pending.ToString() && i.LanguageID == detail.LanguageID);

            if (pendingComments > 0)
            {
                node.li_attr._class += " hasPendingComments";
                nodeText += $"<small class='hasPendingCommentsWrapper'>Pending Comments</small>";
            }

            if (detail.MediaType.GetRoles().Count > 0 || detail.Media.RolesMedias.Count > 0)
            {
                node.li_attr._class += " restricted";
                nodeText += $"<small class='restrictedWrapper'><i class='fa fa-lock' aria-hidden='true'></i> Restricted</small>";
            }

            node.text = nodeText;

            //node.LinkAttributes.Add("data-frontendurl", detail.Media.PermaLink);
            node.a_attr.frontendurl = detail.AbsoluteUrl;
            node.li_attr.mediaDetailId = detail.ID.ToString();

            node.a_attr.href = URIHelper.ConvertToAbsUrl(WebApplication.BasePage.GetAdminUrl(detail.MediaTypeID, detail.MediaID));

            if (detail?.MediaType?.Name == "Website" || detail?.MediaType?.Name == "RootPage")
            {
                node.state.opened = true;
            }

            IEnumerable<Media> parentItems = new List<Media>();

            if (AdminBasePage.SelectedMedia != null)
            {
                parentItems = MediaDetailsMapper.GetAllParentMedias(AdminBasePage.SelectedMedia);

                if (parentItems.Any(i => i.ID == detail.MediaID))
                {
                    node.state.opened = true;
                }
            }

            if (detail.MediaID == AdminBasePage.SelectedMedia?.ID)
            {
                node.state.opened = true;
                node.state.selected = true;
            }
        }
        private JsTreeNode GetJsTreeNode(IMediaDetail mediaDetail)
        {
            var jsTreeNode = new JsTreeNode();
            UpdateTreeNode(jsTreeNode, mediaDetail);

            return jsTreeNode;
        }

        [WebMethod(EnableSession = true)]
        public void GetRootNodes()
        {
            var rootNode = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryForMediaDetail == null && i.Media.ParentMedia == null);
            WriteJSON(StringHelper.ObjectToJson(GetJsTreeNode(rootNode)));
        }

        private bool SearchWithinMediaDetail(IMediaDetail mediaDetail, string filterText)
        {
            if (mediaDetail == null)
                return false;

            if (mediaDetail.HistoryVersionNumber == 0 && mediaDetail.MediaID.ToString() == filterText || mediaDetail.SearchForTerm(filterText) || mediaDetail.Fields.Any(j => j.FieldValue.Contains(filterText)))
                return true;

            foreach (var fieldAssociation in mediaDetail.Fields.SelectMany(i => i.FieldAssociations))
            {
                if (fieldAssociation.MediaDetail.ID == mediaDetail.ID)
                    continue;

                if (SearchWithinMediaDetail(fieldAssociation.MediaDetail, filterText))
                {
                    return true;
                }
            }

            return false;
        }

        [WebMethod(EnableSession = true)]
        public void SearchForNodes(string filterText)
        {
            filterText = filterText.ToLower().Trim();
            var foundItems = BaseMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.ShowInSiteTree &&
                                                                            i.HistoryVersionNumber == 0 &&
                                                                            i.LanguageID == AdminBasePage.CurrentLanguage.ID &&
                                                                            (i.MediaID.ToString() == filterText ||
                                                                                i.Fields.FirstOrDefault(j=>j.FieldCode=="MainContent").FieldValue.ToLower().Contains(filterText) ||
                                                                                i.Fields.FirstOrDefault(j => j.FieldCode == "ShortDescription").FieldValue.ToLower().Contains(filterText) ||
                                                                                i.Fields.FirstOrDefault(j => j.FieldCode == "SectionTitle").FieldValue.ToLower().Contains(filterText) ||
                                                                                i.MainLayout.ToLower().Contains(filterText) ||
                                                                                i.MediaType.MainLayout.ToLower().Contains(filterText) ||
                                                                                i.Fields.Any(j => (j.FieldAssociations.Count == 0 && j.FieldValue.ToLower().Contains(filterText)) ||
                                                                                                j.FieldAssociations.Any(k => !k.MediaDetail.MediaType.ShowInSiteTree &&
                                                                                                                            (k.MediaDetail.Fields.FirstOrDefault(l => l.FieldCode == "SectionTitle").FieldValue.ToLower().Contains(filterText) ||
                                                                                                                            k.MediaDetail.Fields.FirstOrDefault(l => l.FieldCode == "MainContent").FieldValue.ToLower().Contains(filterText) ||
                                                                                                                            k.MediaDetail.Fields.FirstOrDefault(l => l.FieldCode == "MainLayout").FieldValue.ToLower().Contains(filterText))
                                                                                                                        ))
                                                                            )).ToList();

            var jsTreeNodes = foundItems.Select(i => GetJsTreeNode(i));

            var newJsTreeNodes = new List<JsTreeNode>();
            foreach (var node in jsTreeNodes)
            {
                node.parent = "#";
                node.state.opened = false;
                node.state.selected = false;
                node.children = false;

                newJsTreeNodes.Add(node);

            }

            WriteJSON(StringHelper.ObjectToJson(newJsTreeNodes));
        }

        [WebMethod(EnableSession = true)]
        public void GetChildNodes(long id)
        {
            var rootNode = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryForMediaDetail == null && i.MediaID == id);

            if (rootNode != null)
            {
                IEnumerable<IMediaDetail> childMediaDetails = MediaDetailsMapper.GetAtleastOneChildByMedia(rootNode.Media, AdminBasePage.CurrentLanguage).Where(i => i.MediaType.ShowInSiteTree).OrderBy(i => i.Media.OrderIndex);
                //var childMediaDetails = BaseMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.Media.ParentMediaID == rootNode.MediaID && i.ID != rootNode.ID && i.MediaType.ShowInSiteTree && i.LanguageID == AdminBasePage.CurrentLanguage.ID).OrderBy(i => i.Media.OrderIndex).ToList();

                childMediaDetails = childMediaDetails.Where(i =>
                {
                    if(MediaDetailsMapper.CanAccessMediaDetail(i, FrameworkSettings.CurrentUser).IsError)
                    {
                        return false;
                    }

                    return true;
                });

                var jsTreeNodes = childMediaDetails.Select(i => GetJsTreeNode(i));
                WriteJSON(StringHelper.ObjectToJson(jsTreeNodes));
            }
        }

        /*private string JsTreeNodesToJson(IEnumerable<JsTreeNode> nodes)
        {
            var json = "[";
            foreach (var node in nodes)
            {
                json += JsTreeNodesToJson(node) + ",";
            }
            json += "]";

            json = json.Replace(",]", "]");

            return json;
        }

        private string JsTreeNodesToJson(JsTreeNode node)
        {
            var json = "{\"id\":\"" + node.id + "\",\"text\":\"" + node.text + "\",\"parent\":\"" + node.parent + "\",\"status\": {\"opened\":" + node.state.opened.ToString().ToLower() + ", \"disabled\":" + node.state.disabled.ToString().ToLower() + ",\"selected\":" + node.state.selected.ToString().ToLower() + "},\"children\":true, \"li_attr\":{\"mediaDetailId\":\"" + node.li_attr.mediaDetailId + "\",\"class\":\"" + node.li_attr._class + "\"}, \"a_attr\":{\"frontendurl\":\"" + node.a_attr.frontendurl + "\",\"href\":\"" + node.a_attr.href + "\",\"class\":\"" + node.a_attr._class + "\"}}";
            return json;
        }*/

        [WebMethod(EnableSession = true)]
        public void CreateChild(long id)
        {
            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.Save))
            {
                throw new Exception("You do not have the appropriate permissions to create items");
            }

            var detail = MediaDetailsMapper.GetByID(id);

            UserMustHaveAccessTo(detail);

            AdminBasePage.SelectedMediaDetail = detail;
            AdminBasePage.SelectedMedia = detail.Media;
        }

        [WebMethod(EnableSession = true)]
        public void Delete(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            UserMustHaveAccessTo(detail);
            SetDeleteStatus(detail, true);
        }

        [WebMethod(EnableSession = true)]
        public void ClearCache(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            UserMustHaveAccessTo(detail);
            detail.RemoveFromCache();
        }

        [WebMethod(EnableSession = true)]
        public void UnDelete(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            UserMustHaveAccessTo(detail);
            SetDeleteStatus(detail, false);
        }

        [WebMethod(EnableSession = true)]
        public void DeletePermanently(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
            {
                UserMustHaveAccessTo(detail);

                HandleDeletePermanentlyRecursive(detail.Media);
                ContextHelper.ClearAllMemoryCache();
                FileCacheHelper.ClearAllCache();
            }
        }

        [WebMethod(EnableSession = true)]
        public string Duplicate(long id, bool duplicateChildren = false, string newName="")
        {
            var detail = MediaDetailsMapper.GetByID(id);

            if (detail != null)
            {
                UserMustHaveAccessTo(detail);

                var mediaDetail = HandleDuplicate(detail, detail.Media.ParentMedia, duplicateChildren, newName);

                ContextHelper.ClearAllMemoryCache();

                var url = WebApplication.BasePage.GetAdminUrl(mediaDetail.MediaTypeID, mediaDetail.MediaID);
                return url;
            }

            return "";
        }

        [WebMethod(EnableSession = true)]
        public void ShowInMenu(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            UserMustHaveAccessTo(detail);

            SetShowInMenuStatus((MediaDetail)detail, true);
        }

        [WebMethod(EnableSession = true)]
        public void HideFromMenu(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            UserMustHaveAccessTo(detail);

            SetShowInMenuStatus((MediaDetail)detail, false);
        }

        [WebMethod(EnableSession = true)]
        public void MoveUp(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            UserMustHaveAccessTo(detail);

            detail.Media.MoveUp();
        }

        [WebMethod(EnableSession = true)]
        public void MoveDown(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);

            UserMustHaveAccessTo(detail);

            detail.Media.MoveDown();
        }

        [WebMethod(EnableSession = true)]
        public void RenameFileManagerItem(string oldText, string newText, string href)
        {
            var hrefQueryString = System.Web.HttpUtility.ParseQueryString(href);

            var hrefFile =  hrefQueryString["file"];
            var hrefCurrPath = hrefQueryString["currpath"];             
            
            if(!string.IsNullOrEmpty(hrefFile))
            {                                
                var oldFileInfo = new FileInfo(URIHelper.BasePath + hrefFile);

                var newHref = oldFileInfo.Directory.FullName +"\\"+ newText;

                var newFileInfo = new FileInfo(newHref);                

                if(oldFileInfo.Exists && !newFileInfo.Exists)
                {
                    File.Move(oldFileInfo.FullName, newFileInfo.FullName);
                }

            }
            else if(!string.IsNullOrEmpty(hrefCurrPath))
            {
                var absPath = URIHelper.ConvertToAbsPath(hrefCurrPath);
                var oldDirectoryInfo = new DirectoryInfo(absPath);
                
                var newHref = oldDirectoryInfo.Parent.FullName +"\\"+ newText;

                var newDirInfo = new DirectoryInfo(newHref);

                if(oldDirectoryInfo.Exists && !newDirInfo.Exists)
                {
                    Directory.Move(oldDirectoryInfo.FullName, newDirInfo.FullName);
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public void MoveFileManagerItem(string draggedItem, string droppedOn)
        {
            var draggedItemUriSegments = System.Web.HttpUtility.ParseQueryString(draggedItem);
            var droppedOnUriSegments = System.Web.HttpUtility.ParseQueryString(droppedOn);

            var draggedFile =  draggedItemUriSegments["file"];
            var draggedCurrPath = draggedItemUriSegments["currpath"];
            var droppedCurrPath = droppedOnUriSegments["currpath"];
            
            droppedCurrPath = URIHelper.ConvertAbsUrlToTilda(droppedCurrPath).Replace("\\", "/").Replace("//", "/").Replace("~/", "/");

            if (!droppedCurrPath.Contains("/media/uploads"))
            {
                throw new Exception("You cannot move the item to this folder, you can only move it to folders under /media/uploads/");
            }

            var toDirectory = URIHelper.BasePath + droppedCurrPath;                      

            if (draggedFile != "" && draggedFile != null)
            {
                var filePath = URIHelper.BasePath + draggedFile;

                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);

                    File.Move(fileInfo.FullName, toDirectory +"\\"+ fileInfo.Name);
                }
            }
            else if (draggedCurrPath != "" && draggedCurrPath != null)
            {
                var dirPath = URIHelper.BasePath + draggedCurrPath;

                if (Directory.Exists(dirPath))
                {
                    var dirInfo = new DirectoryInfo(dirPath);

                    Directory.Move(dirInfo.FullName, toDirectory +"\\"+ dirInfo.Name);
                }

            }


        }

        [WebMethod(EnableSession = true)]
        public string HandleNodeDragDrop(long sourceMediaId, long parentMediaId, int newPosition)
        {
            var sourceMedia = BaseMapper.GetObjectFromContext(MediasMapper.GetByID(sourceMediaId));
            var parentMedia = BaseMapper.GetObjectFromContext(MediasMapper.GetByID(parentMediaId));

            UserMustHaveAccessTo(sourceMedia.GetLiveMediaDetail());

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

            return returnObj.ToJson();
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

        public IMediaDetail HandleDuplicate(IMediaDetail detail, Media parentMedia, bool duplicateChildren = false, string newName="")
        {
            var duplicatedItem = MediaDetailsMapper.CreateObject(detail.MediaTypeID, null, parentMedia, false);
            duplicatedItem.CopyFrom(detail, new List<string> { "MediaID", "Media" });

            if (newName != "")
            {
                duplicatedItem.LinkTitle = newName;
            }

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
                mediaDetailField.CopyFrom(item);

                if (newName != "" && mediaDetailField.FieldCode == "SectionTitle")
                {
                    mediaDetailField.FieldValue = newName;
                }                                                                

                mediaDetailField.DateCreated = mediaDetailField.DateLastModified = DateTime.Now;

                foreach (var association in item.FieldAssociations)
                {
                    var fieldAssociation = new FieldAssociation();
                    fieldAssociation.CopyFrom(association);

                    if (item.AdminControl.Contains("MultiFile"))
                    {
                        var associatedMediaDetail = MediaDetailsMapper.GetByID(fieldAssociation.AssociatedMediaDetailID);
                        fieldAssociation.AssociatedMediaDetailID = 0;

                        fieldAssociation.MediaDetail = (MediaDetail)HandleDuplicate(associatedMediaDetail, associatedMediaDetail.Media.ParentMedia, true);
                    }

                    mediaDetailField.FieldAssociations.Add(fieldAssociation);
                }

                duplicatedItem.Fields.Add(mediaDetailField);
            }

            var returnObj = MediaDetailsMapper.Insert(duplicatedItem);

            if (returnObj.IsError)
                throw returnObj.Error.Exception;
            else
            {
                if (duplicateChildren)
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
            }

            return duplicatedItem;
        }

        private void HandleDeletePermanently(Media item)
        {
            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.Delete))
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

            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.Delete))
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

        private Exception AccessDeniedException(IMediaDetail detail = null)
        {
            if (detail == null)
            {
                return new Exception("You do not have the appropriate permissions to perform this operation");
            }
            else
            {
                return new Exception($"You do not have the appropriate permissions to perform this operation on '{detail.SectionTitle}'");
            }
        }

        private void UserMustHaveAccessTo(IMediaDetail detail)
        {
            if (FrameworkSettings.CurrentUser == null || !detail.CanUserAccessSection(FrameworkSettings.CurrentUser))
            {
                throw AccessDeniedException(detail);
            }
        }

        private void UserMustBeInRole(RoleEnum roleEnum)
        {
            if (FrameworkSettings.CurrentUser == null || !FrameworkSettings.CurrentUser.IsInRole(roleEnum))
            {
                throw AccessDeniedException();
            }
        }

        private void UserMustHavePermission(PermissionsEnum permissionEnum)
        {
            if (FrameworkSettings.CurrentUser == null || !FrameworkSettings.CurrentUser.HasPermission(permissionEnum))
            {
                throw AccessDeniedException();
            }
        }

        private void SetDeleteStatus(IMediaDetail detail, bool isDeleted)
        {
            UserMustHaveAccessTo(detail);

            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.Delete))
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
            UserMustBeInRole(RoleEnum.Administrator);

            var index = 0;
            foreach (var item in items)
            {
                var mediaField = BaseMapper.GetDataModel().Fields.SingleOrDefault(i => i.ID == item.ID);

                if (mediaField != null)
                {
                    mediaField.OrderIndex = index;

                    index++;
                }

                if(mediaField is MediaTypeField)
                {
                    var mediaTypeField = mediaField as MediaTypeField;

                    foreach (var mediaDetailField in mediaTypeField.MediaDetailFields)
                    {
                        mediaDetailField.OrderIndex = mediaTypeField.OrderIndex;
                    }
                }
            }

            return BaseMapper.GetDataModel().SaveChanges();
        }
    }
}