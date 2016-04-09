using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Selectors.Generic
{
    public partial class MediaTypeSelector : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Bind(MediaTypesMapper.GetAllActive());
        }

        private void Bind(IEnumerable<MediaType> mediaTypes)
        {
            MediaTypes.DataSource = mediaTypes.ToList();
            MediaTypes.DataTextField = "Label";
            MediaTypes.DataValueField = "ID";
            MediaTypes.DataBind();
        }

        public void SetMediaTypes(IEnumerable<MediaType> mediaTypes)
        {
            Bind(mediaTypes);
        }

        public MediaType GetSelectedMediaType()
        {
            return MediaTypesMapper.GetByID(long.Parse(MediaTypes.SelectedValue));
        }

        public DropDownList MediaTypesDropDownList
        {
            get
            {
                return MediaTypes;
            }
        }
    }
}