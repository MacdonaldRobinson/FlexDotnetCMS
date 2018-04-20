<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPages/SiteTemplates/Template1.Master" AutoEventWireup="true" CodeBehind="Blog.aspx.cs" Inherits="WebApplication.Views.MediaTypeHandlers.Blog" %>


<asp:PlaceHolder runat="server" ID="TemplateTopSegment" />


<form runat="server">
    <div class="row">
    <asp:ListView runat="server" ID="BlogCategories" ItemType="FrameworkLibrary.Page">
        <LayoutTemplate>
            <div class="col-md-3 px-2 blog-categories">
                <div class="nav flex-column blog-sidenav" role="navigation" data-sticky data-margin-top="100">
                <h3>Blog Categories</h3>
                <ul>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                </ul>
                    </div>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <a href="<%# Item.AbsoluteUrl %>" class='<%# (Item.ID == CurrentMediaDetail.ID)? "current":"" %>'><%# Item.RenderField("SectionTitle", false) %> ( <%# Item.ChildMediaDetails.Count() %> ) </a>
            </li>
        </ItemTemplate>
    </asp:ListView>

    <asp:ListView runat="server" ID="BlogPosts" ItemType="FrameworkLibrary.Page">
        <LayoutTemplate>
            <div class="col-md-9 px-2 blog-post-list">
                <div class="row">
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                </div>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="col-lg-6 p-2">
                <div class="blog-post-card">
                 <img class="img-fluid" src="<%# Item.RenderField("SectionImage") %>" />
                <h4><a href="<%# Item.AbsoluteUrl %>"><%# Item.RenderField("SectionTitle") %></a></h4>
                <em><%# (Item.PublishDate != null)? StringHelper.FormatOnlyDate((DateTime)Item.PublishDate) : "" %></em>
                <%# Item.RenderField("ShortDescription") %>
                    <a class="button micro green alt-icon" title="Mental Health Concerns" href="<%# Item.AbsoluteUrl %>">Read more <span class="fa fa-arrow-right" aria-hidden="true"></span></a>
                    </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
    </div>
<div class="row">
    <div class="col-md-3"></div>
    <div class="col-md-9 offset-md-3 offset-sm-0">
<nav aria-label="Blog Navigation">
    <Site:Pager runat="server" PageSize="10" PagedControlID="BlogPosts" />
</nav>
    </div>
    </div>

    </form>
<asp:PlaceHolder runat="server" ID="TemplateBottomSegment" />