<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.Tags.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="tabs" class="tabs">
  <ul>
    <li><a href="#Main">Main</a></li>
    <li><a href="#AssociatedMediaItems">Associated Media Items</a></li>
  </ul>
  <div id="Main">
      <fieldset>
        <h1>
            <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h1>
        <div>
            <label for="<%= Name.ClientID %>">Name</label>
            <asp:TextBox ID="Name" runat="server"></asp:TextBox>
        </div>
        <div>
            <label for="<%= Description.ClientID %>">Description</label>
            <asp:TextBox ID="Description" runat="server" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div>
            <label for="<%= ThumbnailPath.ClientID %>">Thumbnail Path</label>
            <Admin:FileSelector ID="ThumbnailPath" runat="server" DirPath="~/media/uploads/images/thumbnails/" />
        </div>
        <div class="buttons">
            <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick" CssClass="SavePageButton">Save</asp:LinkButton>
        </div>
    </fieldset>
  </div>
  <div id="AssociatedMediaItems">
      <fieldset>
        <Admin:TagsAssociationEditor ID="TagsAssociationEditor" runat="server" />
        </fieldset>
  </div>
</div>
</asp:Content>