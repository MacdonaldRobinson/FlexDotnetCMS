<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.MediaTypes.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="Section" runat="server"></asp:Literal></h1>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel">
        <ContentTemplate>
            <a href="javascript:void(0);" onclick="executeAction('Create','', '<%= UpdatePanel.ClientID %>');">Create New</a>
            <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ItemList_PageIndexChanging" OnSorting="ItemList_Sorting" PageSize="50" Width="100%">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Label" HeaderText="Label" SortExpression="Label" />
                    <asp:BoundField DataField="MediaTypeHandler" HeaderText="MediaTypeHandler" SortExpression="MediaTypeHandler" />
                    <asp:BoundField DataField="MasterPage.Name" HeaderText="MasterPage" SortExpression="MasterPage" />
                    <asp:BoundField DataField="UseMediaTypeLayouts" HeaderText="UseMediaTypeLayouts" SortExpression="UseMediaTypeLayouts" />
                    <asp:BoundField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                    <asp:BoundField DataField="ShowInMenu" HeaderText="ShowInMenu" SortExpression="ShowInMenu" />
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