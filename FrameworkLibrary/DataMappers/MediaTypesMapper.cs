using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace FrameworkLibrary
{
    public class MediaTypesMapper : BaseMapper
    {
        private const string MapperKey = "MediaTypesMapperKey";

        /*public static IEnumerable<MediaType> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().MediaTypes.OrderByDescending(c => c.Name));
        }*/

        public static MediaType GetByID(long id)
        {
            return GetDataModel().MediaTypes.FirstOrDefault(item => item.ID == id);
        }

        public static IEnumerable<MediaType> GetAllActive()
        {
            return GetDataModel().MediaTypes.Where(item => item.IsActive == true);
        }

        public static IEnumerable<MediaType> FilterByActiveStatus(IEnumerable<MediaType> items, bool isActive)
        {
            return GetDataModel().MediaTypes.Where(item => item.IsActive == isActive);
        }

        public static MediaTypeEnum GetEnumByName(string name)
        {
            return (MediaTypeEnum)Enum.Parse(typeof(MediaTypeEnum), name);
        }

        public static MediaTypeEnum GetEnumByID(long mediaTypeId)
        {
            return GetEnumByObject(GetByID(mediaTypeId));
        }

        public static MediaTypeEnum GetEnumByObject(MediaType mediaType)
        {
            return GetEnumByName(mediaType.Name);
        }

        public static MediaType GetByEnum(MediaTypeEnum mediaTypeEnum)
        {
            return GetDataModel().MediaTypes.FirstOrDefault(item => item.Name == mediaTypeEnum.ToString());
        }

        public static Return Insert(MediaType obj)
        {
            obj.DateCreated = obj.DateLastModified = DateTime.Now;
            return Insert(MapperKey, obj);
        }

        public static Return Update(MediaType obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static void RemoveChildMediaTypes(MediaType obj)
        {
            var mediaTypes = GetDataModel().MediaTypes;

            foreach (var type in mediaTypes)
            {
                var removeMediaTypes = type.MediaTypes.Where(i => i.ID == obj.ID).ToList();

                foreach (var removeMediaType in removeMediaTypes)
                    type.MediaTypes.Remove(removeMediaType);
            }

            obj.MediaTypes.Clear();
        }

        public static Return DeletePermanently(MediaType obj)
        {
            obj.MediaTypesRoles.Clear();
            RemoveChildMediaTypes(obj);

            return Delete(MapperKey, obj);
        }

        public static MediaType CreateObject()
        {
            return GetDataModel().MediaTypes.Create();
        }
    }
}