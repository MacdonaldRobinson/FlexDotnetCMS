<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.MediaArticle.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--    <h1>
        <asp:Literal ID="Section" runat="server"></asp:Literal></h1>
    <telerik:RadToolBar ID="RadToolBar" AutoPostBack="false" OnClientButtonClicked="clientButtonClicked"
        runat="server">
        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
        <Items>
            <telerik:RadToolBarButton runat="server" Text="Edit selected" Value="Edit">
            </telerik:RadToolBarButton>
            <telerik:RadToolBarButton IsSeparator="true">
            </telerik:RadToolBarButton>
            <telerik:RadToolBarButton runat="server" Text="Delete selected" Value="Delete">
            </telerik:RadToolBarButton>
            <telerik:RadToolBarButton IsSeparator="true">
            </telerik:RadToolBarButton>
            <telerik:RadToolBarButton runat="server" Text="Create new" Value="Create">
            </telerik:RadToolBarButton>
        </Items>
    </telerik:RadToolBar>
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
                <telerik:GridBoundColumn DataField="ShortDescription" HeaderText="Short Description"
                    SortExpression="ShortDescription" UniqueName="ShortDescription" AutoPostBackOnFilter="true">
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
    </telerik:RadGrid>--%>
</asp:Content>