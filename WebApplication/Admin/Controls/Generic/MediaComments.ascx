<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaComments.ascx.cs" Inherits="WebApplication.Admin.Controls.Generic.MediaComments" %>

<asp:ListView runat="server" ItemType="FrameworkLibrary.Comment" ID="CommentsList">
    <LayoutTemplate>
        <ul>
            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <strong>Name: <%# Item.Name %></strong><br />
            <strong>Email: <%# Item.Email %></strong><br />
            <strong>Message</strong><br />
            <div><%# Item.Message %></div>
            <asp:LinkButton ID="DeleteComment" Text="Delete" runat="server" OnClick="DeleteComment_Click" CommandArgument="<%# Item.ID %>" /> |
            <asp:LinkButton ID="ApproveComment" Text="Approve" runat="server" OnClick="ApproveComment_Click" CommandArgument="<%# Item.ID %>" /> |
            <asp:LinkButton ID="RejectComment" Text="Reject" runat="server" OnClick="RejectComment_Click" CommandArgument="<%# Item.ID %>" />
            <hr />
        </li>
    </ItemTemplate>
</asp:ListView>
