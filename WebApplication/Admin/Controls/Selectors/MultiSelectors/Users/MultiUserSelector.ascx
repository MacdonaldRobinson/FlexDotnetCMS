<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiUserSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Selectors.MultiSelectors.MultiUserSelector" %>
<div>
    <Admin:UserPermissionsSelector ID="UserPermissionsSelector" runat="server" />
    <asp:Button ID="Add" runat="server" Text="Add" OnClick="Add_OnClick" />
</div>
<br />

<asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ItemList_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="Key.ID" HeaderText="ID" SortExpression="Key.ID" />
        <asp:BoundField DataField="Key.UserName" HeaderText="UserName" SortExpression="Key.UserName" />
        <asp:TemplateField HeaderText="Permissions">
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
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:LinkButton ID="Delete" runat="server" CommandArgument='<%# Eval("Key.ID") %>' OnClick="Delete_Click">Delete</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<%--<telerik:RadToolBar ID="MediaGridToolbar" AutoPostBack="true" OnButtonClick="MediaGridToolbar_OnButtonClick"
    runat="server">
    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
    <Items>
        <telerik:RadToolBarButton runat="server" Text="Delete User" CommandName="Delete">
        </telerik:RadToolBarButton>
    </Items>
</telerik:RadToolBar>
<telerik:RadGrid ID="ItemList" runat="server" AutoGenerateColumns="False" GridLines="None"
    AllowPaging="true" AllowSorting="true" PageSize="10"
    AllowCustomPaging="False">
    <ClientSettings ReorderColumnsOnClient="True" EnablePostBackOnRowClick="false">
        <Selecting AllowRowSelect="true" />
    </ClientSettings>
    <GroupingSettings CaseSensitive="False"></GroupingSettings>
    <PagerStyle Mode="NumericPages" />
    <MasterTableView AutoGenerateColumns="False" DataKeyNames="Key.ID" ClientDataKeyNames="Key.ID">
        <RowIndicatorColumn Visible="False">
            <HeaderStyle Width="20px"></HeaderStyle>
        </RowIndicatorColumn>
        <ExpandCollapseColumn Visible="False" Resizable="False">
            <HeaderStyle Width="20px"></HeaderStyle>
        </ExpandCollapseColumn>
        <Columns>
            <telerik:GridBoundColumn DataField="Key.ID" HeaderText="ID" SortExpression="Key.ID"
                UniqueName="ID">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Key.UserName" HeaderText="UserName" SortExpression="Key.UserName"
                UniqueName="UserName">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn UniqueName="Permissions" HeaderText="Item User Permissions">
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
</telerik:RadGrid>--%>