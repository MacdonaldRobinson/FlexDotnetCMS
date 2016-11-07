<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication.Views.PageHandlers.Login" %>

<asp:PlaceHolder runat="server" ID="DynamicContent" />
<form runat="server">
    <Site:Login ID="LoginControl" runat="server" />
</form>
