<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiMediaTypeSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Selectors.MultiMediaTypeSelector" %>
<div>
    <label>
        Media Types:</label>
    <Admin:MediaTypeSelector ID="MediaTypeSelector" runat="server" />
    <asp:Button ID="Add" runat="server" Text="Add" OnClick="Add_OnClick" />
</div>
<br />

<asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ItemList_PageIndexChanging" OnSorting="ItemList_Sorting">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:LinkButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Delete_Click">Delete</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>