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

function HandleContextMenuClick(action, target, node) {
    var mediaDetailId = target.parent().attr("mediadetailid");
    var targetText = target.text();

    switch (action) {
        case "CreateChild":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/CreateChild",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    window.location.href = "/Admin/Views/PageHandlers/Media/Create.aspx";
                },
                error: function (xhr, status, error) {
                    //console.log(xhr);
                }
            });
            break;
        case "Delete":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/Delete",
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
                    //console.log(xhr);
                }
            });
            break;
        case "UnDelete":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/UnDelete",
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
                    //console.log(xhr);
                }
            });
            break;
        case "Duplicate":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/Duplicate",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                success:
                function (msg) {
                    //window.location.href = parentNode.get_navigateUrl();
                    //RefreshSiteTreeViewAjaxPanel();
                    RefreshSiteTreeNodeById(node.parent);
                },
                error: function (xhr, status, error) {
                    //console.log(xhr);
                }
            });
            break;
        case "DuplicateAndEdit":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/Duplicate",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                success:
                function (msg) {
                    //RefreshSiteTreeViewAjaxPanel();
                    RefreshSiteTreeNodeById(node.parent);
                    window.location.href = msg.d.replace("~", "");
                },
                error: function (xhr, status, error) {
                    //console.log(xhr);
                }
            });
            break;
        case "ShowInMenu":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/ShowInMenu",
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
                    //console.log(xhr);
                }
            });
            break;
        case "HideFromMenu":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/HideFromMenu",
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
                    //console.log(xhr);
                }
            });
            break;
        case "MoveUp":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/MoveUp",
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
                    //console.log(xhr);
                }
            });
            break;
        case "MoveDown":
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/MoveDown",
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
                    //console.log(xhr);
                }
            });
            break;
        case "DeletePermanently":
            var areYouSure = confirm('Are you sure you want to permanently delete the item "' + targetText + '" and all its associations, including ALL its child items and history if any? NOTE: This action is irreversible');

            if (areYouSure) {
                jQuery.ajax({
                    type: "POST",
                    url: "/Admin/Views/MasterPages/Webservice.asmx/DeletePermanently",
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
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/MasterPages/Webservice.asmx/ClearCache",
                data: "{'id':'" + mediaDetailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    RefreshSiteTreeNodeById(node.parent);
                },
                error: function (xhr, status, error) {
                    //console.log(xhr);
                }
            });
            break;
        case "ViewFrontEnd":
            //console.log(target.attr("data-frontendurl"));
            window.open(target.attr("frontendurl"));
            break;
    }
}

