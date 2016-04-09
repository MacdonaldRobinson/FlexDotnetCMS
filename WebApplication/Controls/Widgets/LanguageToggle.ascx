<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageToggle.ascx.cs" Inherits="WebApplication.Controls.Widgets.LanguageToggle" %>

<asp:ListView runat="server" OnItemDataBound="LanguageToggleLinks_ItemDataBound" ID="LanguageToggleLinks" OnDataBound="LanguageToggleLinks_DataBound">
    <LayoutTemplate>
        <ul>
            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <asp:HyperLink ID="LanguageLink" runat="server"></asp:HyperLink>
        </li>
    </ItemTemplate>
</asp:ListView>