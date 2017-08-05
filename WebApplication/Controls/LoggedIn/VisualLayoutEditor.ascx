<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualLayoutEditor.ascx.cs" Inherits="WebApplication.Controls.LoggedIn.VisualLayoutEditor" %>

<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify-css.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.14/beautify-html.min.js"></script>

<script>

    function UpdateVisualEditor(source) {        
        var fieldCode = $(source).attr("data-fieldcode");

        var mediaDetailId = $('.AddField.clicked').closest(".UseMainLayout").attr("data-mediadetailid");

        var url = "/Admin/Views/MasterPages/WebService.asmx/RenderField?mediaDetailId=" + mediaDetailId+"&fieldCode=" + fieldCode;

        $.get(url, function (data) {
            try {
                
                $('.AddField.clicked').closest(".col").append(data);
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

    function initEvents() {    

        $(".fieldControls a.remove").show();

        $(".UseMainLayout").each(function () {
            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = $("#ToolBarTemplate").clone();
                toolBar.find(".ToolBar").css("display", "block");
                toolBar.find(".AddField").css("display", "none");
                toolBar.find(".AddColumn").css("display", "none");
                toolBar.find(".DeleteColumn").css("display", "none");
                toolBar.find(".IncreaseColumnSize").css("display", "none");
                toolBar.find(".DecreaseColumnSize").css("display", "none");

                $(this).prepend(toolBar.html());                
            }
        });

        $(".row").each(function () {
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

                $(this).prepend(toolBar.html());
            }
        });

        $(".col").each(function () {
            if ($(this).children(".ToolBar").length == 0) {
                var toolBar = $("#ToolBarTemplate").clone();
                toolBar.find(".ToolBar").css("display", "block");
                toolBar.find(".AddRow").css("display", "inline");
                toolBar.find(".AddColumn").css("display", "none");
                toolBar.find(".DeleteColumn").css("display", "inline");
                toolBar.find(".IncreaseColumnSize").css("display", "inline");
                toolBar.find(".DecreaseColumnSize").css("display", "inline");

                $(this).prepend(toolBar.html());
            }
        });

        $(".UseMainLayout").addClass("container-fluid");
        BindDragDrop();

        function BindDragDrop()
        {
            $(".UseMainLayout").sortable({
                tolerance: "pointer",
                handle: ".Handle",
                revert: true
            });     

            $(".row").sortable({
                tolerance: "pointer",
                handle: ".Handle",
                revert: true,
                connectWith: '.row'
            });

            /*$(".col").sortable({
                tolerance: "pointer",
                handle: ".Handle",
                revert: true,
                connectWith: '.col'
            });*/

            $(".col").sortable({
                items: "> .field",
                tolerance: "pointer",
                connectWith: '.col',
                revert: true
            });         
        }

        $(document).on("mouseover", ".UseMainLayout, .row, .col", function () {
            $(this).children(".ToolBar").show();
        });

        $(document).on("mouseout", ".UseMainLayout, .row, .col", function () {
            $(this).children(".ToolBar").hide();
        });

        function UpdateClassesFields()
        {
            $(".UpdateClasses").each(function () {
                UpdateClassesField(this);
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

            $(this).closest(".col, .container-fluid").append(row);

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

            UpdateClassesFields();
            BindDragDrop();            
        });

        $(document).on("click", ".AddField", function () {

            $(".AddField").removeClass("clicked");
            $(this).addClass("clicked");

            var mediaDetailId = $(this).closest(".UseMainLayout").attr("data-mediadetailid");

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
        <a href="javascript:void(0)" class="AddRow">Add Row</a>
        <a href="javascript:void(0)" class="AddColumn">Add Column</a>
        <a href="javascript:void(0)" class="AddField">Add Field</a>
        <a href="javascript:void(0)" class="DeleteRow">Delete Row</a>
        <a href="javascript:void(0)" class="DeleteColumn">Delete Column</a>
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

    function SaveLayout()
    {
        $(".UseMainLayout").each(function () {
            var mediaDetailId = $(this).attr("data-mediadetailid");
            var clone = $(this).clone(this);

            clone.find(".ToolBar, .Handle").remove();
            clone.find(".field").each(function () {                
                var fieldCode = $(this).attr("data-fieldcode")
                var shortCode = "{Field:" + fieldCode + "}";

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

        });
    }
</script>

<a href="javascript:void(0)" id="SaveLayout" class="button">Save Layout</a>