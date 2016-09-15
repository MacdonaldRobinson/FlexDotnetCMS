using System;
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

        public static List<string> allWebVirtualPaths = new List<string>();
        public static List<string> GetAllWebsiteVirtualPaths()
        {
            if (allWebVirtualPaths == null)
                allWebVirtualPaths = new List<string>();

            if (allWebVirtualPaths.Count > 0)
                return allWebVirtualPaths;

            allWebVirtualPaths = (List<string>)ContextHelper.GetFromRequestContext("WebsiteVirtualPaths");

            if (allWebVirtualPaths == null || !allWebVirtualPaths.Any())
            {
                allWebVirtualPaths = WebsitesMapper.GetDataModel().MediaDetails.Where(i => !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate > DateTime.Now) && i.MediaType.Name == enumName && i.HistoryVersionNumber == 0 && i.Language.IsActive).Select(i => i.CachedVirtualPath).Distinct().ToList();
                //var test = WebsitesMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == enumName).ToList();
                ContextHelper.SetToRequestContext("WebsiteVirtualPaths", allWebVirtualPaths);
            }

            return allWebVirtualPaths;
        }
    }
}