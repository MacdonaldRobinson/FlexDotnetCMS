<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.Scheduler.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Manage Scheduled Taks</h1>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel">
        <ContentTemplate>
            <div class="buttons">
                <a href="javascript:void(0);" onclick="executeAction('Create','', '<%= UpdatePanel.ClientID %>');">Create New</a>
            </div>
            <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="false" OnPageIndexChanging="ItemList_PageIndexChanging" OnSorting="ItemList_Sorting" PageSize="50" OnDataBound="ItemList_DataBound" class="DataTable">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
					<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                    <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <div class="buttons">
                                <a href="javascript:void(0);" onclick="executeAction('Edit', '<%# Eval("ID") %>', '<%= UpdatePanel.ClientID %>')">Edit</a>
                                <a href="javascript:void(0);" onclick="executeAction('DeletePermanently', '<%# Eval("ID") %>', '<%= UpdatePanel.ClientID %>')">Delete Permanently</a>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
