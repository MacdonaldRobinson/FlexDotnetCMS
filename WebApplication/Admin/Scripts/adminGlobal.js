/// <reference path="../Views/MasterPages/WebService.asmx" />
/// <reference path="../Views/MasterPages/WebService.asmx" />
window.onerror = function (e) {
    if(e.indexOf("UpdatePanel") !=-1)
    {
        window.location.reload();
    }
}
function DisplayJsonException(xhr) {
    try {
        var jsonError = JSON.parse(xhr.responseText);

        jQuery.jGrowl(jsonError.Message, {
            header: "Error",
            life: 10000
        });
    } catch (e) {
    }
}

function executeAction(action, id, updatePanelId) {
    switch (action) {
        case "Refresh":
            break;
        case "Create":
            var createUrl = "Detail.aspx";
            $.colorbox({ href: createUrl, width: colorBoxWidth, height: colorBoxHeight, iframe: true, fixed: true, onClosed: function () { __doPostBack(updatePanelId, ''); } });
            break;
        case "Edit":
            var editUrl = "Detail.aspx?id=" + id;
            $.colorbox({ href: editUrl, width: colorBoxWidth, height: colorBoxHeight, iframe: true, fixed: true, onClosed: function () { __doPostBack(updatePanelId, ''); } });
            break;
        case "Delete":
            if (confirm("Are you sure you want to delete this item?")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/DeleteItem",
                    data: "{'id':'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                function (msg) {
                    eval(msg.d);

                    if (msg.d.indexOf('Error') == -1)
                        __doPostBack(updatePanelId, '');
                },
                    error: function (xhr, status, error) {
                        DisplayErrorMessage("Error Deleting Item", "An unexpected error occured while attempting to delete the item.");
                    }
                });
            }
            break;
        case "UnDelete":
            if (confirm("Are you sure you want to un-delete this item?")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/UnDeleteItem",
                    data: "{'id':'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            __doPostBack(updatePanelId, '');
                    },
                    error: function (xhr, status, error) {
                        DisplayErrorMessage("Error Un-Deleting Item", "An unexpected error occured while attempting to delete the item.");
                    }
                });
            }
            break;
        case "DeletePermanently":
            if (confirm("Are you sure you want to delete this item permanently?")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/DeleteItemPermanently",
                    data: "{'id':'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            __doPostBack(updatePanelId, '');
                    },
                    error: function (xhr, status, error) {
                        DisplayErrorMessage("Error Removing Item", "An unexpected error occured while attempting to delete the item.");
                    }
                });
            }
            break;
        case "Approve":
            if (confirm("Are you sure you want to approve this item?")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/ApproveItem",
                    data: "{'id':'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            __doPostBack(updatePanelId, '');
                    },
                    error: function (xhr, status, error) {
                        DisplayErrorMessage("Error Approving Item", "An unexpected error occured while attempting to delete the item.");
                    }
                });
            }
            break;
        case "TakeOwnership":
            if (confirm("Are you sure you want to take ownership of all items assigned to the selected user with ID (" + id + ") ?")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/TakeOwnership",
                    data: "{'id':'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            __doPostBack(updatePanelId, '');
                    },
                    error: function (xhr, status, error) {
                        DisplayErrorMessage("Error Approving Item", "An unexpected error occured while attempting to delete the item.");
                    }
                });
            }
            break;
        case "Reject":
            if (confirm("Are you sure you want to reject this item? Changes will be deleted permanently.")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/RejectItem",
                    data: "{'id':'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            __doPostBack(updatePanelId, '');
                    },
                    error: function (xhr, status, error) {
                        DisplayErrorMessage("Error Rejecting Item", "An unexpected error occured while attempting to delete the item.");
                    }
                });
            }
            break;
    }
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

var BaseWebserverUrl = BaseUrl + "Admin/Views/MasterPages/Webservice.asmx";

