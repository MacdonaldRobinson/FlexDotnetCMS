using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class MediaTagsMapper : BaseMapper
    {
        private const string MapperKey = "MediaTagsMapperKey";

        public static IEnumerable<MediaTag> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().MediaTags.OrderBy(b => b.OrderIndex));
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }

        public static MediaTag GetByMediaAndTag(Media media, Tag tag)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.TagID == tag.ID && item.MediaID == media.ID);
        }

        public static IEnumerable<MediaTag> GetByMedia(Media media)
        {
            var allItems = GetAll();
            return allItems.Where(item => item.MediaID == media.ID);
        }

        public static IEnumerable<MediaTag> GetByTag(Tag tag)
        {
            var allItems = GetAll();
            return allItems.Where(item => item.TagID == tag.ID);
        }

        public static MediaTag CreateObject()
        {
            return GetDataModel().MediaTags.Create();
        }
    }
}