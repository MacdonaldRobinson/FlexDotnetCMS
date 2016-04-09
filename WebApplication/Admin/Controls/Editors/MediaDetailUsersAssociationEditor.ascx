<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaDetailUsersAssociationEditor.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Editors.MediaDetailUsersAssociationEditor" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="EditItemID" runat="server" />

        <asp:LinkButton Text="Add User" ID="AddUser" OnClick="AddUser_Click" runat="server" />
        <asp:LinkButton Text="Delete User" ID="DeleteUser" OnClick="DeleteUser_Click" runat="server" />

        <asp:Panel ID="EditPanel" runat="server" Visible="false">
            <fieldset>
                <Admin:UserPermissionsSelector ID="UserPermissionsSelector" runat="server" />
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
                <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                <asp:TemplateField HeaderText="Roles">
                    <ItemTemplate>
                        <asp:ListView ID="PermissionsList" runat="server" DataSource='<%# Eval("Roles") %>'>
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
                <asp:TemplateField HeaderText="Permissions">
                    <ItemTemplate>
                        <asp:ListView ID="PermissionsList" runat="server" DataSource='<%# Eval("UsersMediaDetails") %>'>
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <%# Eval("Permission.Name")%>
                            |
                            </ItemTemplate>
                        </asp:ListView>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>