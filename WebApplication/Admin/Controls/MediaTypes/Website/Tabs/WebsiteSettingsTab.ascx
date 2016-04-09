<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebsiteSettingsTab.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.Website.Tabs.WebsiteSettingsTab" %>
<fieldset>
    <div>
        <label for="<%= CodeInHead.ClientID %>">
            Code In Head:</label>
        <div>
            <asp:TextBox runat="server" ID="CodeInHead" TextMode="MultiLine" CssClass="AceEditor"></asp:TextBox>
        </div>
    </div>
    <div>
        <label for="<%= CodeInBody.ClientID %>">
            Code In Body:</label>
        <div>
            <asp:TextBox runat="server" ID="CodeInBody" TextMode="MultiLine" CssClass="AceEditor"></asp:TextBox>
        </div>
    </div>
</fieldset>