using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class MasterPagesMapper : BaseMapper
    {
        private const string MapperKey = "MasterPagesMapperKey";

        public static IEnumerable<MasterPage> GetAll()
        {
            return GetDataModel().MasterPages.OrderByDescending(b => b.DateCreated);
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static MasterPage GetByID(long id)
        {
            return GetDataModel().MasterPages.FirstOrDefault(item => item.ID == id);
        }

        public static MasterPage GetByPathToFile(string pathToFile)
        {
            pathToFile = URIHelper.ConvertAbsUrlToTilda(pathToFile).ToLower();
            
            return GetDataModel().MasterPages.FirstOrDefault(item => item.PathToFile.ToLower() == pathToFile);
        }

        public static Return Insert(MasterPage obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(MasterPage obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(MasterPage obj)
        {
            return Delete(MapperKey, obj);
        }

        public static MasterPage CreateObject()
        {
            return GetDataModel().MasterPages.Create();
        }

        public static MasterPage GetDefaultMasterPage()
        {
            return SettingsMapper.GetSettings().DefaultMasterPage;
        }
    }
}