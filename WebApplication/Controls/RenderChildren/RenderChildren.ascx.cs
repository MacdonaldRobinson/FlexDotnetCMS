using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.RenderChildren
{
    public partial class RenderChildren : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            IEnumerable<IMediaDetail> children = new List<IMediaDetail>();

            try
            {
                IMediaDetail mediaDetail = null;

                if (MediaDetailID != null)
                {
                    mediaDetail = MediaDetailsMapper.GetByID((long)MediaDetailID);
                }

                if (mediaDetail == null)
                {
                    mediaDetail = BasePage.CurrentMediaDetail;
                }

                if (!string.IsNullOrEmpty(Where))
                    children = mediaDetail.ChildMediaDetails.Where(Where);
                else
                    children = mediaDetail.ChildMediaDetails;

                if (!string.IsNullOrEmpty(OrderBy))
                    children = children.OrderBy(OrderBy);

                if (Take > 0)
                    children = children.Take(Take);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);
            }

            Children.DataSource = children.ToList();

            Pager.ShowDataPager = ShowPager;

            if (PageSize == null)
                PageSize = Take;

            if (!ShowPager)
                PageSize = Take;

            if (PageSize is int)
                Pager.PageSize = int.Parse(PageSize.ToString());
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Children.DataBind();
        }

        public string ChildPropertyName { get; set; }

        public int? PageSize { get; set; }

        public string Where { get; set; }

        public string OrderBy { get; set; }

        public long? MediaDetailID { get; set; }

        public bool ShowPager { get; set; }

        public int Take { get; set; }

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }

        protected void Children_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var dataItem = (IMediaDetail)e.Item.DataItem;
            var layout = (Literal)e.Item.FindControl("Layout");                        

            layout.Text = MediaDetailsMapper.ParseSpecialTags(dataItem, "{" + ChildPropertyName + "}");
        }
    }
}