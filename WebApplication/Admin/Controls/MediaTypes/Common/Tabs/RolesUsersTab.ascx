<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RolesUsersTab.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.RolesUsersTab" %>
<fieldset>
    <h2>Limit roles that can access this item</h2>
    <Admin:MultiRoleSelector runat="server" id="MultiRoleSelector" />

    <div>
        <asp:CheckBox ID="CanLimitedRolesAccessAllChildPages" runat="server" /> Allow users belonging to these roles to access child pages as well.
    </div>

</fieldset>