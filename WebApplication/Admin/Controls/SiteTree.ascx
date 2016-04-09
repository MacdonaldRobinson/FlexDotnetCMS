<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteTree.ascx.cs" Inherits="WebApplication.Admin.Controls.SiteTree" %>

<script type="text/javascript" src="/Scripts/jstree/dist/jstree.min.js"></script>
<link rel="stylesheet" href="/Scripts/jstree/dist/themes/default/style.min.css" />
<script t type="text/javascript">
    $(document).ready(function () {
        BindJSTree();
    });

    function pageLoad(sender, args) { 
        
    }

    function RefreshSiteTreeViewAjaxPanel() {
        __doPostBack('<%= SiteTreeViewAjaxPanel.ClientID %>', '');        
    }

    function BindJSTree()
    {
        $('.jstree').jstree();
    }

</script>

<ul id="SiteTreeContextMenu">
    <li><a href="#" data-action="CreateChild">Create Child</a></li>
    <li><a href="#" data-action="Delete">Delete</a></li>
    <li><a href="#" data-action="UnDelete">Un-Delete</a></li>
    <li><a href="#" data-action="DeletePermanently">Delete Permanently</a></li>
    <li><a href="#" data-action="ShowInMenu">Show In Menu</a></li>
    <li><a href="#" data-action="HideFromMenu">Hide From Menu</a></li>
    <li><a href="#" data-action="MoveUp">Move Up</a></li>
    <li><a href="#" data-action="MoveDown">Move Down</a></li>
    <li><a href="#" data-action="ViewFrontEnd">View Front End</a></li>
    <li><a href="#" data-action="Duplicate">Duplicate</a></li>
    <li><a href="#" data-action="DuplicateAndEdit">Duplicate And Edit</a></li>
</ul>



    <asp:UpdatePanel runat="server" ID="SiteTreeViewAjaxPanel">
        <ContentTemplate>
            <script type="text/javascript">
                BindJSTree();
            </script>
            <div id="SiteTree" class="jstree">
                <asp:ListView runat="server" ID="ListView" OnItemDataBound="ListView_ItemDataBound">
                    <LayoutTemplate>
                        <ul>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="Link" runat="server" />
                            <asp:ListView runat="server" ID="ChildListView" OnItemDataBound="ListView_ItemDataBound">
                                <LayoutTemplate>
                                </LayoutTemplate>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:ListView>

                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
