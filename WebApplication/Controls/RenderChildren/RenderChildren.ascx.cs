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

                if (MediaID != null)
                {
                    mediaDetail = MediasMapper.GetByID((long)MediaID)?.GetLiveMediaDetail();
                }

                if (mediaDetail == null)
                {
                    mediaDetail = BasePage.CurrentMediaDetail;
                }

                if(mediaDetail != null)
                {
                    if (!string.IsNullOrEmpty(Where))
                        children = mediaDetail.ChildMediaDetails.Where(Where);
                    else
                        children = mediaDetail.ChildMediaDetails;

                    if (!string.IsNullOrEmpty(OrderBy))
                    {
                        children = children.OrderBy(OrderBy);
                    }                        

                    if (Take > 0)
                        children = children.Take(Take);
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);
            }

            Children.DataSource = children.ToList();

            Pager.ShowDataPager = ShowPager;

            if (PageSize == null)
                PageSize = 0;

            if (!ShowPager)
            {
                PageSize = 0;
                //Pager.Visible = false;
                //Pager.ShowDataPager = false;
                Children.DataBind();
            }

            if(PageSize == 0)
            {
                PageSize = children.Count();
            }

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

        public long? MediaID { get; set; }

        public bool ShowPager { get; set; }

        public int Take { get; set; }

        private Dictionary<string, string> _arguments { get; set; }
        
        public string Arguments
        {
            set
            {
                _arguments = StringHelper.JsonToObject<Dictionary<string, string>>(value);

                foreach (var item in _arguments)
                {
                    ParserHelper.SetValue(this, item.Key, item.Value);
                }                
            }
        }

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