using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FrameworkLibrary;

namespace WebApplication.Admin.Views.PageHandlers.Scheduler
{
	public partial class Detail : AdvanceOptionsBasePage
	{
		private ScheduledTask selectedItem = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			long id;

			if (long.TryParse(Request["id"], out id))
			{
				selectedItem = ScheduledTasksMapper.GetByID(id);

				if (!IsPostBack)
					UpdateFieldsFromObject();
			}

			this.Page.Title = this.SectionTitle.Text = GetSectionTitle();
		}

		private string GetSectionTitle()
		{
			if (selectedItem == null)
				return "New Scheduled Task";
			else
				return "Editing Scheduled Task: " + selectedItem.Name;
		}

		private void UpdateObjectFromFields()
		{
			selectedItem.Name = Name.Text;
			selectedItem.Description = Description.Text;
			selectedItem.CodeToExecute = CodeToExecute.Text;
			selectedItem.IsActive = IsActive.Checked;
		}

		private void UpdateFieldsFromObject()
		{
			Name.Text = selectedItem.Name;
			Description.Text = selectedItem.Description;
			CodeToExecute.Text = selectedItem.CodeToExecute;
			IsActive.Checked = selectedItem.IsActive;
		}

		protected void Save_OnClick(object sender, EventArgs e)
		{
			if (selectedItem == null)
				selectedItem = ScheduledTasksMapper.CreateObject();
			else
				selectedItem = BaseMapper.GetObjectFromContext<ScheduledTask>(selectedItem);

			UpdateObjectFromFields();

			Return returnObj = BaseMapper.GenerateReturn("");

			if (selectedItem.ID == 0)
				returnObj = ScheduledTasksMapper.Insert(selectedItem);
			else
				returnObj = ScheduledTasksMapper.Update(selectedItem);

			if (returnObj.IsError)
				DisplayErrorMessage("Error Saving Item", returnObj.Error);
			else
			{
				DisplaySuccessMessage("Successfully Saved Item");


				/*var rootDetails = BaseMapper.GetObjectFromContext((MediaDetail)FrameworkBaseMedia.RootMediaDetail);
                rootDetails.VirtualPath = URIHelper.GetBaseUrlWithScheduledTask(selectedItem);*/

				/*returnObj = MediaDetailsMapper.Update(rootDetails);

                if (!returnObj.IsError)
                    MediaDetailsMapper.StartUpdateVirtualPathForAllChildren(rootDetails);
                else
                    DisplaySuccessMessage("Error updating root media item");*/
			}
		}
	}
}