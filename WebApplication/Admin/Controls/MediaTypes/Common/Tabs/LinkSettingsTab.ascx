<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkSettingsTab.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.LinkSettings" %>
<fieldset>
    <div>
        <asp:CheckBox ID="ShowInSearchResults" runat="server" Checked="true" />&nbsp;<label class="exception"
            for="<%= ShowInSearchResults.ClientID %>">Show In Search Results</label>
    </div>
    <div>
        <p>
            <asp:CheckBox ID="RedirectToFirstChild" runat="server" /><label for="<%= RedirectToFirstChild.ClientID %>">
                Redirect To First Child</label>
        </p>
    </div>
    <div>
        <p>
            <asp:CheckBox ID="IsProtected" runat="server" /><label for="<%= IsProtected.ClientID %>">
                Is Protected Section</label>
        </p>
    </div>
    <div>
        <asp:CheckBox ID="UseDirectLink" runat="server" Checked="false" />&nbsp;<label class="exception"
            for="<%= UseDirectLink.ClientID %>">Use Direct Link</label>
    </div>

    <div>
        <label for="<%= DirectLink.ClientID %>">
            Direct Link</label>
        <Admin:FileSelector ID="DirectLink" runat="server" DirPath="~/media/uploads/documents/" />
    </div>

    <div>
        <label for="<%= CssClasses.ClientID %>">
            Css Classes</label>
        <asp:TextBox runat="server" ID="CssClasses" />
    </div>

    <div>
        <asp:CheckBox ID="OpenInNewWindow" runat="server" Checked="false" />&nbsp;<label class="exception"
            for="<%= OpenInNewWindow.ClientID %>">Open In New Window</label>
    </div>
    <div>
        <asp:CheckBox ID="ShowOnMobileVersion" runat="server" Checked="false" />&nbsp;<label class="exception"
            for="<%= ShowOnMobileVersion.ClientID %>">Show On Mobile Version</label>
    </div>
    <div>
        <asp:CheckBox ID="RenderInFooter" runat="server" Checked="false" />&nbsp;<label for="<%= RenderInFooter.ClientID %>">Render In Footer</label>
    </div>

    <div>
        <asp:CheckBox ID="ForceSSL" runat="server" Checked="false" />&nbsp;<label for="<%= ForceSSL.ClientID %>">ForceSSL</label>
    </div>
</fieldset>