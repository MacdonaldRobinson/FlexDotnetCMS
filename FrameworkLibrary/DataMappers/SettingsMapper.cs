using System;
using System.Linq;

namespace FrameworkLibrary
{
    public class SettingsMapper : BaseMapper
    {
        private static string mapperKey = "SettingsMapperKey";

        private static Settings settings = null;
        public static Settings GetSettings()
        {
            if (settings != null)
                return settings;

            settings = GetDataModel()?.AllSettings.FirstOrDefault();

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