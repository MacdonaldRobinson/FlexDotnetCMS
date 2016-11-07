<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="WebApplication.Views.PageHandlers.Search" %>

<form runat="server">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
    <Site:Search ID="SearchControl" runat="server" />
</form>