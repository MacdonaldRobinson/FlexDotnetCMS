/// <reference path="../Views/MasterPages/Webservice.asmx" />
var currentRadWindow = null;
var selectedItemId = null;

function RefreshGrid(sender, args) {
    if (typeof (getMasterTableView) == "undefined")
        return;

    var masterTable = getMasterTableView();

    if (masterTable != null)
        masterTable.rebind();
    else
        window.location.href = window.location.href;
}

function GetSelectedItemID() {
    if (typeof (getMasterTableView) == "undefined")
        return;

    var masterTable = getMasterTableView();

    if (masterTable == null)
        return;

    var selectedItems = masterTable.get_selectedItems();

    if (selectedItems.length == 0)
        return null;

    return selectedItems[0].getDataKeyValue("ID");
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

function clientButtonClicked(sender, args) {
    var toolBar = sender;
    var button = args.get_item();
    var currentRadWindow = "";

    var newWindowWidth = $telerik.$(window).width() - 20;
    var newWindowHeight = $telerik.$(window).height() - 20;

    $(window).resize(function () {
        newWindowWidth = $telerik.$(window).width() - 20;
        newWindowHeight = $telerik.$(window).height() - 20;

        currentRadWindow.setSize(newWindowWidth, newWindowHeight);

        currentRadWindow.center();
    });

    switch (button.get_value()) {
        case "Create":
            createUrl = "Detail.aspx?mediaTypeEnum=" + getParameterByName("mediaTypeEnum");

            currentRadWindow = window.radopen(createUrl);
            currentRadWindow.set_modal(true);
            currentRadWindow.setActive(true);

            currentRadWindow.setSize(newWindowWidth, newWindowHeight);
            currentRadWindow.center();
            break;
        case "Edit":
            editUrl = "Detail.aspx?mediaTypeEnum=" + getParameterByName("mediaTypeEnum") + "&id=" + GetSelectedItemID();

            currentRadWindow = window.radopen(editUrl);
            currentRadWindow.set_modal(true);
            currentRadWindow.setActive(true);
            currentRadWindow.setSize(newWindowWidth, newWindowHeight);
            currentRadWindow.center();
            break;
        case "Delete":
            if (confirm("Are you sure you want to delete this item?")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/DeleteItem",
                    data: "{'id':'" + GetSelectedItemID() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                function (msg) {
                    eval(msg.d);

                    if (msg.d.indexOf('Error') == -1)
                        RefreshGrid();
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
                    data: "{'id':'" + GetSelectedItemID() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            RefreshGrid();
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
                    data: "{'id':'" + GetSelectedItemID() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            RefreshGrid();
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
                    data: "{'id':'" + GetSelectedItemID() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            RefreshGrid();
                    },
                    error: function (xhr, status, error) {
                        DisplayErrorMessage("Error Approving Item", "An unexpected error occured while attempting to delete the item.");
                    }
                });
            }
            break;
        case "TakeOwnership":
            if (confirm("Are you sure you want to take ownership of all items assigned to the selected user with ID (" + GetSelectedItemID() + ") ?")) {
                jQuery.ajax({
                    type: "POST",
                    url: "ItemServices.asmx/TakeOwnership",
                    data: "{'id':'" + GetSelectedItemID() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            RefreshGrid();
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
                    data: "{'id':'" + GetSelectedItemID() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                    function (msg) {
                        eval(msg.d);

                        if (msg.d.indexOf('Error') == -1)
                            RefreshGrid();
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

$(document).ready(function () {
    $(window).resize(function () {
        sizeWindow();
    });
});

function HandleContextMenuClick(action, target) {
    var mediaDetailId = target.parent().attr("data-mediadetailid");
    var targetText = target.val();

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
                    //window.location.reload();
                    RefreshSiteTreeViewAjaxPanel();
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
                    RefreshSiteTreeViewAjaxPanel();
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
                    RefreshSiteTreeViewAjaxPanel();
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
                    RefreshSiteTreeViewAjaxPanel();
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
                    RefreshSiteTreeViewAjaxPanel();
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
                    RefreshSiteTreeViewAjaxPanel();
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
                    RefreshSiteTreeViewAjaxPanel();
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
                    RefreshSiteTreeViewAjaxPanel();
                },
                error: function (xhr, status, error) {
                    //console.log(xhr);
                }
            });
            break;
        case "DeletePermanently":
            var areYouSure = confirm('Are you sure you want to permanently delete the item "' + targetText + '" and all its associations and history if any? NOTE: This action is irreversible');

            if (areYouSure) {
                jQuery.ajax({
                    type: "POST",
                    url: "/Admin/Views/MasterPages/Webservice.asmx/DeletePermanently",
                    data: "{'id':'" + mediaDetailId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success:
                    function (msg) {
                        window.location.reload();
                        //RefreshSiteTreeViewAjaxPanel();
                    },
                    error: function (xhr, status, error) {
                        DisplayJsonException(xhr);
                    }
                });
            }
            break;
        case "ViewFrontEnd":
            //console.log(target.attr("data-frontendurl"));
            window.open(target.attr("data-frontendurl"));
            break;
    }
}

function SiteTreeView_OnClientNodeDropping(sender, eventArgs) {
    var sourceNodeId = eventArgs.get_sourceNode().get_value();

    var target = eventArgs.get_htmlElement();

    while (target) {
        if (target.id === "ContentBuckets") {
            eventArgs.set_cancel(true);
            jQuery.ajax({
                type: "POST",
                url: "/Admin/Views/PageHandlers/Media/ItemServices.asmx/AssignContentBucketToMedia",
                data: "{'id':'" + selectedMediaId + "','contentBucketId':'" + sourceNodeId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success:
                function (msg) {
                    ContentBucketsRefresh();
                },
                error: function (xhr, status, error) {
                    ContentBucketsRefresh();
                }
            });
        }

        target = target.parentNode;
    }

    if (eventArgs.get_destNode() == null)
        return;

    var destNodeId = eventArgs.get_destNode().get_value();
    var position = eventArgs.get_dropPosition();
    var destIndex = eventArgs.get_destNode().get_index();

    eventArgs.set_cancel(true);

    jQuery.ajax({
        type: "POST",
        url: "/Admin/Views/MasterPages/Webservice.asmx/HandleNodeDragDrop",
        data: "{'sourceNodeId':'" + sourceNodeId + "', 'destNodeId':'" + destNodeId + "', 'position':'" + position + "', 'destIndex':'" + destIndex + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success:
        function (msg) {
            //window.location.href = eventArgs.get_sourceNode().get_navigateUrl();
            RefreshSiteTreeViewAjaxPanel();
        },
        error: function (xhr, status, error) {
            //console.log(xhr);
        }
    });
}

function SiteTreeView_OnClientContextMenuShowing(sender, args) {
    var node = args.get_node();
    var menu = args.get_menu();

    var classes = node.get_cssClass();
    var ShowInMenu = menu._findItemByValue("ShowInMenu");
    var HideFromMenu = menu._findItemByValue("HideFromMenu");

    var Delete = menu._findItemByValue("Delete");
    var UnDelete = menu._findItemByValue("UnDelete");
    var DeletePermanently = menu._findItemByValue("DeletePermanently");

    if (classes != null) {
        if (classes.indexOf("isHidden") !== -1) {
            ShowInMenu.show();
            HideFromMenu.hide();
        }

        if (classes.indexOf("isDeleted") !== -1) {
            Delete.hide();
            UnDelete.show();
            DeletePermanently.show();
        }
    }
    else {
        ShowInMenu.hide();
        HideFromMenu.show();
        Delete.show();
        UnDelete.hide();
        DeletePermanently.hide();
    }
}

function sizeWindow() {
    if (currentRadWindow != null) {
        if (currentRadWindow.isVisible()) {
            currentRadWindow.center();
        }
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
        var text = $(this).text().replace(/\s/g, '');
        wordsArray.push("{Field:" + text + "}");
    });

    return wordsArray;
}

