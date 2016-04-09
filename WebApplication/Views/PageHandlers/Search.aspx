<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="WebApplication.Views.PageHandlers.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
    <Site:Search ID="SearchControl" runat="server" />
</asp:Content>