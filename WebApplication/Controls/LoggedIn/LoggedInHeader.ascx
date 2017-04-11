<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedInHeader.ascx.cs" Inherits="WebApplication.Controls.OnLogin.LoggedInHeader" %>

    <asp:Panel runat="server" ID="LoggedInHeaderPanel" ClientIDMode="Static" Visible="false" CssClass="cms-header">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
        <script type="text/javascript">
            $(document).ready(function () {
                if (window.top != window) {
                    $("#LoggedInHeaderPanel").hide();
                }
            });
        </script>
        <asp:Panel ID="AccessCMSPermissionsPanel" runat="server" Visible="false" CssClass="cms-header-controls">
            <asp:HyperLink ID="QuickEditCurrentPage" runat="server" CssClass="colorbox iframe" data-OnColorboxClose="window.location.reload()"><i class="fa fa-pencil"></i>&nbsp;Quick Edit Page</asp:HyperLink>
            <br />
            <asp:HyperLink ID="EditCurrentPage" runat="server" Target="_blank" data-OnColorboxClose="window.location.reload()"><i class="fa fa-expand"></i>&nbsp;Edit in CMS</asp:HyperLink>
        </asp:Panel>
        <div class="cms-header-logout">
            <Site:LoginSuccess ID="LoginSuccess" runat="server" />
        </div>

        <style>
            .field {
                border: 1px dotted #000;
                position: relative;                      
                display:inline-block;
            }
            .field:hover{
                border-color:red;
            }
                .field:hover .edit {
                    background-color: red;
                    opacity: 1;
                }
                .field .edit {
                    content: 'Edit';
                    position: absolute;
                    top: -22px;
                    left: -1px;                    
                    background-color: #000;
                    color: #fff;
                    cursor: pointer;
                    padding: 2px 5px;
                    font-size: 12px;
                    font-style: normal;
                    opacity: 0.1;
                }
        </style>

        <script type="text/javascript">
            $(document).ready(function () {
                $("[data-fieldid]").each(function () {
                    var fieldId = $(this).attr("data-fieldid");

                    $(this).prepend("<a class='edit colorbox iframe' href='" + BaseUrl + "Admin/Views/PageHandlers/FieldEditor/Default.aspx?fieldId=" + fieldId + "' data-OnColorboxClose='window.location.reload()' data-width='60%' data-height='60%'>Edit</a>");
                });
            });
        </script>
    </asp:Panel>