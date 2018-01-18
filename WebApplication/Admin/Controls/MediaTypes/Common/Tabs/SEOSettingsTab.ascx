<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SEOSettingsTab.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.SEOSettingsTab" %>
<fieldset>
    <div>
        <label for="<%= PageTitle.ClientID %>">
            Page Title</label>
        <asp:TextBox ID="PageTitle" runat="server" CssClass="MediaTitle"></asp:TextBox>
    </div>
    <div>
        <label for="<%= MetaDescription.ClientID %>">
            Meta Description</label>
        <asp:TextBox ID="MetaDescription" runat="server" TextMode="MultiLine" CssClass="MetaDescription"></asp:TextBox>
    </div>
    <div>
        <label>Meta Keywords</label>
        <asp:TextBox runat="server" ID="MetaKeywords"></asp:TextBox>
    </div>
</fieldset>