<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.FieldFiles.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h1>
    <div>
        <label for="<%= Name.ClientID %>">
            Name</label>
        <asp:TextBox ID="Name" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= PathToFile.ClientID %>">
            Path To File</label>
        <Admin:FileSelector ID="PathToFile" runat="server" />
    </div>
    <div>
        <label for="<%= Link.ClientID %>">
            Link</label>
        <asp:TextBox ID="Link" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= Description.ClientID %>">
            Description</label>
        <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" CssClass="editor" Height="250px"></asp:TextBox>
    </div>
    <div class="buttons">
        <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick">Save</asp:LinkButton>
    </div>
</asp:Content>
