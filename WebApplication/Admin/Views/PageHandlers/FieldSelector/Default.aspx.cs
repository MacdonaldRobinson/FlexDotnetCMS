using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Views.PageHandlers.FieldSelector
{
    public partial class Default : AdminBasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var mediaDetailIdStr = Request["mediaDetailId"];

            if (!string.IsNullOrEmpty(mediaDetailIdStr))
            {                
                var mediaDetailId = long.Parse(mediaDetailIdStr);
                var mediaDetail = MediaDetailsMapper.GetByID(mediaDetailId);

                if (mediaDetail != null)
                {
                    if(mediaDetail.UseMediaTypeLayouts)
                    {
                        MediaTypeFieldsEditorWrapper.Visible = true;
                        MediaTypeFieldsEditor.SetItems(mediaDetail.MediaType);
                    }
                    else
                    {
                        MediaDetailFieldsEditorWrapper.Visible = true;
                        FieldsTab.SetObject(mediaDetail);
                    }
                }
                else
                {
                    MediaDetailFieldsEditorWrapper.Visible = false;
                    MediaDetailFieldsEditorWrapper.Visible = false;
                }
            }
            else
            {
                MediaDetailFieldsEditorWrapper.Visible = false;
                MediaDetailFieldsEditorWrapper.Visible = false;
            }

            BindCommonFields();
        }

        public void BindCommonFields()
        {
            var includeName = MediaTypeEnum.Include.ToString();
            var includes = BaseMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == includeName && i.MediaType.ShowInSiteTree && i.HistoryVersionNumber == 0);

            CommonFields.DataSource = includes.SelectMany(i => i.Fields).ToList();
            CommonFields.DataBind();
        }

        protected void ItemList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            CommonFields.PageIndex = e.NewPageIndex;
            CommonFields.DataBind();
        }
    }
}