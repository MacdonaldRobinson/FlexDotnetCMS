using System;
using System.Linq;

namespace FrameworkLibrary
{
    public class SettingsMapper : BaseMapper
    {
        private static string mapperKey = "SettingsMapperKey";

        public static Settings GetSettings()
        {
            var settings = GetDataModel().AllSettings.FirstOrDefault();

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