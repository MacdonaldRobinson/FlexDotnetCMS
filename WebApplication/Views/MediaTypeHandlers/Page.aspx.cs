using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Views.PageTypes
{
    public partial class Static : FrontEndBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            EmailHelper.SendDirectMessage("it@zgm.ca", "macdonald.robinson@zgm.ca","Test Email","<h1>test</h1><p>test</p>");

            DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseWithTemplate(CurrentMediaDetail)));
        }

        public new FrameworkLibrary.Page CurrentMediaDetail
        {
            get { return (FrameworkLibrary.Page)base.CurrentMediaDetail; }
        }
    }
}