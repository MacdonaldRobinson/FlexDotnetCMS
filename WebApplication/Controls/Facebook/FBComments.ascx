<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FBComments.ascx.cs" Inherits="WebApplication.Controls.Facebook.Comments" %>
<div id="fb-root"></div>
<script>        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
            fjs.parentNode.insertBefore(js, fjs);
        } (document, 'script', 'facebook-jssdk'));</script>

<div class="fb-comments" data-href="{CurrentUrl}" data-num-posts="5" data-width="500"></div>