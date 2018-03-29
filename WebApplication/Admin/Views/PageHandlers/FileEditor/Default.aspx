<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.FileEditor.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <label>Select a file</label>
        <Admin:FileSelector id="FileSelector" runat="server" />
        <div class="buttons">
            <asp:LinkButton ID="LoadFromFile" runat="server" OnClick="LoadFromFile_Click">Load From File</asp:LinkButton>
        </div>
        <div class="clear"></div>
    </div>
    <div>
        <label>File Content</label>
        <div>
            <asp:TextBox runat="server" ID="Editor" CssClass="AceEditor" TextMode="MultiLine" Height="700px"></asp:TextBox>
        </div>
        <div class="buttons">
            <asp:LinkButton runat="server" ID="Save" OnClick="Save_Click">Save</asp:LinkButton>
        </div>
    </div>

</asp:Content>