function HandleContextMenuClick(action, target, node) {

    var mediaDetailId = target.parent().attr("mediadetailid");
    var targetText = target.text();    

    switch (action) {
        case "CreateChild":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/CreateChild",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    window.location.href = BaseUrl + "Admin/Views/PageHandlers/Media/Create.aspx";
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);
                }
            });
            break;
        case "Delete":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/Delete",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    RefreshSiteTreeNodeById(node.parent);
                    //window.location.reload();
                    //RefreshSiteTreeViewAjaxPanel();
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);                    
                }
            });
            break;
        case "UnDelete":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/UnDelete",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    RefreshSiteTreeNodeById(node.parent);
                    //RefreshSiteTreeViewAjaxPanel();
                    //window.location.reload();
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);
                }
            });
            break;
        case "Duplicate":
            var newName = prompt("Enter a new name for the page");
            newName = newName.trim();

            if (newName != null) {
                loadingOverlay.show();
                jQuery.ajax({
                    type: "POST",
                    url: BaseWebserverUrl + "/Duplicate",
                    data: "{'id':'" + mediaDetailId + "', 'duplicateChildren':false,'newName':'" + newName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success:
                    function (msg) {
                        //window.location.href = parentNode.get_navigateUrl();
                        //RefreshSiteTreeViewAjaxPanel();
                        RefreshSiteTreeNodeById(node.parent);
                    },
                    error: function (xhr, status, error) {
                        DisplayJsonException(xhr);
                    }
                });
            }
            break;
        case "DuplicateIncludingAllChildren":
            var newName = prompt("Enter a new name for the page");
            newName = newName.trim();

            if (newName != null) {
                loadingOverlay.show();
                jQuery.ajax({
                    type: "POST",
                    url: BaseWebserverUrl + "/Duplicate",
                    data: "{'id':'" + mediaDetailId + "', 'duplicateChildren':true,'newName':'" + newName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success:
                    function (msg) {
                        //RefreshSiteTreeViewAjaxPanel();
                        RefreshSiteTreeNodeById(node.parent);
                        //window.location.href = msg.d.replace("~", "");
                    },
                    error: function (xhr, status, error) {
                        DisplayJsonException(xhr);
                    }
                });
            }
            break;
        case "ShowInMenu":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/ShowInMenu",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    //window.location.reload();
                    //RefreshSiteTreeViewAjaxPanel();
                    RefreshSiteTreeNodeById(node.parent);
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);
                }
            });
            break;
        case "HideFromMenu":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/HideFromMenu",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    //window.location.reload();
                    //RefreshSiteTreeViewAjaxPanel();
                    RefreshSiteTreeNodeById(node.parent);
                    loadingOverlay.hide();
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);
                }
            });
            break;
        case "MoveUp":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/MoveUp",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    //window.location.reload();
                    //RefreshSiteTreeViewAjaxPanel();
                    RefreshSiteTreeNodeById(node.parent);
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);
                }
            });
            break;
        case "MoveDown":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/MoveDown",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    //window.location.reload();
                    //RefreshSiteTreeViewAjaxPanel();
                    RefreshSiteTreeNodeById(node.parent);
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);
                }
            });
            break;
        case "DeletePermanently":
            var areYouSure = confirm('Are you sure you want to permanently delete the item "' + targetText + '" and all its associations, including ALL its child items and history if any? NOTE: This action is irreversible');

            if (areYouSure) {
                loadingOverlay.show();

                jQuery.ajax({
                    type: "POST",
                    url: BaseWebserverUrl + "/DeletePermanently",
                    data: "{'id':'" + mediaDetailId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success:
                    function (msg) {
                        //window.location.reload();
                        //RefreshSiteTreeViewAjaxPanel();
                        RefreshSiteTreeNodeById(node.parent);
                    },
                    error: function (xhr, status, error) {
                        DisplayJsonException(xhr);
                    }
                });
            }
            break;
        case "ClearCache":
            loadingOverlay.show();
            jQuery.ajax({
                type: "POST",
                url: BaseWebserverUrl + "/ClearCache",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    RefreshSiteTreeNodeById(node.parent);
                },
                error: function (xhr, status, error) {
                    DisplayJsonException(xhr);
                }
            });
            break;
        case "ViewFrontEnd":
            //console.log(target.attr("data-frontendurl"));
            window.open(target.attr("frontendurl"));
            break;
    }
}

function createAutoCompleteObject(caption, value, meta)
{
    return { caption: caption, value: value, meta: meta }; 
}

