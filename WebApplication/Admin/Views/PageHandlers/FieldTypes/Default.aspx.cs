using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrameworkLibrary;

namespace WebApplication.Admin.Views.PageHandlers.FieldTypes
{
    public partial class Default : AdminBasePage
    {
        private List<FieldType> Items
        {
            get
            {
                return (List<FieldType>)ContextHelper.GetFromRequestContext(ItemList.ClientID);
            }
            set
            {
                ContextHelper.SetToRequestContext(ItemList.ClientID, value);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Items == null)
                Items = BaseMapper.GetDataModel().FieldTypes.OrderBy(i=>i.Name).ToList();

            Bind();
        }

        private void Bind()
        {
            ItemList.DataSource = Items;
            ItemList.DataBind();
        }

        protected void ItemList_DataBound(object sender, EventArgs e)
        {
            ItemList.UseAccessibleHeader = true;
            if (ItemList.HeaderRow != null)
            {
                ItemList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        protected void ItemList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            var sortDirection = ((e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending) ? "ASC" : "DESC");
            Items = Items.OrderBy(e.SortExpression + " " + sortDirection).ToList();
            Bind();
        }
    }
}