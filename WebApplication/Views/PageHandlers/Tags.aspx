<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template2.Master" AutoEventWireup="true" CodeBehind="Tags.aspx.cs" Inherits="WebApplication.Views.PageHandlers.Tags" %>
<asp:ListView runat="server" ID="MediaDetailsList">
    <LayoutTemplate>
        <ul>
            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"/>
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <a href="<%# Eval("AbsoluteUrl") %>"><%# Eval("SectionTitle") %></a>
            <p>
                <%# Eval("ShortDescription") %>
            </p>
        </li>
    </ItemTemplate>
</asp:ListView>
