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
            //UpdateFieldsFromObject();
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            UpdateFieldsFromObject();
        }

        public void UpdateFieldsFromObject()
        {
            var media = selectedItem.Media;

            ApprovedCommentsCount.Text = media.Comments.Where(c => c.Status == StatusEnum.Approved.ToString()).Count().ToString();
            RejectedCommentsCount.Text = media.Comments.Where(c => c.Status == StatusEnum.Rejected.ToString()).Count().ToString();
            PendingCommentsCount.Text = media.Comments.Where(c => c.Status == StatusEnum.Pending.ToString()).Count().ToString();

            if ((selectedItem == null) || (selectedItem.ID == 0))
                return;

            /*if (selectedItem.ID != 0)
                selectedItem = MediaDetailsMapper.GetByID(selectedItem.ID);*/

            if (selectedItem == null)
                return;

            PendingCommentsList.SetComments(StatusEnum.Pending, WebApplication.Controls.Lists.CommentsList.Mode.ApproveReject, media);
            PendingCommentsCount.Text = PendingCommentsList.Comments.Count().ToString();

            ApprovedCommentsList.SetComments(StatusEnum.Approved, WebApplication.Controls.Lists.CommentsList.Mode.ApproveReject, media);
            ApprovedCommentsCount.Text = ApprovedCommentsList.Comments.Count().ToString();

            RejectedCommentsList.SetComments(StatusEnum.Rejected, WebApplication.Controls.Lists.CommentsList.Mode.ApproveReject, media);
            RejectedCommentsCount.Text = RejectedCommentsList.Comments.Count().ToString();
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