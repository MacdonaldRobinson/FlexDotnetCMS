<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Instagram.ascx.cs" Inherits="WebApplication.Controls.Widgets.Instagram" %>

<asp:ListView runat="server" ID="InstagramList" OnItemDataBound="InstagramList_ItemDataBound">
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
    </LayoutTemplate>
    <ItemTemplate>
        <div class="instagram-image">
            <asp:Image ID="InstagramImage" runat="server" />
        </div>
    </ItemTemplate>
</asp:ListView>