using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public partial class FieldAssociation : IMustContainID
    {
		public long MediaID
		{
			get
			{
				return this.MediaDetail.MediaID;
			}
		}

		public string PermaLink
		{
			get
			{
				return this.MediaDetail.Media.PermaLink;
			}
		}
	}
}
