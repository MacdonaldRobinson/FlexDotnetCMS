using System;
using System.Linq;

namespace FrameworkLibrary
{
    public class SettingsMapper : BaseMapper
    {
        private static string mapperKey = "SettingsMapperKey";

        private static Settings _settings = null;
        public static Settings GetSettings()
        {
            if (_settings != null)
                return _settings;

            _settings = GetDataModel()?.AllSettings.FirstOrDefault();

            return _settings;
        }

        public static void SetSettings(Settings settings)
        {
            _settings = settings;
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