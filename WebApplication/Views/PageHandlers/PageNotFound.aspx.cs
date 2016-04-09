using FrameworkLibrary;
using System;

namespace WebApplication.Views
{
    public partial class PageNotFound : FrontEndBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseSpecialTags(CurrentMediaDetail)));

            Return returnObj = BaseMapper.GenerateReturn();

            if (Request["requestVirtualPath"] == null)
            {
                returnObj = BaseMapper.GenerateReturn("You must specify a valid 'requestVirtualPath' query string parameter", "");

                DisplayErrorMessage("Error", returnObj.Error);
                return;
            }

            string requestVirtualPath = Request["requestVirtualPath"];

            var ex = new Exception("Page Not found: ( " + requestVirtualPath + " )");

            ErrorHelper.LogException(ex);
        }
    }
}