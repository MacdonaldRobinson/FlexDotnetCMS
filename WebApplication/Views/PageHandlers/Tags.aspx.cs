using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Views.PageHandlers
{
    public partial class Tags : FrontEndBasePage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(Request["names"] == null)
            {
                DisplayErrorMessage("You must specify a names querystring parameter");
                return;
            }

            var tags = new List<Tag>();

            foreach (var item in Request["names"].Split(','))
	        {
		        var tag = TagsMapper.GetByName(item);

                if((tag != null) && (!tags.Contains(tag)))
                    tags.Add(tag);
	        }

            var allByTag = MediaDetailsMapper.FilterByTags(MediaDetailsMapper.GetAllActiveMediaDetails(),tags);

            MediaDetailsList.DataSource = allByTag;
            MediaDetailsList.DataBind();
        }
    }
}