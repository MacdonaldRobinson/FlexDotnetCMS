using FrameworkLibrary;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using WebApplication.Admin;

namespace WebApplication.Controls
{
    public partial class LanguageSwitcher : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DataBind();
        }

        private void DataBind()
        {
            LanguageSelect.DataSource = LanguagesMapper.GetAllActive().ToList();
            LanguageSelect.DataTextField = "Name";
            LanguageSelect.DataValueField = "ID";
            LanguageSelect.DataBind();

            var currentLanguage = AdminBasePage.CurrentLanguage;

            if (currentLanguage != null)
                LanguageSelect.Items.FindByValue(currentLanguage.ID.ToString()).Selected = true;
        }

        public DropDownList ComboBox
        {
            get
            {
                return LanguageSelect;
            }
        }

        protected void LanguageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LanguageSelect.SelectedValue))
                return;

            AdminBasePage.CurrentLanguage = LanguagesMapper.GetByID(long.Parse(LanguageSelect.SelectedValue));

            Response.Redirect(Request.Url.PathAndQuery);
        }
    }
}