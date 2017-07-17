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
        }
    }
}