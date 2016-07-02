<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociationDragDrop.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.AssociationDragDrop" %>

<asp:Panel runat="server" ID="AdminPanel">
    <ul id="UL" runat="server" class="dropZone sortable">
        <li class="hidden">
            <asp:HiddenField runat="server" ID="Values" />
        </li>
    </ul>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>