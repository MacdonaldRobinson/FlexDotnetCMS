<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublishSettingsTab.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.PublishSettingsTab" %>
<div>
    <label for="<%= PublishDate.ClientID %>">
        Publish Date</label>
    <asp:TextBox runat="server" ID="PublishDate" CssClass="datetimepicker" />
</div>
<br />
<div>
    <label for="<%= ExpiryDate.ClientID %>">
        Expiry Date</label>
    <asp:TextBox runat="server" ID="ExpiryDate" CssClass="datetimepicker" />
</div>
<div>
    <br />
    Current time on the server is: <%= DateTime.Now.ToString() %>
</div>
