<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedInHeader.ascx.cs" Inherits="WebApplication.Controls.OnLogin.LoggedInHeader" %>

    <asp:Panel runat="server" ID="LoggedInHeaderPanel" ClientIDMode="Static" Visible="false">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
        <script type="text/javascript">
            $(document).ready(function () {
                if (window.top != window) {
                    $("#LoggedInHeaderPanel").hide();
                }
            });
        </script>
        <asp:Panel ID="AccessCMSPermissionsPanel" runat="server" Visible="false" ClientIDMode="Static">
            <div>            
                <a ID="QuickEditCurrentPage" class="colorbox iframe button" data-OnColorboxClose="window.location.reload()" href="<%= CurrentMediaDetailAdminUrl %>&masterFilePath=~/Admin/Views/MasterPages/Popup.Master"><i class="fa fa-pencil"></i>&nbsp;Edit Page</a>                
                <div id="AdminPanel" runat="server">
                    <a ID="EditSettings" class="colorbox iframe button" data-OnColorboxClose="window.location.reload()" href="<%= URIHelper.BaseUrl %>Admin/Views/PageHandlers/Settings/Default.aspx?masterFilePath=~/Admin/Views/MasterPages/Popup.Master"><i class="fa fa-wrench"></i>&nbsp;Edit Settings</a>
                    <a ID="EditTemplate" class="colorbox iframe button" data-OnColorboxClose="window.location.reload()" href="<%= URIHelper.BaseUrl %>Admin/Views/PageHandlers/MasterPages/Detail.aspx?id=<%= BasePage.CurrentMediaDetail.GetMasterPage().ID %>"><i class="fa fa-file-code-o"></i>&nbsp;Edit Template</a>
                    <a ID="EditMediaType" class="colorbox iframe button" data-OnColorboxClose="window.location.reload()" href="<%= URIHelper.BaseUrl %>Admin/Views/PageHandlers/MediaTypes/Detail.aspx?id=<%= BasePage.CurrentMediaDetail.MediaTypeID %>"><i class="fa fa-file-code-o"></i>&nbsp;Edit Media Type</a>            
                </div>
            </div>
            <div> 
                <a ID="EditCurrentPage" Target="_blank" class="button" data-OnColorboxClose="window.location.reload()" href="<%= CurrentMediaDetailAdminUrl %>"><i class="fa fa-external-link"></i>&nbsp;Edit in CMS</a>
            </div>
            <div class="clear"></div>
        </asp:Panel>

        <style>
            #LoggedInHeaderPanel {
                background-color: #000;
                padding: 5px;
                color: #fff;
                position: fixed;
                bottom:0;
            }

            #AccessCMSPermissionsPanel a.button{
                display: block;
                color: #fff;                
                border: 1px solid #fff;
                padding: 5px;              
                margin-left: 10px;
                text-decoration: none;
            }

            #AccessCMSPermissionsPanel a.button:hover{
                background-color: red;
            }

            #AccessCMSPermissionsPanel:after {
                clear: both;
            }

            div.clear {
                clear: both;
            }

            .floatLeft {
                float: left;
            }

            .floatRight {
                float: right;
            }

            .field {
                border: 1px dashed;
                position: relative;                      
                display:inline-block;
                border-color: rgba(0, 0, 0, .5);
            }

                .field.hover {
                    border: 1px dashed rgba(0,0,0,.3) !important;
                }
                .field .edit:hover {
                    background-color: red;  
                    opacity: 1;
                }
                .field .edit {
                    content: 'Edit';
                    position: absolute;
                    top: 0px;
                    left: 0px;                    
                    background-color: #000;
                    color: #fff;
                    cursor: pointer;
                    padding: 2px 5px;
                    font-size: 12px;
                    font-style: normal;   
                    opacity: 0.2;
                }
        </style>

        <script type="text/javascript">
            $(document).ready(function () {
                $(window).on("load", function () {
                    $("[data-fieldid]").each(function () {
                        var fieldId = $(this).attr("data-fieldid");
                        var fieldcode = $(this).attr("data-fieldcode");

                        $(this).prepend("<a class='edit colorbox iframe' href='" + BaseUrl + "Admin/Views/PageHandlers/FieldEditor/Default.aspx?fieldId=" + fieldId + "' data-OnColorboxClose='window.location.reload()' data-width='60%' data-height='80%'>Edit - {Field:"+fieldcode+"}</a>");
                    });

                    $(document).on("click", ".field .edit", function () {
                        $(".field .edit").hide();
                    });

                    $(document).on("mouseenter", ".field .edit", function () {
                        $(this).parent().addClass("hover");
                    });

                    $(document).on("mouseleave", ".field .edit", function () {
                        $(this).parent().removeClass("hover");
                    });

                });
            });
        </script>
    </asp:Panel>