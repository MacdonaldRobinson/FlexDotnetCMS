using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class GlossaryTermsMapper : BaseMapper
    {
        private const string MapperKey = "GlossaryTermsMapperKey";

        public static IEnumerable<GlossaryTerm> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().GlossaryTerms).OrderByDescending(b => b.DateCreated);
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static GlossaryTerm GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static Return Insert(GlossaryTerm obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(GlossaryTerm obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(GlossaryTerm obj)
        {
            return Delete(MapperKey, obj);
        }

        public static GlossaryTerm CreateObject()
        {
            return GetDataModel().GlossaryTerms.Create();
        }
    }
}