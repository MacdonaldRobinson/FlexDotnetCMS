<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualLayoutEditor.ascx.cs" Inherits="WebApplication.Controls.LoggedIn.VisualLayoutEditor" %>

<!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

<!-- Optional theme -->
<link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

<!-- Latest compiled and minified JavaScript -->
<script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

<script src="//code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.2.6/ace.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.2.6/ext-language_tools.js"></script>
<script src="/Scripts/tinymce/js/tinymce/tinymce.min.js"></script>
<script>

    function CleanHtml() {
        $(".row, .col, .UseMainLayout").sortable("destroy");
    }

    $(function () {                

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
                toolBar.find(".AddRow").css("display", "none");
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
            $(".UseMainLayout, .row, .col").sortable({
                handle: ".Handle"
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

            var source = this;

            $.colorbox({
                href: "/Admin/Views/PageHandlers/FieldSelector/Default.aspx?mediaDetailId=3",
                width: colorBoxWidth,
                height: colorBoxHeight,
                iframe: true,
                fixed: true,
                onClosed: function () {
                    console.log(source);
                }                
            });
        });

        $(document).on("click", ".DeleteRow", function () {
            $(this).closest(".row").remove();
        });

        $(document).on("click", ".DeleteColumn", function () {
            $(this).closest(".col").remove();
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

    });
</script>

<style>
    .UseMainLayout {
        padding: 30px;
        padding-top: 25px;
    }
    .row, .col {
        padding: 30px;        
    }

    .UseMainLayout, .row, .col {
        border: 1px dotted;
        position:relative;
    }

    #ToolBarTemplate {
        display: none;
    }

    .ToolBar a{
        padding: 5px;
    }

    .ToolBar {
        top:0;
    }

    .ToolBar {
        position: absolute;              
        border: 1px solid;
        background-color: yellow;
        display:none;
        z-index: 999;
    }

    .DeleteRow, .DeleteColumn, .IncreaseColumnSize, .DecreaseColumnSize, .UpdateClasses {
        display: none;
    }

    .UseMainLayout > .Handle {
        display: none;
    }

    .Handle {
        position: absolute;
        cursor:move;
        left:0;
        top: 0;
    }

    .ace_editor div {
        font: inherit!important
    }

</style>

<div id="ToolBarTemplate">    
    <a href="javascript:void(0)" class="Handle">Handle</a>
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