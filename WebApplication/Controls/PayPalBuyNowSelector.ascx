<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayPalBuyNowSelector.ascx.cs" Inherits="WebApplication.Controls.PayPalBuyNowSelector" %>

<div class="PayPalBuyNowSelector">

    <div class="classTypeSelectorHolder">
        <label>Class Types</label>
        <asp:DropDownList runat="server" ID="ClassTypesDropDownList" AutoPostBack="true" OnTextChanged="ClassTypesDropDownList_TextChanged" AppendDataBoundItems="true">
            <asp:ListItem Text="--- Please Select a Class ---" Value="0" />
        </asp:DropDownList>
    </div>

    <asp:HyperLink runat="server" ID="BuyNowButton" Visible="false" Target="_blank">
        <img src="https://www.paypalobjects.com/en_US/i/btn/btn_buynowCC_LG.gif" />
        <img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1">
    </asp:HyperLink>
</div>