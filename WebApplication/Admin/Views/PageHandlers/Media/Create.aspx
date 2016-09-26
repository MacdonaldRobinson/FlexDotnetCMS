<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.MediaArticle.Create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Create New Child Item</h1>
    What would you like to create:<br />
    <Admin:MediaTypeSelector ID="MediaTypeSelector" runat="server" />
    <div class="buttons">
        <asp:LinkButton ID="CreateMedia" runat="server" OnClick="CreateMedia_OnClick">Create</asp:LinkButton>
    </div>
</asp:Content>