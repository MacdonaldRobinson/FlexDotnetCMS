<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartSettingsTab.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.ShoppingCartSettingsTab" %>
<fieldset>
    <div>
        <label for="<%= CanAddToCart.ClientID %>">
            Can Add To Cart:
            <asp:CheckBox ID="CanAddToCart" runat="server" /></label>
    </div>
    <div>
        <label for="<%= Price.ClientID %>">
            Price</label>
        <asp:TextBox ID="Price" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= QuantityInStock.ClientID %>">
            QuantityInStock</label>
        <asp:TextBox ID="QuantityInStock" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= RecurringTimePeriod.ClientID %>">
            Recurring Time Period</label><br />
        <asp:DropDownList runat="server" ID="RecurringTimePeriod">
            <asp:ListItem Text="One Time Only" Value="" />
            <asp:ListItem Text="Daily" Value="D" />
            <asp:ListItem Text="Weekly" Value="W" />
            <asp:ListItem Text="Monthly" Value="M" />
            <asp:ListItem Text="Yearly" Value="Y" />
        </asp:DropDownList>
    </div>
</fieldset>