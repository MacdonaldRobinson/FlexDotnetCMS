using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            return field.FieldFiles.OrderBy(i => i.OrderIndex);
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

                        if (!field.FieldFiles.Any(i => i.PathToFile == filePath))
                        {
                            field.FieldFiles.Add(new FieldFile { Name = file.FileName, Description="", PathToFile = filePath, DateCreated = DateTime.Now, DateLastModified = DateTime.Now, OrderIndex = index });

                            index++;
                        }
                    }
                }
            }

            if (FilesToDelete.Text != "" && FilesToDelete.Text != "[]")
            {
                var deleteIds = StringHelper.JsonToObject(FilesToDelete.Text);

                foreach (var id in deleteIds)
                {
                    var fieldFile = field.FieldFiles.SingleOrDefault(i => i.ID == long.Parse(id));

                    if (fieldFile != null)
                    {
                        /*var absPathToFile = URIHelper.ConvertToAbsPath(fieldFile.PathToFile);

                        if (File.Exists(absPathToFile))
                        {
                            File.Delete(absPathToFile);
                        }*/

                        BaseMapper.DeleteObjectFromContext(fieldFile);
                        //field.FieldFiles.Remove(fieldFile);

                        hasDeleted = true;
                    }
                }
            }

            if (ReorderFiles.Text != "" && ReorderFiles.Text != "[]")
            {
                var reorderedIds = StringHelper.JsonToObject(ReorderFiles.Text);
                
                var index = 0;
                foreach (var id in reorderedIds)
                {
                    if (id == null)
                        return;

                    var fieldFile = field.FieldFiles.SingleOrDefault(i => i.ID == long.Parse(id));

                    if (fieldFile != null)
                    {
                        fieldFile.OrderIndex = index;
                        hasReordered = true;

                        index++;
                    }
                }
            }

            if (hasDeleted || hasReordered)
                BaseMapper.SaveDataModel();

            Values.DataSource = GetValue();
            Values.DataBind();

            FilesToDelete.Text = "[]";
            ReorderFiles.Text = "[]";
        }

        public string SaveToFolder { get; set; }
    }
}