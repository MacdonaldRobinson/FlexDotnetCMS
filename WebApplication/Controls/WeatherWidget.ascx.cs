using FrameworkLibrary;
using System;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebApplication.Controls
{
    public partial class WeatherWidget : System.Web.UI.UserControl
    {
        private WebRequestHelper webRequestHelper = new WebRequestHelper();

        public class WeatherObject
        {
            public string data { get; set; }
        }

        public XmlDocument XmlDocument { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            webRequestHelper.EnableCaching = true;
            webRequestHelper.CacheDurationInSeconds = 60 * 60 * 24;

            var response = webRequestHelper.MakeWebRequest("http://api.worldweatheronline.com/free/v1/weather.ashx?q=" + CityAndCountry + "&format=xml&num_of_days=" + NumberOfDaysToPull + "&key=" + AppSettings.WeatherApiKey);

            if (!response.Contains("<"))
                return;

            XmlDocument = new XmlDocument();
            XmlDocument.LoadXml(response);

            CityQuery.Text = GetInnerText("data/request/query");
            ObservationTime.Text = DateTime.Now.ToString("ddd, dd MMM yyyy");
            Temp.Text = GetInnerText("data/current_condition/temp_C");
            Icon.ImageUrl = GetLoadlWeatherIcons(GetInnerText("data/current_condition/weatherIconUrl"));
            WeatherDesc.Text = GetInnerText("data/current_condition/weatherDesc");

            WeatherList.DataSource = GetNodes("data/weather");
            WeatherList.DataBind();
        }

        public XmlNode GetNode(string query)
        {
            var node = XmlDocument.SelectSingleNode(query);
            return node;
        }

        public XmlNodeList GetNodes(string query)
        {
            var node = XmlDocument.SelectNodes(query);
            return node;
        }

        public string GetInnerText(string query)
        {
            var node = GetNode(query);

            if (node == null)
                return "";

            return node.InnerText;
        }

        public string CityAndCountry { get; set; }

        public string Key { get; set; }

        public string NumberOfDaysToPull { get; set; }

        protected void WeatherList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var node = (XmlNode)e.Item.DataItem;

            var icon = (Image)e.Item.FindControl("Icon");
            var minTemp = (Literal)e.Item.FindControl("MinTemp");
            var maxTemp = (Literal)e.Item.FindControl("MaxTemp");
            var date = (Literal)e.Item.FindControl("Date");

            icon.ImageUrl = GetLoadlWeatherIcons(node.SelectSingleNode("weatherIconUrl").InnerText);
            minTemp.Text = node.SelectSingleNode("tempMinC").InnerText;
            maxTemp.Text = node.SelectSingleNode("tempMaxC").InnerText;
            date.Text = DateTime.Parse(node.SelectSingleNode("date").InnerText).ToString("MMM d");
        }

        public string GetLoadlWeatherIcons(string iconUrl)
        {
            if (string.IsNullOrEmpty(iconUrl))
                return "";

            var uri = new Uri(iconUrl);
            var fileName = uri.Segments[uri.Segments.Length - 1];

            return "~/media/images/weatherIcons/" + fileName;
        }
    }
}