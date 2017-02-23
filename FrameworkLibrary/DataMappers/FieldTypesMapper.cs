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

        private static List<FieldType> _allFieldTypes = null;

        public static IEnumerable<FieldType> GetAll()
        {
            if (_allFieldTypes == null)
                _allFieldTypes = GetAll(MapperKey, () => GetDataModel().FieldTypes).OrderBy(b => b.Name).ToList();

            return _allFieldTypes;
        }

        public static void ClearCache()
        {
            _allFieldTypes = null;

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

            _allFieldTypes = null;

            return Insert(MapperKey, obj);
        }

        public static Return Update(FieldType obj)
        {
            obj.DateLastModified = DateTime.Now;

            _allFieldTypes = null;

            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(FieldType obj)
        {
            _allFieldTypes = null;

            return Delete(MapperKey, obj);
        }

        public static FieldType CreateObject()
        {
            return GetDataModel().FieldTypes.Create();
        }
    }
}
