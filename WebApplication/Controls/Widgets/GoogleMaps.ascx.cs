using System;
using System.Collections.Generic;

namespace WebApplication.Controls.Widgets
{
    public partial class GoogleMaps : System.Web.UI.UserControl
    {
        public GoogleMaps()
        {
            AddressesWithInfoWindowContent = new Dictionary<string, string>();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        public Dictionary<string, string> AddressesWithInfoWindowContent { get; set; }
    }
}