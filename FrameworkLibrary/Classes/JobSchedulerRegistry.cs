using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;

namespace FrameworkLibrary.Classes
{	
	public class JobSchedulerRegistry : Registry
	{
		public JobSchedulerRegistry()
		{
			//TODO: Schedule job here
			//Schedule<Job>().ToRunNow().AndEvery(2).Seconds();
		}
	}
}
