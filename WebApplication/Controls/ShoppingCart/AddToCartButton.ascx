<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddToCartButton.ascx.cs" Inherits="WebApplication.Controls.ShoppingCart.AddToCartButton" %>

<asp:Panel ID="AddToCartPanel" runat="server" Visible="false">
    <asp:LinkButton ID="AddToCart" runat="server" Text="Add To Cart" OnClick="AddToCart_OnClick" />
</asp:Panel>