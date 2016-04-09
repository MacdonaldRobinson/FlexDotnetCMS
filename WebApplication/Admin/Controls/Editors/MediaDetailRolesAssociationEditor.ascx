<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaDetailRolesAssociationEditor.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaDetailRolesAssociationEditor" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>

        <asp:LinkButton Text="Add Role" runat="server" ID="AddRole" OnClick="AddRole_Click" />
        <asp:LinkButton Text="Delete Role" runat="server" ID="DeleteRole" OnClick="DeleteRole_Click" />

        <asp:Panel ID="EditPanel" runat="server" Visible="false">
            <fieldset>
                <Admin:RolePermissionsSelector ID="RolePermissionsSelector" runat="server" />
                <div>
                    <asp:Button ID="Save" runat="server" Text="Save" OnClick="Save_OnClick" />
                    <asp:Button ID="Cancel" runat="server" Text="Cancel" OnClick="Cancel_OnClick" />
                </div>
            </fieldset>
            <br />
        </asp:Panel>
        <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ItemList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:TemplateField HeaderText="Permissions">
                    <ItemTemplate>
                        <asp:ListView ID="PermissionsList" runat="server" DataSource='<%# Eval("RolesMediaDetails") %>'>
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <%# Eval("Permission.Name")%> |
                            </ItemTemplate>
                        </asp:ListView>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>