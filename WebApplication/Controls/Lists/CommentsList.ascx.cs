using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Lists
{
    public partial class CommentsList : System.Web.UI.UserControl
    {
        private Mode mode = Mode.None;
        private StatusEnum statusEnum = StatusEnum.Approved;
        private Media media = null;

        public enum Mode
        {
            None,
            ApproveReject
        }        

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!BasePage.IsInAdminSection)
            {
                if(media == null && BasePage.CurrentMedia != null)
                {                          
                    SetComments(this.statusEnum, this.mode, BasePage.CurrentMedia);
                }                
            }
        }

        protected void Approve_OnClick(object sender, EventArgs e)
        {            
            Comment comment = CommentsMapper.GetByID(long.Parse(((Button)sender).CommandArgument));            
            comment = BaseMapper.GetObjectFromContext<Comment>(comment);            

            comment.Status = StatusEnum.Approved.ToString();
            Return obj = CommentsMapper.Update(comment);

            if (obj.IsError)
                this.BasePage.DisplayErrorMessage("Error", obj.Error);

            SetComments(statusEnum, mode, media);

        }

        protected void Reject_OnClick(object sender, EventArgs e)
        {
            Comment comment = CommentsMapper.GetByID(long.Parse(((Button)sender).CommandArgument));
            comment = BaseMapper.GetObjectFromContext<Comment>(comment);

            comment.Status = StatusEnum.Rejected.ToString();
            Return obj = CommentsMapper.Update(comment);

            if (obj.IsError)
                this.BasePage.DisplayErrorMessage("Error", obj.Error);

            SetComments(statusEnum, mode, media);
        }

        protected void DeletePermanently_OnClick(object sender, EventArgs e)
        {
            Comment comment = CommentsMapper.GetByID(long.Parse(((Button)sender).CommandArgument));
            comment = BaseMapper.GetObjectFromContext<Comment>(comment);

            //comment.MediaDetails.Clear();
            Return obj = CommentsMapper.DeletePermanently(comment);

            if (obj.IsError)
                this.BasePage.DisplayErrorMessage("Error", obj.Error);

            SetComments(statusEnum, mode, media);
        }

        protected void ItemsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlContainerControl buttons = (HtmlContainerControl)e.Item.FindControl("Buttons");

                switch (mode)
                {
                    case Mode.None:
                        buttons.Visible = false;
                        break;

                    case Mode.ApproveReject:
                        buttons.Visible = true;
                        break;
                }

                Literal name = (Literal)e.Item.FindControl("name");
                Literal date = (Literal)e.Item.FindControl("date");
                Literal message = (Literal)e.Item.FindControl("message");
                Button approve = (Button)e.Item.FindControl("Approve");
                Button reject = (Button)e.Item.FindControl("Reject");
                Button DeletePermanently = (Button)e.Item.FindControl("DeletePermanently");
                HtmlControl abbrDate = (HtmlControl)e.Item.FindControl("datePosted");
                LinkButton Reply = (LinkButton)e.Item.FindControl("Reply");
                ListView ChildItemsList = (ListView)e.Item.FindControl("ChildItemsList");

                Comment dataItem = (Comment)e.Item.DataItem;

                Reply.CommandArgument = dataItem.ID.ToString();

                if (ReplyToCommentID == dataItem.ID)
                {
                    Panel ReplyPanel = (Panel)e.Item.FindControl("ReplyPanel");
                    CommentsForm ReplyForm = (CommentsForm)ReplyPanel.FindControl("ReplyForm");
                    ReplyForm.SetMedia(dataItem.Media);

                    ReplyPanel.Visible = true;
                }

                name.Text = dataItem.Name;
                date.Text = dataItem.DateCreated.ToString("o");
                message.Text = dataItem.Message;
                abbrDate.Attributes.Add("title", date.Text);

                DeletePermanently.CommandArgument = approve.CommandArgument = reject.CommandArgument = dataItem.ID.ToString();

                if (dataItem.Status == StatusEnum.Approved.ToString())
                    approve.Visible = false;

                if (dataItem.Status == StatusEnum.Rejected.ToString())
                {
                    reject.Visible = false;
                    DeletePermanently.Visible = true;
                }

                if (dataItem.ReplyToComments.Count > 0)
                {
                    ChildItemsList.LayoutTemplate = ItemsList.LayoutTemplate;
                    ChildItemsList.ItemTemplate = ItemsList.ItemTemplate;

                    ChildItemsList.DataSource = dataItem.ReplyToComments.OrderByDescending(i => i.DateCreated).Where(i => i.Status == dataItem.Status);
					ChildItemsList.DataBind();
				}
            }
        }

		private long ReplyToCommentID
		{
			get
			{				
				var replyToComment = ContextHelper.GetFromSession("ReplyToCommentID");

				if (replyToComment == null)
					return 0;

				return (long)replyToComment;
			}
			set
			{				
				ContextHelper.SetToSession("ReplyToCommentID", value);
			}
		}

        protected void Reply_OnClick(object sender, EventArgs e)
        {
			var linkButton = (LinkButton)sender;
			var replyPanel = (Panel)WebFormHelper.FindControlRecursive(linkButton.Parent, "ReplyPanel");

			var replyPanels = WebFormHelper.FindControlsRecursive(ItemsList, "ReplyPanel");

			foreach (var panel in replyPanels)
			{
				panel.Visible = false;
			}

			ReplyToCommentID = long.Parse(linkButton.CommandArgument);

			if (replyPanel != null)
			{
				replyPanel.Visible = true;
			}
		}

        public BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }

        public Mode DisplayMode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
            }
        }

        public ListView List
        {
            get
            {
                return this.ItemsList;
            }
        }

        public void SetComments(StatusEnum statusEnum, Mode mode, Media media)
        {
            this.mode = mode;
            this.statusEnum = statusEnum;
            this.media = media;

            ItemsList.DataSource = media.Comments.Where(i=>i.Status == statusEnum.ToString() && i.ReplyToComment == null).OrderByDescending(i=>i.DateLastModified).ToList();
			ItemsList.DataBind();
        }

        public IEnumerable<Comment> Comments
        {
            get
            {
                return (IEnumerable<Comment>)ItemsList.DataSource;
            }
        }

		protected void CancelReply_Click(object sender, EventArgs e)
		{
			var cancelReply = (LinkButton)sender;
			cancelReply.Parent.Visible = false;
			ReplyToCommentID = 0;
		}
	}
}