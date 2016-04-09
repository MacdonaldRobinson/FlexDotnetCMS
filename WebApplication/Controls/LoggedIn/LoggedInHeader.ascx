<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedInHeader.ascx.cs" Inherits="WebApplication.Controls.OnLogin.LoggedInHeader" %>

<script type="text/javascript">
    $(document).ready(function () {
        if (window.top != window) {
            $("#LoggedInHeaderPanel").hide();
        }
    });
</script>
<asp:Panel runat="server" ID="LoggedInHeaderPanel" ClientIDMode="Static" Visible="false" CssClass="cms-header">
    <asp:Panel ID="AccessCMSPermissionsPanel" runat="server" Visible="false" CssClass="cms-header-controls">
        <asp:HyperLink ID="QuickEditCurrentPage" runat="server" CssClass="colorbox iframe" data-OnColorboxClose="window.location.reload()"><i class="fa fa-pencil"></i>&nbsp;Quick Edit</asp:HyperLink>
        <br />
        <asp:LinkButton ID="EditCurrentPage" runat="server" OnClick="EditCurrentPage_OnClick"><i class="fa fa-expand"></i>&nbsp;Edit In CMS</asp:LinkButton>
    </asp:Panel>
    <div class="cms-header-logout">
        <Site:LoginSuccess ID="LoginSuccess" runat="server" />
    </div>
</asp:Panel>