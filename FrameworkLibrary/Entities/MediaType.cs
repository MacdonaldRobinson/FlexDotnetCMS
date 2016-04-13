using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrameworkLibrary
{
    public partial class MediaType : IMustContainID
    {
        public Return Validate()
        {
            var returnOnj = BaseMapper.GenerateReturn();
            MediaTypeHandler = MediaTypeHandler.Trim();

            if (!File.Exists(URIHelper.ConvertToAbsPath(URIHelper.ConvertAbsUrlToTilda(MediaTypeHandler))))
            {
                var ex = new Exception("Media Type MediaTypeHandler is invalid", new Exception("File (" + MediaTypeHandler + ") does not exist"));
                returnOnj.Error = ErrorHelper.CreateError(ex);
            }

            return returnOnj;
        }

        public void AddChildMediaTypes(IEnumerable<MediaType> items)
        {
            MediaTypes.Clear();

            foreach (MediaType item in items.Where(item => MediaTypes.Where(i => i.ID == item.ID).Count() == 0))
            {
                MediaTypes.Add(BaseMapper.GetObjectFromContext(item));
            }
        }

        public void AddRoles(List<Role> roles)
        {
            var mediaTypesRoles = MediaTypesRoles.ToList();

            foreach (var mediaTypesRole in mediaTypesRoles)
            {
                var mediaTypeRole = BaseMapper.GetDataModel().MediaTypeRoles.FirstOrDefault(i=>i.ID == mediaTypesRole.ID);

                if(mediaTypeRole != null)
                    BaseMapper.GetDataModel().MediaTypeRoles.Remove(mediaTypeRole);                
            }

            var savedChanges = BaseMapper.GetDataModel().SaveChanges();
            
            foreach (var role in roles)
            {
                var contextRole = BaseMapper.GetObjectFromContext(role);

                MediaTypesRoles.Add(new MediaTypeRole() { Role = contextRole, DateCreated = DateTime.Now, DateLastModified = DateTime.Now });
            }            
        }

        public List<Role> GetRoles()
        {
            return MediaTypesRoles.Select(i => i.Role).ToList();
        }

    }
}