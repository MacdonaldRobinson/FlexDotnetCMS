<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlogFeedList.ascx.cs"
    Inherits="WebApplication.Controls.Lists.BlogFeedList" %>
<asp:ListView ID="ItemsList" runat="server" OnItemDataBound="ItemsList_OnItemDataBound">
    <LayoutTemplate>
        <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
    <ItemTemplate>
        <p class="blogpostTitle">
            <asp:HyperLink Target="_blank" ID="Title" runat="server"></asp:HyperLink>
        </p>
        <p class="blogpostInfo">
            <asp:Literal ID="PublishDate" runat="server"></asp:Literal>
        </p>
        <p class="postDesc" id="PostDesc" runat="server">
            <asp:Literal ID="ShortDesc" runat="server"></asp:Literal>
            <asp:HyperLink ID="Link" runat="server" Target="_blank">Read full post&nbsp;&raquo;</asp:HyperLink>
        </p>
    </ItemTemplate>
</asp:ListView>