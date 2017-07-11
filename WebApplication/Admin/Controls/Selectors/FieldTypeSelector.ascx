<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FieldTypeSelector.ascx.cs" Inherits="WebApplication.Admin.Controls.Selectors.FieldTypeSelector" %>

<asp:DropDownList runat="server" ID="FieldTypeDropDown" AppendDataBoundItems="true">
    <asp:ListItem Text="--Select A Type--" Value="" />
</asp:DropDownList>