using FrameworkLibrary;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class FlickerPhotoSetSelector : System.Web.UI.UserControl
    {
        private string userId = "";

        protected void Page_Init(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            FlickerHelper.Instance.SetFlickerAPIKey(AppSettings.FlickerAPIKey);
            FlickerHelper.Instance.SetReturnFormat("rest");
            XmlDocument returnObj = FlickerHelper.Instance.RequestXML("flickr.photosets.getList", AppSettings.FlickerUserID);

            FlickerPhotoSets.DataSource = returnObj.SelectSingleNode("rsp/photosets").ChildNodes;
            FlickerPhotoSets.DataBind();
        }

        protected void FlickerPhotoSets_OnItemDataBound(object sender, EventArgs e)
        {
            //if (e.Item.DataItem == null)
            //    return;

            //XmlNode dataItem = (XmlNode)e.Item.DataItem;

            //e.Item.Text = dataItem.SelectSingleNode("title").InnerText;
            //e.Item.Value = dataItem.Attributes["id"].Value;
        }

        public string SelectedValue
        {
            get
            {
                return FlickerPhotoSets.SelectedValue;
            }
            set
            {
                FlickerPhotoSets.SelectedValue = value;
            }
        }
    }
}