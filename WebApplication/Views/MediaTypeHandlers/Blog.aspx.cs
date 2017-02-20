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

            var blogCategoriesMediaTypeId = 50;
            var blogPostMediaTypeId = 24;

            BlogCategories.DataSource = MediaDetailsMapper.GetAllActiveByMediaType(blogCategoriesMediaTypeId).OrderBy(i=>i.Media.OrderIndex).ToList();
            BlogCategories.DataBind();

            IEnumerable<IMediaDetail> blogPosts = new List<IMediaDetail>();

            if(CurrentMediaDetail.MediaType.ID == blogCategoriesMediaTypeId)
            {
                blogPosts = CurrentMediaDetail.ChildMediaDetails.Where(i=>i.HistoryVersionNumber == 0 && i.LanguageID == CurrentLanguage.ID).OrderByDescending(i => (i.PublishDate == null)? DateTime.Parse(i.Fields.FirstOrDefault(j=>j.FieldCode == "ArticlePublishDate").FieldValue) : i.PublishDate).ToList();
            }
            else
            {
                blogPosts = MediaDetailsMapper.GetAllActiveByMediaType(blogPostMediaTypeId).Where(i => i.HistoryVersionNumber == 0 && i.LanguageID == CurrentLanguage.ID).OrderByDescending(i=> (i.PublishDate == null) ? DateTime.Parse(i.Fields.FirstOrDefault(j => j.FieldCode == "ArticlePublishDate").FieldValue) : i.PublishDate).ToList();
            }

            BlogPosts.DataSource = blogPosts;
            BlogPosts.DataBind();
        }

        public new FrameworkLibrary.Page CurrentMediaDetail
        {
            get { return (FrameworkLibrary.Page)base.CurrentMediaDetail; }
        }
    }
}