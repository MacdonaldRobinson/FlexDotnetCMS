<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateNav.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.GenerateNav" %>

<asp:Panel runat="server" ID="AdminPanel">
<%  var uniqueId = Guid.NewGuid(); %>
<div id="tabs" class="tabs">
    <ul>
        <li><a href="#main-<%=uniqueId %>">Main</a></li>
        <li><a href="#field-settings-<%=uniqueId %>">Field Settings</a></li>
    </ul>
    <div id="main-<%=uniqueId %>">
        <label>ID of the root page</label>
        <asp:TextBox runat="server" ID="MediaID" />
        <asp:HiddenField ID="FieldValue" runat="server" />
    </div>
    <div id="field-settings-<%=uniqueId %>">
        <fieldset>
            <div>
                <asp:CheckBox ID="RenderRootPage" runat="server" /> Render Root Page
            </div> 
            <div>
                <asp:CheckBox ID="RenderBackButton" runat="server" /> Render Back Button
            </div> 
            <div>
                <label>Top Level Anchor Classes</label>
                <asp:PlaceHolder runat="server" ID="TopLevelAnchorClasses" />
            </div>
            <div>
                <label>Sub Anchor Classes</label>
                <asp:PlaceHolder runat="server" ID="SubAnchorClasses" />
            </div>
            <div>
                <label>Sub UL Classes</label>
                <asp:PlaceHolder runat="server" ID="SubULClasses" />
            </div>
            <div>
                <label>Render Parent Item In ChildNav</label>
                <asp:PlaceHolder runat="server" ID="RenderParentItemInChildNav" />
            </div>
        </fieldset>
    </div>
</div>   
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>