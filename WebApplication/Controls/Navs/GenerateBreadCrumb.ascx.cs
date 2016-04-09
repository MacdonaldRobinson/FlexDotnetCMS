using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Controls
{
    public partial class GenerateBreadCrumb : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.BasePage.CurrentMediaDetail == null)
                return;

            var items = MediaDetailsMapper.GetAllParentMediaDetails((MediaDetail)this.BasePage.CurrentMediaDetail, this.BasePage.CurrentLanguage);

            if (items.Any())
                GenerateNav.BindItems(items);
        }

        private BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}