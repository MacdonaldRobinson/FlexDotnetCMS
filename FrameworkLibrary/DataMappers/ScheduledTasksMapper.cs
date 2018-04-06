using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
	public class ScheduledTasksMapper : BaseMapper
	{
		private const string MapperKey = "ScheduledTasksMapperKey";

		/*public static IEnumerable<ScheduledTask> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().ScheduledTasks.OrderByDescending(c => c.Name));
        }*/

		public static ScheduledTask GetByID(long id)
		{
			return GetDataModel().ScheduledTasks.FirstOrDefault(item => item.ID == id);
		}

		public static IEnumerable<ScheduledTask> GetAllActive()
		{
			return GetDataModel().ScheduledTasks.Where(item => item.IsActive == true);
		}

		public static IEnumerable<ScheduledTask> FilterByActiveStatus(IEnumerable<ScheduledTask> items, bool isActive)
		{
			return GetDataModel().ScheduledTasks.Where(item => item.IsActive == isActive);
		}

		public static Return Insert(ScheduledTask obj)
		{
			obj.DateCreated = obj.DateLastModified = DateTime.Now;
			return Insert(MapperKey, obj);
		}

		public static Return Update(ScheduledTask obj)
		{
			obj.DateLastModified = DateTime.Now;
			return Update(MapperKey, obj);
		}

		public static Return DeletePermanently(ScheduledTask obj)
		{
			return Delete(MapperKey, obj);
		}

		public static ScheduledTask CreateObject()
		{
			return GetDataModel().ScheduledTasks.Create();
		}
	}
}
