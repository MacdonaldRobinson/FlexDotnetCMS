using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class CommentsMapper : BaseMapper
    {
        private const string MapperKey = "CommentsMapperKey";

        public static IEnumerable<Comment> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().Comments.OrderByDescending(c => c.DateCreated));
        }

        public static Comment GetByID(long id)
        {
            var allItems = GetAll();
            return allItems.FirstOrDefault(item => item.ID == id);
        }

        public static IEnumerable<Comment> GetByMediaDetail(IMediaDetail detail)
        {
            var allItems = GetAll();

            return allItems.Where(item => item.MediaDetails.Where(i => i.ID == detail.ID).Any());
        }

        public static Return Validate(Comment comment)
        {
            var returnObj = new Return();

            var validationMessage = "";

            if (String.IsNullOrEmpty(comment.Name))
                validationMessage += "Name cannot be blank<br />";

            if (String.IsNullOrEmpty(comment.Message))
                validationMessage += "Message cannot be blank<br />";

            if (!String.IsNullOrEmpty(comment.Email.Trim()))
                if (!ValidationHelper.IsEmail(comment.Email))
                    validationMessage += "Email is not in a valid format";

            if (validationMessage != "")
            {
                var ex = new Exception("Validation Error", new Exception(validationMessage));

                returnObj.Error = ErrorHelper.CreateError(ex);
            }

            if (returnObj.IsError)
                DeleteObjectFromContext(comment);

            return returnObj;
        }

        public static Return Insert(Comment obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            var returnObj = Validate(obj);

            if (returnObj.IsError)
            {
                DeleteObjectFromContext(obj);
                return returnObj;
            }

            return Insert(MapperKey, obj);
        }

        public static Return Update(Comment obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(Comment obj)
        {
            var subItems = obj.ReplyToComments;

            foreach (var item in subItems)
                DeletePermanently(item);

            obj.MediaDetails.Clear();

            return Delete(MapperKey, obj);
        }

        public static Comment CreateObject()
        {
            return GetDataModel().Comments.Create();
        }
    }
}