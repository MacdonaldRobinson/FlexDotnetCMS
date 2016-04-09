<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.Permissions.Detail" %>

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
            Enum Name</label>
        <asp:TextBox ID="EnumName" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= Description.ClientID %>">
            Description</label>
        <asp:TextBox ID="Description" runat="server" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div>
        <label for="<%= IsActive.ClientID %>">
            IsActive</label>
        <asp:CheckBox ID="IsActive" runat="server" />
    </div>
    <div>
        <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick">Save</asp:LinkButton>
    </div>
</asp:Content>