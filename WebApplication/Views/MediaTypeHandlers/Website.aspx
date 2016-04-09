<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="Website.aspx.cs" Inherits="WebApplication.Views.MediaTypeHandlers.Website" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">    
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Content>