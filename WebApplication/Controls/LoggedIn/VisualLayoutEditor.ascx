<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualLayoutEditor.ascx.cs" Inherits="WebApplication.Controls.LoggedIn.VisualLayoutEditor" %>

<% if(BasePage.CurrentUser.HasPermission(PermissionsEnum.AccessAdvanceOptions))
   { 
%>
<style>
	.container, .container-fluid {
		padding-right:15px;
		padding-left: 15px;
	}
</style>

<script>
	popCloseRefreshPage = false;
</script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify-css.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify-html.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.6/jquery.jgrowl.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.6/jquery.jgrowl.min.css" />

<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.3.1/ace.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.3.1/ext-language_tools.js"></script>

<script type="text/javascript" src="/Scripts/tinymce/js/tinymce/tinymce.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.blockUI/2.70/jquery.blockUI.js"></script>


<style>
    .selectable-element-placeholder {
        border: 1px dashed;
        /*background-color: lightgray;*/
        opacity: .5;
    }

    #PageContent {
        padding-top: 20px;
    }

	#OtherOptions {
		display: none;
	}

</style>


<div id="OtherOptions" title="Other Options">
  <div>
	  <label>Element Classes</label>
	  <input type="text" id="ElementClasses" />
  </div>
  <div>
	  <label>Animate CSS</label>
	  <select id="ElementAnimateCss">
		<option value="" selected="selected">--- No Animate Classes ---</option>

        <optgroup label="Attention Seekers">
          <option value="bounce">bounce</option>
          <option value="flash">flash</option>
          <option value="pulse">pulse</option>
          <option value="rubberBand">rubberBand</option>
          <option value="shake">shake</option>
          <option value="swing">swing</option>
          <option value="tada">tada</option>
          <option value="wobble">wobble</option>
          <option value="jello">jello</option>
        </optgroup>

        <optgroup label="Bouncing Entrances">
          <option value="bounceIn">bounceIn</option>
          <option value="bounceInDown">bounceInDown</option>
          <option value="bounceInLeft">bounceInLeft</option>
          <option value="bounceInRight">bounceInRight</option>
          <option value="bounceInUp">bounceInUp</option>
        </optgroup>

        <optgroup label="Bouncing Exits">
          <option value="bounceOut">bounceOut</option>
          <option value="bounceOutDown">bounceOutDown</option>
          <option value="bounceOutLeft">bounceOutLeft</option>
          <option value="bounceOutRight">bounceOutRight</option>
          <option value="bounceOutUp">bounceOutUp</option>
        </optgroup>

        <optgroup label="Fading Entrances">
          <option value="fadeIn">fadeIn</option>
          <option value="fadeInDown">fadeInDown</option>
          <option value="fadeInDownBig">fadeInDownBig</option>
          <option value="fadeInLeft">fadeInLeft</option>
          <option value="fadeInLeftBig">fadeInLeftBig</option>
          <option value="fadeInRight">fadeInRight</option>
          <option value="fadeInRightBig">fadeInRightBig</option>
          <option value="fadeInUp">fadeInUp</option>
          <option value="fadeInUpBig">fadeInUpBig</option>
        </optgroup>

        <optgroup label="Fading Exits">
          <option value="fadeOut">fadeOut</option>
          <option value="fadeOutDown">fadeOutDown</option>
          <option value="fadeOutDownBig">fadeOutDownBig</option>
          <option value="fadeOutLeft">fadeOutLeft</option>
          <option value="fadeOutLeftBig">fadeOutLeftBig</option>
          <option value="fadeOutRight">fadeOutRight</option>
          <option value="fadeOutRightBig">fadeOutRightBig</option>
          <option value="fadeOutUp">fadeOutUp</option>
          <option value="fadeOutUpBig">fadeOutUpBig</option>
        </optgroup>

        <optgroup label="Flippers">
          <option value="flip">flip</option>
          <option value="flipInX">flipInX</option>
          <option value="flipInY">flipInY</option>
          <option value="flipOutX">flipOutX</option>
          <option value="flipOutY">flipOutY</option>
        </optgroup>

        <optgroup label="Lightspeed">
          <option value="lightSpeedIn">lightSpeedIn</option>
          <option value="lightSpeedOut">lightSpeedOut</option>
        </optgroup>

        <optgroup label="Rotating Entrances">
          <option value="rotateIn">rotateIn</option>
          <option value="rotateInDownLeft">rotateInDownLeft</option>
          <option value="rotateInDownRight">rotateInDownRight</option>
          <option value="rotateInUpLeft">rotateInUpLeft</option>
          <option value="rotateInUpRight">rotateInUpRight</option>
        </optgroup>

        <optgroup label="Rotating Exits">
          <option value="rotateOut">rotateOut</option>
          <option value="rotateOutDownLeft">rotateOutDownLeft</option>
          <option value="rotateOutDownRight">rotateOutDownRight</option>
          <option value="rotateOutUpLeft">rotateOutUpLeft</option>
          <option value="rotateOutUpRight">rotateOutUpRight</option>
        </optgroup>

        <optgroup label="Sliding Entrances">
          <option value="slideInUp">slideInUp</option>
          <option value="slideInDown">slideInDown</option>
          <option value="slideInLeft">slideInLeft</option>
          <option value="slideInRight">slideInRight</option>

        </optgroup>
        <optgroup label="Sliding Exits">
          <option value="slideOutUp">slideOutUp</option>
          <option value="slideOutDown">slideOutDown</option>
          <option value="slideOutLeft">slideOutLeft</option>
          <option value="slideOutRight">slideOutRight</option>
          
        </optgroup>
        
        <optgroup label="Zoom Entrances">
          <option value="zoomIn">zoomIn</option>
          <option value="zoomInDown">zoomInDown</option>
          <option value="zoomInLeft">zoomInLeft</option>
          <option value="zoomInRight">zoomInRight</option>
          <option value="zoomInUp">zoomInUp</option>
        </optgroup>
        
        <optgroup label="Zoom Exits">
          <option value="zoomOut">zoomOut</option>
          <option value="zoomOutDown">zoomOutDown</option>
          <option value="zoomOutLeft">zoomOutLeft</option>
          <option value="zoomOutRight">zoomOutRight</option>
          <option value="zoomOutUp">zoomOutUp</option>
        </optgroup>

        <optgroup label="Specials">
          <option value="hinge">hinge</option>
          <option value="jackInTheBox">jackInTheBox</option>
          <option value="rollIn">rollIn</option>
          <option value="rollOut">rollOut</option>
        </optgroup>
      
	  </select>
  </div>
