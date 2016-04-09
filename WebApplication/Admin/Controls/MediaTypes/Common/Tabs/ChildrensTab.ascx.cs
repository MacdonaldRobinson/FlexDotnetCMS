using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class ChildrensTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;

            Bind();
        }        

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            ItemList.DataSource = selectedItem.ChildMediaDetails.ToList();
            ItemList.DataBind();
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ItemList.PageIndex = e.NewPageIndex;
            ItemList.DataBind();
        }

        public void UpdateObjectFromFields()
        {

        }

        public void UpdateFieldsFromObject()
        {

        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (id != "")
            {
                var mediaDetail = MediaDetailsMapper.GetByID(long.Parse(id));

                if (mediaDetail != null)
                {
                    var url = AdminBasePage.GetRedirectToMediaDetailUrl(mediaDetail.MediaTypeID, mediaDetail.MediaID);

                    Response.Redirect(url);
                }
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            if (id != "")
            {
                var mediaDetail = MediaDetailsMapper.GetByID(long.Parse(id));

                if (mediaDetail != null)
                {
                    var returnObj = MediaDetailsMapper.DeletePermanently((MediaDetail)mediaDetail);

                    if(returnObj.IsError)
                    {
                        BasePage.DisplayErrorMessage("Error", returnObj.Error);
                    }
                    else
                    {
                        BasePage.DisplaySuccessMessage("Successfully deleted item");
                    }
                }
            }
        }

        protected void ItemList_DataBound(object sender, EventArgs e)
        {
            ItemList.UseAccessibleHeader = true;
            if (ItemList.HeaderRow != null)
            {
                ItemList.HeaderRow.TableSection = TableRowSection.TableHeader;                
            }
        }
    }
}