function getFieldsAutoComplete()
{
    var wordsArray = [];

    $("#MainFields label:first-child").each(function () {
        var labelFor = $(this).attr("for");

        if (labelFor != undefined)
        {
            var splitFor = $(this).attr("for").split("_");      
            var text = splitFor[splitFor.length - 1];
            //var text = $(this).text().replace(/\s/g, '').replace(":", "");
            text = "{" + text + "}";                   

            wordsArray.push(createAutoCompleteObject(text, text, "main"));
        }
    });

    $(".field > label").each(function () {
        var text = $(this).attr('data-fieldcode').replace(/\s/g, '');        
        text = "{Field:" + text + "}";
        wordsArray.push(createAutoCompleteObject(text, text, "custom field"));        
    });

    wordsArray.push(createAutoCompleteObject(
        '<Site:GenerateNav', 
        '<Site:GenerateNav runat="server" \
                RenderRootMedia="True" \
                RootMediaID="2"\
                RenderDepth="2"\
                DisplayProtectedSections="false" /',
        'user control'
    ));

    wordsArray.push(createAutoCompleteObject(
        '<Site:RenderChildren',
        '<Site:RenderChildren runat="server" \
                MediaID="0" \
                ShowPager="True" \
                PageSize="10" \
                ChildPropertyName="UseSummaryLayout" \
                Where=\'MediaType.Name=="Page"\' \
                OrderBy="DateCreated DESC" />',
        'user control'
    ));

    wordsArray.push(createAutoCompleteObject(
        '<Site:RenderMedia',
        '<Site:RenderMedia runat="server" \
                MediaID="2" \
                PropertyName="UseSummaryLayout" />',
        'user control'
    ));

    wordsArray.push(createAutoCompleteObject(
        'LayoutsTab:RazorIfField', 
        '<!-- LayoutsTab:RazorIfField: Razor Code Showing how to load a field and check its value --> \
@{ \
    var field = Model.RenderField("test1"); \
        \
    <ul>\
    @if(field == "True")\
    {\
        <li>If condition is true</li>\
    }\
    else\
    {\
        <li>You entered: @Raw(field)</li>\
    }\
    </ul>\
}',
        'razor code'
    ));


    wordsArray.push(createAutoCompleteObject(
        'LayoutsTab:RazorLoopAssociatedItems',
        '<!-- LayoutsTab:RazorLoopAssociatedItems: Razor Code showing how you can load a field and loop through its associated items -->\
@{            \
    var field = Model.LoadField("Dropfield");\
        \
    <ul>\
    @foreach(var item in field.FieldAssociations)\
    {\
        var detail = item.MediaDetail;\
        <li><a href="@detail.AbsoluteUrl">@Raw(detail.RenderField("SectionTitle"))</a></li>\
    }\
    </ul>\
}',
        'razor code'
    ));

    wordsArray.push(createAutoCompleteObject(
        'LayoutsTab:RazorRenderChildren',
        '<!-- LayoutsTab:RazorRenderChildren: Razor Code to loop through and render child items -->\
@{\
    var mediaId = Model.MediaID; // You can change this to any Media ID to load the children of that page\
    var media = MediasMapper.GetByID(mediaId);\
    \
    if(media != null)\
    {\
        var mediaDetail = media.GetLiveMediaDetail();\
        \
        if(mediaDetail != null)\
        {\
            var childItems =  mediaDetail.ChildMediaDetails;\
            <ul>\
            @foreach(var child in childItems)\
            {\
                <li><a href="@child.AbsoluteUrl">@Raw(child.RenderField("SectionTitle"))</a></li>\
            }\
            </ul>\
        }\
    }\
}',
        'razor code'
    ));
    
    return wordsArray;
        
}

function launchIntoFullscreen(element) {
    //element = $(element).parent()[0];
    if (element.requestFullscreen) {
        element.requestFullscreen();
    } else if (element.mozRequestFullScreen) {
        element.mozRequestFullScreen();
    } else if (element.webkitRequestFullscreen) {
        element.webkitRequestFullscreen();
    } else if (element.msRequestFullscreen) {
        element.msRequestFullscreen();
    }
}

