<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="Blog.aspx.cs" Inherits="WebApplication.Views.MediaTypeHandlers.Blog" %>


<asp:PlaceHolder runat="server" ID="TemplateTopSegment" />

<div class="blog">

    <asp:ListView runat="server" ID="BlogPosts" ItemType="FrameworkLibrary.Page">
        <LayoutTemplate>
            <div class="blog-post-list">
                <ul>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                </ul>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <h3><a href="<%# Item.AbsoluteUrl %>"><%# Item.SectionTitle %></a></h3>
                <em><%# Item.RenderShortCode("{Field:ArticlePublishDate}") %></em>
                <%# Item.ShortDescription %>
                <a class="btn aqua no-margin" href="<%# Item.AbsoluteUrl %>">Read More</a>
            </li>
        </ItemTemplate>
    </asp:ListView>

    <asp:ListView runat="server" ID="BlogCategories" ItemType="FrameworkLibrary.Page">
        <LayoutTemplate>
            <div class="blog-categories">
                <h3>Categories</h3>
                <ul>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                </ul>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <a href="<%# Item.AbsoluteUrl %>" class='<%# (Item.ID == CurrentMediaDetail.ID)? "current":"" %>'><%# Item.SectionTitle %></a>
            </li>
        </ItemTemplate>
    </asp:ListView>
</div>
<div class="blog-pager">
    <Site:Pager runat="server" PageSize="10" PagedControlID="BlogPosts" />
</div>

<asp:PlaceHolder runat="server" ID="TemplateBottomSegment" />