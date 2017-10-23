using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace FrameworkLibrary
{
    public class TagsMapper : BaseMapper
    {
        private static string mapperKey = "TagsMapperKey";

        public static IEnumerable<Tag> GetAll()
        {
            return GetAll(mapperKey, () => GetDataModel().Tags.OrderByDescending(i => i.DateCreated));
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(mapperKey, ContextType.Cache);
        }

        public static Tag GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static IEnumerable<Tag> FilterByActiveStatus(IEnumerable<Tag> items, bool isActive)
        {
            return items.Where(item => item.IsActive == isActive);
        }

        public static IEnumerable<Tag> GetAllActive()
        {
            return FilterByActiveStatus(GetAll(), true);
        }

        public static Tag GetByName(string name)
        {
            var allItems = GetAll();
            name = name.ToLower();

            return allItems.FirstOrDefault(item => item.Name.ToLower() == name);
        }

        public static Tag GetBySefTitle(string sefTitle)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.SefTitle == sefTitle);
        }

        public static Return Insert(Tag obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            if (obj.ThumbnailPath == null)
                obj.ThumbnailPath = "";

            return Insert(mapperKey, obj);
        }

        public static Return Update(Tag obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(mapperKey, obj);
        }

        public static Return DeletePermanently(Tag obj)
        {
            return Delete(mapperKey, obj);
        }

        public static Tag CreateObject()
        {
            return GetDataModel().Tags.Create();
        }

        public static string GetUrlToDetailsPage(string absListPageUrl, Tag item)
        {
            if (!absListPageUrl.EndsWith("/"))
                absListPageUrl = absListPageUrl + "/";

            return URIHelper.ConvertToAbsUrl(absListPageUrl) + item.SefTitle;
        }

        /*public static IEnumerable<RssItem> GetRssItems(FrameworkBaseMedia frameworkBasePage, IEnumerable<Tag> items)
        {
            IEnumerable<RssItem> rssItems = new IEnumerable<RssItem>();

            foreach (Tag item in items)
            {
                RssItem rssItem = new RssItem(item.Name, item.Description, GetUrlToDetailsPage(frameworkBasePage.CurrentMedia.VirtualPath, item), item.DateLastModified, item);
                rssItems.Add(rssItem);
            }

            return rssItems;
        }*/

        public static string GetAllJSON()
        {
            return GetJSON(GetAll());
        }

        public static string GetJSON(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return "[]";
            string json = "[";

            long counter = 0;
            foreach (Tag item in tags)
            {
                json += "{id: \"" + item.ID.ToString() + "\", name: \"" + item.Name.Replace("\"", "\\\"") + "\"}";
                counter++;

                if (counter < tags.Count())
                    json += ",";
            }

            json += "]";

            if (json == "[]")
                json = "[{}]";

            return json;
        }
    }
}