function initAceEditors() {    
    var wordList = getFieldsAutoComplete();

    $(document).on("change", "#AttachEditorToBrowserPanel", function () {
        if(!$(this).is(":checked"))
        {
            if ($("#PreviewPanel").length > 0)
                $("#PreviewPanel")[0].src = $("#PreviewPanel")[0].src;
        }
        else
        {
            var textarea = $(this).parent().find("textarea");
            var value = textarea.val();

            if ($("#PreviewPanel").length > 0)
                $("#PreviewPanel")[0].contentWindow.document.body.innerHTML = value;
        }
    });

    $(document).on("click", ".AceEditorFullScreen", function () {
        var element = $(this).parent().find(".ace_editor")[0];        
        /*var mainArea = $("#mainArea")[0];        

        if (mainArea != undefined || mainArea != null)
        {
            element = mainArea;
        }*/

        launchIntoFullscreen(element);                
    });


    $(".AceEditor").each(function () {        

        var textarea = $(this);
        
        var id = $(this).attr("id");
        var editorId = $(this).attr("name") + "-editor";

        if (document.getElementById(editorId) != null)
            return;

        if ($("#PreviewPanel").length > 0) {
            if ($(this).hasClass("CanAttachToBrowserPanel"))
            {
                if ($("#" + id).parent().find("#AttachEditorToBrowserPanel").length == 0)
                {
                    $("#" + id).parent().prepend("<input type='checkbox' id='AttachEditorToBrowserPanel' /> Attach editor to browser panel");
                }
            }
        }

        if ($("#" + id).parent().find(".AceEditorFullScreen").length == 0)
        {
            $("#" + id).parent().prepend("<a class='AceEditorFullScreen' href='#' data-editorid='" + editorId +"'>View Full Screen</a><br />");
        }        

        var style = $(this).attr("style");

        $(this).parent().append("<div id='" + editorId + "' class='ace-editor' style='" + style + "'></div>");

        var editor = ace.edit(editorId);       
        textarea.hide();

        editor.on('focus', function () {
            editor.getSession().setUseWorker(true);
        });

        editor.on('blur', function () {
            editor.getSession().setUseWorker(false);
        });

        var session = editor.getSession();
        session.setUseWorker(false);

        editor.setTheme("ace/theme/iplastic");
        editor.setValue(textarea.val(), 1);
        session.setMode("ace/mode/html");        
        editor.$blockScrolling = Infinity;
        editor.$useWorker = false;

        //editor.clearSelection();

        var langTools = ace.require('ace/ext/language_tools');

        // enable autocompletion and snippets
        editor.setOptions({
            enableBasicAutocompletion: true,
            enableSnippets: true,
            enableLiveAutocompletion: false,
            showPrintMargin: false,
        });

        var customCompleter = {
            getCompletions: function (editor, session, pos, prefix, callback) {
                callback(null, wordList.map(function (autoCompleteObject) {
                    return {
                        caption: autoCompleteObject.caption,
                        value: autoCompleteObject.value,
                        meta: autoCompleteObject.meta
                    };
                }));
            }
        }

        editor.completers = [langTools.snippetCompleter, langTools.textCompleter, customCompleter]

        var htmlBeautifyOptions = {
        };

        editor.commands.addCommand({
            name: 'Beautify',
            bindKey: { win: 'Ctrl-S', mac: 'Command-S' },
            exec: function (editor) {
                var value = editor.getSession().getValue();

                // TODO: Format HTML
                value = value.replace(/<[^]+/, function (match) {

                    if (/@for|@if|@[\s]?{|Helper.|!=|List</.test(match)) {
                        return match;
                    }
                    match = html_beautify(match);
                    return match;
                });

                editor.setValue(value, 1);

                $(".SavePageButton")[0].click();

            },
            readOnly: true // false if this command should not apply in readOnly mode
        });

        editor.getSession().on('change', function () {
            var value = editor.getSession().getValue();
            textarea.val(value);

            if ($("#PreviewPanel").length > 0) {
                if (textarea.parent().find("#AttachEditorToBrowserPanel").is(":checked")) {
                    $("#PreviewPanel")[0].contentWindow.document.body.innerHTML = value;
                }
            }

        });
    });
}

function destroyAceEditors() {
    $(".AceEditor").each(function () {
        var editorId = $(this).attr("name") + "-editor";
        var editor = ace.edit(editorId);

        editor.destroy();
    });
}

function destroyTinyMCE() {
    if (typeof (tinyMCE) !== 'undefined') {
        var length = tinymce.editors.length;
        for (var i = length; i > 0; i--) {
            var editor = tinymce.editors[i - 1];
            editor.remove();
        };
    }
}


$(window).load(function () {
    //console.log("window load");
    //setTimeout(function () {
        initAceEditors();
        initTinyMCE();
    //}, 1000);        

    $(document).ajaxComplete(function (event, xhr, settings) {
        //console.log("ajaxComplete");
        if (settings.url.indexOf("Chat.asmx") == -1)
        {
            //setTimeout(function () {
                initAceEditors();
                initTinyMCE();
                BindJQueryUIControls();
            //}, 1000); 
        }
    });
});

function BindMultiFileUploaderImageLoadError()
{
    $(".MultiFileUploader img").error(function () {
        $(this).attr("src", BaseUrl + "media/images/icons/File.jpg");
    });
}

function BindTabs()
{
    $('.tabs').tabs();
}

function BindJQueryUIControls()
{
    try {
        $("select").selectmenu({
            change: function (event, ui) {
                $(event.target).trigger("change");
            }
        });

        //$("[class='RadioButtonList']").each(function () {
        //    var inputItems = $(this).find("input");
        //    var index = $("[class='RadioButtonList']").index(this);

        //    var name = "RadioButtonListItem_" + index;

        //    inputItems.attr("type", "radio")
        //    inputItems.attr("name", name);
        //});

        //$("fieldset input[type=checkbox], fieldset input[type=radio]").checkboxradio();

    }
    catch (ex) {

    }
}

$(document).ready(function () {
    BindJQueryUIControls();
    //BindScrollMagic();    

    $('.tooltip').each(function () {
        var title = $(this).attr("title");
        if(title == undefined || title == "")
        {
            $(this).hide();
        }
    });

    $('.tooltip').tooltipster({
        contentAsHTML: true,
        interactive: true,
        maxWidth: 500,
        trigger: 'click'
    });

    $('ul.sf-menu').superfish();
    BindTabs();

    $('input.datetimepicker').datetimepicker({
        controlType: 'select',
        oneLine: true,
        dateFormat: 'yy-mm-dd',
        timeFormat: 'hh:mm:ss TT'
    });

    $(document).on("keydown", function (event) {
        if (event.ctrlKey || event.metaKey) {
            switch (String.fromCharCode(event.which).toLowerCase()) {
                case 's':
                    $('.SavePageButton')[0].click();
                    event.preventDefault();

                    break;
            }
        }
    });

    $(document).on('click', '.SavePageButton', function (event) {

        $(".save-template").click();

        var text = $(".SaveFieldButton").text();

        if (text.indexOf("Save") != -1) {
            var autoClickedSaveFieldButton = false;

            OnUpdatePanelRefreshComplete(function (event) {
                if (!autoClickedSaveFieldButton) {
                    $(".SaveFieldButton")[0].click();
                    autoClickedSaveFieldButton = true;     

                    //ReloadPreviewPanel();

                }
            });
        }

        return true;
    });
});

function initTinyMCE()
{
    tinymce.editors = [];
    tfm_path = BaseUrl + "Scripts/tinyfilemanager.net";
    tinymce.init({
        selector: ".editor",
        content_css: BaseUrl + "FrontEnd/styles/css/main.css, " + BaseUrl + "Admin/Styles/editor.css",
        menubar: false,
        plugins: [
          'advlist autolink lists link image charmap print preview hr anchor pagebreak',
          'searchreplace wordcount visualblocks visualchars fullscreen',
          'insertdatetime media youtube nonbreaking save table contextmenu directionality',
          'emoticons template paste textcolor colorpicker textpattern imagetools ace_beautify imgmap table map'
        ],
        toolbar1: 'undo redo | paste pastetext | bold italic underline strikethrough superscript subscript charmap emoticons | formatselect blockquote | alignleft aligncenter alignright alignjustify outdent indent | bullist numlist | insert table | anchor link image imgmap media youtube map | visualblocks ace_beautify',
        templates: [
        ],
        image_advtab: true,
        relative_urls: false,
        convert_urls: false,
        remove_script_host: false,
        verify_html: false,
        valid_children: '+a[div|p|ul|ol|li|h1|span|h2|h3|h4|h5|h5|h6]',
        extended_valid_elements: 'span[*],a[*],+iframe[src|width|height|name|align|class]',
        custom_shortcuts: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });

            editor.on('keydown', function (event) {
                if (event.ctrlKey || event.metaKey) {
                    switch (String.fromCharCode(event.which).toLowerCase()) {
                        case 's':
                            $('.SavePageButton')[0].click();
                            event.preventDefault();

                            break;
                    }
                }
            });

        }
    });
}

