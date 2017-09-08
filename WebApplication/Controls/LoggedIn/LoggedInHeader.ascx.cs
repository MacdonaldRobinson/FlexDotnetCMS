using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Controls.OnLogin
{
    public partial class LoggedInHeader : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(Request["VisualLayoutEditor"] != null && Request["VisualLayoutEditor"] == "true")
            {
                VisualLayoutEditor.Visible = true;
            }

            /*if (this.BasePage.CurrentUser != null)
            {
                if (this.BasePage.CurrentUser.HasPermission(PermissionsEnum.AccessCMS))
                    AccessCMSPermissionsPanel.Visible = true;

                LoggedInHeaderPanel.Visible = true;

                if(!this.BasePage.CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions))
                {
                    AdminPanel.Visible = false;
                }

                //QuickEditCurrentPage.NavigateUrl = WebApplication.BasePage.GetRedirectToMediaDetailUrl(BasePage.CurrentMediaDetail.MediaTypeID, BasePage.CurrentMediaDetail.MediaID) + "&masterFilePath=~/Admin/Views/MasterPages/Popup.Master";
                //EditCurrentPage.NavigateUrl = WebApplication.BasePage.GetRedirectToMediaDetailUrl(BasePage.CurrentMediaDetail.MediaTypeID, BasePage.CurrentMediaDetail.MediaID);
            }
            else
            {
                LoggedInHeaderPanel.Visible = false;
            }*/
        }

        /*protected void EditCurrentPage_OnClick(object sender, EventArgs e)
        {
            WebApplication.BasePage.RedirectToAdminUrl(this.BasePage.CurrentMediaDetail);
        }*/

        public string CurrentMediaDetailAdminUrl
        {
            get
            {
                return WebApplication.BasePage.GetAdminUrl(BasePage.CurrentMediaDetail.MediaTypeID, BasePage.CurrentMediaDetail.MediaID);
            }
        }        


        public bool CanAccessVisualLayoutEditor
        {
            get
            {
                if (BasePage.CurrentUser == null)
                    return false;

                if(BasePage.CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions))
                {
                    return true;
                }

                return false;
            }
        }

        public FrontEndBasePage BasePage
        {
            get
            {
                return (FrontEndBasePage)this.Page;
            }
        }
    }
}