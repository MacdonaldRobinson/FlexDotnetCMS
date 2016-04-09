<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RssListView.ascx.cs" Inherits="WebApplication.Controls.RssListView" %>

<asp:ListView ID="ItemsList" runat="server" OnItemDataBound="ItemsList_OnItemDataBound">
    <LayoutTemplate>
        <h4>
            <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h4>
        <ul class="items">
            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li class="item">
            <h4>
                <asp:HyperLink ID="Item" runat="server"></asp:HyperLink></h4>
            <asp:Panel ID="PostInfo" runat="server">
                Posted on <strong>
                    <asp:Literal ID="CreatedOn" runat="server" /></strong> by <strong>
                        <asp:Literal ID="CreatedBy" runat="server" /></strong><br />
                <strong>
                    <asp:Literal ID="NumberOfComments" runat="server" />
                    Comment(s)</strong> | Tag(s): <strong>
                        <asp:Literal ID="Tags" runat="server" /></strong><br />
                <br />
            </asp:Panel>
            <div>
                <asp:Literal ID="Description" runat="server"></asp:Literal>
            </div>
            <asp:HyperLink ID="ReadMore" runat="server">Read More >></asp:HyperLink>
        </li>
    </ItemTemplate>
</asp:ListView>

<Site:Pager ID="Pager" runat="server" PagedControlID="ItemsList" />