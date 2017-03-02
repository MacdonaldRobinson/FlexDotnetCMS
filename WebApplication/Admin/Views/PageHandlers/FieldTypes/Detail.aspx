<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.FieldTypes.Detail" %>
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
        <label for="<%= CodeToRenderAdminControl.ClientID %>">
            CodeToRenderAdminControl</label>
        <asp:TextBox ID="CodeToRenderAdminControl" runat="server" TextMode="MultiLine" Width="100%" CssClass="AceEditor" Height="300px"></asp:TextBox>
    </div>
    <div>
        <label for="<%= CodeToGetAdminControlValue.ClientID %>">
            CodeToGetAdminControlValue</label>
        <asp:TextBox ID="CodeToGetAdminControlValue" runat="server" TextMode="MultiLine" Width="100%" CssClass="AceEditor" Height="300px"></asp:TextBox>
    </div>
    <div>
        <label for="<%= CodeToSetAdminControlValue.ClientID %>">
            CodeToSetAdminControlValue</label>
        <asp:TextBox ID="CodeToSetAdminControlValue" runat="server" TextMode="MultiLine" Width="100%" CssClass="AceEditor" Height="300px"></asp:TextBox>
    </div>
    <div>
        <label for="<%= CodeToRenderFrontEndLayout.ClientID %>">
            CodeToRenderFrontEndLayout</label>
        <asp:TextBox ID="CodeToRenderFrontEndLayout" runat="server" TextMode="MultiLine" Width="100%" CssClass="AceEditor" Height="300px"></asp:TextBox>
    </div>

    <div>
        <label for="<%= FieldDescription.ClientID %>">
            FieldDescription</label>
        <Admin:Editor ID="FieldDescription" runat="server" Height="200px" />
    </div>

    <div class="buttons">
        <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick" CssClass="SavePageButton">Save</asp:LinkButton>
    </div>
</asp:Content>
