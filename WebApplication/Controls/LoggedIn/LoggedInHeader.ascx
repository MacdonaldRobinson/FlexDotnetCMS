<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedInHeader.ascx.cs" Inherits="WebApplication.Controls.OnLogin.LoggedInHeader" %>

<asp:Panel runat="server" ID="LoggedInHeaderPanel" ClientIDMode="Static" Visible="false">        
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>

    <a id="SlideTab"><i class="fa fa-arrow-down" aria-hidden="true"></i>CMS Shortcuts</a>
    <asp:Panel ID="AccessCMSPermissionsPanel" runat="server" Visible="false" ClientIDMode="Static">
        <div>            
            <a id="ToggleFieldEditor" class="button">Hide Field Editors</a>
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
        #SlideTab {
            background-color: red;
            display:block;
            cursor: pointer;         
            text-align: center;
            padding: 5px;
        }

            #SlideTab .fa {
                margin-right: 5px;
            }

        #LoggedInHeaderPanel {
            background-color: #000;
            color: #fff;
            position: fixed;
            bottom:0;
            z-index:999999;
        }

        #ToggleFieldEditor {
            cursor: pointer;
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
        }

            .field.hide {
                padding:0;
                margin:0;
                border:none !important;
                display:inline;

            }
            .field.hide .edit{
                display:none;
            }

            .field.hover {
                border: 1px dashed rgba(0,0,0,.3);
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

            function UpdateSliderTabIcon()
            {                
                if ($("#AccessCMSPermissionsPanel").is(":visible"))
                {                    
                    $("#SlideTab .fa").removeClass("fa-arrow-up");
                    $("#SlideTab .fa").addClass("fa-arrow-down");
                }
                else
                {                    
                    $("#SlideTab .fa").removeClass("fa-arrow-down");
                    $("#SlideTab .fa").addClass("fa-arrow-up");
                }
            }

            if (GetCMSShortcutsVisibility() == "true")
            {
                $("#AccessCMSPermissionsPanel").show(0, function () {
                    UpdateSliderTabIcon();
                });                
            }
            else if (GetCMSShortcutsVisibility() == "false")
            {                
                $("#AccessCMSPermissionsPanel").hide(0, function () {                    
                    UpdateSliderTabIcon();
                });                
            }
            CreateFieldsEditor();
            //HideFieldsEditor();

            function HideFieldsEditor() {
                $(".field").addClass("hide");
            }

            function ShowFieldsEditor() {
                $(".field").removeClass("hide");
            }

            function CreateFieldsEditor() {
                $("[data-fieldid]").each(function () {
                    var fieldId = $(this).attr("data-fieldid");
                    var fieldcode = $(this).attr("data-fieldcode");

                    $(this).prepend("<a class='edit colorbox iframe' href='" + BaseUrl + "Admin/Views/PageHandlers/FieldEditor/Default.aspx?fieldId=" + fieldId + "' data-OnColorboxClose='window.location.reload()' data-width='60%' data-height='80%'>Edit</a>");
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

            $("#ToggleFieldEditor").on("click", function () {                

                var toggleButton = $(this);

                $(".field").each(function () {

                    var text = toggleButton.text();

                    if ($(this).hasClass("hide")) {
                        $(this).removeClass("hide");
                        toggleButton.text(text.replace("Show", "Hide"));
                    }
                    else {
                        $(this).addClass("hide");
                        toggleButton.text(text.replace("Hide", "Show"));
                    }
                });
            });        

            if (window.top != window) {
                $("#LoggedInHeaderPanel").hide();
                HideFieldsEditor();
            }
            else
            {            
                ShowFieldsEditor();
            }

            $("#SlideTab").on("click", function () {
                $("#AccessCMSPermissionsPanel").slideToggle(function () {                        
                    SetCMSShortcutsVisibility($(this).is(":visible"));               
                });
            });

            function GetCMSShortcutsVisibility()
            {
                var visibility = $.cookie('CMSShortcutsVisibility');

                if (visibility == undefined)
                {
                    visibility = $("#AccessCMSPermissionsPanel").is(":visible");
                    SetCMSShortcutsVisibility(visibility);                    
                }

                return visibility;
            }

            function SetCMSShortcutsVisibility(val) {                
                var visibility = $.cookie('CMSShortcutsVisibility', val);
                UpdateSliderTabIcon();

                return visibility;
            }

        });
    </script>
</asp:Panel>