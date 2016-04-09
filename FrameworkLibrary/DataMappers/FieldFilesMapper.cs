using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public class FieldFilesMapper: BaseMapper
    {
        private const string MapperKey = "FieldFilesMapperKey";

        public static IEnumerable<FieldFile> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().FieldFiles).OrderByDescending(b => b.DateCreated);
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static FieldFile GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static Return Insert(FieldFile obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(FieldFile obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(FieldFile obj)
        {
            return Delete(MapperKey, obj);
        }

        public static FieldFile CreateObject()
        {
            return GetDataModel().FieldFiles.Create();
        }
    }
}
