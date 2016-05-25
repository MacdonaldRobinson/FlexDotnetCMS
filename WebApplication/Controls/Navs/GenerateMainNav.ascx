<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateMainNav.ascx.cs" Inherits="WebApplication.Controls.GenerateMainNav" %>

<script>
    var isMobile = false;

    $(document).ready(function () {
        $(".nav li ul").addClass("dropdown-menu");

        $('.dropdown').mouseover(function () {
            if(!isMobile) {
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

<div id="mainMenu">

    <nav class="navbar navbar-default navbar-static-top" role="navigation">
        <!-- Brand and toggle get grouped for better mobile display -->
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
        </div>

        <!-- Mainnav -->
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <Site:GenerateNav ID="GenerateNav" runat="server" RenderRootMedia="true" RootUlClasses="nav navbar-nav navbar-left" RenderDepth="1" DisplayProtectedSections="false" />
        </div>
        <!-- /.navbar-collapse -->
    </nav>
</div>