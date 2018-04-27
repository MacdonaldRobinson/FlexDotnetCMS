<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualLayoutEditor.ascx.cs" Inherits="WebApplication.Controls.LoggedIn.VisualLayoutEditor" %>

<% if(BasePage.CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions))
   { 
%>

<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify-css.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify-html.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.6/jquery.jgrowl.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.6/jquery.jgrowl.min.css" />

<script type="text/javascript" src="/Scripts/tinymce/js/tinymce/tinymce.min.js"></script>

<style>
    .selectable-element-placeholder {
        border: 1px dashed;
        background-color: lightgray;
        opacity: .5;
    }

    #PageContent {
        padding-top: 20px;
    }
</style>

<script>

    function UpdateVisualEditor(source) {        
        var fieldCode = $(source).attr("data-fieldcode");
        var mediaDetailId = $(source).attr("data-mediadetailid");        

        if (mediaDetailId == "" || mediaDetailId == undefined) {
            mediaDetailId = $('.AddField.clicked').closest(".UseMainLayout").attr("data-mediadetailid");

            if (mediaDetailId == "" || mediaDetailId == undefined) {
                mediaDetailId = $('.AddField.clicked').closest("#PageContent").attr("data-mediadetailid");
            }

        }

        var url = "/Admin/Views/MasterPages/WebService.asmx/RenderField?mediaDetailId=" + mediaDetailId+"&fieldCode=" + fieldCode;

        $.get(url, function (data) {
            try {

				if ($('.AddField.clicked').closest(".col").length > 0) {
					$('.AddField.clicked').closest(".col").append(data);
				}
				else {
					$('.AddField.clicked').closest("#PageContent").append(data);
				}
            }
            catch (ex) {

            }

            SaveLayout();
        });

        //$('.AddField.clicked', window.parent.document).closest(".col").append(code);        
    }

    function CleanHtml() {
        $(".row[class~=ui-sortable], .col[class~=ui-sortable], .UseMainLayout[class~=ui-sortable]").sortable("destroy");
    }

    function initVisualLayoutEditor()
    {
        head.ready(function () {
            head.load("/Controls/LoggedIn/css/VisualLayoutEditor.css", function () {
                initEvents();
            });
        });
    }    

    function initTinyMCE() {
        tinymce.editors = [];
        tfm_path = BaseUrl + "Scripts/tinyfilemanager.net";
        tinymce.init({
            selector: ".editor",
            inline: true,
            content_css: BaseUrl + "Views/MasterPages/SiteTemplates/css/main.css, " + BaseUrl + "Admin/Styles/editor.css",
            menubar: false,
            plugins: [
                'advlist autolink lists link image charmap print preview hr anchor pagebreak',
                'searchreplace wordcount visualblocks visualchars fullscreen',
                'insertdatetime media youtube nonbreaking save table contextmenu directionality',
                'emoticons template paste textcolor colorpicker textpattern imagetools ace imgmap table'
            ],
            toolbar1: 'file undo redo | styleselect | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | insert table link image imgmap media youtube ace',
            templates: [
            ],
            image_advtab: true,
            relative_urls: false,
            convert_urls: false,
            remove_script_host: false,
            verify_html: false,
            valid_children: '+a[div|p|ul|ol|li|h1|span|h2|h3|h4|h5|h5|h6]',
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
    }

    function initEvents() {                    

        //$(".col").addClass("editor");

        //initTinyMCE();

        $("#PageContent .fieldControls a.remove").show();        

        $("#PageContent, .UseMainLayout").each(function () {
            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = $("#ToolBarTemplate").clone();
                toolBar.find(".ToolBar").css("display", "block");
				toolBar.find(".AddField").css("display", "none");				
				toolBar.find(".AddColumn").css("display", "none");
				toolBar.find(".FullWidthToggle").css("display", "none");
                toolBar.find(".DeleteColumn").css("display", "none");
                toolBar.find(".IncreaseColumnSize").css("display", "none");
                toolBar.find(".DecreaseColumnSize").css("display", "none");

                $(this).prepend(toolBar.html());                
            }
        });

        $("#PageContent .row").each(function () {
            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = $("#ToolBarTemplate").clone();
                toolBar.find(".ToolBar").css("display", "block");
                toolBar.find(".AddRow").css("display", "none");
                toolBar.find(".AddField").css("display", "none");
                toolBar.find(".DeleteRow").css("display", "inline");
				toolBar.find(".AddColumn").css("display", "inline");				
                toolBar.find(".DeleteColumn").css("display", "none");
                toolBar.find(".IncreaseColumnSize").css("display", "none");
				toolBar.find(".DecreaseColumnSize").css("display", "none");

				if ($(this).hasClass("full-width")) {
					toolBar.find(".FullWidthToggle").addClass("active");
				}

                $(this).prepend(toolBar.html());
            }
        });

        $("#PageContent .col").each(function () {
            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = $("#ToolBarTemplate").clone();
                toolBar.find(".ToolBar").css("display", "block");
                toolBar.find(".AddRow").css("display", "inline");
				toolBar.find(".AddColumn").css("display", "none");
                toolBar.find(".DeleteColumn").css("display", "inline");
                toolBar.find(".IncreaseColumnSize").css("display", "inline");
				toolBar.find(".DecreaseColumnSize").css("display", "inline");

				if ($(this).hasClass("full-width")) {
					toolBar.find(".FullWidthToggle").addClass("active");
				}

                $(this).prepend(toolBar.html());
            }
        });

        //$(".UseMainLayout, #PageContent").addClass("container-fluid");
        BindDragDrop();

        function BindDragDrop()
        {
			var placeHolderClasses = "ui-state-highlight";//"selectable-element-placeholder";
            var startFunction = function (event, ui) {
                var placeholder = ui.placeholder[0];
                var item = ui.item[0];

				$(placeholder).addClass($(item).attr("class"));   
				ui.placeholder.height(ui.helper.height());
            }

            $("#PageContent, .UseMainLayout").sortable({
                tolerance: "pointer",
                handle: ".Handle",
                revert: true,
                placeholder: placeHolderClasses,
                helper: 'clone',
                forceHelperSize: true,
                forcePlaceholderSize: true,
                start: startFunction
            });

            $("#PageContent .row").sortable({
                tolerance: "pointer",
                handle: ".Handle",
                revert: true,
                connectWith: '.row',
                placeholder: placeHolderClasses,
                helper: 'clone',
                forceHelperSize: true,
                forcePlaceholderSize: true,
                start: startFunction
            });

            /*$(".col").sortable({
                tolerance: "pointer",
                handle: ".Handle",
                revert: true,
                connectWith: '.col'
            });*/

            $("#PageContent .col").sortable({
                items: "> .field, > .row",
                handle: ".Handle, .edit",
                tolerance: "pointer",
                connectWith: '.col',
                revert: true,
                placeholder: placeHolderClasses,
                helper: 'clone',
                forceHelperSize: true,
                forcePlaceholderSize: true,
                start: startFunction
            });            
        }

        $(document).on("mouseover", ".UseMainLayout, .row, .col", function () {
            $(this).children(".ToolBar").show();
            $(this).addClass("active");
        });

        $(document).on("mouseout", ".UseMainLayout, .row, .col", function () {
            //$(this).children(".ToolBar").hide();
            $(this).removeClass("active");
        });

        function UpdateClassesFields()
        {
            $(".UpdateClasses").each(function () {
                UpdateClassesField(this);
            });
		}

		function ScrollToElement(elem, animate) {
			$('html,body').animate({ scrollTop: elem.offset().top }, 'slow', function () {
				if (animate) {
					elem.effect("highlight", {}, 200);
				}
			});
		}

        function UpdateClassesField(elem)
        {
            var classes = $(elem).closest(".ToolBar").parent().attr("class");
            $(elem).val(classes);
        }

        $(document).on("click", ".AddRow", function () {
            var toolBar = $("#ToolBarTemplate").clone();
            toolBar.find(".AddRow").css("display", "none"); 
			toolBar.find(".AddField").css("display", "none");			
            toolBar.find(".DeleteRow").css("display", "inline");

            var row = $("<div class='row'></div>");
            row.append(toolBar.html());

			$(this).closest(".col, #PageContent").append(row);

			ScrollToElement(row, true);

            UpdateClassesFields();
            BindDragDrop()                        

        });

        $(document).on("click", ".AddColumn", function () {   
            var toolBar = $("#ToolBarTemplate").clone();
            toolBar.find(".AddColumn").css("display", "none");
            toolBar.find(".AddField").css("display", "inline");
			toolBar.find(".DeleteColumn").css("display", "inline");			
            toolBar.find(".IncreaseColumnSize").css("display", "inline");            
            toolBar.find(".DecreaseColumnSize").css("display", "inline");

            var column = $("<div class='col col-md-4'></div>");
            column.append(toolBar.html());
            
			$(this).closest(".row").append(column);

			ScrollToElement(column, false);

            UpdateClassesFields();
            BindDragDrop();            
		});

		$(document).on("click", ".FullWidthToggle", function () {
			var colOrRow = $(this).closest(".ToolBar").parent();

			colOrRow.toggleClass("full-width");

			if (colOrRow.hasClass("full-width")) {
				$(this).addClass("active");
			}
			else {
				$(this).removeClass("active");
			}
		});

        $(document).on("click", ".AddField", function () {

            $(".AddField").removeClass("clicked");
            $(this).addClass("clicked");

            var mediaDetailId = $(this).closest(".UseMainLayout").attr("data-mediadetailid");

            if (mediaDetailId == undefined)
            {
                mediaDetailId = $(this).closest("#PageContent").attr("data-mediadetailid");
            }

            var source = this;

            $.colorbox({
                href: "/Admin/Views/PageHandlers/FieldSelector/Default.aspx?mediaDetailId=" + mediaDetailId,
                width: colorBoxWidth,
                height: colorBoxHeight,
                iframe: true,
                fixed: true,
                onClosed: function () {
                    //CreateFieldsEditor();
                    //ShowFieldsEditor();                    
                }                
            });
        });

        $(document).on("click", ".DeleteRow", function () {
            var canRemove = confirm("Are you sure you want to remove this row?");

            if (canRemove)
            {
                $(this).closest(".row").remove();
            }
        });

        $(document).on("click", ".DeleteColumn", function () {
            var canRemove = confirm("Are you sure you want to remove this column?");

            if (canRemove)
            {
                $(this).closest(".col").remove();
            }
        });


        $(document).on("keyup", ".UpdateClasses", function () {
            var wrapper = $(this).closest(".ToolBar").parent();
            var classes = $(this).val();

            if (wrapper.hasClass("col"))
            {                
                if (classes.indexOf("col ") == -1)
                {
                    classes = "col " + classes;
                }
            }
            else if (wrapper.hasClass("row"))
            {
                if (classes.indexOf("row ") == -1)
                {
                    classes = "row " + classes;
                }
            }

            wrapper.attr("class", classes);       

            UpdateClassesField(this);
        });        

        $(document).on("click", ".IncreaseColumnSize, .DecreaseColumnSize", function () {
            var rootElement = $(this);
            var wrapper = $(this).closest(".ToolBar").parent();
            var classes = wrapper.attr("class").split(' ');

            $(classes).each(function (index) {
                var updateClass = false;
                var segments = this.split('-');
                $(segments).each(function (index) {
                    var number = parseInt(this);
                    if (number == this)
                    {
                        updateClass = true;

                        var newNumber = this;

                        if (rootElement.hasClass("IncreaseColumnSize"))
                        {
                            newNumber = number + 1;
                        }
                        else if (rootElement.hasClass("DecreaseColumnSize"))
                        {
                            newNumber = number -1;
                        }

                        if (newNumber > 0 && newNumber <= 12)
                        {
                            segments[index] = newNumber;                        
                        }
                    }                    
                });

                var newClass = segments.join('-');

                if (this != newClass)
                {
                    classes[index] = newClass;                    
                }
            });  

            var updatedClasses = classes.join(' ');

            wrapper.attr("class", updatedClasses); 

        });        
    }
