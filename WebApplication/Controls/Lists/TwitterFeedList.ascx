<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TwitterFeedList.ascx.cs" Inherits="WebApplication.Controls.Lists.TwitterFeedList" %>

<asp:ListView ID="ItemsList" runat="server" OnItemDataBound="ItemsList_OnItemDataBound">
    <LayoutTemplate>
        <ul class="twitterFeedList">
            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <asp:Literal ID="Text" runat="server" />
        </li>
    </ItemTemplate>
</asp:ListView>