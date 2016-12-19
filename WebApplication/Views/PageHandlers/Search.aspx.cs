using System;
using FrameworkLibrary;

namespace WebApplication.Views.PageHandlers
{
    public partial class Search : FrontEndBasePage
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