<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCart.ascx.cs" Inherits="WebApplication.Controls.ShoppingCart.ShoppingCart" %>

<fieldset>
    <h3>My Shopping Cart</h3>
    <asp:ListView ID="ShoppingCartList" runat="server" OnItemDataBound="ShoppingCartList_OnItemDataBound" OnDataBound="ShoppingCartList_OnDataBound">
        <LayoutTemplate>
            <ul>
                <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                <asp:LinkButton ID="ReCalculateTotal" runat="server" Text="Re-Calculate Total" OnClick="ReCalculateTotal_OnClick" /><br />
                <br />
                Sub Total:
                <asp:Literal ID="SubTotal" runat="server"></asp:Literal><br />
                Tax:
                <asp:Literal ID="ShoppingCartTax" runat="server"></asp:Literal>%<br />
                Total:
                <asp:Literal ID="Total" runat="server"></asp:Literal>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li>Name:
                <asp:Literal ID="Name" runat="server"></asp:Literal>
                Unit Price:
                <asp:Literal ID="Price" runat="server"></asp:Literal>
                Quantity:
                <asp:TextBox ID="Quantity" runat="server"></asp:TextBox>
                <asp:LinkButton ID="RemoveFromCart" runat="server" OnClick="RemoveFromCart_OnClick" Text="Remove From Cart" />
            </li>
        </ItemTemplate>
    </asp:ListView>
    <asp:Panel ID="EmptyCartPanel" runat="server" Visible="false">
        Your cart is empty!
    </asp:Panel>
</fieldset>