<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BootStrapTabbedLayout.ascx.cs" Inherits="WebApplication.Controls.BootStrap.BootStrapTabbedLayout" %>

<div class="tabbable tabs-left">
    <asp:ListView runat="server" ID="NavTabs">
        <LayoutTemplate>
            <ul class="nav nav-tabs">
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="<%# (Container.DataItemIndex == 0)? "active": "" %>"><a data-toggle="tab" href="#TabContentIndex<%# Container.DataItemIndex %>"><%# Eval("SectionTitle") %></a></li>
        </ItemTemplate>
    </asp:ListView>
    <asp:ListView runat="server" ID="NavTabsContents" OnItemDataBound="NavTabsContents_OnItemDataBound">
        <LayoutTemplate>
            <div class="tab-content">
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div id="TabContentIndex<%# Container.DataItemIndex %>" class="tab-pane <%# (Container.DataItemIndex == 0)? "active": "" %>">
                <%# Eval("ShortDescription") %>
                <a href='<%#Eval("AbsoluteUrl") %>' runat="server" id="LinkToDetailsPage">Continue Reading >></a>
            </div>
        </ItemTemplate>
    </asp:ListView>
</div>