function BindGridViewSortable(CssSelector, WebserviceUrl, UpdatePanelClientId, OnAfterRefreshFunction) {
    var DragDropGridSortable = $(CssSelector).sortable({
        items: 'tr:not(tr:first-child)',
        cursor: 'crosshair',
        connectWith: '.DragDropGrid',
        axis: 'y',
        dropOnEmpty: true,
        receive: function (e, ui) {
            $(this).find("tbody").append(ui.item);
        },
        update: function (event, ui) {
            var ths = $(this).find("th");
            var trs = $(this).find("tr:not(tr:first-child)");

            var cols = [];
            var entries = {};
            ths.each(function () {
                var text = $.trim($(this).text());

                if (text != "")
                    cols.push(text);
            });

            var entries = "";
            var trsIndex = 0;
            trs.each(function () {
                var tds = $(this).find("td");
                var propIndex = 0;
                var properties = "";
                tds.each(function () {
                    var col = cols[propIndex];
                    if (col != undefined) {
                        properties += "\"" + col + "\":\"" + $.trim($(this).text()) + "\"";
                        propIndex++;

                        if (cols[propIndex] != undefined)
                            properties += ",";
                    }
                });

                trsIndex++;

                var entry = "{" + properties + "}";

                if (trs[trsIndex] != undefined)
                    entry += ",";

                entries += entry;
            });

            var jsonString = "{\"items\":[" + entries + "]}";

            if (WebserviceUrl != "") {
                jQuery.ajax({
                    type: "POST",
                    url: WebserviceUrl,
                    data: jsonString,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var isFunc = jQuery.isFunction(OnAfterRefreshFunction);

                        RefreshUpdatePanel(UpdatePanelClientId, function () { BindGridViewSortable(CssSelector, WebserviceUrl, UpdatePanelClientId); if (isFunc) { OnAfterRefreshFunction(); } });
                    },
                    error: function (xhr, status, error) {
                    }
                });
            }
            else
            {
                OnAfterRefreshFunction();
            }
        }
    });
}

$(function () {

    $('div.split-pane').splitPane();
    //$("#SiteTree ul").sortable({ connectWith: "ul" });
    //BindTree();

    $(document).on('click', '#SiteTree a', function () {
        window.location.href = $(this).attr("href");
    });

});

function RefreshSiteTreeParentNode()
{
    var selected = $("#SiteTree").jstree("get_selected");

    if (selected.length > 0)
    {
        var selectedId = selected[0];
        var parentId = $("#SiteTree").jstree(true).get_parent(selectedId);

        RefreshSiteTreeNodeById(parentId);
    }
}

function RefreshSiteTreeSelectedNode()
{
    var selected = $("#SiteTree").jstree("get_selected");

    if (selected.length > 0) {
        var selectedId = selected[0];

        RefreshSiteTreeNodeById(selectedId);
    }
}

function RefreshSiteTreeNodeById(nodeId)
{
    var jsTree = $('#SiteTree').jstree(true);

    if (jsTree != false)
        jsTree.refresh_node(nodeId);
}

