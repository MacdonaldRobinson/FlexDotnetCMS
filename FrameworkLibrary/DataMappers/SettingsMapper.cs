using System;
using System.Linq;

namespace FrameworkLibrary
{
    public class SettingsMapper : BaseMapper
    {
        private static string mapperKey = "SettingsMapperKey";

        public static Settings GetSettings()
        {
            var settings = (Settings)ContextHelper.GetFromRequestContext("SettingsMapper_GetSettings");

            if (settings != null)
                return settings;

            settings = GetDataModel()?.AllSettings.FirstOrDefault();

            if (settings != null && settings.DefaultLanguage == null && settings.DefaultLanguageID > 0)
            {
                settings.DefaultLanguage = LanguagesMapper.GetByID(settings.DefaultLanguageID);
            }

            ContextHelper.SetToRequestContext("SettingsMapper_GetSettings", settings);

            return settings;
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(mapperKey, mapperStorageContext);            
        }

        public static Return Update(Settings obj)
        {
            obj.DateLastModified = DateTime.Now;

            Return returnObj = GenerateReturn();

            returnObj = Update<Settings>(mapperKey, obj);

            return returnObj;
        }
    }
}