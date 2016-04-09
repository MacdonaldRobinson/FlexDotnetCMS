namespace FrameworkLibrary
{
    public class GeoLocationHelper
    {
        public static string APIKey { get; set; }

        private static string GenerateRequestUrl(string visitorIp, string format)
        {
            return "http://freegeoip.net/" + format + "/" + visitorIp;
        }

        public static string GetLocation(string visitorIp, string format = "json")
        {
            var found = IPLocationTrackerHelper.GetByIP(visitorIp);

            if (found != null)
                return found.Location;

            var webRequestHelper = new WebRequestHelper();

            var returnRequest = webRequestHelper.MakeWebRequest(GenerateRequestUrl(visitorIp, format));

            var entry = new IPLocationTrackerEntry();
            entry.IPAddress = visitorIp;
            entry.Location = returnRequest;            

            var returnObj = IPLocationTrackerHelper.Insert(entry);

            return returnRequest;
        }
    }
}