using FrameworkLibrary;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Generic
{
    public partial class MediaDetailsGrid : System.Web.UI.UserControl
    {
        private IEnumerable<IMediaDetail> items;

        public void SetItems(IEnumerable<IMediaDetail> items)
        {
            this.items = items;
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }

        protected void Edit_Click(object sender, System.EventArgs e)
        {
            var commandArgument = ((LinkButton)sender).CommandArgument;

            if (string.IsNullOrEmpty(commandArgument))
                return;

            var item = MediaDetailsMapper.GetByID(long.Parse(commandArgument));

            if (item == null)
                return;

            WebApplication.BasePage.RedirectToAdminUrl(item);
        }
    }
}