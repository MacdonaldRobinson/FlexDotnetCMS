using System;
using System.Net;
using System.Web;

namespace GoogleGeocoder
{
    public interface ISpatialCoordinate
    {
        decimal Latitude { get; set; }

        decimal Longitude { get; set; }
    }

    /// <summary>
    /// Coordiate structure. Holds Latitude and Longitude.
    /// </summary>
    public struct Coordinate : ISpatialCoordinate
    {
        private decimal _latitude;

        private decimal _longitude;

        public Coordinate(decimal latitude, decimal longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public decimal Latitude
        {
            get { return _latitude; }
            set { this._latitude = value; }
        }

        public decimal Longitude
        {
            get { return _longitude; }
            set { this._longitude = value; }
        }
    }

    public struct StudioLocationCoordinate : ISpatialCoordinate
    {
        private decimal _latitude;
        private decimal _longitude;
        private string _toolTipHtml;

        private string _studioGUID;

        public StudioLocationCoordinate(decimal latitude, decimal longitude, string Tooltip, string StudioGuid)
        {
            _latitude = latitude;
            _longitude = longitude;
            _toolTipHtml = Tooltip;
            _studioGUID = StudioGuid;
        }

        public decimal Latitude
        {
            get { return _latitude; }
            set { this._latitude = value; }
        }

        public decimal Longitude
        {
            get { return _longitude; }
            set { this._longitude = value; }
        }

        public string Tooltip
        {
            get { return _toolTipHtml; }
            set { _toolTipHtml = value; }
        }

        public string StudioGUID
        {
            get { return _studioGUID; }
            set { _studioGUID = value; }
        }
    }

    public class Geocode
    {
        private const string _googleUri = "http://maps.google.com/maps/geo?q=";

        private static string _googleKey = null;
        // Get your key from:  http://www.google.com/apis/maps/signup.html

        private const string _outputType = "csv";

        private static Uri GetGeocodeUri(string apiKey, string address)
        {
            _googleKey = apiKey;
            address = HttpUtility.UrlEncode(address);
            return new Uri(String.Format("{0}{1}&output={2}&key={3}", _googleUri, address, _outputType, _googleKey));
        }

        public static Coordinate GetCoordinates(string apiKey, string address)
        {
            _googleKey = apiKey;
            WebClient client = new WebClient();
            Uri uri = GetGeocodeUri(apiKey, address);

            //  The first number is the status code,
            //             * the second is the accuracy,
            //             * the third is the latitude,
            //             * the fourth one is the longitude.
            //

            try
            {
                string[] geocodeInfo = client.DownloadString(uri).Split(',');
                return new Coordinate(Convert.ToDecimal(geocodeInfo[2]), Convert.ToDecimal(geocodeInfo[3]));
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(5000);
                string[] geocodeInfo = client.DownloadString(uri).Split(',');
                return new Coordinate(Convert.ToDecimal(geocodeInfo[2]), Convert.ToDecimal(geocodeInfo[3]));
            }
        }
    }
}