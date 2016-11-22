using FrameworkLibrary;
using System;

namespace WebApplication.Views.PageHandlers
{
    public partial class SiteMap : FrontEndBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseWithTemplate(CurrentMediaDetail)));
        }
    }
}