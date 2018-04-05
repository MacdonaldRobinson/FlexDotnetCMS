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

    <div>
        <label>Robots</label><br />
		<asp:DropDownList runat="server" ID="MetaRobots">
			<asp:ListItem Text="all" Value="all"/>
			<asp:ListItem Text="noindex" Value="noindex"/>
			<asp:ListItem Text="nofollow" Value="nofollow"/>
			<asp:ListItem Text="noindex, nofollow" Value="noindex, nofollow"/>
			<asp:ListItem Text="noarchive" Value="noarchive"/>
			<asp:ListItem Text="nosnippet" Value="nosnippet"/>
			<asp:ListItem Text="notranslate" Value="notranslate"/>
			<asp:ListItem Text="noimageindex" Value="noimageindex"/>			
		</asp:DropDownList>
    </div>	
</fieldset>