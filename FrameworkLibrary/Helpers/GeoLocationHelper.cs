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
            var databasePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/"), "GeoLite2-City.mmdb");
            MaxMindDatabaseReader = new DatabaseReader(databasePath);
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
                var found = MaxMindDatabaseReader.City(visitorIp);

                if (found != null)
                    return found;

                /*var webRequestHelper = new WebRequestHelper();

                var returnRequest = webRequestHelper.MakeWebRequest(GenerateRequestUrl(visitorIp, format));

                var entry = new IPLocationTrackerEntry();
                entry.IPAddress = visitorIp;
                entry.Location = returnRequest;

                var returnObj = IPLocationTrackerHelper.Insert(entry);

                return returnRequest;*/
                return new CityResponse();
            }
            catch (Exception ex)
            {
                return new CityResponse();
            }
        }
    }
}