<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedInHeader.ascx.cs" Inherits="WebApplication.Controls.OnLogin.LoggedInHeader" %>
<%@ Register Src="~/Controls/LoggedIn/VisualLayoutEditor.ascx" TagPrefix="Admin" TagName="VisualLayoutEditor" %>

<asp:Panel runat="server" ID="LoggedInHeaderPanel" ClientIDMode="Static">      

    <Admin:VisualLayoutEditor runat="server" id="VisualLayoutEditor" Visible="false"/>
    
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>

    <a id="SlideTab"><i class="fa fa-arrow-down" aria-hidden="true"></i>CMS Shortcuts</a>
    <asp:Panel ID="AccessCMSPermissionsPanel" runat="server" Visible="true" ClientIDMode="Static">
        <div>                        
            <a ID="QuickEditCurrentPage" class="colorbox iframe button" data-OnColorboxClose="window.location.reload()" href="<%= CurrentMediaDetailAdminUrl %>&masterFilePath=~/Admin/Views/MasterPages/Popup.Master"><i class="fa fa-pencil"></i>&nbsp;Edit Page</a>                
            <div id="AdminPanel" runat="server">
                <a ID="ToggleVisualEditor" class="button" href="javascript:void(0)" onclick="ToggleVisualEditor()"><i class="fa fa-wrench"></i>&nbsp;Toggle Visual Layout Editor</a>
                <a ID="EdiSettings" class="colorbox iframe button" data-OnColorboxClose="window.location.reload()" href="<%= URIHelper.BaseUrl %>Admin/Views/PageHandlers/Settings/Default.aspx?masterFilePath=~/Admin/Views/MasterPages/Popup.Master"><i class="fa fa-wrench"></i>&nbsp;Edit Settings</a>
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
        #SlideTab {
            background-color: red;
            display:block;
            cursor: pointer;         
            text-align: center;
            padding: 5px;
            color: #fff;
        }

            #SlideTab .fa {
                margin-right: 5px;
            }

        #LoggedInHeaderPanel {
            background-color: #000;
            color: #fff;
            position: fixed;
            bottom:0;
            right:0;
            z-index:999999;
            display:none;
        }

        #AccessCMSPermissionsPanel a.button{
            display: block;
            color: #fff;                
            border: 1px solid #fff;
            padding: 5px;              
            margin: 10px;
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
            position: relative;
            padding-top: 25px;
            display: block !important;
        }

            .field.hide {
                padding:0;
                margin:0;
                border:none !important;                

            }
            .field.hide .edit{
                display:none !important;
            }

            .field.hover {
                outline: 2px dashed rgba(0,0,0, 0.5)!important;
            }
            .field .edit:hover {
                background-color: red !important;  
                opacity: 1;
            }
            .field .edit {
                content: 'Edit';
                position: static;
                top: 0px;
                left: 0px;                    
                background-color: #000 !important;
                color: #fff !important;
                cursor: pointer;
                padding: 2px 5px;
                font-size: 12px;
                font-style: normal;   
                opacity: 0.3;
                white-space: nowrap;
                margin-top: -30px;
                z-index: 999999;
                float: left;
                display: none;
            }
            .field .edit.show {
                display: block;
            }

            .field .field .edit {
                margin-top: -5px;
            }

    </style>

    <script type="text/javascript">

        function ToggleVisualEditor()
        {            
            if (window.location.search.toLowerCase().indexOf("visuallayouteditor=true") == -1)
            {
                var url = window.location.href;

                if (url.indexOf("?") == -1)
                {
                    url = url + "?";
                }
                else
                {
                    url = url + "&";
                }

                url = url + "VisualLayoutEditor=true";

                window.location.href = url;
            }
            else
            {
                window.location.href = window.location.href.toLowerCase().replace("&visuallayouteditor=true", "").replace("?visuallayouteditor=true", "");
            }
        }

        function HideFieldsEditor() {
            $(".field").addClass("hide");
            $(".field .edit").removeClass("show");           
        }

        function ShowFieldsEditor() {
            $(".field").removeClass("hide");
            $(".field .edit").addClass("show");
        }

        function CreateFieldsEditor() {
            $("[data-fieldid]").each(function () {
                var fieldId = $(this).attr("data-fieldid");
                var fieldcode = $(this).attr("data-fieldcode");

                $(this).prepend("<a class='edit colorbox iframe' href='" + BaseUrl + "Admin/Views/PageHandlers/FieldEditor/Default.aspx?fieldId=" + fieldId + "' data-OnColorboxClose='window.location.reload()' data-width='60%' data-height='80%'>Edit - {Field:" + fieldcode + "}</a><div class='clear'></div>");
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
        }

        var IsLoggedIn = false;
        $(document).ready(function () {        

            $.get("/WebServices/IMediaDetails.asmx/CanAccessFrontEndEditorForMediaDetail?id=<%= BasePage.CurrentMediaDetail.ID %>", function (data) {
                if (data.IsError)
                {                                        
                    return;
                }
                else
                {
                    IsLoggedIn = true;

                    if (typeof initVisualLayoutEditor == 'function') {
                        initVisualLayoutEditor();
                    }

                    $("#LoggedInHeaderPanel").show();

                    function UpdateSliderTabIcon() {
                        if ($("#AccessCMSPermissionsPanel").is(":visible")) {
                            $("#SlideTab .fa").removeClass("fa-arrow-up");
                            $("#SlideTab .fa").addClass("fa-arrow-down");
                        }
                        else {
                            $("#SlideTab .fa").removeClass("fa-arrow-down");
                            $("#SlideTab .fa").addClass("fa-arrow-up");
                        }
                    }

                    CreateFieldsEditor();

                    if (GetCMSShortcutsVisibility() == "true") {
                        $("#AccessCMSPermissionsPanel").show(0, function () {
                            UpdateSliderTabIcon();
                        });
                    }
                    else if (GetCMSShortcutsVisibility() == "false") {
                        setTimeout(function () {
                            HideFieldsEditor();
                        }, 100);

                        $("#AccessCMSPermissionsPanel").hide(0, function () {
                            UpdateSliderTabIcon();
                        });
                    }

                    if (window.top != window) {
                        $("#LoggedInHeaderPanel").hide();
                        HideFieldsEditor();
                    }
                    else {
                        ShowFieldsEditor();
                    }

                    $("#SlideTab").on("click", function () {
                        $("#AccessCMSPermissionsPanel").slideToggle(function () {
                            SetCMSShortcutsVisibility($(this).is(":visible"));
                        });
                    });

                    function GetCMSShortcutsVisibility() {
                        var visibility = $.cookie('CMSShortcutsVisibility');

                        if (visibility == undefined) {
                            visibility = $("#AccessCMSPermissionsPanel").is(":visible");
                            SetCMSShortcutsVisibility(visibility);
                        }

                        return visibility;
                    }

                    function SetCMSShortcutsVisibility(val) {
                        var visibility = $.cookie('CMSShortcutsVisibility', val);
                        UpdateSliderTabIcon();

                        if (val) {
                            ShowFieldsEditor();
                        }
                        else {
                            HideFieldsEditor();
                        }

                        return visibility;
                    }

                }
            });       
        });
    </script>
</asp:Panel>