function getFieldsAutoComplete()
{
    var wordsArray = [];

    $("#Main label:first-child").each(function () {
        var text = $(this).text().replace(/\s/g, '').replace(":","");
        wordsArray.push("{" + text + "}");
    });

    $(".field > label").each(function () {
        var text = $(this).attr('data-fieldcode').replace(/\s/g, '');
        wordsArray.push("{Field:" + text + "}");
    });

    return wordsArray;
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
                $("#PreviewPanel")[0].contentWindow.document.getElementById("MainContentPlaceHolder").innerHTML = value;
        }
    });


    $(".AceEditor").each(function () {
        var id = $(this).attr("id");
        var editorId = $(this).attr("name") + "-editor";

        if ($("#PreviewPanel").length > 0) {
            if ($(this).hasClass("CanAttachToBrowserPanel"))
            {
                if ($("#" + id).parent().find("#AttachEditorToBrowserPanel").length == 0)
                {
                    $("#" + id).parent().prepend("<input type='checkbox' id='AttachEditorToBrowserPanel' /> Attach editor to browser panel");
                }
            }
        }

        var style = $(this).attr("style");

        $(this).parent().append("<div id='" + editorId + "' class='ace-editor' style='" + style + "'></div>");

        var editor = ace.edit(editorId);
        var textarea = $(this);

        textarea.hide();

        editor.setTheme("ace/theme/iplastic");
        editor.setValue(textarea.val());
        editor.getSession().setMode("ace/mode/html");
        var langTools = ace.require('ace/ext/language_tools');

        // enable autocompletion and snippets
        editor.setOptions({
            enableBasicAutocompletion: true,
            enableSnippets: true,
            enableLiveAutocompletion: true,
            showPrintMargin: false
        });

        var customCompleter = {
            getCompletions: function (editor, session, pos, prefix, callback) {
                callback(null, wordList.map(function (word) {
                    return {
                        caption: word,
                        value: word,
                        meta: "static"
                    };
                }));

            }
        }
        editor.completers = [langTools.snippetCompleter, langTools.textCompleter, customCompleter]

        editor.getSession().on('change', function () {
            var value = editor.getSession().getValue();

            textarea.val(value);

            if ($("#PreviewPanel").length > 0) {
                if (textarea.parent().find("#AttachEditorToBrowserPanel").is(":checked")) {
                    $("#PreviewPanel")[0].contentWindow.document.getElementById("MainContentPlaceHolder").innerHTML = value;
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

$(window).load(function () {
    initAceEditors();

    $(document).ajaxComplete(function () {
        initAceEditors();
    });
});

function BindMultiFileUploaderImageLoadError()
{
    $(".MultiFileUploader img").error(function () {
        $(this).attr("src", "/media/images/icons/File.jpg");
    });
}

function BindTabs()
{
    $('.tabs').tabs();
}

$(document).ready(function () {

    //BindScrollMagic();

    $('ul.sf-menu').superfish();
    BindTabs();

    $('input.datetimepicker').datetimepicker({
        controlType: 'select',
        oneLine: true,
        dateFormat: 'yy-mm-dd',
        timeFormat: 'hh:mm:ss TT'
    });

    tfm_path = "/Scripts/tinyfilemanager.net";
    tinymce.init({
        selector: ".editor",
        content_css: "/Views/MasterPages/SiteTemplates/css/compiled/style.css, /Admin/Styles/editor.css",
        plugins: [
          'advlist autolink lists link image charmap print preview hr anchor pagebreak',
          'searchreplace wordcount visualblocks visualchars fullscreen',
          'insertdatetime media youtube nonbreaking save table contextmenu directionality',
          'emoticons template paste textcolor colorpicker textpattern imagetools ace'
        ],
        toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media youtube ace',
        templates: [
        ],
        image_advtab: true,
        relative_urls: false,
        convert_urls: false,
        remove_script_host: false,
        extended_valid_elements: 'span[*],a[*]',
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
});

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
                        return node.id === '#' ? '/Admin/Views/MasterPages/WebService.asmx/GetRootNodes' : '/Admin/Views/MasterPages/WebService.asmx/GetChildNodes';
                    else
                        return '/Admin/Views/MasterPages/WebService.asmx/SearchForNodes?filterText=' + filterText;

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
                    "DeletePermanently": {
                        "label": "Delete Permanently",
                        "action": function (obj) {
                            HandleContextMenuClick("DeletePermanently", obj.reference, node);
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
                    "DuplicateAndEdit": {
                        "label": "Duplicate And Edit",
                        "action": function (obj) {
                            HandleContextMenuClick("DuplicateAndEdit", obj.reference, node);
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
            url: "/Admin/Views/MasterPages/Webservice.asmx/HandleNodeDragDrop",
            data: "{'sourceMediaId':'" + sourceMediaId + "', 'parentMediaId':'" + parentMediaId + "', 'newPosition':'" + newPosition + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success:
            function (msg) {
                //RefreshSiteTreeViewAjaxPanel();
                RefreshSiteTreeNodeById(parentMediaId);
            },
            error: function (xhr, status, error) {
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

            var li = "<li mediadetailid='" + elem.parent().attr("mediadetailid") + "'><a class='delete'>x</a><span class='text'>" + elem.text() + "</span></li>";

            if (target.find("li[mediadetailid='" + elem.parent().attr("mediadetailid") + "']").length == 0) {
                target.append("<li mediadetailid='" + elem.parent().attr("mediadetailid") + "'><a class='delete'>x</a><span class='text'>" + elem.text() + "</span></li>");
            }
        }

    });

function pageLoad() {

    RefreshSiteTreeNodeById($("#SiteTree").jstree("get_selected")[0]);
    BindScrollMagic();
    BindDataTable();
    BindSortable();
    BindTabs();
    BindMultiFileUploaderImageLoadError();
    initAceEditors();

    if (typeof (BindActiveTabs) == 'function')
        BindActiveTabs();
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
        connectWith: '.dropZone.sortable',
        update: function (event, ui) {
        }
    });

    $(".MultiFileUploader .sortable").sortable({
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
    myObject.scene = new ScrollMagic.Scene({ offset: -45, triggerElement: selector, triggerHook: 0 })
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

function init() {
    BindTree();
    BindSortable();
    //BindScrollMagic();

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

    $("ul.dropZone").bind("DOMSubtreeModified", function () {
        UpdateValuesFromUL(this);
    });

    $("ul.dropZone").sortable({
        update: function (event, ui) {
            UpdateValuesFromUL(this);
        }
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

            $(elem).append("<li mediadetailid='" + this.id + "'><a class='delete'>x</a><span class='text'>" + this.name + "</span></li>");

        });
    }

    function UpdateValuesFromUL(elem) {
        var values = $(elem).find("input[type='hidden']");

        var arr = new Array();

        $(elem).children("li:not(.hidden)").each(function () {
            var mediadetailid = $(this).attr("mediadetailid");
            var name = $(this).children("span.text").text();

            if (name != "") {
                var obj = new Object();
                obj.name = name;
                obj.id = mediadetailid;

                arr.push(obj);
            }

        });

        var jsonString = JSON.stringify(arr);

        values.val(jsonString);
    }

    $(document).on("click", ".DeleteImage", function () {

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