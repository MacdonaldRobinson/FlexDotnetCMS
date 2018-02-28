using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class EmailsMapper : BaseMapper
    {
        private const string MapperKey = "EmailLogsMapperKey";

        /*public static IEnumerable<EmailLog> GetAll()
        {
            return GetDataModel().EmailLogs;
        }*/

        public static EmailLog GetByID(long id)
        {
            return GetDataModel().EmailLogs.FirstOrDefault(item => item.ID == id);
        }

        public static EmailLog CreateObject()
        {
            return GetDataModel().EmailLogs.Create();
        }

        public static Return Insert(EmailLog obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        public static Return Update(EmailLog obj)
        {
            obj.DateLastModified = DateTime.Now;
            return Update(MapperKey, obj);
        }

        public static Return DeletePermanently(EmailLog obj)
        {
            return Delete(MapperKey, obj);
        }
    }
}