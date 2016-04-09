using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public class FieldsMapper : BaseMapper
    {
        private const string MapperKey = "FieldsMapperKey";

        public static IEnumerable<Field> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().Fields).OrderByDescending(b => b.DateCreated);
        }

        public static Field GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }        
    }
}