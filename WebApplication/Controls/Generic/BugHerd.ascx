<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BugHerd.ascx.cs" Inherits="WebApplication.Controls.BugHerd" %>

<script type='text/javascript'>
    (function (d, t) {
        var bh = d.createElement(t), s = d.getElementsByTagName(t)[0];
        bh.type = 'text/javascript';
        bh.src = '//www.bugherd.com/sidebarv2.js?apikey=<%= SettingsMapper.GetSettings().BugHerdApiKey %>';
        s.parentNode.insertBefore(bh, s);
    })(document, 'script');
</script>