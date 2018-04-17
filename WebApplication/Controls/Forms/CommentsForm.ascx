<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsForm.ascx.cs" Inherits="WebApplication.Controls.CommentsForm" %>

<asp:UpdatePanel runat="server">
	<ContentTemplate>
		<div class="form">
			<div>
				<label for="Name">Username: </label>
				<br />
				<asp:TextBox ID="Name" runat="server"></asp:TextBox>
			</div>
			<div>
				<label for="Email">Email (optional):</label><br />
				<asp:TextBox ID="Email" runat="server"></asp:TextBox>
			</div>
			<div>
				<label for="Message">Comment:</label><br />
				<asp:TextBox ID="Message" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox>
			</div>
			<div>
				<asp:Literal ID="ServerMessage" runat="server" />
			</div>
			<div>
				<asp:LinkButton ID="PostComment" Text="Post Comment" runat="server" OnClick="PostComment_OnClick" />
			</div>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>