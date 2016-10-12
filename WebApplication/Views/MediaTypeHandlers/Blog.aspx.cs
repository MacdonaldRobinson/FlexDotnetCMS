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
            //DynamicContent.Controls.Add(this.ParseControl(MediaDetailsMapper.ParseSpecialTags(CurrentMediaDetail)));

            var blogCategoriesMediaTypeId = 26;
            var blogPostMediaTypeId = 24;

            BlogCategories.DataSource = MediaDetailsMapper.GetAllActiveByMediaType(blogCategoriesMediaTypeId).ToList();
            BlogCategories.DataBind();

            IEnumerable<IMediaDetail> blogPosts = new List<IMediaDetail>();

            if(CurrentMediaDetail.MediaType.ID == blogCategoriesMediaTypeId)
            {
                blogPosts = CurrentMediaDetail.ChildMediaDetails.OrderByDescending(i=>i.DateCreated).ToList();
            }
            else
            {
                blogPosts = MediaDetailsMapper.GetAllActiveByMediaType(blogPostMediaTypeId).OrderByDescending(i=>i.DateCreated).ToList();
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