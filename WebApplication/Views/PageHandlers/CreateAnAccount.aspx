<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template2.Master" AutoEventWireup="true" CodeBehind="CreateAnAccount.aspx.cs" Inherits="WebApplication.Views.PageHandlers.CreateAnAccount" %>

<form id="form" runat="server">
    <div>
        <label for="FirstName">First Name</label>
        <asp:TextBox runat="server" ID="FirstName" ClientIDMode="Static"></asp:TextBox>
    </div>
    <div>
        <label for="LastName">Last Name</label>
        <asp:TextBox runat="server" ID="LastName" ClientIDMode="Static"></asp:TextBox>
    </div>
    <div>
        <label for="EmailAddress">Email Address</label>
        <asp:TextBox runat="server" ID="EmailAddress" ClientIDMode="Static"></asp:TextBox>
    </div>
    <div>
        <label for="Password">Password</label>
        <asp:TextBox runat="server" ID="Password" ClientIDMode="Static" TextMode="Password"></asp:TextBox>
    </div>
    <div>
        <label for="Category">Category</label>
        <asp:DropDownList runat="server" ID="Category" ClientIDMode="Static">
            <asp:ListItem Value="Teacher"></asp:ListItem>
            <asp:ListItem Value="Student"></asp:ListItem>
            <asp:ListItem Value="General"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div>
        <asp:CheckBox runat="server" ID="SendMeNews" ClientIDMode="Static"></asp:CheckBox>
        <label for="SendMeNews">Send me news on Canadian Valour</label>
    </div>
    <div>
        <asp:LinkButton runat="server" ID="Create" OnClick="Create_OnClick">Create</asp:LinkButton>
    </div>
    <div>
        <asp:Literal runat="server" ID="Message"></asp:Literal>
    </div>
</form>