<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master"
    AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs" Inherits="WebApplication.Views.PageHandlers.SiteMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
    <Site:GenerateNav ID="SiteMapTree" runat="server" RootVirtualPath="~/" RenderDepth="-1"
        RenderRootMedia="false" RenderFooterMenuItems="true" />
</asp:Content>