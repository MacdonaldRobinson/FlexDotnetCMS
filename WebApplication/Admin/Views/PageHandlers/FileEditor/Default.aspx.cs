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
            if(Request["LoadFile"] != null)
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

            if (File.Exists(absPathToFile))
            {
                var fileContent = File.ReadAllText(absPathToFile);
                Editor.Text = fileContent;

                DisplaySuccessMessage($"Successfully loaded file ( {absPathToFile} ) ");
            }
            else
            {
                DisplayErrorMessage($"File does not exist ( {absPathToFile} ) ");
            }            
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            var relPathToFile = FileSelector.GetValue().ToString();
            var absPathToFile = URIHelper.ConvertToAbsPath(relPathToFile);

            try
            {
                File.WriteAllText(absPathToFile, Editor.Text);

                DisplaySuccessMessage($"Successfully saved ( {absPathToFile} )");
            }
            catch(Exception ex)
            {
                ErrorHelper.LogException(ex);

                DisplayErrorMessage("Error saving", ErrorHelper.CreateError(ex));
            }
        }
    }
}