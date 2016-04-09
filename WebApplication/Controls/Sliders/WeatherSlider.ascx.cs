using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Sliders
{
    public partial class WeatherSlider : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SetCities(IEnumerable<ICity> cities)
        {
            Cities.DataSource = cities;
            Cities.DataBind();
        }

        protected void Cities_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var dataItem = (ICity)e.Item.DataItem;
            var weatherWidget = (WebApplication.Controls.WeatherWidget)e.Item.FindControl("WeatherWidget");

            weatherWidget.CityAndCountry = dataItem.Title + ", " + dataItem.Province + ", " + dataItem.Country;
        }
    }
}