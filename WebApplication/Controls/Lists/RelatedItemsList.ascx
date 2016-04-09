<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedItemsList.ascx.cs" Inherits="WebApplication.Controls.RelatedItems" %>
<asp:ListView ID="RelatedItemsList" runat="server" OnItemDataBound="RelatedItems_OnItemDataBound">
    <LayoutTemplate>
        <h2>Related Items: </h2>
        <ul>
            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <asp:HyperLink ID="Title" runat="server"></asp:HyperLink><br />
            <asp:Literal ID="ShortDesc" runat="server"></asp:Literal>
        </li>
    </ItemTemplate>
</asp:ListView>
<Site:Pager ID="Pager" runat="server" PagedControlID="RelatedItemsList" PageSize="10" />