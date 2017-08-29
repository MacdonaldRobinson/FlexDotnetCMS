<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateNav.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.GenerateNav" %>

<asp:Panel runat="server" ID="AdminPanel">
    <fieldset>
        <div>
            <label>ID</label>
            <asp:TextBox runat="server" ID="MediaID" />
        </div>
        <div>
            <asp:CheckBox ID="RenderRootPage" runat="server" /> Render Root Page
        </div>
        <asp:HiddenField ID="FieldValue" runat="server" />    
    </fieldset>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>