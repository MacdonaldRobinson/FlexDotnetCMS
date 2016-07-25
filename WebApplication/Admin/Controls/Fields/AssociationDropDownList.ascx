<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociationDropDownList.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.AssociationDropDownList" %>

<asp:Panel runat="server" ID="AdminPanel">
    <asp:DropDownList runat="server" ID="ItemsList" AppendDataBoundItems="true">
        <asp:ListItem Text="--- Select ---" Value=""></asp:ListItem>
    </asp:DropDownList>

    <asp:HiddenField ID="Values" runat="server" />
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>