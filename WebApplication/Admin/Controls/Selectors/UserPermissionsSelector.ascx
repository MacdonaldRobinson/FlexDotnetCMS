<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserPermissionsSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Selectors.UserPermissionsSelector" %>
<div>
    <label>
        User:</label>
    <Admin:UserSelector ID="UserSelector" runat="server" />
</div>
<asp:Panel ID="OnUserSelectPanel" runat="server" Visible="false">
    <div>
        Permissions:
        <Admin:PermissionsSelector ID="PermissionsSelector" runat="server" />
    </div>
</asp:Panel>