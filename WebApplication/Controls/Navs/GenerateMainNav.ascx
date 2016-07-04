<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateMainNav.ascx.cs" Inherits="WebApplication.Controls.GenerateMainNav" %>

<script>
    var isMobile = false;

    $(document).ready(function () {
        $(".nav li ul").addClass("dropdown-menu");

        $(".nav li ul").parents("li").addClass("dropdown");

        $('.dropdown').mouseover(function () {            
            if (!isMobile) {
                $(this).find(".dropdown-menu").show();
            }
        });

        $('.dropdown').mouseout(function () {
            if (!isMobile) {
                $(this).find(".dropdown-menu").hide();
            }
        });

        $('.dropdown a').click(function () {
            var topNavLinkClicked = ($(this).parents(".level1").length == 0);
            var hasChildNav = ($(this).parent().find("ul").length > 0);

            var href = $(this).attr("href");

            if ((isMobile) && (hasChildNav))
                return;

            window.location.href = href;

        });

        function resizer() {
            var w = $(window).width();
            if (w < 768) {
                isMobile = true;
            } else {
                isMobile = false;
            }
        }

        $(window).on('load resize', function () {
            resizer();
        });

    });
</script>

<div id="mainNav">
    <Site:GenerateNav ID="GenerateNav" runat="server" RenderRootMedia="false" RootUlClasses="nav navbar-nav" RenderDepth="2" DisplayProtectedSections="false" />
</div>