</div>

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

				if ($('.AddField.clicked').closest(".col, .container, .container-fluid").length > 0) {
					$('.AddField.clicked').closest(".col, .container, .container-fluid").append(data);
				}
				else {
					$('.AddField.clicked').closest("#PageContent").append(data);
				}

				CreateFieldsEditor();

				$("#PageContent .fieldControls a.remove").show();

				$.colorbox.close();

			}
			catch (ex) {
				console.log(ex);
			}

            SaveLayout();
        });

        //$('.AddField.clicked', window.parent.document).closest(".col").append(code);        
    }

    function CleanHtml() {
        $(".row[class~=ui-sortable], .col[class~=ui-sortable], .UseMainLayout[class~=ui-sortable]").sortable("destroy");
    }

    function initVisualLayoutEditor(head)
    {
        head.ready(function () {
            head.load("/Controls/LoggedIn/css/VisualLayoutEditor.css", function () {
                initEvents();
            });
        });
	}    

	var tinyMCETemplates = [];
	function LoadTinyMCETemplates() {
		if (tinyMCETemplates.length == 0) {
			$.get("/Admin/Scripts/TinyMCETemplates.html", function (data) {
				$(data).find(".Template").each(function (index, template) {
					var title = $(this).attr("data-title");
					var description = $(this).attr("data-description");
					var content = $(template).html();

					var templateObj = { title: title, description: description, content: content };
					tinyMCETemplates.push(templateObj);
				});
			});
		}    
	}

    function initTinyMCE() {
		LoadTinyMCETemplates();

        tinymce.editors = [];
        tfm_path = BaseUrl + "Scripts/tinyfilemanager.net";
        tinymce.init({
            selector: ".editor",
            inline: true,
            content_css: BaseUrl + "Views/MasterPages/SiteTemplates/css/main.css, " + BaseUrl + "Admin/Styles/editor.css",
            menubar: false,
			content_css: BaseUrl + "FrontEnd/styles/css/main.css, " + BaseUrl + "Admin/Styles/editor.css",
			menubar: false,
			template_popup_width:"1024",
			template_popup_height: "600",
			plugins: [
				'advlist autolink lists link image charmap print preview hr anchor pagebreak',
				'searchreplace wordcount visualblocks visualchars fullscreen',
				'insertdatetime media youtube nonbreaking save table contextmenu directionality',
				'emoticons template paste textcolor colorpicker textpattern ace_beautify imgmap table map'
			],
			toolbar1: 'undo redo | paste pastetext | bold italic underline strikethrough superscript subscript charmap emoticons | formatselect blockquote',
			toolbar2: 'alignleft aligncenter alignright alignjustify outdent indent | bullist numlist | insert table | anchor link image imgmap media youtube map | visualblocks ace_beautify',
			templates: tinyMCETemplates,
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

	var otherOptionsTargetElem = null;


	function createToolBar(mode)
	{
		var toolBar = $("#ToolBarTemplate").clone();

		if(mode == "Root")
		{
            toolBar.find(".ToolBar").css("display", "block");

			toolBar.find(".AddRow").css("display", "none");
			toolBar.find(".OtherOptions").css("display", "none");
			toolBar.find(".DeleteSection").css("display", "none");
			toolBar.find(".AddField").css("display", "none");				
			toolBar.find(".AddColumn").css("display", "none");
			toolBar.find(".FullWidthToggle").css("display", "none");
            toolBar.find(".DeleteColumn").css("display", "none");
            toolBar.find(".IncreaseColumnSize").css("display", "none");
            toolBar.find(".DecreaseColumnSize").css("display", "none");
			toolBar.find(".OtherOptions").css("display", "none");
			toolBar.find(".FullWidthToggle").css("display", "none");
			toolBar.find(".DeleteEditor").css("display", "none");
			toolBar.find(".AddEditor").css("display", "none");
			toolBar.find(".Duplicate").css("display", "none");
		}
		else if(mode == "AddSection")
		{
			toolBar.find(".AddRow").css("display", "inline"); 
			toolBar.find(".AddSection").css("display", "inline"); 
			toolBar.find(".DeleteSection").css("display", "inline"); 
            toolBar.find(".AddColumn").css("display", "none"); 
			toolBar.find(".AddField").css("display", "inline");			
            toolBar.find(".DeleteRow").css("display", "none");
            toolBar.find(".IncreaseColumnSize").css("display", "none");            
			toolBar.find(".DecreaseColumnSize").css("display", "none");
			toolBar.find(".FullWidthToggle").css("display", "inline");
			toolBar.find(".OtherOptions").css("display", "none");
			toolBar.find(".AddEditor").css("display", "inline");
			toolBar.find(".DeleteEditor").css("display", "none");
			toolBar.find(".Duplicate").css("display", "none");
		}
		else if(mode == "AddRow")
		{
			toolBar.find(".AddSection").css("display", "none"); 
			toolBar.find(".DeleteSection").css("display", "none"); 
            toolBar.find(".AddRow").css("display", "none"); 
			toolBar.find(".AddField").css("display", "none");			
			toolBar.find(".DeleteRow").css("display", "inline");
			toolBar.find(".FullWidthToggle").css("display", "none");
			toolBar.find(".OtherOptions").css("display", "none");
			toolBar.find(".AddEditor").css("display", "none");
			toolBar.find(".DeleteEditor").css("display", "none");
			toolBar.find(".Duplicate").css("display", "none");
		}
		else if(mode == "AddColumn")
		{
			toolBar.find(".AddSection").css("display", "none"); 
			toolBar.find(".DeleteSection").css("display", "none"); 
            toolBar.find(".AddColumn").css("display", "none");
            toolBar.find(".AddField").css("display", "inline");
			toolBar.find(".DeleteColumn").css("display", "inline");			
            toolBar.find(".IncreaseColumnSize").css("display", "inline");            
			toolBar.find(".DecreaseColumnSize").css("display", "inline");
			toolBar.find(".FullWidthToggle").css("display", "none");
			toolBar.find(".OtherOptions").css("display", "none");
			toolBar.find(".AddEditor").css("display", "inline");
			toolBar.find(".DeleteEditor").css("display", "none");
			toolBar.find(".Duplicate").css("display", "none");
		}
		else if(mode == "AddEditor")
		{
			toolBar.find(".AddRow").css("display", "none");
			toolBar.find(".AddSection").css("display", "none"); 
			toolBar.find(".DeleteSection").css("display", "none");
			toolBar.find(".AddField").css("display", "none");				
			toolBar.find(".AddColumn").css("display", "none");
			toolBar.find(".FullWidthToggle").css("display", "none");
            toolBar.find(".DeleteColumn").css("display", "none");
            toolBar.find(".IncreaseColumnSize").css("display", "none");
            toolBar.find(".DecreaseColumnSize").css("display", "none");
			toolBar.find(".OtherOptions").css("display", "none");
			toolBar.find(".FullWidthToggle").css("display", "none");
			toolBar.find(".DeleteEditor").css("display", "inline");
			toolBar.find(".AddEditor").css("display", "none");
			toolBar.find(".Duplicate").css("display", "inline");
		}

		return toolBar;
	}

	/*window.onbeforeunload = function () {
		if (promptBeforeLeave) {
			return "Are you sure that you want to exit?";
		}
		else {
			return;
		}
	   //if we return nothing here (just calling return;) then there will be no pop-up question at all
	   //return;
	};*/

    function initEvents() {                    

        //$(".col").addClass("editor");

        initTinyMCE();	

		$("#OtherOptions").dialog({
			autoOpen: false,
			show: {
			effect: "slideDown",
				duration: 200
			},
			hide: {
				effect: "slideUp",
				duration: 200
			}
		});

		$(document).on("click", ".OtherOptions", function () {						
			UpdateClassesField(this);
			$("#OtherOptions").dialog("open");
		})

        $("#PageContent .fieldControls a.remove").show();        

		$("#PageContent, .UseMainLayout").each(function () {

			if ($(this).children(".ToolBar").length == 0) {
				var toolBar = createToolBar("Root");


                $(this).prepend(toolBar.html());                
            }
        });

		$("#PageContent .container, #PageContent .container-fluid").each(function () {

			if ($(this).closest(".field").length > 0)
				return;

			if ($(this).closest(".editor").length > 0)
				return;

			if ($(this).children(".ToolBar").length == 0) {
				var toolBar = createToolBar("AddSection");
				
				if ($(this).hasClass("full-width")) {
					toolBar.find(".FullWidthToggle").addClass("active");
				}				

                $(this).prepend(toolBar.html());
            }
        });

		$("#PageContent .row").each(function () {
			if ($(this).closest(".field").length > 0)
				return;

			if ($(this).closest(".editor").length > 0)
				return;

            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = createToolBar("AddRow");

				if ($(this).hasClass("full-width")) {
					toolBar.find(".FullWidthToggle").addClass("active");
				}

                $(this).prepend(toolBar.html());
            }
        });

		$("#PageContent .col").each(function () {

			if ($(this).closest(".field").length > 0)
				return;

			if ($(this).closest(".editor").length > 0)
				return;

            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = createToolBar("AddColumn");

				if ($(this).hasClass("full-width")) {
					toolBar.find(".FullWidthToggle").addClass("active");
				}

				$(this).prepend(toolBar.html());				
            }
		});


		$("#PageContent .editor").each(function () {

			if ($(this).closest(".field").length > 0)
				return;

            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = createToolBar("AddEditor");

				if ($(this).hasClass("full-width")) {
					toolBar.find(".FullWidthToggle").addClass("active");
				}

				$(this).closest(".layout-wrapper").prepend(toolBar.html());		
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

            $(".container, .container-fluid").sortable({
                tolerance: "pointer",
                handle: ".Handle",
                revert: true,
                connectWith: '.container, .container-fluid',
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

			$("#PageContent .layout-wrapper").sortable({				
                handle: ".Handle",
                tolerance: "pointer",
                connectWith: '.layout-wrapper',
                revert: true,
                placeholder: placeHolderClasses,
                helper: 'clone',
                forceHelperSize: true,
                forcePlaceholderSize: true,
                start: startFunction
			});

		}

        $(document).on("mouseover", ".UseMainLayout, .row, .col, .container, .container-fluid", function () {
            //$(this).children(".ToolBar").show();
            $(this).addClass("active");
        });

        $(document).on("mouseout", ".UseMainLayout, .row, .col, .container, .container-fluid", function () {
            //$(this).children(".ToolBar").hide();
            $(this).removeClass("active");
        });

		function ScrollToElement(elem, animate) {
			$('html,body').animate({ scrollTop: elem.offset().top-300 }, 'slow', function () {
				if (animate) {
					elem.effect("highlight", {}, 200);
				}
			});
		}

        function UpdateClassesField(elem)
		{
			otherOptionsTargetElem = $(elem).closest(".ToolBar, .fieldControls").parent();
			var cloneElem = otherOptionsTargetElem.clone();
			cloneElem.removeClass("active");
			cloneElem.removeClass("ui-sortable");			
            var classes = cloneElem.attr("class");
			$("#ElementClasses").val(classes);
        }

        $(document).on("click", ".AddRow", function () {
			var toolBar = createToolBar("AddRow");

            var row = $("<div class='row'></div>");
            row.append(toolBar.html());

			$(this).closest(".container, .container-fluid, .col").append(row);

			ScrollToElement(row, true);            
            BindDragDrop()                        

        });

        $(document).on("click", ".AddSection", function () {
            var toolBar = createToolBar("AddSection");

            var container = $("<section class='container'></section>");
            container.append(toolBar.html());

			var siblingSelector = $(this).closest(".container, .container-fluid, section");
			var parentSelector = $(this).closest("#PageContent, .UseMainLayout");

			if (siblingSelector.length != 0) {
				siblingSelector.after(container);
			}
			else {
				parentSelector.append(container);
			}

			ScrollToElement(container, true);            
            BindDragDrop()                        

		});

        $(document).on("click", ".AddEditor", function () {
			var toolBar = createToolBar("AddEditor");

			var container = $("<div class='layout-wrapper'>" + toolBar.html() + "<div class='editor'></div></div>");			

			$(this).closest(".container, .container-fluid, .col, .row").append(container);

			ScrollToElement(container, true);     
			initTinyMCE();
			BindDragDrop();                       

        });

        $(document).on("click", ".AddColumn", function () {   
            var toolBar = createToolBar("AddColumn");

            var column = $("<div class='col col-xl-12'></div>");
            column.append(toolBar.html());
            
			$(this).closest(".row").append(column);

			ScrollToElement(column, false);
            
            BindDragDrop();            
		});

		$(document).on("click", ".Duplicate", function () {
			var elem = $(this).closest(".col, .row, .container, .container-fluid, section, .layout-wrapper");
			var clone = elem.clone();

			elem.after(clone);

			initTinyMCE();

			ScrollToElement(clone, false);            
			BindDragDrop();    

		});

		$(document).on("click", ".FullWidthToggle", function () {
			var parent = $(this).closest(".ToolBar").parent();

			parent.toggleClass("full-width");

			if (parent.hasClass("full-width")) {
				$(this).addClass("active");
			}
			else {
				$(this).removeClass("active");
			}

			if (parent.hasClass("container")) {
				parent.addClass("container-fluid");
				parent.removeClass("container");
				$(this).addClass("active");
			}
			else if (parent.hasClass("container-fluid")) {
				parent.addClass("container");
				parent.removeClass("container-fluid");
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

        $(document).on("click", ".DeleteEditor", function () {
            var canRemove = confirm("Are you sure you want to remove this editor?");

            if (canRemove)
			{
				$(this).closest(".layout-wrapper").remove();
            }
		});

        $(document).on("click", ".DeleteSection", function () {
            var canRemove = confirm("Are you sure you want to remove this container?");

            if (canRemove)
			{
				//console.log($(this).closest(".container, container-fluid"))
                $(this).closest(".container, .container-fluid").remove();
            }
        });


		$(document).on("change", "#ElementAnimateCss", function (event) {

			$("#ElementAnimateCss").find("option").each(function () {				
				otherOptionsTargetElem.removeClass(this.value);
			});

			otherOptionsTargetElem.addClass("animated");
			otherOptionsTargetElem.addClass(this.value);
		});

		$(document).on("keyup", "#ElementClasses", function (event) {

			if (event.which == 32 || event.which == 13)
				return;
            
            var classes = $(this).val();

            if (otherOptionsTargetElem.hasClass("col"))
            {                
                if (classes.indexOf("col ") == -1)
                {
                    classes = "col " + classes;
                }
            }
            else if (otherOptionsTargetElem.hasClass("row"))
            {
                if (classes.indexOf("row ") == -1)
                {
                    classes = "row " + classes;
                }
            }

            otherOptionsTargetElem.attr("class", classes);                   
        });        

        $(document).on("click", ".IncreaseColumnSize, .DecreaseColumnSize", function () {
            var rootElement = $(this);
			var wrapper = $(this).closest(".ToolBar").parent();
			var classesStr = wrapper.attr("class");
			var seperator = "-";
			var currentWindowWidth = $(window).width();

			if (currentWindowWidth < 576) {
				seperator = "col-";
			}
			else if (currentWindowWidth >= 576 && currentWindowWidth < 768) {
				seperator = "col-sm-";
			}
			else if (currentWindowWidth >= 768 && currentWindowWidth < 992) {
				seperator = "col-md-";
			}
			else if (currentWindowWidth >= 992 && currentWindowWidth < 1200) {
				seperator = "col-lg-";
			}
			else if (currentWindowWidth >= 1200) {
				seperator = "col-xl-";
			}

			var regExpression = seperator + "[0-9]+";
			var regEx = new RegExp(regExpression,"gi"); 

			if (!regEx.test(classesStr)) {
				classesStr = classesStr + " " + seperator+"6";
			}

			var classes = classesStr.split(' ');

            $(classes).each(function (index) {
				var updateClass = false;								
				
				var segments = this.split(seperator);				

				$(segments).each(function (index) {

					var numberStr = this.replace(seperator, "");
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

                var newClass = segments.join(seperator);

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
		<a href="javascript:void(0)" class="AddEditor"><i class="fa fa-plus" aria-hidden="true"></i> Add Editor</a>
        <a href="javascript:void(0)" class="AddRow"><i class="fa fa-plus" aria-hidden="true"></i> Add Row</a>
        <a href="javascript:void(0)" class="AddColumn"><i class="fa fa-plus" aria-hidden="true"></i> Add Column</a>		
        <a href="javascript:void(0)" class="AddField"><i class="fa fa-plus" aria-hidden="true"></i> Add Widget</a>
        <a href="javascript:void(0)" class="DeleteRow"><i class="fa fa-trash" aria-hidden="true"></i> Delete Row</a>
		<a href="javascript:void(0)" class="DeleteEditor"><i class="fa fa-trash" aria-hidden="true"></i> Delete Editor</a>
		<a href="javascript:void(0)" class="DeleteSection"><i class="fa fa-trash" aria-hidden="true"></i> Delete Section</a>
		<a href="javascript:void(0)" class="Duplicate"><i class="fa fa-plus" aria-hidden="true"></i> Duplicate</a>
        <a href="javascript:void(0)" class="DeleteColumn"><i class="fa fa-trash" aria-hidden="true"></i> Delete Column</a>
		<a href="javascript:void(0)" class="FullWidthToggle"><i class="fa fa-arrows-h" aria-hidden="true"></i> Stretch</a>
        <a href="javascript:void(0)" class="IncreaseColumnSize">+</a>
        <a href="javascript:void(0)" class="DecreaseColumnSize">-</a>
		<a href="javascript:void(0)" class="OtherOptions">Other</a>        
		<a href="javascript:void(0)" class="AddSection"><i class="fa fa-plus" aria-hidden="true"></i> Add Section</a>		
    </div>
</div>

<script>

	var promptBeforeLeave = true;

    $(document).ready(function () {
		$(document).on("click", "#SaveLayout", function () {
			promptBeforeLeave = false;
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

			if (typeof (tinyMCE) !== 'undefined') {
				var length = tinymce.editors.length;
				for (var i = length; i > 0; i--) {
					var editor = tinymce.editors[i - 1];
					var actualContent = editor.getContent();					

					$(editor.bodyElement).html(actualContent);					

				};
			}

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
            
            clone.find(".col, .row, .container, .container-fluid, .editor, .layout-wrapper").removeClass("ui-sortable");
            clone.find(".col").removeAttr("style");
            clone.find(".row").removeAttr("style");

            var newHtml = clone.html().trim();

            newHtml = newHtml.replace(/^\s*\n/gm, '');
			newHtml = newHtml.replace(/}\s*</gm, '}\n<');
			newHtml = newHtml.replace(/}\s*{/gm, '}\n{');
            newHtml = html_beautify(newHtml, { preserve_newlines: true });

			$.blockUI({ message: "Saving please wait ..." });

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