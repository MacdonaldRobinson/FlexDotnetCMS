<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RolesUsersTab.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.RolesUsersTab" %>
<fieldset>
    <h2>Roles that can access this item:</h2>
    <Admin:MediaDetailRolesAssociationEditor ID="MediaDetailRolesAssociationEditor" runat="server" />
    <br />
    <h2>Users in the roles that can access this item:</h2>
    <Admin:MediaDetailUsersAssociationEditor ID="MediaDetailUsersAssociationEditor" runat="server" />
</fieldset>