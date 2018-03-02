<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.GlossaryTerms.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Literal ID="Section" runat="server"></asp:Literal></h1>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel">
        <ContentTemplate>
            <a href="javascript:void(0);" onclick="executeAction('Create','', '<%= UpdatePanel.ClientID %>');">Create New</a>
            <%-- <a href="javascript:void(0);" onclick="executeAction('TakeOwnership','', '<%= UpdatePanel.ClientID %>');">TakeOwnership</a> --%>
            <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="false" OnPageIndexChanging="ItemList_PageIndexChanging" OnSorting="ItemList_Sorting" OnDataBound="ItemList_DataBound" class="DataTable">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:TemplateField HeaderText="Term">
                        <ItemTemplate>
                            <img src="<%# (!Eval("Term").ToString().StartsWith("/")) ? "#" : Eval("Term")  %>" alt="" style='max-width:50px;<%# (Eval("Term").ToString().StartsWith("/")) ? "" : "display:none"  %>'/>
                            <%# (!Eval("Term").ToString().StartsWith("/")) ? Eval("Term") : ""  %>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="Definition" HeaderText="Definition" SortExpression="Definition" />
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