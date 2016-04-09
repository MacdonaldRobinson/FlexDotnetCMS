<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="WebApplication.Views.PageHandlers.ShoppingCart.CheckOut" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <%= CurrentMediaDetail.LongDescription %>
    <asp:LinkButton ID="PayNow" runat="server" OnClick="PayNow_OnClick">Pay Now</asp:LinkButton>
</asp:Content>
