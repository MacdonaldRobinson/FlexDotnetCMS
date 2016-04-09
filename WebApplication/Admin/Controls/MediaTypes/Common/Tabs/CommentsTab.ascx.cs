using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls.MediaTypes.Common.Tabs
{
    public partial class CommentsTab : BaseTab, ITab
    {
        public void SetObject(IMediaDetail selectedItem)
        {
            this.selectedItem = selectedItem;
            UpdateFieldsFromObject();
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            UpdateFieldsFromObject();
        }

        public void UpdateFieldsFromObject()
        {
            ApprovedCommentsCount.Text = selectedItem.Comments.Where(c => c.Status == StatusEnum.Approved.ToString()).Count().ToString();
            RejectedCommentsCount.Text = selectedItem.Comments.Where(c => c.Status == StatusEnum.Rejected.ToString()).Count().ToString();
            PendingCommentsCount.Text = selectedItem.Comments.Where(c => c.Status == StatusEnum.Pending.ToString()).Count().ToString();

            if ((selectedItem == null) || (selectedItem.ID == 0))
                return;

            /*if (selectedItem.ID != 0)
                selectedItem = MediaDetailsMapper.GetByID(selectedItem.ID);*/

            if (selectedItem == null)
                return;

            IEnumerable<Comment> itemComments = CommentsMapper.GetByMediaDetail(selectedItem);

            PendingCommentsList.Comments = itemComments.Where(c => c.Status == StatusEnum.Pending.ToString());
            PendingCommentsList.DisplayMode = WebApplication.Controls.Lists.CommentsList.Mode.ApproveReject;
            PendingCommentsList.List.DataBind();

            BasePage.TemplateVars["PendingCommentsCount"] = PendingCommentsList.Comments.Count().ToString();

            ApprovedCommentsList.Comments = itemComments.Where(c => c.Status == StatusEnum.Approved.ToString());
            ApprovedCommentsList.DisplayMode = WebApplication.Controls.Lists.CommentsList.Mode.ApproveReject;
            ApprovedCommentsList.List.DataBind();

            BasePage.TemplateVars["ApprovedCommentsCount"] = ApprovedCommentsList.Comments.Count().ToString();

            RejectedCommentsList.Comments = itemComments.Where(c => c.Status == StatusEnum.Rejected.ToString());
            RejectedCommentsList.DisplayMode = WebApplication.Controls.Lists.CommentsList.Mode.ApproveReject;
            RejectedCommentsList.List.DataBind();

            BasePage.TemplateVars["RejectedCommentsCount"] = RejectedCommentsList.Comments.Count().ToString();
        }

        public void UpdateObjectFromFields()
        {
        }

        private AdminBasePage BasePage
        {
            get
            {
                return (AdminBasePage)this.Page;
            }
        }        
    }
}