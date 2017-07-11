<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.FieldEditor.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    

    <link href="<%= URIHelper.BaseUrl %>Admin/Styles/mediaDetails.css" rel="stylesheet" />
    <script type="text/javascript">  
        MasterPage = "FieldEditor";
        $(document).ready(function () {
            $("#tabs").tabs();                        
        });
    </script>

    <div class="buttons floatRight">
        <asp:LinkButton Text="Save" ID="Submit" runat="server" OnClick="Submit_Click" CssClass="SavePageButton"/>        
    </div>
    <div class="clear"></div>            


    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Main</a></li>
            <li id="LayoutsTab" runat="server"><a href="#tabs-2">Layout</a></li>            
        </ul>
        <div id="tabs-1" class="tab">
            <div>
                <% if (Field != null)
                    { %>
                    <%= (!Field.RenderLabelAfterControl) ? "<label data-fieldcode='" + Field.FieldCode + "' data-fieldid=" + Field.ID + ">" + Field.FieldLabel + "</label> <i class='fa fa-question-circle tooltip' aria-hidden='true' title='" + Field.FieldDescription + "'></i><div class='floatRight'><small>USAGE: {Field:" + Field.FieldCode + "} OR {{Load:" + ((Field.MediaDetail != null)? Field.MediaDetail.MediaID.ToString() : "[MediaID]") +"}.Field:"+Field.FieldCode+"}</small></div><div class='clear'></div>" : "" %>
                        <asp:PlaceHolder runat="server" ID="DynamicField" />
                    <%= (Field.RenderLabelAfterControl) ? "<label data-fieldcode='" + Field.FieldCode + "' data-fieldid=" + Field.ID + ">" + Field.FieldLabel + "</label> <i class='fa fa-question-circle tooltip' aria-hidden='true' title='" + Field.FieldDescription + "'></i><div class='floatRight'><small>USAGE: {Field:" + Field.FieldCode + "} OR {{Load:" + ((Field.MediaDetail != null)? Field.MediaDetail.MediaID.ToString() : "[MediaID]") +"}.Field:"+Field.FieldCode+"}<small></div><div class='clear'></div>" : "" %>
                <% } %>
            </div>    
        </div>
        <div id="tabs-2" class="tab">
            <asp:TextBox ID="FrontEndLayout" runat='server' TextMode='Multiline' CssClass='AceEditor' Height='350px' />
        </div>
    </div>

</asp:Content>
