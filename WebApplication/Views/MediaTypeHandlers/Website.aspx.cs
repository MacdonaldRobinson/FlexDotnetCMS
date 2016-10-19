﻿using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Views.MediaTypeHandlers
{
    public partial class Website : FrontEndBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseSpecialTags(CurrentMediaDetail)));
        }

        public new FrameworkLibrary.Website CurrentMediaDetail
        {
            get { return (FrameworkLibrary.Website)base.CurrentMediaDetail; }
        }
    }
}