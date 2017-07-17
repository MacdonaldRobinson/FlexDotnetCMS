<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.FieldSelector.Default" %>

<%@ Register Src="~/Admin/Controls/MediaTypes/Common/Tabs/FieldsTab.ascx" TagPrefix="Admin" TagName="FieldsTab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    <asp:Panel runat="server" ID="MediaTypeFieldsEditorWrapper" Visible="false">
        <Admin:MediaTypeFieldsEditor runat="server" id="MediaTypeFieldsEditor" />
    </asp:Panel>
    <asp:Panel runat="server" ID="MediaDetailFieldsEditorWrapper" Visible="false">
        <Admin:FieldsTab runat="server" ID="FieldsTab" />
    </asp:Panel>

</asp:Content>
