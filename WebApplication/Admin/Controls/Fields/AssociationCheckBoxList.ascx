<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociationCheckBoxList.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.AssociationCheckBoxList" %>

<asp:Panel runat="server" ID="AdminPanel">
    <asp:CheckBoxList runat="server" ID="ItemsList">
    </asp:CheckBoxList>
    <asp:HiddenField ID="Values" runat="server" />
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>