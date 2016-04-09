<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChildrensTab.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.ChildrensTab" %>

<asp:UpdatePanel runat="server" ID="ChildrenUpdatePanel">
    <ContentTemplate>
        <fieldset>
            <legend>Currently Children</legend>
            <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="true" CssClass="DragDropGrid DataTable" OnPageIndexChanging="ItemList_PageIndexChanging" OnDataBound="ItemList_DataBound" Width="100%">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="SectionTitle" HeaderText="SectionTitle" SortExpression="SectionTitle" />
                    <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                    <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="Edit" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Edit_Click">Edit</asp:LinkButton>|
                            <asp:LinkButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Delete_Click" OnClientClick="return confirm('Are you sure you want to perminently delete this field? you will loose all data that has been assigned to this field.')">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>