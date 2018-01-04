<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociationMultiSelect.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.AssociationMultiSelect" %>

<asp:Panel ID="AdminPanel" runat="server">
    <asp:ListBox runat="server" SelectionMode="Multiple" class='chosen' ID="ItemsList">
    </asp:ListBox>    

    <asp:HiddenField ID="Values" runat="server" />
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>
