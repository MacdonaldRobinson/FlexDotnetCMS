<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RenderChildren.ascx.cs" Inherits="WebApplication.Controls.RenderChildren.RenderChildren" %>

<asp:ListView runat="server" ID="Children" OnItemDataBound="Children_ItemDataBound">
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
    </LayoutTemplate>
    <ItemTemplate>
        <asp:Literal ID="Layout" runat="server" />
    </ItemTemplate>
</asp:ListView>

<Site:Pager ID="Pager" runat="server" PagedControlID="Children" />