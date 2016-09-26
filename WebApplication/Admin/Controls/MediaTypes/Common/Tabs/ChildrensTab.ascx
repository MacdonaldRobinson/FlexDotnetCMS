<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChildrensTab.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.ChildrensTab" %>

<%--<script type="text/javascript" src="//cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js "></script>
<script type="text/javascript" src="//cdn.datatables.net/buttons/1.2.1/js/dataTables.buttons.min.js "></script>
<script type="text/javascript" src="//cdn.datatables.net/buttons/1.2.1/js/buttons.flash.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
<script type="text/javascript" src="//cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
<script type="text/javascript" src="//cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
<script type="text/javascript" src="//cdn.datatables.net/buttons/1.2.1/js/buttons.html5.min.js "></script>
<script type="text/javascript" src="//cdn.datatables.net/buttons/1.2.1/js/buttons.print.min.js"></script>--%>

<asp:UpdatePanel runat="server" ID="ChildrenUpdatePanel">
    <ContentTemplate>
        <fieldset>
            <legend>Current Children</legend>
            <div>
                <div class="floatLeft">
                    <asp:Panel runat="server" DefaultButton="SearchItems" id="SearchPanel" Visible="false">
                        <asp:TextBox runat="server" ID="SearchText" style="display:inline; width: 200px;" placeholder="Search ..." />
                        <asp:LinkButton ID="SearchItems" Text="Search" runat="server" OnClick="SearchItems_Click"/>
                    </asp:Panel>
                </div>
                <div class="buttons floatRight">
                    <a href="/Admin/Views/PageHandlers/Media/Create.aspx" id="CreateNewChildItemButton">Create a new child item</a>
                </div>
                <div class="clear"></div>
            </div>
            <asp:GridView runat="server" ID="ItemList" AllowSorting="false" AutoGenerateColumns="false" AllowPaging="true" CssClass="DragDropGrid DataTable" OnPageIndexChanging="ItemList_PageIndexChanging" OnDataBound="ItemList_DataBound" Width="100%" PageSize="20" OnSorting="ItemList_Sorting">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="SectionTitle" HeaderText="SectionTitle" SortExpression="SectionTitle" />
                    <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                    <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="Edit" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Edit_Click">Edit</asp:LinkButton>|
                            <asp:LinkButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Delete_Click" OnClientClick="return confirm('Are you sure you want to perminently delete this item? you will loose all data that has been assigned to this this.')">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>