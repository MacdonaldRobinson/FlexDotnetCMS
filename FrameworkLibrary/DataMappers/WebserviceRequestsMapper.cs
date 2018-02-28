using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class WebserviceRequestsMapper : BaseMapper
    {
        private const string MapperKey = "WebserviceRequestsMapperKey";
        private const int MaxNumberOfLogEntries = 100;

        public static IEnumerable<WebserviceRequest> GetAll()
        {
            return GetDataModel().WebserviceRequests.OrderByDescending(b => b.DateCreated);
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static WebserviceRequest GetByID(long id)
        {
            return GetDataModel().WebserviceRequests.FirstOrDefault(item => item.ID == id);
        }

        public static WebserviceRequest GetByUrl(string url)
        {            
            return GetDataModel().WebserviceRequests.FirstOrDefault(item => (item != null) && (item.Url == url));
        }

        public static Return Insert(WebserviceRequest obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            GetDataModel(true);

            var allItems = GetDataModel().WebserviceRequests.OrderBy(i => i.DateCreated);
            var total = allItems.Count();

            if (total > MaxNumberOfLogEntries)
            {
                var reduceBy = (total - MaxNumberOfLogEntries) + 1;

                for (var i = 0; i < reduceBy; i++)
                    DeleteObjectFromContext(allItems.ElementAt(i));
            }

            return Insert(MapperKey, obj);
        }

        public static Return Update(WebserviceRequest obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(WebserviceRequest obj)
        {
            return Delete(MapperKey, obj);
        }

        public static WebserviceRequest CreateObject()
        {
            return GetDataModel().WebserviceRequests.Create();
        }
    }
}