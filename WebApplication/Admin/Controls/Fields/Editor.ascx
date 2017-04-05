<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.Editor" %>

<asp:Panel runat="server" ID="AdminPanel">    
    <asp:TextBox runat="server" CssClass="editor" ID="Instance" TextMode="MultiLine"></asp:TextBox>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>