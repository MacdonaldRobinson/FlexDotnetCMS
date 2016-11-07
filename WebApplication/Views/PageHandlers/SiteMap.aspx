<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master"
    AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs" Inherits="WebApplication.Views.PageHandlers.SiteMap" %>

<asp:PlaceHolder runat="server" ID="DynamicContent" />
<Site:GenerateNav ID="SiteMapTree" runat="server" RootVirtualPath="~/" RenderDepth="-1"
    RenderRootMedia="false" RenderFooterMenuItems="true" />