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

        public new FrameworkLibrary.Page CurrentMediaDetail
        {
            get { return (FrameworkLibrary.Page)base.CurrentMediaDetail; }
        }
    }
}