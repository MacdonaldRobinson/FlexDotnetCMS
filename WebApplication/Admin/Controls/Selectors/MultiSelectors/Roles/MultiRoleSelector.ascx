<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiRoleSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Selectors.MultiRolesSelector" %>
<div>
    <Admin:RoleSelector runat="server" id="RoleSelector" />
    <asp:Button ID="Add" runat="server" Text="Add" OnClick="Add_OnClick" />
</div>
<br />

<asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ItemList_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:LinkButton Text="Delete" runat="server" ID="Delete" CommandArgument='<%# Eval("ID") %>' OnClick="Delete_Click" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>