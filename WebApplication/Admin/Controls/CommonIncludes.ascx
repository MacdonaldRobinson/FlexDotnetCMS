<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonIncludes.ascx.cs" Inherits="WebApplication.Admin.Controls.CommonIncludes" %>

<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery/1.12.1/jquery.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.colorbox/1.6.3/jquery.colorbox-min.js"></script>
<script type="text/javascript" src="/Scripts/global.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.hoverintent/1.8.1/jquery.hoverIntent.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/superfish/1.7.8/js/superfish.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-ui-timepicker-addon/1.6.1/jquery-ui-timepicker-addon.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.ui-contextmenu/1.11.0/jquery.ui-contextmenu.min.js"></script>
<script type="text/javascript" src="/Scripts/split-pane-master/split-pane.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.2.3/ace.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/ace/1.2.3/ext-language_tools.js"></script>
<script type="text/javascript" src="/Scripts/tinymce/js/tinymce/tinymce.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.4/jquery.jgrowl.min.js"></script>
<script type="text/javascript" src="/Admin/Scripts/jquery.autoSuggest.packed.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jstree/3.2.1/jstree.min.js"></script>

<script src="//cdnjs.cloudflare.com/ajax/libs/ScrollMagic/2.0.5/ScrollMagic.min.js"></script>
<script src="//cdnjs.cloudflare.com/ajax/libs/ScrollMagic/2.0.5/plugins/debug.addIndicators.min.js"></script>

<script type="text/javascript" src="/Admin/Scripts/adminGlobal.js"></script>

<link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.10.11/css/jquery.dataTables.min.css">
<link rel="stylesheet" type="text/css" href="//cdn.datatables.net/buttons/1.1.2/css/buttons.dataTables.min.css">
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jstree/3.2.1/themes/default/style.min.css" />
<link rel="stylesheet" href="/Admin/Styles/autoSuggest.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jquery-jgrowl/1.4.4/jquery.jgrowl.min.css" />
<link rel="stylesheet" href="/Scripts/colorbox-master/example3/colorbox.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/superfish/1.7.8/css/superfish.min.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.css" />
<link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/jquery-ui-timepicker-addon/1.6.1/jquery-ui-timepicker-addon.css" />
<link rel="stylesheet" href="/Scripts/split-pane-master/split-pane.css" />
<link rel="stylesheet" href="/Admin/Styles/adminGlobal.css" />

<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
        <div class='loading-panel'>
            <div class='copy'>Loading ...</div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>