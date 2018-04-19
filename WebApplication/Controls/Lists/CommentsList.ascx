<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsList.ascx.cs"
    Inherits="WebApplication.Controls.Lists.CommentsList" %>

<style>
	.comments {
		list-style-type: none;
	}

	.comments li {		
		padding: 5px;
		border-top: 1px solid #000;
		font-size: 12px;
		background-color:lightgray;
	}
	.comments li:nth-child(2n+1) {
		background-color:#eee;
	}

	.timeago {
		font-size: 10px;
		color: gray;
		font-style: italic;
		font-weight: bold;
	}

	.commentauthor {
		font-weight: bold;
	}

	.who {
		font-size: 16px;		
		font-style: italic;
	}

	.comment {
		padding: 10px;
		font-size: 14px;
		color: #000;
	}
</style>

<asp:UpdatePanel runat="server">
	<ContentTemplate>
		<asp:ListView ID="ItemsList" runat="server" OnItemDataBound="ItemsList_OnItemDataBound" ItemType="FrameworkLibrary.Comment">
			<LayoutTemplate>
				<ul class="comments">
					<asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
				</ul>
			</LayoutTemplate>
			<ItemTemplate>
				<li>
					<div class="commentauthor">
						<img src="<%# ImageHelper.GetGravatarImageURL(Item.Name + Item.Email, 30, true) %>" />
						<strong class="who">
							<asp:Literal ID="name" runat="server"></asp:Literal>
						</strong>
						<span>							
							<span runat="server" id="datePosted" class="timeago">
								<asp:Literal ID="date" runat="server"></asp:Literal>
							</span>
							<asp:LinkButton ID="Reply" runat="server" OnClick="Reply_OnClick" CommandArgument="<%# Item.ID %>">Reply</asp:LinkButton>
						</span>
					</div>
					<div class="comment">
						<asp:Literal ID="message" runat="server"></asp:Literal>
					</div>
					<div class="buttons" id="Buttons" runat="server">
						<asp:Button ID="Approve" runat="server" class="positive" Text="Approve" OnClick="Approve_OnClick" />
						<asp:Button ID="Reject" runat="server" class="negative" Text="Reject" OnClick="Reject_OnClick" />
						<asp:Button ID="DeletePermanently" runat="server" class="negative" Text="Delete Permanently"
							OnClick="DeletePermanently_OnClick" Visible="false" />
						<div class="clear">
						</div>
					</div>

					<asp:Panel ID="ReplyPanel" runat="server" Visible="false">
						<Site:CommentsForm ID="ReplyForm" runat="server" ReplyToCommentID="<%# Item.ID %>"/>
					</asp:Panel>

					<asp:ListView ID="ChildItemsList" runat="server" OnItemDataBound="ItemsList_OnItemDataBound">
						<ItemTemplate></ItemTemplate>
					</asp:ListView>
				</li>
			</ItemTemplate>
		</asp:ListView>
	</ContentTemplate>
</asp:UpdatePanel>
