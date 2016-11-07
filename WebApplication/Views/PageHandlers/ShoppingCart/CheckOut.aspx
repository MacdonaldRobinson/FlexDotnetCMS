<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="WebApplication.Views.PageHandlers.ShoppingCart.CheckOut" %>

<%= CurrentMediaDetail.LongDescription %>
<asp:LinkButton ID="PayNow" runat="server" OnClick="PayNow_OnClick">Pay Now</asp:LinkButton>
