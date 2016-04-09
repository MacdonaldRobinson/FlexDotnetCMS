using FrameworkLibrary;
using System;
using System.Web.UI;

namespace WebApplication.Admin
{
    public class AdvanceOptionsBasePage : AdminBasePage
    {
        protected bool canAccessSection = false;

        public AdvanceOptionsBasePage()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            canAccessSection = CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions);

            base.OnInit(e);

            if ((!canAccessSection) && (!IsPostBack))
                DisplayAccessError();

            Control ContentPlaceHolder1 = WebFormHelper.FindControlRecursive(Page.Master, "ContentPlaceHolder1");

            if ((ContentPlaceHolder1 != null) && (!canAccessSection))
                ContentPlaceHolder1.Visible = false;
        }

        protected void DisplayAccessError()
        {
            if (!canAccessSection)
                DisplayErrorMessage("Error accessing section", ErrorHelper.CreateError(new Exception("You do not have the appropriate permissions to access this section")));
        }
    }
}