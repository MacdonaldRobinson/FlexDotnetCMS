using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class WebsitesMapper : MediaDetailsMapper
    {
        private static string enumName = MediaTypeEnum.Website.ToString();

        public static Website GetWebsite(long versionNumber = 0, Language language = null)
        {
            if (language == null)
                language = FrameworkSettings.GetCurrentLanguage();

            var key = "GetWebsite?version=" + versionNumber + "&languageId=" + language.ID;

            var website = ContextHelper.GetFromRequestContext(key);

            if (website != null)
                return (Website)website;

            website = WebsitesMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == versionNumber && i.MediaType.Name == enumName && i.LanguageID == language.ID);

            if (website == null)
            {
                var defaultLanguage = LanguagesMapper.GetDefaultLanguage();
                website = WebsitesMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == versionNumber && i.MediaType.Name == enumName && i.LanguageID == defaultLanguage.ID);
            }

            ContextHelper.SetToRequestContext(key, website);

            return (Website)website;
        }

        public static List<string> GetAllWebsiteVirtualPaths()
        {
            var WebsiteVirtualPaths = new List<string>();
            WebsiteVirtualPaths = (List<string>)ContextHelper.GetFromRequestContext("WebsiteVirtualPaths");

            if (WebsiteVirtualPaths == null || !WebsiteVirtualPaths.Any())
            {
                WebsiteVirtualPaths = WebsitesMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == enumName && i.HistoryVersionNumber == 0 && i.Language.IsActive).Select(i => i.CachedVirtualPath).Distinct().ToList();
                ContextHelper.SetToRequestContext("WebsiteVirtualPaths", WebsiteVirtualPaths);
            }

            return WebsiteVirtualPaths;
        }
    }
}