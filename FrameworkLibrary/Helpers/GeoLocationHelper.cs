using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using System;
using System.Web;
using System.IO;

namespace FrameworkLibrary
{
    public class GeoLocationHelper
    {
        public static string APIKey { get; set; }
        public static DatabaseReader MaxMindDatabaseReader;

        static GeoLocationHelper()
        {
            /* Database is updated monthly and can be downloaded from: http://dev.maxmind.com/geoip/geoip2/geolite2/ */

            var databasePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/"), "GeoLite2-City.mmdb");

            if (File.Exists(databasePath))
            {
                MaxMindDatabaseReader = new DatabaseReader(databasePath);
            }
        }

        private static string GenerateRequestUrl(string visitorIp, string format)
        {
            return "http://freegeoip.net/" + format + "/" + visitorIp;
        }

        public static CityResponse GetLocation(string visitorIp, string format = "json")
        {
            //var found = IPLocationTrackerHelper.GetByIP(visitorIp);\

            try
            {
                if (MaxMindDatabaseReader != null)
                {
                    var found = MaxMindDatabaseReader.City(visitorIp);
                    return found;
                }

                return new CityResponse();
            }
            catch (Exception ex)
            {
                return new CityResponse();
            }
        }
    }
}