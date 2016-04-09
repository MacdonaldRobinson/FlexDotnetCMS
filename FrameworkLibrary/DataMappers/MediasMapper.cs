using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace FrameworkLibrary
{
    public class MediasMapper : BaseMapper
    {
        private const string MapperKey = "MediasMapperKey";

        /*public static IEnumerable<Media> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().AllMedia.OrderBy(m => m.OrderIndex));
        }*/

        public static Media GetRootMedia()
        {
            return GetDataModel().AllMedia.FirstOrDefault(item => item.ParentMediaID == null);
        }

        public static Media GetByID(long id)
        {
            return GetDataModel().AllMedia.FirstOrDefault(item => item.ID == id);
        }

        public static Media GetAbsoluteRoot()
        {
            return GetDataModel().AllMedia.FirstOrDefault(item => item.ParentMediaID == null);
        }

        public static Media GetByMediaDetail(IMediaDetail mediaDetail)
        {
            if (mediaDetail == null)
                return null;

            return GetDataModel().AllMedia.FirstOrDefault(item => item.ID == mediaDetail.MediaID);
        }

        public static IEnumerable<Media> GetByTags(IEnumerable<Tag> tags)
        {
            var filtered = new List<Media>();

            foreach (var tag in tags)
            {
                filtered.AddRange(GetByTag(tag));
            }

            return filtered.Distinct();
        }

        public static IEnumerable<Media> GetByTag(Tag tag)
        {
            return GetDataModel().AllMedia.Where(item => item.MediaTags.Select(i => i.Tag).Where(i => i.ID == tag.ID).Any());
        }

        public static IEnumerable<Media> GetAllChildMedias(Media media)
        {
            return GetDataModel().AllMedia.Where(item => item.ParentMediaID == media.ID);
        }

        public static Media CreateObject()
        {
            return GetDataModel().AllMedia.Create();
        }

        public static Media GetFromContext(Media obj)
        {
            return GetObjectFromContext(obj);
        }

        public static Return Insert(Media obj)
        {
            return Insert(MapperKey, obj);
        }

        public static Return Update(Media obj)
        {
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(Media obj)
        {
            obj.MediaTags.Clear();

            return Delete(MapperKey, obj);
        }

        public static void ClearCache()
        {
            ContextHelper.Remove(MapperKey, ContextType.Cache);
        }
    }
}