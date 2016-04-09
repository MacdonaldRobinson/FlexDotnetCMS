<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsForm.ascx.cs" Inherits="WebApplication.Controls.CommentsForm" %>

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
        <asp:TextBox ID="Message" runat="server" Rows="10" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div>
        <asp:Button ID="PostComment" runat="server" Text="Post Comment" OnClick="PostComment_OnClick" />
    </div>
</div>