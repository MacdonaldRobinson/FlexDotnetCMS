<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonIncludes.ascx.cs" Inherits="WebApplication.Admin.Controls.CommonIncludes" %>

<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery/1.12.1/jquery.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.colorbox/1.6.4/jquery.colorbox-min.js"></script>
<script type="text/javascript" src="/Scripts/global.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.hoverintent/1.8.1/jquery.hoverIntent.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/superfish/1.7.9/js/superfish.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-ui-timepicker-addon/1.6.3/jquery-ui-timepicker-addon.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.ui-contextmenu/1.13.0/jquery.ui-contextmenu.min.js"></script>
<script type="text/javascript" src="/Scripts/split-pane-master/split-pane.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.2.6/ace.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.2.6/ext-language_tools.js"></script>
<script type="text/javascript" src="/Scripts/tinymce/js/tinymce/tinymce.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.5/jquery.jgrowl.min.js"></script>
<script type="text/javascript" src="/Admin/Scripts/jquery.autoSuggest.packed.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jstree/3.3.3/jstree.min.js"></script>

<script src="//cdnjs.cloudflare.com/ajax/libs/ScrollMagic/2.0.5/ScrollMagic.min.js"></script>
<script src="//cdnjs.cloudflare.com/ajax/libs/ScrollMagic/2.0.5/plugins/debug.addIndicators.min.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.8/beautify.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/js-beautify/1.6.8/beautify-html.js"></script>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/tooltipster/3.3.0/css/tooltipster.min.css" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/tooltipster/3.3.0/js/jquery.tooltipster.min.js"></script>

<script type="text/javascript" src="/Admin/Scripts/adminGlobal.js"></script>

<link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.10.11/css/jquery.dataTables.min.css">
<link rel="stylesheet" type="text/css" href="//cdn.datatables.net/buttons/1.1.2/css/buttons.dataTables.min.css">
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jstree/3.3.3/themes/default/style.min.css" />
<link rel="stylesheet" href="/Admin/Styles/autoSuggest.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.5/jquery.jgrowl.min.css" />
<link rel="stylesheet" href="/Scripts/colorbox-master/example3/colorbox.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/superfish/1.7.9/css/superfish.min.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jquery-ui-timepicker-addon/1.6.3/jquery-ui-timepicker-addon.css" />
<link rel="stylesheet" href="/Scripts/split-pane-master/split-pane.css" />
<link rel="stylesheet" href="/Admin/Styles/adminGlobal.css" />

<script>
    (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
    m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
    })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');


    ga('create', 'UA-2450205-148', 'auto', {allowLinker: true});
    ga('require', 'linker');
    ga('linker:autoLink', ['<% = Request.Url.Host %>']);

    ga('send', 'pageview');
</script>

<script>
    $(document).ajaxStop(function () {
        $('#<%= UpdateProgress1.ClientID%>').hide();
    });

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

<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
        <div class='loading-panel'>
            <div class='copy'>Loading ...</div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>