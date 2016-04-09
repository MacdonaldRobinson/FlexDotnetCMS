<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.GlossaryTerms.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h1>
    <div>
        <label for="<%= Term.ClientID %>">
            Term</label>
        <asp:TextBox ID="Term" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= Definition.ClientID %>">
            Definition</label>
        <asp:TextBox ID="Definition" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
    </div>
    <div class="buttons">
        <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick">Save</asp:LinkButton>
    </div>
</asp:Content>