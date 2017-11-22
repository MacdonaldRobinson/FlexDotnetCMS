<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageToggle.ascx.cs" Inherits="WebApplication.Controls.Widgets.LanguageToggle" %>

<asp:ListView runat="server" OnItemDataBound="LanguageToggleLinks_ItemDataBound" ID="LanguageToggleLinks" OnDataBound="LanguageToggleLinks_DataBound">
    <LayoutTemplate>
            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
    <ItemTemplate>
            <asp:HyperLink ID="LanguageLink" runat="server"></asp:HyperLink>
    </ItemTemplate>
</asp:ListView>