</script>

<div id="ToolBarTemplate" style="display:none;">    
    <a href="javascript:void(0)" class="Handle"><i class="fa fa-arrows" aria-hidden="true"></i></a>
    <div class="ToolBar">                
        <a href="javascript:void(0)" class="AddRow"><i class="fa fa-plus" aria-hidden="true"></i> Add Row</a>
        <a href="javascript:void(0)" class="AddColumn"><i class="fa fa-plus" aria-hidden="true"></i> Add Column</a>		
        <a href="javascript:void(0)" class="AddField"><i class="fa fa-plus" aria-hidden="true"></i> Add Field</a>
        <a href="javascript:void(0)" class="DeleteRow"><i class="fa fa-trash" aria-hidden="true"></i> Delete Row</a>
        <a href="javascript:void(0)" class="DeleteColumn"><i class="fa fa-trash" aria-hidden="true"></i> Delete Column</a>
		<a href="javascript:void(0)" class="FullWidthToggle"><i class="fa fa-arrows-h" aria-hidden="true"></i> Stretch</a>
        <a href="javascript:void(0)" class="IncreaseColumnSize">+</a>
        <a href="javascript:void(0)" class="DecreaseColumnSize">-</a>
        <input type="text" class="UpdateClasses" />
    </div>
</div>

