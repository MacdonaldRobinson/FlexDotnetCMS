<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.Roles.Detail" %>

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
        <label for="<%= EnumName.ClientID %>">
            EnumName</label>
        <asp:TextBox ID="EnumName" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= Description.ClientID %>">
            Description</label>
        <asp:TextBox ID="Description" runat="server" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div>
        <label for="<%= PermissionsSelector.ClientID %>">
            Permissions</label>
        <Admin:PermissionsSelector ID="PermissionsSelector" runat="server" />
    </div>
    <div>
        <asp:CheckBox ID="IsActive" runat="server" Checked="true" />
        <label for="<%= IsActive.ClientID %>">
            Is Active</label>
    </div>
    <div class="buttons">
        <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick" CssClass="SavePageButton">Save</asp:LinkButton>
    </div>
</asp:Content>