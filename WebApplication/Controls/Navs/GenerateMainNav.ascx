<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateMainNav.ascx.cs" Inherits="WebApplication.Controls.GenerateMainNav" %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#mainMenu ul li ul").addClass("dropdown-menu");
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