function ConvertDivToSiteTree(divSelector, targetSelector, mediaId)
{
    var jsTree = $(divSelector).jstree(true);
    var filterText = "";

    if (jsTree != false) {
        jsTree.destroy();
    }

    $(divSelector).jstree({
        "plugins": ["checkbox"],
        "checkbox": {
            "keep_selected_style": false
        },
        'types': {
            'default': {
                'icon': 'jstree-icon jstree-file'
            }
        },
        "core": {
            // so that create works
            "multiple": true,
            "cascade":"",
            'data': {
                'url': function (node) {
                    if (filterText == "" || filterText == undefined || filterText == null)
                        return node.id === '#' ? BaseUrl + 'Admin/Views/MasterPages/WebService.asmx/GetRootNodes' : BaseUrl + 'Admin/Views/MasterPages/WebService.asmx/GetChildNodes';
                    else
                        return BaseUrl + 'Admin/Views/MasterPages/WebService.asmx/SearchForNodes?filterText=' + filterText;
                },
                'data': function (node) {
                    return { 'id': node.id };
                }
            }
        }
    }).on('ready.jstree', function (e, data) {
        data.instance.uncheck_all();  
        }).on("changed.jstree", function (node, action, selected, event) {
            var checkedItems = action.instance.get_checked();
            $(targetSelector).val(JSON.stringify(checkedItems));
    })
    
}

function BindTree(filterText) {

    var jsTree = $('#SiteTree').jstree(true);

    if (jsTree != false)
    {
        jsTree.destroy();
    }

    $('#SiteTree').jstree({
        'types': {
            'default': {
                'icon': 'jstree-icon jstree-file'
            }
        },
        "core": {
            // so that create works
            "check_callback": true,
            "multiple": false,
            'data': {
                'url': function (node) {
                    if (filterText == "" || filterText == undefined || filterText == null)
                        return node.id === '#' ? BaseUrl + 'Admin/Views/MasterPages/WebService.asmx/GetRootNodes' : BaseUrl + 'Admin/Views/MasterPages/WebService.asmx/GetChildNodes';
                    else
                        return BaseUrl + 'Admin/Views/MasterPages/WebService.asmx/SearchForNodes?filterText=' + filterText;

                },
                'data': function (node) {
                    return { 'id': node.id };
                }
            }
        },
        "plugins": ["contextmenu", "dnd", "types"],
        "contextmenu": {
            "items": function (node) {
                return {
                    "Create": {
                        "label": "Create Child",
                        "action": function (obj) {
                            //this.create(obj);
                            HandleContextMenuClick("CreateChild", obj.reference, node);
                        }
                    },
                    "Delete": {
                        "label": "Mark As Deleted",
                        "action": function (obj) {
                            HandleContextMenuClick("Delete", obj.reference, node);
                        }
                    },
                    "UnDelete": {
                        "label": "Restore",
                        "action": function (obj) {
                            HandleContextMenuClick("UnDelete", obj.reference, node);
                        }
                    },
                    "ShowInMenu": {
                        "label": "Show In Menu",
                        "action": function (obj) {
                            HandleContextMenuClick("ShowInMenu", obj.reference, node);
                        }
                    },
                    "HideFromMenu": {
                        "label": "Hide From Menu",
                        "action": function (obj) {
                            HandleContextMenuClick("HideFromMenu", obj.reference, node);
                        }
                    },
                    "MoveUp": {
                        "label": "Move Up",
                        "action": function (obj) {
                            HandleContextMenuClick("MoveUp", obj.reference, node);
                        }
                    },
                    "MoveDown": {
                        "label": "Move Down",
                        "action": function (obj) {
                            HandleContextMenuClick("MoveDown", obj.reference, node);
                        }
                    },
                    "ClearCache": {
                        "label": "Clear Cache",
                        "action": function (obj) {
                            HandleContextMenuClick("ClearCache", obj.reference, node);
                        }
                    },
                    "ViewFrontEnd": {
                        "label": "View Front End",
                        "action": function (obj) {
                            HandleContextMenuClick("ViewFrontEnd", obj.reference, node);
                        }
                    },
                    "Duplicate": {
                        "label": "Duplicate",
                        "action": function (obj) {
                            HandleContextMenuClick("Duplicate", obj.reference, node);
                        }
                    },
                    "DuplicateIncludingAllChildren": {
                        "label": "Duplicate + Children",
                        "action": function (obj) {
                            HandleContextMenuClick("DuplicateIncludingAllChildren", obj.reference, node);
                        }
                    },
                    "DeletePermanently": {
                        "label": "Delete Permanently",
                        "action": function (obj) {
                            HandleContextMenuClick("DeletePermanently", obj.reference, node);
                        }
                    }
                };
            }
        }
    }).on('move_node.jstree', function (e, data) {
        var sourceMediaId = data.node.id;
        var parentNode = $("#" + data.parent);
        var parentMediaId = parentNode.attr("id");
        var newPosition = data.position;

        jQuery.ajax({
            type: "POST",
            url: BaseWebserverUrl + "/HandleNodeDragDrop",
            data: "{'sourceMediaId':'" + sourceMediaId + "', 'parentMediaId':'" + parentMediaId + "', 'newPosition':'" + newPosition + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success:
            function (msg) {
                //RefreshSiteTreeViewAjaxPanel();
                RefreshSiteTreeNodeById(parentMediaId);
            },
            error: function (xhr, status, error) {
                DisplayJsonException(xhr);
                RefreshSiteTreeNodeById(parentMediaId);
            }
        });

    });

}
$(document)
    .on('dnd_move.vakata', function (e, data) {
        var target = $(data.event.target);

        var dropZone = null;
        var parentDropZone = target.parents("ul.dropZone");

        if (target.hasClass("ul.dropZone"))
        {
            dropZone = target;
        }
        else if (parentDropZone.length > 0)
        {
            dropZone = parentDropZone;
        }

        return true;
    })
    .on('dnd_stop.vakata', function (e, data) {
        var elem = $(data.element);
        var target = $(data.event.target);

        var tagName = target.prop("tagName").toLowerCase();

        if (tagName != "ul") {
            target = target.parents("ul");
        }

        var isDropZone = target.hasClass("dropZone")

        if (isDropZone) {
            var mediaDetailId = elem.parent().attr("mediadetailid");
            var text = elem.text();

            var href = elem.attr("href") + "&masterFilePath=~/Admin/Views/MasterPages/Popup.Master";
            var li = "<li mediadetailid='" + mediaDetailId + "'><a class='delete'>x</a><span class='text'>" + text + "</span><a class='edit colorbox iframe' href='" + href + "'>Edit</a></li>";
            
            if (target.find("li[mediadetailid='" + mediaDetailId + "']").length == 0) {               
                target.append(li);
                UpdateValuesFromUL(target);
            }
        }

    });

