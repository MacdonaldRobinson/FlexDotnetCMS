<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Users.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="Section" runat="server"></asp:Literal></h1>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel">
        <ContentTemplate>
            <a href="javascript:void(0);" onclick="executeAction('Create','', '<%= UpdatePanel.ClientID %>');">Create New</a>
            <%-- <a href="javascript:void(0);" onclick="executeAction('TakeOwnership','', '<%= UpdatePanel.ClientID %>');">TakeOwnership</a> --%>
            <asp:GridView class="DataTable" runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="false" OnPageIndexChanging="ItemList_PageIndexChanging" OnSorting="ItemList_Sorting" OnDataBound="ItemList_DataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" SortExpression="EmailAddress" />
                    <asp:TemplateField HeaderText="Roles">
                        <ItemTemplate>
                            <asp:ListView ID="Roles" runat="server" DataSource='<%# Eval("Roles") %>'>
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
                    <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                    <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <a href="javascript:void(0);" onclick="executeAction('Edit', '<%# Eval("ID") %>', '<%= UpdatePanel.ClientID %>')">Edit</a> |
                            <a href="javascript:void(0);" onclick="executeAction('DeletePermanently', '<%# Eval("ID") %>', '<%= UpdatePanel.ClientID %>')">Delete Permanently</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>