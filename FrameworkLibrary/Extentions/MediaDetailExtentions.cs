using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public static class MediaDetailExtentions
    {
        public static IEnumerable<IMediaDetail> WhereField(this IEnumerable<IMediaDetail> items, string fieldCode, string fieldValue)
        {
            return items.Where(i => i.Fields.Any(j => j.FieldCode == fieldCode && j.FieldValue == fieldValue));
        }

        public static IEnumerable<IMediaDetail> WhereField(this IEnumerable<IMediaDetail> items, string whereExpression)
        {
            return items.Where(i => i.Fields.Where(whereExpression).Any());
        }
    }
}
