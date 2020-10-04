using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Admin.Views.PageHandlers.FileEditor
{
    public partial class Default : AdminBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if(!IsPostBack && Request["LoadFile"] != null)
            {
                FileSelector.SetValue(Request["LoadFile"].ToString());
                LoadFile();
            }
        }

        protected void LoadFromFile_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void LoadFile()
        {
            var relPathToFile = FileSelector.GetValue().ToString();
            var absPathToFile = URIHelper.ConvertToAbsPath(relPathToFile);
            var fileManagerConfig = BasePage.GetFileManagerConfig();
            
            var extention = BasePage.GetExtentionFromString(relPathToFile);

            if(!absPathToFile.Contains(fileManagerConfig.strDocRoot))
            {
                DisplaySuccessMessage($@"You cannot load files from outside the web root directory ( {absPathToFile} ) ");
            }
            else if (!fileManagerConfig.arrAllowedFileExtensions.Contains(extention))
            {
                DisplaySuccessMessage($@"You cannot load files with the extention ( {extention} ) ");
            }
            else if (!File.Exists(absPathToFile))
            {
                DisplaySuccessMessage($@"The file does not exist ( {absPathToFile} ) ");
            }
            else if (File.Exists(absPathToFile))
            {
                var fileContent = File.ReadAllText(absPathToFile);
                Editor.Text = fileContent;

                DisplaySuccessMessage($@"Successfully loaded file ( {absPathToFile} ) ");
            }
            else
            {
                DisplayErrorMessage($"Error Loading the file ( {absPathToFile} ) ");
            }            
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            var relPathToFile = FileSelector.GetValue().ToString();
            var absPathToFile = URIHelper.ConvertToAbsPath(relPathToFile);

            try
            {
                File.WriteAllText(absPathToFile, Editor.Text);

                DisplaySuccessMessage($@"Successfully saved ( {absPathToFile} )");
            }
            catch(Exception ex)
            {
                ErrorHelper.LogException(ex);

                DisplayErrorMessage("Error saving", ErrorHelper.CreateError(ex));
            }
        }
    }
}