<script>

    $(document).ready(function () {
        $(document).on("click", "#SaveLayout", function () {
            SaveLayout();
        });
    });

    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    function htmlDecode(value) {
        return $('<div/>').html(value).text();
    }

    function SaveLayout()
    {
        $("#PageContent").each(function () {
            var mediaDetailId = $(this).attr("data-mediadetailid");
            var rootMediaId = $(this).attr("data-mediaid");

            var clone = $(this).clone(this);
            
            clone.find(".editor").removeAttr("contenteditable");
            clone.find(".editor").removeAttr("spellcheck");
            //clone.find(".editor").removeClass("editor");

            clone.find(".ToolBar, .Handle").remove();
            clone.find(".field").each(function () {
                var fieldCode = $(this).attr("data-fieldcode")
                var mediaId = $(this).attr("data-mediaid")

                var shortCode = "{Field:" + fieldCode + "}";

                if (rootMediaId != mediaId) {
                    shortCode = "{{Load:" + mediaId + "}.Field:" + fieldCode + "}";
                }

                $(this).after(shortCode);
                $(this).remove();
            });

            clone.find(".row").removeClass("ui-sortable");
            clone.find(".col").removeClass("ui-sortable");
            clone.find(".col").removeAttr("style");
            clone.find(".row").removeAttr("style");

            var newHtml = clone.html().trim();

            newHtml = newHtml.replace(/^\s*\n/gm, '');            
            newHtml = html_beautify(newHtml, { preserve_newlines: true });

            $.post("/WebServices/IMediaDetails.asmx/SaveUseMainLayout", { mediaDetailId: mediaDetailId, html: encodeURI(newHtml) }, function (data) {                
                window.location.reload();                
            });

        });
        /*$(".UseMainLayout").each(function () {
            var mediaDetailId = $(this).attr("data-mediadetailid");
            var rootMediaId = $(this).attr("data-mediaid");

            var clone = $(this).clone(this);

            clone.find(".ToolBar, .Handle").remove();
            clone.find(".field").each(function () {                
                var fieldCode = $(this).attr("data-fieldcode")
                var mediaId = $(this).attr("data-mediaid")

                var shortCode = "{Field:" + fieldCode + "}";

                if (rootMediaId != mediaId)
                {
                    shortCode = "{{Load:" + mediaId + "}.Field:" + fieldCode + "}";
                }                

                $(this).after(shortCode);
                $(this).remove();                
            });

            clone.find(".row").removeClass("ui-sortable");
            clone.find(".col").removeClass("ui-sortable");
            clone.find(".col").removeAttr("style");
            clone.find(".row").removeAttr("style");

            var newHtml = clone.html().trim();
            newHtml = newHtml.replace(/\s+/g, " ");          
            newHtml = html_beautify(newHtml);                        

            $.post("/WebServices/IMediaDetails.asmx/SaveUseMainLayout", { mediaDetailId: mediaDetailId, html: escape(newHtml) }, function (data) {
                window.location.reload();
            });

        });*/
    }
</script>

<a href="javascript:void(0)" id="SaveLayout" class="button">Save Layout</a>
<% } %>