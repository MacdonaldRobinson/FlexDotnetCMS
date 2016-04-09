using System.Collections.Generic;

namespace FrameworkLibrary
{
    public class PreloadHelper
    {
        private static List<string> list = new List<string>();

        public static void AddToList(string url)
        {
            url = URIHelper.ConvertToAbsUrl(url);

            if (list.Contains(url))
                return;

            list.Add(url);
        }

        public static List<string> PreloadList
        {
            get
            {
                return list;
            }
        }

        public static string GenerateJQueryPreloadCode()
        {
            var urls = "";

            foreach (string url in list)
            {
                urls += "'" + url + "', ";
            }

            var code = "var allSlidesLoaded = false; $.preload([" + urls + "], { loaded_all: function(loaded, total) { allSlidesLoaded = true; } }); ";

            list.Clear();

            return code;
        }
    }
}