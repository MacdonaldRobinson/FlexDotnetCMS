<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UrlRedirectRulePanel.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.UrlRedirectRule.UrlRedirectRulePanel" %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#VirtualPathHolder,#SectionTitleHolder,#LongDescriptionHolder,#ThumbnailPathHolder, #MasterPageSelectorHolder,#TagsHolder").hide();
    });
</script>

<Admin:CommonPanel ID="CommonPanel" runat="server" />

<asp:Panel runat="server" ID="PanelFields">
    <div>
        <label for="<%= VirtualPathToRedirect.ClientID %>">
            Virtual Path To Redirect</label>
        <asp:TextBox runat="server" ID="VirtualPathToRedirect" ClientIDMode="Static"></asp:TextBox>
    </div>
    <div>
        <label for="<%= RedirectToUrl.ClientID %>">
            Redirect To Url</label>
        <asp:TextBox runat="server" ID="RedirectToUrl" ClientIDMode="Static"></asp:TextBox>
    </div>
    <div>
        <label for="<%= Is301Redirect.ClientID %>">
            <asp:CheckBox runat="server" ID="Is301Redirect" ClientIDMode="Static" />
            Is 301 Redirect</label>
    </div>
</asp:Panel>