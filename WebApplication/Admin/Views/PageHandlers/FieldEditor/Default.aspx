<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.FieldEditor.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <link href="<%= URIHelper.BaseUrl %>Admin/Styles/mediaDetails.css" rel="stylesheet" />

    <div>
        <% if (Field != null)
            { %>
            <%= (!Field.RenderLabelAfterControl) ? "<label data-fieldcode='" + Field.FieldCode + "' data-fieldid=" + Field.ID + ">" + Field.FieldLabel + "</label> <i class='fa fa-question-circle tooltip' aria-hidden='true' title='" + Field.FieldDescription + "'></i><div class='floatRight'><small>USAGE: {Field:" + Field.FieldCode + "} OR {Field:" + Field.ID + "}</small></div><div class='clear'></div>" : "" %>
                <asp:PlaceHolder runat="server" ID="DynamicField" />
            <%= (Field.RenderLabelAfterControl) ? "<label data-fieldcode='" + Field.FieldCode + "' data-fieldid=" + Field.ID + ">" + Field.FieldLabel + "</label> <i class='fa fa-question-circle tooltip' aria-hidden='true' title='" + Field.FieldDescription + "'></i><div class='floatRight'><small>USAGE: {Field:" + Field.FieldCode + "} OR {Field:" + Field.ID + "}<small></div><div class='clear'></div>" : "" %>
        <% } %>
    </div>    
    <div class="buttons floatRight">
        <asp:LinkButton Text="Save" ID="Submit" runat="server" OnClick="Submit_Click"/>        
    </div>
    <div class="clear"></div>

</asp:Content>
