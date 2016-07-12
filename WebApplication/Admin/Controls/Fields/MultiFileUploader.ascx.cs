using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public partial class MultiFileUploader : BaseFieldControl
    {
        private bool hasRun = false;

        public override void RenderControlInAdmin()
        {
            base.RenderControlInAdmin();
        }

        public override void RenderControlInFrontEnd()
        {
            base.RenderControlInFrontEnd();
        }

        public override object GetValue()
        {
            var field = GetField();

            return field.FieldAssociations.OrderBy(i => i.OrderIndex).ToList();
        }

        public DirectoryInfo GetFolderPath()
        {
            var field = GetField();

            var folderPath = SaveToFolder;

            if (folderPath == "")
                folderPath = "~/media/uploads/";

            return new DirectoryInfo(FrameworkLibrary.URIHelper.ConvertToAbsPath(folderPath + "fields/" + field.ID + "/"));
        }

        public override void SetValue(object value)
        {
            var field = GetField();
            var hasDeleted = false;
            var hasReordered = false;            

            if (IsPostBack && MultiFileUpload.HasFiles && !hasRun)
            {
                hasRun = true;

                var index = 0;
                foreach (var file in MultiFileUpload.PostedFiles)
                {
                    var fileInfo = (new System.IO.FileInfo(GetFolderPath() + file.FileName));

                    if (!fileInfo.Directory.Exists)
                        fileInfo.Directory.Create();

                    if (fileInfo.Directory.Exists)
                    {
                        file.SaveAs(fileInfo.FullName);

                        var filePath = URIHelper.ConvertAbsUrlToTilda(URIHelper.ConvertAbsPathToAbsUrl(fileInfo.FullName));

                        if (!field.FieldAssociations.Any(i => i.MediaDetail.PathToFile == filePath))
                        {
                            var fieldAssociation = new FieldAssociation();
                            fieldAssociation.MediaDetail = (MediaDetail)PagesMapper.CreateObject(MediaTypeID, MediasMapper.CreateObject(), AdminBasePage.SelectedMedia);
                            fieldAssociation.MediaDetail.PathToFile = filePath;
                            fieldAssociation.MediaDetail.PublishDate = DateTime.Now;
                            fieldAssociation.MediaDetail.CreatedByUser = fieldAssociation.MediaDetail.LastUpdatedByUser = FrameworkSettings.CurrentUser;
                            fieldAssociation.MediaDetail.CachedVirtualPath = fieldAssociation.MediaDetail.CalculatedVirtualPath();
                            fieldAssociation.MediaDetail.Language = SettingsMapper.GetSettings().DefaultLanguage;

                            field.FieldAssociations.Add(fieldAssociation);

                            index++;

                            var returnObj = BaseMapper.SaveDataModel();
                        }
                    }
                }
            }

            if (ItemsToDelete.Text != "" && ItemsToDelete.Text != "[]")
            {
                var deleteIds = StringHelper.JsonToObject<List<long>>(ItemsToDelete.Text);

                foreach (var id in deleteIds)
                {
                    var fieldAssociation = field.FieldAssociations.SingleOrDefault(i => i.ID == id);

                    if (fieldAssociation != null)
                    {
                        var returnObj = MediaDetailsMapper.DeletePermanently(fieldAssociation.MediaDetail);

                        if (!returnObj.IsError)
                            BaseMapper.DeleteObjectFromContext(fieldAssociation);

                        hasDeleted = true;
                    }
                }
            }

            if (ReorderItems.Text != "" && ReorderItems.Text != "[]")
            {
                var reorderedIds = StringHelper.JsonToObject<List<long>>(ReorderItems.Text);

                var index = 0;
                foreach (var id in reorderedIds)
                {
                    if (id == null)
                        return;

                    var fieldAssociation = field.FieldAssociations.SingleOrDefault(i => i.ID == id);

                    if (fieldAssociation != null)
                    {
                        fieldAssociation.OrderIndex = index;
                        hasReordered = true;

                        index++;
                    }
                }
            }

            if (hasDeleted || hasReordered)
            {
                var returnObj = BaseMapper.SaveDataModel();
            }                


            BindValues();
        }

        private void BindValues()
        {
            UpdatePagerSize();

            switch (Mode)
            {
                case ViewMode.GridView:
                    {
                        MultiItemUploaderPanel.Visible = false;
                        FieldItems.Visible = true;

                        FieldItems.DataSource = GetValue();
                        FieldItems.DataBind();

                        if(((dynamic)FieldItems.DataSource).Count > 0)
                        {
                            SearchPanel.Visible = true;
                        }
                        else
                        {
                            SearchPanel.Visible = false;
                        }

                        break;
                    }                                
                default:
                    {
                        MultiItemUploaderPanel.Visible = true;
                        FieldItems.Visible = false;

                        Values.DataSource = GetValue();
                        //Values.DataBind();

                        if (((dynamic)Values.DataSource).Count > 0)
                        {
                            SearchPanel.Visible = true;
                        }
                        else
                        {
                            SearchPanel.Visible = false;
                        }

                        break;
                    }
            }


            ItemsToDelete.Text = "[]";
            ReorderItems.Text = "[]";            
        }

        public enum ViewMode { Uploader, GridView }

        public string SaveToFolder { get; set; }
        public long MediaTypeID { get; set; }
        public int PageSize { get; set; }
        public ViewMode Mode { get; set; }

        protected void AddItem_Click(object sender, EventArgs e)
        {
            var field = GetField();

            var fieldAssociation = new FieldAssociation();
            fieldAssociation.MediaDetail = (MediaDetail)PagesMapper.CreateObject(MediaTypeID, MediasMapper.CreateObject(), AdminBasePage.SelectedMedia);
            fieldAssociation.MediaDetail.PathToFile = "/media/images/icons/File.jpg";
            fieldAssociation.MediaDetail.PublishDate = DateTime.Now;
            fieldAssociation.MediaDetail.CreatedByUser = fieldAssociation.MediaDetail.LastUpdatedByUser = FrameworkSettings.CurrentUser;
            fieldAssociation.MediaDetail.CachedVirtualPath = fieldAssociation.MediaDetail.CalculatedVirtualPath();
            fieldAssociation.MediaDetail.Language = SettingsMapper.GetSettings().DefaultLanguage;            

            field.FieldAssociations.Add(fieldAssociation);
            var returnObj = BaseMapper.SaveDataModel();

            BindValues();
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            FieldItems.PageIndex = e.NewPageIndex;
            FieldItems.DataBind();
        }

        protected void SearchItems_Click(object sender, EventArgs e)
        {
            UpdatePagerSize();

            switch (Mode)
            {
                case ViewMode.GridView:
                    {
                        if (FieldItems.DataSource != null)
                        {
                            var items = ((List<FieldAssociation>)FieldItems.DataSource).Where(i => i.MediaDetail.SectionTitle.Contains(SearchText.Text)).ToList();

                            FieldItems.DataSource = items;
                            FieldItems.DataBind();
                        }

                        break;
                    }
                default:
                    {
                        if (Values.DataSource != null)
                        {
                            var items = ((List<FieldAssociation>)Values.DataSource).Where(i => i.MediaDetail.SectionTitle.Contains(SearchText.Text)).ToList();

                            Values.DataSource = items;
                            Values.DataBind();
                        }
                        break;
                    }
            }
        }

        private void UpdatePagerSize()
        {
            if (PageSize > 0)
            {
                Pager.PageSize = PageSize;
                FieldItems.PageSize = PageSize;
            }
        }
    }
}