using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
	public partial class InjectHtmlTab : BaseTab, ITab
	{
		public void SetObject(IMediaDetail selectedItem)
		{
			this.selectedItem = selectedItem;			
		}

		public void UpdateFieldsFromObject()
		{
			InjectHtml.Text = selectedItem.InjectHtml;
		}

		public void UpdateObjectFromFields()
		{
			selectedItem.InjectHtml = InjectHtml.Text;
		}
	}
}