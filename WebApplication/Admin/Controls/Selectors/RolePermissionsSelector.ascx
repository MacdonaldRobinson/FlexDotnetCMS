<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RolePermissionsSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Selectors.RolePermissionsSelector" %>
<div>
    <label>
        Roles:</label>
    <Admin:RoleSelector ID="RoleSelector" runat="server" />
</div>
<asp:Panel ID="OnRoleSelectPanel" runat="server" Visible="false">
    <div>
        <label>
            Permissions:</label>
        <Admin:PermissionsSelector ID="PermissionsSelector" runat="server" />
    </div>
</asp:Panel>