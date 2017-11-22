using System;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Generic
{
    public partial class Pager : System.Web.UI.UserControl
    {
        private ListView list = null;

        public void Page_Load(object sender, EventArgs e)
        {
            if (list == null)
                list = ((ListView)this.Parent.FindControl(DataPager.PagedControlID));

            if (list != null)
            {
                list.PagePropertiesChanging += list_PagePropertiesChanging;
                //list.PagePropertiesChanged += new EventHandler(list_PagePropertiesChanged);
                list.DataBound += new EventHandler(list_DataBound);
            }
        }

        private void list_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            dataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            if (PageSize > 0)
                dataPager.PageSize = PageSize;

            list.DataBind();
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            if (PageSize > 0)
                dataPager.PageSize = PageSize;

            list.DataBind();
        }

        /*public void list_PagePropertiesChanged(object sender, EventArgs e)
        {
            dataPager.PageSize = PageSize;
            list.DataBind();
        }*/

        public void list_DataBound(object sender, EventArgs e)
        {
            if (DataPager.TotalRowCount <= DataPager.PageSize)
            {
                PagerControl.Visible = false;
                DataPager.Visible = false;
            }
            else
            {
                if (ShowDataPager)
                    DataPager.Visible = true;
            }
        }

        public DataPager DataPager
        {
            get
            {
                return this.dataPager;
            }
        }

        public bool ShowDataPager { get; set; }

        public int PageSize
        {
            get;
            set;
        }

        public string PagedControlID
        {
            get
            {
                return this.dataPager.PagedControlID;
            }
            set
            {
                this.dataPager.PagedControlID = value;
            }
        }
    }
}