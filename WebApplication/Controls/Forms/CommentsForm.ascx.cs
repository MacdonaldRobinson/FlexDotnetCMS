using FrameworkLibrary;
using System;
using System.Web.UI.WebControls;

namespace WebApplication.Controls
{
    public partial class CommentsForm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentMedia == null)
                CurrentMedia = BaseMapper.GetObjectFromContext((Media)BasePage.CurrentMedia);

            //Bind();
        }

        private void Bind()
        {
            //CommentsList.Comments = currentMediaDetail.Comments.OrderByDescending(c => c.DateCreated).Where(c => c.Status == StatusEnum.Approved.ToString());
            //CommentsList.DisplayMode = WebApplication.Controls.Lists.CommentsList.Mode.None;
        }

        public void SetMedia(Media media)
        {
            this.CurrentMedia = media;

            if (this.BasePage.CurrentUser != null)
            {
                if (IsPostBack)
                    return;

                this.Name.Text = this.BasePage.CurrentUser.UserName;
                this.Email.Text = this.BasePage.CurrentUser.EmailAddress;
            }
        }

        /*public void SetReplyToComment(Comment replyToComment)
        {
            this.ReplyToComment = replyToComment;
        }*/

        private Media CurrentMedia { get; set; }

		public long ReplyToCommentID { get; set; }


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

            comment.Status = StatusEnum.Pending.ToString();

            comment.Media = CurrentMedia;
            comment.Language = BasePage.CurrentLanguage;

            return comment;
        }

        protected void PostComment_OnClick(object sender, EventArgs e)
        {
            Comment newComment = CreateFromFields();

			if (this.ReplyToCommentID != 0)
			{
				newComment.ReplyToCommentID = this.ReplyToCommentID;
			}

			var mediaDetail = CurrentMedia.GetLiveMediaDetail();

			if (!mediaDetail.CommentsAreModerated)
			{
				newComment.Status = StatusEnum.Approved.ToString();
			}

            Return ReturnObj = CommentsMapper.Insert(newComment);

            if (ReturnObj != null)
            {
				switch (ReturnObj.IsError)
				{
					case false:
						if (mediaDetail.CommentsAreModerated)
						{
							BasePage.SendMediaCommentApprovalRequest(CurrentMedia, newComment);
						}
						
						if (this.ReplyToCommentID != 0)
						{
							var comment = CommentsMapper.GetByID(ReplyToCommentID);

							if (comment != null)
							{
								BasePage.SendMediaReplyToComment(newComment, comment);
							}
						}

						ServerMessage.Visible = true;

						if (mediaDetail.CommentsAreModerated)
						{
							ServerMessage.InnerHtml = "Thank you for your feedback. This is a moderated post. Your comment has been submitted for approval";
						}
						else
						{
							ServerMessage.InnerHtml = "Thank you for your feedback";
						}

						ServerMessage.Attributes["class"] += " alert alert-primary";


						break;

                    case true:
						ServerMessage.Visible = true;
						ServerMessage.InnerHtml = $"Error adding comment: {ReturnObj.Error}";
						ServerMessage.Attributes["class"] += " alert alert-danger";

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