<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RssReader.ascx.cs" Inherits="WebApplication.Controls.Widgets.RssReader" %>

<asp:ListView runat="server" ID="RssItems" OnItemDataBound="RssItems_ItemDataBound">
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
    </LayoutTemplate>
    <ItemTemplate>
        <h2>
            <asp:Literal runat="server" ID="Header" /></h2>
        <div class="careers-blog-post-info">
            <p>
                <asp:Literal runat="server" ID="PostDate" />
            </p>
        </div>
        <asp:Literal ID="Description" runat="server" />
    </ItemTemplate>
</asp:ListView>