<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiRolesSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Selectors.MultiRolesSelector" %>
<div>
    <Admin:RolePermissionsSelector ID="RolePermissionsSelector" runat="server" />
    <asp:Button ID="Add" runat="server" Text="Add" OnClick="Add_OnClick" />
</div>
<br />
<telerik:RadToolBar ID="MediaGridToolbar" AutoPostBack="true" OnButtonClick="MediaGridToolbar_OnButtonClick"
    runat="server">
    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
    <Items>
        <telerik:RadToolBarButton runat="server" Text="Delete Role" CommandName="Delete">
        </telerik:RadToolBarButton>
    </Items>
</telerik:RadToolBar>
<telerik:RadGrid ID="ItemList" runat="server" AutoGenerateColumns="False" GridLines="None"
    AllowPaging="true" AllowSorting="true" PageSize="10" Skin="Default" OnNeedDataSource="ItemList_OnNeedDataSource"
    AllowCustomPaging="true">
    <ClientSettings ReorderColumnsOnClient="True" EnablePostBackOnRowClick="false">
        <Selecting AllowRowSelect="true" />
    </ClientSettings>
    <PagerStyle Mode="NumericPages" />
    <MasterTableView AutoGenerateColumns="False" DataKeyNames="Key.ID" ClientDataKeyNames="Key.ID">
        <RowIndicatorColumn Visible="False">
            <HeaderStyle Width="20px"></HeaderStyle>
        </RowIndicatorColumn>
        <ExpandCollapseColumn Visible="False" Resizable="False">
            <HeaderStyle Width="20px"></HeaderStyle>
        </ExpandCollapseColumn>
        <Columns>
            <telerik:GridBoundColumn DataField="Key.ID" HeaderText="ID" SortExpression="Key.ID" UniqueName="ID">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Key.Name" HeaderText="Name" SortExpression="Key.Name"
                UniqueName="Name">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn UniqueName="Permissions" HeaderText="Item Role Permissions">
                <ItemTemplate>
                    <asp:ListView ID="PermissionsList" runat="server" DataSource='<%# Eval("Value") %>'>
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <%# Eval("Name")%>
                            |
                        </ItemTemplate>
                    </asp:ListView>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
        <EditFormSettings>
            <PopUpSettings ScrollBars="None"></PopUpSettings>
        </EditFormSettings>
    </MasterTableView>
</telerik:RadGrid>