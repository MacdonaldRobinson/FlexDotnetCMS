<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterPageSelector.ascx.cs" Inherits="WebApplication.Admin.Controls.Selectors.MasterPageSelector" %>

<asp:DropDownList runat="server" AppendDataBoundItems="true" ID="MasterPages">
    <asp:ListItem Text=" --- Use Default ---" Value="" />
</asp:DropDownList>