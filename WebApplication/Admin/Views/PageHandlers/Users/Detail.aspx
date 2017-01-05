<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.Users.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h1>
    <div>
        <label for="<%= ProfilePhoto.ClientID %>">
            Profile Photo:
            <asp:LinkButton ID="GetGravatarUrl" runat="server" OnClick="GetGravatarUrl_OnClick" Text="Get Gravatar Based On Email" />
            |
            <asp:LinkButton ID="GetIdenticonUrl" runat="server" OnClick="GetIdenticonUrl_OnClick" Text="Get Identicon Based On Email" />
            |</label>
        <Admin:FileSelector ID="ProfilePhoto" runat="server" DirPath="~/media/uploads/images/profile-photos" />
    </div>
    <div>
        <label for="<%= Username.ClientID %>">
            Username</label>
        <asp:TextBox ID="Username" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= FirstName.ClientID %>">
            First Name</label>
        <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= LastName.ClientID %>">
            Last Name</label>
        <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= Password.ClientID %>">
            Password</label>
        <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
        <small>(NOTE: leave the password field empty, if you want to keep the same password
            )</small>
    </div>
    <div>
        <label for="<%= EmailAddress.ClientID %>">
            Email Address</label>
        <asp:TextBox ID="EmailAddress" runat="server"></asp:TextBox>
    </div>
    <div>
        <label for="<%= AfterLoginStartPage.ClientID %>">
            After Login Start Page</label>
        <asp:TextBox ID="AfterLoginStartPage" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:CheckBox ID="IsActive" runat="server" Checked="true" />
        <label for="<%= IsActive.ClientID %>">
            Is Active</label>
    </div>
    <div>
        <label for="<%= RolesList.ClientID %>">
            Roles:</label>
        <asp:CheckBoxList ID="RolesList" runat="server" RepeatLayout="UnorderedList">
        </asp:CheckBoxList>
    </div>
    <div class="buttons">
        <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick" CssClass="SavePageButton">Save</asp:LinkButton>
    </div>
</asp:Content>