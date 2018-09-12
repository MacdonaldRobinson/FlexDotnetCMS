<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonIncludes.ascx.cs" Inherits="WebApplication.Admin.Controls.CommonIncludes" %>

<script type="text/javascript">
    var BaseUrl = "<%= URIHelper.BaseUrl%>";
    var MasterPage = "<%= this.Page.MasterPageFile %>";
</script>

<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery/1.12.1/jquery.min.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.colorbox/1.6.4/jquery.colorbox-min.js"></script>
<script defer type="text/javascript" src="<%= URIHelper.BaseUrl %>Scripts/global.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.hoverintent/1.8.1/jquery.hoverIntent.min.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/superfish/1.7.9/js/superfish.min.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-ui-timepicker-addon/1.6.3/jquery-ui-timepicker-addon.min.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.ui-contextmenu/1.13.0/jquery.ui-contextmenu.min.js"></script>
<script defer type="text/javascript" src="<%= URIHelper.BaseUrl %>Scripts/split-pane-master/split-pane.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.3.1/ace.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.3.1/ext-language_tools.js"></script>
<script defer type="text/javascript" src="<%= URIHelper.BaseUrl %>Scripts/tinymce/js/tinymce/tinymce.min.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.5/jquery.jgrowl.min.js"></script>
<script defer type="text/javascript" src="<%= URIHelper.BaseUrl %>Admin/Scripts/jquery.autoSuggest.packed.js"></script>
<script defer type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jstree/3.3.3/jstree.min.js"></script>

<script defer src="//cdnjs.cloudflare.com/ajax/libs/ScrollMagic/2.0.5/ScrollMagic.min.js"></script>
<script defer src="//cdnjs.cloudflare.com/ajax/libs/ScrollMagic/2.0.5/plugins/debug.addIndicators.min.js"></script>

<script defer src="//cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.8/beautify.js"></script>
<script defer src="//cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.8/beautify-html.js"></script>

<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/tooltipster/3.3.0/css/tooltipster.min.css" />
<script defer src="//cdnjs.cloudflare.com/ajax/libs/tooltipster/3.3.0/js/jquery.tooltipster.min.js"></script>

<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.16/b-1.5.0/b-html5-1.5.0/rr-1.2.3/datatables.min.css"/>
<script defer type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.16/b-1.5.0/b-html5-1.5.0/rr-1.2.3/datatables.min.js"></script>

<script defer src="https://cdnjs.cloudflare.com/ajax/libs/jquery.blockUI/2.70/jquery.blockUI.min.js"></script>

<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jstree/3.3.3/themes/default/style.min.css" />
<link rel="stylesheet" href="<%= URIHelper.BaseUrl %>Admin/Styles/autoSuggest.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.5/jquery.jgrowl.min.css" />
<link rel="stylesheet" href="<%= URIHelper.BaseUrl %>Scripts/colorbox-master/example3/colorbox.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/superfish/1.7.9/css/superfish.min.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jquery-ui-timepicker-addon/1.6.3/jquery-ui-timepicker-addon.css" />
<link rel="stylesheet" href="<%= URIHelper.BaseUrl %>Scripts/split-pane-master/split-pane.css" />
<link rel="stylesheet" href="<%= URIHelper.BaseUrl %>Admin/Styles/adminGlobal.css" />

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.2/chosen.min.css" />
<script defer src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.2/chosen.jquery.min.js"></script>


<script defer type="text/javascript" src="<%= URIHelper.BaseUrl %>Admin/Scripts/adminGlobal.js"></script>


<script>
    (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
    m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
    })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');


    ga('create', 'UA-116236541-1', 'auto', {allowLinker: true});
    ga('require', 'linker');
    ga('linker:autoLink', ['<% = Request.Url.Host %>']);

    ga('send', 'pageview');    

</script>

<script defer>    
    $(document).ajaxStop(function () {        
        UnBlockUI();
    });

    function BlockUI()
    {
        $.blockUI({ message: 'Just a moment...</h1>' });
    }

    function UnBlockUI()
    {
        $.unblockUI();
    }


    function BindActiveTabs()
    {
        var TabIndexsJson = $("#<%= SelectedTabIndexs.ClientID %>").val();

        if (TabIndexsJson != "") {
            var TabObjIndexs = JSON.parse(TabIndexsJson);

            $(TabObjIndexs).each(function () {
                var tabUl = $(".tabs > ul")[this.tabUlIndex];

                $($(tabUl).children("li")[this.activeLiIndex]).find("a").click();
            });

        }
    }

    $(document).ready(function () {        

        $(document).on("click", ".tabs li a", function () {
            var tabUl = $(this).parents("ul");
            var tabUlIndex = $(".tabs > ul").index(tabUl);

            var activeLi = tabUl.children("li.ui-state-active");
            var activeLiIndex = tabUl.children().index(activeLi);

            var existingJson = $("#<%= SelectedTabIndexs.ClientID %>").val();
            var existingArray = new Array();

            if (existingJson == "") {
                existingJson = "[]";
                $("#<%= SelectedTabIndexs.ClientID %>").val(existingJson);
            }

            existingArray = JSON.parse(existingJson);

            var obj = new Object();
            obj.tabUlIndex = tabUlIndex;
            obj.activeLiIndex = activeLiIndex;

            var pushNew = true;

            $(existingArray).each(function () {
                if (this.tabUlIndex == obj.tabUlIndex) {
                    this.activeLiIndex = obj.activeLiIndex;
                    pushNew = false;
                }
            });

            if (pushNew)
                existingArray.push(obj);

            var json = JSON.stringify(existingArray);

            $("#<%= SelectedTabIndexs.ClientID %>").val(json);

        });
    });


</script>

<asp:HiddenField ID="SelectedTabIndexs" runat="server" />