<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageSwitcher.ascx.cs"
    Inherits="WebApplication.Controls.LanguageSwitcher" %>

<asp:DropDownList runat="server" AppendDataBoundItems="true" ID="LanguageSelect" OnSelectedIndexChanged="LanguageSelect_SelectedIndexChanged" AutoPostBack="true">
    <asp:ListItem Text="--- Select a Language ---" Value="" />
</asp:DropDownList>