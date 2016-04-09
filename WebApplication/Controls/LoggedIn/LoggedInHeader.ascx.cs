using FrameworkLibrary;
using System;

namespace WebApplication.Controls.OnLogin
{
    public partial class LoggedInHeader : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this.BasePage.CurrentUser != null)
            {
                if (this.BasePage.CurrentUser.HasPermission(PermissionsEnum.AccessCMS))
                    AccessCMSPermissionsPanel.Visible = true;

                LoggedInHeaderPanel.Visible = true;

                QuickEditCurrentPage.NavigateUrl = WebApplication.BasePage.GetRedirectToMediaDetailUrl(BasePage.CurrentMediaDetail.MediaTypeID, BasePage.CurrentMediaDetail.MediaID) + "&masterFilePath=~/Admin/Views/MasterPages/Popup.Master";
            }
        }

        protected void EditCurrentPage_OnClick(object sender, EventArgs e)
        {
            WebApplication.BasePage.RedirectToMediaDetail(this.BasePage.CurrentMediaDetail);
        }

        private FrontEndBasePage BasePage
        {
            get
            {
                return (FrontEndBasePage)this.Page;
            }
        }
    }
}