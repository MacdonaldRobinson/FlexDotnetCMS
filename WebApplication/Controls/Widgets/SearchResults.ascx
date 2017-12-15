<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResults.ascx.cs" Inherits="WebApplication.Controls.SearchResults" %>

<asp:ListView runat="server" ID="Results" ItemType="FrameworkLibrary.MediaDetail">
    <LayoutTemplate>
        <ul>
            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <a href="<%# Item.AbsoluteUrl %>"><%# Item.SectionTitle %></a>
            <p>
                <%# Item.GetMetaDescription() %>
            </p>
        </li>
    </ItemTemplate>
</asp:ListView>
