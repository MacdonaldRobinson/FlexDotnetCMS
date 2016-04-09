using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class MultiMediaTypeSelector : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (MediaTypes == null)
                MediaTypes = new List<MediaType>();

            Bind();
        }

        private List<MediaType> MediaTypes
        {
            get
            {
                return (List<MediaType>)Session["MultiMediaTypeSelector"];
            }
            set
            {
                Session["MultiMediaTypeSelector"] = value;
            }
        }

        public IEnumerable<MediaType> GetSelectedMediaTypes()
        {
            return MediaTypes;
        }

        public void SetMediaTypes(List<MediaType> MediaTypes)
        {
            this.MediaTypes = MediaTypes;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Bind();
        }

        private void Bind()
        {
            this.ItemList.DataSource = MediaTypes;
            this.ItemList.DataBind();
        }

        protected void Add_OnClick(object sender, EventArgs e)
        {
            MediaType addMediaType = MediaTypeSelector.GetSelectedMediaType();

            if (MediaTypes.Where(i => i.ID == addMediaType.ID).Count() == 0)
                MediaTypes.Add(addMediaType);
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        protected void ItemList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            var sortDirection = ((e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending) ? "ASC" : "DESC");
            MediaTypes = MediaTypes.OrderBy(e.SortExpression + " " + sortDirection).ToList();
            Bind();
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (!string.IsNullOrEmpty(id))
            {
                var itemId = long.Parse(id);

                MediaType selectedMediaType = MediaTypes.Where(i => i.ID == itemId).SingleOrDefault();

                if (selectedMediaType != null)
                    MediaTypes.Remove(selectedMediaType);
            }
        }
    }
}