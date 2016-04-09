<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="WebApplication.Controls.Search" %>
<asp:Panel runat="server" DefaultButton="SearchBtn">
    <asp:TextBox runat="server" ID="SearchTerms" ClientIDMode="Static" placeholder="Search" CssClass="search-bar"></asp:TextBox>
    <asp:LinkButton runat="server" OnClick="SearchBtn_OnClick" CssClass="search-button fa fa-search" ID="SearchBtn" />
</asp:Panel>