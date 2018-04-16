<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsList.ascx.cs"
    Inherits="WebApplication.Controls.Lists.CommentsList" %>

<asp:ListView ID="ItemsList" runat="server" OnItemDataBound="ItemsList_OnItemDataBound">
    <LayoutTemplate>
        <ul class="comments">
            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <div class="commentauthor">
                <strong class="who">
                    <asp:Literal ID="name" runat="server"></asp:Literal>
                </strong>-
                <span>
                    <abbr runat="server" id="datePosted" class="timeago">
                        <asp:Literal ID="date" runat="server"></asp:Literal></abbr>
                    <asp:LinkButton ID="Reply" runat="server" OnClick="Reply_OnClick" Visible="false">Reply</asp:LinkButton>
                    <asp:Panel ID="ReplyPanel" runat="server" Visible="false">
                        <Site:CommentsForm ID="ReplyForm" runat="server" />
                    </asp:Panel>
                </span>
            </div>
            <div class="comment">
                <p>
                    <asp:Literal ID="message" runat="server"></asp:Literal>
                </p>
            </div>
            <div class="buttons" id="Buttons" runat="server">
                <asp:Button ID="Approve" runat="server" class="positive" Text="Approve" OnClick="Approve_OnClick" />
                <asp:Button ID="Reject" runat="server" class="negative" Text="Reject" OnClick="Reject_OnClick" />
                <asp:Button ID="DeletePermanently" runat="server" class="negative" Text="Delete Permanently"
                    OnClick="DeletePermanently_OnClick" Visible="false" />
                <div class="clear">
                </div>
            </div>
            <asp:ListView ID="ChildItemsList" runat="server" OnItemDataBound="ItemsList_OnItemDataBound">
                <ItemTemplate></ItemTemplate>
            </asp:ListView>
        </li>
    </ItemTemplate>
</asp:ListView>
<Site:Pager ID="dataPager" PagedControlID="ItemsList" runat="server" PageSize="10" />
