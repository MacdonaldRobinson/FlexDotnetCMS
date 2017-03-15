using FrameworkLibrary;
using Newtonsoft.Json;
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

            var draft = detail.History.FirstOrDefault(i => i.IsDraft && i.LanguageID == detail.LanguageID);

            if (draft != null)
            {
                node.li_attr._class += " hasDraft";
                nodeText += $"<small class='hasDraftWrapper'>Has Draft</small>";

                if (draft.DateLastModified > detail.DateLastModified)
                {
                    node.li_attr._class += " draftIsNewer";
                }
            }

            var pendingComments = detail.Media.Comments.Count(i => i.Status == StatusEnum.Pending.ToString() && i.LanguageID == detail.LanguageID);

            if (pendingComments > 0)
            {
                node.li_attr._class += " hasPendingComments";
                nodeText += $"<small class='hasPendingCommentsWrapper'>Pending(" + pendingComments + ")</small>";
            }

            node.text = nodeText;

            //node.LinkAttributes.Add("data-frontendurl", detail.Media.PermaLink);
            node.a_attr.frontendurl = detail.AbsoluteUrl;
            node.li_attr.mediaDetailId = detail.ID.ToString();

            node.a_attr.href = URIHelper.ConvertToAbsUrl(WebApplication.BasePage.GetRedirectToMediaDetailUrl(detail.MediaTypeID, detail.MediaID));

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
            var foundItems = MediasMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.ShowInSiteTree &&
                                                                            i.HistoryVersionNumber == 0 &&
                                                                            i.LanguageID == AdminBasePage.CurrentLanguage.ID &&
                                                                            (i.MediaID.ToString() == filterText ||
                                                                                i.MainContent.ToLower().Contains(filterText) ||
                                                                                i.ShortDescription.ToLower().Contains(filterText) ||
                                                                                i.SectionTitle.ToLower().Contains(filterText) ||
                                                                                i.MainLayout.ToLower().Contains(filterText) ||
                                                                                i.MediaType.MainLayout.ToLower().Contains(filterText) ||
                                                                                i.Fields.Any(j => (j.FieldAssociations.Count == 0 && j.FieldValue.ToLower().Contains(filterText)) ||
                                                                                                j.FieldAssociations.Any(k => !k.MediaDetail.MediaType.ShowInSiteTree &&
                                                                                                                            (k.MediaDetail.SectionTitle.ToLower().Contains(filterText) ||
                                                                                                                            k.MediaDetail.MainContent.ToLower().Contains(filterText) ||
                                                                                                                            k.MediaDetail.MainLayout.ToLower().Contains(filterText))
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
                    if(i.RolesMediaDetails.Count > 0)
                    {
                        return FrameworkSettings.CurrentUser.IsInRoles(i.RolesMediaDetails.Select(j => j.Role));
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
        public void ClearCache(long id)
        {
            var detail = MediaDetailsMapper.GetByID(id);
            detail.RemoveFromCache();
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
        public string HandleNodeDragDrop(long sourceMediaId, long parentMediaId, int newPosition)
        {
            var sourceMedia = BaseMapper.GetObjectFromContext(MediasMapper.GetByID(sourceMediaId));
            var parentMedia = BaseMapper.GetObjectFromContext(MediasMapper.GetByID(parentMediaId));

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
                mediaDetailField.CopyFrom(item);

                mediaDetailField.DateCreated = mediaDetailField.DateLastModified = DateTime.Now;

                foreach (var association in item.FieldAssociations)
                {
                    var fieldAssociation = new FieldAssociation();
                    fieldAssociation.CopyFrom(association);

                    if (item.AdminControl.Contains("MultiFile"))
                    {
                        var associatedMediaDetail = MediaDetailsMapper.GetByID(fieldAssociation.AssociatedMediaDetailID);
                        fieldAssociation.AssociatedMediaDetailID = 0;

                        fieldAssociation.MediaDetail = (MediaDetail)HandleDuplicate(associatedMediaDetail, associatedMediaDetail.Media.ParentMedia);
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