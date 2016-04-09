using FrameworkLibrary;
using System;
using System.Web.UI.WebControls;

namespace WebApplication.Controls
{
    public partial class CommentsForm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentMediaDetail == null)
                CurrentMediaDetail = BaseMapper.GetObjectFromContext((MediaDetail)BasePage.CurrentMediaDetail);

            //Bind();
        }

        private void Bind()
        {
            //CommentsList.Comments = currentMediaDetail.Comments.OrderByDescending(c => c.DateCreated).Where(c => c.Status == StatusEnum.Approved.ToString());
            //CommentsList.DisplayMode = WebApplication.Controls.Lists.CommentsList.Mode.None;
        }

        public void SetMediaDetail(IMediaDetail mediaDetail)
        {
            this.CurrentMediaDetail = mediaDetail;

            if (this.BasePage.CurrentUser != null)
            {
                if (IsPostBack)
                    return;

                this.Name.Text = this.BasePage.CurrentUser.UserName;
                this.Email.Text = this.BasePage.CurrentUser.EmailAddress;
            }
        }

        public void SetReplyToComment(Comment replyToComment)
        {
            this.ReplyToComment = replyToComment;
        }

        private IMediaDetail CurrentMediaDetail
        {
            get
            {
                return (IMediaDetail)ViewState["CurrentMediaDetail"];
            }
            set
            {
                ViewState["CurrentMediaDetail"] = value;
            }
        }

        private Comment ReplyToComment
        {
            get
            {
                return (Comment)ViewState["ReplyToComment"];
            }
            set
            {
                ViewState["ReplyToComment"] = value;
            }
        }

        protected void CommentsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Literal name = (Literal)e.Item.FindControl("name");
                Literal date = (Literal)e.Item.FindControl("date");
                Literal message = (Literal)e.Item.FindControl("message");

                Comment dataItem = (Comment)e.Item.DataItem;

                name.Text = dataItem.Name;
                date.Text = dataItem.DateCreated.ToLongDateString();
                message.Text = dataItem.Message;
            }
        }

        private Comment CreateFromFields()
        {
            Comment comment = CommentsMapper.CreateObject();

            comment.Name = Name.Text;
            comment.Email = Email.Text;
            comment.Message = Message.Text;

            comment.Language = LanguagesMapper.GetFromContext(FrameworkSettings.GetCurrentLanguage());
            comment.Status = StatusEnum.Pending.ToString();

            comment.MediaDetails.Add(BaseMapper.GetObjectFromContext((MediaDetail)CurrentMediaDetail));

            return comment;
        }

        protected void PostComment_OnClick(object sender, EventArgs e)
        {
            Comment newComment = CreateFromFields();

            if (this.ReplyToComment != null)
                newComment.ReplyToCommentID = this.ReplyToComment.ID;

            if (this.BasePage.CurrentUser != null)
                newComment.UserID = this.BasePage.CurrentUser.ID;

            Return ReturnObj = CommentsMapper.Insert(newComment);

            if (ReturnObj != null)
            {
                switch (ReturnObj.IsError)
                {
                    case false:
                        BasePage.DisplaySuccessMessage("Thank you for your feedback. This is a moderated post. Your comment had been submitted for approval");
                        BasePage.SendMediaCommentApprovalRequest(CurrentMediaDetail);

                        if (this.ReplyToComment != null)
                            BasePage.SendMediaReplyToComment(newComment, this.ReplyToComment);

                        break;

                    case true:
                        BasePage.DisplayErrorMessage("Error adding comment", ReturnObj.Error);
                        Bind();
                        break;
                }
            }
        }

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }
    }
}