function initAceEditors() {
    var wordList = getFieldsAutoComplete();

    $(document).on("change", "#AttachEditorToBrowserPanel", function () {
        if(!$(this).is(":checked"))
        {
            $("#PreviewPanel")[0].src = $("#PreviewPanel")[0].src;
        }
        else
        {
            var textarea = $(this).parent().find("textarea");
            var value = textarea.val();

            $("#PreviewPanel")[0].contentWindow.document.getElementById("MainContentPlaceHolder").innerHTML = value;
        }
    });


    $(".AceEditor").each(function () {
        var id = $(this).attr("id");
        var editorId = $(this).attr("name") + "-editor";

        if ($("#PreviewPanel").length > 0) {
            if ($(this).hasClass("CanAttachToBrowserPanel"))
            {
                $("#" + id).parent().prepend("<input type='checkbox' id='AttachEditorToBrowserPanel' /> Attach editor to browser panel");
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

$(document).ready(function () {
    $('ul.sf-menu').superfish();
    $('.tabs').tabs();

    $('input.datetimepicker').datetimepicker({
        controlType: 'select',
        oneLine: true,
        dateFormat: 'yy-mm-dd',
        timeFormat: 'hh:mm:ss TT'
    });

    tfm_path = "/Scripts/tinyfilemanager.net";
    tinymce.init({
        selector: ".editor",
        content_css: "/Views/MasterPages/SiteTemplates/css/style.min.css",
        plugins: [
          'advlist autolink lists link image charmap print preview hr anchor pagebreak',
          'searchreplace wordcount visualblocks visualchars code fullscreen',
          'insertdatetime media nonbreaking save table contextmenu directionality',
          'emoticons template paste textcolor colorpicker textpattern imagetools'
        ],
        toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media',
        templates: [
        ],
        image_advtab: true,
        relative_urls: false,
        remove_script_host: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
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

    console.log(window.location.href);
    $('div.split-pane').splitPane();
    //$("#SiteTree ul").sortable({ connectWith: "ul" });
    BindTree();

    $(document).on('click', '#SiteTree a', function () {
        window.location.href = $(this).attr("href");
    });

});

function BindTree() {
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
        },
        "plugins": ["contextmenu", "dnd", "types"],
        "contextmenu": {
            "items": function ($node) {
                return {
                    "Create": {
                        "label": "Create Child",
                        "action": function (obj) {
                            //this.create(obj);
                            HandleContextMenuClick("CreateChild", obj.reference);
                        }
                    },
                    "Delete": {
                        "label": "Delete",
                        "action": function (obj) {
                            HandleContextMenuClick("Delete", obj.reference);
                        }
                    },
                    "UnDelete": {
                        "label": "Un Delete",
                        "action": function (obj) {
                            HandleContextMenuClick("UnDelete", obj.reference);
                        }
                    },
                    "DeletePermanently": {
                        "label": "Delete Permanently",
                        "action": function (obj) {
                            HandleContextMenuClick("DeletePermanently", obj.reference);
                        }
                    },
                    "ShowInMenu": {
                        "label": "Show In Menu",
                        "action": function (obj) {
                            HandleContextMenuClick("ShowInMenu", obj.reference);
                        }
                    },
                    "HideFromMenu": {
                        "label": "Hide From Menu",
                        "action": function (obj) {
                            HandleContextMenuClick("HideFromMenu", obj.reference);
                        }
                    },
                    "MoveUp": {
                        "label": "Move Up",
                        "action": function (obj) {
                            HandleContextMenuClick("MoveUp", obj.reference);
                        }
                    },
                    "MoveDown": {
                        "label": "Move Down",
                        "action": function (obj) {
                            HandleContextMenuClick("MoveDown", obj.reference);
                        }
                    },
                    "ViewFrontEnd": {
                        "label": "View Front End",
                        "action": function (obj) {
                            HandleContextMenuClick("ViewFrontEnd", obj.reference);
                        }
                    },
                    "Duplicate": {
                        "label": "Duplicate",
                        "action": function (obj) {
                            HandleContextMenuClick("Duplicate", obj.reference);
                        }
                    },
                    "DuplicateAndEdit": {
                        "label": "Duplicate And Edit",
                        "action": function (obj) {
                            HandleContextMenuClick("DuplicateAndEdit", obj.reference);
                        }
                    }
                };
            }
        }
    }).on('move_node.jstree', function (e, data) {
        var sourceMediaDetailId = data.node.data.mediadetailid;
        var parentNode = $("#" + data.parent);
        var parentMediaDetailId = parentNode.attr("data-mediadetailid");
        var newPosition = data.position;

        jQuery.ajax({
            type: "POST",
            url: "/Admin/Views/MasterPages/Webservice.asmx/HandleNodeDragDrop",
            data: "{'sourceMediaDetailId':'" + sourceMediaDetailId + "', 'parentMediaDetailId':'" + parentMediaDetailId + "', 'newPosition':'" + newPosition + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success:
            function (msg) {
                RefreshSiteTreeViewAjaxPanel();
                //window.location.reload();
            },
            error: function (xhr, status, error) {
                //console.log(xhr);
            }
        });

    });

}
$(document)
    .on('dnd_move.vakata', function (e, data) {
        //console.log("ran");
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
            var li = "<li data-mediadetailid='" + elem.parent().attr("data-mediadetailid") + "'><a class='delete'>x</a><span class='text'>" + elem.text() + "</span></li>";

            if (target.find("li[data-mediadetailid='" + elem.parent().attr("data-mediadetailid") + "']").length == 0) {
                target.append("<li data-mediadetailid='" + elem.parent().attr("data-mediadetailid") + "'><a class='delete'>x</a><span class='text'>" + elem.text() + "</span></li>");
            }
        }

    });

function pageLoad() {
    BindDataTable();
}

function BindDataTable() {
    $('.DataTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'excel',
        ]//,
        //"drawCallback": function (settings) {
        //    var api = this.api();
        //    var rows = api.rows({ page: 'current' }).nodes();
        //    var last = null;

        //    api.column(1, { page: 'current' }).data().each(function (group, i) {
        //        if (last !== group) {
        //            $(rows).eq(i).before(
        //                '<tr class="group"><td colspan="5">' + group + '</td></tr>'
        //            );

        //            last = group;
        //        }
        //    });
        //}
    });
}

function RefreshUpdatePanel(UpdatePanelClientId, OnAfterRefreshFunction) {
    //$("#" + UpdatePanelClientId).prepend("<div class='loading-panel'><div class='copy'>Loading ...</div></div>");

    var OnCompleteFunction = function () {
        //$("#" + UpdatePanelClientId).find("loading-panel").remove();
        OnAfterRefreshFunction();
    }

    OnUpdatePanelRefreshComplete(OnCompleteFunction);

    __doPostBack(UpdatePanelClientId, '');
}

function OnUpdatePanelRefreshComplete(OnUpdatePanelRefreshCompleteFunction) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OnUpdatePanelRefreshCompleteFunction);
}