function pageLoad(sender, args) {    
    BindJQueryUIControls();
    /*RefreshSiteTreeNodeById($("#SiteTree").jstree("get_selected")[0]);
    BindScrollMagic();
    BindDataTable();
    BindSortable();
    BindTabs();
    BindMultiFileUploaderImageLoadError();    

    if (MasterPage != undefined && MasterPage.indexOf("FieldEditor") == -1)
    {
        initTinyMCE();
        setTimeout(function () {
            initAceEditors();
        }, 1000)
    }    

    if (typeof (BindActiveTabs) == 'function')
        BindActiveTabs();*/
}

function BindDataTable() {
    //$('.DataTable').DataTable({
    //    dom: 'Bfrtip',
    //    buttons: [
    //        {
    //            extend: 'csvHtml5',
    //            title: 'Data export'
    //        }
    //    ]
    //});
}

function RefreshAdminUpdatePanel(elem) {

    RefreshUpdatePanel(elem, function () {
        BindSortable();
    });
}

$(document).ready(function () {
    init();
    BindMultiFileUploaderImageLoadError();
});

function BindSortable() {
    $(".dropZone.sortable").sortable({
        tolerance: "pointer",
        connectWith: '.dropZone.sortable',
        update: function (event, ui) {
        }
    });

    $(".MultiFileUploader .sortable").sortable({
        tolerance: "pointer",
        update: function (event, ui) {
            var arr = new Array();
            $(this).children("li").each(function () {
                var id = $(this).attr("data-id");
                arr.push(id);
            });

            var root = $(this).parents(".MultiFileUploader");

            root.find(".ReorderItems").val(JSON.stringify(arr));
        }
    });
}

function BindScrollMagic()
{
    if(window.navigator.appVersion.indexOf("Trident") == -1)
        ScrollMagicSetup(".SavePanel");

    ScrollMagicSetup("#SaveFields");
}

var controllerScenesArray = [];
function ScrollMagicSetup(selector)
{
    var controller = null;
    var scene = null;
    var myObject = {};
    var newEntry = true;

    $(controllerScenesArray).each(function () {
        if (this.selector == selector)
        {
            myObject = this;

            myObject.controller.destroy();
            myObject.scene.destroy();

            newEntry = false;

            return;
        }
    });

    myObject.selector = selector;
    myObject.controller = controller = new ScrollMagic.Controller();
    myObject.scene = new ScrollMagic.Scene({ offset: -50, triggerElement: selector, triggerHook: 0 })
                        .setPin(selector)
                        .addTo(controller);


    if (newEntry)
    {
        controllerScenesArray.push(myObject);
    }
}

function ReloadPreviewPanel() {
    if ($("#PreviewPanel").length > 0)
        $("#PreviewPanel")[0].src = $("#PreviewPanel")[0].src;
}

$(function () {
       $(':text').bind('keydown', function (e) {
    //on keydown for all textboxes
       if(e.target.className != "searchtextbox") {
           if (e.keyCode == 13) { //if this is enter key
               e.preventDefault();
               return false;
               }
               else
                  return true;
               }
           else
               return true;
      });
});

