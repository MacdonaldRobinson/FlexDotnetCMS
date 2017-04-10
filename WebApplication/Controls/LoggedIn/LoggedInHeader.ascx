<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedInHeader.ascx.cs" Inherits="WebApplication.Controls.OnLogin.LoggedInHeader" %>

    <asp:Panel runat="server" ID="LoggedInHeaderPanel" ClientIDMode="Static" Visible="false" CssClass="cms-header">
        <script type="text/javascript">
            $(document).ready(function () {
                if (window.top != window) {
                    $("#LoggedInHeaderPanel").hide();
                }
            });
        </script>
        <asp:Panel ID="AccessCMSPermissionsPanel" runat="server" Visible="false" CssClass="cms-header-controls">
            <asp:HyperLink ID="QuickEditCurrentPage" runat="server" CssClass="colorbox iframe" data-OnColorboxClose="window.location.reload()"><i class="fa fa-pencil"></i>&nbsp;Quick Edit</asp:HyperLink>
            <br />
            <asp:HyperLink ID="EditCurrentPage" runat="server"><i class="fa fa-expand"></i>&nbsp;Edit In CMS</asp:HyperLink>
        </asp:Panel>
        <div class="cms-header-logout">
            <Site:LoginSuccess ID="LoginSuccess" runat="server" />
        </div>

        <style>
            .field {
                border: 1px solid #808080;
                position: relative;
                padding-top: 40px;                
            }
                .field .edit {
                    content: 'Edit';
                    position: absolute;
                    top:0;
                    left: 0;
                    z-index: 99999999;
                    background-color: #000000;
                    color: #fff;
                    cursor:pointer;
                    padding: 2px;
                }
        </style>

        <script type="text/javascript">
            $(document).ready(function () {
                $("[data-fieldid]").each(function () {
                    var fieldId = $(this).attr("data-fieldid");

                    $(this).prepend("<a class='edit colorbox iframe' href='" + BaseUrl + "Admin/Views/PageHandlers/FieldEditor/Default.aspx?fieldId=" + fieldId +"' data-OnColorboxClose='window.location.reload()' data-width='800px' data-height='500px'>Edit</a>");
                });
            });
        </script>
    </asp:Panel>