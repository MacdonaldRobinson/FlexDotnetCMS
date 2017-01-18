<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaDetailsGrid.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Generic.MediaDetailsGrid" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ItemList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="MediaType.Name" HeaderText="MediaType" SortExpression="MediaType.Name" />
                <asp:BoundField DataField="Language.Name" HeaderText="Language" SortExpression="Language.Name" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="Edit" runat="server" OnClick="Edit_Click">Edit</asp:LinkButton> |
                        <asp:HyperLink ID="PreviewHistory" runat="server" Target="_blank" CssClass="colorbox iframe" NavigateUrl='<%# Eval("AbsoluteUrl") +"?version="+ Eval("HistoryVersionNumber") %>'>Preview</asp:HyperLink> |
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>

<%--<asp:UpdatePanel runat="server">
    <ContentTemplate>
    <telerik:RadGrid ID="ItemList" runat="server" AutoGenerateColumns="False" GridLines="None"
        AllowPaging="true" AllowSorting="true" PageSize="10"
        AllowCustomPaging="False" AllowFilteringByColumn="true">
        <ClientSettings ReorderColumnsOnClient="True" EnablePostBackOnRowClick="false">
            <Selecting AllowRowSelect="true" />
        </ClientSettings>
        <GroupingSettings CaseSensitive="False"></GroupingSettings>
        <PagerStyle Mode="NumericPages" />
        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" ClientDataKeyNames="ID">
            <RowIndicatorColumn Visible="False">
                <HeaderStyle Width="20px"></HeaderStyle>
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="False" Resizable="False">
                <HeaderStyle Width="20px"></HeaderStyle>
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn DataField="ID" HeaderText="ID" SortExpression="ID" UniqueName="ID"
                    AutoPostBackOnFilter="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title"
                    UniqueName="Title" AutoPostBackOnFilter="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="MediaType.Name" HeaderText="Media Type" SortExpression="MediaType.Name"
                    UniqueName="MediaType.Name" AutoPostBackOnFilter="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Language.Name" HeaderText="Language" SortExpression="Language.Name"
                    UniqueName="Language.Name" AutoPostBackOnFilter="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateCreated" HeaderText="Date Created" SortExpression="DateCreated"
                    UniqueName="DateCreated" AutoPostBackOnFilter="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateLastModified" HeaderText="Date Last Modified"
                    SortExpression="DateLastModified" UniqueName="DateLastModified" AutoPostBackOnFilter="true">
                </telerik:GridBoundColumn>
            </Columns>
            <EditFormSettings>
                <PopUpSettings ScrollBars="None"></PopUpSettings>
            </EditFormSettings>
        </MasterTableView>
    </telerik:RadGrid>
</ContentTemplate>
</asp:UpdatePanel>--%>