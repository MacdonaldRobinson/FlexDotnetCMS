<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaDetailHistoryEditor.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Editors.MediaDetailHistoryEditor" %>
<script type="text/javascript">
    function PreviewInIFrame(src) {
        //$("#PreviewIFrame").attr("src", src);
        return false;
    }
</script>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="false" OnPageIndexChanging="ItemList_PageIndexChanging" Width="100%" CssClass="DataTable" OnDataBound="ItemList_DataBound">
            <Columns>
                <%--<asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />--%>
                <asp:BoundField DataField="IsDraft" HeaderText="IsDraft" SortExpression="IsDraft" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <%--<asp:BoundField DataField="VirtualPath" HeaderText="VirtualPath" SortExpression="VirtualPath" />--%>
                <asp:BoundField DataField="HistoryVersionNumber" HeaderText="Version" SortExpression="HistoryVersionNumber" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                <%--<asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />--%>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="LoadHistory" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="LoadHistory_Click">Load</asp:LinkButton> |
                        <asp:HyperLink ID="PreviewHistory" runat="server" CssClass="preview" NavigateUrl='<%# Eval("AbsoluteUrl") +"?version="+ Eval("HistoryVersionNumber") %>'>Preview</asp:HyperLink> |
                        <asp:LinkButton ID="DeleteHistory" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="DeleteHistory_Click">Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>