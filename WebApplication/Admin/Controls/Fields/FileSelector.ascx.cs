using FrameworkLibrary;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public partial class FileSelector : BaseFieldControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            SelectedFile.Attributes["data-imgid"] = SelectedImage.ClientID;
        }

        private bool CanSetSelectedImage(string value)
        {
            if (value == null)
                return false;

            value = URIHelper.ConvertAbsUrlToTilda(value.ToLower());

            if (value.EndsWith(".jpg") || value.EndsWith(".png") || value.EndsWith(".gif"))
            {
                if (File.Exists(URIHelper.ConvertToAbsPath(value)))
                {
                    SelectedImage.Visible = true;
                    return true;
                }
                else
                    SelectedImage.Visible = false;
            }
            else
                SelectedImage.Visible = false;

            return false;
        }

        public override void RenderControlInAdmin()
        {
            base.RenderControlInAdmin();
        }

        private string _dirPath;
        public string DirPath
        {
            get
            {
                _dirPath = _dirPath.Replace("~", "");
                return _dirPath;
            }
            set
            {
                _dirPath = value;
            }
        }

        public override void RenderControlInFrontEnd()
        {
            base.RenderControlInFrontEnd();
        }

        public override object GetValue()
        {
            CanSetSelectedImage(SelectedFile.Text);

            if (SelectedFile.Text == "")
                return "";

            if (SelectedFile.Text.Contains("#"))
                return SelectedFile.Text;

            return URIHelper.ConvertToAbsUrl(SelectedFile.Text);
        }

        public override void SetValue(object value)
        {
            if (value != null)
            {
                var valAsString = value.ToString();

                if (!string.IsNullOrEmpty(valAsString) && valAsString.Contains(URIHelper.BaseUrl))
                {
                    valAsString = URIHelper.ConvertToAbsUrl(valAsString);
                    value = valAsString;
                }

                SelectedFile.Text = value.ToString();

                if (!string.IsNullOrEmpty(SelectedFile.Text))
                {
                    try
                    {
                        SelectedFile.Text = ParserHelper.ParseData(SelectedFile.Text, BasePage.GetDefaultTemplateVars(""));
                        DirPath = URIHelper.ConvertAbsUrlToTilda(URIHelper.ConvertAbsPathToAbsUrl(new FileInfo(URIHelper.ConvertToAbsPath(URIHelper.ConvertAbsUrlToTilda(SelectedFile.Text))).DirectoryName)).Replace("~", "");
                    }
                    catch(Exception ex)
                    {                        
                    }
                }

                if (CanSetSelectedImage(SelectedFile.Text))
                    SelectedImage.ImageUrl = SelectedFile.Text+ "?width=300&mode=max";
                else
                    return;
            }
        }
    }
}