function UpdateULFromValues(elem) {

    var values = $(elem).find("input[type='hidden']");

    //console.log(values);
    var json = eval(values.val());
    /*var valsArray = vals.split(",");

    for (item of valsArray) {
        console.log(item);
    }*/

    $(json).each(function () {
        //console.log(this);            
        $(elem).append("<li mediadetailid='" + this.id + "'><a class='delete'>x</a><span class='text'>" + this.name + "</span><a class='edit colorbox iframe' href='" + this.adminUrl + "'>Edit</a></li>");

    });
}

function UpdateValuesFromUL(elem) {
    var values = $(elem).find("input[type='hidden']");

    var arr = new Array();

    $(elem).children("li:not(.hidden)").each(function () {
        var mediadetailid = $(this).attr("mediadetailid");
        var adminUrl = $(this).find("a.edit").attr("href").replace(window.location.origin, "");

        if (adminUrl.indexOf("masterFilePath") == -1)
        {
            adminUrl = adminUrl + "&masterFilePath=~/Admin/Views/MasterPages/Popup.Master";
        }

        var name = $(this).children("span.text").text();

        if (name != "") {
            var obj = new Object();
            obj.name = name;
            obj.id = mediadetailid;
            obj.adminUrl = adminUrl;

            arr.push(obj);
        }

    });

    var jsonString = JSON.stringify(arr);

    values.val(jsonString);
}

function init() {
    BindTree();
    BindSortable();
    BindScrollMagic();

    $("#Filter").on("keyup", function (e) {
        var code = e.keyCode || e.which;

        if (code == 13)
        {
            var text = $(this).val();
            BindTree(text);
        }
    });

    $("ul.dropZone").each(function () {
        UpdateULFromValues(this);
    });

    $(document).on("click", ".dropZone li a.delete", function () {

        var elem = $(this).closest(".dropZone")[0];
        $(this).closest("li").remove();

        UpdateValuesFromUL(elem);
    });

    $("ul.dropZone").sortable({
        update: function (event, ui) {            
            UpdateValuesFromUL(this);
        }
    });

    //TODO
    /*

    $(document).on("click", ".dropZone li a.delete", function () {

        var elem = $(this).closest(".dropZone")[0];
        $(this).closest("li").remove();

        UpdateValuesFromUL(elem);
    });

    $("ul.dropZone").bind("DOMSubtreeModified", function () {
        UpdateValuesFromUL(this);
    });

    $("ul.dropZone").sortable({
        update: function (event, ui) {
            UpdateValuesFromUL(this);
        }
    });*/

    $(document).on("click", ".MultiFileUploader .DeleteItem", function () {

        var root = $(this).closest(".MultiFileUploader");

        var parentItem = $(this).closest(".item");
        var itemsToDelete = root.find(".ItemsToDelete");
        var image = parentItem.find("img");

        if (image.length == 0)
        {
            image = $(this);
        }

        var itemId = $(this).attr('data-id');

        var itemsToDeleteJson = JSON.parse(itemsToDelete.val());
        var src = image.attr("src");

        //console.log(itemsToDeleteJson);

        if (!image.hasClass("MarkedAsDeleted")) {
            image.addClass("MarkedAsDeleted");

            if (itemsToDeleteJson.indexOf(itemId) == -1) {
                itemsToDeleteJson.push(itemId);
            }

            itemsToDelete.val(JSON.stringify(itemsToDeleteJson));

            $(this).text("UnDelete");
        }
        else {
            image.removeClass("MarkedAsDeleted");

            var index = itemsToDeleteJson.indexOf(itemId)

            if (index != -1) {
                itemsToDeleteJson.splice(index, 1);
                //itemsToDeleteJson.push(itemId);
            }

            itemsToDelete.val(JSON.stringify(itemsToDeleteJson));

            $(this).text("Delete");
        }
    });

    $(document).on("change", ".MultiFileUpload", function () {
        if (typeof (FileReader) != "undefined") {
            var root = $(this).parents(".MultiFileUploader");
            var dvPreview = root.find(".dvPreview");
            var uploadFilesNowButtons = root.find(".UploadFilesNowButtons");

            dvPreview.html("");
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp|.pdf|.csv|.docx|.doc)$/;
            $($(this)[0].files).each(function () {
                var file = $(this);
                if (regex.test(file[0].name.toLowerCase())) {
                    var reader = new FileReader();
                    reader.onload = function (e) {

                        if (file[0].type.indexOf("image") != -1) {
                            var img = $("<img />");
                            img.attr("style", "width: 100px; height: 100px;");
                            img.attr("src", e.target.result);
                            dvPreview.append(img);
                        }
                        else {
                            var link = $("<a>" + file[0].name + "</a>");
                            link.attr("href", e.target.result);
                            dvPreview.append(link);
                        }
                    }
                    reader.readAsDataURL(file[0]);
                } else {
                    alert(file[0].name + " is not a valid image file.");
                    dvPreview.html("");
                    return false;
                }

                uploadFilesNowButtons.show();
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    });
}