using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FrameworkLibrary;

namespace WebApplication.Admin.Controls.Generic
{
    public partial class MediaComments : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
			CommentsList.DataSource = AdminBasePage.SelectedMedia.Comments.Where(i=>i.Status == Status.ToString() && i.LanguageID == AdminBasePage.CurrentLanguage.ID).OrderByDescending(i=>i.DateCreated).ToList();
            CommentsList.DataBind();
        }

        public StatusEnum Status { get; set; }

        protected void DeleteComment_Click(object sender, EventArgs e)
        {
            var commandArgument = long.Parse(((LinkButton)sender).CommandArgument);

            var comment = CommentsMapper.GetByID(commandArgument);

            if(comment != null)
            {
                var returnObj = CommentsMapper.DeletePermanently(comment);

                if (returnObj.IsError)
                    BasePage.DisplayErrorMessage("Error", returnObj.Error);
                else
                {
                    BasePage.DisplaySuccessMessage("Successfully Deleted Comment");
                    BasePage.ExecuteRawJS("UpdateCommentsTab(); RefreshSiteTreeNodeById(" + AdminBasePage.SelectedMedia.ParentMediaID + "); ReloadPreviewPanel(); ");
                }
            }
        }

        protected void ApproveComment_Click(object sender, EventArgs e)
        {
            var commandArgument = long.Parse(((LinkButton)sender).CommandArgument);

            ChangeStatus(commandArgument, StatusEnum.Approved);

        }

        protected void RejectComment_Click(object sender, EventArgs e)
        {
            var commandArgument = long.Parse(((LinkButton)sender).CommandArgument);

            ChangeStatus(commandArgument, StatusEnum.Rejected);
        }

        private void ChangeStatus(long commandArgument, StatusEnum newStatus)
        {
            var comment = CommentsMapper.GetByID(commandArgument);

            if (comment != null)
            {
                comment.Status = newStatus.ToString();

                var returnObj = CommentsMapper.Update(comment);

                if (returnObj.IsError)
                    BasePage.DisplayErrorMessage("Error", returnObj.Error);
                else
                {
                    BasePage.DisplaySuccessMessage("Successfully Updated Comment");
					BasePage.ExecuteRawJS("UpdateCommentsTab(); RefreshSiteTreeNodeById(" + AdminBasePage.SelectedMedia.ParentMediaID + "); ReloadPreviewPanel(); ");
				}
            }
        }

        public AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }
    }
}