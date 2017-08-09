<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.FieldSelector.Default" %>

<%@ Register Src="~/Admin/Controls/MediaTypes/Common/Tabs/FieldsTab.ascx" TagPrefix="Admin" TagName="FieldsTab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    <fieldset>
        <legend>Common Fields</legend>
        <asp:GridView runat="server" ID="CommonFields" AutoGenerateColumns="false" AllowPaging="true" CssClass="DragDropGrid" OnPageIndexChanging="ItemList_PageIndexChanging" PageSize="10">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                <asp:BoundField DataField="OrderIndex" HeaderText="OrderIndex" SortExpression="OrderIndex" />
                <asp:BoundField DataField="FieldCode" HeaderText="FieldCode" SortExpression="FieldCode" />
                <asp:BoundField DataField="FieldLabel" HeaderText="FieldLabel" SortExpression="FieldLabel" />
                <asp:BoundField DataField="GroupName" HeaderText="GroupName" SortExpression="GroupName" />
                <asp:BoundField DataField="MediaTypeFieldID" HeaderText="MediaTypeFieldID" SortExpression="MediaTypeFieldID" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <%--<asp:LinkButton ID="Edit" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Edit_Click">Edit</asp:LinkButton> |--%>
                        <asp:LinkButton ID="Select" runat="server" OnClientClick='parent.UpdateVisualEditor(this)' data-mediadetailid='<%# Eval("MediaDetail.ID") %>' data-fieldCode='<%# Eval("FieldCode") %>'>Select</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>

    <asp:Panel runat="server" ID="MediaTypeFieldsEditorWrapper" Visible="false">
        <Admin:MediaTypeFieldsEditor runat="server" id="MediaTypeFieldsEditor" />
    </asp:Panel>
    <asp:Panel runat="server" ID="MediaDetailFieldsEditorWrapper" Visible="false">
        <Admin:FieldsTab runat="server" ID="FieldsTab" />
    </asp:Panel>

</asp:Content>
