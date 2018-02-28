using System;
using System.Collections.Generic;
using System.Data.Entity.Core;

using System.Linq;

namespace FrameworkLibrary
{
    public class MediaTypesRolesMapper : BaseMapper
    {
        private const string MapperKey = "MediaTypesRolesMapperKey";

        public static IEnumerable<MediaTypeRole> GetAll()
        {
            return GetDataModel().MediaTypeRoles;
        }

        public static MediaTypeRole GetByID(long id)
        {
            return GetDataModel().MediaTypeRoles.FirstOrDefault(item => item.ID == id);
        }

        public static Return Insert(MediaTypeRole obj)
        {
            obj.DateCreated = obj.DateLastModified = DateTime.Now;
            return Insert(MapperKey, obj);
        }

        public static Return Update(MediaTypeRole obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(MediaTypeRole obj)
        {
            var permissions = obj.MediaTypeRolesPermissions;

            foreach (var p in permissions)
                GetDataModel().MediaTypeRolesPermissions.Remove(p);

            return Delete(MapperKey, obj);
        }

        public static MediaTypeRole CreateObject()
        {
            return GetDataModel().MediaTypeRoles.Create();
        }
    }
}