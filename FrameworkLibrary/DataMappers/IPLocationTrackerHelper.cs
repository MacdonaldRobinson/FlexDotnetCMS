using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public class IPLocationTrackerHelper : BaseMapper
    {
        private const string MapperKey = "IPLocationTrackerEntriesMapperKey";

        public static IEnumerable<IPLocationTrackerEntry> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().IPLocationTrackerEntries).OrderByDescending(b => b.DateCreated);
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static IPLocationTrackerEntry GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static IPLocationTrackerEntry GetByIP(string ip)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.IPAddress == ip);
        }

        public static Return Insert(IPLocationTrackerEntry obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(IPLocationTrackerEntry obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(IPLocationTrackerEntry obj)
        {
            return Delete(MapperKey, obj);
        }
    }
}
