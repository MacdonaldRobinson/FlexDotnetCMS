using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Views.MediaTypeHandlers
{
    public partial class Blog : FrontEndBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var templateTopAndBottomSegments = CurrentMediaDetail.GetTemplateTopAndBottomSegments(this);

            if(templateTopAndBottomSegments.Count > 1)
            {
                TemplateTopSegment.Controls.Add(templateTopAndBottomSegments.ElementAt(0));
                TemplateBottomSegment.Controls.Add(templateTopAndBottomSegments.ElementAt(1));
            }

            var blogPosts = BaseMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == "BlogPost" && i.HistoryVersionNumber == 0);
            var categories = BaseMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == "BlogCategory" && i.HistoryVersionNumber == 0);
            
            BlogCategories.DataSource = categories.ToList().Where(i => i.CanRender).ToList();
            BlogCategories.DataBind();
            
            if(CurrentMediaDetail.MediaType.Name == "BlogCategory")
            {
                blogPosts = blogPosts.Where(i => i.Media.ParentMediaID == CurrentMediaDetail.MediaID);
            }

            BlogPosts.DataSource = blogPosts.ToList().Where(i => i.CanRender).OrderByDescending(i => i.PublishDate).ToList();
            BlogPosts.DataBind();
        }

        public new FrameworkLibrary.Page CurrentMediaDetail
        {
            get { return (FrameworkLibrary.Page)base.CurrentMediaDetail; }
        }
    }
}