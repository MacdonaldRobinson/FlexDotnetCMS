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

	var otherOptionsTargetElem = null;

    function initEvents() {                    

        //$(".col").addClass("editor");

        //initTinyMCE();	

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
            //$(this).children(".ToolBar").show();
            $(this).addClass("active");
        });

        $(document).on("mouseout", ".UseMainLayout, .row, .col", function () {
            //$(this).children(".ToolBar").hide();
            $(this).removeClass("active");
        });

		function ScrollToElement(elem, animate) {
			$('html,body').animate({ scrollTop: elem.offset().top }, 'slow', function () {
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
            var toolBar = $("#ToolBarTemplate").clone();
            toolBar.find(".AddRow").css("display", "none"); 
			toolBar.find(".AddField").css("display", "none");			
            toolBar.find(".DeleteRow").css("display", "inline");

            var row = $("<div class='row'></div>");
            row.append(toolBar.html());

			$(this).closest(".col, #PageContent").append(row);

			ScrollToElement(row, true);            
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
        <a href="javascript:void(0)" class="AddRow"><i class="fa fa-plus" aria-hidden="true"></i> Add Row</a>
        <a href="javascript:void(0)" class="AddColumn"><i class="fa fa-plus" aria-hidden="true"></i> Add Column</a>		
        <a href="javascript:void(0)" class="AddField"><i class="fa fa-plus" aria-hidden="true"></i> Add Field</a>
        <a href="javascript:void(0)" class="DeleteRow"><i class="fa fa-trash" aria-hidden="true"></i> Delete Row</a>
        <a href="javascript:void(0)" class="DeleteColumn"><i class="fa fa-trash" aria-hidden="true"></i> Delete Column</a>
		<a href="javascript:void(0)" class="FullWidthToggle"><i class="fa fa-arrows-h" aria-hidden="true"></i> Stretch</a>
        <a href="javascript:void(0)" class="IncreaseColumnSize">+</a>
        <a href="javascript:void(0)" class="DecreaseColumnSize">-</a>
		<a href="javascript:void(0)" class="OtherOptions">Other</a>        
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
			newHtml = newHtml.replace(/}\s*</gm, '}\n<');
			newHtml = newHtml.replace(/}\s*{/gm, '}\n{');
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