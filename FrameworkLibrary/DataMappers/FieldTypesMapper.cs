using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public class FieldTypesMapper : BaseMapper
    {
        private const string MapperKey = "FieldTypesMapperKey";

        public static IEnumerable<FieldType> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().FieldTypes).OrderByDescending(b => b.DateCreated);
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static FieldType GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static Return Insert(FieldType obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(FieldType obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(FieldType obj)
        {
            return Delete(MapperKey, obj);
        }

        public static FieldType CreateObject()
        {
            return GetDataModel().FieldTypes.Create();
        }
    }
}
